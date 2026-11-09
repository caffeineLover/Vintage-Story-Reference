using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000095 RID: 149
	[NullableContext(1)]
	public interface INodeTypeResolver
	{
		// Token: 0x0600038B RID: 907
		bool Resolve([Nullable(2)] NodeEvent nodeEvent, ref Type currentType);
	}
}
