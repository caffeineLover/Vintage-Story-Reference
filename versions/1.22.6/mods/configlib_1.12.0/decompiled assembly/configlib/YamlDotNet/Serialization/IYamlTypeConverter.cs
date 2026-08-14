using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000AD RID: 173
	[NullableContext(1)]
	public interface IYamlTypeConverter
	{
		// Token: 0x0600040B RID: 1035
		bool Accepts(Type type);

		// Token: 0x0600040C RID: 1036
		[return: Nullable(2)]
		object ReadYaml(IParser parser, Type type);

		// Token: 0x0600040D RID: 1037
		void WriteYaml(IEmitter emitter, [Nullable(2)] object value, Type type);
	}
}
