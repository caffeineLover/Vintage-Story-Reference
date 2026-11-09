using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Fonts.TrueType.Tables.Kerning;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables
{
	// Token: 0x0200003D RID: 61
	internal class KerningTable
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00009B75 File Offset: 0x00007D75
		public IReadOnlyList<KerningSubTable> KerningTables { get; }

		// Token: 0x060001AF RID: 431 RVA: 0x00009B80 File Offset: 0x00007D80
		public KerningTable(IReadOnlyList<KerningSubTable> kerningTables)
		{
			List<KerningSubTable> list = new List<KerningSubTable>();
			foreach (KerningSubTable kerningSubTable in kerningTables)
			{
				if (kerningSubTable != null)
				{
					list.Add(kerningSubTable);
				}
			}
			this.KerningTables = list;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00009BE0 File Offset: 0x00007DE0
		public static KerningTable Load(TrueTypeDataBytes data, TrueTypeHeaderTable headerTable)
		{
			data.Seek((long)((ulong)headerTable.Offset));
			data.ReadUnsignedShort();
			ushort num = data.ReadUnsignedShort();
			KerningSubTable[] array = new KerningSubTable[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				long position = data.Position;
				ushort version = data.ReadUnsignedShort();
				data.ReadUnsignedShort();
				KernCoverage coverage;
				int num2 = (int)(((coverage = (KernCoverage)data.ReadUnsignedShort()) & (KernCoverage)255) >> 8);
				if (num2 != 0)
				{
					if (num2 == 2)
					{
						array[i] = KerningTable.ReadFormat2Table((int)version, data, coverage, position);
					}
				}
				else
				{
					array[i] = KerningTable.ReadFormat0Table((int)version, data, coverage);
				}
			}
			return new KerningTable(array);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00009C74 File Offset: 0x00007E74
		private static KerningSubTable ReadFormat0Table(int version, TrueTypeDataBytes data, KernCoverage coverage)
		{
			ushort num = data.ReadUnsignedShort();
			data.ReadUnsignedShort();
			data.ReadUnsignedShort();
			data.ReadUnsignedShort();
			KernPair[] array = new KernPair[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				ushort leftGlyphIndex = data.ReadUnsignedShort();
				ushort rightGlyphIndex = data.ReadUnsignedShort();
				short value = data.ReadSignedShort();
				array[i] = new KernPair((int)leftGlyphIndex, (int)rightGlyphIndex, value);
			}
			return new KerningSubTable(version, coverage, array);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00009CE0 File Offset: 0x00007EE0
		private static KerningSubTable ReadFormat2Table(int version, TrueTypeDataBytes data, KernCoverage coverage, long tableStartOffset)
		{
			return null;
		}
	}
}
