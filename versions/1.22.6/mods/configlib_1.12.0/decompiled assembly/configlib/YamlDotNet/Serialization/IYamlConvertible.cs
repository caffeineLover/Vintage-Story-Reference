using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000A9 RID: 169
	[NullableContext(1)]
	public interface IYamlConvertible
	{
		// Token: 0x060003FF RID: 1023
		void Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer);

		// Token: 0x06000400 RID: 1024
		void Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer);
	}
}
