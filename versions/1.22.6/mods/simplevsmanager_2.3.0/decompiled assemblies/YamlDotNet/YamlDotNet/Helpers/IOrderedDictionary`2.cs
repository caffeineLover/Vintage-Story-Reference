using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Helpers
{
	// Token: 0x020000BA RID: 186
	[NullableContext(1)]
	public interface IOrderedDictionary<TKey, [Nullable(2)] TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
		// Token: 0x1700006D RID: 109
		[Nullable(new byte[]
		{
			0,
			1,
			1
		})]
		KeyValuePair<TKey, TValue> this[int index]
		{
			[return: Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			get;
			[param: Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			set;
		}

		// Token: 0x06000458 RID: 1112
		void Insert(int index, TKey key, TValue value);

		// Token: 0x06000459 RID: 1113
		void RemoveAt(int index);
	}
}
