using System.Collections.Generic;

namespace Utilities
{
    public sealed class StackLikeList<T> : List<T>
    {
        public void Push(T item) => Add(item);
        public T Peek() => Count > 0 ? this[Count - 1] : default;

        public T Pop()
        {
            if (Count > 0)
            {
                var result = Peek();
                RemoveAt(Count - 1);
                return result;
            }

            return default;
        }

        public bool TryPeek(out T item)
        {
            item = default;
            if (Count == 0)
                return false;
            item = Peek();
            return true;
        }

        public bool TryPop(out T item)
        {
            item = default;
            if (Count == 0)
                return false;
            item = Pop();
            return true;
        }
    }
}