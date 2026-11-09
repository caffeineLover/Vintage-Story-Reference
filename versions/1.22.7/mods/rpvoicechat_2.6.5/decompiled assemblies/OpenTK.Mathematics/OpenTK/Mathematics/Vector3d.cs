using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace OpenTK.Mathematics
{
	// Token: 0x02000027 RID: 39
	[Serializable]
	public struct Vector3d : IEquatable<Vector3d>, IFormattable
	{
		// Token: 0x06000A65 RID: 2661 RVA: 0x0002748E File Offset: 0x0002568E
		public Vector3d(double value)
		{
			this.X = value;
			this.Y = value;
			this.Z = value;
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x000274A5 File Offset: 0x000256A5
		public Vector3d(double x, double y, double z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x000274BC File Offset: 0x000256BC
		public Vector3d(Vector2d v)
		{
			this.X = v.X;
			this.Y = v.Y;
			this.Z = 0.0;
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x000274E5 File Offset: 0x000256E5
		public Vector3d(Vector3d v)
		{
			this.X = v.X;
			this.Y = v.Y;
			this.Z = v.Z;
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x0002750B File Offset: 0x0002570B
		public Vector3d(Vector4d v)
		{
			this.X = v.X;
			this.Y = v.Y;
			this.Z = v.Z;
		}

		// Token: 0x170001F7 RID: 503
		public unsafe double this[int index]
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

		// Token: 0x06000A6C RID: 2668 RVA: 0x0002757E File Offset: 0x0002577E
		[MethodImpl(256)]
		private readonly ref double GetElementUnsafe(in Vector3d v, int index)
		{
			return Unsafe.Add<double>(Unsafe.AsRef<double>(ref v.X), index);
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x00027591 File Offset: 0x00025791
		public readonly double Length
		{
			get
			{
				return Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x000275C1 File Offset: 0x000257C1
		public readonly double ReciprocalLengthFast
		{
			get
			{
				return MathHelper.InverseSqrtFast(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x000275F1 File Offset: 0x000257F1
		public readonly double LengthFast
		{
			get
			{
				return 1.0 / MathHelper.InverseSqrtFast(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000A70 RID: 2672 RVA: 0x0002762B File Offset: 0x0002582B
		public readonly double LengthSquared
		{
			get
			{
				return this.X * this.X + this.Y * this.Y + this.Z * this.Z;
			}
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x00027658 File Offset: 0x00025858
		public readonly Vector3d Normalized()
		{
			Vector3d result = this;
			result.Normalize();
			return result;
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00027674 File Offset: 0x00025874
		public void Normalize()
		{
			double num = 1.0 / this.Length;
			this.X *= num;
			this.Y *= num;
			this.Z *= num;
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x000276BC File Offset: 0x000258BC
		public void NormalizeFast()
		{
			double num = MathHelper.InverseSqrtFast(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
			this.X *= num;
			this.Y *= num;
			this.Z *= num;
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x00027724 File Offset: 0x00025924
		public readonly Vector3d Abs()
		{
			Vector3d vector3d = this;
			vector3d.X = Math.Abs(vector3d.X);
			vector3d.Y = Math.Abs(vector3d.Y);
			vector3d.Z = Math.Abs(vector3d.Z);
			return vector3d;
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x0002776F File Offset: 0x0002596F
		public readonly Vector3d Round()
		{
			return Vector3d.Round(this);
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0002777C File Offset: 0x0002597C
		public readonly Vector3d Round(MidpointRounding rounding)
		{
			return Vector3d.Round(this, rounding);
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x0002778A File Offset: 0x0002598A
		public readonly Vector3d Ceiling()
		{
			return Vector3d.Ceiling(this);
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x00027797 File Offset: 0x00025997
		public readonly Vector3d Floor()
		{
			return Vector3d.Floor(this);
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x000277A4 File Offset: 0x000259A4
		public readonly Vector3d Truncate()
		{
			return Vector3d.Truncate(this);
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x000277B1 File Offset: 0x000259B1
		public static Vector3d Add(Vector3d a, Vector3d b)
		{
			Vector3d.Add(a, b, out a);
			return a;
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x000277BF File Offset: 0x000259BF
		public static void Add(in Vector3d a, in Vector3d b, out Vector3d result)
		{
			result.X = a.X + b.X;
			result.Y = a.Y + b.Y;
			result.Z = a.Z + b.Z;
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x000277FA File Offset: 0x000259FA
		public static Vector3d Subtract(Vector3d a, Vector3d b)
		{
			Vector3d.Subtract(a, b, out a);
			return a;
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x00027808 File Offset: 0x00025A08
		public static void Subtract(in Vector3d a, in Vector3d b, out Vector3d result)
		{
			result.X = a.X - b.X;
			result.Y = a.Y - b.Y;
			result.Z = a.Z - b.Z;
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00027843 File Offset: 0x00025A43
		public static Vector3d Multiply(Vector3d vector, double scale)
		{
			Vector3d.Multiply(vector, scale, out vector);
			return vector;
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x00027850 File Offset: 0x00025A50
		public static void Multiply(in Vector3d vector, double scale, out Vector3d result)
		{
			result.X = vector.X * scale;
			result.Y = vector.Y * scale;
			result.Z = vector.Z * scale;
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x0002787C File Offset: 0x00025A7C
		public static Vector3d Multiply(Vector3d vector, Vector3d scale)
		{
			Vector3d.Multiply(vector, scale, out vector);
			return vector;
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x0002788A File Offset: 0x00025A8A
		public static void Multiply(in Vector3d vector, in Vector3d scale, out Vector3d result)
		{
			result.X = vector.X * scale.X;
			result.Y = vector.Y * scale.Y;
			result.Z = vector.Z * scale.Z;
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x000278C5 File Offset: 0x00025AC5
		public static Vector3d Divide(Vector3d vector, double scale)
		{
			Vector3d.Divide(vector, scale, out vector);
			return vector;
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x000278D2 File Offset: 0x00025AD2
		public static void Divide(in Vector3d vector, double scale, out Vector3d result)
		{
			result.X = vector.X / scale;
			result.Y = vector.Y / scale;
			result.Z = vector.Z / scale;
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x000278FE File Offset: 0x00025AFE
		public static Vector3d Divide(Vector3d vector, Vector3d scale)
		{
			Vector3d.Divide(vector, scale, out vector);
			return vector;
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x0002790C File Offset: 0x00025B0C
		public static void Divide(in Vector3d vector, in Vector3d scale, out Vector3d result)
		{
			result.X = vector.X / scale.X;
			result.Y = vector.Y / scale.Y;
			result.Z = vector.Z / scale.Z;
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x00027948 File Offset: 0x00025B48
		public static Vector3d ComponentMin(Vector3d a, Vector3d b)
		{
			a.X = ((a.X < b.X) ? a.X : b.X);
			a.Y = ((a.Y < b.Y) ? a.Y : b.Y);
			a.Z = ((a.Z < b.Z) ? a.Z : b.Z);
			return a;
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x000279C0 File Offset: 0x00025BC0
		public static void ComponentMin(in Vector3d a, in Vector3d b, out Vector3d result)
		{
			result.X = ((a.X < b.X) ? a.X : b.X);
			result.Y = ((a.Y < b.Y) ? a.Y : b.Y);
			result.Z = ((a.Z < b.Z) ? a.Z : b.Z);
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00027A34 File Offset: 0x00025C34
		public static Vector3d ComponentMax(Vector3d a, Vector3d b)
		{
			a.X = ((a.X > b.X) ? a.X : b.X);
			a.Y = ((a.Y > b.Y) ? a.Y : b.Y);
			a.Z = ((a.Z > b.Z) ? a.Z : b.Z);
			return a;
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00027AAC File Offset: 0x00025CAC
		public static void ComponentMax(in Vector3d a, in Vector3d b, out Vector3d result)
		{
			result.X = ((a.X > b.X) ? a.X : b.X);
			result.Y = ((a.Y > b.Y) ? a.Y : b.Y);
			result.Z = ((a.Z > b.Z) ? a.Z : b.Z);
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00027B1F File Offset: 0x00025D1F
		public static Vector3d MagnitudeMin(Vector3d left, Vector3d right)
		{
			if (left.LengthSquared >= right.LengthSquared)
			{
				return right;
			}
			return left;
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x00027B34 File Offset: 0x00025D34
		public static void MagnitudeMin(in Vector3d left, in Vector3d right, out Vector3d result)
		{
			result = ((left.LengthSquared < right.LengthSquared) ? left : right);
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x00027B58 File Offset: 0x00025D58
		public static Vector3d MagnitudeMax(Vector3d left, Vector3d right)
		{
			if (left.LengthSquared < right.LengthSquared)
			{
				return right;
			}
			return left;
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x00027B6D File Offset: 0x00025D6D
		public static void MagnitudeMax(in Vector3d left, in Vector3d right, out Vector3d result)
		{
			result = ((left.LengthSquared >= right.LengthSquared) ? left : right);
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x00027B94 File Offset: 0x00025D94
		public static Vector3d Clamp(Vector3d vec, Vector3d min, Vector3d max)
		{
			vec.X = ((vec.X < min.X) ? min.X : ((vec.X > max.X) ? max.X : vec.X));
			vec.Y = ((vec.Y < min.Y) ? min.Y : ((vec.Y > max.Y) ? max.Y : vec.Y));
			vec.Z = ((vec.Z < min.Z) ? min.Z : ((vec.Z > max.Z) ? max.Z : vec.Z));
			return vec;
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x00027C50 File Offset: 0x00025E50
		public static void Clamp(in Vector3d vec, in Vector3d min, in Vector3d max, out Vector3d result)
		{
			result.X = ((vec.X < min.X) ? min.X : ((vec.X > max.X) ? max.X : vec.X));
			result.Y = ((vec.Y < min.Y) ? min.Y : ((vec.Y > max.Y) ? max.Y : vec.Y));
			result.Z = ((vec.Z < min.Z) ? min.Z : ((vec.Z > max.Z) ? max.Z : vec.Z));
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x00027D05 File Offset: 0x00025F05
		public static Vector3d Abs(Vector3d vec)
		{
			vec.X = Math.Abs(vec.X);
			vec.Y = Math.Abs(vec.Y);
			vec.Z = Math.Abs(vec.Z);
			return vec;
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x00027D3E File Offset: 0x00025F3E
		public static void Abs(in Vector3d vec, out Vector3d result)
		{
			result.X = Math.Abs(vec.X);
			result.Y = Math.Abs(vec.Y);
			result.Z = Math.Abs(vec.Z);
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x00027D73 File Offset: 0x00025F73
		public static Vector3d Round(Vector3d vec)
		{
			vec.X = Math.Round(vec.X);
			vec.Y = Math.Round(vec.Y);
			vec.Z = Math.Round(vec.Z);
			return vec;
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x00027DAC File Offset: 0x00025FAC
		public static void Round(in Vector3d vec, out Vector3d result)
		{
			result.X = Math.Round(vec.X);
			result.Y = Math.Round(vec.Y);
			result.Z = Math.Round(vec.Z);
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x00027DE1 File Offset: 0x00025FE1
		public static Vector3d Round(Vector3d vec, MidpointRounding rounding)
		{
			vec.X = Math.Round(vec.X, rounding);
			vec.Y = Math.Round(vec.Y, rounding);
			vec.Z = Math.Round(vec.Z, rounding);
			return vec;
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x00027E1D File Offset: 0x0002601D
		public static void Round(in Vector3d vec, MidpointRounding rounding, out Vector3d result)
		{
			result.X = Math.Round(vec.X, rounding);
			result.Y = Math.Round(vec.Y, rounding);
			result.Z = Math.Round(vec.Z, rounding);
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x00027E55 File Offset: 0x00026055
		public static Vector3d Ceiling(Vector3d vec)
		{
			vec.X = Math.Ceiling(vec.X);
			vec.Y = Math.Ceiling(vec.Y);
			vec.Z = Math.Ceiling(vec.Z);
			return vec;
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x00027E8E File Offset: 0x0002608E
		public static void Ceiling(in Vector3d vec, out Vector3d result)
		{
			result.X = Math.Ceiling(vec.X);
			result.Y = Math.Ceiling(vec.Y);
			result.Z = Math.Ceiling(vec.Z);
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x00027EC3 File Offset: 0x000260C3
		public static Vector3d Floor(Vector3d vec)
		{
			vec.X = Math.Floor(vec.X);
			vec.Y = Math.Floor(vec.Y);
			vec.Z = Math.Floor(vec.Z);
			return vec;
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x00027EFC File Offset: 0x000260FC
		public static void Floor(in Vector3d vec, out Vector3d result)
		{
			result.X = Math.Floor(vec.X);
			result.Y = Math.Floor(vec.Y);
			result.Z = Math.Floor(vec.Z);
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x00027F31 File Offset: 0x00026131
		public static Vector3d Truncate(Vector3d vec)
		{
			vec.X = Math.Truncate(vec.X);
			vec.Y = Math.Truncate(vec.Y);
			vec.Z = Math.Truncate(vec.Z);
			return vec;
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x00027F6A File Offset: 0x0002616A
		public static void Truncate(in Vector3d vec, out Vector3d result)
		{
			result.X = Math.Truncate(vec.X);
			result.Y = Math.Truncate(vec.Y);
			result.Z = Math.Truncate(vec.Z);
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x00027FA0 File Offset: 0x000261A0
		public static double Distance(Vector3d vec1, Vector3d vec2)
		{
			double result;
			Vector3d.Distance(vec1, vec2, out result);
			return result;
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x00027FBC File Offset: 0x000261BC
		public static void Distance(in Vector3d vec1, in Vector3d vec2, out double result)
		{
			result = Math.Sqrt((vec2.X - vec1.X) * (vec2.X - vec1.X) + (vec2.Y - vec1.Y) * (vec2.Y - vec1.Y) + (vec2.Z - vec1.Z) * (vec2.Z - vec1.Z));
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x00028024 File Offset: 0x00026224
		public static double DistanceSquared(Vector3d vec1, Vector3d vec2)
		{
			double result;
			Vector3d.DistanceSquared(vec1, vec2, out result);
			return result;
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x00028040 File Offset: 0x00026240
		public static void DistanceSquared(in Vector3d vec1, in Vector3d vec2, out double result)
		{
			result = (vec2.X - vec1.X) * (vec2.X - vec1.X) + (vec2.Y - vec1.Y) * (vec2.Y - vec1.Y) + (vec2.Z - vec1.Z) * (vec2.Z - vec1.Z);
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x000280A4 File Offset: 0x000262A4
		public static Vector3d Normalize(Vector3d vec)
		{
			double num = 1.0 / vec.Length;
			vec.X *= num;
			vec.Y *= num;
			vec.Z *= num;
			return vec;
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x000280E8 File Offset: 0x000262E8
		public static void Normalize(in Vector3d vec, out Vector3d result)
		{
			double num = MathHelper.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z);
			num = 1.0 / vec.Length;
			result.X = vec.X * num;
			result.Y = vec.Y * num;
			result.Z = vec.Z * num;
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x00028160 File Offset: 0x00026360
		public static Vector3d NormalizeFast(Vector3d vec)
		{
			double num = MathHelper.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z);
			vec.X *= num;
			vec.Y *= num;
			vec.Z *= num;
			return vec;
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x000281C4 File Offset: 0x000263C4
		public static void NormalizeFast(in Vector3d vec, out Vector3d result)
		{
			double num = MathHelper.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z);
			result.X = vec.X * num;
			result.Y = vec.Y * num;
			result.Z = vec.Z * num;
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x0002822A File Offset: 0x0002642A
		public static double Dot(Vector3d left, Vector3d right)
		{
			return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x00028255 File Offset: 0x00026455
		public static void Dot(in Vector3d left, in Vector3d right, out double result)
		{
			result = left.X * right.X + left.Y * right.Y + left.Z * right.Z;
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x00028284 File Offset: 0x00026484
		public static Vector3d Cross(Vector3d left, Vector3d right)
		{
			Vector3d result;
			Vector3d.Cross(left, right, out result);
			return result;
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x000282A0 File Offset: 0x000264A0
		public static void Cross(in Vector3d left, in Vector3d right, out Vector3d result)
		{
			result.X = left.Y * right.Z - left.Z * right.Y;
			result.Y = left.Z * right.X - left.X * right.Z;
			result.Z = left.X * right.Y - left.Y * right.X;
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00028310 File Offset: 0x00026510
		public static Vector3d Lerp(Vector3d a, Vector3d b, double blend)
		{
			a.X = blend * (b.X - a.X) + a.X;
			a.Y = blend * (b.Y - a.Y) + a.Y;
			a.Z = blend * (b.Z - a.Z) + a.Z;
			return a;
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00028378 File Offset: 0x00026578
		public static void Lerp(in Vector3d a, in Vector3d b, double blend, out Vector3d result)
		{
			result.X = blend * (b.X - a.X) + a.X;
			result.Y = blend * (b.Y - a.Y) + a.Y;
			result.Z = blend * (b.Z - a.Z) + a.Z;
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x000283DC File Offset: 0x000265DC
		public static Vector3d Lerp(Vector3d a, Vector3d b, Vector3d blend)
		{
			a.X = blend.X * (b.X - a.X) + a.X;
			a.Y = blend.Y * (b.Y - a.Y) + a.Y;
			a.Z = blend.Z * (b.Z - a.Z) + a.Z;
			return a;
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x00028450 File Offset: 0x00026650
		public static void Lerp(in Vector3d a, in Vector3d b, Vector3d blend, out Vector3d result)
		{
			result.X = blend.X * (b.X - a.X) + a.X;
			result.Y = blend.Y * (b.Y - a.Y) + a.Y;
			result.Z = blend.Z * (b.Z - a.Z) + a.Z;
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x000284C0 File Offset: 0x000266C0
		public static Vector3d Slerp(Vector3d a, Vector3d b, double t)
		{
			double num = a.Length * b.Length;
			double num2;
			if (num == 0.0 || Math.Abs(num2 = Vector3d.Dot(a, b) / num) > 0.99999999)
			{
				return Vector3d.Lerp(a, b, t);
			}
			double num3 = Math.Acos(Math.Clamp(num2, -1.0, 1.0));
			double num4 = Math.Sqrt(1.0 - num2 * num2);
			double scale = Math.Sin((1.0 - t) * num3) / num4;
			double scale2 = Math.Sin(t * num3) / num4;
			return scale * a + scale2 * b;
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x00028574 File Offset: 0x00026774
		public static void Slerp(in Vector3d a, in Vector3d b, double t, out Vector3d result)
		{
			double num = a.Length * b.Length;
			if (num == 0.0)
			{
				Vector3d.Lerp(a, b, t, out result);
				return;
			}
			double num2;
			Vector3d.Dot(a, b, out num2);
			num2 /= num;
			if (Math.Abs(num2) > 0.99999999)
			{
				Vector3d.Lerp(a, b, t, out result);
				return;
			}
			double num3 = Math.Acos(num2);
			double num4 = Math.Sqrt(1.0 - num2 * num2);
			double scale = Math.Sin((1.0 - t) * num3) / num4;
			double scale2 = Math.Sin(t * num3) / num4;
			result = scale * a + scale2 * b;
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x00028630 File Offset: 0x00026830
		public static Vector3d Elerp(Vector3d a, Vector3d b, double t)
		{
			a.X = Math.Pow(a.X, 1.0 - t) * Math.Pow(b.X, t);
			a.Y = Math.Pow(a.Y, 1.0 - t) * Math.Pow(b.Y, t);
			a.Z = Math.Pow(a.Z, 1.0 - t) * Math.Pow(b.Z, t);
			return a;
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x000286BC File Offset: 0x000268BC
		public static void Elerp(in Vector3d a, in Vector3d b, double t, out Vector3d result)
		{
			result.X = Math.Pow(a.X, 1.0 - t) * Math.Pow(b.X, t);
			result.Y = Math.Pow(a.Y, 1.0 - t) * Math.Pow(b.Y, t);
			result.Z = Math.Pow(a.Z, 1.0 - t) * Math.Pow(b.Z, t);
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x00028744 File Offset: 0x00026944
		public static Vector3d BaryCentric(Vector3d a, Vector3d b, Vector3d c, double u, double v)
		{
			Vector3d result;
			Vector3d.BaryCentric(a, b, c, u, v, out result);
			return result;
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x00028764 File Offset: 0x00026964
		public static void BaryCentric(in Vector3d a, in Vector3d b, in Vector3d c, double u, double v, out Vector3d result)
		{
			Vector3d vector3d;
			Vector3d.Subtract(b, a, out vector3d);
			Vector3d vector3d2;
			Vector3d.Multiply(vector3d, u, out vector3d2);
			Vector3d vector3d3;
			Vector3d.Add(a, vector3d2, out vector3d3);
			Vector3d vector3d4;
			Vector3d.Subtract(c, a, out vector3d4);
			Vector3d vector3d5;
			Vector3d.Multiply(vector3d4, v, out vector3d5);
			Vector3d.Add(vector3d3, vector3d5, out result);
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x000287B0 File Offset: 0x000269B0
		public static Vector3d TransformVector(Vector3d vec, Matrix4d mat)
		{
			Vector3d result;
			Vector3d.TransformVector(vec, mat, out result);
			return result;
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x000287CC File Offset: 0x000269CC
		public static void TransformVector(in Vector3d vec, in Matrix4d mat, out Vector3d result)
		{
			result.X = vec.X * mat.Row0.X + vec.Y * mat.Row1.X + vec.Z * mat.Row2.X;
			result.Y = vec.X * mat.Row0.Y + vec.Y * mat.Row1.Y + vec.Z * mat.Row2.Y;
			result.Z = vec.X * mat.Row0.Z + vec.Y * mat.Row1.Z + vec.Z * mat.Row2.Z;
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x00028893 File Offset: 0x00026A93
		public static Vector3d TransformNormal(Vector3d norm, Matrix4d mat)
		{
			mat.Invert();
			return Vector3d.TransformNormalInverse(norm, mat);
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x000288A4 File Offset: 0x00026AA4
		public static void TransformNormal(in Vector3d norm, in Matrix4d mat, out Vector3d result)
		{
			Matrix4d matrix4d = Matrix4d.Invert(mat);
			Vector3d.TransformNormalInverse(norm, matrix4d, out result);
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x000288C4 File Offset: 0x00026AC4
		public static Vector3d TransformNormalInverse(Vector3d norm, Matrix4d invMat)
		{
			Vector3d result;
			Vector3d.TransformNormalInverse(norm, invMat, out result);
			return result;
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x000288E0 File Offset: 0x00026AE0
		public static void TransformNormalInverse(in Vector3d norm, in Matrix4d invMat, out Vector3d result)
		{
			result.X = norm.X * invMat.Row0.X + norm.Y * invMat.Row0.Y + norm.Z * invMat.Row0.Z;
			result.Y = norm.X * invMat.Row1.X + norm.Y * invMat.Row1.Y + norm.Z * invMat.Row1.Z;
			result.Z = norm.X * invMat.Row2.X + norm.Y * invMat.Row2.Y + norm.Z * invMat.Row2.Z;
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x000289A8 File Offset: 0x00026BA8
		public static Vector3d TransformPosition(Vector3d pos, Matrix4d mat)
		{
			Vector3d result;
			Vector3d.TransformPosition(pos, mat, out result);
			return result;
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x000289C4 File Offset: 0x00026BC4
		public static void TransformPosition(in Vector3d pos, in Matrix4d mat, out Vector3d result)
		{
			result.X = pos.X * mat.Row0.X + pos.Y * mat.Row1.X + pos.Z * mat.Row2.X + mat.Row3.X;
			result.Y = pos.X * mat.Row0.Y + pos.Y * mat.Row1.Y + pos.Z * mat.Row2.Y + mat.Row3.Y;
			result.Z = pos.X * mat.Row0.Z + pos.Y * mat.Row1.Z + pos.Z * mat.Row2.Z + mat.Row3.Z;
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x00028AB0 File Offset: 0x00026CB0
		public static Vector3d TransformRow(Vector3d vec, Matrix3d mat)
		{
			Vector3d result;
			Vector3d.TransformRow(vec, mat, out result);
			return result;
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x00028ACC File Offset: 0x00026CCC
		public static void TransformRow(in Vector3d vec, in Matrix3d mat, out Vector3d result)
		{
			result.X = vec.X * mat.Row0.X + vec.Y * mat.Row1.X + vec.Z * mat.Row2.X;
			result.Y = vec.X * mat.Row0.Y + vec.Y * mat.Row1.Y + vec.Z * mat.Row2.Y;
			result.Z = vec.X * mat.Row0.Z + vec.Y * mat.Row1.Z + vec.Z * mat.Row2.Z;
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x00028B94 File Offset: 0x00026D94
		public static Vector3d Transform(Vector3d vec, Quaterniond quat)
		{
			Vector3d result;
			Vector3d.Transform(vec, quat, out result);
			return result;
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x00028BB0 File Offset: 0x00026DB0
		public static void Transform(in Vector3d vec, in Quaterniond quat, out Vector3d result)
		{
			Vector3d xyz = quat.Xyz;
			Vector3d vector3d;
			Vector3d.Cross(xyz, vec, out vector3d);
			Vector3d vector3d2;
			Vector3d.Multiply(vec, quat.W, out vector3d2);
			Vector3d.Add(vector3d, vector3d2, out vector3d);
			Vector3d.Cross(xyz, vector3d, out vector3d2);
			Vector3d.Multiply(vector3d2, 2.0, out vector3d2);
			Vector3d.Add(vec, vector3d2, out result);
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x00028C10 File Offset: 0x00026E10
		public static Vector3d TransformColumn(Matrix3d mat, Vector3d vec)
		{
			Vector3d result;
			Vector3d.TransformColumn(mat, vec, out result);
			return result;
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x00028C2C File Offset: 0x00026E2C
		public static void TransformColumn(in Matrix3d mat, in Vector3d vec, out Vector3d result)
		{
			result.X = mat.Row0.X * vec.X + mat.Row0.Y * vec.Y + mat.Row0.Z * vec.Z;
			result.Y = mat.Row1.X * vec.X + mat.Row1.Y * vec.Y + mat.Row1.Z * vec.Z;
			result.Z = mat.Row2.X * vec.X + mat.Row2.Y * vec.Y + mat.Row2.Z * vec.Z;
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x00028CF4 File Offset: 0x00026EF4
		public static Vector3d TransformPerspective(Vector3d vec, Matrix4d mat)
		{
			Vector3d result;
			Vector3d.TransformPerspective(vec, mat, out result);
			return result;
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x00028D10 File Offset: 0x00026F10
		public static void TransformPerspective(in Vector3d vec, in Matrix4d mat, out Vector3d result)
		{
			Vector4d vector4d = new Vector4d(vec.X, vec.Y, vec.Z, 1.0);
			Vector4d.TransformRow(vector4d, mat, out vector4d);
			result.X = vector4d.X / vector4d.W;
			result.Y = vector4d.Y / vector4d.W;
			result.Z = vector4d.Z / vector4d.W;
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x00028D84 File Offset: 0x00026F84
		public static double CalculateAngle(Vector3d first, Vector3d second)
		{
			double result;
			Vector3d.CalculateAngle(first, second, out result);
			return result;
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x00028DA0 File Offset: 0x00026FA0
		public static void CalculateAngle(in Vector3d first, in Vector3d second, out double result)
		{
			double num;
			Vector3d.Dot(first, second, out num);
			result = Math.Acos(MathHelper.Clamp(num / (first.Length * second.Length), -1.0, 1.0));
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x00028DE3 File Offset: 0x00026FE3
		// (set) Token: 0x06000AC5 RID: 2757 RVA: 0x00028DF0 File Offset: 0x00026FF0
		[XmlIgnore]
		public unsafe Vector2d Xy
		{
			get
			{
				return *Unsafe.As<Vector3d, Vector2d>(ref this);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x00028E0A File Offset: 0x0002700A
		// (set) Token: 0x06000AC7 RID: 2759 RVA: 0x00028E1D File Offset: 0x0002701D
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

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x00028E37 File Offset: 0x00027037
		// (set) Token: 0x06000AC9 RID: 2761 RVA: 0x00028E4A File Offset: 0x0002704A
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

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000ACA RID: 2762 RVA: 0x00028E64 File Offset: 0x00027064
		// (set) Token: 0x06000ACB RID: 2763 RVA: 0x00028E77 File Offset: 0x00027077
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

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000ACC RID: 2764 RVA: 0x00028E91 File Offset: 0x00027091
		// (set) Token: 0x06000ACD RID: 2765 RVA: 0x00028EA4 File Offset: 0x000270A4
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

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000ACE RID: 2766 RVA: 0x00028EBE File Offset: 0x000270BE
		// (set) Token: 0x06000ACF RID: 2767 RVA: 0x00028ED1 File Offset: 0x000270D1
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

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x00028EEB File Offset: 0x000270EB
		// (set) Token: 0x06000AD1 RID: 2769 RVA: 0x00028F04 File Offset: 0x00027104
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

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000AD2 RID: 2770 RVA: 0x00028F2A File Offset: 0x0002712A
		// (set) Token: 0x06000AD3 RID: 2771 RVA: 0x00028F43 File Offset: 0x00027143
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

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x00028F69 File Offset: 0x00027169
		// (set) Token: 0x06000AD5 RID: 2773 RVA: 0x00028F82 File Offset: 0x00027182
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

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x00028FA8 File Offset: 0x000271A8
		// (set) Token: 0x06000AD7 RID: 2775 RVA: 0x00028FC1 File Offset: 0x000271C1
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

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x00028FE7 File Offset: 0x000271E7
		// (set) Token: 0x06000AD9 RID: 2777 RVA: 0x00029000 File Offset: 0x00027200
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

		// Token: 0x06000ADA RID: 2778 RVA: 0x00029026 File Offset: 0x00027226
		public static Vector3d operator +(Vector3d left, Vector3d right)
		{
			left.X += right.X;
			left.Y += right.Y;
			left.Z += right.Z;
			return left;
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0002905C File Offset: 0x0002725C
		public static Vector3d operator -(Vector3d left, Vector3d right)
		{
			left.X -= right.X;
			left.Y -= right.Y;
			left.Z -= right.Z;
			return left;
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x00029092 File Offset: 0x00027292
		public static Vector3d operator -(Vector3d vec)
		{
			vec.X = -vec.X;
			vec.Y = -vec.Y;
			vec.Z = -vec.Z;
			return vec;
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x000290BF File Offset: 0x000272BF
		public static Vector3d operator *(Vector3d vec, double scale)
		{
			vec.X *= scale;
			vec.Y *= scale;
			vec.Z *= scale;
			return vec;
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x000290E6 File Offset: 0x000272E6
		public static Vector3d operator *(double scale, Vector3d vec)
		{
			vec.X *= scale;
			vec.Y *= scale;
			vec.Z *= scale;
			return vec;
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x0002910D File Offset: 0x0002730D
		public static Vector3d operator *(Vector3d vec, Vector3d scale)
		{
			vec.X *= scale.X;
			vec.Y *= scale.Y;
			vec.Z *= scale.Z;
			return vec;
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x00029144 File Offset: 0x00027344
		public static Vector3d operator *(Vector3d vec, Matrix3d mat)
		{
			Vector3d result;
			Vector3d.TransformRow(vec, mat, out result);
			return result;
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x00029160 File Offset: 0x00027360
		public static Vector3d operator *(Matrix3d mat, Vector3d vec)
		{
			Vector3d result;
			Vector3d.TransformColumn(mat, vec, out result);
			return result;
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x0002917C File Offset: 0x0002737C
		public static Vector3d operator *(Quaterniond quat, Vector3d vec)
		{
			Vector3d result;
			Vector3d.Transform(vec, quat, out result);
			return result;
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x00029195 File Offset: 0x00027395
		public static Vector3d operator /(Vector3d vec, double scale)
		{
			vec.X /= scale;
			vec.Y /= scale;
			vec.Z /= scale;
			return vec;
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x000291BC File Offset: 0x000273BC
		public static Vector3d operator /(Vector3d vec, Vector3d scale)
		{
			vec.X /= scale.X;
			vec.Y /= scale.Y;
			vec.Z /= scale.Z;
			return vec;
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x000291F2 File Offset: 0x000273F2
		public static bool operator ==(Vector3d left, Vector3d right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x000291FC File Offset: 0x000273FC
		public static bool operator !=(Vector3d left, Vector3d right)
		{
			return !(left == right);
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x00029208 File Offset: 0x00027408
		public static explicit operator Vector3(Vector3d vec)
		{
			return new Vector3((float)vec.X, (float)vec.Y, (float)vec.Z);
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x00029224 File Offset: 0x00027424
		public static explicit operator Vector3h(Vector3d vec)
		{
			return new Vector3h(new Half(vec.X), new Half(vec.Y), new Half(vec.Z));
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0002924C File Offset: 0x0002744C
		public static explicit operator Vector3i(Vector3d vec)
		{
			return new Vector3i((int)vec.X, (int)vec.Y, (int)vec.Z);
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x00029268 File Offset: 0x00027468
		public static implicit operator Vector3d([TupleElementNames(new string[]
		{
			"X",
			"Y",
			"Z"
		})] ValueTuple<double, double, double> values)
		{
			return new Vector3d(values.Item1, values.Item2, values.Item3);
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x00029281 File Offset: 0x00027481
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x0002928B File Offset: 0x0002748B
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x00029295 File Offset: 0x00027495
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x000292A0 File Offset: 0x000274A0
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

		// Token: 0x06000AEF RID: 2799 RVA: 0x000292F6 File Offset: 0x000274F6
		public override bool Equals(object obj)
		{
			return obj is Vector3d && this.Equals((Vector3d)obj);
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x0002930E File Offset: 0x0002750E
		public readonly bool Equals(Vector3d other)
		{
			return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0002933C File Offset: 0x0002753C
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<double, double, double>(this.X, this.Y, this.Z);
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x00029355 File Offset: 0x00027555
		public readonly void Deconstruct(out double x, out double y, out double z)
		{
			x = this.X;
			y = this.Y;
			z = this.Z;
		}

		// Token: 0x040000AD RID: 173
		public double X;

		// Token: 0x040000AE RID: 174
		public double Y;

		// Token: 0x040000AF RID: 175
		public double Z;

		// Token: 0x040000B0 RID: 176
		public static readonly Vector3d UnitX = new Vector3d(1.0, 0.0, 0.0);

		// Token: 0x040000B1 RID: 177
		public static readonly Vector3d UnitY = new Vector3d(0.0, 1.0, 0.0);

		// Token: 0x040000B2 RID: 178
		public static readonly Vector3d UnitZ = new Vector3d(0.0, 0.0, 1.0);

		// Token: 0x040000B3 RID: 179
		public static readonly Vector3d Zero = new Vector3d(0.0, 0.0, 0.0);

		// Token: 0x040000B4 RID: 180
		public static readonly Vector3d One = new Vector3d(1.0, 1.0, 1.0);

		// Token: 0x040000B5 RID: 181
		public static readonly Vector3d PositiveInfinity = new Vector3d(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);

		// Token: 0x040000B6 RID: 182
		public static readonly Vector3d NegativeInfinity = new Vector3d(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);

		// Token: 0x040000B7 RID: 183
		public static readonly int SizeInBytes = Unsafe.SizeOf<Vector3d>();
	}
}
