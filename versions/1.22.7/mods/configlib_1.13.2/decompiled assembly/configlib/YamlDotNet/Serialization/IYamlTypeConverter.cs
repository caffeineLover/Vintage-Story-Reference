using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000A9 RID: 169
	[NullableContext(1)]
	public interface IYamlTypeConverter
	{
		// Token: 0x060003CF RID: 975
		bool Accepts(Type type);

		// Token: 0x060003D0 RID: 976
		[return: Nullable(2)]
		object ReadYaml(IParser parser, Type type);

		// Token: 0x060003D1 RID: 977
		void WriteYaml(IEmitter emitter, [Nullable(2)] object value, Type type);
	}
}
