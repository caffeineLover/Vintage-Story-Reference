using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Graphics.Colors;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.PdfFonts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001F5 RID: 501
	internal class ResourceStore : IResourceStore
	{
		// Token: 0x06000C1D RID: 3101 RVA: 0x0003BE58 File Offset: 0x0003A058
		public ResourceStore(IPdfTokenScanner scanner, IFontFactory fontFactory, ILookupFilterProvider filterProvider)
		{
			this.scanner = scanner;
			this.fontFactory = fontFactory;
			this.filterProvider = filterProvider;
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x0003BEE4 File Offset: 0x0003A0E4
		public void LoadResourceDictionary(DictionaryToken resourceDictionary, InternalParsingOptions parsingOptions)
		{
			this.lastLoadedFont = new ValueTuple<NameToken, IFont>(null, null);
			this.loadedNamedColorSpaceDetails.Clear();
			this.namedColorSpaces.Push();
			this.currentResourceState.Push();
			IToken token;
			if (resourceDictionary.TryGet(NameToken.Font, ref token))
			{
				DictionaryToken fontDictionary = DirectObjectFinder.Get<DictionaryToken>(token, this.scanner);
				this.LoadFontDictionary(fontDictionary, parsingOptions);
			}
			IToken token2;
			if (resourceDictionary.TryGet(NameToken.Xobject, ref token2))
			{
				foreach (KeyValuePair<string, IToken> keyValuePair in DirectObjectFinder.Get<DictionaryToken>(token2, this.scanner).Data)
				{
					if (!(keyValuePair.Value is NullToken))
					{
						IndirectReferenceToken indirectReferenceToken = keyValuePair.Value as IndirectReferenceToken;
						if (indirectReferenceToken == null)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(91, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Expected the XObject dictionary value for key /");
							defaultInterpolatedStringHandler.AppendFormatted(keyValuePair.Key);
							defaultInterpolatedStringHandler.AppendLiteral(" to be an indirect reference, instead got: ");
							defaultInterpolatedStringHandler.AppendFormatted<IToken>(keyValuePair.Value);
							defaultInterpolatedStringHandler.AppendLiteral(".");
							throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						this.currentResourceState[NameToken.Create(keyValuePair.Key)] = indirectReferenceToken.Data;
					}
				}
			}
			DictionaryToken dictionaryToken;
			if (resourceDictionary.TryGet(NameToken.ExtGState, this.scanner, out dictionaryToken))
			{
				foreach (KeyValuePair<string, IToken> keyValuePair2 in dictionaryToken.Data)
				{
					NameToken nameToken = NameToken.Create(keyValuePair2.Key);
					DictionaryToken dictionaryToken2 = DirectObjectFinder.Get<DictionaryToken>(keyValuePair2.Value, this.scanner);
					this.extendedGraphicsStates[nameToken] = dictionaryToken2;
				}
			}
			DictionaryToken dictionaryToken3;
			if (resourceDictionary.TryGet(NameToken.ColorSpace, this.scanner, out dictionaryToken3))
			{
				foreach (KeyValuePair<string, IToken> keyValuePair3 in dictionaryToken3.Data)
				{
					NameToken nameToken2 = NameToken.Create(keyValuePair3.Key);
					NameToken name;
					if (DirectObjectFinder.TryGet<NameToken>(keyValuePair3.Value, this.scanner, out name))
					{
						this.namedColorSpaces[nameToken2] = new ResourceColorSpace(name);
					}
					else
					{
						ArrayToken arrayToken;
						if (!DirectObjectFinder.TryGet<ArrayToken>(keyValuePair3.Value, this.scanner, out arrayToken))
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(67, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Invalid ColorSpace token encountered in page resource dictionary: ");
							defaultInterpolatedStringHandler.AppendFormatted<IToken>(keyValuePair3.Value);
							defaultInterpolatedStringHandler.AppendLiteral(".");
							throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						if (arrayToken.Length == 0)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(65, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Empty ColorSpace array encountered in page resource dictionary: ");
							defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(resourceDictionary);
							defaultInterpolatedStringHandler.AppendLiteral(".");
							throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						NameToken nameToken3 = arrayToken.Data[0] as NameToken;
						if (nameToken3 == null)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(67, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Invalid ColorSpace array encountered in page resource dictionary: ");
							defaultInterpolatedStringHandler.AppendFormatted<ArrayToken>(arrayToken);
							defaultInterpolatedStringHandler.AppendLiteral(".");
							throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						this.namedColorSpaces[nameToken2] = new ResourceColorSpace(nameToken3, arrayToken);
					}
				}
			}
			DictionaryToken dictionaryToken4;
			if (resourceDictionary.TryGet(NameToken.Pattern, this.scanner, out dictionaryToken4))
			{
				foreach (KeyValuePair<string, IToken> keyValuePair4 in dictionaryToken4.Data)
				{
					NameToken nameToken4 = NameToken.Create(keyValuePair4.Key);
					this.patternsProperties[nameToken4] = PatternParser.Create(keyValuePair4.Value, this.scanner, this, this.filterProvider);
				}
			}
			DictionaryToken dictionaryToken5;
			if (resourceDictionary.TryGet(NameToken.Properties, this.scanner, out dictionaryToken5))
			{
				foreach (KeyValuePair<string, IToken> keyValuePair5 in dictionaryToken5.Data)
				{
					NameToken nameToken5 = NameToken.Create(keyValuePair5.Key);
					DictionaryToken dictionaryToken6;
					if (DirectObjectFinder.TryGet<DictionaryToken>(keyValuePair5.Value, this.scanner, out dictionaryToken6))
					{
						this.markedContentProperties[nameToken5] = dictionaryToken6;
					}
				}
			}
			DictionaryToken dictionaryToken7;
			if (resourceDictionary.TryGet(NameToken.Shading, this.scanner, out dictionaryToken7))
			{
				foreach (KeyValuePair<string, IToken> keyValuePair6 in dictionaryToken7.Data)
				{
					NameToken nameToken6 = NameToken.Create(keyValuePair6.Key);
					DictionaryToken shading;
					if (DirectObjectFinder.TryGet<DictionaryToken>(keyValuePair6.Value, this.scanner, out shading))
					{
						this.shadingsProperties[nameToken6] = ShadingParser.Create(shading, this.scanner, this, this.filterProvider);
					}
					else
					{
						StreamToken shading2;
						if (!DirectObjectFinder.TryGet<StreamToken>(keyValuePair6.Value, this.scanner, out shading2))
						{
							throw new NotImplementedException("Shading");
						}
						this.shadingsProperties[nameToken6] = ShadingParser.Create(shading2, this.scanner, this, this.filterProvider);
					}
				}
			}
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x0003C48C File Offset: 0x0003A68C
		public void UnloadResourceDictionary()
		{
			this.lastLoadedFont = new ValueTuple<NameToken, IFont>(null, null);
			this.loadedNamedColorSpaceDetails.Clear();
			this.currentResourceState.Pop();
			this.namedColorSpaces.Pop();
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x0003C4BC File Offset: 0x0003A6BC
		private void LoadFontDictionary(DictionaryToken fontDictionary, InternalParsingOptions parsingOptions)
		{
			this.lastLoadedFont = new ValueTuple<NameToken, IFont>(null, null);
			foreach (KeyValuePair<string, IToken> keyValuePair in fontDictionary.Data)
			{
				IndirectReferenceToken indirectReferenceToken = keyValuePair.Value as IndirectReferenceToken;
				if (indirectReferenceToken != null)
				{
					IndirectReference data = indirectReferenceToken.Data;
					this.currentResourceState[NameToken.Create(keyValuePair.Key)] = data;
					if (this.loadedFonts.ContainsKey(data))
					{
						continue;
					}
					DictionaryToken dictionaryToken = DirectObjectFinder.Get<DictionaryToken>(indirectReferenceToken, this.scanner);
					if (dictionaryToken == null)
					{
						continue;
					}
					try
					{
						this.loadedFonts[data] = this.fontFactory.Get(dictionaryToken);
						continue;
					}
					catch
					{
						if (!parsingOptions.SkipMissingFonts)
						{
							throw;
						}
						continue;
					}
				}
				DictionaryToken dictionaryToken2 = keyValuePair.Value as DictionaryToken;
				if (dictionaryToken2 != null)
				{
					this.loadedDirectFonts[NameToken.Create(keyValuePair.Key)] = this.fontFactory.Get(dictionaryToken2);
				}
			}
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x0003C5D0 File Offset: 0x0003A7D0
		public IFont GetFont(NameToken name)
		{
			if (this.lastLoadedFont.Item1 == name)
			{
				return this.lastLoadedFont.Item2;
			}
			IndirectReference indirectReference;
			IFont font;
			if (this.currentResourceState.TryGetValue(name, ref indirectReference))
			{
				this.loadedFonts.TryGetValue(indirectReference, ref font);
			}
			else if (!this.loadedDirectFonts.TryGetValue(name, ref font))
			{
				return null;
			}
			this.lastLoadedFont = new ValueTuple<NameToken, IFont>(name, font);
			return font;
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x0003C640 File Offset: 0x0003A840
		public IFont GetFontDirectly(IndirectReferenceToken fontReferenceToken)
		{
			this.lastLoadedFont = new ValueTuple<NameToken, IFont>(null, null);
			DictionaryToken dictionary;
			if (!DirectObjectFinder.TryGet<DictionaryToken>(fontReferenceToken, this.scanner, out dictionary))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(50, 1);
				defaultInterpolatedStringHandler.AppendLiteral("The requested font reference token ");
				defaultInterpolatedStringHandler.AppendFormatted<IndirectReferenceToken>(fontReferenceToken);
				defaultInterpolatedStringHandler.AppendLiteral(" wasn't a font.");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return this.fontFactory.Get(dictionary);
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x0003C6B0 File Offset: 0x0003A8B0
		public bool TryGetNamedColorSpace(NameToken name, out ResourceColorSpace namedToken)
		{
			namedToken = default(ResourceColorSpace);
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			ResourceColorSpace resourceColorSpace;
			if (!this.namedColorSpaces.TryGetValue(name, ref resourceColorSpace))
			{
				return false;
			}
			namedToken = resourceColorSpace;
			return true;
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x0003C6F4 File Offset: 0x0003A8F4
		public ColorSpaceDetails GetColorSpaceDetails(NameToken name, DictionaryToken dictionary)
		{
			if (dictionary == null)
			{
				dictionary = new DictionaryToken(new Dictionary<NameToken, IToken>());
			}
			if (name == null)
			{
				return ColorSpaceDetailsParser.GetColorSpaceDetails(default(ColorSpace?), dictionary, this.scanner, this, this.filterProvider, false);
			}
			ColorSpace colorSpace;
			if (name.TryMapToColorSpace(out colorSpace))
			{
				return ColorSpaceDetailsParser.GetColorSpaceDetails(new ColorSpace?(colorSpace), dictionary, this.scanner, this, this.filterProvider, false);
			}
			ColorSpaceDetails result;
			if (this.loadedNamedColorSpaceDetails.TryGetValue(name, ref result))
			{
				return result;
			}
			ResourceColorSpace resourceColorSpace;
			ColorSpace colorSpace2;
			if (this.TryGetNamedColorSpace(name, out resourceColorSpace) && resourceColorSpace.Name.TryMapToColorSpace(out colorSpace2))
			{
				if (resourceColorSpace.Data == null)
				{
					return ColorSpaceDetailsParser.GetColorSpaceDetails(new ColorSpace?(colorSpace2), dictionary, this.scanner, this, this.filterProvider, false);
				}
				ArrayToken arrayToken = resourceColorSpace.Data as ArrayToken;
				if (arrayToken != null)
				{
					ColorSpaceDetails colorSpaceDetails = ColorSpaceDetailsParser.GetColorSpaceDetails(new ColorSpace?(colorSpace2), dictionary.With(NameToken.ColorSpace, arrayToken), this.scanner, this, this.filterProvider, false);
					this.loadedNamedColorSpaceDetails[name] = colorSpaceDetails;
					return colorSpaceDetails;
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(40, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Could not find color space for token '");
			defaultInterpolatedStringHandler.AppendFormatted<NameToken>(name);
			defaultInterpolatedStringHandler.AppendLiteral("'.");
			throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x0003C82C File Offset: 0x0003AA2C
		public bool TryGetXObject(NameToken name, out StreamToken stream)
		{
			stream = null;
			IndirectReference indirectReference;
			return this.currentResourceState.TryGetValue(name, ref indirectReference) && DirectObjectFinder.TryGet<StreamToken>(new IndirectReferenceToken(indirectReference), this.scanner, out stream);
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x0003C860 File Offset: 0x0003AA60
		public DictionaryToken GetExtendedGraphicsStateDictionary(NameToken name)
		{
			return this.extendedGraphicsStates[name];
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x0003C870 File Offset: 0x0003AA70
		public DictionaryToken GetMarkedContentPropertiesDictionary(NameToken name)
		{
			DictionaryToken result;
			if (!this.markedContentProperties.TryGetValue(name, ref result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x0003C890 File Offset: 0x0003AA90
		public Shading GetShading(NameToken name)
		{
			return this.shadingsProperties[name];
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x0003C89E File Offset: 0x0003AA9E
		public IReadOnlyDictionary<NameToken, PatternColor> GetPatterns()
		{
			return this.patternsProperties;
		}

		// Token: 0x04000664 RID: 1636
		private readonly IPdfTokenScanner scanner;

		// Token: 0x04000665 RID: 1637
		private readonly IFontFactory fontFactory;

		// Token: 0x04000666 RID: 1638
		private readonly ILookupFilterProvider filterProvider;

		// Token: 0x04000667 RID: 1639
		private readonly Dictionary<IndirectReference, IFont> loadedFonts = new Dictionary<IndirectReference, IFont>();

		// Token: 0x04000668 RID: 1640
		private readonly Dictionary<NameToken, IFont> loadedDirectFonts = new Dictionary<NameToken, IFont>();

		// Token: 0x04000669 RID: 1641
		private readonly StackDictionary<NameToken, IndirectReference> currentResourceState = new StackDictionary<NameToken, IndirectReference>();

		// Token: 0x0400066A RID: 1642
		private readonly Dictionary<NameToken, DictionaryToken> extendedGraphicsStates = new Dictionary<NameToken, DictionaryToken>();

		// Token: 0x0400066B RID: 1643
		private readonly StackDictionary<NameToken, ResourceColorSpace> namedColorSpaces = new StackDictionary<NameToken, ResourceColorSpace>();

		// Token: 0x0400066C RID: 1644
		private readonly Dictionary<NameToken, ColorSpaceDetails> loadedNamedColorSpaceDetails = new Dictionary<NameToken, ColorSpaceDetails>();

		// Token: 0x0400066D RID: 1645
		private readonly Dictionary<NameToken, DictionaryToken> markedContentProperties = new Dictionary<NameToken, DictionaryToken>();

		// Token: 0x0400066E RID: 1646
		private readonly Dictionary<NameToken, Shading> shadingsProperties = new Dictionary<NameToken, Shading>();

		// Token: 0x0400066F RID: 1647
		private readonly Dictionary<NameToken, PatternColor> patternsProperties = new Dictionary<NameToken, PatternColor>();

		// Token: 0x04000670 RID: 1648
		[TupleElementNames(new string[]
		{
			"name",
			"font"
		})]
		private ValueTuple<NameToken, IFont> lastLoadedFont;
	}
}
