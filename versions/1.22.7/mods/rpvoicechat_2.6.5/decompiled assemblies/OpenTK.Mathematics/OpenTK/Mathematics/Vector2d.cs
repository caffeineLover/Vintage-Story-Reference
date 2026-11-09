using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace OpenTK.Mathematics
{
	// Token: 0x02000023 RID: 35
	[Serializable]
	public struct Vector2d : IEquatable<Vector2d>, IFormattable
	{
		// Token: 0x06000904 RID: 2308 RVA: 0x0002351B File Offset: 0x0002171B
		public Vector2d(double value)
		{
			this.X = value;
			this.Y = value;
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0002352B File Offset: 0x0002172B
		public Vector2d(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x170001D7 RID: 471
		public unsafe double this[int index]
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

		// Token: 0x06000908 RID: 2312 RVA: 0x00023588 File Offset: 0x00021788
		[MethodImpl(256)]
		private readonly ref double GetElementUnsafe(in Vector2d v, int index)
		{
			return Unsafe.Add<double>(Unsafe.AsRef<double>(ref v.X), index);
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000909 RID: 2313 RVA: 0x0002359B File Offset: 0x0002179B
		public readonly double Length
		{
			get
			{
				return Math.Sqrt(this.X * this.X + this.Y * this.Y);
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600090A RID: 2314 RVA: 0x000235BD File Offset: 0x000217BD
		public readonly double ReciprocalLengthFast
		{
			get
			{
				return MathHelper.InverseSqrtFast(this.X * this.X + this.Y * this.Y);
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600090B RID: 2315 RVA: 0x000235DF File Offset: 0x000217DF
		public readonly double LengthFast
		{
			get
			{
				return 1.0 / MathHelper.InverseSqrtFast(this.X * this.X + this.Y * this.Y);
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600090C RID: 2316 RVA: 0x0002360B File Offset: 0x0002180B
		public readonly double LengthSquared
		{
			get
			{
				return this.X * this.X + this.Y * this.Y;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600090D RID: 2317 RVA: 0x00023628 File Offset: 0x00021828
		public readonly Vector2d PerpendicularRight
		{
			get
			{
				return new Vector2d(this.Y, -this.X);
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x0002363C File Offset: 0x0002183C
		public readonly Vector2d PerpendicularLeft
		{
			get
			{
				return new Vector2d(-this.Y, this.X);
			}
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x00023650 File Offset: 0x00021850
		public readonly Vector2d Normalized()
		{
			Vector2d result = this;
			result.Normalize();
			return result;
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x0002366C File Offset: 0x0002186C
		public void Normalize()
		{
			double num = 1.0 / this.Length;
			this.X *= num;
			this.Y *= num;
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x000236A8 File Offset: 0x000218A8
		public void NormalizeFast()
		{
			double num = MathHelper.InverseSqrtFast(this.X * this.X + this.Y * this.Y);
			this.X *= num;
			this.Y *= num;
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x000236F4 File Offset: 0x000218F4
		public readonly Vector2d Abs()
		{
			Vector2d vector2d = this;
			vector2d.X = Math.Abs(vector2d.X);
			vector2d.Y = Math.Abs(vector2d.Y);
			return vector2d;
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0002372D File Offset: 0x0002192D
		public readonly Vector2d Round()
		{
			return Vector2d.Round(this);
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0002373A File Offset: 0x0002193A
		public readonly Vector2d Round(MidpointRounding rounding)
		{
			return Vector2d.Round(this, rounding);
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x00023748 File Offset: 0x00021948
		public readonly Vector2d Ceiling()
		{
			return Vector2d.Ceiling(this);
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x00023755 File Offset: 0x00021955
		public readonly Vector2d Floor()
		{
			return Vector2d.Floor(this);
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x00023762 File Offset: 0x00021962
		public readonly Vector2d Truncate()
		{
			return Vector2d.Truncate(this);
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0002376F File Offset: 0x0002196F
		public static Vector2d Add(Vector2d a, Vector2d b)
		{
			Vector2d.Add(a, b, out a);
			return a;
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0002377D File Offset: 0x0002197D
		public static void Add(in Vector2d a, in Vector2d b, out Vector2d result)
		{
			result.X = a.X + b.X;
			result.Y = a.Y + b.Y;
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x000237A5 File Offset: 0x000219A5
		public static Vector2d Subtract(Vector2d a, Vector2d b)
		{
			Vector2d.Subtract(a, b, out a);
			return a;
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x000237B3 File Offset: 0x000219B3
		public static void Subtract(in Vector2d a, in Vector2d b, out Vector2d result)
		{
			result.X = a.X - b.X;
			result.Y = a.Y - b.Y;
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x000237DB File Offset: 0x000219DB
		public static Vector2d Multiply(Vector2d vector, double scale)
		{
			Vector2d.Multiply(vector, scale, out vector);
			return vector;
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x000237E8 File Offset: 0x000219E8
		public static void Multiply(in Vector2d vector, double scale, out Vector2d result)
		{
			result.X = vector.X * scale;
			result.Y = vector.Y * scale;
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x00023806 File Offset: 0x00021A06
		public static Vector2d Multiply(Vector2d vector, Vector2d scale)
		{
			Vector2d.Multiply(vector, scale, out vector);
			return vector;
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x00023814 File Offset: 0x00021A14
		public static void Multiply(in Vector2d vector, in Vector2d scale, out Vector2d result)
		{
			result.X = vector.X * scale.X;
			result.Y = vector.Y * scale.Y;
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0002383C File Offset: 0x00021A3C
		public static Vector2d Divide(Vector2d vector, double scale)
		{
			Vector2d.Divide(vector, scale, out vector);
			return vector;
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x00023849 File Offset: 0x00021A49
		public static void Divide(in Vector2d vector, double scale, out Vector2d result)
		{
			result.X = vector.X / scale;
			result.Y = vector.Y / scale;
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x00023867 File Offset: 0x00021A67
		public static Vector2d Divide(Vector2d vector, Vector2d scale)
		{
			Vector2d.Divide(vector, scale, out vector);
			return vector;
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x00023875 File Offset: 0x00021A75
		public static void Divide(in Vector2d vector, in Vector2d scale, out Vector2d result)
		{
			result.X = vector.X / scale.X;
			result.Y = vector.Y / scale.Y;
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x000238A0 File Offset: 0x00021AA0
		public static Vector2d ComponentMin(Vector2d a, Vector2d b)
		{
			a.X = ((a.X < b.X) ? a.X : b.X);
			a.Y = ((a.Y < b.Y) ? a.Y : b.Y);
			return a;
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x000238F4 File Offset: 0x00021AF4
		public static void ComponentMin(in Vector2d a, in Vector2d b, out Vector2d result)
		{
			result.X = ((a.X < b.X) ? a.X : b.X);
			result.Y = ((a.Y < b.Y) ? a.Y : b.Y);
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x00023948 File Offset: 0x00021B48
		public static Vector2d ComponentMax(Vector2d a, Vector2d b)
		{
			a.X = ((a.X > b.X) ? a.X : b.X);
			a.Y = ((a.Y > b.Y) ? a.Y : b.Y);
			return a;
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x0002399C File Offset: 0x00021B9C
		public static void ComponentMax(in Vector2d a, in Vector2d b, out Vector2d result)
		{
			result.X = ((a.X > b.X) ? a.X : b.X);
			result.Y = ((a.Y > b.Y) ? a.Y : b.Y);
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x000239ED File Offset: 0x00021BED
		public static Vector2d MagnitudeMin(Vector2d left, Vector2d right)
		{
			if (left.LengthSquared >= right.LengthSquared)
			{
				return right;
			}
			return left;
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x00023A02 File Offset: 0x00021C02
		public static void MagnitudeMin(in Vector2d left, in Vector2d right, out Vector2d result)
		{
			result = ((left.LengthSquared < right.LengthSquared) ? left : right);
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x00023A26 File Offset: 0x00021C26
		public static Vector2d MagnitudeMax(Vector2d left, Vector2d right)
		{
			if (left.LengthSquared < right.LengthSquared)
			{
				return right;
			}
			return left;
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x00023A3B File Offset: 0x00021C3B
		public static void MagnitudeMax(in Vector2d left, in Vector2d right, out Vector2d result)
		{
			result = ((left.LengthSquared >= right.LengthSquared) ? left : right);
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x00023A60 File Offset: 0x00021C60
		public static Vector2d Clamp(Vector2d vec, Vector2d min, Vector2d max)
		{
			vec.X = ((vec.X < min.X) ? min.X : ((vec.X > max.X) ? max.X : vec.X));
			vec.Y = ((vec.Y < min.Y) ? min.Y : ((vec.Y > max.Y) ? max.Y : vec.Y));
			return vec;
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x00023AE0 File Offset: 0x00021CE0
		public static void Clamp(in Vector2d vec, in Vector2d min, in Vector2d max, out Vector2d result)
		{
			result.X = ((vec.X < min.X) ? min.X : ((vec.X > max.X) ? max.X : vec.X));
			result.Y = ((vec.Y < min.Y) ? min.Y : ((vec.Y > max.Y) ? max.Y : vec.Y));
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x00023B5D File Offset: 0x00021D5D
		public static Vector2d Abs(Vector2d vec)
		{
			vec.X = Math.Abs(vec.X);
			vec.Y = Math.Abs(vec.Y);
			return vec;
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x00023B84 File Offset: 0x00021D84
		public static void Abs(in Vector2d vec, out Vector2d result)
		{
			result.X = Math.Abs(vec.X);
			result.Y = Math.Abs(vec.Y);
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x00023BA8 File Offset: 0x00021DA8
		public static Vector2d Round(Vector2d vec)
		{
			vec.X = Math.Round(vec.X);
			vec.Y = Math.Round(vec.Y);
			return vec;
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x00023BCF File Offset: 0x00021DCF
		public static void Round(in Vector2d vec, out Vector2d result)
		{
			result.X = Math.Round(vec.X);
			result.Y = Math.Round(vec.Y);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x00023BF3 File Offset: 0x00021DF3
		public static Vector2d Round(Vector2d vec, MidpointRounding rounding)
		{
			vec.X = Math.Round(vec.X, rounding);
			vec.Y = Math.Round(vec.Y, rounding);
			return vec;
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x00023C1C File Offset: 0x00021E1C
		public static void Round(in Vector2d vec, MidpointRounding rounding, out Vector2d result)
		{
			result.X = Math.Round(vec.X, rounding);
			result.Y = Math.Round(vec.Y, rounding);
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x00023C42 File Offset: 0x00021E42
		public static Vector2d Ceiling(Vector2d vec)
		{
			vec.X = Math.Ceiling(vec.X);
			vec.Y = Math.Ceiling(vec.Y);
			return vec;
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x00023C69 File Offset: 0x00021E69
		public static void Ceiling(in Vector2d vec, out Vector2d result)
		{
			result.X = Math.Ceiling(vec.X);
			result.Y = Math.Ceiling(vec.Y);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x00023C8D File Offset: 0x00021E8D
		public static Vector2d Floor(Vector2d vec)
		{
			vec.X = Math.Floor(vec.X);
			vec.Y = Math.Floor(vec.Y);
			return vec;
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x00023CB4 File Offset: 0x00021EB4
		public static void Floor(in Vector2d vec, out Vector2d result)
		{
			result.X = Math.Floor(vec.X);
			result.Y = Math.Floor(vec.Y);
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x00023CD8 File Offset: 0x00021ED8
		public static Vector2d Truncate(Vector2d vec)
		{
			vec.X = Math.Truncate(vec.X);
			vec.Y = Math.Truncate(vec.Y);
			return vec;
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x00023CFF File Offset: 0x00021EFF
		public static void Truncate(in Vector2d vec, out Vector2d result)
		{
			result.X = Math.Truncate(vec.X);
			result.Y = Math.Truncate(vec.Y);
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x00023D24 File Offset: 0x00021F24
		public static double Distance(Vector2d vec1, Vector2d vec2)
		{
			double result;
			Vector2d.Distance(vec1, vec2, out result);
			return result;
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x00023D3D File Offset: 0x00021F3D
		public static void Distance(in Vector2d vec1, in Vector2d vec2, out double result)
		{
			result = Math.Sqrt((vec2.X - vec1.X) * (vec2.X - vec1.X) + (vec2.Y - vec1.Y) * (vec2.Y - vec1.Y));
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x00023D80 File Offset: 0x00021F80
		public static double DistanceSquared(Vector2d vec1, Vector2d vec2)
		{
			double result;
			Vector2d.DistanceSquared(vec1, vec2, out result);
			return result;
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x00023D99 File Offset: 0x00021F99
		public static void DistanceSquared(in Vector2d vec1, in Vector2d vec2, out double result)
		{
			result = (vec2.X - vec1.X) * (vec2.X - vec1.X) + (vec2.Y - vec1.Y) * (vec2.Y - vec1.Y);
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x00023DD4 File Offset: 0x00021FD4
		public static Vector2d Normalize(Vector2d vec)
		{
			double num = 1.0 / vec.Length;
			vec.X *= num;
			vec.Y *= num;
			return vec;
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x00023E0C File Offset: 0x0002200C
		public static void Normalize(in Vector2d vec, out Vector2d result)
		{
			double num = 1.0 / vec.Length;
			result.X = vec.X * num;
			result.Y = vec.Y * num;
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x00023E48 File Offset: 0x00022048
		public static Vector2d NormalizeFast(Vector2d vec)
		{
			double num = MathHelper.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y);
			vec.X *= num;
			vec.Y *= num;
			return vec;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x00023E90 File Offset: 0x00022090
		public static void NormalizeFast(in Vector2d vec, out Vector2d result)
		{
			double num = MathHelper.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y);
			result.X = vec.X * num;
			result.Y = vec.Y * num;
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x00023EDA File Offset: 0x000220DA
		public static double Dot(Vector2d left, Vector2d right)
		{
			return left.X * right.X + left.Y * right.Y;
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x00023EF7 File Offset: 0x000220F7
		public static void Dot(in Vector2d left, in Vector2d right, out double result)
		{
			result = left.X * right.X + left.Y * right.Y;
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x00023F16 File Offset: 0x00022116
		public static Vector2d Lerp(Vector2d a, Vector2d b, double blend)
		{
			a.X = blend * (b.X - a.X) + a.X;
			a.Y = blend * (b.Y - a.Y) + a.Y;
			return a;
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x00023F53 File Offset: 0x00022153
		public static void Lerp(in Vector2d a, in Vector2d b, double blend, out Vector2d result)
		{
			result.X = blend * (b.X - a.X) + a.X;
			result.Y = blend * (b.Y - a.Y) + a.Y;
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x00023F90 File Offset: 0x00022190
		public static Vector2d Lerp(Vector2d a, Vector2d b, Vector2d blend)
		{
			a.X = blend.X * (b.X - a.X) + a.X;
			a.Y = blend.Y * (b.Y - a.Y) + a.Y;
			return a;
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x00023FE4 File Offset: 0x000221E4
		public static void Lerp(in Vector2d a, in Vector2d b, Vector2d blend, out Vector2d result)
		{
			result.X = blend.X * (b.X - a.X) + a.X;
			result.Y = blend.Y * (b.Y - a.Y) + a.Y;
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x00024034 File Offset: 0x00022234
		public static Vector2d Slerp(Vector2d a, Vector2d b, double t)
		{
			double num = a.Length * b.Length;
			double num2;
			if (num == 0.0 || Math.Abs(num2 = Vector2d.Dot(a, b) / num) > 0.99999999)
			{
				return Vector2d.Lerp(a, b, t);
			}
			double num3 = Math.Acos(Math.Clamp(num2, -1.0, 1.0));
			double num4 = Math.Sqrt(1.0 - num2 * num2);
			double f = Math.Sin((1.0 - t) * num3) / num4;
			double f2 = Math.Sin(t * num3) / num4;
			return f * a + f2 * b;
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x000240E8 File Offset: 0x000222E8
		public static void Slerp(in Vector2d a, in Vector2d b, double t, out Vector2d result)
		{
			double num = a.Length * b.Length;
			if (num == 0.0)
			{
				Vector2d.Lerp(a, b, t, out result);
				return;
			}
			double num2;
			Vector2d.Dot(a, b, out num2);
			num2 /= num;
			if (Math.Abs(num2) > 0.99999999)
			{
				Vector2d.Lerp(a, b, t, out result);
				return;
			}
			double num3 = Math.Acos(num2);
			double num4 = Math.Sqrt(1.0 - num2 * num2);
			double f = Math.Sin((1.0 - t) * num3) / num4;
			double f2 = Math.Sin(t * num3) / num4;
			result = f * a + f2 * b;
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x000241A4 File Offset: 0x000223A4
		public static Vector2d Elerp(Vector2d a, Vector2d b, double t)
		{
			a.X = Math.Pow(a.X, 1.0 - t) * Math.Pow(b.X, t);
			a.Y = Math.Pow(a.Y, 1.0 - t) * Math.Pow(b.Y, t);
			return a;
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x00024208 File Offset: 0x00022408
		public static void Elerp(in Vector2d a, in Vector2d b, double t, out Vector2d result)
		{
			result.X = Math.Pow(a.X, 1.0 - t) * Math.Pow(b.X, t);
			result.Y = Math.Pow(a.Y, 1.0 - t) * Math.Pow(b.Y, t);
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x00024268 File Offset: 0x00022468
		public static Vector2d BaryCentric(Vector2d a, Vector2d b, Vector2d c, double u, double v)
		{
			Vector2d result;
			Vector2d.BaryCentric(a, b, c, u, v, out result);
			return result;
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x00024288 File Offset: 0x00022488
		public static void BaryCentric(in Vector2d a, in Vector2d b, in Vector2d c, double u, double v, out Vector2d result)
		{
			Vector2d vector2d;
			Vector2d.Subtract(b, a, out vector2d);
			Vector2d vector2d2;
			Vector2d.Multiply(vector2d, u, out vector2d2);
			Vector2d vector2d3;
			Vector2d.Add(a, vector2d2, out vector2d3);
			Vector2d vector2d4;
			Vector2d.Subtract(c, a, out vector2d4);
			Vector2d vector2d5;
			Vector2d.Multiply(vector2d4, v, out vector2d5);
			Vector2d.Add(vector2d3, vector2d5, out result);
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x000242D4 File Offset: 0x000224D4
		public static Vector2d TransformRow(Vector2d vec, Matrix2d mat)
		{
			Vector2d result;
			Vector2d.TransformRow(vec, mat, out result);
			return result;
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x000242F0 File Offset: 0x000224F0
		public static void TransformRow(in Vector2d vec, in Matrix2d mat, out Vector2d result)
		{
			result = new Vector2d(vec.X * mat.Row0.X + vec.Y * mat.Row1.X, vec.X * mat.Row0.Y + vec.Y * mat.Row1.Y);
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x00024354 File Offset: 0x00022554
		public static Vector2d Transform(Vector2d vec, Quaterniond quat)
		{
			Vector2d result;
			Vector2d.Transform(vec, quat, out result);
			return result;
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x00024370 File Offset: 0x00022570
		public static void Transform(in Vector2d vec, in Quaterniond quat, out Vector2d result)
		{
			Quaterniond quaterniond = new Quaterniond(vec.X, vec.Y, 0.0, 0.0);
			Quaterniond quaterniond2;
			Quaterniond.Invert(quat, out quaterniond2);
			Quaterniond quaterniond3;
			Quaterniond.Multiply(quat, quaterniond, out quaterniond3);
			Quaterniond.Multiply(quaterniond3, quaterniond2, out quaterniond);
			result.X = quaterniond.X;
			result.Y = quaterniond.Y;
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x000243DC File Offset: 0x000225DC
		public static Vector2d TransformColumn(Matrix2d mat, Vector2d vec)
		{
			Vector2d result;
			Vector2d.TransformColumn(mat, vec, out result);
			return result;
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x000243F8 File Offset: 0x000225F8
		public static void TransformColumn(in Matrix2d mat, in Vector2d vec, out Vector2d result)
		{
			result.X = mat.Row0.X * vec.X + mat.Row0.Y * vec.Y;
			result.Y = mat.Row1.X * vec.X + mat.Row1.Y * vec.Y;
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x0002445B File Offset: 0x0002265B
		// (set) Token: 0x06000955 RID: 2389 RVA: 0x0002446E File Offset: 0x0002266E
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

		// Token: 0x06000956 RID: 2390 RVA: 0x00024488 File Offset: 0x00022688
		public static Vector2d operator +(Vector2d left, Vector2d right)
		{
			left.X += right.X;
			left.Y += right.Y;
			return left;
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x000244AD File Offset: 0x000226AD
		public static Vector2d operator -(Vector2d left, Vector2d right)
		{
			left.X -= right.X;
			left.Y -= right.Y;
			return left;
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x000244D2 File Offset: 0x000226D2
		public static Vector2d operator -(Vector2d vec)
		{
			vec.X = -vec.X;
			vec.Y = -vec.Y;
			return vec;
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x000244F1 File Offset: 0x000226F1
		public static Vector2d operator *(Vector2d vec, double f)
		{
			vec.X *= f;
			vec.Y *= f;
			return vec;
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0002450C File Offset: 0x0002270C
		public static Vector2d operator *(double f, Vector2d vec)
		{
			vec.X *= f;
			vec.Y *= f;
			return vec;
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x00024527 File Offset: 0x00022727
		public static Vector2d operator *(Vector2d vec, Vector2d scale)
		{
			vec.X *= scale.X;
			vec.Y *= scale.Y;
			return vec;
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0002454C File Offset: 0x0002274C
		public static Vector2d operator *(Vector2d vec, Matrix2d mat)
		{
			Vector2d result;
			Vector2d.TransformRow(vec, mat, out result);
			return result;
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x00024568 File Offset: 0x00022768
		public static Vector2d operator *(Matrix2d mat, Vector2d vec)
		{
			Vector2d result;
			Vector2d.TransformColumn(mat, vec, out result);
			return result;
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x00024584 File Offset: 0x00022784
		public static Vector2d operator *(Quaterniond quat, Vector2d vec)
		{
			Vector2d result;
			Vector2d.Transform(vec, quat, out result);
			return result;
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0002459D File Offset: 0x0002279D
		public static Vector2d operator /(Vector2d vec, double f)
		{
			vec.X /= f;
			vec.Y /= f;
			return vec;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x000245B8 File Offset: 0x000227B8
		public static Vector2d operator /(Vector2d vec, Vector2d scale)
		{
			vec.X /= scale.X;
			vec.Y /= scale.Y;
			return vec;
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x000245DD File Offset: 0x000227DD
		public static bool operator ==(Vector2d left, Vector2d right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x000245E7 File Offset: 0x000227E7
		public static bool operator !=(Vector2d left, Vector2d right)
		{
			return !(left == right);
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x000245F3 File Offset: 0x000227F3
		public static explicit operator Vector2(Vector2d vec)
		{
			return new Vector2((float)vec.X, (float)vec.Y);
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x00024608 File Offset: 0x00022808
		public static explicit operator Vector2h(Vector2d vec)
		{
			return new Vector2h(new Half(vec.X), new Half(vec.Y));
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x00024625 File Offset: 0x00022825
		public static explicit operator Vector2i(Vector2d vec)
		{
			return new Vector2i((int)vec.X, (int)vec.Y);
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0002463A File Offset: 0x0002283A
		public static implicit operator Vector2d([TupleElementNames(new string[]
		{
			"X",
			"Y"
		})] ValueTuple<double, double> values)
		{
			return new Vector2d(values.Item1, values.Item2);
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0002464D File Offset: 0x0002284D
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x00024657 File Offset: 0x00022857
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x00024661 File Offset: 0x00022861
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0002466B File Offset: 0x0002286B
		public readonly string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("({0}{2} {1})", this.X.ToString(format, formatProvider), this.Y.ToString(format, formatProvider), MathHelper.GetListSeparator(formatProvider));
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x00024697 File Offset: 0x00022897
		public override bool Equals(object obj)
		{
			return obj is Vector2d && this.Equals((Vector2d)obj);
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x000246AF File Offset: 0x000228AF
		public readonly bool Equals(Vector2d other)
		{
			return this.X == other.X && this.Y == other.Y;
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x000246CF File Offset: 0x000228CF
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<double, double>(this.X, this.Y);
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x000246E2 File Offset: 0x000228E2
		public readonly void Deconstruct(out double x, out double y)
		{
			x = this.X;
			y = this.Y;
		}

		// Token: 0x0400008F RID: 143
		public double X;

		// Token: 0x04000090 RID: 144
		public double Y;

		// Token: 0x04000091 RID: 145
		public static readonly Vector2d UnitX = new Vector2d(1.0, 0.0);

		// Token: 0x04000092 RID: 146
		public static readonly Vector2d UnitY = new Vector2d(0.0, 1.0);

		// Token: 0x04000093 RID: 147
		public static readonly Vector2d Zero = new Vector2d(0.0, 0.0);

		// Token: 0x04000094 RID: 148
		public static readonly Vector2d One = new Vector2d(1.0, 1.0);

		// Token: 0x04000095 RID: 149
		public static readonly Vector2d PositiveInfinity = new Vector2d(double.PositiveInfinity, double.PositiveInfinity);

		// Token: 0x04000096 RID: 150
		public static readonly Vector2d NegativeInfinity = new Vector2d(double.NegativeInfinity, double.NegativeInfinity);

		// Token: 0x04000097 RID: 151
		public static readonly int SizeInBytes = Unsafe.SizeOf<Vector2>();
	}
}
