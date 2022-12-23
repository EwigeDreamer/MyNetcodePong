using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Utilities.Geometry
{
    public static class GeometryMathHelper
    {
        //https://blog.dakwamine.fr/?p=1943
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectLines(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out Vector2 point)
        {
            point = default;
            var tmp = ((double)b2.x - b1.x) * ((double)a2.y - a1.y) - ((double)b2.y - b1.y) * ((double)a2.x - a1.x);
            if (Mathf.Approximately((float)tmp, 0f)) return false;
            var mu = (((double)a1.x - b1.x) * ((double)a2.y - a1.y) - ((double)a1.y - b1.y) * ((double)a2.x - a1.x)) / tmp;
            point = new(b1.x + (b2.x - b1.x) * (float)mu, b1.y + (b2.y - b1.y) * (float)mu);
            return true;
        }

        //https://habr.com/ru/post/267037/
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectLineSegments(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out Vector2 point)
        {
            point = default;
            Vector3 cut1 = a2 - a1;
            Vector3 cut2 = b2 - b1;
            Vector3 prod1, prod2;
            
            prod1 = Vector3.Cross(cut1, b1 - a1);
            prod2 = Vector3.Cross(cut1, b2 - a1);
            if (Mathf.Approximately(prod1.z, 0f)
                || Mathf.Approximately(prod2.z, 0f)
                || Math.Sign(prod1.z) == Math.Sign(prod2.z))
                return false;
            
            prod1 = Vector3.Cross(cut2, a1 - b1);
            prod2 = Vector3.Cross(cut2, a2 - b1);
            if (Mathf.Approximately(prod1.z, 0f)
                || Mathf.Approximately(prod2.z, 0f)
                || Math.Sign(prod1.z) == Math.Sign(prod2.z))
                return false;

            point.x = a1.x + cut1.x * Mathf.Abs(prod1.z) / Mathf.Abs(prod2.z - prod1.z);
            point.y = a1.y + cut1.y * Mathf.Abs(prod1.z) / Mathf.Abs(prod2.z - prod1.z);
            return true;
        }
        
        //https://pro-prof.com/forums/topic/canonical_equation_prolog
        //https://e-maxx.ru/algo/circle_line_intersection
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectCircleLine(Vector2 p0, float r, Vector2 p1, Vector2 p2, out Vector2 point1, out Vector2 point2)
        {
            var a = (double)p1.y - p2.y;
            var b = (double)p2.x - p1.x;
            var c = ((double)p1.x-p0.x) * ((double)p2.y-p0.y) - ((double)p2.x-p0.x) * ((double)p1.y-p0.y);

            var aabb = a * a + b * b;
            var x0 = -a * c / aabb;
            var y0 = -b * c / aabb;
            
            if (c * c > r * r * aabb + float.Epsilon)
            {
                point1 = default;
                point2 = default;
                return false;
            }
            
            if (Math.Abs(c * c - r * r * aabb) < float.Epsilon)
            {
                var pc = new Vector2((float)x0, (float)y0);
                point1 = pc + p0;
                point2 = point1;
                return true;
            }
            
            var d = r * r - c * c / aabb;
            var mult = Math.Sqrt(d / aabb);
            var ax = x0 - b * mult;
            var ay = y0 + a * mult;
            var bx = x0 + b * mult;
            var by = y0 - a * mult;
            point1 = new Vector2((float) ax, (float) ay) + p0;
            point2 = new Vector2((float) bx, (float) by) + p0;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntersectCircleLineSegment(Vector2 p0, float r, Vector2 p1, Vector2 p2, out Vector2 point1, out Vector2 point2)
        {
            if (IntersectCircleLine(p0, r, p1, p2, out point1, out point2))
            {
                var isPoint1OnSegment = IsInRange(point1.x, p1.x, p2.x) && IsInRange(point1.y, p1.y, p2.y);
                var isPoint2OnSegment = IsInRange(point2.x, p1.x, p2.x) && IsInRange(point2.y, p1.y, p2.y);
                
                if (!isPoint1OnSegment)
                    point1 = point2;
                if (!isPoint2OnSegment)
                    point2 = point1;
                
                return isPoint1OnSegment || isPoint2OnSegment;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRange(float x, float a, float b)
        {
            return x >= Math.Min(a, b) && x <= Math.Max(a, b);
        }
    }
}