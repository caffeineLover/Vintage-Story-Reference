using System;

namespace MS.Internal
{
	// Token: 0x02000209 RID: 521
	internal static class DoubleUtil
	{
		// Token: 0x06000F29 RID: 3881 RVA: 0x0002B962 File Offset: 0x00029B62
		public static int DoubleToInt(double val)
		{
			if (0.0 >= val)
			{
				return (int)(val - 0.5);
			}
			return (int)(val + 0.5);
		}
	}
}
