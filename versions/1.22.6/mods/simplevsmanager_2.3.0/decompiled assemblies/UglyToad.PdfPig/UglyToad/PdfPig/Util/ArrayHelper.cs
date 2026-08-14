using System;

namespace UglyToad.PdfPig.Util
{
	// Token: 0x02000026 RID: 38
	internal static class ArrayHelper
	{
		// Token: 0x06000113 RID: 275 RVA: 0x0000A620 File Offset: 0x00008820
		public static void Fill<T>(T[] array, int start, int end, T value)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (start < 0 || start >= end)
			{
				throw new ArgumentOutOfRangeException("start");
			}
			if (end >= array.Length)
			{
				throw new ArgumentOutOfRangeException("end");
			}
			for (int i = start; i < end; i++)
			{
				array[i] = value;
			}
		}
	}
}
