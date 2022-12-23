using UnityEngine;

namespace MyPong.Core
{
    public struct Capsule
    {
        public Vector2 p1, p2;
        public float r;
        
        public Capsule(Vector2 p1, Vector2 p2, float r)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.r = r;
        }
    }
}