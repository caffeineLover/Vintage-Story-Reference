using System;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000065 RID: 101
	public interface IExtendedNetworkClient : INetworkClient, IDisposable
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600027C RID: 636
		// (remove) Token: 0x0600027D RID: 637
		event Action<bool, IExtendedNetworkClient> OnConnectionLost;

		// Token: 0x0600027E RID: 638
		ConnectionInfo Connect(ConnectionInfo serverConnection);
	}
}
