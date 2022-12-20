using Unity.Netcode;

public class UnetWrapper
{
    public readonly NetworkManager NetworkManager;

    public NetworkTransport Transport => NetworkManager.NetworkConfig.NetworkTransport; 
    
    public UnetWrapper(NetworkManager networkManager)
    {
        NetworkManager = networkManager;
    }

    public void StartClient(string ip, string port)
    {
        
    }

    public void StartHost(string port)
    {
        
    }
}