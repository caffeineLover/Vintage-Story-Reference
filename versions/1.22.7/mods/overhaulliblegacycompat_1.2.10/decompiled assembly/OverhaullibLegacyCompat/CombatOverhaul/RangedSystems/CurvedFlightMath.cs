using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000C9 RID: 201
	[NullableContext(1)]
	[Nullable(0)]
	internal static class CurvedFlightMath
	{
		// Token: 0x06000A59 RID: 2649 RVA: 0x00036D10 File Offset: 0x00034F10
		internal static void RotateHorizontal(Vec3d motion, float turnRate, int direction, float dt)
		{
			if (turnRate <= 0f || direction == 0 || dt <= 0f)
			{
				return;
			}
			if (motion.X * motion.X + motion.Z * motion.Z <= 1E-12)
			{
				return;
			}
			double num = (double)(turnRate * (float)Math.Sign(direction) * dt);
			double cosine = Math.Cos(num);
			double sine = Math.Sin(num);
			double x = motion.X;
			double z = motion.Z;
			motion.X = x * cosine + z * sine;
			motion.Z = -x * sine + z * cosine;
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x00036D9C File Offset: 0x00034F9C
		internal static void SteerToward(Vec3d motion, Vec3d desiredVelocity, double maximumDelta)
		{
			if (maximumDelta <= 0.0)
			{
				return;
			}
			Vec3d delta = desiredVelocity.SubCopy(motion);
			double deltaLength = delta.Length();
			if (deltaLength <= 1E-12)
			{
				return;
			}
			if (deltaLength > maximumDelta)
			{
				delta.Mul(maximumDelta / deltaLength);
			}
			motion.Add(delta);
		}
	}
}
