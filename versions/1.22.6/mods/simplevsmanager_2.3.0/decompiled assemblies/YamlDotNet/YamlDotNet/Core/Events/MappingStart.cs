using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x02000109 RID: 265
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MappingStart : NodeEvent
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000678 RID: 1656 RVA: 0x00017783 File Offset: 0x00015983
		public override int NestingIncrease
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x00017786 File Offset: 0x00015986
		internal override EventType Type
		{
			get
			{
				return EventType.MappingStart;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600067A RID: 1658 RVA: 0x0001778A File Offset: 0x0001598A
		public bool IsImplicit { get; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x00017792 File Offset: 0x00015992
		public override bool IsCanonical
		{
			get
			{
				return !this.IsImplicit;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600067C RID: 1660 RVA: 0x0001779D File Offset: 0x0001599D
		public MappingStyle Style { get; }

		// Token: 0x0600067D RID: 1661 RVA: 0x000177A5 File Offset: 0x000159A5
		public MappingStart(AnchorName anchor, TagName tag, bool isImplicit, MappingStyle style, Mark start, Mark end) : base(anchor, tag, start, end)
		{
			this.IsImplicit = isImplicit;
			this.Style = style;
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x000177C2 File Offset: 0x000159C2
		public MappingStart(AnchorName anchor, TagName tag, bool isImplicit, MappingStyle style) : this(anchor, tag, isImplicit, style, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x000177D9 File Offset: 0x000159D9
		public MappingStart() : this(AnchorName.Empty, TagName.Empty, true, MappingStyle.Any, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x000177F8 File Offset: 0x000159F8
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(58, 4);
			defaultInterpolatedStringHandler.AppendLiteral("Mapping start [anchor = ");
			defaultInterpolatedStringHandler.AppendFormatted<AnchorName>(base.Anchor);
			defaultInterpolatedStringHandler.AppendLiteral(", tag = ");
			defaultInterpolatedStringHandler.AppendFormatted<TagName>(base.Tag);
			defaultInterpolatedStringHandler.AppendLiteral(", isImplicit = ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.IsImplicit);
			defaultInterpolatedStringHandler.AppendLiteral(", style = ");
			defaultInterpolatedStringHandler.AppendFormatted<MappingStyle>(this.Style);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00017886 File Offset: 0x00015A86
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
