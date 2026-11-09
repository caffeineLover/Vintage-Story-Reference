using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000093 RID: 147
	[NullableContext(1)]
	public interface INamingConvention
	{
		// Token: 0x06000389 RID: 905
		string Apply(string value);
	}
}
