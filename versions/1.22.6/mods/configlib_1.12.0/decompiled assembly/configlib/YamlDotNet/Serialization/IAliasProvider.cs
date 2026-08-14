using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000094 RID: 148
	[NullableContext(1)]
	public interface IAliasProvider
	{
		// Token: 0x060003B5 RID: 949
		AnchorName GetAlias(object target);
	}
}
