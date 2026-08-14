using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000028 RID: 40
	[NullableContext(1)]
	public interface IValueDeserializer
	{
		// Token: 0x060000ED RID: 237
		[return: Nullable(2)]
		object DeserializeValue(IParser parser, Type expectedType, SerializerState state, IValueDeserializer nestedObjectDeserializer);
	}
}
