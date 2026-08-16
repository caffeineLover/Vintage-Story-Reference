using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000CB RID: 203
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public class ShotPacket
	{
		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000ABE RID: 2750 RVA: 0x00038B42 File Offset: 0x00036D42
		// (set) Token: 0x06000ABF RID: 2751 RVA: 0x00038B4A File Offset: 0x00036D4A
		public Guid[] ProjectileId { get; set; } = Array.Empty<Guid>();

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x00038B53 File Offset: 0x00036D53
		// (set) Token: 0x06000AC1 RID: 2753 RVA: 0x00038B5B File Offset: 0x00036D5B
		public int ShotId { get; set; }

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000AC2 RID: 2754 RVA: 0x00038B64 File Offset: 0x00036D64
		// (set) Token: 0x06000AC3 RID: 2755 RVA: 0x00038B6C File Offset: 0x00036D6C
		public double[] Position { get; set; }

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x00038B75 File Offset: 0x00036D75
		// (set) Token: 0x06000AC5 RID: 2757 RVA: 0x00038B7D File Offset: 0x00036D7D
		public double[] Velocity { get; set; }

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x00038B86 File Offset: 0x00036D86
		// (set) Token: 0x06000AC7 RID: 2759 RVA: 0x00038B8E File Offset: 0x00036D8E
		public int ItemId { get; set; }

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x00038B97 File Offset: 0x00036D97
		// (set) Token: 0x06000AC9 RID: 2761 RVA: 0x00038B9F File Offset: 0x00036D9F
		public int Amount { get; set; }

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000ACA RID: 2762 RVA: 0x00038BA8 File Offset: 0x00036DA8
		// (set) Token: 0x06000ACB RID: 2763 RVA: 0x00038BB0 File Offset: 0x00036DB0
		public bool RightHand { get; set; }

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000ACC RID: 2764 RVA: 0x00038BB9 File Offset: 0x00036DB9
		// (set) Token: 0x06000ACD RID: 2765 RVA: 0x00038BC1 File Offset: 0x00036DC1
		public byte[] Data { get; set; } = Array.Empty<byte>();
	}
}
