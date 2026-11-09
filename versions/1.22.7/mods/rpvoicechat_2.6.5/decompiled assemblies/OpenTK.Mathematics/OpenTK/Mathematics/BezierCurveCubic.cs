using System;

namespace OpenTK.Mathematics
{
	// Token: 0x02000007 RID: 7
	[Serializable]
	public struct BezierCurveCubic
	{
		// Token: 0x0600014F RID: 335 RVA: 0x00006020 File Offset: 0x00004220
		public BezierCurveCubic(Vector2 startAnchor, Vector2 endAnchor, Vector2 firstControlPoint, Vector2 secondControlPoint)
		{
			this.StartAnchor = startAnchor;
			this.EndAnchor = endAnchor;
			this.FirstControlPoint = firstControlPoint;
			this.SecondControlPoint = secondControlPoint;
			this.Parallel = 0f;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000604A File Offset: 0x0000424A
		public BezierCurveCubic(float parallel, Vector2 startAnchor, Vector2 endAnchor, Vector2 firstControlPoint, Vector2 secondControlPoint)
		{
			this.Parallel = parallel;
			this.StartAnchor = startAnchor;
			this.EndAnchor = endAnchor;
			this.FirstControlPoint = firstControlPoint;
			this.SecondControlPoint = secondControlPoint;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00006074 File Offset: 0x00004274
		public Vector2 CalculatePoint(float t)
		{
			float num = 1f - t;
			float x = this.StartAnchor.X * num * num * num + this.FirstControlPoint.X * 3f * t * num * num + this.SecondControlPoint.X * 3f * t * t * num + this.EndAnchor.X * t * t * t;
			float y = this.StartAnchor.Y * num * num * num + this.FirstControlPoint.Y * 3f * t * num * num + this.SecondControlPoint.Y * 3f * t * t * num + this.EndAnchor.Y * t * t * t;
			Vector2 vector = new Vector2(x, y);
			if (this.Parallel == 0f)
			{
				return vector;
			}
			Vector2 vec;
			if (t == 0f)
			{
				vec = this.FirstControlPoint - this.StartAnchor;
			}
			else
			{
				vec = vector - this.CalculatePointOfDerivative(t);
			}
			return vector + Vector2.Normalize(vec).PerpendicularRight * this.Parallel;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00006198 File Offset: 0x00004398
		private readonly Vector2 CalculatePointOfDerivative(float t)
		{
			float num = 1f - t;
			return new Vector2(num * num * this.StartAnchor.X + 2f * t * num * this.FirstControlPoint.X + t * t * this.SecondControlPoint.X, num * num * this.StartAnchor.Y + 2f * t * num * this.FirstControlPoint.Y + t * t * this.SecondControlPoint.Y);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000621C File Offset: 0x0000441C
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

		// Token: 0x04000013 RID: 19
		public Vector2 StartAnchor;

		// Token: 0x04000014 RID: 20
		public Vector2 EndAnchor;

		// Token: 0x04000015 RID: 21
		public Vector2 FirstControlPoint;

		// Token: 0x04000016 RID: 22
		public Vector2 SecondControlPoint;

		// Token: 0x04000017 RID: 23
		public float Parallel;
	}
}
