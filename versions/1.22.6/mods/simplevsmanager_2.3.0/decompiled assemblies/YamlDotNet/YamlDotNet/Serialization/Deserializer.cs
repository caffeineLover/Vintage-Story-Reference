using System;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200000B RID: 11
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Deserializer : IDeserializer
	{
		// Token: 0x06000042 RID: 66 RVA: 0x00002AB1 File Offset: 0x00000CB1
		public Deserializer() : this(new DeserializerBuilder().BuildValueDeserializer())
		{
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002AC3 File Offset: 0x00000CC3
		private Deserializer(IValueDeserializer valueDeserializer)
		{
			if (valueDeserializer == null)
			{
				throw new ArgumentNullException("valueDeserializer");
			}
			this.valueDeserializer = valueDeserializer;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002AE1 File Offset: 0x00000CE1
		public static Deserializer FromValueDeserializer(IValueDeserializer valueDeserializer)
		{
			return new Deserializer(valueDeserializer);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002AEC File Offset: 0x00000CEC
		public T Deserialize<[Nullable(2)] T>(string input)
		{
			T result;
			using (StringReader stringReader = new StringReader(input))
			{
				result = this.Deserialize<T>(stringReader);
			}
			return result;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002B28 File Offset: 0x00000D28
		public T Deserialize<[Nullable(2)] T>(TextReader input)
		{
			return this.Deserialize<T>(new Parser(input));
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002B36 File Offset: 0x00000D36
		public T Deserialize<[Nullable(2)] T>(IParser parser)
		{
			return (T)((object)this.Deserialize(parser, typeof(T)));
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002B4E File Offset: 0x00000D4E
		[return: Nullable(2)]
		public object Deserialize(string input)
		{
			return this.Deserialize<object>(input);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002B57 File Offset: 0x00000D57
		[return: Nullable(2)]
		public object Deserialize(TextReader input)
		{
			return this.Deserialize<object>(input);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002B60 File Offset: 0x00000D60
		[return: Nullable(2)]
		public object Deserialize(IParser parser)
		{
			return this.Deserialize<object>(parser);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002B6C File Offset: 0x00000D6C
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

		// Token: 0x0600004C RID: 76 RVA: 0x00002BA8 File Offset: 0x00000DA8
		[return: Nullable(2)]
		public object Deserialize(TextReader input, Type type)
		{
			return this.Deserialize(new Parser(input), type);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002BB8 File Offset: 0x00000DB8
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

		// Token: 0x04000014 RID: 20
		private readonly IValueDeserializer valueDeserializer;
	}
}
