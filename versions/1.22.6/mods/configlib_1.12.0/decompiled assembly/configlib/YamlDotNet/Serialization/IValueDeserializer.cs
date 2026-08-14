using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000A6 RID: 166
	[NullableContext(1)]
	public interface IValueDeserializer
	{
		// Token: 0x060003FB RID: 1019
		[return: Nullable(2)]
		object DeserializeValue(IParser parser, Type expectedType, SerializerState state, IValueDeserializer nestedObjectDeserializer);
	}
}
