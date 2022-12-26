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
    }
}