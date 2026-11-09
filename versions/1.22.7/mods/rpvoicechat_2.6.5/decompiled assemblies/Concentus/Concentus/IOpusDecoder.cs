using System;
using Concentus.Enums;

namespace Concentus
{
	// Token: 0x02000002 RID: 2
	public interface IOpusDecoder : IDisposable
	{
		// Token: 0x06000001 RID: 1
		int Decode(ReadOnlySpan<byte> in_data, Span<float> out_pcm, int frame_size, bool decode_fec = false);

		// Token: 0x06000002 RID: 2
		int Decode(ReadOnlySpan<byte> in_data, Span<short> out_pcm, int frame_size, bool decode_fec = false);

		// Token: 0x06000003 RID: 3
		void ResetState();

		// Token: 0x06000004 RID: 4
		string GetVersionString();

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000005 RID: 5
		OpusBandwidth Bandwidth { get; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000006 RID: 6
		uint FinalRange { get; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7
		// (set) Token: 0x06000008 RID: 8
		int Gain { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9
		int LastPacketDuration { get; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000A RID: 10
		int NumChannels { get; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000B RID: 11
		int Pitch { get; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000C RID: 12
		int SampleRate { get; }
	}
}
