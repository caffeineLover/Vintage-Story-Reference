using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x020000FE RID: 254
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class DamageBlockStats
	{
		// Token: 0x06000CE1 RID: 3297 RVA: 0x0004036C File Offset: 0x0003E56C
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

		// Token: 0x0400064B RID: 1611
		public readonly PlayerBodyPart ZoneType;

		// Token: 0x0400064C RID: 1612
		public readonly DirectionConstrain Directions;

		// Token: 0x0400064D RID: 1613
		[Nullable(1)]
		public readonly Action<float, int, int> Callback;

		// Token: 0x0400064E RID: 1614
		public readonly string Sound;

		// Token: 0x0400064F RID: 1615
		public readonly Dictionary<EnumDamageType, int> BlockTier;

		// Token: 0x04000650 RID: 1616
		public readonly bool CanBlockProjectiles;

		// Token: 0x04000651 RID: 1617
		public readonly TimeSpan StaggerTime;

		// Token: 0x04000652 RID: 1618
		public readonly int StaggerTier;

		// Token: 0x04000653 RID: 1619
		public readonly EnumDamageBlockKind Kind;

		// Token: 0x04000654 RID: 1620
		public readonly bool MainHand;
	}
}
