using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001F7 RID: 503
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StreamEnd : ParsingEvent
	{
		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000B14 RID: 2836 RVA: 0x000252B2 File Offset: 0x000234B2
		public override int NestingIncrease
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x0001B36D File Offset: 0x0001956D
		internal override EventType Type
		{
			get
			{
				return EventType.StreamEnd;
			}
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x00025400 File Offset: 0x00023600
		public StreamEnd(in Mark start, in Mark end) : base(start, end)
		{
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x00025893 File Offset: 0x00023A93
		public StreamEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x000258A5 File Offset: 0x00023AA5
		public override string ToString()
		{
			return "Stream end";
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x000258AC File Offset: 0x00023AAC
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
