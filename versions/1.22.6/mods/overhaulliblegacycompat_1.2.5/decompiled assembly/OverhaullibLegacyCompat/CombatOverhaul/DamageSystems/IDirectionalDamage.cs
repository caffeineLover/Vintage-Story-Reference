using System;
using CombatOverhaul.Utils;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x0200011F RID: 287
	public interface IDirectionalDamage
	{
		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06000E04 RID: 3588
		DirectionOffset Direction { get; }

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06000E05 RID: 3589
		PlayerBodyPart Target { get; }

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000E06 RID: 3590
		float WeightMultiplier { get; }
	}
}
