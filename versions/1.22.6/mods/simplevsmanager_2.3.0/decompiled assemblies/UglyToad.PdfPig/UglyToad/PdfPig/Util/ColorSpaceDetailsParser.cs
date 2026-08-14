using System;
using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Functions;
using UglyToad.PdfPig.Graphics.Colors;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Util
{
	// Token: 0x02000028 RID: 40
	internal static class ColorSpaceDetailsParser
	{
		// Token: 0x06000115 RID: 277 RVA: 0x0000A6AC File Offset: 0x000088AC
		public static ColorSpaceDetails GetColorSpaceDetails(ColorSpace? colorSpace, DictionaryToken imageDictionary, IPdfTokenScanner scanner, IResourceStore resourceStore, ILookupFilterProvider filterProvider, bool cannotRecurse = false)
		{
			if (imageDictionary.GetObjectOrDefault(NameToken.ImageMask, NameToken.Im) != null || Enumerable.Any<CcittFaxDecodeFilter>(Enumerable.OfType<CcittFaxDecodeFilter>(filterProvider.GetFilters(imageDictionary, scanner))))
			{
				if (cannotRecurse)
				{
					return DeviceGrayColorSpaceDetails.Instance;
				}
				ColorSpaceDetails colorSpaceDetails = ColorSpaceDetailsParser.GetColorSpaceDetails(colorSpace, imageDictionary.Without(NameToken.Filter).Without(NameToken.F), scanner, resourceStore, filterProvider, true);
				double[] decode = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(((imageDictionary.GetObjectOrDefault(NameToken.Decode, NameToken.D) as ArrayToken) ?? new ArrayToken(EmptyArray<IToken>.Instance)).Data), (NumericToken x) => x.Double));
				return IndexedColorSpaceDetails.Stencil(colorSpaceDetails, decode);
			}
			else
			{
				if (colorSpace == null)
				{
					return UnsupportedColorSpaceDetails.Instance;
				}
				switch (colorSpace.Value)
				{
				case ColorSpace.DeviceGray:
					return DeviceGrayColorSpaceDetails.Instance;
				case ColorSpace.DeviceRGB:
					return DeviceRgbColorSpaceDetails.Instance;
				case ColorSpace.DeviceCMYK:
					return DeviceCmykColorSpaceDetails.Instance;
				case ColorSpace.CalGray:
				{
					ArrayToken arrayToken;
					if (!ColorSpaceDetailsParser.TryGetColorSpaceArray(imageDictionary, resourceStore, scanner, out arrayToken) || arrayToken.Length != 2)
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					NameToken nameToken = arrayToken[0] as NameToken;
					ColorSpace colorSpace2;
					if (nameToken == null || !ColorSpaceMapper.TryMap(nameToken, resourceStore, out colorSpace2) || colorSpace2 != ColorSpace.CalGray)
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					DictionaryToken dictionary;
					ArrayToken arrayToken2;
					if (!DirectObjectFinder.TryGet<DictionaryToken>(arrayToken[1], scanner, out dictionary) || !dictionary.TryGet(NameToken.WhitePoint, scanner, out arrayToken2))
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					IReadOnlyList<double> whitePoint = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(arrayToken2.Data), (NumericToken x) => x.Double));
					IReadOnlyList<double> blackPoint = null;
					ArrayToken arrayToken3;
					if (dictionary.TryGet(NameToken.BlackPoint, scanner, out arrayToken3))
					{
						blackPoint = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(arrayToken3.Data), (NumericToken x) => x.Double));
					}
					double? gamma = default(double?);
					NumericToken numericToken;
					if (dictionary.TryGet(NameToken.Gamma, scanner, out numericToken))
					{
						gamma = new double?(numericToken.Double);
					}
					return new CalGrayColorSpaceDetails(whitePoint, blackPoint, gamma);
				}
				case ColorSpace.CalRGB:
				{
					ArrayToken arrayToken4;
					if (!ColorSpaceDetailsParser.TryGetColorSpaceArray(imageDictionary, resourceStore, scanner, out arrayToken4) || arrayToken4.Length != 2)
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					NameToken nameToken2 = arrayToken4[0] as NameToken;
					ColorSpace colorSpace3;
					if (nameToken2 == null || !ColorSpaceMapper.TryMap(nameToken2, resourceStore, out colorSpace3) || colorSpace3 != ColorSpace.CalRGB)
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					DictionaryToken dictionary2;
					ArrayToken arrayToken5;
					if (!DirectObjectFinder.TryGet<DictionaryToken>(arrayToken4[1], scanner, out dictionary2) || !dictionary2.TryGet(NameToken.WhitePoint, scanner, out arrayToken5))
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					IReadOnlyList<double> whitePoint2 = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(arrayToken5.Data), (NumericToken x) => x.Double));
					IReadOnlyList<double> blackPoint2 = null;
					ArrayToken arrayToken6;
					if (dictionary2.TryGet(NameToken.BlackPoint, scanner, out arrayToken6))
					{
						blackPoint2 = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(arrayToken6.Data), (NumericToken x) => x.Double));
					}
					IReadOnlyList<double> gamma2 = null;
					ArrayToken arrayToken7;
					if (dictionary2.TryGet(NameToken.Gamma, scanner, out arrayToken7))
					{
						gamma2 = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(arrayToken7.Data), (NumericToken x) => x.Double));
					}
					IReadOnlyList<double> matrix = null;
					ArrayToken arrayToken8;
					if (dictionary2.TryGet(NameToken.Matrix, scanner, out arrayToken8))
					{
						matrix = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(arrayToken8.Data), (NumericToken x) => x.Double));
					}
					return new CalRGBColorSpaceDetails(whitePoint2, blackPoint2, gamma2, matrix);
				}
				case ColorSpace.Lab:
				{
					ArrayToken arrayToken9;
					if (!ColorSpaceDetailsParser.TryGetColorSpaceArray(imageDictionary, resourceStore, scanner, out arrayToken9) || arrayToken9.Length != 2)
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					NameToken nameToken3 = arrayToken9[0] as NameToken;
					ColorSpace colorSpace4;
					if (nameToken3 == null || !ColorSpaceMapper.TryMap(nameToken3, resourceStore, out colorSpace4) || colorSpace4 != ColorSpace.Lab)
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					DictionaryToken dictionary3;
					ArrayToken arrayToken10;
					if (!DirectObjectFinder.TryGet<DictionaryToken>(arrayToken9[1], scanner, out dictionary3) || !dictionary3.TryGet(NameToken.WhitePoint, scanner, out arrayToken10))
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					IReadOnlyList<double> whitePoint3 = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(arrayToken10.Data), (NumericToken x) => x.Double));
					IReadOnlyList<double> blackPoint3 = null;
					ArrayToken arrayToken11;
					if (dictionary3.TryGet(NameToken.BlackPoint, scanner, out arrayToken11))
					{
						blackPoint3 = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(arrayToken11.Data), (NumericToken x) => x.Double));
					}
					IReadOnlyList<double> matrix2 = null;
					ArrayToken arrayToken12;
					if (dictionary3.TryGet(NameToken.Matrix, scanner, out arrayToken12))
					{
						matrix2 = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(arrayToken12.Data), (NumericToken x) => x.Double));
					}
					return new LabColorSpaceDetails(whitePoint3, blackPoint3, matrix2);
				}
				case ColorSpace.ICCBased:
				{
					ArrayToken arrayToken13;
					if (!ColorSpaceDetailsParser.TryGetColorSpaceArray(imageDictionary, resourceStore, scanner, out arrayToken13) || arrayToken13.Length != 2)
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					NameToken nameToken4 = arrayToken13[0] as NameToken;
					ColorSpace colorSpace5;
					if (nameToken4 == null || !ColorSpaceMapper.TryMap(nameToken4, resourceStore, out colorSpace5) || colorSpace5 != ColorSpace.ICCBased)
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					StreamToken streamToken;
					NumericToken numericToken2;
					if (!DirectObjectFinder.TryGet<StreamToken>(arrayToken13[1], scanner, out streamToken) || !streamToken.StreamDictionary.TryGet(NameToken.N, scanner, out numericToken2))
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					ColorSpaceDetails alternateColorSpaceDetails = null;
					NameToken name;
					ColorSpace colorSpace6;
					if (streamToken.StreamDictionary.TryGet<NameToken>(NameToken.Alternate, ref name) && ColorSpaceMapper.TryMap(name, resourceStore, out colorSpace6))
					{
						alternateColorSpaceDetails = ColorSpaceDetailsParser.GetColorSpaceDetails(new ColorSpace?(colorSpace6), imageDictionary, scanner, resourceStore, filterProvider, true);
					}
					IReadOnlyList<double> range = null;
					ArrayToken arrayToken14;
					if (streamToken.StreamDictionary.TryGet(NameToken.Range, scanner, out arrayToken14))
					{
						range = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(arrayToken14.Data), (NumericToken x) => x.Double));
					}
					XmpMetadata metadata = null;
					StreamToken stream;
					if (streamToken.StreamDictionary.TryGet(NameToken.Metadata, scanner, out stream))
					{
						metadata = new XmpMetadata(stream, filterProvider, scanner);
					}
					return new ICCBasedColorSpaceDetails(numericToken2.Int, alternateColorSpaceDetails, range, metadata);
				}
				case ColorSpace.Indexed:
				{
					if (cannotRecurse)
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					ArrayToken arrayToken15;
					if (!ColorSpaceDetailsParser.TryGetColorSpaceArray(imageDictionary, resourceStore, scanner, out arrayToken15) || arrayToken15.Length != 4)
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					NameToken nameToken5 = arrayToken15[0] as NameToken;
					ColorSpace colorSpace7;
					if (nameToken5 == null || !ColorSpaceMapper.TryMap(nameToken5, resourceStore, out colorSpace7) || colorSpace7 != ColorSpace.Indexed)
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					IToken token = arrayToken15[1];
					NameToken name2;
					ColorSpace colorSpace8;
					ColorSpaceDetails colorSpaceDetails2;
					if (!DirectObjectFinder.TryGet<NameToken>(token, scanner, out name2) || !ColorSpaceMapper.TryMap(name2, resourceStore, out colorSpace8))
					{
						ArrayToken arrayToken16;
						if (DirectObjectFinder.TryGet<ArrayToken>(token, scanner, out arrayToken16) && arrayToken16.Length > 0)
						{
							NameToken nameToken6 = arrayToken16[0] as NameToken;
							ColorSpace colorSpace9;
							if (nameToken6 != null && ColorSpaceMapper.TryMap(nameToken6, resourceStore, out colorSpace9))
							{
								Dictionary<NameToken, IToken> dictionary4 = new Dictionary<NameToken, IToken>();
								dictionary4.Add(NameToken.ColorSpace, arrayToken16);
								DictionaryToken imageDictionary2 = new DictionaryToken(dictionary4);
								colorSpaceDetails2 = ColorSpaceDetailsParser.GetColorSpaceDetails(new ColorSpace?(colorSpace9), imageDictionary2, scanner, resourceStore, filterProvider, true);
								goto IL_746;
							}
						}
						return UnsupportedColorSpaceDetails.Instance;
					}
					colorSpaceDetails2 = ColorSpaceDetailsParser.GetColorSpaceDetails(new ColorSpace?(colorSpace8), imageDictionary, scanner, resourceStore, filterProvider, true);
					IL_746:
					if (colorSpaceDetails2 is UnsupportedColorSpaceDetails)
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					NumericToken numericToken3;
					if (!DirectObjectFinder.TryGet<NumericToken>(arrayToken15[2], scanner, out numericToken3))
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					int @int = numericToken3.Int;
					IToken token2 = arrayToken15[3];
					HexToken hexToken;
					IReadOnlyList<byte> colorTable;
					StreamToken stream2;
					if (DirectObjectFinder.TryGet<HexToken>(token2, scanner, out hexToken))
					{
						colorTable = hexToken.Bytes;
					}
					else if (DirectObjectFinder.TryGet<StreamToken>(token2, scanner, out stream2))
					{
						colorTable = stream2.Decode(filterProvider, scanner);
					}
					else
					{
						StringToken stringToken;
						if (!DirectObjectFinder.TryGet<StringToken>(token2, scanner, out stringToken))
						{
							return UnsupportedColorSpaceDetails.Instance;
						}
						colorTable = stringToken.GetBytes();
					}
					return new IndexedColorSpaceDetails(colorSpaceDetails2, (byte)@int, colorTable);
				}
				case ColorSpace.Pattern:
				{
					if (cannotRecurse)
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					ColorSpaceDetails underlyingColourSpace = UnsupportedColorSpaceDetails.Instance;
					if (imageDictionary.Data.Count > 0)
					{
						ArrayToken arrayToken17;
						if (!ColorSpaceDetailsParser.TryGetColorSpaceArray(imageDictionary, resourceStore, scanner, out arrayToken17) || (arrayToken17.Length != 1 && arrayToken17.Length != 2))
						{
							return UnsupportedColorSpaceDetails.Instance;
						}
						NameToken nameToken7;
						if (!DirectObjectFinder.TryGet<NameToken>(arrayToken17[0], scanner, out nameToken7) || !nameToken7.Equals(NameToken.Pattern))
						{
							return UnsupportedColorSpaceDetails.Instance;
						}
						NameToken name3;
						ColorSpace colorSpace10;
						if (arrayToken17.Length > 1 && DirectObjectFinder.TryGet<NameToken>(arrayToken17[1], scanner, out name3) && ColorSpaceMapper.TryMap(name3, resourceStore, out colorSpace10))
						{
							underlyingColourSpace = ColorSpaceDetailsParser.GetColorSpaceDetails(new ColorSpace?(colorSpace10), imageDictionary, scanner, resourceStore, filterProvider, true);
						}
					}
					return new PatternColorSpaceDetails(resourceStore.GetPatterns(), underlyingColourSpace);
				}
				case ColorSpace.Separation:
				{
					ArrayToken arrayToken18;
					if (!ColorSpaceDetailsParser.TryGetColorSpaceArray(imageDictionary, resourceStore, scanner, out arrayToken18) || arrayToken18.Length != 4)
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					NameToken nameToken8;
					if (!DirectObjectFinder.TryGet<NameToken>(arrayToken18[0], scanner, out nameToken8) || !nameToken8.Equals(NameToken.Separation))
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					NameToken name4;
					if (!DirectObjectFinder.TryGet<NameToken>(arrayToken18[1], scanner, out name4))
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					NameToken name5;
					ColorSpace colorSpace11;
					ColorSpaceDetails colorSpaceDetails3;
					if (!DirectObjectFinder.TryGet<NameToken>(arrayToken18[2], scanner, out name5) || !ColorSpaceMapper.TryMap(name5, resourceStore, out colorSpace11))
					{
						ArrayToken arrayToken19;
						if (DirectObjectFinder.TryGet<ArrayToken>(arrayToken18[2], scanner, out arrayToken19) && arrayToken19.Length > 0)
						{
							NameToken nameToken9 = arrayToken19[0] as NameToken;
							ColorSpace colorSpace12;
							if (nameToken9 != null && ColorSpaceMapper.TryMap(nameToken9, resourceStore, out colorSpace12))
							{
								Dictionary<NameToken, IToken> dictionary5 = new Dictionary<NameToken, IToken>();
								dictionary5.Add(NameToken.ColorSpace, arrayToken19);
								DictionaryToken imageDictionary3 = new DictionaryToken(dictionary5);
								colorSpaceDetails3 = ColorSpaceDetailsParser.GetColorSpaceDetails(new ColorSpace?(colorSpace12), imageDictionary3, scanner, resourceStore, filterProvider, true);
								goto IL_992;
							}
						}
						return UnsupportedColorSpaceDetails.Instance;
					}
					colorSpaceDetails3 = ColorSpaceDetailsParser.GetColorSpaceDetails(new ColorSpace?(colorSpace11), imageDictionary, scanner, resourceStore, filterProvider, true);
					IL_992:
					IToken token3 = arrayToken18[3];
					DictionaryToken function;
					PdfFunction tintFunction;
					if (DirectObjectFinder.TryGet<DictionaryToken>(token3, scanner, out function))
					{
						tintFunction = PdfFunctionParser.Create(function, scanner, filterProvider);
					}
					else
					{
						StreamToken function2;
						if (!DirectObjectFinder.TryGet<StreamToken>(token3, scanner, out function2))
						{
							return UnsupportedColorSpaceDetails.Instance;
						}
						tintFunction = PdfFunctionParser.Create(function2, scanner, filterProvider);
					}
					return new SeparationColorSpaceDetails(name4, colorSpaceDetails3, tintFunction);
				}
				case ColorSpace.DeviceN:
				{
					ArrayToken arrayToken20;
					if (!ColorSpaceDetailsParser.TryGetColorSpaceArray(imageDictionary, resourceStore, scanner, out arrayToken20) || (arrayToken20.Length != 4 && arrayToken20.Length != 5))
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					NameToken nameToken10;
					if (!DirectObjectFinder.TryGet<NameToken>(arrayToken20[0], scanner, out nameToken10) || !nameToken10.Equals(NameToken.Devicen))
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					ArrayToken arrayToken21;
					if (!DirectObjectFinder.TryGet<ArrayToken>(arrayToken20[1], scanner, out arrayToken21))
					{
						return UnsupportedColorSpaceDetails.Instance;
					}
					NameToken name6;
					ColorSpace colorSpace13;
					ColorSpaceDetails colorSpaceDetails4;
					if (!DirectObjectFinder.TryGet<NameToken>(arrayToken20[2], scanner, out name6) || !ColorSpaceMapper.TryMap(name6, resourceStore, out colorSpace13))
					{
						ArrayToken arrayToken22;
						if (DirectObjectFinder.TryGet<ArrayToken>(arrayToken20[2], scanner, out arrayToken22) && arrayToken22.Length > 0)
						{
							NameToken nameToken11 = arrayToken22[0] as NameToken;
							ColorSpace colorSpace14;
							if (nameToken11 != null && ColorSpaceMapper.TryMap(nameToken11, resourceStore, out colorSpace14))
							{
								Dictionary<NameToken, IToken> dictionary6 = new Dictionary<NameToken, IToken>();
								dictionary6.Add(NameToken.ColorSpace, arrayToken22);
								DictionaryToken imageDictionary4 = new DictionaryToken(dictionary6);
								colorSpaceDetails4 = ColorSpaceDetailsParser.GetColorSpaceDetails(new ColorSpace?(colorSpace14), imageDictionary4, scanner, resourceStore, filterProvider, true);
								goto IL_AEB;
							}
						}
						return UnsupportedColorSpaceDetails.Instance;
					}
					colorSpaceDetails4 = ColorSpaceDetailsParser.GetColorSpaceDetails(new ColorSpace?(colorSpace13), imageDictionary, scanner, resourceStore, filterProvider, true);
					IL_AEB:
					PdfFunction tintFunction2 = PdfFunctionParser.Create(arrayToken20[3], scanner, filterProvider);
					DictionaryToken dictionaryToken;
					if (arrayToken20.Length > 4 && DirectObjectFinder.TryGet<DictionaryToken>(arrayToken20[4], scanner, out dictionaryToken))
					{
						NameToken subtype = NameToken.Devicen;
						if (dictionaryToken.ContainsKey(NameToken.Subtype))
						{
							subtype = dictionaryToken.Get(NameToken.Subtype, scanner);
						}
						DictionaryToken colorants = null;
						if (dictionaryToken.ContainsKey(NameToken.Colorants))
						{
							colorants = dictionaryToken.Get(NameToken.Colorants, scanner);
						}
						DictionaryToken process = null;
						if (dictionaryToken.ContainsKey(NameToken.Process))
						{
							process = dictionaryToken.Get(NameToken.Process, scanner);
						}
						DictionaryToken mixingHints = null;
						if (dictionaryToken.ContainsKey(NameToken.MixingHints))
						{
							mixingHints = dictionaryToken.Get(NameToken.MixingHints, scanner);
						}
						DeviceNColorSpaceDetails.DeviceNColorSpaceAttributes deviceNColorSpaceAttributes = new DeviceNColorSpaceDetails.DeviceNColorSpaceAttributes(subtype, colorants, process, mixingHints);
						return new DeviceNColorSpaceDetails(Enumerable.ToArray<NameToken>(Enumerable.OfType<NameToken>(arrayToken21.Data)), colorSpaceDetails4, tintFunction2, new DeviceNColorSpaceDetails.DeviceNColorSpaceAttributes?(deviceNColorSpaceAttributes));
					}
					return new DeviceNColorSpaceDetails(Enumerable.ToArray<NameToken>(Enumerable.OfType<NameToken>(arrayToken21.Data)), colorSpaceDetails4, tintFunction2, default(DeviceNColorSpaceDetails.DeviceNColorSpaceAttributes?));
				}
				default:
					return UnsupportedColorSpaceDetails.Instance;
				}
			}
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000B2B8 File Offset: 0x000094B8
		private static bool TryGetColorSpaceArray(DictionaryToken imageDictionary, IResourceStore resourceStore, IPdfTokenScanner scanner, out ArrayToken colorSpaceArray)
		{
			IToken objectOrDefault = imageDictionary.GetObjectOrDefault(NameToken.ColorSpace, NameToken.Cs);
			NameToken name;
			ResourceColorSpace resourceColorSpace;
			if (!DirectObjectFinder.TryGet<ArrayToken>(objectOrDefault, scanner, out colorSpaceArray) && DirectObjectFinder.TryGet<NameToken>(objectOrDefault, scanner, out name) && resourceStore.TryGetNamedColorSpace(name, out resourceColorSpace))
			{
				colorSpaceArray = (resourceColorSpace.Data as ArrayToken);
			}
			return colorSpaceArray != null;
		}
	}
}
