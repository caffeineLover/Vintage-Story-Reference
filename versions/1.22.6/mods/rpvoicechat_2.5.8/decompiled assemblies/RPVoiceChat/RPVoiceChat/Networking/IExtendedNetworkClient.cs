using System;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000048 RID: 72
	public interface IExtendedNetworkClient : INetworkClient, IDisposable
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060001E1 RID: 481
		// (remove) Token: 0x060001E2 RID: 482
		event Action<bool, IExtendedNetworkClient> OnConnectionLost;

		// Token: 0x060001E3 RID: 483
		ConnectionInfo Connect(ConnectionInfo serverConnection);
	}
}
