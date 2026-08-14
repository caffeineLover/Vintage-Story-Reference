using System;

namespace RPVoiceChat.Networking
{
	// Token: 0x0200004A RID: 74
	public interface INetworkClient : IDisposable
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060001E6 RID: 486
		// (remove) Token: 0x060001E7 RID: 487
		event Action<AudioPacket> OnAudioReceived;

		// Token: 0x060001E8 RID: 488
		string GetTransportID();

		// Token: 0x060001E9 RID: 489
		bool SendAudioToServer(AudioPacket packet);
	}
}
