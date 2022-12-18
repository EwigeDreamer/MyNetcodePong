using MyPong.Popups;
using MyPong.Popups.Base;
using Unity.Netcode;

namespace MyPong.Popups
{
    public class ClientSettingsPopupController : BasePopupController
    {
        public ClientSettingsPopupController(PopupService popupService) : base(popupService) { }
    }

    public class ClientSettingsPopup : BasePopupWithController<ClientSettingsPopup.Data, ClientSettingsPopupController>
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