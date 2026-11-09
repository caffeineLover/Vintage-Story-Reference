using System;
using ProtoBuf;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000F1 RID: 241
	[ProtoContract(ImplicitFields = 1)]
	public sealed class ImpaleReleasePacket
	{
		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000C3D RID: 3133 RVA: 0x0003D7B2 File Offset: 0x0003B9B2
		// (set) Token: 0x06000C3E RID: 3134 RVA: 0x0003D7BA File Offset: 0x0003B9BA
		public long AttackerEntityId { get; set; }

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000C3F RID: 3135 RVA: 0x0003D7C3 File Offset: 0x0003B9C3
		// (set) Token: 0x06000C40 RID: 3136 RVA: 0x0003D7CB File Offset: 0x0003B9CB
		public long TargetEntityId { get; set; }

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000C41 RID: 3137 RVA: 0x0003D7D4 File Offset: 0x0003B9D4
		// (set) Token: 0x06000C42 RID: 3138 RVA: 0x0003D7DC File Offset: 0x0003B9DC
		public bool MainHand { get; set; }
	}
}
