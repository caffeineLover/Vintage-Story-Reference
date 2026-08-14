using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeTypeResolvers
{
	// Token: 0x0200013F RID: 319
	public sealed class YamlSerializableTypeResolver : INodeTypeResolver
	{
		// Token: 0x060006C5 RID: 1733 RVA: 0x00017481 File Offset: 0x00015681
		[NullableContext(1)]
		public bool Resolve([Nullable(2)] NodeEvent nodeEvent, ref Type currentType)
		{
			return typeof(IYamlSerializable).IsAssignableFrom(currentType);
		}
	}
}
