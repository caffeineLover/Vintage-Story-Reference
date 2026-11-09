using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.BufferedDeserialization
{
	// Token: 0x02000173 RID: 371
	public class ParserBuffer : IParser
	{
		// Token: 0x06000767 RID: 1895 RVA: 0x00019560 File Offset: 0x00017760
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
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x000195FE File Offset: 0x000177FE
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

		// Token: 0x06000769 RID: 1897 RVA: 0x00019611 File Offset: 0x00017811
		public bool MoveNext()
		{
			LinkedListNode<ParsingEvent> linkedListNode = this.current;
			this.current = ((linkedListNode != null) ? linkedListNode.Next : null);
			return this.current != null;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00019634 File Offset: 0x00017834
		public void Reset()
		{
			this.current = this.buffer.First;
		}

		// Token: 0x0400032A RID: 810
		[Nullable(1)]
		private readonly LinkedList<ParsingEvent> buffer;

		// Token: 0x0400032B RID: 811
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private LinkedListNode<ParsingEvent> current;
	}
}
