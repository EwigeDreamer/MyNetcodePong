using Sirenix.OdinInspector;
using UnityEngine;

namespace MyPong.Tools
{
    public class PlayerEditorTool : MonoBehaviour
    {
        [SerializeField] private NetworkPlayer _networkPlayer;

        private void OnValidate()
        {
            if (_networkPlayer == null) _networkPlayer = GetComponent<NetworkPlayer>();
        }

        [Button]
        private void CallServer()
        {
            _networkPlayer.RequestServerRpc();
        }

        [Button]
        private void CallClient()
        {
            _networkPlayer.RequestClientRpc();
        }
    }
}