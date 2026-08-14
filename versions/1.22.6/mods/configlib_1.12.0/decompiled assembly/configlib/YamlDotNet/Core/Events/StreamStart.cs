using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001FC RID: 508
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StreamStart : ParsingEvent
	{
		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000B56 RID: 2902 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
		public override int NestingIncrease
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000B57 RID: 2903 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
		internal override EventType Type
		{
			get
			{
				return EventType.StreamStart;
			}
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x000261FD File Offset: 0x000243FD
		public StreamStart() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00025D48 File Offset: 0x00023F48
		public StreamStart(in Mark start, in Mark end) : base(start, end)
		{
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x0002620F File Offset: 0x0002440F
		public override string ToString()
		{
			return "Stream start";
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00026216 File Offset: 0x00024416
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
