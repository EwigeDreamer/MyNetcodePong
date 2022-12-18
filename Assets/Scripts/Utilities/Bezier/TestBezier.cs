using Extensions.Colors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Bezier;

namespace Utilities.Bezier.Test
{
    public class TestBezier : MonoBehaviour
    {
        public Transform a1;
        public Transform b1;
        public Transform c1;
        [Space]
        public Transform a2;
        public Transform b2;
        public Transform c2;
        public Transform d2;


        private void OnDrawGizmos()
        {
            if (a1 && b1 && c1)
            {
                var color_tmp = Gizmos.color;
                Gizmos.color = Color.yellow.SetAlpha(0.5f);
                Gizmos.DrawLine(a1.position, b1.position);
                Gizmos.DrawLine(b1.position, c1.position);
                Gizmos.DrawSphere(b1.position, 0.1f);
                Gizmos.color = Color.Lerp(Color.green, Color.white, 0.5f);
                for (int i = 0; i < 100; ++i)
                {
                    Gizmos.DrawLine(
                        BezierUtility.GetPoint(a1.position, b1.position, c1.position, i / 100f),
                        BezierUtility.GetPoint(a1.position, b1.position, c1.position, (i + 1) / 100f));
                }
                Gizmos.DrawSphere(a1.position, 0.1f);
                Gizmos.DrawSphere(c1.position, 0.1f);
                Gizmos.color = color_tmp;
            }
            if (a2 && b2 && c2 && d2)
            {
                var color_tmp = Gizmos.color;
                Gizmos.color = Color.yellow.SetAlpha(0.5f);
                Gizmos.DrawLine(a2.position, b2.position);
                Gizmos.DrawLine(b2.position, c2.position);
                Gizmos.DrawLine(c2.position, d2.position);
                Gizmos.DrawSphere(b2.position, 0.1f);
                Gizmos.DrawSphere(c2.position, 0.1f);
                Gizmos.color = Color.Lerp(Color.green, Color.white, 0.5f);
                for (int i = 0; i < 100; ++i)
                {
                    Gizmos.DrawLine(
                        BezierUtility.GetPoint(a2.position, b2.position, c2.position, d2.position, i / 100f),
                        BezierUtility.GetPoint(a2.position, b2.position, c2.position, d2.position, (i + 1) / 100f));
                }
                Gizmos.DrawSphere(a2.position, 0.1f);
                Gizmos.DrawSphere(d2.position, 0.1f);
                Gizmos.color = color_tmp;
            }
        }
    }
}
