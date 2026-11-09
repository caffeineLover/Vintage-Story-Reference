using System;
using Concentus.Common;
using Concentus.Silk.Structs;

namespace Concentus.Silk
{
	// Token: 0x0200001F RID: 31
	internal static class EncodeIndices
	{
		// Token: 0x060000BF RID: 191 RVA: 0x0000901C File Offset: 0x0000721C
		internal static void silk_encode_indices(SilkChannelEncoder psEncC, EntropyCoder psRangeEnc, Span<byte> encodedDataOut, int FrameIndex, int encode_LBRR, int condCoding)
		{
			short[] array = new short[16];
			byte[] array2 = new byte[16];
			SideInfoIndices sideInfoIndices;
			if (encode_LBRR != 0)
			{
				sideInfoIndices = psEncC.indices_LBRR[FrameIndex];
			}
			else
			{
				sideInfoIndices = psEncC.indices;
			}
			int num = (int)(2 * sideInfoIndices.signalType + sideInfoIndices.quantOffsetType);
			if (encode_LBRR != 0 || num >= 2)
			{
				psRangeEnc.enc_icdf(encodedDataOut, num - 2, Tables.silk_type_offset_VAD_iCDF, 8U);
			}
			else
			{
				psRangeEnc.enc_icdf(encodedDataOut, num, Tables.silk_type_offset_no_VAD_iCDF, 8U);
			}
			if (condCoding == 2)
			{
				psRangeEnc.enc_icdf(encodedDataOut, (int)sideInfoIndices.GainsIndices[0], Tables.silk_delta_gain_iCDF, 8U);
			}
			else
			{
				psRangeEnc.enc_icdf(encodedDataOut, Inlines.silk_RSHIFT((int)sideInfoIndices.GainsIndices[0], 3), Tables.silk_gain_iCDF[(int)sideInfoIndices.signalType], 8U);
				psRangeEnc.enc_icdf(encodedDataOut, (int)(sideInfoIndices.GainsIndices[0] & 7), Tables.silk_uniform8_iCDF, 8U);
			}
			for (int i = 1; i < psEncC.nb_subfr; i++)
			{
				psRangeEnc.enc_icdf(encodedDataOut, (int)sideInfoIndices.GainsIndices[i], Tables.silk_delta_gain_iCDF, 8U);
			}
			psRangeEnc.enc_icdf(encodedDataOut, (int)sideInfoIndices.NLSFIndices[0], psEncC.psNLSF_CB.CB1_iCDF, (sideInfoIndices.signalType >> 1) * (int)psEncC.psNLSF_CB.nVectors, 8U);
			NLSF.silk_NLSF_unpack(array, array2, psEncC.psNLSF_CB, (int)sideInfoIndices.NLSFIndices[0]);
			for (int i = 0; i < (int)psEncC.psNLSF_CB.order; i++)
			{
				if (sideInfoIndices.NLSFIndices[i + 1] >= 4)
				{
					psRangeEnc.enc_icdf(encodedDataOut, 8, psEncC.psNLSF_CB.ec_iCDF, (int)array[i], 8U);
					psRangeEnc.enc_icdf(encodedDataOut, (int)(sideInfoIndices.NLSFIndices[i + 1] - 4), Tables.silk_NLSF_EXT_iCDF, 8U);
				}
				else if (sideInfoIndices.NLSFIndices[i + 1] <= -4)
				{
					psRangeEnc.enc_icdf(encodedDataOut, 0, psEncC.psNLSF_CB.ec_iCDF, (int)array[i], 8U);
					psRangeEnc.enc_icdf(encodedDataOut, (int)(-sideInfoIndices.NLSFIndices[i + 1] - 4), Tables.silk_NLSF_EXT_iCDF, 8U);
				}
				else
				{
					psRangeEnc.enc_icdf(encodedDataOut, (int)(sideInfoIndices.NLSFIndices[i + 1] + 4), psEncC.psNLSF_CB.ec_iCDF, (int)array[i], 8U);
				}
			}
			if (psEncC.nb_subfr == 4)
			{
				psRangeEnc.enc_icdf(encodedDataOut, (int)sideInfoIndices.NLSFInterpCoef_Q2, Tables.silk_NLSF_interpolation_factor_iCDF, 8U);
			}
			if (sideInfoIndices.signalType == 2)
			{
				int num2 = 1;
				if (condCoding == 2 && psEncC.ec_prevSignalType == 2)
				{
					int num3 = (int)(sideInfoIndices.lagIndex - psEncC.ec_prevLagIndex);
					if (num3 < -8 || num3 > 11)
					{
						num3 = 0;
					}
					else
					{
						num3 += 9;
						num2 = 0;
					}
					psRangeEnc.enc_icdf(encodedDataOut, num3, Tables.silk_pitch_delta_iCDF, 8U);
				}
				if (num2 != 0)
				{
					int num4 = Inlines.silk_DIV32_16((int)sideInfoIndices.lagIndex, Inlines.silk_RSHIFT(psEncC.fs_kHz, 1));
					int s = (int)sideInfoIndices.lagIndex - Inlines.silk_SMULBB(num4, Inlines.silk_RSHIFT(psEncC.fs_kHz, 1));
					psRangeEnc.enc_icdf(encodedDataOut, num4, Tables.silk_pitch_lag_iCDF, 8U);
					psRangeEnc.enc_icdf(encodedDataOut, s, psEncC.pitch_lag_low_bits_iCDF, 8U);
				}
				psEncC.ec_prevLagIndex = sideInfoIndices.lagIndex;
				psRangeEnc.enc_icdf(encodedDataOut, (int)sideInfoIndices.contourIndex, psEncC.pitch_contour_iCDF, 8U);
				psRangeEnc.enc_icdf(encodedDataOut, (int)sideInfoIndices.PERIndex, Tables.silk_LTP_per_index_iCDF, 8U);
				for (int j = 0; j < psEncC.nb_subfr; j++)
				{
					psRangeEnc.enc_icdf(encodedDataOut, (int)sideInfoIndices.LTPIndex[j], Tables.silk_LTP_gain_iCDF_ptrs[(int)sideInfoIndices.PERIndex], 8U);
				}
				if (condCoding == 0)
				{
					psRangeEnc.enc_icdf(encodedDataOut, (int)sideInfoIndices.LTP_scaleIndex, Tables.silk_LTPscale_iCDF, 8U);
				}
			}
			psEncC.ec_prevSignalType = (int)sideInfoIndices.signalType;
			psRangeEnc.enc_icdf(encodedDataOut, (int)sideInfoIndices.Seed, Tables.silk_uniform4_iCDF, 8U);
		}
	}
}
