using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Utilities.Geometry;

namespace MyPong.Core
{
    public static class CastHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LineCast(Line l, Vector2 start, Vector2 end, out Vector2 point, out Vector2 normal)
        {
            if (GeometryMathHelper.IntersectLineSegments(l.p1, l.p2, start, end, out point))
            {
                var pr = Vector2.Perpendicular(l.p2 - l.p1).normalized;
                normal = Vector2.Dot(start - point, pr) > 0f ? pr : -pr;
                return true;
            }
            normal = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LineCast(Circle c, Vector2 start, Vector2 end, out Vector2 point, out Vector2 normal)
        {
            if (GeometryMathHelper.IntersectCircleLineSegment(c.p, c.r, start, end, out point, out _))
            {
                normal = (point - c.p).normalized;
                return true;
            }
            normal = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LineCast(Capsule c, Vector2 start, Vector2 end, out Vector2 point, out Vector2 normal)
        {
            point = default;
            normal = default;
            var c1 = new Circle(c.p1, c.r);
            var c2 = new Circle(c.p2, c.r);
            var pr = Vector2.Perpendicular(c.p2 - c.p1).normalized * c.r;
            var l1 = new Line(c.p1 + pr, c.p2 + pr);
            var l2 = new Line(c.p1 - pr, c.p2 - pr);
            Vector2 pointTmp, normalTmp;
            float sqrDistance = float.MaxValue;
            bool found = false;
            
            if (LineCast(c1, start, end, out pointTmp, out normalTmp))
            {
                var sqrDistanceTmp = (pointTmp - start).sqrMagnitude;
                if (sqrDistanceTmp < sqrDistance)
                {
                    point = pointTmp;
                    normal = normalTmp;
                    sqrDistance = sqrDistanceTmp;
                    found = true;
                }
            }
            
            if (LineCast(c2, start, end, out pointTmp, out normalTmp))
            {
                var sqrDistanceTmp = (pointTmp - start).sqrMagnitude;
                if (sqrDistanceTmp < sqrDistance)
                {
                    point = pointTmp;
                    normal = normalTmp;
                    sqrDistance = sqrDistanceTmp;
                    found = true;
                }
            }
            
            if (LineCast(l1, start, end, out pointTmp, out normalTmp))
            {
                var sqrDistanceTmp = (pointTmp - start).sqrMagnitude;
                if (sqrDistanceTmp < sqrDistance)
                {
                    point = pointTmp;
                    normal = normalTmp;
                    sqrDistance = sqrDistanceTmp;
                    found = true;
                }
            }
            
            if (LineCast(l2, start, end, out pointTmp, out normalTmp))
            {
                var sqrDistanceTmp = (pointTmp - start).sqrMagnitude;
                if (sqrDistanceTmp < sqrDistance)
                {
                    point = pointTmp;
                    normal = normalTmp;
                    // sqrDistance = sqrDistanceTmp;
                    found = true;
                }
            }

            return found;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CircleCast(Line l, Vector2 start, Vector2 end, float r, out Vector2 point, out Vector2 normal)
        {
            return LineCast(new Capsule(l.p1, l.p2, r), start, end, out point, out normal);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CircleCast(Circle c, Vector2 start, Vector2 end, float r, out Vector2 point, out Vector2 normal)
        {
            return LineCast(new Circle(c.p, c.r + r), start, end, out point, out normal);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CircleCast(Capsule c, Vector2 start, Vector2 end, float r, out Vector2 point, out Vector2 normal)
        {
            return LineCast(new Capsule(c.p1, c.p2, c.r + r), start, end, out point, out normal);
        }
    }
}