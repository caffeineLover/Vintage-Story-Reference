using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x020000FF RID: 255
	public class NullTypeConverter : ITypeConverter
	{
		// Token: 0x060005C3 RID: 1475 RVA: 0x00009452 File Offset: 0x00007652
		[NullableContext(2)]
		public object ChangeType(object value, [Nullable(1)] Type expectedType)
		{
			return value;
		}
	}
}
