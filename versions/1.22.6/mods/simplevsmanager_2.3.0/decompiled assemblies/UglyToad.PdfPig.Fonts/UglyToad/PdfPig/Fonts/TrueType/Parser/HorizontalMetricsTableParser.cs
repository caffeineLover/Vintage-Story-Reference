using System;
using UglyToad.PdfPig.Fonts.TrueType.Glyphs;
using UglyToad.PdfPig.Fonts.TrueType.Tables;

namespace UglyToad.PdfPig.Fonts.TrueType.Parser
{
	// Token: 0x02000057 RID: 87
	internal class HorizontalMetricsTableParser : ITrueTypeTableParser<HorizontalMetricsTable>
	{
		// Token: 0x06000259 RID: 601 RVA: 0x0000C17C File Offset: 0x0000A37C
		public HorizontalMetricsTable Parse(TrueTypeHeaderTable header, TrueTypeDataBytes data, TableRegister.Builder register)
		{
			int numberOfGlyphs = register.MaximumProfileTable.NumberOfGlyphs;
			ushort numberOfHeaderMetrics = register.HorizontalHeaderTable.NumberOfHeaderMetrics;
			data.Seek((long)((ulong)header.Offset));
			int num = 0;
			HorizontalMetric[] array = new HorizontalMetric[(int)numberOfHeaderMetrics];
			for (int i = 0; i < (int)numberOfHeaderMetrics; i++)
			{
				ushort advanceWidth = data.ReadUnsignedShort();
				short leftSideBearing = data.ReadSignedShort();
				array[i] = new HorizontalMetric(advanceWidth, leftSideBearing);
				num += 4;
			}
			int num2 = numberOfGlyphs - (int)numberOfHeaderMetrics;
			if (num2 < 0)
			{
				num2 = numberOfGlyphs;
			}
			short[] array2 = new short[num2];
			int num3 = 0;
			while (num3 < array2.Length && (long)num < (long)((ulong)header.Length))
			{
				array2[num3] = data.ReadSignedShort();
				num += 2;
				num3++;
			}
			return new HorizontalMetricsTable(header, array, array2);
		}
	}
}
