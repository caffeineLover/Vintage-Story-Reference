using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.Collections
{
	// Token: 0x0200003A RID: 58
	public interface IReadOnlyObservableGroup : INotifyPropertyChanged, INotifyCollectionChanged, IEnumerable
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000172 RID: 370
		[Nullable(1)]
		object Key { [NullableContext(1)] get; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000173 RID: 371
		int Count { get; }

		// Token: 0x17000035 RID: 53
		[Nullable(2)]
		object this[int index]
		{
			[NullableContext(2)]
			get;
		}
	}
}
