using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using RPVoiceChat.Networking;

namespace RPVoiceChat.Util
{
	// Token: 0x0200002A RID: 42
	public static class NetworkUtils
	{
		// Token: 0x06000100 RID: 256 RVA: 0x00007C78 File Offset: 0x00005E78
		public static IPAddress ParseIP(string ipString)
		{
			if (ipString == null)
			{
				throw new ArgumentNullException("IP string can not be null");
			}
			string[] addressParts = ipString.Split(':', 0);
			int endOfAddressIndex = ipString.IndexOf(']');
			if (ipString.StartsWith("[") && endOfAddressIndex != -1)
			{
				return IPAddress.Parse(ipString.Substring(1, endOfAddressIndex - 1));
			}
			if (addressParts.Length == 1 || addressParts.Length == 2)
			{
				return IPAddress.Parse(addressParts[0]);
			}
			try
			{
				return IPAddress.Parse(ipString);
			}
			catch (FormatException)
			{
			}
			try
			{
				List<string> listAddressParts = new List<string>(addressParts);
				listAddressParts.Remove(Enumerable.Last<string>(listAddressParts));
				return IPAddress.Parse(string.Join(":", listAddressParts));
			}
			catch (FormatException)
			{
			}
			throw new ArgumentException("IP string \"" + ipString + "\" is not a valid IP address");
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00007D48 File Offset: 0x00005F48
		public static bool IsInternalNetwork(string ip)
		{
			return NetworkUtils.IsInternalNetwork(IPAddress.Parse(ip));
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00007D58 File Offset: 0x00005F58
		public static bool IsInternalNetwork(IPAddress ip)
		{
			byte[] ipParts = ip.GetAddressBytes();
			return ipParts[0] == 10 || (ipParts[0] == 192 && ipParts[1] == 168) || (ipParts[0] == 172 && ipParts[1] >= 16 && ipParts[1] <= 31) || ipParts[0] == 25 || ipParts[0] == 26 || (ipParts[0] == 127 && ipParts[1] == 0 && ipParts[2] == 0 && ipParts[3] == 1);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00007DC7 File Offset: 0x00005FC7
		public static IPEndPoint GetEndPoint(ConnectionInfo connectionInfo)
		{
			return new IPEndPoint(IPAddress.Parse(connectionInfo.Address), connectionInfo.Port);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00007DE0 File Offset: 0x00005FE0
		public static string GetPublicIP()
		{
			return new HttpClient().GetStringAsync("https://ipinfo.io/ip").GetAwaiter().GetResult();
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00007E0C File Offset: 0x0000600C
		public static bool AssertEqual(IPEndPoint firstEndPoint, IPEndPoint secondEndPoint)
		{
			bool flag = firstEndPoint.Address.MapToIPv4().ToString() == secondEndPoint.Address.MapToIPv4().ToString();
			bool isSamePort = firstEndPoint.Port == secondEndPoint.Port;
			return flag && isSamePort;
		}
	}
}
