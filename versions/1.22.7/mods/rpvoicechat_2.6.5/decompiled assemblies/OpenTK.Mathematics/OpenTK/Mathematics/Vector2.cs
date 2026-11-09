using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace OpenTK.Mathematics
{
	// Token: 0x02000022 RID: 34
	[Serializable]
	public struct Vector2 : IEquatable<Vector2>, IFormattable
	{
		// Token: 0x06000892 RID: 2194 RVA: 0x0002228D File Offset: 0x0002048D
		public Vector2(float value)
		{
			this.X = value;
			this.Y = value;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0002229D File Offset: 0x0002049D
		public Vector2(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x170001CF RID: 463
		public unsafe float this[int index]
		{
			readonly get
			{
				if (index < 2)
				{
					return *this.GetElementUnsafe(this, index);
				}
				throw new IndexOutOfRangeException("You tried to access this vector at index: " + index.ToString());
			}
			set
			{
				if (index < 2)
				{
					*this.GetElementUnsafe(this, index) = value;
					return;
				}
				throw new IndexOutOfRangeException("You tried to set this vector at index: " + index.ToString());
			}
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x000222FA File Offset: 0x000204FA
		[MethodImpl(256)]
		private readonly ref float GetElementUnsafe(in Vector2 v, int index)
		{
			return Unsafe.Add<float>(Unsafe.AsRef<float>(ref v.X), index);
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x0002230D File Offset: 0x0002050D
		public readonly float Length
		{
			get
			{
				return MathF.Sqrt(this.X * this.X + this.Y * this.Y);
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x0002232F File Offset: 0x0002052F
		public readonly float ReciprocalLengthFast
		{
			get
			{
				return MathHelper.InverseSqrtFast(this.X * this.X + this.Y * this.Y);
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x00022351 File Offset: 0x00020551
		public readonly float LengthFast
		{
			get
			{
				return 1f / MathHelper.InverseSqrtFast(this.X * this.X + this.Y * this.Y);
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x00022379 File Offset: 0x00020579
		public readonly float LengthSquared
		{
			get
			{
				return this.X * this.X + this.Y * this.Y;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600089B RID: 2203 RVA: 0x00022396 File Offset: 0x00020596
		public readonly Vector2 PerpendicularRight
		{
			get
			{
				return new Vector2(this.Y, -this.X);
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x000223AA File Offset: 0x000205AA
		public readonly Vector2 PerpendicularLeft
		{
			get
			{
				return new Vector2(-this.Y, this.X);
			}
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x000223C0 File Offset: 0x000205C0
		public readonly Vector2 Normalized()
		{
			Vector2 result = this;
			result.Normalize();
			return result;
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x000223DC File Offset: 0x000205DC
		public void Normalize()
		{
			float num = 1f / this.Length;
			this.X *= num;
			this.Y *= num;
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x00022414 File Offset: 0x00020614
		public void NormalizeFast()
		{
			float num = MathHelper.InverseSqrtFast(this.X * this.X + this.Y * this.Y);
			this.X *= num;
			this.Y *= num;
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00022460 File Offset: 0x00020660
		public readonly Vector2 Abs()
		{
			Vector2 vector = this;
			vector.X = MathF.Abs(vector.X);
			vector.Y = MathF.Abs(vector.Y);
			return vector;
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x00022499 File Offset: 0x00020699
		public readonly Vector2 Round()
		{
			return Vector2.Round(this);
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x000224A6 File Offset: 0x000206A6
		public readonly Vector2 Round(MidpointRounding rounding)
		{
			return Vector2.Round(this, rounding);
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x000224B4 File Offset: 0x000206B4
		public readonly Vector2 Ceiling()
		{
			return Vector2.Ceiling(this);
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x000224C1 File Offset: 0x000206C1
		public readonly Vector2 Floor()
		{
			return Vector2.Floor(this);
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x000224CE File Offset: 0x000206CE
		public readonly Vector2 Truncate()
		{
			return Vector2.Truncate(this);
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x000224DB File Offset: 0x000206DB
		public static Vector2 Add(Vector2 a, Vector2 b)
		{
			Vector2.Add(a, b, out a);
			return a;
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x000224E9 File Offset: 0x000206E9
		public static void Add(in Vector2 a, in Vector2 b, out Vector2 result)
		{
			result.X = a.X + b.X;
			result.Y = a.Y + b.Y;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x00022511 File Offset: 0x00020711
		public static Vector2 Subtract(Vector2 a, Vector2 b)
		{
			Vector2.Subtract(a, b, out a);
			return a;
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0002251F File Offset: 0x0002071F
		public static void Subtract(in Vector2 a, in Vector2 b, out Vector2 result)
		{
			result.X = a.X - b.X;
			result.Y = a.Y - b.Y;
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x00022547 File Offset: 0x00020747
		public static Vector2 Multiply(Vector2 vector, float scale)
		{
			Vector2.Multiply(vector, scale, out vector);
			return vector;
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x00022554 File Offset: 0x00020754
		public static void Multiply(in Vector2 vector, float scale, out Vector2 result)
		{
			result.X = vector.X * scale;
			result.Y = vector.Y * scale;
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00022572 File Offset: 0x00020772
		public static Vector2 Multiply(Vector2 vector, Vector2 scale)
		{
			Vector2.Multiply(vector, scale, out vector);
			return vector;
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00022580 File Offset: 0x00020780
		public static void Multiply(in Vector2 vector, in Vector2 scale, out Vector2 result)
		{
			result.X = vector.X * scale.X;
			result.Y = vector.Y * scale.Y;
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x000225A8 File Offset: 0x000207A8
		public static Vector2 Divide(Vector2 vector, float scale)
		{
			Vector2.Divide(vector, scale, out vector);
			return vector;
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x000225B5 File Offset: 0x000207B5
		public static void Divide(in Vector2 vector, float scale, out Vector2 result)
		{
			result.X = vector.X / scale;
			result.Y = vector.Y / scale;
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x000225D3 File Offset: 0x000207D3
		public static Vector2 Divide(Vector2 vector, Vector2 scale)
		{
			Vector2.Divide(vector, scale, out vector);
			return vector;
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x000225E1 File Offset: 0x000207E1
		public static void Divide(in Vector2 vector, in Vector2 scale, out Vector2 result)
		{
			result.X = vector.X / scale.X;
			result.Y = vector.Y / scale.Y;
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0002260C File Offset: 0x0002080C
		public static Vector2 ComponentMin(Vector2 a, Vector2 b)
		{
			a.X = ((a.X < b.X) ? a.X : b.X);
			a.Y = ((a.Y < b.Y) ? a.Y : b.Y);
			return a;
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x00022660 File Offset: 0x00020860
		public static void ComponentMin(in Vector2 a, in Vector2 b, out Vector2 result)
		{
			result.X = ((a.X < b.X) ? a.X : b.X);
			result.Y = ((a.Y < b.Y) ? a.Y : b.Y);
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x000226B4 File Offset: 0x000208B4
		public static Vector2 ComponentMax(Vector2 a, Vector2 b)
		{
			a.X = ((a.X > b.X) ? a.X : b.X);
			a.Y = ((a.Y > b.Y) ? a.Y : b.Y);
			return a;
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x00022708 File Offset: 0x00020908
		public static void ComponentMax(in Vector2 a, in Vector2 b, out Vector2 result)
		{
			result.X = ((a.X > b.X) ? a.X : b.X);
			result.Y = ((a.Y > b.Y) ? a.Y : b.Y);
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00022759 File Offset: 0x00020959
		public static Vector2 MagnitudeMin(Vector2 left, Vector2 right)
		{
			if (left.LengthSquared >= right.LengthSquared)
			{
				return right;
			}
			return left;
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0002276E File Offset: 0x0002096E
		public static void MagnitudeMin(in Vector2 left, in Vector2 right, out Vector2 result)
		{
			result = ((left.LengthSquared < right.LengthSquared) ? left : right);
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x00022792 File Offset: 0x00020992
		public static Vector2 MagnitudeMax(Vector2 left, Vector2 right)
		{
			if (left.LengthSquared < right.LengthSquared)
			{
				return right;
			}
			return left;
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x000227A7 File Offset: 0x000209A7
		public static void MagnitudeMax(in Vector2 left, in Vector2 right, out Vector2 result)
		{
			result = ((left.LengthSquared >= right.LengthSquared) ? left : right);
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x000227CC File Offset: 0x000209CC
		public static Vector2 Clamp(Vector2 vec, Vector2 min, Vector2 max)
		{
			vec.X = ((vec.X < min.X) ? min.X : ((vec.X > max.X) ? max.X : vec.X));
			vec.Y = ((vec.Y < min.Y) ? min.Y : ((vec.Y > max.Y) ? max.Y : vec.Y));
			return vec;
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0002284C File Offset: 0x00020A4C
		public static void Clamp(in Vector2 vec, in Vector2 min, in Vector2 max, out Vector2 result)
		{
			result.X = ((vec.X < min.X) ? min.X : ((vec.X > max.X) ? max.X : vec.X));
			result.Y = ((vec.Y < min.Y) ? min.Y : ((vec.Y > max.Y) ? max.Y : vec.Y));
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x000228C9 File Offset: 0x00020AC9
		public static Vector2 Abs(Vector2 vec)
		{
			vec.X = MathF.Abs(vec.X);
			vec.Y = MathF.Abs(vec.Y);
			return vec;
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x000228F0 File Offset: 0x00020AF0
		public static void Abs(in Vector2 vec, out Vector2 result)
		{
			result.X = MathF.Abs(vec.X);
			result.Y = MathF.Abs(vec.Y);
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x00022914 File Offset: 0x00020B14
		public static Vector2 Round(Vector2 vec)
		{
			vec.X = MathF.Round(vec.X);
			vec.Y = MathF.Round(vec.Y);
			return vec;
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x0002293B File Offset: 0x00020B3B
		public static void Round(in Vector2 vec, out Vector2 result)
		{
			result.X = MathF.Round(vec.X);
			result.Y = MathF.Round(vec.Y);
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x0002295F File Offset: 0x00020B5F
		public static Vector2 Round(Vector2 vec, MidpointRounding rounding)
		{
			vec.X = MathF.Round(vec.X, rounding);
			vec.Y = MathF.Round(vec.Y, rounding);
			return vec;
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x00022988 File Offset: 0x00020B88
		public static void Round(in Vector2 vec, MidpointRounding rounding, out Vector2 result)
		{
			result.X = MathF.Round(vec.X, rounding);
			result.Y = MathF.Round(vec.Y, rounding);
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x000229AE File Offset: 0x00020BAE
		public static Vector2 Ceiling(Vector2 vec)
		{
			vec.X = MathF.Ceiling(vec.X);
			vec.Y = MathF.Ceiling(vec.Y);
			return vec;
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x000229D5 File Offset: 0x00020BD5
		public static void Ceiling(in Vector2 vec, out Vector2 result)
		{
			result.X = MathF.Ceiling(vec.X);
			result.Y = MathF.Ceiling(vec.Y);
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x000229F9 File Offset: 0x00020BF9
		public static Vector2 Floor(Vector2 vec)
		{
			vec.X = MathF.Floor(vec.X);
			vec.Y = MathF.Floor(vec.Y);
			return vec;
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x00022A20 File Offset: 0x00020C20
		public static void Floor(in Vector2 vec, out Vector2 result)
		{
			result.X = MathF.Floor(vec.X);
			result.Y = MathF.Floor(vec.Y);
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x00022A44 File Offset: 0x00020C44
		public static Vector2 Truncate(Vector2 vec)
		{
			vec.X = MathF.Truncate(vec.X);
			vec.Y = MathF.Truncate(vec.Y);
			return vec;
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x00022A6B File Offset: 0x00020C6B
		public static void Truncate(in Vector2 vec, out Vector2 result)
		{
			result.X = MathF.Truncate(vec.X);
			result.Y = MathF.Truncate(vec.Y);
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00022A90 File Offset: 0x00020C90
		public static float Distance(Vector2 vec1, Vector2 vec2)
		{
			float result;
			Vector2.Distance(vec1, vec2, out result);
			return result;
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x00022AA9 File Offset: 0x00020CA9
		public static void Distance(in Vector2 vec1, in Vector2 vec2, out float result)
		{
			result = MathF.Sqrt((vec2.X - vec1.X) * (vec2.X - vec1.X) + (vec2.Y - vec1.Y) * (vec2.Y - vec1.Y));
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x00022AEC File Offset: 0x00020CEC
		public static float DistanceSquared(Vector2 vec1, Vector2 vec2)
		{
			float result;
			Vector2.DistanceSquared(vec1, vec2, out result);
			return result;
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x00022B05 File Offset: 0x00020D05
		public static void DistanceSquared(in Vector2 vec1, in Vector2 vec2, out float result)
		{
			result = (vec2.X - vec1.X) * (vec2.X - vec1.X) + (vec2.Y - vec1.Y) * (vec2.Y - vec1.Y);
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x00022B40 File Offset: 0x00020D40
		public static Vector2 Normalize(Vector2 vec)
		{
			float num = 1f / vec.Length;
			vec.X *= num;
			vec.Y *= num;
			return vec;
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x00022B74 File Offset: 0x00020D74
		public static void Normalize(in Vector2 vec, out Vector2 result)
		{
			float num = 1f / vec.Length;
			result.X = vec.X * num;
			result.Y = vec.Y * num;
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x00022BAC File Offset: 0x00020DAC
		public static Vector2 NormalizeFast(Vector2 vec)
		{
			float num = MathHelper.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y);
			vec.X *= num;
			vec.Y *= num;
			return vec;
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x00022BF4 File Offset: 0x00020DF4
		public static void NormalizeFast(in Vector2 vec, out Vector2 result)
		{
			float num = MathHelper.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y);
			result.X = vec.X * num;
			result.Y = vec.Y * num;
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x00022C3E File Offset: 0x00020E3E
		public static float Dot(Vector2 left, Vector2 right)
		{
			return left.X * right.X + left.Y * right.Y;
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x00022C5B File Offset: 0x00020E5B
		public static void Dot(in Vector2 left, in Vector2 right, out float result)
		{
			result = left.X * right.X + left.Y * right.Y;
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x00022C7A File Offset: 0x00020E7A
		public static float PerpDot(Vector2 left, Vector2 right)
		{
			return left.X * right.Y - left.Y * right.X;
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x00022C97 File Offset: 0x00020E97
		public static void PerpDot(in Vector2 left, in Vector2 right, out float result)
		{
			result = left.X * right.Y - left.Y * right.X;
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x00022CB6 File Offset: 0x00020EB6
		public static Vector2 Lerp(Vector2 a, Vector2 b, float blend)
		{
			a.X = blend * (b.X - a.X) + a.X;
			a.Y = blend * (b.Y - a.Y) + a.Y;
			return a;
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x00022CF3 File Offset: 0x00020EF3
		public static void Lerp(in Vector2 a, in Vector2 b, float blend, out Vector2 result)
		{
			result.X = blend * (b.X - a.X) + a.X;
			result.Y = blend * (b.Y - a.Y) + a.Y;
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x00022D30 File Offset: 0x00020F30
		public static Vector2 Lerp(Vector2 a, Vector2 b, Vector2 blend)
		{
			a.X = blend.X * (b.X - a.X) + a.X;
			a.Y = blend.Y * (b.Y - a.Y) + a.Y;
			return a;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x00022D84 File Offset: 0x00020F84
		public static void Lerp(in Vector2 a, in Vector2 b, Vector2 blend, out Vector2 result)
		{
			result.X = blend.X * (b.X - a.X) + a.X;
			result.Y = blend.Y * (b.Y - a.Y) + a.Y;
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x00022DD4 File Offset: 0x00020FD4
		public static Vector2 Slerp(Vector2 a, Vector2 b, float t)
		{
			float num = a.Length * b.Length;
			float num2;
			if (num == 0f || Math.Abs(num2 = Vector2.Dot(a, b) / num) > 0.9999f)
			{
				return Vector2.Lerp(a, b, t);
			}
			float num3 = MathF.Acos(Math.Clamp(num2, -1f, 1f));
			float num4 = MathF.Sqrt(1f - num2 * num2);
			float scale = MathF.Sin((1f - t) * num3) / num4;
			float scale2 = MathF.Sin(t * num3) / num4;
			return scale * a + scale2 * b;
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x00022E70 File Offset: 0x00021070
		public static void Slerp(in Vector2 a, in Vector2 b, float t, out Vector2 result)
		{
			float num = a.Length * b.Length;
			if (num == 0f)
			{
				Vector2.Lerp(a, b, t, out result);
				return;
			}
			float num2;
			Vector2.Dot(a, b, out num2);
			num2 /= num;
			if (Math.Abs(num2) > 0.9999f)
			{
				Vector2.Lerp(a, b, t, out result);
				return;
			}
			float num3 = MathF.Acos(num2);
			float num4 = MathF.Sqrt(1f - num2 * num2);
			float scale = MathF.Sin((1f - t) * num3) / num4;
			float scale2 = MathF.Sin(t * num3) / num4;
			result = scale * a + scale2 * b;
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x00022F1C File Offset: 0x0002111C
		public static Vector2 Elerp(Vector2 a, Vector2 b, float t)
		{
			a.X = MathF.Pow(a.X, 1f - t) * MathF.Pow(b.X, t);
			a.Y = MathF.Pow(a.Y, 1f - t) * MathF.Pow(b.Y, t);
			return a;
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x00022F78 File Offset: 0x00021178
		public static void Elerp(in Vector2 a, in Vector2 b, float t, out Vector2 result)
		{
			result.X = MathF.Pow(a.X, 1f - t) * MathF.Pow(b.X, t);
			result.Y = MathF.Pow(a.Y, 1f - t) * MathF.Pow(b.Y, t);
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x00022FD0 File Offset: 0x000211D0
		public static Vector2 BaryCentric(Vector2 a, Vector2 b, Vector2 c, float u, float v)
		{
			Vector2 result;
			Vector2.BaryCentric(a, b, c, u, v, out result);
			return result;
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x00022FF0 File Offset: 0x000211F0
		public static void BaryCentric(in Vector2 a, in Vector2 b, in Vector2 c, float u, float v, out Vector2 result)
		{
			Vector2 vector;
			Vector2.Subtract(b, a, out vector);
			Vector2 vector2;
			Vector2.Multiply(vector, u, out vector2);
			Vector2 vector3;
			Vector2.Add(a, vector2, out vector3);
			Vector2 vector4;
			Vector2.Subtract(c, a, out vector4);
			Vector2 vector5;
			Vector2.Multiply(vector4, v, out vector5);
			Vector2.Add(vector3, vector5, out result);
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0002303C File Offset: 0x0002123C
		public static Vector2 TransformRow(Vector2 vec, Matrix2 mat)
		{
			Vector2 result;
			Vector2.TransformRow(vec, mat, out result);
			return result;
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x00023058 File Offset: 0x00021258
		public static void TransformRow(in Vector2 vec, in Matrix2 mat, out Vector2 result)
		{
			result = new Vector2(vec.X * mat.Row0.X + vec.Y * mat.Row1.X, vec.X * mat.Row0.Y + vec.Y * mat.Row1.Y);
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x000230BC File Offset: 0x000212BC
		public static Vector2 Transform(Vector2 vec, Quaternion quat)
		{
			Vector2 result;
			Vector2.Transform(vec, quat, out result);
			return result;
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x000230D8 File Offset: 0x000212D8
		public static void Transform(in Vector2 vec, in Quaternion quat, out Vector2 result)
		{
			Quaternion quaternion = new Quaternion(vec.X, vec.Y, 0f, 0f);
			Quaternion quaternion2;
			Quaternion.Invert(quat, out quaternion2);
			Quaternion quaternion3;
			Quaternion.Multiply(quat, quaternion, out quaternion3);
			Quaternion.Multiply(quaternion3, quaternion2, out quaternion);
			result.X = quaternion.X;
			result.Y = quaternion.Y;
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0002313C File Offset: 0x0002133C
		public static Vector2 TransformColumn(Matrix2 mat, Vector2 vec)
		{
			Vector2 result;
			Vector2.TransformColumn(mat, vec, out result);
			return result;
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x00023158 File Offset: 0x00021358
		public static void TransformColumn(in Matrix2 mat, in Vector2 vec, out Vector2 result)
		{
			result.X = mat.Row0.X * vec.X + mat.Row0.Y * vec.Y;
			result.Y = mat.Row1.X * vec.X + mat.Row1.Y * vec.Y;
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060008E4 RID: 2276 RVA: 0x000231BB File Offset: 0x000213BB
		// (set) Token: 0x060008E5 RID: 2277 RVA: 0x000231CE File Offset: 0x000213CE
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

		// Token: 0x060008E6 RID: 2278 RVA: 0x000231E8 File Offset: 0x000213E8
		public static Vector2 operator +(Vector2 left, Vector2 right)
		{
			left.X += right.X;
			left.Y += right.Y;
			return left;
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0002320D File Offset: 0x0002140D
		public static Vector2 operator -(Vector2 left, Vector2 right)
		{
			left.X -= right.X;
			left.Y -= right.Y;
			return left;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00023232 File Offset: 0x00021432
		public static Vector2 operator -(Vector2 vec)
		{
			vec.X = -vec.X;
			vec.Y = -vec.Y;
			return vec;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00023251 File Offset: 0x00021451
		public static Vector2 operator *(Vector2 vec, float scale)
		{
			vec.X *= scale;
			vec.Y *= scale;
			return vec;
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0002326C File Offset: 0x0002146C
		public static Vector2 operator *(float scale, Vector2 vec)
		{
			vec.X *= scale;
			vec.Y *= scale;
			return vec;
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x00023287 File Offset: 0x00021487
		public static Vector2 operator *(Vector2 vec, Vector2 scale)
		{
			vec.X *= scale.X;
			vec.Y *= scale.Y;
			return vec;
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x000232AC File Offset: 0x000214AC
		public static Vector2 operator *(Vector2 vec, Matrix2 mat)
		{
			Vector2 result;
			Vector2.TransformRow(vec, mat, out result);
			return result;
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x000232C8 File Offset: 0x000214C8
		public static Vector2 operator *(Matrix2 mat, Vector2 vec)
		{
			Vector2 result;
			Vector2.TransformColumn(mat, vec, out result);
			return result;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x000232E4 File Offset: 0x000214E4
		public static Vector2 operator *(Quaternion quat, Vector2 vec)
		{
			Vector2 result;
			Vector2.Transform(vec, quat, out result);
			return result;
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x000232FD File Offset: 0x000214FD
		public static Vector2 operator /(Vector2 vec, float scale)
		{
			vec.X /= scale;
			vec.Y /= scale;
			return vec;
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x00023318 File Offset: 0x00021518
		public static Vector2 operator /(Vector2 vec, Vector2 scale)
		{
			vec.X /= scale.X;
			vec.Y /= scale.Y;
			return vec;
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x0002333D File Offset: 0x0002153D
		public static bool operator ==(Vector2 left, Vector2 right)
		{
			return left.Equals(right);
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x00023347 File Offset: 0x00021547
		public static bool operator !=(Vector2 left, Vector2 right)
		{
			return !(left == right);
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x00023353 File Offset: 0x00021553
		public static implicit operator Vector2([TupleElementNames(new string[]
		{
			"X",
			"Y"
		})] ValueTuple<float, float> values)
		{
			return new Vector2(values.Item1, values.Item2);
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x00023366 File Offset: 0x00021566
		public static implicit operator Vector2d(Vector2 vec)
		{
			return new Vector2d((double)vec.X, (double)vec.Y);
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0002337B File Offset: 0x0002157B
		public static explicit operator Vector2h(Vector2 vec)
		{
			return new Vector2h(vec.X, vec.Y);
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0002338E File Offset: 0x0002158E
		public static explicit operator Vector2i(Vector2 vec)
		{
			return new Vector2i((int)vec.X, (int)vec.Y);
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x000233A3 File Offset: 0x000215A3
		public static explicit operator PointF(Vector2 vec)
		{
			return new PointF(vec.X, vec.Y);
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x000233B6 File Offset: 0x000215B6
		public static explicit operator SizeF(Vector2 vec)
		{
			return new SizeF(vec.X, vec.Y);
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x000233C9 File Offset: 0x000215C9
		public unsafe static explicit operator Vector2(Vector2 vec)
		{
			return *Unsafe.As<Vector2, Vector2>(ref vec);
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x000233D7 File Offset: 0x000215D7
		public unsafe static explicit operator Vector2(Vector2 vec)
		{
			return *Unsafe.As<Vector2, Vector2>(ref vec);
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x000233E5 File Offset: 0x000215E5
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x000233EF File Offset: 0x000215EF
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x000233F9 File Offset: 0x000215F9
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x00023403 File Offset: 0x00021603
		public readonly string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("({0}{2} {1})", this.X.ToString(format, formatProvider), this.Y.ToString(format, formatProvider), MathHelper.GetListSeparator(formatProvider));
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0002342F File Offset: 0x0002162F
		public override bool Equals(object obj)
		{
			return obj is Vector2 && this.Equals((Vector2)obj);
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x00023447 File Offset: 0x00021647
		public readonly bool Equals(Vector2 other)
		{
			return this.X == other.X && this.Y == other.Y;
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x00023467 File Offset: 0x00021667
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<float, float>(this.X, this.Y);
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x0002347A File Offset: 0x0002167A
		public readonly void Deconstruct(out float x, out float y)
		{
			x = this.X;
			y = this.Y;
		}

		// Token: 0x04000086 RID: 134
		public float X;

		// Token: 0x04000087 RID: 135
		public float Y;

		// Token: 0x04000088 RID: 136
		public static readonly Vector2 UnitX = new Vector2(1f, 0f);

		// Token: 0x04000089 RID: 137
		public static readonly Vector2 UnitY = new Vector2(0f, 1f);

		// Token: 0x0400008A RID: 138
		public static readonly Vector2 Zero = new Vector2(0f, 0f);

		// Token: 0x0400008B RID: 139
		public static readonly Vector2 One = new Vector2(1f, 1f);

		// Token: 0x0400008C RID: 140
		public static readonly Vector2 PositiveInfinity = new Vector2(float.PositiveInfinity, float.PositiveInfinity);

		// Token: 0x0400008D RID: 141
		public static readonly Vector2 NegativeInfinity = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

		// Token: 0x0400008E RID: 142
		public static readonly int SizeInBytes = Unsafe.SizeOf<Vector2>();
	}
}
