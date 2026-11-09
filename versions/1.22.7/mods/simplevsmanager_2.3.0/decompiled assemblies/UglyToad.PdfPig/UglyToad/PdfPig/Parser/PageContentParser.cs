using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Graphics;
using UglyToad.PdfPig.Graphics.Operations;
using UglyToad.PdfPig.Graphics.Operations.InlineImages;
using UglyToad.PdfPig.Graphics.Operations.TextObjects;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Parser
{
	// Token: 0x020000C5 RID: 197
	internal class PageContentParser : IPageContentParser
	{
		// Token: 0x060003C0 RID: 960 RVA: 0x00016272 File Offset: 0x00014472
		public PageContentParser(IGraphicsStateOperationFactory operationFactory)
		{
			this.operationFactory = operationFactory;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00016284 File Offset: 0x00014484
		public IReadOnlyList<IGraphicsStateOperation> Parse(int pageNumber, IInputBytes inputBytes, ILog log)
		{
			CoreTokenScanner coreTokenScanner = new CoreTokenScanner(inputBytes, 0, null);
			List<IToken> list = new List<IToken>();
			List<IGraphicsStateOperation> list2 = new List<IGraphicsStateOperation>();
			long? num = default(long?);
			while (coreTokenScanner.MoveNext())
			{
				IToken currentToken = coreTokenScanner.CurrentToken;
				InlineImageDataToken inlineImageDataToken = currentToken as InlineImageDataToken;
				if (inlineImageDataToken != null)
				{
					Dictionary<NameToken, IToken> dictionary = new Dictionary<NameToken, IToken>();
					for (int i = 0; i < list.Count - 1; i++)
					{
						NameToken nameToken = list[i] as NameToken;
						if (nameToken != null)
						{
							i++;
							dictionary[nameToken] = list[i];
						}
					}
					list2.Add(new BeginInlineImageData(dictionary));
					list2.Add(new EndInlineImage(inlineImageDataToken.Data));
					num = new long?(coreTokenScanner.CurrentPosition - 2L);
					list.Clear();
				}
				else
				{
					OperatorToken operatorToken = currentToken as OperatorToken;
					if (operatorToken != null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
						if (operatorToken.Data == "EI")
						{
							IGraphicsStateOperation graphicsStateOperation = (list2.Count > 0) ? list2[list2.Count - 1] : null;
							if (num != null && graphicsStateOperation != null)
							{
								EndInlineImage endInlineImage = graphicsStateOperation as EndInlineImage;
								if (endInlineImage != null)
								{
									long num2 = coreTokenScanner.CurrentPosition - 3L;
									defaultInterpolatedStringHandler..ctor(77, 1);
									defaultInterpolatedStringHandler.AppendLiteral("End inline image (EI) encountered after previous EI, attempting recovery at ");
									defaultInterpolatedStringHandler.AppendFormatted<long>(num2);
									defaultInterpolatedStringHandler.AppendLiteral(".");
									log.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
									int num3 = (int)(num2 - num).Value;
									long currentOffset = inputBytes.CurrentOffset;
									inputBytes.Seek(num.Value);
									byte[] array = new byte[num3];
									if (inputBytes.Read(array, default(int?)) != num3)
									{
										defaultInterpolatedStringHandler..ctor(97, 3);
										defaultInterpolatedStringHandler.AppendLiteral("Failed to read expected buffer length ");
										defaultInterpolatedStringHandler.AppendFormatted<int>(num3);
										defaultInterpolatedStringHandler.AppendLiteral(" on page ");
										defaultInterpolatedStringHandler.AppendFormatted<int>(pageNumber);
										defaultInterpolatedStringHandler.AppendLiteral(" ");
										defaultInterpolatedStringHandler.AppendLiteral("when reading inline image at offset in content: ");
										defaultInterpolatedStringHandler.AppendFormatted<long>(num.Value);
										defaultInterpolatedStringHandler.AppendLiteral(".");
										throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
									}
									list2.Remove(endInlineImage);
									list2.Add(new EndInlineImage(Enumerable.ToArray<byte>(Enumerable.Concat<byte>(endInlineImage.ImageData, array))));
									num = new long?(num2);
									inputBytes.Seek(currentOffset);
									goto IL_46E;
								}
							}
							string text = "Encountered End Image token outside an inline image on ";
							defaultInterpolatedStringHandler..ctor(29, 2);
							defaultInterpolatedStringHandler.AppendLiteral("page ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(pageNumber);
							defaultInterpolatedStringHandler.AppendLiteral(" at offset in content: ");
							defaultInterpolatedStringHandler.AppendFormatted<long>(coreTokenScanner.CurrentPosition);
							defaultInterpolatedStringHandler.AppendLiteral(".");
							throw new PdfDocumentFormatException(text + defaultInterpolatedStringHandler.ToStringAndClear());
						}
						IGraphicsStateOperation graphicsStateOperation2;
						try
						{
							graphicsStateOperation2 = this.operationFactory.Create(operatorToken, list);
						}
						catch (Exception ex)
						{
							EndInlineImage endInlineImage2;
							int num4;
							if (!PageContentParser.TryGetLastEndImage(list2, out endInlineImage2, out num4))
							{
								throw;
							}
							defaultInterpolatedStringHandler..ctor(49, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Failed reading an operation at offset ");
							defaultInterpolatedStringHandler.AppendFormatted<long>(inputBytes.CurrentOffset);
							defaultInterpolatedStringHandler.AppendLiteral(" for page ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(pageNumber);
							defaultInterpolatedStringHandler.AppendLiteral(".");
							log.Error(defaultInterpolatedStringHandler.ToStringAndClear(), ex);
							graphicsStateOperation2 = null;
						}
						if (graphicsStateOperation2 != null)
						{
							list2.Add(graphicsStateOperation2);
						}
						else if (list2.Count > 0)
						{
							EndInlineImage endInlineImage3;
							int num5;
							if (PageContentParser.TryGetLastEndImage(list2, out endInlineImage3, out num5) && num != null)
							{
								defaultInterpolatedStringHandler..ctor(69, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Operator ");
								defaultInterpolatedStringHandler.AppendFormatted(operatorToken.Data);
								defaultInterpolatedStringHandler.AppendLiteral(" was not understood following end of inline image data at ");
								defaultInterpolatedStringHandler.AppendFormatted<long?>(num);
								defaultInterpolatedStringHandler.AppendLiteral(", ");
								log.Warn(defaultInterpolatedStringHandler.ToStringAndClear() + "attempting recovery.");
								IReadOnlyList<byte> readOnlyList = coreTokenScanner.RecoverFromIncorrectEndImage(num.Value);
								list2.RemoveRange(num5, list2.Count - num5);
								EndInlineImage endInlineImage4 = new EndInlineImage(Enumerable.ToList<byte>(Enumerable.Concat<byte>(endInlineImage3.ImageData, readOnlyList)));
								list2.Add(endInlineImage4);
								num = new long?(coreTokenScanner.CurrentPosition - 3L);
							}
							else
							{
								if (operatorToken.Data == "inf")
								{
									list.Add(NumericToken.Zero);
									continue;
								}
								log.Warn("Operator which was not understood encountered. Values was " + operatorToken.Data + ". Ignoring.");
							}
						}
						IL_46E:
						list.Clear();
					}
					else if (!(currentToken is CommentToken))
					{
						list.Add(currentToken);
					}
				}
			}
			return list2;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00016734 File Offset: 0x00014934
		private static bool TryGetLastEndImage(List<IGraphicsStateOperation> graphicsStateOperations, out EndInlineImage endImage, out int index)
		{
			index = -1;
			endImage = null;
			if (graphicsStateOperations.Count == 0)
			{
				return false;
			}
			for (int i = graphicsStateOperations.Count - 1; i >= 0; i--)
			{
				IGraphicsStateOperation graphicsStateOperation = graphicsStateOperations[i];
				EndInlineImage endInlineImage = graphicsStateOperation as EndInlineImage;
				if (endInlineImage != null)
				{
					endImage = endInlineImage;
					index = i;
					return true;
				}
				if (graphicsStateOperation is EndText || graphicsStateOperation is BeginInlineImageData)
				{
					break;
				}
			}
			return false;
		}

		// Token: 0x040001EE RID: 494
		private readonly IGraphicsStateOperationFactory operationFactory;
	}
}
