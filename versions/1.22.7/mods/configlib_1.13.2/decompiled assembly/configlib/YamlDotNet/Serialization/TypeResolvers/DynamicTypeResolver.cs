using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.TypeResolvers
{
	// Token: 0x02000106 RID: 262
	public sealed class DynamicTypeResolver : ITypeResolver
	{
		// Token: 0x060005B0 RID: 1456 RVA: 0x00014AF3 File Offset: 0x00012CF3
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
