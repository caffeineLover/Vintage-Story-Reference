using System;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Networking.Packets
{
	// Token: 0x02000065 RID: 101
	[ProtoContract(ImplicitFields = 1)]
	public class TelegraphSettingsPacket
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x0000F6F1 File Offset: 0x0000D8F1
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x0000F6F9 File Offset: 0x0000D8F9
		public BlockPos TelegraphPos { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000F702 File Offset: 0x0000D902
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x0000F70A File Offset: 0x0000D90A
		public TelegraphSettingsOperation Operation { get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000F713 File Offset: 0x0000D913
		// (set) Token: 0x060002D5 RID: 725 RVA: 0x0000F71B File Offset: 0x0000D91B
		public string Value { get; set; }
	}
}
