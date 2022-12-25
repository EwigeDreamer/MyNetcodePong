using System;
using System.Collections.Generic;
using Extensions.Vectors;
using MyPong.Core.Interfaces;
using MyPong.Core.Objects;
using MyPong.Core.Physics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyPong.Core
{
    public class PongCore
    {
        public readonly Field Field;
        public readonly IReadOnlyList<Paddle> Paddles;
        public readonly Ball Ball;

        public event Action<int> OnGoal;
        public event Action<Ball, Vector2> OnBallBounce;

        private readonly float StartBallSpeed; 
        private readonly float BallSpeedIncrease; 

        public PongCore(
            Vector2 fieldScale,
            float paddleWidth,
            float paddleThickness,
            float paddleMaxSpeed,
            float ballScale,
            float ballSpeed,
            float ballSpeedIncrease)
        {
            Field = new Field(fieldScale);
            Paddles = new[]
            {
                new Paddle(0, new Vector2(0f, -fieldScale.y / 2f + 1f), paddleWidth, paddleThickness, paddleMaxSpeed),
                new Paddle(1, new Vector2(0f, fieldScale.y / 2f - 1f), paddleWidth, paddleThickness, paddleMaxSpeed),
            };
            Ball = new Ball(Vector2.zero, ballScale / 2f);
            StartBallSpeed = ballSpeed;
            BallSpeedIncrease = ballSpeedIncrease;
            ResetBall();
        }

        public void Update(float deltaTime)
        {
            foreach (var paddle in Paddles)
                MovePaddle(paddle, deltaTime);
            MoveBall(deltaTime);
        }

        private void ResetBall()
        {
            Ball.position = Vector2.zero;
            Ball.direction = new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(0, 2) > 0 ? -1f : 1f)
                .normalized;
            Ball.speed = StartBallSpeed;
            Ball.speedIncrease = BallSpeedIncrease;
        }

        private void MovePaddle(Paddle paddle, float deltaTime)
        {
            var currPos = paddle.position.x;
            var trgtPos = paddle.targetPosition;

            //ускорение
            paddle.currentSpeed += Math.Sign(trgtPos - currPos) * paddle.acceleration * deltaTime;
            //замедление вблизи
            paddle.currentSpeed *= (float)Math.Pow(Math.Abs(trgtPos - currPos) / Field.Scale.x, 1f/4f);
            //ограничение
            paddle.currentSpeed = Math.Clamp(paddle.currentSpeed, -paddle.maxSpeed, paddle.maxSpeed);
            //перемещение
            currPos += paddle.currentSpeed * deltaTime;

            paddle.position = paddle.position.SetX(currPos);
        }
        
        private void MoveBall(float deltaTime)
        {
            var ball = Ball;
            var step = ball.direction * ball.speed * deltaTime;
            var left = Vector2.zero;
            
            CastBallRecursively(ball, ref step, ref left);

            foreach (var goal in Field.Goals)
            {
                var start = ball.position + left;
                var cast = new Capsule(ball.position, start + step, ball.radius);
                if (CastHelper.CircleCast(goal.Collider, cast, out var point, out var normal, out _))
                {
                    OnGoal?.Invoke(goal.Id);
                    ResetBall();
                    return;
                }
            }

            ball.position += left + step;
        }

        private void CastBallRecursively(Ball ball, ref Vector2 step, ref Vector2 left)
        {
            foreach (var wall in Field.Walls)
            {
                if (CheckCast(ball, wall, ref step, ref left, out var point))
                {
                    OnBallBounce?.Invoke(ball, point);
                    CastBallRecursively(ball, ref step, ref left);
                    return;
                }
            }
            foreach (var paddle in Paddles)
            {
                if (CheckCast(ball, paddle, ref step, ref left, out var point))
                {
                    OnBallBounce?.Invoke(ball, point);
                    ball.SpeedUp();
                    
                    //трение
                    var paddleSpeed = Vector2.right * paddle.currentSpeed;
                    var ballSpeed = ball.direction * ball.speed;
                    var resultSpeed = ballSpeed + paddleSpeed * 0.5f;
                    ball.direction = resultSpeed.normalized;
                    
                    CastBallRecursively(ball, ref step, ref left);
                    return;
                }
            }
        }

        private bool CheckCast(Ball ball, ICastable castable, ref Vector2 step, ref Vector2 left, out Vector2 point)
        {
            var start = ball.position + left;
            var cast = new Capsule(ball.position, start + step, ball.radius);
            if (castable.CircleCast(cast, out point, out var normal))
            {
                var leftTmp = point - start;
                step -= leftTmp;
                left += leftTmp;
                ball.direction = Vector2.Reflect(ball.direction, normal).normalized;
                step = ball.direction * step.magnitude;
                return true;
            }
            return false;
        }
    }

}