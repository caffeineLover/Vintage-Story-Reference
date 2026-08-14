using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x0200010D RID: 269
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Scalar : NodeEvent
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x00017900 File Offset: 0x00015B00
		internal override EventType Type
		{
			get
			{
				return EventType.Scalar;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600068E RID: 1678 RVA: 0x00017903 File Offset: 0x00015B03
		public string Value { get; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x0001790B File Offset: 0x00015B0B
		public ScalarStyle Style { get; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000690 RID: 1680 RVA: 0x00017913 File Offset: 0x00015B13
		public bool IsPlainImplicit { get; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x0001791B File Offset: 0x00015B1B
		public bool IsQuotedImplicit { get; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000692 RID: 1682 RVA: 0x00017923 File Offset: 0x00015B23
		public override bool IsCanonical
		{
			get
			{
				return !this.IsPlainImplicit && !this.IsQuotedImplicit;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x00017938 File Offset: 0x00015B38
		public bool IsKey { get; }

		// Token: 0x06000694 RID: 1684 RVA: 0x00017940 File Offset: 0x00015B40
		public Scalar(AnchorName anchor, TagName tag, string value, ScalarStyle style, bool isPlainImplicit, bool isQuotedImplicit, Mark start, Mark end, bool isKey = false) : base(anchor, tag, start, end)
		{
			this.Value = value;
			this.Style = style;
			this.IsPlainImplicit = isPlainImplicit;
			this.IsQuotedImplicit = isQuotedImplicit;
			this.IsKey = isKey;
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00017978 File Offset: 0x00015B78
		public Scalar(AnchorName anchor, TagName tag, string value, ScalarStyle style, bool isPlainImplicit, bool isQuotedImplicit) : this(anchor, tag, value, style, isPlainImplicit, isQuotedImplicit, Mark.Empty, Mark.Empty, false)
		{
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x000179A0 File Offset: 0x00015BA0
		public Scalar(string value) : this(AnchorName.Empty, TagName.Empty, value, ScalarStyle.Any, true, true, Mark.Empty, Mark.Empty, false)
		{
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x000179CC File Offset: 0x00015BCC
		public Scalar(TagName tag, string value) : this(AnchorName.Empty, tag, value, ScalarStyle.Any, true, true, Mark.Empty, Mark.Empty, false)
		{
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x000179F4 File Offset: 0x00015BF4
		public Scalar(AnchorName anchor, TagName tag, string value) : this(anchor, tag, value, ScalarStyle.Any, true, true, Mark.Empty, Mark.Empty, false)
		{
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00017A18 File Offset: 0x00015C18
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

		// Token: 0x0600069A RID: 1690 RVA: 0x00017AD8 File Offset: 0x00015CD8
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
