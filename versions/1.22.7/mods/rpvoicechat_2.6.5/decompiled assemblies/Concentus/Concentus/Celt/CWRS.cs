using System;
using Concentus.Common;

namespace Concentus.Celt
{
	// Token: 0x02000088 RID: 136
	internal static class CWRS
	{
		// Token: 0x060004C8 RID: 1224 RVA: 0x0002DAA1 File Offset: 0x0002BCA1
		private static uint CELT_PVQ_U(int _n, int _k)
		{
			return Tables.CELT_PVQ_U_DATA[(int)(checked((IntPtr)(unchecked((ulong)CWRS.CELT_PVQ_U_ROW[Inlines.IMIN(_n, _k)] + (ulong)((long)Inlines.IMAX(_n, _k))))))];
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0002DAC1 File Offset: 0x0002BCC1
		private static uint CELT_PVQ_V(int _n, int _k)
		{
			return CWRS.CELT_PVQ_U(_n, _k) + CWRS.CELT_PVQ_U(_n, _k + 1);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0002DAD4 File Offset: 0x0002BCD4
		internal static uint icwrs(int _n, int[] _y)
		{
			int num = _n - 1;
			uint num2 = (_y[num] < 0) ? 1U : 0U;
			int num3 = Inlines.abs(_y[num]);
			do
			{
				num--;
				num2 += CWRS.CELT_PVQ_U(_n - num, num3);
				num3 += Inlines.abs(_y[num]);
				if (_y[num] < 0)
				{
					num2 += CWRS.CELT_PVQ_U(_n - num, num3 + 1);
				}
			}
			while (num > 0);
			return num2;
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0002DB29 File Offset: 0x0002BD29
		internal static void encode_pulses(int[] _y, int _n, int _k, EntropyCoder _enc, Span<byte> encodedData)
		{
			_enc.enc_uint(encodedData, CWRS.icwrs(_n, _y), CWRS.CELT_PVQ_V(_n, _k));
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0002DB44 File Offset: 0x0002BD44
		internal static int cwrsi(int _n, int _k, uint _i, int[] _y)
		{
			int c = 0;
			int num = 0;
			uint num3;
			int num4;
			int num5;
			short num7;
			while (_n > 2)
			{
				if (_k >= _n)
				{
					uint num2 = CWRS.CELT_PVQ_U_ROW[_n];
					num3 = Tables.CELT_PVQ_U_DATA[(int)(checked((IntPtr)(unchecked((ulong)num2 + (ulong)((long)_k) + 1UL))))];
					num4 = ((false - (_i >= num3)) ? 1 : 0);
					_i -= (num3 & (uint)num4);
					num5 = _k;
					checked
					{
						uint num6 = Tables.CELT_PVQ_U_DATA[(int)((IntPtr)(unchecked((ulong)num2 + (ulong)((long)_n))))];
						if (num6 > _i)
						{
							_k = _n;
							do
							{
								num3 = Tables.CELT_PVQ_U_DATA[(int)((IntPtr)(unchecked((ulong)CWRS.CELT_PVQ_U_ROW[--_k] + (ulong)((long)_n))))];
							}
							while (num3 > _i);
						}
						else
						{
							for (num3 = Tables.CELT_PVQ_U_DATA[(int)((IntPtr)(unchecked((ulong)num2 + (ulong)((long)_k))))]; num3 > _i; num3 = Tables.CELT_PVQ_U_DATA[(int)((IntPtr)(unchecked((ulong)num2 + (ulong)((long)_k))))])
							{
								unchecked
								{
									_k--;
								}
							}
						}
					}
					_i -= num3;
					num7 = (short)(num5 - _k + num4 ^ num4);
					_y[num++] = (int)num7;
					c = Inlines.MAC16_16(c, num7, num7);
				}
				else
				{
					uint num6;
					checked
					{
						num3 = Tables.CELT_PVQ_U_DATA[(int)((IntPtr)(unchecked((ulong)CWRS.CELT_PVQ_U_ROW[_k] + (ulong)((long)_n))))];
						num6 = Tables.CELT_PVQ_U_DATA[(int)((IntPtr)(unchecked((ulong)CWRS.CELT_PVQ_U_ROW[_k + 1] + (ulong)((long)_n))))];
					}
					if (num3 <= _i && _i < num6)
					{
						_i -= num3;
						_y[num++] = 0;
					}
					else
					{
						num4 = ((false - (_i >= num6)) ? 1 : 0);
						_i -= (num6 & (uint)num4);
						num5 = _k;
						do
						{
							num3 = Tables.CELT_PVQ_U_DATA[(int)(checked((IntPtr)(unchecked((ulong)CWRS.CELT_PVQ_U_ROW[--_k] + (ulong)((long)_n)))))];
						}
						while (num3 > _i);
						_i -= num3;
						num7 = (short)(num5 - _k + num4 ^ num4);
						_y[num++] = (int)num7;
						c = Inlines.MAC16_16(c, num7, num7);
					}
				}
				_n--;
			}
			num3 = (uint)(2 * _k + 1);
			num4 = ((false - (_i >= num3)) ? 1 : 0);
			_i -= (num3 & (uint)num4);
			num5 = _k;
			_k = (int)(_i + 1U >> 1);
			if (_k != 0)
			{
				_i -= (uint)(2 * _k - 1);
			}
			num7 = (short)(num5 - _k + num4 ^ num4);
			_y[num++] = (int)num7;
			c = Inlines.MAC16_16(c, num7, num7);
			num4 = (int)(-(int)_i);
			num7 = (short)(_k + num4 ^ num4);
			_y[num] = (int)num7;
			return Inlines.MAC16_16(c, num7, num7);
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0002DD1D File Offset: 0x0002BF1D
		internal static int decode_pulses(int[] _y, int _n, int _k, EntropyCoder _dec, ReadOnlySpan<byte> encodedData)
		{
			return CWRS.cwrsi(_n, _k, _dec.dec_uint(encodedData, CWRS.CELT_PVQ_V(_n, _k)), _y);
		}

		// Token: 0x04000450 RID: 1104
		internal static readonly uint[] CELT_PVQ_U_ROW = new uint[]
		{
			0U,
			176U,
			351U,
			525U,
			698U,
			870U,
			1041U,
			1131U,
			1178U,
			1207U,
			1226U,
			1240U,
			1248U,
			1254U,
			1257U
		};
	}
}
