using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Tokens;

namespace YamlDotNet.Core
{
	// Token: 0x020001D0 RID: 464
	[NullableContext(1)]
	[Nullable(new byte[]
	{
		0,
		1,
		1
	})]
	public sealed class TagDirectiveCollection : KeyedCollection<string, TagDirective>
	{
		// Token: 0x06000A89 RID: 2697 RVA: 0x000252B8 File Offset: 0x000234B8
		public TagDirectiveCollection()
		{
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x000252C0 File Offset: 0x000234C0
		public TagDirectiveCollection(IEnumerable<TagDirective> tagDirectives)
		{
			foreach (TagDirective tagDirective in tagDirectives)
			{
				base.Add(tagDirective);
			}
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x00025310 File Offset: 0x00023510
		protected override string GetKeyForItem(TagDirective item)
		{
			return item.Handle;
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x00025318 File Offset: 0x00023518
		public bool Contains(TagDirective directive)
		{
			return base.Contains(this.GetKeyForItem(directive));
		}
	}
}
