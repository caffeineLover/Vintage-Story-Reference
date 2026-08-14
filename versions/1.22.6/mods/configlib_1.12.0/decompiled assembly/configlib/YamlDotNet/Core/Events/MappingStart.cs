using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001F3 RID: 499
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MappingStart : NodeEvent
	{
		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000B1E RID: 2846 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
		public override int NestingIncrease
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000B1F RID: 2847 RVA: 0x00025D74 File Offset: 0x00023F74
		internal override EventType Type
		{
			get
			{
				return EventType.MappingStart;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000B20 RID: 2848 RVA: 0x00025D78 File Offset: 0x00023F78
		public bool IsImplicit { get; }

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000B21 RID: 2849 RVA: 0x00025D80 File Offset: 0x00023F80
		public override bool IsCanonical
		{
			get
			{
				return !this.IsImplicit;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000B22 RID: 2850 RVA: 0x00025D8B File Offset: 0x00023F8B
		public MappingStyle Style { get; }

		// Token: 0x06000B23 RID: 2851 RVA: 0x00025D93 File Offset: 0x00023F93
		public MappingStart(AnchorName anchor, TagName tag, bool isImplicit, MappingStyle style, Mark start, Mark end) : base(anchor, tag, start, end)
		{
			this.IsImplicit = isImplicit;
			this.Style = style;
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x00025DB0 File Offset: 0x00023FB0
		public MappingStart(AnchorName anchor, TagName tag, bool isImplicit, MappingStyle style) : this(anchor, tag, isImplicit, style, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00025DC7 File Offset: 0x00023FC7
		public MappingStart() : this(AnchorName.Empty, TagName.Empty, true, MappingStyle.Any, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x00025DE8 File Offset: 0x00023FE8
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

		// Token: 0x06000B27 RID: 2855 RVA: 0x00025E76 File Offset: 0x00024076
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
