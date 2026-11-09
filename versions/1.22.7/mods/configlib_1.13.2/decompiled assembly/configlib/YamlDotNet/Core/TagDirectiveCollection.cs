using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Tokens;

namespace YamlDotNet.Core
{
	// Token: 0x020001CC RID: 460
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1,
		1
	})]
	public sealed class TagDirectiveCollection : KeyedCollection<string, TagDirective>
	{
		// Token: 0x06000A4D RID: 2637 RVA: 0x00024970 File Offset: 0x00022B70
		public TagDirectiveCollection()
		{
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x00024978 File Offset: 0x00022B78
		public TagDirectiveCollection(IEnumerable<TagDirective> tagDirectives)
		{
			foreach (TagDirective tagDirective in tagDirectives)
			{
				base.Add(tagDirective);
			}
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x000249C8 File Offset: 0x00022BC8
		protected override string GetKeyForItem(TagDirective item)
		{
			return item.Handle;
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x000249D0 File Offset: 0x00022BD0
		public bool Contains(TagDirective directive)
		{
			return base.Contains(this.GetKeyForItem(directive));
		}
	}
}
