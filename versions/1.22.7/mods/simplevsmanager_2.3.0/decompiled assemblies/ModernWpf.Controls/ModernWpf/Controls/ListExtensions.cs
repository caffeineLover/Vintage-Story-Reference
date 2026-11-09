using System;
using System.Collections.Generic;
using System.Linq;

namespace ModernWpf.Controls
{
	// Token: 0x0200002D RID: 45
	internal static class ListExtensions
	{
		// Token: 0x060001AB RID: 427 RVA: 0x000064A8 File Offset: 0x000046A8
		public static void Resize<T>(this List<T> list, int size, T element = default(T))
		{
			int count = list.Count;
			if (size < count)
			{
				list.RemoveRange(size, count - size);
				return;
			}
			if (size > count)
			{
				if (size > list.Capacity)
				{
					list.Capacity = size;
				}
				list.AddRange(Enumerable.Repeat<T>(element, size - count));
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000064EE File Offset: 0x000046EE
		public static T Last<T>(this List<T> list)
		{
			return list[list.Count - 1];
		}

		// Token: 0x060001AD RID: 429 RVA: 0x000064FE File Offset: 0x000046FE
		public static void RemoveLast<T>(this List<T> list)
		{
			list.RemoveAt(list.Count - 1);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000650E File Offset: 0x0000470E
		public static bool Empty<T>(this List<T> list)
		{
			return list.Count == 0;
		}
	}
}
