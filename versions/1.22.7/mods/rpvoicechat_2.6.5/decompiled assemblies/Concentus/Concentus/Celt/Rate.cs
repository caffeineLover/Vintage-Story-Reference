using System;
using Concentus.Celt.Structs;
using Concentus.Common;

namespace Concentus.Celt
{
	// Token: 0x0200008F RID: 143
	internal static class Rate
	{
		// Token: 0x060004F6 RID: 1270 RVA: 0x0003134E File Offset: 0x0002F54E
		internal static int get_pulses(int i)
		{
			if (i >= 8)
			{
				return 8 + (i & 7) << (i >> 3) - 1;
			}
			return i;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00031364 File Offset: 0x0002F564
		internal static int bits2pulses(CeltMode m, int band, int LM, int bits)
		{
			LM++;
			byte[] bits2 = m.cache.bits;
			int num = (int)m.cache.index[LM * m.nbEBands + band];
			int num2 = 0;
			int num3 = (int)bits2[num];
			bits--;
			for (int i = 0; i < 6; i++)
			{
				int num4 = num2 + num3 + 1 >> 1;
				if ((int)bits2[num + num4] >= bits)
				{
					num3 = num4;
				}
				else
				{
					num2 = num4;
				}
			}
			if (bits - ((num2 == 0) ? -1 : ((int)bits2[num + num2])) <= (int)bits2[num + num3] - bits)
			{
				return num2;
			}
			return num3;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x000313E8 File Offset: 0x0002F5E8
		internal static int pulses2bits(CeltMode m, int band, int LM, int pulses)
		{
			LM++;
			if (pulses != 0)
			{
				return (int)(m.cache.bits[(int)m.cache.index[LM * m.nbEBands + band] + pulses] + 1);
			}
			return 0;
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0003141C File Offset: 0x0002F61C
		internal static int interp_bits2pulses_encode(CeltMode m, int start, int end, int skip_start, int[] bits1, int[] bits2, int[] thresh, int[] cap, int total, out int _balance, int skip_rsv, ref int intensity, int intensity_rsv, ref int dual_stereo, int dual_stereo_rsv, int[] bits, int[] ebits, int[] fine_priority, int C, int LM, EntropyCoder ec, Span<byte> encodedData, int prev, int signalBandwidth)
		{
			int num = C << 3;
			int num2 = (C > 1) ? 1 : 0;
			int num3 = LM << 3;
			int num4 = 0;
			int num5 = 64;
			int num7;
			int num8;
			int j;
			for (int i = 0; i < 6; i++)
			{
				int num6 = num4 + num5 >> 1;
				num7 = 0;
				num8 = 0;
				j = end;
				while (j-- > start)
				{
					int num9 = bits1[j] + (num6 * bits2[j] >> 6);
					if (num9 >= thresh[j] || num8 != 0)
					{
						num8 = 1;
						num7 += Inlines.IMIN(num9, cap[j]);
					}
					else if (num9 >= num)
					{
						num7 += num;
					}
				}
				if (num7 > total)
				{
					num5 = num6;
				}
				else
				{
					num4 = num6;
				}
			}
			num7 = 0;
			num8 = 0;
			j = end;
			while (j-- > start)
			{
				int num10 = bits1[j] + (num4 * bits2[j] >> 6);
				if (num10 < thresh[j] && num8 == 0)
				{
					if (num10 >= num)
					{
						num10 = num;
					}
					else
					{
						num10 = 0;
					}
				}
				else
				{
					num8 = 1;
				}
				num10 = Inlines.IMIN(num10, cap[j]);
				bits[j] = num10;
				num7 += num10;
			}
			int num11 = end;
			int num12;
			int num13;
			for (;;)
			{
				j = num11 - 1;
				if (j <= skip_start)
				{
					break;
				}
				num12 = total - num7;
				num13 = Inlines.celt_udiv(num12, (int)(m.eBands[num11] - m.eBands[start]));
				num12 -= (int)(m.eBands[num11] - m.eBands[start]) * num13;
				int num14 = Inlines.IMAX(num12 - (int)(m.eBands[j] - m.eBands[start]), 0);
				int num15 = (int)(m.eBands[num11] - m.eBands[j]);
				int num16 = bits[j] + num13 * num15 + num14;
				if (num16 >= Inlines.IMAX(thresh[j], num + 8))
				{
					if (num11 <= start + 2 || (num16 > ((j < prev) ? 7 : 9) * num15 << LM << 3 >> 4 && j <= signalBandwidth))
					{
						goto IL_1BF;
					}
					ec.enc_bit_logp(encodedData, 0, 1U);
					num7 += 8;
					num16 -= 8;
				}
				num7 -= bits[j] + intensity_rsv;
				if (intensity_rsv > 0)
				{
					intensity_rsv = (int)Rate.LOG2_FRAC_TABLE[j - start];
				}
				num7 += intensity_rsv;
				if (num16 >= num)
				{
					num7 += num;
					bits[j] = num;
				}
				else
				{
					bits[j] = 0;
				}
				num11--;
			}
			total += skip_rsv;
			goto IL_227;
			IL_1BF:
			ec.enc_bit_logp(encodedData, 1, 1U);
			IL_227:
			if (intensity_rsv > 0)
			{
				intensity = Inlines.IMIN(intensity, num11);
				ec.enc_uint(encodedData, (uint)(intensity - start), (uint)(num11 + 1 - start));
			}
			else
			{
				intensity = 0;
			}
			if (intensity <= start)
			{
				total += dual_stereo_rsv;
				dual_stereo_rsv = 0;
			}
			if (dual_stereo_rsv > 0)
			{
				ec.enc_bit_logp(encodedData, dual_stereo, 1U);
			}
			else
			{
				dual_stereo = 0;
			}
			num12 = total - num7;
			num13 = Inlines.celt_udiv(num12, (int)(m.eBands[num11] - m.eBands[start]));
			num12 -= (int)(m.eBands[num11] - m.eBands[start]) * num13;
			for (j = start; j < num11; j++)
			{
				bits[j] += num13 * (int)(m.eBands[j + 1] - m.eBands[j]);
			}
			for (j = start; j < num11; j++)
			{
				int num17 = Inlines.IMIN(num12, (int)(m.eBands[j + 1] - m.eBands[j]));
				bits[j] += num17;
				num12 -= num17;
			}
			int num18 = 0;
			for (j = start; j < num11; j++)
			{
				int num19 = (int)(m.eBands[j + 1] - m.eBands[j]) << LM;
				int num20 = bits[j] + num18;
				int num21;
				if (num19 > 1)
				{
					num21 = Inlines.MAX32(num20 - cap[j], 0);
					bits[j] = num20 - num21;
					int num22 = C * num19 + ((C == 2 && num19 > 2 && dual_stereo == 0 && j < intensity) ? 1 : 0);
					int num23 = num22 * ((int)m.logN[j] + num3);
					int num24 = (num23 >> 1) - num22 * 21;
					if (num19 == 2)
					{
						num24 += num22 << 3 >> 2;
					}
					if (bits[j] + num24 < num22 * 2 << 3)
					{
						num24 += num23 >> 2;
					}
					else if (bits[j] + num24 < num22 * 3 << 3)
					{
						num24 += num23 >> 3;
					}
					ebits[j] = Inlines.IMAX(0, bits[j] + num24 + (num22 << 2));
					ebits[j] = Inlines.celt_udiv(ebits[j], num22) >> 3;
					if (C * ebits[j] > bits[j] >> 3)
					{
						ebits[j] = bits[j] >> num2 >> 3;
					}
					ebits[j] = Inlines.IMIN(ebits[j], 8);
					fine_priority[j] = ((ebits[j] * (num22 << 3) >= bits[j] + num24) ? 1 : 0);
					bits[j] -= C * ebits[j] << 3;
				}
				else
				{
					num21 = Inlines.MAX32(0, num20 - (C << 3));
					bits[j] = num20 - num21;
					ebits[j] = 0;
					fine_priority[j] = 1;
				}
				if (num21 > 0)
				{
					int num25 = Inlines.IMIN(num21 >> num2 + 3, 8 - ebits[j]);
					ebits[j] += num25;
					int num26 = num25 * C << 3;
					fine_priority[j] = ((num26 >= num21 - num18) ? 1 : 0);
					num21 -= num26;
				}
				num18 = num21;
			}
			_balance = num18;
			while (j < end)
			{
				ebits[j] = bits[j] >> num2 >> 3;
				bits[j] = 0;
				fine_priority[j] = ((ebits[j] < 1) ? 1 : 0);
				j++;
			}
			return num11;
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00031984 File Offset: 0x0002FB84
		internal static int interp_bits2pulses_decode(CeltMode m, int start, int end, int skip_start, int[] bits1, int[] bits2, int[] thresh, int[] cap, int total, out int _balance, int skip_rsv, ref int intensity, int intensity_rsv, ref int dual_stereo, int dual_stereo_rsv, int[] bits, int[] ebits, int[] fine_priority, int C, int LM, EntropyCoder ec, ReadOnlySpan<byte> encodedData, int prev, int signalBandwidth)
		{
			int num = C << 3;
			int num2 = (C > 1) ? 1 : 0;
			int num3 = LM << 3;
			int num4 = 0;
			int num5 = 64;
			int num7;
			int num8;
			int j;
			for (int i = 0; i < 6; i++)
			{
				int num6 = num4 + num5 >> 1;
				num7 = 0;
				num8 = 0;
				j = end;
				while (j-- > start)
				{
					int num9 = bits1[j] + (num6 * bits2[j] >> 6);
					if (num9 >= thresh[j] || num8 != 0)
					{
						num8 = 1;
						num7 += Inlines.IMIN(num9, cap[j]);
					}
					else if (num9 >= num)
					{
						num7 += num;
					}
				}
				if (num7 > total)
				{
					num5 = num6;
				}
				else
				{
					num4 = num6;
				}
			}
			num7 = 0;
			num8 = 0;
			j = end;
			while (j-- > start)
			{
				int num10 = bits1[j] + (num4 * bits2[j] >> 6);
				if (num10 < thresh[j] && num8 == 0)
				{
					if (num10 >= num)
					{
						num10 = num;
					}
					else
					{
						num10 = 0;
					}
				}
				else
				{
					num8 = 1;
				}
				num10 = Inlines.IMIN(num10, cap[j]);
				bits[j] = num10;
				num7 += num10;
			}
			int num11 = end;
			int num12;
			int num13;
			for (;;)
			{
				j = num11 - 1;
				if (j <= skip_start)
				{
					break;
				}
				num12 = total - num7;
				num13 = Inlines.celt_udiv(num12, (int)(m.eBands[num11] - m.eBands[start]));
				num12 -= (int)(m.eBands[num11] - m.eBands[start]) * num13;
				int num14 = Inlines.IMAX(num12 - (int)(m.eBands[j] - m.eBands[start]), 0);
				int num15 = (int)(m.eBands[num11] - m.eBands[j]);
				int num16 = bits[j] + num13 * num15 + num14;
				if (num16 >= Inlines.IMAX(thresh[j], num + 8))
				{
					if (ec.dec_bit_logp(encodedData, 1U) != 0)
					{
						goto IL_1F2;
					}
					num7 += 8;
					num16 -= 8;
				}
				num7 -= bits[j] + intensity_rsv;
				if (intensity_rsv > 0)
				{
					intensity_rsv = (int)Rate.LOG2_FRAC_TABLE[j - start];
				}
				num7 += intensity_rsv;
				if (num16 >= num)
				{
					num7 += num;
					bits[j] = num;
				}
				else
				{
					bits[j] = 0;
				}
				num11--;
			}
			total += skip_rsv;
			IL_1F2:
			if (intensity_rsv > 0)
			{
				intensity = start + (int)ec.dec_uint(encodedData, (uint)(num11 + 1 - start));
			}
			else
			{
				intensity = 0;
			}
			if (intensity <= start)
			{
				total += dual_stereo_rsv;
				dual_stereo_rsv = 0;
			}
			if (dual_stereo_rsv > 0)
			{
				dual_stereo = ec.dec_bit_logp(encodedData, 1U);
			}
			else
			{
				dual_stereo = 0;
			}
			num12 = total - num7;
			num13 = Inlines.celt_udiv(num12, (int)(m.eBands[num11] - m.eBands[start]));
			num12 -= (int)(m.eBands[num11] - m.eBands[start]) * num13;
			for (j = start; j < num11; j++)
			{
				bits[j] += num13 * (int)(m.eBands[j + 1] - m.eBands[j]);
			}
			for (j = start; j < num11; j++)
			{
				int num17 = Inlines.IMIN(num12, (int)(m.eBands[j + 1] - m.eBands[j]));
				bits[j] += num17;
				num12 -= num17;
			}
			int num18 = 0;
			for (j = start; j < num11; j++)
			{
				int num19 = (int)(m.eBands[j + 1] - m.eBands[j]) << LM;
				int num20 = bits[j] + num18;
				int num21;
				if (num19 > 1)
				{
					num21 = Inlines.MAX32(num20 - cap[j], 0);
					bits[j] = num20 - num21;
					int num22 = C * num19 + ((C == 2 && num19 > 2 && dual_stereo == 0 && j < intensity) ? 1 : 0);
					int num23 = num22 * ((int)m.logN[j] + num3);
					int num24 = (num23 >> 1) - num22 * 21;
					if (num19 == 2)
					{
						num24 += num22 << 3 >> 2;
					}
					if (bits[j] + num24 < num22 * 2 << 3)
					{
						num24 += num23 >> 2;
					}
					else if (bits[j] + num24 < num22 * 3 << 3)
					{
						num24 += num23 >> 3;
					}
					ebits[j] = Inlines.IMAX(0, bits[j] + num24 + (num22 << 2));
					ebits[j] = Inlines.celt_udiv(ebits[j], num22) >> 3;
					if (C * ebits[j] > bits[j] >> 3)
					{
						ebits[j] = bits[j] >> num2 >> 3;
					}
					ebits[j] = Inlines.IMIN(ebits[j], 8);
					fine_priority[j] = ((ebits[j] * (num22 << 3) >= bits[j] + num24) ? 1 : 0);
					bits[j] -= C * ebits[j] << 3;
				}
				else
				{
					num21 = Inlines.MAX32(0, num20 - (C << 3));
					bits[j] = num20 - num21;
					ebits[j] = 0;
					fine_priority[j] = 1;
				}
				if (num21 > 0)
				{
					int num25 = Inlines.IMIN(num21 >> num2 + 3, 8 - ebits[j]);
					ebits[j] += num25;
					int num26 = num25 * C << 3;
					fine_priority[j] = ((num26 >= num21 - num18) ? 1 : 0);
					num21 -= num26;
				}
				num18 = num21;
			}
			_balance = num18;
			while (j < end)
			{
				ebits[j] = bits[j] >> num2 >> 3;
				bits[j] = 0;
				fine_priority[j] = ((ebits[j] < 1) ? 1 : 0);
				j++;
			}
			return num11;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00031EA8 File Offset: 0x000300A8
		internal static int compute_allocation_encode(CeltMode m, int start, int end, int[] offsets, int[] cap, int alloc_trim, ref int intensity, ref int dual_stereo, int total, out int balance, int[] pulses, int[] ebits, int[] fine_priority, int C, int LM, EntropyCoder ec, Span<byte> encodedData, int prev, int signalBandwidth)
		{
			total = Inlines.IMAX(total, 0);
			int nbEBands = m.nbEBands;
			int skip_start = start;
			int num = (total >= 8) ? 8 : 0;
			total -= num;
			int num3;
			int num2 = num3 = 0;
			if (C == 2)
			{
				num3 = (int)Rate.LOG2_FRAC_TABLE[end - start];
				if (num3 > total)
				{
					num3 = 0;
				}
				else
				{
					total -= num3;
					num2 = ((total >= 8) ? 8 : 0);
					total -= num2;
				}
			}
			int[] array = new int[nbEBands];
			int[] array2 = new int[nbEBands];
			int[] array3 = new int[nbEBands];
			int[] array4 = new int[nbEBands];
			for (int i = start; i < end; i++)
			{
				array3[i] = Inlines.IMAX(C << 3, 3 * (m.eBands[i + 1] - m.eBands[i]) << LM << 3 >> 4);
				array4[i] = C * (int)(m.eBands[i + 1] - m.eBands[i]) * (alloc_trim - 5 - LM) * (end - i - 1) * (1 << LM + 3) >> 6;
				if ((int)(m.eBands[i + 1] - m.eBands[i]) << LM == 1)
				{
					array4[i] -= C << 3;
				}
			}
			int num4 = 1;
			int num5 = m.nbAllocVectors - 1;
			do
			{
				int num6 = 0;
				int num7 = 0;
				int num8 = num4 + num5 >> 1;
				int i = end;
				while (i-- > start)
				{
					int num9 = (int)(m.eBands[i + 1] - m.eBands[i]);
					int num10 = C * num9 * (int)m.allocVectors[num8 * nbEBands + i] << LM >> 2;
					if (num10 > 0)
					{
						num10 = Inlines.IMAX(0, num10 + array4[i]);
					}
					num10 += offsets[i];
					if (num10 >= array3[i] || num6 != 0)
					{
						num6 = 1;
						num7 += Inlines.IMIN(num10, cap[i]);
					}
					else if (num10 >= C << 3)
					{
						num7 += C << 3;
					}
				}
				if (num7 > total)
				{
					num5 = num8 - 1;
				}
				else
				{
					num4 = num8 + 1;
				}
			}
			while (num4 <= num5);
			num5 = num4--;
			for (int i = start; i < end; i++)
			{
				int num11 = (int)(m.eBands[i + 1] - m.eBands[i]);
				int num12 = C * num11 * (int)m.allocVectors[num4 * nbEBands + i] << LM >> 2;
				int num13 = (num5 >= m.nbAllocVectors) ? cap[i] : (C * num11 * (int)m.allocVectors[num5 * nbEBands + i] << LM >> 2);
				if (num12 > 0)
				{
					num12 = Inlines.IMAX(0, num12 + array4[i]);
				}
				if (num13 > 0)
				{
					num13 = Inlines.IMAX(0, num13 + array4[i]);
				}
				if (num4 > 0)
				{
					num12 += offsets[i];
				}
				num13 += offsets[i];
				if (offsets[i] > 0)
				{
					skip_start = i;
				}
				num13 = Inlines.IMAX(0, num13 - num12);
				array[i] = num12;
				array2[i] = num13;
			}
			return Rate.interp_bits2pulses_encode(m, start, end, skip_start, array, array2, array3, cap, total, out balance, num, ref intensity, num3, ref dual_stereo, num2, pulses, ebits, fine_priority, C, LM, ec, encodedData, prev, signalBandwidth);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0003219C File Offset: 0x0003039C
		internal static int compute_allocation_decode(CeltMode m, int start, int end, int[] offsets, int[] cap, int alloc_trim, ref int intensity, ref int dual_stereo, int total, out int balance, int[] pulses, int[] ebits, int[] fine_priority, int C, int LM, EntropyCoder ec, ReadOnlySpan<byte> encodedData, int prev, int signalBandwidth)
		{
			total = Inlines.IMAX(total, 0);
			int nbEBands = m.nbEBands;
			int skip_start = start;
			int num = (total >= 8) ? 8 : 0;
			total -= num;
			int num3;
			int num2 = num3 = 0;
			if (C == 2)
			{
				num3 = (int)Rate.LOG2_FRAC_TABLE[end - start];
				if (num3 > total)
				{
					num3 = 0;
				}
				else
				{
					total -= num3;
					num2 = ((total >= 8) ? 8 : 0);
					total -= num2;
				}
			}
			int[] array = new int[nbEBands];
			int[] array2 = new int[nbEBands];
			int[] array3 = new int[nbEBands];
			int[] array4 = new int[nbEBands];
			for (int i = start; i < end; i++)
			{
				array3[i] = Inlines.IMAX(C << 3, 3 * (m.eBands[i + 1] - m.eBands[i]) << LM << 3 >> 4);
				array4[i] = C * (int)(m.eBands[i + 1] - m.eBands[i]) * (alloc_trim - 5 - LM) * (end - i - 1) * (1 << LM + 3) >> 6;
				if ((int)(m.eBands[i + 1] - m.eBands[i]) << LM == 1)
				{
					array4[i] -= C << 3;
				}
			}
			int num4 = 1;
			int num5 = m.nbAllocVectors - 1;
			do
			{
				int num6 = 0;
				int num7 = 0;
				int num8 = num4 + num5 >> 1;
				int i = end;
				while (i-- > start)
				{
					int num9 = (int)(m.eBands[i + 1] - m.eBands[i]);
					int num10 = C * num9 * (int)m.allocVectors[num8 * nbEBands + i] << LM >> 2;
					if (num10 > 0)
					{
						num10 = Inlines.IMAX(0, num10 + array4[i]);
					}
					num10 += offsets[i];
					if (num10 >= array3[i] || num6 != 0)
					{
						num6 = 1;
						num7 += Inlines.IMIN(num10, cap[i]);
					}
					else if (num10 >= C << 3)
					{
						num7 += C << 3;
					}
				}
				if (num7 > total)
				{
					num5 = num8 - 1;
				}
				else
				{
					num4 = num8 + 1;
				}
			}
			while (num4 <= num5);
			num5 = num4--;
			for (int i = start; i < end; i++)
			{
				int num11 = (int)(m.eBands[i + 1] - m.eBands[i]);
				int num12 = C * num11 * (int)m.allocVectors[num4 * nbEBands + i] << LM >> 2;
				int num13 = (num5 >= m.nbAllocVectors) ? cap[i] : (C * num11 * (int)m.allocVectors[num5 * nbEBands + i] << LM >> 2);
				if (num12 > 0)
				{
					num12 = Inlines.IMAX(0, num12 + array4[i]);
				}
				if (num13 > 0)
				{
					num13 = Inlines.IMAX(0, num13 + array4[i]);
				}
				if (num4 > 0)
				{
					num12 += offsets[i];
				}
				num13 += offsets[i];
				if (offsets[i] > 0)
				{
					skip_start = i;
				}
				num13 = Inlines.IMAX(0, num13 - num12);
				array[i] = num12;
				array2[i] = num13;
			}
			return Rate.interp_bits2pulses_decode(m, start, end, skip_start, array, array2, array3, cap, total, out balance, num, ref intensity, num3, ref dual_stereo, num2, pulses, ebits, fine_priority, C, LM, ec, encodedData, prev, signalBandwidth);
		}

		// Token: 0x0400045A RID: 1114
		private static readonly byte[] LOG2_FRAC_TABLE = new byte[]
		{
			0,
			8,
			13,
			16,
			19,
			21,
			23,
			24,
			26,
			27,
			28,
			29,
			30,
			31,
			32,
			32,
			33,
			34,
			34,
			35,
			36,
			36,
			37,
			37
		};

		// Token: 0x0400045B RID: 1115
		private const int ALLOC_STEPS = 6;
	}
}
