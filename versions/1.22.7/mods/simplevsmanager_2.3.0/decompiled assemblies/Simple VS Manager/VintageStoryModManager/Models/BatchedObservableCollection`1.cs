using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Models
{
	// Token: 0x0200009C RID: 156
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1
	})]
	public sealed class BatchedObservableCollection<[Nullable(2)] T> : ObservableCollection<T>
	{
		// Token: 0x06000D51 RID: 3409 RVA: 0x00050C0F File Offset: 0x0004EE0F
		public IDisposable SuspendNotifications()
		{
			return new BatchedObservableCollection<T>.SuspensionScope(this);
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x00050C18 File Offset: 0x0004EE18
		public void AddRange(IEnumerable<T> items)
		{
			ArgumentNullException.ThrowIfNull(items, "items");
			using (this.SuspendNotifications())
			{
				foreach (T t in items)
				{
					base.Add(t);
				}
			}
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00050C88 File Offset: 0x0004EE88
		public void Reset(IEnumerable<T> items)
		{
			ArgumentNullException.ThrowIfNull(items, "items");
			using (this.SuspendNotifications())
			{
				base.Clear();
				foreach (T t in items)
				{
					base.Add(t);
				}
			}
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00050D00 File Offset: 0x0004EF00
		public void ReplaceRange([TupleElementNames(new string[]
		{
			"Index",
			"Item"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1
		})] IReadOnlyList<ValueTuple<int, T>> replacements)
		{
			ArgumentNullException.ThrowIfNull(replacements, "replacements");
			using (this.SuspendNotifications())
			{
				foreach (ValueTuple<int, T> valueTuple in replacements)
				{
					int item = valueTuple.Item1;
					T item2 = valueTuple.Item2;
					if (item >= 0 && item < base.Count)
					{
						base[item] = item2;
					}
				}
			}
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00050D8C File Offset: 0x0004EF8C
		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (this._suspendCount > 0)
			{
				this._hasChangedWhileSuspended = true;
				return;
			}
			base.OnCollectionChanged(e);
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00050DA6 File Offset: 0x0004EFA6
		protected override void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (this._suspendCount > 0)
			{
				return;
			}
			base.OnPropertyChanged(e);
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x00050DB9 File Offset: 0x0004EFB9
		private void BeginSuspension()
		{
			this._suspendCount++;
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00050DCC File Offset: 0x0004EFCC
		private void EndSuspension()
		{
			if (this._suspendCount <= 0)
			{
				throw new InvalidOperationException("Cannot end suspension that was not started.");
			}
			this._suspendCount--;
			if (this._suspendCount == 0 && this._hasChangedWhileSuspended)
			{
				this._hasChangedWhileSuspended = false;
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(4));
				this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
				this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
			}
		}

		// Token: 0x040005E0 RID: 1504
		private int _suspendCount;

		// Token: 0x040005E1 RID: 1505
		private bool _hasChangedWhileSuspended;

		// Token: 0x02000315 RID: 789
		[NullableContext(0)]
		private sealed class SuspensionScope : IDisposable
		{
			// Token: 0x0600199A RID: 6554 RVA: 0x0009383F File Offset: 0x00091A3F
			[NullableContext(1)]
			public SuspensionScope(BatchedObservableCollection<T> collection)
			{
				this._collection = collection;
				this._collection.BeginSuspension();
			}

			// Token: 0x0600199B RID: 6555 RVA: 0x00093859 File Offset: 0x00091A59
			public void Dispose()
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
				this._collection.EndSuspension();
			}

			// Token: 0x040013C5 RID: 5061
			[Nullable(1)]
			private readonly BatchedObservableCollection<T> _collection;

			// Token: 0x040013C6 RID: 5062
			private bool _disposed;
		}
	}
}
