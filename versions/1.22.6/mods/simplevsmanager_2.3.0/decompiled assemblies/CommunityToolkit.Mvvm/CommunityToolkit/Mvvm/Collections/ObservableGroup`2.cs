using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Collections.Internals;

namespace CommunityToolkit.Mvvm.Collections
{
	// Token: 0x0200003F RID: 63
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1
	})]
	[DebuggerDisplay("Key = {Key}, Count = {Count}")]
	public sealed class ObservableGroup<TKey, [Nullable(2)] TElement> : ObservableCollection<TElement>, IReadOnlyObservableGroup<TKey, TElement>, IReadOnlyObservableGroup<TKey>, IReadOnlyObservableGroup, INotifyPropertyChanged, INotifyCollectionChanged, IEnumerable, IReadOnlyList<TElement>, IEnumerable<TElement>, IReadOnlyCollection<TElement>, IGrouping<TKey, TElement>
	{
		// Token: 0x06000199 RID: 409 RVA: 0x00007639 File Offset: 0x00005839
		public ObservableGroup(TKey key)
		{
			ArgumentNullException.For<TKey>.ThrowIfNull(key, "key");
			this.key = key;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00007653 File Offset: 0x00005853
		public ObservableGroup(IGrouping<TKey, TElement> grouping) : base(grouping)
		{
			this.key = grouping.Key;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00007668 File Offset: 0x00005868
		public ObservableGroup(TKey key, IEnumerable<TElement> collection) : base(collection)
		{
			ArgumentNullException.For<TKey>.ThrowIfNull(key, "key");
			this.key = key;
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00007683 File Offset: 0x00005883
		// (set) Token: 0x0600019D RID: 413 RVA: 0x0000768B File Offset: 0x0000588B
		public TKey Key
		{
			get
			{
				return this.key;
			}
			set
			{
				ArgumentNullException.For<TKey>.ThrowIfNull(value, "value");
				if (!EqualityComparer<TKey>.Default.Equals(this.key, value))
				{
					this.key = value;
					this.OnPropertyChanged(ObservableGroupHelper.KeyChangedEventArgs);
				}
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000076BD File Offset: 0x000058BD
		[MethodImpl(256)]
		internal bool TryGetList([Nullable(new byte[]
		{
			2,
			1
		})] [NotNullWhen(true)] out List<TElement> list)
		{
			list = (base.Items as List<TElement>);
			return list != null;
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600019F RID: 415 RVA: 0x000076D4 File Offset: 0x000058D4
		object IReadOnlyObservableGroup.Key
		{
			get
			{
				return this.Key;
			}
		}

		// Token: 0x1700003B RID: 59
		[Nullable(2)]
		object IReadOnlyObservableGroup.this[int index]
		{
			[NullableContext(2)]
			get
			{
				return base[index];
			}
		}

		// Token: 0x0400006F RID: 111
		private TKey key;
	}
}
