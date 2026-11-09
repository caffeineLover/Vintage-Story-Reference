using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.ViewModels
{
	// Token: 0x0200004F RID: 79
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SortOption
	{
		// Token: 0x06000874 RID: 2164 RVA: 0x00033E6F File Offset: 0x0003206F
		public SortOption(string displayName, [TupleElementNames(new string[]
		{
			"Property",
			"Direction"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1
		})] params ValueTuple<string, ListSortDirection>[] sorts)
		{
			if (displayName == null)
			{
				throw new ArgumentNullException("displayName");
			}
			this.DisplayName = displayName;
			this._sorts = (sorts ?? Array.Empty<ValueTuple<string, ListSortDirection>>());
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x00033E9D File Offset: 0x0003209D
		public string DisplayName { get; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000876 RID: 2166 RVA: 0x00033EA5 File Offset: 0x000320A5
		[TupleElementNames(new string[]
		{
			"Property",
			"Direction"
		})]
		[Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		public IReadOnlyList<ValueTuple<string, ListSortDirection>> SortDescriptions
		{
			[return: TupleElementNames(new string[]
			{
				"Property",
				"Direction"
			})]
			[return: Nullable(new byte[]
			{
				1,
				0,
				1
			})]
			get
			{
				return this._sorts;
			}
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x00033EB0 File Offset: 0x000320B0
		public void Apply(ICollectionView view)
		{
			if (view == null)
			{
				return;
			}
			if (this.IsSortAlreadyApplied(view))
			{
				return;
			}
			using (view.DeferRefresh())
			{
				view.SortDescriptions.Clear();
				foreach (ValueTuple<string, ListSortDirection> valueTuple in this._sorts)
				{
					view.SortDescriptions.Add(new SortDescription(valueTuple.Item1, valueTuple.Item2));
				}
			}
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x00033F34 File Offset: 0x00032134
		private bool IsSortAlreadyApplied(ICollectionView view)
		{
			SortDescriptionCollection sortDescriptions = view.SortDescriptions;
			if (sortDescriptions.Count != this._sorts.Length)
			{
				return false;
			}
			for (int i = 0; i < this._sorts.Length; i++)
			{
				SortDescription sortDescription = sortDescriptions[i];
				ValueTuple<string, ListSortDirection> valueTuple = this._sorts[i];
				if (!string.Equals(sortDescription.PropertyName, valueTuple.Item1, 4) || sortDescription.Direction != valueTuple.Item2)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00033FAC File Offset: 0x000321AC
		public override string ToString()
		{
			return this.DisplayName;
		}

		// Token: 0x0400042D RID: 1069
		[TupleElementNames(new string[]
		{
			"Property",
			"Direction"
		})]
		[Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		private readonly ValueTuple<string, ListSortDirection>[] _sorts;
	}
}
