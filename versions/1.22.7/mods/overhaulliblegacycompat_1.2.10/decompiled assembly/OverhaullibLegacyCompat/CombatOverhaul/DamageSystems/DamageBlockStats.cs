using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000109 RID: 265
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class DamageBlockStats
	{
		// Token: 0x06000D3D RID: 3389 RVA: 0x0004135C File Offset: 0x0003F55C
		public DamageBlockStats(PlayerBodyPart type, DirectionConstrain directions, [Nullable(1)] Action<float, int, int> callback, string sound, Dictionary<EnumDamageType, int> blockTier, bool canBlockProjectiles, TimeSpan staggerTime, int staggerTier, EnumDamageBlockKind kind, bool mainHand)
		{
			this.ZoneType = type;
			this.Directions = directions;
			this.Callback = callback;
			this.Sound = sound;
			this.BlockTier = blockTier;
			this.CanBlockProjectiles = canBlockProjectiles;
			this.StaggerTime = staggerTime;
			this.StaggerTier = staggerTier;
			this.Kind = kind;
			this.MainHand = mainHand;
		}

		// Token: 0x0400066D RID: 1645
		public readonly PlayerBodyPart ZoneType;

		// Token: 0x0400066E RID: 1646
		public readonly DirectionConstrain Directions;

		// Token: 0x0400066F RID: 1647
		[Nullable(1)]
		public readonly Action<float, int, int> Callback;

		// Token: 0x04000670 RID: 1648
		public readonly string Sound;

		// Token: 0x04000671 RID: 1649
		public readonly Dictionary<EnumDamageType, int> BlockTier;

		// Token: 0x04000672 RID: 1650
		public readonly bool CanBlockProjectiles;

		// Token: 0x04000673 RID: 1651
		public readonly TimeSpan StaggerTime;

		// Token: 0x04000674 RID: 1652
		public readonly int StaggerTier;

		// Token: 0x04000675 RID: 1653
		public readonly EnumDamageBlockKind Kind;

		// Token: 0x04000676 RID: 1654
		public readonly bool MainHand;
	}
}
