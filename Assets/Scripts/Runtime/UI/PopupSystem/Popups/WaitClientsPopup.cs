using MyPong.UI.Popups.Base;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MyPong.UI.Popups
{
    public class WaitClientsPopupController : BasePopupController
    {
        public readonly UnetWrapper UnetWrapper;
        
        public WaitClientsPopupController(
            PopupService popupService,
            UnetWrapper unetWrapper)
            : base(popupService)
        {
            UnetWrapper = unetWrapper;
        }
    }
    public class WaitClientsPopup : BasePopupWithController<WaitClientsPopup.Data, WaitClientsPopupController>
    {
        public override bool IsUnclosable => false;
        public override bool IsOnlyOne => true;

        [SerializeField] private Button _cancelButton;
        
        protected override void InternalInit()
        {
            _cancelButton.OnClickAsObservable().Subscribe(_ => Controller.UnetWrapper.Shutdown()).AddTo(this);
        }
        
        public override void Dispose()
        {
            
        }
        
        public class Data : IPopupData { }
    }
}