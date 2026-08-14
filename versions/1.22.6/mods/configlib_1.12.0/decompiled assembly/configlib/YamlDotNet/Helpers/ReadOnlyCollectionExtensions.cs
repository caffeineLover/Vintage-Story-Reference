using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Helpers
{
	// Token: 0x020001A7 RID: 423
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ReadOnlyCollectionExtensions
	{
		// Token: 0x06000912 RID: 2322 RVA: 0x0000E3B0 File Offset: 0x0000C5B0
		public static IReadOnlyList<T> AsReadonlyList<[Nullable(2)] T>(this List<T> list)
		{
			return list;
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0000E3B0 File Offset: 0x0000C5B0
		public static IReadOnlyDictionary<TKey, TValue> AsReadonlyDictionary<TKey, [Nullable(2)] TValue>(this Dictionary<TKey, TValue> dictionary)
		{
			return dictionary;
		}
	}
}
