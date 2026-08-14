using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.TypeResolvers
{
	// Token: 0x02000053 RID: 83
	public sealed class DynamicTypeResolver : ITypeResolver
	{
		// Token: 0x0600022F RID: 559 RVA: 0x000077B5 File Offset: 0x000059B5
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
