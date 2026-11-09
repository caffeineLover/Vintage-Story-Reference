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
		// Token: 0x06000109 RID: 265 RVA: 0x000084B8 File Offset: 0x000066B8
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

		// Token: 0x0600010A RID: 266 RVA: 0x00008588 File Offset: 0x00006788
		public static bool IsInternalNetwork(string ip)
		{
			return NetworkUtils.IsInternalNetwork(IPAddress.Parse(ip));
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00008598 File Offset: 0x00006798
		public static bool IsInternalNetwork(IPAddress ip)
		{
			byte[] ipParts = ip.GetAddressBytes();
			return ipParts[0] == 10 || (ipParts[0] == 192 && ipParts[1] == 168) || (ipParts[0] == 172 && ipParts[1] >= 16 && ipParts[1] <= 31) || ipParts[0] == 25 || ipParts[0] == 26 || (ipParts[0] == 127 && ipParts[1] == 0 && ipParts[2] == 0 && ipParts[3] == 1);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00008607 File Offset: 0x00006807
		public static IPEndPoint GetEndPoint(ConnectionInfo connectionInfo)
		{
			return new IPEndPoint(IPAddress.Parse(connectionInfo.Address), connectionInfo.Port);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00008620 File Offset: 0x00006820
		public static string GetPublicIP()
		{
			return new HttpClient().GetStringAsync("https://ipinfo.io/ip").GetAwaiter().GetResult();
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000864C File Offset: 0x0000684C
		public static bool AssertEqual(IPEndPoint firstEndPoint, IPEndPoint secondEndPoint)
		{
			bool flag = firstEndPoint.Address.MapToIPv4().ToString() == secondEndPoint.Address.MapToIPv4().ToString();
			bool isSamePort = firstEndPoint.Port == secondEndPoint.Port;
			return flag && isSamePort;
		}
	}
}
