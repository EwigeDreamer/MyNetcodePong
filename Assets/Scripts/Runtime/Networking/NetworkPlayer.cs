using System;
using Extensions.Strings;
using UniRx;
using Unity.Netcode;
using UnityEngine;

namespace MyPong.Networking
{
    public class NetworkPlayer : NetworkBehaviour
    {
        public readonly ReactiveProperty<int> MyScoreRx = new(0);
        public readonly ReactiveProperty<int> EnemyScoreRx = new(0);

        private Subject<float> _onPositionControl = new();
        public IObservable<float> OnPositionControl => _onPositionControl;
        
        
        [ServerRpc]
        public void ControlPositionServerRpc(float position)
        {
            _onPositionControl.OnNext(position);
        }

        [ClientRpc]
        public void SetScoreClientRpc(int myScore, int enemyScore)
        {
            MyScoreRx.Value = myScore;
            EnemyScoreRx.Value = enemyScore;
        }
        

        public override void OnNetworkSpawn()
        {
            SpawnDebug();
        }

        public override void OnNetworkDespawn()
        {
            DespawnDebug();
        }

        private void SpawnDebug()
        {
            var log = $"SPAWN!!!".Bold().Color(Color.cyan) + "\n";
            log += $"{nameof(IsOwner)}: {IsOwner}\n";
            log += $"{nameof(IsServer)}: {IsServer}\n";
            log += $"{nameof(IsClient)}: {IsClient}\n";
            log += $"{nameof(IsHost)}: {IsHost}\n";
            log += $"{nameof(IsSpawned)}: {IsSpawned}\n";
            log += $"{nameof(IsLocalPlayer)}: {IsLocalPlayer}\n";
            log += $"{nameof(IsOwnedByServer)}: {IsOwnedByServer}\n";
            Debug.Log(log, this);
        }

        private void DespawnDebug()
        {
            var log = $"DESPAWN!!!".Bold().Color(Color.yellow) + "\n";
            log += $"{nameof(IsOwner)}: {IsOwner}\n";
            log += $"{nameof(IsServer)}: {IsServer}\n";
            log += $"{nameof(IsClient)}: {IsClient}\n";
            log += $"{nameof(IsHost)}: {IsHost}\n";
            log += $"{nameof(IsSpawned)}: {IsSpawned}\n";
            log += $"{nameof(IsLocalPlayer)}: {IsLocalPlayer}\n";
            log += $"{nameof(IsOwnedByServer)}: {IsOwnedByServer}\n";
            Debug.Log(log, this);
        }
    }
}