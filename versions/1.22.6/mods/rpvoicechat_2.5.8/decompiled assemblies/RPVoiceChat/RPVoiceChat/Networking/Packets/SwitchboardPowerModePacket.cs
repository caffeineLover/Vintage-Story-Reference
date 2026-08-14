using System;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Networking.Packets
{
	// Token: 0x02000062 RID: 98
	[ProtoContract(ImplicitFields = 1)]
	public class SwitchboardPowerModePacket
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x0000F69D File Offset: 0x0000D89D
		// (set) Token: 0x060002C7 RID: 711 RVA: 0x0000F6A5 File Offset: 0x0000D8A5
		public BlockPos SwitchboardPos { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x0000F6AE File Offset: 0x0000D8AE
		// (set) Token: 0x060002C9 RID: 713 RVA: 0x0000F6B6 File Offset: 0x0000D8B6
		public bool UsePowerRequirements { get; set; }
	}
}
