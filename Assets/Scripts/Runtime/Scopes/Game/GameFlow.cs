using System;
using Cysharp.Threading.Tasks;
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

        private CompositeDisposable _disposables = new();

        public GameFlow(
            PopupService popupService,
            LoadingScreen loadingScreen,
            UnetWrapper unetWrapper)
        {
            PopupService = popupService;
            LoadingScreen = loadingScreen;
            UnetWrapper = unetWrapper;
        }

        public void Start()
        {
            UnetWrapper.OnConnected.Subscribe(_ => OnConnect()).AddTo(_disposables);
            UnetWrapper.OnDisconnect.Subscribe(_ => OnDisconnect()).AddTo(_disposables);
            StartGame();
        }

        private async void StartGame()
        {
            await PopupService.OpenPopup<SelectAppTypePopup>();
            LoadingScreen.Hide();
        }

        private async void OnConnect()
        {
            await PopupService.CloseAll<HostSettingsPopup>();
            await PopupService.CloseAll<ClientSettingsPopup>();
            await PopupService.OpenPopup<GameHudPopup>();
        }

        private async void OnDisconnect()
        {
            await PopupService.CloseAll<GameHudPopup>();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
