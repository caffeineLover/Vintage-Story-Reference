using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x020000FE RID: 254
	[NullableContext(2)]
	public interface ITypeConverter
	{
		// Token: 0x060005C2 RID: 1474
		object ChangeType(object value, [Nullable(1)] Type expectedType);
	}
}
