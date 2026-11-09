using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.RangedSystems.Aiming
{
	// Token: 0x020000E5 RID: 229
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class MovingAimingAccuracy : AccuracyModifier
	{
		// Token: 0x06000B89 RID: 2953 RVA: 0x0003B742 File Offset: 0x00039942
		public MovingAimingAccuracy(EntityAgent entity, ClientAimingSystem clientAimingSystem) : base(entity, clientAimingSystem)
		{
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0003B74C File Offset: 0x0003994C
		public override void BeginAim()
		{
			base.BeginAim();
			this._walkAccuracyPenalty = 0f;
			this._sprintAccuracyPenalty = 0f;
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x0003B76C File Offset: 0x0003996C
		public override void Update(float dt, AimingStats weaponStats)
		{
			this._walkAccuracyPenalty = GameMath.Clamp(this.Entity.Controls.TriesToMove ? (this._walkAccuracyPenalty + dt * 6.5f) : (this._walkAccuracyPenalty - dt * 2.5f), 0f, 1f);
			this._sprintAccuracyPenalty = GameMath.Clamp((this.Entity.Controls.TriesToMove && this.Entity.Controls.Sprint) ? (this._sprintAccuracyPenalty + dt * 6.5f) : (this._sprintAccuracyPenalty - dt * 2.5f), 0f, 1.5f);
			if (this.ClientAimingSystem != null)
			{
				this.ClientAimingSystem.DriftMultiplier += (this._walkAccuracyPenalty + this._sprintAccuracyPenalty) * 0.8f * weaponStats.AccuracyMovePenalty;
				this.ClientAimingSystem.TwitchMultiplier += (this._walkAccuracyPenalty + this._sprintAccuracyPenalty) * 0.6f * weaponStats.AccuracyMovePenalty;
			}
		}

		// Token: 0x04000564 RID: 1380
		private float _walkAccuracyPenalty;

		// Token: 0x04000565 RID: 1381
		private float _sprintAccuracyPenalty;

		// Token: 0x04000566 RID: 1382
		private const float _walkMaxPenaltyMod = 1f;

		// Token: 0x04000567 RID: 1383
		private const float _sprintMaxPenaltyMod = 1.5f;

		// Token: 0x04000568 RID: 1384
		private const float _penaltyRiseRate = 6.5f;

		// Token: 0x04000569 RID: 1385
		private const float _penaltyDropRate = 2.5f;

		// Token: 0x0400056A RID: 1386
		private const float _driftMod = 0.8f;

		// Token: 0x0400056B RID: 1387
		private const float _twitchMod = 0.6f;
	}
}
