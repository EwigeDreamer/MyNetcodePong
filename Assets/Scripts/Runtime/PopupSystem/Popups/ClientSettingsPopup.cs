using MyPong.Popups;
using MyPong.Popups.Base;
using TMPro;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace MyPong.Popups
{
    public class ClientSettingsPopupController : BasePopupController
    {
        public ClientSettingsPopupController(PopupService popupService) : base(popupService) { }

        public void StartClient(string ip, string port)
        {
            var nm = NetworkManager.Singleton;
            Debug.LogError($"(fake) CONNECT TO: {ip}:{port}");
        }
    }

    public class ClientSettingsPopup : BasePopupWithController<ClientSettingsPopup.Data, ClientSettingsPopupController>
    {
        [SerializeField] private TMP_InputField _ipField;
        [SerializeField] private TMP_InputField _portField;
        [SerializeField] private Button _connectButton;
        
        public override bool IsUnclosable => false;
        public override bool IsOnlyOne => false;

        protected override void InternalInit()
        {
            _connectButton
                .OnClickAsObservable()
                .Subscribe(_ => Controller.StartClient(_ipField.text, _portField.text))
                .AddTo(this);
        }

        public override void Dispose()
        {

        }

        public class Data : IPopupData
        {
        }
    }
}