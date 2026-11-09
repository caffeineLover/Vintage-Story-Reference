using System;
using CombatOverhaul.Utils;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200012A RID: 298
	public interface IDirectionalDamage
	{
		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000E6A RID: 3690
		DirectionOffset Direction { get; }

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000E6B RID: 3691
		PlayerBodyPart Target { get; }

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000E6C RID: 3692
		float WeightMultiplier { get; }
	}
}
