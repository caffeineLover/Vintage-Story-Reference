using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x0200004C RID: 76
	public class NullTypeConverter : ITypeConverter
	{
		// Token: 0x06000214 RID: 532 RVA: 0x00006FA4 File Offset: 0x000051A4
		[NullableContext(1)]
		[return: Nullable(2)]
		public object ChangeType([Nullable(2)] object value, Type expectedType, INamingConvention enumNamingConvention, ITypeInspector typeInspector)
		{
			return value;
		}
	}
}
