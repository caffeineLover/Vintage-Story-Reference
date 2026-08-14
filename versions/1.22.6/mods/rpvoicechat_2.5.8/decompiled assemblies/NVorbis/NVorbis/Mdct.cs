using System;
using System.Collections.Generic;
using NVorbis.Contracts;

namespace NVorbis
{
	// Token: 0x0200000D RID: 13
	internal class Mdct : IMdct
	{
		// Token: 0x06000068 RID: 104 RVA: 0x00003F78 File Offset: 0x00002178
		public void Reverse(float[] samples, int sampleCount)
		{
			Mdct.MdctImpl mdctImpl;
			if (!this._setupCache.TryGetValue(sampleCount, ref mdctImpl))
			{
				mdctImpl = new Mdct.MdctImpl(sampleCount);
				this._setupCache[sampleCount] = mdctImpl;
			}
			mdctImpl.CalcReverse(samples);
		}

		// Token: 0x04000037 RID: 55
		private const float M_PI = 3.1415927f;

		// Token: 0x04000038 RID: 56
		private Dictionary<int, Mdct.MdctImpl> _setupCache = new Dictionary<int, Mdct.MdctImpl>();

		// Token: 0x02000041 RID: 65
		private class MdctImpl
		{
			// Token: 0x0600025E RID: 606 RVA: 0x000087B0 File Offset: 0x000069B0
			public MdctImpl(int n)
			{
				this._n = n;
				this._n2 = n >> 1;
				this._n4 = this._n2 >> 1;
				this._n8 = this._n4 >> 1;
				this._ld = Utils.ilog(n) - 1;
				this._a = new float[this._n2];
				this._b = new float[this._n2];
				this._c = new float[this._n4];
				int i;
				int num = i = 0;
				while (i < this._n4)
				{
					this._a[num] = (float)Math.Cos((double)((float)(4 * i) * 3.1415927f / (float)n));
					this._a[num + 1] = (float)(-(float)Math.Sin((double)((float)(4 * i) * 3.1415927f / (float)n)));
					this._b[num] = (float)Math.Cos((double)((float)(num + 1) * 3.1415927f / (float)n / 2f)) * 0.5f;
					this._b[num + 1] = (float)Math.Sin((double)((float)(num + 1) * 3.1415927f / (float)n / 2f)) * 0.5f;
					i++;
					num += 2;
				}
				num = (i = 0);
				while (i < this._n8)
				{
					this._c[num] = (float)Math.Cos((double)((float)(2 * (num + 1)) * 3.1415927f / (float)n));
					this._c[num + 1] = (float)(-(float)Math.Sin((double)((float)(2 * (num + 1)) * 3.1415927f / (float)n)));
					i++;
					num += 2;
				}
				this._bitrev = new ushort[this._n8];
				for (int j = 0; j < this._n8; j++)
				{
					this._bitrev[j] = (ushort)(Utils.BitReverse((uint)j, this._ld - 3) << 2);
				}
			}

			// Token: 0x0600025F RID: 607 RVA: 0x0000896C File Offset: 0x00006B6C
			internal void CalcReverse(float[] buffer)
			{
				float[] array = new float[this._n2];
				int i = this._n2 - 2;
				int num = 0;
				int num2 = 0;
				int n = this._n2;
				while (num2 != n)
				{
					array[i + 1] = buffer[num2] * this._a[num] - buffer[num2 + 2] * this._a[num + 1];
					array[i] = buffer[num2] * this._a[num + 1] + buffer[num2 + 2] * this._a[num];
					i -= 2;
					num += 2;
					num2 += 4;
				}
				num2 = this._n2 - 3;
				while (i >= 0)
				{
					array[i + 1] = -buffer[num2 + 2] * this._a[num] - -buffer[num2] * this._a[num + 1];
					array[i] = -buffer[num2 + 2] * this._a[num + 1] + -buffer[num2] * this._a[num];
					i -= 2;
					num += 2;
					num2 -= 4;
				}
				float[] array2 = array;
				int j = this._n2 - 8;
				int num3 = this._n4;
				int num4 = 0;
				int num5 = this._n4;
				int num6 = 0;
				while (j >= 0)
				{
					float num7 = array2[num3 + 1] - array2[num4 + 1];
					float num8 = array2[num3] - array2[num4];
					buffer[num5 + 1] = array2[num3 + 1] + array2[num4 + 1];
					buffer[num5] = array2[num3] + array2[num4];
					buffer[num6 + 1] = num7 * this._a[j + 4] - num8 * this._a[j + 5];
					buffer[num6] = num8 * this._a[j + 4] + num7 * this._a[j + 5];
					num7 = array2[num3 + 3] - array2[num4 + 3];
					num8 = array2[num3 + 2] - array2[num4 + 2];
					buffer[num5 + 3] = array2[num3 + 3] + array2[num4 + 3];
					buffer[num5 + 2] = array2[num3 + 2] + array2[num4 + 2];
					buffer[num6 + 3] = num7 * this._a[j] - num8 * this._a[j + 1];
					buffer[num6 + 2] = num8 * this._a[j] + num7 * this._a[j + 1];
					j -= 8;
					num5 += 4;
					num6 += 4;
					num3 += 4;
					num4 += 4;
				}
				int n2 = this._n >> 4;
				int num9 = this._n2 - 1;
				int n3 = this._n4;
				this.step3_iter0_loop(n2, buffer, num9 - 0, -this._n8);
				this.step3_iter0_loop(this._n >> 4, buffer, this._n2 - 1 - this._n4, -this._n8);
				int lim = this._n >> 5;
				int num10 = this._n2 - 1;
				int n4 = this._n8;
				this.step3_inner_r_loop(lim, buffer, num10 - 0, -(this._n >> 4), 16);
				this.step3_inner_r_loop(this._n >> 5, buffer, this._n2 - 1 - this._n8, -(this._n >> 4), 16);
				this.step3_inner_r_loop(this._n >> 5, buffer, this._n2 - 1 - this._n8 * 2, -(this._n >> 4), 16);
				this.step3_inner_r_loop(this._n >> 5, buffer, this._n2 - 1 - this._n8 * 3, -(this._n >> 4), 16);
				int k;
				for (k = 2; k < this._ld - 3 >> 1; k++)
				{
					int num11 = this._n >> k + 2;
					int num12 = num11 >> 1;
					int num13 = 1 << k + 1;
					for (int l = 0; l < num13; l++)
					{
						this.step3_inner_r_loop(this._n >> k + 4, buffer, this._n2 - 1 - num11 * l, -num12, 1 << k + 3);
					}
				}
				while (k < this._ld - 6)
				{
					int num14 = this._n >> k + 2;
					int num15 = 1 << k + 3;
					int num16 = num14 >> 1;
					int num17 = this._n >> k + 6;
					int n5 = 1 << k + 1;
					int num18 = this._n2 - 1;
					int num19 = 0;
					for (int m = num17; m > 0; m--)
					{
						this.step3_inner_s_loop(n5, buffer, num18, -num16, num19, num15, num14);
						num19 += num15 * 4;
						num18 -= 8;
					}
					k++;
				}
				this.step3_inner_s_loop_ld654(this._n >> 5, buffer, this._n2 - 1, this._n);
				int num20 = 0;
				int num21 = this._n4 - 4;
				int num22 = this._n2 - 4;
				while (num21 >= 0)
				{
					int num23 = (int)this._bitrev[num20];
					array2[num22 + 3] = buffer[num23];
					array2[num22 + 2] = buffer[num23 + 1];
					array2[num21 + 3] = buffer[num23 + 2];
					array2[num21 + 2] = buffer[num23 + 3];
					num23 = (int)this._bitrev[num20 + 1];
					array2[num22 + 1] = buffer[num23];
					array2[num22] = buffer[num23 + 1];
					array2[num21 + 1] = buffer[num23 + 2];
					array2[num21] = buffer[num23 + 3];
					num21 -= 4;
					num22 -= 4;
					num20 += 2;
				}
				int num24 = 0;
				int num25 = 0;
				int num26 = this._n2 - 4;
				while (num25 < num26)
				{
					float num27 = array2[num25] - array2[num26 + 2];
					float num28 = array2[num25 + 1] + array2[num26 + 3];
					float num29 = this._c[num24 + 1] * num27 + this._c[num24] * num28;
					float num30 = this._c[num24 + 1] * num28 - this._c[num24] * num27;
					float num31 = array2[num25] + array2[num26 + 2];
					float num32 = array2[num25 + 1] - array2[num26 + 3];
					array2[num25] = num31 + num29;
					array2[num25 + 1] = num32 + num30;
					array2[num26 + 2] = num31 - num29;
					array2[num26 + 3] = num30 - num32;
					num27 = array2[num25 + 2] - array2[num26];
					num28 = array2[num25 + 3] + array2[num26 + 1];
					num29 = this._c[num24 + 3] * num27 + this._c[num24 + 2] * num28;
					num30 = this._c[num24 + 3] * num28 - this._c[num24 + 2] * num27;
					num31 = array2[num25 + 2] + array2[num26];
					num32 = array2[num25 + 3] - array2[num26 + 1];
					array2[num25 + 2] = num31 + num29;
					array2[num25 + 3] = num32 + num30;
					array2[num26] = num31 - num29;
					array2[num26 + 1] = num30 - num32;
					num24 += 4;
					num25 += 4;
					num26 -= 4;
				}
				int num33 = this._n2 - 8;
				int num34 = this._n2 - 8;
				int num35 = 0;
				int num36 = this._n2 - 4;
				int num37 = this._n2;
				int num38 = this._n - 4;
				while (num34 >= 0)
				{
					float num39 = array[num34 + 6] * this._b[num33 + 7] - array[num34 + 7] * this._b[num33 + 6];
					float num40 = -array[num34 + 6] * this._b[num33 + 6] - array[num34 + 7] * this._b[num33 + 7];
					buffer[num35] = num39;
					buffer[num36 + 3] = -num39;
					buffer[num37] = num40;
					buffer[num38 + 3] = num40;
					float num41 = array[num34 + 4] * this._b[num33 + 5] - array[num34 + 5] * this._b[num33 + 4];
					float num42 = -array[num34 + 4] * this._b[num33 + 4] - array[num34 + 5] * this._b[num33 + 5];
					buffer[num35 + 1] = num41;
					buffer[num36 + 2] = -num41;
					buffer[num37 + 1] = num42;
					buffer[num38 + 2] = num42;
					num39 = array[num34 + 2] * this._b[num33 + 3] - array[num34 + 3] * this._b[num33 + 2];
					num40 = -array[num34 + 2] * this._b[num33 + 2] - array[num34 + 3] * this._b[num33 + 3];
					buffer[num35 + 2] = num39;
					buffer[num36 + 1] = -num39;
					buffer[num37 + 2] = num40;
					buffer[num38 + 1] = num40;
					num41 = array[num34] * this._b[num33 + 1] - array[num34 + 1] * this._b[num33];
					num42 = -array[num34] * this._b[num33] - array[num34 + 1] * this._b[num33 + 1];
					buffer[num35 + 3] = num41;
					buffer[num36] = -num41;
					buffer[num37 + 3] = num42;
					buffer[num38] = num42;
					num33 -= 8;
					num34 -= 8;
					num35 += 4;
					num37 += 4;
					num36 -= 4;
					num38 -= 4;
				}
			}

			// Token: 0x06000260 RID: 608 RVA: 0x00009200 File Offset: 0x00007400
			private void step3_iter0_loop(int n, float[] e, int i_off, int k_off)
			{
				int num = i_off;
				int num2 = num + k_off;
				int num3 = 0;
				for (int i = n >> 2; i > 0; i--)
				{
					float num4 = e[num] - e[num2];
					float num5 = e[num - 1] - e[num2 - 1];
					e[num] += e[num2];
					e[num - 1] += e[num2 - 1];
					e[num2] = num4 * this._a[num3] - num5 * this._a[num3 + 1];
					e[num2 - 1] = num5 * this._a[num3] + num4 * this._a[num3 + 1];
					num3 += 8;
					num4 = e[num - 2] - e[num2 - 2];
					num5 = e[num - 3] - e[num2 - 3];
					e[num - 2] += e[num2 - 2];
					e[num - 3] += e[num2 - 3];
					e[num2 - 2] = num4 * this._a[num3] - num5 * this._a[num3 + 1];
					e[num2 - 3] = num5 * this._a[num3] + num4 * this._a[num3 + 1];
					num3 += 8;
					num4 = e[num - 4] - e[num2 - 4];
					num5 = e[num - 5] - e[num2 - 5];
					e[num - 4] += e[num2 - 4];
					e[num - 5] += e[num2 - 5];
					e[num2 - 4] = num4 * this._a[num3] - num5 * this._a[num3 + 1];
					e[num2 - 5] = num5 * this._a[num3] + num4 * this._a[num3 + 1];
					num3 += 8;
					num4 = e[num - 6] - e[num2 - 6];
					num5 = e[num - 7] - e[num2 - 7];
					e[num - 6] += e[num2 - 6];
					e[num - 7] += e[num2 - 7];
					e[num2 - 6] = num4 * this._a[num3] - num5 * this._a[num3 + 1];
					e[num2 - 7] = num5 * this._a[num3] + num4 * this._a[num3 + 1];
					num3 += 8;
					num -= 8;
					num2 -= 8;
				}
			}

			// Token: 0x06000261 RID: 609 RVA: 0x00009420 File Offset: 0x00007620
			private void step3_inner_r_loop(int lim, float[] e, int d0, int k_off, int k1)
			{
				int num = d0;
				int num2 = num + k_off;
				int num3 = 0;
				for (int i = lim >> 2; i > 0; i--)
				{
					float num4 = e[num] - e[num2];
					float num5 = e[num - 1] - e[num2 - 1];
					e[num] += e[num2];
					e[num - 1] += e[num2 - 1];
					e[num2] = num4 * this._a[num3] - num5 * this._a[num3 + 1];
					e[num2 - 1] = num5 * this._a[num3] + num4 * this._a[num3 + 1];
					num3 += k1;
					num4 = e[num - 2] - e[num2 - 2];
					num5 = e[num - 3] - e[num2 - 3];
					e[num - 2] += e[num2 - 2];
					e[num - 3] += e[num2 - 3];
					e[num2 - 2] = num4 * this._a[num3] - num5 * this._a[num3 + 1];
					e[num2 - 3] = num5 * this._a[num3] + num4 * this._a[num3 + 1];
					num3 += k1;
					num4 = e[num - 4] - e[num2 - 4];
					num5 = e[num - 5] - e[num2 - 5];
					e[num - 4] += e[num2 - 4];
					e[num - 5] += e[num2 - 5];
					e[num2 - 4] = num4 * this._a[num3] - num5 * this._a[num3 + 1];
					e[num2 - 5] = num5 * this._a[num3] + num4 * this._a[num3 + 1];
					num3 += k1;
					num4 = e[num - 6] - e[num2 - 6];
					num5 = e[num - 7] - e[num2 - 7];
					e[num - 6] += e[num2 - 6];
					e[num - 7] += e[num2 - 7];
					e[num2 - 6] = num4 * this._a[num3] - num5 * this._a[num3 + 1];
					e[num2 - 7] = num5 * this._a[num3] + num4 * this._a[num3 + 1];
					num3 += k1;
					num -= 8;
					num2 -= 8;
				}
			}

			// Token: 0x06000262 RID: 610 RVA: 0x0000964C File Offset: 0x0000784C
			private void step3_inner_s_loop(int n, float[] e, int i_off, int k_off, int a, int a_off, int k0)
			{
				float num = this._a[a];
				float num2 = this._a[a + 1];
				float num3 = this._a[a + a_off];
				float num4 = this._a[a + a_off + 1];
				float num5 = this._a[a + a_off * 2];
				float num6 = this._a[a + a_off * 2 + 1];
				float num7 = this._a[a + a_off * 3];
				float num8 = this._a[a + a_off * 3 + 1];
				int num9 = i_off;
				int num10 = num9 + k_off;
				for (int i = n; i > 0; i--)
				{
					float num11 = e[num9] - e[num10];
					float num12 = e[num9 - 1] - e[num10 - 1];
					e[num9] += e[num10];
					e[num9 - 1] += e[num10 - 1];
					e[num10] = num11 * num - num12 * num2;
					e[num10 - 1] = num12 * num + num11 * num2;
					num11 = e[num9 - 2] - e[num10 - 2];
					num12 = e[num9 - 3] - e[num10 - 3];
					e[num9 - 2] += e[num10 - 2];
					e[num9 - 3] += e[num10 - 3];
					e[num10 - 2] = num11 * num3 - num12 * num4;
					e[num10 - 3] = num12 * num3 + num11 * num4;
					num11 = e[num9 - 4] - e[num10 - 4];
					num12 = e[num9 - 5] - e[num10 - 5];
					e[num9 - 4] += e[num10 - 4];
					e[num9 - 5] += e[num10 - 5];
					e[num10 - 4] = num11 * num5 - num12 * num6;
					e[num10 - 5] = num12 * num5 + num11 * num6;
					num11 = e[num9 - 6] - e[num10 - 6];
					num12 = e[num9 - 7] - e[num10 - 7];
					e[num9 - 6] += e[num10 - 6];
					e[num9 - 7] += e[num10 - 7];
					e[num10 - 6] = num11 * num7 - num12 * num8;
					e[num10 - 7] = num12 * num7 + num11 * num8;
					num9 -= k0;
					num10 -= k0;
				}
			}

			// Token: 0x06000263 RID: 611 RVA: 0x0000988C File Offset: 0x00007A8C
			private void step3_inner_s_loop_ld654(int n, float[] e, int i_off, int base_n)
			{
				int num = base_n >> 3;
				float num2 = this._a[num];
				int i = i_off;
				int num3 = i - 16 * n;
				while (i > num3)
				{
					float num4 = e[i] - e[i - 8];
					float num5 = e[i - 1] - e[i - 9];
					e[i] += e[i - 8];
					e[i - 1] += e[i - 9];
					e[i - 8] = num4;
					e[i - 9] = num5;
					num4 = e[i - 2] - e[i - 10];
					num5 = e[i - 3] - e[i - 11];
					e[i - 2] += e[i - 10];
					e[i - 3] += e[i - 11];
					e[i - 10] = (num4 + num5) * num2;
					e[i - 11] = (num5 - num4) * num2;
					num4 = e[i - 12] - e[i - 4];
					num5 = e[i - 5] - e[i - 13];
					e[i - 4] += e[i - 12];
					e[i - 5] += e[i - 13];
					e[i - 12] = num5;
					e[i - 13] = num4;
					num4 = e[i - 14] - e[i - 6];
					num5 = e[i - 7] - e[i - 15];
					e[i - 6] += e[i - 14];
					e[i - 7] += e[i - 15];
					e[i - 14] = (num4 + num5) * num2;
					e[i - 15] = (num4 - num5) * num2;
					this.iter_54(e, i);
					this.iter_54(e, i - 8);
					i -= 16;
				}
			}

			// Token: 0x06000264 RID: 612 RVA: 0x00009A28 File Offset: 0x00007C28
			private void iter_54(float[] e, int z)
			{
				float num = e[z] - e[z - 4];
				float num2 = e[z] + e[z - 4];
				float num3 = e[z - 2] + e[z - 6];
				float num4 = e[z - 2] - e[z - 6];
				e[z] = num2 + num3;
				e[z - 2] = num2 - num3;
				float num5 = e[z - 3] - e[z - 7];
				e[z - 4] = num + num5;
				e[z - 6] = num - num5;
				float num6 = e[z - 1] - e[z - 5];
				float num7 = e[z - 1] + e[z - 5];
				float num8 = e[z - 3] + e[z - 7];
				e[z - 1] = num7 + num8;
				e[z - 3] = num7 - num8;
				e[z - 5] = num6 - num4;
				e[z - 7] = num6 + num4;
			}

			// Token: 0x040000F5 RID: 245
			private readonly int _n;

			// Token: 0x040000F6 RID: 246
			private readonly int _n2;

			// Token: 0x040000F7 RID: 247
			private readonly int _n4;

			// Token: 0x040000F8 RID: 248
			private readonly int _n8;

			// Token: 0x040000F9 RID: 249
			private readonly int _ld;

			// Token: 0x040000FA RID: 250
			private readonly float[] _a;

			// Token: 0x040000FB RID: 251
			private readonly float[] _b;

			// Token: 0x040000FC RID: 252
			private readonly float[] _c;

			// Token: 0x040000FD RID: 253
			private readonly ushort[] _bitrev;
		}
	}
}
