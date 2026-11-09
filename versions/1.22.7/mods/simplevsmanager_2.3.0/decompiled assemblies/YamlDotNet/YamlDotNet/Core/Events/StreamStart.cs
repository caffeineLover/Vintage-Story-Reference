using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x02000112 RID: 274
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StreamStart : ParsingEvent
	{
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x00017C31 File Offset: 0x00015E31
		public override int NestingIncrease
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x00017C34 File Offset: 0x00015E34
		internal override EventType Type
		{
			get
			{
				return EventType.StreamStart;
			}
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00017C37 File Offset: 0x00015E37
		public StreamStart() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00017C49 File Offset: 0x00015E49
		public StreamStart(in Mark start, in Mark end) : base(start, end)
		{
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00017C53 File Offset: 0x00015E53
		public override string ToString()
		{
			return "Stream start";
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x00017C5A File Offset: 0x00015E5A
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
