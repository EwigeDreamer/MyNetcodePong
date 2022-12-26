using System;
using System.Collections.Generic;
using Extensions.Vectors;
using MyPong.Core.Interfaces;
using MyPong.Core.Objects;
using MyPong.Core.Physics;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyPong.Core
{
    public class PongCore
    {
        public readonly Field Field;
        public readonly IReadOnlyList<Paddle> Paddles;
        public readonly Ball Ball;

        public readonly List<ICastable> CustomCastables = new();

        private readonly Subject<int> _onGoal = new();
        private readonly Subject<(Ball ball, Vector2 point)> _onBallBounce = new();
        private readonly Subject<ICastable> _onCustomCast = new();

        public IObservable<int> OnGoal => _onGoal;
        public IObservable<(Ball ball, Vector2 point)> OnBallBounce => _onBallBounce;
        public IObservable<ICastable> OnCustomCast => _onCustomCast;

        private readonly float StartBallSpeed;
        private readonly float BallSpeedIncrease;

        public PongCore(
            Vector2 fieldScale,
            float paddleWidth,
            float paddleThickness,
            float paddleMaxSpeed,
            float paddleAcceleration,
            float ballScale,
            float ballSpeed,
            float ballSpeedIncrease)
        {
            Field = new Field(fieldScale);
            Paddles = new[]
            {
                new Paddle(
                    0,
                    new Vector2(0f, -fieldScale.y / 2f + 1f),
                    paddleWidth,
                    paddleThickness,
                    paddleMaxSpeed,
                    paddleAcceleration),
                new Paddle(1,
                    new Vector2(0f, fieldScale.y / 2f - 1f),
                    paddleWidth,
                    paddleThickness,
                    paddleMaxSpeed,
                    paddleAcceleration),
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
            var currentPos = paddle.position.x;
            var targetPos = paddle.targetPosition;

            //ускорение
            paddle.currentSpeed += Math.Sign(targetPos - currentPos) * paddle.acceleration * deltaTime;
            //замедление вблизи
            paddle.currentSpeed *= DragFunction((targetPos - currentPos) / Field.Scale.x);
            //ограничение
            paddle.currentSpeed = Math.Clamp(paddle.currentSpeed, -paddle.maxSpeed, paddle.maxSpeed);
            //перемещение
            currentPos += paddle.currentSpeed * deltaTime;

            var range = Field.Scale.x * 0.5f - paddle.GetWidth() * 0.5f - paddle.thickness * 0.5f;
            if (Math.Abs(currentPos) > range)
            {
                paddle.currentSpeed = 0f;
                currentPos = Math.Clamp(currentPos, -range, range);
            }
            paddle.position = paddle.position.SetX(currentPos);
        }

        private float DragFunction(float x)
        {
            x = Math.Abs(x);
            var threshold = 0.25f;
            var factor = 1f / 0.5f;
            if (x < threshold) return (float)Math.Pow(x * factor, 1f / 4f);//ветвь параболы
            return 1f;
        }
        
        private void MoveBall(float deltaTime)
        {
            var ball = Ball;
            var step = ball.direction * ball.GetSpeed() * deltaTime;
            var left = Vector2.zero;
            
            CastBallRecursively(ball, ref step, ref left);

            //custom objects, e.g. boosters
            for (int i = CustomCastables.Count - 1; i >= 0; --i)
            {
                var castable = CustomCastables[i];
                if (castable == null)
                    CustomCastables.RemoveAt(i);
                else
                {
                    var start = ball.position + left;
                    var cast = new Capsule(ball.position, start + step, ball.radius);
                    if (castable.CircleCast(cast, out _, out _))
                    {
                        _onCustomCast.OnNext(castable);
                    }
                }
            }

            foreach (var goal in Field.Goals)
            {
                var start = ball.position + left;
                var cast = new Capsule(ball.position, start + step, ball.radius);
                if (CastHelper.CircleCast(goal.Collider, cast, out var point, out var normal, out _))
                {
                    _onGoal.OnNext(goal.Id);
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
                    _onBallBounce.OnNext((ball, point));
                    CastBallRecursively(ball, ref step, ref left);
                    return;
                }
            }
            foreach (var paddle in Paddles)
            {
                if (CheckCast(ball, paddle, ref step, ref left, out var point))
                {
                    _onBallBounce.OnNext((ball, point));
                    ball.SpeedUp();
                    ball.id = paddle.id;
                    
                    //трение
                    var paddleSpeed = Vector2.right * paddle.currentSpeed;
                    var ballSpeed = ball.direction * ball.GetSpeed();
                    var resultSpeed = ballSpeed + paddleSpeed;
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