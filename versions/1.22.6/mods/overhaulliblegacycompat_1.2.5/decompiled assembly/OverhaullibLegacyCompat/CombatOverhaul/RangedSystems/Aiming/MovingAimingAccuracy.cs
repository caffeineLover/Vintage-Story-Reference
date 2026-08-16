using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.RangedSystems.Aiming
{
	// Token: 0x020000DA RID: 218
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class MovingAimingAccuracy : AccuracyModifier
	{
		// Token: 0x06000B2D RID: 2861 RVA: 0x0003A752 File Offset: 0x00038952
		public MovingAimingAccuracy(EntityAgent entity, ClientAimingSystem clientAimingSystem) : base(entity, clientAimingSystem)
		{
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x0003A75C File Offset: 0x0003895C
		public override void BeginAim()
		{
			base.BeginAim();
			this._walkAccuracyPenalty = 0f;
			this._sprintAccuracyPenalty = 0f;
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x0003A77C File Offset: 0x0003897C
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

		// Token: 0x04000542 RID: 1346
		private float _walkAccuracyPenalty;

		// Token: 0x04000543 RID: 1347
		private float _sprintAccuracyPenalty;

		// Token: 0x04000544 RID: 1348
		private const float _walkMaxPenaltyMod = 1f;

		// Token: 0x04000545 RID: 1349
		private const float _sprintMaxPenaltyMod = 1.5f;

		// Token: 0x04000546 RID: 1350
		private const float _penaltyRiseRate = 6.5f;

		// Token: 0x04000547 RID: 1351
		private const float _penaltyDropRate = 2.5f;

		// Token: 0x04000548 RID: 1352
		private const float _driftMod = 0.8f;

		// Token: 0x04000549 RID: 1353
		private const float _twitchMod = 0.6f;
	}
}
