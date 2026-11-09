using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200015D RID: 349
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class BlockTagRule
	{
		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06001158 RID: 4440 RVA: 0x00053301 File Offset: 0x00051501
		public string[] Tags { get; }

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06001159 RID: 4441 RVA: 0x00053309 File Offset: 0x00051509
		public TagSet TagSet { get; }

		// Token: 0x0600115A RID: 4442 RVA: 0x00053314 File Offset: 0x00051514
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

		// Token: 0x0600115B RID: 4443 RVA: 0x00053390 File Offset: 0x00051590
		private BlockTagRule(string[] tags)
		{
			this.Tags = (tags ?? Array.Empty<string>());
			this.TagSet = TagSet.Empty;
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x000533B4 File Offset: 0x000515B4
		public static bool ContainsAllFromAtLeastOne(IEnumerable<string> sample, IEnumerable<BlockTagRule> rules)
		{
			HashSet<string> sampleSet = new HashSet<string>(sample ?? Array.Empty<string>());
			return Enumerable.Any<BlockTagRule>(rules, (BlockTagRule rule) => rule.Tags.Length != 0 && Enumerable.All<string>(rule.Tags, new Func<string, bool>(sampleSet.Contains)));
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x000533F0 File Offset: 0x000515F0
		public static bool ContainsAllFromAtLeastOne(TagSet sample, IEnumerable<BlockTagRule> rules)
		{
			return Enumerable.Any<BlockTagRule>(rules, (BlockTagRule rule) => rule.Tags.Length != 0 && !rule.TagSet.IsEmpty && rule.TagSet.IsFullyContainedIn(ref sample));
		}

		// Token: 0x04000800 RID: 2048
		public static readonly BlockTagRule Empty = new BlockTagRule(Array.Empty<string>());
	}
}
