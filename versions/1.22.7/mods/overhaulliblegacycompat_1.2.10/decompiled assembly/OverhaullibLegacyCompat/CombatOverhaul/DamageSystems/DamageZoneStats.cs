using System;
using CombatOverhaul.Utils;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200011D RID: 285
	public readonly struct DamageZoneStats
	{
		// Token: 0x06000DD1 RID: 3537 RVA: 0x000427F4 File Offset: 0x000409F4
		public DamageZoneStats(PlayerBodyPart type, float coverage, DirectionConstrain directions, float damageMultiplier)
		{
			this.ZoneType = type;
			this.Coverage = coverage;
			this.Directions = directions;
			this.DamageMultiplier = damageMultiplier;
		}

		// Token: 0x040006C7 RID: 1735
		public readonly PlayerBodyPart ZoneType;

		// Token: 0x040006C8 RID: 1736
		public readonly float Coverage;

		// Token: 0x040006C9 RID: 1737
		public readonly DirectionConstrain Directions;

		// Token: 0x040006CA RID: 1738
		public readonly float DamageMultiplier;
	}
}
