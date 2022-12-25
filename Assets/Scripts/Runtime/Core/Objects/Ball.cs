using MyPong.Core.Interfaces;
using MyPong.Core.Physics;
using UnityEngine;

namespace MyPong.Core.Objects
{
    public class Ball
    {
        public Vector2 position;
        public Vector2 direction;
        public float radius;
        public float speed;
        public float speedIncrease;

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