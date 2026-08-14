using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.Collections
{
	// Token: 0x0200003C RID: 60
	public interface IReadOnlyObservableGroup<[Nullable(1)] out TKey> : IReadOnlyObservableGroup, INotifyPropertyChanged, INotifyCollectionChanged, IEnumerable
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000176 RID: 374
		[Nullable(1)]
		TKey Key { [NullableContext(1)] get; }
	}
}
