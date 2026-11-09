using System;
using System.IO;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200007F RID: 127
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Deserializer : IDeserializer
	{
		// Token: 0x06000306 RID: 774 RVA: 0x0000F4F9 File Offset: 0x0000D6F9
		public Deserializer() : this(new DeserializerBuilder().BuildValueDeserializer())
		{
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000F50B File Offset: 0x0000D70B
		private Deserializer(IValueDeserializer valueDeserializer)
		{
			if (valueDeserializer == null)
			{
				throw new ArgumentNullException("valueDeserializer");
			}
			this.valueDeserializer = valueDeserializer;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000F529 File Offset: 0x0000D729
		public static Deserializer FromValueDeserializer(IValueDeserializer valueDeserializer)
		{
			return new Deserializer(valueDeserializer);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000F534 File Offset: 0x0000D734
		public T Deserialize<[Nullable(2)] T>(string input)
		{
			T result;
			using (StringReader stringReader = new StringReader(input))
			{
				result = this.Deserialize<T>(stringReader);
			}
			return result;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000F570 File Offset: 0x0000D770
		public T Deserialize<[Nullable(2)] T>(TextReader input)
		{
			return this.Deserialize<T>(new Parser(input));
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000F57E File Offset: 0x0000D77E
		public T Deserialize<[Nullable(2)] T>(IParser parser)
		{
			return (T)((object)this.Deserialize(parser, typeof(T)));
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000F596 File Offset: 0x0000D796
		[return: Nullable(2)]
		public object Deserialize(string input)
		{
			return this.Deserialize(input, typeof(object));
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000F5A9 File Offset: 0x0000D7A9
		[return: Nullable(2)]
		public object Deserialize(TextReader input)
		{
			return this.Deserialize(input, typeof(object));
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000F5BC File Offset: 0x0000D7BC
		[return: Nullable(2)]
		public object Deserialize(IParser parser)
		{
			return this.Deserialize(parser, typeof(object));
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000F5D0 File Offset: 0x0000D7D0
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

		// Token: 0x06000310 RID: 784 RVA: 0x0000F60C File Offset: 0x0000D80C
		[return: Nullable(2)]
		public object Deserialize(TextReader input, Type type)
		{
			return this.Deserialize(new Parser(input), type);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000F61C File Offset: 0x0000D81C
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

		// Token: 0x04000177 RID: 375
		private readonly IValueDeserializer valueDeserializer;
	}
}
