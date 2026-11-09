using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Helpers
{
	// Token: 0x02000196 RID: 406
	[NullableContext(1)]
	public interface IOrderedDictionary<TKey, [Nullable(2)] TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
		// Token: 0x170000FA RID: 250
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

		// Token: 0x06000894 RID: 2196
		void Insert(int index, TKey key, TValue value);

		// Token: 0x06000895 RID: 2197
		void RemoveAt(int index);
	}
}
