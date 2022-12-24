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
        
        public void StartCoreGameplay()
        {
            _core = new(
                new Vector2(15f, 20f),
                4f,
                1f,
                3f,
                1f,
                2f);
            _view = new(_core);
            _view.Init().Forget();
        }

        public void Dispose()
        {
            _view?.Dispose();
        }
    }
}