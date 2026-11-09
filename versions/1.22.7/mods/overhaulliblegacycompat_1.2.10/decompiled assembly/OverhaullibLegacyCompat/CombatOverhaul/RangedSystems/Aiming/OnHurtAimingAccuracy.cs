using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.RangedSystems.Aiming
{
	// Token: 0x020000E7 RID: 231
	[NullableContext(1)]
	[Nullable(0)]
	internal class OnHurtAimingAccuracy : AccuracyModifier
	{
		// Token: 0x06000B8F RID: 2959 RVA: 0x0003B9CE File Offset: 0x00039BCE
		public OnHurtAimingAccuracy(EntityAgent entity, ClientAimingSystem clientAimingSystem) : base(entity, clientAimingSystem)
		{
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x0003B9D8 File Offset: 0x00039BD8
		public override void Update(float dt, AimingStats weaponStats)
		{
			this._accuracyPenalty = GameMath.Clamp(this._accuracyPenalty - dt / 3f, 0f, 0.4f);
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x0003B9FD File Offset: 0x00039BFD
		public override void OnHurt(float damage)
		{
			if (damage > 3f)
			{
				this._accuracyPenalty = -0.4f;
			}
		}

		// Token: 0x04000574 RID: 1396
		private float _accuracyPenalty;
	}
}
