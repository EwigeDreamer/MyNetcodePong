using System;
using MyPong.Core.Objects;
using UnityEngine;

namespace MyPong.View
{
    public class PaddleView : MonoBehaviour, IUpdatableView
    {
        private Paddle _paddle;

        public PaddleView Init(Paddle paddle)
        {
            _paddle = paddle;
            return this;
        }
        
        [SerializeField] private Transform _circle1; 
        [SerializeField] private Transform _circle2; 
        [SerializeField] private Transform _square; 
        public void UpdateView()
        {
            transform.localPosition = _paddle.position;
            transform.localRotation = Quaternion.identity;
            _circle1.localPosition = Vector3.left * _paddle.width / 2f;
            _circle2.localPosition = Vector3.right * _paddle.width / 2f;
            _circle1.localScale = Vector3.one * _paddle.thickness;
            _circle2.localScale = Vector3.one * _paddle.thickness;
            _square.localPosition = Vector3.zero;
            _square.localRotation = Quaternion.identity;
            _square.localScale = new Vector3(_paddle.width, _paddle.thickness, 1f);
        }
    }
}