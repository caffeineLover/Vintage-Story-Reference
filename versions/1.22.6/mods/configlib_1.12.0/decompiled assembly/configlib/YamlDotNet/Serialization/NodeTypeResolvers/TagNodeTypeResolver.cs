using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeTypeResolvers
{
	// Token: 0x0200013C RID: 316
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TagNodeTypeResolver : INodeTypeResolver
	{
		// Token: 0x060006BF RID: 1727 RVA: 0x000173C7 File Offset: 0x000155C7
		public TagNodeTypeResolver(IDictionary<TagName, Type> tagMappings)
		{
			if (tagMappings == null)
			{
				throw new ArgumentNullException("tagMappings");
			}
			this.tagMappings = tagMappings;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x000173E8 File Offset: 0x000155E8
		bool INodeTypeResolver.Resolve([Nullable(2)] NodeEvent nodeEvent, ref Type currentType)
		{
			Type type;
			if (nodeEvent != null && !nodeEvent.Tag.IsEmpty && this.tagMappings.TryGetValue(nodeEvent.Tag, ref type))
			{
				currentType = type;
				return true;
			}
			return false;
		}

		// Token: 0x040002F9 RID: 761
		private readonly IDictionary<TagName, Type> tagMappings;
	}
}
