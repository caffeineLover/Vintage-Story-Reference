using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200003D RID: 61
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StreamFragment : IYamlConvertible
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x000064BF File Offset: 0x000046BF
		public IList<ParsingEvent> Events
		{
			get
			{
				return this.events;
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000064C8 File Offset: 0x000046C8
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

		// Token: 0x060001D6 RID: 470 RVA: 0x00006518 File Offset: 0x00004718
		void IYamlConvertible.Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
		{
			foreach (ParsingEvent @event in this.events)
			{
				emitter.Emit(@event);
			}
		}

		// Token: 0x0400006B RID: 107
		private readonly List<ParsingEvent> events = new List<ParsingEvent>();
	}
}
