using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x020000FD RID: 253
	public class ReflectionTypeConverter : ITypeConverter
	{
		// Token: 0x0600058D RID: 1421 RVA: 0x000142E2 File Offset: 0x000124E2
		[NullableContext(2)]
		public object ChangeType(object value, [Nullable(1)] Type expectedType)
		{
			return TypeConverter.ChangeType(value, expectedType);
		}
	}
}
