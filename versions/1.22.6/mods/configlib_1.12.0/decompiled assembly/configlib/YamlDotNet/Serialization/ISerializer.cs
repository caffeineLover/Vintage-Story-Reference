using System;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000A3 RID: 163
	[NullableContext(1)]
	public interface ISerializer
	{
		// Token: 0x060003F2 RID: 1010
		string Serialize([Nullable(2)] object graph);

		// Token: 0x060003F3 RID: 1011
		string Serialize([Nullable(2)] object graph, Type type);

		// Token: 0x060003F4 RID: 1012
		void Serialize(TextWriter writer, [Nullable(2)] object graph);

		// Token: 0x060003F5 RID: 1013
		void Serialize(TextWriter writer, [Nullable(2)] object graph, Type type);

		// Token: 0x060003F6 RID: 1014
		void Serialize(IEmitter emitter, [Nullable(2)] object graph);

		// Token: 0x060003F7 RID: 1015
		void Serialize(IEmitter emitter, [Nullable(2)] object graph, Type type);
	}
}
