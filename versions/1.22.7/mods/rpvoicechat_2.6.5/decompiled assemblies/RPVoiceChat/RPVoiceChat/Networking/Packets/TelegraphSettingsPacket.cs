using System;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Networking.Packets
{
	// Token: 0x02000088 RID: 136
	[ProtoContract(ImplicitFields = 1)]
	public class TelegraphSettingsPacket
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000385 RID: 901 RVA: 0x00013298 File Offset: 0x00011498
		// (set) Token: 0x06000386 RID: 902 RVA: 0x000132A0 File Offset: 0x000114A0
		public BlockPos TelegraphPos { get; set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000387 RID: 903 RVA: 0x000132A9 File Offset: 0x000114A9
		// (set) Token: 0x06000388 RID: 904 RVA: 0x000132B1 File Offset: 0x000114B1
		public TelegraphSettingsOperation Operation { get; set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000389 RID: 905 RVA: 0x000132BA File Offset: 0x000114BA
		// (set) Token: 0x0600038A RID: 906 RVA: 0x000132C2 File Offset: 0x000114C2
		public string Value { get; set; }
	}
}
