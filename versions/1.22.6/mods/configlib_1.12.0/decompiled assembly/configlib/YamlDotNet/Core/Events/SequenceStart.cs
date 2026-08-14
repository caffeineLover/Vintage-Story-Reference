using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001F9 RID: 505
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SequenceStart : NodeEvent
	{
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000B47 RID: 2887 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
		public override int NestingIncrease
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000B48 RID: 2888 RVA: 0x000260F2 File Offset: 0x000242F2
		internal override EventType Type
		{
			get
			{
				return EventType.SequenceStart;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000B49 RID: 2889 RVA: 0x000260F5 File Offset: 0x000242F5
		public bool IsImplicit { get; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000B4A RID: 2890 RVA: 0x000260FD File Offset: 0x000242FD
		public override bool IsCanonical
		{
			get
			{
				return !this.IsImplicit;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000B4B RID: 2891 RVA: 0x00026108 File Offset: 0x00024308
		public SequenceStyle Style { get; }

		// Token: 0x06000B4C RID: 2892 RVA: 0x00026110 File Offset: 0x00024310
		public SequenceStart(AnchorName anchor, TagName tag, bool isImplicit, SequenceStyle style, Mark start, Mark end) : base(anchor, tag, start, end)
		{
			this.IsImplicit = isImplicit;
			this.Style = style;
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x0002612D File Offset: 0x0002432D
		public SequenceStart(AnchorName anchor, TagName tag, bool isImplicit, SequenceStyle style) : this(anchor, tag, isImplicit, style, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x00026144 File Offset: 0x00024344
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(59, 4);
			defaultInterpolatedStringHandler.AppendLiteral("Sequence start [anchor = ");
			defaultInterpolatedStringHandler.AppendFormatted<AnchorName>(base.Anchor);
			defaultInterpolatedStringHandler.AppendLiteral(", tag = ");
			defaultInterpolatedStringHandler.AppendFormatted<TagName>(base.Tag);
			defaultInterpolatedStringHandler.AppendLiteral(", isImplicit = ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.IsImplicit);
			defaultInterpolatedStringHandler.AppendLiteral(", style = ");
			defaultInterpolatedStringHandler.AppendFormatted<SequenceStyle>(this.Style);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x000261D2 File Offset: 0x000243D2
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
