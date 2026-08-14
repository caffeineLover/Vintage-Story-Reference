using System;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000017 RID: 23
	[NullableContext(1)]
	public interface IDeserializer
	{
		// Token: 0x060000A2 RID: 162
		T Deserialize<[Nullable(2)] T>(string input);

		// Token: 0x060000A3 RID: 163
		T Deserialize<[Nullable(2)] T>(TextReader input);

		// Token: 0x060000A4 RID: 164
		T Deserialize<[Nullable(2)] T>(IParser parser);

		// Token: 0x060000A5 RID: 165
		[return: Nullable(2)]
		object Deserialize(string input);

		// Token: 0x060000A6 RID: 166
		[return: Nullable(2)]
		object Deserialize(TextReader input);

		// Token: 0x060000A7 RID: 167
		[return: Nullable(2)]
		object Deserialize(IParser parser);

		// Token: 0x060000A8 RID: 168
		[return: Nullable(2)]
		object Deserialize(string input, Type type);

		// Token: 0x060000A9 RID: 169
		[return: Nullable(2)]
		object Deserialize(TextReader input, Type type);

		// Token: 0x060000AA RID: 170
		[return: Nullable(2)]
		object Deserialize(IParser parser, Type type);
	}
}
