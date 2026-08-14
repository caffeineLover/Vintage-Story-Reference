using System;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Networking.Packets
{
	// Token: 0x02000063 RID: 99
	[ProtoContract(ImplicitFields = 1)]
	public class SwitchboardRenameNetworkPacket
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000F6C7 File Offset: 0x0000D8C7
		// (set) Token: 0x060002CC RID: 716 RVA: 0x0000F6CF File Offset: 0x0000D8CF
		public BlockPos SwitchboardPos { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0000F6D8 File Offset: 0x0000D8D8
		// (set) Token: 0x060002CE RID: 718 RVA: 0x0000F6E0 File Offset: 0x0000D8E0
		public string NetworkName { get; set; }
	}
}
