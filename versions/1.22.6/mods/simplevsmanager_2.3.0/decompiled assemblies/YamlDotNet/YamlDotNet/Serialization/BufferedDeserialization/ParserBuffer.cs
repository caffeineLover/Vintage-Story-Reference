using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.BufferedDeserialization
{
	// Token: 0x0200009E RID: 158
	public class ParserBuffer : IParser
	{
		// Token: 0x0600035B RID: 859 RVA: 0x0000C1E8 File Offset: 0x0000A3E8
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

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600035C RID: 860 RVA: 0x0000C286 File Offset: 0x0000A486
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

		// Token: 0x0600035D RID: 861 RVA: 0x0000C299 File Offset: 0x0000A499
		public bool MoveNext()
		{
			LinkedListNode<ParsingEvent> linkedListNode = this.current;
			this.current = ((linkedListNode != null) ? linkedListNode.Next : null);
			return this.current != null;
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000C2BC File Offset: 0x0000A4BC
		public void Reset()
		{
			this.current = this.buffer.First;
		}

		// Token: 0x040000F3 RID: 243
		[Nullable(1)]
		private readonly LinkedList<ParsingEvent> buffer;

		// Token: 0x040000F4 RID: 244
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private LinkedListNode<ParsingEvent> current;
	}
}
