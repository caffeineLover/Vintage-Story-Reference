using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000151 RID: 337
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BlockTagRule
	{
		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x060010E4 RID: 4324 RVA: 0x000519F9 File Offset: 0x0004FBF9
		public string[] Tags { get; }

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x060010E5 RID: 4325 RVA: 0x00051A01 File Offset: 0x0004FC01
		public TagSet TagSet { get; }

		// Token: 0x060010E6 RID: 4326 RVA: 0x00051A0C File Offset: 0x0004FC0C
		public BlockTagRule(ICoreAPI api, string[] tags)
		{
			this.Tags = (tags ?? Array.Empty<string>());
			if (this.Tags.Length == 0)
			{
				this.TagSet = TagSet.Empty;
				return;
			}
			try
			{
				TagSet tagSet;
				api.CollectibleTagRegistry.TryRegisterAndCreateTagSetAndLogIssues(ref tagSet, this.Tags);
				this.TagSet = tagSet;
			}
			catch
			{
				this.TagSet = TagSet.Empty;
			}
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x00051A88 File Offset: 0x0004FC88
		private BlockTagRule(string[] tags)
		{
			this.Tags = (tags ?? Array.Empty<string>());
			this.TagSet = TagSet.Empty;
		}

		// Token: 0x060010E8 RID: 4328 RVA: 0x00051AAC File Offset: 0x0004FCAC
		public static bool ContainsAllFromAtLeastOne(IEnumerable<string> sample, IEnumerable<BlockTagRule> rules)
		{
			HashSet<string> sampleSet = new HashSet<string>(sample ?? Array.Empty<string>());
			return Enumerable.Any<BlockTagRule>(rules, (BlockTagRule rule) => rule.Tags.Length != 0 && Enumerable.All<string>(rule.Tags, new Func<string, bool>(sampleSet.Contains)));
		}

		// Token: 0x060010E9 RID: 4329 RVA: 0x00051AE8 File Offset: 0x0004FCE8
		public static bool ContainsAllFromAtLeastOne(TagSet sample, IEnumerable<BlockTagRule> rules)
		{
			return Enumerable.Any<BlockTagRule>(rules, (BlockTagRule rule) => rule.Tags.Length != 0 && !rule.TagSet.IsEmpty && rule.TagSet.IsFullyContainedIn(ref sample));
		}

		// Token: 0x040007DA RID: 2010
		public static readonly BlockTagRule Empty = new BlockTagRule(Array.Empty<string>());
	}
}
