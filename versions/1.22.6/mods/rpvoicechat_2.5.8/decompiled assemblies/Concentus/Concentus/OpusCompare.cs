using System;
using Concentus.Common.CPlusPlus;

namespace Concentus
{
	// Token: 0x0200000C RID: 12
	internal static class OpusCompare
	{
		// Token: 0x06000096 RID: 150 RVA: 0x00004628 File Offset: 0x00002828
		private static void band_energy(Pointer<float> _out, Pointer<float> _ps, Pointer<int> _bands, int _nbands, Pointer<float> _in, int _nchannels, int _nframes, int _window_sz, int _step, int _downsample)
		{
			Pointer<float> pointer = Pointer.Malloc<float>((3 + _nchannels) * _window_sz);
			Pointer<float> pointer2 = pointer.Point(_window_sz);
			Pointer<float> pointer3 = pointer2.Point(_window_sz);
			Pointer<float> pointer4 = pointer3.Point(_window_sz);
			int num = _window_sz / 2;
			for (int i = 0; i < _window_sz; i++)
			{
				pointer[i] = (float)(0.5 - 0.5 * Math.Cos(6.283185307179586 / (double)(_window_sz - 1) * (double)i));
			}
			for (int i = 0; i < _window_sz; i++)
			{
				pointer2[i] = (float)Math.Cos(6.283185307179586 / (double)_window_sz * (double)i);
			}
			for (int i = 0; i < _window_sz; i++)
			{
				pointer3[i] = (float)Math.Sin(6.283185307179586 / (double)_window_sz * (double)i);
			}
			for (int j = 0; j < _nframes; j++)
			{
				for (int k = 0; k < _nchannels; k++)
				{
					for (int l = 0; l < _window_sz; l++)
					{
						pointer4[k * _window_sz + l] = pointer[l] * _in[(j * _step + l) * _nchannels + k];
					}
				}
				int m;
				int i = m = 0;
				while (m < _nbands)
				{
					float[] array = new float[2];
					while (i < _bands[m + 1])
					{
						for (int k = 0; k < _nchannels; k++)
						{
							int num2 = 0;
							float num4;
							float num3 = num4 = 0f;
							for (int l = 0; l < _window_sz; l++)
							{
								num4 += pointer2[num2] * pointer4[k * _window_sz + l];
								num3 -= pointer3[num2] * pointer4[k * _window_sz + l];
								num2 += i;
								if (num2 >= _window_sz)
								{
									num2 -= _window_sz;
								}
							}
							num4 *= (float)_downsample;
							num3 *= (float)_downsample;
							_ps[(j * num + i) * _nchannels + k] = num4 * num4 + num3 * num3 + 100000f;
							array[k] += _ps[(j * num + i) * _nchannels + k];
						}
						i++;
					}
					if (_out != null)
					{
						_out[(j * _nbands + m) * _nchannels] = array[0] / (float)(_bands[m + 1] - _bands[m]);
						if (_nchannels == 2)
						{
							_out[(j * _nbands + m) * _nchannels + 1] = array[1] / (float)(_bands[m + 1] - _bands[m]);
						}
					}
					m++;
				}
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000048E0 File Offset: 0x00002AE0
		internal static float compare(float[] x, float[] y, int nchannels, int rate = 48000)
		{
			int num = x.Length;
			int num2 = y.Length;
			int num3 = 21;
			int num4 = 240;
			if (rate != 8000 && rate != 12000 && rate != 16000 && rate != 24000 && rate != 48000)
			{
				throw new ArgumentException("Sampling rate must be 8000, 12000, 16000, 24000, or 48000\n");
			}
			int num5;
			if (rate != 48000)
			{
				num5 = 48000 / rate;
				if (rate <= 12000)
				{
					if (rate != 8000)
					{
						if (rate == 12000)
						{
							num3 = 15;
						}
					}
					else
					{
						num3 = 13;
					}
				}
				else if (rate != 16000)
				{
					if (rate == 24000)
					{
						num3 = 19;
					}
				}
				else
				{
					num3 = 17;
				}
				num4 = 240 / num5;
			}
			else
			{
				num5 = 1;
			}
			if (num != num2 * num5)
			{
				throw new ArgumentException("Sample counts do not match");
			}
			if (num < 480)
			{
				throw new ArgumentException("Insufficient sample data");
			}
			int num6 = (num - 480 + 120) / 120;
			Pointer<float> pointer = Pointer.Malloc<float>(num6 * 21 * nchannels);
			Pointer<float> pointer2 = Pointer.Malloc<float>(num6 * 240 * nchannels);
			Pointer<float> pointer3 = Pointer.Malloc<float>(num6 * num4 * nchannels);
			OpusCompare.band_energy(pointer, pointer2, OpusCompare.BANDS.GetPointer(0), 21, x.GetPointer(0), nchannels, num6, 480, 120, 1);
			OpusCompare.band_energy(null, pointer3, OpusCompare.BANDS.GetPointer(0), num3, y.GetPointer(0), nchannels, num6, 480 / num5, 120 / num5, num5);
			for (int i = 0; i < num6; i++)
			{
				int j;
				for (j = 1; j < 21; j++)
				{
					for (int k = 0; k < nchannels; k++)
					{
						Pointer<float> pointer4 = pointer;
						int num7 = (i * 21 + j) * nchannels + k;
						pointer4[num7] += 0.1f * pointer[(i * 21 + j - 1) * nchannels + k];
					}
				}
				j = 20;
				while (j-- > 0)
				{
					for (int k = 0; k < nchannels; k++)
					{
						Pointer<float> pointer4 = pointer;
						int num7 = (i * 21 + j) * nchannels + k;
						pointer4[num7] += 0.03f * pointer[(i * 21 + j + 1) * nchannels + k];
					}
				}
				if (i > 0)
				{
					for (j = 0; j < 21; j++)
					{
						for (int k = 0; k < nchannels; k++)
						{
							Pointer<float> pointer4 = pointer;
							int num7 = (i * 21 + j) * nchannels + k;
							pointer4[num7] += 0.5f * pointer[((i - 1) * 21 + j) * nchannels + k];
						}
					}
				}
				if (nchannels == 2)
				{
					for (j = 0; j < 21; j++)
					{
						float num8 = pointer[(i * 21 + j) * nchannels];
						float num9 = pointer[(i * 21 + j) * nchannels + 1];
						Pointer<float> pointer4 = pointer;
						int num7 = (i * 21 + j) * nchannels;
						pointer4[num7] += 0.01f * num9;
						pointer4 = pointer;
						num7 = (i * 21 + j) * nchannels + 1;
						pointer4[num7] += 0.01f * num8;
					}
				}
				for (j = 0; j < num3; j++)
				{
					for (int l = OpusCompare.BANDS[j]; l < OpusCompare.BANDS[j + 1]; l++)
					{
						for (int k = 0; k < nchannels; k++)
						{
							Pointer<float> pointer4 = pointer2;
							int num7 = (i * 240 + l) * nchannels + k;
							pointer4[num7] += 0.1f * pointer[(i * 21 + j) * nchannels + k];
							pointer4 = pointer3;
							num7 = (i * num4 + l) * nchannels + k;
							pointer4[num7] += 0.1f * pointer[(i * 21 + j) * nchannels + k];
						}
					}
				}
			}
			for (int j = 0; j < num3; j++)
			{
				for (int l = OpusCompare.BANDS[j]; l < OpusCompare.BANDS[j + 1]; l++)
				{
					for (int k = 0; k < nchannels; k++)
					{
						float num10 = pointer2[l * nchannels + k];
						float num11 = pointer3[l * nchannels + k];
						for (int i = 1; i < num6; i++)
						{
							float num12 = pointer2[(i * 240 + l) * nchannels + k];
							float num13 = pointer3[(i * num4 + l) * nchannels + k];
							Pointer<float> pointer4 = pointer2;
							int num7 = (i * 240 + l) * nchannels + k;
							pointer4[num7] += num10;
							pointer4 = pointer3;
							num7 = (i * num4 + l) * nchannels + k;
							pointer4[num7] += num11;
							num10 = num12;
							num11 = num13;
						}
					}
				}
			}
			int num14;
			if (rate == 48000)
			{
				num14 = OpusCompare.BANDS[21];
			}
			else if (rate == 12000)
			{
				num14 = OpusCompare.BANDS[num3];
			}
			else
			{
				num14 = OpusCompare.BANDS[num3] - 3;
			}
			double num15 = 0.0;
			for (int i = 0; i < num6; i++)
			{
				double num16 = 0.0;
				for (int j = 0; j < num3; j++)
				{
					double num17 = 0.0;
					int l = OpusCompare.BANDS[j];
					while (l < OpusCompare.BANDS[j + 1] && l < num14)
					{
						for (int k = 0; k < nchannels; k++)
						{
							float num18 = pointer3[(i * num4 + l) * nchannels + k] / pointer2[(i * 240 + l) * nchannels + k];
							float num19 = num18 - (float)Math.Log((double)num18) - 1f;
							if (l >= 79 && l <= 81)
							{
								num19 *= 0.1f;
							}
							if (l == 80)
							{
								num19 *= 0.1f;
							}
							num17 += (double)num19;
						}
						l++;
					}
					num17 /= (double)((OpusCompare.BANDS[j + 1] - OpusCompare.BANDS[j]) * nchannels);
					num16 += num17 * num17;
				}
				num16 /= 21.0;
				num16 *= num16;
				num15 += num16 * num16;
			}
			num15 = Math.Pow(num15 / (double)num6, 0.0625);
			return (float)(100.0 * (1.0 - 0.5 * Math.Log(1.0 + num15) / Math.Log(1.13)));
		}

		// Token: 0x0400000D RID: 13
		private const int NBANDS = 21;

		// Token: 0x0400000E RID: 14
		private const int NFREQS = 240;

		// Token: 0x0400000F RID: 15
		private const int TEST_WIN_SIZE = 480;

		// Token: 0x04000010 RID: 16
		private const int TEST_WIN_STEP = 120;

		// Token: 0x04000011 RID: 17
		private static readonly int[] BANDS = new int[]
		{
			0,
			2,
			4,
			6,
			8,
			10,
			12,
			14,
			16,
			20,
			24,
			28,
			32,
			40,
			48,
			56,
			68,
			80,
			96,
			120,
			156,
			200
		};
	}
}
