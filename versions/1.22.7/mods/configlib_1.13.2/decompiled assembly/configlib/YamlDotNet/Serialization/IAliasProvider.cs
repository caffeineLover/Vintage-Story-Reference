using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000090 RID: 144
	[NullableContext(1)]
	public interface IAliasProvider
	{
		// Token: 0x06000379 RID: 889
		AnchorName GetAlias(object target);
	}
}
