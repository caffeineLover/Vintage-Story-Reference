using System;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables
{
	// Token: 0x0200003F RID: 63
	internal class MaximumProfileTable : BasicMaximumProfileTable
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00009E1C File Offset: 0x0000801C
		public int MaximumPoints { get; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00009E24 File Offset: 0x00008024
		public int MaximumContours { get; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060001BC RID: 444 RVA: 0x00009E2C File Offset: 0x0000802C
		public int MaximumCompositePoints { get; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00009E34 File Offset: 0x00008034
		public int MaximumCompositeContours { get; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00009E3C File Offset: 0x0000803C
		public int MaximumZones { get; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060001BF RID: 447 RVA: 0x00009E44 File Offset: 0x00008044
		public int MaximumTwilightPoints { get; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00009E4C File Offset: 0x0000804C
		public int MaximumStorage { get; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x00009E54 File Offset: 0x00008054
		public int MaximumFunctionDefinitions { get; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x00009E5C File Offset: 0x0000805C
		public int MaximumInstructionDefinitions { get; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00009E64 File Offset: 0x00008064
		public int MaximumStackElements { get; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x00009E6C File Offset: 0x0000806C
		public int MaximumSizeOfInstructions { get; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00009E74 File Offset: 0x00008074
		public int MaximumComponentElements { get; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x00009E7C File Offset: 0x0000807C
		public int MaximumComponentDepth { get; }

		// Token: 0x060001C7 RID: 455 RVA: 0x00009E84 File Offset: 0x00008084
		public MaximumProfileTable(TrueTypeHeaderTable directoryTable, float version, int numberOfGlyphs, int maximumPoints, int maximumContours, int maximumCompositePoints, int maximumCompositeContours, int maximumZones, int maximumTwilightPoints, int maximumStorage, int maximumFunctionDefinitions, int maximumInstructionDefinitions, int maximumStackElements, int maximumSizeOfInstructions, int maximumComponentElements, int maximumComponentDepth) : base(directoryTable, version, numberOfGlyphs)
		{
			this.MaximumPoints = maximumPoints;
			this.MaximumContours = maximumContours;
			this.MaximumCompositePoints = maximumCompositePoints;
			this.MaximumCompositeContours = maximumCompositeContours;
			this.MaximumZones = maximumZones;
			this.MaximumTwilightPoints = maximumTwilightPoints;
			this.MaximumStorage = maximumStorage;
			this.MaximumFunctionDefinitions = maximumFunctionDefinitions;
			this.MaximumInstructionDefinitions = maximumInstructionDefinitions;
			this.MaximumStackElements = maximumStackElements;
			this.MaximumSizeOfInstructions = maximumSizeOfInstructions;
			this.MaximumComponentElements = maximumComponentElements;
			this.MaximumComponentDepth = maximumComponentDepth;
		}
	}
}
