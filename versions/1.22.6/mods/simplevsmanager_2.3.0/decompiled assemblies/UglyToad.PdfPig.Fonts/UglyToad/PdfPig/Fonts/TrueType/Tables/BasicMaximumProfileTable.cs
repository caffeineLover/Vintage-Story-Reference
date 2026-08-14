using System;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables
{
	// Token: 0x0200003E RID: 62
	internal class BasicMaximumProfileTable : ITrueTypeTable
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00009CEE File Offset: 0x00007EEE
		public string Tag
		{
			get
			{
				return "maxp";
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00009CF5 File Offset: 0x00007EF5
		public TrueTypeHeaderTable DirectoryTable { get; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00009CFD File Offset: 0x00007EFD
		public bool IsCompressedFontFormat
		{
			get
			{
				return this.Version == 0.5m;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00009D14 File Offset: 0x00007F14
		public decimal Version { get; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00009D1C File Offset: 0x00007F1C
		public int NumberOfGlyphs { get; }

		// Token: 0x060001B8 RID: 440 RVA: 0x00009D24 File Offset: 0x00007F24
		public BasicMaximumProfileTable(TrueTypeHeaderTable directoryTable, float version, int numberOfGlyphs)
		{
			this.DirectoryTable = directoryTable;
			this.Version = (decimal)version;
			this.NumberOfGlyphs = numberOfGlyphs;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00009D48 File Offset: 0x00007F48
		public static BasicMaximumProfileTable Load(TrueTypeDataBytes data, TrueTypeHeaderTable table)
		{
			data.Seek((long)((ulong)table.Offset));
			float num = data.Read32Fixed();
			ushort numberOfGlyphs = data.ReadUnsignedShort();
			if (Math.Abs((double)num - 0.5) < 1.401298464324817E-45)
			{
				return new BasicMaximumProfileTable(table, num, (int)numberOfGlyphs);
			}
			ushort maximumPoints = data.ReadUnsignedShort();
			ushort maximumContours = data.ReadUnsignedShort();
			ushort maximumCompositePoints = data.ReadUnsignedShort();
			ushort maximumCompositeContours = data.ReadUnsignedShort();
			ushort maximumZones = data.ReadUnsignedShort();
			ushort maximumTwilightPoints = data.ReadUnsignedShort();
			ushort maximumStorage = data.ReadUnsignedShort();
			ushort maximumFunctionDefinitions = data.ReadUnsignedShort();
			ushort maximumInstructionDefinitions = data.ReadUnsignedShort();
			ushort maximumStackElements = data.ReadUnsignedShort();
			ushort maximumSizeOfInstructions = data.ReadUnsignedShort();
			ushort maximumComponentElements = data.ReadUnsignedShort();
			ushort maximumComponentDepth = data.ReadUnsignedShort();
			return new MaximumProfileTable(table, num, (int)numberOfGlyphs, (int)maximumPoints, (int)maximumContours, (int)maximumCompositePoints, (int)maximumCompositeContours, (int)maximumZones, (int)maximumTwilightPoints, (int)maximumStorage, (int)maximumFunctionDefinitions, (int)maximumInstructionDefinitions, (int)maximumStackElements, (int)maximumSizeOfInstructions, (int)maximumComponentElements, (int)maximumComponentDepth);
		}
	}
}
