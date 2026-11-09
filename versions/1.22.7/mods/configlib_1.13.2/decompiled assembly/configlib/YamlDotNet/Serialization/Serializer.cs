using System;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000BB RID: 187
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Serializer : ISerializer
	{
		// Token: 0x06000421 RID: 1057 RVA: 0x00010C51 File Offset: 0x0000EE51
		public Serializer() : this(new SerializerBuilder().BuildValueSerializer(), EmitterSettings.Default)
		{
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00010C68 File Offset: 0x0000EE68
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

		// Token: 0x06000423 RID: 1059 RVA: 0x00010C9C File Offset: 0x0000EE9C
		public static Serializer FromValueSerializer(IValueSerializer valueSerializer, EmitterSettings emitterSettings)
		{
			return new Serializer(valueSerializer, emitterSettings);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00010CA8 File Offset: 0x0000EEA8
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

		// Token: 0x06000425 RID: 1061 RVA: 0x00010CE8 File Offset: 0x0000EEE8
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

		// Token: 0x06000426 RID: 1062 RVA: 0x00010D28 File Offset: 0x0000EF28
		public void Serialize(TextWriter writer, [Nullable(2)] object graph)
		{
			this.Serialize(new Emitter(writer, this.emitterSettings), graph);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00010D3D File Offset: 0x0000EF3D
		public void Serialize(TextWriter writer, [Nullable(2)] object graph, Type type)
		{
			this.Serialize(new Emitter(writer, this.emitterSettings), graph, type);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00010D53 File Offset: 0x0000EF53
		public void Serialize(IEmitter emitter, [Nullable(2)] object graph)
		{
			if (emitter == null)
			{
				throw new ArgumentNullException("emitter");
			}
			this.EmitDocument(emitter, graph, null);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00010D6C File Offset: 0x0000EF6C
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

		// Token: 0x0600042A RID: 1066 RVA: 0x00010D99 File Offset: 0x0000EF99
		[NullableContext(2)]
		private void EmitDocument([Nullable(1)] IEmitter emitter, object graph, Type type)
		{
			emitter.Emit(new StreamStart());
			emitter.Emit(new DocumentStart());
			this.valueSerializer.SerializeValue(emitter, graph, type);
			emitter.Emit(new DocumentEnd(true));
			emitter.Emit(new StreamEnd());
		}

		// Token: 0x040001C5 RID: 453
		private readonly IValueSerializer valueSerializer;

		// Token: 0x040001C6 RID: 454
		private readonly EmitterSettings emitterSettings;
	}
}
