using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeTypeResolvers
{
	// Token: 0x02000139 RID: 313
	public sealed class DefaultContainersNodeTypeResolver : INodeTypeResolver
	{
		// Token: 0x060006B9 RID: 1721 RVA: 0x0001721C File Offset: 0x0001541C
		[NullableContext(1)]
		bool INodeTypeResolver.Resolve([Nullable(2)] NodeEvent nodeEvent, ref Type currentType)
		{
			if (currentType == typeof(object))
			{
				if (nodeEvent is SequenceStart)
				{
					currentType = typeof(List<object>);
					return true;
				}
				if (nodeEvent is MappingStart)
				{
					currentType = typeof(Dictionary<object, object>);
					return true;
				}
			}
			return false;
		}
	}
}
