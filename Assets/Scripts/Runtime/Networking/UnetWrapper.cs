using System;
using DG.Tweening;
using MyPong.UI;
using UniRx;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using Utilities.DOTween;
using Utilities.Network;

namespace MyPong
{
    public class UnetWrapper
    {
        public readonly NetworkManager NetworkManager;
        public readonly ScreenLocker ScreenLocker;

        private Subject<Unit> _onConnected = new();
        private Subject<Unit> _onDisconnect = new();
        public IObservable<Unit> OnConnected => _onConnected;
        public IObservable<Unit> OnDisconnect => _onDisconnect;

        public UnityTransport Transport => NetworkManager.NetworkConfig.NetworkTransport as UnityTransport;
        public bool IsServer => NetworkManager.IsServer;
        public bool IsClient => NetworkManager.IsClient;
        public bool IsRunning => IsServer || IsClient;
        public bool ItsMe(ulong id) => id == NetworkManager.LocalClientId;

        public UnetWrapper(
            NetworkManager networkManager,
            ScreenLocker screenLocker)
        {
            NetworkManager = networkManager;
            ScreenLocker = screenLocker;

            NetworkManager.OnServerStarted += () => Debug.LogError(nameof(NetworkManager.OnServerStarted));
            NetworkManager.OnTransportFailure += () => Debug.LogError(nameof(NetworkManager.OnTransportFailure));
            NetworkManager.OnClientConnectedCallback += id => Debug.LogError($"{nameof(NetworkManager.OnClientConnectedCallback)}: {id} {id == NetworkManager.LocalClientId}");
            NetworkManager.OnClientDisconnectCallback += id => Debug.LogError($"{nameof(NetworkManager.OnClientDisconnectCallback)}: {id} {id == NetworkManager.LocalClientId}");

            NetworkManager.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.OnClientDisconnectCallback += OnClientDisconnect;
        }


        private Tween _connectWaiter = null;
        private void StartConnectWaiter()
        {
            var connectTimeout = Transport.ConnectTimeoutMS * Transport.MaxConnectAttempts / 1000f;
            if (_connectWaiter != null)
                _connectWaiter.Kill(true);
            _connectWaiter = DOTweenUtility.Delay(connectTimeout + 1f)
                .OnStart(() => ScreenLocker.Show(ScreenLockTypes.Unet))
                .OnComplete(() => ScreenLocker.Hide(ScreenLockTypes.Unet))
                .Play();
        }
        private void StopConnectWaiter()
        {
            if (_connectWaiter != null)
                _connectWaiter.Kill(true);
            _connectWaiter = null;
        }

        private void OnClientConnected(ulong id)
        {
            StopConnectWaiter();
            _onConnected.OnNext(default);
        }

        private void OnClientDisconnect(ulong id)
        {
            if (!IsServer || ItsMe(id))
            {
                // NetworkManager.Shutdown();
                _onDisconnect.OnNext(default);
            }
        }
        
        public bool StartClient(string ip, string portStr)
        {
            if (!NetworkUtility.IsValidIPv4(ip)) return false;
            if (!NetworkUtility.IsValidPort(portStr)) return false;

            var port = ushort.Parse(portStr);

            Transport.SetConnectionData(ip, port);
            StartConnectWaiter();

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