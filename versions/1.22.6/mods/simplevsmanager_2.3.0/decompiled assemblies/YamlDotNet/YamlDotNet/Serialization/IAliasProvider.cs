using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000016 RID: 22
	[NullableContext(1)]
	public interface IAliasProvider
	{
		// Token: 0x060000A1 RID: 161
		AnchorName GetAlias(object target);
	}
}
