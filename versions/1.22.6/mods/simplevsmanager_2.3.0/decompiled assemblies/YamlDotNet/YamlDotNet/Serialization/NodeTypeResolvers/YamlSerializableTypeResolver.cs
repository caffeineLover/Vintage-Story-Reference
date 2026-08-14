using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeTypeResolvers
{
	// Token: 0x02000077 RID: 119
	public sealed class YamlSerializableTypeResolver : INodeTypeResolver
	{
		// Token: 0x060002D5 RID: 725 RVA: 0x000097F9 File Offset: 0x000079F9
		[NullableContext(1)]
		public bool Resolve([Nullable(2)] NodeEvent nodeEvent, ref Type currentType)
		{
			return typeof(IYamlSerializable).IsAssignableFrom(currentType);
		}
	}
}
