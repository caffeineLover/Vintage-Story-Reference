using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001FB RID: 507
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StreamEnd : ParsingEvent
	{
		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000B50 RID: 2896 RVA: 0x00025BFA File Offset: 0x00023DFA
		public override int NestingIncrease
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000B51 RID: 2897 RVA: 0x0001BCB5 File Offset: 0x00019EB5
		internal override EventType Type
		{
			get
			{
				return EventType.StreamEnd;
			}
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x00025D48 File Offset: 0x00023F48
		public StreamEnd(in Mark start, in Mark end) : base(start, end)
		{
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x000261DB File Offset: 0x000243DB
		public StreamEnd() : this(Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x000261ED File Offset: 0x000243ED
		public override string ToString()
		{
			return "Stream end";
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x000261F4 File Offset: 0x000243F4
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
