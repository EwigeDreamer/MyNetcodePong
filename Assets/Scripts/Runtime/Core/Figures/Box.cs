using UnityEngine;

namespace MyPong.Core
{
    public class Box
    {
        public Vector2 p1, p2;
        public float r;
        
        public Box(Vector2 p1, Vector2 p2, float r)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.r = r;
        }
    }
}