using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UglyToad.PdfPig.Actions;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.Standard14Fonts;
using UglyToad.PdfPig.Fonts.TrueType;
using UglyToad.PdfPig.Fonts.TrueType.Parser;
using UglyToad.PdfPig.Outline;
using UglyToad.PdfPig.Outline.Destinations;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;
using UglyToad.PdfPig.Writer.Fonts;

namespace UglyToad.PdfPig.Writer
{
	// Token: 0x02000015 RID: 21
	public class PdfDocumentBuilder : IDisposable
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000064 RID: 100 RVA: 0x0000340C File Offset: 0x0000160C
		// (set) Token: 0x06000065 RID: 101 RVA: 0x00003414 File Offset: 0x00001614
		public PdfAStandard ArchiveStandard { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000066 RID: 102 RVA: 0x0000341D File Offset: 0x0000161D
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00003425 File Offset: 0x00001625
		public bool IncludeDocumentInformation { get; set; } = true;

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000068 RID: 104 RVA: 0x0000342E File Offset: 0x0000162E
		// (set) Token: 0x06000069 RID: 105 RVA: 0x00003436 File Offset: 0x00001636
		public PdfDocumentBuilder.DocumentInformationBuilder DocumentInformation { get; set; } = new PdfDocumentBuilder.DocumentInformationBuilder();

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600006A RID: 106 RVA: 0x0000343F File Offset: 0x0000163F
		// (set) Token: 0x0600006B RID: 107 RVA: 0x00003447 File Offset: 0x00001647
		public Bookmarks Bookmarks { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00003450 File Offset: 0x00001650
		public IReadOnlyDictionary<int, PdfPageBuilder> Pages
		{
			get
			{
				return this.pages;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00003458 File Offset: 0x00001658
		internal IReadOnlyDictionary<Guid, PdfDocumentBuilder.FontStored> Fonts
		{
			get
			{
				return this.fonts;
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003460 File Offset: 0x00001660
		public PdfDocumentBuilder()
		{
			this.context = new PdfStreamWriter(new MemoryStream(), true, null, delegate(decimal x)
			{
				this.version = x;
			});
			this.context.InitializePdf(1.7m);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000034F8 File Offset: 0x000016F8
		public PdfDocumentBuilder(decimal version)
		{
			this.context = new PdfStreamWriter(new MemoryStream(), true, null, delegate(decimal x)
			{
				version = x;
			});
			this.context.InitializePdf(version);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003598 File Offset: 0x00001798
		public PdfDocumentBuilder(Stream stream, bool disposeStream = false, PdfWriterType type = PdfWriterType.Default, [Optional] decimal version = 1.7m, ITokenWriter tokenWriter = null)
		{
			if (type == PdfWriterType.ObjectInMemoryDedup)
			{
				this.context = new PdfDedupStreamWriter(stream, disposeStream, tokenWriter, delegate(decimal x)
				{
					version = x;
				});
			}
			else
			{
				this.context = new PdfStreamWriter(stream, disposeStream, tokenWriter, delegate(decimal x)
				{
					version = x;
				});
			}
			this.context.InitializePdf(version);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003658 File Offset: 0x00001858
		public bool CanUseTrueTypeFont(IReadOnlyList<byte> fontFileBytes, out IReadOnlyList<string> reasons)
		{
			List<string> list = new List<string>();
			reasons = list;
			bool result;
			try
			{
				if (fontFileBytes == null)
				{
					list.Add("Provided bytes were null.");
					result = false;
				}
				else if (fontFileBytes.Count == 0)
				{
					list.Add("Provided bytes were empty.");
					result = false;
				}
				else
				{
					TrueTypeFont trueTypeFont = TrueTypeFontParser.Parse(new TrueTypeDataBytes(new ByteArrayInputBytes(fontFileBytes)));
					if (trueTypeFont.TableRegister.CMapTable == null)
					{
						list.Add("The provided font did not contain a cmap table, used to map character codes to glyph codes.");
						result = false;
					}
					else if (trueTypeFont.TableRegister.Os2Table == null)
					{
						list.Add("The provided font did not contain an OS/2 table, used to fill in the font descriptor dictionary.");
						result = false;
					}
					else if (trueTypeFont.TableRegister.PostScriptTable == null)
					{
						list.Add("The provided font did not contain a post PostScript table, used to map character codes to glyph codes.");
						result = false;
					}
					else
					{
						result = true;
					}
				}
			}
			catch (Exception ex)
			{
				list.Add(ex.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003728 File Offset: 0x00001928
		public PdfDocumentBuilder.AddedFont AddTrueTypeFont(IReadOnlyList<byte> fontFileBytes)
		{
			PdfDocumentBuilder.AddedFont result;
			try
			{
				TrueTypeFont font = TrueTypeFontParser.Parse(new TrueTypeDataBytes(new ByteArrayInputBytes(fontFileBytes)));
				Guid guid = Guid.NewGuid();
				PdfDocumentBuilder.AddedFont addedFont = new PdfDocumentBuilder.AddedFont(guid, this.context.ReserveObjectNumber());
				this.fonts[guid] = new PdfDocumentBuilder.FontStored(addedFont, new TrueTypeWritingFont(font, fontFileBytes));
				result = addedFont;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Writing only supports TrueType fonts, please provide a valid TrueType font.", ex);
			}
			return result;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000379C File Offset: 0x0000199C
		public PdfDocumentBuilder.AddedFont AddStandard14Font(Standard14Font type)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (this.ArchiveStandard != PdfAStandard.None)
			{
				defaultInterpolatedStringHandler..ctor(72, 2);
				defaultInterpolatedStringHandler.AppendLiteral("PDF/A ");
				defaultInterpolatedStringHandler.AppendFormatted<PdfAStandard>(this.ArchiveStandard);
				defaultInterpolatedStringHandler.AppendLiteral(" requires the font to be embedded in the file, only ");
				defaultInterpolatedStringHandler.AppendFormatted("AddTrueTypeFont");
				defaultInterpolatedStringHandler.AppendLiteral(" is supported.");
				throw new NotSupportedException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Guid guid = Guid.NewGuid();
			defaultInterpolatedStringHandler..ctor(1, 1);
			defaultInterpolatedStringHandler.AppendLiteral("F");
			int num = this.fontId;
			this.fontId = num + 1;
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			NameToken.Create(defaultInterpolatedStringHandler.ToStringAndClear());
			PdfDocumentBuilder.AddedFont addedFont = new PdfDocumentBuilder.AddedFont(guid, this.context.ReserveObjectNumber());
			this.fonts[guid] = new PdfDocumentBuilder.FontStored(addedFont, new Standard14WritingFont(Standard14.GetAdobeFontMetrics(type)));
			return addedFont;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003878 File Offset: 0x00001A78
		internal IndirectReferenceToken AddImage(DictionaryToken dictionary, byte[] bytes)
		{
			StreamToken token = new StreamToken(dictionary, bytes);
			return this.context.WriteToken(token);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000389C File Offset: 0x00001A9C
		public PdfPageBuilder AddPage(double width, double height)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (width < 0.0)
			{
				string text = "width";
				defaultInterpolatedStringHandler..ctor(32, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Width cannot be negative, got: ");
				defaultInterpolatedStringHandler.AppendFormatted<double>(width);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentOutOfRangeException(text, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (height < 0.0)
			{
				string text2 = "height";
				defaultInterpolatedStringHandler..ctor(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Height cannot be negative, got: ");
				defaultInterpolatedStringHandler.AppendFormatted<double>(height);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentOutOfRangeException(text2, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			PdfPageBuilder pdfPageBuilder = null;
			for (int i = 0; i < this.pages.Count; i++)
			{
				if (!this.pages.ContainsKey(i + 1))
				{
					pdfPageBuilder = new PdfPageBuilder(i + 1, this);
					break;
				}
			}
			if (pdfPageBuilder == null)
			{
				pdfPageBuilder = new PdfPageBuilder(this.pages.Count + 1, this);
			}
			pdfPageBuilder.PageSize = new PdfRectangle(0.0, 0.0, width, height);
			this.pages[pdfPageBuilder.PageNumber] = pdfPageBuilder;
			return pdfPageBuilder;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000039B8 File Offset: 0x00001BB8
		public PdfPageBuilder AddPage(PageSize size, bool isPortrait = true)
		{
			if (size == PageSize.Custom)
			{
				throw new ArgumentException("Cannot use $Custom for $AddPage using the $PageSize enum, call the overload with width and height instead.", "size");
			}
			PdfRectangle pdfRectangle;
			if (!size.TryGetPdfRectangle(out pdfRectangle))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("No rectangle found for Page Size ");
				defaultInterpolatedStringHandler.AppendFormatted<PageSize>(size);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (!isPortrait)
			{
				return this.AddPage(pdfRectangle.Height, pdfRectangle.Width);
			}
			return this.AddPage(pdfRectangle.Width, pdfRectangle.Height);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003A48 File Offset: 0x00001C48
		internal IToken CopyToken(IPdfTokenScanner source, IToken token)
		{
			Dictionary<IndirectReference, IndirectReferenceToken> dictionary;
			if (!this.existingCopies.TryGetValue(source, ref dictionary))
			{
				dictionary = new Dictionary<IndirectReference, IndirectReferenceToken>();
				this.existingCopies.Add(source, dictionary);
			}
			return WriterUtil.CopyToken(this.context, token, source, dictionary, null);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003A87 File Offset: 0x00001C87
		public PdfPageBuilder AddPage(PdfDocument document, int pageNumber)
		{
			return this.AddPage(document, pageNumber, null);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003A94 File Offset: 0x00001C94
		public PdfPageBuilder AddPage(PdfDocument document, int pageNumber, Func<PdfAction, PdfAction> copyLink)
		{
			PdfDocumentBuilder.<>c__DisplayClass41_0 CS$<>8__locals1;
			CS$<>8__locals1.document = document;
			CS$<>8__locals1.<>4__this = this;
			if (!this.existingCopies.TryGetValue(CS$<>8__locals1.document.Structure.TokenScanner, ref CS$<>8__locals1.refs))
			{
				CS$<>8__locals1.refs = new Dictionary<IndirectReference, IndirectReferenceToken>();
				this.existingCopies.Add(CS$<>8__locals1.document.Structure.TokenScanner, CS$<>8__locals1.refs);
			}
			Dictionary<int, PdfDocumentBuilder.PageInfo> dictionary;
			if (!this.existingTrees.TryGetValue(CS$<>8__locals1.document, ref dictionary))
			{
				dictionary = new Dictionary<int, PdfDocumentBuilder.PageInfo>();
				int num = 1;
				foreach (ValueTuple<DictionaryToken, IReadOnlyList<DictionaryToken>> valueTuple in WriterUtil.WalkTree(CS$<>8__locals1.document.Structure.Catalog.Pages.PageTree, null))
				{
					DictionaryToken item = valueTuple.Item1;
					IReadOnlyList<DictionaryToken> item2 = valueTuple.Item2;
					dictionary[num] = new PdfDocumentBuilder.PageInfo
					{
						Page = item,
						Parents = item2
					};
					num++;
				}
				this.existingTrees.Add(CS$<>8__locals1.document, dictionary);
			}
			PdfDocumentBuilder.PageInfo pageInfo;
			if (!dictionary.TryGetValue(pageNumber, ref pageInfo))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(43, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Page ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(pageNumber);
				defaultInterpolatedStringHandler.AppendLiteral(" was not found in the source document.");
				throw new KeyNotFoundException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Page page = CS$<>8__locals1.document.GetPage(pageNumber);
			List<PdfPageBuilder.CopiedContentStream> list = new List<PdfPageBuilder.CopiedContentStream>();
			IToken token;
			if (pageInfo.Page.TryGet(NameToken.Contents, ref token))
			{
				bool attemptDeduplication = this.context.AttemptDeduplication;
				this.context.AttemptDeduplication = false;
				ArrayToken arrayToken = token as ArrayToken;
				if (arrayToken != null)
				{
					using (IEnumerator<IToken> enumerator2 = arrayToken.Data.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							IToken token2 = enumerator2.Current;
							IndirectReferenceToken indirectReferenceToken = token2 as IndirectReferenceToken;
							if (indirectReferenceToken != null)
							{
								list.Add(new PdfPageBuilder.CopiedContentStream(WriterUtil.CopyToken(this.context, indirectReferenceToken, CS$<>8__locals1.document.Structure.TokenScanner, CS$<>8__locals1.refs, null) as IndirectReferenceToken));
							}
						}
						goto IL_250;
					}
				}
				IndirectReferenceToken indirectReferenceToken2 = token as IndirectReferenceToken;
				if (indirectReferenceToken2 != null)
				{
					list.Add(new PdfPageBuilder.CopiedContentStream(WriterUtil.CopyToken(this.context, indirectReferenceToken2, CS$<>8__locals1.document.Structure.TokenScanner, CS$<>8__locals1.refs, null) as IndirectReferenceToken));
				}
				IL_250:
				this.context.AttemptDeduplication = attemptDeduplication;
			}
			Dictionary<NameToken, IToken> dictionary2 = new Dictionary<NameToken, IToken>();
			List<ValueTuple<DictionaryToken, PdfAction>> list2 = new List<ValueTuple<DictionaryToken, PdfAction>>();
			Dictionary<NameToken, IToken> dictionary3 = new Dictionary<NameToken, IToken>();
			foreach (DictionaryToken dictionaryToken in pageInfo.Parents)
			{
				IToken token3;
				if (dictionaryToken.TryGet(NameToken.Resources, ref token3))
				{
					this.<AddPage>g__CopyResourceDict|41_0(token3, dictionary3, ref CS$<>8__locals1);
				}
				IToken tokenToCopy;
				if (dictionaryToken.TryGet(NameToken.MediaBox, ref tokenToCopy))
				{
					dictionary2[NameToken.MediaBox] = WriterUtil.CopyToken(this.context, tokenToCopy, CS$<>8__locals1.document.Structure.TokenScanner, CS$<>8__locals1.refs, null);
				}
				IToken tokenToCopy2;
				if (dictionaryToken.TryGet(NameToken.CropBox, ref tokenToCopy2))
				{
					dictionary2[NameToken.CropBox] = WriterUtil.CopyToken(this.context, tokenToCopy2, CS$<>8__locals1.document.Structure.TokenScanner, CS$<>8__locals1.refs, null);
				}
				IToken tokenToCopy3;
				if (dictionaryToken.TryGet(NameToken.Rotate, ref tokenToCopy3))
				{
					dictionary2[NameToken.Rotate] = WriterUtil.CopyToken(this.context, tokenToCopy3, CS$<>8__locals1.document.Structure.TokenScanner, CS$<>8__locals1.refs, null);
				}
			}
			foreach (KeyValuePair<string, IToken> keyValuePair in pageInfo.Page.Data)
			{
				if (!(keyValuePair.Key == NameToken.Contents) && !(keyValuePair.Key == NameToken.Parent) && !(keyValuePair.Key == NameToken.Type))
				{
					if (keyValuePair.Key == NameToken.Resources)
					{
						this.<AddPage>g__CopyResourceDict|41_0(keyValuePair.Value, dictionary3, ref CS$<>8__locals1);
					}
					else if (keyValuePair.Key == NameToken.Annots)
					{
						IToken token4 = keyValuePair.Value;
						IndirectReferenceToken indirectReferenceToken3 = keyValuePair.Value as IndirectReferenceToken;
						if (indirectReferenceToken3 != null)
						{
							ObjectToken objectToken = CS$<>8__locals1.document.Structure.TokenScanner.Get(indirectReferenceToken3.Data);
							if (objectToken == null)
							{
								continue;
							}
							token4 = objectToken.Data;
						}
						ArrayToken arrayToken2 = token4 as ArrayToken;
						if (arrayToken2 != null)
						{
							List<IToken> list3 = new List<IToken>();
							foreach (IToken token5 in arrayToken2.Data)
							{
								DictionaryToken dictionaryToken2 = this.<AddPage>g__GetRemoteDict|41_1(token5, ref CS$<>8__locals1);
								if (dictionaryToken2 != null)
								{
									IToken token6;
									if (dictionaryToken2.TryGet(NameToken.Subtype, ref token6))
									{
										NameToken nameToken = token6 as NameToken;
										if (nameToken != null && nameToken == NameToken.Link)
										{
											if (copyLink == null)
											{
												continue;
											}
											PdfAction action = page.annotationProvider.GetAction(dictionaryToken2);
											if (action == null)
											{
												continue;
											}
											PdfAction pdfAction = copyLink.Invoke(action);
											if (pdfAction == null)
											{
												continue;
											}
											if (pdfAction != action)
											{
												DictionaryToken dictionaryToken3 = (DictionaryToken)WriterUtil.CopyToken(this.context, dictionaryToken2, CS$<>8__locals1.document.Structure.TokenScanner, CS$<>8__locals1.refs, null);
												list2.Add(new ValueTuple<DictionaryToken, PdfAction>(dictionaryToken3, pdfAction));
												continue;
											}
										}
									}
									list3.Add(WriterUtil.CopyToken(this.context, dictionaryToken2, CS$<>8__locals1.document.Structure.TokenScanner, CS$<>8__locals1.refs, null));
								}
							}
							dictionary2[NameToken.Annots] = new ArrayToken(list3);
						}
					}
					else
					{
						dictionary2[NameToken.Create(keyValuePair.Key)] = WriterUtil.CopyToken(this.context, keyValuePair.Value, CS$<>8__locals1.document.Structure.TokenScanner, CS$<>8__locals1.refs, null);
					}
				}
			}
			dictionary2[NameToken.Resources] = new DictionaryToken(dictionary3);
			PdfPageBuilder pdfPageBuilder = new PdfPageBuilder(this.pages.Count + 1, this, list, dictionary2, list2);
			this.pages[pdfPageBuilder.PageNumber] = pdfPageBuilder;
			return pdfPageBuilder;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004170 File Offset: 0x00002370
		private void CompleteDocument()
		{
			PdfDocumentBuilder.<>c__DisplayClass42_0 CS$<>8__locals1 = new PdfDocumentBuilder.<>c__DisplayClass42_0();
			CS$<>8__locals1.<>4__this = this;
			foreach (KeyValuePair<Guid, PdfDocumentBuilder.FontStored> keyValuePair in this.fonts)
			{
				keyValuePair.Value.FontProgram.WriteFont(this.context, keyValuePair.Value.FontKey.Reference);
			}
			int num = (int)Math.Ceiling(decimal.Divide(this.Pages.Count, 25m));
			List<IndirectReferenceToken> list = new List<IndirectReferenceToken>();
			List<List<IndirectReferenceToken>> list2 = new List<List<IndirectReferenceToken>>();
			List<Dictionary<NameToken, IToken>> list3 = new List<Dictionary<NameToken, IToken>>();
			for (int i = 0; i < num; i++)
			{
				List<Dictionary<NameToken, IToken>> list4 = list3;
				Dictionary<NameToken, IToken> dictionary = new Dictionary<NameToken, IToken>();
				dictionary.Add(NameToken.Type, NameToken.Pages);
				list4.Add(dictionary);
				list2.Add(new List<IndirectReferenceToken>());
				list.Add(this.context.ReserveObjectNumber());
			}
			int num2 = 0;
			Dictionary<int, IndirectReferenceToken> dictionary2 = Enumerable.ToDictionary<KeyValuePair<int, PdfPageBuilder>, int, IndirectReferenceToken>(this.pages, (KeyValuePair<int, PdfPageBuilder> p) => p.Key, (KeyValuePair<int, PdfPageBuilder> p) => CS$<>8__locals1.<>4__this.context.ReserveObjectNumber());
			foreach (KeyValuePair<int, PdfPageBuilder> keyValuePair2 in this.pages)
			{
				Dictionary<NameToken, IToken> pageDictionary = keyValuePair2.Value.pageDictionary;
				pageDictionary[NameToken.Type] = NameToken.Page;
				pageDictionary[NameToken.Parent] = list[num2];
				pageDictionary[NameToken.ProcSet] = PdfDocumentBuilder.DefaultProcSet;
				if (!pageDictionary.ContainsKey(NameToken.MediaBox))
				{
					pageDictionary[NameToken.MediaBox] = PdfDocumentBuilder.RectangleToArray(keyValuePair2.Value.PageSize);
				}
				if (keyValuePair2.Value.rotation != null)
				{
					pageDictionary[NameToken.Rotate] = new NumericToken(keyValuePair2.Value.rotation.Value);
				}
				bool attemptDeduplication = this.context.AttemptDeduplication;
				this.context.AttemptDeduplication = false;
				List<PdfPageBuilder.IPageContentStream> list5 = Enumerable.ToList<PdfPageBuilder.IPageContentStream>(Enumerable.Where<PdfPageBuilder.IPageContentStream>(keyValuePair2.Value.contentStreams, (PdfPageBuilder.IPageContentStream x) => x.HasContent));
				if (list5.Count == 0)
				{
					pageDictionary[NameToken.Contents] = new PdfPageBuilder.DefaultContentStream().Write(this.context);
				}
				else if (list5.Count == 1)
				{
					pageDictionary[NameToken.Contents] = list5[0].Write(this.context);
				}
				else
				{
					List<IToken> list6 = new List<IToken>();
					foreach (PdfPageBuilder.IPageContentStream pageContentStream in list5)
					{
						list6.Add(pageContentStream.Write(this.context));
					}
					pageDictionary[NameToken.Contents] = new ArrayToken(list6);
				}
				this.context.AttemptDeduplication = attemptDeduplication;
				if (keyValuePair2.Value.links != null && keyValuePair2.Value.links.Count > 0)
				{
					List<IToken> list7 = new List<IToken>();
					IToken token;
					if (pageDictionary.TryGetValue(NameToken.Annots, ref token))
					{
						list7.AddRange(((ArrayToken)token).Data);
					}
					foreach (ValueTuple<DictionaryToken, PdfAction> valueTuple in keyValuePair2.Value.links)
					{
						DictionaryToken item = valueTuple.Item1;
						PdfAction item2 = valueTuple.Item2;
						list7.Add(PdfDocumentBuilder.CreateLinkAnnotationToken(item, item2, dictionary2));
					}
					pageDictionary[NameToken.Annots] = new ArrayToken(list7);
				}
				list2[num2].Add(this.context.WriteToken(new DictionaryToken(pageDictionary), dictionary2[keyValuePair2.Key]));
				if (list2[num2].Count >= 25)
				{
					num2++;
				}
			}
			CS$<>8__locals1.dummyName = NameToken.Create("ObjIdToUse");
			for (int j = 0; j < list3.Count; j++)
			{
				list3[j][NameToken.Kids] = new ArrayToken(list2[j]);
				list3[j][NameToken.Count] = new NumericToken(list2[j].Count);
				list3[j][CS$<>8__locals1.dummyName] = list[j];
			}
			Dictionary<NameToken, IToken> dictionary3 = new Dictionary<NameToken, IToken>();
			dictionary3.Add(NameToken.Type, NameToken.Catalog);
			Dictionary<NameToken, IToken> dictionary4 = dictionary3;
			if (list3.Count == 1)
			{
				Dictionary<NameToken, IToken> dictionary5 = list3[0];
				IndirectReferenceToken indirectReference = dictionary5[CS$<>8__locals1.dummyName] as IndirectReferenceToken;
				dictionary5.Remove(CS$<>8__locals1.dummyName);
				dictionary4[NameToken.Pages] = this.context.WriteToken(new DictionaryToken(dictionary5), indirectReference);
			}
			else
			{
				ValueTuple<int, IndirectReferenceToken> valueTuple2 = CS$<>8__locals1.<CompleteDocument>g__CreatePageTree|2(list3, null);
				dictionary4[NameToken.Pages] = valueTuple2.Item2;
			}
			if (this.Bookmarks != null && this.Bookmarks.Roots.Count > 0)
			{
				IndirectReferenceToken[] array = this.CreateBookmarkTree(this.Bookmarks.Roots, dictionary2, null);
				Dictionary<NameToken, IToken> dictionary6 = new Dictionary<NameToken, IToken>();
				dictionary6.Add(NameToken.Type, NameToken.Outlines);
				dictionary6.Add(NameToken.Count, new NumericToken(this.Bookmarks.Roots.Count));
				dictionary6.Add(NameToken.First, array[0]);
				dictionary6.Add(NameToken.Last, array[array.Length - 1]);
				Dictionary<NameToken, IToken> dictionary7 = dictionary6;
				dictionary4[NameToken.Outlines] = this.context.WriteToken(new DictionaryToken(dictionary7));
			}
			if (this.ArchiveStandard != PdfAStandard.None)
			{
				Func<IToken, IndirectReferenceToken> writerFunc = (IToken x) => CS$<>8__locals1.<>4__this.context.WriteToken(x);
				PdfABaselineRuleBuilder.Obey(dictionary4, writerFunc, this.DocumentInformation, this.ArchiveStandard, this.version);
				switch (this.ArchiveStandard)
				{
				case PdfAStandard.A1A:
					PdfA1ARuleBuilder.Obey(dictionary4);
					break;
				case PdfAStandard.A2A:
					PdfA1ARuleBuilder.Obey(dictionary4);
					break;
				case PdfAStandard.A3A:
					PdfA1ARuleBuilder.Obey(dictionary4);
					break;
				}
			}
			DictionaryToken token2 = new DictionaryToken(dictionary4);
			IndirectReferenceToken catalogReference = this.context.WriteToken(token2);
			IndirectReferenceToken documentInformationReference = null;
			if (this.IncludeDocumentInformation)
			{
				Dictionary<NameToken, IToken> dictionary8 = this.DocumentInformation.ToDictionary();
				if (dictionary8.Count > 0)
				{
					DictionaryToken token3 = new DictionaryToken(dictionary8);
					documentInformationReference = this.context.WriteToken(token3);
				}
			}
			this.context.CompletePdf(catalogReference, documentInformationReference);
			this.completed = true;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004898 File Offset: 0x00002A98
		public byte[] Build()
		{
			this.CompleteDocument();
			MemoryStream memoryStream = this.context.Stream as MemoryStream;
			if (memoryStream != null)
			{
				return memoryStream.ToArray();
			}
			if (!this.context.Stream.CanSeek)
			{
				throw new InvalidOperationException("PdfDocument.Build() called with non-seekable stream.");
			}
			byte[] result;
			using (MemoryStream memoryStream2 = new MemoryStream())
			{
				this.context.Stream.Seek(0L, 0);
				this.context.Stream.CopyTo(memoryStream2);
				result = memoryStream2.ToArray();
			}
			return result;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004934 File Offset: 0x00002B34
		private static ArrayToken RectangleToArray(PdfRectangle rectangle)
		{
			return new ArrayToken(new NumericToken[]
			{
				new NumericToken((decimal)rectangle.BottomLeft.X),
				new NumericToken((decimal)rectangle.BottomLeft.Y),
				new NumericToken((decimal)rectangle.TopRight.X),
				new NumericToken((decimal)rectangle.TopRight.Y)
			});
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000049BC File Offset: 0x00002BBC
		private IndirectReferenceToken[] CreateBookmarkTree(IReadOnlyList<BookmarkNode> nodes, Dictionary<int, IndirectReferenceToken> pageReferences, IndirectReferenceToken parent)
		{
			IndirectReferenceToken[] array = new IndirectReferenceToken[nodes.Count];
			for (int i = 0; i < nodes.Count; i++)
			{
				array[i] = this.context.ReserveObjectNumber();
			}
			for (int j = 0; j < nodes.Count; j++)
			{
				BookmarkNode bookmarkNode = nodes[j];
				IndirectReferenceToken indirectReferenceToken = array[j];
				Dictionary<NameToken, IToken> dictionary = new Dictionary<NameToken, IToken>();
				dictionary.Add(NameToken.Title, new StringToken(bookmarkNode.Title, 0));
				dictionary.Add(NameToken.Count, new NumericToken(bookmarkNode.Children.Count));
				Dictionary<NameToken, IToken> dictionary2 = dictionary;
				if (parent != null)
				{
					dictionary2[NameToken.Parent] = parent;
				}
				if (j > 0)
				{
					dictionary2[NameToken.Prev] = array[j - 1];
				}
				if (j < array.Length - 1)
				{
					dictionary2[NameToken.Next] = array[j + 1];
				}
				if (bookmarkNode.Children.Count > 0)
				{
					IndirectReferenceToken[] array2 = this.CreateBookmarkTree(bookmarkNode.Children, pageReferences, indirectReferenceToken);
					dictionary2[NameToken.First] = array2[0];
					dictionary2[NameToken.Last] = array2[array2.Length - 1];
				}
				DocumentBookmarkNode documentBookmarkNode = bookmarkNode as DocumentBookmarkNode;
				if (documentBookmarkNode == null)
				{
					UriBookmarkNode uriBookmarkNode = bookmarkNode as UriBookmarkNode;
					if (uriBookmarkNode == null)
					{
						throw new NotSupportedException(bookmarkNode.GetType().Name + " is not a supported bookmark node type.");
					}
					Dictionary<NameToken, IToken> dictionary3 = dictionary2;
					NameToken a = NameToken.A;
					Dictionary<NameToken, IToken> dictionary4 = new Dictionary<NameToken, IToken>();
					NameToken s = NameToken.S;
					dictionary4[s] = NameToken.Uri;
					NameToken uri = NameToken.Uri;
					dictionary4[uri] = new StringToken(uriBookmarkNode.Uri, 0);
					dictionary3[a] = new DictionaryToken(dictionary4);
				}
				else
				{
					dictionary2[NameToken.Dest] = PdfDocumentBuilder.CreateExplicitDestinationToken(documentBookmarkNode.Destination, pageReferences);
				}
				this.context.WriteToken(new DictionaryToken(dictionary2), indirectReferenceToken);
			}
			return array;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004B88 File Offset: 0x00002D88
		private static ArrayToken CreateExplicitDestinationToken(ExplicitDestination destination, Dictionary<int, IndirectReferenceToken> pageReferences)
		{
			IndirectReferenceToken indirectReferenceToken;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (!pageReferences.TryGetValue(destination.PageNumber, ref indirectReferenceToken))
			{
				defaultInterpolatedStringHandler..ctor(43, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Page ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(destination.PageNumber);
				defaultInterpolatedStringHandler.AppendLiteral(" was not found in the source document.");
				throw new KeyNotFoundException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			switch (destination.Type)
			{
			case ExplicitDestinationType.XyzCoordinates:
				return new ArrayToken(new IToken[]
				{
					indirectReferenceToken,
					NameToken.XYZ,
					new NumericToken(destination.Coordinates.Left.GetValueOrDefault()),
					new NumericToken(destination.Coordinates.Top.GetValueOrDefault())
				});
			case ExplicitDestinationType.FitPage:
				return new ArrayToken(new IToken[]
				{
					indirectReferenceToken,
					NameToken.Fit
				});
			case ExplicitDestinationType.FitHorizontally:
				return new ArrayToken(new IToken[]
				{
					indirectReferenceToken,
					NameToken.FitH,
					new NumericToken(destination.Coordinates.Top.GetValueOrDefault())
				});
			case ExplicitDestinationType.FitVertically:
				return new ArrayToken(new IToken[]
				{
					indirectReferenceToken,
					NameToken.FitV,
					new NumericToken(destination.Coordinates.Left.GetValueOrDefault())
				});
			case ExplicitDestinationType.FitRectangle:
				return new ArrayToken(new IToken[]
				{
					indirectReferenceToken,
					NameToken.FitR,
					new NumericToken(destination.Coordinates.Left.GetValueOrDefault()),
					new NumericToken(destination.Coordinates.Top.GetValueOrDefault()),
					new NumericToken(destination.Coordinates.Right.GetValueOrDefault()),
					new NumericToken(destination.Coordinates.Bottom.GetValueOrDefault())
				});
			case ExplicitDestinationType.FitBoundingBox:
				return new ArrayToken(new IToken[]
				{
					indirectReferenceToken,
					NameToken.FitB
				});
			case ExplicitDestinationType.FitBoundingBoxHorizontally:
				return new ArrayToken(new IToken[]
				{
					indirectReferenceToken,
					NameToken.FitBH,
					new NumericToken(destination.Coordinates.Left.GetValueOrDefault())
				});
			case ExplicitDestinationType.FitBoundingBoxVertically:
				return new ArrayToken(new IToken[]
				{
					indirectReferenceToken,
					NameToken.FitBV,
					new NumericToken(destination.Coordinates.Left.GetValueOrDefault())
				});
			default:
				defaultInterpolatedStringHandler..ctor(46, 1);
				defaultInterpolatedStringHandler.AppendFormatted<ExplicitDestinationType>(destination.Type);
				defaultInterpolatedStringHandler.AppendLiteral(" is not a supported bookmark destination type.");
				throw new NotSupportedException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004E10 File Offset: 0x00003010
		private static DictionaryToken CreateLinkAnnotationToken(DictionaryToken token, PdfAction action, Dictionary<int, IndirectReferenceToken> pageReferences)
		{
			Dictionary<NameToken, IToken> dictionary = new Dictionary<NameToken, IToken>();
			foreach (KeyValuePair<string, IToken> keyValuePair in token.Data)
			{
				NameToken nameToken = NameToken.Create(keyValuePair.Key);
				if (!(nameToken == NameToken.A) && !(nameToken == NameToken.Dest))
				{
					dictionary[nameToken] = keyValuePair.Value;
				}
			}
			dictionary[NameToken.A] = PdfDocumentBuilder.CreateActionToken(action, pageReferences);
			return new DictionaryToken(dictionary);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004EAC File Offset: 0x000030AC
		private static DictionaryToken CreateActionToken(PdfAction action, Dictionary<int, IndirectReferenceToken> pageReferences)
		{
			UriAction uriAction = action as UriAction;
			NameToken nameToken;
			NameToken nameToken2;
			if (uriAction != null)
			{
				Dictionary<NameToken, IToken> dictionary = new Dictionary<NameToken, IToken>();
				nameToken = NameToken.S;
				dictionary[nameToken] = NameToken.Uri;
				nameToken2 = NameToken.Uri;
				dictionary[nameToken2] = new StringToken(uriAction.Uri, 0);
				return new DictionaryToken(dictionary);
			}
			GoToAction goToAction = action as GoToAction;
			if (goToAction != null)
			{
				Dictionary<NameToken, IToken> dictionary2 = new Dictionary<NameToken, IToken>();
				nameToken2 = NameToken.S;
				dictionary2[nameToken2] = NameToken.GoTo;
				nameToken = NameToken.D;
				dictionary2[nameToken] = PdfDocumentBuilder.CreateExplicitDestinationToken(goToAction.Destination, pageReferences);
				return new DictionaryToken(dictionary2);
			}
			GoToEAction goToEAction = action as GoToEAction;
			NameToken nameToken3;
			if (goToEAction != null)
			{
				Dictionary<NameToken, IToken> dictionary3 = new Dictionary<NameToken, IToken>();
				nameToken = NameToken.S;
				dictionary3[nameToken] = NameToken.GoToE;
				nameToken2 = NameToken.F;
				dictionary3[nameToken2] = new StringToken(goToEAction.FileSpecification, 0);
				nameToken3 = NameToken.D;
				dictionary3[nameToken3] = PdfDocumentBuilder.CreateExplicitDestinationToken(goToEAction.Destination, pageReferences);
				return new DictionaryToken(dictionary3);
			}
			GoToRAction goToRAction = action as GoToRAction;
			if (goToRAction == null)
			{
				throw new NotSupportedException(action.GetType().Name + " is not a supported PDF action type.");
			}
			Dictionary<NameToken, IToken> dictionary4 = new Dictionary<NameToken, IToken>();
			nameToken3 = NameToken.S;
			dictionary4[nameToken3] = NameToken.GoToR;
			nameToken2 = NameToken.F;
			dictionary4[nameToken2] = new StringToken(goToRAction.Filename, 0);
			nameToken = NameToken.D;
			dictionary4[nameToken] = PdfDocumentBuilder.CreateExplicitDestinationToken(goToRAction.Destination, pageReferences);
			return new DictionaryToken(dictionary4);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00005024 File Offset: 0x00003224
		public void Dispose()
		{
			if (!this.completed)
			{
				this.CompleteDocument();
			}
			this.context.Dispose();
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00005040 File Offset: 0x00003240
		// Note: this type is marked as 'beforefieldinit'.
		static PdfDocumentBuilder()
		{
			List<NameToken> list = new List<NameToken>();
			list.Add(NameToken.Create("PDF"));
			list.Add(NameToken.Text);
			list.Add(NameToken.ImageB);
			list.Add(NameToken.ImageC);
			list.Add(NameToken.ImageI);
			PdfDocumentBuilder.DefaultProcSet = new ArrayToken(list);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000050A4 File Offset: 0x000032A4
		[CompilerGenerated]
		private void <AddPage>g__CopyResourceDict|41_0(IToken token, Dictionary<NameToken, IToken> destinationDict, ref PdfDocumentBuilder.<>c__DisplayClass41_0 A_3)
		{
			DictionaryToken dictionaryToken = this.<AddPage>g__GetRemoteDict|41_1(token, ref A_3);
			if (dictionaryToken == null)
			{
				return;
			}
			foreach (KeyValuePair<string, IToken> keyValuePair in dictionaryToken.Data)
			{
				if (!destinationDict.ContainsKey(NameToken.Create(keyValuePair.Key)))
				{
					IndirectReferenceToken indirectReferenceToken = keyValuePair.Value as IndirectReferenceToken;
					if (indirectReferenceToken != null)
					{
						ObjectToken objectToken = A_3.document.Structure.TokenScanner.Get(indirectReferenceToken.Data);
						if (objectToken.Data is StreamToken)
						{
							destinationDict[NameToken.Create(keyValuePair.Key)] = WriterUtil.CopyToken(this.context, keyValuePair.Value, A_3.document.Structure.TokenScanner, A_3.refs, null);
						}
						else
						{
							destinationDict[NameToken.Create(keyValuePair.Key)] = WriterUtil.CopyToken(this.context, objectToken.Data, A_3.document.Structure.TokenScanner, A_3.refs, null);
						}
					}
					else
					{
						destinationDict[NameToken.Create(keyValuePair.Key)] = WriterUtil.CopyToken(this.context, keyValuePair.Value, A_3.document.Structure.TokenScanner, A_3.refs, null);
					}
				}
				else
				{
					DictionaryToken dictionaryToken2 = this.<AddPage>g__GetRemoteDict|41_1(keyValuePair.Value, ref A_3);
					if (!(destinationDict[NameToken.Create(keyValuePair.Key)] is DictionaryToken) || dictionaryToken2 == null)
					{
						IndirectReferenceToken indirectReferenceToken2 = keyValuePair.Value as IndirectReferenceToken;
						if (indirectReferenceToken2 != null)
						{
							destinationDict[NameToken.Create(keyValuePair.Key)] = WriterUtil.CopyToken(this.context, A_3.document.Structure.TokenScanner.Get(indirectReferenceToken2.Data).Data, A_3.document.Structure.TokenScanner, A_3.refs, null);
						}
						else
						{
							destinationDict[NameToken.Create(keyValuePair.Key)] = WriterUtil.CopyToken(this.context, keyValuePair.Value, A_3.document.Structure.TokenScanner, A_3.refs, null);
						}
					}
					else
					{
						foreach (KeyValuePair<string, IToken> keyValuePair2 in dictionaryToken2.Data)
						{
							destinationDict[NameToken.Create(keyValuePair2.Key)] = WriterUtil.CopyToken(this.context, keyValuePair2.Value, A_3.document.Structure.TokenScanner, A_3.refs, null);
						}
					}
				}
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000537C File Offset: 0x0000357C
		[CompilerGenerated]
		private DictionaryToken <AddPage>g__GetRemoteDict|41_1(IToken token, ref PdfDocumentBuilder.<>c__DisplayClass41_0 A_2)
		{
			DictionaryToken result = null;
			IndirectReferenceToken indirectReferenceToken = token as IndirectReferenceToken;
			if (indirectReferenceToken != null)
			{
				result = (A_2.document.Structure.TokenScanner.Get(indirectReferenceToken.Data).Data as DictionaryToken);
			}
			else
			{
				DictionaryToken dictionaryToken = token as DictionaryToken;
				if (dictionaryToken != null)
				{
					result = dictionaryToken;
				}
			}
			return result;
		}

		// Token: 0x04000040 RID: 64
		private readonly IPdfStreamWriter context;

		// Token: 0x04000041 RID: 65
		private readonly Dictionary<int, PdfPageBuilder> pages = new Dictionary<int, PdfPageBuilder>();

		// Token: 0x04000042 RID: 66
		private readonly Dictionary<Guid, PdfDocumentBuilder.FontStored> fonts = new Dictionary<Guid, PdfDocumentBuilder.FontStored>();

		// Token: 0x04000043 RID: 67
		private bool completed;

		// Token: 0x04000044 RID: 68
		private int fontId;

		// Token: 0x04000045 RID: 69
		private decimal version = 1.7m;

		// Token: 0x04000046 RID: 70
		private static readonly ArrayToken DefaultProcSet;

		// Token: 0x0400004B RID: 75
		private readonly ConditionalWeakTable<IPdfTokenScanner, Dictionary<IndirectReference, IndirectReferenceToken>> existingCopies = new ConditionalWeakTable<IPdfTokenScanner, Dictionary<IndirectReference, IndirectReferenceToken>>();

		// Token: 0x0400004C RID: 76
		private readonly ConditionalWeakTable<PdfDocument, Dictionary<int, PdfDocumentBuilder.PageInfo>> existingTrees = new ConditionalWeakTable<PdfDocument, Dictionary<int, PdfDocumentBuilder.PageInfo>>();

		// Token: 0x02000228 RID: 552
		private class PageInfo
		{
			// Token: 0x170003CF RID: 975
			// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x0003FA4A File Offset: 0x0003DC4A
			// (set) Token: 0x06000CD8 RID: 3288 RVA: 0x0003FA52 File Offset: 0x0003DC52
			public DictionaryToken Page { get; set; }

			// Token: 0x170003D0 RID: 976
			// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x0003FA5B File Offset: 0x0003DC5B
			// (set) Token: 0x06000CDA RID: 3290 RVA: 0x0003FA63 File Offset: 0x0003DC63
			public IReadOnlyList<DictionaryToken> Parents { get; set; }
		}

		// Token: 0x02000229 RID: 553
		internal class FontStored
		{
			// Token: 0x170003D1 RID: 977
			// (get) Token: 0x06000CDC RID: 3292 RVA: 0x0003FA74 File Offset: 0x0003DC74
			[NotNull]
			public PdfDocumentBuilder.AddedFont FontKey { get; }

			// Token: 0x170003D2 RID: 978
			// (get) Token: 0x06000CDD RID: 3293 RVA: 0x0003FA7C File Offset: 0x0003DC7C
			[NotNull]
			public IWritingFont FontProgram { get; }

			// Token: 0x06000CDE RID: 3294 RVA: 0x0003FA84 File Offset: 0x0003DC84
			public FontStored(PdfDocumentBuilder.AddedFont fontKey, IWritingFont fontProgram)
			{
				if (fontKey == null)
				{
					throw new ArgumentNullException("fontKey");
				}
				this.FontKey = fontKey;
				if (fontProgram == null)
				{
					throw new ArgumentNullException("fontProgram");
				}
				this.FontProgram = fontProgram;
			}
		}

		// Token: 0x0200022A RID: 554
		public class AddedFont
		{
			// Token: 0x170003D3 RID: 979
			// (get) Token: 0x06000CDF RID: 3295 RVA: 0x0003FAB8 File Offset: 0x0003DCB8
			internal Guid Id { get; }

			// Token: 0x170003D4 RID: 980
			// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x0003FAC0 File Offset: 0x0003DCC0
			internal IndirectReferenceToken Reference { get; }

			// Token: 0x06000CE1 RID: 3297 RVA: 0x0003FAC8 File Offset: 0x0003DCC8
			internal AddedFont(Guid id, IndirectReferenceToken reference)
			{
				this.Id = id;
				this.Reference = reference;
			}
		}

		// Token: 0x0200022B RID: 555
		public class DocumentInformationBuilder
		{
			// Token: 0x170003D5 RID: 981
			// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x0003FADE File Offset: 0x0003DCDE
			// (set) Token: 0x06000CE3 RID: 3299 RVA: 0x0003FAE6 File Offset: 0x0003DCE6
			public string Title { get; set; }

			// Token: 0x170003D6 RID: 982
			// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x0003FAEF File Offset: 0x0003DCEF
			// (set) Token: 0x06000CE5 RID: 3301 RVA: 0x0003FAF7 File Offset: 0x0003DCF7
			public string Author { get; set; }

			// Token: 0x170003D7 RID: 983
			// (get) Token: 0x06000CE6 RID: 3302 RVA: 0x0003FB00 File Offset: 0x0003DD00
			// (set) Token: 0x06000CE7 RID: 3303 RVA: 0x0003FB08 File Offset: 0x0003DD08
			public string Subject { get; set; }

			// Token: 0x170003D8 RID: 984
			// (get) Token: 0x06000CE8 RID: 3304 RVA: 0x0003FB11 File Offset: 0x0003DD11
			// (set) Token: 0x06000CE9 RID: 3305 RVA: 0x0003FB19 File Offset: 0x0003DD19
			public string Keywords { get; set; }

			// Token: 0x170003D9 RID: 985
			// (get) Token: 0x06000CEA RID: 3306 RVA: 0x0003FB22 File Offset: 0x0003DD22
			// (set) Token: 0x06000CEB RID: 3307 RVA: 0x0003FB2A File Offset: 0x0003DD2A
			public string Creator { get; set; }

			// Token: 0x170003DA RID: 986
			// (get) Token: 0x06000CEC RID: 3308 RVA: 0x0003FB33 File Offset: 0x0003DD33
			// (set) Token: 0x06000CED RID: 3309 RVA: 0x0003FB3B File Offset: 0x0003DD3B
			public string Producer { get; set; } = "PdfPig";

			// Token: 0x170003DB RID: 987
			// (get) Token: 0x06000CEE RID: 3310 RVA: 0x0003FB44 File Offset: 0x0003DD44
			// (set) Token: 0x06000CEF RID: 3311 RVA: 0x0003FB4C File Offset: 0x0003DD4C
			public string CreationDate { get; set; }

			// Token: 0x170003DC RID: 988
			// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x0003FB55 File Offset: 0x0003DD55
			// (set) Token: 0x06000CF1 RID: 3313 RVA: 0x0003FB5D File Offset: 0x0003DD5D
			public string ModifiedDate { get; set; }

			// Token: 0x06000CF2 RID: 3314 RVA: 0x0003FB68 File Offset: 0x0003DD68
			internal Dictionary<NameToken, IToken> ToDictionary()
			{
				Dictionary<NameToken, IToken> dictionary = new Dictionary<NameToken, IToken>();
				if (this.Title != null)
				{
					dictionary[NameToken.Title] = new StringToken(this.Title, 0);
				}
				if (this.Author != null)
				{
					dictionary[NameToken.Author] = new StringToken(this.Author, 0);
				}
				if (this.Subject != null)
				{
					dictionary[NameToken.Subject] = new StringToken(this.Subject, 0);
				}
				if (this.Keywords != null)
				{
					dictionary[NameToken.Keywords] = new StringToken(this.Keywords, 0);
				}
				if (this.Creator != null)
				{
					dictionary[NameToken.Creator] = new StringToken(this.Creator, 0);
				}
				if (this.Producer != null)
				{
					dictionary[NameToken.Producer] = new StringToken(this.Producer, 0);
				}
				if (this.CreationDate != null)
				{
					dictionary[NameToken.CreationDate] = new StringToken(this.CreationDate, 0);
				}
				if (this.ModifiedDate != null)
				{
					dictionary[NameToken.ModDate] = new StringToken(this.ModifiedDate, 0);
				}
				return dictionary;
			}
		}
	}
}
