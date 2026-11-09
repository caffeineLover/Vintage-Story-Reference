using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeTypeResolvers
{
	// Token: 0x02000138 RID: 312
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TagNodeTypeResolver : INodeTypeResolver
	{
		// Token: 0x06000683 RID: 1667 RVA: 0x00016A7F File Offset: 0x00014C7F
		public TagNodeTypeResolver(IDictionary<TagName, Type> tagMappings)
		{
			if (tagMappings == null)
			{
				throw new ArgumentNullException("tagMappings");
			}
			this.tagMappings = tagMappings;
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00016AA0 File Offset: 0x00014CA0
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

		// Token: 0x040002CB RID: 715
		private readonly IDictionary<TagName, Type> tagMappings;
	}
}
