using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Xabe.FFmpeg.Streams
{
	// Token: 0x0200003E RID: 62
	internal class ParametersList<T> : IEnumerable<T>, IEnumerable
	{
		// Token: 0x06000206 RID: 518 RVA: 0x00006DB8 File Offset: 0x00004FB8
		public IEnumerator<T> GetEnumerator()
		{
			IEnumerable<KeyValuePair<T, T>> items = this._items;
			Func<KeyValuePair<T, T>, T> func;
			if ((func = ParametersList<T>.<>c.<>9__1_0) == null)
			{
				func = (ParametersList<T>.<>c.<>9__1_0 = new Func<KeyValuePair<T, T>, T>(ParametersList<T>.<>c.<>9.<GetEnumerator>b__1_0));
			}
			return Enumerable.Select<KeyValuePair<T, T>, T>(items, func).GetEnumerator();
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00006DE9 File Offset: 0x00004FE9
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00006DF1 File Offset: 0x00004FF1
		internal void Add(T item)
		{
			this._items[item] = item;
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00006E00 File Offset: 0x00005000
		internal void Remove(T item)
		{
			if (this._items.ContainsKey(item))
			{
				this._items.Remove(item);
			}
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00006E1D File Offset: 0x0000501D
		internal void Clear()
		{
			this._items.Clear();
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00006E2A File Offset: 0x0000502A
		public ParametersList()
		{
			this._items = new Dictionary<T, T>();
			base..ctor();
		}

		// Token: 0x040004FB RID: 1275
		private readonly Dictionary<T, T> _items;
	}
}
