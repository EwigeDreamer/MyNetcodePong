using System;
using MyPong.Core.Objects;
using Unity.Netcode;
using UnityEngine;

namespace MyPong.View
{
    public class PaddleView : NetworkBehaviour, IUpdatableView
    {
        [SerializeField] private Transform _circle1; 
        [SerializeField] private Transform _circle2; 
        [SerializeField] private Transform _square; 
        
        private Paddle _paddle;

        public PaddleView Init(Paddle paddle)
        {
            _paddle = paddle;
            return this;
        }
        
        public void UpdateView()
        {
            var width = _paddle.GetWidth();
            
            transform.localPosition = _paddle.position;
            transform.localRotation = Quaternion.identity;
            _circle1.localPosition = Vector3.left * width / 2f;
            _circle2.localPosition = Vector3.right * width / 2f;
            _circle1.localScale = Vector3.one * _paddle.thickness;
            _circle2.localScale = Vector3.one * _paddle.thickness;
            _square.localPosition = Vector3.zero;
            _square.localRotation = Quaternion.identity;
            _square.localScale = new Vector3(width, _paddle.thickness, 1f);
        }
    }
}