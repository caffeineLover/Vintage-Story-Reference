using System;
using System.Linq;
using System.Runtime.CompilerServices;
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
	// Token: 0x02000035 RID: 53
	internal static class ShadingParser
	{
		// Token: 0x0600014E RID: 334 RVA: 0x0000CD90 File Offset: 0x0000AF90
		public static Shading Create(IToken shading, IPdfTokenScanner scanner, IResourceStore resourceStore, ILookupFilterProvider filterProvider)
		{
			DictionaryToken dictionaryToken = null;
			StreamToken streamToken = null;
			StreamToken streamToken2 = shading as StreamToken;
			if (streamToken2 != null)
			{
				dictionaryToken = streamToken2.StreamDictionary;
				streamToken = new StreamToken(streamToken2.StreamDictionary, streamToken2.Decode(filterProvider, scanner));
			}
			else
			{
				DictionaryToken dictionaryToken2 = shading as DictionaryToken;
				if (dictionaryToken2 != null)
				{
					dictionaryToken = dictionaryToken2;
				}
			}
			NumericToken numericToken;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (!dictionaryToken.TryGet(NameToken.ShadingType, scanner, out numericToken))
			{
				defaultInterpolatedStringHandler..ctor(27, 1);
				defaultInterpolatedStringHandler.AppendLiteral("'");
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.ShadingType);
				defaultInterpolatedStringHandler.AppendLiteral("' is required for shading.");
				throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (numericToken.Int >= 4 && streamToken == null)
			{
				string text = "shadingStream";
				defaultInterpolatedStringHandler..ctor(91, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Shading type '");
				defaultInterpolatedStringHandler.AppendFormatted<ShadingType>((ShadingType)numericToken.Int);
				defaultInterpolatedStringHandler.AppendLiteral("' is not properly defined. Shading types 4 to 7 shall be defined by a stream.");
				throw new ArgumentNullException(text, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			ShadingType shadingType = (ShadingType)numericToken.Int;
			NameToken name;
			ColorSpaceDetails colorSpaceDetails;
			if (dictionaryToken.TryGet(NameToken.ColorSpace, scanner, out name))
			{
				colorSpaceDetails = resourceStore.GetColorSpaceDetails(name, dictionaryToken);
			}
			else
			{
				ArrayToken arrayToken;
				if (!dictionaryToken.TryGet(NameToken.ColorSpace, scanner, out arrayToken))
				{
					defaultInterpolatedStringHandler..ctor(27, 1);
					defaultInterpolatedStringHandler.AppendLiteral("'");
					defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.ColorSpace);
					defaultInterpolatedStringHandler.AppendLiteral("' is required for shading.");
					throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				NameToken nameToken = arrayToken.Data[0] as NameToken;
				if (nameToken == null)
				{
					throw new ArgumentNullException("Invalid color space found in shading.");
				}
				colorSpaceDetails = resourceStore.GetColorSpaceDetails(nameToken, dictionaryToken);
			}
			double[] background = null;
			ArrayToken arrayToken2;
			if (dictionaryToken.TryGet(NameToken.Background, scanner, out arrayToken2))
			{
				background = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(arrayToken2.Data), (NumericToken v) => v.Double));
			}
			PdfRectangle? bbox = default(PdfRectangle?);
			ArrayToken array;
			if (dictionaryToken.TryGet(NameToken.Bbox, scanner, out array))
			{
				bbox = new PdfRectangle?(array.ToRectangle(scanner));
			}
			BooleanToken booleanToken;
			bool antiAlias = dictionaryToken.TryGet(NameToken.AntiAlias, scanner, out booleanToken) && booleanToken.Data;
			switch (shadingType)
			{
			case ShadingType.FunctionBased:
				return ShadingParser.CreateFunctionBasedShading(dictionaryToken, colorSpaceDetails, background, bbox, antiAlias, scanner, filterProvider);
			case ShadingType.Axial:
				return ShadingParser.CreateAxialShading(dictionaryToken, colorSpaceDetails, background, bbox, antiAlias, scanner, filterProvider);
			case ShadingType.Radial:
				return ShadingParser.CreateRadialShading(dictionaryToken, colorSpaceDetails, background, bbox, antiAlias, scanner, filterProvider);
			case ShadingType.FreeFormGouraud:
				return ShadingParser.CreateFreeFormGouraudShadedTriangleMeshesShading(streamToken, colorSpaceDetails, background, bbox, antiAlias, scanner, filterProvider);
			case ShadingType.LatticeFormGouraud:
				return ShadingParser.CreateLatticeFormGouraudShadedTriangleMeshesShading(streamToken, colorSpaceDetails, background, bbox, antiAlias, scanner, filterProvider);
			case ShadingType.CoonsPatch:
				return ShadingParser.CreateCoonsPatchMeshesShading(streamToken, colorSpaceDetails, background, bbox, antiAlias, scanner, filterProvider);
			case ShadingType.TensorProductPatch:
				return ShadingParser.CreateTensorProductPatchMeshesShading(streamToken, colorSpaceDetails, background, bbox, antiAlias, scanner, filterProvider);
			default:
				defaultInterpolatedStringHandler..ctor(65, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid Shading type encountered in page resource dictionary: '");
				defaultInterpolatedStringHandler.AppendFormatted<ShadingType>(shadingType);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000D078 File Offset: 0x0000B278
		private static PdfFunction[] GetFunctions(IToken functionToken, IPdfTokenScanner scanner, ILookupFilterProvider filterProvider)
		{
			ArrayToken arrayToken;
			if (DirectObjectFinder.TryGet<ArrayToken>(functionToken, scanner, out arrayToken))
			{
				PdfFunction[] array = new PdfFunction[arrayToken.Length];
				for (int i = 0; i < arrayToken.Length; i++)
				{
					array[i] = PdfFunctionParser.Create(arrayToken[i], scanner, filterProvider);
				}
				return array;
			}
			return new PdfFunction[]
			{
				PdfFunctionParser.Create(functionToken, scanner, filterProvider)
			};
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000D0D4 File Offset: 0x0000B2D4
		private static FunctionBasedShading CreateFunctionBasedShading(DictionaryToken shadingDictionary, ColorSpaceDetails colorSpace, double[] background, PdfRectangle? bbox, bool antiAlias, IPdfTokenScanner scanner, ILookupFilterProvider filterProvider)
		{
			ArrayToken arrayToken;
			double[] domain;
			if (shadingDictionary.TryGet(NameToken.Domain, scanner, out arrayToken))
			{
				domain = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(arrayToken.Data), (NumericToken v) => v.Double));
			}
			else
			{
				domain = new double[]
				{
					default(double),
					1.0,
					default(double),
					1.0
				};
			}
			ArrayToken arrayToken2;
			TransformationMatrix matrix;
			if (shadingDictionary.TryGet(NameToken.Coords, scanner, out arrayToken2))
			{
				matrix = TransformationMatrix.FromArray(Enumerable.ToArray<decimal>(Enumerable.Select<NumericToken, decimal>(Enumerable.OfType<NumericToken>(arrayToken2.Data), (NumericToken n) => n.Data)));
			}
			else
			{
				decimal[] array = new decimal[6];
				array[0] = 1m;
				array[3] = 1m;
				matrix = TransformationMatrix.FromArray(array);
			}
			if (!shadingDictionary.ContainsKey(NameToken.Function))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("'");
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.Function);
				defaultInterpolatedStringHandler.AppendLiteral("' is required for shading type '");
				defaultInterpolatedStringHandler.AppendFormatted<ShadingType>(ShadingType.FunctionBased);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			PdfFunction[] functions = ShadingParser.GetFunctions(shadingDictionary.Data[NameToken.Function], scanner, filterProvider);
			return new FunctionBasedShading(antiAlias, shadingDictionary, colorSpace, bbox, background, domain, matrix, functions);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000D248 File Offset: 0x0000B448
		private static AxialShading CreateAxialShading(DictionaryToken shadingDictionary, ColorSpaceDetails colorSpace, double[] background, PdfRectangle? bbox, bool antiAlias, IPdfTokenScanner scanner, ILookupFilterProvider filterProvider)
		{
			ArrayToken arrayToken;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (!shadingDictionary.TryGet(NameToken.Coords, scanner, out arrayToken))
			{
				defaultInterpolatedStringHandler..ctor(33, 2);
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.Coords);
				defaultInterpolatedStringHandler.AppendLiteral(" is required for shading type '");
				defaultInterpolatedStringHandler.AppendFormatted<ShadingType>(ShadingType.Axial);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			double[] coords = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(arrayToken.Data), (NumericToken v) => v.Double));
			ArrayToken arrayToken2;
			double[] domain;
			if (shadingDictionary.TryGet(NameToken.Domain, scanner, out arrayToken2))
			{
				domain = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(arrayToken2.Data), (NumericToken v) => v.Double));
			}
			else
			{
				domain = new double[]
				{
					default(double),
					1.0
				};
			}
			if (!shadingDictionary.ContainsKey(NameToken.Function))
			{
				defaultInterpolatedStringHandler..ctor(33, 2);
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.Function);
				defaultInterpolatedStringHandler.AppendLiteral(" is required for shading type '");
				defaultInterpolatedStringHandler.AppendFormatted<ShadingType>(ShadingType.Axial);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			PdfFunction[] functions = ShadingParser.GetFunctions(shadingDictionary.Data[NameToken.Function], scanner, filterProvider);
			bool[] extend = new bool[2];
			ArrayToken arrayToken3;
			if (shadingDictionary.TryGet(NameToken.Extend, scanner, out arrayToken3))
			{
				extend = Enumerable.ToArray<bool>(Enumerable.Select<BooleanToken, bool>(Enumerable.OfType<BooleanToken>(arrayToken3.Data), (BooleanToken v) => v.Data));
			}
			return new AxialShading(antiAlias, shadingDictionary, colorSpace, bbox, background, coords, domain, functions, extend);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000D410 File Offset: 0x0000B610
		private static RadialShading CreateRadialShading(DictionaryToken shadingDictionary, ColorSpaceDetails colorSpace, double[] background, PdfRectangle? bbox, bool antiAlias, IPdfTokenScanner scanner, ILookupFilterProvider filterProvider)
		{
			ArrayToken arrayToken;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (!shadingDictionary.TryGet(NameToken.Coords, scanner, out arrayToken))
			{
				defaultInterpolatedStringHandler..ctor(33, 2);
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.Coords);
				defaultInterpolatedStringHandler.AppendLiteral(" is required for shading type '");
				defaultInterpolatedStringHandler.AppendFormatted<ShadingType>(ShadingType.Radial);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			double[] coords = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(arrayToken.Data), (NumericToken v) => v.Double));
			ArrayToken arrayToken2;
			double[] domain;
			if (shadingDictionary.TryGet(NameToken.Domain, scanner, out arrayToken2))
			{
				domain = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(arrayToken2.Data), (NumericToken v) => v.Double));
			}
			else
			{
				domain = new double[]
				{
					default(double),
					1.0
				};
			}
			if (!shadingDictionary.ContainsKey(NameToken.Function))
			{
				defaultInterpolatedStringHandler..ctor(33, 2);
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.Function);
				defaultInterpolatedStringHandler.AppendLiteral(" is required for shading type '");
				defaultInterpolatedStringHandler.AppendFormatted<ShadingType>(ShadingType.Radial);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			PdfFunction[] functions = ShadingParser.GetFunctions(shadingDictionary.Data[NameToken.Function], scanner, filterProvider);
			bool[] extend = new bool[2];
			ArrayToken arrayToken3;
			if (shadingDictionary.TryGet(NameToken.Extend, scanner, out arrayToken3))
			{
				extend = Enumerable.ToArray<bool>(Enumerable.Select<BooleanToken, bool>(Enumerable.OfType<BooleanToken>(arrayToken3.Data), (BooleanToken v) => v.Data));
			}
			return new RadialShading(antiAlias, shadingDictionary, colorSpace, bbox, background, coords, domain, functions, extend);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000D5D8 File Offset: 0x0000B7D8
		private static FreeFormGouraudShading CreateFreeFormGouraudShadedTriangleMeshesShading(StreamToken shadingStream, ColorSpaceDetails colorSpace, double[] background, PdfRectangle? bbox, bool antiAlias, IPdfTokenScanner scanner, ILookupFilterProvider filterProvider)
		{
			NumericToken numericToken;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (!shadingStream.StreamDictionary.TryGet(NameToken.BitsPerCoordinate, scanner, out numericToken))
			{
				defaultInterpolatedStringHandler..ctor(33, 2);
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.BitsPerCoordinate);
				defaultInterpolatedStringHandler.AppendLiteral(" is required for shading type '");
				defaultInterpolatedStringHandler.AppendFormatted<ShadingType>(ShadingType.FreeFormGouraud);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int @int = numericToken.Int;
			NumericToken numericToken2;
			if (!shadingStream.StreamDictionary.TryGet(NameToken.BitsPerComponent, scanner, out numericToken2))
			{
				defaultInterpolatedStringHandler..ctor(33, 2);
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.BitsPerComponent);
				defaultInterpolatedStringHandler.AppendLiteral(" is required for shading type '");
				defaultInterpolatedStringHandler.AppendFormatted<ShadingType>(ShadingType.FreeFormGouraud);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int int2 = numericToken2.Int;
			NumericToken numericToken3;
			if (!shadingStream.StreamDictionary.TryGet(NameToken.BitsPerFlag, scanner, out numericToken3))
			{
				defaultInterpolatedStringHandler..ctor(33, 2);
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.BitsPerFlag);
				defaultInterpolatedStringHandler.AppendLiteral(" is required for shading type '");
				defaultInterpolatedStringHandler.AppendFormatted<ShadingType>(ShadingType.FreeFormGouraud);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int int3 = numericToken3.Int;
			ArrayToken arrayToken;
			if (shadingStream.StreamDictionary.TryGet(NameToken.Decode, scanner, out arrayToken))
			{
				double[] decode = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(arrayToken.Data), (NumericToken v) => v.Double));
				PdfFunction[] functions = null;
				if (shadingStream.StreamDictionary.ContainsKey(NameToken.Function))
				{
					functions = ShadingParser.GetFunctions(shadingStream.StreamDictionary.Data[NameToken.Function], scanner, filterProvider);
				}
				return new FreeFormGouraudShading(antiAlias, shadingStream, colorSpace, bbox, background, @int, int2, int3, decode, functions);
			}
			defaultInterpolatedStringHandler..ctor(33, 2);
			defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.Decode);
			defaultInterpolatedStringHandler.AppendLiteral(" is required for shading type '");
			defaultInterpolatedStringHandler.AppendFormatted<ShadingType>(ShadingType.FreeFormGouraud);
			defaultInterpolatedStringHandler.AppendLiteral("'.");
			throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000D7EC File Offset: 0x0000B9EC
		private static LatticeFormGouraudShading CreateLatticeFormGouraudShadedTriangleMeshesShading(StreamToken shadingStream, ColorSpaceDetails colorSpace, double[] background, PdfRectangle? bbox, bool antiAlias, IPdfTokenScanner scanner, ILookupFilterProvider filterProvider)
		{
			NumericToken numericToken;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (!shadingStream.StreamDictionary.TryGet(NameToken.BitsPerCoordinate, scanner, out numericToken))
			{
				defaultInterpolatedStringHandler..ctor(33, 2);
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.BitsPerCoordinate);
				defaultInterpolatedStringHandler.AppendLiteral(" is required for shading type '");
				defaultInterpolatedStringHandler.AppendFormatted<ShadingType>(ShadingType.LatticeFormGouraud);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int @int = numericToken.Int;
			NumericToken numericToken2;
			if (!shadingStream.StreamDictionary.TryGet(NameToken.BitsPerComponent, scanner, out numericToken2))
			{
				defaultInterpolatedStringHandler..ctor(33, 2);
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.BitsPerComponent);
				defaultInterpolatedStringHandler.AppendLiteral(" is required for shading type '");
				defaultInterpolatedStringHandler.AppendFormatted<ShadingType>(ShadingType.LatticeFormGouraud);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int int2 = numericToken2.Int;
			NumericToken numericToken3;
			if (!shadingStream.StreamDictionary.TryGet(NameToken.VerticesPerRow, scanner, out numericToken3))
			{
				defaultInterpolatedStringHandler..ctor(33, 2);
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.VerticesPerRow);
				defaultInterpolatedStringHandler.AppendLiteral(" is required for shading type '");
				defaultInterpolatedStringHandler.AppendFormatted<ShadingType>(ShadingType.LatticeFormGouraud);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int int3 = numericToken3.Int;
			ArrayToken arrayToken;
			if (shadingStream.StreamDictionary.TryGet(NameToken.Decode, scanner, out arrayToken))
			{
				double[] decode = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(arrayToken.Data), (NumericToken v) => v.Double));
				PdfFunction[] functions = null;
				if (shadingStream.StreamDictionary.ContainsKey(NameToken.Function))
				{
					functions = ShadingParser.GetFunctions(shadingStream.StreamDictionary.Data[NameToken.Function], scanner, filterProvider);
				}
				return new LatticeFormGouraudShading(antiAlias, shadingStream, colorSpace, bbox, background, @int, int2, int3, decode, functions);
			}
			defaultInterpolatedStringHandler..ctor(33, 2);
			defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.Decode);
			defaultInterpolatedStringHandler.AppendLiteral(" is required for shading type '");
			defaultInterpolatedStringHandler.AppendFormatted<ShadingType>(ShadingType.LatticeFormGouraud);
			defaultInterpolatedStringHandler.AppendLiteral("'.");
			throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000DA00 File Offset: 0x0000BC00
		private static CoonsPatchMeshesShading CreateCoonsPatchMeshesShading(StreamToken shadingStream, ColorSpaceDetails colorSpace, double[] background, PdfRectangle? bbox, bool antiAlias, IPdfTokenScanner scanner, ILookupFilterProvider filterProvider)
		{
			NumericToken numericToken;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (!shadingStream.StreamDictionary.TryGet(NameToken.BitsPerCoordinate, scanner, out numericToken))
			{
				defaultInterpolatedStringHandler..ctor(33, 2);
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.BitsPerCoordinate);
				defaultInterpolatedStringHandler.AppendLiteral(" is required for shading type '");
				defaultInterpolatedStringHandler.AppendFormatted<ShadingType>(ShadingType.CoonsPatch);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int @int = numericToken.Int;
			NumericToken numericToken2;
			if (!shadingStream.StreamDictionary.TryGet(NameToken.BitsPerComponent, scanner, out numericToken2))
			{
				defaultInterpolatedStringHandler..ctor(33, 2);
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.BitsPerComponent);
				defaultInterpolatedStringHandler.AppendLiteral(" is required for shading type '");
				defaultInterpolatedStringHandler.AppendFormatted<ShadingType>(ShadingType.CoonsPatch);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int int2 = numericToken2.Int;
			NumericToken numericToken3;
			if (!shadingStream.StreamDictionary.TryGet(NameToken.BitsPerFlag, scanner, out numericToken3))
			{
				defaultInterpolatedStringHandler..ctor(33, 2);
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.BitsPerFlag);
				defaultInterpolatedStringHandler.AppendLiteral(" is required for shading type '");
				defaultInterpolatedStringHandler.AppendFormatted<ShadingType>(ShadingType.CoonsPatch);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int int3 = numericToken3.Int;
			ArrayToken arrayToken;
			if (shadingStream.StreamDictionary.TryGet(NameToken.Decode, scanner, out arrayToken))
			{
				double[] decode = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(arrayToken.Data), (NumericToken v) => v.Double));
				PdfFunction[] functions = null;
				if (shadingStream.StreamDictionary.ContainsKey(NameToken.Function))
				{
					functions = ShadingParser.GetFunctions(shadingStream.StreamDictionary.Data[NameToken.Function], scanner, filterProvider);
				}
				return new CoonsPatchMeshesShading(antiAlias, shadingStream, colorSpace, bbox, background, @int, int2, int3, decode, functions);
			}
			defaultInterpolatedStringHandler..ctor(33, 2);
			defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.Decode);
			defaultInterpolatedStringHandler.AppendLiteral(" is required for shading type '");
			defaultInterpolatedStringHandler.AppendFormatted<ShadingType>(ShadingType.CoonsPatch);
			defaultInterpolatedStringHandler.AppendLiteral("'.");
			throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000DC14 File Offset: 0x0000BE14
		private static TensorProductPatchMeshesShading CreateTensorProductPatchMeshesShading(StreamToken shadingStream, ColorSpaceDetails colorSpace, double[] background, PdfRectangle? bbox, bool antiAlias, IPdfTokenScanner scanner, ILookupFilterProvider filterProvider)
		{
			NumericToken numericToken;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (!shadingStream.StreamDictionary.TryGet(NameToken.BitsPerCoordinate, scanner, out numericToken))
			{
				defaultInterpolatedStringHandler..ctor(33, 2);
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.BitsPerCoordinate);
				defaultInterpolatedStringHandler.AppendLiteral(" is required for shading type '");
				defaultInterpolatedStringHandler.AppendFormatted<ShadingType>(ShadingType.TensorProductPatch);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int @int = numericToken.Int;
			NumericToken numericToken2;
			if (!shadingStream.StreamDictionary.TryGet(NameToken.BitsPerComponent, scanner, out numericToken2))
			{
				defaultInterpolatedStringHandler..ctor(33, 2);
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.BitsPerComponent);
				defaultInterpolatedStringHandler.AppendLiteral(" is required for shading type '");
				defaultInterpolatedStringHandler.AppendFormatted<ShadingType>(ShadingType.TensorProductPatch);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int int2 = numericToken2.Int;
			NumericToken numericToken3;
			if (!shadingStream.StreamDictionary.TryGet(NameToken.BitsPerFlag, scanner, out numericToken3))
			{
				defaultInterpolatedStringHandler..ctor(33, 2);
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.BitsPerFlag);
				defaultInterpolatedStringHandler.AppendLiteral(" is required for shading type '");
				defaultInterpolatedStringHandler.AppendFormatted<ShadingType>(ShadingType.TensorProductPatch);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			int int3 = numericToken3.Int;
			ArrayToken arrayToken;
			if (shadingStream.StreamDictionary.TryGet(NameToken.Decode, scanner, out arrayToken))
			{
				double[] decode = Enumerable.ToArray<double>(Enumerable.Select<NumericToken, double>(Enumerable.OfType<NumericToken>(arrayToken.Data), (NumericToken v) => v.Double));
				PdfFunction[] functions = null;
				if (shadingStream.StreamDictionary.ContainsKey(NameToken.Function))
				{
					functions = ShadingParser.GetFunctions(shadingStream.StreamDictionary.Data[NameToken.Function], scanner, filterProvider);
				}
				return new TensorProductPatchMeshesShading(antiAlias, shadingStream, colorSpace, bbox, background, @int, int2, int3, decode, functions);
			}
			defaultInterpolatedStringHandler..ctor(33, 2);
			defaultInterpolatedStringHandler.AppendFormatted<NameToken>(NameToken.Decode);
			defaultInterpolatedStringHandler.AppendLiteral(" is required for shading type '");
			defaultInterpolatedStringHandler.AppendFormatted<ShadingType>(ShadingType.TensorProductPatch);
			defaultInterpolatedStringHandler.AppendLiteral("'.");
			throw new ArgumentNullException(defaultInterpolatedStringHandler.ToStringAndClear());
		}
	}
}
