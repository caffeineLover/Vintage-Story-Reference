using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace OpenTK.Mathematics
{
	// Token: 0x02000026 RID: 38
	[Serializable]
	public struct Vector3 : IEquatable<Vector3>, IFormattable
	{
		// Token: 0x060009D2 RID: 2514 RVA: 0x000252D2 File Offset: 0x000234D2
		public Vector3(float value)
		{
			this.X = value;
			this.Y = value;
			this.Z = value;
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x000252E9 File Offset: 0x000234E9
		public Vector3(float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x00025300 File Offset: 0x00023500
		public Vector3(Vector2 v)
		{
			this.X = v.X;
			this.Y = v.Y;
			this.Z = 0f;
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x00025325 File Offset: 0x00023525
		public Vector3(Vector3 v)
		{
			this.X = v.X;
			this.Y = v.Y;
			this.Z = v.Z;
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0002534B File Offset: 0x0002354B
		public Vector3(Vector4 v)
		{
			this.X = v.X;
			this.Y = v.Y;
			this.Z = v.Z;
		}

		// Token: 0x170001E7 RID: 487
		public unsafe float this[int index]
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

		// Token: 0x060009D9 RID: 2521 RVA: 0x000253BE File Offset: 0x000235BE
		[MethodImpl(256)]
		private readonly ref float GetElementUnsafe(in Vector3 v, int index)
		{
			return Unsafe.Add<float>(Unsafe.AsRef<float>(ref v.X), index);
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060009DA RID: 2522 RVA: 0x000253D1 File Offset: 0x000235D1
		public readonly float Length
		{
			get
			{
				return MathF.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060009DB RID: 2523 RVA: 0x00025401 File Offset: 0x00023601
		public readonly float ReciprocalLengthFast
		{
			get
			{
				return MathHelper.InverseSqrtFast(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060009DC RID: 2524 RVA: 0x00025431 File Offset: 0x00023631
		public readonly float LengthFast
		{
			get
			{
				return 1f / MathHelper.InverseSqrtFast(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060009DD RID: 2525 RVA: 0x00025467 File Offset: 0x00023667
		public readonly float LengthSquared
		{
			get
			{
				return this.X * this.X + this.Y * this.Y + this.Z * this.Z;
			}
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x00025494 File Offset: 0x00023694
		public readonly Vector3 Normalized()
		{
			Vector3 result = this;
			result.Normalize();
			return result;
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x000254B0 File Offset: 0x000236B0
		public void Normalize()
		{
			float num = 1f / this.Length;
			this.X *= num;
			this.Y *= num;
			this.Z *= num;
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x000254F4 File Offset: 0x000236F4
		public void NormalizeFast()
		{
			float num = MathHelper.InverseSqrtFast(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
			this.X *= num;
			this.Y *= num;
			this.Z *= num;
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0002555C File Offset: 0x0002375C
		public readonly Vector3 Abs()
		{
			Vector3 vector = this;
			vector.X = MathF.Abs(vector.X);
			vector.Y = MathF.Abs(vector.Y);
			vector.Z = MathF.Abs(vector.Z);
			return vector;
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x000255A7 File Offset: 0x000237A7
		public readonly Vector3 Round()
		{
			return Vector3.Round(this);
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x000255B4 File Offset: 0x000237B4
		public readonly Vector3 Round(MidpointRounding rounding)
		{
			return Vector3.Round(this, rounding);
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x000255C2 File Offset: 0x000237C2
		public readonly Vector3 Ceiling()
		{
			return Vector3.Ceiling(this);
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x000255CF File Offset: 0x000237CF
		public readonly Vector3 Floor()
		{
			return Vector3.Floor(this);
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x000255DC File Offset: 0x000237DC
		public readonly Vector3 Truncate()
		{
			return Vector3.Truncate(this);
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x000255E9 File Offset: 0x000237E9
		public static Vector3 Add(Vector3 a, Vector3 b)
		{
			Vector3.Add(a, b, out a);
			return a;
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x000255F7 File Offset: 0x000237F7
		public static void Add(in Vector3 a, in Vector3 b, out Vector3 result)
		{
			result.X = a.X + b.X;
			result.Y = a.Y + b.Y;
			result.Z = a.Z + b.Z;
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x00025632 File Offset: 0x00023832
		public static Vector3 Subtract(Vector3 a, Vector3 b)
		{
			Vector3.Subtract(a, b, out a);
			return a;
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x00025640 File Offset: 0x00023840
		public static void Subtract(in Vector3 a, in Vector3 b, out Vector3 result)
		{
			result.X = a.X - b.X;
			result.Y = a.Y - b.Y;
			result.Z = a.Z - b.Z;
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0002567B File Offset: 0x0002387B
		public static Vector3 Multiply(Vector3 vector, float scale)
		{
			Vector3.Multiply(vector, scale, out vector);
			return vector;
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x00025688 File Offset: 0x00023888
		public static void Multiply(in Vector3 vector, float scale, out Vector3 result)
		{
			result.X = vector.X * scale;
			result.Y = vector.Y * scale;
			result.Z = vector.Z * scale;
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x000256B4 File Offset: 0x000238B4
		public static Vector3 Multiply(Vector3 vector, Vector3 scale)
		{
			Vector3.Multiply(vector, scale, out vector);
			return vector;
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x000256C2 File Offset: 0x000238C2
		public static void Multiply(in Vector3 vector, in Vector3 scale, out Vector3 result)
		{
			result.X = vector.X * scale.X;
			result.Y = vector.Y * scale.Y;
			result.Z = vector.Z * scale.Z;
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x000256FD File Offset: 0x000238FD
		public static Vector3 Divide(Vector3 vector, float scale)
		{
			Vector3.Divide(vector, scale, out vector);
			return vector;
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0002570A File Offset: 0x0002390A
		public static void Divide(in Vector3 vector, float scale, out Vector3 result)
		{
			result.X = vector.X / scale;
			result.Y = vector.Y / scale;
			result.Z = vector.Z / scale;
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x00025736 File Offset: 0x00023936
		public static Vector3 Divide(Vector3 vector, Vector3 scale)
		{
			Vector3.Divide(vector, scale, out vector);
			return vector;
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00025744 File Offset: 0x00023944
		public static void Divide(in Vector3 vector, in Vector3 scale, out Vector3 result)
		{
			result.X = vector.X / scale.X;
			result.Y = vector.Y / scale.Y;
			result.Z = vector.Z / scale.Z;
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00025780 File Offset: 0x00023980
		public static Vector3 ComponentMin(Vector3 a, Vector3 b)
		{
			a.X = ((a.X < b.X) ? a.X : b.X);
			a.Y = ((a.Y < b.Y) ? a.Y : b.Y);
			a.Z = ((a.Z < b.Z) ? a.Z : b.Z);
			return a;
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x000257F8 File Offset: 0x000239F8
		public static void ComponentMin(in Vector3 a, in Vector3 b, out Vector3 result)
		{
			result.X = ((a.X < b.X) ? a.X : b.X);
			result.Y = ((a.Y < b.Y) ? a.Y : b.Y);
			result.Z = ((a.Z < b.Z) ? a.Z : b.Z);
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0002586C File Offset: 0x00023A6C
		public static Vector3 ComponentMax(Vector3 a, Vector3 b)
		{
			a.X = ((a.X > b.X) ? a.X : b.X);
			a.Y = ((a.Y > b.Y) ? a.Y : b.Y);
			a.Z = ((a.Z > b.Z) ? a.Z : b.Z);
			return a;
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x000258E4 File Offset: 0x00023AE4
		public static void ComponentMax(in Vector3 a, in Vector3 b, out Vector3 result)
		{
			result.X = ((a.X > b.X) ? a.X : b.X);
			result.Y = ((a.Y > b.Y) ? a.Y : b.Y);
			result.Z = ((a.Z > b.Z) ? a.Z : b.Z);
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x00025957 File Offset: 0x00023B57
		public static Vector3 MagnitudeMin(Vector3 left, Vector3 right)
		{
			if (left.LengthSquared >= right.LengthSquared)
			{
				return right;
			}
			return left;
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0002596C File Offset: 0x00023B6C
		public static void MagnitudeMin(in Vector3 left, in Vector3 right, out Vector3 result)
		{
			result = ((left.LengthSquared < right.LengthSquared) ? left : right);
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x00025990 File Offset: 0x00023B90
		public static Vector3 MagnitudeMax(Vector3 left, Vector3 right)
		{
			if (left.LengthSquared < right.LengthSquared)
			{
				return right;
			}
			return left;
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x000259A5 File Offset: 0x00023BA5
		public static void MagnitudeMax(in Vector3 left, in Vector3 right, out Vector3 result)
		{
			result = ((left.LengthSquared >= right.LengthSquared) ? left : right);
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x000259CC File Offset: 0x00023BCC
		public static Vector3 Clamp(Vector3 vec, Vector3 min, Vector3 max)
		{
			vec.X = ((vec.X < min.X) ? min.X : ((vec.X > max.X) ? max.X : vec.X));
			vec.Y = ((vec.Y < min.Y) ? min.Y : ((vec.Y > max.Y) ? max.Y : vec.Y));
			vec.Z = ((vec.Z < min.Z) ? min.Z : ((vec.Z > max.Z) ? max.Z : vec.Z));
			return vec;
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x00025A88 File Offset: 0x00023C88
		public static void Clamp(in Vector3 vec, in Vector3 min, in Vector3 max, out Vector3 result)
		{
			result.X = ((vec.X < min.X) ? min.X : ((vec.X > max.X) ? max.X : vec.X));
			result.Y = ((vec.Y < min.Y) ? min.Y : ((vec.Y > max.Y) ? max.Y : vec.Y));
			result.Z = ((vec.Z < min.Z) ? min.Z : ((vec.Z > max.Z) ? max.Z : vec.Z));
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x00025B3D File Offset: 0x00023D3D
		public static Vector3 Abs(Vector3 vec)
		{
			vec.X = MathF.Abs(vec.X);
			vec.Y = MathF.Abs(vec.Y);
			vec.Z = MathF.Abs(vec.Z);
			return vec;
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x00025B76 File Offset: 0x00023D76
		public static void Abs(in Vector3 vec, out Vector3 result)
		{
			result.X = MathF.Abs(vec.X);
			result.Y = MathF.Abs(vec.Y);
			result.Z = MathF.Abs(vec.Z);
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x00025BAB File Offset: 0x00023DAB
		public static Vector3 Round(Vector3 vec)
		{
			vec.X = MathF.Round(vec.X);
			vec.Y = MathF.Round(vec.Y);
			vec.Z = MathF.Round(vec.Z);
			return vec;
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x00025BE4 File Offset: 0x00023DE4
		public static void Round(in Vector3 vec, out Vector3 result)
		{
			result.X = MathF.Round(vec.X);
			result.Y = MathF.Round(vec.Y);
			result.Z = MathF.Round(vec.Z);
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x00025C19 File Offset: 0x00023E19
		public static Vector3 Round(Vector3 vec, MidpointRounding rounding)
		{
			vec.X = MathF.Round(vec.X, rounding);
			vec.Y = MathF.Round(vec.Y, rounding);
			vec.Z = MathF.Round(vec.Z, rounding);
			return vec;
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x00025C55 File Offset: 0x00023E55
		public static void Round(in Vector3 vec, MidpointRounding rounding, out Vector3 result)
		{
			result.X = MathF.Round(vec.X, rounding);
			result.Y = MathF.Round(vec.Y, rounding);
			result.Z = MathF.Round(vec.Z, rounding);
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x00025C8D File Offset: 0x00023E8D
		public static Vector3 Ceiling(Vector3 vec)
		{
			vec.X = MathF.Ceiling(vec.X);
			vec.Y = MathF.Ceiling(vec.Y);
			vec.Z = MathF.Ceiling(vec.Z);
			return vec;
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x00025CC6 File Offset: 0x00023EC6
		public static void Ceiling(in Vector3 vec, out Vector3 result)
		{
			result.X = MathF.Ceiling(vec.X);
			result.Y = MathF.Ceiling(vec.Y);
			result.Z = MathF.Ceiling(vec.Z);
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x00025CFB File Offset: 0x00023EFB
		public static Vector3 Floor(Vector3 vec)
		{
			vec.X = MathF.Floor(vec.X);
			vec.Y = MathF.Floor(vec.Y);
			vec.Z = MathF.Floor(vec.Z);
			return vec;
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x00025D34 File Offset: 0x00023F34
		public static void Floor(in Vector3 vec, out Vector3 result)
		{
			result.X = MathF.Floor(vec.X);
			result.Y = MathF.Floor(vec.Y);
			result.Z = MathF.Floor(vec.Z);
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x00025D69 File Offset: 0x00023F69
		public static Vector3 Truncate(Vector3 vec)
		{
			vec.X = MathF.Truncate(vec.X);
			vec.Y = MathF.Truncate(vec.Y);
			vec.Z = MathF.Truncate(vec.Z);
			return vec;
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x00025DA2 File Offset: 0x00023FA2
		public static void Truncate(in Vector3 vec, out Vector3 result)
		{
			result.X = MathF.Truncate(vec.X);
			result.Y = MathF.Truncate(vec.Y);
			result.Z = MathF.Truncate(vec.Z);
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x00025DD8 File Offset: 0x00023FD8
		public static float Distance(Vector3 vec1, Vector3 vec2)
		{
			float result;
			Vector3.Distance(vec1, vec2, out result);
			return result;
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x00025DF4 File Offset: 0x00023FF4
		public static void Distance(in Vector3 vec1, in Vector3 vec2, out float result)
		{
			result = MathF.Sqrt((vec2.X - vec1.X) * (vec2.X - vec1.X) + (vec2.Y - vec1.Y) * (vec2.Y - vec1.Y) + (vec2.Z - vec1.Z) * (vec2.Z - vec1.Z));
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x00025E5C File Offset: 0x0002405C
		public static float DistanceSquared(Vector3 vec1, Vector3 vec2)
		{
			float result;
			Vector3.DistanceSquared(vec1, vec2, out result);
			return result;
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x00025E78 File Offset: 0x00024078
		public static void DistanceSquared(in Vector3 vec1, in Vector3 vec2, out float result)
		{
			result = (vec2.X - vec1.X) * (vec2.X - vec1.X) + (vec2.Y - vec1.Y) * (vec2.Y - vec1.Y) + (vec2.Z - vec1.Z) * (vec2.Z - vec1.Z);
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x00025EDC File Offset: 0x000240DC
		public static Vector3 Normalize(Vector3 vec)
		{
			float num = 1f / vec.Length;
			vec.X *= num;
			vec.Y *= num;
			vec.Z *= num;
			return vec;
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x00025F1C File Offset: 0x0002411C
		public static void Normalize(in Vector3 vec, out Vector3 result)
		{
			float num = 1f / vec.Length;
			result.X = vec.X * num;
			result.Y = vec.Y * num;
			result.Z = vec.Z * num;
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x00025F60 File Offset: 0x00024160
		public static Vector3 NormalizeFast(Vector3 vec)
		{
			float num = MathHelper.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z);
			vec.X *= num;
			vec.Y *= num;
			vec.Z *= num;
			return vec;
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x00025FC4 File Offset: 0x000241C4
		public static void NormalizeFast(in Vector3 vec, out Vector3 result)
		{
			float num = MathHelper.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z);
			result.X = vec.X * num;
			result.Y = vec.Y * num;
			result.Z = vec.Z * num;
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x0002602A File Offset: 0x0002422A
		public static float Dot(Vector3 left, Vector3 right)
		{
			return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x00026055 File Offset: 0x00024255
		public static void Dot(in Vector3 left, in Vector3 right, out float result)
		{
			result = left.X * right.X + left.Y * right.Y + left.Z * right.Z;
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x00026084 File Offset: 0x00024284
		public static Vector3 Cross(Vector3 left, Vector3 right)
		{
			Vector3 result;
			Vector3.Cross(left, right, out result);
			return result;
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x000260A0 File Offset: 0x000242A0
		public static void Cross(in Vector3 left, in Vector3 right, out Vector3 result)
		{
			result.X = left.Y * right.Z - left.Z * right.Y;
			result.Y = left.Z * right.X - left.X * right.Z;
			result.Z = left.X * right.Y - left.Y * right.X;
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x00026110 File Offset: 0x00024310
		public static Vector3 Lerp(Vector3 a, Vector3 b, float blend)
		{
			a.X = blend * (b.X - a.X) + a.X;
			a.Y = blend * (b.Y - a.Y) + a.Y;
			a.Z = blend * (b.Z - a.Z) + a.Z;
			return a;
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x00026178 File Offset: 0x00024378
		public static void Lerp(in Vector3 a, in Vector3 b, float blend, out Vector3 result)
		{
			result.X = blend * (b.X - a.X) + a.X;
			result.Y = blend * (b.Y - a.Y) + a.Y;
			result.Z = blend * (b.Z - a.Z) + a.Z;
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x000261DC File Offset: 0x000243DC
		public static Vector3 Lerp(Vector3 a, Vector3 b, Vector3 blend)
		{
			a.X = blend.X * (b.X - a.X) + a.X;
			a.Y = blend.Y * (b.Y - a.Y) + a.Y;
			a.Z = blend.Z * (b.Z - a.Z) + a.Z;
			return a;
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x00026250 File Offset: 0x00024450
		public static void Lerp(in Vector3 a, in Vector3 b, Vector3 blend, out Vector3 result)
		{
			result.X = blend.X * (b.X - a.X) + a.X;
			result.Y = blend.Y * (b.Y - a.Y) + a.Y;
			result.Z = blend.Z * (b.Z - a.Z) + a.Z;
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x000262C0 File Offset: 0x000244C0
		public static Vector3 Slerp(Vector3 a, Vector3 b, float t)
		{
			float num = a.Length * b.Length;
			float num2;
			if (num == 0f || Math.Abs(num2 = Vector3.Dot(a, b) / num) > 0.9999f)
			{
				return Vector3.Lerp(a, b, t);
			}
			float num3 = MathF.Acos(Math.Clamp(num2, -1f, 1f));
			float num4 = MathF.Sqrt(1f - num2 * num2);
			float scale = MathF.Sin((1f - t) * num3) / num4;
			float scale2 = MathF.Sin(t * num3) / num4;
			return scale * a + scale2 * b;
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0002635C File Offset: 0x0002455C
		public static void Slerp(in Vector3 a, in Vector3 b, float t, out Vector3 result)
		{
			float num = a.Length * b.Length;
			if (num == 0f)
			{
				Vector3.Lerp(a, b, t, out result);
				return;
			}
			float num2;
			Vector3.Dot(a, b, out num2);
			num2 /= num;
			if (Math.Abs(num2) > 0.9999f)
			{
				Vector3.Lerp(a, b, t, out result);
				return;
			}
			float num3 = MathF.Acos(num2);
			float num4 = MathF.Sqrt(1f - num2 * num2);
			float scale = MathF.Sin((1f - t) * num3) / num4;
			float scale2 = MathF.Sin(t * num3) / num4;
			result = scale * a + scale2 * b;
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x00026408 File Offset: 0x00024608
		public static Vector3 Elerp(Vector3 a, Vector3 b, float t)
		{
			a.X = MathF.Pow(a.X, 1f - t) * MathF.Pow(b.X, t);
			a.Y = MathF.Pow(a.Y, 1f - t) * MathF.Pow(b.Y, t);
			a.Z = MathF.Pow(a.Z, 1f - t) * MathF.Pow(b.Z, t);
			return a;
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x00026488 File Offset: 0x00024688
		public static void Elerp(in Vector3 a, in Vector3 b, float t, out Vector3 result)
		{
			result.X = MathF.Pow(a.X, 1f - t) * MathF.Pow(b.X, t);
			result.Y = MathF.Pow(a.Y, 1f - t) * MathF.Pow(b.Y, t);
			result.Z = MathF.Pow(a.Z, 1f - t) * MathF.Pow(b.Z, t);
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x00026504 File Offset: 0x00024704
		public static Vector3 BaryCentric(Vector3 a, Vector3 b, Vector3 c, float u, float v)
		{
			Vector3 result;
			Vector3.BaryCentric(a, b, c, u, v, out result);
			return result;
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x00026524 File Offset: 0x00024724
		public static void BaryCentric(in Vector3 a, in Vector3 b, in Vector3 c, float u, float v, out Vector3 result)
		{
			Vector3 vector;
			Vector3.Subtract(b, a, out vector);
			Vector3 vector2;
			Vector3.Multiply(vector, u, out vector2);
			Vector3 vector3;
			Vector3.Add(a, vector2, out vector3);
			Vector3 vector4;
			Vector3.Subtract(c, a, out vector4);
			Vector3 vector5;
			Vector3.Multiply(vector4, v, out vector5);
			Vector3.Add(vector3, vector5, out result);
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x00026570 File Offset: 0x00024770
		public static Vector3 TransformVector(Vector3 vec, Matrix4 mat)
		{
			Vector3 result;
			Vector3.TransformVector(vec, mat, out result);
			return result;
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x0002658C File Offset: 0x0002478C
		public static void TransformVector(in Vector3 vec, in Matrix4 mat, out Vector3 result)
		{
			result.X = vec.X * mat.Row0.X + vec.Y * mat.Row1.X + vec.Z * mat.Row2.X;
			result.Y = vec.X * mat.Row0.Y + vec.Y * mat.Row1.Y + vec.Z * mat.Row2.Y;
			result.Z = vec.X * mat.Row0.Z + vec.Y * mat.Row1.Z + vec.Z * mat.Row2.Z;
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x00026654 File Offset: 0x00024854
		public static Vector3 TransformNormal(Vector3 norm, Matrix4 mat)
		{
			Vector3 result;
			Vector3.TransformNormal(norm, mat, out result);
			return result;
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x00026670 File Offset: 0x00024870
		public static void TransformNormal(in Vector3 norm, in Matrix4 mat, out Vector3 result)
		{
			Matrix4 matrix = Matrix4.Invert(mat);
			Vector3.TransformNormalInverse(norm, matrix, out result);
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00026694 File Offset: 0x00024894
		public static Vector3 TransformNormalInverse(Vector3 norm, Matrix4 invMat)
		{
			Vector3 result;
			Vector3.TransformNormalInverse(norm, invMat, out result);
			return result;
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x000266B0 File Offset: 0x000248B0
		public static void TransformNormalInverse(in Vector3 norm, in Matrix4 invMat, out Vector3 result)
		{
			result.X = norm.X * invMat.Row0.X + norm.Y * invMat.Row0.Y + norm.Z * invMat.Row0.Z;
			result.Y = norm.X * invMat.Row1.X + norm.Y * invMat.Row1.Y + norm.Z * invMat.Row1.Z;
			result.Z = norm.X * invMat.Row2.X + norm.Y * invMat.Row2.Y + norm.Z * invMat.Row2.Z;
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x00026778 File Offset: 0x00024978
		public static Vector3 TransformPosition(Vector3 pos, Matrix4 mat)
		{
			Vector3 result;
			Vector3.TransformPosition(pos, mat, out result);
			return result;
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x00026794 File Offset: 0x00024994
		public static void TransformPosition(in Vector3 pos, in Matrix4 mat, out Vector3 result)
		{
			result.X = pos.X * mat.Row0.X + pos.Y * mat.Row1.X + pos.Z * mat.Row2.X + mat.Row3.X;
			result.Y = pos.X * mat.Row0.Y + pos.Y * mat.Row1.Y + pos.Z * mat.Row2.Y + mat.Row3.Y;
			result.Z = pos.X * mat.Row0.Z + pos.Y * mat.Row1.Z + pos.Z * mat.Row2.Z + mat.Row3.Z;
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x00026880 File Offset: 0x00024A80
		public static Vector3 TransformRow(Vector3 vec, Matrix3 mat)
		{
			Vector3 result;
			Vector3.TransformRow(vec, mat, out result);
			return result;
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0002689C File Offset: 0x00024A9C
		public static void TransformRow(in Vector3 vec, in Matrix3 mat, out Vector3 result)
		{
			result.X = vec.X * mat.Row0.X + vec.Y * mat.Row1.X + vec.Z * mat.Row2.X;
			result.Y = vec.X * mat.Row0.Y + vec.Y * mat.Row1.Y + vec.Z * mat.Row2.Y;
			result.Z = vec.X * mat.Row0.Z + vec.Y * mat.Row1.Z + vec.Z * mat.Row2.Z;
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x00026964 File Offset: 0x00024B64
		public static Vector3 Transform(Vector3 vec, Quaternion quat)
		{
			Vector3 result;
			Vector3.Transform(vec, quat, out result);
			return result;
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x00026980 File Offset: 0x00024B80
		public static void Transform(in Vector3 vec, in Quaternion quat, out Vector3 result)
		{
			Vector3 xyz = quat.Xyz;
			Vector3 vector;
			Vector3.Cross(xyz, vec, out vector);
			Vector3 vector2;
			Vector3.Multiply(vec, quat.W, out vector2);
			Vector3.Add(vector, vector2, out vector);
			Vector3.Cross(xyz, vector, out vector2);
			Vector3.Multiply(vector2, 2f, out vector2);
			Vector3.Add(vec, vector2, out result);
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x000269DC File Offset: 0x00024BDC
		public static Vector3 TransformColumn(Matrix3 mat, Vector3 vec)
		{
			Vector3 result;
			Vector3.TransformColumn(mat, vec, out result);
			return result;
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x000269F8 File Offset: 0x00024BF8
		public static void TransformColumn(in Matrix3 mat, in Vector3 vec, out Vector3 result)
		{
			result.X = mat.Row0.X * vec.X + mat.Row0.Y * vec.Y + mat.Row0.Z * vec.Z;
			result.Y = mat.Row1.X * vec.X + mat.Row1.Y * vec.Y + mat.Row1.Z * vec.Z;
			result.Z = mat.Row2.X * vec.X + mat.Row2.Y * vec.Y + mat.Row2.Z * vec.Z;
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x00026AC0 File Offset: 0x00024CC0
		public static Vector3 TransformPerspective(Vector3 vec, Matrix4 mat)
		{
			Vector3 result;
			Vector3.TransformPerspective(vec, mat, out result);
			return result;
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x00026ADC File Offset: 0x00024CDC
		public static void TransformPerspective(in Vector3 vec, in Matrix4 mat, out Vector3 result)
		{
			Vector4 vector = new Vector4(vec.X, vec.Y, vec.Z, 1f);
			Vector4.TransformRow(vector, mat, out vector);
			result.X = vector.X / vector.W;
			result.Y = vector.Y / vector.W;
			result.Z = vector.Z / vector.W;
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x00026B4C File Offset: 0x00024D4C
		public static float CalculateAngle(Vector3 first, Vector3 second)
		{
			float result;
			Vector3.CalculateAngle(first, second, out result);
			return result;
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x00026B68 File Offset: 0x00024D68
		public static void CalculateAngle(in Vector3 first, in Vector3 second, out float result)
		{
			float num;
			Vector3.Dot(first, second, out num);
			result = MathF.Acos(MathHelper.Clamp(num / (first.Length * second.Length), -1f, 1f));
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x00026BA4 File Offset: 0x00024DA4
		public static Vector3 Project(Vector3 vector, float x, float y, float width, float height, float minZ, float maxZ, Matrix4 worldViewProjection)
		{
			Vector4 vector2;
			vector2.X = vector.X * worldViewProjection.M11 + vector.Y * worldViewProjection.M21 + vector.Z * worldViewProjection.M31 + worldViewProjection.M41;
			vector2.Y = vector.X * worldViewProjection.M12 + vector.Y * worldViewProjection.M22 + vector.Z * worldViewProjection.M32 + worldViewProjection.M42;
			vector2.Z = vector.X * worldViewProjection.M13 + vector.Y * worldViewProjection.M23 + vector.Z * worldViewProjection.M33 + worldViewProjection.M43;
			vector2.W = vector.X * worldViewProjection.M14 + vector.Y * worldViewProjection.M24 + vector.Z * worldViewProjection.M34 + worldViewProjection.M44;
			vector2 /= vector2.W;
			vector2.X = x + width * ((vector2.X + 1f) / 2f);
			vector2.Y = y + height * ((vector2.Y + 1f) / 2f);
			vector2.Z = minZ + (maxZ - minZ) * ((vector2.Z + 1f) / 2f);
			return new Vector3(vector2.X, vector2.Y, vector2.Z);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x00026D20 File Offset: 0x00024F20
		public static Vector3 Unproject(Vector3 vector, float x, float y, float width, float height, float minZ, float maxZ, Matrix4 inverseWorldViewProjection)
		{
			float num = (vector.X - x) / width * 2f - 1f;
			float num2 = (vector.Y - y) / height * 2f - 1f;
			float num3 = (vector.Z - minZ) / (maxZ - minZ) * 2f - 1f;
			Vector3 vector2;
			vector2.X = num * inverseWorldViewProjection.M11 + num2 * inverseWorldViewProjection.M21 + num3 * inverseWorldViewProjection.M31 + inverseWorldViewProjection.M41;
			vector2.Y = num * inverseWorldViewProjection.M12 + num2 * inverseWorldViewProjection.M22 + num3 * inverseWorldViewProjection.M32 + inverseWorldViewProjection.M42;
			vector2.Z = num * inverseWorldViewProjection.M13 + num2 * inverseWorldViewProjection.M23 + num3 * inverseWorldViewProjection.M33 + inverseWorldViewProjection.M43;
			float scale = num * inverseWorldViewProjection.M14 + num2 * inverseWorldViewProjection.M24 + num3 * inverseWorldViewProjection.M34 + inverseWorldViewProjection.M44;
			vector2 /= scale;
			return vector2;
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x00026E2D File Offset: 0x0002502D
		// (set) Token: 0x06000A34 RID: 2612 RVA: 0x00026E3A File Offset: 0x0002503A
		[XmlIgnore]
		public unsafe Vector2 Xy
		{
			get
			{
				return *Unsafe.As<Vector3, Vector2>(ref this);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x00026E54 File Offset: 0x00025054
		// (set) Token: 0x06000A36 RID: 2614 RVA: 0x00026E67 File Offset: 0x00025067
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

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x00026E81 File Offset: 0x00025081
		// (set) Token: 0x06000A38 RID: 2616 RVA: 0x00026E94 File Offset: 0x00025094
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

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x00026EAE File Offset: 0x000250AE
		// (set) Token: 0x06000A3A RID: 2618 RVA: 0x00026EC1 File Offset: 0x000250C1
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

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x00026EDB File Offset: 0x000250DB
		// (set) Token: 0x06000A3C RID: 2620 RVA: 0x00026EEE File Offset: 0x000250EE
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

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x00026F08 File Offset: 0x00025108
		// (set) Token: 0x06000A3E RID: 2622 RVA: 0x00026F1B File Offset: 0x0002511B
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

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000A3F RID: 2623 RVA: 0x00026F35 File Offset: 0x00025135
		// (set) Token: 0x06000A40 RID: 2624 RVA: 0x00026F4E File Offset: 0x0002514E
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

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000A41 RID: 2625 RVA: 0x00026F74 File Offset: 0x00025174
		// (set) Token: 0x06000A42 RID: 2626 RVA: 0x00026F8D File Offset: 0x0002518D
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

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000A43 RID: 2627 RVA: 0x00026FB3 File Offset: 0x000251B3
		// (set) Token: 0x06000A44 RID: 2628 RVA: 0x00026FCC File Offset: 0x000251CC
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

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000A45 RID: 2629 RVA: 0x00026FF2 File Offset: 0x000251F2
		// (set) Token: 0x06000A46 RID: 2630 RVA: 0x0002700B File Offset: 0x0002520B
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

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000A47 RID: 2631 RVA: 0x00027031 File Offset: 0x00025231
		// (set) Token: 0x06000A48 RID: 2632 RVA: 0x0002704A File Offset: 0x0002524A
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

		// Token: 0x06000A49 RID: 2633 RVA: 0x00027070 File Offset: 0x00025270
		public static Vector3 operator +(Vector3 left, Vector3 right)
		{
			left.X += right.X;
			left.Y += right.Y;
			left.Z += right.Z;
			return left;
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x000270A6 File Offset: 0x000252A6
		public static Vector3 operator -(Vector3 left, Vector3 right)
		{
			left.X -= right.X;
			left.Y -= right.Y;
			left.Z -= right.Z;
			return left;
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x000270DC File Offset: 0x000252DC
		public static Vector3 operator -(Vector3 vec)
		{
			vec.X = -vec.X;
			vec.Y = -vec.Y;
			vec.Z = -vec.Z;
			return vec;
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x00027109 File Offset: 0x00025309
		public static Vector3 operator *(Vector3 vec, float scale)
		{
			vec.X *= scale;
			vec.Y *= scale;
			vec.Z *= scale;
			return vec;
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x00027130 File Offset: 0x00025330
		public static Vector3 operator *(float scale, Vector3 vec)
		{
			vec.X *= scale;
			vec.Y *= scale;
			vec.Z *= scale;
			return vec;
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x00027157 File Offset: 0x00025357
		public static Vector3 operator *(Vector3 vec, Vector3 scale)
		{
			vec.X *= scale.X;
			vec.Y *= scale.Y;
			vec.Z *= scale.Z;
			return vec;
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x00027190 File Offset: 0x00025390
		public static Vector3 operator *(Vector3 vec, Matrix3 mat)
		{
			Vector3 result;
			Vector3.TransformRow(vec, mat, out result);
			return result;
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x000271AC File Offset: 0x000253AC
		public static Vector3 operator *(Matrix3 mat, Vector3 vec)
		{
			Vector3 result;
			Vector3.TransformColumn(mat, vec, out result);
			return result;
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x000271C8 File Offset: 0x000253C8
		public static Vector3 operator *(Quaternion quat, Vector3 vec)
		{
			Vector3 result;
			Vector3.Transform(vec, quat, out result);
			return result;
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x000271E1 File Offset: 0x000253E1
		public static Vector3 operator /(Vector3 vec, float scale)
		{
			vec.X /= scale;
			vec.Y /= scale;
			vec.Z /= scale;
			return vec;
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x00027208 File Offset: 0x00025408
		public static Vector3 operator /(Vector3 vec, Vector3 scale)
		{
			vec.X /= scale.X;
			vec.Y /= scale.Y;
			vec.Z /= scale.Z;
			return vec;
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x0002723E File Offset: 0x0002543E
		public static bool operator ==(Vector3 left, Vector3 right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x00027248 File Offset: 0x00025448
		public static bool operator !=(Vector3 left, Vector3 right)
		{
			return !(left == right);
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x00027254 File Offset: 0x00025454
		public static implicit operator Vector3d(Vector3 vec)
		{
			return new Vector3d((double)vec.X, (double)vec.Y, (double)vec.Z);
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x00027270 File Offset: 0x00025470
		public static explicit operator Vector3h(Vector3 vec)
		{
			return new Vector3h(vec.X, vec.Y, vec.Z);
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x00027289 File Offset: 0x00025489
		public static explicit operator Vector3i(Vector3 vec)
		{
			return new Vector3i((int)vec.X, (int)vec.Y, (int)vec.Z);
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x000272A5 File Offset: 0x000254A5
		public static implicit operator Vector3([TupleElementNames(new string[]
		{
			"X",
			"Y",
			"Z"
		})] ValueTuple<float, float, float> values)
		{
			return new Vector3(values.Item1, values.Item2, values.Item3);
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x000272BE File Offset: 0x000254BE
		public unsafe static explicit operator Vector3(Vector3 vec)
		{
			return *Unsafe.As<Vector3, Vector3>(ref vec);
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x000272CC File Offset: 0x000254CC
		public unsafe static explicit operator Vector3(Vector3 vec)
		{
			return *Unsafe.As<Vector3, Vector3>(ref vec);
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x000272DA File Offset: 0x000254DA
		public override readonly string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x000272E4 File Offset: 0x000254E4
		public readonly string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x000272EE File Offset: 0x000254EE
		public readonly string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x000272F8 File Offset: 0x000254F8
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

		// Token: 0x06000A60 RID: 2656 RVA: 0x0002734E File Offset: 0x0002554E
		public override readonly bool Equals(object obj)
		{
			return obj is Vector3 && this.Equals((Vector3)obj);
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x00027366 File Offset: 0x00025566
		public readonly bool Equals(Vector3 other)
		{
			return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x00027394 File Offset: 0x00025594
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<float, float, float>(this.X, this.Y, this.Z);
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x000273AD File Offset: 0x000255AD
		public readonly void Deconstruct(out float x, out float y, out float z)
		{
			x = this.X;
			y = this.Y;
			z = this.Z;
		}

		// Token: 0x040000A2 RID: 162
		public float X;

		// Token: 0x040000A3 RID: 163
		public float Y;

		// Token: 0x040000A4 RID: 164
		public float Z;

		// Token: 0x040000A5 RID: 165
		public static readonly Vector3 UnitX = new Vector3(1f, 0f, 0f);

		// Token: 0x040000A6 RID: 166
		public static readonly Vector3 UnitY = new Vector3(0f, 1f, 0f);

		// Token: 0x040000A7 RID: 167
		public static readonly Vector3 UnitZ = new Vector3(0f, 0f, 1f);

		// Token: 0x040000A8 RID: 168
		public static readonly Vector3 Zero = new Vector3(0f, 0f, 0f);

		// Token: 0x040000A9 RID: 169
		public static readonly Vector3 One = new Vector3(1f, 1f, 1f);

		// Token: 0x040000AA RID: 170
		public static readonly Vector3 PositiveInfinity = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

		// Token: 0x040000AB RID: 171
		public static readonly Vector3 NegativeInfinity = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

		// Token: 0x040000AC RID: 172
		public static readonly int SizeInBytes = Marshal.SizeOf<Vector3>();
	}
}
