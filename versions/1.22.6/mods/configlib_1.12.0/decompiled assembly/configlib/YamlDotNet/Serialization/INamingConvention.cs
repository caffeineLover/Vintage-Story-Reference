using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000097 RID: 151
	[NullableContext(1)]
	public interface INamingConvention
	{
		// Token: 0x060003C5 RID: 965
		string Apply(string value);
	}
}
