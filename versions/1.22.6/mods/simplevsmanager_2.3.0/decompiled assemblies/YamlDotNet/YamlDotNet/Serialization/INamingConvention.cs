using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000019 RID: 25
	[NullableContext(1)]
	public interface INamingConvention
	{
		// Token: 0x060000B1 RID: 177
		string Apply(string value);

		// Token: 0x060000B2 RID: 178
		string Reverse(string value);
	}
}
