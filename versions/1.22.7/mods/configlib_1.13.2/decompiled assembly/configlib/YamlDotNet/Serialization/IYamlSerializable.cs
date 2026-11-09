using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000A8 RID: 168
	[NullableContext(1)]
	[Obsolete("Please use IYamlConvertible instead")]
	public interface IYamlSerializable
	{
		// Token: 0x060003CD RID: 973
		void ReadYaml(IParser parser);

		// Token: 0x060003CE RID: 974
		void WriteYaml(IEmitter emitter);
	}
}
