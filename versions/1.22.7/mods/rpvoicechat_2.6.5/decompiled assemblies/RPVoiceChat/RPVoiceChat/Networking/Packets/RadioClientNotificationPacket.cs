using System;
using ProtoBuf;

namespace RPVoiceChat.Networking.Packets
{
	// Token: 0x0200007F RID: 127
	[ProtoContract(ImplicitFields = 1)]
	public class RadioClientNotificationPacket
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000361 RID: 865 RVA: 0x00013169 File Offset: 0x00011369
		// (set) Token: 0x06000362 RID: 866 RVA: 0x00013171 File Offset: 0x00011371
		public string LangKey { get; set; }
	}
}
