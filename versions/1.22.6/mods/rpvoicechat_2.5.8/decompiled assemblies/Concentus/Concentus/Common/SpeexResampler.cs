using System;
using System.Numerics;

namespace Concentus.Common
{
	// Token: 0x0200007B RID: 123
	public class SpeexResampler : IResampler, IDisposable
	{
		// Token: 0x06000439 RID: 1081 RVA: 0x0002640A File Offset: 0x0002460A
		private static short FLOAT2INT(float x)
		{
			if (x < -32768f)
			{
				return short.MinValue;
			}
			if (x <= 32767f)
			{
				return (short)x;
			}
			return short.MaxValue;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0002642C File Offset: 0x0002462C
		private static double compute_func(float x, SpeexResampler.FuncDef func)
		{
			float num = x * (float)func.oversample;
			int num2 = (int)Math.Floor((double)num);
			float num3 = num - (float)num2;
			double num4 = -0.1666666667 * (double)num3 + 0.1666666667 * (double)(num3 * num3 * num3);
			double num5 = (double)num3 + 0.5 * (double)(num3 * num3) - 0.5 * (double)(num3 * num3 * num3);
			double num6 = -0.3333333333 * (double)num3 + 0.5 * (double)(num3 * num3) - 0.1666666667 * (double)(num3 * num3 * num3);
			double num7 = 1.0 - num4 - num5 - num6;
			return num6 * func.table[num2] + num7 * func.table[num2 + 1] + num5 * func.table[num2 + 2] + num4 * func.table[num2 + 3];
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00026508 File Offset: 0x00024708
		private static float sinc(float cutoff, float x, int N, SpeexResampler.FuncDef window_func)
		{
			float num = x * cutoff;
			if (Math.Abs(x) < 1E-06f)
			{
				return cutoff;
			}
			if (Math.Abs(x) > 0.5f * (float)N)
			{
				return 0f;
			}
			return (float)((double)cutoff * Math.Sin(3.141592653589793 * (double)num) / (3.141592653589793 * (double)num) * SpeexResampler.compute_func(Math.Abs(2f * x / (float)N), window_func));
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00026578 File Offset: 0x00024778
		private unsafe static void cubic_coef(float frac, Span<float> interp)
		{
			*interp[0] = -0.16667f * frac + 0.16667f * frac * frac * frac;
			*interp[1] = frac + 0.5f * frac * frac - 0.5f * frac * frac * frac;
			*interp[3] = -0.33333f * frac + 0.5f * frac * frac - 0.16667f * frac * frac * frac;
			*interp[2] = 1f - *interp[0] - *interp[1] - *interp[3];
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00026614 File Offset: 0x00024814
		private unsafe int resampler_basic_direct_single(int channel_index, Span<float> input, int input_ptr, ref int in_len, Span<float> output, int output_ptr, ref int out_len)
		{
			int num = this.filt_len;
			int num2 = 0;
			int num3 = this.last_sample[channel_index];
			int num4 = this.samp_frac_num[channel_index];
			while (num3 < in_len && num2 < out_len)
			{
				int num5 = num4 * num;
				int num6 = input_ptr + num3;
				float num7 = 0f;
				for (int i = 0; i < num; i++)
				{
					num7 += this.sinc_table[num5 + i] * *input[num6 + i];
				}
				*output[output_ptr + this.out_stride * num2++] = num7;
				num3 += this.int_advance;
				num4 += this.frac_advance;
				if (num4 >= this.den_rate)
				{
					num4 -= this.den_rate;
					num3++;
				}
			}
			this.last_sample[channel_index] = num3;
			this.samp_frac_num[channel_index] = num4;
			return num2;
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x000266E4 File Offset: 0x000248E4
		private unsafe int resampler_basic_direct_single_SIMD(int channel_index, Span<float> input, int input_ptr, ref int in_len, Span<float> output, int output_ptr, ref int out_len)
		{
			int num = this.filt_len;
			int num2 = 0;
			int num3 = this.last_sample[channel_index];
			int num4 = this.samp_frac_num[channel_index];
			while (num3 < in_len && num2 < out_len)
			{
				int num5 = num4 * num;
				int num6 = input_ptr + num3;
				int i = 0;
				float num7 = 0f;
				int num8 = num - num % Vector<float>.Count;
				while (i < num8)
				{
					num7 += Vector.Dot<float>(Vector<float>.One, Vector.Multiply<float>(new Vector<float>(this.sinc_table, num5 + i), new Vector<float>(input.Slice(num6 + i, Vector<float>.Count))));
					i += Vector<float>.Count;
				}
				while (i < num)
				{
					num7 += this.sinc_table[num5 + i] * *input[num6 + i];
					i++;
				}
				*output[output_ptr + this.out_stride * num2++] = num7;
				num3 += this.int_advance;
				num4 += this.frac_advance;
				if (num4 >= this.den_rate)
				{
					num4 -= this.den_rate;
					num3++;
				}
			}
			this.last_sample[channel_index] = num3;
			this.samp_frac_num[channel_index] = num4;
			return num2;
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0002680C File Offset: 0x00024A0C
		private unsafe int resampler_basic_interpolate_single(int channel_index, Span<float> input, int input_ptr, ref int in_len, Span<float> output, int output_ptr, ref int out_len)
		{
			int num = this.filt_len;
			int num2 = 0;
			int num3 = this.last_sample[channel_index];
			int num4 = this.samp_frac_num[channel_index];
			Span<float> interp = new Span<float>(stackalloc byte[(UIntPtr)16], 4);
			Span<float> span = new Span<float>(stackalloc byte[(UIntPtr)16], 4);
			while (num3 < in_len && num2 < out_len)
			{
				int num5 = input_ptr + num3;
				int num6 = num4 * this.oversample / this.den_rate;
				float frac = (float)(num4 * this.oversample % this.den_rate) / (float)this.den_rate;
				*span[0] = 0f;
				*span[1] = 0f;
				*span[2] = 0f;
				*span[3] = 0f;
				for (int i = 0; i < num; i++)
				{
					float num7 = *input[num5 + i];
					*span[0] += num7 * this.sinc_table[4 + (i + 1) * this.oversample - num6 - 2];
					*span[1] += num7 * this.sinc_table[4 + (i + 1) * this.oversample - num6 - 1];
					*span[2] += num7 * this.sinc_table[4 + (i + 1) * this.oversample - num6];
					*span[3] += num7 * this.sinc_table[4 + (i + 1) * this.oversample - num6 + 1];
				}
				SpeexResampler.cubic_coef(frac, interp);
				float num8 = *interp[0] * *span[0] + *interp[1] * *span[1] + *interp[2] * *span[2] + *interp[3] * *span[3];
				*output[output_ptr + this.out_stride * num2++] = num8;
				num3 += this.int_advance;
				num4 += this.frac_advance;
				if (num4 >= this.den_rate)
				{
					num4 -= this.den_rate;
					num3++;
				}
			}
			this.last_sample[channel_index] = num3;
			this.samp_frac_num[channel_index] = num4;
			return num2;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00026A3C File Offset: 0x00024C3C
		private void update_filter()
		{
			int num = this.filt_len;
			this.oversample = SpeexResampler.QualityMapping.quality_map[this.quality].oversample;
			this.filt_len = SpeexResampler.QualityMapping.quality_map[this.quality].base_length;
			if (this.num_rate > this.den_rate)
			{
				this.cutoff = SpeexResampler.QualityMapping.quality_map[this.quality].downsample_bandwidth * (float)this.den_rate / (float)this.num_rate;
				this.filt_len = this.filt_len * this.num_rate / this.den_rate;
				this.filt_len = (this.filt_len - 1 & -8) + 8;
				if (2 * this.den_rate < this.num_rate)
				{
					this.oversample >>= 1;
				}
				if (4 * this.den_rate < this.num_rate)
				{
					this.oversample >>= 1;
				}
				if (8 * this.den_rate < this.num_rate)
				{
					this.oversample >>= 1;
				}
				if (16 * this.den_rate < this.num_rate)
				{
					this.oversample >>= 1;
				}
				if (this.oversample < 1)
				{
					this.oversample = 1;
				}
			}
			else
			{
				this.cutoff = SpeexResampler.QualityMapping.quality_map[this.quality].upsample_bandwidth;
			}
			if (this.den_rate <= 16 * (this.oversample + 8))
			{
				if (this.sinc_table == null)
				{
					this.sinc_table = new float[this.filt_len * this.den_rate];
				}
				else if (this.sinc_table_length < this.filt_len * this.den_rate)
				{
					this.sinc_table = new float[this.filt_len * this.den_rate];
					this.sinc_table_length = this.filt_len * this.den_rate;
				}
				for (int i = 0; i < this.den_rate; i++)
				{
					for (int j = 0; j < this.filt_len; j++)
					{
						this.sinc_table[i * this.filt_len + j] = SpeexResampler.sinc(this.cutoff, (float)(j - this.filt_len / 2 + 1) - (float)i / (float)this.den_rate, this.filt_len, SpeexResampler.QualityMapping.quality_map[this.quality].window_func);
					}
				}
				if (Vector.IsHardwareAccelerated)
				{
					this.resampler_ptr = new SpeexResampler.resampler_basic_func(this.resampler_basic_direct_single_SIMD);
				}
				else
				{
					this.resampler_ptr = new SpeexResampler.resampler_basic_func(this.resampler_basic_direct_single);
				}
			}
			else
			{
				if (this.sinc_table == null)
				{
					this.sinc_table = new float[this.filt_len * this.oversample + 8];
				}
				else if (this.sinc_table_length < this.filt_len * this.oversample + 8)
				{
					this.sinc_table = new float[this.filt_len * this.oversample + 8];
					this.sinc_table_length = this.filt_len * this.oversample + 8;
				}
				for (int k = -4; k < this.oversample * this.filt_len + 4; k++)
				{
					this.sinc_table[k + 4] = SpeexResampler.sinc(this.cutoff, (float)k / (float)this.oversample - (float)(this.filt_len / 2), this.filt_len, SpeexResampler.QualityMapping.quality_map[this.quality].window_func);
				}
				this.resampler_ptr = new SpeexResampler.resampler_basic_func(this.resampler_basic_interpolate_single);
			}
			this.int_advance = this.num_rate / this.den_rate;
			this.frac_advance = this.num_rate % this.den_rate;
			if (this.mem == null)
			{
				this.mem_alloc_size = this.filt_len - 1 + this.buffer_size;
				this.mem = new float[this.nb_channels * this.mem_alloc_size];
				for (int l = 0; l < this.nb_channels * this.mem_alloc_size; l++)
				{
					this.mem[l] = 0f;
				}
				return;
			}
			if (this.started == 0)
			{
				this.mem_alloc_size = this.filt_len - 1 + this.buffer_size;
				this.mem = new float[this.nb_channels * this.mem_alloc_size];
				for (int m = 0; m < this.nb_channels * this.mem_alloc_size; m++)
				{
					this.mem[m] = 0f;
				}
				return;
			}
			if (this.filt_len > num)
			{
				int num2 = this.mem_alloc_size;
				if (this.filt_len - 1 + this.buffer_size > this.mem_alloc_size)
				{
					this.mem_alloc_size = this.filt_len - 1 + this.buffer_size;
					this.mem = new float[this.nb_channels * this.mem_alloc_size];
				}
				for (int n = this.nb_channels - 1; n >= 0; n--)
				{
					int num3 = num + 2 * this.magic_samples[n];
					for (int num4 = num - 2 + this.magic_samples[n]; num4 >= 0; num4--)
					{
						this.mem[n * this.mem_alloc_size + num4 + this.magic_samples[n]] = this.mem[n * num2 + num4];
					}
					for (int num4 = 0; num4 < this.magic_samples[n]; num4++)
					{
						this.mem[n * this.mem_alloc_size + num4] = 0f;
					}
					this.magic_samples[n] = 0;
					if (this.filt_len > num3)
					{
						int num4;
						for (num4 = 0; num4 < num3 - 1; num4++)
						{
							this.mem[n * this.mem_alloc_size + (this.filt_len - 2 - num4)] = this.mem[n * this.mem_alloc_size + (num3 - 2 - num4)];
						}
						while (num4 < this.filt_len - 1)
						{
							this.mem[n * this.mem_alloc_size + (this.filt_len - 2 - num4)] = 0f;
							num4++;
						}
						this.last_sample[n] += (this.filt_len - num3) / 2;
					}
					else
					{
						this.magic_samples[n] = (num3 - this.filt_len) / 2;
						for (int num4 = 0; num4 < this.filt_len - 1 + this.magic_samples[n]; num4++)
						{
							this.mem[n * this.mem_alloc_size + num4] = this.mem[n * this.mem_alloc_size + num4 + this.magic_samples[n]];
						}
					}
				}
				return;
			}
			if (this.filt_len < num)
			{
				for (int num5 = 0; num5 < this.nb_channels; num5++)
				{
					int num6 = this.magic_samples[num5];
					this.magic_samples[num5] = (num - this.filt_len) / 2;
					for (int num7 = 0; num7 < this.filt_len - 1 + this.magic_samples[num5] + num6; num7++)
					{
						this.mem[num5 * this.mem_alloc_size + num7] = this.mem[num5 * this.mem_alloc_size + num7 + this.magic_samples[num5]];
					}
					this.magic_samples[num5] += num6;
				}
			}
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00027120 File Offset: 0x00025320
		private void speex_resampler_process_native(int channel_index, ref int in_len, Span<float> output, int output_ptr, ref int out_len)
		{
			int num = this.filt_len;
			int num2 = channel_index * this.mem_alloc_size;
			this.started = 1;
			int num3 = this.resampler_ptr(channel_index, this.mem, num2, ref in_len, output, output_ptr, ref out_len);
			if (this.last_sample[channel_index] < in_len)
			{
				in_len = this.last_sample[channel_index];
			}
			out_len = num3;
			this.last_sample[channel_index] -= in_len;
			int num4 = in_len;
			for (int i = num2; i < num - 1 + num2; i++)
			{
				this.mem[i] = this.mem[i + num4];
			}
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x000271BC File Offset: 0x000253BC
		private int speex_resampler_magic(int channel_index, Span<float> output, ref int output_ptr, int out_len)
		{
			int num = this.magic_samples[channel_index];
			int num2 = channel_index * this.mem_alloc_size;
			int num3 = this.filt_len;
			this.speex_resampler_process_native(channel_index, ref num, output, output_ptr, ref out_len);
			this.magic_samples[channel_index] -= num;
			if (this.magic_samples[channel_index] != 0)
			{
				for (int i = num2; i < this.magic_samples[channel_index] + num2; i++)
				{
					this.mem[num3 - 1 + i] = this.mem[num3 - 1 + i + num];
				}
			}
			output_ptr += out_len * this.out_stride;
			return out_len;
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0002724A File Offset: 0x0002544A
		[Obsolete("Use ResamplerFactory.CreateResampler instead")]
		public SpeexResampler(int nb_channels, int in_rate, int out_rate, int quality) : this(nb_channels, in_rate, out_rate, in_rate, out_rate, quality)
		{
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0002725C File Offset: 0x0002545C
		[Obsolete("Use ResamplerFactory.CreateResampler instead")]
		public SpeexResampler(int nb_channels, int ratio_num, int ratio_den, int in_rate, int out_rate, int quality)
		{
			if (quality > 10 || quality < 0)
			{
				throw new ArgumentException("Quality must be between 0 and 10");
			}
			this.initialised = 0;
			this.started = 0;
			this.in_rate = 0;
			this.out_rate = 0;
			this.num_rate = 0;
			this.den_rate = 0;
			this.quality = -1;
			this.sinc_table_length = 0;
			this.mem_alloc_size = 0;
			this.filt_len = 0;
			this.mem = null;
			this.resampler_ptr = null;
			this.cutoff = 1f;
			this.nb_channels = nb_channels;
			this.in_stride = 1;
			this.out_stride = 1;
			this.buffer_size = 160;
			this.last_sample = new int[nb_channels];
			this.magic_samples = new int[nb_channels];
			this.samp_frac_num = new int[nb_channels];
			for (int i = 0; i < nb_channels; i++)
			{
				this.last_sample[i] = 0;
				this.magic_samples[i] = 0;
				this.samp_frac_num[i] = 0;
			}
			this.Quality = quality;
			this.SetRateFraction(ratio_num, ratio_den, in_rate, out_rate);
			this.update_filter();
			this.initialised = 1;
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00027370 File Offset: 0x00025570
		public void Process(int channel_index, Span<float> input, ref int in_len, Span<float> output, ref int out_len)
		{
			this.Process(channel_index, input, 0, ref in_len, output, 0, ref out_len);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00027384 File Offset: 0x00025584
		private unsafe void Process(int channel_index, Span<float> input, int input_ptr, ref int in_len, Span<float> output, int output_ptr, ref int out_len)
		{
			int num = in_len;
			int num2 = out_len;
			int num3 = channel_index * this.mem_alloc_size;
			int num4 = this.filt_len - 1;
			int num5 = this.mem_alloc_size - num4;
			int num6 = this.in_stride;
			if (this.magic_samples[channel_index] != 0)
			{
				num2 -= this.speex_resampler_magic(channel_index, output, ref output_ptr, num2);
			}
			if (this.magic_samples[channel_index] == 0)
			{
				while (num != 0 && num2 != 0)
				{
					int num7 = (num > num5) ? num5 : num;
					int num8 = num2;
					if (input != null)
					{
						for (int i = 0; i < num7; i++)
						{
							this.mem[num3 + i + num4] = *input[input_ptr + i * num6];
						}
					}
					else
					{
						for (int i = 0; i < num7; i++)
						{
							this.mem[num3 + i + num4] = 0f;
						}
					}
					this.speex_resampler_process_native(channel_index, ref num7, output, output_ptr, ref num8);
					num -= num7;
					num2 -= num8;
					output_ptr += num8 * this.out_stride;
					if (input != null)
					{
						input_ptr += num7 * num6;
					}
				}
			}
			in_len -= num;
			out_len -= num2;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x000274A7 File Offset: 0x000256A7
		public void Process(int channel_index, Span<short> input, ref int in_len, Span<short> output, ref int out_len)
		{
			this.Process(channel_index, input, 0, ref in_len, output, 0, ref out_len);
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x000274B8 File Offset: 0x000256B8
		private unsafe void Process(int channel_index, Span<short> input, int input_ptr, ref int in_len, Span<short> output, int output_ptr, ref int out_len)
		{
			int num = this.in_stride;
			int num2 = this.out_stride;
			int num3 = in_len;
			int num4 = out_len;
			int num5 = channel_index * this.mem_alloc_size;
			int num6 = this.mem_alloc_size - (this.filt_len - 1);
			int num7 = (num4 < 8192) ? num4 : 8192;
			float[] array = new float[num7];
			this.out_stride = 1;
			while (num3 != 0 && num4 != 0)
			{
				int output_ptr2 = 0;
				int num8 = (num3 > num6) ? num6 : num3;
				int num9 = (num4 > num7) ? num7 : num4;
				int num10 = 0;
				if (this.magic_samples[channel_index] != 0)
				{
					num10 = this.speex_resampler_magic(channel_index, array, ref output_ptr2, num9);
					num9 -= num10;
					num4 -= num10;
				}
				if (this.magic_samples[channel_index] == 0)
				{
					if (input != null)
					{
						for (int i = 0; i < num8; i++)
						{
							this.mem[num5 + i + this.filt_len - 1] = (float)(*input[input_ptr + i * num]);
						}
					}
					else
					{
						for (int i = 0; i < num8; i++)
						{
							this.mem[num5 + i + this.filt_len - 1] = 0f;
						}
					}
					this.speex_resampler_process_native(channel_index, ref num8, array, output_ptr2, ref num9);
				}
				else
				{
					num8 = 0;
					num9 = 0;
				}
				for (int i = 0; i < num9 + num10; i++)
				{
					*output[output_ptr + i * num2] = SpeexResampler.FLOAT2INT(array[i]);
				}
				num3 -= num8;
				num4 -= num9;
				output_ptr += (num9 + num10) * num2;
				if (input != null)
				{
					input_ptr += num8 * num;
				}
			}
			this.out_stride = num2;
			in_len -= num3;
			out_len -= num4;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00027670 File Offset: 0x00025870
		public void ProcessInterleaved(Span<float> input, ref int in_len, Span<float> output, ref int out_len)
		{
			int num = out_len;
			int num2 = in_len;
			int num3 = this.in_stride;
			int num4 = this.out_stride;
			this.in_stride = (this.out_stride = this.nb_channels);
			for (int i = 0; i < this.nb_channels; i++)
			{
				out_len = num;
				in_len = num2;
				if (input != null)
				{
					this.Process(i, input, i, ref in_len, output, i, ref out_len);
				}
				else
				{
					this.Process(i, null, 0, ref in_len, output, i, ref out_len);
				}
			}
			this.in_stride = num3;
			this.out_stride = num4;
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00027704 File Offset: 0x00025904
		public void ProcessInterleaved(Span<short> input, ref int in_len, Span<short> output, ref int out_len)
		{
			int num = out_len;
			int num2 = in_len;
			int num3 = this.in_stride;
			int num4 = this.out_stride;
			this.in_stride = (this.out_stride = this.nb_channels);
			for (int i = 0; i < this.nb_channels; i++)
			{
				out_len = num;
				in_len = num2;
				if (input != null)
				{
					this.Process(i, input, i, ref in_len, output, i, ref out_len);
				}
				else
				{
					this.Process(i, null, 0, ref in_len, output, i, ref out_len);
				}
			}
			this.in_stride = num3;
			this.out_stride = num4;
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00027798 File Offset: 0x00025998
		public void SkipZeroes()
		{
			for (int i = 0; i < this.nb_channels; i++)
			{
				this.last_sample[i] = this.filt_len / 2;
			}
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x000277C8 File Offset: 0x000259C8
		public void ResetMem()
		{
			for (int i = 0; i < this.nb_channels; i++)
			{
				this.last_sample[i] = 0;
				this.magic_samples[i] = 0;
				this.samp_frac_num[i] = 0;
			}
			for (int i = 0; i < this.nb_channels * (this.filt_len - 1); i++)
			{
				this.mem[i] = 0f;
			}
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00027828 File Offset: 0x00025A28
		public void Dispose()
		{
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0002782A File Offset: 0x00025A2A
		public void SetRates(int in_rate, int out_rate)
		{
			this.SetRateFraction(in_rate, out_rate, in_rate, out_rate);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00027836 File Offset: 0x00025A36
		public void GetRates(out int in_rate, out int out_rate)
		{
			in_rate = this.in_rate;
			out_rate = this.out_rate;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00027848 File Offset: 0x00025A48
		public void SetRateFraction(int ratio_num, int ratio_den, int in_rate, int out_rate)
		{
			if (this.in_rate == in_rate && this.out_rate == out_rate && this.num_rate == ratio_num && this.den_rate == ratio_den)
			{
				return;
			}
			int num = this.den_rate;
			this.in_rate = in_rate;
			this.out_rate = out_rate;
			this.num_rate = ratio_num;
			this.den_rate = ratio_den;
			for (int i = 2; i <= Inlines.IMIN(this.num_rate, this.den_rate); i++)
			{
				while (this.num_rate % i == 0 && this.den_rate % i == 0)
				{
					this.num_rate /= i;
					this.den_rate /= i;
				}
			}
			if (num > 0)
			{
				for (int j = 0; j < this.nb_channels; j++)
				{
					this.samp_frac_num[j] = this.samp_frac_num[j] * this.den_rate / num;
					if (this.samp_frac_num[j] >= this.den_rate)
					{
						this.samp_frac_num[j] = this.den_rate - 1;
					}
				}
			}
			if (this.initialised != 0)
			{
				this.update_filter();
			}
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00027947 File Offset: 0x00025B47
		public void GetRateFraction(out int ratio_num, out int ratio_den)
		{
			ratio_num = this.num_rate;
			ratio_den = this.den_rate;
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x00027959 File Offset: 0x00025B59
		// (set) Token: 0x06000453 RID: 1107 RVA: 0x00027961 File Offset: 0x00025B61
		public int Quality
		{
			get
			{
				return this.quality;
			}
			set
			{
				if (value > 10 || value < 0)
				{
					throw new ArgumentException("Quality must be between 0 and 10");
				}
				if (this.quality == value)
				{
					return;
				}
				this.quality = value;
				if (this.initialised != 0)
				{
					this.update_filter();
				}
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x00027996 File Offset: 0x00025B96
		// (set) Token: 0x06000455 RID: 1109 RVA: 0x0002799E File Offset: 0x00025B9E
		public int InputStride
		{
			get
			{
				return this.in_stride;
			}
			set
			{
				this.in_stride = value;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x000279A7 File Offset: 0x00025BA7
		// (set) Token: 0x06000457 RID: 1111 RVA: 0x000279AF File Offset: 0x00025BAF
		public int OutputStride
		{
			get
			{
				return this.out_stride;
			}
			set
			{
				this.out_stride = value;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x000279B8 File Offset: 0x00025BB8
		public int InputLatency
		{
			get
			{
				return this.filt_len / 2;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x000279C2 File Offset: 0x00025BC2
		public int OutputLatencySamples
		{
			get
			{
				return (this.filt_len / 2 * this.den_rate + (this.num_rate >> 1)) / this.num_rate;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x000279E3 File Offset: 0x00025BE3
		public TimeSpan OutputLatency
		{
			get
			{
				return TimeSpan.FromTicks((long)this.OutputLatencySamples * 10000000L / (long)this.out_rate);
			}
		}

		// Token: 0x04000411 RID: 1041
		private const int FIXED_STACK_ALLOC = 8192;

		// Token: 0x04000412 RID: 1042
		private int in_rate;

		// Token: 0x04000413 RID: 1043
		private int out_rate;

		// Token: 0x04000414 RID: 1044
		private int num_rate;

		// Token: 0x04000415 RID: 1045
		private int den_rate;

		// Token: 0x04000416 RID: 1046
		private int quality;

		// Token: 0x04000417 RID: 1047
		private int nb_channels;

		// Token: 0x04000418 RID: 1048
		private int filt_len;

		// Token: 0x04000419 RID: 1049
		private int mem_alloc_size;

		// Token: 0x0400041A RID: 1050
		private int buffer_size;

		// Token: 0x0400041B RID: 1051
		private int int_advance;

		// Token: 0x0400041C RID: 1052
		private int frac_advance;

		// Token: 0x0400041D RID: 1053
		private float cutoff;

		// Token: 0x0400041E RID: 1054
		private int oversample;

		// Token: 0x0400041F RID: 1055
		private int initialised;

		// Token: 0x04000420 RID: 1056
		private int started;

		// Token: 0x04000421 RID: 1057
		private int[] last_sample;

		// Token: 0x04000422 RID: 1058
		private int[] samp_frac_num;

		// Token: 0x04000423 RID: 1059
		private int[] magic_samples;

		// Token: 0x04000424 RID: 1060
		private float[] mem;

		// Token: 0x04000425 RID: 1061
		private float[] sinc_table;

		// Token: 0x04000426 RID: 1062
		private int sinc_table_length;

		// Token: 0x04000427 RID: 1063
		private SpeexResampler.resampler_basic_func resampler_ptr;

		// Token: 0x04000428 RID: 1064
		private int in_stride;

		// Token: 0x04000429 RID: 1065
		private int out_stride;

		// Token: 0x020000A8 RID: 168
		private class FuncDef
		{
			// Token: 0x0600055F RID: 1375 RVA: 0x0003654C File Offset: 0x0003474C
			public FuncDef(double[] t, int os)
			{
				this.table = t;
				this.oversample = os;
			}

			// Token: 0x040005FE RID: 1534
			public double[] table;

			// Token: 0x040005FF RID: 1535
			public int oversample;

			// Token: 0x04000600 RID: 1536
			public static readonly double[] kaiser12_table = new double[]
			{
				0.99859849,
				1.0,
				0.99859849,
				0.99440475,
				0.98745105,
				0.97779076,
				0.9654977,
				0.95066529,
				0.93340547,
				0.91384741,
				0.89213598,
				0.86843014,
				0.84290116,
				0.81573067,
				0.78710866,
				0.75723148,
				0.7262997,
				0.69451601,
				0.66208321,
				0.62920216,
				0.59606986,
				0.56287762,
				0.52980938,
				0.49704014,
				0.46473455,
				0.43304576,
				0.40211431,
				0.37206735,
				0.343018,
				0.3150649,
				0.28829195,
				0.26276832,
				0.23854851,
				0.21567274,
				0.19416736,
				0.17404546,
				0.15530766,
				0.13794294,
				0.12192957,
				0.10723616,
				0.09382272,
				0.08164178,
				0.0706395,
				0.06075685,
				0.05193064,
				0.04409466,
				0.03718069,
				0.03111947,
				0.02584161,
				0.02127838,
				0.0173625,
				0.01402878,
				0.01121463,
				0.00886058,
				0.00691064,
				0.00531256,
				0.00401805,
				0.00298291,
				0.00216702,
				0.00153438,
				0.00105297,
				0.00069463,
				0.00043489,
				0.00025272,
				0.00013031,
				5.27734E-05,
				1E-05,
				0.0
			};

			// Token: 0x04000601 RID: 1537
			public static readonly double[] kaiser10_table = new double[]
			{
				0.99537781,
				1.0,
				0.99537781,
				0.98162644,
				0.95908712,
				0.92831446,
				0.89005583,
				0.84522401,
				0.79486424,
				0.74011713,
				0.68217934,
				0.62226347,
				0.56155915,
				0.5011968,
				0.44221549,
				0.38553619,
				0.33194107,
				0.28205962,
				0.23636152,
				0.19515633,
				0.15859932,
				0.1267028,
				0.09935205,
				0.07632451,
				0.05731132,
				0.0419398,
				0.02979584,
				0.0204451,
				0.01345224,
				0.00839739,
				0.00488951,
				0.00257636,
				0.00115101,
				0.00035515,
				0.0,
				0.0
			};

			// Token: 0x04000602 RID: 1538
			public static readonly double[] kaiser8_table = new double[]
			{
				0.99635258,
				1.0,
				0.99635258,
				0.98548012,
				0.96759014,
				0.943022,
				0.91223751,
				0.87580811,
				0.83439927,
				0.78875245,
				0.73966538,
				0.68797126,
				0.6345175,
				0.58014482,
				0.52566725,
				0.47185369,
				0.4194115,
				0.36897272,
				0.32108304,
				0.27619388,
				0.23465776,
				0.1967267,
				0.1625538,
				0.13219758,
				0.10562887,
				0.08273982,
				0.06335451,
				0.04724088,
				0.03412321,
				0.0236949,
				0.01563093,
				0.00959968,
				0.00527363,
				0.00233883,
				0.0005,
				0.0
			};

			// Token: 0x04000603 RID: 1539
			public static readonly double[] kaiser6_table = new double[]
			{
				0.99733006,
				1.0,
				0.99733006,
				0.98935595,
				0.97618418,
				0.95799003,
				0.93501423,
				0.90755855,
				0.87598009,
				0.84068475,
				0.80211977,
				0.76076565,
				0.71712752,
				0.67172623,
				0.62508937,
				0.57774224,
				0.53019925,
				0.48295561,
				0.43647969,
				0.39120616,
				0.34752997,
				0.30580127,
				0.26632152,
				0.22934058,
				0.19505503,
				0.16360756,
				0.13508755,
				0.10953262,
				0.0869312,
				0.067226,
				0.0503182,
				0.03607231,
				0.02432151,
				0.01487334,
				0.00752,
				0.0
			};
		}

		// Token: 0x020000A9 RID: 169
		private class QualityMapping
		{
			// Token: 0x06000561 RID: 1377 RVA: 0x000365CD File Offset: 0x000347CD
			private QualityMapping(int bl, int os, float dsb, float usb, SpeexResampler.FuncDef wf)
			{
				this.base_length = bl;
				this.oversample = os;
				this.downsample_bandwidth = dsb;
				this.upsample_bandwidth = usb;
				this.window_func = wf;
			}

			// Token: 0x04000604 RID: 1540
			public int base_length;

			// Token: 0x04000605 RID: 1541
			public int oversample;

			// Token: 0x04000606 RID: 1542
			public float downsample_bandwidth;

			// Token: 0x04000607 RID: 1543
			public float upsample_bandwidth;

			// Token: 0x04000608 RID: 1544
			public SpeexResampler.FuncDef window_func;

			// Token: 0x04000609 RID: 1545
			public static readonly SpeexResampler.QualityMapping[] quality_map = new SpeexResampler.QualityMapping[]
			{
				new SpeexResampler.QualityMapping(8, 4, 0.83f, 0.86f, new SpeexResampler.FuncDef(SpeexResampler.FuncDef.kaiser6_table, 32)),
				new SpeexResampler.QualityMapping(16, 4, 0.85f, 0.88f, new SpeexResampler.FuncDef(SpeexResampler.FuncDef.kaiser6_table, 32)),
				new SpeexResampler.QualityMapping(32, 4, 0.882f, 0.91f, new SpeexResampler.FuncDef(SpeexResampler.FuncDef.kaiser6_table, 32)),
				new SpeexResampler.QualityMapping(48, 8, 0.895f, 0.917f, new SpeexResampler.FuncDef(SpeexResampler.FuncDef.kaiser8_table, 32)),
				new SpeexResampler.QualityMapping(64, 8, 0.921f, 0.94f, new SpeexResampler.FuncDef(SpeexResampler.FuncDef.kaiser8_table, 32)),
				new SpeexResampler.QualityMapping(80, 16, 0.922f, 0.94f, new SpeexResampler.FuncDef(SpeexResampler.FuncDef.kaiser10_table, 32)),
				new SpeexResampler.QualityMapping(96, 16, 0.94f, 0.945f, new SpeexResampler.FuncDef(SpeexResampler.FuncDef.kaiser10_table, 32)),
				new SpeexResampler.QualityMapping(128, 16, 0.95f, 0.95f, new SpeexResampler.FuncDef(SpeexResampler.FuncDef.kaiser10_table, 32)),
				new SpeexResampler.QualityMapping(160, 16, 0.96f, 0.96f, new SpeexResampler.FuncDef(SpeexResampler.FuncDef.kaiser10_table, 32)),
				new SpeexResampler.QualityMapping(192, 32, 0.968f, 0.968f, new SpeexResampler.FuncDef(SpeexResampler.FuncDef.kaiser12_table, 64)),
				new SpeexResampler.QualityMapping(256, 32, 0.975f, 0.975f, new SpeexResampler.FuncDef(SpeexResampler.FuncDef.kaiser12_table, 64))
			};
		}

		// Token: 0x020000AA RID: 170
		// (Invoke) Token: 0x06000564 RID: 1380
		private delegate int resampler_basic_func(int channel_index, Span<float> input, int input_ptr, ref int in_len, Span<float> output, int output_ptr, ref int out_len);
	}
}
