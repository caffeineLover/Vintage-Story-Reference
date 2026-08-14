using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UglyToad.PdfPig.Actions;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Graphics.Colors;
using UglyToad.PdfPig.Graphics.Operations;
using UglyToad.PdfPig.Graphics.Operations.General;
using UglyToad.PdfPig.Graphics.Operations.PathConstruction;
using UglyToad.PdfPig.Graphics.Operations.PathPainting;
using UglyToad.PdfPig.Graphics.Operations.SpecialGraphicsState;
using UglyToad.PdfPig.Graphics.Operations.TextObjects;
using UglyToad.PdfPig.Graphics.Operations.TextPositioning;
using UglyToad.PdfPig.Graphics.Operations.TextShowing;
using UglyToad.PdfPig.Graphics.Operations.TextState;
using UglyToad.PdfPig.Images;
using UglyToad.PdfPig.Images.Png;
using UglyToad.PdfPig.PdfFonts;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Writer.Fonts;

namespace UglyToad.PdfPig.Writer
{
	// Token: 0x02000018 RID: 24
	public class PdfPageBuilder
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00005A84 File Offset: 0x00003C84
		internal IReadOnlyDictionary<string, IToken> Resources
		{
			get
			{
				return this.pageDictionary.GetOrCreateDict(NameToken.Resources);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00005A96 File Offset: 0x00003C96
		public int PageNumber { get; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00005A9E File Offset: 0x00003C9E
		// (set) Token: 0x06000098 RID: 152 RVA: 0x00005AA6 File Offset: 0x00003CA6
		public PdfRectangle PageSize { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00005AAF File Offset: 0x00003CAF
		public PdfPageBuilder.IContentStream CurrentStream
		{
			get
			{
				return this.currentStream;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00005AB7 File Offset: 0x00003CB7
		public IReadOnlyList<PdfPageBuilder.IContentStream> ContentStreams
		{
			get
			{
				return this.contentStreams;
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00005AC0 File Offset: 0x00003CC0
		internal PdfPageBuilder(int number, PdfDocumentBuilder documentBuilder)
		{
			if (documentBuilder == null)
			{
				throw new ArgumentNullException("documentBuilder");
			}
			this.documentBuilder = documentBuilder;
			this.PageNumber = number;
			this.currentStream = new PdfPageBuilder.DefaultContentStream();
			List<PdfPageBuilder.IPageContentStream> list = new List<PdfPageBuilder.IPageContentStream>();
			list.Add(this.currentStream);
			this.contentStreams = list;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00005B50 File Offset: 0x00003D50
		internal PdfPageBuilder(int number, PdfDocumentBuilder documentBuilder, IEnumerable<PdfPageBuilder.CopiedContentStream> copied, Dictionary<NameToken, IToken> pageDict, [TupleElementNames(new string[]
		{
			"token",
			"action"
		})] List<ValueTuple<DictionaryToken, PdfAction>> links)
		{
			if (documentBuilder == null)
			{
				throw new ArgumentNullException("documentBuilder");
			}
			this.documentBuilder = documentBuilder;
			this.links = links;
			this.PageNumber = number;
			this.pageDictionary = pageDict;
			this.contentStreams = new List<PdfPageBuilder.IPageContentStream>();
			this.contentStreams.AddRange(copied);
			this.currentStream = new PdfPageBuilder.DefaultContentStream();
			this.contentStreams.Add(this.currentStream);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00005C00 File Offset: 0x00003E00
		public void NewContentStreamBefore()
		{
			int num = Math.Max(this.contentStreams.IndexOf(this.currentStream) - 1, 0);
			this.currentStream = new PdfPageBuilder.DefaultContentStream();
			this.contentStreams.Insert(num, this.currentStream);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00005C44 File Offset: 0x00003E44
		public void NewContentStreamAfter()
		{
			int num = Math.Min(this.contentStreams.IndexOf(this.currentStream) + 1, this.contentStreams.Count);
			this.currentStream = new PdfPageBuilder.DefaultContentStream();
			this.contentStreams.Insert(num, this.currentStream);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00005C92 File Offset: 0x00003E92
		public void SelectContentStream(int index)
		{
			if (index < 0 || index >= this.contentStreams.Count)
			{
				throw new IndexOutOfRangeException("index");
			}
			this.currentStream = this.contentStreams[index];
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00005CC4 File Offset: 0x00003EC4
		public PdfPageBuilder DrawLine(PdfPoint from, PdfPoint to, [Optional] decimal lineWidth = 1m)
		{
			if (lineWidth != 1m)
			{
				this.currentStream.Add(new SetLineWidth(lineWidth));
			}
			this.currentStream.Add(new BeginNewSubpath((decimal)from.X, (decimal)from.Y));
			this.currentStream.Add(new AppendStraightLineSegment((decimal)to.X, (decimal)to.Y));
			this.currentStream.Add(StrokePath.Value);
			if (lineWidth != 1m)
			{
				this.currentStream.Add(new SetLineWidth(1m));
			}
			return this;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005D74 File Offset: 0x00003F74
		public PdfPageBuilder DrawRectangle(PdfPoint position, decimal width, decimal height, [Optional] decimal lineWidth = 1m, bool fill = false)
		{
			if (lineWidth != 1m)
			{
				this.currentStream.Add(new SetLineWidth(lineWidth));
			}
			this.currentStream.Add(new AppendRectangle((decimal)position.X, (decimal)position.Y, width, height));
			if (fill)
			{
				this.currentStream.Add(FillPathEvenOddRuleAndStroke.Value);
			}
			else
			{
				this.currentStream.Add(StrokePath.Value);
			}
			if (lineWidth != 1m)
			{
				this.currentStream.Add(new SetLineWidth(lineWidth));
			}
			return this;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005E12 File Offset: 0x00004012
		public PdfPageBuilder SetRotation(PageRotationDegrees degrees)
		{
			this.rotation = new int?(degrees.Value);
			return this;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005E28 File Offset: 0x00004028
		public PdfPageBuilder DrawTriangle(PdfPoint point1, PdfPoint point2, PdfPoint point3, [Optional] decimal lineWidth = 1m, bool fill = false)
		{
			if (lineWidth != 1m)
			{
				this.currentStream.Add(new SetLineWidth(lineWidth));
			}
			this.currentStream.Add(new BeginNewSubpath((decimal)point1.X, (decimal)point1.Y));
			this.currentStream.Add(new AppendStraightLineSegment((decimal)point2.X, (decimal)point2.Y));
			this.currentStream.Add(new AppendStraightLineSegment((decimal)point3.X, (decimal)point3.Y));
			this.currentStream.Add(new AppendStraightLineSegment((decimal)point1.X, (decimal)point1.Y));
			if (fill)
			{
				this.currentStream.Add(FillPathEvenOddRuleAndStroke.Value);
			}
			else
			{
				this.currentStream.Add(StrokePath.Value);
			}
			if (lineWidth != 1m)
			{
				this.currentStream.Add(new SetLineWidth(lineWidth));
			}
			return this;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00005F3C File Offset: 0x0000413C
		public PdfPageBuilder DrawCircle(PdfPoint center, decimal diameter, [Optional] decimal lineWidth = 1m, bool fill = false)
		{
			this.DrawEllipsis(center, diameter, diameter, lineWidth, fill);
			return this;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005F4C File Offset: 0x0000414C
		public PdfPageBuilder DrawEllipsis(PdfPoint center, decimal width, decimal height, [Optional] decimal lineWidth = 1m, bool fill = false)
		{
			width /= 2m;
			height /= 2m;
			decimal num = 0.55228474983079m;
			if (lineWidth != 1m)
			{
				this.currentStream.Add(new SetLineWidth(lineWidth));
			}
			this.currentStream.Add(new BeginNewSubpath((decimal)center.X - width, (decimal)center.Y));
			this.currentStream.Add(new AppendDualControlPointBezierCurve((decimal)center.X - width, (decimal)center.Y + height * num, (decimal)center.X - width * num, (decimal)center.Y + height, (decimal)center.X, (decimal)center.Y + height));
			this.currentStream.Add(new AppendDualControlPointBezierCurve((decimal)center.X + width * num, (decimal)center.Y + height, (decimal)center.X + width, (decimal)center.Y + height * num, (decimal)center.X + width, (decimal)center.Y));
			this.currentStream.Add(new AppendDualControlPointBezierCurve((decimal)center.X + width, (decimal)center.Y - height * num, (decimal)center.X + width * num, (decimal)center.Y - height, (decimal)center.X, (decimal)center.Y - height));
			this.currentStream.Add(new AppendDualControlPointBezierCurve((decimal)center.X - width * num, (decimal)center.Y - height, (decimal)center.X - width, (decimal)center.Y - height * num, (decimal)center.X - width, (decimal)center.Y));
			if (fill)
			{
				this.currentStream.Add(FillPathEvenOddRuleAndStroke.Value);
			}
			else
			{
				this.currentStream.Add(StrokePath.Value);
			}
			if (lineWidth != 1m)
			{
				this.currentStream.Add(new SetLineWidth(lineWidth));
			}
			return this;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00006227 File Offset: 0x00004427
		public PdfPageBuilder SetStrokeColor(byte r, byte g, byte b)
		{
			this.currentStream.Add(Push.Value);
			this.currentStream.Add(new SetStrokeColorDeviceRgb(PdfPageBuilder.RgbToDecimal(r), PdfPageBuilder.RgbToDecimal(g), PdfPageBuilder.RgbToDecimal(b)));
			return this;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000625C File Offset: 0x0000445C
		internal PdfPageBuilder SetStrokeColorExact(decimal r, decimal g, decimal b)
		{
			this.currentStream.Add(Push.Value);
			this.currentStream.Add(new SetStrokeColorDeviceRgb(PdfPageBuilder.CheckRgbDecimal(r, "r"), PdfPageBuilder.CheckRgbDecimal(g, "g"), PdfPageBuilder.CheckRgbDecimal(b, "b")));
			return this;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000062AB File Offset: 0x000044AB
		public PdfPageBuilder SetTextAndFillColor(byte r, byte g, byte b)
		{
			this.currentStream.Add(Push.Value);
			this.currentStream.Add(new SetNonStrokeColorDeviceRgb(PdfPageBuilder.RgbToDecimal(r), PdfPageBuilder.RgbToDecimal(g), PdfPageBuilder.RgbToDecimal(b)));
			return this;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000062E0 File Offset: 0x000044E0
		public PdfPageBuilder ResetColor()
		{
			this.currentStream.Add(Pop.Value);
			return this;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000062F4 File Offset: 0x000044F4
		public IReadOnlyList<Letter> MeasureText(string text, decimal fontSize, PdfPoint position, PdfDocumentBuilder.AddedFont font)
		{
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			PdfDocumentBuilder.FontStored fontStored;
			if (!this.documentBuilder.Fonts.TryGetValue(font.Id, ref fontStored))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(84, 2);
				defaultInterpolatedStringHandler.AppendLiteral("No font has been added to the PdfDocumentBuilder with Id: ");
				defaultInterpolatedStringHandler.AppendFormatted<Guid>(font.Id);
				defaultInterpolatedStringHandler.AppendLiteral(". ");
				defaultInterpolatedStringHandler.AppendLiteral("Use ");
				defaultInterpolatedStringHandler.AppendFormatted("AddTrueTypeFont");
				defaultInterpolatedStringHandler.AppendLiteral(" to register a font.");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), "font");
			}
			if (fontSize <= 0m)
			{
				throw new ArgumentOutOfRangeException("fontSize", "Font size must be greater than 0");
			}
			IWritingFont fontProgram = fontStored.FontProgram;
			TransformationMatrix fontMatrix = fontProgram.GetFontMatrix();
			TransformationMatrix textMatrix = TransformationMatrix.FromValues(1.0, 0.0, 0.0, 1.0, position.X, position.Y);
			return this.DrawLetters(null, text, fontProgram, fontMatrix, fontSize, textMatrix);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00006410 File Offset: 0x00004610
		public IReadOnlyList<Letter> AddText(string text, decimal fontSize, PdfPoint position, PdfDocumentBuilder.AddedFont font)
		{
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			PdfDocumentBuilder.FontStored fontStored;
			if (!this.documentBuilder.Fonts.TryGetValue(font.Id, ref fontStored))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(84, 2);
				defaultInterpolatedStringHandler.AppendLiteral("No font has been added to the PdfDocumentBuilder with Id: ");
				defaultInterpolatedStringHandler.AppendFormatted<Guid>(font.Id);
				defaultInterpolatedStringHandler.AppendLiteral(". ");
				defaultInterpolatedStringHandler.AppendLiteral("Use ");
				defaultInterpolatedStringHandler.AppendFormatted("AddTrueTypeFont");
				defaultInterpolatedStringHandler.AppendLiteral(" to register a font.");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), "font");
			}
			if (fontSize <= 0m)
			{
				throw new ArgumentOutOfRangeException("fontSize", "Font size must be greater than 0");
			}
			NameToken addedFont = this.GetAddedFont(font);
			IWritingFont fontProgram = fontStored.FontProgram;
			TransformationMatrix fontMatrix = fontProgram.GetFontMatrix();
			TransformationMatrix textMatrix = TransformationMatrix.FromValues(1.0, 0.0, 0.0, 1.0, position.X, position.Y);
			List<Letter> result = this.DrawLetters(addedFont, text, fontProgram, fontMatrix, fontSize, textMatrix);
			this.currentStream.Add(BeginText.Value);
			this.currentStream.Add(new SetFontAndSize(addedFont, fontSize));
			this.currentStream.Add(new MoveToNextLineWithOffset((decimal)position.X, (decimal)position.Y));
			List<byte> list = new List<byte>();
			for (int i = 0; i < text.Length; i++)
			{
				char c = text.get_Chars(i);
				if (char.IsWhiteSpace(c))
				{
					this.currentStream.Add(new ShowText(list.ToArray()));
					list.Clear();
				}
				byte valueForCharacter = fontProgram.GetValueForCharacter(c);
				list.Add(valueForCharacter);
			}
			if (list.Count > 0)
			{
				this.currentStream.Add(new ShowText(list.ToArray()));
			}
			this.currentStream.Add(EndText.Value);
			return result;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00006619 File Offset: 0x00004819
		public PdfPageBuilder SetTextRenderingMode(TextRenderingMode mode)
		{
			this.currentStream.Add(new SetTextRenderingMode(mode));
			return this;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00006630 File Offset: 0x00004830
		private NameToken GetAddedFont(PdfDocumentBuilder.AddedFont font)
		{
			NameToken nameToken;
			if (!this.documentFonts.TryGetValue(font.Id, ref nameToken))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(1, 1);
				defaultInterpolatedStringHandler.AppendLiteral("F");
				int num = this.nextFontId;
				this.nextFontId = num + 1;
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				nameToken = NameToken.Create(defaultInterpolatedStringHandler.ToStringAndClear());
				Dictionary<string, IToken> orCreateDict = this.pageDictionary.GetOrCreateDict(NameToken.Resources).GetOrCreateDict(NameToken.Font);
				while (orCreateDict.ContainsKey(nameToken))
				{
					defaultInterpolatedStringHandler..ctor(1, 1);
					defaultInterpolatedStringHandler.AppendLiteral("F");
					num = this.nextFontId;
					this.nextFontId = num + 1;
					defaultInterpolatedStringHandler.AppendFormatted<int>(num);
					nameToken = NameToken.Create(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.documentFonts[font.Id] = nameToken;
				orCreateDict[nameToken] = font.Reference;
			}
			return nameToken;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00006720 File Offset: 0x00004920
		public PdfPageBuilder.AddedImage AddJpeg(byte[] fileBytes, PdfRectangle placementRectangle)
		{
			PdfPageBuilder.AddedImage result;
			using (MemoryStream memoryStream = new MemoryStream(fileBytes))
			{
				result = this.AddJpeg(memoryStream, placementRectangle);
			}
			return result;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000675C File Offset: 0x0000495C
		public PdfPageBuilder.AddedImage AddJpeg(Stream fileStream, PdfRectangle placementRectangle = default(PdfRectangle))
		{
			long position = fileStream.Position;
			JpegInformation information = JpegHandler.GetInformation(fileStream);
			if (placementRectangle.Equals(default(PdfRectangle)))
			{
				placementRectangle..ctor(0.0, 0.0, (double)information.Width, (double)information.Height);
			}
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				fileStream.Seek(position, 0);
				fileStream.CopyTo(memoryStream);
				array = memoryStream.ToArray();
			}
			Dictionary<NameToken, IToken> dictionary = new Dictionary<NameToken, IToken>();
			dictionary.Add(NameToken.Type, NameToken.Xobject);
			dictionary.Add(NameToken.Subtype, NameToken.Image);
			dictionary.Add(NameToken.Width, new NumericToken(information.Width));
			dictionary.Add(NameToken.Height, new NumericToken(information.Height));
			dictionary.Add(NameToken.BitsPerComponent, new NumericToken(information.BitsPerComponent));
			dictionary.Add(NameToken.ColorSpace, NameToken.Devicergb);
			dictionary.Add(NameToken.Filter, NameToken.DctDecode);
			dictionary.Add(NameToken.Length, new NumericToken(array.Length));
			Dictionary<NameToken, IToken> dictionary2 = dictionary;
			IndirectReferenceToken indirectReferenceToken = this.documentBuilder.AddImage(new DictionaryToken(dictionary2), array);
			Dictionary<string, IToken> orCreateDict = this.pageDictionary.GetOrCreateDict(NameToken.Resources).GetOrCreateDict(NameToken.Xobject);
			NameToken nameToken = NameToken.Create(this.xobjectsNames.NewName(null));
			orCreateDict[nameToken] = indirectReferenceToken;
			this.currentStream.Add(Push.Value);
			this.currentStream.Add(new ModifyCurrentTransformationMatrix(new decimal[]
			{
				(decimal)placementRectangle.Width,
				default(decimal),
				default(decimal),
				(decimal)placementRectangle.Height,
				(decimal)placementRectangle.BottomLeft.X,
				(decimal)placementRectangle.BottomLeft.Y
			}));
			this.currentStream.Add(new InvokeNamedXObject(nameToken));
			this.currentStream.Add(Pop.Value);
			return new PdfPageBuilder.AddedImage(indirectReferenceToken.Data, information.Width, information.Height);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000069BC File Offset: 0x00004BBC
		public void AddJpeg(PdfPageBuilder.AddedImage image, PdfRectangle placementRectangle)
		{
			this.AddImage(image, placementRectangle);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000069C8 File Offset: 0x00004BC8
		public void AddImage(PdfPageBuilder.AddedImage image, PdfRectangle placementRectangle)
		{
			Dictionary<string, IToken> orCreateDict = this.pageDictionary.GetOrCreateDict(NameToken.Resources).GetOrCreateDict(NameToken.Xobject);
			NameToken nameToken = NameToken.Create(this.xobjectsNames.NewName(null));
			orCreateDict[nameToken] = new IndirectReferenceToken(image.Reference);
			this.currentStream.Add(Push.Value);
			this.currentStream.Add(new ModifyCurrentTransformationMatrix(new decimal[]
			{
				(decimal)placementRectangle.Width,
				default(decimal),
				default(decimal),
				(decimal)placementRectangle.Height,
				(decimal)placementRectangle.BottomLeft.X,
				(decimal)placementRectangle.BottomLeft.Y
			}));
			this.currentStream.Add(new InvokeNamedXObject(nameToken));
			this.currentStream.Add(Pop.Value);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00006AC0 File Offset: 0x00004CC0
		public PdfPageBuilder.AddedImage AddPng(byte[] pngBytes, PdfRectangle placementRectangle)
		{
			PdfPageBuilder.AddedImage result;
			using (MemoryStream memoryStream = new MemoryStream(pngBytes))
			{
				result = this.AddPng(memoryStream, placementRectangle);
			}
			return result;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00006AFC File Offset: 0x00004CFC
		public PdfPageBuilder.AddedImage AddPng(Stream pngStream, PdfRectangle placementRectangle = default(PdfRectangle))
		{
			Png png = Png.Open(pngStream, null);
			if (placementRectangle.Equals(default(PdfRectangle)))
			{
				placementRectangle..ctor(0.0, 0.0, (double)png.Width, (double)png.Height);
			}
			byte[] array = new byte[3];
			byte[] array2;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				for (int i = 0; i < png.Height; i++)
				{
					for (int j = 0; j < png.Width; j++)
					{
						Pixel pixel = png.GetPixel(j, i);
						array[0] = pixel.R;
						array[1] = pixel.G;
						array[2] = pixel.B;
						memoryStream.Write(array, 0, array.Length);
					}
				}
				array2 = memoryStream.ToArray();
			}
			NumericToken numericToken = new NumericToken(png.Width);
			NumericToken numericToken2 = new NumericToken(png.Height);
			IndirectReferenceToken indirectReferenceToken = null;
			if (png.HasAlphaChannel && this.documentBuilder.ArchiveStandard != PdfAStandard.A1B && this.documentBuilder.ArchiveStandard != PdfAStandard.A1A)
			{
				byte[] array3 = new byte[array2.Length / 3];
				for (int k = 0; k < png.Height; k++)
				{
					for (int l = 0; l < png.Width; l++)
					{
						Pixel pixel2 = png.GetPixel(l, k);
						int num = k * png.Width + l;
						array3[num] = pixel2.A;
					}
				}
				byte[] array4 = DataCompresser.CompressBytes(array3);
				Dictionary<NameToken, IToken> dictionary = new Dictionary<NameToken, IToken>();
				dictionary.Add(NameToken.Type, NameToken.Xobject);
				dictionary.Add(NameToken.Subtype, NameToken.Image);
				dictionary.Add(NameToken.Width, numericToken);
				dictionary.Add(NameToken.Height, numericToken2);
				dictionary.Add(NameToken.ColorSpace, NameToken.Devicegray);
				dictionary.Add(NameToken.BitsPerComponent, new NumericToken(8m));
				dictionary.Add(NameToken.Decode, new ArrayToken(new IToken[]
				{
					new NumericToken(0m),
					new NumericToken(1m)
				}));
				dictionary.Add(NameToken.Length, new NumericToken(array4.Length));
				dictionary.Add(NameToken.Filter, NameToken.FlateDecode);
				Dictionary<NameToken, IToken> dictionary2 = dictionary;
				indirectReferenceToken = this.documentBuilder.AddImage(new DictionaryToken(dictionary2), array4);
			}
			byte[] array5 = DataCompresser.CompressBytes(array2);
			Dictionary<NameToken, IToken> dictionary3 = new Dictionary<NameToken, IToken>();
			dictionary3.Add(NameToken.Type, NameToken.Xobject);
			dictionary3.Add(NameToken.Subtype, NameToken.Image);
			dictionary3.Add(NameToken.Width, numericToken);
			dictionary3.Add(NameToken.Height, numericToken2);
			dictionary3.Add(NameToken.BitsPerComponent, new NumericToken(8m));
			dictionary3.Add(NameToken.ColorSpace, NameToken.Devicergb);
			dictionary3.Add(NameToken.Filter, NameToken.FlateDecode);
			dictionary3.Add(NameToken.Length, new NumericToken(array5.Length));
			Dictionary<NameToken, IToken> dictionary4 = dictionary3;
			if (indirectReferenceToken != null)
			{
				dictionary4.Add(NameToken.Smask, indirectReferenceToken);
			}
			IndirectReferenceToken indirectReferenceToken2 = this.documentBuilder.AddImage(new DictionaryToken(dictionary4), array5);
			Dictionary<string, IToken> orCreateDict = this.pageDictionary.GetOrCreateDict(NameToken.Resources).GetOrCreateDict(NameToken.Xobject);
			NameToken nameToken = NameToken.Create(this.xobjectsNames.NewName(null));
			orCreateDict[nameToken] = indirectReferenceToken2;
			this.currentStream.Add(Push.Value);
			this.currentStream.Add(new ModifyCurrentTransformationMatrix(new decimal[]
			{
				(decimal)placementRectangle.Width,
				default(decimal),
				default(decimal),
				(decimal)placementRectangle.Height,
				(decimal)placementRectangle.BottomLeft.X,
				(decimal)placementRectangle.BottomLeft.Y
			}));
			this.currentStream.Add(new InvokeNamedXObject(nameToken));
			this.currentStream.Add(Pop.Value);
			return new PdfPageBuilder.AddedImage(indirectReferenceToken2.Data, png.Width, png.Height);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00006F44 File Offset: 0x00005144
		public PdfPageBuilder CopyFrom(Page srcPage)
		{
			if (this.currentStream.Operations.Count > 0)
			{
				this.NewContentStreamAfter();
			}
			PdfPageBuilder.IPageContentStream pageContentStream = this.currentStream;
			DictionaryToken dictionaryToken;
			if (!srcPage.Dictionary.TryGet(NameToken.Resources, srcPage.pdfScanner, out dictionaryToken))
			{
				pageContentStream.Operations.AddRange(srcPage.Operations);
				return this;
			}
			List<IGraphicsStateOperation> list = new List<IGraphicsStateOperation>(srcPage.Operations);
			Dictionary<string, IToken> orCreateDict = this.pageDictionary.GetOrCreateDict(NameToken.Resources);
			foreach (KeyValuePair<string, IToken> keyValuePair in dictionaryToken.Data)
			{
				NameToken nameToken = NameToken.Create(keyValuePair.Key);
				if (!(nameToken == NameToken.Font) && !(nameToken == NameToken.Xobject) && !orCreateDict.ContainsKey(nameToken))
				{
					orCreateDict[nameToken] = this.documentBuilder.CopyToken(srcPage.pdfScanner, keyValuePair.Value);
				}
			}
			DictionaryToken dictionaryToken2;
			if (dictionaryToken.TryGet(NameToken.Font, srcPage.pdfScanner, out dictionaryToken2))
			{
				Dictionary<string, IToken> orCreateDict2 = orCreateDict.GetOrCreateDict(NameToken.Font);
				foreach (KeyValuePair<string, IToken> keyValuePair2 in dictionaryToken2.Data)
				{
					PdfPageBuilder.<>c__DisplayClass49_0 CS$<>8__locals1 = new PdfPageBuilder.<>c__DisplayClass49_0();
					CS$<>8__locals1.fontName = NameToken.Create(keyValuePair2.Key);
					if (orCreateDict2.ContainsKey(CS$<>8__locals1.fontName))
					{
						PdfPageBuilder.<>c__DisplayClass49_0 CS$<>8__locals2 = CS$<>8__locals1;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
						defaultInterpolatedStringHandler.AppendLiteral("F");
						int num = this.nextFontId;
						this.nextFontId = num + 1;
						defaultInterpolatedStringHandler.AppendFormatted<int>(num);
						CS$<>8__locals2.newName = NameToken.Create(defaultInterpolatedStringHandler.ToStringAndClear());
						while (orCreateDict2.ContainsKey(CS$<>8__locals1.newName))
						{
							PdfPageBuilder.<>c__DisplayClass49_0 CS$<>8__locals3 = CS$<>8__locals1;
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
							defaultInterpolatedStringHandler.AppendLiteral("F");
							num = this.nextFontId;
							this.nextFontId = num + 1;
							defaultInterpolatedStringHandler.AppendFormatted<int>(num);
							CS$<>8__locals3.newName = NameToken.Create(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						list = Enumerable.ToList<IGraphicsStateOperation>(Enumerable.Select<IGraphicsStateOperation, IGraphicsStateOperation>(list, delegate(IGraphicsStateOperation op)
						{
							SetFontAndSize setFontAndSize = op as SetFontAndSize;
							if (setFontAndSize == null)
							{
								return op;
							}
							if (setFontAndSize.Font.Data == CS$<>8__locals1.fontName)
							{
								return new SetFontAndSize(CS$<>8__locals1.newName, setFontAndSize.Size);
							}
							return op;
						}));
						CS$<>8__locals1.fontName = CS$<>8__locals1.newName;
					}
					IndirectReferenceToken indirectReferenceToken = keyValuePair2.Value as IndirectReferenceToken;
					if (indirectReferenceToken == null)
					{
						throw new PdfDocumentFormatException("Expected a IndirectReferenceToken for the font, got a " + keyValuePair2.Value.GetType().Name);
					}
					orCreateDict2.Add(CS$<>8__locals1.fontName, this.documentBuilder.CopyToken(srcPage.pdfScanner, indirectReferenceToken));
				}
			}
			DictionaryToken dictionaryToken3;
			if (dictionaryToken.TryGet(NameToken.Xobject, srcPage.pdfScanner, out dictionaryToken3))
			{
				Dictionary<string, IToken> orCreateDict3 = orCreateDict.GetOrCreateDict(NameToken.Xobject);
				foreach (KeyValuePair<string, IToken> keyValuePair3 in dictionaryToken3.Data)
				{
					string xobjectName = keyValuePair3.Key;
					string newName = this.xobjectsNames.FixName(xobjectName);
					if (xobjectName != newName)
					{
						list = Enumerable.ToList<IGraphicsStateOperation>(Enumerable.Select<IGraphicsStateOperation, IGraphicsStateOperation>(list, delegate(IGraphicsStateOperation op)
						{
							InvokeNamedXObject invokeNamedXObject = op as InvokeNamedXObject;
							if (invokeNamedXObject == null)
							{
								return op;
							}
							if (invokeNamedXObject.Name.Data == xobjectName)
							{
								return new InvokeNamedXObject(NameToken.Create(newName));
							}
							return op;
						}));
					}
					xobjectName = newName;
					IndirectReferenceToken indirectReferenceToken2 = keyValuePair3.Value as IndirectReferenceToken;
					if (indirectReferenceToken2 == null)
					{
						throw new PdfDocumentFormatException("Expected a IndirectReferenceToken for the XObject, got a " + keyValuePair3.Value.GetType().Name);
					}
					orCreateDict3[xobjectName] = this.documentBuilder.CopyToken(srcPage.pdfScanner, indirectReferenceToken2);
				}
			}
			DictionaryToken dictionaryToken4;
			if (dictionaryToken.TryGet(NameToken.ExtGState, srcPage.pdfScanner, out dictionaryToken4))
			{
				Dictionary<string, IToken> orCreateDict4 = orCreateDict.GetOrCreateDict(NameToken.ExtGState);
				foreach (KeyValuePair<string, IToken> keyValuePair4 in dictionaryToken4.Data)
				{
					string gstateName = keyValuePair4.Key;
					string newName = this.gStateNames.FixName(gstateName);
					if (newName != gstateName)
					{
						list = Enumerable.ToList<IGraphicsStateOperation>(Enumerable.Select<IGraphicsStateOperation, IGraphicsStateOperation>(list, delegate(IGraphicsStateOperation op)
						{
							SetGraphicsStateParametersFromDictionary setGraphicsStateParametersFromDictionary = op as SetGraphicsStateParametersFromDictionary;
							if (setGraphicsStateParametersFromDictionary == null)
							{
								return op;
							}
							if (setGraphicsStateParametersFromDictionary.Name.Data == gstateName)
							{
								return new SetGraphicsStateParametersFromDictionary(NameToken.Create(newName));
							}
							return op;
						}));
						gstateName = newName;
					}
					IndirectReferenceToken indirectReferenceToken3 = keyValuePair4.Value as IndirectReferenceToken;
					if (indirectReferenceToken3 == null)
					{
						throw new PdfDocumentFormatException("Expected a IndirectReferenceToken for the XObject, got a " + keyValuePair4.Value.GetType().Name);
					}
					orCreateDict4[gstateName] = this.documentBuilder.CopyToken(srcPage.pdfScanner, indirectReferenceToken3);
				}
			}
			pageContentStream.Operations.AddRange(list);
			return this;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000074A4 File Offset: 0x000056A4
		private List<Letter> DrawLetters(NameToken name, string text, IWritingFont font, TransformationMatrix fontMatrix, decimal fontSize, TransformationMatrix textMatrix)
		{
			int num = 1;
			int num2 = 0;
			List<Letter> list = new List<Letter>();
			TransformationMatrix transformationMatrix = TransformationMatrix.FromValues((double)fontSize * (double)num, 0.0, 0.0, (double)fontSize, 0.0, (double)num2);
			double num3 = 0.0;
			this.textSequence++;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text.get_Chars(i);
				PdfRectangle pdfRectangle;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (!font.TryGetBoundingBox(c, out pdfRectangle))
				{
					defaultInterpolatedStringHandler..ctor(47, 2);
					defaultInterpolatedStringHandler.AppendLiteral("The font does not contain a character: '");
					defaultInterpolatedStringHandler.AppendFormatted<char>(c);
					defaultInterpolatedStringHandler.AppendLiteral("' (0x");
					defaultInterpolatedStringHandler.AppendFormatted<int>((int)c, "X");
					defaultInterpolatedStringHandler.AppendLiteral(").");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				double num4;
				if (!font.TryGetAdvanceWidth(c, out num4))
				{
					defaultInterpolatedStringHandler..ctor(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("The font does not contain a character: ");
					defaultInterpolatedStringHandler.AppendFormatted<char>(c);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				PdfRectangle pdfRectangle2;
				pdfRectangle2..ctor(0.0, 0.0, num4, 0.0);
				pdfRectangle2 = textMatrix.Transform(transformationMatrix.Transform(fontMatrix.Transform(pdfRectangle2)));
				PdfRectangle glyphRectangle = textMatrix.Transform(transformationMatrix.Transform(fontMatrix.Transform(pdfRectangle)));
				Letter letter = new Letter(c.ToString(), glyphRectangle, pdfRectangle2.BottomLeft, pdfRectangle2.BottomRight, num3, (double)fontSize, FontDetails.GetDefault(name), 0, GrayColor.Black, GrayColor.Black, (double)fontSize, this.textSequence);
				list.Add(letter);
				double num5 = pdfRectangle2.Width * (double)num;
				int num6 = 0;
				TransformationMatrix translationMatrix = TransformationMatrix.GetTranslationMatrix(num5, (double)num6);
				num3 += num5;
				textMatrix = translationMatrix.Multiply(textMatrix);
			}
			return list;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000076AC File Offset: 0x000058AC
		private static decimal RgbToDecimal(byte value)
		{
			decimal num = Math.Max(0m, value / 255m);
			return Math.Round(Math.Min(1m, num), 4);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x000076EC File Offset: 0x000058EC
		private static decimal CheckRgbDecimal(decimal value, string argument)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (value < 0m)
			{
				defaultInterpolatedStringHandler..ctor(52, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Provided decimal for RGB color was less than zero: ");
				defaultInterpolatedStringHandler.AppendFormatted<decimal>(value);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentOutOfRangeException(argument, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (value > 1m)
			{
				defaultInterpolatedStringHandler..ctor(54, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Provided decimal for RGB color was greater than one: ");
				defaultInterpolatedStringHandler.AppendFormatted<decimal>(value);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentOutOfRangeException(argument, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return value;
		}

		// Token: 0x04000052 RID: 82
		private readonly PdfDocumentBuilder documentBuilder;

		// Token: 0x04000053 RID: 83
		internal readonly Dictionary<NameToken, IToken> pageDictionary = new Dictionary<NameToken, IToken>();

		// Token: 0x04000054 RID: 84
		internal readonly List<PdfPageBuilder.IPageContentStream> contentStreams;

		// Token: 0x04000055 RID: 85
		private PdfPageBuilder.IPageContentStream currentStream;

		// Token: 0x04000056 RID: 86
		[TupleElementNames(new string[]
		{
			"token",
			"action"
		})]
		internal readonly List<ValueTuple<DictionaryToken, PdfAction>> links;

		// Token: 0x04000057 RID: 87
		private readonly Dictionary<Guid, NameToken> documentFonts = new Dictionary<Guid, NameToken>();

		// Token: 0x04000058 RID: 88
		internal int nextFontId = 1;

		// Token: 0x04000059 RID: 89
		private int textSequence;

		// Token: 0x0400005A RID: 90
		private NameConflictSolver xobjectsNames = new NameConflictSolver("I");

		// Token: 0x0400005B RID: 91
		private NameConflictSolver gStateNames = new NameConflictSolver("GS");

		// Token: 0x0400005C RID: 92
		internal int? rotation;

		// Token: 0x02000234 RID: 564
		public interface IContentStream
		{
			// Token: 0x170003DD RID: 989
			// (get) Token: 0x06000D0C RID: 3340
			List<IGraphicsStateOperation> Operations { get; }
		}

		// Token: 0x02000235 RID: 565
		internal interface IPageContentStream : PdfPageBuilder.IContentStream
		{
			// Token: 0x170003DE RID: 990
			// (get) Token: 0x06000D0D RID: 3341
			bool ReadOnly { get; }

			// Token: 0x170003DF RID: 991
			// (get) Token: 0x06000D0E RID: 3342
			bool HasContent { get; }

			// Token: 0x06000D0F RID: 3343
			void Add(IGraphicsStateOperation operation);

			// Token: 0x06000D10 RID: 3344
			IndirectReferenceToken Write(IPdfStreamWriter writer);
		}

		// Token: 0x02000236 RID: 566
		internal class DefaultContentStream : PdfPageBuilder.IPageContentStream, PdfPageBuilder.IContentStream
		{
			// Token: 0x06000D11 RID: 3345 RVA: 0x0003FFD5 File Offset: 0x0003E1D5
			public DefaultContentStream() : this(new List<IGraphicsStateOperation>())
			{
			}

			// Token: 0x06000D12 RID: 3346 RVA: 0x0003FFE2 File Offset: 0x0003E1E2
			public DefaultContentStream(List<IGraphicsStateOperation> operations)
			{
				this.operations = operations;
			}

			// Token: 0x170003E0 RID: 992
			// (get) Token: 0x06000D13 RID: 3347 RVA: 0x0003FFF1 File Offset: 0x0003E1F1
			public bool ReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x170003E1 RID: 993
			// (get) Token: 0x06000D14 RID: 3348 RVA: 0x0003FFF4 File Offset: 0x0003E1F4
			public bool HasContent
			{
				get
				{
					return Enumerable.Any<IGraphicsStateOperation>(this.operations);
				}
			}

			// Token: 0x06000D15 RID: 3349 RVA: 0x00040001 File Offset: 0x0003E201
			public void Add(IGraphicsStateOperation operation)
			{
				this.operations.Add(operation);
			}

			// Token: 0x170003E2 RID: 994
			// (get) Token: 0x06000D16 RID: 3350 RVA: 0x0004000F File Offset: 0x0003E20F
			public List<IGraphicsStateOperation> Operations
			{
				get
				{
					return this.operations;
				}
			}

			// Token: 0x06000D17 RID: 3351 RVA: 0x00040018 File Offset: 0x0003E218
			public IndirectReferenceToken Write(IPdfStreamWriter writer)
			{
				IndirectReferenceToken result;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					foreach (IGraphicsStateOperation graphicsStateOperation in this.operations)
					{
						graphicsStateOperation.Write(memoryStream);
					}
					StreamToken token = DataCompresser.CompressToStream(memoryStream.ToArray());
					result = writer.WriteToken(token);
				}
				return result;
			}

			// Token: 0x04000792 RID: 1938
			private readonly List<IGraphicsStateOperation> operations;
		}

		// Token: 0x02000237 RID: 567
		internal class CopiedContentStream : PdfPageBuilder.IPageContentStream, PdfPageBuilder.IContentStream
		{
			// Token: 0x170003E3 RID: 995
			// (get) Token: 0x06000D18 RID: 3352 RVA: 0x0004009C File Offset: 0x0003E29C
			public bool ReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x170003E4 RID: 996
			// (get) Token: 0x06000D19 RID: 3353 RVA: 0x0004009F File Offset: 0x0003E29F
			public bool HasContent
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06000D1A RID: 3354 RVA: 0x000400A2 File Offset: 0x0003E2A2
			public CopiedContentStream(IndirectReferenceToken indirectReferenceToken)
			{
				this.token = indirectReferenceToken;
			}

			// Token: 0x06000D1B RID: 3355 RVA: 0x000400B1 File Offset: 0x0003E2B1
			public IndirectReferenceToken Write(IPdfStreamWriter writer)
			{
				return this.token;
			}

			// Token: 0x06000D1C RID: 3356 RVA: 0x000400B9 File Offset: 0x0003E2B9
			public void Add(IGraphicsStateOperation operation)
			{
				throw new NotSupportedException("Writing to a copied content stream is not supported.");
			}

			// Token: 0x170003E5 RID: 997
			// (get) Token: 0x06000D1D RID: 3357 RVA: 0x000400C5 File Offset: 0x0003E2C5
			public List<IGraphicsStateOperation> Operations
			{
				get
				{
					throw new NotSupportedException("Reading raw operations is not supported from a copied content stream.");
				}
			}

			// Token: 0x04000793 RID: 1939
			private readonly IndirectReferenceToken token;
		}

		// Token: 0x02000238 RID: 568
		public class AddedImage
		{
			// Token: 0x170003E6 RID: 998
			// (get) Token: 0x06000D1E RID: 3358 RVA: 0x000400D1 File Offset: 0x0003E2D1
			internal Guid Id { get; }

			// Token: 0x170003E7 RID: 999
			// (get) Token: 0x06000D1F RID: 3359 RVA: 0x000400D9 File Offset: 0x0003E2D9
			internal IndirectReference Reference { get; }

			// Token: 0x170003E8 RID: 1000
			// (get) Token: 0x06000D20 RID: 3360 RVA: 0x000400E1 File Offset: 0x0003E2E1
			public int Width { get; }

			// Token: 0x170003E9 RID: 1001
			// (get) Token: 0x06000D21 RID: 3361 RVA: 0x000400E9 File Offset: 0x0003E2E9
			public int Height { get; }

			// Token: 0x06000D22 RID: 3362 RVA: 0x000400F1 File Offset: 0x0003E2F1
			internal AddedImage(IndirectReference reference, int width, int height)
			{
				this.Id = Guid.NewGuid();
				this.Reference = reference;
				this.Width = width;
				this.Height = height;
			}
		}
	}
}
