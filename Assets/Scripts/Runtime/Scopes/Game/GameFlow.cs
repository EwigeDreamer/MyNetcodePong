using System;
using Cysharp.Threading.Tasks;
using MyPong.Core;
using MyPong.Input;
using MyPong.Networking;
using MyPong.UI;
using MyPong.UI.Popups;
using MyPong.View;
using VContainer.Unity;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using NetworkPlayer = MyPong.Networking.NetworkPlayer;

namespace MyPong
{
    public class GameFlow : IStartable, IDisposable
    {
        private readonly PopupService PopupService;
        private readonly LoadingScreen LoadingScreen;
        private readonly UnetWrapper UnetWrapper;
        private readonly CameraController CameraController;
        private readonly PongCoreController PongCoreController;
        private readonly InputController InputController;

        private CompositeDisposable _disposables = new();

        public GameFlow(
            PopupService popupService,
            LoadingScreen loadingScreen,
            UnetWrapper unetWrapper,
            CameraController cameraController,
            PongCoreController pongCoreController,
            InputController inputController)
        {
            PopupService = popupService;
            LoadingScreen = loadingScreen;
            UnetWrapper = unetWrapper;
            CameraController = cameraController;
            PongCoreController = pongCoreController;
            InputController = inputController;
        }

        public void Start()
        {
            UnetWrapper.OnConnectedToServer.Subscribe(OnConnectedToServer).AddTo(_disposables);
            UnetWrapper.OnDisconnectFromServer.Subscribe(OnDisconnectFromServer).AddTo(_disposables);
            UnetWrapper.OnStartHost.Subscribe(OnStartHost).AddTo(_disposables);
            UnetWrapper.OnStartClient.Subscribe(OnStartClient).AddTo(_disposables);
            UnetWrapper.OnShutdown.Subscribe(OnShutdown).AddTo(_disposables);
            UnetWrapper.OnPlayersEnough.Subscribe(OnPlayersEnough).AddTo(_disposables);

            UnetWrapper.SpawnEventService.OnSpawnNetworkObject
                .Select(a => a.GetComponent<FieldView>())
                .Where(a => a != null)
                .Subscribe(field => field.OnChangeScale
                    .Subscribe(a => OnFieldViewChange(a.field, a.scale))
                    .AddTo(field))
                .AddTo(_disposables);

            UnetWrapper.SpawnEventService.OnSpawnNetworkObject
                .Select(a => a.GetComponent<NetworkPlayer>())
                .Where(a => a != null)
                .Where(a => UnetWrapper.ItsMe(a.OwnerClientId))
                .Subscribe(player => player.OnGameOver
                    .Subscribe(_ => OnClientGameOver(player))
                    .AddTo(player))
                .AddTo(_disposables);

            PongCoreController.OnGameOver.Subscribe(OnCoreGameOver).AddTo(_disposables);
            
            StartGame();
        }

        private void OnFieldViewChange(FieldView field, Vector2 scale)
        {
            field.transform.localRotation = Quaternion.Euler(0f, 0f, UnetWrapper.IsServer ? 0f : 180f);
            CameraController.FocusOnField(scale);
        }

        private async void StartGame()
        {
            InputController.Start();
            await PopupService.OpenPopup<SelectAppTypePopup>();
            LoadingScreen.Hide();
        }

        //call only on HOST
        private void OnCoreGameOver(Unit _)
        {
            PongCoreController.Pause();
            foreach (var player in UnetWrapper.GetAllPlayers())
                player.OnGameOverClientRpc();
        }

        private async void OnClientGameOver(NetworkPlayer player)
        {
            var myScore = player.MyScoreRx.Value;
            var enemyScore = player.EnemyScoreRx.Value;
            if (myScore > enemyScore)
                await PopupService.OpenPopup<MessagePopup>(new MessagePopup.Data("You won!", UnetWrapper.Shutdown));
            else if (myScore > enemyScore)
                await PopupService.OpenPopup<MessagePopup>(new MessagePopup.Data("You lose!", UnetWrapper.Shutdown));
            else
                await PopupService.OpenPopup<MessagePopup>(new MessagePopup.Data("Tie... How?", UnetWrapper.Shutdown));
        }

        private async void OnConnectedToServer(ulong id)
        {
            if (!UnetWrapper.IsServer)
            {
                await UniTask.WhenAll(
                    PopupService.OpenPopup<StartTimerPopup>(
                        new StartTimerPopup.Data(
                            Constants.Gameplay.StartTimerSeconds,
                            () => PopupService.OpenPopup<GameHudPopup>().Forget())));
            }
        }

        private async void OnDisconnectFromServer(ulong id)
        {
            await UniTask.WhenAll(
                PopupService.CloseAll<StartTimerPopup>(),
                PopupService.CloseAll<GameHudPopup>());
        }

        //call only on HOST
        private async void OnPlayersEnough(bool value)
        {
            if (value)
            {
                PongCoreController.StartCoreGameplay();
                await UniTask.WhenAll(
                    PopupService.CloseAll<WaitClientsPopup>(),
                    PopupService.OpenPopup<StartTimerPopup>(new StartTimerPopup.Data(
                        Constants.Gameplay.StartTimerSeconds,
                        () => PopupService.OpenPopup<GameHudPopup>().Forget())));
            }
            else
            {
                PongCoreController.StopCoreGameplay();
                await UniTask.WhenAll(
                    PopupService.CloseAll<StartTimerPopup>(),
                    PopupService.CloseAll<GameHudPopup>(),
                    PopupService.OpenPopup<WaitClientsPopup>());
            }
        }

        private void OnStartHost(Unit _)
        {
            
        }

        private void OnStartClient(Unit _)
        {
            
        }

        private async void OnShutdown(Unit _)
        {
            await UniTask.WhenAll(
                PopupService.CloseAll<StartTimerPopup>(),
                PopupService.CloseAll<GameHudPopup>(),
                PopupService.CloseAll<WaitClientsPopup>());
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
