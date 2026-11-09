using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x0200004B RID: 75
	[NullableContext(1)]
	public interface ITypeConverter
	{
		// Token: 0x06000213 RID: 531
		[return: Nullable(2)]
		object ChangeType([Nullable(2)] object value, Type expectedType, INamingConvention enumNamingConvention, ITypeInspector typeInspector);
	}
}
