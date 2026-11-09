using System;

namespace Xabe.FFmpeg.Events
{
	// Token: 0x0200004E RID: 78
	public class VideoDataEventArgs : EventArgs
	{
		// Token: 0x06000229 RID: 553 RVA: 0x000072AF File Offset: 0x000054AF
		public VideoDataEventArgs(byte[] data)
		{
			this.Data = data;
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600022A RID: 554 RVA: 0x000072BE File Offset: 0x000054BE
		public byte[] Data { get; }
	}
}
