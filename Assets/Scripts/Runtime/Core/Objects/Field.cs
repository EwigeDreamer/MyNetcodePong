using System.Collections.Generic;
using UnityEngine;

namespace MyPong.Core.Objects
{
    public class Field
    {
        public readonly Vector2 Scale;
        public readonly Vector2 Size;
        public readonly IReadOnlyList<Wall> Walls;
        public readonly IReadOnlyList<Goal> Goals;

        public Field(Vector2 scale)
        {
            Scale = scale;
            Size = scale * 0.5f;
            var corners = new[]
            {
                //12
                //03
                new Vector2(-Size.x, -Size.y),
                new Vector2(-Size.x, Size.y),
                new Vector2(Size.x, Size.y),
                new Vector2(Size.x, -Size.y),
            };
            Walls = new[]
            {
                new Wall(corners[0], corners[1]),
                new Wall(corners[2], corners[3]),
            };
            Goals = new[]
            {
                new Goal(corners[1], corners[2], 0),
                new Goal(corners[3], corners[0], 1),
            };
        }
    }
}