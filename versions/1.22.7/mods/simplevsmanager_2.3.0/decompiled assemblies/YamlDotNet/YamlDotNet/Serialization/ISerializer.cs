using System;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000025 RID: 37
	[NullableContext(1)]
	public interface ISerializer
	{
		// Token: 0x060000E2 RID: 226
		string Serialize([Nullable(2)] object graph);

		// Token: 0x060000E3 RID: 227
		string Serialize([Nullable(2)] object graph, Type type);

		// Token: 0x060000E4 RID: 228
		void Serialize(TextWriter writer, [Nullable(2)] object graph);

		// Token: 0x060000E5 RID: 229
		void Serialize(TextWriter writer, [Nullable(2)] object graph, Type type);

		// Token: 0x060000E6 RID: 230
		void Serialize(IEmitter emitter, [Nullable(2)] object graph);

		// Token: 0x060000E7 RID: 231
		void Serialize(IEmitter emitter, [Nullable(2)] object graph, Type type);
	}
}
