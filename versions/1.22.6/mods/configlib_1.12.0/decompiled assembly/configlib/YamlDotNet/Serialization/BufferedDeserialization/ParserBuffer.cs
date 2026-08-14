using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.BufferedDeserialization
{
	// Token: 0x02000177 RID: 375
	public class ParserBuffer : IParser
	{
		// Token: 0x060007A3 RID: 1955 RVA: 0x00019EA8 File Offset: 0x000180A8
		[NullableContext(1)]
		public ParserBuffer(IParser parserToBuffer, int maxDepth, int maxLength)
		{
			this.buffer = new LinkedList<ParsingEvent>();
			this.buffer.AddLast(parserToBuffer.Consume<MappingStart>());
			int num = 0;
			for (;;)
			{
				ParsingEvent parsingEvent = parserToBuffer.Consume<ParsingEvent>();
				num += parsingEvent.NestingIncrease;
				this.buffer.AddLast(parsingEvent);
				if (maxDepth > -1 && num > maxDepth)
				{
					break;
				}
				if (maxLength > -1 && this.buffer.Count > maxLength)
				{
					goto Block_4;
				}
				if (num < 0)
				{
					goto Block_5;
				}
			}
			throw new ArgumentOutOfRangeException("parserToBuffer", "Parser buffer exceeded max depth");
			Block_4:
			throw new ArgumentOutOfRangeException("parserToBuffer", "Parser buffer exceeded max length");
			Block_5:
			this.current = this.buffer.First;
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x00019F46 File Offset: 0x00018146
		[Nullable(2)]
		public ParsingEvent Current
		{
			[NullableContext(2)]
			get
			{
				LinkedListNode<ParsingEvent> linkedListNode = this.current;
				if (linkedListNode == null)
				{
					return null;
				}
				return linkedListNode.Value;
			}
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x00019F59 File Offset: 0x00018159
		public bool MoveNext()
		{
			LinkedListNode<ParsingEvent> linkedListNode = this.current;
			this.current = ((linkedListNode != null) ? linkedListNode.Next : null);
			return this.current != null;
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x00019F7C File Offset: 0x0001817C
		public void Reset()
		{
			this.current = this.buffer.First;
		}

		// Token: 0x04000358 RID: 856
		[Nullable(1)]
		private readonly LinkedList<ParsingEvent> buffer;

		// Token: 0x04000359 RID: 857
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private LinkedListNode<ParsingEvent> current;
	}
}
