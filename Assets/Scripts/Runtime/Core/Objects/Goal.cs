using UnityEngine;

namespace MyPong.Core.Objects
{
    public class Goal : Wall
    {
        public readonly int Id;
        public Goal(Vector2 p1, Vector2 p2, int id) : base(p1, p2)
        {
            Id = id;
        }
    }
}