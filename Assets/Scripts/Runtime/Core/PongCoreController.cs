using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Extensions.Vectors;
using MyPong.Core.Objects;
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
        public async void StartCoreGameplay()
        {
            if (_core != null) return;
            var ratio = new Vector2(9f, 16f);
            _core = new(
                ratio * 1.5f,
                4f,
                1f,
                20f,
                50f,
                1f,
                5f,
                1f);
            _view = new(_core);
            _view.Init().Forget();

            var players = UnetWrapper.GetAllPlayers();
            
            var player0 = players.First(a => UnetWrapper.ItsMe(a.OwnerClientId));
            player0.OnPositionControl.Subscribe(v => SetPaddleTargetPosition(0, v)).AddTo(_disposable);
            
#if !PONG_BOT
            var player1 = players.First(a => !UnetWrapper.ItsMe(a.OwnerClientId));
            player1.OnPositionControl.Subscribe(v => SetPaddleTargetPosition(1, -v)).AddTo(_disposable);
#endif

            await UniTask.Delay(Constants.Gameplay.StartTimerSeconds * 1000);

            _disposable?.Dispose();
            _disposable = new();
            Observable.EveryUpdate().Subscribe(Update).AddTo(_disposable);
        }

        private void SetPaddleTargetPosition(int id, float position)
        {
            _core.Paddles[id].targetPosition = position;
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
#if PONG_BOT
            _core.Paddles[1].targetPosition = _core.Ball.position.x;
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