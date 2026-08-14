using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000A8 RID: 168
	[NullableContext(2)]
	public interface IValueSerializer
	{
		// Token: 0x060003FE RID: 1022
		void SerializeValue([Nullable(1)] IEmitter emitter, object value, Type type);
	}
}
