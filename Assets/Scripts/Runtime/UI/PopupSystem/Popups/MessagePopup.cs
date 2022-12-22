using MyPong.UI.Popups.Base;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MyPong.UI.Popups
{
    [UnityEngine.Scripting.Preserve]
    public class MessagePopupController : BasePopupController
    {
        public MessagePopupController(PopupService popupService) : base(popupService) { }
    }

    public class MessagePopup : BasePopupWithController<MessagePopup.Data, MessagePopupController>
    {
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private Button _okButton;

        public override bool IsUnclosable => false;
        public override bool IsOnlyOne => false;

        protected override void InternalInit()
        {
            _messageText.text = PopupData.Message;
            _okButton.OnClickAsObservable().Subscribe(_ => ClosePopup()).AddTo(this);
        }

        public override void Dispose() { }

        public class Data : IPopupData
        {
            public readonly string Message;
            public Data(string message) => Message = message;
        }
    }
}