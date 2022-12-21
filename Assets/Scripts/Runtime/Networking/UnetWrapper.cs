using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using Utilities.Network;

namespace MyPong
{
    public class UnetWrapper
    {
        public readonly NetworkManager NetworkManager;

        public UnityTransport Transport => NetworkManager.NetworkConfig.NetworkTransport as UnityTransport;
        public bool IsRunning => NetworkManager.IsServer || NetworkManager.IsClient;

        public UnetWrapper(NetworkManager networkManager)
        {
            NetworkManager = networkManager;
        }

        public bool StartClient(string ip, string portStr)
        {
            if (!NetworkUtility.IsValidIPv4(ip)) return false;
            if (!NetworkUtility.IsValidPort(portStr)) return false;

            var port = ushort.Parse(portStr);

            Transport.SetConnectionData(ip, port);
            return NetworkManager.StartClient();
        }

        public bool StartHost(string portStr)
        {
            if (!NetworkUtility.IsValidPort(portStr)) return false;

            var ip = NetworkUtility.GetLocalIPv4();
            var port = ushort.Parse(portStr);

            Transport.SetConnectionData(ip, port);
            NetworkManager.NetworkConfig.ConnectionApproval = true;
            NetworkManager.ConnectionApprovalCallback = ApprovalCheck;
            return NetworkManager.StartHost();
        }

        private void ApprovalCheck(
            NetworkManager.ConnectionApprovalRequest request,
            NetworkManager.ConnectionApprovalResponse response)
        {
            Debug.LogError($"CHECK CONNECTION {NetworkManager.ConnectedClientsIds.Count}");
            
            
            if (NetworkManager.ConnectedClientsIds.Count < 1 || request.ClientNetworkId == NetworkManager.LocalClientId)
            {
                response.Approved = true;
                response.CreatePlayerObject = true;
                response.PlayerPrefabHash = null;
                response.Position = null;
                response.Rotation = null;
                response.Pending = false;
                response.Reason = null;
            }
            else
            {
                response.Approved = false;
                response.CreatePlayerObject = false;
                response.PlayerPrefabHash = null;
                response.Position = null;
                response.Rotation = null;
                response.Pending = false;
                response.Reason = "Server is full";
            }
        }
    }
}