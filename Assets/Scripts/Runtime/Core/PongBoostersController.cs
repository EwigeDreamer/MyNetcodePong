using System;
using System.Linq;
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
        
        private IDisposable _intervalSubscription;

        public void StartBoosters()
        {
            _intervalSubscription?.Dispose();
            _intervalSubscription = Observable.Interval(TimeSpan.FromSeconds(Constants.Gameplay.BoostersSpawnInterval))
                .Subscribe(_ => CreateBooster());
        }

        public void StopBoosters()
        {
            _intervalSubscription?.Dispose();
            _intervalSubscription = null;
        }

        private bool CanProcess => !PongCoreController.IsPaused && PongCoreController.IsRunning;

        private void CreateBooster()
        {
            if (!CanProcess) return;
            switch (Random.Range(0,3))
            {
                case 0: CreateExpandPaddleBooster(); break;
                case 1: CreateNarrowPaddleBooster(); break;
                case 2: CreateSpeedBallBooster(); break;
            }
        }
        
        private async void CreateExpandPaddleBooster()
        {
            var pos = GetRandomBoosterPosition();
            var booster = new ExpandPaddle(pos);
            var prefab = await AssetService.LoadGameObjectFromAddressablesAsync<ExpandPaddleView>(ResourceType.View);
            if (CanProcess) InitBooster(booster, prefab);
        }

        private async void CreateNarrowPaddleBooster()
        {
            var pos = GetRandomBoosterPosition();
            var booster = new NarrowPaddle(pos);
            var prefab = await AssetService.LoadGameObjectFromAddressablesAsync<NarrowPaddleView>(ResourceType.View);
            if (CanProcess) InitBooster(booster, prefab);
        }

        private async void CreateSpeedBallBooster()
        {
            var pos = GetRandomBoosterPosition();
            var booster = new SpeedBall(pos);
            var prefab = await AssetService.LoadGameObjectFromAddressablesAsync<SpeedBallView>(ResourceType.View);
            if (CanProcess) InitBooster(booster, prefab);
        }

        private Vector2 GetRandomBoosterPosition()
        {
            if (!CanProcess) return default;
            var core = PongCoreController.Core;
            var xRange = core.Field.Size.x;
            var yRange = core.Field.Size.y * 0.1f;
            var x = Random.Range(-xRange, xRange);
            var y = Random.Range(-yRange, yRange);
            return new Vector2(x, y);
        }

        private async void InitBooster(BaseBooster booster, BaseBoosterView prefab)
        {
            var view = Object.Instantiate(prefab).Init(booster);
            view.NetworkObject.Spawn();
            view.transform.SetParent(PongCoreController.View.Container);
            view.UpdateView();
            
            PongCoreController.Core.CustomCastables.Add(booster);
            var subscription = PongCoreController.Core.OnCustomCast
                .Where(a => a == booster)
                .Select(a=>a as BaseBooster)
                .Subscribe(b =>
                {
                    b.ApplyBooster(PongCoreController.Core);
                    if (view != null) Object.Destroy(view.gameObject);
                });

            await UniTask.Delay(TimeSpan.FromSeconds(booster.lifeTime));
            
            if (view != null) Object.Destroy(view.gameObject);
            subscription?.Dispose();
            PongCoreController.Core?.CustomCastables.Remove(booster);
        }
    }
}