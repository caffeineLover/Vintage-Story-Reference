using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using VintageStoryModManager.ViewModels;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200008E RID: 142
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class TagFilterService
	{
		// Token: 0x06000BB4 RID: 2996 RVA: 0x00048000 File Offset: 0x00046200
		public TagFilterService(TagCacheService tagCache)
		{
			if (tagCache == null)
			{
				throw new ArgumentNullException("tagCache");
			}
			this._tagCache = tagCache;
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000BB5 RID: 2997 RVA: 0x0004805E File Offset: 0x0004625E
		public int FilterVersion
		{
			get
			{
				return this._filterVersion;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000BB6 RID: 2998 RVA: 0x00048068 File Offset: 0x00046268
		public bool HasSelectedTags
		{
			get
			{
				object filterLock = this._filterLock;
				bool result;
				lock (filterLock)
				{
					result = (this._selectedInstalledTags.Count > 0);
				}
				return result;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000BB7 RID: 2999 RVA: 0x000480B4 File Offset: 0x000462B4
		public bool HasSelectedInstalledTags
		{
			get
			{
				object filterLock = this._filterLock;
				bool result;
				lock (filterLock)
				{
					result = (this._selectedInstalledTags.Count > 0);
				}
				return result;
			}
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x00048100 File Offset: 0x00046300
		public IReadOnlyList<string> GetSelectedInstalledTags()
		{
			object filterLock = this._filterLock;
			IReadOnlyList<string> result;
			lock (filterLock)
			{
				result = Enumerable.ToArray<string>(this._selectedInstalledTags);
			}
			return result;
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x00048148 File Offset: 0x00046348
		public bool SetSelectedInstalledTags([Nullable(new byte[]
		{
			2,
			1
		})] IEnumerable<string> tags)
		{
			object filterLock = this._filterLock;
			bool result;
			lock (filterLock)
			{
				IReadOnlyList<string> readOnlyList = TagFilterService.NormalizeTags(tags);
				if (TagFilterService.TagSetsEqual(this._selectedInstalledTags, readOnlyList))
				{
					result = false;
				}
				else
				{
					this._selectedInstalledTags.Clear();
					foreach (string text in readOnlyList)
					{
						this._selectedInstalledTags.Add(text);
					}
					this._cachedSelectedInstalledTags = ((this._selectedInstalledTags.Count > 0) ? Enumerable.ToArray<string>(this._selectedInstalledTags) : Array.Empty<string>());
					this.IncrementVersion();
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0004821C File Offset: 0x0004641C
		public bool ToggleInstalledTag(string tag)
		{
			if (string.IsNullOrWhiteSpace(tag))
			{
				return false;
			}
			string text = tag.Trim();
			if (text.Length == 0)
			{
				return false;
			}
			object filterLock = this._filterLock;
			bool result;
			lock (filterLock)
			{
				bool flag2;
				if (this._selectedInstalledTags.Contains(text))
				{
					flag2 = this._selectedInstalledTags.Remove(text);
				}
				else
				{
					flag2 = this._selectedInstalledTags.Add(text);
				}
				if (flag2)
				{
					this._cachedSelectedInstalledTags = ((this._selectedInstalledTags.Count > 0) ? Enumerable.ToArray<string>(this._selectedInstalledTags) : Array.Empty<string>());
					this.IncrementVersion();
				}
				result = flag2;
			}
			return result;
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x000482D4 File Offset: 0x000464D4
		public void ClearAllSelections()
		{
			object filterLock = this._filterLock;
			lock (filterLock)
			{
				if (this._selectedInstalledTags.Count != 0)
				{
					this._selectedInstalledTags.Clear();
					this._cachedSelectedInstalledTags = Array.Empty<string>();
					this.IncrementVersion();
				}
			}
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x0004833C File Offset: 0x0004653C
		public void ClearInstalledTagSelection()
		{
			object filterLock = this._filterLock;
			lock (filterLock)
			{
				if (this._selectedInstalledTags.Count != 0)
				{
					this._selectedInstalledTags.Clear();
					this._cachedSelectedInstalledTags = Array.Empty<string>();
					this.IncrementVersion();
				}
			}
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x000483A4 File Offset: 0x000465A4
		public bool PassesInstalledTagFilter([Nullable(new byte[]
		{
			2,
			1
		})] IReadOnlyList<string> modTags)
		{
			IReadOnlyList<string> cachedSelectedInstalledTags = this._cachedSelectedInstalledTags;
			return cachedSelectedInstalledTags.Count == 0 || TagFilterService.ContainsAllTags(modTags, cachedSelectedInstalledTags);
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x000483CC File Offset: 0x000465CC
		public void SetInstalledAvailableTags(IEnumerable<string> tags)
		{
			IReadOnlyList<string> installedAvailableTags = TagFilterService.SortAndDeduplicate(tags);
			this._installedAvailableTags = installedAvailableTags;
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x000483EC File Offset: 0x000465EC
		public IReadOnlyList<string> GetInstalledAvailableTags()
		{
			object filterLock = this._filterLock;
			IReadOnlyList<string> readOnlyList;
			lock (filterLock)
			{
				readOnlyList = Enumerable.ToArray<string>(this._selectedInstalledTags);
			}
			if (readOnlyList.Count == 0)
			{
				return this._installedAvailableTags;
			}
			return TagFilterService.MergeTags(this._installedAvailableTags, readOnlyList);
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x00048454 File Offset: 0x00046654
		public void UpdateInstalledAvailableTagsFromMods(IEnumerable<ModListItemViewModel> mods)
		{
			IEnumerable<string> installedAvailableTags = TagFilterService.EnumerateModTags(mods);
			this.SetInstalledAvailableTags(installedAvailableTags);
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x00048470 File Offset: 0x00046670
		public bool IsInstalledTagSelected(string tag)
		{
			if (string.IsNullOrWhiteSpace(tag))
			{
				return false;
			}
			object filterLock = this._filterLock;
			bool result;
			lock (filterLock)
			{
				result = this._selectedInstalledTags.Contains(tag.Trim());
			}
			return result;
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x000484C8 File Offset: 0x000466C8
		private void IncrementVersion()
		{
			Interlocked.Increment(ref this._filterVersion);
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x000484D8 File Offset: 0x000466D8
		private static IReadOnlyList<string> NormalizeTags([Nullable(new byte[]
		{
			2,
			1
		})] IEnumerable<string> tags)
		{
			if (tags == null)
			{
				return Array.Empty<string>();
			}
			List<string> list = new List<string>();
			foreach (string text in tags)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					string text2 = text.Trim();
					if (text2.Length != 0)
					{
						list.Add(text2);
					}
				}
			}
			return list;
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x00048548 File Offset: 0x00046748
		private static bool TagSetsEqual(HashSet<string> set, IReadOnlyList<string> list)
		{
			if (set.Count != list.Count)
			{
				return false;
			}
			foreach (string text in list)
			{
				if (!set.Contains(text))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x000485AC File Offset: 0x000467AC
		private unsafe static bool ContainsAllTags([Nullable(new byte[]
		{
			2,
			1
		})] IReadOnlyList<string> modTags, IReadOnlyList<string> requiredTags)
		{
			if (requiredTags.Count == 0)
			{
				return true;
			}
			if (modTags == null || modTags.Count == 0)
			{
				return false;
			}
			if (requiredTags.Count <= 3)
			{
				int count = requiredTags.Count;
				Span<bool> span = new Span<bool>(stackalloc byte[(UIntPtr)count], count);
				int num = 0;
				foreach (string text in modTags)
				{
					if (!string.IsNullOrWhiteSpace(text))
					{
						string text2 = text.Trim();
						if (text2.Length != 0)
						{
							int i = 0;
							while (i < requiredTags.Count)
							{
								if (!(*span[i]) && string.Equals(text2, requiredTags[i], 5))
								{
									*span[i] = true;
									num++;
									if (num == requiredTags.Count)
									{
										return true;
									}
									break;
								}
								else
								{
									i++;
								}
							}
						}
					}
				}
				return num == requiredTags.Count;
			}
			HashSet<string> hashSet = new HashSet<string>(modTags.Count, StringComparer.OrdinalIgnoreCase);
			foreach (string text3 in modTags)
			{
				if (!string.IsNullOrWhiteSpace(text3))
				{
					string text4 = text3.Trim();
					if (text4.Length > 0)
					{
						hashSet.Add(text4);
					}
				}
			}
			foreach (string text5 in requiredTags)
			{
				if (!hashSet.Contains(text5))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x00048758 File Offset: 0x00046958
		private static IEnumerable<string> EnumerateModTags(IEnumerable<ModListItemViewModel> mods)
		{
			TagFilterService.<EnumerateModTags>d__27 <EnumerateModTags>d__ = new TagFilterService.<EnumerateModTags>d__27(-2);
			<EnumerateModTags>d__.<>3__mods = mods;
			return <EnumerateModTags>d__;
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x00048768 File Offset: 0x00046968
		private static IReadOnlyList<string> SortAndDeduplicate(IEnumerable<string> tags)
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (string text in tags)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					string text2 = text.Trim();
					if (text2.Length > 0)
					{
						hashSet.Add(text2);
					}
				}
			}
			if (hashSet.Count == 0)
			{
				return Array.Empty<string>();
			}
			List<string> list = Enumerable.ToList<string>(hashSet);
			list.Sort(StringComparer.OrdinalIgnoreCase);
			return list.AsReadOnly();
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x000487F8 File Offset: 0x000469F8
		private static IReadOnlyList<string> MergeTags(IReadOnlyList<string> existing, IReadOnlyList<string> additional)
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (string text in existing)
			{
				hashSet.Add(text);
			}
			foreach (string text2 in additional)
			{
				if (!string.IsNullOrWhiteSpace(text2))
				{
					string text3 = text2.Trim();
					if (text3.Length > 0)
					{
						hashSet.Add(text3);
					}
				}
			}
			List<string> list = Enumerable.ToList<string>(hashSet);
			list.Sort(StringComparer.OrdinalIgnoreCase);
			return list.AsReadOnly();
		}

		// Token: 0x0400055D RID: 1373
		private readonly TagCacheService _tagCache;

		// Token: 0x0400055E RID: 1374
		private readonly object _filterLock = new object();

		// Token: 0x0400055F RID: 1375
		private readonly HashSet<string> _selectedInstalledTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000560 RID: 1376
		private volatile IReadOnlyList<string> _cachedSelectedInstalledTags = Array.Empty<string>();

		// Token: 0x04000561 RID: 1377
		private volatile IReadOnlyList<string> _installedAvailableTags = Array.Empty<string>();

		// Token: 0x04000562 RID: 1378
		private volatile int _filterVersion;
	}
}
