using System;

namespace Xabe.FFmpeg
{
	// Token: 0x02000027 RID: 39
	internal class FormatModel
	{
		// Token: 0x0600011F RID: 287 RVA: 0x00005B37 File Offset: 0x00003D37
		private FormatModel()
		{
		}

		// Token: 0x02000099 RID: 153
		internal class Root
		{
			// Token: 0x17000076 RID: 118
			// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000BF26 File Offset: 0x0000A126
			// (set) Token: 0x060002D5 RID: 725 RVA: 0x0000BF2E File Offset: 0x0000A12E
			public FormatModel.Format format { get; set; }
		}

		// Token: 0x0200009A RID: 154
		internal class Tags
		{
			// Token: 0x17000077 RID: 119
			// (get) Token: 0x060002D7 RID: 727 RVA: 0x0000BF3F File Offset: 0x0000A13F
			// (set) Token: 0x060002D8 RID: 728 RVA: 0x0000BF47 File Offset: 0x0000A147
			public string creation_time { get; set; }
		}

		// Token: 0x0200009B RID: 155
		internal class Format
		{
			// Token: 0x17000078 RID: 120
			// (get) Token: 0x060002DA RID: 730 RVA: 0x0000BF58 File Offset: 0x0000A158
			// (set) Token: 0x060002DB RID: 731 RVA: 0x0000BF60 File Offset: 0x0000A160
			public string size { get; set; }

			// Token: 0x17000079 RID: 121
			// (get) Token: 0x060002DC RID: 732 RVA: 0x0000BF69 File Offset: 0x0000A169
			// (set) Token: 0x060002DD RID: 733 RVA: 0x0000BF71 File Offset: 0x0000A171
			public long bit_Rate { get; set; }

			// Token: 0x1700007A RID: 122
			// (get) Token: 0x060002DE RID: 734 RVA: 0x0000BF7A File Offset: 0x0000A17A
			// (set) Token: 0x060002DF RID: 735 RVA: 0x0000BF82 File Offset: 0x0000A182
			public double duration { get; set; }

			// Token: 0x1700007B RID: 123
			// (get) Token: 0x060002E0 RID: 736 RVA: 0x0000BF8B File Offset: 0x0000A18B
			// (set) Token: 0x060002E1 RID: 737 RVA: 0x0000BF93 File Offset: 0x0000A193
			public FormatModel.Tags tags { get; set; }
		}
	}
}
