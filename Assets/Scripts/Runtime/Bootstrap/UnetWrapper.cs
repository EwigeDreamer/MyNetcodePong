using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Utilities.Network;

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
        return NetworkManager.StartHost();
    }
}