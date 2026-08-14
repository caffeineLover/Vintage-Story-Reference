using System;
using Concentus.Common;
using Concentus.Silk.Structs;

namespace Concentus.Silk
{
	// Token: 0x0200002C RID: 44
	internal static class LTPScaleControl
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x0000BA8C File Offset: 0x00009C8C
		internal static void silk_LTP_scale_ctrl(SilkChannelEncoder psEnc, SilkEncoderControl psEncCtrl, int condCoding)
		{
			if (condCoding == 0)
			{
				int a = psEnc.PacketLoss_perc + psEnc.nFramesPerPacket;
				psEnc.indices.LTP_scaleIndex = (sbyte)Inlines.silk_LIMIT(Inlines.silk_SMULWB(Inlines.silk_SMULBB(a, psEncCtrl.LTPredCodGain_Q7), 51), 0, 2);
			}
			else
			{
				psEnc.indices.LTP_scaleIndex = 0;
			}
			psEncCtrl.LTP_scale_Q14 = (int)Tables.silk_LTPScales_table_Q14[(int)psEnc.indices.LTP_scaleIndex];
		}
	}
}
