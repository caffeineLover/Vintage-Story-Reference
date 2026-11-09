using System;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200009F RID: 159
	[NullableContext(1)]
	public interface ISerializer
	{
		// Token: 0x060003B6 RID: 950
		string Serialize([Nullable(2)] object graph);

		// Token: 0x060003B7 RID: 951
		string Serialize([Nullable(2)] object graph, Type type);

		// Token: 0x060003B8 RID: 952
		void Serialize(TextWriter writer, [Nullable(2)] object graph);

		// Token: 0x060003B9 RID: 953
		void Serialize(TextWriter writer, [Nullable(2)] object graph, Type type);

		// Token: 0x060003BA RID: 954
		void Serialize(IEmitter emitter, [Nullable(2)] object graph);

		// Token: 0x060003BB RID: 955
		void Serialize(IEmitter emitter, [Nullable(2)] object graph, Type type);
	}
}
