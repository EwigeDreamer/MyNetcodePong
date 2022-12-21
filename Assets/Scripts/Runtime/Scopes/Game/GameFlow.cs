using System;
using Cysharp.Threading.Tasks;
using MyPong.Popups;
using VContainer.Unity;
using UniRx;

namespace MyPong
{
    public class GameFlow : IStartable, IDisposable
    {
        private readonly PopupService PopupService;
        private readonly UnetWrapper UnetWrapper;

        private CompositeDisposable _disposables = new();

        public GameFlow(
            PopupService popupService,
            UnetWrapper unetWrapper)
        {
            PopupService = popupService;
            UnetWrapper = unetWrapper;
        }

        public void Start()
        {
            UnetWrapper.OnConnect.Subscribe(_ => OnConnect()).AddTo(_disposables);
            UnetWrapper.OnDisconnect.Subscribe(_ => OnDisconnect()).AddTo(_disposables);
            
            PopupService.OpenPopup<SelectAppTypePopup>().Forget();
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
