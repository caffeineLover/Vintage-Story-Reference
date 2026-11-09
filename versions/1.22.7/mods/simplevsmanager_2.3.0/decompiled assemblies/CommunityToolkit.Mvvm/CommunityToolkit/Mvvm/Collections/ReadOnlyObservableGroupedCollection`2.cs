using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.Collections
{
	// Token: 0x02000040 RID: 64
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1,
		1,
		1
	})]
	public sealed class ReadOnlyObservableGroupedCollection<TKey, [Nullable(2)] TElement> : ReadOnlyObservableCollection<ReadOnlyObservableGroup<TKey, TElement>>, ILookup<!0, !1>, IEnumerable<IGrouping<!0, !1>>, IEnumerable
	{
		// Token: 0x060001A1 RID: 417 RVA: 0x000076F0 File Offset: 0x000058F0
		public ReadOnlyObservableGroupedCollection(ObservableCollection<ObservableGroup<TKey, TElement>> collection)
		{
			IEnumerable<ReadOnlyObservableGroup<TKey, TElement>> enumerable;
			if (collection == null)
			{
				enumerable = null;
			}
			else
			{
				Func<ObservableGroup<TKey, TElement>, ReadOnlyObservableGroup<TKey, TElement>> func;
				if ((func = ReadOnlyObservableGroupedCollection<TKey, TElement>.<>c.<>9__0_0) == null)
				{
					func = (ReadOnlyObservableGroupedCollection<TKey, TElement>.<>c.<>9__0_0 = new Func<ObservableGroup<TKey, TElement>, ReadOnlyObservableGroup<TKey, TElement>>(ReadOnlyObservableGroupedCollection<TKey, TElement>.<>c.<>9.<.ctor>b__0_0));
				}
				enumerable = Enumerable.Select<ObservableGroup<TKey, TElement>, ReadOnlyObservableGroup<TKey, TElement>>(collection, func);
			}
			base..ctor(new ObservableCollection<ReadOnlyObservableGroup<TKey, TElement>>(enumerable));
			collection.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnSourceCollectionChanged);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00007745 File Offset: 0x00005945
		public ReadOnlyObservableGroupedCollection(ObservableCollection<ReadOnlyObservableGroup<TKey, TElement>> collection) : base(collection)
		{
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x00007750 File Offset: 0x00005950
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

		// Token: 0x060001A4 RID: 420 RVA: 0x00007779 File Offset: 0x00005979
		bool ILookup<!0, !1>.Contains(TKey key)
		{
			return key != null && this.FirstGroupByKeyOrDefault(key) != null;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00007792 File Offset: 0x00005992
		IEnumerator<IGrouping<TKey, TElement>> IEnumerable<IGrouping<!0, !1>>.GetEnumerator()
		{
			return base.GetEnumerator();
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000779C File Offset: 0x0000599C
		private void OnSourceCollectionChanged([Nullable(2)] object sender, NotifyCollectionChangedEventArgs e)
		{
			ObservableCollection<ReadOnlyObservableGroup<TKey, TElement>> observableCollection = (ObservableCollection<ReadOnlyObservableGroup<TKey, TElement>>)base.Items;
			switch (e.Action)
			{
			case 0:
				if (e.NewItems.Count == 1)
				{
					ObservableGroup<TKey, TElement> observableGroup = (ObservableGroup<TKey, TElement>)e.NewItems[0];
					observableCollection.Insert(e.NewStartingIndex, new ReadOnlyObservableGroup<TKey, TElement>(observableGroup));
					return;
				}
				if (e.NewItems.Count > 1)
				{
					ReadOnlyObservableGroupedCollection<TKey, TElement>.<OnSourceCollectionChanged>g__ThrowNotSupportedExceptionForRangeOperation|6_0();
					return;
				}
				break;
			case 1:
				if (e.OldItems.Count == 1)
				{
					observableCollection.RemoveAt(e.OldStartingIndex);
					return;
				}
				if (e.OldItems.Count > 1)
				{
					ReadOnlyObservableGroupedCollection<TKey, TElement>.<OnSourceCollectionChanged>g__ThrowNotSupportedExceptionForRangeOperation|6_0();
					return;
				}
				break;
			case 2:
				if (e.OldItems.Count == 1 && e.NewItems.Count == 1)
				{
					ObservableGroup<TKey, TElement> observableGroup2 = (ObservableGroup<TKey, TElement>)e.NewItems[0];
					observableCollection[e.OldStartingIndex] = new ReadOnlyObservableGroup<TKey, TElement>(observableGroup2);
					return;
				}
				if (e.OldItems.Count > 1 || e.NewItems.Count > 1)
				{
					ReadOnlyObservableGroupedCollection<TKey, TElement>.<OnSourceCollectionChanged>g__ThrowNotSupportedExceptionForRangeOperation|6_0();
					return;
				}
				break;
			case 3:
				if (e.OldItems.Count == 1 && e.NewItems.Count == 1)
				{
					observableCollection.Move(e.OldStartingIndex, e.NewStartingIndex);
					return;
				}
				if (e.OldItems.Count > 1 || e.NewItems.Count > 1)
				{
					ReadOnlyObservableGroupedCollection<TKey, TElement>.<OnSourceCollectionChanged>g__ThrowNotSupportedExceptionForRangeOperation|6_0();
					return;
				}
				break;
			case 4:
				observableCollection.Clear();
				break;
			default:
				return;
			}
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00007910 File Offset: 0x00005B10
		[return: Nullable(new byte[]
		{
			2,
			1
		})]
		private IEnumerable<TElement> FirstGroupByKeyOrDefault(TKey key)
		{
			List<ReadOnlyObservableGroup<TKey, TElement>> list = base.Items as List<ReadOnlyObservableGroup<TKey, TElement>>;
			if (list != null)
			{
				foreach (ReadOnlyObservableGroup<TKey, TElement> readOnlyObservableGroup in list)
				{
					if (EqualityComparer<TKey>.Default.Equals(readOnlyObservableGroup.Key, key))
					{
						return readOnlyObservableGroup;
					}
				}
				return null;
			}
			return ReadOnlyObservableGroupedCollection<TKey, TElement>.<FirstGroupByKeyOrDefault>g__FirstGroupByKeyOrDefaultFallback|7_0(this, key);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00007988 File Offset: 0x00005B88
		[CompilerGenerated]
		[DoesNotReturn]
		internal static void <OnSourceCollectionChanged>g__ThrowNotSupportedExceptionForRangeOperation|6_0()
		{
			throw new NotSupportedException("ReadOnlyObservableGroupedCollection<TKey, TValue> doesn't support operations on multiple items at once.\nIf this exception was thrown, it likely means support for batched item updates has been added to the underlying ObservableCollection<T> type, and this implementation doesn't support that feature yet.\nPlease consider opening an issue in https://aka.ms/toolkit/dotnet to report this.");
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00007994 File Offset: 0x00005B94
		[CompilerGenerated]
		[MethodImpl(8)]
		[return: Nullable(new byte[]
		{
			2,
			1
		})]
		internal static IEnumerable<TElement> <FirstGroupByKeyOrDefault>g__FirstGroupByKeyOrDefaultFallback|7_0(ReadOnlyObservableGroupedCollection<TKey, TElement> source, TKey key)
		{
			ReadOnlyObservableGroupedCollection<TKey, TElement>.<>c__DisplayClass7_0 CS$<>8__locals1 = new ReadOnlyObservableGroupedCollection<TKey, TElement>.<>c__DisplayClass7_0();
			CS$<>8__locals1.key = key;
			return Enumerable.FirstOrDefault<ReadOnlyObservableGroup<TKey, TElement>>(source, new Func<ReadOnlyObservableGroup<TKey, TElement>, bool>(CS$<>8__locals1.<FirstGroupByKeyOrDefault>b__1));
		}
	}
}
