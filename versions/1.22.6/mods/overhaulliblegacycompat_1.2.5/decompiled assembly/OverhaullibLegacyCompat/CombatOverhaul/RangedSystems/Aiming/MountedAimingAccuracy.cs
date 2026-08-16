using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.RangedSystems.Aiming
{
	// Token: 0x020000DB RID: 219
	[NullableContext(1)]
	[Nullable(0)]
	internal class MountedAimingAccuracy : AccuracyModifier
	{
		// Token: 0x06000B30 RID: 2864 RVA: 0x0003A885 File Offset: 0x00038A85
		public MountedAimingAccuracy(EntityAgent entity, ClientAimingSystem clientAimingSystem) : base(entity, clientAimingSystem)
		{
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x0003A88F File Offset: 0x00038A8F
		public override void BeginAim()
		{
			base.BeginAim();
			this._walkAccuracyPenalty = 0f;
			this._sprintAccuracyPenalty = 0f;
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x0003A8B0 File Offset: 0x00038AB0
		public override void Update(float dt, AimingStats weaponStats)
		{
			IMountableSeat mountedOn = this.Entity.MountedOn;
			bool mountTriesToMove = ((mountedOn != null) ? mountedOn.Controls : null) != null && this.Entity.MountedOn.Controls.TriesToMove;
			bool flag;
			if (mountTriesToMove)
			{
				IMountableSeat mountedOn2 = this.Entity.MountedOn;
				flag = (mountedOn2 != null && mountedOn2.Controls.Sprint);
			}
			else
			{
				flag = false;
			}
			bool mountTriesToSprint = flag;
			this._walkAccuracyPenalty = GameMath.Clamp(mountTriesToMove ? (this._walkAccuracyPenalty + dt * 6.5f) : (this._walkAccuracyPenalty - dt * 2f), 0f, 0.8f);
			this._sprintAccuracyPenalty = GameMath.Clamp(mountTriesToSprint ? (this._sprintAccuracyPenalty + dt * 6.5f) : (this._sprintAccuracyPenalty - dt * 2f), 0f, 1.5f);
			if (this.ClientAimingSystem != null)
			{
				this.ClientAimingSystem.DriftMultiplier += (this._walkAccuracyPenalty + this._sprintAccuracyPenalty) * 0.8f * weaponStats.AccuracyMovePenalty;
				this.ClientAimingSystem.TwitchMultiplier += (this._walkAccuracyPenalty + this._sprintAccuracyPenalty) * 0.6f * weaponStats.AccuracyMovePenalty;
			}
		}

		// Token: 0x0400054A RID: 1354
		private float _walkAccuracyPenalty;

		// Token: 0x0400054B RID: 1355
		private float _sprintAccuracyPenalty;

		// Token: 0x0400054C RID: 1356
		private const float _walkMaxPenaltyMod = 0.8f;

		// Token: 0x0400054D RID: 1357
		private const float _sprintMaxPenaltyMod = 1.5f;

		// Token: 0x0400054E RID: 1358
		private const float _penaltyRiseRate = 6.5f;

		// Token: 0x0400054F RID: 1359
		private const float _penaltyDropRate = 2f;

		// Token: 0x04000550 RID: 1360
		private const float _driftMod = 0.8f;

		// Token: 0x04000551 RID: 1361
		private const float _twitchMod = 0.6f;
	}
}
