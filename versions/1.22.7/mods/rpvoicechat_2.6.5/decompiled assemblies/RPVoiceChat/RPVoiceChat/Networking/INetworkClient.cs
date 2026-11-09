using System;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000067 RID: 103
	public interface INetworkClient : IDisposable
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000281 RID: 641
		// (remove) Token: 0x06000282 RID: 642
		event Action<AudioPacket> OnAudioReceived;

		// Token: 0x06000283 RID: 643
		string GetTransportID();

		// Token: 0x06000284 RID: 644
		bool SendAudioToServer(AudioPacket packet);
	}
}
