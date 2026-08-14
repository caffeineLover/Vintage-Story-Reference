using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.TypeResolvers
{
	// Token: 0x0200010A RID: 266
	public sealed class DynamicTypeResolver : ITypeResolver
	{
		// Token: 0x060005EC RID: 1516 RVA: 0x0001543B File Offset: 0x0001363B
		[NullableContext(1)]
		public Type Resolve(Type staticType, [Nullable(2)] object actualValue)
		{
			if (actualValue == null)
			{
				return staticType;
			}
			return actualValue.GetType();
		}
	}
}
