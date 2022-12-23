using System.Collections.Generic;
using Extensions.Vectors;
using UnityEngine;

namespace MyPong.Core
{
    public class PongCore
    {
        public IReadOnlyList<Paddle> Paddles = new[]
        {
            new Paddle(),
            new Paddle(),
        };

        public void Update()
        {
            
            Bounds b = default;
            // b.ClosestPoint()
        }

        private void FlyBall()
        {
            
        }
    }

    public class Paddle
    {
        public Vector2 Position { get; set; }
    }

    public class Ball
    {
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public float Speed { get; set; }
    }

    public class Field
    {
        public readonly Vector2 Scale;
        public readonly Vector2 Size;
        public IReadOnlyList<Vector2> Corners;
        public IReadOnlyList<Wall> Walls;

        public Field(Vector2 scale)
        {
            Scale = scale;
            Size = scale * 0.5f;
            Corners = new[]
            {
                new Vector2(-Size.x, -Size.y),
                new Vector2(-Size.x, Size.y),
                new Vector2(Size.x, Size.y),
                new Vector2(Size.x, -Size.y),
            };
            Walls = new[]
            {
                new Wall(Corners[0], Corners[1]),
                new Wall(Corners[1], Corners[2]),
                new Wall(Corners[2], Corners[3]),
                new Wall(Corners[3], Corners[0]),
            };
        }
    }

    public struct Wall
    {
        private Vector2 p1, p2;
        public Wall(Vector2 p1, Vector2 p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }
        // public bool Intersect(Vector2 v1, Vector2 v2, out )
    }
}