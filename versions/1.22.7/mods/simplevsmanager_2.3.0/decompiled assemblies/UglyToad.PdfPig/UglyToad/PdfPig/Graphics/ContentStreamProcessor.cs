using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Geometry;
using UglyToad.PdfPig.Graphics.Colors;
using UglyToad.PdfPig.Graphics.Core;
using UglyToad.PdfPig.Graphics.Operations;
using UglyToad.PdfPig.Graphics.Operations.TextPositioning;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Parser;
using UglyToad.PdfPig.PdfFonts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;
using UglyToad.PdfPig.XObjects;

namespace UglyToad.PdfPig.Graphics
{
	// Token: 0x02000104 RID: 260
	internal class ContentStreamProcessor : IOperationContext
	{
		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x0001DC31 File Offset: 0x0001BE31
		public TextMatrices TextMatrices { get; } = new TextMatrices();

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060004FF RID: 1279 RVA: 0x0001DC39 File Offset: 0x0001BE39
		public TransformationMatrix CurrentTransformationMatrix
		{
			get
			{
				return this.GetCurrentState().CurrentTransformationMatrix;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x0001DC46 File Offset: 0x0001BE46
		// (set) Token: 0x06000501 RID: 1281 RVA: 0x0001DC4E File Offset: 0x0001BE4E
		public PdfSubpath CurrentSubpath { get; private set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x0001DC57 File Offset: 0x0001BE57
		// (set) Token: 0x06000503 RID: 1283 RVA: 0x0001DC5F File Offset: 0x0001BE5F
		public PdfPath CurrentPath { get; private set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x0001DC68 File Offset: 0x0001BE68
		// (set) Token: 0x06000505 RID: 1285 RVA: 0x0001DC70 File Offset: 0x0001BE70
		public PdfPoint CurrentPosition { get; set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x0001DC79 File Offset: 0x0001BE79
		public int StackSize
		{
			get
			{
				return this.graphicsStack.Count;
			}
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0001DC88 File Offset: 0x0001BE88
		public ContentStreamProcessor(int pageNumber, IResourceStore resourceStore, UserSpaceUnit userSpaceUnit, MediaBox mediaBox, CropBox cropBox, PageRotationDegrees rotation, IPdfTokenScanner pdfScanner, IPageContentParser pageContentParser, ILookupFilterProvider filterProvider, InternalParsingOptions parsingOptions)
		{
			Dictionary<XObjectType, List<XObjectContentRecord>> dictionary = new Dictionary<XObjectType, List<XObjectContentRecord>>();
			dictionary.Add(XObjectType.Image, new List<XObjectContentRecord>());
			dictionary.Add(XObjectType.PostScript, new List<XObjectContentRecord>());
			this.xObjects = dictionary;
			base..ctor();
			this.pageNumber = pageNumber;
			this.resourceStore = resourceStore;
			this.userSpaceUnit = userSpaceUnit;
			this.rotation = rotation;
			if (pdfScanner == null)
			{
				throw new ArgumentNullException("pdfScanner");
			}
			this.pdfScanner = pdfScanner;
			if (pageContentParser == null)
			{
				throw new ArgumentNullException("pageContentParser");
			}
			this.pageContentParser = pageContentParser;
			if (filterProvider == null)
			{
				throw new ArgumentNullException("filterProvider");
			}
			this.filterProvider = filterProvider;
			this.parsingOptions = parsingOptions;
			PdfSubpath pdfSubpath = new PdfSubpath();
			pdfSubpath.Rectangle(cropBox.Bounds.BottomLeft.X, cropBox.Bounds.BottomLeft.Y, cropBox.Bounds.Width, cropBox.Bounds.Height);
			PdfPath pdfPath = new PdfPath();
			pdfPath.Add(pdfSubpath);
			PdfPath pdfPath2 = pdfPath;
			pdfPath2.SetClipping(1);
			this.graphicsStack.Push(new CurrentGraphicsState
			{
				CurrentTransformationMatrix = ContentStreamProcessor.GetInitialMatrix(userSpaceUnit, mediaBox, cropBox, rotation, parsingOptions.Logger),
				CurrentClippingPath = pdfPath2,
				ColorSpaceContext = new ColorSpaceContext(new Func<CurrentGraphicsState>(this.GetCurrentState), resourceStore)
			});
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0001DE2C File Offset: 0x0001C02C
		internal static TransformationMatrix GetInitialMatrix(UserSpaceUnit userSpaceUnit, MediaBox mediaBox, CropBox cropBox, PageRotationDegrees rotation, ILog log)
		{
			PdfRectangle pdfRectangle = mediaBox.Bounds.Intersect(cropBox.Bounds) ?? cropBox.Bounds;
			if (rotation.Value == 0 && pdfRectangle.Left == 0.0 && pdfRectangle.Bottom == 0.0 && userSpaceUnit.PointMultiples == 1)
			{
				return TransformationMatrix.Identity;
			}
			TransformationMatrix translationMatrix = TransformationMatrix.GetTranslationMatrix(-pdfRectangle.Left, -pdfRectangle.Bottom);
			if (userSpaceUnit.PointMultiples != 1)
			{
				log.Warn("User space unit other than 1 is not implemented");
			}
			int value = rotation.Value;
			double num;
			double num2;
			if (value <= 90)
			{
				if (value == 0)
				{
					return translationMatrix;
				}
				if (value == 90)
				{
					num = 0.0;
					num2 = pdfRectangle.Width;
					goto IL_13D;
				}
			}
			else
			{
				if (value == 180)
				{
					num = pdfRectangle.Width;
					num2 = pdfRectangle.Height;
					goto IL_13D;
				}
				if (value == 270)
				{
					num = pdfRectangle.Height;
					num2 = 0.0;
					goto IL_13D;
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(34, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Invalid value for page rotation: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(rotation.Value);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			IL_13D:
			TransformationMatrix rotationMatrix = TransformationMatrix.GetRotationMatrix((double)(-(double)rotation.Value));
			TransformationMatrix translationMatrix2 = TransformationMatrix.GetTranslationMatrix(num, num2);
			return translationMatrix.Multiply(rotationMatrix.Multiply(translationMatrix2));
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0001DFA0 File Offset: 0x0001C1A0
		public PageContent Process(int pageNumberCurrent, IReadOnlyList<IGraphicsStateOperation> operations)
		{
			this.pageNumber = pageNumberCurrent;
			this.CloneAllStates();
			this.ProcessOperations(operations);
			return new PageContent(operations, this.letters, this.paths, this.images, this.markedContents, this.pdfScanner, this.filterProvider, this.resourceStore);
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0001DFF4 File Offset: 0x0001C1F4
		private void ProcessOperations(IReadOnlyList<IGraphicsStateOperation> operations)
		{
			foreach (IGraphicsStateOperation graphicsStateOperation in operations)
			{
				graphicsStateOperation.Run(this);
			}
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0001E03C File Offset: 0x0001C23C
		private Stack<CurrentGraphicsState> CloneAllStates()
		{
			Stack<CurrentGraphicsState> stack = this.graphicsStack;
			this.graphicsStack = new Stack<CurrentGraphicsState>();
			this.graphicsStack.Push(stack.Peek().DeepClone());
			return stack;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0001E072 File Offset: 0x0001C272
		[DebuggerStepThrough]
		public CurrentGraphicsState GetCurrentState()
		{
			return this.graphicsStack.Peek();
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0001E07F File Offset: 0x0001C27F
		public void PopState()
		{
			this.graphicsStack.Pop();
			this.activeExtendedGraphicsStateFont = null;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0001E094 File Offset: 0x0001C294
		public void PushState()
		{
			this.graphicsStack.Push(this.graphicsStack.Peek().DeepClone());
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0001E0B4 File Offset: 0x0001C2B4
		public void ShowText(IInputBytes bytes)
		{
			CurrentGraphicsState currentState = this.GetCurrentState();
			IFont font = currentState.FontState.FromExtendedGraphicsState ? this.activeExtendedGraphicsStateFont : this.resourceStore.GetFont(currentState.FontState.FontName);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (font != null)
			{
				double fontSize = currentState.FontState.FontSize;
				double num = currentState.FontState.HorizontalScaling / 100.0;
				double characterSpacing = currentState.FontState.CharacterSpacing;
				double rise = currentState.FontState.Rise;
				TransformationMatrix currentTransformationMatrix = currentState.CurrentTransformationMatrix;
				TransformationMatrix first = TransformationMatrix.FromValues(fontSize * num, 0.0, 0.0, fontSize, 0.0, rise);
				double pointSize = Math.Round(currentTransformationMatrix.Multiply(this.TextMatrices.TextMatrix).Transform(new PdfRectangle(0.0, 0.0, 1.0, fontSize)).Height, 2);
				while (bytes.MoveNext())
				{
					int num3;
					int num2 = font.ReadCharacterCode(bytes, out num3);
					string text;
					if (!font.TryGetUnicode(num2, out text) || text == null)
					{
						ILog logger = this.parsingOptions.Logger;
						defaultInterpolatedStringHandler..ctor(66, 2);
						defaultInterpolatedStringHandler.AppendLiteral("We could not find the corresponding character with code ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
						defaultInterpolatedStringHandler.AppendLiteral(" in font ");
						defaultInterpolatedStringHandler.AppendFormatted<NameToken>(font.Name);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
						text = new string((char)num2, 1);
					}
					double num4 = 0.0;
					if (num2 == 32 && num3 == 1)
					{
						num4 += this.GetCurrentState().FontState.WordSpacing;
					}
					TransformationMatrix second = this.TextMatrices.TextMatrix;
					if (font.IsVertical)
					{
						IVerticalWritingSupported verticalWritingSupported = font as IVerticalWritingSupported;
						if (verticalWritingSupported == null)
						{
							defaultInterpolatedStringHandler..ctor(58, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Font ");
							defaultInterpolatedStringHandler.AppendFormatted<NameToken>(font.Name);
							defaultInterpolatedStringHandler.AppendLiteral(" was in vertical writing mode but did not implement ");
							defaultInterpolatedStringHandler.AppendFormatted("IVerticalWritingSupported");
							defaultInterpolatedStringHandler.AppendLiteral(".");
							throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						PdfVector positionVector = verticalWritingSupported.GetPositionVector(num2);
						second = second.Translate(positionVector.X, positionVector.Y);
					}
					CharacterBoundingBox boundingBox = font.GetBoundingBox(num2);
					PdfRectangle glyphRectangle = PerformantRectangleTransformer.Transform(first, second, currentTransformationMatrix, boundingBox.GlyphBounds);
					PdfRectangle pdfRectangle = PerformantRectangleTransformer.Transform(first, second, currentTransformationMatrix, new PdfRectangle(0.0, 0.0, boundingBox.Width, 0.0));
					Letter letter = null;
					if (Diacritics.IsInCombiningDiacriticRange(text) && bytes.CurrentOffset > 0L && this.letters.Count > 0)
					{
						Letter letter2 = this.letters[this.letters.Count - 1];
						string value;
						if (letter2.TextSequence == this.textSequence && Diacritics.TryCombineDiacriticWithPreviousLetter(text, letter2.Value, out value))
						{
							this.letters.Remove(letter2);
							letter = new Letter(value, letter2.GlyphRectangle, letter2.StartBaseLine, letter2.EndBaseLine, letter2.Width, letter2.FontSize, letter2.Font, letter2.RenderingMode, letter2.StrokeColor, letter2.FillColor, letter2.PointSize, letter2.TextSequence);
						}
					}
					if (letter == null)
					{
						letter = new Letter(text, glyphRectangle, pdfRectangle.BottomLeft, pdfRectangle.BottomRight, pdfRectangle.Width, fontSize, font.Details, currentState.FontState.TextRenderingMode, currentState.CurrentStrokingColor, currentState.CurrentNonStrokingColor, pointSize, this.textSequence);
					}
					this.letters.Add(letter);
					this.markedContentStack.AddLetter(letter);
					double num5;
					double num6;
					if (font.IsVertical)
					{
						PdfVector displacementVector = ((IVerticalWritingSupported)font).GetDisplacementVector(num2);
						num5 = 0.0;
						num6 = displacementVector.Y * fontSize + characterSpacing + num4;
					}
					else
					{
						num5 = (boundingBox.Width * fontSize + characterSpacing + num4) * num;
						num6 = 0.0;
					}
					this.TextMatrices.TextMatrix = this.TextMatrices.TextMatrix.Translate(num5, num6);
				}
				return;
			}
			if (this.parsingOptions.SkipMissingFonts)
			{
				ILog logger2 = this.parsingOptions.Logger;
				defaultInterpolatedStringHandler..ctor(80, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Skipping a missing font with name ");
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(currentState.FontState.FontName);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendLiteral("since it is not present in the document and ");
				defaultInterpolatedStringHandler.AppendFormatted("SkipMissingFonts");
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear() + "is set to true. This may result in some text being skipped and not included in the output.");
				return;
			}
			defaultInterpolatedStringHandler..ctor(85, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Could not find the font with name ");
			defaultInterpolatedStringHandler.AppendFormatted<NameToken>(currentState.FontState.FontName);
			defaultInterpolatedStringHandler.AppendLiteral(" in the resource store. It has not been loaded yet.");
			throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0001E5C4 File Offset: 0x0001C7C4
		public void ShowPositionedText(IReadOnlyList<IToken> tokens)
		{
			this.textSequence++;
			CurrentGraphicsState currentState = this.GetCurrentState();
			CurrentFontState fontState = currentState.FontState;
			double fontSize = fontState.FontSize;
			double num = fontState.HorizontalScaling / 100.0;
			IFont font = this.resourceStore.GetFont(fontState.FontName);
			if (font != null)
			{
				bool isVertical = font.IsVertical;
				foreach (IToken token in tokens)
				{
					NumericToken numericToken = token as NumericToken;
					if (numericToken != null)
					{
						double num2 = (double)numericToken.Data;
						double tx;
						double ty;
						if (isVertical)
						{
							tx = 0.0;
							ty = -num2 / 1000.0 * fontSize;
						}
						else
						{
							tx = -num2 / 1000.0 * fontSize * num;
							ty = 0.0;
						}
						this.AdjustTextMatrix(tx, ty);
					}
					else
					{
						HexToken hexToken = token as HexToken;
						IReadOnlyList<byte> readOnlyList;
						if (hexToken != null)
						{
							readOnlyList = hexToken.Bytes;
						}
						else
						{
							readOnlyList = OtherEncodings.StringAsLatin1Bytes(((StringToken)token).Data);
						}
						this.ShowText(new ByteArrayInputBytes(readOnlyList));
					}
				}
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (this.parsingOptions.SkipMissingFonts)
			{
				ILog logger = this.parsingOptions.Logger;
				defaultInterpolatedStringHandler..ctor(80, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Skipping a missing font with name ");
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(currentState.FontState.FontName);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendLiteral("since it is not present in the document and ");
				defaultInterpolatedStringHandler.AppendFormatted("SkipMissingFonts");
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear() + "is set to true. This may result in some text being skipped and not included in the output.");
				return;
			}
			defaultInterpolatedStringHandler..ctor(85, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Could not find the font with name ");
			defaultInterpolatedStringHandler.AppendFormatted<NameToken>(currentState.FontState.FontName);
			defaultInterpolatedStringHandler.AppendLiteral(" in the resource store. It has not been loaded yet.");
			throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0001E7D0 File Offset: 0x0001C9D0
		public void ApplyXObject(NameToken xObjectName)
		{
			StreamToken streamToken;
			if (!this.resourceStore.TryGetXObject(xObjectName, out streamToken))
			{
				if (this.parsingOptions.SkipMissingFonts)
				{
					return;
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(37, 2);
				defaultInterpolatedStringHandler.AppendLiteral("No XObject with name ");
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(xObjectName);
				defaultInterpolatedStringHandler.AppendLiteral(" found on page ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.pageNumber);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			else
			{
				NameToken nameToken = (NameToken)streamToken.StreamDictionary.Data[NameToken.Subtype.Data];
				CurrentGraphicsState currentState = this.GetCurrentState();
				TransformationMatrix currentTransformationMatrix = currentState.CurrentTransformationMatrix;
				if (nameToken.Equals(NameToken.Ps))
				{
					XObjectType type = XObjectType.PostScript;
					StreamToken stream = streamToken;
					TransformationMatrix appliedTransformation = currentTransformationMatrix;
					RenderingIntent renderingIntent = currentState.RenderingIntent;
					IColorSpaceContext colorSpaceContext = currentState.ColorSpaceContext;
					XObjectContentRecord xobjectContentRecord = new XObjectContentRecord(type, stream, appliedTransformation, renderingIntent, ((colorSpaceContext != null) ? colorSpaceContext.CurrentStrokingColorSpace : null) ?? DeviceRgbColorSpaceDetails.Instance);
					this.xObjects[XObjectType.PostScript].Add(xobjectContentRecord);
					return;
				}
				if (nameToken.Equals(NameToken.Image))
				{
					XObjectType type2 = XObjectType.Image;
					StreamToken stream2 = streamToken;
					TransformationMatrix appliedTransformation2 = currentTransformationMatrix;
					RenderingIntent renderingIntent2 = currentState.RenderingIntent;
					IColorSpaceContext colorSpaceContext2 = currentState.ColorSpaceContext;
					XObjectContentRecord xobjectContentRecord2 = new XObjectContentRecord(type2, stream2, appliedTransformation2, renderingIntent2, ((colorSpaceContext2 != null) ? colorSpaceContext2.CurrentStrokingColorSpace : null) ?? DeviceRgbColorSpaceDetails.Instance);
					this.images.Add(Union<XObjectContentRecord, InlineImage>.One(xobjectContentRecord2));
					this.markedContentStack.AddXObject(xobjectContentRecord2, this.pdfScanner, this.filterProvider, this.resourceStore);
					return;
				}
				if (nameToken.Equals(NameToken.Form))
				{
					this.ProcessFormXObject(streamToken);
					return;
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(47, 2);
				defaultInterpolatedStringHandler.AppendLiteral("XObject encountered with unexpected SubType ");
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(nameToken);
				defaultInterpolatedStringHandler.AppendLiteral(". ");
				defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(streamToken.StreamDictionary);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0001E994 File Offset: 0x0001CB94
		private void ProcessFormXObject(StreamToken formStream)
		{
			DictionaryToken resourceDictionary;
			bool flag = formStream.StreamDictionary.TryGet(NameToken.Resources, this.pdfScanner, out resourceDictionary);
			if (flag)
			{
				this.resourceStore.LoadResourceDictionary(resourceDictionary, this.parsingOptions);
			}
			this.PushState();
			CurrentGraphicsState currentState = this.GetCurrentState();
			DictionaryToken dictionary;
			if (formStream.StreamDictionary.TryGet(NameToken.Group, this.pdfScanner, out dictionary))
			{
				NameToken nameToken;
				if (!dictionary.TryGet(NameToken.S, this.pdfScanner, out nameToken) || nameToken != NameToken.Transparency)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(75, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid Transparency Group XObject, '");
					defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.S);
					defaultInterpolatedStringHandler.AppendLiteral("' token is not set or not equal to '");
					defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.Transparency);
					defaultInterpolatedStringHandler.AppendLiteral("'.");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				currentState.AlphaConstantNonStroking = 1.0m;
				currentState.AlphaConstantStroking = 1.0m;
				NameToken colorspace;
				ArrayToken arrayToken;
				if (dictionary.TryGet(NameToken.Cs, this.pdfScanner, out colorspace))
				{
					currentState.ColorSpaceContext.SetNonStrokingColorspace(colorspace, null);
				}
				else if (dictionary.TryGet(NameToken.Cs, this.pdfScanner, out arrayToken) && arrayToken.Length > 0)
				{
					NameToken nameToken2 = arrayToken.Data[0] as NameToken;
					if (nameToken2 == null)
					{
						throw new InvalidOperationException("Invalid color space in Transparency Group XObjects.");
					}
					currentState.ColorSpaceContext.SetNonStrokingColorspace(nameToken2, dictionary);
				}
				BooleanToken booleanToken;
				if (dictionary.TryGet(NameToken.I, this.pdfScanner, out booleanToken))
				{
					bool data = booleanToken.Data;
				}
				BooleanToken booleanToken2;
				if (dictionary.TryGet(NameToken.K, this.pdfScanner, out booleanToken2))
				{
					bool data2 = booleanToken2.Data;
				}
			}
			TransformationMatrix transformationMatrix = TransformationMatrix.Identity;
			ArrayToken arrayToken2;
			if (formStream.StreamDictionary.TryGet(NameToken.Matrix, this.pdfScanner, out arrayToken2))
			{
				transformationMatrix = TransformationMatrix.FromArray(Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(arrayToken2.Data), (NumericToken x) => x.Double)));
			}
			currentState.CurrentTransformationMatrix = transformationMatrix.Multiply(currentState.CurrentTransformationMatrix);
			IReadOnlyList<byte> readOnlyList = formStream.Decode(this.filterProvider, this.pdfScanner);
			IReadOnlyList<IGraphicsStateOperation> operations = this.pageContentParser.Parse(this.pageNumber, new ByteArrayInputBytes(readOnlyList), this.parsingOptions.Logger);
			this.ProcessOperations(operations);
			this.PopState();
			if (flag)
			{
				this.resourceStore.UnloadResourceDictionary();
			}
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0001EC03 File Offset: 0x0001CE03
		public void BeginSubpath()
		{
			if (this.CurrentPath == null)
			{
				this.CurrentPath = new PdfPath();
			}
			this.AddCurrentSubpath();
			this.CurrentSubpath = new PdfSubpath();
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0001EC2C File Offset: 0x0001CE2C
		public PdfPoint? CloseSubpath()
		{
			if (this.CurrentSubpath == null)
			{
				return default(PdfPoint?);
			}
			PdfSubpath.Move move = this.CurrentSubpath.Commands[0] as PdfSubpath.Move;
			if (move != null)
			{
				PdfPoint location = move.Location;
				this.CurrentSubpath.CloseSubpath();
				this.AddCurrentSubpath();
				return new PdfPoint?(location);
			}
			throw new ArgumentException("CloseSubpath(): first command not Move.");
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0001EC90 File Offset: 0x0001CE90
		public void AddCurrentSubpath()
		{
			if (this.CurrentSubpath == null)
			{
				return;
			}
			this.CurrentPath.Add(this.CurrentSubpath);
			this.CurrentSubpath = null;
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0001ECB3 File Offset: 0x0001CEB3
		public void StrokePath(bool close)
		{
			if (this.CurrentPath == null)
			{
				return;
			}
			this.CurrentPath.SetStroked();
			if (close)
			{
				PdfSubpath currentSubpath = this.CurrentSubpath;
				if (currentSubpath != null)
				{
					currentSubpath.CloseSubpath();
				}
			}
			this.ClosePath();
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0001ECE3 File Offset: 0x0001CEE3
		public void FillPath(FillingRule fillingRule, bool close)
		{
			if (this.CurrentPath == null)
			{
				return;
			}
			this.CurrentPath.SetFilled(fillingRule);
			if (close)
			{
				PdfSubpath currentSubpath = this.CurrentSubpath;
				if (currentSubpath != null)
				{
					currentSubpath.CloseSubpath();
				}
			}
			this.ClosePath();
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0001ED14 File Offset: 0x0001CF14
		public void FillStrokePath(FillingRule fillingRule, bool close)
		{
			if (this.CurrentPath == null)
			{
				return;
			}
			this.CurrentPath.SetFilled(fillingRule);
			this.CurrentPath.SetStroked();
			if (close)
			{
				PdfSubpath currentSubpath = this.CurrentSubpath;
				if (currentSubpath != null)
				{
					currentSubpath.CloseSubpath();
				}
			}
			this.ClosePath();
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0001ED50 File Offset: 0x0001CF50
		public void MoveTo(double x, double y)
		{
			this.BeginSubpath();
			PdfPoint currentPosition = this.CurrentTransformationMatrix.Transform(new PdfPoint(x, y));
			this.CurrentPosition = currentPosition;
			this.CurrentSubpath.MoveTo(currentPosition.X, currentPosition.Y);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0001ED9C File Offset: 0x0001CF9C
		public void BezierCurveTo(double x2, double y2, double x3, double y3)
		{
			if (this.CurrentSubpath == null)
			{
				return;
			}
			PdfPoint pdfPoint = this.CurrentTransformationMatrix.Transform(new PdfPoint(x2, y2));
			PdfPoint currentPosition = this.CurrentTransformationMatrix.Transform(new PdfPoint(x3, y3));
			this.CurrentSubpath.BezierCurveTo(this.CurrentPosition.X, this.CurrentPosition.Y, pdfPoint.X, pdfPoint.Y, currentPosition.X, currentPosition.Y);
			this.CurrentPosition = currentPosition;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0001EE2C File Offset: 0x0001D02C
		public void BezierCurveTo(double x1, double y1, double x2, double y2, double x3, double y3)
		{
			if (this.CurrentSubpath == null)
			{
				return;
			}
			PdfPoint pdfPoint = this.CurrentTransformationMatrix.Transform(new PdfPoint(x1, y1));
			PdfPoint pdfPoint2 = this.CurrentTransformationMatrix.Transform(new PdfPoint(x2, y2));
			PdfPoint currentPosition = this.CurrentTransformationMatrix.Transform(new PdfPoint(x3, y3));
			this.CurrentSubpath.BezierCurveTo(pdfPoint.X, pdfPoint.Y, pdfPoint2.X, pdfPoint2.Y, currentPosition.X, currentPosition.Y);
			this.CurrentPosition = currentPosition;
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0001EEC4 File Offset: 0x0001D0C4
		public void LineTo(double x, double y)
		{
			if (this.CurrentSubpath == null)
			{
				return;
			}
			PdfPoint currentPosition = this.CurrentTransformationMatrix.Transform(new PdfPoint(x, y));
			this.CurrentSubpath.LineTo(currentPosition.X, currentPosition.Y);
			this.CurrentPosition = currentPosition;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0001EF10 File Offset: 0x0001D110
		public void Rectangle(double x, double y, double width, double height)
		{
			this.BeginSubpath();
			PdfPoint pdfPoint = this.CurrentTransformationMatrix.Transform(new PdfPoint(x, y));
			PdfPoint pdfPoint2 = this.CurrentTransformationMatrix.Transform(new PdfPoint(x + width, y + height));
			this.CurrentSubpath.Rectangle(pdfPoint.X, pdfPoint.Y, pdfPoint2.X - pdfPoint.X, pdfPoint2.Y - pdfPoint.Y);
			this.AddCurrentSubpath();
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0001EF94 File Offset: 0x0001D194
		public void EndPath()
		{
			if (this.CurrentPath == null)
			{
				return;
			}
			this.AddCurrentSubpath();
			if (this.CurrentPath.IsClipping)
			{
				if (!this.parsingOptions.ClipPaths)
				{
					this.paths.Add(this.CurrentPath);
					this.markedContentStack.AddPath(this.CurrentPath);
				}
				this.CurrentPath = null;
				return;
			}
			this.paths.Add(this.CurrentPath);
			this.markedContentStack.AddPath(this.CurrentPath);
			this.CurrentPath = null;
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0001F020 File Offset: 0x0001D220
		public void ClosePath()
		{
			this.AddCurrentSubpath();
			if (this.CurrentPath.IsClipping)
			{
				this.EndPath();
				return;
			}
			CurrentGraphicsState currentState = this.GetCurrentState();
			if (this.CurrentPath.IsStroked)
			{
				this.CurrentPath.LineDashPattern = new LineDashPattern?(currentState.LineDashPattern);
				this.CurrentPath.StrokeColor = currentState.CurrentStrokingColor;
				this.CurrentPath.LineWidth = currentState.LineWidth;
				this.CurrentPath.LineCapStyle = currentState.CapStyle;
				this.CurrentPath.LineJoinStyle = currentState.JoinStyle;
			}
			if (this.CurrentPath.IsFilled)
			{
				this.CurrentPath.FillColor = currentState.CurrentNonStrokingColor;
			}
			if (this.parsingOptions.ClipPaths)
			{
				PdfPath pdfPath = currentState.CurrentClippingPath.Clip(this.CurrentPath, this.parsingOptions.Logger);
				if (pdfPath != null)
				{
					this.paths.Add(pdfPath);
					this.markedContentStack.AddPath(pdfPath);
				}
			}
			else
			{
				this.paths.Add(this.CurrentPath);
				this.markedContentStack.AddPath(this.CurrentPath);
			}
			this.CurrentPath = null;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0001F144 File Offset: 0x0001D344
		public void ModifyClippingIntersect(FillingRule clippingRule)
		{
			if (this.CurrentPath == null)
			{
				return;
			}
			this.AddCurrentSubpath();
			this.CurrentPath.SetClipping(clippingRule);
			if (this.parsingOptions.ClipPaths)
			{
				PdfPath currentClippingPath = this.GetCurrentState().CurrentClippingPath;
				currentClippingPath.SetClipping(clippingRule);
				PdfPath pdfPath = this.CurrentPath.Clip(currentClippingPath, this.parsingOptions.Logger);
				if (pdfPath == null)
				{
					this.parsingOptions.Logger.Warn("Empty clipping path found. Clipping path not updated.");
					return;
				}
				this.GetCurrentState().CurrentClippingPath = pdfPath;
			}
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0001F1CC File Offset: 0x0001D3CC
		public void SetNamedGraphicsState(NameToken stateName)
		{
			CurrentGraphicsState currentState = this.GetCurrentState();
			DictionaryToken extendedGraphicsStateDictionary = this.resourceStore.GetExtendedGraphicsStateDictionary(stateName);
			NumericToken numericToken;
			if (extendedGraphicsStateDictionary.TryGet(NameToken.Lw, this.pdfScanner, out numericToken))
			{
				currentState.LineWidth = numericToken.Data;
			}
			NumericToken numericToken2;
			if (extendedGraphicsStateDictionary.TryGet(NameToken.Lc, this.pdfScanner, out numericToken2))
			{
				currentState.CapStyle = (LineCapStyle)numericToken2.Int;
			}
			NumericToken numericToken3;
			if (extendedGraphicsStateDictionary.TryGet(NameToken.Lj, this.pdfScanner, out numericToken3))
			{
				currentState.JoinStyle = (LineJoinStyle)numericToken3.Int;
			}
			ArrayToken arrayToken;
			if (extendedGraphicsStateDictionary.TryGet(NameToken.Font, this.pdfScanner, out arrayToken) && arrayToken.Length == 2)
			{
				IndirectReferenceToken indirectReferenceToken = arrayToken.Data[0] as IndirectReferenceToken;
				if (indirectReferenceToken != null)
				{
					NumericToken numericToken4 = arrayToken.Data[1] as NumericToken;
					if (numericToken4 != null)
					{
						currentState.FontState.FromExtendedGraphicsState = true;
						currentState.FontState.FontSize = (double)numericToken4.Data;
						this.activeExtendedGraphicsStateFont = this.resourceStore.GetFontDirectly(indirectReferenceToken);
					}
				}
			}
			BooleanToken booleanToken;
			if (extendedGraphicsStateDictionary.TryGet(NameToken.Ais, this.pdfScanner, out booleanToken))
			{
				currentState.AlphaSource = booleanToken.Data;
			}
			NumericToken numericToken5;
			if (extendedGraphicsStateDictionary.TryGet(NameToken.Ca, this.pdfScanner, out numericToken5))
			{
				currentState.AlphaConstantStroking = numericToken5.Data;
			}
			NumericToken numericToken6;
			if (extendedGraphicsStateDictionary.TryGet(NameToken.CaNs, this.pdfScanner, out numericToken6))
			{
				currentState.AlphaConstantNonStroking = numericToken6.Data;
			}
			BooleanToken booleanToken2;
			if (extendedGraphicsStateDictionary.TryGet(NameToken.Op, this.pdfScanner, out booleanToken2))
			{
				currentState.Overprint = booleanToken2.Data;
			}
			BooleanToken booleanToken3;
			if (extendedGraphicsStateDictionary.TryGet(NameToken.OpNs, this.pdfScanner, out booleanToken3))
			{
				currentState.NonStrokingOverprint = booleanToken3.Data;
			}
			NumericToken numericToken7;
			if (extendedGraphicsStateDictionary.TryGet(NameToken.Opm, this.pdfScanner, out numericToken7))
			{
				currentState.OverprintMode = numericToken7.Data;
			}
			BooleanToken booleanToken4;
			if (extendedGraphicsStateDictionary.TryGet(NameToken.Sa, this.pdfScanner, out booleanToken4))
			{
				currentState.StrokeAdjustment = booleanToken4.Data;
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0001F3C2 File Offset: 0x0001D5C2
		public void BeginInlineImage()
		{
			if (this.inlineImageBuilder != null)
			{
				this.parsingOptions.Logger.Error("Begin inline image (BI) command encountered while another inline image was active.");
			}
			this.inlineImageBuilder = new InlineImageBuilder();
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0001F3EC File Offset: 0x0001D5EC
		public void SetInlineImageProperties(IReadOnlyDictionary<NameToken, IToken> properties)
		{
			if (this.inlineImageBuilder == null)
			{
				this.parsingOptions.Logger.Error("Begin inline image data (ID) command encountered without a corresponding begin inline image (BI) command.");
				return;
			}
			this.inlineImageBuilder.Properties = properties;
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0001F418 File Offset: 0x0001D618
		public void EndInlineImage(IReadOnlyList<byte> bytes)
		{
			if (this.inlineImageBuilder == null)
			{
				this.parsingOptions.Logger.Error("End inline image (EI) command encountered without a corresponding begin inline image (BI) command.");
				return;
			}
			this.inlineImageBuilder.Bytes = bytes;
			InlineImage inlineImage = this.inlineImageBuilder.CreateInlineImage(this.CurrentTransformationMatrix, this.filterProvider, this.pdfScanner, this.GetCurrentState().RenderingIntent, this.resourceStore);
			this.images.Add(Union<XObjectContentRecord, InlineImage>.Two(inlineImage));
			this.markedContentStack.AddImage(inlineImage);
			this.inlineImageBuilder = null;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0001F4A2 File Offset: 0x0001D6A2
		public void BeginMarkedContent(NameToken name, NameToken propertyDictionaryName, DictionaryToken properties)
		{
			if (propertyDictionaryName != null)
			{
				properties = (this.resourceStore.GetMarkedContentPropertiesDictionary(propertyDictionaryName) ?? properties);
			}
			this.markedContentStack.Push(name, properties);
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0001F4D0 File Offset: 0x0001D6D0
		public void EndMarkedContent()
		{
			if (this.markedContentStack.CanPop)
			{
				MarkedContentElement markedContentElement = this.markedContentStack.Pop(this.pdfScanner);
				if (markedContentElement != null)
				{
					this.markedContents.Add(markedContentElement);
				}
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0001F50C File Offset: 0x0001D70C
		private void AdjustTextMatrix(double tx, double ty)
		{
			TransformationMatrix translationMatrix = TransformationMatrix.GetTranslationMatrix(tx, ty);
			this.TextMatrices.TextMatrix = translationMatrix.Multiply(this.TextMatrices.TextMatrix);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0001F53E File Offset: 0x0001D73E
		public void SetFlatnessTolerance(decimal tolerance)
		{
			this.GetCurrentState().Flatness = tolerance;
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001F54C File Offset: 0x0001D74C
		public void SetLineCap(LineCapStyle cap)
		{
			this.GetCurrentState().CapStyle = cap;
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001F55A File Offset: 0x0001D75A
		public void SetLineDashPattern(LineDashPattern pattern)
		{
			this.GetCurrentState().LineDashPattern = pattern;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001F568 File Offset: 0x0001D768
		public void SetLineJoin(LineJoinStyle join)
		{
			this.GetCurrentState().JoinStyle = join;
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0001F576 File Offset: 0x0001D776
		public void SetLineWidth(decimal width)
		{
			this.GetCurrentState().LineWidth = width;
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0001F584 File Offset: 0x0001D784
		public void SetMiterLimit(decimal limit)
		{
			this.GetCurrentState().MiterLimit = limit;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0001F592 File Offset: 0x0001D792
		public void MoveToNextLineWithOffset()
		{
			new MoveToNextLineWithOffset(0m, -1m * (decimal)this.GetCurrentState().FontState.Leading).Run(this);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0001F5C3 File Offset: 0x0001D7C3
		public void SetFontAndSize(NameToken font, double size)
		{
			CurrentGraphicsState currentState = this.GetCurrentState();
			currentState.FontState.FontSize = size;
			currentState.FontState.FontName = font;
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0001F5E2 File Offset: 0x0001D7E2
		public void SetHorizontalScaling(double scale)
		{
			this.GetCurrentState().FontState.HorizontalScaling = scale;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0001F5F5 File Offset: 0x0001D7F5
		public void SetTextLeading(double leading)
		{
			this.GetCurrentState().FontState.Leading = leading;
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001F608 File Offset: 0x0001D808
		public void SetTextRenderingMode(TextRenderingMode mode)
		{
			this.GetCurrentState().FontState.TextRenderingMode = mode;
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0001F61B File Offset: 0x0001D81B
		public void SetTextRise(double rise)
		{
			this.GetCurrentState().FontState.Rise = rise;
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0001F62E File Offset: 0x0001D82E
		public void SetWordSpacing(double spacing)
		{
			this.GetCurrentState().FontState.WordSpacing = spacing;
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0001F644 File Offset: 0x0001D844
		public void ModifyCurrentTransformationMatrix(double[] value)
		{
			TransformationMatrix currentTransformationMatrix = this.GetCurrentState().CurrentTransformationMatrix;
			this.GetCurrentState().CurrentTransformationMatrix = TransformationMatrix.FromArray(value).Multiply(currentTransformationMatrix);
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0001F677 File Offset: 0x0001D877
		public void SetCharacterSpacing(double spacing)
		{
			this.GetCurrentState().FontState.CharacterSpacing = spacing;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0001F68A File Offset: 0x0001D88A
		public void PaintShading(NameToken shadingName)
		{
		}

		// Token: 0x040002A9 RID: 681
		private readonly List<Letter> letters = new List<Letter>();

		// Token: 0x040002AA RID: 682
		private readonly List<PdfPath> paths = new List<PdfPath>();

		// Token: 0x040002AB RID: 683
		private readonly List<Union<XObjectContentRecord, InlineImage>> images = new List<Union<XObjectContentRecord, InlineImage>>();

		// Token: 0x040002AC RID: 684
		private readonly List<MarkedContentElement> markedContents = new List<MarkedContentElement>();

		// Token: 0x040002AD RID: 685
		private readonly IResourceStore resourceStore;

		// Token: 0x040002AE RID: 686
		private readonly UserSpaceUnit userSpaceUnit;

		// Token: 0x040002AF RID: 687
		private readonly PageRotationDegrees rotation;

		// Token: 0x040002B0 RID: 688
		private readonly IPdfTokenScanner pdfScanner;

		// Token: 0x040002B1 RID: 689
		private readonly IPageContentParser pageContentParser;

		// Token: 0x040002B2 RID: 690
		private readonly ILookupFilterProvider filterProvider;

		// Token: 0x040002B3 RID: 691
		private readonly InternalParsingOptions parsingOptions;

		// Token: 0x040002B4 RID: 692
		private readonly MarkedContentStack markedContentStack = new MarkedContentStack();

		// Token: 0x040002B5 RID: 693
		private Stack<CurrentGraphicsState> graphicsStack = new Stack<CurrentGraphicsState>();

		// Token: 0x040002B6 RID: 694
		private IFont activeExtendedGraphicsStateFont;

		// Token: 0x040002B7 RID: 695
		private InlineImageBuilder inlineImageBuilder;

		// Token: 0x040002B8 RID: 696
		private int pageNumber;

		// Token: 0x040002B9 RID: 697
		private int textSequence;

		// Token: 0x040002BE RID: 702
		private readonly Dictionary<XObjectType, List<XObjectContentRecord>> xObjects;
	}
}
