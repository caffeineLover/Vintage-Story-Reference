using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Helpers
{
	// Token: 0x020001A3 RID: 419
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ReadOnlyCollectionExtensions
	{
		// Token: 0x060008D6 RID: 2262 RVA: 0x00002072 File Offset: 0x00000272
		public static IReadOnlyList<T> AsReadonlyList<[Nullable(2)] T>(this List<T> list)
		{
			return list;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x00002072 File Offset: 0x00000272
		public static IReadOnlyDictionary<TKey, TValue> AsReadonlyDictionary<TKey, [Nullable(2)] TValue>(this Dictionary<TKey, TValue> dictionary)
		{
			return dictionary;
		}
	}
}
