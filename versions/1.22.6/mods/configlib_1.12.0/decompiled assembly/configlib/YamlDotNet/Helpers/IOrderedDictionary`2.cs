using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Helpers
{
	// Token: 0x0200019A RID: 410
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

		// Token: 0x060008D0 RID: 2256
		void Insert(int index, TKey key, TValue value);

		// Token: 0x060008D1 RID: 2257
		void RemoveAt(int index);
	}
}
