using System;
using Concentus.Common.CPlusPlus;

namespace Concentus.Common
{
	// Token: 0x02000079 RID: 121
	internal class EntropyCoder
	{
		// Token: 0x0600031E RID: 798 RVA: 0x00023EBE File Offset: 0x000220BE
		internal EntropyCoder()
		{
			this.Reset();
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00023ECC File Offset: 0x000220CC
		internal void Reset()
		{
			this.storage = 0U;
			this.end_offs = 0U;
			this.end_window = 0U;
			this.nend_bits = 0;
			this.offs = 0U;
			this.rng = 0U;
			this.val = 0U;
			this.ext = 0U;
			this.rem = 0;
			this.error = 0;
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00023F20 File Offset: 0x00022120
		internal void Assign(EntropyCoder other)
		{
			this.storage = other.storage;
			this.end_offs = other.end_offs;
			this.end_window = other.end_window;
			this.nend_bits = other.nend_bits;
			this.nbits_total = other.nbits_total;
			this.offs = other.offs;
			this.rng = other.rng;
			this.val = other.val;
			this.ext = other.ext;
			this.rem = other.rem;
			this.error = other.error;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00023FB4 File Offset: 0x000221B4
		internal unsafe int read_byte(ReadOnlySpan<byte> buf)
		{
			if (this.offs >= this.storage)
			{
				return 0;
			}
			uint num = this.offs;
			this.offs = num + 1U;
			return (int)(*buf[(int)num]);
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00023FEC File Offset: 0x000221EC
		internal unsafe int read_byte_from_end(ReadOnlySpan<byte> buf)
		{
			if (this.end_offs >= this.storage)
			{
				return 0;
			}
			int num = (int)this.storage;
			uint num2 = this.end_offs + 1U;
			this.end_offs = num2;
			return (int)(*buf[num - (int)num2]);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0002402C File Offset: 0x0002222C
		internal unsafe int write_byte(Span<byte> buf, uint _value)
		{
			if (this.offs + this.end_offs >= this.storage)
			{
				return -1;
			}
			uint num = this.offs;
			this.offs = num + 1U;
			*buf[(int)num] = (byte)_value;
			return 0;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0002406C File Offset: 0x0002226C
		internal unsafe int write_byte_at_end(Span<byte> buf, uint _value)
		{
			if (this.offs + this.end_offs >= this.storage)
			{
				return -1;
			}
			int num = (int)this.storage;
			uint num2 = this.end_offs + 1U;
			this.end_offs = num2;
			*buf[num - (int)num2] = (byte)_value;
			return 0;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x000240B4 File Offset: 0x000222B4
		internal void dec_normalize(ReadOnlySpan<byte> buf)
		{
			while (this.rng <= 8388608U)
			{
				this.nbits_total += 8;
				this.rng <<= 8;
				int num = this.rem;
				this.rem = this.read_byte(buf);
				num = (num << 8 | this.rem) >> 1;
				this.val = ((uint)((ulong)((ulong)this.val << 8) + (ulong)(255L & (long)(~(long)num))) & 2147483647U);
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00024130 File Offset: 0x00022330
		internal void dec_init(ReadOnlySpan<byte> buf, uint _storage)
		{
			this.storage = _storage;
			this.end_offs = 0U;
			this.end_window = 0U;
			this.nend_bits = 0;
			this.nbits_total = 9;
			this.offs = 0U;
			this.rng = 128U;
			this.rem = this.read_byte(buf);
			this.val = this.rng - 1U - (uint)(this.rem >> 1);
			this.error = 0;
			this.dec_normalize(buf);
		}

		// Token: 0x06000327 RID: 807 RVA: 0x000241A8 File Offset: 0x000223A8
		internal uint decode(uint _ft)
		{
			this.ext = this.rng / _ft;
			uint num = this.val / this.ext;
			return _ft - Inlines.EC_MINI(num + 1U, _ft);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x000241DC File Offset: 0x000223DC
		internal uint decode_bin(uint _bits)
		{
			this.ext = this.rng >> (int)_bits;
			uint num = this.val / this.ext;
			return (1U << (int)_bits) - Inlines.EC_MINI(num + 1U, 1U << (int)_bits);
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00024220 File Offset: 0x00022420
		internal void dec_update(ReadOnlySpan<byte> buf, uint _fl, uint _fh, uint _ft)
		{
			uint num = this.ext * (_ft - _fh);
			this.val -= num;
			this.rng = ((_fl > 0U) ? (this.ext * (_fh - _fl)) : (this.rng - num));
			this.dec_normalize(buf);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0002426C File Offset: 0x0002246C
		internal int dec_bit_logp(ReadOnlySpan<byte> buf, uint _logp)
		{
			uint num = this.rng;
			uint num2 = this.val;
			uint num3 = num >> (int)_logp;
			int num4 = (num2 < num3) ? 1 : 0;
			if (num4 == 0)
			{
				this.val = num2 - num3;
			}
			this.rng = ((num4 != 0) ? num3 : (num - num3));
			this.dec_normalize(buf);
			return num4;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x000242B8 File Offset: 0x000224B8
		internal int dec_icdf(ReadOnlySpan<byte> buf, byte[] _icdf, uint _ftb)
		{
			uint num = this.rng;
			uint num2 = this.val;
			uint num3 = num >> (int)_ftb;
			int num4 = -1;
			uint num5;
			do
			{
				num5 = num;
				num = num3 * (uint)_icdf[++num4];
			}
			while (num2 < num);
			this.val = num2 - num;
			this.rng = num5 - num;
			this.dec_normalize(buf);
			return num4;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0002430C File Offset: 0x0002250C
		internal int dec_icdf(ReadOnlySpan<byte> buf, byte[] _icdf, int _icdf_offset, uint _ftb)
		{
			uint num = this.rng;
			uint num2 = this.val;
			uint num3 = num >> (int)_ftb;
			int num4 = _icdf_offset - 1;
			uint num5;
			do
			{
				num5 = num;
				num = num3 * (uint)_icdf[++num4];
			}
			while (num2 < num);
			this.val = num2 - num;
			this.rng = num5 - num;
			this.dec_normalize(buf);
			return num4 - _icdf_offset;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00024364 File Offset: 0x00022564
		internal uint dec_uint(ReadOnlySpan<byte> buf, uint _ft)
		{
			_ft -= 1U;
			int num = Inlines.EC_ILOG(_ft);
			uint num2;
			if (num <= 8)
			{
				_ft += 1U;
				num2 = this.decode(_ft);
				this.dec_update(buf, num2, num2 + 1U, _ft);
				return num2;
			}
			num -= 8;
			uint ft = (_ft >> num) + 1U;
			num2 = this.decode(ft);
			this.dec_update(buf, num2, num2 + 1U, ft);
			uint num3 = num2 << num | this.dec_bits(buf, (uint)num);
			if (num3 <= _ft)
			{
				return num3;
			}
			this.error = 1;
			return _ft;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x000243DC File Offset: 0x000225DC
		internal uint dec_bits(ReadOnlySpan<byte> buf, uint _bits)
		{
			uint num = this.end_window;
			int num2 = this.nend_bits;
			if (num2 < (int)_bits)
			{
				do
				{
					num |= (uint)((uint)this.read_byte_from_end(buf) << num2);
					num2 += 8;
				}
				while (num2 <= 24);
			}
			uint result = num & (1U << (int)_bits) - 1U;
			num >>= (int)_bits;
			num2 -= (int)_bits;
			this.end_window = num;
			this.nend_bits = num2;
			this.nbits_total += (int)_bits;
			return result;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00024444 File Offset: 0x00022644
		internal void enc_carry_out(Span<byte> buf, int _c)
		{
			if ((long)_c != 255L)
			{
				int num = _c >> 8;
				if (this.rem >= 0)
				{
					this.error |= this.write_byte(buf, (uint)(this.rem + num));
				}
				if (this.ext > 0U)
				{
					uint value = (uint)(255 + num & 255);
					uint num2;
					do
					{
						this.error |= this.write_byte(buf, value);
						num2 = this.ext - 1U;
						this.ext = num2;
					}
					while (num2 > 0U);
				}
				this.rem = (_c & 255);
				return;
			}
			this.ext += 1U;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x000244E0 File Offset: 0x000226E0
		internal void enc_normalize(Span<byte> buf)
		{
			while (this.rng <= 8388608U)
			{
				this.enc_carry_out(buf, (int)(this.val >> 23));
				this.val = (this.val << 8 & 2147483647U);
				this.rng <<= 8;
				this.nbits_total += 8;
			}
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0002453C File Offset: 0x0002273C
		internal void enc_init(uint _size)
		{
			this.end_offs = 0U;
			this.end_window = 0U;
			this.nend_bits = 0;
			this.nbits_total = 33;
			this.offs = 0U;
			this.rng = 2147483648U;
			this.rem = -1;
			this.val = 0U;
			this.ext = 0U;
			this.storage = _size;
			this.error = 0;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0002459C File Offset: 0x0002279C
		internal void encode(Span<byte> buf, uint _fl, uint _fh, uint _ft)
		{
			uint num = this.rng / _ft;
			if (_fl > 0U)
			{
				this.val += this.rng - num * (_ft - _fl);
				this.rng = num * (_fh - _fl);
			}
			else
			{
				this.rng -= num * (_ft - _fh);
			}
			this.enc_normalize(buf);
		}

		// Token: 0x06000333 RID: 819 RVA: 0x000245F8 File Offset: 0x000227F8
		internal void encode_bin(Span<byte> buf, uint _fl, uint _fh, uint _bits)
		{
			uint num = this.rng >> (int)_bits;
			if (_fl > 0U)
			{
				this.val += this.rng - num * ((1U << (int)_bits) - _fl);
				this.rng = num * (_fh - _fl);
			}
			else
			{
				this.rng -= num * ((1U << (int)_bits) - _fh);
			}
			this.enc_normalize(buf);
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00024664 File Offset: 0x00022864
		internal void enc_bit_logp(Span<byte> buf, int _val, uint _logp)
		{
			uint num = this.rng;
			uint num2 = this.val;
			uint num3 = num >> (int)_logp;
			num -= num3;
			if (_val != 0)
			{
				this.val = num2 + num;
			}
			this.rng = ((_val != 0) ? num3 : num);
			this.enc_normalize(buf);
		}

		// Token: 0x06000335 RID: 821 RVA: 0x000246AC File Offset: 0x000228AC
		internal void enc_icdf(Span<byte> buf, int _s, byte[] _icdf, uint _ftb)
		{
			uint num = this.rng >> (int)_ftb;
			if (_s > 0)
			{
				this.val += this.rng - num * (uint)_icdf[_s - 1];
				this.rng = num * (uint)(_icdf[_s - 1] - _icdf[_s]);
			}
			else
			{
				this.rng -= num * (uint)_icdf[_s];
			}
			this.enc_normalize(buf);
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00024714 File Offset: 0x00022914
		internal void enc_icdf(Span<byte> buf, int _s, byte[] _icdf, int icdf_ptr, uint _ftb)
		{
			uint num = this.rng >> (int)_ftb;
			if (_s > 0)
			{
				this.val += this.rng - num * (uint)_icdf[icdf_ptr + _s - 1];
				this.rng = num * (uint)(_icdf[icdf_ptr + _s - 1] - _icdf[icdf_ptr + _s]);
			}
			else
			{
				this.rng -= num * (uint)_icdf[icdf_ptr + _s];
			}
			this.enc_normalize(buf);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x00024788 File Offset: 0x00022988
		internal void enc_uint(Span<byte> buf, uint _fl, uint _ft)
		{
			_ft -= 1U;
			int num = Inlines.EC_ILOG(_ft);
			if (num > 8)
			{
				num -= 8;
				uint ft = (_ft >> num) + 1U;
				uint num2 = _fl >> num;
				this.encode(buf, num2, num2 + 1U, ft);
				this.enc_bits(buf, _fl & (1U << num) - 1U, (uint)num);
				return;
			}
			this.encode(buf, _fl, _fl + 1U, _ft + 1U);
		}

		// Token: 0x06000338 RID: 824 RVA: 0x000247E8 File Offset: 0x000229E8
		internal void enc_bits(Span<byte> buf, uint _fl, uint _bits)
		{
			uint num = this.end_window;
			int num2 = this.nend_bits;
			if ((long)num2 + (long)((ulong)_bits) > 32L)
			{
				do
				{
					this.error |= this.write_byte_at_end(buf, num & 255U);
					num >>= 8;
					num2 -= 8;
				}
				while (num2 >= 8);
			}
			num |= _fl << num2;
			num2 += (int)_bits;
			this.end_window = num;
			this.nend_bits = num2;
			this.nbits_total += (int)_bits;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00024860 File Offset: 0x00022A60
		internal unsafe void enc_patch_initial_bits(Span<byte> buf, uint _val, uint _nbits)
		{
			int num = (int)(8U - _nbits);
			uint num2 = (1U << (int)_nbits) - 1U << num;
			if (this.offs > 0U)
			{
				*buf[0] = (byte)(((uint)(*buf[0]) & ~num2) | _val << num);
				return;
			}
			if (this.rem >= 0)
			{
				this.rem = ((this.rem & (int)(~(int)num2)) | (int)_val) << num;
				return;
			}
			if (this.rng <= 2147483648U >> (int)_nbits)
			{
				this.val = ((this.val & ~(num2 << 23)) | _val << (int)(23L + (long)num));
				return;
			}
			this.error = -1;
		}

		// Token: 0x0600033A RID: 826 RVA: 0x000248FF File Offset: 0x00022AFF
		internal void enc_shrink(Span<byte> buf, uint _size)
		{
			Arrays.MemMoveByte(buf, (int)(_size - this.end_offs), (int)(this.storage - this.end_offs), (int)this.end_offs);
			this.storage = _size;
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00024929 File Offset: 0x00022B29
		internal uint range_bytes()
		{
			return this.offs;
		}

		// Token: 0x0600033C RID: 828 RVA: 0x00024931 File Offset: 0x00022B31
		internal int get_error()
		{
			return this.error;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00024939 File Offset: 0x00022B39
		internal int tell()
		{
			return this.nbits_total - Inlines.EC_ILOG(this.rng);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00024950 File Offset: 0x00022B50
		internal uint tell_frac()
		{
			uint num = (uint)((uint)this.nbits_total << 3);
			int num2 = Inlines.EC_ILOG(this.rng);
			int num3 = (int)(this.rng >> num2 - 16);
			uint num4 = (uint)((num3 >> 12) - 8);
			num4 += (((long)num3 > (long)((ulong)EntropyCoder.correction[(int)num4])) ? 1U : 0U);
			num2 = (int)((long)((long)num2 << 3) + (long)((ulong)num4));
			return num - (uint)num2;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x000249A4 File Offset: 0x00022BA4
		internal void enc_done(Span<byte> buf)
		{
			int i = 32 - Inlines.EC_ILOG(this.rng);
			uint num = 2147483647U >> i;
			uint num2 = this.val + num & ~num;
			if ((num2 | num) >= this.val + this.rng)
			{
				i++;
				num >>= 1;
				num2 = (this.val + num & ~num);
			}
			while (i > 0)
			{
				this.enc_carry_out(buf, (int)(num2 >> 23));
				num2 = (num2 << 8 & 2147483647U);
				i -= 8;
			}
			if (this.rem >= 0 || this.ext > 0U)
			{
				this.enc_carry_out(buf, 0);
			}
			uint num3 = this.end_window;
			int j;
			for (j = this.nend_bits; j >= 8; j -= 8)
			{
				this.error |= this.write_byte_at_end(buf, num3 & 255U);
				num3 >>= 8;
			}
			if (this.error == 0)
			{
				Arrays.MemSetWithOffset<byte>(buf, 0, (int)this.offs, (int)(this.storage - this.offs - this.end_offs));
				if (j > 0)
				{
					if (this.end_offs >= this.storage)
					{
						this.error = -1;
						return;
					}
					i = -i;
					if (this.offs + this.end_offs >= this.storage && i < j)
					{
						num3 &= (1U << i) - 1U;
						this.error = -1;
					}
					ref byte ptr = buf[(int)(this.storage - this.end_offs - 1U)];
					ptr |= (byte)num3;
				}
			}
		}

		// Token: 0x040003F8 RID: 1016
		private const int EC_WINDOW_SIZE = 32;

		// Token: 0x040003F9 RID: 1017
		private const int EC_UINT_BITS = 8;

		// Token: 0x040003FA RID: 1018
		internal const int BITRES = 3;

		// Token: 0x040003FB RID: 1019
		private const int EC_SYM_BITS = 8;

		// Token: 0x040003FC RID: 1020
		private const int EC_CODE_BITS = 32;

		// Token: 0x040003FD RID: 1021
		private const uint EC_SYM_MAX = 255U;

		// Token: 0x040003FE RID: 1022
		private const uint EC_CODE_SHIFT = 23U;

		// Token: 0x040003FF RID: 1023
		private const uint EC_CODE_TOP = 2147483648U;

		// Token: 0x04000400 RID: 1024
		private const uint EC_CODE_BOT = 8388608U;

		// Token: 0x04000401 RID: 1025
		private const int EC_CODE_EXTRA = 7;

		// Token: 0x04000402 RID: 1026
		internal uint storage;

		// Token: 0x04000403 RID: 1027
		internal uint end_offs;

		// Token: 0x04000404 RID: 1028
		internal uint end_window;

		// Token: 0x04000405 RID: 1029
		internal int nend_bits;

		// Token: 0x04000406 RID: 1030
		internal int nbits_total;

		// Token: 0x04000407 RID: 1031
		internal uint offs;

		// Token: 0x04000408 RID: 1032
		internal uint rng;

		// Token: 0x04000409 RID: 1033
		internal uint val;

		// Token: 0x0400040A RID: 1034
		internal uint ext;

		// Token: 0x0400040B RID: 1035
		internal int rem;

		// Token: 0x0400040C RID: 1036
		internal int error;

		// Token: 0x0400040D RID: 1037
		private static readonly uint[] correction = new uint[]
		{
			35733U,
			38967U,
			42495U,
			46340U,
			50535U,
			55109U,
			60097U,
			65535U
		};
	}
}
