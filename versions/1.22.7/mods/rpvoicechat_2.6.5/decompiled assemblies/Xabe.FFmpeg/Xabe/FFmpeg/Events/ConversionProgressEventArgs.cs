using System;

namespace Xabe.FFmpeg.Events
{
	// Token: 0x0200004C RID: 76
	public class ConversionProgressEventArgs : EventArgs
	{
		// Token: 0x06000220 RID: 544 RVA: 0x0000723E File Offset: 0x0000543E
		public ConversionProgressEventArgs(TimeSpan timeSpan, TimeSpan totalTime, int processId)
		{
			this.Duration = timeSpan;
			this.TotalLength = totalTime;
			this.ProcessId = (long)processId;
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0000725C File Offset: 0x0000545C
		public TimeSpan Duration { get; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00007264 File Offset: 0x00005464
		public TimeSpan TotalLength { get; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000726C File Offset: 0x0000546C
		public long ProcessId { get; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00007274 File Offset: 0x00005474
		public int Percent
		{
			get
			{
				return (int)(Math.Round(this.Duration.TotalSeconds / this.TotalLength.TotalSeconds, 2) * 100.0);
			}
		}
	}
}
