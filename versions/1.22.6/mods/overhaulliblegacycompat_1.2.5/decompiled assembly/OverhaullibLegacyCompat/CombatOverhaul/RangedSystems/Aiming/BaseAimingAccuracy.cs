using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.RangedSystems.Aiming
{
	// Token: 0x020000D9 RID: 217
	[NullableContext(1)]
	[Nullable(0)]
	internal class BaseAimingAccuracy : AccuracyModifier
	{
		// Token: 0x06000B2B RID: 2859 RVA: 0x0003A6A0 File Offset: 0x000388A0
		public BaseAimingAccuracy(EntityAgent entity, ClientAimingSystem clientAimingSystem) : base(entity, clientAimingSystem)
		{
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0003A6AC File Offset: 0x000388AC
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
