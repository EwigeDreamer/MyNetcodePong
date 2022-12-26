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
    }
}