using System;
using DG.Tweening;
using UnityEngine;

namespace MyPong.UI
{
    [Flags]
    public enum ScreenLockTypes
    {
        None = 0,
        Unet = 1<<0,
    }
    
    [RequireComponent(typeof(CanvasGroup))]
    public class ScreenLocker : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private Tween _fadeTween = null;
        private ScreenLockTypes _lockedTypes = ScreenLockTypes.None;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            DontDestroyOnLoad(gameObject);
            gameObject.SetActive(false);
        }

        public void Show(ScreenLockTypes type)
        {
            _lockedTypes |= type;
            if (_lockedTypes != ScreenLockTypes.None)
            {
                if (_fadeTween != null) _fadeTween.Kill(false);
                _fadeTween = DOTween.Sequence()
                    .AppendCallback(() => gameObject.SetActive(true))
                    .Append(_canvasGroup.DOFade(1f, 0.25f))
                    .AppendCallback(() => _fadeTween = null)
                    .Play();
            }
        }

        public void Hide(ScreenLockTypes type)
        {
            _lockedTypes &= ~type;
            if (_lockedTypes == ScreenLockTypes.None)
            {
                if (_fadeTween != null) _fadeTween.Kill(false);
                _fadeTween = DOTween.Sequence()
                    .Append(_canvasGroup.DOFade(0f, 0.25f))
                    .AppendCallback(() =>
                    {
                        gameObject.SetActive(false);
                        _fadeTween = null;
                    })
                    .Play();
            }
        }
    }
}