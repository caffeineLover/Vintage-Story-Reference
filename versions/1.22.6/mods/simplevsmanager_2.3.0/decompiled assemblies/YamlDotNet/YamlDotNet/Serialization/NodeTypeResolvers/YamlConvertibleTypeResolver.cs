using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeTypeResolvers
{
	// Token: 0x02000076 RID: 118
	public sealed class YamlConvertibleTypeResolver : INodeTypeResolver
	{
		// Token: 0x060002D3 RID: 723 RVA: 0x000097DE File Offset: 0x000079DE
		[NullableContext(1)]
		public bool Resolve([Nullable(2)] NodeEvent nodeEvent, ref Type currentType)
		{
			return typeof(IYamlConvertible).IsAssignableFrom(currentType);
		}
	}
}
