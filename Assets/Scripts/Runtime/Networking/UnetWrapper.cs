using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Extensions.Strings;
using MyPong.UI;
using MyPong.UI.Popups;
using UniRx;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using Utilities.DOTween;
using Utilities.Network;

namespace MyPong.Networking
{
    [UnityEngine.Scripting.Preserve]
    public class UnetWrapper
    {
        public readonly NetworkManager NetworkManager;
        public readonly SpawnEventService SpawnEventService;
        public readonly ScreenLocker ScreenLocker;

        private Subject<ulong> _onConnectedToServer = new();
        private Subject<ulong> _onDisconnectFromServer = new();
        private Subject<Unit> _onShutdown = new();
        private Subject<Unit> _onStartHost = new();
        private Subject<Unit> _onStartClient = new();
        private Subject<bool> _onPlayersEnough = new();
        public IObservable<ulong> OnConnectedToServer => _onConnectedToServer;
        public IObservable<ulong> OnDisconnectFromServer => _onDisconnectFromServer;
        public IObservable<Unit> OnShutdown => _onShutdown;
        public IObservable<Unit> OnStartHost => _onStartHost;
        public IObservable<Unit> OnStartClient => _onStartClient;
        public IObservable<bool> OnPlayersEnough => _onPlayersEnough;

        public UnityTransport Transport => NetworkManager.NetworkConfig.NetworkTransport as UnityTransport;
        public bool IsServer => NetworkManager.IsServer;
        public bool IsClient => NetworkManager.IsClient;
        public bool IsRunning => IsServer || IsClient;
        public bool ItsMe(ulong id) => id == NetworkManager.LocalClientId;

        private readonly List<ulong> _connectedClients = new();

        [UnityEngine.Scripting.Preserve]
        public UnetWrapper(
            NetworkManager networkManager,
            SpawnEventService spawnEventService,
            ScreenLocker screenLocker)
        {
            NetworkManager = networkManager;
            SpawnEventService = spawnEventService;
            ScreenLocker = screenLocker;

            NetworkManager.OnServerStarted += () => Debug.Log(nameof(NetworkManager.OnServerStarted).Bold());
            NetworkManager.OnTransportFailure += () => Debug.LogWarning(nameof(NetworkManager.OnTransportFailure).Bold());
            NetworkManager.OnClientConnectedCallback += id => Debug.Log($"{nameof(NetworkManager.OnClientConnectedCallback).Bold()}: id: {id.ToString().Bold()}, it's me: {id == NetworkManager.LocalClientId}");
            NetworkManager.OnClientDisconnectCallback += id => Debug.Log($"{nameof(NetworkManager.OnClientDisconnectCallback).Bold()}: id: {id.ToString().Bold()}, it's me: {id == NetworkManager.LocalClientId}");

            NetworkManager.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.OnClientDisconnectCallback += OnClientDisconnect;
        }

        public List<NetworkPlayer> GetAllPlayers()
        {
            return NetworkManager.ConnectedClientsList
                .Select(a => a.PlayerObject.GetComponent<NetworkPlayer>())
                .ToList();
        }

        public NetworkPlayer GetLocalPlayer()
        {
            return NetworkManager.LocalClient?.PlayerObject?.GetComponent<NetworkPlayer>();
        }

        private Tween _connectWaiter = null;
        private void StartConnectScreenLocker()
        {
            var connectTimeout = Transport.ConnectTimeoutMS * Transport.MaxConnectAttempts / 1000f;
            if (_connectWaiter != null)
                _connectWaiter.Kill(true);
            _connectWaiter = DOTweenUtility.Delay(connectTimeout + 1f)
                .OnStart(() => ScreenLocker.Show(ScreenLockTypes.Unet))
                .OnComplete(() => ScreenLocker.Hide(ScreenLockTypes.Unet))
                .Play();
        }
        private void StopConnectScreenLocker()
        {
            if (_connectWaiter != null)
                _connectWaiter.Kill(true);
            _connectWaiter = null;
        }

        private void OnClientConnected(ulong id)
        {
            if (IsServer)
            {
                _connectedClients.Add(id);
                CheckClientsCount();
            }
            StopConnectScreenLocker();
            _onConnectedToServer.OnNext(id);
        }

        private void OnClientDisconnect(ulong id)
        {
            if (IsServer)
            {
                _connectedClients.Remove(id);
                CheckClientsCount();
            }
            if (!IsServer || ItsMe(id))
            {
                _onDisconnectFromServer.OnNext(id);
            }
        }

        public bool StartClient(string ip, string portStr)
        {
            if (!NetworkUtility.IsValidIPv4(ip)) return false;
            if (!NetworkUtility.IsValidPort(portStr)) return false;

            var port = ushort.Parse(portStr);

            Transport.SetConnectionData(ip, port);
            StartConnectScreenLocker();
            
            var result = NetworkManager.StartClient();
            if (result)
                _onStartClient.OnNext(default);
            return result;
        }

        public bool StartHost(string portStr)
        {
            if (!NetworkUtility.IsValidPort(portStr)) return false;

            var ip = NetworkUtility.GetLocalIPv4();
            var port = ushort.Parse(portStr);

            Transport.SetConnectionData(ip, port);
            NetworkManager.ConnectionApprovalCallback = ApprovalCheck;
            
            var result = NetworkManager.StartHost();
            if (result)
                _onStartHost.OnNext(default);
            return result;
        }

        private void ApprovalCheck(
            NetworkManager.ConnectionApprovalRequest request,
            NetworkManager.ConnectionApprovalResponse response)
        {
            if (NetworkManager.ConnectedClientsIds.Count < Constants.Network.PlayersCount
                || request.ClientNetworkId == NetworkManager.LocalClientId)
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

        private void CheckClientsCount()
        {
            Debug.Log($"TOTAL CLIENTS: {_connectedClients.Count}".Bold().Color(Color.yellow));
            _onPlayersEnough.OnNext(_connectedClients.Count >= Constants.Network.PlayersCount);
        }

        public void Shutdown()
        {
            NetworkManager.Shutdown();
            _connectedClients.Clear();
            _onShutdown.OnNext(default);
        }

        public void ForEachPlayer(Action<NetworkPlayer> action)
        {
            foreach (var c in NetworkManager.ConnectedClientsList)
            {
                action?.Invoke(c.PlayerObject.GetComponent<NetworkPlayer>());
            }
        }
    }
}