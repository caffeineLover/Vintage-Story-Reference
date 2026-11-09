using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeTypeResolvers
{
	// Token: 0x0200013B RID: 315
	public sealed class YamlSerializableTypeResolver : INodeTypeResolver
	{
		// Token: 0x06000689 RID: 1673 RVA: 0x00016B39 File Offset: 0x00014D39
		[NullableContext(1)]
		public bool Resolve([Nullable(2)] NodeEvent nodeEvent, ref Type currentType)
		{
			return typeof(IYamlSerializable).IsAssignableFrom(currentType);
		}
	}
}
