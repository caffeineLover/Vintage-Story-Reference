using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000099 RID: 153
	[NullableContext(1)]
	public interface INodeTypeResolver
	{
		// Token: 0x060003C7 RID: 967
		bool Resolve([Nullable(2)] NodeEvent nodeEvent, ref Type currentType);
	}
}
