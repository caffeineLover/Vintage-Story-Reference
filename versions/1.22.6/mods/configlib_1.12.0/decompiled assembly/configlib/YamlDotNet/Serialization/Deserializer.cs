using System;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000083 RID: 131
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Deserializer : IDeserializer
	{
		// Token: 0x06000342 RID: 834 RVA: 0x0000FE41 File Offset: 0x0000E041
		public Deserializer() : this(new DeserializerBuilder().BuildValueDeserializer())
		{
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000FE53 File Offset: 0x0000E053
		private Deserializer(IValueDeserializer valueDeserializer)
		{
			if (valueDeserializer == null)
			{
				throw new ArgumentNullException("valueDeserializer");
			}
			this.valueDeserializer = valueDeserializer;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000FE71 File Offset: 0x0000E071
		public static Deserializer FromValueDeserializer(IValueDeserializer valueDeserializer)
		{
			return new Deserializer(valueDeserializer);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000FE7C File Offset: 0x0000E07C
		public T Deserialize<[Nullable(2)] T>(string input)
		{
			T result;
			using (StringReader stringReader = new StringReader(input))
			{
				result = this.Deserialize<T>(stringReader);
			}
			return result;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000FEB8 File Offset: 0x0000E0B8
		public T Deserialize<[Nullable(2)] T>(TextReader input)
		{
			return this.Deserialize<T>(new Parser(input));
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000FEC6 File Offset: 0x0000E0C6
		public T Deserialize<[Nullable(2)] T>(IParser parser)
		{
			return (T)((object)this.Deserialize(parser, typeof(T)));
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000FEDE File Offset: 0x0000E0DE
		[return: Nullable(2)]
		public object Deserialize(string input)
		{
			return this.Deserialize(input, typeof(object));
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000FEF1 File Offset: 0x0000E0F1
		[return: Nullable(2)]
		public object Deserialize(TextReader input)
		{
			return this.Deserialize(input, typeof(object));
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000FF04 File Offset: 0x0000E104
		[return: Nullable(2)]
		public object Deserialize(IParser parser)
		{
			return this.Deserialize(parser, typeof(object));
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000FF18 File Offset: 0x0000E118
		[return: Nullable(2)]
		public object Deserialize(string input, Type type)
		{
			object result;
			using (StringReader stringReader = new StringReader(input))
			{
				result = this.Deserialize(stringReader, type);
			}
			return result;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000FF54 File Offset: 0x0000E154
		[return: Nullable(2)]
		public object Deserialize(TextReader input, Type type)
		{
			return this.Deserialize(new Parser(input), type);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000FF64 File Offset: 0x0000E164
		[return: Nullable(2)]
		public object Deserialize(IParser parser, Type type)
		{
			if (parser == null)
			{
				throw new ArgumentNullException("parser");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			StreamStart streamStart;
			bool flag = parser.TryConsume(out streamStart);
			DocumentStart documentStart;
			bool flag2 = parser.TryConsume(out documentStart);
			object result = null;
			DocumentEnd documentEnd;
			StreamEnd streamEnd;
			if (!parser.Accept(out documentEnd) && !parser.Accept(out streamEnd))
			{
				using (SerializerState serializerState = new SerializerState())
				{
					result = this.valueDeserializer.DeserializeValue(parser, type, serializerState, this.valueDeserializer);
					serializerState.OnDeserialization();
				}
			}
			if (flag2)
			{
				parser.Consume<DocumentEnd>();
			}
			if (flag)
			{
				parser.Consume<StreamEnd>();
			}
			return result;
		}

		// Token: 0x040001A5 RID: 421
		private readonly IValueDeserializer valueDeserializer;
	}
}
