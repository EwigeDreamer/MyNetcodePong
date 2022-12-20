using MyPong.Popups.Base;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MyPong.Popups
{
    [UnityEngine.Scripting.Preserve]
    public class GameHudPopupController : BasePopupController
    {
        public readonly UnetWrapper UnetWrapper;
        public GameHudPopupController(
            PopupService popupService,
            UnetWrapper unetWrapper)
            : base(popupService)
        {
            UnetWrapper = unetWrapper;
        }
    }
    
    public class GameHudPopup : BasePopupWithController<GameHudPopup.Data, GameHudPopupController>
    {
        [SerializeField] private Button _sutdownButton;
        
        public override bool IsUnclosable => false;
        public override bool IsOnlyOne => true;
        
        protected override void InternalInit()
        {
            _sutdownButton.OnClickAsObservable().Subscribe(_ =>
            {
                Controller.UnetWrapper.NetworkManager.Shutdown();
                ClosePopup();
            }).AddTo(this);
        }

        public override void Dispose()
        {
            
        }
        
        public class Data : IPopupData
        {
            
        }
    }
}