using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000D6 RID: 214
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public class ShotPacket
	{
		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000B1A RID: 2842 RVA: 0x00039B32 File Offset: 0x00037D32
		// (set) Token: 0x06000B1B RID: 2843 RVA: 0x00039B3A File Offset: 0x00037D3A
		public Guid[] ProjectileId { get; set; } = Array.Empty<Guid>();

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000B1C RID: 2844 RVA: 0x00039B43 File Offset: 0x00037D43
		// (set) Token: 0x06000B1D RID: 2845 RVA: 0x00039B4B File Offset: 0x00037D4B
		public int ShotId { get; set; }

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000B1E RID: 2846 RVA: 0x00039B54 File Offset: 0x00037D54
		// (set) Token: 0x06000B1F RID: 2847 RVA: 0x00039B5C File Offset: 0x00037D5C
		public double[] Position { get; set; }

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000B20 RID: 2848 RVA: 0x00039B65 File Offset: 0x00037D65
		// (set) Token: 0x06000B21 RID: 2849 RVA: 0x00039B6D File Offset: 0x00037D6D
		public double[] Velocity { get; set; }

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000B22 RID: 2850 RVA: 0x00039B76 File Offset: 0x00037D76
		// (set) Token: 0x06000B23 RID: 2851 RVA: 0x00039B7E File Offset: 0x00037D7E
		public int ItemId { get; set; }

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000B24 RID: 2852 RVA: 0x00039B87 File Offset: 0x00037D87
		// (set) Token: 0x06000B25 RID: 2853 RVA: 0x00039B8F File Offset: 0x00037D8F
		public int Amount { get; set; }

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000B26 RID: 2854 RVA: 0x00039B98 File Offset: 0x00037D98
		// (set) Token: 0x06000B27 RID: 2855 RVA: 0x00039BA0 File Offset: 0x00037DA0
		public bool RightHand { get; set; }

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000B28 RID: 2856 RVA: 0x00039BA9 File Offset: 0x00037DA9
		// (set) Token: 0x06000B29 RID: 2857 RVA: 0x00039BB1 File Offset: 0x00037DB1
		public byte[] Data { get; set; } = Array.Empty<byte>();
	}
}
