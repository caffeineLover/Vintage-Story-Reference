using System;
using Vintagestory.API.Common;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000069 RID: 105
	public abstract class NativeNetworkBase
	{
		// Token: 0x0600028B RID: 651 RVA: 0x0001044F File Offset: 0x0000E64F
		public NativeNetworkBase(ICoreAPI api)
		{
			api.Network.RegisterChannel("RPAudioChannel").RegisterMessageType<AudioPacket>();
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0001046D File Offset: 0x0000E66D
		public string GetTransportID()
		{
			return "NativeTCP";
		}

		// Token: 0x04000110 RID: 272
		protected const string ChannelName = "RPAudioChannel";

		// Token: 0x04000111 RID: 273
		protected const string SPChannelName = "RPAudioChannel_SP";

		// Token: 0x04000112 RID: 274
		protected const string _transportID = "NativeTCP";
	}
}
