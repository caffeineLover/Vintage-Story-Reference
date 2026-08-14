using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001F7 RID: 503
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Scalar : NodeEvent
	{
		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000B33 RID: 2867 RVA: 0x00025EED File Offset: 0x000240ED
		internal override EventType Type
		{
			get
			{
				return EventType.Scalar;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000B34 RID: 2868 RVA: 0x00025EF0 File Offset: 0x000240F0
		public string Value { get; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000B35 RID: 2869 RVA: 0x00025EF8 File Offset: 0x000240F8
		public ScalarStyle Style { get; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000B36 RID: 2870 RVA: 0x00025F00 File Offset: 0x00024100
		public bool IsPlainImplicit { get; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000B37 RID: 2871 RVA: 0x00025F08 File Offset: 0x00024108
		public bool IsQuotedImplicit { get; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000B38 RID: 2872 RVA: 0x00025F10 File Offset: 0x00024110
		public override bool IsCanonical
		{
			get
			{
				return !this.IsPlainImplicit && !this.IsQuotedImplicit;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000B39 RID: 2873 RVA: 0x00025F25 File Offset: 0x00024125
		public bool IsKey { get; }

		// Token: 0x06000B3A RID: 2874 RVA: 0x00025F2D File Offset: 0x0002412D
		public Scalar(AnchorName anchor, TagName tag, string value, ScalarStyle style, bool isPlainImplicit, bool isQuotedImplicit, Mark start, Mark end, bool isKey = false) : base(anchor, tag, start, end)
		{
			this.Value = value;
			this.Style = style;
			this.IsPlainImplicit = isPlainImplicit;
			this.IsQuotedImplicit = isQuotedImplicit;
			this.IsKey = isKey;
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x00025F64 File Offset: 0x00024164
		public Scalar(AnchorName anchor, TagName tag, string value, ScalarStyle style, bool isPlainImplicit, bool isQuotedImplicit) : this(anchor, tag, value, style, isPlainImplicit, isQuotedImplicit, Mark.Empty, Mark.Empty, false)
		{
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x00025F8C File Offset: 0x0002418C
		public Scalar(string value) : this(AnchorName.Empty, TagName.Empty, value, ScalarStyle.Any, true, true, Mark.Empty, Mark.Empty, false)
		{
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x00025FB8 File Offset: 0x000241B8
		public Scalar(TagName tag, string value) : this(AnchorName.Empty, tag, value, ScalarStyle.Any, true, true, Mark.Empty, Mark.Empty, false)
		{
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x00025FE0 File Offset: 0x000241E0
		public Scalar(AnchorName anchor, TagName tag, string value) : this(anchor, tag, value, ScalarStyle.Any, true, true, Mark.Empty, Mark.Empty, false)
		{
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00026004 File Offset: 0x00024204
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

		// Token: 0x06000B40 RID: 2880 RVA: 0x000260C4 File Offset: 0x000242C4
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
