using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x0200010E RID: 270
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SequenceEnd : ParsingEvent
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x00017AE1 File Offset: 0x00015CE1
		public override int NestingIncrease
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x00017AE4 File Offset: 0x00015CE4
		internal override EventType Type
		{
			get
			{
				return EventType.SequenceEnd;
			}
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00017AE7 File Offset: 0x00015CE7
		public SequenceEnd(in Mark start, in Mark end) : base(start, end)
		{
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00017AF1 File Offset: 0x00015CF1
		public SequenceEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00017B03 File Offset: 0x00015D03
		public override string ToString()
		{
			return "Sequence end";
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00017B0A File Offset: 0x00015D0A
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
