using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MyPong.Core.Boosters
{
    public class ExpandPaddle : BaseBooster
    {
        public readonly float ExpandFactor;
        
        public ExpandPaddle(
            Vector2 position,
            float radius = 1f,
            float lifeTime = 10f,
            float duration = 10f,
            float expandFactor = 1.5f)
            : base(position, radius, lifeTime, duration)
        {
            ExpandFactor = expandFactor;
        }

        public override BaseBoosterEffect GetEffect()
        {
            return new PaddleWidthEffect(ExpandFactor);
        }

        public override async void ApplyBooster(PongCore core)
        {
            if (core == null) return;
            var id = core.Ball.id;
            var effect = GetEffect();
            var paddle = core.Paddles.First(a => a.id == id);

            RemoveOverlapEffects(paddle.Effects, effect);

            paddle.Effects.Add(effect);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            paddle.Effects.Remove(effect);
        }
    }
}