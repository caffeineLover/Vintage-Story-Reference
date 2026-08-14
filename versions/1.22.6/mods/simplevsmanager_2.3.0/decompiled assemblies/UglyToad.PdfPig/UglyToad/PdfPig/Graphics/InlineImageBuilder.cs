using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Graphics.Colors;
using UglyToad.PdfPig.Graphics.Core;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics
{
	// Token: 0x02000109 RID: 265
	internal class InlineImageBuilder
	{
		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x0001FAD0 File Offset: 0x0001DCD0
		// (set) Token: 0x06000588 RID: 1416 RVA: 0x0001FAD8 File Offset: 0x0001DCD8
		public IReadOnlyDictionary<NameToken, IToken> Properties { get; set; }

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x0001FAE1 File Offset: 0x0001DCE1
		// (set) Token: 0x0600058A RID: 1418 RVA: 0x0001FAE9 File Offset: 0x0001DCE9
		public IReadOnlyList<byte> Bytes { get; set; }

		// Token: 0x0600058B RID: 1419 RVA: 0x0001FAF4 File Offset: 0x0001DCF4
		public InlineImage CreateInlineImage(TransformationMatrix transformationMatrix, ILookupFilterProvider filterProvider, IPdfTokenScanner tokenScanner, RenderingIntent defaultRenderingIntent, IResourceStore resourceStore)
		{
			if (this.Properties == null || this.Bytes == null)
			{
				throw new InvalidOperationException("Inline image builder not completely defined before calling CreateInlineImage.");
			}
			PdfRectangle bounds = transformationMatrix.Transform(new PdfRectangle(new PdfPoint(1, 1), new PdfPoint(0, 0)));
			int @int = this.GetByKeys<NumericToken>(NameToken.Width, NameToken.W, true).Int;
			int int2 = this.GetByKeys<NumericToken>(NameToken.Height, NameToken.H, true).Int;
			BooleanToken byKeys = this.GetByKeys<BooleanToken>(NameToken.ImageMask, NameToken.Im, false);
			bool flag = byKeys != null && byKeys.Data;
			NumericToken byKeys2 = this.GetByKeys<NumericToken>(NameToken.BitsPerComponent, NameToken.Bpc, !flag);
			int bitsPerComponent = (byKeys2 != null) ? byKeys2.Int : 1;
			NameToken nameToken = null;
			if (!flag)
			{
				nameToken = this.GetByKeys<NameToken>(NameToken.ColorSpace, NameToken.Cs, false);
				if (nameToken == null)
				{
					ArrayToken byKeys3 = this.GetByKeys<ArrayToken>(NameToken.ColorSpace, NameToken.Cs, true);
					if (byKeys3.Length == 0)
					{
						throw new PdfDocumentFormatException("Empty ColorSpace array defined for inline image.");
					}
					NameToken nameToken2 = byKeys3.Data[0] as NameToken;
					if (nameToken2 == null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
						defaultInterpolatedStringHandler..ctor(52, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Invalid ColorSpace array defined for inline image: ");
						defaultInterpolatedStringHandler.AppendFormatted<ArrayToken>(byKeys3);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					nameToken = nameToken2;
				}
			}
			DictionaryToken dictionaryToken = new DictionaryToken(this.Properties ?? new Dictionary<NameToken, IToken>());
			ColorSpaceDetails colorSpaceDetails = resourceStore.GetColorSpaceDetails(nameToken, dictionaryToken);
			NameToken byKeys4 = this.GetByKeys<NameToken>(NameToken.Intent, null, false);
			Nullable<RenderingIntent> renderingIntent;
			if (byKeys4 == null)
			{
				renderingIntent = default(RenderingIntent?);
			}
			else
			{
				string data = byKeys4.Data;
				renderingIntent = ((data != null) ? new RenderingIntent?(data.ToRenderingIntent()) : default(RenderingIntent?));
			}
			RenderingIntent renderingIntent2 = renderingIntent ?? defaultRenderingIntent;
			List<NameToken> list = new List<NameToken>();
			NameToken byKeys5 = this.GetByKeys<NameToken>(NameToken.Filter, NameToken.F, false);
			if (byKeys5 == null)
			{
				ArrayToken byKeys6 = this.GetByKeys<ArrayToken>(NameToken.Filter, NameToken.F, false);
				if (byKeys6 != null)
				{
					list.AddRange(Enumerable.OfType<NameToken>(byKeys6.Data));
				}
			}
			else
			{
				list.Add(byKeys5);
			}
			IReadOnlyList<IFilter> namedFilters = filterProvider.GetNamedFilters(list);
			decimal[] decode = Enumerable.ToArray<decimal>(Enumerable.Select<NumericToken, decimal>(Enumerable.OfType<NumericToken>((this.GetByKeys<ArrayToken>(NameToken.Decode, NameToken.D, false) ?? new ArrayToken(EmptyArray<IToken>.Instance)).Data), (NumericToken x) => x.Data));
			BooleanToken byKeys7 = this.GetByKeys<BooleanToken>(NameToken.Interpolate, NameToken.I, false);
			bool interpolate = byKeys7 != null && byKeys7.Data;
			return new InlineImage(bounds, @int, int2, bitsPerComponent, flag, renderingIntent2, interpolate, decode, this.Bytes, namedFilters, dictionaryToken, colorSpaceDetails);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0001FDA8 File Offset: 0x0001DFA8
		private T GetByKeys<T>(NameToken name1, NameToken name2, bool required) where T : IToken
		{
			IToken token;
			if (this.Properties.TryGetValue(name1, ref token) && token is T)
			{
				return (T)((object)token);
			}
			if (name2 != null && this.Properties.TryGetValue(name2, ref token) && token is T)
			{
				return (T)((object)token);
			}
			if (required)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(49, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Inline image dictionary missing required entry ");
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(name1);
				defaultInterpolatedStringHandler.AppendLiteral("/");
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(name2);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return default(T);
		}
	}
}
