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

		// Token: 0x0600098C RID: 2444 RVA: 0x000334B8 File Offset: 0x000316B8
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

		// Token: 0x02000252 RID: 594
		public enum EnumArcCalculationType
		{
			// Token: 0x04000C69 RID: 3177
			Any,
			// Token: 0x04000C6A RID: 3178
			Full,
			// Token: 0x04000C6B RID: 3179
			Horizontal,
			// Token: 0x04000C6C RID: 3180
			Vertical
		}

		// Token: 0x02000253 RID: 595
		public readonly struct TargetData
		{
			// Token: 0x060016DA RID: 5850 RVA: 0x0006BB6F File Offset: 0x00069D6F
			public TargetData(FastVec3d shooterPosition, FastVec3d targetPosition, FastVec3d targetVelocity, double gravityAcceleration, double speed)
			{
				this.ShooterPosition = shooterPosition;
				this.TargetPosition = targetPosition;
				this.TargetVelocity = targetVelocity;
				this.GravityAcceleration = gravityAcceleration;
				this.Speed = speed;
			}

			// Token: 0x04000C6D RID: 3181
			public readonly FastVec3d ShooterPosition;

			// Token: 0x04000C6E RID: 3182
			public readonly FastVec3d TargetPosition;

			// Token: 0x04000C6F RID: 3183
			public readonly FastVec3d TargetVelocity;

			// Token: 0x04000C70 RID: 3184
			public readonly double GravityAcceleration;

			// Token: 0x04000C71 RID: 3185
			public readonly double Speed;
		}

		// Token: 0x02000254 RID: 596
		public readonly struct BallisticOutput
		{
			// Token: 0x060016DB RID: 5851 RVA: 0x0006BB96 File Offset: 0x00069D96
			public BallisticOutput(bool success, FastVec3d velocity, IBallisticSolver.EnumArcCalculationType calculationType)
			{
				this.Success = success;
				this.Velocity = velocity;
				this.ArcCalculationType = calculationType;
			}

			// Token: 0x04000C72 RID: 3186
			public readonly bool Success;

			// Token: 0x04000C73 RID: 3187
			public readonly FastVec3d Velocity;

			// Token: 0x04000C74 RID: 3188
			public readonly IBallisticSolver.EnumArcCalculationType ArcCalculationType;
		}

		// Token: 0x02000255 RID: 597
		[NullableContext(1)]
		[Nullable(0)]
		public readonly struct DispersionData
		{
			// Token: 0x060016DC RID: 5852 RVA: 0x0006BBAD File Offset: 0x00069DAD
			public DispersionData(Angle yawDispersion, Angle pitchDispersion, NatFloat distribution)
			{
				this.YawDispersion = yawDispersion;
				this.PitchDispersion = pitchDispersion;
				this.Distribution = distribution;
			}

			// Token: 0x04000C75 RID: 3189
			public readonly Angle YawDispersion;

			// Token: 0x04000C76 RID: 3190
			public readonly Angle PitchDispersion;

			// Token: 0x04000C77 RID: 3191
			public readonly NatFloat Distribution;
		}
	}
}
