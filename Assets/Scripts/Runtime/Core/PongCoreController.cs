using System;
using System.Collections.Generic;
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
        private int[] _scores = new int[2];
        private bool _isPaused = false;

        private readonly Subject<int> _onGoal = new();
        private readonly Subject<Unit> _onGameOver = new();
        public IObservable<int> OnGoal => _onGoal;
        public IObservable<Unit> OnGameOver => _onGameOver;
        public IReadOnlyList<int> Scores => _scores;

        public bool IsRunning => _core != null;

        [UnityEngine.Scripting.Preserve]
        public PongCoreController(UnetWrapper unetWrapper)
        {
            UnetWrapper = unetWrapper;
        }
        
        //call only on HOST
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
            
            _disposable?.Dispose();
            _disposable = new();

            _scores[0] = 0;
            _scores[1] = 0;
            _core.OnGoal.Where(id => id == 0).Subscribe(_ => _scores[0]++).AddTo(_disposable);
            _core.OnGoal.Where(id => id == 1).Subscribe(_ => _scores[1]++).AddTo(_disposable);

            _core.OnGoal.Subscribe(_onGoal.OnNext).AddTo(_disposable);
            var players = UnetWrapper.GetAllPlayers();
            
            var player0 = players.First(a => UnetWrapper.ItsMe(a.OwnerClientId));
            player0.OnPositionControl.Subscribe(v => SetPaddleTargetPosition(0, v)).AddTo(_disposable);
            _core.OnGoal.Subscribe(_ => player0.SetScoreClientRpc(_scores[0], _scores[1])).AddTo(_disposable);
            
#if !PONG_BOT
            var player1 = players.First(a => !UnetWrapper.ItsMe(a.OwnerClientId));
            player1.OnPositionControl.Subscribe(v => SetPaddleTargetPosition(1, -v)).AddTo(_disposable);
            _core.OnGoal.Subscribe(_ => player1.SetScoreClientRpc(_scores[1], _scores[0])).AddTo(_disposable);
#endif
            await UniTask.Delay(Constants.Gameplay.StartTimerSeconds * 1000);
            _core.OnGoal.Subscribe(_ => CheckScore()).AddTo(_disposable);
            Observable.EveryUpdate().Subscribe(Update).AddTo(_disposable);
        }

        private void CheckScore()
        {
            var max = Constants.Gameplay.MaxScore;
            if (_scores[0] >= max || _scores[1] >= max)
                _onGameOver.OnNext(default);
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
            if (_isPaused) return;
            
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

        public void Pause()
        {
            _isPaused = true;
        }

        public void Resume()
        {
            _isPaused = false;
        }
    }
}