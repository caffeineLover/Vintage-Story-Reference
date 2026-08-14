using System;
using Concentus.Common;

namespace Concentus.Silk
{
	// Token: 0x02000016 RID: 22
	internal static class CodeSigns
	{
		// Token: 0x060000AE RID: 174 RVA: 0x000061BE File Offset: 0x000043BE
		private static int silk_enc_map(int a)
		{
			return Inlines.silk_RSHIFT(a, 15) + 1;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000061CA File Offset: 0x000043CA
		private static int silk_dec_map(int a)
		{
			return Inlines.silk_LSHIFT(a, 1) - 1;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000061D8 File Offset: 0x000043D8
		internal unsafe static void silk_encode_signs(EntropyCoder psRangeEnc, Span<byte> encodedData, Span<sbyte> pulses, int length, int signalType, int quantOffsetType, int[] sum_pulses)
		{
			byte[] array = new byte[2];
			byte[] silk_sign_iCDF = Tables.silk_sign_iCDF;
			array[1] = 0;
			int num = 0;
			int i = Inlines.silk_SMULBB(7, Inlines.silk_ADD_LSHIFT(quantOffsetType, signalType, 1));
			int num2 = i;
			length = Inlines.silk_RSHIFT(length + 8, 4);
			for (i = 0; i < length; i++)
			{
				int num3 = sum_pulses[i];
				if (num3 > 0)
				{
					array[0] = silk_sign_iCDF[num2 + Inlines.silk_min(num3 & 31, 6)];
					for (int j = num; j < num + 16; j++)
					{
						if (*pulses[j] != 0)
						{
							psRangeEnc.enc_icdf(encodedData, CodeSigns.silk_enc_map((int)(*pulses[j])), array, 8U);
						}
					}
				}
				num += 16;
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000627C File Offset: 0x0000447C
		internal static void silk_decode_signs(EntropyCoder psRangeDec, ReadOnlySpan<byte> encodedData, short[] pulses, int length, int signalType, int quantOffsetType, int[] sum_pulses)
		{
			byte[] array = new byte[2];
			byte[] silk_sign_iCDF = Tables.silk_sign_iCDF;
			array[1] = 0;
			int num = 0;
			int i = Inlines.silk_SMULBB(7, Inlines.silk_ADD_LSHIFT(quantOffsetType, signalType, 1));
			int num2 = i;
			length = Inlines.silk_RSHIFT(length + 8, 4);
			for (i = 0; i < length; i++)
			{
				int num3 = sum_pulses[i];
				if (num3 > 0)
				{
					array[0] = silk_sign_iCDF[num2 + Inlines.silk_min(num3 & 31, 6)];
					for (int j = 0; j < 16; j++)
					{
						if (pulses[num + j] > 0)
						{
							int num4 = num + j;
							pulses[num4] *= (short)CodeSigns.silk_dec_map(psRangeDec.dec_icdf(encodedData, array, 8U));
						}
					}
				}
				num += 16;
			}
		}
	}
}
