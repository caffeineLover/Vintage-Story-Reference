using System;

namespace Xabe.FFmpeg
{
	// Token: 0x0200003C RID: 60
	internal static class VideoSizeExtensions
	{
		// Token: 0x060001C8 RID: 456 RVA: 0x00006278 File Offset: 0x00004478
		public static string ToFFmpegFormat(this VideoSize videoSize)
		{
			switch (videoSize)
			{
			case VideoSize.Ntsc:
				return "720x480";
			case VideoSize.Pal:
				return "720x576";
			case VideoSize.Qntsc:
				return "352x240";
			case VideoSize.Qpal:
				return "352x288";
			case VideoSize.Sntsc:
				return "640x480";
			case VideoSize.Spal:
				return "768x576";
			case VideoSize.Film:
				return "352x240";
			case VideoSize.NtscFilm:
				return "352x240";
			case VideoSize.Sqcif:
				return "128x96";
			case VideoSize.Qcif:
				return "176x144";
			case VideoSize.Cif:
				return "352x288";
			case VideoSize._4Cif:
				return "704x576";
			case VideoSize._16cif:
				return "1408x1152";
			case VideoSize.Qqvga:
				return "160x120";
			case VideoSize.Qvga:
				return "320x240";
			case VideoSize.Vga:
				return "640x480";
			case VideoSize.Svga:
				return "800x600";
			case VideoSize.Xga:
				return "1024x768";
			case VideoSize.Uxga:
				return "1600x1200";
			case VideoSize.Qxga:
				return "2048x1536";
			case VideoSize.Sxga:
				return "1280x1024";
			case VideoSize.Qsxga:
				return "2560x2048";
			case VideoSize.Hsxga:
				return "5120x4096";
			case VideoSize.Wvga:
				return "852x480";
			case VideoSize.Wxga:
				return "1366x768";
			case VideoSize.Wsxga:
				return "1600x1024";
			case VideoSize.Wuxga:
				return "1920x1200";
			case VideoSize.Woxga:
				return "2560x1600";
			case VideoSize.Wqsxga:
				return "3200x2048";
			case VideoSize.Wquxga:
				return "3840x2400";
			case VideoSize.Whsxga:
				return "6400x4096";
			case VideoSize.Whuxga:
				return "7680x4800";
			case VideoSize.Cga:
				return "320x200";
			case VideoSize.Ega:
				return "640x350";
			case VideoSize.Hd480:
				return "852x480";
			case VideoSize.Hd720:
				return "1280x720";
			case VideoSize.Hd1080:
				return "1920x1080";
			case VideoSize._2K:
				return "2048x1080";
			case VideoSize._2Kflat:
				return "1998x1080";
			case VideoSize._2Kscope:
				return "2048x858";
			case VideoSize._4K:
				return "4096x2160";
			case VideoSize._4Kflat:
				return "3996x2160";
			case VideoSize._4Kscope:
				return "4096x1716";
			case VideoSize.Nhd:
				return "640x360";
			case VideoSize.Hqvga:
				return "240x160";
			case VideoSize.Wqvga:
				return "400x240";
			case VideoSize.Fwqvga:
				return "432x240";
			case VideoSize.Hvga:
				return "480x320";
			case VideoSize.Qhd:
				return "960x540";
			case VideoSize._2Kdci:
				return "2048x1080";
			case VideoSize._4Kdci:
				return "4096x2160";
			case VideoSize.Uhd2160:
				return "3840x2160";
			case VideoSize.Uhd4320:
				return "7680x4320";
			default:
				throw new InvalidOperationException();
			}
		}
	}
}
