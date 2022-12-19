using MyPong.Popups;
using MyPong.Popups.Base;
using TMPro;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace MyPong.Popups
{
    public class HostSettingsPopupController : BasePopupController
    {
        public HostSettingsPopupController(PopupService popupService) : base(popupService) { }

        public void StartHost(string port)
        {
            var nm = NetworkManager.Singleton;
            var ip = NetworkUtility.GetLocalIPv4();
            Debug.LogError($"(fake) START HOST: {ip}:{port}");
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

        public override void Dispose()
        {

        }

        public class Data : IPopupData
        {
        }
    }
}