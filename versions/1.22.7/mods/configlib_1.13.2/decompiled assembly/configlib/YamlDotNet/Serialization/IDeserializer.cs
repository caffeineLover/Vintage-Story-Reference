using System;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000091 RID: 145
	[NullableContext(1)]
	public interface IDeserializer
	{
		// Token: 0x0600037A RID: 890
		T Deserialize<[Nullable(2)] T>(string input);

		// Token: 0x0600037B RID: 891
		T Deserialize<[Nullable(2)] T>(TextReader input);

		// Token: 0x0600037C RID: 892
		T Deserialize<[Nullable(2)] T>(IParser parser);

		// Token: 0x0600037D RID: 893
		[return: Nullable(2)]
		object Deserialize(string input);

		// Token: 0x0600037E RID: 894
		[return: Nullable(2)]
		object Deserialize(TextReader input);

		// Token: 0x0600037F RID: 895
		[return: Nullable(2)]
		object Deserialize(IParser parser);

		// Token: 0x06000380 RID: 896
		[return: Nullable(2)]
		object Deserialize(string input, Type type);

		// Token: 0x06000381 RID: 897
		[return: Nullable(2)]
		object Deserialize(TextReader input, Type type);

		// Token: 0x06000382 RID: 898
		[return: Nullable(2)]
		object Deserialize(IParser parser, Type type);
	}
}
