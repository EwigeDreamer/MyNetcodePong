using System;
using UnityEngine;

namespace Utilities.Patterns
{
    public abstract class MonoSingleton<TMe> : MonoBehaviour where TMe : MonoSingleton<TMe>
    {
        static TMe _i = null;
        public static TMe I => _i;

        protected virtual void Awake()
        {
            if (_i != null) Destroy(this.gameObject);
            else _i = this as TMe;
        }

        protected virtual void OnDestroy()
        {
            if (_i == this) _i = null;
        }
    }
}