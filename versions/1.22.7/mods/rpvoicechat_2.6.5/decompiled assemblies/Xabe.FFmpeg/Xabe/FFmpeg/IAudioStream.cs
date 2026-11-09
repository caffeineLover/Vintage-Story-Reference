using System;

namespace Xabe.FFmpeg
{
	// Token: 0x0200002D RID: 45
	public interface IAudioStream : IStream
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000159 RID: 345
		TimeSpan Duration { get; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600015A RID: 346
		long Bitrate { get; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600015B RID: 347
		int SampleRate { get; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600015C RID: 348
		int Channels { get; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600015D RID: 349
		string Language { get; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600015E RID: 350
		string Title { get; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600015F RID: 351
		int? Default { get; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000160 RID: 352
		int? Forced { get; }

		// Token: 0x06000161 RID: 353
		IAudioStream CopyStream();

		// Token: 0x06000162 RID: 354
		IAudioStream Reverse();

		// Token: 0x06000163 RID: 355
		IAudioStream SetChannels(int channels);

		// Token: 0x06000164 RID: 356
		IAudioStream SetCodec(AudioCodec codec);

		// Token: 0x06000165 RID: 357
		IAudioStream SetCodec(string codec);

		// Token: 0x06000166 RID: 358
		IAudioStream SetBitstreamFilter(BitstreamFilter filter);

		// Token: 0x06000167 RID: 359
		IAudioStream SetBitrate(long bitRate);

		// Token: 0x06000168 RID: 360
		IAudioStream SetBitrate(long minBitrate, long maxBitrate, long bufferSize);

		// Token: 0x06000169 RID: 361
		IAudioStream SetSampleRate(int sampleRate);

		// Token: 0x0600016A RID: 362
		IAudioStream ChangeSpeed(double multiplicator);

		// Token: 0x0600016B RID: 363
		IAudioStream Split(TimeSpan startTime, TimeSpan duration);

		// Token: 0x0600016C RID: 364
		IAudioStream SetSeek(TimeSpan? seek);

		// Token: 0x0600016D RID: 365
		IAudioStream SetBitstreamFilter(string filter);

		// Token: 0x0600016E RID: 366
		IAudioStream SetInputFormat(string inputFormat);

		// Token: 0x0600016F RID: 367
		IAudioStream SetInputFormat(Format inputFormat);

		// Token: 0x06000170 RID: 368
		IAudioStream UseNativeInputRead(bool readInputAtNativeFrameRate);

		// Token: 0x06000171 RID: 369
		IAudioStream SetStreamLoop(int loopCount);
	}
}
