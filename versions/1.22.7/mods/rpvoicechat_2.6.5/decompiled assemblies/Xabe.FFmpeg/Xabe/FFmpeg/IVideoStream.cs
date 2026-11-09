using System;

namespace Xabe.FFmpeg
{
	// Token: 0x02000037 RID: 55
	public interface IVideoStream : IStream
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001A3 RID: 419
		TimeSpan Duration { get; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001A4 RID: 420
		int Width { get; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001A5 RID: 421
		int Height { get; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001A6 RID: 422
		double Framerate { get; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001A7 RID: 423
		string Ratio { get; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001A8 RID: 424
		long Bitrate { get; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001A9 RID: 425
		int? Default { get; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001AA RID: 426
		int? Forced { get; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001AB RID: 427
		string PixelFormat { get; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001AC RID: 428
		int? Rotation { get; }

		// Token: 0x060001AD RID: 429
		IVideoStream Rotate(RotateDegrees rotateDegrees);

		// Token: 0x060001AE RID: 430
		IVideoStream Pad(int width, int height);

		// Token: 0x060001AF RID: 431
		IVideoStream ChangeSpeed(double multiplicator);

		// Token: 0x060001B0 RID: 432
		IVideoStream SetWatermark(string imagePath, Position position);

		// Token: 0x060001B1 RID: 433
		IVideoStream Reverse();

		// Token: 0x060001B2 RID: 434
		IVideoStream SetFlags(params Flag[] flags);

		// Token: 0x060001B3 RID: 435
		IVideoStream SetFlags(params string[] flags);

		// Token: 0x060001B4 RID: 436
		IVideoStream SetFramerate(double framerate);

		// Token: 0x060001B5 RID: 437
		IVideoStream SetBitrate(long minBitrate, long maxBitrate, long bufferSize);

		// Token: 0x060001B6 RID: 438
		IVideoStream SetBitrate(long bitrate);

		// Token: 0x060001B7 RID: 439
		IVideoStream SetSize(VideoSize size);

		// Token: 0x060001B8 RID: 440
		IVideoStream SetSize(int width, int height);

		// Token: 0x060001B9 RID: 441
		IVideoStream SetCodec(VideoCodec codec);

		// Token: 0x060001BA RID: 442
		IVideoStream SetCodec(string codec);

		// Token: 0x060001BB RID: 443
		IVideoStream CopyStream();

		// Token: 0x060001BC RID: 444
		IVideoStream SetBitstreamFilter(BitstreamFilter filter);

		// Token: 0x060001BD RID: 445
		IVideoStream SetLoop(int count, int delay = 0);

		// Token: 0x060001BE RID: 446
		IVideoStream SetOutputFramesCount(int number);

		// Token: 0x060001BF RID: 447
		IVideoStream SetSeek(TimeSpan seek);

		// Token: 0x060001C0 RID: 448
		IVideoStream AddSubtitles(string subtitlePath, string encode = null, string style = null);

		// Token: 0x060001C1 RID: 449
		IVideoStream AddSubtitles(string subtitlePath, VideoSize originalSize, string encode = null, string style = null);

		// Token: 0x060001C2 RID: 450
		IVideoStream Split(TimeSpan startTime, TimeSpan duration);

		// Token: 0x060001C3 RID: 451
		IVideoStream SetBitstreamFilter(string filter);

		// Token: 0x060001C4 RID: 452
		IVideoStream SetInputFormat(string inputFormat);

		// Token: 0x060001C5 RID: 453
		IVideoStream SetInputFormat(Format inputFormat);

		// Token: 0x060001C6 RID: 454
		IVideoStream UseNativeInputRead(bool readInputAtNativeFrameRate);

		// Token: 0x060001C7 RID: 455
		IVideoStream SetStreamLoop(int loopCount);
	}
}
