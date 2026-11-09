using System;

namespace Xabe.FFmpeg
{
	// Token: 0x02000028 RID: 40
	internal class ProbeModel
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00005B3F File Offset: 0x00003D3F
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00005B47 File Offset: 0x00003D47
		public ProbeModel.Stream[] Streams { get; set; }

		// Token: 0x0200009C RID: 156
		public class Stream
		{
			// Token: 0x1700007C RID: 124
			// (get) Token: 0x060002E3 RID: 739 RVA: 0x0000BFA4 File Offset: 0x0000A1A4
			// (set) Token: 0x060002E4 RID: 740 RVA: 0x0000BFAC File Offset: 0x0000A1AC
			public string CodecName { get; set; }

			// Token: 0x1700007D RID: 125
			// (get) Token: 0x060002E5 RID: 741 RVA: 0x0000BFB5 File Offset: 0x0000A1B5
			// (set) Token: 0x060002E6 RID: 742 RVA: 0x0000BFBD File Offset: 0x0000A1BD
			public int Height { get; set; }

			// Token: 0x1700007E RID: 126
			// (get) Token: 0x060002E7 RID: 743 RVA: 0x0000BFC6 File Offset: 0x0000A1C6
			// (set) Token: 0x060002E8 RID: 744 RVA: 0x0000BFCE File Offset: 0x0000A1CE
			public int Width { get; set; }

			// Token: 0x1700007F RID: 127
			// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000BFD7 File Offset: 0x0000A1D7
			// (set) Token: 0x060002EA RID: 746 RVA: 0x0000BFDF File Offset: 0x0000A1DF
			public string CodecType { get; set; }

			// Token: 0x17000080 RID: 128
			// (get) Token: 0x060002EB RID: 747 RVA: 0x0000BFE8 File Offset: 0x0000A1E8
			// (set) Token: 0x060002EC RID: 748 RVA: 0x0000BFF0 File Offset: 0x0000A1F0
			public string RFrameRate { get; set; }

			// Token: 0x17000081 RID: 129
			// (get) Token: 0x060002ED RID: 749 RVA: 0x0000BFF9 File Offset: 0x0000A1F9
			// (set) Token: 0x060002EE RID: 750 RVA: 0x0000C001 File Offset: 0x0000A201
			public double Duration { get; set; }

			// Token: 0x17000082 RID: 130
			// (get) Token: 0x060002EF RID: 751 RVA: 0x0000C00A File Offset: 0x0000A20A
			// (set) Token: 0x060002F0 RID: 752 RVA: 0x0000C012 File Offset: 0x0000A212
			public long BitRate { get; set; }

			// Token: 0x17000083 RID: 131
			// (get) Token: 0x060002F1 RID: 753 RVA: 0x0000C01B File Offset: 0x0000A21B
			// (set) Token: 0x060002F2 RID: 754 RVA: 0x0000C023 File Offset: 0x0000A223
			public int Index { get; set; }

			// Token: 0x17000084 RID: 132
			// (get) Token: 0x060002F3 RID: 755 RVA: 0x0000C02C File Offset: 0x0000A22C
			// (set) Token: 0x060002F4 RID: 756 RVA: 0x0000C034 File Offset: 0x0000A234
			public int Channels { get; set; }

			// Token: 0x17000085 RID: 133
			// (get) Token: 0x060002F5 RID: 757 RVA: 0x0000C03D File Offset: 0x0000A23D
			// (set) Token: 0x060002F6 RID: 758 RVA: 0x0000C045 File Offset: 0x0000A245
			public int SampleRate { get; set; }

			// Token: 0x17000086 RID: 134
			// (get) Token: 0x060002F7 RID: 759 RVA: 0x0000C04E File Offset: 0x0000A24E
			// (set) Token: 0x060002F8 RID: 760 RVA: 0x0000C056 File Offset: 0x0000A256
			public string PixFmt { get; set; }

			// Token: 0x17000087 RID: 135
			// (get) Token: 0x060002F9 RID: 761 RVA: 0x0000C05F File Offset: 0x0000A25F
			// (set) Token: 0x060002FA RID: 762 RVA: 0x0000C067 File Offset: 0x0000A267
			public ProbeModel.Tags Tags { get; set; }

			// Token: 0x17000088 RID: 136
			// (get) Token: 0x060002FB RID: 763 RVA: 0x0000C070 File Offset: 0x0000A270
			// (set) Token: 0x060002FC RID: 764 RVA: 0x0000C078 File Offset: 0x0000A278
			public string NbFrames { get; set; }

			// Token: 0x17000089 RID: 137
			// (get) Token: 0x060002FD RID: 765 RVA: 0x0000C081 File Offset: 0x0000A281
			// (set) Token: 0x060002FE RID: 766 RVA: 0x0000C089 File Offset: 0x0000A289
			public ProbeModel.Disposition Disposition { get; set; }
		}

		// Token: 0x0200009D RID: 157
		internal class Tags
		{
			// Token: 0x1700008A RID: 138
			// (get) Token: 0x06000300 RID: 768 RVA: 0x0000C09A File Offset: 0x0000A29A
			// (set) Token: 0x06000301 RID: 769 RVA: 0x0000C0A2 File Offset: 0x0000A2A2
			public string Language { get; set; }

			// Token: 0x1700008B RID: 139
			// (get) Token: 0x06000302 RID: 770 RVA: 0x0000C0AB File Offset: 0x0000A2AB
			// (set) Token: 0x06000303 RID: 771 RVA: 0x0000C0B3 File Offset: 0x0000A2B3
			public string Title { get; set; }

			// Token: 0x1700008C RID: 140
			// (get) Token: 0x06000304 RID: 772 RVA: 0x0000C0BC File Offset: 0x0000A2BC
			// (set) Token: 0x06000305 RID: 773 RVA: 0x0000C0C4 File Offset: 0x0000A2C4
			public int? Rotate { get; set; }
		}

		// Token: 0x0200009E RID: 158
		internal class Disposition
		{
			// Token: 0x1700008D RID: 141
			// (get) Token: 0x06000307 RID: 775 RVA: 0x0000C0D5 File Offset: 0x0000A2D5
			// (set) Token: 0x06000308 RID: 776 RVA: 0x0000C0DD File Offset: 0x0000A2DD
			public int Default { get; set; }

			// Token: 0x1700008E RID: 142
			// (get) Token: 0x06000309 RID: 777 RVA: 0x0000C0E6 File Offset: 0x0000A2E6
			// (set) Token: 0x0600030A RID: 778 RVA: 0x0000C0EE File Offset: 0x0000A2EE
			public int Forced { get; set; }
		}
	}
}
