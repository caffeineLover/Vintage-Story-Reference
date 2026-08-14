using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x02000101 RID: 257
	public class ReflectionTypeConverter : ITypeConverter
	{
		// Token: 0x060005C9 RID: 1481 RVA: 0x00014C2A File Offset: 0x00012E2A
		[NullableContext(2)]
		public object ChangeType(object value, [Nullable(1)] Type expectedType)
		{
			return TypeConverter.ChangeType(value, expectedType);
		}
	}
}
