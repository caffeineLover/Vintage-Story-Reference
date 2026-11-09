using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using ProtoBuf;
using Vintagestory.API.Common;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200010A RID: 266
	[NullableContext(2)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public sealed class DamageBlockPacket
	{
		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000D3E RID: 3390 RVA: 0x000413BC File Offset: 0x0003F5BC
		// (set) Token: 0x06000D3F RID: 3391 RVA: 0x000413C4 File Offset: 0x0003F5C4
		public int Zones { get; set; }

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000D40 RID: 3392 RVA: 0x000413CD File Offset: 0x0003F5CD
		// (set) Token: 0x06000D41 RID: 3393 RVA: 0x000413D5 File Offset: 0x0003F5D5
		[Nullable(1)]
		public float[] Directions { [NullableContext(1)] get; [NullableContext(1)] set; } = Array.Empty<float>();

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000D42 RID: 3394 RVA: 0x000413DE File Offset: 0x0003F5DE
		// (set) Token: 0x06000D43 RID: 3395 RVA: 0x000413E6 File Offset: 0x0003F5E6
		public bool MainHand { get; set; }

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000D44 RID: 3396 RVA: 0x000413EF File Offset: 0x0003F5EF
		// (set) Token: 0x06000D45 RID: 3397 RVA: 0x000413F7 File Offset: 0x0003F5F7
		public string Sound { get; set; }

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000D46 RID: 3398 RVA: 0x00041400 File Offset: 0x0003F600
		// (set) Token: 0x06000D47 RID: 3399 RVA: 0x00041408 File Offset: 0x0003F608
		public Dictionary<EnumDamageType, int> BlockTier { get; set; }

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000D48 RID: 3400 RVA: 0x00041411 File Offset: 0x0003F611
		// (set) Token: 0x06000D49 RID: 3401 RVA: 0x00041419 File Offset: 0x0003F619
		public bool CanBlockProjectiles { get; set; }

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000D4A RID: 3402 RVA: 0x00041422 File Offset: 0x0003F622
		// (set) Token: 0x06000D4B RID: 3403 RVA: 0x0004142A File Offset: 0x0003F62A
		public int StaggerTimeMs { get; set; }

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000D4C RID: 3404 RVA: 0x00041433 File Offset: 0x0003F633
		// (set) Token: 0x06000D4D RID: 3405 RVA: 0x0004143B File Offset: 0x0003F63B
		public int StaggerTier { get; set; }

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000D4E RID: 3406 RVA: 0x00041444 File Offset: 0x0003F644
		// (set) Token: 0x06000D4F RID: 3407 RVA: 0x0004144C File Offset: 0x0003F64C
		public EnumDamageBlockKind Kind { get; set; }

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000D50 RID: 3408 RVA: 0x00041455 File Offset: 0x0003F655
		// (set) Token: 0x06000D51 RID: 3409 RVA: 0x0004145D File Offset: 0x0003F65D
		public ulong Id { get; set; }

		// Token: 0x06000D52 RID: 3410 RVA: 0x00041468 File Offset: 0x0003F668
		[NullableContext(1)]
		public DamageBlockStats ToBlockStats(Action<float, int, int> callback)
		{
			return new DamageBlockStats((PlayerBodyPart)this.Zones, DirectionConstrain.FromArray(this.Directions), callback, this.Sound, this.BlockTier, this.CanBlockProjectiles, TimeSpan.FromMilliseconds((long)this.StaggerTimeMs), this.StaggerTier, this.Kind, this.MainHand);
		}
	}
}
