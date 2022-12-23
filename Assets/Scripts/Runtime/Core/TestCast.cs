using System;
using UnityEngine;

namespace MyPong.Core
{
    public class TestCast : MonoBehaviour
    {
        [SerializeField] private Transform p0;
        [SerializeField] private float r;
        [SerializeField] private Transform p1;
        [SerializeField] private Transform p2;

        private void OnDrawGizmos()
        {
            if (p0 != null)
            {
                UnityEditor.Handles.DrawWireDisc(p0.position, Vector3.forward, r);
            }
            
            if (p1 != null && p2 != null)
            {
                Gizmos.DrawLine(p1.position, p2.position);
                if (CastHelper.LineCast(
                    new Circle(p0.position, r),
                    p1.position,
                    p2.position,
                    out var p,
                    out var n))
                {
                    Gizmos.DrawWireSphere(p, 0.25f);
                    Gizmos.DrawLine(p, p + n);
                }
            }
        }
    }
}