using System;
using Extensions.Vectors;
using UnityEngine;

namespace MyPong
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        public Camera Camera { get; private set; }

        private void Awake()
        {
            Camera = GetComponent<Camera>();
        }

        public void FocusOnField(Vector2 fieldScale)
        {
            var screenScale = new Vector2(Screen.width, Screen.height);
            var screenCenter = screenScale / 2f;
            var safeArea = Screen.safeArea;
            var safeScale = new Vector2(safeArea.width, safeArea.height);
            var safeMin = safeArea.position;
            var safeMax = safeArea.position + safeArea.size;
            var safeCenter = (safeMin + safeMax) / 2f;
            var safeOffset = safeCenter - screenCenter;

            var fieldRatio = fieldScale.x / fieldScale.y;
            var safeRatio = safeArea.width / safeArea.height;

            if (fieldRatio < safeRatio)
            {
                //выставляем по высоте
                var scaleFactor = screenScale.y / safeScale.y;
                var cameraScale = fieldScale.y * scaleFactor;
                SetCameraSizeVertical(cameraScale / 2f);
                var pixelToMeter = cameraScale / screenScale.y;
                Camera.transform.position = (-safeOffset * pixelToMeter).ToV3_xy0().SetZ(Camera.transform.position.z);
            }
            else
            {
                //выставляем по ширине
                var scaleFactor = screenScale.x / safeScale.x;
                var cameraScale = fieldScale.x * scaleFactor;
                SetCameraSizeHorizontal(cameraScale / 2f);
                var pixelToMeter = cameraScale / screenScale.x;
                Camera.transform.position = (-safeOffset * pixelToMeter).ToV3_xy0().SetZ(Camera.transform.position.z);
            }
        }

        private void SetCameraSizeVertical(float size)
        {
            Camera.orthographicSize = size;
        }

        private void SetCameraSizeHorizontal(float size)
        {
            Camera.orthographicSize = size / Camera.aspect;
        }
    }
}