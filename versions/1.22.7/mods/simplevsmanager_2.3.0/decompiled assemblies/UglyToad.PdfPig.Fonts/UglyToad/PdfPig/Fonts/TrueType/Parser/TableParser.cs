using System;
using System.Diagnostics;
using UglyToad.PdfPig.Fonts.TrueType.Tables;

namespace UglyToad.PdfPig.Fonts.TrueType.Parser
{
	// Token: 0x0200005B RID: 91
	internal static class TableParser
	{
		// Token: 0x06000262 RID: 610 RVA: 0x0000C760 File Offset: 0x0000A960
		public static T Parse<T>(TrueTypeHeaderTable table, TrueTypeDataBytes data, TableRegister.Builder register) where T : ITrueTypeTable
		{
			uint num = 0U;
			uint num2 = (table.Length + 3U) / 4U;
			data.Seek((long)((ulong)table.Offset));
			while (num2-- > 0U)
			{
				num += (uint)data.ReadSignedInt();
			}
			if (num != table.CheckSum)
			{
				Trace.TraceWarning("Table with invalid checksum found in TrueType font file.");
			}
			if (typeof(T) == typeof(CMapTable))
			{
				return (T)((object)TableParser.CMapTableParser.Parse(table, data, register));
			}
			if (typeof(T) == typeof(HorizontalMetricsTable))
			{
				return (T)((object)TableParser.HorizontalMetricsTableParser.Parse(table, data, register));
			}
			if (typeof(T) == typeof(NameTable))
			{
				return (T)((object)TableParser.NameTableParser.Parse(table, data, register));
			}
			if (typeof(T) == typeof(Os2Table))
			{
				return (T)((object)TableParser.Os2TableParser.Parse(table, data, register));
			}
			if (typeof(T) == typeof(HorizontalHeaderTable))
			{
				return (T)((object)TableParser.HorizontalHeaderTableParser.Parse(table, data, register));
			}
			throw new NotImplementedException();
		}

		// Token: 0x040001DD RID: 477
		private static readonly CMapTableParser CMapTableParser = new CMapTableParser();

		// Token: 0x040001DE RID: 478
		private static readonly HorizontalMetricsTableParser HorizontalMetricsTableParser = new HorizontalMetricsTableParser();

		// Token: 0x040001DF RID: 479
		private static readonly HorizontalHeaderTableParser HorizontalHeaderTableParser = new HorizontalHeaderTableParser();

		// Token: 0x040001E0 RID: 480
		private static readonly NameTableParser NameTableParser = new NameTableParser();

		// Token: 0x040001E1 RID: 481
		private static readonly Os2TableParser Os2TableParser = new Os2TableParser();
	}
}
