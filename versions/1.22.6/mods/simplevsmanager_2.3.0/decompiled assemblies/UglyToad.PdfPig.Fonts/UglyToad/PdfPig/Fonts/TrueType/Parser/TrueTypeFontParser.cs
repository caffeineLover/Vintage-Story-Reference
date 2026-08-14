using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Fonts.TrueType.Tables;

namespace UglyToad.PdfPig.Fonts.TrueType.Parser
{
	// Token: 0x0200005D RID: 93
	public static class TrueTypeFontParser
	{
		// Token: 0x06000270 RID: 624 RVA: 0x0000C9FC File Offset: 0x0000ABFC
		public static TrueTypeFont Parse(TrueTypeDataBytes data)
		{
			float version = data.Read32Fixed();
			int num = (int)data.ReadUnsignedShort();
			data.ReadUnsignedShort();
			data.ReadUnsignedShort();
			data.ReadUnsignedShort();
			Dictionary<string, TrueTypeHeaderTable> dictionary = new Dictionary<string, TrueTypeHeaderTable>(StringComparer.OrdinalIgnoreCase);
			for (int i = 0; i < num; i++)
			{
				TrueTypeHeaderTable? trueTypeHeaderTable = TrueTypeFontParser.ReadTable(data);
				if (trueTypeHeaderTable != null)
				{
					dictionary[trueTypeHeaderTable.Value.Tag] = trueTypeHeaderTable.Value;
				}
			}
			return TrueTypeFontParser.ParseTables(version, dictionary, data);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000CA7C File Offset: 0x0000AC7C
		private static TrueTypeHeaderTable? ReadTable(TrueTypeDataBytes data)
		{
			string text = data.ReadTag();
			uint checkSum = data.ReadUnsignedInt();
			uint offset = data.ReadUnsignedInt();
			uint num = data.ReadUnsignedInt();
			if (num == 0U && !string.Equals(text, "glyf", 5))
			{
				return default(TrueTypeHeaderTable?);
			}
			return new TrueTypeHeaderTable?(new TrueTypeHeaderTable(text, checkSum, offset, num));
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000CAD0 File Offset: 0x0000ACD0
		private static TrueTypeFont ParseTables(float version, IReadOnlyDictionary<string, TrueTypeHeaderTable> tables, TrueTypeDataBytes data)
		{
			bool flag = tables.ContainsKey("cff ");
			TableRegister.Builder builder = new TableRegister.Builder();
			TrueTypeHeaderTable table;
			if (!tables.TryGetValue("head", ref table))
			{
				throw new InvalidFontFormatException("The head table is required.");
			}
			builder.HeaderTable = HeaderTable.Load(data, table);
			TrueTypeHeaderTable table2;
			if (!tables.TryGetValue("hhea", ref table2))
			{
				throw new InvalidFontFormatException("The horizontal header table is required.");
			}
			builder.HorizontalHeaderTable = TableParser.Parse<HorizontalHeaderTable>(table2, data, builder);
			TrueTypeHeaderTable table3;
			if (!tables.TryGetValue("maxp", ref table3))
			{
				throw new InvalidFontFormatException("The maximum profile table is required.");
			}
			builder.MaximumProfileTable = BasicMaximumProfileTable.Load(data, table3);
			TrueTypeHeaderTable table4;
			if (tables.TryGetValue("post", ref table4))
			{
				builder.PostScriptTable = PostScriptTable.Load(data, table4, builder.MaximumProfileTable);
			}
			TrueTypeHeaderTable table5;
			if (tables.TryGetValue("name", ref table5))
			{
				builder.NameTable = TableParser.Parse<NameTable>(table5, data, builder);
			}
			TrueTypeHeaderTable table6;
			if (tables.TryGetValue("OS/2", ref table6))
			{
				builder.Os2Table = TableParser.Parse<Os2Table>(table6, data, builder);
			}
			if (!flag)
			{
				TrueTypeHeaderTable table7;
				if (!tables.TryGetValue("loca", ref table7))
				{
					throw new InvalidFontFormatException("The location to index table is required for non-PostScript fonts.");
				}
				builder.IndexToLocationTable = IndexToLocationTable.Load(data, table7, builder);
				TrueTypeHeaderTable table8;
				if (!tables.TryGetValue("glyf", ref table8))
				{
					throw new InvalidFontFormatException("The glyph table is required for non-PostScript fonts.");
				}
				builder.GlyphDataTable = GlyphDataTable.Load(data, table8, builder);
				TrueTypeFontParser.OptionallyParseTables(tables, data, builder);
			}
			return new TrueTypeFont(version, tables, builder.Build());
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000CC2C File Offset: 0x0000AE2C
		internal static NameTable GetNameTable(TrueTypeDataBytes data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			data.Read32Fixed();
			int num = (int)data.ReadUnsignedShort();
			data.ReadUnsignedShort();
			data.ReadUnsignedShort();
			data.ReadUnsignedShort();
			TrueTypeHeaderTable? trueTypeHeaderTable = default(TrueTypeHeaderTable?);
			for (int i = 0; i < num; i++)
			{
				TrueTypeHeaderTable? trueTypeHeaderTable2 = TrueTypeFontParser.ReadTable(data);
				if (trueTypeHeaderTable2 != null && trueTypeHeaderTable2.Value.Tag == "name")
				{
					trueTypeHeaderTable = trueTypeHeaderTable2;
					break;
				}
			}
			if (trueTypeHeaderTable == null)
			{
				return null;
			}
			return TableParser.Parse<NameTable>(trueTypeHeaderTable.Value, data, new TableRegister.Builder());
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000CCCC File Offset: 0x0000AECC
		private static void OptionallyParseTables(IReadOnlyDictionary<string, TrueTypeHeaderTable> tables, TrueTypeDataBytes data, TableRegister.Builder tableRegister)
		{
			TrueTypeHeaderTable table;
			if (tables.TryGetValue("cmap", ref table))
			{
				tableRegister.CMapTable = TableParser.Parse<CMapTable>(table, data, tableRegister);
			}
			TrueTypeHeaderTable table2;
			if (tables.TryGetValue("hmtx", ref table2))
			{
				tableRegister.HorizontalMetricsTable = TableParser.Parse<HorizontalMetricsTable>(table2, data, tableRegister);
			}
			TrueTypeHeaderTable headerTable;
			if (tables.TryGetValue("kern", ref headerTable))
			{
				tableRegister.KerningTable = KerningTable.Load(data, headerTable);
			}
		}
	}
}
