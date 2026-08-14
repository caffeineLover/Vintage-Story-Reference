using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.Collections
{
	// Token: 0x0200003B RID: 59
	public interface IReadOnlyObservableGroup<[Nullable(1)] out TKey, [Nullable(2)] out TElement> : IReadOnlyObservableGroup<TKey>, IReadOnlyObservableGroup, INotifyPropertyChanged, INotifyCollectionChanged, IEnumerable, IReadOnlyList<TElement>, IEnumerable<TElement>, IReadOnlyCollection<TElement>, IGrouping<TKey, TElement>
	{
		// Token: 0x17000036 RID: 54
		[Nullable(1)]
		TElement this[int index]
		{
			[NullableContext(1)]
			get;
		}
	}
}
