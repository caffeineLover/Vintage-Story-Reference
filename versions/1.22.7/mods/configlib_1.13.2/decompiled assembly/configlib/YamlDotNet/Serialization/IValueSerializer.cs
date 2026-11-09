using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000A4 RID: 164
	[NullableContext(2)]
	public interface IValueSerializer
	{
		// Token: 0x060003C2 RID: 962
		void SerializeValue([Nullable(1)] IEmitter emitter, object value, Type type);
	}
}
