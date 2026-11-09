using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace OpenTK.Mathematics
{
	// Token: 0x0200002B RID: 43
	[Serializable]
	public struct Vector4d : IEquatable<Vector4d>, IFormattable
	{
		// Token: 0x06000C6B RID: 3179 RVA: 0x0002D923 File Offset: 0x0002BB23
		public Vector4d(double value)
		{
			this.X = value;
			this.Y = value;
			this.Z = value;
			this.W = value;
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x0002D941 File Offset: 0x0002BB41
		public Vector4d(double x, double y, double z, double w)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x0002D960 File Offset: 0x0002BB60
		public Vector4d(Vector2d v)
		{
			this.X = v.X;
			this.Y = v.Y;
			this.Z = 0.0;
			this.W = 0.0;
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x0002D998 File Offset: 0x0002BB98
		public Vector4d(Vector3d v)
		{
			this.X = v.X;
			this.Y = v.Y;
			this.Z = v.Z;
			this.W = 0.0;
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x0002D9CD File Offset: 0x0002BBCD
		public Vector4d(Vector3d v, double w)
		{
			this.X = v.X;
			this.Y = v.Y;
			this.Z = v.Z;
			this.W = w;
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x0002D9FA File Offset: 0x0002BBFA
		public Vector4d(Vector4d v)
		{
			this.X = v.X;
			this.Y = v.Y;
			this.Z = v.Z;
			this.W = v.W;
		}

		// Token: 0x17000265 RID: 613
		public unsafe double this[int index]
		{
			readonly get
			{
				if (index < 4)
				{
					return *this.GetElementUnsafe(this, index);
				}
				MathHelper.ThrowOutOfRangeException("You tried to access this vector at index: " + index.ToString());
				return 0.0;
			}
			set
			{
				if (index < 4)
				{
					*this.GetElementUnsafe(this, index) = value;
					return;
				}
				throw new IndexOutOfRangeException("You tried to set this vector at index: " + index.ToString());
			}
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x0002DA82 File Offset: 0x0002BC82
		[MethodImpl(256)]
		private readonly ref double GetElementUnsafe(in Vector4d v, int index)
		{
			return Unsafe.Add<double>(Unsafe.AsRef<double>(ref v.X), index);
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000C74 RID: 3188 RVA: 0x0002DA95 File Offset: 0x0002BC95
		public readonly double Length
		{
			get
			{
				return Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W);
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000C75 RID: 3189 RVA: 0x0002DAD3 File Offset: 0x0002BCD3
		public readonly double ReciprocalLengthFast
		{
			get
			{
				return MathHelper.InverseSqrtFast(this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W);
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000C76 RID: 3190 RVA: 0x0002DB14 File Offset: 0x0002BD14
		public readonly double LengthFast
		{
			get
			{
				return 1.0 / MathHelper.InverseSqrtFast(this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W);
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000C77 RID: 3191 RVA: 0x0002DB67 File Offset: 0x0002BD67
		public readonly double LengthSquared
		{
			get
			{
				return this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W;
			}
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x0002DBA0 File Offset: 0x0002BDA0
		public readonly Vector4d Normalized()
		{
			Vector4d result = this;
			result.Normalize();
			return result;
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x0002DBBC File Offset: 0x0002BDBC
		public void Normalize()
		{
			double num = 1.0 / this.Length;
			this.X *= num;
			this.Y *= num;
			this.Z *= num;
			this.W *= num;
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x0002DC14 File Offset: 0x0002BE14
		public void NormalizeFast()
		{
			double num = MathHelper.InverseSqrtFast(this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W);
			this.X *= num;
			this.Y *= num;
			this.Z *= num;
			this.W *= num;
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x0002DC98 File Offset: 0x0002BE98
		public readonly Vector4d Abs()
		{
			Vector4d vector4d = this;
			vector4d.X = Math.Abs(vector4d.X);
			vector4d.Y = Math.Abs(vector4d.Y);
			vector4d.Z = Math.Abs(vector4d.Z);
			vector4d.W = Math.Abs(vector4d.W);
			return vector4d;
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x0002DCF5 File Offset: 0x0002BEF5
		public readonly Vector4d Round()
		{
			return Vector4d.Round(this);
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x0002DD02 File Offset: 0x0002BF02
		public readonly Vector4d Round(MidpointRounding rounding)
		{
			return Vector4d.Round(this, rounding);
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x0002DD10 File Offset: 0x0002BF10
		public readonly Vector4d Ceiling()
		{
			return Vector4d.Ceiling(this);
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x0002DD1D File Offset: 0x0002BF1D
		public readonly Vector4d Floor()
		{
			return Vector4d.Floor(this);
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x0002DD2A File Offset: 0x0002BF2A
		public readonly Vector4d Truncate()
		{
			return Vector4d.Truncate(this);
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x0002DD37 File Offset: 0x0002BF37
		public static Vector4d Add(Vector4d a, Vector4d b)
		{
			Vector4d.Add(a, b, out a);
			return a;
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x0002DD48 File Offset: 0x0002BF48
		public static void Add(in Vector4d a, in Vector4d b, out Vector4d result)
		{
			result.X = a.X + b.X;
			result.Y = a.Y + b.Y;
			result.Z = a.Z + b.Z;
			result.W = a.W + b.W;
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x0002DDA1 File Offset: 0x0002BFA1
		public static Vector4d Subtract(Vector4d a, Vector4d b)
		{
			Vector4d.Subtract(a, b, out a);
			return a;
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x0002DDB0 File Offset: 0x0002BFB0
		public static void Subtract(in Vector4d a, in Vector4d b, out Vector4d result)
		{
			result.X = a.X - b.X;
			result.Y = a.Y - b.Y;
			result.Z = a.Z - b.Z;
			result.W = a.W - b.W;
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x0002DE09 File Offset: 0x0002C009
		public static Vector4d Multiply(Vector4d vector, double scale)
		{
			Vector4d.Multiply(vector, scale, out vector);
			return vector;
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x0002DE16 File Offset: 0x0002C016
		public static void Multiply(in Vector4d vector, double scale, out Vector4d result)
		{
			result.X = vector.X * scale;
			result.Y = vector.Y * scale;
			result.Z = vector.Z * scale;
			result.W = vector.W * scale;
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x0002DE50 File Offset: 0x0002C050
		public static Vector4d Multiply(Vector4d vector, Vector4d scale)
		{
			Vector4d.Multiply(vector, scale, out vector);
			return vector;
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x0002DE60 File Offset: 0x0002C060
		public static void Multiply(in Vector4d vector, in Vector4d scale, out Vector4d result)
		{
			result.X = vector.X * scale.X;
			result.Y = vector.Y * scale.Y;
			result.Z = vector.Z * scale.Z;
			result.W = vector.W * scale.W;
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x0002DEB9 File Offset: 0x0002C0B9
		public static Vector4d Divide(Vector4d vector, double scale)
		{
			Vector4d.Divide(vector, scale, out vector);
			return vector;
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x0002DEC6 File Offset: 0x0002C0C6
		public static void Divide(in Vector4d vector, double scale, out Vector4d result)
		{
			result.X = vector.X / scale;
			result.Y = vector.Y / scale;
			result.Z = vector.Z / scale;
			result.W = vector.W / scale;
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x0002DF00 File Offset: 0x0002C100
		public static Vector4d Divide(Vector4d vector, Vector4d scale)
		{
			Vector4d.Divide(vector, scale, out vector);
			return vector;
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x0002DF10 File Offset: 0x0002C110
		public static void Divide(in Vector4d vector, in Vector4d scale, out Vector4d result)
		{
			result.X = vector.X / scale.X;
			result.Y = vector.Y / scale.Y;
			result.Z = vector.Z / scale.Z;
			result.W = vector.W / scale.W;
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x0002DF6C File Offset: 0x0002C16C
		public static Vector4d ComponentMin(Vector4d a, Vector4d b)
		{
			a.X = ((a.X < b.X) ? a.X : b.X);
			a.Y = ((a.Y < b.Y) ? a.Y : b.Y);
			a.Z = ((a.Z < b.Z) ? a.Z : b.Z);
			a.W = ((a.W < b.W) ? a.W : b.W);
			return a;
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x0002E008 File Offset: 0x0002C208
		public static void ComponentMin(in Vector4d a, in Vector4d b, out Vector4d result)
		{
			result.X = ((a.X < b.X) ? a.X : b.X);
			result.Y = ((a.Y < b.Y) ? a.Y : b.Y);
			result.Z = ((a.Z < b.Z) ? a.Z : b.Z);
			result.W = ((a.W < b.W) ? a.W : b.W);
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x0002E0A0 File Offset: 0x0002C2A0
		public static Vector4d ComponentMax(Vector4d a, Vector4d b)
		{
			a.X = ((a.X > b.X) ? a.X : b.X);
			a.Y = ((a.Y > b.Y) ? a.Y : b.Y);
			a.Z = ((a.Z > b.Z) ? a.Z : b.Z);
			a.W = ((a.W > b.W) ? a.W : b.W);
			return a;
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x0002E13C File Offset: 0x0002C33C
		public static void ComponentMax(in Vector4d a, in Vector4d b, out Vector4d result)
		{
			result.X = ((a.X > b.X) ? a.X : b.X);
			result.Y = ((a.Y > b.Y) ? a.Y : b.Y);
			result.Z = ((a.Z > b.Z) ? a.Z : b.Z);
			result.W = ((a.W > b.W) ? a.W : b.W);
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x0002E1D1 File Offset: 0x0002C3D1
		public static Vector4d MagnitudeMin(Vector4d left, Vector4d right)
		{
			if (left.LengthSquared >= right.LengthSquared)
			{
				return right;
			}
			return left;
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x0002E1E6 File Offset: 0x0002C3E6
		public static void MagnitudeMin(in Vector4d left, in Vector4d right, out Vector4d result)
		{
			result = ((left.LengthSquared < right.LengthSquared) ? left : right);
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x0002E20A File Offset: 0x0002C40A
		public static Vector4d MagnitudeMax(Vector4d left, Vector4d right)
		{
			if (left.LengthSquared < right.LengthSquared)
			{
				return right;
			}
			return left;
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x0002E21F File Offset: 0x0002C41F
		public static void MagnitudeMax(in Vector4d left, in Vector4d right, out Vector4d result)
		{
			result = ((left.LengthSquared >= right.LengthSquared) ? left : right);
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x0002E244 File Offset: 0x0002C444
		public static Vector4d Clamp(Vector4d vec, Vector4d min, Vector4d max)
		{
			vec.X = ((vec.X < min.X) ? min.X : ((vec.X > max.X) ? max.X : vec.X));
			vec.Y = ((vec.Y < min.Y) ? min.Y : ((vec.Y > max.Y) ? max.Y : vec.Y));
			vec.Z = ((vec.X < min.Z) ? min.Z : ((vec.Z > max.Z) ? max.Z : vec.Z));
			vec.W = ((vec.Y < min.W) ? min.W : ((vec.W > max.W) ? max.W : vec.W));
			return vec;
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x0002E338 File Offset: 0x0002C538
		public static void Clamp(in Vector4d vec, in Vector4d min, in Vector4d max, out Vector4d result)
		{
			result.X = ((vec.X < min.X) ? min.X : ((vec.X > max.X) ? max.X : vec.X));
			result.Y = ((vec.Y < min.Y) ? min.Y : ((vec.Y > max.Y) ? max.Y : vec.Y));
			result.Z = ((vec.X < min.Z) ? min.Z : ((vec.Z > max.Z) ? max.Z : vec.Z));
			result.W = ((vec.Y < min.W) ? min.W : ((vec.W > max.W) ? max.W : vec.W));
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x0002E428 File Offset: 0x0002C628
		public static Vector4d Abs(Vector4d vec)
		{
			vec.X = Math.Abs(vec.X);
			vec.Y = Math.Abs(vec.Y);
			vec.Z = Math.Abs(vec.Z);
			vec.W = Math.Abs(vec.W);
			return vec;
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x0002E480 File Offset: 0x0002C680
		public static void Abs(in Vector4d vec, out Vector4d result)
		{
			result.X = Math.Abs(vec.X);
			result.Y = Math.Abs(vec.Y);
			result.Z = Math.Abs(vec.Z);
			result.W = Math.Abs(vec.W);
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x0002E4D4 File Offset: 0x0002C6D4
		public static Vector4d Round(Vector4d vec)
		{
			vec.X = Math.Round(vec.X);
			vec.Y = Math.Round(vec.Y);
			vec.Z = Math.Round(vec.Z);
			vec.W = Math.Round(vec.W);
			return vec;
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x0002E52C File Offset: 0x0002C72C
		public static void Round(in Vector4d vec, out Vector4d result)
		{
			result.X = Math.Round(vec.X);
			result.Y = Math.Round(vec.Y);
			result.Z = Math.Round(vec.Z);
			result.W = Math.Round(vec.W);
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x0002E580 File Offset: 0x0002C780
		public static Vector4d Round(Vector4d vec, MidpointRounding rounding)
		{
			vec.X = Math.Round(vec.X, rounding);
			vec.Y = Math.Round(vec.Y, rounding);
			vec.Z = Math.Round(vec.Z, rounding);
			vec.W = Math.Round(vec.W, rounding);
			return vec;
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x0002E5DC File Offset: 0x0002C7DC
		public static void Round(in Vector4d vec, MidpointRounding rounding, out Vector4d result)
		{
			result.X = Math.Round(vec.X, rounding);
			result.Y = Math.Round(vec.Y, rounding);
			result.Z = Math.Round(vec.Z, rounding);
			result.W = Math.Round(vec.W, rounding);
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x0002E634 File Offset: 0x0002C834
		public static Vector4d Ceiling(Vector4d vec)
		{
			vec.X = Math.Ceiling(vec.X);
			vec.Y = Math.Ceiling(vec.Y);
			vec.Z = Math.Ceiling(vec.Z);
			vec.W = Math.Ceiling(vec.W);
			return vec;
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x0002E68C File Offset: 0x0002C88C
		public static void Ceiling(in Vector4d vec, out Vector4d result)
		{
			result.X = Math.Ceiling(vec.X);
			result.Y = Math.Ceiling(vec.Y);
			result.Z = Math.Ceiling(vec.Z);
			result.W = Math.Ceiling(vec.W);
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x0002E6E0 File Offset: 0x0002C8E0
		public static Vector4d Floor(Vector4d vec)
		{
			vec.X = Math.Floor(vec.X);
			vec.Y = Math.Floor(vec.Y);
			vec.Z = Math.Floor(vec.Z);
			vec.W = Math.Floor(vec.W);
			return vec;
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x0002E738 File Offset: 0x0002C938
		public static void Floor(in Vector4d vec, out Vector4d result)
		{
			result.X = Math.Floor(vec.X);
			result.Y = Math.Floor(vec.Y);
			result.Z = Math.Floor(vec.Z);
			result.W = Math.Floor(vec.W);
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x0002E78C File Offset: 0x0002C98C
		public static Vector4d Truncate(Vector4d vec)
		{
			vec.X = Math.Truncate(vec.X);
			vec.Y = Math.Truncate(vec.Y);
			vec.Z = Math.Truncate(vec.Z);
			vec.W = Math.Truncate(vec.W);
			return vec;
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x0002E7E4 File Offset: 0x0002C9E4
		public static void Truncate(in Vector4d vec, out Vector4d result)
		{
			result.X = Math.Truncate(vec.X);
			result.Y = Math.Truncate(vec.Y);
			result.Z = Math.Truncate(vec.Z);
			result.W = Math.Truncate(vec.W);
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x0002E838 File Offset: 0x0002CA38
		public static double Distance(Vector4d vec1, Vector4d vec2)
		{
			double result;
			Vector4d.Distance(vec1, vec2, out result);
			return result;
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x0002E854 File Offset: 0x0002CA54
		public static void Distance(in Vector4d vec1, in Vector4d vec2, out double result)
		{
			result = Math.Sqrt((vec2.X - vec1.X) * (vec2.X - vec1.X) + (vec2.Y - vec1.Y) * (vec2.Y - vec1.Y) + (vec2.Z - vec1.Z) * (vec2.Z - vec1.Z) + (vec2.W - vec1.W) * (vec2.W - vec1.W));
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x0002E8D8 File Offset: 0x0002CAD8
		public static double DistanceSquared(Vector4d vec1, Vector4d vec2)
		{
			double result;
			Vector4d.DistanceSquared(vec1, vec2, out result);
			return result;
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x0002E8F4 File Offset: 0x0002CAF4
		public static void DistanceSquared(in Vector4d vec1, in Vector4d vec2, out double result)
		{
			result = (vec2.X - vec1.X) * (vec2.X - vec1.X) + (vec2.Y - vec1.Y) * (vec2.Y - vec1.Y) + (vec2.Z - vec1.Z) * (vec2.Z - vec1.Z) + (vec2.W - vec1.W) * (vec2.W - vec1.W);
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x0002E974 File Offset: 0x0002CB74
		public static Vector4d Normalize(Vector4d vec)
		{
			double num = 1.0 / vec.Length;
			vec.X *= num;
			vec.Y *= num;
			vec.Z *= num;
			vec.W *= num;
			return vec;
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x0002E9C4 File Offset: 0x0002CBC4
		public static void Normalize(in Vector4d vec, out Vector4d result)
		{
			double num = 1.0 / vec.Length;
			result.X = vec.X * num;
			result.Y = vec.Y * num;
			result.Z = vec.Z * num;
			result.W = vec.W * num;
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x0002EA1C File Offset: 0x0002CC1C
		public static Vector4d NormalizeFast(Vector4d vec)
		{
			double num = MathHelper.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z + vec.W * vec.W);
			vec.X *= num;
			vec.Y *= num;
			vec.Z *= num;
			vec.W *= num;
			return vec;
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x0002EA98 File Offset: 0x0002CC98
		public static void NormalizeFast(in Vector4d vec, out Vector4d result)
		{
			double num = MathHelper.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z + vec.W * vec.W);
			result.X = vec.X * num;
			result.Y = vec.Y * num;
			result.Z = vec.Z * num;
			result.W = vec.W * num;
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x0002EB1A File Offset: 0x0002CD1A
		public static double Dot(Vector4d left, Vector4d right)
		{
			return left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x0002EB53 File Offset: 0x0002CD53
		public static void Dot(in Vector4d left, in Vector4d right, out double result)
		{
			result = left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x0002EB90 File Offset: 0x0002CD90
		public static Vector4d Lerp(Vector4d a, Vector4d b, double blend)
		{
			a.X = blend * (b.X - a.X) + a.X;
			a.Y = blend * (b.Y - a.Y) + a.Y;
			a.Z = blend * (b.Z - a.Z) + a.Z;
			a.W = blend * (b.W - a.W) + a.W;
			return a;
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x0002EC14 File Offset: 0x0002CE14
		public static void Lerp(in Vector4d a, in Vector4d b, double blend, out Vector4d result)
		{
			result.X = blend * (b.X - a.X) + a.X;
			result.Y = blend * (b.Y - a.Y) + a.Y;
			result.Z = blend * (b.Z - a.Z) + a.Z;
			result.W = blend * (b.W - a.W) + a.W;
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x0002EC94 File Offset: 0x0002CE94
		public static Vector4d Lerp(Vector4d a, Vector4d b, Vector4d blend)
		{
			a.X = blend.X * (b.X - a.X) + a.X;
			a.Y = blend.Y * (b.Y - a.Y) + a.Y;
			a.Z = blend.Z * (b.Z - a.Z) + a.Z;
			a.W = blend.W * (b.W - a.W) + a.W;
			return a;
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x0002ED2C File Offset: 0x0002CF2C
		public static void Lerp(in Vector4d a, in Vector4d b, Vector4d blend, out Vector4d result)
		{
			result.X = blend.X * (b.X - a.X) + a.X;
			result.Y = blend.Y * (b.Y - a.Y) + a.Y;
			result.Z = blend.Z * (b.Z - a.Z) + a.Z;
			result.W = blend.W * (b.W - a.W) + a.W;
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x0002EDC0 File Offset: 0x0002CFC0
		public static Vector4d Slerp(Vector4d a, Vector4d b, double t)
		{
			double num = a.Length * b.Length;
			double num2;
			if (num == 0.0 || Math.Abs(num2 = Vector4d.Dot(a, b) / num) > 0.99999999)
			{
				return Vector4d.Lerp(a, b, t);
			}
			double num3 = Math.Acos(Math.Clamp(num2, -1.0, 1.0));
			double num4 = Math.Sqrt(1.0 - num2 * num2);
			double scale = Math.Sin((1.0 - t) * num3) / num4;
			double scale2 = Math.Sin(t * num3) / num4;
			return scale * a + scale2 * b;
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x0002EE74 File Offset: 0x0002D074
		public static void Slerp(in Vector4d a, in Vector4d b, double t, out Vector4d result)
		{
			double num = a.Length * b.Length;
			if (num == 0.0)
			{
				Vector4d.Lerp(a, b, t, out result);
				return;
			}
			double num2;
			Vector4d.Dot(a, b, out num2);
			num2 /= num;
			if (Math.Abs(num2) > 0.99999999)
			{
				Vector4d.Lerp(a, b, t, out result);
				return;
			}
			double num3 = Math.Acos(num2);
			double num4 = Math.Sqrt(1.0 - num2 * num2);
			double scale = Math.Sin((1.0 - t) * num3) / num4;
			double scale2 = Math.Sin(t * num3) / num4;
			result = scale * a + scale2 * b;
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x0002EF30 File Offset: 0x0002D130
		public static Vector4d Elerp(Vector4d a, Vector4d b, float t)
		{
			a.X = Math.Pow(a.X, (double)(1f - t)) * Math.Pow(b.X, (double)t);
			a.Y = Math.Pow(a.Y, (double)(1f - t)) * Math.Pow(b.Y, (double)t);
			a.Z = Math.Pow(a.Z, (double)(1f - t)) * Math.Pow(b.Z, (double)t);
			a.W = Math.Pow(a.W, (double)(1f - t)) * Math.Pow(b.W, (double)t);
			return a;
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x0002EFE0 File Offset: 0x0002D1E0
		public static void Elerp(in Vector4d a, in Vector4d b, float t, out Vector4d result)
		{
			result.X = Math.Pow(a.X, (double)(1f - t)) * Math.Pow(b.X, (double)t);
			result.Y = Math.Pow(a.Y, (double)(1f - t)) * Math.Pow(b.Y, (double)t);
			result.Z = Math.Pow(a.Z, (double)(1f - t)) * Math.Pow(b.Z, (double)t);
			result.W = Math.Pow(a.W, (double)(1f - t)) * Math.Pow(b.W, (double)t);
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x0002F08C File Offset: 0x0002D28C
		public static Vector4d BaryCentric(Vector4d a, Vector4d b, Vector4d c, double u, double v)
		{
			Vector4d result;
			Vector4d.BaryCentric(a, b, c, u, v, out result);
			return result;
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x0002F0AC File Offset: 0x0002D2AC
		public static void BaryCentric(in Vector4d a, in Vector4d b, in Vector4d c, double u, double v, out Vector4d result)
		{
			Vector4d vector4d;
			Vector4d.Subtract(b, a, out vector4d);
			Vector4d vector4d2;
			Vector4d.Multiply(vector4d, u, out vector4d2);
			Vector4d vector4d3;
			Vector4d.Add(a, vector4d2, out vector4d3);
			Vector4d vector4d4;
			Vector4d.Subtract(c, a, out vector4d4);
			Vector4d vector4d5;
			Vector4d.Multiply(vector4d4, v, out vector4d5);
			Vector4d.Add(vector4d3, vector4d5, out result);
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x0002F0F8 File Offset: 0x0002D2F8
		public static Vector4d TransformRow(Vector4d vec, Matrix4d mat)
		{
			Vector4d result;
			Vector4d.TransformRow(vec, mat, out result);
			return result;
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x0002F114 File Offset: 0x0002D314
		public static void TransformRow(in Vector4d vec, in Matrix4d mat, out Vector4d result)
		{
			result = new Vector4d(vec.X * mat.Row0.X + vec.Y * mat.Row1.X + vec.Z * mat.Row2.X + vec.W * mat.Row3.X, vec.X * mat.Row0.Y + vec.Y * mat.Row1.Y + vec.Z * mat.Row2.Y + vec.W * mat.Row3.Y, vec.X * mat.Row0.Z + vec.Y * mat.Row1.Z + vec.Z * mat.Row2.Z + vec.W * mat.Row3.Z, vec.X * mat.Row0.W + vec.Y * mat.Row1.W + vec.Z * mat.Row2.W + vec.W * mat.Row3.W);
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x0002F258 File Offset: 0x0002D458
		public static Vector4d Transform(Vector4d vec, Quaterniond quat)
		{
			Vector4d result;
			Vector4d.Transform(vec, quat, out result);
			return result;
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x0002F274 File Offset: 0x0002D474
		public static void Transform(in Vector4d vec, in Quaterniond quat, out Vector4d result)
		{
			Quaterniond quaterniond = new Quaterniond(vec.X, vec.Y, vec.Z, vec.W);
			Quaterniond quaterniond2;
			Quaterniond.Invert(quat, out quaterniond2);
			Quaterniond quaterniond3;
			Quaterniond.Multiply(quat, quaterniond, out quaterniond3);
			Quaterniond.Multiply(quaterniond3, quaterniond2, out quaterniond);
			result.X = quaterniond.X;
			result.Y = quaterniond.Y;
			result.Z = quaterniond.Z;
			result.W = quaterniond.W;
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0002F2F0 File Offset: 0x0002D4F0
		public static Vector4d TransformColumn(Matrix4d mat, Vector4d vec)
		{
			Vector4d result;
			Vector4d.TransformColumn(mat, vec, out result);
			return result;
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0002F30C File Offset: 0x0002D50C
		public static void TransformColumn(in Matrix4d mat, in Vector4d vec, out Vector4d result)
		{
			result = new Vector4d(mat.Row0.X * vec.X + mat.Row0.Y * vec.Y + mat.Row0.Z * vec.Z + mat.Row0.W * vec.W, mat.Row1.X * vec.X + mat.Row1.Y * vec.Y + mat.Row1.Z * vec.Z + mat.Row1.W * vec.W, mat.Row2.X * vec.X + mat.Row2.Y * vec.Y + mat.Row2.Z * vec.Z + mat.Row2.W * vec.W, mat.Row3.X * vec.X + mat.Row3.Y * vec.Y + mat.Row3.Z * vec.Z + mat.Row3.W * vec.W);
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000CBD RID: 3261 RVA: 0x0002F450 File Offset: 0x0002D650
		// (set) Token: 0x06000CBE RID: 3262 RVA: 0x0002F45D File Offset: 0x0002D65D
		[XmlIgnore]
		public unsafe Vector2d Xy
		{
			get
			{
				return *Unsafe.As<Vector4d, Vector2d>(ref this);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000CBF RID: 3263 RVA: 0x0002F477 File Offset: 0x0002D677
		// (set) Token: 0x06000CC0 RID: 3264 RVA: 0x0002F48A File Offset: 0x0002D68A
		[XmlIgnore]
		public Vector2d Xz
		{
			readonly get
			{
				return new Vector2d(this.X, this.Z);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x0002F4A4 File Offset: 0x0002D6A4
		// (set) Token: 0x06000CC2 RID: 3266 RVA: 0x0002F4B7 File Offset: 0x0002D6B7
		[XmlIgnore]
		public Vector2d Xw
		{
			readonly get
			{
				return new Vector2d(this.X, this.W);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x0002F4D1 File Offset: 0x0002D6D1
		// (set) Token: 0x06000CC4 RID: 3268 RVA: 0x0002F4E4 File Offset: 0x0002D6E4
		[XmlIgnore]
		public Vector2d Yx
		{
			readonly get
			{
				return new Vector2d(this.Y, this.X);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x0002F4FE File Offset: 0x0002D6FE
		// (set) Token: 0x06000CC6 RID: 3270 RVA: 0x0002F511 File Offset: 0x0002D711
		[XmlIgnore]
		public Vector2d Yz
		{
			readonly get
			{
				return new Vector2d(this.Y, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x0002F52B File Offset: 0x0002D72B
		// (set) Token: 0x06000CC8 RID: 3272 RVA: 0x0002F53E File Offset: 0x0002D73E
		[XmlIgnore]
		public Vector2d Yw
		{
			readonly get
			{
				return new Vector2d(this.Y, this.W);
			}
			set
			{
				this.Y = value.X;
				this.W = value.Y;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x0002F558 File Offset: 0x0002D758
		// (set) Token: 0x06000CCA RID: 3274 RVA: 0x0002F56B File Offset: 0x0002D76B
		[XmlIgnore]
		public Vector2d Zx
		{
			readonly get
			{
				return new Vector2d(this.Z, this.X);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x0002F585 File Offset: 0x0002D785
		// (set) Token: 0x06000CCC RID: 3276 RVA: 0x0002F598 File Offset: 0x0002D798
		[XmlIgnore]
		public Vector2d Zy
		{
			readonly get
			{
				return new Vector2d(this.Z, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x0002F5B2 File Offset: 0x0002D7B2
		// (set) Token: 0x06000CCE RID: 3278 RVA: 0x0002F5C5 File Offset: 0x0002D7C5
		[XmlIgnore]
		public Vector2d Zw
		{
			readonly get
			{
				return new Vector2d(this.Z, this.W);
			}
			set
			{
				this.Z = value.X;
				this.W = value.Y;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x0002F5DF File Offset: 0x0002D7DF
		// (set) Token: 0x06000CD0 RID: 3280 RVA: 0x0002F5F2 File Offset: 0x0002D7F2
		[XmlIgnore]
		public Vector2d Wx
		{
			readonly get
			{
				return new Vector2d(this.W, this.X);
			}
			set
			{
				this.W = value.X;
				this.X = value.Y;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x0002F60C File Offset: 0x0002D80C
		// (set) Token: 0x06000CD2 RID: 3282 RVA: 0x0002F61F File Offset: 0x0002D81F
		[XmlIgnore]
		public Vector2d Wy
		{
			readonly get
			{
				return new Vector2d(this.W, this.Y);
			}
			set
			{
				this.W = value.X;
				this.Y = value.Y;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x0002F639 File Offset: 0x0002D839
		// (set) Token: 0x06000CD4 RID: 3284 RVA: 0x0002F64C File Offset: 0x0002D84C
		[XmlIgnore]
		public Vector2d Wz
		{
			readonly get
			{
				return new Vector2d(this.W, this.Z);
			}
			set
			{
				this.W = value.X;
				this.Z = value.Y;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x0002F666 File Offset: 0x0002D866
		// (set) Token: 0x06000CD6 RID: 3286 RVA: 0x0002F673 File Offset: 0x0002D873
		[XmlIgnore]
		public unsafe Vector3d Xyz
		{
			get
			{
				return *Unsafe.As<Vector4d, Vector3d>(ref this);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000CD7 RID: 3287 RVA: 0x0002F699 File Offset: 0x0002D899
		// (set) Token: 0x06000CD8 RID: 3288 RVA: 0x0002F6B2 File Offset: 0x0002D8B2
		[XmlIgnore]
		public Vector3d Xyw
		{
			readonly get
			{
				return new Vector3d(this.X, this.Y, this.W);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000CD9 RID: 3289 RVA: 0x0002F6D8 File Offset: 0x0002D8D8
		// (set) Token: 0x06000CDA RID: 3290 RVA: 0x0002F6F1 File Offset: 0x0002D8F1
		[XmlIgnore]
		public Vector3d Xzy
		{
			readonly get
			{
				return new Vector3d(this.X, this.Z, this.Y);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x0002F717 File Offset: 0x0002D917
		// (set) Token: 0x06000CDC RID: 3292 RVA: 0x0002F730 File Offset: 0x0002D930
		[XmlIgnore]
		public Vector3d Xzw
		{
			readonly get
			{
				return new Vector3d(this.X, this.Z, this.W);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000CDD RID: 3293 RVA: 0x0002F756 File Offset: 0x0002D956
		// (set) Token: 0x06000CDE RID: 3294 RVA: 0x0002F76F File Offset: 0x0002D96F
		[XmlIgnore]
		public Vector3d Xwy
		{
			readonly get
			{
				return new Vector3d(this.X, this.W, this.Y);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000CDF RID: 3295 RVA: 0x0002F795 File Offset: 0x0002D995
		// (set) Token: 0x06000CE0 RID: 3296 RVA: 0x0002F7AE File Offset: 0x0002D9AE
		[XmlIgnore]
		public Vector3d Xwz
		{
			readonly get
			{
				return new Vector3d(this.X, this.W, this.Z);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000CE1 RID: 3297 RVA: 0x0002F7D4 File Offset: 0x0002D9D4
		// (set) Token: 0x06000CE2 RID: 3298 RVA: 0x0002F7ED File Offset: 0x0002D9ED
		[XmlIgnore]
		public Vector3d Yxz
		{
			readonly get
			{
				return new Vector3d(this.Y, this.X, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000CE3 RID: 3299 RVA: 0x0002F813 File Offset: 0x0002DA13
		// (set) Token: 0x06000CE4 RID: 3300 RVA: 0x0002F82C File Offset: 0x0002DA2C
		[XmlIgnore]
		public Vector3d Yxw
		{
			readonly get
			{
				return new Vector3d(this.Y, this.X, this.W);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000CE5 RID: 3301 RVA: 0x0002F852 File Offset: 0x0002DA52
		// (set) Token: 0x06000CE6 RID: 3302 RVA: 0x0002F86B File Offset: 0x0002DA6B
		[XmlIgnore]
		public Vector3d Yzx
		{
			readonly get
			{
				return new Vector3d(this.Y, this.Z, this.X);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x0002F891 File Offset: 0x0002DA91
		// (set) Token: 0x06000CE8 RID: 3304 RVA: 0x0002F8AA File Offset: 0x0002DAAA
		[XmlIgnore]
		public Vector3d Yzw
		{
			readonly get
			{
				return new Vector3d(this.Y, this.Z, this.W);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x0002F8D0 File Offset: 0x0002DAD0
		// (set) Token: 0x06000CEA RID: 3306 RVA: 0x0002F8E9 File Offset: 0x0002DAE9
		[XmlIgnore]
		public Vector3d Ywx
		{
			readonly get
			{
				return new Vector3d(this.Y, this.W, this.X);
			}
			set
			{
				this.Y = value.X;
				this.W = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000CEB RID: 3307 RVA: 0x0002F90F File Offset: 0x0002DB0F
		// (set) Token: 0x06000CEC RID: 3308 RVA: 0x0002F928 File Offset: 0x0002DB28
		[XmlIgnore]
		public Vector3d Ywz
		{
			readonly get
			{
				return new Vector3d(this.Y, this.W, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.W = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000CED RID: 3309 RVA: 0x0002F94E File Offset: 0x0002DB4E
		// (set) Token: 0x06000CEE RID: 3310 RVA: 0x0002F967 File Offset: 0x0002DB67
		[XmlIgnore]
		public Vector3d Zxy
		{
			readonly get
			{
				return new Vector3d(this.Z, this.X, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000CEF RID: 3311 RVA: 0x0002F98D File Offset: 0x0002DB8D
		// (set) Token: 0x06000CF0 RID: 3312 RVA: 0x0002F9A6 File Offset: 0x0002DBA6
		[XmlIgnore]
		public Vector3d Zxw
		{
			readonly get
			{
				return new Vector3d(this.Z, this.X, this.W);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x0002F9CC File Offset: 0x0002DBCC
		// (set) Token: 0x06000CF2 RID: 3314 RVA: 0x0002F9E5 File Offset: 0x0002DBE5
		[XmlIgnore]
		public Vector3d Zyx
		{
			readonly get
			{
				return new Vector3d(this.Z, this.Y, this.X);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x0002FA0B File Offset: 0x0002DC0B
		// (set) Token: 0x06000CF4 RID: 3316 RVA: 0x0002FA24 File Offset: 0x0002DC24
		[XmlIgnore]
		public Vector3d Zyw
		{
			readonly get
			{
				return new Vector3d(this.Z, this.Y, this.W);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x0002FA4A File Offset: 0x0002DC4A
		// (set) Token: 0x06000CF6 RID: 3318 RVA: 0x0002FA63 File Offset: 0x0002DC63
		[XmlIgnore]
		public Vector3d Zwx
		{
			readonly get
			{
				return new Vector3d(this.Z, this.W, this.X);
			}
			set
			{
				this.Z = value.X;
				this.W = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x0002FA89 File Offset: 0x0002DC89
		// (set) Token: 0x06000CF8 RID: 3320 RVA: 0x0002FAA2 File Offset: 0x0002DCA2
		[XmlIgnore]
		public Vector3d Zwy
		{
			readonly get
			{
				return new Vector3d(this.Z, this.W, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.W = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x0002FAC8 File Offset: 0x0002DCC8
		// (set) Token: 0x06000CFA RID: 3322 RVA: 0x0002FAE1 File Offset: 0x0002DCE1
		[XmlIgnore]
		public Vector3d Wxy
		{
			readonly get
			{
				return new Vector3d(this.W, this.X, this.Y);
			}
			set
			{
				this.W = value.X;
				this.X = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000CFB RID: 3323 RVA: 0x0002FB07 File Offset: 0x0002DD07
		// (set) Token: 0x06000CFC RID: 3324 RVA: 0x0002FB20 File Offset: 0x0002DD20
		[XmlIgnore]
		public Vector3d Wxz
		{
			readonly get
			{
				return new Vector3d(this.W, this.X, this.Z);
			}
			set
			{
				this.W = value.X;
				this.X = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000CFD RID: 3325 RVA: 0x0002FB46 File Offset: 0x0002DD46
		// (set) Token: 0x06000CFE RID: 3326 RVA: 0x0002FB5F File Offset: 0x0002DD5F
		[XmlIgnore]
		public Vector3d Wyx
		{
			readonly get
			{
				return new Vector3d(this.W, this.Y, this.X);
			}
			set
			{
				this.W = value.X;
				this.Y = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000CFF RID: 3327 RVA: 0x0002FB85 File Offset: 0x0002DD85
		// (set) Token: 0x06000D00 RID: 3328 RVA: 0x0002FB9E File Offset: 0x0002DD9E
		[XmlIgnore]
		public Vector3d Wyz
		{
			readonly get
			{
				return new Vector3d(this.W, this.Y, this.Z);
			}
			set
			{
				this.W = value.X;
				this.Y = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000D01 RID: 3329 RVA: 0x0002FBC4 File Offset: 0x0002DDC4
		// (set) Token: 0x06000D02 RID: 3330 RVA: 0x0002FBDD File Offset: 0x0002DDDD
		[XmlIgnore]
		public Vector3d Wzx
		{
			readonly get
			{
				return new Vector3d(this.W, this.Z, this.X);
			}
			set
			{
				this.W = value.X;
				this.Z = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000D03 RID: 3331 RVA: 0x0002FC03 File Offset: 0x0002DE03
		// (set) Token: 0x06000D04 RID: 3332 RVA: 0x0002FC1C File Offset: 0x0002DE1C
		[XmlIgnore]
		public Vector3d Wzy
		{
			readonly get
			{
				return new Vector3d(this.W, this.Z, this.Y);
			}
			set
			{
				this.W = value.X;
				this.Z = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x0002FC42 File Offset: 0x0002DE42
		// (set) Token: 0x06000D06 RID: 3334 RVA: 0x0002FC61 File Offset: 0x0002DE61
		[XmlIgnore]
		public Vector4d Xywz
		{
			readonly get
			{
				return new Vector4d(this.X, this.Y, this.W, this.Z);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
				this.W = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x0002FC93 File Offset: 0x0002DE93
		// (set) Token: 0x06000D08 RID: 3336 RVA: 0x0002FCB2 File Offset: 0x0002DEB2
		[XmlIgnore]
		public Vector4d Xzyw
		{
			readonly get
			{
				return new Vector4d(this.X, this.Z, this.Y, this.W);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
				this.Y = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x0002FCE4 File Offset: 0x0002DEE4
		// (set) Token: 0x06000D0A RID: 3338 RVA: 0x0002FD03 File Offset: 0x0002DF03
		[XmlIgnore]
		public Vector4d Xzwy
		{
			readonly get
			{
				return new Vector4d(this.X, this.Z, this.W, this.Y);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
				this.W = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000D0B RID: 3339 RVA: 0x0002FD35 File Offset: 0x0002DF35
		// (set) Token: 0x06000D0C RID: 3340 RVA: 0x0002FD54 File Offset: 0x0002DF54
		[XmlIgnore]
		public Vector4d Xwyz
		{
			readonly get
			{
				return new Vector4d(this.X, this.W, this.Y, this.Z);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
				this.Y = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000D0D RID: 3341 RVA: 0x0002FD86 File Offset: 0x0002DF86
		// (set) Token: 0x06000D0E RID: 3342 RVA: 0x0002FDA5 File Offset: 0x0002DFA5
		[XmlIgnore]
		public Vector4d Xwzy
		{
			readonly get
			{
				return new Vector4d(this.X, this.W, this.Z, this.Y);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
				this.Z = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000D0F RID: 3343 RVA: 0x0002FDD7 File Offset: 0x0002DFD7
		// (set) Token: 0x06000D10 RID: 3344 RVA: 0x0002FDF6 File Offset: 0x0002DFF6
		[XmlIgnore]
		public Vector4d Yxzw
		{
			readonly get
			{
				return new Vector4d(this.Y, this.X, this.Z, this.W);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
				this.Z = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000D11 RID: 3345 RVA: 0x0002FE28 File Offset: 0x0002E028
		// (set) Token: 0x06000D12 RID: 3346 RVA: 0x0002FE47 File Offset: 0x0002E047
		[XmlIgnore]
		public Vector4d Yxwz
		{
			readonly get
			{
				return new Vector4d(this.Y, this.X, this.W, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
				this.W = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000D13 RID: 3347 RVA: 0x0002FE79 File Offset: 0x0002E079
		// (set) Token: 0x06000D14 RID: 3348 RVA: 0x0002FE98 File Offset: 0x0002E098
		[XmlIgnore]
		public Vector4d Yyzw
		{
			readonly get
			{
				return new Vector4d(this.Y, this.Y, this.Z, this.W);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
				this.Z = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x0002FECA File Offset: 0x0002E0CA
		// (set) Token: 0x06000D16 RID: 3350 RVA: 0x0002FEE9 File Offset: 0x0002E0E9
		[XmlIgnore]
		public Vector4d Yywz
		{
			readonly get
			{
				return new Vector4d(this.Y, this.Y, this.W, this.Z);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
				this.W = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000D17 RID: 3351 RVA: 0x0002FF1B File Offset: 0x0002E11B
		// (set) Token: 0x06000D18 RID: 3352 RVA: 0x0002FF3A File Offset: 0x0002E13A
		[XmlIgnore]
		public Vector4d Yzxw
		{
			readonly get
			{
				return new Vector4d(this.Y, this.Z, this.X, this.W);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
				this.X = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x0002FF6C File Offset: 0x0002E16C
		// (set) Token: 0x06000D1A RID: 3354 RVA: 0x0002FF8B File Offset: 0x0002E18B
		[XmlIgnore]
		public Vector4d Yzwx
		{
			readonly get
			{
				return new Vector4d(this.Y, this.Z, this.W, this.X);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
				this.W = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x0002FFBD File Offset: 0x0002E1BD
		// (set) Token: 0x06000D1C RID: 3356 RVA: 0x0002FFDC File Offset: 0x0002E1DC
		[XmlIgnore]
		public Vector4d Ywxz
		{
			readonly get
			{
				return new Vector4d(this.Y, this.W, this.X, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.W = value.Y;
				this.X = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000D1D RID: 3357 RVA: 0x0003000E File Offset: 0x0002E20E
		// (set) Token: 0x06000D1E RID: 3358 RVA: 0x0003002D File Offset: 0x0002E22D
		[XmlIgnore]
		public Vector4d Ywzx
		{
			readonly get
			{
				return new Vector4d(this.Y, this.W, this.Z, this.X);
			}
			set
			{
				this.Y = value.X;
				this.W = value.Y;
				this.Z = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000D1F RID: 3359 RVA: 0x0003005F File Offset: 0x0002E25F
		// (set) Token: 0x06000D20 RID: 3360 RVA: 0x0003007E File Offset: 0x0002E27E
		[XmlIgnore]
		public Vector4d Zxyw
		{
			readonly get
			{
				return new Vector4d(this.Z, this.X, this.Y, this.W);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
				this.Y = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000D21 RID: 3361 RVA: 0x000300B0 File Offset: 0x0002E2B0
		// (set) Token: 0x06000D22 RID: 3362 RVA: 0x000300CF File Offset: 0x0002E2CF
		[XmlIgnore]
		public Vector4d Zxwy
		{
			readonly get
			{
				return new Vector4d(this.Z, this.X, this.W, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
				this.W = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000D23 RID: 3363 RVA: 0x00030101 File Offset: 0x0002E301
		// (set) Token: 0x06000D24 RID: 3364 RVA: 0x00030120 File Offset: 0x0002E320
		[XmlIgnore]
		public Vector4d Zyxw
		{
			readonly get
			{
				return new Vector4d(this.Z, this.Y, this.X, this.W);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
				this.X = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000D25 RID: 3365 RVA: 0x00030152 File Offset: 0x0002E352
		// (set) Token: 0x06000D26 RID: 3366 RVA: 0x00030171 File Offset: 0x0002E371
		[XmlIgnore]
		public Vector4d Zywx
		{
			readonly get
			{
				return new Vector4d(this.Z, this.Y, this.W, this.X);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
				this.W = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000D27 RID: 3367 RVA: 0x000301A3 File Offset: 0x0002E3A3
		// (set) Token: 0x06000D28 RID: 3368 RVA: 0x000301C2 File Offset: 0x0002E3C2
		[XmlIgnore]
		public Vector4d Zwxy
		{
			readonly get
			{
				return new Vector4d(this.Z, this.W, this.X, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.W = value.Y;
				this.X = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000D29 RID: 3369 RVA: 0x000301F4 File Offset: 0x0002E3F4
		// (set) Token: 0x06000D2A RID: 3370 RVA: 0x00030213 File Offset: 0x0002E413
		[XmlIgnore]
		public Vector4d Zwyx
		{
			readonly get
			{
				return new Vector4d(this.Z, this.W, this.Y, this.X);
			}
			set
			{
				this.Z = value.X;
				this.W = value.Y;
				this.Y = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000D2B RID: 3371 RVA: 0x00030245 File Offset: 0x0002E445
		// (set) Token: 0x06000D2C RID: 3372 RVA: 0x00030264 File Offset: 0x0002E464
		[XmlIgnore]
		public Vector4d Zwzy
		{
			readonly get
			{
				return new Vector4d(this.Z, this.W, this.Z, this.Y);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
				this.Z = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000D2D RID: 3373 RVA: 0x00030296 File Offset: 0x0002E496
		// (set) Token: 0x06000D2E RID: 3374 RVA: 0x000302B5 File Offset: 0x0002E4B5
		[XmlIgnore]
		public Vector4d Wxyz
		{
			readonly get
			{
				return new Vector4d(this.W, this.X, this.Y, this.Z);
			}
			set
			{
				this.W = value.X;
				this.X = value.Y;
				this.Y = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000D2F RID: 3375 RVA: 0x000302E7 File Offset: 0x0002E4E7
		// (set) Token: 0x06000D30 RID: 3376 RVA: 0x00030306 File Offset: 0x0002E506
		[XmlIgnore]
		public Vector4d Wxzy
		{
			readonly get
			{
				return new Vector4d(this.W, this.X, this.Z, this.Y);
			}
			set
			{
				this.W = value.X;
				this.X = value.Y;
				this.Z = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000D31 RID: 3377 RVA: 0x00030338 File Offset: 0x0002E538
		// (set) Token: 0x06000D32 RID: 3378 RVA: 0x00030357 File Offset: 0x0002E557
		[XmlIgnore]
		public Vector4d Wyxz
		{
			readonly get
			{
				return new Vector4d(this.W, this.Y, this.X, this.Z);
			}
			set
			{
				this.W = value.X;
				this.Y = value.Y;
				this.X = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000D33 RID: 3379 RVA: 0x00030389 File Offset: 0x0002E589
		// (set) Token: 0x06000D34 RID: 3380 RVA: 0x000303A8 File Offset: 0x0002E5A8
		[XmlIgnore]
		public Vector4d Wyzx
		{
			readonly get
			{
				return new Vector4d(this.W, this.Y, this.Z, this.X);
			}
			set
			{
				this.W = value.X;
				this.Y = value.Y;
				this.Z = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000D35 RID: 3381 RVA: 0x000303DA File Offset: 0x0002E5DA
		// (set) Token: 0x06000D36 RID: 3382 RVA: 0x000303F9 File Offset: 0x0002E5F9
		[XmlIgnore]
		public Vector4d Wzxy
		{
			readonly get
			{
				return new Vector4d(this.W, this.Z, this.X, this.Y);
			}
			set
			{
				this.W = value.X;
				this.Z = value.Y;
				this.X = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000D37 RID: 3383 RVA: 0x0003042B File Offset: 0x0002E62B
		// (set) Token: 0x06000D38 RID: 3384 RVA: 0x0003044A File Offset: 0x0002E64A
		[XmlIgnore]
		public Vector4d Wzyx
		{
			readonly get
			{
				return new Vector4d(this.W, this.Z, this.Y, this.X);
			}
			set
			{
				this.W = value.X;
				this.Z = value.Y;
				this.Y = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000D39 RID: 3385 RVA: 0x0003047C File Offset: 0x0002E67C
		// (set) Token: 0x06000D3A RID: 3386 RVA: 0x0003049B File Offset: 0x0002E69B
		[XmlIgnore]
		public Vector4d Wzyw
		{
			readonly get
			{
				return new Vector4d(this.W, this.Z, this.Y, this.W);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
				this.Y = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x000304D0 File Offset: 0x0002E6D0
		public static Vector4d operator +(Vector4d left, Vector4d right)
		{
			left.X += right.X;
			left.Y += right.Y;
			left.Z += right.Z;
			left.W += right.W;
			return left;
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00030524 File Offset: 0x0002E724
		public static Vector4d operator -(Vector4d left, Vector4d right)
		{
			left.X -= right.X;
			left.Y -= right.Y;
			left.Z -= right.Z;
			left.W -= right.W;
			return left;
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00030576 File Offset: 0x0002E776
		public static Vector4d operator -(Vector4d vec)
		{
			vec.X = -vec.X;
			vec.Y = -vec.Y;
			vec.Z = -vec.Z;
			vec.W = -vec.W;
			return vec;
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x000305B1 File Offset: 0x0002E7B1
		public static Vector4d operator *(Vector4d vec, double scale)
		{
			vec.X *= scale;
			vec.Y *= scale;
			vec.Z *= scale;
			vec.W *= scale;
			return vec;
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x000305E4 File Offset: 0x0002E7E4
		public static Vector4d operator *(double scale, Vector4d vec)
		{
			vec.X *= scale;
			vec.Y *= scale;
			vec.Z *= scale;
			vec.W *= scale;
			return vec;
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00030618 File Offset: 0x0002E818
		public static Vector4d operator *(Vector4d vec, Vector4d scale)
		{
			vec.X *= scale.X;
			vec.Y *= scale.Y;
			vec.Z *= scale.Z;
			vec.W *= scale.W;
			return vec;
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x0003066C File Offset: 0x0002E86C
		public static Vector4d operator *(Vector4d vec, Matrix4d mat)
		{
			Vector4d result;
			Vector4d.TransformRow(vec, mat, out result);
			return result;
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x00030688 File Offset: 0x0002E888
		public static Vector4d operator *(Matrix4d mat, Vector4d vec)
		{
			Vector4d result;
			Vector4d.TransformColumn(mat, vec, out result);
			return result;
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x000306A4 File Offset: 0x0002E8A4
		public static Vector4d operator *(Quaterniond quat, Vector4d vec)
		{
			Vector4d result;
			Vector4d.Transform(vec, quat, out result);
			return result;
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x000306BD File Offset: 0x0002E8BD
		public static Vector4d operator /(Vector4d vec, double scale)
		{
			vec.X /= scale;
			vec.Y /= scale;
			vec.Z /= scale;
			vec.W /= scale;
			return vec;
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x000306F0 File Offset: 0x0002E8F0
		public static Vector4d operator /(Vector4d vec, Vector4d scale)
		{
			vec.X /= scale.X;
			vec.Y /= scale.Y;
			vec.Z /= scale.Z;
			vec.W /= scale.W;
			return vec;
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x00030742 File Offset: 0x0002E942
		public static bool operator ==(Vector4d left, Vector4d right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x0003074C File Offset: 0x0002E94C
		public static bool operator !=(Vector4d left, Vector4d right)
		{
			return !(left == right);
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x00030758 File Offset: 0x0002E958
		public unsafe static explicit operator double*(Vector4d v)
		{
			return &v.X;
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x00030762 File Offset: 0x0002E962
		public unsafe static explicit operator IntPtr(Vector4d v)
		{
			return (IntPtr)((void*)(&v.X));
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x00030771 File Offset: 0x0002E971
		public static explicit operator Vector4(Vector4d vec)
		{
			return new Vector4((float)vec.X, (float)vec.Y, (float)vec.Z, (float)vec.W);
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x00030794 File Offset: 0x0002E994
		public static explicit operator Vector4h(Vector4d vec)
		{
			return new Vector4h(new Half(vec.X), new Half(vec.Y), new Half(vec.Z), new Half(vec.W));
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x000307C7 File Offset: 0x0002E9C7
		public static explicit operator Vector4i(Vector4d vec)
		{
			return new Vector4i((int)vec.X, (int)vec.Y, (int)vec.Z, (int)vec.W);
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x000307EA File Offset: 0x0002E9EA
		public static implicit operator Vector4d([TupleElementNames(new string[]
		{
			"X",
			"Y",
			"Z",
			"W"
		})] ValueTuple<double, double, double, double> values)
		{
			return new Vector4d(values.Item1, values.Item2, values.Item3, values.Item4);
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x00030809 File Offset: 0x0002EA09
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x00030813 File Offset: 0x0002EA13
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x0003081D File Offset: 0x0002EA1D
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x00030828 File Offset: 0x0002EA28
		public readonly string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("({0}{4} {1}{4} {2}{4} {3})", new object[]
			{
				this.X.ToString(format, formatProvider),
				this.Y.ToString(format, formatProvider),
				this.Z.ToString(format, formatProvider),
				this.W.ToString(format, formatProvider),
				MathHelper.GetListSeparator(formatProvider)
			});
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0003088E File Offset: 0x0002EA8E
		public override bool Equals(object obj)
		{
			return obj is Vector4d && this.Equals((Vector4d)obj);
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x000308A6 File Offset: 0x0002EAA6
		public readonly bool Equals(Vector4d other)
		{
			return this.X == other.X && this.Y == other.Y && this.Z == other.Z && this.W == other.W;
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x000308E2 File Offset: 0x0002EAE2
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<double, double, double, double>(this.X, this.Y, this.Z, this.W);
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00030901 File Offset: 0x0002EB01
		public readonly void Deconstruct(out double x, out double y, out double z, out double w)
		{
			x = this.X;
			y = this.Y;
			z = this.Z;
			w = this.W;
		}

		// Token: 0x040000D2 RID: 210
		public double X;

		// Token: 0x040000D3 RID: 211
		public double Y;

		// Token: 0x040000D4 RID: 212
		public double Z;

		// Token: 0x040000D5 RID: 213
		public double W;

		// Token: 0x040000D6 RID: 214
		public static readonly Vector4d UnitX = new Vector4d(1.0, 0.0, 0.0, 0.0);

		// Token: 0x040000D7 RID: 215
		public static readonly Vector4d UnitY = new Vector4d(0.0, 1.0, 0.0, 0.0);

		// Token: 0x040000D8 RID: 216
		public static readonly Vector4d UnitZ = new Vector4d(0.0, 0.0, 1.0, 0.0);

		// Token: 0x040000D9 RID: 217
		public static readonly Vector4d UnitW = new Vector4d(0.0, 0.0, 0.0, 1.0);

		// Token: 0x040000DA RID: 218
		public static readonly Vector4d Zero = new Vector4d(0.0, 0.0, 0.0, 0.0);

		// Token: 0x040000DB RID: 219
		public static readonly Vector4d One = new Vector4d(1.0, 1.0, 1.0, 1.0);

		// Token: 0x040000DC RID: 220
		public static readonly Vector4d PositiveInfinity = new Vector4d(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);

		// Token: 0x040000DD RID: 221
		public static readonly Vector4d NegativeInfinity = new Vector4d(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);

		// Token: 0x040000DE RID: 222
		public static readonly int SizeInBytes = Unsafe.SizeOf<Vector4d>();
	}
}
