using MyPong.Core.Interfaces;
using MyPong.Core.Physics;
using UnityEngine;

namespace MyPong.Core.Boosters
{
    public class BaseBooster : ICastable
    {
        public Vector2 position;
        public float radius;

        public BaseBooster(Vector2 position, float radius)
        {
            this.position = position;
            this.radius = radius;
        }

        public Circle Collider => new(position, radius);
        
        public bool LineCast(Vector2 start, Vector2 end, out Vector2 point, out Vector2 normal)
        {
            return CastHelper.LineCast(Collider, start, end, out point, out normal, out _);
        }

        public bool CircleCast(Vector2 start, Vector2 end, float r, out Vector2 point, out Vector2 normal)
        {
            return CastHelper.CircleCast(Collider, start, end, r, out point, out normal, out _);
        }

        public bool CircleCast(Capsule cast, out Vector2 point, out Vector2 normal)
        {
            return CastHelper.CircleCast(Collider, cast, out point, out normal, out _);
        }
    }
}