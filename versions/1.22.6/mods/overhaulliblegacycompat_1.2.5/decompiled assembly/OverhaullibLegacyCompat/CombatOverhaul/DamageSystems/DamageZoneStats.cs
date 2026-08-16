using System;
using CombatOverhaul.Utils;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000112 RID: 274
	public readonly struct DamageZoneStats
	{
		// Token: 0x06000D75 RID: 3445 RVA: 0x000417D0 File Offset: 0x0003F9D0
		public DamageZoneStats(PlayerBodyPart type, float coverage, DirectionConstrain directions, float damageMultiplier)
		{
			this.ZoneType = type;
			this.Coverage = coverage;
			this.Directions = directions;
			this.DamageMultiplier = damageMultiplier;
		}

		// Token: 0x040006A6 RID: 1702
		public readonly PlayerBodyPart ZoneType;

		// Token: 0x040006A7 RID: 1703
		public readonly float Coverage;

		// Token: 0x040006A8 RID: 1704
		public readonly DirectionConstrain Directions;

		// Token: 0x040006A9 RID: 1705
		public readonly float DamageMultiplier;
	}
}
