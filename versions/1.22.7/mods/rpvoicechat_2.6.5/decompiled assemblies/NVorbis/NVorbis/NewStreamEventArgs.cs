using System;
using NVorbis.Contracts;

namespace NVorbis
{
	// Token: 0x0200000F RID: 15
	[Serializable]
	public class NewStreamEventArgs : EventArgs
	{
		// Token: 0x06000071 RID: 113 RVA: 0x0000434C File Offset: 0x0000254C
		public NewStreamEventArgs(IStreamDecoder streamDecoder)
		{
			if (streamDecoder == null)
			{
				throw new ArgumentNullException("streamDecoder");
			}
			this.StreamDecoder = streamDecoder;
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000072 RID: 114 RVA: 0x0000436A File Offset: 0x0000256A
		public IStreamDecoder StreamDecoder { get; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00004372 File Offset: 0x00002572
		// (set) Token: 0x06000074 RID: 116 RVA: 0x0000437A File Offset: 0x0000257A
		public bool IgnoreStream { get; set; }
	}
}
