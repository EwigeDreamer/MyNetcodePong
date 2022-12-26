using MyPong.Core.Boosters;
using Unity.Netcode;
using UnityEngine;

namespace MyPong.View
{
    public abstract class BaseBoosterView : NetworkBehaviour, IUpdatableView
    {
        [SerializeField] protected Transform _icon;

        private BaseBooster _booster;

        public BaseBoosterView Init(BaseBooster booster)
        {
            _booster = booster;
            return this;
        }
        
        public virtual void UpdateView()
        {
            transform.localPosition = _booster.position;
            _icon.localScale = Vector3.one * _booster.radius * 2f;
        }
    }
}