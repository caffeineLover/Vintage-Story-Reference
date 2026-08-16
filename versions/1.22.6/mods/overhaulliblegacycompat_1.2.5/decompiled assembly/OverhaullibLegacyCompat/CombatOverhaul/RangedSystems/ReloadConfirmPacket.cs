using System;
using ProtoBuf;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000CA RID: 202
	[ProtoContract(ImplicitFields = 1)]
	public class ReloadConfirmPacket
	{
		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x00038B18 File Offset: 0x00036D18
		// (set) Token: 0x06000ABA RID: 2746 RVA: 0x00038B20 File Offset: 0x00036D20
		public int ReloadId { get; set; }

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000ABB RID: 2747 RVA: 0x00038B29 File Offset: 0x00036D29
		// (set) Token: 0x06000ABC RID: 2748 RVA: 0x00038B31 File Offset: 0x00036D31
		public bool Success { get; set; }
	}
}
