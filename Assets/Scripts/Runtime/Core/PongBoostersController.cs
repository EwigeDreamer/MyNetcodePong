using System;
using Cysharp.Threading.Tasks;
using MyPong.Core.Boosters;
using MyPong.View;
using UniRx;
using UnityEngine;
using Utilities;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace MyPong.Core
{
    [UnityEngine.Scripting.Preserve]
    public class PongBoostersController
    {
        private readonly PongCoreController PongCoreController;
        
        [UnityEngine.Scripting.Preserve]
        public PongBoostersController(PongCoreController pongCoreController)
        {
            PongCoreController = pongCoreController;
        }
        
        private CompositeDisposable _interval;

        public void StartBoosters()
        {
            _interval?.Dispose();
            _interval = new();
            Observable.Interval(TimeSpan.FromSeconds(Constants.Gameplay.BoostersSpawnInterval))
                .Subscribe(_ => CreateBooster())
                .AddTo(_interval);
        }

        public void StopBoosters()
        {
            _interval?.Dispose();
            _interval = null;
        }

        private bool CanDo => !PongCoreController.IsPaused && PongCoreController.IsRunning;

        private void CreateBooster()
        {
            if (!CanDo) return;
            switch (Random.Range(0,3))
            {
                case 0: CreateExpandPaddleBooster(); break;
                case 1: CreateNarrowPaddleBooster(); break;
                case 2: CreateSpeedBallBooster(); break;
            }
        }

        private Vector2 GetRandomBoosterPosition()
        {
            if (!CanDo) return default;
            var core = PongCoreController.Core;
            var xRange = core.Field.Size.x;
            var yRange = core.Field.Size.y * 0.1f;
            var x = Random.Range(-xRange, xRange);
            var y = Random.Range(-yRange, yRange);
            return new Vector2(x, y);
        }
        
        private async void CreateExpandPaddleBooster()
        {
            if (!CanDo) return;
            var pos = GetRandomBoosterPosition();
            var booster = new ExpandPaddle(pos);
            var prefab = await AssetService.LoadGameObjectFromAddressablesAsync<ExpandPaddleView>(ResourceType.View);
            if (!CanDo) return;
            var view = Object.Instantiate(prefab).Init(booster);
            view.NetworkObject.Spawn();
            view.transform.SetParent(PongCoreController.View.Container);
            view.UpdateView();

            await UniTask.Delay(TimeSpan.FromSeconds(booster.lifeTime));
            
            if (view != null)
                Object.Destroy(view.gameObject);
        }

        private async void CreateNarrowPaddleBooster()
        {
            if (!CanDo) return;
            var pos = GetRandomBoosterPosition();
            var booster = new NarrowPaddle(pos);
            var prefab = await AssetService.LoadGameObjectFromAddressablesAsync<NarrowPaddleView>(ResourceType.View);
            if (!CanDo) return;
            var view = Object.Instantiate(prefab).Init(booster);
            view.NetworkObject.Spawn();
            view.transform.SetParent(PongCoreController.View.Container);
            view.UpdateView();

            await UniTask.Delay(TimeSpan.FromSeconds(booster.lifeTime));
            
            if (view != null)
                Object.Destroy(view.gameObject);
        }

        private async void CreateSpeedBallBooster()
        {
            if (!CanDo) return;
            var pos = GetRandomBoosterPosition();
            var booster = new SpeedBall(pos);
            var prefab = await AssetService.LoadGameObjectFromAddressablesAsync<SpeedBallView>(ResourceType.View);
            if (!CanDo) return;
            var view = Object.Instantiate(prefab).Init(booster);
            view.NetworkObject.Spawn();
            view.transform.SetParent(PongCoreController.View.Container);
            view.UpdateView();

            await UniTask.Delay(TimeSpan.FromSeconds(booster.lifeTime));
            
            if (view != null)
                Object.Destroy(view.gameObject);
        }
    }
}