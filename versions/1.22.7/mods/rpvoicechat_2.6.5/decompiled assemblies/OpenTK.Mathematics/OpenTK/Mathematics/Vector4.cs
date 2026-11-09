using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace OpenTK.Mathematics
{
	// Token: 0x0200002A RID: 42
	[Serializable]
	public struct Vector4 : IEquatable<Vector4>, IFormattable
	{
		// Token: 0x06000B7C RID: 2940 RVA: 0x0002A85C File Offset: 0x00028A5C
		public Vector4(float value)
		{
			this.X = value;
			this.Y = value;
			this.Z = value;
			this.W = value;
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x0002A87A File Offset: 0x00028A7A
		public Vector4(float x, float y, float z, float w)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x0002A899 File Offset: 0x00028A99
		public Vector4(Vector2 v)
		{
			this.X = v.X;
			this.Y = v.Y;
			this.Z = 0f;
			this.W = 0f;
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x0002A8C9 File Offset: 0x00028AC9
		public Vector4(Vector3 v)
		{
			this.X = v.X;
			this.Y = v.Y;
			this.Z = v.Z;
			this.W = 0f;
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x0002A8FA File Offset: 0x00028AFA
		public Vector4(Vector3 v, float w)
		{
			this.X = v.X;
			this.Y = v.Y;
			this.Z = v.Z;
			this.W = w;
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x0002A927 File Offset: 0x00028B27
		public Vector4(Vector4 v)
		{
			this.X = v.X;
			this.Y = v.Y;
			this.Z = v.Z;
			this.W = v.W;
		}

		// Token: 0x17000221 RID: 545
		public unsafe float this[int index]
		{
			readonly get
			{
				if (index < 4)
				{
					return *this.GetElementUnsafe(this, index);
				}
				MathHelper.ThrowOutOfRangeException("You tried to access this vector at index: " + index.ToString());
				return 0f;
			}
			set
			{
				if (index < 4)
				{
					*this.GetElementUnsafe(this, index) = value;
					return;
				}
				MathHelper.ThrowOutOfRangeException("You tried to set this vector at index: " + index.ToString());
			}
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0002A9AB File Offset: 0x00028BAB
		[MethodImpl(256)]
		private readonly ref float GetElementUnsafe(in Vector4 v, int index)
		{
			return Unsafe.Add<float>(Unsafe.AsRef<float>(ref v.X), index);
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x0002A9BE File Offset: 0x00028BBE
		public readonly float Length
		{
			get
			{
				return MathF.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W);
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000B86 RID: 2950 RVA: 0x0002A9FC File Offset: 0x00028BFC
		public readonly float ReciprocalLengthFast
		{
			get
			{
				return MathHelper.InverseSqrtFast(this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W);
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000B87 RID: 2951 RVA: 0x0002AA3C File Offset: 0x00028C3C
		public readonly float LengthFast
		{
			get
			{
				return 1f / MathHelper.InverseSqrtFast(this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W);
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000B88 RID: 2952 RVA: 0x0002AA8B File Offset: 0x00028C8B
		public readonly float LengthSquared
		{
			get
			{
				return this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W;
			}
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0002AAC4 File Offset: 0x00028CC4
		public readonly Vector4 Normalized()
		{
			Vector4 result = this;
			result.Normalize();
			return result;
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0002AAE0 File Offset: 0x00028CE0
		public void Normalize()
		{
			float num = 1f / this.Length;
			this.X *= num;
			this.Y *= num;
			this.Z *= num;
			this.W *= num;
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x0002AB34 File Offset: 0x00028D34
		public void NormalizeFast()
		{
			float num = MathHelper.InverseSqrtFast(this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W);
			this.X *= num;
			this.Y *= num;
			this.Z *= num;
			this.W *= num;
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x0002ABB8 File Offset: 0x00028DB8
		public readonly Vector4 Abs()
		{
			Vector4 vector = this;
			vector.X = MathF.Abs(vector.X);
			vector.Y = MathF.Abs(vector.Y);
			vector.Z = MathF.Abs(vector.Z);
			vector.W = MathF.Abs(vector.W);
			return vector;
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x0002AC15 File Offset: 0x00028E15
		public readonly Vector4 Round()
		{
			return Vector4.Round(this);
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x0002AC22 File Offset: 0x00028E22
		public readonly Vector4 Round(MidpointRounding rounding)
		{
			return Vector4.Round(this, rounding);
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x0002AC30 File Offset: 0x00028E30
		public readonly Vector4 Ceiling()
		{
			return Vector4.Ceiling(this);
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x0002AC3D File Offset: 0x00028E3D
		public readonly Vector4 Floor()
		{
			return Vector4.Floor(this);
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x0002AC4A File Offset: 0x00028E4A
		public readonly Vector4 Truncate()
		{
			return Vector4.Truncate(this);
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x0002AC57 File Offset: 0x00028E57
		public static Vector4 Add(Vector4 a, Vector4 b)
		{
			Vector4.Add(a, b, out a);
			return a;
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x0002AC68 File Offset: 0x00028E68
		public static void Add(in Vector4 a, in Vector4 b, out Vector4 result)
		{
			result.X = a.X + b.X;
			result.Y = a.Y + b.Y;
			result.Z = a.Z + b.Z;
			result.W = a.W + b.W;
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x0002ACC1 File Offset: 0x00028EC1
		public static Vector4 Subtract(Vector4 a, Vector4 b)
		{
			Vector4.Subtract(a, b, out a);
			return a;
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x0002ACD0 File Offset: 0x00028ED0
		public static void Subtract(in Vector4 a, in Vector4 b, out Vector4 result)
		{
			result.X = a.X - b.X;
			result.Y = a.Y - b.Y;
			result.Z = a.Z - b.Z;
			result.W = a.W - b.W;
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x0002AD29 File Offset: 0x00028F29
		public static Vector4 Multiply(Vector4 vector, float scale)
		{
			Vector4.Multiply(vector, scale, out vector);
			return vector;
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x0002AD36 File Offset: 0x00028F36
		public static void Multiply(in Vector4 vector, float scale, out Vector4 result)
		{
			result.X = vector.X * scale;
			result.Y = vector.Y * scale;
			result.Z = vector.Z * scale;
			result.W = vector.W * scale;
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x0002AD70 File Offset: 0x00028F70
		public static Vector4 Multiply(Vector4 vector, Vector4 scale)
		{
			Vector4.Multiply(vector, scale, out vector);
			return vector;
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x0002AD80 File Offset: 0x00028F80
		public static void Multiply(in Vector4 vector, in Vector4 scale, out Vector4 result)
		{
			result.X = vector.X * scale.X;
			result.Y = vector.Y * scale.Y;
			result.Z = vector.Z * scale.Z;
			result.W = vector.W * scale.W;
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x0002ADD9 File Offset: 0x00028FD9
		public static Vector4 Divide(Vector4 vector, float scale)
		{
			Vector4.Divide(vector, scale, out vector);
			return vector;
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x0002ADE6 File Offset: 0x00028FE6
		public static void Divide(in Vector4 vector, float scale, out Vector4 result)
		{
			result.X = vector.X / scale;
			result.Y = vector.Y / scale;
			result.Z = vector.Z / scale;
			result.W = vector.W / scale;
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0002AE20 File Offset: 0x00029020
		public static Vector4 Divide(Vector4 vector, Vector4 scale)
		{
			Vector4.Divide(vector, scale, out vector);
			return vector;
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x0002AE30 File Offset: 0x00029030
		public static void Divide(in Vector4 vector, in Vector4 scale, out Vector4 result)
		{
			result.X = vector.X / scale.X;
			result.Y = vector.Y / scale.Y;
			result.Z = vector.Z / scale.Z;
			result.W = vector.W / scale.W;
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x0002AE8C File Offset: 0x0002908C
		public static Vector4 ComponentMin(Vector4 a, Vector4 b)
		{
			a.X = ((a.X < b.X) ? a.X : b.X);
			a.Y = ((a.Y < b.Y) ? a.Y : b.Y);
			a.Z = ((a.Z < b.Z) ? a.Z : b.Z);
			a.W = ((a.W < b.W) ? a.W : b.W);
			return a;
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x0002AF28 File Offset: 0x00029128
		public static void ComponentMin(in Vector4 a, in Vector4 b, out Vector4 result)
		{
			result.X = ((a.X < b.X) ? a.X : b.X);
			result.Y = ((a.Y < b.Y) ? a.Y : b.Y);
			result.Z = ((a.Z < b.Z) ? a.Z : b.Z);
			result.W = ((a.W < b.W) ? a.W : b.W);
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x0002AFC0 File Offset: 0x000291C0
		public static Vector4 ComponentMax(Vector4 a, Vector4 b)
		{
			a.X = ((a.X > b.X) ? a.X : b.X);
			a.Y = ((a.Y > b.Y) ? a.Y : b.Y);
			a.Z = ((a.Z > b.Z) ? a.Z : b.Z);
			a.W = ((a.W > b.W) ? a.W : b.W);
			return a;
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x0002B05C File Offset: 0x0002925C
		public static void ComponentMax(in Vector4 a, in Vector4 b, out Vector4 result)
		{
			result.X = ((a.X > b.X) ? a.X : b.X);
			result.Y = ((a.Y > b.Y) ? a.Y : b.Y);
			result.Z = ((a.Z > b.Z) ? a.Z : b.Z);
			result.W = ((a.W > b.W) ? a.W : b.W);
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x0002B0F1 File Offset: 0x000292F1
		public static Vector4 MagnitudeMin(Vector4 left, Vector4 right)
		{
			if (left.LengthSquared >= right.LengthSquared)
			{
				return right;
			}
			return left;
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x0002B106 File Offset: 0x00029306
		public static void MagnitudeMin(in Vector4 left, in Vector4 right, out Vector4 result)
		{
			result = ((left.LengthSquared < right.LengthSquared) ? left : right);
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x0002B12A File Offset: 0x0002932A
		public static Vector4 MagnitudeMax(Vector4 left, Vector4 right)
		{
			if (left.LengthSquared < right.LengthSquared)
			{
				return right;
			}
			return left;
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0002B13F File Offset: 0x0002933F
		public static void MagnitudeMax(in Vector4 left, in Vector4 right, out Vector4 result)
		{
			result = ((left.LengthSquared >= right.LengthSquared) ? left : right);
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x0002B164 File Offset: 0x00029364
		public static Vector4 Clamp(Vector4 vec, Vector4 min, Vector4 max)
		{
			vec.X = ((vec.X < min.X) ? min.X : ((vec.X > max.X) ? max.X : vec.X));
			vec.Y = ((vec.Y < min.Y) ? min.Y : ((vec.Y > max.Y) ? max.Y : vec.Y));
			vec.Z = ((vec.Z < min.Z) ? min.Z : ((vec.Z > max.Z) ? max.Z : vec.Z));
			vec.W = ((vec.W < min.W) ? min.W : ((vec.W > max.W) ? max.W : vec.W));
			return vec;
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x0002B258 File Offset: 0x00029458
		public static void Clamp(in Vector4 vec, in Vector4 min, in Vector4 max, out Vector4 result)
		{
			result.X = ((vec.X < min.X) ? min.X : ((vec.X > max.X) ? max.X : vec.X));
			result.Y = ((vec.Y < min.Y) ? min.Y : ((vec.Y > max.Y) ? max.Y : vec.Y));
			result.Z = ((vec.Z < min.Z) ? min.Z : ((vec.Z > max.Z) ? max.Z : vec.Z));
			result.W = ((vec.W < min.W) ? min.W : ((vec.W > max.W) ? max.W : vec.W));
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x0002B348 File Offset: 0x00029548
		public static Vector4 Abs(Vector4 vec)
		{
			vec.X = MathF.Abs(vec.X);
			vec.Y = MathF.Abs(vec.Y);
			vec.Z = MathF.Abs(vec.Z);
			vec.W = MathF.Abs(vec.W);
			return vec;
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x0002B3A0 File Offset: 0x000295A0
		public static void Abs(in Vector4 vec, out Vector4 result)
		{
			result.X = MathF.Abs(vec.X);
			result.Y = MathF.Abs(vec.Y);
			result.Z = MathF.Abs(vec.Z);
			result.W = MathF.Abs(vec.W);
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x0002B3F4 File Offset: 0x000295F4
		public static Vector4 Round(Vector4 vec)
		{
			vec.X = MathF.Round(vec.X);
			vec.Y = MathF.Round(vec.Y);
			vec.Z = MathF.Round(vec.Z);
			vec.W = MathF.Round(vec.W);
			return vec;
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x0002B44C File Offset: 0x0002964C
		public static void Round(in Vector4 vec, out Vector4 result)
		{
			result.X = MathF.Round(vec.X);
			result.Y = MathF.Round(vec.Y);
			result.Z = MathF.Round(vec.Z);
			result.W = MathF.Round(vec.W);
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x0002B4A0 File Offset: 0x000296A0
		public static Vector4 Round(Vector4 vec, MidpointRounding rounding)
		{
			vec.X = MathF.Round(vec.X, rounding);
			vec.Y = MathF.Round(vec.Y, rounding);
			vec.Z = MathF.Round(vec.Z, rounding);
			vec.W = MathF.Round(vec.W, rounding);
			return vec;
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x0002B4FC File Offset: 0x000296FC
		public static void Round(in Vector4 vec, MidpointRounding rounding, out Vector4 result)
		{
			result.X = MathF.Round(vec.X, rounding);
			result.Y = MathF.Round(vec.Y, rounding);
			result.Z = MathF.Round(vec.Z, rounding);
			result.W = MathF.Round(vec.W, rounding);
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x0002B554 File Offset: 0x00029754
		public static Vector4 Ceiling(Vector4 vec)
		{
			vec.X = MathF.Ceiling(vec.X);
			vec.Y = MathF.Ceiling(vec.Y);
			vec.Z = MathF.Ceiling(vec.Z);
			vec.W = MathF.Ceiling(vec.W);
			return vec;
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x0002B5AC File Offset: 0x000297AC
		public static void Ceiling(in Vector4 vec, out Vector4 result)
		{
			result.X = MathF.Ceiling(vec.X);
			result.Y = MathF.Ceiling(vec.Y);
			result.Z = MathF.Ceiling(vec.Z);
			result.W = MathF.Ceiling(vec.W);
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x0002B600 File Offset: 0x00029800
		public static Vector4 Floor(Vector4 vec)
		{
			vec.X = MathF.Floor(vec.X);
			vec.Y = MathF.Floor(vec.Y);
			vec.Z = MathF.Floor(vec.Z);
			vec.W = MathF.Floor(vec.W);
			return vec;
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x0002B658 File Offset: 0x00029858
		public static void Floor(in Vector4 vec, out Vector4 result)
		{
			result.X = MathF.Floor(vec.X);
			result.Y = MathF.Floor(vec.Y);
			result.Z = MathF.Floor(vec.Z);
			result.W = MathF.Floor(vec.W);
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x0002B6AC File Offset: 0x000298AC
		public static Vector4 Truncate(Vector4 vec)
		{
			vec.X = MathF.Truncate(vec.X);
			vec.Y = MathF.Truncate(vec.Y);
			vec.Z = MathF.Truncate(vec.Z);
			vec.W = MathF.Truncate(vec.W);
			return vec;
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x0002B704 File Offset: 0x00029904
		public static void Truncate(in Vector4 vec, out Vector4 result)
		{
			result.X = MathF.Truncate(vec.X);
			result.Y = MathF.Truncate(vec.Y);
			result.Z = MathF.Truncate(vec.Z);
			result.W = MathF.Truncate(vec.W);
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x0002B758 File Offset: 0x00029958
		public static float Distance(Vector4 vec1, Vector4 vec2)
		{
			float result;
			Vector4.Distance(vec1, vec2, out result);
			return result;
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x0002B774 File Offset: 0x00029974
		public static void Distance(in Vector4 vec1, in Vector4 vec2, out float result)
		{
			result = MathF.Sqrt((vec2.X - vec1.X) * (vec2.X - vec1.X) + (vec2.Y - vec1.Y) * (vec2.Y - vec1.Y) + (vec2.Z - vec1.Z) * (vec2.Z - vec1.Z) + (vec2.W - vec1.W) * (vec2.W - vec1.W));
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x0002B7F8 File Offset: 0x000299F8
		public static float DistanceSquared(Vector4 vec1, Vector4 vec2)
		{
			float result;
			Vector4.DistanceSquared(vec1, vec2, out result);
			return result;
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x0002B814 File Offset: 0x00029A14
		public static void DistanceSquared(in Vector4 vec1, in Vector4 vec2, out float result)
		{
			result = (vec2.X - vec1.X) * (vec2.X - vec1.X) + (vec2.Y - vec1.Y) * (vec2.Y - vec1.Y) + (vec2.Z - vec1.Z) * (vec2.Z - vec1.Z) + (vec2.W - vec1.W) * (vec2.W - vec1.W);
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x0002B894 File Offset: 0x00029A94
		public static Vector4 Normalize(Vector4 vec)
		{
			float num = 1f / vec.Length;
			vec.X *= num;
			vec.Y *= num;
			vec.Z *= num;
			vec.W *= num;
			return vec;
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x0002B8E0 File Offset: 0x00029AE0
		public static void Normalize(in Vector4 vec, out Vector4 result)
		{
			float num = 1f / vec.Length;
			result.X = vec.X * num;
			result.Y = vec.Y * num;
			result.Z = vec.Z * num;
			result.W = vec.W * num;
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0002B934 File Offset: 0x00029B34
		public static Vector4 NormalizeFast(Vector4 vec)
		{
			float num = MathHelper.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z + vec.W * vec.W);
			vec.X *= num;
			vec.Y *= num;
			vec.Z *= num;
			vec.W *= num;
			return vec;
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x0002B9B0 File Offset: 0x00029BB0
		public static void NormalizeFast(in Vector4 vec, out Vector4 result)
		{
			float num = MathHelper.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z + vec.W * vec.W);
			result.X = vec.X * num;
			result.Y = vec.Y * num;
			result.Z = vec.Z * num;
			result.W = vec.W * num;
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x0002BA32 File Offset: 0x00029C32
		public static float Dot(Vector4 left, Vector4 right)
		{
			return left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x0002BA6B File Offset: 0x00029C6B
		public static void Dot(in Vector4 left, in Vector4 right, out float result)
		{
			result = left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x0002BAA8 File Offset: 0x00029CA8
		public static Vector4 Lerp(Vector4 a, Vector4 b, float blend)
		{
			a.X = blend * (b.X - a.X) + a.X;
			a.Y = blend * (b.Y - a.Y) + a.Y;
			a.Z = blend * (b.Z - a.Z) + a.Z;
			a.W = blend * (b.W - a.W) + a.W;
			return a;
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x0002BB2C File Offset: 0x00029D2C
		public static void Lerp(in Vector4 a, in Vector4 b, float blend, out Vector4 result)
		{
			result.X = blend * (b.X - a.X) + a.X;
			result.Y = blend * (b.Y - a.Y) + a.Y;
			result.Z = blend * (b.Z - a.Z) + a.Z;
			result.W = blend * (b.W - a.W) + a.W;
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x0002BBAC File Offset: 0x00029DAC
		public static Vector4 Lerp(Vector4 a, Vector4 b, Vector4 blend)
		{
			a.X = blend.X * (b.X - a.X) + a.X;
			a.Y = blend.Y * (b.Y - a.Y) + a.Y;
			a.Z = blend.Z * (b.Z - a.Z) + a.Z;
			a.W = blend.W * (b.W - a.W) + a.W;
			return a;
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x0002BC44 File Offset: 0x00029E44
		public static void Lerp(in Vector4 a, in Vector4 b, Vector4 blend, out Vector4 result)
		{
			result.X = blend.X * (b.X - a.X) + a.X;
			result.Y = blend.Y * (b.Y - a.Y) + a.Y;
			result.Z = blend.Z * (b.Z - a.Z) + a.Z;
			result.W = blend.W * (b.W - a.W) + a.W;
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x0002BCD8 File Offset: 0x00029ED8
		public static Vector4 Slerp(Vector4 a, Vector4 b, float t)
		{
			float num = a.Length * b.Length;
			float num2;
			if (num == 0f || Math.Abs(num2 = Vector4.Dot(a, b) / num) > 0.9999f)
			{
				return Vector4.Lerp(a, b, t);
			}
			float num3 = MathF.Acos(Math.Clamp(num2, -1f, 1f));
			float num4 = MathF.Sqrt(1f - num2 * num2);
			float scale = MathF.Sin((1f - t) * num3) / num4;
			float scale2 = MathF.Sin(t * num3) / num4;
			return scale * a + scale2 * b;
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0002BD74 File Offset: 0x00029F74
		public static void Slerp(in Vector4 a, in Vector4 b, float t, out Vector4 result)
		{
			float num = a.Length * b.Length;
			if (num == 0f)
			{
				Vector4.Lerp(a, b, t, out result);
				return;
			}
			float num2;
			Vector4.Dot(a, b, out num2);
			num2 /= num;
			if (Math.Abs(num2) > 0.9999f)
			{
				Vector4.Lerp(a, b, t, out result);
				return;
			}
			float num3 = MathF.Acos(num2);
			float num4 = MathF.Sqrt(1f - num2 * num2);
			float scale = MathF.Sin((1f - t) * num3) / num4;
			float scale2 = MathF.Sin(t * num3) / num4;
			result = scale * a + scale2 * b;
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x0002BE20 File Offset: 0x0002A020
		public static Vector4 Elerp(Vector4 a, Vector4 b, float t)
		{
			a.X = MathF.Pow(a.X, 1f - t) * MathF.Pow(b.X, t);
			a.Y = MathF.Pow(a.Y, 1f - t) * MathF.Pow(b.Y, t);
			a.Z = MathF.Pow(a.Z, 1f - t) * MathF.Pow(b.Z, t);
			a.W = MathF.Pow(a.W, 1f - t) * MathF.Pow(b.W, t);
			return a;
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x0002BEC8 File Offset: 0x0002A0C8
		public static void Elerp(in Vector4 a, in Vector4 b, float t, out Vector4 result)
		{
			result.X = MathF.Pow(a.X, 1f - t) * MathF.Pow(b.X, t);
			result.Y = MathF.Pow(a.Y, 1f - t) * MathF.Pow(b.Y, t);
			result.Z = MathF.Pow(a.Z, 1f - t) * MathF.Pow(b.Z, t);
			result.W = MathF.Pow(a.W, 1f - t) * MathF.Pow(b.W, t);
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x0002BF6C File Offset: 0x0002A16C
		public static Vector4 BaryCentric(Vector4 a, Vector4 b, Vector4 c, float u, float v)
		{
			Vector4 result;
			Vector4.BaryCentric(a, b, c, u, v, out result);
			return result;
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x0002BF8C File Offset: 0x0002A18C
		public static void BaryCentric(in Vector4 a, in Vector4 b, in Vector4 c, float u, float v, out Vector4 result)
		{
			Vector4 vector;
			Vector4.Subtract(b, a, out vector);
			Vector4 vector2;
			Vector4.Multiply(vector, u, out vector2);
			Vector4 vector3;
			Vector4.Add(a, vector2, out vector3);
			Vector4 vector4;
			Vector4.Subtract(c, a, out vector4);
			Vector4 vector5;
			Vector4.Multiply(vector4, v, out vector5);
			Vector4.Add(vector3, vector5, out result);
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x0002BFD8 File Offset: 0x0002A1D8
		public static Vector4 TransformRow(Vector4 vec, Matrix4 mat)
		{
			Vector4 result;
			Vector4.TransformRow(vec, mat, out result);
			return result;
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x0002BFF4 File Offset: 0x0002A1F4
		public static void TransformRow(in Vector4 vec, in Matrix4 mat, out Vector4 result)
		{
			result = new Vector4(vec.X * mat.Row0.X + vec.Y * mat.Row1.X + vec.Z * mat.Row2.X + vec.W * mat.Row3.X, vec.X * mat.Row0.Y + vec.Y * mat.Row1.Y + vec.Z * mat.Row2.Y + vec.W * mat.Row3.Y, vec.X * mat.Row0.Z + vec.Y * mat.Row1.Z + vec.Z * mat.Row2.Z + vec.W * mat.Row3.Z, vec.X * mat.Row0.W + vec.Y * mat.Row1.W + vec.Z * mat.Row2.W + vec.W * mat.Row3.W);
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x0002C138 File Offset: 0x0002A338
		public static Vector4 Transform(Vector4 vec, Quaternion quat)
		{
			Vector4 result;
			Vector4.Transform(vec, quat, out result);
			return result;
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x0002C154 File Offset: 0x0002A354
		public static void Transform(in Vector4 vec, in Quaternion quat, out Vector4 result)
		{
			Quaternion quaternion = new Quaternion(vec.X, vec.Y, vec.Z, vec.W);
			Quaternion quaternion2;
			Quaternion.Invert(quat, out quaternion2);
			Quaternion quaternion3;
			Quaternion.Multiply(quat, quaternion, out quaternion3);
			Quaternion.Multiply(quaternion3, quaternion2, out quaternion);
			result.X = quaternion.X;
			result.Y = quaternion.Y;
			result.Z = quaternion.Z;
			result.W = quaternion.W;
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x0002C1D0 File Offset: 0x0002A3D0
		public static Vector4 TransformColumn(Matrix4 mat, Vector4 vec)
		{
			Vector4 result;
			Vector4.TransformColumn(mat, vec, out result);
			return result;
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x0002C1EC File Offset: 0x0002A3EC
		public static void TransformColumn(in Matrix4 mat, in Vector4 vec, out Vector4 result)
		{
			result = new Vector4(mat.Row0.X * vec.X + mat.Row0.Y * vec.Y + mat.Row0.Z * vec.Z + mat.Row0.W * vec.W, mat.Row1.X * vec.X + mat.Row1.Y * vec.Y + mat.Row1.Z * vec.Z + mat.Row1.W * vec.W, mat.Row2.X * vec.X + mat.Row2.Y * vec.Y + mat.Row2.Z * vec.Z + mat.Row2.W * vec.W, mat.Row3.X * vec.X + mat.Row3.Y * vec.Y + mat.Row3.Z * vec.Z + mat.Row3.W * vec.W);
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000BCE RID: 3022 RVA: 0x0002C330 File Offset: 0x0002A530
		// (set) Token: 0x06000BCF RID: 3023 RVA: 0x0002C33D File Offset: 0x0002A53D
		[XmlIgnore]
		public unsafe Vector2 Xy
		{
			get
			{
				return *Unsafe.As<Vector4, Vector2>(ref this);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000BD0 RID: 3024 RVA: 0x0002C357 File Offset: 0x0002A557
		// (set) Token: 0x06000BD1 RID: 3025 RVA: 0x0002C36A File Offset: 0x0002A56A
		[XmlIgnore]
		public Vector2 Xz
		{
			readonly get
			{
				return new Vector2(this.X, this.Z);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000BD2 RID: 3026 RVA: 0x0002C384 File Offset: 0x0002A584
		// (set) Token: 0x06000BD3 RID: 3027 RVA: 0x0002C397 File Offset: 0x0002A597
		[XmlIgnore]
		public Vector2 Xw
		{
			readonly get
			{
				return new Vector2(this.X, this.W);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000BD4 RID: 3028 RVA: 0x0002C3B1 File Offset: 0x0002A5B1
		// (set) Token: 0x06000BD5 RID: 3029 RVA: 0x0002C3C4 File Offset: 0x0002A5C4
		[XmlIgnore]
		public Vector2 Yx
		{
			readonly get
			{
				return new Vector2(this.Y, this.X);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000BD6 RID: 3030 RVA: 0x0002C3DE File Offset: 0x0002A5DE
		// (set) Token: 0x06000BD7 RID: 3031 RVA: 0x0002C3F1 File Offset: 0x0002A5F1
		[XmlIgnore]
		public Vector2 Yz
		{
			readonly get
			{
				return new Vector2(this.Y, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x0002C40B File Offset: 0x0002A60B
		// (set) Token: 0x06000BD9 RID: 3033 RVA: 0x0002C41E File Offset: 0x0002A61E
		[XmlIgnore]
		public Vector2 Yw
		{
			readonly get
			{
				return new Vector2(this.Y, this.W);
			}
			set
			{
				this.Y = value.X;
				this.W = value.Y;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000BDA RID: 3034 RVA: 0x0002C438 File Offset: 0x0002A638
		// (set) Token: 0x06000BDB RID: 3035 RVA: 0x0002C44B File Offset: 0x0002A64B
		[XmlIgnore]
		public Vector2 Zx
		{
			readonly get
			{
				return new Vector2(this.Z, this.X);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000BDC RID: 3036 RVA: 0x0002C465 File Offset: 0x0002A665
		// (set) Token: 0x06000BDD RID: 3037 RVA: 0x0002C478 File Offset: 0x0002A678
		[XmlIgnore]
		public Vector2 Zy
		{
			readonly get
			{
				return new Vector2(this.Z, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x0002C492 File Offset: 0x0002A692
		// (set) Token: 0x06000BDF RID: 3039 RVA: 0x0002C4A5 File Offset: 0x0002A6A5
		[XmlIgnore]
		public Vector2 Zw
		{
			readonly get
			{
				return new Vector2(this.Z, this.W);
			}
			set
			{
				this.Z = value.X;
				this.W = value.Y;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000BE0 RID: 3040 RVA: 0x0002C4BF File Offset: 0x0002A6BF
		// (set) Token: 0x06000BE1 RID: 3041 RVA: 0x0002C4D2 File Offset: 0x0002A6D2
		[XmlIgnore]
		public Vector2 Wx
		{
			readonly get
			{
				return new Vector2(this.W, this.X);
			}
			set
			{
				this.W = value.X;
				this.X = value.Y;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x0002C4EC File Offset: 0x0002A6EC
		// (set) Token: 0x06000BE3 RID: 3043 RVA: 0x0002C4FF File Offset: 0x0002A6FF
		[XmlIgnore]
		public Vector2 Wy
		{
			readonly get
			{
				return new Vector2(this.W, this.Y);
			}
			set
			{
				this.W = value.X;
				this.Y = value.Y;
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x0002C519 File Offset: 0x0002A719
		// (set) Token: 0x06000BE5 RID: 3045 RVA: 0x0002C52C File Offset: 0x0002A72C
		[XmlIgnore]
		public Vector2 Wz
		{
			readonly get
			{
				return new Vector2(this.W, this.Z);
			}
			set
			{
				this.W = value.X;
				this.Z = value.Y;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x0002C546 File Offset: 0x0002A746
		// (set) Token: 0x06000BE7 RID: 3047 RVA: 0x0002C553 File Offset: 0x0002A753
		[XmlIgnore]
		public unsafe Vector3 Xyz
		{
			get
			{
				return *Unsafe.As<Vector4, Vector3>(ref this);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x0002C579 File Offset: 0x0002A779
		// (set) Token: 0x06000BE9 RID: 3049 RVA: 0x0002C592 File Offset: 0x0002A792
		[XmlIgnore]
		public Vector3 Xyw
		{
			readonly get
			{
				return new Vector3(this.X, this.Y, this.W);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x0002C5B8 File Offset: 0x0002A7B8
		// (set) Token: 0x06000BEB RID: 3051 RVA: 0x0002C5D1 File Offset: 0x0002A7D1
		[XmlIgnore]
		public Vector3 Xzy
		{
			readonly get
			{
				return new Vector3(this.X, this.Z, this.Y);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000BEC RID: 3052 RVA: 0x0002C5F7 File Offset: 0x0002A7F7
		// (set) Token: 0x06000BED RID: 3053 RVA: 0x0002C610 File Offset: 0x0002A810
		[XmlIgnore]
		public Vector3 Xzw
		{
			readonly get
			{
				return new Vector3(this.X, this.Z, this.W);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000BEE RID: 3054 RVA: 0x0002C636 File Offset: 0x0002A836
		// (set) Token: 0x06000BEF RID: 3055 RVA: 0x0002C64F File Offset: 0x0002A84F
		[XmlIgnore]
		public Vector3 Xwy
		{
			readonly get
			{
				return new Vector3(this.X, this.W, this.Y);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000BF0 RID: 3056 RVA: 0x0002C675 File Offset: 0x0002A875
		// (set) Token: 0x06000BF1 RID: 3057 RVA: 0x0002C68E File Offset: 0x0002A88E
		[XmlIgnore]
		public Vector3 Xwz
		{
			readonly get
			{
				return new Vector3(this.X, this.W, this.Z);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000BF2 RID: 3058 RVA: 0x0002C6B4 File Offset: 0x0002A8B4
		// (set) Token: 0x06000BF3 RID: 3059 RVA: 0x0002C6CD File Offset: 0x0002A8CD
		[XmlIgnore]
		public Vector3 Yxz
		{
			readonly get
			{
				return new Vector3(this.Y, this.X, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000BF4 RID: 3060 RVA: 0x0002C6F3 File Offset: 0x0002A8F3
		// (set) Token: 0x06000BF5 RID: 3061 RVA: 0x0002C70C File Offset: 0x0002A90C
		[XmlIgnore]
		public Vector3 Yxw
		{
			readonly get
			{
				return new Vector3(this.Y, this.X, this.W);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x0002C732 File Offset: 0x0002A932
		// (set) Token: 0x06000BF7 RID: 3063 RVA: 0x0002C74B File Offset: 0x0002A94B
		[XmlIgnore]
		public Vector3 Yzx
		{
			readonly get
			{
				return new Vector3(this.Y, this.Z, this.X);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000BF8 RID: 3064 RVA: 0x0002C771 File Offset: 0x0002A971
		// (set) Token: 0x06000BF9 RID: 3065 RVA: 0x0002C78A File Offset: 0x0002A98A
		[XmlIgnore]
		public Vector3 Yzw
		{
			readonly get
			{
				return new Vector3(this.Y, this.Z, this.W);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000BFA RID: 3066 RVA: 0x0002C7B0 File Offset: 0x0002A9B0
		// (set) Token: 0x06000BFB RID: 3067 RVA: 0x0002C7C9 File Offset: 0x0002A9C9
		[XmlIgnore]
		public Vector3 Ywx
		{
			readonly get
			{
				return new Vector3(this.Y, this.W, this.X);
			}
			set
			{
				this.Y = value.X;
				this.W = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000BFC RID: 3068 RVA: 0x0002C7EF File Offset: 0x0002A9EF
		// (set) Token: 0x06000BFD RID: 3069 RVA: 0x0002C808 File Offset: 0x0002AA08
		[XmlIgnore]
		public Vector3 Ywz
		{
			readonly get
			{
				return new Vector3(this.Y, this.W, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.W = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000BFE RID: 3070 RVA: 0x0002C82E File Offset: 0x0002AA2E
		// (set) Token: 0x06000BFF RID: 3071 RVA: 0x0002C847 File Offset: 0x0002AA47
		[XmlIgnore]
		public Vector3 Zxy
		{
			readonly get
			{
				return new Vector3(this.Z, this.X, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000C00 RID: 3072 RVA: 0x0002C86D File Offset: 0x0002AA6D
		// (set) Token: 0x06000C01 RID: 3073 RVA: 0x0002C886 File Offset: 0x0002AA86
		[XmlIgnore]
		public Vector3 Zxw
		{
			readonly get
			{
				return new Vector3(this.Z, this.X, this.W);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000C02 RID: 3074 RVA: 0x0002C8AC File Offset: 0x0002AAAC
		// (set) Token: 0x06000C03 RID: 3075 RVA: 0x0002C8C5 File Offset: 0x0002AAC5
		[XmlIgnore]
		public Vector3 Zyx
		{
			readonly get
			{
				return new Vector3(this.Z, this.Y, this.X);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000C04 RID: 3076 RVA: 0x0002C8EB File Offset: 0x0002AAEB
		// (set) Token: 0x06000C05 RID: 3077 RVA: 0x0002C904 File Offset: 0x0002AB04
		[XmlIgnore]
		public Vector3 Zyw
		{
			readonly get
			{
				return new Vector3(this.Z, this.Y, this.W);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000C06 RID: 3078 RVA: 0x0002C92A File Offset: 0x0002AB2A
		// (set) Token: 0x06000C07 RID: 3079 RVA: 0x0002C943 File Offset: 0x0002AB43
		[XmlIgnore]
		public Vector3 Zwx
		{
			readonly get
			{
				return new Vector3(this.Z, this.W, this.X);
			}
			set
			{
				this.Z = value.X;
				this.W = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000C08 RID: 3080 RVA: 0x0002C969 File Offset: 0x0002AB69
		// (set) Token: 0x06000C09 RID: 3081 RVA: 0x0002C982 File Offset: 0x0002AB82
		[XmlIgnore]
		public Vector3 Zwy
		{
			readonly get
			{
				return new Vector3(this.Z, this.W, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.W = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000C0A RID: 3082 RVA: 0x0002C9A8 File Offset: 0x0002ABA8
		// (set) Token: 0x06000C0B RID: 3083 RVA: 0x0002C9C1 File Offset: 0x0002ABC1
		[XmlIgnore]
		public Vector3 Wxy
		{
			readonly get
			{
				return new Vector3(this.W, this.X, this.Y);
			}
			set
			{
				this.W = value.X;
				this.X = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000C0C RID: 3084 RVA: 0x0002C9E7 File Offset: 0x0002ABE7
		// (set) Token: 0x06000C0D RID: 3085 RVA: 0x0002CA00 File Offset: 0x0002AC00
		[XmlIgnore]
		public Vector3 Wxz
		{
			readonly get
			{
				return new Vector3(this.W, this.X, this.Z);
			}
			set
			{
				this.W = value.X;
				this.X = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000C0E RID: 3086 RVA: 0x0002CA26 File Offset: 0x0002AC26
		// (set) Token: 0x06000C0F RID: 3087 RVA: 0x0002CA3F File Offset: 0x0002AC3F
		[XmlIgnore]
		public Vector3 Wyx
		{
			readonly get
			{
				return new Vector3(this.W, this.Y, this.X);
			}
			set
			{
				this.W = value.X;
				this.Y = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000C10 RID: 3088 RVA: 0x0002CA65 File Offset: 0x0002AC65
		// (set) Token: 0x06000C11 RID: 3089 RVA: 0x0002CA7E File Offset: 0x0002AC7E
		[XmlIgnore]
		public Vector3 Wyz
		{
			readonly get
			{
				return new Vector3(this.W, this.Y, this.Z);
			}
			set
			{
				this.W = value.X;
				this.Y = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000C12 RID: 3090 RVA: 0x0002CAA4 File Offset: 0x0002ACA4
		// (set) Token: 0x06000C13 RID: 3091 RVA: 0x0002CABD File Offset: 0x0002ACBD
		[XmlIgnore]
		public Vector3 Wzx
		{
			readonly get
			{
				return new Vector3(this.W, this.Z, this.X);
			}
			set
			{
				this.W = value.X;
				this.Z = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000C14 RID: 3092 RVA: 0x0002CAE3 File Offset: 0x0002ACE3
		// (set) Token: 0x06000C15 RID: 3093 RVA: 0x0002CAFC File Offset: 0x0002ACFC
		[XmlIgnore]
		public Vector3 Wzy
		{
			readonly get
			{
				return new Vector3(this.W, this.Z, this.Y);
			}
			set
			{
				this.W = value.X;
				this.Z = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000C16 RID: 3094 RVA: 0x0002CB22 File Offset: 0x0002AD22
		// (set) Token: 0x06000C17 RID: 3095 RVA: 0x0002CB41 File Offset: 0x0002AD41
		[XmlIgnore]
		public Vector4 Xywz
		{
			readonly get
			{
				return new Vector4(this.X, this.Y, this.W, this.Z);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
				this.W = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000C18 RID: 3096 RVA: 0x0002CB73 File Offset: 0x0002AD73
		// (set) Token: 0x06000C19 RID: 3097 RVA: 0x0002CB92 File Offset: 0x0002AD92
		[XmlIgnore]
		public Vector4 Xzyw
		{
			readonly get
			{
				return new Vector4(this.X, this.Z, this.Y, this.W);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
				this.Y = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000C1A RID: 3098 RVA: 0x0002CBC4 File Offset: 0x0002ADC4
		// (set) Token: 0x06000C1B RID: 3099 RVA: 0x0002CBE3 File Offset: 0x0002ADE3
		[XmlIgnore]
		public Vector4 Xzwy
		{
			readonly get
			{
				return new Vector4(this.X, this.Z, this.W, this.Y);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
				this.W = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000C1C RID: 3100 RVA: 0x0002CC15 File Offset: 0x0002AE15
		// (set) Token: 0x06000C1D RID: 3101 RVA: 0x0002CC34 File Offset: 0x0002AE34
		[XmlIgnore]
		public Vector4 Xwyz
		{
			readonly get
			{
				return new Vector4(this.X, this.W, this.Y, this.Z);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
				this.Y = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000C1E RID: 3102 RVA: 0x0002CC66 File Offset: 0x0002AE66
		// (set) Token: 0x06000C1F RID: 3103 RVA: 0x0002CC85 File Offset: 0x0002AE85
		[XmlIgnore]
		public Vector4 Xwzy
		{
			readonly get
			{
				return new Vector4(this.X, this.W, this.Z, this.Y);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
				this.Z = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000C20 RID: 3104 RVA: 0x0002CCB7 File Offset: 0x0002AEB7
		// (set) Token: 0x06000C21 RID: 3105 RVA: 0x0002CCD6 File Offset: 0x0002AED6
		[XmlIgnore]
		public Vector4 Yxzw
		{
			readonly get
			{
				return new Vector4(this.Y, this.X, this.Z, this.W);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
				this.Z = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000C22 RID: 3106 RVA: 0x0002CD08 File Offset: 0x0002AF08
		// (set) Token: 0x06000C23 RID: 3107 RVA: 0x0002CD27 File Offset: 0x0002AF27
		[XmlIgnore]
		public Vector4 Yxwz
		{
			readonly get
			{
				return new Vector4(this.Y, this.X, this.W, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
				this.W = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000C24 RID: 3108 RVA: 0x0002CD59 File Offset: 0x0002AF59
		// (set) Token: 0x06000C25 RID: 3109 RVA: 0x0002CD78 File Offset: 0x0002AF78
		[XmlIgnore]
		public Vector4 Yyzw
		{
			readonly get
			{
				return new Vector4(this.Y, this.Y, this.Z, this.W);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
				this.Z = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000C26 RID: 3110 RVA: 0x0002CDAA File Offset: 0x0002AFAA
		// (set) Token: 0x06000C27 RID: 3111 RVA: 0x0002CDC9 File Offset: 0x0002AFC9
		[XmlIgnore]
		public Vector4 Yywz
		{
			readonly get
			{
				return new Vector4(this.Y, this.Y, this.W, this.Z);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
				this.W = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000C28 RID: 3112 RVA: 0x0002CDFB File Offset: 0x0002AFFB
		// (set) Token: 0x06000C29 RID: 3113 RVA: 0x0002CE1A File Offset: 0x0002B01A
		[XmlIgnore]
		public Vector4 Yzxw
		{
			readonly get
			{
				return new Vector4(this.Y, this.Z, this.X, this.W);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
				this.X = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000C2A RID: 3114 RVA: 0x0002CE4C File Offset: 0x0002B04C
		// (set) Token: 0x06000C2B RID: 3115 RVA: 0x0002CE6B File Offset: 0x0002B06B
		[XmlIgnore]
		public Vector4 Yzwx
		{
			readonly get
			{
				return new Vector4(this.Y, this.Z, this.W, this.X);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
				this.W = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000C2C RID: 3116 RVA: 0x0002CE9D File Offset: 0x0002B09D
		// (set) Token: 0x06000C2D RID: 3117 RVA: 0x0002CEBC File Offset: 0x0002B0BC
		[XmlIgnore]
		public Vector4 Ywxz
		{
			readonly get
			{
				return new Vector4(this.Y, this.W, this.X, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.W = value.Y;
				this.X = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000C2E RID: 3118 RVA: 0x0002CEEE File Offset: 0x0002B0EE
		// (set) Token: 0x06000C2F RID: 3119 RVA: 0x0002CF0D File Offset: 0x0002B10D
		[XmlIgnore]
		public Vector4 Ywzx
		{
			readonly get
			{
				return new Vector4(this.Y, this.W, this.Z, this.X);
			}
			set
			{
				this.Y = value.X;
				this.W = value.Y;
				this.Z = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x0002CF3F File Offset: 0x0002B13F
		// (set) Token: 0x06000C31 RID: 3121 RVA: 0x0002CF5E File Offset: 0x0002B15E
		[XmlIgnore]
		public Vector4 Zxyw
		{
			readonly get
			{
				return new Vector4(this.Z, this.X, this.Y, this.W);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
				this.Y = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x0002CF90 File Offset: 0x0002B190
		// (set) Token: 0x06000C33 RID: 3123 RVA: 0x0002CFAF File Offset: 0x0002B1AF
		[XmlIgnore]
		public Vector4 Zxwy
		{
			readonly get
			{
				return new Vector4(this.Z, this.X, this.W, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
				this.W = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000C34 RID: 3124 RVA: 0x0002CFE1 File Offset: 0x0002B1E1
		// (set) Token: 0x06000C35 RID: 3125 RVA: 0x0002D000 File Offset: 0x0002B200
		[XmlIgnore]
		public Vector4 Zyxw
		{
			readonly get
			{
				return new Vector4(this.Z, this.Y, this.X, this.W);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
				this.X = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000C36 RID: 3126 RVA: 0x0002D032 File Offset: 0x0002B232
		// (set) Token: 0x06000C37 RID: 3127 RVA: 0x0002D051 File Offset: 0x0002B251
		[XmlIgnore]
		public Vector4 Zywx
		{
			readonly get
			{
				return new Vector4(this.Z, this.Y, this.W, this.X);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
				this.W = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000C38 RID: 3128 RVA: 0x0002D083 File Offset: 0x0002B283
		// (set) Token: 0x06000C39 RID: 3129 RVA: 0x0002D0A2 File Offset: 0x0002B2A2
		[XmlIgnore]
		public Vector4 Zwxy
		{
			readonly get
			{
				return new Vector4(this.Z, this.W, this.X, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.W = value.Y;
				this.X = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000C3A RID: 3130 RVA: 0x0002D0D4 File Offset: 0x0002B2D4
		// (set) Token: 0x06000C3B RID: 3131 RVA: 0x0002D0F3 File Offset: 0x0002B2F3
		[XmlIgnore]
		public Vector4 Zwyx
		{
			readonly get
			{
				return new Vector4(this.Z, this.W, this.Y, this.X);
			}
			set
			{
				this.Z = value.X;
				this.W = value.Y;
				this.Y = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000C3C RID: 3132 RVA: 0x0002D125 File Offset: 0x0002B325
		// (set) Token: 0x06000C3D RID: 3133 RVA: 0x0002D144 File Offset: 0x0002B344
		[XmlIgnore]
		public Vector4 Zwzy
		{
			readonly get
			{
				return new Vector4(this.Z, this.W, this.Z, this.Y);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
				this.Z = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000C3E RID: 3134 RVA: 0x0002D176 File Offset: 0x0002B376
		// (set) Token: 0x06000C3F RID: 3135 RVA: 0x0002D195 File Offset: 0x0002B395
		[XmlIgnore]
		public Vector4 Wxyz
		{
			readonly get
			{
				return new Vector4(this.W, this.X, this.Y, this.Z);
			}
			set
			{
				this.W = value.X;
				this.X = value.Y;
				this.Y = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000C40 RID: 3136 RVA: 0x0002D1C7 File Offset: 0x0002B3C7
		// (set) Token: 0x06000C41 RID: 3137 RVA: 0x0002D1E6 File Offset: 0x0002B3E6
		[XmlIgnore]
		public Vector4 Wxzy
		{
			readonly get
			{
				return new Vector4(this.W, this.X, this.Z, this.Y);
			}
			set
			{
				this.W = value.X;
				this.X = value.Y;
				this.Z = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x0002D218 File Offset: 0x0002B418
		// (set) Token: 0x06000C43 RID: 3139 RVA: 0x0002D237 File Offset: 0x0002B437
		[XmlIgnore]
		public Vector4 Wyxz
		{
			readonly get
			{
				return new Vector4(this.W, this.Y, this.X, this.Z);
			}
			set
			{
				this.W = value.X;
				this.Y = value.Y;
				this.X = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000C44 RID: 3140 RVA: 0x0002D269 File Offset: 0x0002B469
		// (set) Token: 0x06000C45 RID: 3141 RVA: 0x0002D288 File Offset: 0x0002B488
		[XmlIgnore]
		public Vector4 Wyzx
		{
			readonly get
			{
				return new Vector4(this.W, this.Y, this.Z, this.X);
			}
			set
			{
				this.W = value.X;
				this.Y = value.Y;
				this.Z = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000C46 RID: 3142 RVA: 0x0002D2BA File Offset: 0x0002B4BA
		// (set) Token: 0x06000C47 RID: 3143 RVA: 0x0002D2D9 File Offset: 0x0002B4D9
		[XmlIgnore]
		public Vector4 Wzxy
		{
			readonly get
			{
				return new Vector4(this.W, this.Z, this.X, this.Y);
			}
			set
			{
				this.W = value.X;
				this.Z = value.Y;
				this.X = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000C48 RID: 3144 RVA: 0x0002D30B File Offset: 0x0002B50B
		// (set) Token: 0x06000C49 RID: 3145 RVA: 0x0002D32A File Offset: 0x0002B52A
		[XmlIgnore]
		public Vector4 Wzyx
		{
			readonly get
			{
				return new Vector4(this.W, this.Z, this.Y, this.X);
			}
			set
			{
				this.W = value.X;
				this.Z = value.Y;
				this.Y = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000C4A RID: 3146 RVA: 0x0002D35C File Offset: 0x0002B55C
		// (set) Token: 0x06000C4B RID: 3147 RVA: 0x0002D37B File Offset: 0x0002B57B
		[XmlIgnore]
		public Vector4 Wzyw
		{
			readonly get
			{
				return new Vector4(this.W, this.Z, this.Y, this.W);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
				this.Y = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x0002D3B0 File Offset: 0x0002B5B0
		public static Vector4 operator +(Vector4 left, Vector4 right)
		{
			left.X += right.X;
			left.Y += right.Y;
			left.Z += right.Z;
			left.W += right.W;
			return left;
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x0002D404 File Offset: 0x0002B604
		public static Vector4 operator -(Vector4 left, Vector4 right)
		{
			left.X -= right.X;
			left.Y -= right.Y;
			left.Z -= right.Z;
			left.W -= right.W;
			return left;
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x0002D456 File Offset: 0x0002B656
		public static Vector4 operator -(Vector4 vec)
		{
			vec.X = -vec.X;
			vec.Y = -vec.Y;
			vec.Z = -vec.Z;
			vec.W = -vec.W;
			return vec;
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x0002D491 File Offset: 0x0002B691
		public static Vector4 operator *(Vector4 vec, float scale)
		{
			vec.X *= scale;
			vec.Y *= scale;
			vec.Z *= scale;
			vec.W *= scale;
			return vec;
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x0002D4C4 File Offset: 0x0002B6C4
		public static Vector4 operator *(float scale, Vector4 vec)
		{
			vec.X *= scale;
			vec.Y *= scale;
			vec.Z *= scale;
			vec.W *= scale;
			return vec;
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x0002D4F8 File Offset: 0x0002B6F8
		public static Vector4 operator *(Vector4 vec, Vector4 scale)
		{
			vec.X *= scale.X;
			vec.Y *= scale.Y;
			vec.Z *= scale.Z;
			vec.W *= scale.W;
			return vec;
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x0002D54C File Offset: 0x0002B74C
		public static Vector4 operator *(Vector4 vec, Matrix4 mat)
		{
			Vector4 result;
			Vector4.TransformRow(vec, mat, out result);
			return result;
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x0002D568 File Offset: 0x0002B768
		public static Vector4 operator *(Matrix4 mat, Vector4 vec)
		{
			Vector4 result;
			Vector4.TransformColumn(mat, vec, out result);
			return result;
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x0002D584 File Offset: 0x0002B784
		public static Vector4 operator *(Quaternion quat, Vector4 vec)
		{
			Vector4 result;
			Vector4.Transform(vec, quat, out result);
			return result;
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x0002D59D File Offset: 0x0002B79D
		public static Vector4 operator /(Vector4 vec, float scale)
		{
			vec.X /= scale;
			vec.Y /= scale;
			vec.Z /= scale;
			vec.W /= scale;
			return vec;
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x0002D5D0 File Offset: 0x0002B7D0
		public static Vector4 operator /(Vector4 vec, Vector4 scale)
		{
			vec.X /= scale.X;
			vec.Y /= scale.Y;
			vec.Z /= scale.Z;
			vec.W /= scale.W;
			return vec;
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x0002D622 File Offset: 0x0002B822
		public static bool operator ==(Vector4 left, Vector4 right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x0002D62C File Offset: 0x0002B82C
		public static bool operator !=(Vector4 left, Vector4 right)
		{
			return !(left == right);
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x0002D638 File Offset: 0x0002B838
		public unsafe static explicit operator float*(Vector4 v)
		{
			return &v.X;
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x0002D642 File Offset: 0x0002B842
		public unsafe static explicit operator IntPtr(Vector4 v)
		{
			return (IntPtr)((void*)(&v.X));
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x0002D651 File Offset: 0x0002B851
		[MethodImpl(256)]
		public unsafe static explicit operator Color4(Vector4 v)
		{
			return *Unsafe.As<Vector4, Color4>(ref v);
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x0002D65F File Offset: 0x0002B85F
		public static implicit operator Vector4d(Vector4 vec)
		{
			return new Vector4d((double)vec.X, (double)vec.Y, (double)vec.Z, (double)vec.W);
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x0002D682 File Offset: 0x0002B882
		public static explicit operator Vector4h(Vector4 vec)
		{
			return new Vector4h(vec.X, vec.Y, vec.Z, vec.W);
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x0002D6A1 File Offset: 0x0002B8A1
		public static explicit operator Vector4i(Vector4 vec)
		{
			return new Vector4i((int)vec.X, (int)vec.Y, (int)vec.Z, (int)vec.W);
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x0002D6C4 File Offset: 0x0002B8C4
		public static implicit operator Vector4([TupleElementNames(new string[]
		{
			"X",
			"Y",
			"Z",
			"W"
		})] ValueTuple<float, float, float, float> values)
		{
			return new Vector4(values.Item1, values.Item2, values.Item3, values.Item4);
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x0002D6E3 File Offset: 0x0002B8E3
		public unsafe static explicit operator Vector4(Vector4 vec)
		{
			return *Unsafe.As<Vector4, Vector4>(ref vec);
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x0002D6F1 File Offset: 0x0002B8F1
		public unsafe static explicit operator Vector4(Vector4 vec)
		{
			return *Unsafe.As<Vector4, Vector4>(ref vec);
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x0002D6FF File Offset: 0x0002B8FF
		public override readonly string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x0002D709 File Offset: 0x0002B909
		public readonly string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x0002D713 File Offset: 0x0002B913
		public readonly string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x0002D720 File Offset: 0x0002B920
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

		// Token: 0x06000C66 RID: 3174 RVA: 0x0002D786 File Offset: 0x0002B986
		public override readonly bool Equals(object obj)
		{
			return obj is Vector4 && this.Equals((Vector4)obj);
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x0002D79E File Offset: 0x0002B99E
		public readonly bool Equals(Vector4 other)
		{
			return this.X == other.X && this.Y == other.Y && this.Z == other.Z && this.W == other.W;
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x0002D7DA File Offset: 0x0002B9DA
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<float, float, float, float>(this.X, this.Y, this.Z, this.W);
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x0002D7F9 File Offset: 0x0002B9F9
		public readonly void Deconstruct(out float x, out float y, out float z, out float w)
		{
			x = this.X;
			y = this.Y;
			z = this.Z;
			w = this.W;
		}

		// Token: 0x040000C5 RID: 197
		public float X;

		// Token: 0x040000C6 RID: 198
		public float Y;

		// Token: 0x040000C7 RID: 199
		public float Z;

		// Token: 0x040000C8 RID: 200
		public float W;

		// Token: 0x040000C9 RID: 201
		public static readonly Vector4 UnitX = new Vector4(1f, 0f, 0f, 0f);

		// Token: 0x040000CA RID: 202
		public static readonly Vector4 UnitY = new Vector4(0f, 1f, 0f, 0f);

		// Token: 0x040000CB RID: 203
		public static readonly Vector4 UnitZ = new Vector4(0f, 0f, 1f, 0f);

		// Token: 0x040000CC RID: 204
		public static readonly Vector4 UnitW = new Vector4(0f, 0f, 0f, 1f);

		// Token: 0x040000CD RID: 205
		public static readonly Vector4 Zero = new Vector4(0f, 0f, 0f, 0f);

		// Token: 0x040000CE RID: 206
		public static readonly Vector4 One = new Vector4(1f, 1f, 1f, 1f);

		// Token: 0x040000CF RID: 207
		public static readonly Vector4 PositiveInfinity = new Vector4(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

		// Token: 0x040000D0 RID: 208
		public static readonly Vector4 NegativeInfinity = new Vector4(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

		// Token: 0x040000D1 RID: 209
		public static readonly int SizeInBytes = Unsafe.SizeOf<Vector4>();
	}
}
