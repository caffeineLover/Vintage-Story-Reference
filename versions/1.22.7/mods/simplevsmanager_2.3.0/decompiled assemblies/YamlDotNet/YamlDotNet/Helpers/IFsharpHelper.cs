using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization;

namespace YamlDotNet.Helpers
{
	// Token: 0x020000B9 RID: 185
	[NullableContext(1)]
	public interface IFsharpHelper
	{
		// Token: 0x06000451 RID: 1105
		bool IsOptionType(Type t);

		// Token: 0x06000452 RID: 1106
		[return: Nullable(2)]
		Type GetOptionUnderlyingType(Type t);

		// Token: 0x06000453 RID: 1107
		[return: Nullable(2)]
		object GetValue(IObjectDescriptor objectDescriptor);

		// Token: 0x06000454 RID: 1108
		bool IsFsharpListType(Type t);

		// Token: 0x06000455 RID: 1109
		[return: Nullable(2)]
		object CreateFsharpListFromArray(Type t, Type itemsType, Array arr);
	}
}
