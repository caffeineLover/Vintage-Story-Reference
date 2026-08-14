using System;

namespace Concentus.Silk.Structs
{
	// Token: 0x02000049 RID: 73
	internal class SilkDecoder
	{
		// Token: 0x06000153 RID: 339 RVA: 0x00016490 File Offset: 0x00014690
		internal SilkDecoder()
		{
			for (int i = 0; i < 2; i++)
			{
				this.channel_state[i] = new SilkChannelDecoder();
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x000164D4 File Offset: 0x000146D4
		internal void Reset()
		{
			for (int i = 0; i < 2; i++)
			{
				this.channel_state[i].Reset();
			}
			this.sStereo.Reset();
			this.nChannelsAPI = 0;
			this.nChannelsInternal = 0;
			this.prev_decode_only_middle = 0;
		}

		// Token: 0x04000236 RID: 566
		internal readonly SilkChannelDecoder[] channel_state = new SilkChannelDecoder[2];

		// Token: 0x04000237 RID: 567
		internal readonly StereoDecodeState sStereo = new StereoDecodeState();

		// Token: 0x04000238 RID: 568
		internal int nChannelsAPI;

		// Token: 0x04000239 RID: 569
		internal int nChannelsInternal;

		// Token: 0x0400023A RID: 570
		internal int prev_decode_only_middle;
	}
}
