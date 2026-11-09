using System;
using ProtoBuf;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000F0 RID: 240
	[ProtoContract(ImplicitFields = 1)]
	public sealed class ImpaleAttachPacket
	{
		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000C36 RID: 3126 RVA: 0x0003D777 File Offset: 0x0003B977
		// (set) Token: 0x06000C37 RID: 3127 RVA: 0x0003D77F File Offset: 0x0003B97F
		public long AttackerEntityId { get; set; }

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000C38 RID: 3128 RVA: 0x0003D788 File Offset: 0x0003B988
		// (set) Token: 0x06000C39 RID: 3129 RVA: 0x0003D790 File Offset: 0x0003B990
		public long TargetEntityId { get; set; }

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000C3A RID: 3130 RVA: 0x0003D799 File Offset: 0x0003B999
		// (set) Token: 0x06000C3B RID: 3131 RVA: 0x0003D7A1 File Offset: 0x0003B9A1
		public bool MainHand { get; set; }
	}
}
