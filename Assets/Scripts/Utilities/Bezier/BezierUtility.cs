using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Bezier
{
    public static class BezierUtility
    {
        public static Vector2 GetPoint(Vector2 a, Vector2 b, Vector2 c, float t)
        {
            var u = 1f - t;
            return  (u * u) * a +
                    (2f * t * u) * b +
                    (t * t) * c;
        }

        public static Vector3 GetPoint(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            var u = 1f - t;
            return  (u * u) * a +
                    (2f * t * u) * b +
                    (t * t) * c;
        }

        public static Vector4 GetPoint(Vector4 a, Vector4 b, Vector4 c, float t)
        {
            var u = 1f - t;
            return  (u * u) * a +
                    (2f * t * u) * b +
                    (t * t) * c;
        }

        public static Vector2 GetPoint(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
        {
            var u = 1f - t;
            var uu = u * u;
            var tt = t * t;
            return  (uu * u) * a +
                    (uu * 3 * t) * b +
                    (tt * 3 * u) * c +
                    (tt * t) * d;
        }

        public static Vector3 GetPoint(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
        {
            var u = 1f - t;
            var uu = u * u;
            var tt = t * t;
            return  (uu * u) * a +
                    (uu * 3 * t) * b +
                    (tt * 3 * u) * c +
                    (tt * t) * d;
        }

        public static Vector4 GetPoint(Vector4 a, Vector4 b, Vector4 c, Vector4 d, float t)
        {
            var u = 1f - t;
            var uu = u * u;
            var tt = t * t;
            return  (uu * u) * a +
                    (uu * 3 * t) * b +
                    (tt * 3 * u) * c +
                    (tt * t) * d;
        }
    }
}