using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200002A RID: 42
	[NullableContext(2)]
	public interface IValueSerializer
	{
		// Token: 0x060000F0 RID: 240
		void SerializeValue([Nullable(1)] IEmitter emitter, object value, Type type);
	}
}
