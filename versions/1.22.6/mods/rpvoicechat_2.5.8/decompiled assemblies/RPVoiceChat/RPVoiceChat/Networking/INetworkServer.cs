using System;

namespace RPVoiceChat.Networking
{
	// Token: 0x0200004B RID: 75
	public interface INetworkServer : IDisposable
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060001EA RID: 490
		// (remove) Token: 0x060001EB RID: 491
		event Action<AudioPacket> AudioPacketReceived;

		// Token: 0x060001EC RID: 492
		void Launch();

		// Token: 0x060001ED RID: 493
		ConnectionInfo GetConnectionInfo();

		// Token: 0x060001EE RID: 494
		string GetTransportID();

		// Token: 0x060001EF RID: 495
		bool SendPacket(NetworkPacket packet, string playerId);
	}
}
