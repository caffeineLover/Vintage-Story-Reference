using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x02000037 RID: 55
	internal static class ReasonListExtensions
	{
		// Token: 0x060004FB RID: 1275 RVA: 0x00023714 File Offset: 0x00021914
		[NullableContext(1)]
		public static int IndexOf(this IReadOnlyList<string> source, string value)
		{
			for (int i = 0; i < source.Count; i++)
			{
				if (string.Equals(source[i], value, 4))
				{
					return i;
				}
			}
			return -1;
		}
	}
}
