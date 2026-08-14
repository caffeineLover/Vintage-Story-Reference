using System;
using RPVoiceChat.Util;
using Vintagestory.Common;

namespace RPVoiceChat.Audio
{
	// Token: 0x020000C3 RID: 195
	public class DummyCodec : IAudioCodec
	{
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x000270F5 File Offset: 0x000252F5
		public string Name { get; } = "Dummy";

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x000270FD File Offset: 0x000252FD
		public int SampleRate { get; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x00027105 File Offset: 0x00025305
		public int Channels { get; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x0002710D File Offset: 0x0002530D
		public int FrameSize { get; } = 1;

		// Token: 0x060006E1 RID: 1761 RVA: 0x00027115 File Offset: 0x00025315
		public DummyCodec(int frequency, int channelCount)
		{
			this.SampleRate = frequency;
			this.Channels = channelCount;
			this.compressor = new CompressionGzip();
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00027148 File Offset: 0x00025348
		public byte[] Encode(short[] pcmData)
		{
			byte[] encoded = AudioUtils.ShortsToBytes(pcmData, 0, pcmData.Length);
			return this.compressor.Compress(encoded);
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0002716C File Offset: 0x0002536C
		public byte[] Decode(byte[] encodedData)
		{
			return this.compressor.Decompress(encodedData);
		}

		// Token: 0x04000315 RID: 789
		public const string _Name = "Dummy";

		// Token: 0x0400031A RID: 794
		private ICompression compressor;
	}
}
