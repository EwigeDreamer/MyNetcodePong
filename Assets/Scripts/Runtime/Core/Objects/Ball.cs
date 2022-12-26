using System.Collections.Generic;
using MyPong.Core.Boosters;
using MyPong.Core.Interfaces;
using MyPong.Core.Physics;
using UnityEngine;

namespace MyPong.Core.Objects
{
    public class Ball
    {
        public readonly List<BaseBoosterEffect> Effects = new();

        public Vector2 position;
        public Vector2 direction;
        public float radius;
        public float speed;
        public float speedIncrease;

        public float GetSpeed()
        {
            var s = speed;
            foreach (var e in Effects)
                s = e.SpeedEffect(s);
            return s;
        }

        public Ball(Vector2 position, float radius)
        {
            this.position = position;
            this.radius = radius;
            this.direction = default;
            this.speed = default;
        }

        public void SpeedUp()
        {
            this.speed += speedIncrease;
        }
    }
}