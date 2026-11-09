using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.RangedSystems.Aiming
{
	// Token: 0x020000E4 RID: 228
	[NullableContext(1)]
	[Nullable(0)]
	internal class BaseAimingAccuracy : AccuracyModifier
	{
		// Token: 0x06000B87 RID: 2951 RVA: 0x0003B690 File Offset: 0x00039890
		public BaseAimingAccuracy(EntityAgent entity, ClientAimingSystem clientAimingSystem) : base(entity, clientAimingSystem)
		{
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0003B69C File Offset: 0x0003989C
		public override void Update(float dt, AimingStats weaponStats)
		{
			if (weaponStats.AccuracyOvertime == TimeSpan.Zero)
			{
				this.ClientAimingSystem.DriftMultiplier = 1f;
				this.ClientAimingSystem.TwitchMultiplier = 1f;
				return;
			}
			float bullseyeAccuracy = (double)base.SecondsSinceAimStart > weaponStats.AccuracyOvertime.TotalSeconds;
			if ((double)base.SecondsSinceAimStart > weaponStats.AccuracyOvertime.TotalSeconds)
			{
				this.ClientAimingSystem.AimingState = WeaponAimingState.PartCharge;
			}
			this.ClientAimingSystem.DriftMultiplier = 1f + bullseyeAccuracy;
			this.ClientAimingSystem.TwitchMultiplier = 1f + bullseyeAccuracy * 3f;
		}
	}
}
