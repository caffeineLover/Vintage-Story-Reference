using System;
using Concentus.Common;
using Concentus.Silk.Structs;

namespace Concentus.Silk
{
	// Token: 0x0200001A RID: 26
	internal static class DecodeIndices
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x00007648 File Offset: 0x00005848
		internal static void silk_decode_indices(SilkChannelDecoder psDec, EntropyCoder psRangeDec, ReadOnlySpan<byte> frameData, int FrameIndex, int decode_LBRR, int condCoding)
		{
			short[] array = new short[psDec.LPC_order];
			byte[] array2 = new byte[psDec.LPC_order];
			int num;
			if (decode_LBRR != 0 || psDec.VAD_flags[FrameIndex] != 0)
			{
				num = psRangeDec.dec_icdf(frameData, Tables.silk_type_offset_VAD_iCDF, 8U) + 2;
			}
			else
			{
				num = psRangeDec.dec_icdf(frameData, Tables.silk_type_offset_no_VAD_iCDF, 8U);
			}
			psDec.indices.signalType = (sbyte)Inlines.silk_RSHIFT(num, 1);
			psDec.indices.quantOffsetType = (sbyte)(num & 1);
			if (condCoding == 2)
			{
				psDec.indices.GainsIndices[0] = (sbyte)psRangeDec.dec_icdf(frameData, Tables.silk_delta_gain_iCDF, 8U);
			}
			else
			{
				psDec.indices.GainsIndices[0] = (sbyte)Inlines.silk_LSHIFT(psRangeDec.dec_icdf(frameData, Tables.silk_gain_iCDF[(int)psDec.indices.signalType], 8U), 3);
				sbyte[] gainsIndices = psDec.indices.GainsIndices;
				int num2 = 0;
				gainsIndices[num2] += (sbyte)psRangeDec.dec_icdf(frameData, Tables.silk_uniform8_iCDF, 8U);
			}
			for (int i = 1; i < psDec.nb_subfr; i++)
			{
				psDec.indices.GainsIndices[i] = (sbyte)psRangeDec.dec_icdf(frameData, Tables.silk_delta_gain_iCDF, 8U);
			}
			psDec.indices.NLSFIndices[0] = (sbyte)psRangeDec.dec_icdf(frameData, psDec.psNLSF_CB.CB1_iCDF, (psDec.indices.signalType >> 1) * (int)psDec.psNLSF_CB.nVectors, 8U);
			NLSF.silk_NLSF_unpack(array, array2, psDec.psNLSF_CB, (int)psDec.indices.NLSFIndices[0]);
			for (int i = 0; i < (int)psDec.psNLSF_CB.order; i++)
			{
				num = psRangeDec.dec_icdf(frameData, psDec.psNLSF_CB.ec_iCDF, (int)array[i], 8U);
				if (num == 0)
				{
					num -= psRangeDec.dec_icdf(frameData, Tables.silk_NLSF_EXT_iCDF, 8U);
				}
				else if (num == 8)
				{
					num += psRangeDec.dec_icdf(frameData, Tables.silk_NLSF_EXT_iCDF, 8U);
				}
				psDec.indices.NLSFIndices[i + 1] = (sbyte)(num - 4);
			}
			if (psDec.nb_subfr == 4)
			{
				psDec.indices.NLSFInterpCoef_Q2 = (sbyte)psRangeDec.dec_icdf(frameData, Tables.silk_NLSF_interpolation_factor_iCDF, 8U);
			}
			else
			{
				psDec.indices.NLSFInterpCoef_Q2 = 4;
			}
			if (psDec.indices.signalType == 2)
			{
				int num3 = 1;
				if (condCoding == 2 && psDec.ec_prevSignalType == 2)
				{
					int num4 = (int)((short)psRangeDec.dec_icdf(frameData, Tables.silk_pitch_delta_iCDF, 8U));
					if (num4 > 0)
					{
						num4 -= 9;
						psDec.indices.lagIndex = (short)((int)psDec.ec_prevLagIndex + num4);
						num3 = 0;
					}
				}
				if (num3 != 0)
				{
					psDec.indices.lagIndex = (short)(psRangeDec.dec_icdf(frameData, Tables.silk_pitch_lag_iCDF, 8U) * Inlines.silk_RSHIFT(psDec.fs_kHz, 1));
					SideInfoIndices indices = psDec.indices;
					indices.lagIndex += (short)psRangeDec.dec_icdf(frameData, psDec.pitch_lag_low_bits_iCDF, 8U);
				}
				psDec.ec_prevLagIndex = psDec.indices.lagIndex;
				psDec.indices.contourIndex = (sbyte)psRangeDec.dec_icdf(frameData, psDec.pitch_contour_iCDF, 8U);
				psDec.indices.PERIndex = (sbyte)psRangeDec.dec_icdf(frameData, Tables.silk_LTP_per_index_iCDF, 8U);
				for (int j = 0; j < psDec.nb_subfr; j++)
				{
					psDec.indices.LTPIndex[j] = (sbyte)psRangeDec.dec_icdf(frameData, Tables.silk_LTP_gain_iCDF_ptrs[(int)psDec.indices.PERIndex], 8U);
				}
				if (condCoding == 0)
				{
					psDec.indices.LTP_scaleIndex = (sbyte)psRangeDec.dec_icdf(frameData, Tables.silk_LTPscale_iCDF, 8U);
				}
				else
				{
					psDec.indices.LTP_scaleIndex = 0;
				}
			}
			psDec.ec_prevSignalType = (int)psDec.indices.signalType;
			psDec.indices.Seed = (sbyte)psRangeDec.dec_icdf(frameData, Tables.silk_uniform4_iCDF, 8U);
		}
	}
}
