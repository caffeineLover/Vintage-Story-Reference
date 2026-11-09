using System;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000036 RID: 54
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Serializer : ISerializer
	{
		// Token: 0x06000131 RID: 305 RVA: 0x00003E84 File Offset: 0x00002084
		public Serializer() : this(new SerializerBuilder().BuildValueSerializer(), EmitterSettings.Default)
		{
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00003E9B File Offset: 0x0000209B
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

		// Token: 0x06000133 RID: 307 RVA: 0x00003ECF File Offset: 0x000020CF
		public static Serializer FromValueSerializer(IValueSerializer valueSerializer, EmitterSettings emitterSettings)
		{
			return new Serializer(valueSerializer, emitterSettings);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00003ED8 File Offset: 0x000020D8
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

		// Token: 0x06000135 RID: 309 RVA: 0x00003F18 File Offset: 0x00002118
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

		// Token: 0x06000136 RID: 310 RVA: 0x00003F58 File Offset: 0x00002158
		public void Serialize(TextWriter writer, [Nullable(2)] object graph)
		{
			this.Serialize(new Emitter(writer, this.emitterSettings), graph);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00003F6D File Offset: 0x0000216D
		public void Serialize(TextWriter writer, [Nullable(2)] object graph, Type type)
		{
			this.Serialize(new Emitter(writer, this.emitterSettings), graph, type);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00003F83 File Offset: 0x00002183
		public void Serialize(IEmitter emitter, [Nullable(2)] object graph)
		{
			if (emitter == null)
			{
				throw new ArgumentNullException("emitter");
			}
			this.EmitDocument(emitter, graph, null);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00003F9C File Offset: 0x0000219C
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

		// Token: 0x0600013A RID: 314 RVA: 0x00003FC9 File Offset: 0x000021C9
		[NullableContext(2)]
		private void EmitDocument([Nullable(1)] IEmitter emitter, object graph, Type type)
		{
			emitter.Emit(new StreamStart());
			emitter.Emit(new DocumentStart());
			this.valueSerializer.SerializeValue(emitter, graph, type);
			emitter.Emit(new DocumentEnd(true));
			emitter.Emit(new StreamEnd());
		}

		// Token: 0x0400003B RID: 59
		private readonly IValueSerializer valueSerializer;

		// Token: 0x0400003C RID: 60
		private readonly EmitterSettings emitterSettings;
	}
}
