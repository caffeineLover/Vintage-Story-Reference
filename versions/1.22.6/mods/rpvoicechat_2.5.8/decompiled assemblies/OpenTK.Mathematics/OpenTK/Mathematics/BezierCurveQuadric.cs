using System;

namespace OpenTK.Mathematics
{
	// Token: 0x02000008 RID: 8
	[Serializable]
	public struct BezierCurveQuadric
	{
		// Token: 0x06000154 RID: 340 RVA: 0x0000626B File Offset: 0x0000446B
		public BezierCurveQuadric(Vector2 startAnchor, Vector2 endAnchor, Vector2 controlPoint)
		{
			this.StartAnchor = startAnchor;
			this.EndAnchor = endAnchor;
			this.ControlPoint = controlPoint;
			this.Parallel = 0f;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000628D File Offset: 0x0000448D
		public BezierCurveQuadric(float parallel, Vector2 startAnchor, Vector2 endAnchor, Vector2 controlPoint)
		{
			this.Parallel = parallel;
			this.StartAnchor = startAnchor;
			this.EndAnchor = endAnchor;
			this.ControlPoint = controlPoint;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x000062AC File Offset: 0x000044AC
		public Vector2 CalculatePoint(float t)
		{
			float num = 1f - t;
			Vector2 vector = new Vector2(num * num * this.StartAnchor.X + 2f * t * num * this.ControlPoint.X + t * t * this.EndAnchor.X, num * num * this.StartAnchor.Y + 2f * t * num * this.ControlPoint.Y + t * t * this.EndAnchor.Y);
			if (this.Parallel == 0f)
			{
				return vector;
			}
			Vector2 vec;
			if (t == 0f)
			{
				vec = this.ControlPoint - this.StartAnchor;
			}
			else
			{
				vec = vector - this.CalculatePointOfDerivative(t);
			}
			return vector + Vector2.Normalize(vec).PerpendicularRight * this.Parallel;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000638C File Offset: 0x0000458C
		private Vector2 CalculatePointOfDerivative(float t)
		{
			return new Vector2
			{
				X = (1f - t) * this.StartAnchor.X + t * this.ControlPoint.X,
				Y = (1f - t) * this.StartAnchor.Y + t * this.ControlPoint.Y
			};
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000063F4 File Offset: 0x000045F4
		public float CalculateLength(float precision)
		{
			float num = 0f;
			Vector2 right = this.CalculatePoint(0f);
			for (float num2 = precision; num2 < 1f + precision; num2 += precision)
			{
				Vector2 vector = this.CalculatePoint(num2);
				num += (vector - right).Length;
				right = vector;
			}
			return num;
		}

		// Token: 0x04000018 RID: 24
		public Vector2 StartAnchor;

		// Token: 0x04000019 RID: 25
		public Vector2 EndAnchor;

		// Token: 0x0400001A RID: 26
		public Vector2 ControlPoint;

		// Token: 0x0400001B RID: 27
		public float Parallel;
	}
}
