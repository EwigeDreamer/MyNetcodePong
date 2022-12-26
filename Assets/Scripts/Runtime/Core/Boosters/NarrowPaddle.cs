using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MyPong.Core.Boosters
{
    public class NarrowPaddle : BaseBooster
    {
        public readonly float NarrowFactor;
        
        public NarrowPaddle(
            Vector2 position,
            float radius = 1f,
            float lifeTime = 10f,
            float duration = 10f,
            float narrowFactor = 1.5f)
            : base(position, radius, lifeTime, duration)
        {
            NarrowFactor = narrowFactor;
        }

        public override BaseBoosterEffect GetEffect()
        {
            return new PaddleWidthEffect(1f / NarrowFactor);
        }

        public override async void ApplyBooster(PongCore core)
        {
            if (core == null) return;
            var id = core.Ball.id;
            var effect = GetEffect();
            var paddle = core.Paddles.First(a => a.id != id);

            RemoveOverlapEffects(paddle.Effects, effect);

            paddle.Effects.Add(effect);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            paddle.Effects.Remove(effect);
        }
    }
}