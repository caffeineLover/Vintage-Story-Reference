using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.TrueType.Parser;
using UglyToad.PdfPig.Fonts.TrueType.Tables;
using UglyToad.PdfPig.Fonts.TrueType.Tables.CMapSubTables;

namespace UglyToad.PdfPig.Fonts.TrueType
{
	// Token: 0x02000033 RID: 51
	public class TrueTypeFont
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600014B RID: 331 RVA: 0x00007BEB File Offset: 0x00005DEB
		public float Version { get; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00007BF3 File Offset: 0x00005DF3
		public IReadOnlyDictionary<string, TrueTypeHeaderTable> TableHeaders { get; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600014D RID: 333 RVA: 0x00007BFB File Offset: 0x00005DFB
		public TableRegister TableRegister { get; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00007C03 File Offset: 0x00005E03
		public string Name
		{
			get
			{
				NameTable nameTable = this.TableRegister.NameTable;
				if (nameTable == null)
				{
					return null;
				}
				return nameTable.FontName;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00007C1B File Offset: 0x00005E1B
		public ICMapSubTable WindowsUnicodeCMap { get; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00007C23 File Offset: 0x00005E23
		public ICMapSubTable MacRomanCMap { get; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00007C2B File Offset: 0x00005E2B
		public ICMapSubTable WindowsSymbolCMap { get; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00007C33 File Offset: 0x00005E33
		public int NumberOfTables { get; }

		// Token: 0x06000153 RID: 339 RVA: 0x00007C3C File Offset: 0x00005E3C
		internal TrueTypeFont(float version, IReadOnlyDictionary<string, TrueTypeHeaderTable> tableHeaders, TableRegister tableRegister)
		{
			this.Version = version;
			if (tableHeaders == null)
			{
				throw new ArgumentNullException("tableHeaders");
			}
			this.TableHeaders = tableHeaders;
			if (tableRegister == null)
			{
				throw new ArgumentNullException("tableRegister");
			}
			this.TableRegister = tableRegister;
			this.NumberOfTables = tableHeaders.Count;
			if (this.TableRegister.CMapTable != null)
			{
				foreach (ICMapSubTable icmapSubTable in this.TableRegister.CMapTable.SubTables)
				{
					if (this.WindowsSymbolCMap == null && icmapSubTable.PlatformId == TrueTypeCMapPlatform.Windows && icmapSubTable.EncodingId == 0)
					{
						this.WindowsSymbolCMap = icmapSubTable;
					}
					else if (this.WindowsUnicodeCMap == null && icmapSubTable.PlatformId == TrueTypeCMapPlatform.Windows && icmapSubTable.EncodingId == 1)
					{
						this.WindowsUnicodeCMap = icmapSubTable;
					}
					else if (this.MacRomanCMap == null && icmapSubTable.PlatformId == TrueTypeCMapPlatform.Macintosh && icmapSubTable.EncodingId == 0)
					{
						this.MacRomanCMap = icmapSubTable;
					}
				}
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00007D48 File Offset: 0x00005F48
		public bool TryGetBoundingBox(int characterCode, out PdfRectangle boundingBox)
		{
			return this.TryGetBoundingBox(characterCode, null, out boundingBox);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00007D54 File Offset: 0x00005F54
		public bool TryGetBoundingBox(int characterCode, Func<int, int?> characterCodeToGlyphId, out PdfRectangle boundingBox)
		{
			boundingBox = default(PdfRectangle);
			int num;
			if (!this.TryGetGlyphIndex(characterCode, characterCodeToGlyphId, out num) || this.TableRegister.GlyphTable == null)
			{
				return false;
			}
			if (!this.TableRegister.GlyphTable.TryGetGlyphBounds(num, out boundingBox))
			{
				return false;
			}
			double num2;
			if (boundingBox.Width.Equals(0.0) && this.TryGetBoundingAdvancedWidthByIndex(num, out num2))
			{
				boundingBox = new PdfRectangle(0.0, 0.0, num2, 0.0);
			}
			return true;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00007DE5 File Offset: 0x00005FE5
		public bool TryGetPath(int characterCode, out IReadOnlyList<PdfSubpath> path)
		{
			return this.TryGetPath(characterCode, null, out path);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00007DF0 File Offset: 0x00005FF0
		public bool TryGetPath(int characterCode, Func<int, int?> characterCodeToGlyphId, out IReadOnlyList<PdfSubpath> path)
		{
			path = EmptyArray<PdfSubpath>.Instance;
			int num;
			return this.TryGetGlyphIndex(characterCode, characterCodeToGlyphId, out num) && this.TableRegister.GlyphTable != null && this.TableRegister.GlyphTable.Glyphs[num].TryGetGlyphPath(out path);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00007E3B File Offset: 0x0000603B
		public bool TryGetAdvanceWidth(int characterCode, out double width)
		{
			return this.TryGetAdvanceWidth(characterCode, null, out width);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00007E48 File Offset: 0x00006048
		public bool TryGetAdvanceWidth(int characterCode, Func<int, int?> characterCodeToGlyphId, out double width)
		{
			width = 0.0;
			int index;
			return this.TryGetGlyphIndex(characterCode, characterCodeToGlyphId, out index) && this.TryGetBoundingAdvancedWidthByIndex(index, out width);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00007E76 File Offset: 0x00006076
		public int GetUnitsPerEm()
		{
			return (int)this.TableRegister.HeaderTable.UnitsPerEm;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00007E88 File Offset: 0x00006088
		private bool TryGetBoundingAdvancedWidthByIndex(int index, out double width)
		{
			width = 0.0;
			if (this.TableRegister.HorizontalMetricsTable == null)
			{
				return false;
			}
			width = (double)this.TableRegister.HorizontalMetricsTable.GetAdvanceWidth(index);
			return true;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00007EBC File Offset: 0x000060BC
		private bool TryGetGlyphIndex(int characterIdentifier, Func<int, int?> characterCodeToGlyphId, out int glyphId)
		{
			glyphId = 0;
			int? num = (characterCodeToGlyphId != null) ? characterCodeToGlyphId.Invoke(characterIdentifier) : default(int?);
			if (num != null)
			{
				glyphId = num.Value;
				return true;
			}
			return this.TableRegister.CMapTable != null && this.TableRegister.CMapTable.TryGetGlyphIndex(characterIdentifier, out glyphId);
		}
	}
}
