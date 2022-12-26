using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Extensions.Vectors;
using MyPong.Core.Boosters;
using MyPong.Core.Objects;
using MyPong.Networking;
using MyPong.View;
using UniRx;
using UnityEngine;
using Utilities;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace MyPong.Core
{
    [UnityEngine.Scripting.Preserve]
    public class PongCoreController : IDisposable
    {
        private readonly UnetWrapper UnetWrapper;

        public PongCore Core { get; private set; }
        public PongView View { get; private set; }

        private CompositeDisposable _disposable;
        private int[] _scores = new int[2];

        private readonly Subject<int> _onGoal = new();
        private readonly Subject<Unit> _onGameOver = new();

        public IObservable<int> OnGoal => _onGoal;
        public IObservable<Unit> OnGameOver => _onGameOver;
        public IReadOnlyList<int> Scores => _scores;
        public bool IsRunning => Core != null;
        public bool IsPaused { get; private set; } = false;

        [UnityEngine.Scripting.Preserve]
        public PongCoreController(UnetWrapper unetWrapper)
        {
            UnetWrapper = unetWrapper;
        }
        
        //call only on HOST
        public async void StartCoreGameplay()
        {
            if (Core != null) return;
            var ratio = new Vector2(9f, 16f);
            Core = new(
                ratio * 1.5f,
                4f,
                1f,
                20f,
                50f,
                1f,
                5f,
                1f);
            View = new(Core);
            View.Init().Forget();
            
            _disposable?.Dispose();
            _disposable = new();

            _scores[0] = 0;
            _scores[1] = 0;
            Core.OnGoal.Where(id => id == 0).Subscribe(_ => _scores[0]++).AddTo(_disposable);
            Core.OnGoal.Where(id => id == 1).Subscribe(_ => _scores[1]++).AddTo(_disposable);

            Core.OnGoal.Subscribe(_onGoal.OnNext).AddTo(_disposable);
            var players = UnetWrapper.GetAllPlayers();
            
            var player0 = players.First(a => UnetWrapper.ItsMe(a.OwnerClientId));
            player0.OnPositionControl.Subscribe(v => SetPaddleTargetPosition(0, v)).AddTo(_disposable);
            Core.OnGoal.Subscribe(_ => player0.SetScoreClientRpc(_scores[0], _scores[1])).AddTo(_disposable);
            
#if !PONG_BOT
            var player1 = players.First(a => !UnetWrapper.ItsMe(a.OwnerClientId));
            player1.OnPositionControl.Subscribe(v => SetPaddleTargetPosition(1, -v)).AddTo(_disposable);
            Core.OnGoal.Subscribe(_ => player1.SetScoreClientRpc(_scores[1], _scores[0])).AddTo(_disposable);
#endif
            await UniTask.Delay((Constants.Gameplay.StartTimerSeconds + 1) * 1000);
            Core.OnGoal.Subscribe(_ => CheckScore()).AddTo(_disposable);
            Observable.EveryUpdate().Subscribe(Update).AddTo(_disposable);

            Resume();
        }

        private void CheckScore()
        {
            var max = Constants.Gameplay.MaxScore;
            if (_scores[0] >= max || _scores[1] >= max)
                _onGameOver.OnNext(default);
        }

        private void SetPaddleTargetPosition(int id, float position)
        {
            Core.Paddles[id].targetPosition = position;
        }

        public void StopCoreGameplay()
        {
            _disposable?.Dispose();
            View?.Dispose();
            View = null;
            Core = null;
        }

        private void Update(long _)
        {
            if (IsPaused) return;
            
            var dt = Time.deltaTime;
#if PONG_BOT
            Core.Paddles[1].targetPosition = Core.Ball.position.x;
#endif
            Core.Update(dt);
            View.UpdateView();
        }

        public void Dispose()
        {
            StopCoreGameplay();
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public void Resume()
        {
            IsPaused = false;
        }
    }
}