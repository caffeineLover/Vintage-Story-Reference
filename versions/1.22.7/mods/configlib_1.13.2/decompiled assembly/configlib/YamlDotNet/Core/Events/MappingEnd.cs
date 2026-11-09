using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001EE RID: 494
	[NullableContext(1)]
	[Nullable(0)]
	public class MappingEnd : ParsingEvent
	{
		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x000252B2 File Offset: 0x000234B2
		public override int NestingIncrease
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000ADD RID: 2781 RVA: 0x000253FC File Offset: 0x000235FC
		internal override EventType Type
		{
			get
			{
				return EventType.MappingEnd;
			}
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x00025400 File Offset: 0x00023600
		public MappingEnd(in Mark start, in Mark end) : base(start, end)
		{
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0002540A File Offset: 0x0002360A
		public MappingEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x0002541C File Offset: 0x0002361C
		public override string ToString()
		{
			return "Mapping end";
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x00025423 File Offset: 0x00023623
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
