using System;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using Utilities.Patterns;

namespace MyPong.Networking
{
    public class SpawnEventService : MonoSingleton<SpawnEventService>
    {
        private readonly Subject<NetworkObject> _onSpawnNetworkObject = new();
        private readonly Subject<NetworkObject> _onDespawnNetworkObject = new();
        
        public IObservable<NetworkObject> OnSpawnNetworkObject => _onSpawnNetworkObject;
        public IObservable<NetworkObject> OnDespawnNetworkObject => _onDespawnNetworkObject;
        
        public void CallSpawnNetworkObject(NetworkObject obj) => _onSpawnNetworkObject.OnNext(obj);
        public void CallDespawnNetworkObject(NetworkObject obj) => _onDespawnNetworkObject.OnNext(obj);
    }
}