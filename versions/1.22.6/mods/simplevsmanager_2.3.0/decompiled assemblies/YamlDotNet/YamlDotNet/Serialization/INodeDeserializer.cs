using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200001A RID: 26
	[NullableContext(1)]
	public interface INodeDeserializer
	{
		// Token: 0x060000B3 RID: 179
		bool Deserialize(IParser reader, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value, ObjectDeserializer rootDeserializer);
	}
}
