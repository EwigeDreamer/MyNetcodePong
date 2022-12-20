using System.Net;
using System.Text.RegularExpressions;

namespace Utilities.Network
{
    public static class NetworkUtility
    {
        public static string GetLocalIPv4()
        {
            var host = Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            throw new System.Exception("No network adapters with an IPv4 address in the system!");
        }

        public static bool IsValidIPv4(string ip)
        {
            return Regex.IsMatch(ip, @"^((25[0-5]|(2[0-4]|1\d|[1-9]|)\d)\.?\b){4}$");
        }

        public static bool IsValidPort(string port)
        {
            return ushort.TryParse(port, out _);
        }
    }
}