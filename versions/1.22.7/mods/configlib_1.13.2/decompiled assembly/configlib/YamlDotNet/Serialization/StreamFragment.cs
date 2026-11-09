using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000E5 RID: 229
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StreamFragment : IYamlConvertible
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x0001341F File Offset: 0x0001161F
		public IList<ParsingEvent> Events
		{
			get
			{
				return this.events;
			}
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00013428 File Offset: 0x00011628
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

		// Token: 0x0600052A RID: 1322 RVA: 0x00013478 File Offset: 0x00011678
		void IYamlConvertible.Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
		{
			foreach (ParsingEvent @event in this.events)
			{
				emitter.Emit(@event);
			}
		}

		// Token: 0x0400024A RID: 586
		private readonly List<ParsingEvent> events = new List<ParsingEvent>();
	}
}
