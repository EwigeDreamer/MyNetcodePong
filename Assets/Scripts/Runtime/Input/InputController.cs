using System;
using Extensions.Strings;
using Extensions.Vectors;
using MyPong;
using MyPong.Core;
using MyPong.Networking;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using NetworkPlayer = MyPong.Networking.NetworkPlayer;

namespace MyPong.Input
{
    [UnityEngine.Scripting.Preserve]
    public class InputController : IDisposable
    {
        private readonly CameraController CameraController;
        private readonly UnetWrapper UnetWrapper;
        private readonly PongCoreController PongCoreController;

        private CompositeDisposable _disposable = new();

        [UnityEngine.Scripting.Preserve]
        public InputController(
            CameraController cameraController,
            UnetWrapper unetWrapper,
            PongCoreController pongCoreController)
        {
            CameraController = cameraController;
            UnetWrapper = unetWrapper;
            PongCoreController = pongCoreController;
        }

        public void Start()
        {
            UnetWrapper.SpawnEventService.OnSpawnNetworkObject
                .Select(a=>a.GetComponent<NetworkPlayer>())
                .Where(a=>a!= null)
                .Where(a=>UnetWrapper.ItsMe(a.OwnerClientId))
                .Subscribe(StartUpdating)
                .AddTo(_disposable);
            UnetWrapper.SpawnEventService.OnDespawnNetworkObject
                .Select(a=>a.GetComponent<NetworkPlayer>())
                .Where(a=>a!= null)
                .Where(a=>UnetWrapper.ItsMe(a.OwnerClientId))
                .Subscribe(_ => StopUpdating())
                .AddTo(_disposable);
        }

        private NetworkPlayer _player;
        private IDisposable _updateSubscription = null;
        private void StartUpdating(NetworkPlayer player)
        {
            _player = player;
            _updateSubscription?.Dispose();
            _updateSubscription = Observable.EveryUpdate().Subscribe(Update);
            Debug.Log("Start Input Updating!");
        }
        private void StopUpdating()
        {
            _player = null;
            _updateSubscription?.Dispose();
            Debug.Log("Stop Input Updating!");
        }

        private void Update(long _)
        {
            if (UnityEngine.Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                var sp = UnityEngine.Input.mousePosition;
                var wp = CameraController.Camera.ScreenToWorldPoint(sp);
                _player.ControlPositionServerRpc(wp.x);
            }
        }
        
        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}