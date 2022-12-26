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
    }
}