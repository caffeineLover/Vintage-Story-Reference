using System;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000068 RID: 104
	public interface INetworkServer : IDisposable
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000285 RID: 645
		// (remove) Token: 0x06000286 RID: 646
		event Action<AudioPacket> AudioPacketReceived;

		// Token: 0x06000287 RID: 647
		void Launch();

		// Token: 0x06000288 RID: 648
		ConnectionInfo GetConnectionInfo();

		// Token: 0x06000289 RID: 649
		string GetTransportID();

		// Token: 0x0600028A RID: 650
		bool SendPacket(NetworkPacket packet, string playerId);
	}
}
