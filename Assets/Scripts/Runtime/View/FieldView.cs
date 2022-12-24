using System;
using Extensions.Vectors;
using MyPong.Core.Objects;
using Unity.Netcode;
using UnityEngine;

namespace MyPong.View
{
    public class FieldView : NetworkBehaviour, IUpdatableView
    {
        [SerializeField] private Transform _square;
        
        private Field _field;

        public FieldView Init(Field field)
        {
            _field = field;
            return this;
        }
        
        public void UpdateView()
        {
            transform.localPosition = Vector3.zero;
            _square.localScale = _field.Scale.ToV3_xy0().SetZ(1f);
        }
    }
}