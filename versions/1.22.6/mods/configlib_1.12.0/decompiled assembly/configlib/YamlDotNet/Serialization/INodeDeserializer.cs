using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000098 RID: 152
	[NullableContext(1)]
	public interface INodeDeserializer
	{
		// Token: 0x060003C6 RID: 966
		bool Deserialize(IParser reader, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value);
	}
}
