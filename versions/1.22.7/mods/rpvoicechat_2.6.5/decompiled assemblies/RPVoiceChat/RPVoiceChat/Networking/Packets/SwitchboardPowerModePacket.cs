using System;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Networking.Packets
{
	// Token: 0x02000085 RID: 133
	[ProtoContract(ImplicitFields = 1)]
	public class SwitchboardPowerModePacket
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600037B RID: 891 RVA: 0x00013244 File Offset: 0x00011444
		// (set) Token: 0x0600037C RID: 892 RVA: 0x0001324C File Offset: 0x0001144C
		public BlockPos SwitchboardPos { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600037D RID: 893 RVA: 0x00013255 File Offset: 0x00011455
		// (set) Token: 0x0600037E RID: 894 RVA: 0x0001325D File Offset: 0x0001145D
		public bool UsePowerRequirements { get; set; }
	}
}
