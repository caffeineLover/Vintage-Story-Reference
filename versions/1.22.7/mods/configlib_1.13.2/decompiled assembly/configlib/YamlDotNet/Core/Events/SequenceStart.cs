using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001F5 RID: 501
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SequenceStart : NodeEvent
	{
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000B0B RID: 2827 RVA: 0x0000206C File Offset: 0x0000026C
		public override int NestingIncrease
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000B0C RID: 2828 RVA: 0x000257AA File Offset: 0x000239AA
		internal override EventType Type
		{
			get
			{
				return EventType.SequenceStart;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000B0D RID: 2829 RVA: 0x000257AD File Offset: 0x000239AD
		public bool IsImplicit { get; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000B0E RID: 2830 RVA: 0x000257B5 File Offset: 0x000239B5
		public override bool IsCanonical
		{
			get
			{
				return !this.IsImplicit;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000B0F RID: 2831 RVA: 0x000257C0 File Offset: 0x000239C0
		public SequenceStyle Style { get; }

		// Token: 0x06000B10 RID: 2832 RVA: 0x000257C8 File Offset: 0x000239C8
		public SequenceStart(AnchorName anchor, TagName tag, bool isImplicit, SequenceStyle style, Mark start, Mark end) : base(anchor, tag, start, end)
		{
			this.IsImplicit = isImplicit;
			this.Style = style;
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x000257E5 File Offset: 0x000239E5
		public SequenceStart(AnchorName anchor, TagName tag, bool isImplicit, SequenceStyle style) : this(anchor, tag, isImplicit, style, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x000257FC File Offset: 0x000239FC
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

		// Token: 0x06000B13 RID: 2835 RVA: 0x0002588A File Offset: 0x00023A8A
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
