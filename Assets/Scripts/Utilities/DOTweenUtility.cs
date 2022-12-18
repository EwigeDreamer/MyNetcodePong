using UnityEngine;
using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Utilities.Bezier;

namespace Utilities.DOTween
{
    using DOTween = DG.Tweening.DOTween;
    public static class DOTweenUtility
    {
        public static TweenerCore<float, float, FloatOptions> DoFloat(Func<float> getter, Action<float> setter, Func<float> value, float duration)
        {
            float start = default;
            var tween = DOTween.To(
                () => 0f,
                t => setter(Mathf.LerpUnclamped(start, value(), t)),
                1f,
                duration);
            return tween.OnPlay(() => start = getter());
        }

        public static TweenerCore<float, float, FloatOptions> DoVector2(Func<Vector2> getter, Action<Vector2> setter, Func<Vector2> value, float duration)
        {
            Vector2 start = default;
            var tween = DOTween.To(
                () => 0f,
                t => setter(Vector2.LerpUnclamped(start, value(), t)),
                1f,
                duration);
            return tween.OnPlay(() => start = getter());
        }

        public static TweenerCore<float, float, FloatOptions> DoVector3(Func<Vector3> getter, Action<Vector3> setter, Func<Vector3> value, float duration)
        {
            Vector3 start = default;
            var tween = DOTween.To(
                () => 0f,
                t => setter(Vector3.LerpUnclamped(start, value(), t)),
                1f,
                duration);
            return tween.OnPlay(() => start = getter());
        }

        public static TweenerCore<float, float, FloatOptions> DoColor(Func<Color> getter, Action<Color> setter, Func<Color> value, float duration)
        {
            Color start = default;
            var tween = DOTween.To(
                () => 0f,
                t => setter(Color.LerpUnclamped(start, value(), t)),
                1f,
                duration);
            return tween.OnPlay(() => start = getter());
        }

        public static TweenerCore<float, float, FloatOptions> DoBezier3(Action<Vector2> setter, Func<Vector2> a, Func<Vector2> b, Func<Vector2> c, float duration)
        {
            Vector2 aa = default;
            Vector2 bb = default;
            var tween = DOTween.To(
                () => 0f,
                t => setter(BezierUtility.GetPoint(aa, bb, c(), t)),
                1f,
                duration);
            return tween.OnPlay(() => { aa = a(); bb = b(); });
        }

        public static TweenerCore<float, float, FloatOptions> DoBezier3(Action<Vector3> setter, Func<Vector3> a, Func<Vector3> b, Func<Vector3> c, float duration)
        {
            Vector3 aa = default;
            Vector3 bb = default;
            var tween = DOTween.To(
                () => 0f,
                t => setter(BezierUtility.GetPoint(aa, bb, c(), t)),
                1f,
                duration);
            return tween.OnPlay(() => { aa = a(); bb = b(); });
        }

        public static TweenerCore<float, float, FloatOptions> DoBezier4(Action<Vector2> setter, Func<Vector2> a, Func<Vector2> b, Func<Vector2> c, Func<Vector2> d, float duration)
        {
            Vector2 aa = default;
            Vector2 bb = default;
            Vector2 cc = default;
            var tween = DOTween.To(
                () => 0f,
                t => setter(BezierUtility.GetPoint(aa, bb, cc, d(), t)),
                1f,
                duration);
            return tween.OnPlay(() => { aa = a(); bb = b(); cc = c(); });
        }

        public static TweenerCore<float, float, FloatOptions> DoBezier4(Action<Vector3> setter, Func<Vector3> a, Func<Vector3> b, Func<Vector3> c, Func<Vector3> d, float duration)
        {
            Vector3 aa = default;
            Vector3 bb = default;
            Vector3 cc = default;
            var tween = DOTween.To(
                () => 0f,
                t => setter(BezierUtility.GetPoint(aa, bb, cc, d(), t)),
                1f,
                duration);
            return tween.OnPlay(() => { aa = a(); bb = b(); cc = c(); });
        }

        public static TweenerCore<float, float, FloatOptions> DoQuaternion(Func<Quaternion> getter, Action<Quaternion> setter, Func<Quaternion> value, float duration)
        {
            Quaternion start = default;
            var tween = DOTween.To(
                () => 0f,
                t => setter(Quaternion.SlerpUnclamped(start, value(), t)),
                1f,
                duration);
            return tween.OnPlay(() => start = getter());
        }

        public static TweenerCore<float, float, FloatOptions> Delay(float duration)
        {
            return DOTween.To(() => 0f, t => { }, 1f, duration);
        }

        public static TweenerCore<float, float, FloatOptions> DelayedCall(float delay, TweenCallback callback)
        {
            return Delay(delay).AddOnComplete(callback);
        }
        
        public static void PlayDelayedCall(float duration, TweenCallback action)
        {
            DelayedCall(duration, action).Play();
        }

        public static TweenerCore<float, float, FloatOptions> DoAngleDegrees(Func<float> getter, Action<float> setter, Func<float> value, float duration)
        {
            float start = default;
            var tween = DOTween.To(
                () => 0f,
                t => setter(Mathf.LerpAngle(start, value(), t)),
                1f,
                duration);
            return tween.OnPlay(() => start = getter());
        }

        public static TweenerCore<float, float, FloatOptions> DoAngleRadians(Func<float> getter, Action<float> setter, Func<float> value, float duration)
        {
            float start = default;
            var tween = DOTween.To(
                () => 0f,
                t => setter(Mathf.LerpAngle(start * Mathf.Rad2Deg, value() * Mathf.Rad2Deg, t) * Mathf.Deg2Rad),
                1f,
                duration);
            return tween.OnPlay(() => start = getter());
        }


        public static void Destroy<T>(this T tween, bool complete = false) where T : Tween
        {
            if (tween != null) tween.Kill(complete);
        }

        public static T AddOnPlay<T>(this T tween, TweenCallback callback) where T : Tween
        {
            tween.onPlay += callback;
            return tween;
        }
        public static T AddOnComplete<T>(this T tween, TweenCallback callback) where T : Tween
        {
            tween.onComplete += callback;
            return tween;
        }
    }
}
