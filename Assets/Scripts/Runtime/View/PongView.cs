using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MyPong.Core;
using UniRx.Triggers;
using UniRx;
using UnityEngine;
using Utilities;

namespace MyPong.View
{
    public class PongView : IUpdatableView, IDisposable
    {
        private readonly PongCore PongCore;
        private readonly Vector2 Scale;
        private readonly List<IUpdatableView> Updatables = new();
        private readonly List<GameObject> ToDestroy = new();
        
        public PongView(PongCore core)
        {
            PongCore = core;
            Scale = core.Field.Scale;
        }
        
        public Transform Container { get; private set; }

        public async UniTask Init()
        {
            var fieldTask = AssetService.LoadGameObjectFromAddressablesAsync<FieldView>(ResourceType.View);
            var paddleTask = AssetService.LoadGameObjectFromAddressablesAsync<PaddleView>(ResourceType.View);
            var ballTask = AssetService.LoadGameObjectFromAddressablesAsync<BallView>(ResourceType.View);
            
            var (fieldPrefab, paddlePrefab, ballPrefab)
                = await UniTask.WhenAll(fieldTask, paddleTask, ballTask);
            
            var fieldView = UnityEngine.Object.Instantiate(fieldPrefab).Init(PongCore.Field);
            fieldView.NetworkObject.Spawn();
            Updatables.Add(fieldView);
            fieldView.OnDestroyAsObservable().Subscribe(_ => Updatables.Remove(fieldView));
            ToDestroy.Add(fieldView.gameObject);
            Container = fieldView.transform;
            
            foreach (var paddle in PongCore.Paddles)
            {
                var paddleView = UnityEngine.Object.Instantiate(paddlePrefab).Init(paddle);
                paddleView.NetworkObject.Spawn();
                paddleView.transform.SetParent(fieldView.transform);
                Updatables.Add(paddleView);
                paddleView.OnDestroyAsObservable().Subscribe(_ => Updatables.Remove(paddleView));
                ToDestroy.Add(paddleView.gameObject);
            }
            
            var ballView = UnityEngine.Object.Instantiate(ballPrefab).Init(PongCore.Ball);
            ballView.NetworkObject.Spawn();
            ballView.transform.SetParent(fieldView.transform);
            Updatables.Add(ballView);
            ballView.OnDestroyAsObservable().Subscribe(_ => Updatables.Remove(ballView));
            ToDestroy.Add(ballView.gameObject);

            PongCore.OnBallReset.Subscribe(d => ballView.TeleportClientRpc(d.point));
            PongCore.OnBallBounce.Subscribe(d => ballView.TeleportClientRpc(d.point));
            
            UpdateView();
        }

        public void UpdateView()
        {
            foreach(var a in Updatables)
                a.UpdateView();
        }

        public void Dispose()
        {
            foreach(var obj in ToDestroy)
                UnityEngine.Object.Destroy(obj);
            ToDestroy.Clear();
            Updatables.Clear();
            Container = null;
        }
    }
}