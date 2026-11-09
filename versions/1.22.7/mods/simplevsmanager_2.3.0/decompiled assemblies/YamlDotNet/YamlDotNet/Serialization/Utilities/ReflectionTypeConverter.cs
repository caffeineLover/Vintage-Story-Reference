using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.NamingConventions;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x0200004E RID: 78
	public class ReflectionTypeConverter : ITypeConverter
	{
		// Token: 0x0600021A RID: 538 RVA: 0x00007032 File Offset: 0x00005232
		[NullableContext(1)]
		[return: Nullable(2)]
		public object ChangeType([Nullable(2)] object value, Type expectedType, ITypeInspector typeInspector)
		{
			return this.ChangeType(value, expectedType, NullNamingConvention.Instance, typeInspector);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00007042 File Offset: 0x00005242
		[NullableContext(1)]
		[return: Nullable(2)]
		public object ChangeType([Nullable(2)] object value, Type expectedType, INamingConvention enumNamingConvention, ITypeInspector typeInspector)
		{
			return TypeConverter.ChangeType(value, expectedType, enumNamingConvention, typeInspector);
		}
	}
}
