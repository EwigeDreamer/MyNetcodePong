using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Extensions.Vectors;
using MyPong.Networking;
using MyPong.View;
using UniRx;
using UnityEngine;

namespace MyPong.Core
{
    [UnityEngine.Scripting.Preserve]
    public class PongCoreController : IDisposable
    {
        private readonly UnetWrapper UnetWrapper;
        
        private PongCore _core;
        private PongView _view;

        private CompositeDisposable _disposable;

        public bool IsRunning => _core != null;

        [UnityEngine.Scripting.Preserve]
        public PongCoreController(UnetWrapper unetWrapper)
        {
            UnetWrapper = unetWrapper;
        }
        
        //запускается только на HOST'е
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
                5f,
                1f);
            _view = new(_core);
            _view.Init().Forget();

            _disposable?.Dispose();
            _disposable = new();
            Observable.EveryUpdate().Subscribe(Update).AddTo(_disposable);

            var players = UnetWrapper.GetAllPlayers();
            var player0 = players.First(a => UnetWrapper.ItsMe(a.OwnerClientId));
            var player1 = players.First(a => !UnetWrapper.ItsMe(a.OwnerClientId));
            player0.OnPositionControl.Subscribe(v => _core.Paddles[0].SetPosition(v)).AddTo(_disposable);
            player1.OnPositionControl.Subscribe(v => _core.Paddles[1].SetPosition(-v)).AddTo(_disposable);
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
#if UNITY_EDITOR
            _core.Paddles[1].position = _core.Paddles[1].position.SetX(_core.Ball.position.x);
#endif
            _core.Update(dt);
            _view.UpdateView();
        }

        public void Dispose()
        {
            StopCoreGameplay();
        }
    }
}