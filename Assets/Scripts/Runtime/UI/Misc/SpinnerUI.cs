using DG.Tweening;
using UnityEngine;
using Utilities.DOTween;

namespace MyPong.UI
{
    public class SpinnerUI : MonoBehaviour
    {
        private Tween _spinnerTween = null;

        private void OnEnable()
        {
            if (_spinnerTween != null)
                _spinnerTween.Kill(false);
            _spinnerTween = DOTweenUtility.DoFloat(
                    () => 0f,
                    v => transform.localRotation = Quaternion.Euler(0f, 0f, v),
                    () => -360f,
                    1f)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear)
                .Play();
        }

        private void OnDisable()
        {
            if (_spinnerTween != null)
                _spinnerTween.Kill(false);
            _spinnerTween = null;
        }
    }
}