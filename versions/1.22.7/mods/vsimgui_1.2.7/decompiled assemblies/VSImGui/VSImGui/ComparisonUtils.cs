using System;

namespace VSImGui
{
	// Token: 0x0200000E RID: 14
	internal static class ComparisonUtils
	{
		// Token: 0x0600018C RID: 396 RVA: 0x0000ACC3 File Offset: 0x00008EC3
		public static bool CompareFloats(float first, float second)
		{
			return MathF.Abs(first - second) < 1E-09f;
		}

		// Token: 0x04000032 RID: 50
		public const float Epsilon = 1E-09f;
	}
}
