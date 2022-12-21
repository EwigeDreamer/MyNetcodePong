using System;
using UniRx;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using Utilities.Network;

namespace MyPong
{
    public class UnetWrapper
    {
        public readonly NetworkManager NetworkManager;

        private Subject<Unit> _onConnect = new();
        private Subject<Unit> _onDisconnect = new();

        public UnityTransport Transport => NetworkManager.NetworkConfig.NetworkTransport as UnityTransport;
        public bool IsRunning => NetworkManager.IsServer || NetworkManager.IsClient;

        public UnetWrapper(NetworkManager networkManager)
        {
            NetworkManager = networkManager;
            NetworkManager.OnServerStarted += () => Debug.LogError(nameof(NetworkManager.OnServerStarted));
            NetworkManager.OnTransportFailure += () => Debug.LogError(nameof(NetworkManager.OnTransportFailure));
            NetworkManager.OnClientConnectedCallback += id => Debug.LogError($"{nameof(NetworkManager.OnClientConnectedCallback)}: {id} {id == NetworkManager.LocalClientId}");
            NetworkManager.OnClientDisconnectCallback += id => Debug.LogError($"{nameof(NetworkManager.OnClientDisconnectCallback)}: {id} {id == NetworkManager.LocalClientId}");

            NetworkManager.OnClientConnectedCallback += id => _onConnect.OnNext(default);
            // NetworkManager.OnClientDisconnectCallback += id => 
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
            NetworkManager.ConnectionApprovalCallback = ApprovalCheck;
            return NetworkManager.StartHost();
        }

        private void ApprovalCheck(
            NetworkManager.ConnectionApprovalRequest request,
            NetworkManager.ConnectionApprovalResponse response)
        {
            Debug.LogError($"CHECK CONNECTION {NetworkManager.ConnectedClientsIds.Count}");
            
            
            if (NetworkManager.ConnectedClientsIds.Count < 2 || request.ClientNetworkId == NetworkManager.LocalClientId)
            {
                response.Approved = true;
                response.CreatePlayerObject = true;
            }
            else
            {
                response.Approved = false;
                response.Reason = "Server is full";
            }
        }
    }
}