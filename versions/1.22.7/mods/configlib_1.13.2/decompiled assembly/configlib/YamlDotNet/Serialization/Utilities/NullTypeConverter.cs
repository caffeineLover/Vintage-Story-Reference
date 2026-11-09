using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x020000FB RID: 251
	public class NullTypeConverter : ITypeConverter
	{
		// Token: 0x06000587 RID: 1415 RVA: 0x0000976E File Offset: 0x0000796E
		[NullableContext(2)]
		public object ChangeType(object value, [Nullable(1)] Type expectedType)
		{
			return value;
		}
	}
}
