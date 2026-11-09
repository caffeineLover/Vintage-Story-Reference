using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.Collections
{
	// Token: 0x0200003E RID: 62
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1,
		1,
		1
	})]
	public sealed class ObservableGroupedCollection<TKey, [Nullable(2)] TElement> : ObservableCollection<ObservableGroup<TKey, TElement>>, ILookup<TKey, TElement>, IEnumerable<IGrouping<TKey, TElement>>, IEnumerable
	{
		// Token: 0x06000193 RID: 403 RVA: 0x0000759C File Offset: 0x0000579C
		public ObservableGroupedCollection()
		{
		}

		// Token: 0x06000194 RID: 404 RVA: 0x000075A4 File Offset: 0x000057A4
		public ObservableGroupedCollection(IEnumerable<IGrouping<TKey, TElement>> collection)
		{
			IEnumerable<ObservableGroup<TKey, TElement>> enumerable;
			if (collection == null)
			{
				enumerable = null;
			}
			else
			{
				Func<IGrouping<TKey, TElement>, ObservableGroup<TKey, TElement>> func;
				if ((func = ObservableGroupedCollection<TKey, TElement>.<>c.<>9__1_0) == null)
				{
					func = (ObservableGroupedCollection<TKey, TElement>.<>c.<>9__1_0 = new Func<IGrouping<TKey, TElement>, ObservableGroup<TKey, TElement>>(ObservableGroupedCollection<TKey, TElement>.<>c.<>9.<.ctor>b__1_0));
				}
				enumerable = Enumerable.Select<IGrouping<TKey, TElement>, ObservableGroup<TKey, TElement>>(collection, func);
			}
			base..ctor(enumerable);
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000195 RID: 405 RVA: 0x000075D8 File Offset: 0x000057D8
		IEnumerable<TElement> ILookup<!0, !1>.Item
		{
			get
			{
				IEnumerable<TElement> enumerable = null;
				if (key != null)
				{
					enumerable = this.FirstGroupByKeyOrDefault(key);
				}
				return enumerable ?? Enumerable.Empty<TElement>();
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00007601 File Offset: 0x00005801
		[MethodImpl(256)]
		internal bool TryGetList([Nullable(new byte[]
		{
			2,
			1,
			1,
			1
		})] [NotNullWhen(true)] out List<ObservableGroup<TKey, TElement>> list)
		{
			list = (base.Items as List<ObservableGroup<TKey, TElement>>);
			return list != null;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00007618 File Offset: 0x00005818
		bool ILookup<!0, !1>.Contains(TKey key)
		{
			return key != null && this.FirstGroupByKey(key) != null;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00007631 File Offset: 0x00005831
		IEnumerator<IGrouping<TKey, TElement>> IEnumerable<IGrouping<!0, !1>>.GetEnumerator()
		{
			return base.GetEnumerator();
		}
	}
}
