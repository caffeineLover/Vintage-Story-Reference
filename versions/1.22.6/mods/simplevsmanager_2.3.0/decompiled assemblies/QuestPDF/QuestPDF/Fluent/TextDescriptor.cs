using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Elements.Text;
using QuestPDF.Elements.Text.Items;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x020000AE RID: 174
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TextDescriptor
	{
		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x0000C79F File Offset: 0x0000A99F
		internal TextBlock TextBlock { get; } = new TextBlock();

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x0000C7A7 File Offset: 0x0000A9A7
		// (set) Token: 0x060004DF RID: 1247 RVA: 0x0000C7AF File Offset: 0x0000A9AF
		[Nullable(2)]
		private TextStyle DefaultStyle { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x060004E0 RID: 1248 RVA: 0x0000C7B8 File Offset: 0x0000A9B8
		internal TextDescriptor()
		{
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0000C7CB File Offset: 0x0000A9CB
		public void DefaultTextStyle(TextStyle style)
		{
			this.DefaultStyle = style;
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0000C7D4 File Offset: 0x0000A9D4
		public void DefaultTextStyle(Func<TextStyle, TextStyle> style)
		{
			this.DefaultStyle = style.Invoke(TextStyle.Default);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0000C7E7 File Offset: 0x0000A9E7
		public void AlignLeft()
		{
			this.TextBlock.Alignment = new TextHorizontalAlignment?(TextHorizontalAlignment.Left);
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0000C7FA File Offset: 0x0000A9FA
		public void AlignCenter()
		{
			this.TextBlock.Alignment = new TextHorizontalAlignment?(TextHorizontalAlignment.Center);
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0000C80D File Offset: 0x0000AA0D
		public void AlignRight()
		{
			this.TextBlock.Alignment = new TextHorizontalAlignment?(TextHorizontalAlignment.Right);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0000C820 File Offset: 0x0000AA20
		public void Justify()
		{
			this.TextBlock.Alignment = new TextHorizontalAlignment?(TextHorizontalAlignment.Justify);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0000C833 File Offset: 0x0000AA33
		public void AlignStart()
		{
			this.TextBlock.Alignment = new TextHorizontalAlignment?(TextHorizontalAlignment.Start);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0000C846 File Offset: 0x0000AA46
		public void AlignEnd()
		{
			this.TextBlock.Alignment = new TextHorizontalAlignment?(TextHorizontalAlignment.End);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0000C859 File Offset: 0x0000AA59
		public void ClampLines(int maxLines, string ellipsis = "…")
		{
			this.TextBlock.LineClamp = new int?(maxLines);
			this.TextBlock.LineClampEllipsis = ellipsis;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0000C878 File Offset: 0x0000AA78
		public void ParagraphSpacing(float value, Unit unit = Unit.Point)
		{
			if (value < 0f)
			{
				throw new ArgumentException("Paragraph spacing must be greater or equal to zero", "value");
			}
			this.TextBlock.ParagraphSpacing = value.ToPoints(unit);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0000C8A4 File Offset: 0x0000AAA4
		public void ParagraphFirstLineIndentation(float value, Unit unit = Unit.Point)
		{
			if (value < 0f)
			{
				throw new ArgumentException("Paragraph indentation must be greater or equal to zero", "value");
			}
			this.TextBlock.ParagraphFirstLineIndentation = value.ToPoints(unit);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0000C8D0 File Offset: 0x0000AAD0
		[Obsolete("This element has been renamed since version 2022.3. Please use the overload that returns a TextSpanDescriptor object which allows to specify text style.")]
		[ExcludeFromCodeCoverage]
		public void Span([Nullable(2)] string text, TextStyle style)
		{
			this.Span(text).Style(style);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0000C8E0 File Offset: 0x0000AAE0
		public TextSpanDescriptor Span([Nullable(2)] string text)
		{
			if (text == null)
			{
				return new TextSpanDescriptor(new TextBlockSpan());
			}
			TextBlockSpan textBlockSpan = new TextBlockSpan
			{
				Text = text
			};
			this.TextBlock.Items.Add(textBlockSpan);
			return new TextSpanDescriptor(textBlockSpan);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0000C91F File Offset: 0x0000AB1F
		public TextSpanDescriptor Line([Nullable(2)] string text)
		{
			if (text == null)
			{
				text = string.Empty;
			}
			return this.Span(text + "\n");
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0000C93C File Offset: 0x0000AB3C
		public TextSpanDescriptor EmptyLine()
		{
			return this.Span("\n");
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0000C94C File Offset: 0x0000AB4C
		private TextPageNumberDescriptor PageNumber(Func<IPageContext, int?> pageNumber)
		{
			TextBlockPageNumber textBlockItem = new TextBlockPageNumber();
			this.TextBlock.Items.Add(textBlockItem);
			return new TextPageNumberDescriptor(textBlockItem, delegate(PageNumberFormatter x)
			{
				textBlockItem.Source = ((IPageContext context) => x(pageNumber.Invoke(context)));
			});
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0000C99E File Offset: 0x0000AB9E
		public TextPageNumberDescriptor CurrentPageNumber()
		{
			return this.PageNumber((IPageContext x) => new int?(x.CurrentPage));
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0000C9C5 File Offset: 0x0000ABC5
		public TextPageNumberDescriptor TotalPages()
		{
			return this.PageNumber((IPageContext x) => new int?(x.DocumentLength));
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0000C9EC File Offset: 0x0000ABEC
		[Obsolete("This element has been renamed since version 2022.3. Please use the BeginPageNumberOfSection method.")]
		[ExcludeFromCodeCoverage]
		public void PageNumberOfLocation(string sectionName, [Nullable(2)] TextStyle style = null)
		{
			this.BeginPageNumberOfSection(sectionName).Style(style);
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0000C9FC File Offset: 0x0000ABFC
		public TextPageNumberDescriptor BeginPageNumberOfSection(string sectionName)
		{
			return this.PageNumber(delegate(IPageContext x)
			{
				DocumentLocation location = x.GetLocation(sectionName);
				if (location == null)
				{
					return default(int?);
				}
				return new int?(location.PageStart);
			});
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0000CA28 File Offset: 0x0000AC28
		public TextPageNumberDescriptor EndPageNumberOfSection(string sectionName)
		{
			return this.PageNumber(delegate(IPageContext x)
			{
				DocumentLocation location = x.GetLocation(sectionName);
				if (location == null)
				{
					return default(int?);
				}
				return new int?(location.PageEnd);
			});
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0000CA54 File Offset: 0x0000AC54
		public TextPageNumberDescriptor PageNumberWithinSection(string sectionName)
		{
			return this.PageNumber(delegate(IPageContext x)
			{
				int num = x.CurrentPage + 1;
				DocumentLocation location = x.GetLocation(sectionName);
				int? num2 = (location != null) ? new int?(location.PageStart) : default(int?);
				if (num2 == null)
				{
					return default(int?);
				}
				return new int?(num - num2.GetValueOrDefault());
			});
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0000CA80 File Offset: 0x0000AC80
		public TextPageNumberDescriptor TotalPagesWithinSection(string sectionName)
		{
			return this.PageNumber(delegate(IPageContext x)
			{
				DocumentLocation location = x.GetLocation(sectionName);
				if (location == null)
				{
					return default(int?);
				}
				return new int?(location.Length);
			});
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0000CAAC File Offset: 0x0000ACAC
		public TextSpanDescriptor SectionLink([Nullable(2)] string text, string sectionName)
		{
			if (string.IsNullOrWhiteSpace(sectionName))
			{
				throw new ArgumentException("Section name cannot be null or whitespace.", "sectionName");
			}
			if (text == null)
			{
				return new TextSpanDescriptor(new TextBlockSpan());
			}
			TextBlockSectionLink textBlockSectionLink = new TextBlockSectionLink
			{
				Text = text,
				SectionName = sectionName
			};
			this.TextBlock.Items.Add(textBlockSectionLink);
			return new TextSpanDescriptor(textBlockSectionLink);
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0000CB0A File Offset: 0x0000AD0A
		[NullableContext(2)]
		[Obsolete("This element has been renamed since version 2022.3. Please use the SectionLink method.")]
		[ExcludeFromCodeCoverage]
		public void InternalLocation(string text, [Nullable(1)] string locationName, TextStyle style = null)
		{
			this.SectionLink(text, locationName).Style(style);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0000CB1C File Offset: 0x0000AD1C
		public TextSpanDescriptor Hyperlink([Nullable(2)] string text, string url)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				throw new ArgumentException("Url cannot be null or whitespace.", "url");
			}
			if (text == null)
			{
				return new TextSpanDescriptor(new TextBlockSpan());
			}
			TextBlockHyperlink textBlockHyperlink = new TextBlockHyperlink
			{
				Text = text,
				Url = url
			};
			this.TextBlock.Items.Add(textBlockHyperlink);
			return new TextSpanDescriptor(textBlockHyperlink);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0000CB7A File Offset: 0x0000AD7A
		[NullableContext(2)]
		[Obsolete("This element has been renamed since version 2022.3. Please use the Hyperlink method.")]
		[ExcludeFromCodeCoverage]
		public void ExternalLocation(string text, [Nullable(1)] string url, TextStyle style = null)
		{
			this.Hyperlink(text, url).Style(style);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0000CB8C File Offset: 0x0000AD8C
		public IContainer Element(TextInjectedElementAlignment alignment = TextInjectedElementAlignment.AboveBaseline)
		{
			Container element = new Container();
			this.TextBlock.Items.Add(new TextBlockElement
			{
				Element = element,
				Alignment = alignment
			});
			return element.AlignBottom().MinimalBox();
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0000CBCD File Offset: 0x0000ADCD
		public void Element(Action<IContainer> handler, TextInjectedElementAlignment alignment = TextInjectedElementAlignment.AboveBaseline)
		{
			handler.Invoke(this.Element(alignment));
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0000CBDC File Offset: 0x0000ADDC
		internal void Compose(IContainer container)
		{
			if (this.DefaultStyle != null)
			{
				container = container.DefaultTextStyle(this.DefaultStyle);
			}
			container.Element(this.TextBlock);
		}

		// Token: 0x04000291 RID: 657
		internal const string DefaultLineClampEllipsis = "…";
	}
}
