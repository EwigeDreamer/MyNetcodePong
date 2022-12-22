using Extensions.Strings;
using Unity.Netcode;
using UnityEngine;

namespace MyPong
{
    public class NetworkPlayer : NetworkBehaviour
    {
        [SerializeField] private NetworkVariable<Color> _networkColor = new();

        public override void OnNetworkSpawn()
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

        public override void OnNetworkDespawn()
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

        [ServerRpc]
        public void RequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Debug.LogWarning("SERVER RPC!".Bold().Color(Color.magenta));
        }

        [ClientRpc]
        public void RequestClientRpc(ClientRpcParams rpcParams = default)
        {
            Debug.LogWarning("CLIENT RPC!".Bold().Color(Color.green));
        }
    }
}