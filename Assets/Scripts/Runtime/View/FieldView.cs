using System;
using Extensions.Vectors;
using MyPong.Core.Objects;
using UnityEngine;

namespace MyPong.View
{
    public class FieldView : MonoBehaviour, IUpdatableView
    {
        private Field _field;

        public FieldView Init(Field field)
        {
            _field = field;
            return this;
        }
        
        [SerializeField] private SpriteRenderer _box;
        
        public void UpdateView()
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = _field.Scale.ToV3_xy0().SetZ(1f);
        }
    }
}