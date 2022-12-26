namespace MyPong.Core.Boosters
{
    public class PaddleWidthEffect : BaseBoosterEffect
    {
        public readonly float Factor;

        public PaddleWidthEffect(float factor)
        {
            Factor = factor;
        }
        
        public override float WidthEffect(float width)
        {
            return width * Factor;
        }
    }
}