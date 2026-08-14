using System;
using System.Collections.Generic;

namespace ModernWpf.Controls
{
	// Token: 0x02000027 RID: 39
	internal static class CollectionHelper
	{
		// Token: 0x0600019F RID: 415 RVA: 0x0000643D File Offset: 0x0000463D
		public static bool contains<T>(IList<T> c, T v)
		{
			return c.Contains(v);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00006446 File Offset: 0x00004646
		public static void unique_push_back<T>(IList<T> c, T v)
		{
			if (!c.Contains(v))
			{
				c.Add(v);
			}
		}
	}
}
