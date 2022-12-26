namespace MyPong.Core.Boosters
{
    public class BallSpeedEffect : BaseBoosterEffect
    {
        public readonly float Factor;

        public BallSpeedEffect(float factor)
        {
            Factor = factor;
        }

        public override float SpeedEffect(float speed)
        {
            return speed * Factor;
        }
    }
}