using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using OpenTK.Mathematics;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000BA RID: 186
	[NullableContext(1)]
	[Nullable(0)]
	public struct BallisticSolver : IBallisticSolver
	{
		// Token: 0x0600098D RID: 2445 RVA: 0x0003358C File Offset: 0x0003178C
		public IBallisticSolver.BallisticOutput SolveBallisticArc(IBallisticSolver.TargetData data)
		{
			Vector3d velocity;
			IBallisticSolver.EnumArcCalculationType calcType;
			return new IBallisticSolver.BallisticOutput(BallisticSolver.GetProjectilePositionAndVelocity(out velocity, data.ShooterPosition.ToOpenTK(), data.TargetPosition.ToOpenTK(), data.TargetVelocity.ToOpenTK(), data.GravityAcceleration, data.Speed, out calcType), velocity.ToVanilla(), calcType);
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x000335DB File Offset: 0x000317DB
		public void DrawBallisticArc(IBallisticSolver.BallisticOutput arcData, IBallisticSolver.TargetData targetData, IWorldAccessor world)
		{
			BallisticSolver.DrawBallisticArc(arcData.Velocity.ToOpenTK(), targetData.ShooterPosition.ToOpenTK(), targetData.TargetPosition.ToOpenTK(), targetData.GravityAcceleration, arcData.ArcCalculationType, world);
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x00033610 File Offset: 0x00031810
		public IBallisticSolver.BallisticOutput ApplyDispersion(IBallisticSolver.BallisticOutput targetData, IBallisticSolver.DispersionData dispersionData)
		{
			Vector3d direction = targetData.Velocity.ToOpenTK();
			double speed = direction.Length;
			Vector2 dispersion;
			dispersion..ctor(dispersionData.YawDispersion.Degrees, dispersionData.PitchDispersion.Degrees);
			direction = BallisticSolver.GetDirectionWithDispersion(Vector3d.Normalize(direction), dispersion, dispersionData.Distribution);
			return new IBallisticSolver.BallisticOutput(targetData.Success, (direction * speed).ToVanilla(), targetData.ArcCalculationType);
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00033680 File Offset: 0x00031880
		private static bool GetProjectilePositionAndVelocity(out Vector3d velocity, Vector3d shooterPos, Vector3d targetPos, Vector3d targetVelocity, double gravityAcceleration, double speed, out IBallisticSolver.EnumArcCalculationType calcType)
		{
			calcType = IBallisticSolver.EnumArcCalculationType.Any;
			velocity = default(Vector3d);
			Vector3d originalTargetPos = targetPos;
			float blocksPerSecondToBlocksPerMinute = 0.016666668f;
			speed *= (double)blocksPerSecondToBlocksPerMinute;
			Vector3d start;
			start..ctor(shooterPos.X, shooterPos.Y, shooterPos.Z);
			Vector3d target;
			target..ctor(targetPos.X, targetPos.Y, targetPos.Z);
			bool solvedBallisticArc = false;
			for (int triesCount = 0; triesCount < 30; triesCount++)
			{
				double time;
				solvedBallisticArc = BallisticSolver.SolveBallisticArc(out velocity, out time, start, target, speed, gravityAcceleration, out calcType, IBallisticSolver.EnumArcCalculationType.Any);
				if (targetVelocity.Length < 0.01)
				{
					if (solvedBallisticArc)
					{
						break;
					}
					speed *= 1.100000023841858;
				}
				else
				{
					targetPos += targetVelocity * time;
					target..ctor(targetPos.X, targetPos.Y, targetPos.Z);
					BallisticSolver.SolveBallisticArc(out velocity, out time, start, target, speed, gravityAcceleration, out calcType, IBallisticSolver.EnumArcCalculationType.Any);
					targetPos = originalTargetPos + targetVelocity * time;
					target..ctor(targetPos.X, targetPos.Y, targetPos.Z);
					double num;
					IBallisticSolver.EnumArcCalculationType resultCalcType;
					solvedBallisticArc = BallisticSolver.SolveBallisticArc(out velocity, out num, start, target, speed, gravityAcceleration, out resultCalcType, calcType);
					calcType = resultCalcType;
					if (solvedBallisticArc)
					{
						break;
					}
					speed *= 1.100000023841858;
				}
			}
			return solvedBallisticArc;
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x000337C4 File Offset: 0x000319C4
		private static void DrawBallisticArc(Vector3d velocity, Vector3d start, Vector3d target, double acceleration, IBallisticSolver.EnumArcCalculationType calcType, IWorldAccessor world)
		{
			int num;
			switch (calcType)
			{
			case IBallisticSolver.EnumArcCalculationType.Any:
				num = ColorUtil.ColorFromRgba(255, 255, 255, 255);
				break;
			case IBallisticSolver.EnumArcCalculationType.Full:
				num = ColorUtil.ColorFromRgba(0, 255, 0, 255);
				break;
			case IBallisticSolver.EnumArcCalculationType.Horizontal:
				num = ColorUtil.ColorFromRgba(255, 0, 255, 255);
				break;
			case IBallisticSolver.EnumArcCalculationType.Vertical:
				num = ColorUtil.ColorFromRgba(0, 255, 255, 255);
				break;
			default:
				num = ColorUtil.ColorFromRgba(255, 255, 255, 255);
				break;
			}
			int color = num;
			double cutoff = Math.Min(start.Y, target.Y) - 1.0;
			Vector3d position = start;
			double dt = 0.01;
			double time = 0.0;
			while (position.Y > cutoff)
			{
				time += dt;
				position += velocity * dt;
				velocity.Y -= acceleration * dt;
				if (time > 0.1)
				{
					time = 0.0;
					world.SpawnParticles(1f, color, new Vec3d(position.X, position.Y, position.Z), new Vec3d(position.X, position.Y, position.Z), new Vec3f(), new Vec3f(), 2f, 0f, 0.5f, 1, null);
				}
			}
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00033940 File Offset: 0x00031B40
		private static bool SolveBallisticArc(out Vector3d velocity, out double time, Vector3d start, Vector3d target, double speed, double g, out IBallisticSolver.EnumArcCalculationType calcType, IBallisticSolver.EnumArcCalculationType forceCalcType)
		{
			calcType = forceCalcType;
			switch (forceCalcType)
			{
			case IBallisticSolver.EnumArcCalculationType.Full:
				return BallisticSolver.SolveBallisticArcFullSpeed(out velocity, out time, start, target, speed, g);
			case IBallisticSolver.EnumArcCalculationType.Horizontal:
				return BallisticSolver.SolveBallisticArcHor(out velocity, out time, start, target, speed, g);
			case IBallisticSolver.EnumArcCalculationType.Vertical:
				return BallisticSolver.SolveBallisticArcVert(out velocity, out time, start, target, speed, g);
			default:
			{
				bool flag = BallisticSolver.SolveBallisticArcFullSpeed(out velocity, out time, start, target, speed, g);
				calcType = IBallisticSolver.EnumArcCalculationType.Full;
				if (flag)
				{
					return true;
				}
				Vector3d delta = target - start;
				Vector2d deltaXZ;
				deltaXZ..ctor(delta.X, delta.Z);
				double num = Math.Abs(delta.Y);
				double i = deltaXZ.Length / 2.0;
				if (num > i)
				{
					bool solved = BallisticSolver.SolveBallisticArcVert(out velocity, out time, start, target, speed, g);
					calcType = IBallisticSolver.EnumArcCalculationType.Vertical;
					if (!solved)
					{
						calcType = IBallisticSolver.EnumArcCalculationType.Horizontal;
						return BallisticSolver.SolveBallisticArcHor(out velocity, out time, start, target, speed, g);
					}
					return solved;
				}
				else
				{
					bool solved2 = BallisticSolver.SolveBallisticArcHor(out velocity, out time, start, target, speed, g);
					calcType = IBallisticSolver.EnumArcCalculationType.Horizontal;
					if (!solved2)
					{
						calcType = IBallisticSolver.EnumArcCalculationType.Vertical;
						return BallisticSolver.SolveBallisticArcVert(out velocity, out time, start, target, speed, g);
					}
					return solved2;
				}
				break;
			}
			}
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00033A38 File Offset: 0x00031C38
		private static bool SolveBallisticArcHor(out Vector3d velocity, out double time, Vector3d start, Vector3d target, double horizSpeed, double g)
		{
			Vector3d delta = target - start;
			Vector2d deltaXZ;
			deltaXZ..ctor(delta.X, delta.Z);
			double h = delta.Y;
			double i = deltaXZ.Length;
			time = i / horizSpeed;
			if (i < 1E-06)
			{
				velocity = Vector3d.Zero;
				return false;
			}
			double t = i / horizSpeed;
			double vY = h / t + 0.5 * g * t;
			deltaXZ = Vector2d.Normalize(deltaXZ) * horizSpeed;
			velocity = new Vector3d(deltaXZ.X, vY, deltaXZ.Y);
			return true;
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x00033AD4 File Offset: 0x00031CD4
		private static bool SolveBallisticArcVert(out Vector3d velocity, out double time, Vector3d start, Vector3d target, double vY, double g)
		{
			velocity = Vector3d.Zero;
			time = 0.0;
			Vector3d delta = target - start;
			Vector2d deltaXZ;
			deltaXZ..ctor(delta.X, delta.Z);
			double h = delta.Y;
			double i = deltaXZ.Length;
			double a = 0.5 * g;
			double b = -vY;
			double c = h;
			double discriminant = b * b - 4.0 * a * c;
			if (discriminant < 0.0)
			{
				return false;
			}
			double sqrtDisc = Math.Sqrt(discriminant);
			double t = (-b + sqrtDisc) / (2.0 * a);
			double t2 = (-b - sqrtDisc) / (2.0 * a);
			double t3 = double.NaN;
			if (t > 1E-06)
			{
				t3 = t;
			}
			if (t2 > 1E-06 && (double.IsNaN(t3) || t2 < t3))
			{
				t3 = t2;
			}
			if (double.IsNaN(t3))
			{
				return false;
			}
			time = t3;
			if (i < 1E-06)
			{
				velocity = new Vector3d(0.0, (double)((float)vY), 0.0);
				return true;
			}
			deltaXZ = Vector2d.Normalize(deltaXZ) * (double)((float)(i / t3));
			velocity = new Vector3d(deltaXZ.X, (double)((float)vY), deltaXZ.Y);
			return true;
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x00033C38 File Offset: 0x00031E38
		private static bool SolveBallisticArcFullSpeed(out Vector3d velocity, out double time, Vector3d start, Vector3d target, double speed, double acceleration)
		{
			velocity = Vector3d.Zero;
			Vector3d delta = target - start;
			Vector2d deltaXZ;
			deltaXZ..ctor(delta.X, delta.Z);
			double horizontalDist = deltaXZ.Length;
			double verticalDist = delta.Y;
			double speedSq = speed * speed;
			double discriminant = speedSq * speedSq - acceleration * (acceleration * horizontalDist * horizontalDist + 2.0 * verticalDist * speedSq);
			time = 0.0;
			if (discriminant < 0.0)
			{
				return false;
			}
			double sqrtDisc = Math.Sqrt(discriminant);
			double angle = Math.Atan2(speedSq - sqrtDisc, acceleration * horizontalDist);
			double vy = speed * Math.Sin(angle);
			double horizontalSpeed = speed * Math.Cos(angle);
			Vector2d vector2d = Vector2d.Normalize(deltaXZ);
			double vx = vector2d.X * horizontalSpeed;
			double vz = vector2d.Y * horizontalSpeed;
			velocity = new Vector3d(vx, vy, vz);
			time = deltaXZ.Length / horizontalSpeed;
			return true;
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x00033D28 File Offset: 0x00031F28
		private static Vector3d GetDirectionWithDispersion(Vector3d direction, Vector2 dispersionDeg, NatFloat random)
		{
			float randomYaw = random.nextFloat() * dispersionDeg.X * 0.017453292f;
			float randomPitch = random.nextFloat() * dispersionDeg.Y * 0.017453292f;
			Vector3d forwardAxis = Vector3d.Normalize(direction);
			Vector3d verticalAxis;
			verticalAxis..ctor(0.0, 0.0, 1.0);
			if (Math.Abs(Vector3d.Dot(forwardAxis, verticalAxis)) > 0.999999)
			{
				verticalAxis..ctor(0.0, 1.0, 0.0);
			}
			Vector3d vector3d = Vector3d.Normalize(Vector3d.Cross(forwardAxis, verticalAxis));
			Vector3d pitchAxis = Vector3d.Normalize(Vector3d.Cross(vector3d, forwardAxis));
			Vector3d yawComponent = vector3d * Math.Tan((double)randomYaw);
			Vector3d pitchComponent = pitchAxis * Math.Tan((double)randomPitch);
			return Vector3d.Normalize(forwardAxis + yawComponent + pitchComponent);
		}
	}
}
