using System;
using Cysharp.Threading.Tasks;
using MyPong.View;
using UnityEngine;

namespace MyPong.Core
{
    [UnityEngine.Scripting.Preserve]
    public class PongCoreController : IDisposable
    {
        private PongCore _core;
        private PongView _view;

        private readonly CameraController CameraController;

        public PongCoreController(CameraController cameraController)
        {
            CameraController = cameraController;
        }
        
        public void StartCoreGameplay()
        {
            var ratio = new Vector2(9f, 16f);
            _core = new(
                ratio * 1.5f,
                4f,
                1f,
                3f,
                1f,
                2f);
            CameraController.FocusOnField(_core.Field.Scale);
            _view = new(_core);
            _view.Init().Forget();
        }

        public void Dispose()
        {
            _view?.Dispose();
        }
    }
}