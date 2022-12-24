using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MyPong.Core;
using UnityEngine;
using Utilities;

namespace MyPong.View
{
    public class PongView : IUpdatableView, IDisposable
    {
        private readonly PongCore PongCore;
        private readonly Vector2 Scale;
        private readonly List<IUpdatableView> Updatables = new();
        private GameObject _container;
        
        public PongView(PongCore core)
        {
            PongCore = core;
            Scale = core.Field.Scale;
        }

        public async UniTask Init()
        {
            var fieldTask = AssetService.LoadGameObjectFromAddressablesAsync<FieldView>(ResourceType.View);
            var paddleTask = AssetService.LoadGameObjectFromAddressablesAsync<PaddleView>(ResourceType.View);
            var ballTask = AssetService.LoadGameObjectFromAddressablesAsync<BallView>(ResourceType.View);
            
            var (fieldPrefab, paddlePrefab, ballPrefab)
                = await UniTask.WhenAll(fieldTask, paddleTask, ballTask);
            
            var fieldView = UnityEngine.Object.Instantiate(fieldPrefab).Init(PongCore.Field);
            Updatables.Add(fieldView);

            foreach (var paddle in PongCore.Paddles)
            {
                var paddleView = UnityEngine.Object.Instantiate(paddlePrefab, fieldView.transform).Init(paddle);
                Updatables.Add(paddleView);
            }
            
            var ballView = UnityEngine.Object.Instantiate(ballPrefab).Init(PongCore.Ball);
            Updatables.Add(ballView);
        }

        public void UpdateView()
        {
            foreach(var a in Updatables)
                a.UpdateView();
        }

        public void Dispose()
        {
            if (_container != null)
                UnityEngine.Object.Destroy(_container);
            Updatables.Clear();
        }
    }
}