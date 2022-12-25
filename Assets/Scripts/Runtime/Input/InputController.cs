using System;
using Extensions.Vectors;
using MyPong;
using MyPong.Networking;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using NetworkPlayer = MyPong.Networking.NetworkPlayer;

namespace MyPong.Input
{
    public class InputController : IDisposable
    {
        public readonly CameraController CameraController;
        public readonly UnetWrapper UnetWrapper;

        private CompositeDisposable _disposable = new();

        public InputController(
            CameraController cameraController,
            UnetWrapper unetWrapper)
        {
            CameraController = cameraController;
            UnetWrapper = unetWrapper;

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
            StopUpdating();
            _player = player;
            _updateSubscription = Observable.EveryUpdate().Subscribe(Update);
        }
        private void StopUpdating()
        {
            _player = null;
            _updateSubscription?.Dispose();
        }

        private void Update(long _)
        {
            if (UnityEngine.Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                var sp = UnityEngine.Input.mousePosition;
                var wp = CameraController.Camera.ScreenToWorldPoint(sp);
                Debug.LogError($"POINTER! screen: {sp.ToV2_xy()}, world: {wp.ToV2_xy()}");
            }
        }
        
        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}