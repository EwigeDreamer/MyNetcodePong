using Cysharp.Threading.Tasks;
using MyPong.Networking;
using MyPong.UI.Popups.Base;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Network;

namespace MyPong.UI.Popups
{
    [UnityEngine.Scripting.Preserve]
    public class HostSettingsPopupController : BasePopupController
    {
        public readonly UnetWrapper UnetWrapper;
        
        public HostSettingsPopupController(
            PopupService popupService,
            UnetWrapper unetWrapper)
            : base(popupService)
        {
            UnetWrapper = unetWrapper;
        }

        public void StartHost(string port)
        {
            if (!NetworkUtility.IsValidPort(port))
            {
                PopupService.OpenPopup<MessagePopup>(new MessagePopup.Data("Invalid port!")).Forget();
                return;
            }
            if (!UnetWrapper.StartHost(port))
            {
                PopupService.OpenPopup<MessagePopup>(new MessagePopup.Data("Something went wrong!")).Forget();
            }
        }
    }

    public class HostSettingsPopup : BasePopupWithController<HostSettingsPopup.Data, HostSettingsPopupController>
    {
        [SerializeField] private TMP_InputField _ipField;
        [SerializeField] private TMP_InputField _portField;
        [SerializeField] private Button _startButton;
        
        public override bool IsUnclosable => false;
        public override bool IsOnlyOne => false;

        protected override void InternalInit()
        {
            _ipField.text = NetworkUtility.GetLocalIPv4();
            _startButton
                .OnClickAsObservable()
                .Subscribe(_ => Controller.StartHost(_portField.text))
                .AddTo(this);
        }

        public override void Dispose() { }

        public class Data : IPopupData { }
    }
}