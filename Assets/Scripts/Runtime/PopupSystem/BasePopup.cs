using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace MyPong.Popups.Base
{
    public interface IPopupData { }
    
    public abstract class BasePopupController
    {
        public readonly PopupService PopupService;

        [Inject]
        [UnityEngine.Scripting.Preserve]
        public BasePopupController(PopupService popupService)
        {
            PopupService = popupService;
        }

        public void ClosePopup(BasePopup basePopup) => PopupService.ClosePopup(basePopup).Forget();
    }

    public abstract class BasePopupWithController<TData, TController> : BasePopup, IInstaller
        where TController : BasePopupController
    {
        [SerializeField] private Button _closeButton;
        
        protected TData PopupData { get; private set; }
        protected TController Controller { get; private set; }
        
        public void Install(IContainerBuilder builder)
        {
            builder.Register<TController>(Lifetime.Scoped).AsSelf();
        }

        [Inject]
        [UnityEngine.Scripting.Preserve]
        protected void Inject(TController controller)
        {
            Controller = controller;
        }

        public override void Init(IPopupData data)
        {
            if (data != null)
                PopupData = (TData) data;
            if (_closeButton != null)
                _closeButton.OnClickAsObservable().Subscribe(_ => ClosePopup()).AddTo(this);
            InternalInit();
        }

        protected abstract void InternalInit();

        public void ClosePopup() => Controller.ClosePopup(this);
    }
    
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BasePopup : MonoBehaviour, IDisposable
    {
        protected CanvasGroup CanvasGroup { get; private set; }

        protected virtual void Awake()
        {
            CanvasGroup = GetComponent<CanvasGroup>();
        }

        public abstract void Init(IPopupData data);

        public bool IsOpened { get; private set; } = false;

        public abstract bool IsUnclosable { get; }
        public abstract bool IsOnlyOne { get; }

        public UniTask Show()
        {
            if (IsOpened) return UniTask.CompletedTask;

            return DOTween.Sequence()
                .AppendCallback(() =>
                {
                    CanvasGroup.interactable = false;
                    gameObject.SetActive(true);
                })
                .Append(GetShowingTween())
                .AppendCallback(() =>
                {
                    CanvasGroup.interactable = true;
                    IsOpened = true;
                })
                .Play()
                .ToUniTask();
        }

        public UniTask Hide()
        {
            if (!IsOpened) return UniTask.CompletedTask;

            return DOTween.Sequence()
                .AppendCallback(() => { CanvasGroup.interactable = false; })
                .Append(GetHidingTween())
                .AppendCallback(() =>
                {
                    gameObject.SetActive(false);
                    IsOpened = false;
                })
                .Play()
                .ToUniTask();
        }

        protected virtual Tween GetShowingTween()
        {
            float duration = 0.25f;
            var minScale = Vector3.one * 1.1f;
            return DOTween.Sequence()
                .AppendCallback(() =>
                {
                    CanvasGroup.alpha = 0f;
                    transform.localScale = minScale;
                })
                .Append(CanvasGroup.DOFade(1f, duration)
                    .SetEase(Ease.OutSine))
                .Join(transform.DOScale(Vector3.one, duration)
                    .SetEase(Ease.OutSine));
        }

        protected virtual Tween GetHidingTween()
        {
            float duration = 0.25f;
            var minScale = Vector3.one * 1.1f;
            return DOTween.Sequence()
                .Append(CanvasGroup.DOFade(0f, duration)
                    .SetEase(Ease.InSine))
                .Join(transform.DOScale(minScale, duration)
                    .SetEase(Ease.InSine));
        }

        public abstract void Dispose();
    }
}
