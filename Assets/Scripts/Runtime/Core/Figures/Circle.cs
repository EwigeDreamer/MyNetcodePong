using UnityEngine;

namespace MyPong.Core
{
    public struct Circle
    {
        public Vector2 p;
        public float r;
        
        public Circle(Vector2 p, float r)
        {
            this.p = p;
            this.r = r;
        }
    }
}