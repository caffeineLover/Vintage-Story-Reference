using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Helpers
{
	// Token: 0x020000BE RID: 190
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ReadOnlyCollectionExtensions
	{
		// Token: 0x0600047C RID: 1148 RVA: 0x0000EB5C File Offset: 0x0000CD5C
		public static IReadOnlyList<T> AsReadonlyList<[Nullable(2)] T>(this List<T> list)
		{
			return list;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0000EB5F File Offset: 0x0000CD5F
		public static IReadOnlyDictionary<TKey, TValue> AsReadonlyDictionary<TKey, [Nullable(2)] TValue>(this Dictionary<TKey, TValue> dictionary)
		{
			return dictionary;
		}
	}
}
