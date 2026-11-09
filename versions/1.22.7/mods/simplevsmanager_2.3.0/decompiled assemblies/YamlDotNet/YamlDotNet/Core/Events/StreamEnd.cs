using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x02000111 RID: 273
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StreamEnd : ParsingEvent
	{
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060006AA RID: 1706 RVA: 0x00017BFF File Offset: 0x00015DFF
		public override int NestingIncrease
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x00017C02 File Offset: 0x00015E02
		internal override EventType Type
		{
			get
			{
				return EventType.StreamEnd;
			}
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00017C05 File Offset: 0x00015E05
		public StreamEnd(in Mark start, in Mark end) : base(start, end)
		{
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x00017C0F File Offset: 0x00015E0F
		public StreamEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00017C21 File Offset: 0x00015E21
		public override string ToString()
		{
			return "Stream end";
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00017C28 File Offset: 0x00015E28
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
