using System;
using Concentus.Common;

namespace Concentus.Silk
{
	// Token: 0x0200001C RID: 28
	internal static class DecodePitch
	{
		// Token: 0x060000BA RID: 186 RVA: 0x00007C2C File Offset: 0x00005E2C
		internal static void silk_decode_pitch(short lagIndex, sbyte contourIndex, int[] pitch_lags, int Fs_kHz, int nb_subfr)
		{
			sbyte[][] array;
			if (Fs_kHz == 8)
			{
				if (nb_subfr == 4)
				{
					array = Tables.silk_CB_lags_stage2;
				}
				else
				{
					array = Tables.silk_CB_lags_stage2_10_ms;
				}
			}
			else if (nb_subfr == 4)
			{
				array = Tables.silk_CB_lags_stage3;
			}
			else
			{
				array = Tables.silk_CB_lags_stage3_10_ms;
			}
			int num = Inlines.silk_SMULBB(2, Fs_kHz);
			int limit = Inlines.silk_SMULBB(18, Fs_kHz);
			int num2 = num + (int)lagIndex;
			for (int i = 0; i < nb_subfr; i++)
			{
				pitch_lags[i] = num2 + (int)array[i][(int)contourIndex];
				pitch_lags[i] = Inlines.silk_LIMIT(pitch_lags[i], num, limit);
			}
		}
	}
}
