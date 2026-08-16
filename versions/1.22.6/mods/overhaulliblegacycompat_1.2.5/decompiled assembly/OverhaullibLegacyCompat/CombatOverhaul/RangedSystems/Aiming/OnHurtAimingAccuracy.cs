using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.RangedSystems.Aiming
{
	// Token: 0x020000DC RID: 220
	[NullableContext(1)]
	[Nullable(0)]
	internal class OnHurtAimingAccuracy : AccuracyModifier
	{
		// Token: 0x06000B33 RID: 2867 RVA: 0x0003A9DE File Offset: 0x00038BDE
		public OnHurtAimingAccuracy(EntityAgent entity, ClientAimingSystem clientAimingSystem) : base(entity, clientAimingSystem)
		{
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0003A9E8 File Offset: 0x00038BE8
		public override void Update(float dt, AimingStats weaponStats)
		{
			this._accuracyPenalty = GameMath.Clamp(this._accuracyPenalty - dt / 3f, 0f, 0.4f);
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0003AA0D File Offset: 0x00038C0D
		public override void OnHurt(float damage)
		{
			if (damage > 3f)
			{
				this._accuracyPenalty = -0.4f;
			}
		}

		// Token: 0x04000552 RID: 1362
		private float _accuracyPenalty;
	}
}
