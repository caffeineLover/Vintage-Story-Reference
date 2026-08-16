using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000150 RID: 336
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ItemTagRule
	{
		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x060010DD RID: 4317 RVA: 0x000518D0 File Offset: 0x0004FAD0
		public string[] Tags { get; }

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x060010DE RID: 4318 RVA: 0x000518D8 File Offset: 0x0004FAD8
		public TagSet TagSet { get; }

		// Token: 0x060010DF RID: 4319 RVA: 0x000518E0 File Offset: 0x0004FAE0
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

		// Token: 0x060010E0 RID: 4320 RVA: 0x0005195C File Offset: 0x0004FB5C
		private ItemTagRule(string[] tags)
		{
			this.Tags = (tags ?? Array.Empty<string>());
			this.TagSet = TagSet.Empty;
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x00051980 File Offset: 0x0004FB80
		public static bool ContainsAllFromAtLeastOne(IEnumerable<string> sample, IEnumerable<ItemTagRule> rules)
		{
			HashSet<string> sampleSet = new HashSet<string>(sample ?? Array.Empty<string>());
			return Enumerable.Any<ItemTagRule>(rules, (ItemTagRule rule) => rule.Tags.Length != 0 && Enumerable.All<string>(rule.Tags, new Func<string, bool>(sampleSet.Contains)));
		}

		// Token: 0x060010E2 RID: 4322 RVA: 0x000519BC File Offset: 0x0004FBBC
		public static bool ContainsAllFromAtLeastOne(TagSet sample, IEnumerable<ItemTagRule> rules)
		{
			return Enumerable.Any<ItemTagRule>(rules, (ItemTagRule rule) => rule.Tags.Length != 0 && !rule.TagSet.IsEmpty && rule.TagSet.IsFullyContainedIn(ref sample));
		}

		// Token: 0x040007D7 RID: 2007
		public static readonly ItemTagRule Empty = new ItemTagRule(Array.Empty<string>());
	}
}
