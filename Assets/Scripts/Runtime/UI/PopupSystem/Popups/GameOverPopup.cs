using System;
using Cysharp.Threading.Tasks;
using MyPong.UI.Popups.Base;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MyPong.UI.Popups
{
    [UnityEngine.Scripting.Preserve]
    public class GameOverPopupController : BasePopupController
    {
        public GameOverPopupController(PopupService popupService) : base(popupService) { }
    }
    public class GameOverPopup : BasePopupWithController<GameOverPopup.Data, GameOverPopupController>
    {
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private Button _exitButton;
        [SerializeField] private GameObject _buttonContainer;
        
        public override bool IsUnclosable => false;
        public override bool IsOnlyOne => true;

        protected override void InternalInit()
        {
            _messageText.text = PopupData.Message;
            _exitButton.OnClickAsObservable().Subscribe(_ => ExitClick()).AddTo(this);
            HideAndShowButton();
        }

        private async void HideAndShowButton()
        {
            _buttonContainer.SetActive(false);
            await UniTask.Delay(3000);
            _buttonContainer.SetActive(true);
        }

        private void ExitClick()
        {
            PopupData.OnClose?.Invoke();
            ClosePopup();
        }
        
        public override void Dispose() { }
        
        public class Data : IPopupData
        {
            public readonly string Message;
            public readonly Action OnClose;
            public Data(string message, Action onClose = null)
            {
                Message = message;
                OnClose = onClose;
            }
        }
    }
}