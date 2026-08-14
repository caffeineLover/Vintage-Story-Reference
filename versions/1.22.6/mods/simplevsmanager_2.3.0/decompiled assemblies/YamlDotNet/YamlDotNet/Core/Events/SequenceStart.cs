using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x0200010F RID: 271
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SequenceStart : NodeEvent
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x00017B13 File Offset: 0x00015D13
		public override int NestingIncrease
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060006A2 RID: 1698 RVA: 0x00017B16 File Offset: 0x00015D16
		internal override EventType Type
		{
			get
			{
				return EventType.SequenceStart;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x00017B19 File Offset: 0x00015D19
		public bool IsImplicit { get; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060006A4 RID: 1700 RVA: 0x00017B21 File Offset: 0x00015D21
		public override bool IsCanonical
		{
			get
			{
				return !this.IsImplicit;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x00017B2C File Offset: 0x00015D2C
		public SequenceStyle Style { get; }

		// Token: 0x060006A6 RID: 1702 RVA: 0x00017B34 File Offset: 0x00015D34
		public SequenceStart(AnchorName anchor, TagName tag, bool isImplicit, SequenceStyle style, Mark start, Mark end) : base(anchor, tag, start, end)
		{
			this.IsImplicit = isImplicit;
			this.Style = style;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00017B51 File Offset: 0x00015D51
		public SequenceStart(AnchorName anchor, TagName tag, bool isImplicit, SequenceStyle style) : this(anchor, tag, isImplicit, style, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00017B68 File Offset: 0x00015D68
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

		// Token: 0x060006A9 RID: 1705 RVA: 0x00017BF6 File Offset: 0x00015DF6
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
