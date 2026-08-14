using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeTypeResolvers
{
	// Token: 0x02000075 RID: 117
	[Obsolete("The mechanism that this class uses to specify type names is non-standard. Register the tags explicitly instead of using this convention.")]
	public sealed class TypeNameInTagNodeTypeResolver : INodeTypeResolver
	{
		// Token: 0x060002D1 RID: 721 RVA: 0x0000978C File Offset: 0x0000798C
		[NullableContext(1)]
		bool INodeTypeResolver.Resolve([Nullable(2)] NodeEvent nodeEvent, ref Type currentType)
		{
			if (nodeEvent != null && !nodeEvent.Tag.IsEmpty)
			{
				Type type = Type.GetType(nodeEvent.Tag.Value.Substring(1), false);
				if (type != null)
				{
					currentType = type;
					return true;
				}
			}
			return false;
		}
	}
}
