using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeTypeResolvers
{
	// Token: 0x02000074 RID: 116
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TagNodeTypeResolver : INodeTypeResolver
	{
		// Token: 0x060002CF RID: 719 RVA: 0x0000972F File Offset: 0x0000792F
		public TagNodeTypeResolver(IDictionary<TagName, Type> tagMappings)
		{
			if (tagMappings == null)
			{
				throw new ArgumentNullException("tagMappings");
			}
			this.tagMappings = tagMappings;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x00009750 File Offset: 0x00007950
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

		// Token: 0x040000AF RID: 175
		private readonly IDictionary<TagName, Type> tagMappings;
	}
}
