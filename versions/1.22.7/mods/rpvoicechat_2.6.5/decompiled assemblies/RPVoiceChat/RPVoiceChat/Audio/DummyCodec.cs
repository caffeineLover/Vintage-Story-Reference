using System;
using RPVoiceChat.Util;
using Vintagestory.Common;

namespace RPVoiceChat.Audio
{
	// Token: 0x020000FC RID: 252
	public class DummyCodec : IAudioCodec
	{
		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x00032589 File Offset: 0x00030789
		public string Name { get; } = "Dummy";

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060008F6 RID: 2294 RVA: 0x00032591 File Offset: 0x00030791
		public int SampleRate { get; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060008F7 RID: 2295 RVA: 0x00032599 File Offset: 0x00030799
		public int Channels { get; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060008F8 RID: 2296 RVA: 0x000325A1 File Offset: 0x000307A1
		public int FrameSize { get; } = 1;

		// Token: 0x060008F9 RID: 2297 RVA: 0x000325A9 File Offset: 0x000307A9
		public DummyCodec(int frequency, int channelCount)
		{
			this.SampleRate = frequency;
			this.Channels = channelCount;
			this.compressor = new CompressionGzip();
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x000325DC File Offset: 0x000307DC
		public byte[] Encode(short[] pcmData)
		{
			byte[] encoded = AudioUtils.ShortsToBytes(pcmData, 0, pcmData.Length);
			return this.compressor.Compress(encoded);
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x00032600 File Offset: 0x00030800
		public byte[] Decode(byte[] encodedData)
		{
			return this.compressor.Decompress(encodedData);
		}

		// Token: 0x040003F4 RID: 1012
		public const string _Name = "Dummy";

		// Token: 0x040003F9 RID: 1017
		private ICompression compressor;
	}
}
