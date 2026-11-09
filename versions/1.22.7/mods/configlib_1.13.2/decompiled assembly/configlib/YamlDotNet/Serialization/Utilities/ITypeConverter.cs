using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x020000FA RID: 250
	[NullableContext(2)]
	public interface ITypeConverter
	{
		// Token: 0x06000586 RID: 1414
		object ChangeType(object value, [Nullable(1)] Type expectedType);
	}
}
