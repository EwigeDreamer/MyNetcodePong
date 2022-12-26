using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MyPong.Core.Boosters
{
    public class SpeedBall : BaseBooster
    {
        public readonly float SpeedFactor;
        
        public SpeedBall(
            Vector2 position,
            float radius = 1f,
            float lifeTime = 10f,
            float duration = 10f,
            float speedFactor = 2f)
            : base(position, radius, lifeTime, duration)
        {
            SpeedFactor = speedFactor;
        }

        public override BaseBoosterEffect GetEffect()
        {
            return new BallSpeedEffect(SpeedFactor);
        }

        public override async void ApplyBooster(PongCore core)
        {
            if (core == null) return;
            var effect = GetEffect();
            var ball = core.Ball;

            RemoveOverlapEffects(ball.Effects, effect);

            ball.Effects.Add(effect);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            ball.Effects.Remove(effect);
        }
    }
}