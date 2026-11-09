using System;
using System.Numerics;

namespace QuestPDF.Skia
{
	// Token: 0x02000009 RID: 9
	internal struct SkCanvasMatrix
	{
		// Token: 0x06000045 RID: 69 RVA: 0x000026D4 File Offset: 0x000008D4
		public Matrix4x4 ToMatrix4x4()
		{
			return new Matrix4x4(this.ScaleX, this.SkewY, 0f, 0f, this.SkewX, this.ScaleY, 0f, 0f, 0f, 0f, this.Perspective3, 0f, this.TranslateX, this.TranslateY, 0f, 1f);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002740 File Offset: 0x00000940
		public static SkCanvasMatrix FromMatrix4x4(Matrix4x4 matrix)
		{
			return new SkCanvasMatrix
			{
				ScaleX = matrix.M11,
				SkewX = matrix.M21,
				TranslateX = matrix.M41,
				SkewY = matrix.M12,
				ScaleY = matrix.M22,
				TranslateY = matrix.M42,
				Perspective1 = 0f,
				Perspective2 = 0f,
				Perspective3 = 1f
			};
		}

		// Token: 0x04000015 RID: 21
		public float ScaleX;

		// Token: 0x04000016 RID: 22
		public float SkewX;

		// Token: 0x04000017 RID: 23
		public float TranslateX;

		// Token: 0x04000018 RID: 24
		public float SkewY;

		// Token: 0x04000019 RID: 25
		public float ScaleY;

		// Token: 0x0400001A RID: 26
		public float TranslateY;

		// Token: 0x0400001B RID: 27
		public float Perspective1;

		// Token: 0x0400001C RID: 28
		public float Perspective2;

		// Token: 0x0400001D RID: 29
		public float Perspective3;

		// Token: 0x0400001E RID: 30
		public static readonly SkCanvasMatrix Identity = SkCanvasMatrix.FromMatrix4x4(Matrix4x4.Identity);
	}
}
