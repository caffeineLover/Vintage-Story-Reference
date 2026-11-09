using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001F4 RID: 500
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SequenceEnd : ParsingEvent
	{
		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000B05 RID: 2821 RVA: 0x000252B2 File Offset: 0x000234B2
		public override int NestingIncrease
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000B06 RID: 2822 RVA: 0x00025785 File Offset: 0x00023985
		internal override EventType Type
		{
			get
			{
				return EventType.SequenceEnd;
			}
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x00025400 File Offset: 0x00023600
		public SequenceEnd(in Mark start, in Mark end) : base(start, end)
		{
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x00025788 File Offset: 0x00023988
		public SequenceEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x0002579A File Offset: 0x0002399A
		public override string ToString()
		{
			return "Sequence end";
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x000257A1 File Offset: 0x000239A1
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
