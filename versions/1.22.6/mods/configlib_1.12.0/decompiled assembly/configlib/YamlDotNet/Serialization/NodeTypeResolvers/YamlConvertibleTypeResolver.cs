using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeTypeResolvers
{
	// Token: 0x0200013E RID: 318
	public sealed class YamlConvertibleTypeResolver : INodeTypeResolver
	{
		// Token: 0x060006C3 RID: 1731 RVA: 0x0001746E File Offset: 0x0001566E
		[NullableContext(1)]
		public bool Resolve([Nullable(2)] NodeEvent nodeEvent, ref Type currentType)
		{
			return typeof(IYamlConvertible).IsAssignableFrom(currentType);
		}
	}
}
