using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001F3 RID: 499
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Scalar : NodeEvent
	{
		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x000255A5 File Offset: 0x000237A5
		internal override EventType Type
		{
			get
			{
				return EventType.Scalar;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x000255A8 File Offset: 0x000237A8
		public string Value { get; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000AF9 RID: 2809 RVA: 0x000255B0 File Offset: 0x000237B0
		public ScalarStyle Style { get; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000AFA RID: 2810 RVA: 0x000255B8 File Offset: 0x000237B8
		public bool IsPlainImplicit { get; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000AFB RID: 2811 RVA: 0x000255C0 File Offset: 0x000237C0
		public bool IsQuotedImplicit { get; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000AFC RID: 2812 RVA: 0x000255C8 File Offset: 0x000237C8
		public override bool IsCanonical
		{
			get
			{
				return !this.IsPlainImplicit && !this.IsQuotedImplicit;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000AFD RID: 2813 RVA: 0x000255DD File Offset: 0x000237DD
		public bool IsKey { get; }

		// Token: 0x06000AFE RID: 2814 RVA: 0x000255E5 File Offset: 0x000237E5
		public Scalar(AnchorName anchor, TagName tag, string value, ScalarStyle style, bool isPlainImplicit, bool isQuotedImplicit, Mark start, Mark end, bool isKey = false) : base(anchor, tag, start, end)
		{
			this.Value = value;
			this.Style = style;
			this.IsPlainImplicit = isPlainImplicit;
			this.IsQuotedImplicit = isQuotedImplicit;
			this.IsKey = isKey;
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x0002561C File Offset: 0x0002381C
		public Scalar(AnchorName anchor, TagName tag, string value, ScalarStyle style, bool isPlainImplicit, bool isQuotedImplicit) : this(anchor, tag, value, style, isPlainImplicit, isQuotedImplicit, Mark.Empty, Mark.Empty, false)
		{
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x00025644 File Offset: 0x00023844
		public Scalar(string value) : this(AnchorName.Empty, TagName.Empty, value, ScalarStyle.Any, true, true, Mark.Empty, Mark.Empty, false)
		{
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x00025670 File Offset: 0x00023870
		public Scalar(TagName tag, string value) : this(AnchorName.Empty, tag, value, ScalarStyle.Any, true, true, Mark.Empty, Mark.Empty, false)
		{
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x00025698 File Offset: 0x00023898
		public Scalar(AnchorName anchor, TagName tag, string value) : this(anchor, tag, value, ScalarStyle.Any, true, true, Mark.Empty, Mark.Empty, false)
		{
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x000256BC File Offset: 0x000238BC
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(87, 6);
			defaultInterpolatedStringHandler.AppendLiteral("Scalar [anchor = ");
			defaultInterpolatedStringHandler.AppendFormatted<AnchorName>(base.Anchor);
			defaultInterpolatedStringHandler.AppendLiteral(", tag = ");
			defaultInterpolatedStringHandler.AppendFormatted<TagName>(base.Tag);
			defaultInterpolatedStringHandler.AppendLiteral(", value = ");
			defaultInterpolatedStringHandler.AppendFormatted(this.Value);
			defaultInterpolatedStringHandler.AppendLiteral(", style = ");
			defaultInterpolatedStringHandler.AppendFormatted<ScalarStyle>(this.Style);
			defaultInterpolatedStringHandler.AppendLiteral(", isPlainImplicit = ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.IsPlainImplicit);
			defaultInterpolatedStringHandler.AppendLiteral(", isQuotedImplicit = ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.IsQuotedImplicit);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x0002577C File Offset: 0x0002397C
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
