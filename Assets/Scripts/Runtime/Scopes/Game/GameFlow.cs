using System;
using Cysharp.Threading.Tasks;
using MyPong.Core;
using MyPong.UI;
using MyPong.UI.Popups;
using VContainer.Unity;
using UniRx;

namespace MyPong
{
    public class GameFlow : IStartable, IDisposable
    {
        private readonly PopupService PopupService;
        private readonly LoadingScreen LoadingScreen;
        private readonly UnetWrapper UnetWrapper;
        private readonly PongCoreController PongCoreController;

        private CompositeDisposable _disposables = new();

        public GameFlow(
            PopupService popupService,
            LoadingScreen loadingScreen,
            UnetWrapper unetWrapper,
            PongCoreController pongCoreController)
        {
            PopupService = popupService;
            LoadingScreen = loadingScreen;
            UnetWrapper = unetWrapper;
            PongCoreController = pongCoreController;
        }

        public void Start()
        {
            UnetWrapper.OnConnectedToServer.Subscribe(OnConnectedToServer).AddTo(_disposables);
            UnetWrapper.OnDisconnectFromServer.Subscribe(OnDisconnectFromServer).AddTo(_disposables);
            UnetWrapper.OnStartHost.Subscribe(OnStartHost).AddTo(_disposables);
            UnetWrapper.OnStartClient.Subscribe(OnStartClient).AddTo(_disposables);
            UnetWrapper.OnShutdown.Subscribe(OnShutdown).AddTo(_disposables);
            UnetWrapper.OnPlayersEnough.Subscribe(OnPlayersEnough).AddTo(_disposables);
            
            StartGame();
        }

        private async void StartGame()
        {
            await PopupService.OpenPopup<SelectAppTypePopup>();
            LoadingScreen.Hide();
        }

        private async void OnConnectedToServer(ulong id)
        {
            if (!UnetWrapper.IsServer)
            {
                await UniTask.WhenAll(
                    PopupService.OpenPopup<GameHudPopup>());
            }
        }

        private async void OnDisconnectFromServer(ulong id)
        {
            await UniTask.WhenAll(
                PopupService.CloseAll<GameHudPopup>());
        }

        private async void OnPlayersEnough(bool value)
        {
            if (value)
            {
                PongCoreController.StartCoreGameplay();
                await UniTask.WhenAll(
                    PopupService.CloseAll<WaitClientsPopup>(),
                    PopupService.OpenPopup<GameHudPopup>());
            }
            else
            {
                PongCoreController.StopCoreGameplay();
                await UniTask.WhenAll(
                    PopupService.CloseAll<GameHudPopup>(),
                    PopupService.OpenPopup<WaitClientsPopup>());
            }
        }

        private async void OnStartHost(Unit _)
        {
            await UniTask.WhenAll(
                PopupService.OpenPopup<WaitClientsPopup>());
        }

        private void OnStartClient(Unit _)
        {
            
        }

        private async void OnShutdown(Unit _)
        {
            await UniTask.WhenAll(
                PopupService.CloseAll<GameHudPopup>(),
                PopupService.CloseAll<WaitClientsPopup>());
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
