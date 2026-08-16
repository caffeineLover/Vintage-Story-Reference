using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000B9 RID: 185
	public interface IBallisticSolver
	{
		// Token: 0x06000989 RID: 2441
		IBallisticSolver.BallisticOutput SolveBallisticArc(IBallisticSolver.TargetData data);

		// Token: 0x0600098A RID: 2442
		[NullableContext(1)]
		void DrawBallisticArc(IBallisticSolver.BallisticOutput arcData, IBallisticSolver.TargetData targetData, IWorldAccessor world);

		// Token: 0x0600098B RID: 2443
		IBallisticSolver.BallisticOutput ApplyDispersion(IBallisticSolver.BallisticOutput targetData, IBallisticSolver.DispersionData dispersionData);

		// Token: 0x0600098C RID: 2444 RVA: 0x00033480 File Offset: 0x00031680
		[NullableContext(1)]
		public static IBallisticSolver.TargetData GetTargetData(Entity shooterEntity, Entity targetEntity, double gravityFactor, double speed)
		{
			Vec3d pos = shooterEntity.ServerPos.XYZ.Add(0.0, shooterEntity.LocalEyePos.Y, 0.0);
			Vec3d targetPos = targetEntity.ServerPos.XYZ.Add(0.0, targetEntity.LocalEyePos.Y, 0.0);
			Vec3d targetVelocity = targetEntity.ServerPos.Motion;
			double gravityAcceleration = gravityFactor * (double)GlobalConstants.GravityPerSecond * 0.01666666753590107;
			return new IBallisticSolver.TargetData(new FastVec3d(pos.X, pos.Y, pos.Z), new FastVec3d(targetPos.X, targetPos.Y, targetPos.Z), new FastVec3d(targetVelocity.X, targetVelocity.Y, targetVelocity.Z), gravityAcceleration, speed);
		}

		// Token: 0x04000460 RID: 1120
		private const float _blocksPerSecondToBlocksPerMinute = 0.016666668f;

		// Token: 0x02000244 RID: 580
		public enum EnumArcCalculationType
		{
			// Token: 0x04000C41 RID: 3137
			Any,
			// Token: 0x04000C42 RID: 3138
			Full,
			// Token: 0x04000C43 RID: 3139
			Horizontal,
			// Token: 0x04000C44 RID: 3140
			Vertical
		}

		// Token: 0x02000245 RID: 581
		public readonly struct TargetData
		{
			// Token: 0x06001662 RID: 5730 RVA: 0x0006A0C7 File Offset: 0x000682C7
			public TargetData(FastVec3d shooterPosition, FastVec3d targetPosition, FastVec3d targetVelocity, double gravityAcceleration, double speed)
			{
				this.ShooterPosition = shooterPosition;
				this.TargetPosition = targetPosition;
				this.TargetVelocity = targetVelocity;
				this.GravityAcceleration = gravityAcceleration;
				this.Speed = speed;
			}

			// Token: 0x04000C45 RID: 3141
			public readonly FastVec3d ShooterPosition;

			// Token: 0x04000C46 RID: 3142
			public readonly FastVec3d TargetPosition;

			// Token: 0x04000C47 RID: 3143
			public readonly FastVec3d TargetVelocity;

			// Token: 0x04000C48 RID: 3144
			public readonly double GravityAcceleration;

			// Token: 0x04000C49 RID: 3145
			public readonly double Speed;
		}

		// Token: 0x02000246 RID: 582
		public readonly struct BallisticOutput
		{
			// Token: 0x06001663 RID: 5731 RVA: 0x0006A0EE File Offset: 0x000682EE
			public BallisticOutput(bool success, FastVec3d velocity, IBallisticSolver.EnumArcCalculationType calculationType)
			{
				this.Success = success;
				this.Velocity = velocity;
				this.ArcCalculationType = calculationType;
			}

			// Token: 0x04000C4A RID: 3146
			public readonly bool Success;

			// Token: 0x04000C4B RID: 3147
			public readonly FastVec3d Velocity;

			// Token: 0x04000C4C RID: 3148
			public readonly IBallisticSolver.EnumArcCalculationType ArcCalculationType;
		}

		// Token: 0x02000247 RID: 583
		[NullableContext(1)]
		[Nullable(0)]
		public readonly struct DispersionData
		{
			// Token: 0x06001664 RID: 5732 RVA: 0x0006A105 File Offset: 0x00068305
			public DispersionData(Angle yawDispersion, Angle pitchDispersion, NatFloat distribution)
			{
				this.YawDispersion = yawDispersion;
				this.PitchDispersion = pitchDispersion;
				this.Distribution = distribution;
			}

			// Token: 0x04000C4D RID: 3149
			public readonly Angle YawDispersion;

			// Token: 0x04000C4E RID: 3150
			public readonly Angle PitchDispersion;

			// Token: 0x04000C4F RID: 3151
			public readonly NatFloat Distribution;
		}
	}
}
