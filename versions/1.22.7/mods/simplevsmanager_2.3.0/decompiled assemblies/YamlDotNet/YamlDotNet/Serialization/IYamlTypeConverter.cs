using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200002F RID: 47
	[NullableContext(1)]
	public interface IYamlTypeConverter
	{
		// Token: 0x060000FD RID: 253
		bool Accepts(Type type);

		// Token: 0x060000FE RID: 254
		[return: Nullable(2)]
		object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer);

		// Token: 0x060000FF RID: 255
		void WriteYaml(IEmitter emitter, [Nullable(2)] object value, Type type, ObjectSerializer serializer);
	}
}
