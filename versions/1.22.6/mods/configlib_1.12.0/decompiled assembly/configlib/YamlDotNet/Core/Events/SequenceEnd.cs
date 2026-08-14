using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001F8 RID: 504
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SequenceEnd : ParsingEvent
	{
		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000B41 RID: 2881 RVA: 0x00025BFA File Offset: 0x00023DFA
		public override int NestingIncrease
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000B42 RID: 2882 RVA: 0x000260CD File Offset: 0x000242CD
		internal override EventType Type
		{
			get
			{
				return EventType.SequenceEnd;
			}
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x00025D48 File Offset: 0x00023F48
		public SequenceEnd(in Mark start, in Mark end) : base(start, end)
		{
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x000260D0 File Offset: 0x000242D0
		public SequenceEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x000260E2 File Offset: 0x000242E2
		public override string ToString()
		{
			return "Sequence end";
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x000260E9 File Offset: 0x000242E9
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
