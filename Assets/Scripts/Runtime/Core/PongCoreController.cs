using System;
using Cysharp.Threading.Tasks;
using MyPong.View;
using UniRx;
using UnityEngine;

namespace MyPong.Core
{
    [UnityEngine.Scripting.Preserve]
    public class PongCoreController : IDisposable
    {
        private PongCore _core;
        private PongView _view;

        private CompositeDisposable _disposable;
        
        public void StartCoreGameplay()
        {
            if (_core != null) return;
            var ratio = new Vector2(9f, 16f);
            _core = new(
                ratio * 1.5f,
                4f,
                1f,
                3f,
                1f,
                2f);
            _view = new(_core);
            _view.Init().Forget();

            _disposable?.Dispose();
            _disposable = new();
            Observable.EveryUpdate().Subscribe(Update).AddTo(_disposable);
        }

        public void StopCoreGameplay()
        {
            _disposable?.Dispose();
            _view?.Dispose();
            _view = null;
            _core = null;
        }

        private void Update(long _)
        {
            var dt = Time.deltaTime;
            _core.Update(dt);
            _view.UpdateView();
        }

        public void Dispose()
        {
            StopCoreGameplay();
        }
    }
}