using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.Standard14Fonts;
using UglyToad.PdfPig.Fonts.TrueType;
using UglyToad.PdfPig.Fonts.TrueType.Parser;
using UglyToad.PdfPig.Fonts.TrueType.Tables;

namespace UglyToad.PdfPig.Fonts.SystemFonts
{
	// Token: 0x02000071 RID: 113
	public class SystemFontFinder : ISystemFontFinder
	{
		// Token: 0x060002AF RID: 687 RVA: 0x0000D69C File Offset: 0x0000B89C
		static SystemFontFinder()
		{
			Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
			dictionary.Add("Courier", new string[]
			{
				"CourierNew",
				"CourierNewPSMT",
				"LiberationMono",
				"NimbusMonL-Regu"
			});
			dictionary.Add("Courier-Bold", new string[]
			{
				"CourierNewPS-BoldMT",
				"CourierNew-Bold",
				"LiberationMono-Bold",
				"NimbusMonL-Bold"
			});
			dictionary.Add("Courier-Oblique", new string[]
			{
				"CourierNewPS-ItalicMT",
				"CourierNew-Italic",
				"LiberationMono-Italic",
				"NimbusMonL-ReguObli"
			});
			dictionary.Add("Courier-BoldOblique", new string[]
			{
				"CourierNewPS-BoldItalicMT",
				"CourierNew-BoldItalic",
				"LiberationMono-BoldItalic",
				"NimbusMonL-BoldObli"
			});
			dictionary.Add("Helvetica", new string[]
			{
				"ArialMT",
				"Arial",
				"LiberationSans",
				"NimbusSanL-Regu"
			});
			dictionary.Add("Helvetica-Bold", new string[]
			{
				"Arial-BoldMT",
				"Arial-Bold",
				"LiberationSans-Bold",
				"NimbusSanL-Bold"
			});
			dictionary.Add("Helvetica-BoldOblique", new string[]
			{
				"Arial-BoldItalicMT",
				"Helvetica-BoldItalic",
				"LiberationSans-BoldItalic",
				"NimbusSanL-BoldItal"
			});
			dictionary.Add("Helvetica-Oblique", new string[]
			{
				"Arial-ItalicMT",
				"Arial-Italic",
				"Helvetica-Italic",
				"LiberationSans-Italic",
				"NimbusSanL-ReguItal"
			});
			dictionary.Add("Times-Roman", new string[]
			{
				"TimesNewRomanPSMT",
				"TimesNewRoman",
				"TimesNewRomanPS",
				"LiberationSerif",
				"NimbusRomNo9L-Regu"
			});
			dictionary.Add("Times-Bold", new string[]
			{
				"TimesNewRomanPS-BoldMT",
				"TimesNewRomanPS-Bold",
				"TimesNewRoman-Bold",
				"LiberationSerif-Bold",
				"NimbusRomNo9L-Medi"
			});
			dictionary.Add("Times-Italic", new string[]
			{
				"TimesNewRomanPS-ItalicMT",
				"TimesNewRomanPS-Italic",
				"TimesNewRoman-Italic",
				"LiberationSerif-Italic",
				"NimbusRomNo9L-ReguItal"
			});
			dictionary.Add("TimesNewRomanPS-BoldItalicMT", new string[]
			{
				"TimesNewRomanPS-BoldItalic",
				"TimesNewRoman-BoldItalic",
				"LiberationSerif-BoldItalic",
				"NimbusRomNo9L-MediItal"
			});
			dictionary.Add("Symbol", new string[]
			{
				"SymbolMT",
				"StandardSymL"
			});
			dictionary.Add("ZapfDingbats", new string[]
			{
				"ZapfDingbatsITC",
				"Dingbats",
				"MS-Gothic"
			});
			Dictionary<string, string[]> dictionary2 = dictionary;
			HashSet<string> names;
			try
			{
				names = Standard14.GetNames();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Failed to load the Standard 14 fonts from the assembly's resources.", ex);
			}
			foreach (string text in names)
			{
				if (!dictionary2.ContainsKey(text))
				{
					string mappedFontName = Standard14.GetMappedFontName(text);
					string[] array;
					if (dictionary2.TryGetValue(mappedFontName, ref array))
					{
						dictionary2[text] = array;
					}
					else
					{
						dictionary2[text] = new string[]
						{
							mappedFontName
						};
					}
				}
			}
			SystemFontFinder.NameSubstitutes = dictionary2;
			ISystemFontLister lister = new WindowsSystemFontLister();
			SystemFontFinder.AvailableFonts = new Lazy<IReadOnlyList<SystemFontRecord>>(() => Enumerable.ToList<SystemFontRecord>(lister.GetAllFonts()));
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000DA54 File Offset: 0x0000BC54
		private SystemFontFinder()
		{
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000DA84 File Offset: 0x0000BC84
		public TrueTypeFont GetTrueTypeFont(string name)
		{
			TrueTypeFont trueTypeFontNamed = this.GetTrueTypeFontNamed(name);
			if (trueTypeFontNamed != null)
			{
				return trueTypeFontNamed;
			}
			if (name.Contains("-"))
			{
				trueTypeFontNamed = this.GetTrueTypeFontNamed(name.Replace("-", string.Empty));
				if (trueTypeFontNamed != null)
				{
					return trueTypeFontNamed;
				}
			}
			if (name.Contains(","))
			{
				trueTypeFontNamed = this.GetTrueTypeFontNamed(name.Replace(",", "-"));
				if (trueTypeFontNamed != null)
				{
					return trueTypeFontNamed;
				}
			}
			foreach (string name2 in this.GetSubstituteNames(name))
			{
				trueTypeFontNamed = this.GetTrueTypeFontNamed(name2);
				if (trueTypeFontNamed != null)
				{
					return trueTypeFontNamed;
				}
			}
			return this.GetTrueTypeFontNamed(name + "-Regular");
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000DB50 File Offset: 0x0000BD50
		private IEnumerable<string> GetSubstituteNames(string name)
		{
			name = name.Replace(" ", string.Empty);
			string[] result;
			if (SystemFontFinder.NameSubstitutes.TryGetValue(name, ref result))
			{
				return result;
			}
			return EmptyArray<string>.Instance;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000DB88 File Offset: 0x0000BD88
		private TrueTypeFont GetTrueTypeFontNamed(string name)
		{
			object cacheLock = SystemFontFinder.CacheLock;
			lock (cacheLock)
			{
				TrueTypeFont result;
				if (SystemFontFinder.Cache.TryGetValue(name, ref result))
				{
					return result;
				}
			}
			string fileName;
			if (!this.nameToFileNameMap.TryGetValue(name, ref fileName))
			{
				foreach (SystemFontRecord record in Enumerable.Where<SystemFontRecord>(SystemFontFinder.AvailableFonts.Value, delegate(SystemFontRecord x)
				{
					string fileName2 = Path.GetFileName(x.Path);
					return fileName2 != null && fileName2.StartsWith(name.get_Chars(0).ToString(), 5);
				}))
				{
					TrueTypeFont result2;
					if (this.TryGetTrueTypeFont(name, record, out result2))
					{
						return result2;
					}
				}
				foreach (SystemFontRecord record2 in SystemFontFinder.AvailableFonts.Value)
				{
					TrueTypeFont result3;
					if (this.TryGetTrueTypeFont(name, record2, out result3))
					{
						return result3;
					}
				}
				return null;
			}
			TrueTypeFont result4;
			if (this.TryReadFile(fileName, false, name, out result4))
			{
				return result4;
			}
			return null;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000DCDC File Offset: 0x0000BEDC
		private bool TryGetTrueTypeFont(string name, SystemFontRecord record, out TrueTypeFont font)
		{
			font = null;
			if (record.Type == SystemFontType.TrueType)
			{
				object obj = this.readFilesLock;
				lock (obj)
				{
					if (this.readFiles.Contains(record.Path))
					{
						return false;
					}
				}
				return this.TryReadFile(record.Path, true, name, out font);
			}
			return false;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000DD50 File Offset: 0x0000BF50
		private bool TryReadFile(string fileName, bool readNameFirst, string fontName, out TrueTypeFont font)
		{
			font = null;
			TrueTypeDataBytes trueTypeDataBytes = new TrueTypeDataBytes(new ByteArrayInputBytes(File.ReadAllBytes(fileName)));
			object cacheLock;
			if (readNameFirst)
			{
				NameTable nameTable = TrueTypeFontParser.GetNameTable(trueTypeDataBytes);
				if (nameTable == null)
				{
					cacheLock = this.readFilesLock;
					lock (cacheLock)
					{
						this.readFiles.Add(fileName);
					}
					return false;
				}
				string text = nameTable.GetPostscriptName() ?? nameTable.FontName;
				this.nameToFileNameMap.TryAdd(text, fileName);
				if (!string.Equals(text, fontName, 5))
				{
					cacheLock = this.readFilesLock;
					lock (cacheLock)
					{
						this.readFiles.Add(fileName);
					}
					return false;
				}
			}
			trueTypeDataBytes.Seek(0L);
			font = TrueTypeFontParser.Parse(trueTypeDataBytes);
			NameTable nameTable2 = font.TableRegister.NameTable;
			string text2 = ((nameTable2 != null) ? nameTable2.GetPostscriptName() : null) ?? font.Name;
			cacheLock = SystemFontFinder.CacheLock;
			lock (cacheLock)
			{
				if (!SystemFontFinder.Cache.ContainsKey(text2))
				{
					SystemFontFinder.Cache[text2] = font;
				}
			}
			cacheLock = this.readFilesLock;
			lock (cacheLock)
			{
				this.readFiles.Add(fileName);
			}
			return true;
		}

		// Token: 0x040002C8 RID: 712
		private static readonly IReadOnlyDictionary<string, string[]> NameSubstitutes;

		// Token: 0x040002C9 RID: 713
		private static readonly Lazy<IReadOnlyList<SystemFontRecord>> AvailableFonts;

		// Token: 0x040002CA RID: 714
		private static readonly object CacheLock = new object();

		// Token: 0x040002CB RID: 715
		private static readonly Dictionary<string, TrueTypeFont> Cache = new Dictionary<string, TrueTypeFont>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040002CC RID: 716
		public static readonly ISystemFontFinder Instance = new SystemFontFinder();

		// Token: 0x040002CD RID: 717
		private readonly ConcurrentDictionary<string, string> nameToFileNameMap = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040002CE RID: 718
		private readonly object readFilesLock = new object();

		// Token: 0x040002CF RID: 719
		private readonly HashSet<string> readFiles = new HashSet<string>();
	}
}
