using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using ProtoBuf;
using Vintagestory.API.Common;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x020000FF RID: 255
	[NullableContext(2)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public sealed class DamageBlockPacket
	{
		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x000403CC File Offset: 0x0003E5CC
		// (set) Token: 0x06000CE3 RID: 3299 RVA: 0x000403D4 File Offset: 0x0003E5D4
		public int Zones { get; set; }

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x000403DD File Offset: 0x0003E5DD
		// (set) Token: 0x06000CE5 RID: 3301 RVA: 0x000403E5 File Offset: 0x0003E5E5
		[Nullable(1)]
		public float[] Directions { [NullableContext(1)] get; [NullableContext(1)] set; } = Array.Empty<float>();

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x000403EE File Offset: 0x0003E5EE
		// (set) Token: 0x06000CE7 RID: 3303 RVA: 0x000403F6 File Offset: 0x0003E5F6
		public bool MainHand { get; set; }

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x000403FF File Offset: 0x0003E5FF
		// (set) Token: 0x06000CE9 RID: 3305 RVA: 0x00040407 File Offset: 0x0003E607
		public string Sound { get; set; }

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000CEA RID: 3306 RVA: 0x00040410 File Offset: 0x0003E610
		// (set) Token: 0x06000CEB RID: 3307 RVA: 0x00040418 File Offset: 0x0003E618
		public Dictionary<EnumDamageType, int> BlockTier { get; set; }

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x00040421 File Offset: 0x0003E621
		// (set) Token: 0x06000CED RID: 3309 RVA: 0x00040429 File Offset: 0x0003E629
		public bool CanBlockProjectiles { get; set; }

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000CEE RID: 3310 RVA: 0x00040432 File Offset: 0x0003E632
		// (set) Token: 0x06000CEF RID: 3311 RVA: 0x0004043A File Offset: 0x0003E63A
		public int StaggerTimeMs { get; set; }

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x00040443 File Offset: 0x0003E643
		// (set) Token: 0x06000CF1 RID: 3313 RVA: 0x0004044B File Offset: 0x0003E64B
		public int StaggerTier { get; set; }

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000CF2 RID: 3314 RVA: 0x00040454 File Offset: 0x0003E654
		// (set) Token: 0x06000CF3 RID: 3315 RVA: 0x0004045C File Offset: 0x0003E65C
		public EnumDamageBlockKind Kind { get; set; }

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000CF4 RID: 3316 RVA: 0x00040465 File Offset: 0x0003E665
		// (set) Token: 0x06000CF5 RID: 3317 RVA: 0x0004046D File Offset: 0x0003E66D
		public ulong Id { get; set; }

		// Token: 0x06000CF6 RID: 3318 RVA: 0x00040478 File Offset: 0x0003E678
		[NullableContext(1)]
		public DamageBlockStats ToBlockStats(Action<float, int, int> callback)
		{
			return new DamageBlockStats((PlayerBodyPart)this.Zones, DirectionConstrain.FromArray(this.Directions), callback, this.Sound, this.BlockTier, this.CanBlockProjectiles, TimeSpan.FromMilliseconds((long)this.StaggerTimeMs), this.StaggerTier, this.Kind, this.MainHand);
		}
	}
}
