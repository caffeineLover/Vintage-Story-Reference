using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200008D RID: 141
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class TagCacheService
	{
		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x00047D1A File Offset: 0x00045F1A
		public int Version
		{
			get
			{
				return this._cacheVersion;
			}
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x00047D24 File Offset: 0x00045F24
		public TagCacheService.TagSet GetOrCreateModTags(string modId)
		{
			if (string.IsNullOrWhiteSpace(modId))
			{
				return TagCacheService.TagSet.Empty;
			}
			return this._modTagCache.GetOrAdd(modId, (string _) => new TagCacheService.TagSet());
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x00047D60 File Offset: 0x00045F60
		public void SetModTags(string modId, [Nullable(new byte[]
		{
			2,
			1
		})] IReadOnlyList<string> tags)
		{
			if (string.IsNullOrWhiteSpace(modId))
			{
				return;
			}
			this.GetOrCreateModTags(modId).SetTags(tags);
			if (tags != null && tags.Count > 0)
			{
				foreach (string text in tags)
				{
					if (!string.IsNullOrWhiteSpace(text))
					{
						string text2 = text.Trim();
						if (text2.Length != 0)
						{
							this._globalTagIndex.TryAdd(text2.ToLowerInvariant(), text2);
						}
					}
				}
			}
			this.InvalidateSortedCache();
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x00047DF4 File Offset: 0x00045FF4
		public IReadOnlyList<string> GetModTags(string modId)
		{
			if (string.IsNullOrWhiteSpace(modId))
			{
				return Array.Empty<string>();
			}
			TagCacheService.TagSet tagSet;
			if (!this._modTagCache.TryGetValue(modId, ref tagSet))
			{
				return Array.Empty<string>();
			}
			return tagSet.GetTags();
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x00047E30 File Offset: 0x00046030
		public bool ModHasAllTags(string modId, IReadOnlyList<string> requiredTags)
		{
			TagCacheService.TagSet tagSet;
			return requiredTags.Count == 0 || (!string.IsNullOrWhiteSpace(modId) && this._modTagCache.TryGetValue(modId, ref tagSet) && tagSet.ContainsAll(requiredTags));
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x00047E6C File Offset: 0x0004606C
		public IReadOnlyList<string> GetAllTagsSorted()
		{
			IReadOnlyList<string> sortedTagListCache = this._sortedTagListCache;
			if (sortedTagListCache != null)
			{
				return sortedTagListCache;
			}
			List<string> list = Enumerable.ToList<string>(this._globalTagIndex.Values);
			list.Sort(StringComparer.OrdinalIgnoreCase);
			this._sortedTagListCache = list.AsReadOnly();
			return this._sortedTagListCache;
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x00047EB9 File Offset: 0x000460B9
		public void Clear()
		{
			this._modTagCache.Clear();
			this._globalTagIndex.Clear();
			this.InvalidateSortedCache();
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x00047ED8 File Offset: 0x000460D8
		public void RemoveModTags(string modId)
		{
			if (string.IsNullOrWhiteSpace(modId))
			{
				return;
			}
			TagCacheService.TagSet tagSet;
			this._modTagCache.TryRemove(modId, ref tagSet);
			this.InvalidateSortedCache();
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x00047F04 File Offset: 0x00046104
		public void RebuildGlobalIndex()
		{
			this._globalTagIndex.Clear();
			foreach (TagCacheService.TagSet tagSet in this._modTagCache.Values)
			{
				foreach (string text in tagSet.GetTags())
				{
					if (!string.IsNullOrWhiteSpace(text))
					{
						string text2 = text.Trim();
						if (text2.Length != 0)
						{
							this._globalTagIndex.TryAdd(text2.ToLowerInvariant(), text2);
						}
					}
				}
			}
			this.InvalidateSortedCache();
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x00047FC0 File Offset: 0x000461C0
		private void InvalidateSortedCache()
		{
			this._sortedTagListCache = null;
			Interlocked.Increment(ref this._cacheVersion);
		}

		// Token: 0x04000559 RID: 1369
		private readonly ConcurrentDictionary<string, string> _globalTagIndex = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400055A RID: 1370
		private readonly ConcurrentDictionary<string, TagCacheService.TagSet> _modTagCache = new ConcurrentDictionary<string, TagCacheService.TagSet>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400055B RID: 1371
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private volatile IReadOnlyList<string> _sortedTagListCache;

		// Token: 0x0400055C RID: 1372
		private volatile int _cacheVersion;

		// Token: 0x02000306 RID: 774
		[Nullable(0)]
		public sealed class TagSet
		{
			// Token: 0x06001912 RID: 6418 RVA: 0x00091B0E File Offset: 0x0008FD0E
			public IReadOnlyList<string> GetTags()
			{
				return this._tags;
			}

			// Token: 0x06001913 RID: 6419 RVA: 0x00091B18 File Offset: 0x0008FD18
			public void SetTags([Nullable(new byte[]
			{
				2,
				1
			})] IReadOnlyList<string> tags)
			{
				object @lock = this._lock;
				lock (@lock)
				{
					if (tags == null || tags.Count == 0)
					{
						this._tags = Array.Empty<string>();
						this._tagLookup = null;
					}
					else
					{
						List<string> list = new List<string>(tags.Count);
						HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
						foreach (string text in tags)
						{
							if (!string.IsNullOrWhiteSpace(text))
							{
								string text2 = text.Trim();
								if (text2.Length != 0 && hashSet.Add(text2))
								{
									list.Add(text2);
								}
							}
						}
						if (list.Count == 0)
						{
							this._tags = Array.Empty<string>();
							this._tagLookup = null;
						}
						else
						{
							this._tags = list.ToArray();
							this._tagLookup = null;
						}
					}
				}
			}

			// Token: 0x06001914 RID: 6420 RVA: 0x00091C2C File Offset: 0x0008FE2C
			public bool ContainsAll(IReadOnlyList<string> requiredTags)
			{
				if (requiredTags.Count == 0)
				{
					return true;
				}
				IReadOnlyList<string> tags = this._tags;
				if (tags.Count == 0)
				{
					return false;
				}
				if (requiredTags.Count <= 3)
				{
					return TagCacheService.TagSet.ContainsAllLinear(tags, requiredTags);
				}
				return this.ContainsAllWithSet(tags, requiredTags);
			}

			// Token: 0x06001915 RID: 6421 RVA: 0x00091C70 File Offset: 0x0008FE70
			public bool Contains(string tag)
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
				IReadOnlyList<string> tags = this._tags;
				if (tags.Count == 0)
				{
					return false;
				}
				if (tags.Count <= 10)
				{
					using (IEnumerator<string> enumerator = tags.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (string.Equals(enumerator.Current, text, 5))
							{
								return true;
							}
						}
					}
					return false;
				}
				return this.EnsureLookup().Contains(text);
			}

			// Token: 0x06001916 RID: 6422 RVA: 0x00091D08 File Offset: 0x0008FF08
			private unsafe static bool ContainsAllLinear(IReadOnlyList<string> tags, IReadOnlyList<string> requiredTags)
			{
				int count = requiredTags.Count;
				Span<bool> span = new Span<bool>(stackalloc byte[(UIntPtr)count], count);
				int num = 0;
				foreach (string text in tags)
				{
					if (num == requiredTags.Count)
					{
						return true;
					}
					for (int i = 0; i < requiredTags.Count; i++)
					{
						if (!(*span[i]) && string.Equals(text, requiredTags[i], 5))
						{
							*span[i] = true;
							num++;
							break;
						}
					}
				}
				return num == requiredTags.Count;
			}

			// Token: 0x06001917 RID: 6423 RVA: 0x00091DBC File Offset: 0x0008FFBC
			private bool ContainsAllWithSet(IReadOnlyList<string> tags, IReadOnlyList<string> requiredTags)
			{
				HashSet<string> hashSet = this.EnsureLookup();
				foreach (string text in requiredTags)
				{
					if (!hashSet.Contains(text))
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x06001918 RID: 6424 RVA: 0x00091E14 File Offset: 0x00090014
			private HashSet<string> EnsureLookup()
			{
				HashSet<string> hashSet = this._tagLookup;
				if (hashSet != null)
				{
					return hashSet;
				}
				object @lock = this._lock;
				HashSet<string> result;
				lock (@lock)
				{
					hashSet = this._tagLookup;
					if (hashSet != null)
					{
						result = hashSet;
					}
					else
					{
						IReadOnlyList<string> tags = this._tags;
						hashSet = new HashSet<string>(tags.Count, StringComparer.OrdinalIgnoreCase);
						foreach (string text in tags)
						{
							hashSet.Add(text);
						}
						this._tagLookup = hashSet;
						result = hashSet;
					}
				}
				return result;
			}

			// Token: 0x04001350 RID: 4944
			public static readonly TagCacheService.TagSet Empty = new TagCacheService.TagSet();

			// Token: 0x04001351 RID: 4945
			private readonly object _lock = new object();

			// Token: 0x04001352 RID: 4946
			private volatile IReadOnlyList<string> _tags = Array.Empty<string>();

			// Token: 0x04001353 RID: 4947
			[Nullable(new byte[]
			{
				2,
				1
			})]
			private volatile HashSet<string> _tagLookup;
		}
	}
}
