using Unity.Netcode;

namespace MyPong.Networking
{
    public class SpawnEventConponent : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            SpawnEventService.I?.CallSpawnNetworkObject(NetworkObject);
        }

        public override void OnNetworkDespawn()
        {
            SpawnEventService.I?.CallDespawnNetworkObject(NetworkObject);
        }
    }
}