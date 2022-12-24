using UnityEngine;

namespace MyPong.Core.Interfaces
{
    public interface ICastable
    {
        bool LineCast(Vector2 start, Vector2 end, out Vector2 point, out Vector2 normal);
        bool CircleCast(Vector2 start, Vector2 end, float r, out Vector2 point, out Vector2 normal);
        bool CircleCast(Capsule cast, out Vector2 point, out Vector2 normal);
    }
}