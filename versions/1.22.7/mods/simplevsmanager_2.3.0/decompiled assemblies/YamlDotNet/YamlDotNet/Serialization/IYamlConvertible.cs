using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200002B RID: 43
	[NullableContext(1)]
	public interface IYamlConvertible
	{
		// Token: 0x060000F1 RID: 241
		void Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer);

		// Token: 0x060000F2 RID: 242
		void Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer);
	}
}
