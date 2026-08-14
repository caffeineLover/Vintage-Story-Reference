using System;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000BF RID: 191
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Serializer : ISerializer
	{
		// Token: 0x0600045D RID: 1117 RVA: 0x00011599 File Offset: 0x0000F799
		public Serializer() : this(new SerializerBuilder().BuildValueSerializer(), EmitterSettings.Default)
		{
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x000115B0 File Offset: 0x0000F7B0
		private Serializer(IValueSerializer valueSerializer, EmitterSettings emitterSettings)
		{
			if (valueSerializer == null)
			{
				throw new ArgumentNullException("valueSerializer");
			}
			this.valueSerializer = valueSerializer;
			if (emitterSettings == null)
			{
				throw new ArgumentNullException("emitterSettings");
			}
			this.emitterSettings = emitterSettings;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x000115E4 File Offset: 0x0000F7E4
		public static Serializer FromValueSerializer(IValueSerializer valueSerializer, EmitterSettings emitterSettings)
		{
			return new Serializer(valueSerializer, emitterSettings);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x000115F0 File Offset: 0x0000F7F0
		public string Serialize([Nullable(2)] object graph)
		{
			string result;
			using (StringWriter stringWriter = new StringWriter())
			{
				this.Serialize(stringWriter, graph);
				result = stringWriter.ToString();
			}
			return result;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00011630 File Offset: 0x0000F830
		public string Serialize([Nullable(2)] object graph, Type type)
		{
			string result;
			using (StringWriter stringWriter = new StringWriter())
			{
				this.Serialize(stringWriter, graph, type);
				result = stringWriter.ToString();
			}
			return result;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00011670 File Offset: 0x0000F870
		public void Serialize(TextWriter writer, [Nullable(2)] object graph)
		{
			this.Serialize(new Emitter(writer, this.emitterSettings), graph);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00011685 File Offset: 0x0000F885
		public void Serialize(TextWriter writer, [Nullable(2)] object graph, Type type)
		{
			this.Serialize(new Emitter(writer, this.emitterSettings), graph, type);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0001169B File Offset: 0x0000F89B
		public void Serialize(IEmitter emitter, [Nullable(2)] object graph)
		{
			if (emitter == null)
			{
				throw new ArgumentNullException("emitter");
			}
			this.EmitDocument(emitter, graph, null);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x000116B4 File Offset: 0x0000F8B4
		public void Serialize(IEmitter emitter, [Nullable(2)] object graph, Type type)
		{
			if (emitter == null)
			{
				throw new ArgumentNullException("emitter");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.EmitDocument(emitter, graph, type);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x000116E1 File Offset: 0x0000F8E1
		[NullableContext(2)]
		private void EmitDocument([Nullable(1)] IEmitter emitter, object graph, Type type)
		{
			emitter.Emit(new StreamStart());
			emitter.Emit(new DocumentStart());
			this.valueSerializer.SerializeValue(emitter, graph, type);
			emitter.Emit(new DocumentEnd(true));
			emitter.Emit(new StreamEnd());
		}

		// Token: 0x040001F3 RID: 499
		private readonly IValueSerializer valueSerializer;

		// Token: 0x040001F4 RID: 500
		private readonly EmitterSettings emitterSettings;
	}
}
