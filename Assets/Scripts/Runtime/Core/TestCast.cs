using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR

namespace MyPong.Core.Test
{
    public class TestCast : MonoBehaviour
    {
#if UNITY_EDITOR
        private static Vector2 Step(float t) => Vector2.right * 3f * t;
        
        private void OnDrawGizmos()
        {
            LineLineCast();
            LineCircleCast();
            LineCapsuleCast();
            CircleLineCast();
            CircleCircleCast();
            CircleCapsuleCast();
        }

        [System.Serializable]
        private class LineLineCastData
        {
            public Vector2 p1 = Vector2.left + Vector2.down * 0.5f + Step(1);
            public Vector2 p2 = Vector2.right + Vector2.up * 0.5f + Step(1);
            public Vector2 p3 = Vector2.left + Vector2.up * 0.5f + Step(1);
            public Vector2 p4 = Vector2.right + Vector2.down * 0.5f + Step(1);
        }
        [SerializeField] private LineLineCastData _lineLineCastData = new();
        private void LineLineCast()
        {
            var d = _lineLineCastData;
            var l1 = new Line(d.p1, d.p2);
            var l2 = new Line(d.p3, d.p4);
            
            DrawLine(l1, Color.white);
            DrawCircle(new Circle(l2.p1, 0.1f), Color.yellow);
            DrawLine(l2, Color.green);
            
            if (CastHelper.LineCast(
                    l1,
                    l2.p1,
                    l2.p2,
                    out var p,
                    out var n))
            {
                Gizmos.DrawWireSphere(p, 0.15f);
                Gizmos.DrawLine(p, p + n);
            }
        }

        [System.Serializable]
        private class LineCircleCastData
        {
            public Vector2 p = Vector2.zero + Step(2);
            public float r = 0.5f;
            public Vector2 p1 = Vector2.left + Vector2.up + Step(2);
            public Vector2 p2 = Vector2.right + Step(2);
        }
        [SerializeField] private LineCircleCastData _lineCircleCastData = new();
        private void LineCircleCast()
        {
            var d = _lineCircleCastData;
            var c = new Circle(d.p, d.r);
            var l = new Line(d.p1, d.p2);
            
            DrawCircle(c, Color.white);
            DrawCircle(new Circle(l.p1, 0.1f), Color.yellow);
            DrawLine(l, Color.green);
            
            if (CastHelper.LineCast(
                    c,
                    l.p1,
                    l.p2,
                    out var p,
                    out var n))
            {
                Gizmos.DrawWireSphere(p, 0.15f);
                Gizmos.DrawLine(p, p + n);
            }
        }

        [System.Serializable]
        private class LineCapsuleCastData
        {
            public Vector2 p1 = Vector2.left + Vector2.down * 0.5f + Step(3);
            public Vector2 p2 = Vector2.right + Vector2.up * 0.5f + Step(3);
            public float r = 0.25f;
            public Vector2 p3 = Vector2.left + Vector2.up * 0.5f + Step(3);
            public Vector2 p4 = Vector2.right + Vector2.down * 0.5f + Step(3);
        }
        [SerializeField] private LineCapsuleCastData _lineCapsuleCastData = new();
        private void LineCapsuleCast()
        {
            var d = _lineCapsuleCastData;
            var c = new Capsule(d.p1, d.p2, d.r);
            var l = new Line(d.p3, d.p4);
            
            DrawCapsule(c, Color.white);
            DrawCircle(new Circle(l.p1, 0.1f), Color.yellow);
            DrawLine(l, Color.green);
            
            if (CastHelper.LineCast(
                    c,
                    l.p1,
                    l.p2,
                    out var p,
                    out var n))
            {
                Gizmos.DrawWireSphere(p, 0.15f);
                Gizmos.DrawLine(p, p + n);
            }
        }

        [System.Serializable]
        private class CircleLineCastData
        {
            public Vector2 p1 = Vector2.left + Vector2.down * 0.5f + Step(4);
            public Vector2 p2 = Vector2.right + Vector2.up * 0.5f + Step(4);
            public Vector2 p3 = Vector2.left + Vector2.up * 0.5f + Step(4);
            public Vector2 p4 = Vector2.right + Vector2.down * 0.5f + Step(4);
            public float r = 0.25f;
        }
        [SerializeField] private CircleLineCastData _circleLineCastData = new();
        private void CircleLineCast()
        {
            var d = _circleLineCastData;
            var l = new Line(d.p1, d.p2);
            var c = new Capsule(d.p3, d.p4, d.r);
            
            DrawLine(l, Color.white);
            DrawCircle(new Circle(c.p1, 0.1f), Color.yellow);
            DrawCapsule(c, Color.green);
            
            if (CastHelper.CircleCast(
                    l,
                    c.p1,
                    c.p2,
                    c.r,
                    out var p,
                    out var n))
            {
                Gizmos.DrawWireSphere(p, c.r);
                Gizmos.DrawLine(p, p + n);
            }
        }

        [System.Serializable]
        private class CircleCircleCastData
        {
            public Vector2 p = Vector2.zero + Step(5);
            public float r1 = 0.5f;
            public Vector2 p1 = Vector2.left + Vector2.up + Step(5);
            public Vector2 p2 = Vector2.right + Step(5);
            public float r2 = 0.25f;
        }
        [SerializeField] private CircleCircleCastData _circleCircleCastData = new();
        private void CircleCircleCast()
        {
            var d = _circleCircleCastData;
            var cr = new Circle(d.p, d.r1);
            var cp = new Capsule(d.p1, d.p2, d.r2);
            
            DrawCircle(cr, Color.white);
            DrawCircle(new Circle(cp.p1, 0.1f), Color.yellow);
            DrawCapsule(cp, Color.green);
            
            if (CastHelper.CircleCast(
                    cr,
                    cp.p1,
                    cp.p2,
                    cp.r,
                    out var p,
                    out var n))
            {
                Gizmos.DrawWireSphere(p, cp.r);
                Gizmos.DrawLine(p, p + n);
            }
        }

        [System.Serializable]
        private class CircleCapsuleCastData
        {
            public Vector2 p1 = Vector2.left + Vector2.down * 0.5f + Step(6);
            public Vector2 p2 = Vector2.right + Vector2.up * 0.5f + Step(6);
            public float r1 = 0.25f;
            public Vector2 p3 = Vector2.left + Vector2.up * 0.5f + Step(6);
            public Vector2 p4 = Vector2.right + Vector2.down * 0.5f + Step(6);
            public float r2 = 0.25f;
        }
        [SerializeField] private CircleCapsuleCastData _circleCapsuleCastData = new();
        private void CircleCapsuleCast()
        {
            var d = _circleCapsuleCastData;
            var c1 = new Capsule(d.p1, d.p2, d.r1);
            var c2 = new Capsule(d.p3, d.p4, d.r2);
            
            DrawCapsule(c1, Color.white);
            DrawCircle(new Circle(c2.p1, 0.1f), Color.yellow);
            DrawCapsule(c2, Color.green);
            
            if (CastHelper.CircleCast(
                    c1,
                    c2.p1,
                    c2.p2,
                    c2.r,
                    out var p,
                    out var n))
            {
                Gizmos.DrawWireSphere(p, c2.r);
                Gizmos.DrawLine(p, p + n);
            }
        }
        
        
        
        
        
        
        

        private void DrawLine(Line l, Color col)
        {
            var colTmp = Gizmos.color;
            Gizmos.color = col;
            Gizmos.DrawLine(l.p1, l.p2);
            Gizmos.color = colTmp;
        }

        private void DrawCircle(Circle c, Color col)
        {
            var colTmp1 = Gizmos.color;
            var colTmp2 = Handles.color;
            Gizmos.color = col;
            Handles.color = col;
            Handles.DrawWireDisc(c.p, Vector3.forward, c.r);
            Gizmos.color = colTmp1;
            Handles.color = colTmp2;
        }

        private void DrawCapsule(Capsule c, Color col)
        {
            var colTmp1 = Gizmos.color;
            var colTmp2 = Handles.color;
            Gizmos.color = col;
            Handles.color = col;
            Handles.DrawWireDisc(c.p1, Vector3.forward, c.r);
            Handles.DrawWireDisc(c.p2, Vector3.forward, c.r);
            var pr = Vector2.Perpendicular(c.p2 - c.p1).normalized * c.r;
            Gizmos.DrawLine(c.p1 + pr, c.p2 + pr);
            Gizmos.DrawLine(c.p1 - pr, c.p2 - pr);
            Gizmos.color = colTmp1;
            Handles.color = colTmp2;
        }
#endif //UNITY_EDITOR
    }
}