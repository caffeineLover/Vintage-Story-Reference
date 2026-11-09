using System;
using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts;
using UglyToad.PdfPig.Fonts.AdobeFontMetrics;
using UglyToad.PdfPig.Fonts.Encodings;
using UglyToad.PdfPig.Fonts.TrueType;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.PdfFonts.Simple
{
	// Token: 0x02000097 RID: 151
	internal class TrueTypeStandard14FallbackSimpleFont : IFont
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600027E RID: 638 RVA: 0x000109B5 File Offset: 0x0000EBB5
		public NameToken Name { get; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600027F RID: 639 RVA: 0x000109BD File Offset: 0x0000EBBD
		public bool IsVertical { get; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000280 RID: 640 RVA: 0x000109C5 File Offset: 0x0000EBC5
		// (set) Token: 0x06000281 RID: 641 RVA: 0x000109CD File Offset: 0x0000EBCD
		public FontDetails Details { get; set; }

		// Token: 0x06000282 RID: 642 RVA: 0x000109D8 File Offset: 0x0000EBD8
		public TrueTypeStandard14FallbackSimpleFont(NameToken name, AdobeFontMetrics fontMetrics, Encoding encoding, TrueTypeFont font, TrueTypeStandard14FallbackSimpleFont.MetricOverrides overrides)
		{
			this.fontMetrics = fontMetrics;
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			this.encoding = encoding;
			this.font = font;
			this.overrides = overrides;
			this.Name = name;
			FontDetails details;
			if (fontMetrics != null)
			{
				NameToken name2 = this.Name;
				details = new FontDetails((name2 != null) ? name2.Data : null, fontMetrics.Weight == "Bold", (fontMetrics.Weight == "Bold") ? 700 : 500, fontMetrics.ItalicAngle != 0m);
			}
			else
			{
				NameToken name3 = this.Name;
				details = FontDetails.GetDefault((name3 != null) ? name3.Data : null);
			}
			this.Details = details;
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00010A96 File Offset: 0x0000EC96
		public int ReadCharacterCode(IInputBytes bytes, out int codeLength)
		{
			codeLength = 1;
			return (int)bytes.CurrentByte;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00010AA4 File Offset: 0x0000ECA4
		public bool TryGetUnicode(int characterCode, out string value)
		{
			value = null;
			string name = this.encoding.GetName(characterCode);
			try
			{
				value = GlyphList.AdobeGlyphList.NameToUnicode(name);
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00010AE8 File Offset: 0x0000ECE8
		public CharacterBoundingBox GetBoundingBox(int characterCode)
		{
			double num = 0.0;
			TransformationMatrix fontMatrix = this.GetFontMatrix();
			PdfRectangle pdfRectangle;
			if (this.font != null && this.font.TryGetBoundingBox(characterCode, ref pdfRectangle))
			{
				pdfRectangle = fontMatrix.Transform(pdfRectangle);
				TrueTypeStandard14FallbackSimpleFont.MetricOverrides metricOverrides = this.overrides;
				if (metricOverrides == null || !metricOverrides.TryGetWidth(characterCode, out num))
				{
					string name = this.encoding.GetName(characterCode);
					AdobeFontMetricsIndividualCharacterMetric adobeFontMetricsIndividualCharacterMetric;
					if (this.fontMetrics.CharacterMetrics.TryGetValue(name, ref adobeFontMetricsIndividualCharacterMetric))
					{
						num = TrueTypeStandard14FallbackSimpleFont.DefaultTransformation.TransformX(adobeFontMetricsIndividualCharacterMetric.Width.X);
					}
					else
					{
						num = pdfRectangle.Width;
					}
				}
				else
				{
					num = TrueTypeStandard14FallbackSimpleFont.DefaultTransformation.TransformX(num);
				}
				return new CharacterBoundingBox(pdfRectangle, num);
			}
			string name2 = this.encoding.GetName(characterCode);
			AdobeFontMetricsIndividualCharacterMetric adobeFontMetricsIndividualCharacterMetric2;
			if (!this.fontMetrics.CharacterMetrics.TryGetValue(name2, ref adobeFontMetricsIndividualCharacterMetric2))
			{
				return new CharacterBoundingBox(default(PdfRectangle), 0.0);
			}
			TrueTypeStandard14FallbackSimpleFont.MetricOverrides metricOverrides2 = this.overrides;
			if (metricOverrides2 == null || !metricOverrides2.TryGetWidth(characterCode, out num))
			{
				num = fontMatrix.TransformX(adobeFontMetricsIndividualCharacterMetric2.Width.X);
			}
			else
			{
				num = TrueTypeStandard14FallbackSimpleFont.DefaultTransformation.TransformX(num);
			}
			pdfRectangle = fontMatrix.Transform(adobeFontMetricsIndividualCharacterMetric2.BoundingBox);
			return new CharacterBoundingBox(pdfRectangle, num);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00010C44 File Offset: 0x0000EE44
		public TransformationMatrix GetFontMatrix()
		{
			TrueTypeFont trueTypeFont = this.font;
			if (((trueTypeFont != null) ? trueTypeFont.TableRegister.HeaderTable : null) != null)
			{
				double num = (double)this.font.GetUnitsPerEm();
				return TransformationMatrix.FromValues(1.0 / num, 0.0, 0.0, 1.0 / num, 0.0, 0.0);
			}
			return TrueTypeStandard14FallbackSimpleFont.DefaultTransformation;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00010CBC File Offset: 0x0000EEBC
		public bool TryGetPath(int characterCode, out IReadOnlyList<PdfSubpath> path)
		{
			path = null;
			return this.font != null && this.font.TryGetPath(characterCode, ref path);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00010CD8 File Offset: 0x0000EED8
		public bool TryGetNormalisedPath(int characterCode, out IReadOnlyList<PdfSubpath> path)
		{
			if (!this.TryGetPath(characterCode, out path))
			{
				return false;
			}
			path = Enumerable.ToList<PdfSubpath>(this.GetFontMatrix().Transform(path));
			return true;
		}

		// Token: 0x04000155 RID: 341
		private static readonly TransformationMatrix DefaultTransformation = TransformationMatrix.FromValues(0.001, 0.0, 0.0, 0.001, 0.0, 0.0);

		// Token: 0x04000156 RID: 342
		private readonly AdobeFontMetrics fontMetrics;

		// Token: 0x04000157 RID: 343
		private readonly Encoding encoding;

		// Token: 0x04000158 RID: 344
		private readonly TrueTypeFont font;

		// Token: 0x04000159 RID: 345
		private readonly TrueTypeStandard14FallbackSimpleFont.MetricOverrides overrides;

		// Token: 0x02000253 RID: 595
		public class MetricOverrides
		{
			// Token: 0x17000407 RID: 1031
			// (get) Token: 0x06000DBB RID: 3515 RVA: 0x0004119A File Offset: 0x0003F39A
			public int? FirstCharacterCode { get; }

			// Token: 0x17000408 RID: 1032
			// (get) Token: 0x06000DBC RID: 3516 RVA: 0x000411A2 File Offset: 0x0003F3A2
			public IReadOnlyList<double> Widths { get; }

			// Token: 0x17000409 RID: 1033
			// (get) Token: 0x06000DBD RID: 3517 RVA: 0x000411AA File Offset: 0x0003F3AA
			public bool HasOverriddenMetrics { get; }

			// Token: 0x06000DBE RID: 3518 RVA: 0x000411B4 File Offset: 0x0003F3B4
			public MetricOverrides(int? firstCharacterCode, IReadOnlyList<double> widths)
			{
				this.FirstCharacterCode = firstCharacterCode;
				this.Widths = widths;
				this.HasOverriddenMetrics = (this.FirstCharacterCode != null && this.Widths != null && this.Widths.Count > 0);
			}

			// Token: 0x06000DBF RID: 3519 RVA: 0x00041204 File Offset: 0x0003F404
			public bool TryGetWidth(int characterCode, out double width)
			{
				width = 0.0;
				if (!this.HasOverriddenMetrics || this.FirstCharacterCode == null)
				{
					return false;
				}
				int num = characterCode - this.FirstCharacterCode.Value;
				if (num < 0 || num >= this.Widths.Count)
				{
					return false;
				}
				width = this.Widths[num];
				return true;
			}
		}
	}
}
