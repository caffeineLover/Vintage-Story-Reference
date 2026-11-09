using System;
using Concentus.Common;
using Concentus.Common.CPlusPlus;

namespace Concentus.Silk
{
	// Token: 0x0200001D RID: 29
	internal static class DecodePulses
	{
		// Token: 0x060000BB RID: 187 RVA: 0x00007CA4 File Offset: 0x00005EA4
		internal static void silk_decode_pulses(EntropyCoder psRangeDec, ReadOnlySpan<byte> frameData, short[] pulses, int signalType, int quantOffsetType, int frame_length)
		{
			int[] array = new int[20];
			int[] array2 = new int[20];
			int num = psRangeDec.dec_icdf(frameData, Tables.silk_rate_levels_iCDF[signalType >> 1], 8U);
			int num2 = Inlines.silk_RSHIFT(frame_length, 4);
			if (num2 * 16 < frame_length)
			{
				num2++;
			}
			for (int i = 0; i < num2; i++)
			{
				array2[i] = 0;
				array[i] = psRangeDec.dec_icdf(frameData, Tables.silk_pulses_per_block_iCDF[num], 8U);
				while (array[i] == 17)
				{
					array2[i]++;
					array[i] = psRangeDec.dec_icdf(frameData, Tables.silk_pulses_per_block_iCDF[9], (array2[i] == 10) ? 1 : 0, 8U);
				}
			}
			for (int i = 0; i < num2; i++)
			{
				if (array[i] > 0)
				{
					ShellCoder.silk_shell_decoder(pulses, Inlines.silk_SMULBB(i, 16), psRangeDec, frameData, array[i]);
				}
				else
				{
					Arrays.MemSetWithOffset<short>(pulses, 0, Inlines.silk_SMULBB(i, 16), 16);
				}
			}
			for (int i = 0; i < num2; i++)
			{
				if (array2[i] > 0)
				{
					int num3 = array2[i];
					int num4 = Inlines.silk_SMULBB(i, 16);
					for (int j = 0; j < 16; j++)
					{
						int num5 = (int)pulses[num4 + j];
						for (int k = 0; k < num3; k++)
						{
							num5 = Inlines.silk_LSHIFT(num5, 1);
							num5 += psRangeDec.dec_icdf(frameData, Tables.silk_lsb_iCDF, 8U);
						}
						pulses[num4 + j] = (short)num5;
					}
					array[i] |= num3 << 5;
				}
			}
			CodeSigns.silk_decode_signs(psRangeDec, frameData, pulses, frame_length, signalType, quantOffsetType, array);
		}
	}
}
