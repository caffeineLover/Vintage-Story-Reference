using System;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000049 RID: 73
	public interface IExtendedNetworkServer : INetworkServer, IDisposable
	{
		// Token: 0x060001E4 RID: 484
		void PlayerConnected(string playerId, ConnectionInfo connectionInfo);

		// Token: 0x060001E5 RID: 485
		void PlayerDisconnected(string playerId);
	}
}
