using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000A5 RID: 165
	[NullableContext(1)]
	public interface IYamlConvertible
	{
		// Token: 0x060003C3 RID: 963
		void Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer);

		// Token: 0x060003C4 RID: 964
		void Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer);
	}
}
