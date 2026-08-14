using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.PdfFonts.Cmap
{
	// Token: 0x020000B5 RID: 181
	internal static class CMapUtils
	{
		// Token: 0x0600034B RID: 843 RVA: 0x0001522C File Offset: 0x0001342C
		public static int ToInt(this IReadOnlyList<byte> data, int length)
		{
			int num = 0;
			for (int i = 0; i < length; i++)
			{
				num <<= 8;
				num |= (int)(data[i] & byte.MaxValue);
			}
			return num;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0001525C File Offset: 0x0001345C
		public static void PutAll<TKey, TValue>(this Dictionary<TKey, TValue> target, IReadOnlyDictionary<TKey, TValue> source)
		{
			foreach (KeyValuePair<TKey, TValue> keyValuePair in source)
			{
				target[keyValuePair.Key] = keyValuePair.Value;
			}
		}
	}
}
