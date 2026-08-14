using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using HtmlAgilityPack;

namespace VintageStoryModManager.Helpers
{
	// Token: 0x020000C7 RID: 199
	[NullableContext(1)]
	[Nullable(0)]
	public static class HtmlToFlowDocument
	{
		// Token: 0x06001009 RID: 4105 RVA: 0x00054E08 File Offset: 0x00053008
		[NullableContext(2)]
		[return: Nullable(1)]
		public static FlowDocument Convert(string html, Brush foreground, FontFamily fontFamily, double fontSize)
		{
			FlowDocument flowDocument = new FlowDocument
			{
				PagePadding = new Thickness(24.0, 16.0, 24.0, 20.0),
				FontSize = ((fontSize > 0.0) ? fontSize : 14.0)
			};
			if (foreground != null)
			{
				flowDocument.Foreground = foreground;
			}
			if (fontFamily != null)
			{
				flowDocument.FontFamily = fontFamily;
			}
			if (string.IsNullOrWhiteSpace(html))
			{
				return flowDocument;
			}
			try
			{
				HtmlDocument htmlDocument = new HtmlDocument();
				htmlDocument.OptionFixNestedTags = true;
				htmlDocument.LoadHtml(html);
				foreach (Block block in HtmlToFlowDocument.ParseBlocks(htmlDocument.DocumentNode.ChildNodes))
				{
					flowDocument.Blocks.Add(block);
				}
			}
			catch (Exception)
			{
				flowDocument.Blocks.Add(new Paragraph(new Run(HtmlToFlowDocument.StripTags(html))));
			}
			return flowDocument;
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x00054F1C File Offset: 0x0005311C
		private static List<Block> ParseBlocks([Nullable(2)] HtmlNodeCollection nodes)
		{
			HtmlToFlowDocument.<>c__DisplayClass8_0 CS$<>8__locals1;
			CS$<>8__locals1.blocks = new List<Block>();
			if (nodes == null)
			{
				return CS$<>8__locals1.blocks;
			}
			CS$<>8__locals1.pending = null;
			foreach (HtmlNode htmlNode in nodes)
			{
				if (htmlNode.NodeType == 1 && HtmlToFlowDocument.BlockElements.Contains(htmlNode.Name))
				{
					HtmlToFlowDocument.<ParseBlocks>g__FlushPending|8_0(ref CS$<>8__locals1);
					CS$<>8__locals1.blocks.AddRange(HtmlToFlowDocument.ParseBlockElement(htmlNode));
				}
				else
				{
					if (CS$<>8__locals1.pending == null)
					{
						CS$<>8__locals1.pending = new List<Inline>();
					}
					CS$<>8__locals1.pending.AddRange(HtmlToFlowDocument.ParseInlines(htmlNode));
				}
			}
			HtmlToFlowDocument.<ParseBlocks>g__FlushPending|8_0(ref CS$<>8__locals1);
			return CS$<>8__locals1.blocks;
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x00054FE8 File Offset: 0x000531E8
		private static IEnumerable<Block> ParseBlockElement(HtmlNode node)
		{
			string text = node.Name.ToLowerInvariant();
			if (text != null)
			{
				int length = text.Length;
				switch (length)
				{
				case 2:
				{
					char c = text.get_Chars(1);
					if (c <= 'i')
					{
						switch (c)
						{
						case '1':
							if (text == "h1")
							{
								return new Paragraph[]
								{
									HtmlToFlowDocument.CreateHeading(node, 24.0)
								};
							}
							break;
						case '2':
							if (text == "h2")
							{
								return new Paragraph[]
								{
									HtmlToFlowDocument.CreateHeading(node, 21.0)
								};
							}
							break;
						case '3':
							if (text == "h3")
							{
								return new Paragraph[]
								{
									HtmlToFlowDocument.CreateHeading(node, 18.0)
								};
							}
							break;
						case '4':
							if (text == "h4")
							{
								return new Paragraph[]
								{
									HtmlToFlowDocument.CreateHeading(node, 16.0)
								};
							}
							break;
						case '5':
							if (text == "h5")
							{
								return new Paragraph[]
								{
									HtmlToFlowDocument.CreateHeading(node, 14.0)
								};
							}
							break;
						case '6':
							if (text == "h6")
							{
								return new Paragraph[]
								{
									HtmlToFlowDocument.CreateHeading(node, 13.0)
								};
							}
							break;
						default:
							if (c == 'i')
							{
								if (text == "li")
								{
									return HtmlToFlowDocument.ParseBlocks(node.ChildNodes);
								}
							}
							break;
						}
					}
					else if (c != 'l')
					{
						if (c == 'r')
						{
							if (text == "hr")
							{
								return new Block[]
								{
									HtmlToFlowDocument.CreateHorizontalRule()
								};
							}
						}
					}
					else
					{
						if (text == "ul")
						{
							return new Block[]
							{
								HtmlToFlowDocument.CreateList(node, 1)
							};
						}
						if (text == "ol")
						{
							return new Block[]
							{
								HtmlToFlowDocument.CreateList(node, 9)
							};
						}
					}
					break;
				}
				case 3:
					if (text == "pre")
					{
						return new Paragraph[]
						{
							HtmlToFlowDocument.CreatePreformatted(node)
						};
					}
					break;
				case 4:
					break;
				case 5:
					if (text == "table")
					{
						return new Block[]
						{
							HtmlToFlowDocument.CreateTable(node)
						};
					}
					break;
				default:
					if (length == 10)
					{
						if (text == "blockquote")
						{
							return new Block[]
							{
								HtmlToFlowDocument.CreateBlockquote(node)
							};
						}
					}
					break;
				}
			}
			return HtmlToFlowDocument.ParseBlocks(node.ChildNodes);
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x000552A0 File Offset: 0x000534A0
		private static Paragraph CreateHeading(HtmlNode node, double fontSize)
		{
			Paragraph paragraph = new Paragraph
			{
				FontSize = fontSize,
				FontWeight = FontWeights.SemiBold,
				Margin = new Thickness(0.0, 10.0, 0.0, 6.0)
			};
			foreach (Inline inline in HtmlToFlowDocument.ParseInlines(node))
			{
				paragraph.Inlines.Add(inline);
			}
			return paragraph;
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0005533C File Offset: 0x0005353C
		private static List CreateList(HtmlNode node, TextMarkerStyle markerStyle)
		{
			List list = new List
			{
				MarkerStyle = markerStyle,
				Margin = new Thickness(0.0, 4.0, 0.0, 8.0),
				Padding = new Thickness(20.0, 0.0, 0.0, 0.0)
			};
			foreach (HtmlNode htmlNode in node.ChildNodes)
			{
				if (htmlNode.NodeType == 1 && string.Equals(htmlNode.Name, "li", 5))
				{
					ListItem listItem = new ListItem();
					List<Block> list2 = HtmlToFlowDocument.ParseBlocks(htmlNode.ChildNodes);
					if (list2.Count == 0)
					{
						list2.Add(new Paragraph());
					}
					foreach (Block block in list2)
					{
						listItem.Blocks.Add(block);
					}
					list.ListItems.Add(listItem);
				}
			}
			return list;
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x00055494 File Offset: 0x00053694
		private static Section CreateBlockquote(HtmlNode node)
		{
			Section section = new Section
			{
				Margin = new Thickness(2.0, 4.0, 0.0, 8.0),
				Padding = new Thickness(12.0, 0.0, 0.0, 0.0),
				BorderBrush = HtmlToFlowDocument.QuoteBorder,
				BorderThickness = new Thickness(3.0, 0.0, 0.0, 0.0)
			};
			foreach (Block block in HtmlToFlowDocument.ParseBlocks(node.ChildNodes))
			{
				section.Blocks.Add(block);
			}
			return section;
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x00055590 File Offset: 0x00053790
		private static Paragraph CreatePreformatted(HtmlNode node)
		{
			Paragraph paragraph = new Paragraph
			{
				FontFamily = new FontFamily("Consolas, Courier New, monospace"),
				Background = HtmlToFlowDocument.CodeBackground,
				Padding = new Thickness(10.0, 6.0, 10.0, 6.0),
				Margin = new Thickness(0.0, 4.0, 0.0, 8.0)
			};
			string[] array = (HtmlEntity.DeEntitize(node.InnerText) ?? string.Empty).Replace("\r\n", "\n", 4).Replace('\r', '\n').Split('\n', 0);
			for (int i = 0; i < array.Length; i++)
			{
				paragraph.Inlines.Add(new Run(array[i]));
				if (i < array.Length - 1)
				{
					paragraph.Inlines.Add(new LineBreak());
				}
			}
			return paragraph;
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0005568C File Offset: 0x0005388C
		private static BlockUIContainer CreateHorizontalRule()
		{
			return new BlockUIContainer(new Border
			{
				Height = 1.0,
				Background = HtmlToFlowDocument.RuleBrush,
				Margin = new Thickness(0.0, 8.0, 0.0, 8.0)
			});
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x000556EC File Offset: 0x000538EC
		private static Table CreateTable(HtmlNode node)
		{
			Table table = new Table
			{
				CellSpacing = 0.0,
				Margin = new Thickness(0.0, 4.0, 0.0, 8.0)
			};
			TableRowGroup tableRowGroup = new TableRowGroup();
			int num = 0;
			foreach (HtmlNode htmlNode in node.Descendants("tr"))
			{
				TableRow tableRow = new TableRow();
				List<HtmlNode> list = Enumerable.ToList<HtmlNode>(Enumerable.Where<HtmlNode>(htmlNode.ChildNodes, (HtmlNode c) => c.NodeType == 1 && (string.Equals(c.Name, "td", 5) || string.Equals(c.Name, "th", 5))));
				foreach (HtmlNode htmlNode2 in list)
				{
					bool flag = string.Equals(htmlNode2.Name, "th", 5);
					TableCell tableCell = new TableCell
					{
						Padding = new Thickness(8.0, 4.0, 8.0, 4.0),
						BorderBrush = HtmlToFlowDocument.QuoteBorder,
						BorderThickness = new Thickness(0.0, 0.0, 1.0, 1.0)
					};
					foreach (Block block in HtmlToFlowDocument.ParseBlocks(htmlNode2.ChildNodes))
					{
						if (flag)
						{
							block.FontWeight = FontWeights.SemiBold;
						}
						tableCell.Blocks.Add(block);
					}
					if (tableCell.Blocks.Count == 0)
					{
						tableCell.Blocks.Add(new Paragraph());
					}
					tableRow.Cells.Add(tableCell);
				}
				num = Math.Max(num, list.Count);
				if (tableRow.Cells.Count > 0)
				{
					tableRowGroup.Rows.Add(tableRow);
				}
			}
			for (int i = 0; i < num; i++)
			{
				table.Columns.Add(new TableColumn());
			}
			table.RowGroups.Add(tableRowGroup);
			return table;
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x00055988 File Offset: 0x00053B88
		private static IEnumerable<Inline> ParseInlines(HtmlNode node)
		{
			HtmlToFlowDocument.<ParseInlines>d__16 <ParseInlines>d__ = new HtmlToFlowDocument.<ParseInlines>d__16(-2);
			<ParseInlines>d__.<>3__node = node;
			return <ParseInlines>d__;
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x00055998 File Offset: 0x00053B98
		private static Span WrapChildren(HtmlNode node, Span span)
		{
			foreach (Inline inline in HtmlToFlowDocument.ParseChildInlines(node))
			{
				span.Inlines.Add(inline);
			}
			return span;
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x000559EC File Offset: 0x00053BEC
		private static IEnumerable<Inline> ParseChildInlines(HtmlNode node)
		{
			HtmlToFlowDocument.<ParseChildInlines>d__18 <ParseChildInlines>d__ = new HtmlToFlowDocument.<ParseChildInlines>d__18(-2);
			<ParseChildInlines>d__.<>3__node = node;
			return <ParseChildInlines>d__;
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x000559FC File Offset: 0x00053BFC
		private static Inline CreateHyperlink(HtmlNode node)
		{
			Hyperlink hyperlink = new Hyperlink
			{
				Foreground = HtmlToFlowDocument.LinkBrush
			};
			foreach (Inline inline in HtmlToFlowDocument.ParseChildInlines(node))
			{
				hyperlink.Inlines.Add(inline);
			}
			if (hyperlink.Inlines.Count == 0)
			{
				hyperlink.Inlines.Add(new Run(node.GetAttributeValue("href", string.Empty)));
			}
			HtmlAttribute htmlAttribute = node.Attributes["href"];
			Uri uri = HtmlToFlowDocument.ResolveUri((htmlAttribute != null) ? htmlAttribute.Value : null);
			if (uri != null)
			{
				hyperlink.NavigateUri = uri;
				hyperlink.ToolTip = uri.ToString();
				Hyperlink hyperlink2 = hyperlink;
				RequestNavigateEventHandler requestNavigateEventHandler;
				if ((requestNavigateEventHandler = HtmlToFlowDocument.<>O.<0>__OnRequestNavigate) == null)
				{
					requestNavigateEventHandler = (HtmlToFlowDocument.<>O.<0>__OnRequestNavigate = new RequestNavigateEventHandler(HtmlToFlowDocument.OnRequestNavigate));
				}
				hyperlink2.RequestNavigate += requestNavigateEventHandler;
			}
			return hyperlink;
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x00055AEC File Offset: 0x00053CEC
		[return: Nullable(2)]
		private static Inline CreateImage(HtmlNode node)
		{
			HtmlAttribute htmlAttribute = node.Attributes["src"];
			Uri uri = HtmlToFlowDocument.ResolveUri((htmlAttribute != null) ? htmlAttribute.Value : null);
			if (uri == null)
			{
				return null;
			}
			Inline result;
			try
			{
				BitmapImage bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.UriSource = uri;
				bitmapImage.CreateOptions = 4;
				bitmapImage.EndInit();
				Image image = new Image
				{
					Source = bitmapImage,
					Stretch = 2,
					HorizontalAlignment = 0
				};
				double? num = HtmlToFlowDocument.ReadImageDimension(node, "width");
				double? num2 = HtmlToFlowDocument.ReadImageDimension(node, "height");
				if (num != null || num2 != null)
				{
					if (num != null)
					{
						image.Width = Math.Min(num.Value, 640.0);
					}
					if (num2 != null)
					{
						image.Height = num2.Value;
					}
					image.StretchDirection = 2;
				}
				else
				{
					image.MaxWidth = 640.0;
					image.StretchDirection = 1;
				}
				result = new InlineUIContainer(image);
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x00055C08 File Offset: 0x00053E08
		private static double? ReadImageDimension(HtmlNode node, string name)
		{
			HtmlAttribute htmlAttribute = node.Attributes["style"];
			string text = (htmlAttribute != null) ? htmlAttribute.Value : null;
			if (!string.IsNullOrEmpty(text))
			{
				Match match = Regex.Match(text, "(?:^|;)\\s*" + name + "\\s*:\\s*(\\d+(?:\\.\\d+)?)\\s*px", 1);
				double num;
				if (match.Success && double.TryParse(match.Groups[1].Value, 167, CultureInfo.InvariantCulture, ref num) && num > 0.0)
				{
					return new double?(num);
				}
			}
			HtmlAttribute htmlAttribute2 = node.Attributes[name];
			string text2;
			if (htmlAttribute2 == null)
			{
				text2 = null;
			}
			else
			{
				string value = htmlAttribute2.Value;
				text2 = ((value != null) ? value.Trim() : null);
			}
			string text3 = text2;
			if (string.IsNullOrEmpty(text3) || text3.EndsWith("%", 4))
			{
				return default(double?);
			}
			text3 = text3.Replace("px", string.Empty, 5).Trim();
			double num2;
			if (double.TryParse(text3, 167, CultureInfo.InvariantCulture, ref num2) && num2 > 0.0)
			{
				return new double?(num2);
			}
			return default(double?);
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x00055D24 File Offset: 0x00053F24
		private static void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			try
			{
				Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri)
				{
					UseShellExecute = true
				});
			}
			catch (Exception)
			{
			}
			e.Handled = true;
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x00055D6C File Offset: 0x00053F6C
		private static bool InlinesHaveContent(IEnumerable<Inline> inlines)
		{
			foreach (Inline inline in inlines)
			{
				Run run = inline as Run;
				if (run == null)
				{
					if (!(inline is LineBreak) && !(inline is InlineUIContainer))
					{
						Span span = inline as Span;
						if (span == null)
						{
							continue;
						}
						if (HtmlToFlowDocument.InlinesHaveContent(span.Inlines))
						{
							return true;
						}
						continue;
					}
				}
				else if (string.IsNullOrWhiteSpace(run.Text))
				{
					continue;
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x00055DFC File Offset: 0x00053FFC
		private static string NormalizeText([Nullable(2)] string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(value.Length);
			bool flag = false;
			for (int i = 0; i < value.Length; i++)
			{
				char c = value.get_Chars(i);
				if (char.IsWhiteSpace(c))
				{
					if (!flag)
					{
						stringBuilder.Append(' ');
					}
					flag = true;
				}
				else
				{
					stringBuilder.Append(c);
					flag = false;
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x00055E6C File Offset: 0x0005406C
		[NullableContext(2)]
		private static Uri ResolveUri(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}
			string text = value.Trim();
			if (text.StartsWith("//", 4))
			{
				text = "https:" + text;
			}
			else if (text.StartsWith("/", 4))
			{
				text = "https://mods.vintagestory.at" + text;
			}
			Uri uri;
			if (Uri.TryCreate(text, 1, ref uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
			{
				return uri;
			}
			return null;
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x00055EF4 File Offset: 0x000540F4
		private static string StripTags(string html)
		{
			string result;
			try
			{
				HtmlDocument htmlDocument = new HtmlDocument();
				htmlDocument.LoadHtml(html);
				string text = HtmlEntity.DeEntitize(htmlDocument.DocumentNode.InnerText);
				result = (((text != null) ? text.Trim() : null) ?? string.Empty);
			}
			catch
			{
				result = html;
			}
			return result;
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x00055F4C File Offset: 0x0005414C
		private static SolidColorBrush CreateFrozenBrush(Color color)
		{
			SolidColorBrush solidColorBrush = new SolidColorBrush(color);
			solidColorBrush.Freeze();
			return solidColorBrush;
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x00055F5C File Offset: 0x0005415C
		// Note: this type is marked as 'beforefieldinit'.
		static HtmlToFlowDocument()
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			hashSet.Add("p");
			hashSet.Add("div");
			hashSet.Add("section");
			hashSet.Add("article");
			hashSet.Add("header");
			hashSet.Add("footer");
			hashSet.Add("main");
			hashSet.Add("aside");
			hashSet.Add("figure");
			hashSet.Add("figcaption");
			hashSet.Add("h1");
			hashSet.Add("h2");
			hashSet.Add("h3");
			hashSet.Add("h4");
			hashSet.Add("h5");
			hashSet.Add("h6");
			hashSet.Add("ul");
			hashSet.Add("ol");
			hashSet.Add("li");
			hashSet.Add("blockquote");
			hashSet.Add("pre");
			hashSet.Add("hr");
			hashSet.Add("table");
			HtmlToFlowDocument.BlockElements = hashSet;
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x0005610C File Offset: 0x0005430C
		[CompilerGenerated]
		internal static void <ParseBlocks>g__FlushPending|8_0(ref HtmlToFlowDocument.<>c__DisplayClass8_0 A_0)
		{
			if (A_0.pending == null)
			{
				return;
			}
			if (HtmlToFlowDocument.InlinesHaveContent(A_0.pending))
			{
				Paragraph paragraph = new Paragraph();
				foreach (Inline inline in A_0.pending)
				{
					paragraph.Inlines.Add(inline);
				}
				A_0.blocks.Add(paragraph);
			}
			A_0.pending = null;
		}

		// Token: 0x04000704 RID: 1796
		private const string BaseUrl = "https://mods.vintagestory.at";

		// Token: 0x04000705 RID: 1797
		private const double MaxImageWidth = 640.0;

		// Token: 0x04000706 RID: 1798
		private static readonly SolidColorBrush LinkBrush = HtmlToFlowDocument.CreateFrozenBrush(Color.FromRgb(78, 161, 247));

		// Token: 0x04000707 RID: 1799
		private static readonly SolidColorBrush CodeBackground = HtmlToFlowDocument.CreateFrozenBrush(Color.FromArgb(51, 128, 128, 128));

		// Token: 0x04000708 RID: 1800
		private static readonly SolidColorBrush QuoteBorder = HtmlToFlowDocument.CreateFrozenBrush(Color.FromArgb(128, 128, 128, 128));

		// Token: 0x04000709 RID: 1801
		private static readonly SolidColorBrush RuleBrush = HtmlToFlowDocument.CreateFrozenBrush(Color.FromArgb(85, 128, 128, 128));

		// Token: 0x0400070A RID: 1802
		private static readonly HashSet<string> BlockElements;

		// Token: 0x02000318 RID: 792
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040013CB RID: 5067
			[Nullable(0)]
			public static RequestNavigateEventHandler <0>__OnRequestNavigate;
		}
	}
}
