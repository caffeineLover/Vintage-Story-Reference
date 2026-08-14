using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.AdobeFontMetrics;

namespace UglyToad.PdfPig.Fonts.Standard14Fonts
{
	// Token: 0x02000075 RID: 117
	public static class Standard14
	{
		// Token: 0x060002BC RID: 700 RVA: 0x0000DFA0 File Offset: 0x0000C1A0
		static Standard14()
		{
			Standard14.AddAdobeFontMetrics("Courier-Bold", new Standard14Font?(Standard14Font.CourierBold));
			Standard14.AddAdobeFontMetrics("Courier-BoldOblique", new Standard14Font?(Standard14Font.CourierBoldOblique));
			Standard14.AddAdobeFontMetrics("Courier", new Standard14Font?(Standard14Font.Courier));
			Standard14.AddAdobeFontMetrics("Courier-Oblique", new Standard14Font?(Standard14Font.CourierOblique));
			Standard14.AddAdobeFontMetrics("Helvetica", new Standard14Font?(Standard14Font.Helvetica));
			Standard14.AddAdobeFontMetrics("Helvetica-Bold", new Standard14Font?(Standard14Font.HelveticaBold));
			Standard14.AddAdobeFontMetrics("Helvetica-BoldOblique", new Standard14Font?(Standard14Font.HelveticaBoldOblique));
			Standard14.AddAdobeFontMetrics("Helvetica-Oblique", new Standard14Font?(Standard14Font.HelveticaOblique));
			Standard14.AddAdobeFontMetrics("Symbol", new Standard14Font?(Standard14Font.Symbol));
			Standard14.AddAdobeFontMetrics("Times-Bold", new Standard14Font?(Standard14Font.TimesBold));
			Standard14.AddAdobeFontMetrics("Times-BoldItalic", new Standard14Font?(Standard14Font.TimesBoldItalic));
			Standard14.AddAdobeFontMetrics("Times-Italic", new Standard14Font?(Standard14Font.TimesItalic));
			Standard14.AddAdobeFontMetrics("Times-Roman", new Standard14Font?(Standard14Font.TimesRoman));
			Standard14.AddAdobeFontMetrics("ZapfDingbats", new Standard14Font?(Standard14Font.ZapfDingbats));
			Standard14.AddAdobeFontMetrics("CourierCourierNew", "Courier", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("CourierNew", "Courier", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("CourierNew,Italic", "Courier-Oblique", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("CourierNew,Bold", "Courier-Bold", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("CourierNew,BoldItalic", "Courier-BoldOblique", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("Arial", "Helvetica", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("Arial,Italic", "Helvetica-Oblique", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("Arial,Bold", "Helvetica-Bold", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("Arial,BoldItalic", "Helvetica-BoldOblique", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("TimesNewRoman", "Times-Roman", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("TimesNewRoman,Italic", "Times-Italic", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("TimesNewRoman,Bold", "Times-Bold", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("TimesNewRoman,BoldItalic", "Times-BoldItalic", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("Symbol,Italic", "Symbol", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("Symbol,Bold", "Symbol", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("Symbol,BoldItalic", "Symbol", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("Times", "Times-Roman", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("Times,Italic", "Times-Italic", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("Times,Bold", "Times-Bold", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("Times,BoldItalic", "Times-BoldItalic", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("ArialMT", "Helvetica", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("Arial-ItalicMT", "Helvetica-Oblique", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("Arial-BoldMT", "Helvetica-Bold", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("Arial-BoldMT,Bold", "Helvetica-Bold", default(Standard14Font?));
			Standard14.AddAdobeFontMetrics("Arial-BoldItalicMT", "Helvetica-BoldOblique", default(Standard14Font?));
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000E318 File Offset: 0x0000C518
		private static void AddAdobeFontMetrics(string fontName, Standard14Font? type = null)
		{
			Standard14.AddAdobeFontMetrics(fontName, fontName, type);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000E324 File Offset: 0x0000C524
		private static void AddAdobeFontMetrics(string fontName, string afmName, Standard14Font? type = null)
		{
			Standard14.Standard14Names.Add(fontName);
			Standard14.Standard14Mapping.Add(fontName, afmName);
			if (type != null)
			{
				Standard14.BuilderTypesToNames[type.Value] = afmName;
			}
			AdobeFontMetrics adobeFontMetrics;
			if (Standard14.Standard14Cache.TryGetValue(afmName, ref adobeFontMetrics))
			{
				Standard14.Standard14Cache[fontName] = adobeFontMetrics;
			}
			try
			{
				Assembly assembly = typeof(Standard14).Assembly;
				string text = "UglyToad.PdfPig.Fonts.Resources.AdobeFontMetrics." + afmName + ".afm";
				IInputBytes bytes;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (Stream manifestResourceStream = assembly.GetManifestResourceStream(text))
					{
						if (manifestResourceStream == null)
						{
							throw new InvalidOperationException("Could not find AFM resource with name: " + text + ".");
						}
						manifestResourceStream.CopyTo(memoryStream);
						bytes = new ByteArrayInputBytes(memoryStream.ToArray());
					}
				}
				Standard14.Standard14Cache[fontName] = AdobeFontMetricsParser.Parse(bytes, true);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Could not load " + fontName + " from the AFM files.", ex);
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000E454 File Offset: 0x0000C654
		public static AdobeFontMetrics GetAdobeFontMetrics(string baseName)
		{
			AdobeFontMetrics result;
			Standard14.Standard14Cache.TryGetValue(baseName, ref result);
			return result;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000E470 File Offset: 0x0000C670
		public static AdobeFontMetrics GetAdobeFontMetrics(Standard14Font fontType)
		{
			return Standard14.Standard14Cache[Standard14.BuilderTypesToNames[fontType]];
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000E487 File Offset: 0x0000C687
		public static bool IsFontInStandard14(string baseName)
		{
			return Standard14.Standard14Names.Contains(baseName);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000E494 File Offset: 0x0000C694
		public static HashSet<string> GetNames()
		{
			return new HashSet<string>(Standard14.Standard14Names);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000E4A0 File Offset: 0x0000C6A0
		public static string GetMappedFontName(string baseName)
		{
			string result;
			Standard14.Standard14Mapping.TryGetValue(baseName, ref result);
			return result;
		}

		// Token: 0x040002D9 RID: 729
		private static readonly HashSet<string> Standard14Names = new HashSet<string>();

		// Token: 0x040002DA RID: 730
		private static readonly Dictionary<string, string> Standard14Mapping = new Dictionary<string, string>(34);

		// Token: 0x040002DB RID: 731
		private static readonly Dictionary<Standard14Font, string> BuilderTypesToNames = new Dictionary<Standard14Font, string>(14);

		// Token: 0x040002DC RID: 732
		private static readonly Dictionary<string, AdobeFontMetrics> Standard14Cache = new Dictionary<string, AdobeFontMetrics>(34);
	}
}
