namespace MyPong.Core.Boosters
{
    public abstract class BaseBoosterEffect
    {
        public virtual float SpeedEffect(float speed) => speed;
        public virtual float WidthEffect(float width) => width;
    }
}