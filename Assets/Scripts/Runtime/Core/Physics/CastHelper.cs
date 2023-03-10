using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Utilities.Geometry;

namespace MyPong.Core.Physics
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
        public static bool LineCast(Box b, Vector2 start, Vector2 end, out Vector2 point, out Vector2 normal, out bool isInside)
        {
            point = default;
            normal = default;
            isInside = default;
            
            var pr = Vector2.Perpendicular(b.p2 - b.p1).normalized * b.r;
            var p1 = b.p1 - pr;
            var p2 = b.p2 - pr;
            var p3 = b.p2 + pr;
            var p4 = b.p1 + pr;
            var l1 = new Line(p1, p2);
            var l2 = new Line(p2, p3);
            var l3 = new Line(p3, p4);
            var l4 = new Line(p4, p1);
            
            isInside = !IsOutside(l1, l2, l3, l4, start);
            if (isInside) return false;
            
            Vector2 pointTmp, normalTmp;
            float sqrDistance = float.MaxValue;
            bool found = false;
            
            
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
                    sqrDistance = sqrDistanceTmp;
                    found = true;
                }
            }
            
            if (LineCast(l3, start, end, out pointTmp, out normalTmp))
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
            
            if (LineCast(l4, start, end, out pointTmp, out normalTmp))
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
        public static bool LineCast(Circle c, Vector2 start, Vector2 end, out Vector2 point, out Vector2 normal, out bool isInside)
        {
            point = default;
            normal = default;

            isInside = !IsOutside(c, start);
            if (isInside) return false;
            
            if (GeometryMathHelper.IntersectCircleLineSegment(c.p, c.r, start, end, out point, out _))
            {
                normal = (point - c.p).normalized;
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LineCast(Capsule c, Vector2 start, Vector2 end, out Vector2 point, out Vector2 normal, out bool isInside)
        {
            point = default;
            normal = default;
            var c1 = new Circle(c.p1, c.r);
            var c2 = new Circle(c.p2, c.r);
            var b = new Box(c.p1, c.p2, c.r);

            Vector2 pointTmp, normalTmp;
            float sqrDistance = float.MaxValue;
            bool found = false;
            
            if (LineCast(c1, start, end, out pointTmp, out normalTmp, out isInside))
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
            else if (isInside) return false;
            
            if (LineCast(c2, start, end, out pointTmp, out normalTmp, out isInside))
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
            else if (isInside) return false;
            
            if (LineCast(b, start, end, out pointTmp, out normalTmp, out isInside))
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
            else if (isInside) return false;

            return found;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CircleCast(Line l, Vector2 start, Vector2 end, float r, out Vector2 point, out Vector2 normal, out bool isInside)
        {
            return LineCast(new Capsule(l.p1, l.p2, r), start, end, out point, out normal, out isInside);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CircleCast(Circle c, Vector2 start, Vector2 end, float r, out Vector2 point, out Vector2 normal, out bool isInside)
        {
            return LineCast(new Circle(c.p, c.r + r), start, end, out point, out normal, out isInside);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CircleCast(Capsule c, Vector2 start, Vector2 end, float r, out Vector2 point, out Vector2 normal, out bool isInside)
        {
            return LineCast(new Capsule(c.p1, c.p2, c.r + r), start, end, out point, out normal, out isInside);
        }
        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CircleCast(Line l, Capsule cast, out Vector2 point, out Vector2 normal, out bool isInside)
        {
            return CircleCast(l, cast.p1, cast.p2, cast.r, out point, out normal, out isInside);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CircleCast(Circle c, Capsule cast, out Vector2 point, out Vector2 normal, out bool isInside)
        {
            return CircleCast(c, cast.p1, cast.p2, cast.r, out point, out normal, out isInside);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CircleCast(Capsule c, Capsule cast, out Vector2 point, out Vector2 normal, out bool isInside)
        {
            return CircleCast(c, cast.p1, cast.p2, cast.r, out point, out normal, out isInside);
        }
        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOutside(this Circle c, Vector2 p)
        {
            return (p - c.p).sqrMagnitude > c.r * c.r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOutside(this Box b, Vector2 p)
        {
            var pr = Vector2.Perpendicular(b.p2 - b.p1).normalized * b.r;
            var p1 = b.p1 - pr;
            var p2 = b.p2 - pr;
            var p3 = b.p2 + pr;
            var p4 = b.p1 + pr;
            var l1 = new Line(p1, p2);
            var l2 = new Line(p2, p3);
            var l3 = new Line(p3, p4);
            var l4 = new Line(p4, p1);
            return IsOutside(l1, l2, l3, l4, p);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOutside(Line l1, Line l2, Line l3, Line l4, Vector2 p)
        {
            return Vector3.Cross(l1.p1 - l1.p2, p - l1.p1).z > 0f
                   || Vector3.Cross(l2.p1 - l2.p2, p - l2.p1).z > 0f
                   || Vector3.Cross(l3.p1 - l3.p2, p - l3.p1).z > 0f
                   || Vector3.Cross(l4.p1 - l4.p2, p - l4.p1).z > 0f;
        }
        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOutside(this Capsule c, Vector2 p)
        {
            return IsOutside(new Circle(c.p1, c.r), p)
                   && IsOutside(new Circle(c.p2, c.r), p)
                   && IsOutside(new Box(c.p1, c.p2, c.r), p);
        }
    }
}