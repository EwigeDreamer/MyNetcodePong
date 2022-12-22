using Cysharp.Threading.Tasks;
using MyPong.UI.Popups.Base;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Network;

namespace MyPong.UI.Popups
{
    [UnityEngine.Scripting.Preserve]
    public class ClientSettingsPopupController : BasePopupController
    {
        public readonly UnetWrapper UnetWrapper;
        
        public ClientSettingsPopupController(
            PopupService popupService,
            UnetWrapper unetWrapper)
            : base(popupService)
        {
            UnetWrapper = unetWrapper;
        }

        public void StartClient(string ip, string port)
        {
            if (!NetworkUtility.IsValidIPv4(ip))
            {
                PopupService.OpenPopup<MessagePopup>(new MessagePopup.Data("Invalid IP address!")).Forget();
                return;
            }
            if (!NetworkUtility.IsValidPort(port))
            {
                PopupService.OpenPopup<MessagePopup>(new MessagePopup.Data("Invalid port!")).Forget();
                return;
            }
            if (!UnetWrapper.StartClient(ip, port))
            {
                PopupService.OpenPopup<MessagePopup>(new MessagePopup.Data("Something went wrong!")).Forget();
            }
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
            _ipField.text = NetworkUtility.GetLocalIPv4();
            _connectButton
                .OnClickAsObservable()
                .Subscribe(_ => Controller.StartClient(_ipField.text, _portField.text))
                .AddTo(this);
        }

        public override void Dispose() { }

        public class Data : IPopupData { }
    }
}