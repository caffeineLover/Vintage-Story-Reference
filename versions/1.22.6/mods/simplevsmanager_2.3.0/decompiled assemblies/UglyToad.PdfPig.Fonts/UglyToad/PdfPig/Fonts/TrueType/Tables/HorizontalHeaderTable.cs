using System;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables
{
	// Token: 0x02000039 RID: 57
	public class HorizontalHeaderTable : ITrueTypeTable
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600018D RID: 397 RVA: 0x0000976C File Offset: 0x0000796C
		public string Tag
		{
			get
			{
				return "hhea";
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00009773 File Offset: 0x00007973
		public TrueTypeHeaderTable DirectoryTable { get; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600018F RID: 399 RVA: 0x0000977B File Offset: 0x0000797B
		public int MajorVersion { get; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00009783 File Offset: 0x00007983
		public int MinorVersion { get; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000191 RID: 401 RVA: 0x0000978B File Offset: 0x0000798B
		public short Ascent { get; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00009793 File Offset: 0x00007993
		public short Descent { get; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000193 RID: 403 RVA: 0x0000979B File Offset: 0x0000799B
		public short LineGap { get; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000194 RID: 404 RVA: 0x000097A3 File Offset: 0x000079A3
		public ushort AdvanceWidthMaximum { get; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000195 RID: 405 RVA: 0x000097AB File Offset: 0x000079AB
		public short MinimumLeftSideBearing { get; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000196 RID: 406 RVA: 0x000097B3 File Offset: 0x000079B3
		public short MinimumRightSideBearing { get; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000197 RID: 407 RVA: 0x000097BB File Offset: 0x000079BB
		public short XMaxExtent { get; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000198 RID: 408 RVA: 0x000097C3 File Offset: 0x000079C3
		public short CaretSlopeRise { get; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000199 RID: 409 RVA: 0x000097CB File Offset: 0x000079CB
		public short CaretSlopeRun { get; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600019A RID: 410 RVA: 0x000097D3 File Offset: 0x000079D3
		public short CaretOffset { get; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600019B RID: 411 RVA: 0x000097DB File Offset: 0x000079DB
		public short MetricDataFormat { get; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600019C RID: 412 RVA: 0x000097E3 File Offset: 0x000079E3
		public ushort NumberOfHeaderMetrics { get; }

		// Token: 0x0600019D RID: 413 RVA: 0x000097EC File Offset: 0x000079EC
		public HorizontalHeaderTable(TrueTypeHeaderTable directoryTable, int majorVersion, int minorVersion, short ascent, short descent, short lineGap, ushort advanceWidthMaximum, short minimumLeftSideBearing, short minimumRightSideBearing, short xMaxExtent, short caretSlopeRise, short caretSlopeRun, short caretOffset, short metricDataFormat, ushort numberOfHeaderMetrics)
		{
			this.DirectoryTable = directoryTable;
			this.MajorVersion = majorVersion;
			this.MinorVersion = minorVersion;
			this.Ascent = ascent;
			this.Descent = descent;
			this.LineGap = lineGap;
			this.AdvanceWidthMaximum = advanceWidthMaximum;
			this.MinimumLeftSideBearing = minimumLeftSideBearing;
			this.MinimumRightSideBearing = minimumRightSideBearing;
			this.XMaxExtent = xMaxExtent;
			this.CaretSlopeRise = caretSlopeRise;
			this.CaretSlopeRun = caretSlopeRun;
			this.CaretOffset = caretOffset;
			this.MetricDataFormat = metricDataFormat;
			this.NumberOfHeaderMetrics = numberOfHeaderMetrics;
		}
	}
}
