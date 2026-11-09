using System;

namespace RPVoiceChat.Audio
{
	// Token: 0x020000FD RID: 253
	public interface IAudioCodec
	{
		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060008FC RID: 2300
		string Name { get; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060008FD RID: 2301
		int SampleRate { get; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060008FE RID: 2302
		int Channels { get; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060008FF RID: 2303
		int FrameSize { get; }

		// Token: 0x06000900 RID: 2304
		byte[] Encode(short[] pcmData);

		// Token: 0x06000901 RID: 2305
		byte[] Decode(byte[] encodedData);

		// Token: 0x06000902 RID: 2306 RVA: 0x0003260E File Offset: 0x0003080E
		byte[] EncodeForBroadcast(short[] pcmData)
		{
			return this.Encode(pcmData);
		}
	}
}
