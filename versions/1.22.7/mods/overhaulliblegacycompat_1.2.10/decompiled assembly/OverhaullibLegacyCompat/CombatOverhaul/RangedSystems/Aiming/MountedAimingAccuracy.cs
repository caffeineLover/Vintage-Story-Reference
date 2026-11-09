using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.RangedSystems.Aiming
{
	// Token: 0x020000E6 RID: 230
	[NullableContext(1)]
	[Nullable(0)]
	internal class MountedAimingAccuracy : AccuracyModifier
	{
		// Token: 0x06000B8C RID: 2956 RVA: 0x0003B875 File Offset: 0x00039A75
		public MountedAimingAccuracy(EntityAgent entity, ClientAimingSystem clientAimingSystem) : base(entity, clientAimingSystem)
		{
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x0003B87F File Offset: 0x00039A7F
		public override void BeginAim()
		{
			base.BeginAim();
			this._walkAccuracyPenalty = 0f;
			this._sprintAccuracyPenalty = 0f;
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x0003B8A0 File Offset: 0x00039AA0
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

		// Token: 0x0400056C RID: 1388
		private float _walkAccuracyPenalty;

		// Token: 0x0400056D RID: 1389
		private float _sprintAccuracyPenalty;

		// Token: 0x0400056E RID: 1390
		private const float _walkMaxPenaltyMod = 0.8f;

		// Token: 0x0400056F RID: 1391
		private const float _sprintMaxPenaltyMod = 1.5f;

		// Token: 0x04000570 RID: 1392
		private const float _penaltyRiseRate = 6.5f;

		// Token: 0x04000571 RID: 1393
		private const float _penaltyDropRate = 2f;

		// Token: 0x04000572 RID: 1394
		private const float _driftMod = 0.8f;

		// Token: 0x04000573 RID: 1395
		private const float _twitchMod = 0.6f;
	}
}
