using System;
using MyPong.Core.Objects;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace MyPong.View
{
    [RequireComponent(typeof(NetworkTransform))]
    public class BallView : NetworkBehaviour, IUpdatableView
    {
        [SerializeField] private Transform _circle;
        
        private Ball _ball;

        private void Awake()
        {
            throw new NotImplementedException();
        }

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

        [ClientRpc]
        public void TeleportClientRpc(Vector2 position)
        {
            GetComponent<NetworkTransform>().Teleport(
                position,
                Quaternion.identity,
                transform.localScale);
        }
    }
}