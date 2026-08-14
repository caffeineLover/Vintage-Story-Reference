using System;
using Concentus.Common;

namespace Concentus.Silk.Structs
{
	// Token: 0x0200004B RID: 75
	internal class SilkEncoder
	{
		// Token: 0x06000157 RID: 343 RVA: 0x000165B4 File Offset: 0x000147B4
		internal SilkEncoder()
		{
			for (int i = 0; i < 2; i++)
			{
				this.state_Fxx[i] = new SilkChannelEncoder();
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000165F8 File Offset: 0x000147F8
		internal void Reset()
		{
			for (int i = 0; i < 2; i++)
			{
				this.state_Fxx[i].Reset();
			}
			this.sStereo.Reset();
			this.nBitsUsedLBRR = 0;
			this.nBitsExceeded = 0;
			this.nChannelsAPI = 0;
			this.nChannelsInternal = 0;
			this.nPrevChannelsInternal = 0;
			this.timeSinceSwitchAllowed_ms = 0;
			this.allowBandwidthSwitch = 0;
			this.prev_decode_only_middle = 0;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00016664 File Offset: 0x00014864
		internal static int silk_init_encoder(SilkChannelEncoder psEnc)
		{
			int num = 0;
			psEnc.Reset();
			psEnc.variable_HP_smth1_Q15 = Inlines.silk_LSHIFT(Inlines.silk_lin2log(3932160) - 2048, 8);
			psEnc.variable_HP_smth2_Q15 = psEnc.variable_HP_smth1_Q15;
			psEnc.first_frame_after_reset = 1;
			return num + VoiceActivityDetection.silk_VAD_Init(psEnc.sVAD);
		}

		// Token: 0x04000240 RID: 576
		internal readonly SilkChannelEncoder[] state_Fxx = new SilkChannelEncoder[2];

		// Token: 0x04000241 RID: 577
		internal readonly StereoEncodeState sStereo = new StereoEncodeState();

		// Token: 0x04000242 RID: 578
		internal int nBitsUsedLBRR;

		// Token: 0x04000243 RID: 579
		internal int nBitsExceeded;

		// Token: 0x04000244 RID: 580
		internal int nChannelsAPI;

		// Token: 0x04000245 RID: 581
		internal int nChannelsInternal;

		// Token: 0x04000246 RID: 582
		internal int nPrevChannelsInternal;

		// Token: 0x04000247 RID: 583
		internal int timeSinceSwitchAllowed_ms;

		// Token: 0x04000248 RID: 584
		internal int allowBandwidthSwitch;

		// Token: 0x04000249 RID: 585
		internal int prev_decode_only_middle;
	}
}
