using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000A5 RID: 165
	[NullableContext(1)]
	public interface ITypeResolver
	{
		// Token: 0x060003FA RID: 1018
		Type Resolve(Type staticType, [Nullable(2)] object actualValue);
	}
}
