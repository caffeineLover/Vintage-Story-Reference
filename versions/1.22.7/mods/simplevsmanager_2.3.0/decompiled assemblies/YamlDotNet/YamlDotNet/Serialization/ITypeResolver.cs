using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000027 RID: 39
	[NullableContext(1)]
	public interface ITypeResolver
	{
		// Token: 0x060000EC RID: 236
		Type Resolve(Type staticType, [Nullable(2)] object actualValue);
	}
}
