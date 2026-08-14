using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200002E RID: 46
	[NullableContext(1)]
	[Obsolete("Please use IYamlConvertible instead")]
	public interface IYamlSerializable
	{
		// Token: 0x060000FB RID: 251
		void ReadYaml(IParser parser);

		// Token: 0x060000FC RID: 252
		void WriteYaml(IEmitter emitter);
	}
}
