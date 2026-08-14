using System;

namespace RPVoiceChat.Audio
{
	// Token: 0x020000C4 RID: 196
	public interface IAudioCodec
	{
		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060006E4 RID: 1764
		string Name { get; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060006E5 RID: 1765
		int SampleRate { get; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060006E6 RID: 1766
		int Channels { get; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060006E7 RID: 1767
		int FrameSize { get; }

		// Token: 0x060006E8 RID: 1768
		byte[] Encode(short[] pcmData);

		// Token: 0x060006E9 RID: 1769
		byte[] Decode(byte[] encodedData);

		// Token: 0x060006EA RID: 1770 RVA: 0x0002717A File Offset: 0x0002537A
		byte[] EncodeForBroadcast(short[] pcmData)
		{
			return this.Encode(pcmData);
		}
	}
}
