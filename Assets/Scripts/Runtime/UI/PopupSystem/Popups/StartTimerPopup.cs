using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Extensions.Unity;
using MyPong.UI.Popups.Base;
using TMPro;
using UnityEngine;
using Utilities.DOTween;

namespace MyPong.UI.Popups
{
    [UnityEngine.Scripting.Preserve]
    public class StartTimerPopupController : BasePopupController
    {
        public StartTimerPopupController(PopupService popupService) : base(popupService) { }
    }
    public class StartTimerPopup : BasePopupWithController<StartTimerPopup.Data, StartTimerPopupController>
    {
        [SerializeField] private TMP_Text _secondsTextTemplate;
        
        public override bool IsUnclosable => false;
        public override bool IsOnlyOne => true;
        

        private Tween _counter = null;
        protected override void InternalInit()
        {
            _secondsTextTemplate.gameObject.SetActive(false);
            var counter = DOTween.Sequence();
            foreach (var t in Enumerable.Range(1, PopupData?.Seconds ?? 3).Reverse())
            {
                var text = Instantiate(_secondsTextTemplate, _secondsTextTemplate.transform.parent);
                var textGroup = text.AddMissingComponent<CanvasGroup>();
                counter
                    .AppendCallback(() =>
                    {
                        text.text = t.ToString();
                        text.gameObject.SetActive(true);
                        textGroup.alpha = 0f;
                        text.transform.localScale = Vector3.one * 1.25f;
                    })
                    .Append(textGroup.DOFade(1f, 0.5f)
                        .SetEase(Ease.InCubic))
                    .Join(text.transform.DOScale(Vector3.one, 0.5f)
                        .SetEase(Ease.InCubic))
                    .Append(DOTweenUtility.Delay(0.5f))
                    .AppendCallback(() =>
                    {
                        if (t > 1) Destroy(text.gameObject);
                    });
            }
            counter.AppendCallback(() =>
            {
                _counter = null;
                PopupData?.OnComplete?.Invoke();
                ClosePopup();
            });
            _counter = counter.Play();
        }
        
        public override void Dispose()
        {
            _counter?.Kill(false);
        }
        
        public class Data : IPopupData
        {
            public readonly int Seconds;
            public readonly Action OnComplete;

            public Data(int seconds, Action onComplete)
            {
                Seconds = seconds;
                OnComplete = onComplete;
            }
        }
    }
}