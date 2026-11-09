using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CommunityToolkit.Mvvm.Collections
{
	// Token: 0x0200003D RID: 61
	[NullableContext(1)]
	[Nullable(0)]
	public static class ObservableGroupedCollectionExtensions
	{
		// Token: 0x06000177 RID: 375 RVA: 0x000068D0 File Offset: 0x00004AD0
		public static ObservableGroup<TKey, TElement> FirstGroupByKey<TKey, [Nullable(2)] TElement>(this ObservableGroupedCollection<TKey, TElement> source, TKey key)
		{
			ArgumentNullException.ThrowIfNull(source, "source");
			ArgumentNullException.For<TKey>.ThrowIfNull(key, "key");
			ObservableGroup<TKey, TElement> observableGroup = source.FirstGroupByKeyOrDefault(key);
			if (observableGroup == null)
			{
				ObservableGroupedCollectionExtensions.<FirstGroupByKey>g__ThrowArgumentExceptionForKeyNotFound|0_0<TKey, TElement>();
			}
			return observableGroup;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000068F8 File Offset: 0x00004AF8
		[return: Nullable(new byte[]
		{
			2,
			1,
			1
		})]
		public static ObservableGroup<TKey, TElement> FirstGroupByKeyOrDefault<TKey, [Nullable(2)] TElement>(this ObservableGroupedCollection<TKey, TElement> source, TKey key)
		{
			ArgumentNullException.ThrowIfNull(source, "source");
			ArgumentNullException.For<TKey>.ThrowIfNull(key, "key");
			List<ObservableGroup<TKey, TElement>> list;
			if (source.TryGetList(ref list))
			{
				foreach (ObservableGroup<TKey, TElement> observableGroup in list)
				{
					if (EqualityComparer<TKey>.Default.Equals(observableGroup.Key, key))
					{
						return observableGroup;
					}
				}
				return null;
			}
			return ObservableGroupedCollectionExtensions.<FirstGroupByKeyOrDefault>g__FirstGroupByKeyOrDefaultFallback|1_0<TKey, TElement>(source, key);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00006984 File Offset: 0x00004B84
		public static ObservableGroup<TKey, TElement> AddGroup<TKey, [Nullable(2)] TElement>(this ObservableGroupedCollection<TKey, TElement> source, TKey key)
		{
			ArgumentNullException.ThrowIfNull(source, "source");
			ArgumentNullException.For<TKey>.ThrowIfNull(key, "key");
			ObservableGroup<TKey, TElement> observableGroup = new ObservableGroup<TKey, TElement>(key);
			source.Add(observableGroup);
			return observableGroup;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000069B8 File Offset: 0x00004BB8
		public static ObservableGroup<TKey, TElement> AddGroup<TKey, [Nullable(2)] TElement>(this ObservableGroupedCollection<TKey, TElement> source, IGrouping<TKey, TElement> grouping)
		{
			ArgumentNullException.ThrowIfNull(source, "source");
			ArgumentNullException.ThrowIfNull(grouping, "grouping");
			ObservableGroup<TKey, TElement> observableGroup = new ObservableGroup<TKey, TElement>(grouping);
			source.Add(observableGroup);
			return observableGroup;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000069EC File Offset: 0x00004BEC
		public static ObservableGroup<TKey, TElement> AddGroup<TKey, [Nullable(2)] TElement>(this ObservableGroupedCollection<TKey, TElement> source, TKey key, IEnumerable<TElement> collection)
		{
			ArgumentNullException.ThrowIfNull(source, "source");
			ArgumentNullException.For<TKey>.ThrowIfNull(key, "key");
			ArgumentNullException.ThrowIfNull(collection, "collection");
			ObservableGroup<TKey, TElement> observableGroup = new ObservableGroup<TKey, TElement>(key, collection);
			source.Add(observableGroup);
			return observableGroup;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00006A2C File Offset: 0x00004C2C
		public static ObservableGroup<TKey, TElement> InsertGroup<TKey, [Nullable(2)] TElement>(this ObservableGroupedCollection<TKey, TElement> source, TKey key)
		{
			ArgumentNullException.ThrowIfNull(source, "source");
			ArgumentNullException.For<TKey>.ThrowIfNull(key, "key");
			List<ObservableGroup<TKey, TElement>> list;
			if (source.TryGetList(ref list))
			{
				int num = 0;
				foreach (ObservableGroup<TKey, TElement> observableGroup in list)
				{
					if (Comparer<TKey>.Default.Compare(key, observableGroup.Key) < 0)
					{
						break;
					}
					num++;
				}
				ObservableGroup<TKey, TElement> observableGroup2 = new ObservableGroup<TKey, TElement>(key);
				source.Insert(num, observableGroup2);
				return observableGroup2;
			}
			return ObservableGroupedCollectionExtensions.<InsertGroup>g__InsertGroupFallback|5_0<TKey, TElement>(source, key);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00006ACC File Offset: 0x00004CCC
		public static ObservableGroup<TKey, TElement> InsertGroup<TKey, [Nullable(2)] TElement>(this ObservableGroupedCollection<TKey, TElement> source, IGrouping<TKey, TElement> grouping)
		{
			ArgumentNullException.ThrowIfNull(source, "source");
			ArgumentNullException.ThrowIfNull(grouping, "grouping");
			List<ObservableGroup<TKey, TElement>> list;
			if (source.TryGetList(ref list))
			{
				int num = 0;
				foreach (ObservableGroup<TKey, TElement> observableGroup in list)
				{
					if (Comparer<TKey>.Default.Compare(grouping.Key, observableGroup.Key) < 0)
					{
						break;
					}
					num++;
				}
				ObservableGroup<TKey, TElement> observableGroup2 = new ObservableGroup<TKey, TElement>(grouping);
				source.Insert(num, observableGroup2);
				return observableGroup2;
			}
			return ObservableGroupedCollectionExtensions.<InsertGroup>g__InsertGroupFallback|6_0<TKey, TElement>(source, grouping);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00006B70 File Offset: 0x00004D70
		public static ObservableGroup<TKey, TElement> InsertGroup<TKey, [Nullable(2)] TElement>(this ObservableGroupedCollection<TKey, TElement> source, TKey key, IEnumerable<TElement> collection)
		{
			ArgumentNullException.ThrowIfNull(source, "source");
			ArgumentNullException.For<TKey>.ThrowIfNull(key, "key");
			ArgumentNullException.ThrowIfNull(collection, "collection");
			List<ObservableGroup<TKey, TElement>> list;
			if (source.TryGetList(ref list))
			{
				int num = 0;
				foreach (ObservableGroup<TKey, TElement> observableGroup in list)
				{
					if (Comparer<TKey>.Default.Compare(key, observableGroup.Key) < 0)
					{
						break;
					}
					num++;
				}
				ObservableGroup<TKey, TElement> observableGroup2 = new ObservableGroup<TKey, TElement>(key, collection);
				source.Insert(num, observableGroup2);
				return observableGroup2;
			}
			return ObservableGroupedCollectionExtensions.<InsertGroup>g__InsertGroupFallback|7_0<TKey, TElement>(source, key, collection);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00006C1C File Offset: 0x00004E1C
		public static ObservableGroup<TKey, TElement> InsertGroup<TKey, [Nullable(2)] TElement>(this ObservableGroupedCollection<TKey, TElement> source, TKey key, IComparer<TKey> comparer)
		{
			ArgumentNullException.ThrowIfNull(source, "source");
			ArgumentNullException.For<TKey>.ThrowIfNull(key, "key");
			ArgumentNullException.ThrowIfNull(comparer, "comparer");
			List<ObservableGroup<TKey, TElement>> list;
			if (source.TryGetList(ref list))
			{
				int num = 0;
				foreach (ObservableGroup<TKey, TElement> observableGroup in list)
				{
					if (comparer.Compare(key, observableGroup.Key) < 0)
					{
						break;
					}
					num++;
				}
				ObservableGroup<TKey, TElement> observableGroup2 = new ObservableGroup<TKey, TElement>(key);
				source.Insert(num, observableGroup2);
				return observableGroup2;
			}
			return ObservableGroupedCollectionExtensions.<InsertGroup>g__InsertGroupFallback|8_0<TKey, TElement>(source, key, comparer);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00006CC4 File Offset: 0x00004EC4
		public static ObservableGroup<TKey, TElement> InsertGroup<TKey, [Nullable(2)] TElement>(this ObservableGroupedCollection<TKey, TElement> source, IGrouping<TKey, TElement> grouping, IComparer<TKey> comparer)
		{
			ArgumentNullException.ThrowIfNull(source, "source");
			ArgumentNullException.ThrowIfNull(grouping, "grouping");
			ArgumentNullException.ThrowIfNull(comparer, "comparer");
			List<ObservableGroup<TKey, TElement>> list;
			if (source.TryGetList(ref list))
			{
				int num = 0;
				foreach (ObservableGroup<TKey, TElement> observableGroup in list)
				{
					if (comparer.Compare(grouping.Key, observableGroup.Key) < 0)
					{
						break;
					}
					num++;
				}
				ObservableGroup<TKey, TElement> observableGroup2 = new ObservableGroup<TKey, TElement>(grouping);
				source.Insert(num, observableGroup2);
				return observableGroup2;
			}
			return ObservableGroupedCollectionExtensions.<InsertGroup>g__InsertGroupFallback|9_0<TKey, TElement>(source, grouping, comparer);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00006D70 File Offset: 0x00004F70
		public static ObservableGroup<TKey, TElement> InsertGroup<TKey, [Nullable(2)] TElement>(this ObservableGroupedCollection<TKey, TElement> source, TKey key, IComparer<TKey> comparer, IEnumerable<TElement> collection)
		{
			ArgumentNullException.ThrowIfNull(source, "source");
			ArgumentNullException.For<TKey>.ThrowIfNull(key, "key");
			ArgumentNullException.ThrowIfNull(comparer, "comparer");
			ArgumentNullException.ThrowIfNull(collection, "collection");
			List<ObservableGroup<TKey, TElement>> list;
			if (source.TryGetList(ref list))
			{
				int num = 0;
				foreach (ObservableGroup<TKey, TElement> observableGroup in list)
				{
					if (comparer.Compare(key, observableGroup.Key) < 0)
					{
						break;
					}
					num++;
				}
				ObservableGroup<TKey, TElement> observableGroup2 = new ObservableGroup<TKey, TElement>(key, collection);
				source.Insert(num, observableGroup2);
				return observableGroup2;
			}
			return ObservableGroupedCollectionExtensions.<InsertGroup>g__InsertGroupFallback|10_0<TKey, TElement>(source, key, comparer, collection);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00006E24 File Offset: 0x00005024
		public static ObservableGroup<TKey, TElement> AddItem<TKey, [Nullable(2)] TElement>(this ObservableGroupedCollection<TKey, TElement> source, TKey key, TElement item)
		{
			ArgumentNullException.ThrowIfNull(source, "source");
			ArgumentNullException.For<TKey>.ThrowIfNull(key, "key");
			ObservableGroup<TKey, TElement> observableGroup = source.FirstGroupByKeyOrDefault(key);
			if (observableGroup == null)
			{
				observableGroup = new ObservableGroup<TKey, TElement>(key)
				{
					item
				};
				source.Add(observableGroup);
			}
			else
			{
				observableGroup.Add(item);
			}
			return observableGroup;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00006E74 File Offset: 0x00005074
		public static ObservableGroup<TKey, TElement> InsertItem<TKey, [Nullable(2)] TElement>(this ObservableGroupedCollection<TKey, TElement> source, TKey key, TElement item)
		{
			ArgumentNullException.ThrowIfNull(source, "source");
			ArgumentNullException.For<TKey>.ThrowIfNull(key, "key");
			ObservableGroup<TKey, TElement> observableGroup = source.FirstGroupByKeyOrDefault(key);
			List<TElement> list;
			if (observableGroup == null)
			{
				observableGroup = source.InsertGroup(key, new TElement[]
				{
					item
				});
			}
			else if (observableGroup.TryGetList(ref list))
			{
				int num = 0;
				foreach (TElement telement in list)
				{
					if (Comparer<TElement>.Default.Compare(item, telement) < 0)
					{
						break;
					}
					num++;
				}
				observableGroup.Insert(num, item);
			}
			else
			{
				ObservableGroupedCollectionExtensions.<InsertItem>g__InsertItemFallback|12_0<TKey, TElement>(observableGroup, item);
			}
			return observableGroup;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00006F2C File Offset: 0x0000512C
		public static ObservableGroup<TKey, TElement> InsertItem<TKey, [Nullable(2)] TElement>(this ObservableGroupedCollection<TKey, TElement> source, TKey key, IComparer<TKey> keyComparer, TElement item, IComparer<TElement> itemComparer)
		{
			ArgumentNullException.ThrowIfNull(source, "source");
			ArgumentNullException.For<TKey>.ThrowIfNull(key, "key");
			ArgumentNullException.ThrowIfNull(keyComparer, "keyComparer");
			ArgumentNullException.ThrowIfNull(itemComparer, "itemComparer");
			ObservableGroup<TKey, TElement> observableGroup = source.FirstGroupByKeyOrDefault(key);
			List<TElement> list;
			if (observableGroup == null)
			{
				observableGroup = source.InsertGroup(key, keyComparer, new TElement[]
				{
					item
				});
			}
			else if (observableGroup.TryGetList(ref list))
			{
				int num = 0;
				foreach (TElement telement in list)
				{
					if (itemComparer.Compare(item, telement) < 0)
					{
						break;
					}
					num++;
				}
				observableGroup.Insert(num, item);
			}
			else
			{
				ObservableGroupedCollectionExtensions.<InsertItem>g__InsertItemFallback|13_0<TKey, TElement>(observableGroup, item, itemComparer);
			}
			return observableGroup;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00006FF8 File Offset: 0x000051F8
		public static void RemoveGroup<TKey, [Nullable(2)] TValue>(this ObservableGroupedCollection<TKey, TValue> source, TKey key)
		{
			ArgumentNullException.ThrowIfNull(source, "source");
			ArgumentNullException.For<TKey>.ThrowIfNull(key, "key");
			List<ObservableGroup<TKey, TValue>> list;
			if (source.TryGetList(ref list))
			{
				int num = 0;
				using (List<ObservableGroup<TKey, TValue>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ObservableGroup<TKey, TValue> observableGroup = enumerator.Current;
						if (EqualityComparer<TKey>.Default.Equals(observableGroup.Key, key))
						{
							source.RemoveAt(num);
							break;
						}
						num++;
					}
					return;
				}
			}
			ObservableGroupedCollectionExtensions.<RemoveGroup>g__RemoveGroupFallback|14_0<TKey, TValue>(source, key);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00007088 File Offset: 0x00005288
		public static void RemoveItem<TKey, [Nullable(2)] TValue>(this ObservableGroupedCollection<TKey, TValue> source, TKey key, TValue item, bool removeGroupIfEmpty = true)
		{
			ArgumentNullException.ThrowIfNull(source, "source");
			ArgumentNullException.For<TKey>.ThrowIfNull(key, "key");
			List<ObservableGroup<TKey, TValue>> list;
			if (source.TryGetList(ref list))
			{
				int num = 0;
				using (List<ObservableGroup<TKey, TValue>>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ObservableGroup<TKey, TValue> observableGroup = enumerator.Current;
						if (EqualityComparer<TKey>.Default.Equals(observableGroup.Key, key))
						{
							if (observableGroup.Remove(item) && removeGroupIfEmpty && observableGroup.Count == 0)
							{
								source.RemoveAt(num);
							}
							break;
						}
						num++;
					}
					return;
				}
			}
			ObservableGroupedCollectionExtensions.<RemoveItem>g__RemoveItemFallback|15_0<TKey, TValue>(source, key, item, removeGroupIfEmpty);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00007130 File Offset: 0x00005330
		[CompilerGenerated]
		[DoesNotReturn]
		internal static void <FirstGroupByKey>g__ThrowArgumentExceptionForKeyNotFound|0_0<TKey, TElement>()
		{
			throw new InvalidOperationException("The requested key was not present in the collection.");
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000713C File Offset: 0x0000533C
		[CompilerGenerated]
		[MethodImpl(8)]
		[return: Nullable(new byte[]
		{
			2,
			1,
			0
		})]
		internal static ObservableGroup<TKey, TElement> <FirstGroupByKeyOrDefault>g__FirstGroupByKeyOrDefaultFallback|1_0<TKey, TElement>([Nullable(new byte[]
		{
			1,
			1,
			0
		})] ObservableGroupedCollection<TKey, TElement> source, TKey key)
		{
			ObservableGroupedCollectionExtensions.<>c__DisplayClass1_0<TKey, TElement> CS$<>8__locals1 = new ObservableGroupedCollectionExtensions.<>c__DisplayClass1_0<TKey, TElement>();
			CS$<>8__locals1.key = key;
			return Enumerable.FirstOrDefault<ObservableGroup<TKey, TElement>>(source, new Func<ObservableGroup<TKey, TElement>, bool>(CS$<>8__locals1.<FirstGroupByKeyOrDefault>b__1));
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00007168 File Offset: 0x00005368
		[CompilerGenerated]
		[MethodImpl(8)]
		[return: Nullable(new byte[]
		{
			1,
			1,
			0
		})]
		internal static ObservableGroup<TKey, TElement> <InsertGroup>g__InsertGroupFallback|5_0<TKey, TElement>([Nullable(new byte[]
		{
			1,
			1,
			0
		})] ObservableGroupedCollection<TKey, TElement> source, TKey key)
		{
			int num = 0;
			foreach (ObservableGroup<TKey, TElement> observableGroup in source)
			{
				if (Comparer<TKey>.Default.Compare(key, observableGroup.Key) < 0)
				{
					break;
				}
				num++;
			}
			ObservableGroup<TKey, TElement> observableGroup2 = new ObservableGroup<TKey, TElement>(key);
			source.Insert(num, observableGroup2);
			return observableGroup2;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000071D8 File Offset: 0x000053D8
		[CompilerGenerated]
		[MethodImpl(8)]
		[return: Nullable(new byte[]
		{
			1,
			1,
			0
		})]
		internal static ObservableGroup<TKey, TElement> <InsertGroup>g__InsertGroupFallback|6_0<TKey, TElement>([Nullable(new byte[]
		{
			1,
			1,
			0
		})] ObservableGroupedCollection<TKey, TElement> source, [Nullable(new byte[]
		{
			1,
			1,
			0
		})] IGrouping<TKey, TElement> grouping)
		{
			int num = 0;
			foreach (ObservableGroup<TKey, TElement> observableGroup in source)
			{
				if (Comparer<TKey>.Default.Compare(grouping.Key, observableGroup.Key) < 0)
				{
					break;
				}
				num++;
			}
			ObservableGroup<TKey, TElement> observableGroup2 = new ObservableGroup<TKey, TElement>(grouping);
			source.Insert(num, observableGroup2);
			return observableGroup2;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000724C File Offset: 0x0000544C
		[CompilerGenerated]
		[MethodImpl(8)]
		[return: Nullable(new byte[]
		{
			1,
			1,
			0
		})]
		internal static ObservableGroup<TKey, TElement> <InsertGroup>g__InsertGroupFallback|7_0<TKey, TElement>([Nullable(new byte[]
		{
			1,
			1,
			0
		})] ObservableGroupedCollection<TKey, TElement> source, TKey key, [Nullable(new byte[]
		{
			1,
			0
		})] IEnumerable<TElement> collection)
		{
			int num = 0;
			foreach (ObservableGroup<TKey, TElement> observableGroup in source)
			{
				if (Comparer<TKey>.Default.Compare(key, observableGroup.Key) < 0)
				{
					break;
				}
				num++;
			}
			ObservableGroup<TKey, TElement> observableGroup2 = new ObservableGroup<TKey, TElement>(key, collection);
			source.Insert(num, observableGroup2);
			return observableGroup2;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000072BC File Offset: 0x000054BC
		[CompilerGenerated]
		[MethodImpl(8)]
		[return: Nullable(new byte[]
		{
			1,
			1,
			0
		})]
		internal static ObservableGroup<TKey, TElement> <InsertGroup>g__InsertGroupFallback|8_0<TKey, TElement>([Nullable(new byte[]
		{
			1,
			1,
			0
		})] ObservableGroupedCollection<TKey, TElement> source, TKey key, IComparer<TKey> comparer)
		{
			int num = 0;
			foreach (ObservableGroup<TKey, TElement> observableGroup in source)
			{
				if (comparer.Compare(key, observableGroup.Key) < 0)
				{
					break;
				}
				num++;
			}
			ObservableGroup<TKey, TElement> observableGroup2 = new ObservableGroup<TKey, TElement>(key);
			source.Insert(num, observableGroup2);
			return observableGroup2;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00007328 File Offset: 0x00005528
		[CompilerGenerated]
		[MethodImpl(8)]
		[return: Nullable(new byte[]
		{
			1,
			1,
			0
		})]
		internal static ObservableGroup<TKey, TElement> <InsertGroup>g__InsertGroupFallback|9_0<TKey, TElement>([Nullable(new byte[]
		{
			1,
			1,
			0
		})] ObservableGroupedCollection<TKey, TElement> source, [Nullable(new byte[]
		{
			1,
			1,
			0
		})] IGrouping<TKey, TElement> grouping, IComparer<TKey> comparer)
		{
			int num = 0;
			foreach (ObservableGroup<TKey, TElement> observableGroup in source)
			{
				if (comparer.Compare(grouping.Key, observableGroup.Key) < 0)
				{
					break;
				}
				num++;
			}
			ObservableGroup<TKey, TElement> observableGroup2 = new ObservableGroup<TKey, TElement>(grouping);
			source.Insert(num, observableGroup2);
			return observableGroup2;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00007398 File Offset: 0x00005598
		[CompilerGenerated]
		[MethodImpl(8)]
		[return: Nullable(new byte[]
		{
			1,
			1,
			0
		})]
		internal static ObservableGroup<TKey, TElement> <InsertGroup>g__InsertGroupFallback|10_0<TKey, TElement>([Nullable(new byte[]
		{
			1,
			1,
			0
		})] ObservableGroupedCollection<TKey, TElement> source, TKey key, IComparer<TKey> comparer, [Nullable(new byte[]
		{
			1,
			0
		})] IEnumerable<TElement> collection)
		{
			int num = 0;
			foreach (ObservableGroup<TKey, TElement> observableGroup in source)
			{
				if (comparer.Compare(key, observableGroup.Key) < 0)
				{
					break;
				}
				num++;
			}
			ObservableGroup<TKey, TElement> observableGroup2 = new ObservableGroup<TKey, TElement>(key, collection);
			source.Insert(num, observableGroup2);
			return observableGroup2;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00007404 File Offset: 0x00005604
		[NullableContext(0)]
		[CompilerGenerated]
		[MethodImpl(8)]
		internal static void <InsertItem>g__InsertItemFallback|12_0<TKey, TElement>([Nullable(new byte[]
		{
			1,
			0
		})] ObservableCollection<TElement> source, TElement item)
		{
			int num = 0;
			foreach (TElement telement in source)
			{
				if (Comparer<TElement>.Default.Compare(item, telement) < 0)
				{
					break;
				}
				num++;
			}
			source.Insert(num, item);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00007464 File Offset: 0x00005664
		[NullableContext(0)]
		[CompilerGenerated]
		[MethodImpl(8)]
		internal static void <InsertItem>g__InsertItemFallback|13_0<TKey, TElement>([Nullable(new byte[]
		{
			1,
			0
		})] ObservableCollection<TElement> source, TElement item, [Nullable(new byte[]
		{
			1,
			0
		})] IComparer<TElement> comparer)
		{
			int num = 0;
			foreach (TElement telement in source)
			{
				if (comparer.Compare(item, telement) < 0)
				{
					break;
				}
				num++;
			}
			source.Insert(num, item);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000074C0 File Offset: 0x000056C0
		[CompilerGenerated]
		[MethodImpl(8)]
		internal static void <RemoveGroup>g__RemoveGroupFallback|14_0<TKey, TValue>([Nullable(new byte[]
		{
			1,
			1,
			0
		})] ObservableGroupedCollection<TKey, TValue> source, TKey key)
		{
			int num = 0;
			foreach (ObservableGroup<TKey, TValue> observableGroup in source)
			{
				if (EqualityComparer<TKey>.Default.Equals(observableGroup.Key, key))
				{
					source.RemoveAt(num);
					break;
				}
				num++;
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00007524 File Offset: 0x00005724
		[NullableContext(0)]
		[CompilerGenerated]
		[MethodImpl(8)]
		internal static void <RemoveItem>g__RemoveItemFallback|15_0<TKey, TValue>([Nullable(new byte[]
		{
			1,
			1,
			0
		})] ObservableGroupedCollection<TKey, TValue> source, [Nullable(1)] TKey key, TValue item, bool removeGroupIfEmpty)
		{
			int num = 0;
			foreach (ObservableGroup<TKey, TValue> observableGroup in source)
			{
				if (EqualityComparer<TKey>.Default.Equals(observableGroup.Key, key))
				{
					if (observableGroup.Remove(item) && removeGroupIfEmpty && observableGroup.Count == 0)
					{
						source.RemoveAt(num);
					}
					break;
				}
				num++;
			}
		}
	}
}
