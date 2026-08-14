using System;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
	// Token: 0x0200000B RID: 11
	internal interface IDictionary2<in TKey> : IDictionary2 where TKey : IEquatable<TKey>
	{
		// Token: 0x06000030 RID: 48
		[NullableContext(1)]
		bool TryRemove(TKey key);
	}
}
