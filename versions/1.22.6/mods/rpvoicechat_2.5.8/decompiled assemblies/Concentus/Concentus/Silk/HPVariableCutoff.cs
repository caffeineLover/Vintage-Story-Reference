using System;
using Concentus.Common;
using Concentus.Silk.Structs;

namespace Concentus.Silk
{
	// Token: 0x02000027 RID: 39
	internal static class HPVariableCutoff
	{
		// Token: 0x060000D8 RID: 216 RVA: 0x0000B2BC File Offset: 0x000094BC
		internal static void silk_HP_variable_cutoff(SilkChannelEncoder[] state_Fxx)
		{
			SilkChannelEncoder silkChannelEncoder = state_Fxx[0];
			if (silkChannelEncoder.prevSignalType == 2)
			{
				int num = Inlines.silk_lin2log(Inlines.silk_DIV32_16(Inlines.silk_LSHIFT(Inlines.silk_MUL(silkChannelEncoder.fs_kHz, 1000), 16), silkChannelEncoder.prevLag)) - 2048;
				int num2 = silkChannelEncoder.input_quality_bands_Q15[0];
				num = Inlines.silk_SMLAWB(num, Inlines.silk_SMULWB(Inlines.silk_LSHIFT(-num2, 2), num2), num - (Inlines.silk_lin2log(3932160) - 2048));
				int num3 = num - Inlines.silk_RSHIFT(silkChannelEncoder.variable_HP_smth1_Q15, 8);
				if (num3 < 0)
				{
					num3 = Inlines.silk_MUL(num3, 3);
				}
				num3 = Inlines.silk_LIMIT_32(num3, -51, 51);
				silkChannelEncoder.variable_HP_smth1_Q15 = Inlines.silk_SMLAWB(silkChannelEncoder.variable_HP_smth1_Q15, Inlines.silk_SMULBB(silkChannelEncoder.speech_activity_Q8, num3), 6554);
				silkChannelEncoder.variable_HP_smth1_Q15 = Inlines.silk_LIMIT_32(silkChannelEncoder.variable_HP_smth1_Q15, Inlines.silk_LSHIFT(Inlines.silk_lin2log(60), 8), Inlines.silk_LSHIFT(Inlines.silk_lin2log(100), 8));
			}
		}
	}
}
