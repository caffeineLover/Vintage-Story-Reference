using System;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000095 RID: 149
	[NullableContext(1)]
	public interface IDeserializer
	{
		// Token: 0x060003B6 RID: 950
		T Deserialize<[Nullable(2)] T>(string input);

		// Token: 0x060003B7 RID: 951
		T Deserialize<[Nullable(2)] T>(TextReader input);

		// Token: 0x060003B8 RID: 952
		T Deserialize<[Nullable(2)] T>(IParser parser);

		// Token: 0x060003B9 RID: 953
		[return: Nullable(2)]
		object Deserialize(string input);

		// Token: 0x060003BA RID: 954
		[return: Nullable(2)]
		object Deserialize(TextReader input);

		// Token: 0x060003BB RID: 955
		[return: Nullable(2)]
		object Deserialize(IParser parser);

		// Token: 0x060003BC RID: 956
		[return: Nullable(2)]
		object Deserialize(string input, Type type);

		// Token: 0x060003BD RID: 957
		[return: Nullable(2)]
		object Deserialize(TextReader input, Type type);

		// Token: 0x060003BE RID: 958
		[return: Nullable(2)]
		object Deserialize(IParser parser, Type type);
	}
}
