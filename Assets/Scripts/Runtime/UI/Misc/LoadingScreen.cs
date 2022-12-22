using DG.Tweening;
using UnityEngine;

namespace MyPong.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class LoadingScreen : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private Tween _fadeTween = null;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            DontDestroyOnLoad(gameObject);
        }

        public void Show()
        {
            if (_fadeTween != null) _fadeTween.Kill(false);
            _fadeTween = DOTween.Sequence()
                .AppendCallback(() => gameObject.SetActive(true))
                .Append(_canvasGroup.DOFade(1f, 0.25f))
                .AppendCallback(() => _fadeTween = null)
                .Play();
        }

        public void Hide()
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