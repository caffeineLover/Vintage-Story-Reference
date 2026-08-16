using System;
using ProtoBuf;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000E6 RID: 230
	[ProtoContract(ImplicitFields = 1)]
	public sealed class ImpaleReleasePacket
	{
		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000BE1 RID: 3041 RVA: 0x0003C7C2 File Offset: 0x0003A9C2
		// (set) Token: 0x06000BE2 RID: 3042 RVA: 0x0003C7CA File Offset: 0x0003A9CA
		public long AttackerEntityId { get; set; }

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x0003C7D3 File Offset: 0x0003A9D3
		// (set) Token: 0x06000BE4 RID: 3044 RVA: 0x0003C7DB File Offset: 0x0003A9DB
		public long TargetEntityId { get; set; }

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x0003C7E4 File Offset: 0x0003A9E4
		// (set) Token: 0x06000BE6 RID: 3046 RVA: 0x0003C7EC File Offset: 0x0003A9EC
		public bool MainHand { get; set; }
	}
}
