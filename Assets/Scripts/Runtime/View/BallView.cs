using System;
using MyPong.Core.Objects;
using Unity.Netcode;
using UnityEngine;

namespace MyPong.View
{
    public class BallView : NetworkBehaviour, IUpdatableView
    {
        [SerializeField] private Transform _circle;
        
        private Ball _ball;

        public BallView Init(Ball ball)
        {
            _ball = ball;
            return this;
        }

        public void UpdateView()
        {
            transform.localPosition = _ball.position;
            _circle.localScale = Vector3.one * _ball.radius * 2f;
        }
    }
}