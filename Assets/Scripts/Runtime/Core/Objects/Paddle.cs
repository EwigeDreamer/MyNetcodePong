using System.Collections.Generic;
using Extensions.Vectors;
using MyPong.Core.Boosters;
using MyPong.Core.Interfaces;
using MyPong.Core.Physics;
using UnityEngine;

namespace MyPong.Core.Objects
{
    public class Paddle : ICastable
    {
        public readonly List<BaseBoosterEffect> Effects = new();
        
        public readonly int id;
        public Vector2 position;
        public float width;
        public float thickness;

        public float targetPosition;
        public float currentSpeed;
        public float targetSpeed;
        public float maxSpeed;
        public float acceleration;

        public float GetWidth()
        {
            var w = width;
            foreach (var e in Effects)
                w = e.WidthEffect(w);
            return w;
        }

        public Capsule Collider
        {
            get
            {
                var hw = GetWidth() / 2f;
                return new(
                    position + Vector2.left * hw,
                    position + Vector2.right * hw,
                    thickness / 2f);
            }
        }

        public Paddle(
            int id,
            Vector2 position,
            float width,
            float thickness,
            float maxSpeed,
            float acceleration)
        {
            this.id = id;
            this.position = position;
            this.width = width;
            this.thickness = thickness;
            this.currentSpeed = default;
            this.targetSpeed = default;
            this.maxSpeed = maxSpeed;
            this.acceleration = acceleration;
        }

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