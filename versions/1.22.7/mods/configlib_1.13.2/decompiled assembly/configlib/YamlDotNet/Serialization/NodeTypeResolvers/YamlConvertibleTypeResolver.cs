using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeTypeResolvers
{
	// Token: 0x0200013A RID: 314
	public sealed class YamlConvertibleTypeResolver : INodeTypeResolver
	{
		// Token: 0x06000687 RID: 1671 RVA: 0x00016B26 File Offset: 0x00014D26
		[NullableContext(1)]
		public bool Resolve([Nullable(2)] NodeEvent nodeEvent, ref Type currentType)
		{
			return typeof(IYamlConvertible).IsAssignableFrom(currentType);
		}
	}
}
