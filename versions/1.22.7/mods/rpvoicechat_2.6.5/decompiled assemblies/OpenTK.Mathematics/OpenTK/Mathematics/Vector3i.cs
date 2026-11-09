using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace OpenTK.Mathematics
{
	// Token: 0x02000029 RID: 41
	[Serializable]
	public struct Vector3i : IEquatable<Vector3i>, IFormattable
	{
		// Token: 0x06000B2E RID: 2862 RVA: 0x00029CC5 File Offset: 0x00027EC5
		public Vector3i(int value)
		{
			this.X = value;
			this.Y = value;
			this.Z = value;
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x00029CDC File Offset: 0x00027EDC
		public Vector3i(int x, int y, int z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x00029CF3 File Offset: 0x00027EF3
		public Vector3i(Vector2i v)
		{
			this.X = v.X;
			this.Y = v.Y;
			this.Z = 0;
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x00029D14 File Offset: 0x00027F14
		public Vector3i(Vector2i v, int z)
		{
			this.X = v.X;
			this.Y = v.Y;
			this.Z = z;
		}

		// Token: 0x17000212 RID: 530
		public unsafe int this[int index]
		{
			readonly get
			{
				if (index < 3)
				{
					return *this.GetElementUnsafe(this, index);
				}
				throw new IndexOutOfRangeException("You tried to access this vector at index: " + index.ToString());
			}
			set
			{
				if (index < 3)
				{
					*this.GetElementUnsafe(this, index) = value;
					return;
				}
				throw new IndexOutOfRangeException("You tried to set this vector at index: " + index.ToString());
			}
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x00029D82 File Offset: 0x00027F82
		[MethodImpl(256)]
		private readonly ref int GetElementUnsafe(in Vector3i v, int index)
		{
			return Unsafe.Add<int>(Unsafe.AsRef<int>(ref v.X), index);
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000B35 RID: 2869 RVA: 0x00029D95 File Offset: 0x00027F95
		public readonly int ManhattanLength
		{
			get
			{
				return Math.Abs(this.X) + Math.Abs(this.Y) + Math.Abs(this.Z);
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000B36 RID: 2870 RVA: 0x00029DBA File Offset: 0x00027FBA
		public readonly int EuclideanLengthSquared
		{
			get
			{
				return this.X * this.X + this.Y * this.Y + this.Z * this.Z;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000B37 RID: 2871 RVA: 0x00029DE5 File Offset: 0x00027FE5
		public readonly float EuclideanLength
		{
			get
			{
				return MathF.Sqrt((float)(this.X * this.X + this.Y * this.Y + this.Z * this.Z));
			}
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x00029E18 File Offset: 0x00028018
		public readonly Vector3i Abs()
		{
			Vector3i vector3i = this;
			vector3i.X = Math.Abs(vector3i.X);
			vector3i.Y = Math.Abs(vector3i.Y);
			vector3i.Z = Math.Abs(vector3i.Z);
			return vector3i;
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x00029E63 File Offset: 0x00028063
		public static Vector3i Add(Vector3i a, Vector3i b)
		{
			Vector3i.Add(a, b, out a);
			return a;
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x00029E71 File Offset: 0x00028071
		public static void Add(in Vector3i a, in Vector3i b, out Vector3i result)
		{
			result.X = a.X + b.X;
			result.Y = a.Y + b.Y;
			result.Z = a.Z + b.Z;
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x00029EAC File Offset: 0x000280AC
		public static Vector3i Subtract(Vector3i a, Vector3i b)
		{
			Vector3i.Subtract(a, b, out a);
			return a;
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x00029EBA File Offset: 0x000280BA
		public static void Subtract(in Vector3i a, in Vector3i b, out Vector3i result)
		{
			result.X = a.X - b.X;
			result.Y = a.Y - b.Y;
			result.Z = a.Z - b.Z;
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x00029EF5 File Offset: 0x000280F5
		public static Vector3i Multiply(Vector3i vector, int scale)
		{
			Vector3i.Multiply(vector, scale, out vector);
			return vector;
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x00029F02 File Offset: 0x00028102
		public static void Multiply(in Vector3i vector, int scale, out Vector3i result)
		{
			result.X = vector.X * scale;
			result.Y = vector.Y * scale;
			result.Z = vector.Z * scale;
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00029F2E File Offset: 0x0002812E
		public static Vector3i Multiply(Vector3i vector, Vector3i scale)
		{
			Vector3i.Multiply(vector, scale, out vector);
			return vector;
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x00029F3C File Offset: 0x0002813C
		public static void Multiply(in Vector3i vector, in Vector3i scale, out Vector3i result)
		{
			result.X = vector.X * scale.X;
			result.Y = vector.Y * scale.Y;
			result.Z = vector.Z * scale.Z;
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x00029F77 File Offset: 0x00028177
		public static Vector3i Divide(Vector3i vector, int scale)
		{
			Vector3i.Divide(vector, scale, out vector);
			return vector;
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x00029F84 File Offset: 0x00028184
		public static void Divide(in Vector3i vector, int scale, out Vector3i result)
		{
			result.X = vector.X / scale;
			result.Y = vector.Y / scale;
			result.Z = vector.Z / scale;
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x00029FB0 File Offset: 0x000281B0
		public static Vector3i Divide(Vector3i vector, Vector3i scale)
		{
			Vector3i.Divide(vector, scale, out vector);
			return vector;
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x00029FBE File Offset: 0x000281BE
		public static void Divide(in Vector3i vector, in Vector3i scale, out Vector3i result)
		{
			result.X = vector.X / scale.X;
			result.Y = vector.Y / scale.Y;
			result.Z = vector.Z / scale.Z;
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00029FFC File Offset: 0x000281FC
		public static Vector3i ComponentMin(Vector3i a, Vector3i b)
		{
			Vector3i result;
			result.X = Math.Min(a.X, b.X);
			result.Y = Math.Min(a.Y, b.Y);
			result.Z = Math.Min(a.Z, b.Z);
			return result;
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x0002A054 File Offset: 0x00028254
		public static void ComponentMin(in Vector3i a, in Vector3i b, out Vector3i result)
		{
			result.X = Math.Min(a.X, b.X);
			result.Y = Math.Min(a.Y, b.Y);
			result.Z = Math.Min(a.Z, b.Z);
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x0002A0A8 File Offset: 0x000282A8
		public static Vector3i ComponentMax(Vector3i a, Vector3i b)
		{
			Vector3i result;
			result.X = Math.Max(a.X, b.X);
			result.Y = Math.Max(a.Y, b.Y);
			result.Z = Math.Max(a.Z, b.Z);
			return result;
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x0002A100 File Offset: 0x00028300
		public static void ComponentMax(in Vector3i a, in Vector3i b, out Vector3i result)
		{
			result.X = Math.Max(a.X, b.X);
			result.Y = Math.Max(a.Y, b.Y);
			result.Z = Math.Max(a.Z, b.Z);
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x0002A154 File Offset: 0x00028354
		public static Vector3i Clamp(Vector3i vec, Vector3i min, Vector3i max)
		{
			Vector3i result;
			result.X = MathHelper.Clamp(vec.X, min.X, max.X);
			result.Y = MathHelper.Clamp(vec.Y, min.Y, max.Y);
			result.Z = MathHelper.Clamp(vec.Z, min.Z, max.Z);
			return result;
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x0002A1BC File Offset: 0x000283BC
		public static void Clamp(in Vector3i vec, in Vector3i min, in Vector3i max, out Vector3i result)
		{
			result.X = MathHelper.Clamp(vec.X, min.X, max.X);
			result.Y = MathHelper.Clamp(vec.Y, min.Y, max.Y);
			result.Z = MathHelper.Clamp(vec.Z, min.Z, max.Z);
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x0002A220 File Offset: 0x00028420
		public static Vector3i Abs(Vector3i vec)
		{
			vec.X = Math.Abs(vec.X);
			vec.Y = Math.Abs(vec.Y);
			vec.Z = Math.Abs(vec.Z);
			return vec;
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x0002A259 File Offset: 0x00028459
		public static void Abs(in Vector3i vec, out Vector3i result)
		{
			result.X = Math.Abs(vec.X);
			result.Y = Math.Abs(vec.Y);
			result.Z = Math.Abs(vec.Z);
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000B4D RID: 2893 RVA: 0x0002A28E File Offset: 0x0002848E
		// (set) Token: 0x06000B4E RID: 2894 RVA: 0x0002A29B File Offset: 0x0002849B
		[XmlIgnore]
		public unsafe Vector2i Xy
		{
			get
			{
				return *Unsafe.As<Vector3i, Vector2i>(ref this);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000B4F RID: 2895 RVA: 0x0002A2B5 File Offset: 0x000284B5
		// (set) Token: 0x06000B50 RID: 2896 RVA: 0x0002A2C8 File Offset: 0x000284C8
		[XmlIgnore]
		public Vector2i Xz
		{
			readonly get
			{
				return new Vector2i(this.X, this.Z);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000B51 RID: 2897 RVA: 0x0002A2E2 File Offset: 0x000284E2
		// (set) Token: 0x06000B52 RID: 2898 RVA: 0x0002A2F5 File Offset: 0x000284F5
		[XmlIgnore]
		public Vector2i Yx
		{
			readonly get
			{
				return new Vector2i(this.Y, this.X);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000B53 RID: 2899 RVA: 0x0002A30F File Offset: 0x0002850F
		// (set) Token: 0x06000B54 RID: 2900 RVA: 0x0002A322 File Offset: 0x00028522
		[XmlIgnore]
		public Vector2i Yz
		{
			readonly get
			{
				return new Vector2i(this.Y, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000B55 RID: 2901 RVA: 0x0002A33C File Offset: 0x0002853C
		// (set) Token: 0x06000B56 RID: 2902 RVA: 0x0002A34F File Offset: 0x0002854F
		[XmlIgnore]
		public Vector2i Zx
		{
			readonly get
			{
				return new Vector2i(this.Z, this.X);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000B57 RID: 2903 RVA: 0x0002A369 File Offset: 0x00028569
		// (set) Token: 0x06000B58 RID: 2904 RVA: 0x0002A37C File Offset: 0x0002857C
		[XmlIgnore]
		public Vector2i Zy
		{
			readonly get
			{
				return new Vector2i(this.Z, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000B59 RID: 2905 RVA: 0x0002A396 File Offset: 0x00028596
		// (set) Token: 0x06000B5A RID: 2906 RVA: 0x0002A3AF File Offset: 0x000285AF
		[XmlIgnore]
		public Vector3i Xzy
		{
			readonly get
			{
				return new Vector3i(this.X, this.Z, this.Y);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000B5B RID: 2907 RVA: 0x0002A3D5 File Offset: 0x000285D5
		// (set) Token: 0x06000B5C RID: 2908 RVA: 0x0002A3EE File Offset: 0x000285EE
		[XmlIgnore]
		public Vector3i Yxz
		{
			readonly get
			{
				return new Vector3i(this.Y, this.X, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000B5D RID: 2909 RVA: 0x0002A414 File Offset: 0x00028614
		// (set) Token: 0x06000B5E RID: 2910 RVA: 0x0002A42D File Offset: 0x0002862D
		[XmlIgnore]
		public Vector3i Yzx
		{
			readonly get
			{
				return new Vector3i(this.Y, this.Z, this.X);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000B5F RID: 2911 RVA: 0x0002A453 File Offset: 0x00028653
		// (set) Token: 0x06000B60 RID: 2912 RVA: 0x0002A46C File Offset: 0x0002866C
		[XmlIgnore]
		public Vector3i Zxy
		{
			readonly get
			{
				return new Vector3i(this.Z, this.X, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x0002A492 File Offset: 0x00028692
		// (set) Token: 0x06000B62 RID: 2914 RVA: 0x0002A4AB File Offset: 0x000286AB
		[XmlIgnore]
		public Vector3i Zyx
		{
			readonly get
			{
				return new Vector3i(this.Z, this.Y, this.X);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x0002A4D1 File Offset: 0x000286D1
		public readonly Vector3 ToVector3()
		{
			return new Vector3((float)this.X, (float)this.Y, (float)this.Z);
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0002A4ED File Offset: 0x000286ED
		public static void ToVector3(in Vector3i input, out Vector3 result)
		{
			result.X = (float)input.X;
			result.Y = (float)input.Y;
			result.Z = (float)input.Z;
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0002A516 File Offset: 0x00028716
		public static Vector3i operator +(Vector3i left, Vector3i right)
		{
			left.X += right.X;
			left.Y += right.Y;
			left.Z += right.Z;
			return left;
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0002A54C File Offset: 0x0002874C
		public static Vector3i operator -(Vector3i left, Vector3i right)
		{
			left.X -= right.X;
			left.Y -= right.Y;
			left.Z -= right.Z;
			return left;
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x0002A582 File Offset: 0x00028782
		public static Vector3i operator -(Vector3i vec)
		{
			vec.X = -vec.X;
			vec.Y = -vec.Y;
			vec.Z = -vec.Z;
			return vec;
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x0002A5AF File Offset: 0x000287AF
		public static Vector3i operator *(Vector3i vec, int scale)
		{
			vec.X *= scale;
			vec.Y *= scale;
			vec.Z *= scale;
			return vec;
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0002A5D6 File Offset: 0x000287D6
		public static Vector3i operator *(int scale, Vector3i vec)
		{
			vec.X *= scale;
			vec.Y *= scale;
			vec.Z *= scale;
			return vec;
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x0002A5FD File Offset: 0x000287FD
		public static Vector3i operator *(Vector3i vec, Vector3i scale)
		{
			vec.X *= scale.X;
			vec.Y *= scale.Y;
			vec.Z *= scale.Z;
			return vec;
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x0002A633 File Offset: 0x00028833
		public static Vector3i operator /(Vector3i vec, int scale)
		{
			vec.X /= scale;
			vec.Y /= scale;
			vec.Z /= scale;
			return vec;
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0002A65A File Offset: 0x0002885A
		public static Vector3i operator /(Vector3i vec, Vector3i scale)
		{
			vec.X /= scale.X;
			vec.Y /= scale.Y;
			vec.Z /= scale.Z;
			return vec;
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0002A690 File Offset: 0x00028890
		public static bool operator ==(Vector3i left, Vector3i right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x0002A69A File Offset: 0x0002889A
		public static bool operator !=(Vector3i left, Vector3i right)
		{
			return !(left == right);
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x0002A6A6 File Offset: 0x000288A6
		public static implicit operator Vector3(Vector3i vec)
		{
			return new Vector3((float)vec.X, (float)vec.Y, (float)vec.Z);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0002A6C2 File Offset: 0x000288C2
		public static implicit operator Vector3d(Vector3i vec)
		{
			return new Vector3d((double)vec.X, (double)vec.Y, (double)vec.Z);
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x0002A6DE File Offset: 0x000288DE
		public static explicit operator Vector3h(Vector3i vec)
		{
			return new Vector3h((float)vec.X, (float)vec.Y, (float)vec.Z);
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x0002A6FA File Offset: 0x000288FA
		public static implicit operator Vector3i([TupleElementNames(new string[]
		{
			"X",
			"Y",
			"Z"
		})] ValueTuple<int, int, int> values)
		{
			return new Vector3i(values.Item1, values.Item2, values.Item3);
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x0002A713 File Offset: 0x00028913
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0002A71D File Offset: 0x0002891D
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x0002A727 File Offset: 0x00028927
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x0002A734 File Offset: 0x00028934
		public readonly string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("({0}{3} {1}{3} {2})", new object[]
			{
				this.X.ToString(format, formatProvider),
				this.Y.ToString(format, formatProvider),
				this.Z.ToString(format, formatProvider),
				MathHelper.GetListSeparator(formatProvider)
			});
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x0002A78A File Offset: 0x0002898A
		public override bool Equals(object obj)
		{
			return obj is Vector3i && this.Equals((Vector3i)obj);
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x0002A7A2 File Offset: 0x000289A2
		public readonly bool Equals(Vector3i other)
		{
			return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x0002A7D0 File Offset: 0x000289D0
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<int, int, int>(this.X, this.Y, this.Z);
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x0002A7E9 File Offset: 0x000289E9
		public readonly void Deconstruct(out int x, out int y, out int z)
		{
			x = this.X;
			y = this.Y;
			z = this.Z;
		}

		// Token: 0x040000BC RID: 188
		public int X;

		// Token: 0x040000BD RID: 189
		public int Y;

		// Token: 0x040000BE RID: 190
		public int Z;

		// Token: 0x040000BF RID: 191
		public static readonly Vector3i UnitX = new Vector3i(1, 0, 0);

		// Token: 0x040000C0 RID: 192
		public static readonly Vector3i UnitY = new Vector3i(0, 1, 0);

		// Token: 0x040000C1 RID: 193
		public static readonly Vector3i UnitZ = new Vector3i(0, 0, 1);

		// Token: 0x040000C2 RID: 194
		public static readonly Vector3i Zero = new Vector3i(0, 0, 0);

		// Token: 0x040000C3 RID: 195
		public static readonly Vector3i One = new Vector3i(1, 1, 1);

		// Token: 0x040000C4 RID: 196
		public static readonly int SizeInBytes = Unsafe.SizeOf<Vector3i>();
	}
}
