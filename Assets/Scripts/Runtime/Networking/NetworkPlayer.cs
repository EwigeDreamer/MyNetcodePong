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

        private readonly Subject<float> _onPositionControl = new();
        private readonly Subject<Unit> _onGameOver = new();
        public IObservable<float> OnPositionControl => _onPositionControl;
        public IObservable<Unit> OnGameOver => _onGameOver;
        
        
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

        [ClientRpc]
        public void OnGameOverClientRpc()
        {
            _onGameOver.OnNext(default);
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
            var log = $"Spawn Player".Bold().Color(Color.cyan) + "\n";
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
            var log = $"Despawn Player".Bold().Color(Color.yellow) + "\n";
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