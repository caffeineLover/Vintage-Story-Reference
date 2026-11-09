using System;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000066 RID: 102
	public interface IExtendedNetworkServer : INetworkServer, IDisposable
	{
		// Token: 0x0600027F RID: 639
		void PlayerConnected(string playerId, ConnectionInfo connectionInfo);

		// Token: 0x06000280 RID: 640
		void PlayerDisconnected(string playerId);
	}
}
