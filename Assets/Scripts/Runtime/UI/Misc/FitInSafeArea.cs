using System;
using UnityEngine;

namespace MyPong.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class FitInSafeArea : MonoBehaviour
    {
        private void Awake()
        {
            var rtr = GetComponent<RectTransform>();
            var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            rtr.anchorMin = anchorMin;
            rtr.anchorMax = anchorMax;
        }
    }
}