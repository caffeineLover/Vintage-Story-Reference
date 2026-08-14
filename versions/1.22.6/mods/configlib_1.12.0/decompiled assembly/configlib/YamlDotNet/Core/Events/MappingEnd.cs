using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001F2 RID: 498
	[NullableContext(1)]
	[Nullable(0)]
	public class MappingEnd : ParsingEvent
	{
		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000B18 RID: 2840 RVA: 0x00025BFA File Offset: 0x00023DFA
		public override int NestingIncrease
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x00025D44 File Offset: 0x00023F44
		internal override EventType Type
		{
			get
			{
				return EventType.MappingEnd;
			}
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x00025D48 File Offset: 0x00023F48
		public MappingEnd(in Mark start, in Mark end) : base(start, end)
		{
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x00025D52 File Offset: 0x00023F52
		public MappingEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x00025D64 File Offset: 0x00023F64
		public override string ToString()
		{
			return "Mapping end";
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x00025D6B File Offset: 0x00023F6B
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
