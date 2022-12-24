using MyPong.Core.Interfaces;
using MyPong.Core.Physics;
using UnityEngine;

namespace MyPong.Core.Objects
{
    public class Wall : ICastable
    {
        public readonly Vector2 p1;
        public readonly Vector2 p2;
        
        public Wall(Vector2 p1, Vector2 p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        public Line Collider => new(p1, p2);

        public bool LineCast(Vector2 start, Vector2 end, out Vector2 point, out Vector2 normal)
        {
            return CastHelper.LineCast(Collider, start, end, out point, out normal);
        }

        public bool CircleCast(Vector2 start, Vector2 end, float r, out Vector2 point, out Vector2 normal)
        {
            return CastHelper.CircleCast(Collider, start, end, r, out point, out normal);
        }

        public bool CircleCast(Capsule cast, out Vector2 point, out Vector2 normal)
        {
            return CastHelper.CircleCast(Collider, cast, out point, out normal);
        }
    }
}