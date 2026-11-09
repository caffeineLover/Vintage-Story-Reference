using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000CA RID: 202
	internal static class OvalFlightMath
	{
		// Token: 0x06000A5B RID: 2651 RVA: 0x00036DE8 File Offset: 0x00034FE8
		internal static void Sample(float maximumDistance, float lateralRadius, double progress, out double forward, out double lateral)
		{
			double angle = GameMath.Clamp(progress, 0.0, 1.0) * 3.141592653589793 * 2.0;
			double forwardRadius = (double)(Math.Max(0f, maximumDistance) / 2f);
			forward = forwardRadius * (1.0 - Math.Cos(angle));
			lateral = (double)Math.Max(0f, lateralRadius) * Math.Sin(angle);
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x00036E60 File Offset: 0x00035060
		[NullableContext(1)]
		internal static bool SegmentIntersectsAabb(Vec3d start, Vec3d end, double minX, double minY, double minZ, double maxX, double maxY, double maxZ)
		{
			double tMin = 0.0;
			double tMax = 1.0;
			return OvalFlightMath.IntersectsSlab(start.X, end.X - start.X, minX, maxX, ref tMin, ref tMax) && OvalFlightMath.IntersectsSlab(start.Y, end.Y - start.Y, minY, maxY, ref tMin, ref tMax) && OvalFlightMath.IntersectsSlab(start.Z, end.Z - start.Z, minZ, maxZ, ref tMin, ref tMax);
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x00036EE8 File Offset: 0x000350E8
		private static bool IntersectsSlab(double start, double delta, double minimum, double maximum, ref double tMin, ref double tMax)
		{
			if (Math.Abs(delta) <= 1E-12)
			{
				return start >= minimum && start <= maximum;
			}
			double inverse = 1.0 / delta;
			double near = (minimum - start) * inverse;
			double far = (maximum - start) * inverse;
			if (near > far)
			{
				double num = far;
				far = near;
				near = num;
			}
			tMin = Math.Max(tMin, near);
			tMax = Math.Min(tMax, far);
			return tMin <= tMax;
		}
	}
}
