using System;

namespace Xabe.FFmpeg
{
	// Token: 0x02000016 RID: 22
	internal class ConversionResult : IConversionResult
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00004540 File Offset: 0x00002740
		// (set) Token: 0x060000AD RID: 173 RVA: 0x00004548 File Offset: 0x00002748
		public DateTime StartTime { get; internal set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00004551 File Offset: 0x00002751
		// (set) Token: 0x060000AF RID: 175 RVA: 0x00004559 File Offset: 0x00002759
		public DateTime EndTime { get; internal set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00004562 File Offset: 0x00002762
		public TimeSpan Duration
		{
			get
			{
				return this.EndTime - this.StartTime;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00004575 File Offset: 0x00002775
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x0000457D File Offset: 0x0000277D
		public string Arguments { get; internal set; }
	}
}
