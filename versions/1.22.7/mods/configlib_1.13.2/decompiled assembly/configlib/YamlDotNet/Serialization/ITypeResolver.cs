using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000A1 RID: 161
	[NullableContext(1)]
	public interface ITypeResolver
	{
		// Token: 0x060003BE RID: 958
		Type Resolve(Type staticType, [Nullable(2)] object actualValue);
	}
}
