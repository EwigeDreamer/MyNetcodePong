using MyPong.Popups;
using MyPong.Popups.Base;
using Unity.Netcode;

namespace MyPong.Popups
{
    public class HostSettingsPopupController : BasePopupController
    {
        public HostSettingsPopupController(PopupService popupService) : base(popupService) { }

        public void SetupHost(string port)
        {
            var nm = NetworkManager.Singleton;
        }
    }

    public class HostSettingsPopup : BasePopupWithController<HostSettingsPopup.Data, HostSettingsPopupController>
    {
        public override bool IsUnclosable => false;
        public override bool IsOnlyOne => false;

        protected override void InternalInit()
        {

        }

        public override void Dispose()
        {

        }

        public class Data : IPopupData
        {
        }
    }
}