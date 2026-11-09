using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200015C RID: 348
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ItemTagRule
	{
		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06001151 RID: 4433 RVA: 0x000531D8 File Offset: 0x000513D8
		public string[] Tags { get; }

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06001152 RID: 4434 RVA: 0x000531E0 File Offset: 0x000513E0
		public TagSet TagSet { get; }

		// Token: 0x06001153 RID: 4435 RVA: 0x000531E8 File Offset: 0x000513E8
		public ItemTagRule(ICoreAPI api, string[] tags)
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

		// Token: 0x06001154 RID: 4436 RVA: 0x00053264 File Offset: 0x00051464
		private ItemTagRule(string[] tags)
		{
			this.Tags = (tags ?? Array.Empty<string>());
			this.TagSet = TagSet.Empty;
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x00053288 File Offset: 0x00051488
		public static bool ContainsAllFromAtLeastOne(IEnumerable<string> sample, IEnumerable<ItemTagRule> rules)
		{
			HashSet<string> sampleSet = new HashSet<string>(sample ?? Array.Empty<string>());
			return Enumerable.Any<ItemTagRule>(rules, (ItemTagRule rule) => rule.Tags.Length != 0 && Enumerable.All<string>(rule.Tags, new Func<string, bool>(sampleSet.Contains)));
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x000532C4 File Offset: 0x000514C4
		public static bool ContainsAllFromAtLeastOne(TagSet sample, IEnumerable<ItemTagRule> rules)
		{
			return Enumerable.Any<ItemTagRule>(rules, (ItemTagRule rule) => rule.Tags.Length != 0 && !rule.TagSet.IsEmpty && rule.TagSet.IsFullyContainedIn(ref sample));
		}

		// Token: 0x040007FD RID: 2045
		public static readonly ItemTagRule Empty = new ItemTagRule(Array.Empty<string>());
	}
}
