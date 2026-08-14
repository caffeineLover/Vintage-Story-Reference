using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.Collections
{
	// Token: 0x02000041 RID: 65
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1
	})]
	[DebuggerDisplay("Key = {Key}, Count = {Count}")]
	public sealed class ReadOnlyObservableGroup<TKey, [Nullable(2)] TElement> : ReadOnlyObservableCollection<TElement>, IReadOnlyObservableGroup<TKey, TElement>, IReadOnlyObservableGroup<TKey>, IReadOnlyObservableGroup, INotifyPropertyChanged, INotifyCollectionChanged, IEnumerable, IReadOnlyList<TElement>, IEnumerable<TElement>, IReadOnlyCollection<TElement>, IGrouping<TKey, TElement>
	{
		// Token: 0x060001AA RID: 426 RVA: 0x000079C0 File Offset: 0x00005BC0
		public ReadOnlyObservableGroup(TKey key, ObservableCollection<TElement> collection) : base(collection)
		{
			ArgumentNullException.For<TKey>.ThrowIfNull(key, "key");
			this.<Key>k__BackingField = key;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x000079DB File Offset: 0x00005BDB
		public ReadOnlyObservableGroup(ObservableGroup<TKey, TElement> group) : base(group)
		{
			this.<Key>k__BackingField = group.Key;
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001AC RID: 428 RVA: 0x000079F0 File Offset: 0x00005BF0
		public TKey Key
		{
			[CompilerGenerated]
			get
			{
				return this.<Key>k__BackingField;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001AD RID: 429 RVA: 0x000079F8 File Offset: 0x00005BF8
		object IReadOnlyObservableGroup.Key
		{
			get
			{
				return this.Key;
			}
		}

		// Token: 0x1700003F RID: 63
		[Nullable(2)]
		object IReadOnlyObservableGroup.this[int index]
		{
			[NullableContext(2)]
			get
			{
				return base[index];
			}
		}
	}
}
