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
    }
}