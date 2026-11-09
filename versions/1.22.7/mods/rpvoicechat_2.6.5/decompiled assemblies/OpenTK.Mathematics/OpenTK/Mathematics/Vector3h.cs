using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace OpenTK.Mathematics
{
	// Token: 0x02000028 RID: 40
	[Serializable]
	public struct Vector3h : ISerializable, IEquatable<Vector3h>, IFormattable
	{
		// Token: 0x06000AF4 RID: 2804 RVA: 0x0002948A File Offset: 0x0002768A
		public Vector3h(Half value)
		{
			this.X = value;
			this.Y = value;
			this.Z = value;
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x000294A1 File Offset: 0x000276A1
		public Vector3h(float value)
		{
			this.X = new Half(value);
			this.Y = new Half(value);
			this.Z = new Half(value);
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x000294C7 File Offset: 0x000276C7
		public Vector3h(Half x, Half y, Half z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x000294DE File Offset: 0x000276DE
		public Vector3h(float x, float y, float z)
		{
			this.X = new Half(x);
			this.Y = new Half(y);
			this.Z = new Half(z);
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x00029504 File Offset: 0x00027704
		public Vector3h(float x, float y, float z, bool throwOnError)
		{
			this.X = new Half(x, throwOnError);
			this.Y = new Half(y, throwOnError);
			this.Z = new Half(z, throwOnError);
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x00029530 File Offset: 0x00027730
		public Vector3h(Vector3 v)
		{
			this.X = new Half(v.X);
			this.Y = new Half(v.Y);
			this.Z = new Half(v.Z);
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00029565 File Offset: 0x00027765
		public Vector3h(Vector3 v, bool throwOnError)
		{
			this.X = new Half(v.X, throwOnError);
			this.Y = new Half(v.Y, throwOnError);
			this.Z = new Half(v.Z, throwOnError);
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0002959D File Offset: 0x0002779D
		public Vector3h(in Vector3 v)
		{
			this.X = new Half(v.X);
			this.Y = new Half(v.Y);
			this.Z = new Half(v.Z);
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x000295D2 File Offset: 0x000277D2
		public Vector3h(in Vector3 v, bool throwOnError)
		{
			this.X = new Half(v.X, throwOnError);
			this.Y = new Half(v.Y, throwOnError);
			this.Z = new Half(v.Z, throwOnError);
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0002960A File Offset: 0x0002780A
		public Vector3h(Vector3d v)
		{
			this.X = new Half(v.X);
			this.Y = new Half(v.Y);
			this.Z = new Half(v.Z);
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0002963F File Offset: 0x0002783F
		public Vector3h(Vector3d v, bool throwOnError)
		{
			this.X = new Half(v.X, throwOnError);
			this.Y = new Half(v.Y, throwOnError);
			this.Z = new Half(v.Z, throwOnError);
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x00029677 File Offset: 0x00027877
		public Vector3h(in Vector3d v)
		{
			this.X = new Half(v.X);
			this.Y = new Half(v.Y);
			this.Z = new Half(v.Z);
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x000296AC File Offset: 0x000278AC
		public Vector3h(in Vector3d v, bool throwOnError)
		{
			this.X = new Half(v.X, throwOnError);
			this.Y = new Half(v.Y, throwOnError);
			this.Z = new Half(v.Z, throwOnError);
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000B01 RID: 2817 RVA: 0x000296E4 File Offset: 0x000278E4
		// (set) Token: 0x06000B02 RID: 2818 RVA: 0x000296F1 File Offset: 0x000278F1
		[XmlIgnore]
		public unsafe Vector2h Xy
		{
			get
			{
				return *Unsafe.As<Vector3h, Vector2h>(ref this);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000B03 RID: 2819 RVA: 0x0002970B File Offset: 0x0002790B
		// (set) Token: 0x06000B04 RID: 2820 RVA: 0x0002971E File Offset: 0x0002791E
		[XmlIgnore]
		public Vector2h Xz
		{
			readonly get
			{
				return new Vector2h(this.X, this.Z);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000B05 RID: 2821 RVA: 0x00029738 File Offset: 0x00027938
		// (set) Token: 0x06000B06 RID: 2822 RVA: 0x0002974B File Offset: 0x0002794B
		[XmlIgnore]
		public Vector2h Yx
		{
			readonly get
			{
				return new Vector2h(this.Y, this.X);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000B07 RID: 2823 RVA: 0x00029765 File Offset: 0x00027965
		// (set) Token: 0x06000B08 RID: 2824 RVA: 0x00029778 File Offset: 0x00027978
		[XmlIgnore]
		public Vector2h Yz
		{
			readonly get
			{
				return new Vector2h(this.Y, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000B09 RID: 2825 RVA: 0x00029792 File Offset: 0x00027992
		// (set) Token: 0x06000B0A RID: 2826 RVA: 0x000297A5 File Offset: 0x000279A5
		[XmlIgnore]
		public Vector2h Zx
		{
			readonly get
			{
				return new Vector2h(this.Z, this.X);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000B0B RID: 2827 RVA: 0x000297BF File Offset: 0x000279BF
		// (set) Token: 0x06000B0C RID: 2828 RVA: 0x000297D2 File Offset: 0x000279D2
		[XmlIgnore]
		public Vector2h Zy
		{
			readonly get
			{
				return new Vector2h(this.Z, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000B0D RID: 2829 RVA: 0x000297EC File Offset: 0x000279EC
		// (set) Token: 0x06000B0E RID: 2830 RVA: 0x00029805 File Offset: 0x00027A05
		[XmlIgnore]
		public Vector3h Xzy
		{
			readonly get
			{
				return new Vector3h(this.X, this.Z, this.Y);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000B0F RID: 2831 RVA: 0x0002982B File Offset: 0x00027A2B
		// (set) Token: 0x06000B10 RID: 2832 RVA: 0x00029844 File Offset: 0x00027A44
		[XmlIgnore]
		public Vector3h Yxz
		{
			readonly get
			{
				return new Vector3h(this.Y, this.X, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000B11 RID: 2833 RVA: 0x0002986A File Offset: 0x00027A6A
		// (set) Token: 0x06000B12 RID: 2834 RVA: 0x00029883 File Offset: 0x00027A83
		[XmlIgnore]
		public Vector3h Yzx
		{
			readonly get
			{
				return new Vector3h(this.Y, this.Z, this.X);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x000298A9 File Offset: 0x00027AA9
		// (set) Token: 0x06000B14 RID: 2836 RVA: 0x000298C2 File Offset: 0x00027AC2
		[XmlIgnore]
		public Vector3h Zxy
		{
			readonly get
			{
				return new Vector3h(this.Z, this.X, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x000298E8 File Offset: 0x00027AE8
		// (set) Token: 0x06000B16 RID: 2838 RVA: 0x00029901 File Offset: 0x00027B01
		[XmlIgnore]
		public Vector3h Zyx
		{
			readonly get
			{
				return new Vector3h(this.Z, this.Y, this.X);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x00029927 File Offset: 0x00027B27
		public readonly Vector3 ToVector3()
		{
			return new Vector3(this.X, this.Y, this.Z);
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x0002994F File Offset: 0x00027B4F
		public readonly Vector3d ToVector3d()
		{
			return new Vector3d(this.X, this.Y, this.Z);
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x00029977 File Offset: 0x00027B77
		public static implicit operator Vector3(Vector3h vec)
		{
			return new Vector3(vec.X, vec.Y, vec.Z);
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0002999F File Offset: 0x00027B9F
		public static implicit operator Vector3d(Vector3h vec)
		{
			return new Vector3d(vec.X, vec.Y, vec.Z);
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x000299C7 File Offset: 0x00027BC7
		public static explicit operator Vector3i(Vector3h vec)
		{
			return new Vector3i((int)vec.X, (int)vec.Y, (int)vec.Z);
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x000299F5 File Offset: 0x00027BF5
		public static implicit operator Vector3h([TupleElementNames(new string[]
		{
			"X",
			"Y",
			"Z"
		})] ValueTuple<Half, Half, Half> values)
		{
			return new Vector3h(values.Item1, values.Item2, values.Item3);
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x00029A0E File Offset: 0x00027C0E
		public static bool operator ==(Vector3h left, Vector3h right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x00029A18 File Offset: 0x00027C18
		public static bool operator !=(Vector3h left, Vector3h right)
		{
			return !(left == right);
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x00029A24 File Offset: 0x00027C24
		public Vector3h(SerializationInfo info, StreamingContext context)
		{
			this.X = (Half)info.GetValue("X", typeof(Half));
			this.Y = (Half)info.GetValue("Y", typeof(Half));
			this.Z = (Half)info.GetValue("Z", typeof(Half));
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x00029A94 File Offset: 0x00027C94
		public readonly void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("X", this.X);
			info.AddValue("Y", this.Y);
			info.AddValue("Z", this.Z);
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x00029AE3 File Offset: 0x00027CE3
		public void FromBinaryStream(BinaryReader bin)
		{
			this.X.FromBinaryStream(bin);
			this.Y.FromBinaryStream(bin);
			this.Z.FromBinaryStream(bin);
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x00029B09 File Offset: 0x00027D09
		public void ToBinaryStream(BinaryWriter bin)
		{
			this.X.ToBinaryStream(bin);
			this.Y.ToBinaryStream(bin);
			this.Z.ToBinaryStream(bin);
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x00029B2F File Offset: 0x00027D2F
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x00029B39 File Offset: 0x00027D39
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00029B43 File Offset: 0x00027D43
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x00029B50 File Offset: 0x00027D50
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("({0}{3} {1}{3} {2})", new object[]
			{
				this.X.ToString(format, formatProvider),
				this.Y.ToString(format, formatProvider),
				this.Z.ToString(format, formatProvider),
				MathHelper.GetListSeparator(formatProvider)
			});
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x00029BA6 File Offset: 0x00027DA6
		public override bool Equals(object obj)
		{
			return obj is Vector3h && this.Equals((Vector3h)obj);
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x00029BBE File Offset: 0x00027DBE
		public bool Equals(Vector3h other)
		{
			return this.X.Equals(other.X) && this.Y.Equals(other.Y) && this.Z.Equals(other.Z);
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x00029BF9 File Offset: 0x00027DF9
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Half, Half, Half>(this.X, this.Y, this.Z);
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x00029C14 File Offset: 0x00027E14
		public static byte[] GetBytes(Vector3h h)
		{
			byte[] array = new byte[Vector3h.SizeInBytes];
			byte[] bytes = Half.GetBytes(h.X);
			array[0] = bytes[0];
			array[1] = bytes[1];
			bytes = Half.GetBytes(h.Y);
			array[2] = bytes[0];
			array[3] = bytes[1];
			bytes = Half.GetBytes(h.Z);
			array[4] = bytes[0];
			array[5] = bytes[1];
			return array;
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x00029C73 File Offset: 0x00027E73
		public static Vector3h FromBytes(byte[] value, int startIndex)
		{
			return new Vector3h(Half.FromBytes(value, startIndex), Half.FromBytes(value, startIndex + 2), Half.FromBytes(value, startIndex + 4));
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00029C93 File Offset: 0x00027E93
		public readonly void Deconstruct(out Half x, out Half y, out Half z)
		{
			x = this.X;
			y = this.Y;
			z = this.Z;
		}

		// Token: 0x040000B8 RID: 184
		public Half X;

		// Token: 0x040000B9 RID: 185
		public Half Y;

		// Token: 0x040000BA RID: 186
		public Half Z;

		// Token: 0x040000BB RID: 187
		public static readonly int SizeInBytes = Unsafe.SizeOf<Vector3h>();
	}
}
