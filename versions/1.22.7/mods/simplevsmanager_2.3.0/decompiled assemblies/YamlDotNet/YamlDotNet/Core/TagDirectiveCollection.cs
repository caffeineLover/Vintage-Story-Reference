using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Tokens;

namespace YamlDotNet.Core
{
	// Token: 0x020000DD RID: 221
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1,
		1
	})]
	public sealed class TagDirectiveCollection : KeyedCollection<string, TagDirective>
	{
		// Token: 0x060005C9 RID: 1481 RVA: 0x0001699A File Offset: 0x00014B9A
		public TagDirectiveCollection()
		{
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x000169A4 File Offset: 0x00014BA4
		public TagDirectiveCollection(IEnumerable<TagDirective> tagDirectives)
		{
			foreach (TagDirective tagDirective in tagDirectives)
			{
				base.Add(tagDirective);
			}
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x000169F4 File Offset: 0x00014BF4
		protected override string GetKeyForItem(TagDirective item)
		{
			return item.Handle;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x000169FC File Offset: 0x00014BFC
		public bool Contains(TagDirective directive)
		{
			return base.Contains(this.GetKeyForItem(directive));
		}
	}
}
