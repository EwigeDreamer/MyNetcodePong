using System;
using Extensions.Vectors;
using MyPong.Core.Objects;
using UniRx;
using Unity.Netcode;
using UnityEngine;

namespace MyPong.View
{
    public class FieldView : NetworkBehaviour, IUpdatableView
    {
        private Subject<(FieldView field, Vector2 scale)> _onChangeScale = new();
        public IObservable<(FieldView field, Vector2 scale)> OnChangeScale => _onChangeScale;

        [SerializeField] private Transform _square;
        
        private Field _field;
        
        private Vector2 _fieldScale = Vector2.zero;

        public FieldView Init(Field field)
        {
            _field = field;
            return this;
        }
        
        public void UpdateView()
        {
            transform.localPosition = Vector3.zero;
            _square.localScale = _field.Scale.ToV3_xy0().SetZ(1f);
            if (_field.Scale != _fieldScale)
            {
                _fieldScale = _field.Scale;
                CallOnChangeScaleClientRpc(_field.Scale);
            }
        }

        [ClientRpc]
        private void CallOnChangeScaleClientRpc(Vector2 fieldScale)
        {
            _onChangeScale.OnNext((this, fieldScale));
        }
    }
}