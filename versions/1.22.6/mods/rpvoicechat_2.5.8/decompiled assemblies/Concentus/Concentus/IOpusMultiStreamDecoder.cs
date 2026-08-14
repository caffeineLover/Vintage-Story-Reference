using System;
using Concentus.Enums;

namespace Concentus
{
	// Token: 0x02000004 RID: 4
	public interface IOpusMultiStreamDecoder : IDisposable
	{
		// Token: 0x06000034 RID: 52
		int DecodeMultistream(ReadOnlySpan<byte> data, Span<float> out_pcm, int frame_size, bool decode_fec);

		// Token: 0x06000035 RID: 53
		int DecodeMultistream(ReadOnlySpan<byte> data, Span<short> out_pcm, int frame_size, bool decode_fec);

		// Token: 0x06000036 RID: 54
		void ResetState();

		// Token: 0x06000037 RID: 55
		string GetVersionString();

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000038 RID: 56
		OpusBandwidth Bandwidth { get; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000039 RID: 57
		uint FinalRange { get; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600003A RID: 58
		// (set) Token: 0x0600003B RID: 59
		int Gain { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600003C RID: 60
		int LastPacketDuration { get; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600003D RID: 61
		int SampleRate { get; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600003E RID: 62
		int NumChannels { get; }
	}
}
