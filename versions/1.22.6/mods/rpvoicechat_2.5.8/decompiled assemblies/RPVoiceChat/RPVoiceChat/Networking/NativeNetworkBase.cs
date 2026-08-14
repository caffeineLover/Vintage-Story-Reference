using System;
using Vintagestory.API.Common;

namespace RPVoiceChat.Networking
{
	// Token: 0x0200004C RID: 76
	public abstract class NativeNetworkBase
	{
		// Token: 0x060001F0 RID: 496 RVA: 0x0000C983 File Offset: 0x0000AB83
		public NativeNetworkBase(ICoreAPI api)
		{
			api.Network.RegisterChannel("RPAudioChannel").RegisterMessageType<AudioPacket>();
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000C9A1 File Offset: 0x0000ABA1
		public string GetTransportID()
		{
			return "NativeTCP";
		}

		// Token: 0x040000BE RID: 190
		protected const string ChannelName = "RPAudioChannel";

		// Token: 0x040000BF RID: 191
		protected const string SPChannelName = "RPAudioChannel_SP";

		// Token: 0x040000C0 RID: 192
		protected const string _transportID = "NativeTCP";
	}
}
