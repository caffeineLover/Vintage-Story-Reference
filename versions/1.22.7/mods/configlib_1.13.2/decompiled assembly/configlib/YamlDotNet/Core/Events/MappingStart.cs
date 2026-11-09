using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001EF RID: 495
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MappingStart : NodeEvent
	{
		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000AE2 RID: 2786 RVA: 0x0000206C File Offset: 0x0000026C
		public override int NestingIncrease
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x0002542C File Offset: 0x0002362C
		internal override EventType Type
		{
			get
			{
				return EventType.MappingStart;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x00025430 File Offset: 0x00023630
		public bool IsImplicit { get; }

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x00025438 File Offset: 0x00023638
		public override bool IsCanonical
		{
			get
			{
				return !this.IsImplicit;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x00025443 File Offset: 0x00023643
		public MappingStyle Style { get; }

		// Token: 0x06000AE7 RID: 2791 RVA: 0x0002544B File Offset: 0x0002364B
		public MappingStart(AnchorName anchor, TagName tag, bool isImplicit, MappingStyle style, Mark start, Mark end) : base(anchor, tag, start, end)
		{
			this.IsImplicit = isImplicit;
			this.Style = style;
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x00025468 File Offset: 0x00023668
		public MappingStart(AnchorName anchor, TagName tag, bool isImplicit, MappingStyle style) : this(anchor, tag, isImplicit, style, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0002547F File Offset: 0x0002367F
		public MappingStart() : this(AnchorName.Empty, TagName.Empty, true, MappingStyle.Any, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x000254A0 File Offset: 0x000236A0
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

		// Token: 0x06000AEB RID: 2795 RVA: 0x0002552E File Offset: 0x0002372E
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
