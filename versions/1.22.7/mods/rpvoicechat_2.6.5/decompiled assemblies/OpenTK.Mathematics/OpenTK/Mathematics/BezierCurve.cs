using System;
using System.Collections.Generic;

namespace OpenTK.Mathematics
{
	// Token: 0x02000006 RID: 6
	[Serializable]
	public struct BezierCurve
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00005D1F File Offset: 0x00003F1F
		public readonly IList<Vector2> Points
		{
			get
			{
				return this._points;
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00005D27 File Offset: 0x00003F27
		public BezierCurve(IEnumerable<Vector2> points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points", "Must point to a valid list of Vector2 structures.");
			}
			this._points = new List<Vector2>(points);
			this.Parallel = 0f;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00005D53 File Offset: 0x00003F53
		public BezierCurve(params Vector2[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points", "Must point to a valid list of Vector2 structures.");
			}
			this._points = new List<Vector2>(points);
			this.Parallel = 0f;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00005D7F File Offset: 0x00003F7F
		public BezierCurve(float parallel, params Vector2[] points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points", "Must point to a valid list of Vector2 structures.");
			}
			this.Parallel = parallel;
			this._points = new List<Vector2>(points);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00005DA7 File Offset: 0x00003FA7
		public BezierCurve(float parallel, IEnumerable<Vector2> points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points", "Must point to a valid list of Vector2 structures.");
			}
			this.Parallel = parallel;
			this._points = new List<Vector2>(points);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005DCF File Offset: 0x00003FCF
		public readonly Vector2 CalculatePoint(float t)
		{
			return BezierCurve.CalculatePoint(this._points, t, this.Parallel);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00005DE3 File Offset: 0x00003FE3
		public readonly float CalculateLength(float precision)
		{
			return BezierCurve.CalculateLength(this._points, precision, this.Parallel);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005DF7 File Offset: 0x00003FF7
		public static float CalculateLength(IList<Vector2> points, float precision)
		{
			return BezierCurve.CalculateLength(points, precision, 0f);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00005E08 File Offset: 0x00004008
		public static float CalculateLength(IList<Vector2> points, float precision, float parallel)
		{
			float num = 0f;
			Vector2 right = BezierCurve.CalculatePoint(points, 0f, parallel);
			for (float num2 = precision; num2 < 1f + precision; num2 += precision)
			{
				Vector2 vector = BezierCurve.CalculatePoint(points, num2, parallel);
				num += (vector - right).Length;
				right = vector;
			}
			return num;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00005E59 File Offset: 0x00004059
		public static Vector2 CalculatePoint(IList<Vector2> points, float t)
		{
			return BezierCurve.CalculatePoint(points, t, 0f);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00005E68 File Offset: 0x00004068
		public static Vector2 CalculatePoint(IList<Vector2> points, float t, float parallel)
		{
			Vector2 vector = default(Vector2);
			float num = 1f - t;
			int num2 = 0;
			foreach (Vector2 vector2 in points)
			{
				float num3 = (float)MathHelper.BinomialCoefficient(points.Count - 1, num2) * (MathF.Pow(t, (float)num2) * MathF.Pow(num, (float)(points.Count - 1 - num2)));
				vector.X += num3 * vector2.X;
				vector.Y += num3 * vector2.Y;
				num2++;
			}
			if (parallel == 0f)
			{
				return vector;
			}
			Vector2 vec;
			if (t != 0f)
			{
				vec = vector - BezierCurve.CalculatePointOfDerivative(points, t);
			}
			else
			{
				vec = points[1] - points[0];
			}
			return vector + Vector2.Normalize(vec).PerpendicularRight * parallel;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00005F6C File Offset: 0x0000416C
		private static Vector2 CalculatePointOfDerivative(IList<Vector2> points, float t)
		{
			Vector2 result = default(Vector2);
			float num = 1f - t;
			int num2 = 0;
			foreach (Vector2 vector in points)
			{
				float num3 = (float)MathHelper.BinomialCoefficient(points.Count - 2, num2) * (MathF.Pow(t, (float)num2) * MathF.Pow(num, (float)(points.Count - 2 - num2)));
				result.X += num3 * vector.X;
				result.Y += num3 * vector.Y;
				num2++;
			}
			return result;
		}

		// Token: 0x04000011 RID: 17
		private readonly List<Vector2> _points;

		// Token: 0x04000012 RID: 18
		public float Parallel;
	}
}
