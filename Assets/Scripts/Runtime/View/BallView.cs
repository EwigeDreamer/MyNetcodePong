using System;
using MyPong.Core.Objects;
using UnityEngine;

namespace MyPong.View
{
    public class BallView : MonoBehaviour, IUpdatableView
    {
        private Ball _ball;

        public BallView Init(Ball ball)
        {
            _ball = ball;
            return this;
        }

        public void UpdateView()
        {
            transform.localPosition = _ball.position;
            transform.localScale = Vector3.one * _ball.radius;
        }
    }
}