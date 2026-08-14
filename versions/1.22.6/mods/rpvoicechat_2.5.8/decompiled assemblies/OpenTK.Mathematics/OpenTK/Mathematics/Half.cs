using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;

namespace OpenTK.Mathematics
{
	// Token: 0x02000003 RID: 3
	[Serializable]
	public struct Half : ISerializable, IComparable<Half>, IFormattable, IEquatable<Half>
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00003B55 File Offset: 0x00001D55
		public readonly bool IsZero
		{
			get
			{
				return this._bits == 0 || this._bits == 32768;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00003B6E File Offset: 0x00001D6E
		public readonly bool IsNaN
		{
			get
			{
				return (this._bits & 31744) == 31744 && (this._bits & 1023) > 0;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00003B94 File Offset: 0x00001D94
		public readonly bool IsPositiveInfinity
		{
			get
			{
				return this._bits == 31744;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00003BA3 File Offset: 0x00001DA3
		public readonly bool IsNegativeInfinity
		{
			get
			{
				return this._bits == 64512;
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003BB2 File Offset: 0x00001DB2
		public unsafe Half(float f)
		{
			this = default(Half);
			this._bits = this.SingleToHalf(*(int*)(&f));
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00003BCC File Offset: 0x00001DCC
		public Half(float f, bool throwOnError)
		{
			this = new Half(f);
			if (throwOnError)
			{
				if (f > Half.MaxValue)
				{
					throw new ArithmeticException("Half: Positive maximum value exceeded.");
				}
				if (f < -Half.MaxValue)
				{
					throw new ArithmeticException("Half: Negative minimum value exceeded.");
				}
				if (float.IsNaN(f))
				{
					throw new ArithmeticException("Half: Input is not a number (NaN).");
				}
				if (float.IsPositiveInfinity(f))
				{
					throw new ArithmeticException("Half: Input is positive infinity.");
				}
				if (float.IsNegativeInfinity(f))
				{
					throw new ArithmeticException("Half: Input is negative infinity.");
				}
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003C43 File Offset: 0x00001E43
		public Half(double d)
		{
			this = new Half((float)d);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003C4D File Offset: 0x00001E4D
		public Half(double d, bool throwOnError)
		{
			this = new Half((float)d, throwOnError);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003C58 File Offset: 0x00001E58
		private readonly ushort SingleToHalf(int si32)
		{
			int num = si32 >> 16 & 32768;
			int num2 = (si32 >> 23 & 255) - 112;
			int num3 = si32 & 8388607;
			if (num2 <= 0)
			{
				if (num2 < -10)
				{
					return (ushort)num;
				}
				num3 |= 8388608;
				int num4 = 14 - num2;
				int num5 = (1 << num4 - 1) - 1;
				int num6 = num3 >> num4 & 1;
				num3 = num3 + num5 + num6 >> num4;
				return (ushort)(num | num3);
			}
			else if (num2 == 143)
			{
				if (num3 == 0)
				{
					return (ushort)(num | 31744);
				}
				num3 >>= 13;
				return (ushort)(num | 31744 | num3 | ((num3 == 0) ? 1 : 0));
			}
			else
			{
				num3 = num3 + 4095 + (num3 >> 13 & 1);
				if ((num3 & 8388608) != 0)
				{
					num3 = 0;
					num2++;
				}
				if (num2 > 30)
				{
					throw new ArithmeticException("Half: Hardware floating-point overflow.");
				}
				return (ushort)(num | num2 << 10 | num3 >> 13);
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003D2C File Offset: 0x00001F2C
		public unsafe float ToSingle()
		{
			int num = this.HalfToFloat(this._bits);
			return *(float*)(&num);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003D4C File Offset: 0x00001F4C
		private readonly int HalfToFloat(ushort ui16)
		{
			int num = ui16 >> 15 & 1;
			int num2 = ui16 >> 10 & 31;
			int num3 = (int)(ui16 & 1023);
			if (num2 == 0)
			{
				if (num3 == 0)
				{
					return num << 31;
				}
				while ((num3 & 1024) == 0)
				{
					num3 <<= 1;
					num2--;
				}
				num2++;
				num3 &= -1025;
			}
			else if (num2 == 31)
			{
				if (num3 == 0)
				{
					return num << 31 | 2139095040;
				}
				return num << 31 | 2139095040 | num3 << 13;
			}
			num2 += 112;
			num3 <<= 13;
			return num << 31 | num2 << 23 | num3;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003DD2 File Offset: 0x00001FD2
		public static explicit operator Half(float f)
		{
			return new Half(f);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003DDA File Offset: 0x00001FDA
		public static explicit operator Half(double d)
		{
			return new Half(d);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00003DE2 File Offset: 0x00001FE2
		public static implicit operator float(Half h)
		{
			return h.ToSingle();
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00003DEB File Offset: 0x00001FEB
		public static implicit operator double(Half h)
		{
			return (double)h.ToSingle();
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00003DF5 File Offset: 0x00001FF5
		public static bool operator ==(Half left, Half right)
		{
			return left.Equals(right);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00003DFF File Offset: 0x00001FFF
		public static bool operator !=(Half left, Half right)
		{
			return !(left == right);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00003E0B File Offset: 0x0000200B
		public Half(SerializationInfo info, StreamingContext context)
		{
			this._bits = (ushort)info.GetValue("bits", typeof(ushort));
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00003E2D File Offset: 0x0000202D
		public readonly void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("bits", this._bits);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00003E40 File Offset: 0x00002040
		public void FromBinaryStream(BinaryReader bin)
		{
			this._bits = bin.ReadUInt16();
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00003E4E File Offset: 0x0000204E
		public readonly void ToBinaryStream(BinaryWriter bin)
		{
			bin.Write(this._bits);
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00003E5C File Offset: 0x0000205C
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<ushort>(this._bits);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00003E69 File Offset: 0x00002069
		public override readonly bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00003E7C File Offset: 0x0000207C
		public readonly bool Equals(Half other)
		{
			short num = (short)other._bits;
			short num2 = (short)this._bits;
			if (num < 0)
			{
				num = (short)(32768 - (int)num);
			}
			if (num2 < 0)
			{
				num2 = (short)(32768 - (int)num2);
			}
			return Math.Abs(num - num2) <= 1;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00003EC4 File Offset: 0x000020C4
		public readonly int CompareTo(Half other)
		{
			return this.CompareTo(other);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00003EEB File Offset: 0x000020EB
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00003EF5 File Offset: 0x000020F5
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00003EFF File Offset: 0x000020FF
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00003F0C File Offset: 0x0000210C
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return this.ToSingle().ToString(format, formatProvider);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00003F29 File Offset: 0x00002129
		public static Half Parse(string s)
		{
			return (Half)float.Parse(s);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00003F36 File Offset: 0x00002136
		public static Half Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			return (Half)float.Parse(s, style, provider);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00003F48 File Offset: 0x00002148
		public static bool TryParse(string s, out Half result)
		{
			float f;
			bool result2 = float.TryParse(s, ref f);
			result = (Half)f;
			return result2;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00003F6C File Offset: 0x0000216C
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out Half result)
		{
			float f;
			bool result2 = float.TryParse(s, style, provider, ref f);
			result = (Half)f;
			return result2;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00003F8F File Offset: 0x0000218F
		public static byte[] GetBytes(Half h)
		{
			return BitConverter.GetBytes(h._bits);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00003F9C File Offset: 0x0000219C
		public static Half FromBytes(byte[] value, int startIndex)
		{
			Half result;
			result._bits = BitConverter.ToUInt16(value, startIndex);
			return result;
		}

		// Token: 0x04000005 RID: 5
		private ushort _bits;

		// Token: 0x04000006 RID: 6
		public static readonly int SizeInBytes = 2;

		// Token: 0x04000007 RID: 7
		public static readonly float MinValue = 5.9604645E-08f;

		// Token: 0x04000008 RID: 8
		public static readonly float MinNormalizedValue = 6.1035156E-05f;

		// Token: 0x04000009 RID: 9
		public static readonly float MaxValue = 65504f;

		// Token: 0x0400000A RID: 10
		public static readonly float Epsilon = 0.00097656f;
	}
}
