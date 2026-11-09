using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001F8 RID: 504
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StreamStart : ParsingEvent
	{
		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000B1A RID: 2842 RVA: 0x0000206C File Offset: 0x0000026C
		public override int NestingIncrease
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000B1B RID: 2843 RVA: 0x0000206C File Offset: 0x0000026C
		internal override EventType Type
		{
			get
			{
				return EventType.StreamStart;
			}
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x000258B5 File Offset: 0x00023AB5
		public StreamStart() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x00025400 File Offset: 0x00023600
		public StreamStart(in Mark start, in Mark end) : base(start, end)
		{
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x000258C7 File Offset: 0x00023AC7
		public override string ToString()
		{
			return "Stream start";
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x000258CE File Offset: 0x00023ACE
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
