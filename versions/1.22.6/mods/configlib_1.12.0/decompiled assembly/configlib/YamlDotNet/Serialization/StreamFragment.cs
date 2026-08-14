using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000E9 RID: 233
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StreamFragment : IYamlConvertible
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x00013D67 File Offset: 0x00011F67
		public IList<ParsingEvent> Events
		{
			get
			{
				return this.events;
			}
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00013D70 File Offset: 0x00011F70
		void IYamlConvertible.Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
		{
			this.events.Clear();
			int num = 0;
			while (parser.MoveNext())
			{
				ParsingEvent parsingEvent = parser.Current;
				this.events.Add(parsingEvent);
				num += parsingEvent.NestingIncrease;
				if (num <= 0)
				{
					return;
				}
			}
			throw new InvalidOperationException("The parser has reached the end before deserialization completed.");
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00013DC0 File Offset: 0x00011FC0
		void IYamlConvertible.Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
		{
			foreach (ParsingEvent @event in this.events)
			{
				emitter.Emit(@event);
			}
		}

		// Token: 0x04000278 RID: 632
		private readonly List<ParsingEvent> events = new List<ParsingEvent>();
	}
}
