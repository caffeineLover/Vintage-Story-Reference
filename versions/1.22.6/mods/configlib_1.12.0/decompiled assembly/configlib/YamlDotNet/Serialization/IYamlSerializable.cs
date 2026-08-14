using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000AC RID: 172
	[NullableContext(1)]
	[Obsolete("Please use IYamlConvertible instead")]
	public interface IYamlSerializable
	{
		// Token: 0x06000409 RID: 1033
		void ReadYaml(IParser parser);

		// Token: 0x0600040A RID: 1034
		void WriteYaml(IEmitter emitter);
	}
}
