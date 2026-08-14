using System;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
	// Token: 0x0200000A RID: 10
	[NullableContext(1)]
	internal interface IDictionary2<[Nullable(0)] in TKey, [Nullable(2)] out TValue> : IDictionary2<TKey>, IDictionary2 where TKey : IEquatable<TKey> where TValue : class
	{
		// Token: 0x17000008 RID: 8
		TValue this[TKey key]
		{
			get;
		}
	}
}
