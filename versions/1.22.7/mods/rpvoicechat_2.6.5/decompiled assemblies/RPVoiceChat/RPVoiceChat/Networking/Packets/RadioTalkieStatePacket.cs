using System;
using ProtoBuf;

namespace RPVoiceChat.Networking.Packets
{
	// Token: 0x02000084 RID: 132
	[ProtoContract(ImplicitFields = 1)]
	public class RadioTalkieStatePacket
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000376 RID: 886 RVA: 0x0001321A File Offset: 0x0001141A
		// (set) Token: 0x06000377 RID: 887 RVA: 0x00013222 File Offset: 0x00011422
		public bool Transmitting { get; set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000378 RID: 888 RVA: 0x0001322B File Offset: 0x0001142B
		// (set) Token: 0x06000379 RID: 889 RVA: 0x00013233 File Offset: 0x00011433
		public string Frequency { get; set; }
	}
}
