using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace OpenTK.Mathematics
{
	// Token: 0x02000025 RID: 37
	[Serializable]
	public struct Vector2i : IEquatable<Vector2i>, IFormattable
	{
		// Token: 0x06000996 RID: 2454 RVA: 0x00024C08 File Offset: 0x00022E08
		public Vector2i(int value)
		{
			this.X = value;
			this.Y = value;
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x00024C18 File Offset: 0x00022E18
		public Vector2i(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		// Token: 0x170001E0 RID: 480
		public unsafe int this[int index]
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

		// Token: 0x0600099A RID: 2458 RVA: 0x00024C75 File Offset: 0x00022E75
		[MethodImpl(256)]
		private readonly ref int GetElementUnsafe(in Vector2i v, int index)
		{
			return Unsafe.Add<int>(Unsafe.AsRef<int>(ref v.X), index);
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x0600099B RID: 2459 RVA: 0x00024C88 File Offset: 0x00022E88
		public readonly int ManhattanLength
		{
			get
			{
				return Math.Abs(this.X) + Math.Abs(this.Y);
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600099C RID: 2460 RVA: 0x00024CA1 File Offset: 0x00022EA1
		public readonly int EuclideanLengthSquared
		{
			get
			{
				return this.X * this.X + this.Y * this.Y;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600099D RID: 2461 RVA: 0x00024CBE File Offset: 0x00022EBE
		public readonly float EuclideanLength
		{
			get
			{
				return MathF.Sqrt((float)(this.X * this.X + this.Y * this.Y));
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x0600099E RID: 2462 RVA: 0x00024CE1 File Offset: 0x00022EE1
		public readonly Vector2i PerpendicularRight
		{
			get
			{
				return new Vector2i(this.Y, -this.X);
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x0600099F RID: 2463 RVA: 0x00024CF5 File Offset: 0x00022EF5
		public readonly Vector2i PerpendicularLeft
		{
			get
			{
				return new Vector2i(-this.Y, this.X);
			}
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x00024D0C File Offset: 0x00022F0C
		public readonly Vector2i Abs()
		{
			Vector2i vector2i = this;
			vector2i.X = Math.Abs(vector2i.X);
			vector2i.Y = Math.Abs(vector2i.Y);
			return vector2i;
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x00024D45 File Offset: 0x00022F45
		public static Vector2i Add(Vector2i a, Vector2i b)
		{
			Vector2i.Add(a, b, out a);
			return a;
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x00024D53 File Offset: 0x00022F53
		public static void Add(in Vector2i a, in Vector2i b, out Vector2i result)
		{
			result.X = a.X + b.X;
			result.Y = a.Y + b.Y;
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x00024D7B File Offset: 0x00022F7B
		public static Vector2i Subtract(Vector2i a, Vector2i b)
		{
			Vector2i.Subtract(a, b, out a);
			return a;
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x00024D89 File Offset: 0x00022F89
		public static void Subtract(in Vector2i a, in Vector2i b, out Vector2i result)
		{
			result.X = a.X - b.X;
			result.Y = a.Y - b.Y;
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x00024DB1 File Offset: 0x00022FB1
		public static Vector2i Multiply(Vector2i vector, int scale)
		{
			Vector2i.Multiply(vector, scale, out vector);
			return vector;
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x00024DBE File Offset: 0x00022FBE
		public static void Multiply(in Vector2i vector, int scale, out Vector2i result)
		{
			result.X = vector.X * scale;
			result.Y = vector.Y * scale;
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x00024DDC File Offset: 0x00022FDC
		public static Vector2i Multiply(Vector2i vector, Vector2i scale)
		{
			Vector2i.Multiply(vector, scale, out vector);
			return vector;
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x00024DEA File Offset: 0x00022FEA
		public static void Multiply(in Vector2i vector, in Vector2i scale, out Vector2i result)
		{
			result.X = vector.X * scale.X;
			result.Y = vector.Y * scale.Y;
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x00024E12 File Offset: 0x00023012
		public static Vector2i Divide(Vector2i vector, int scale)
		{
			Vector2i.Divide(vector, scale, out vector);
			return vector;
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x00024E1F File Offset: 0x0002301F
		public static void Divide(in Vector2i vector, int scale, out Vector2i result)
		{
			result.X = vector.X / scale;
			result.Y = vector.Y / scale;
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x00024E3D File Offset: 0x0002303D
		public static Vector2i Divide(Vector2i vector, Vector2i scale)
		{
			Vector2i.Divide(vector, scale, out vector);
			return vector;
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x00024E4B File Offset: 0x0002304B
		public static void Divide(in Vector2i vector, in Vector2i scale, out Vector2i result)
		{
			result.X = vector.X / scale.X;
			result.Y = vector.Y / scale.Y;
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x00024E73 File Offset: 0x00023073
		public static Vector2i ComponentMin(Vector2i a, Vector2i b)
		{
			a.X = Math.Min(a.X, b.X);
			a.Y = Math.Min(a.Y, b.Y);
			return a;
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x00024EA6 File Offset: 0x000230A6
		public static void ComponentMin(in Vector2i a, in Vector2i b, out Vector2i result)
		{
			result.X = Math.Min(a.X, b.X);
			result.Y = Math.Min(a.Y, b.Y);
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x00024ED6 File Offset: 0x000230D6
		public static Vector2i ComponentMax(Vector2i a, Vector2i b)
		{
			a.X = Math.Max(a.X, b.X);
			a.Y = Math.Max(a.Y, b.Y);
			return a;
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x00024F09 File Offset: 0x00023109
		public static void ComponentMax(in Vector2i a, in Vector2i b, out Vector2i result)
		{
			result.X = Math.Max(a.X, b.X);
			result.Y = Math.Max(a.Y, b.Y);
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x00024F39 File Offset: 0x00023139
		public static Vector2i Clamp(Vector2i vec, Vector2i min, Vector2i max)
		{
			vec.X = MathHelper.Clamp(vec.X, min.X, max.X);
			vec.Y = MathHelper.Clamp(vec.Y, min.Y, max.Y);
			return vec;
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x00024F78 File Offset: 0x00023178
		public static void Clamp(in Vector2i vec, in Vector2i min, in Vector2i max, out Vector2i result)
		{
			result.X = MathHelper.Clamp(vec.X, min.X, max.X);
			result.Y = MathHelper.Clamp(vec.Y, min.Y, max.Y);
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x00024FB4 File Offset: 0x000231B4
		public static Vector2i Abs(Vector2i vec)
		{
			vec.X = Math.Abs(vec.X);
			vec.Y = Math.Abs(vec.Y);
			return vec;
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x00024FDB File Offset: 0x000231DB
		public static void Abs(in Vector2i vec, out Vector2i result)
		{
			result.X = Math.Abs(vec.X);
			result.Y = Math.Abs(vec.Y);
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060009B5 RID: 2485 RVA: 0x00024FFF File Offset: 0x000231FF
		// (set) Token: 0x060009B6 RID: 2486 RVA: 0x00025012 File Offset: 0x00023212
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

		// Token: 0x060009B7 RID: 2487 RVA: 0x0002502C File Offset: 0x0002322C
		public readonly Vector2 ToVector2()
		{
			return new Vector2((float)this.X, (float)this.Y);
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x00025041 File Offset: 0x00023241
		public static void ToVector2(in Vector2i input, out Vector2 result)
		{
			result.X = (float)input.X;
			result.Y = (float)input.Y;
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0002505D File Offset: 0x0002325D
		public static Vector2i operator +(Vector2i left, Vector2i right)
		{
			left.X += right.X;
			left.Y += right.Y;
			return left;
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x00025082 File Offset: 0x00023282
		public static Vector2i operator -(Vector2i left, Vector2i right)
		{
			left.X -= right.X;
			left.Y -= right.Y;
			return left;
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x000250A7 File Offset: 0x000232A7
		public static Vector2i operator -(Vector2i vec)
		{
			vec.X = -vec.X;
			vec.Y = -vec.Y;
			return vec;
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x000250C6 File Offset: 0x000232C6
		public static Vector2i operator *(Vector2i vec, int scale)
		{
			vec.X *= scale;
			vec.Y *= scale;
			return vec;
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x000250E1 File Offset: 0x000232E1
		public static Vector2i operator *(int scale, Vector2i vec)
		{
			vec.X *= scale;
			vec.Y *= scale;
			return vec;
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x000250FC File Offset: 0x000232FC
		public static Vector2i operator *(Vector2i vec, Vector2i scale)
		{
			vec.X *= scale.X;
			vec.Y *= scale.Y;
			return vec;
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x00025121 File Offset: 0x00023321
		public static Vector2i operator /(Vector2i vec, int scale)
		{
			vec.X /= scale;
			vec.Y /= scale;
			return vec;
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0002513C File Offset: 0x0002333C
		public static Vector2i operator /(Vector2i vec, Vector2i scale)
		{
			vec.X /= scale.X;
			vec.Y /= scale.Y;
			return vec;
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x00025161 File Offset: 0x00023361
		public static bool operator ==(Vector2i left, Vector2i right)
		{
			return left.Equals(right);
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0002516B File Offset: 0x0002336B
		public static bool operator !=(Vector2i left, Vector2i right)
		{
			return !(left == right);
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x00025177 File Offset: 0x00023377
		public static implicit operator Vector2(Vector2i vec)
		{
			return new Vector2((float)vec.X, (float)vec.Y);
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0002518C File Offset: 0x0002338C
		public static implicit operator Vector2d(Vector2i vec)
		{
			return new Vector2d((double)vec.X, (double)vec.Y);
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x000251A1 File Offset: 0x000233A1
		public static explicit operator Vector2h(Vector2i vec)
		{
			return new Vector2h((float)vec.X, (float)vec.Y);
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x000251B6 File Offset: 0x000233B6
		public static implicit operator Vector2i([TupleElementNames(new string[]
		{
			"X",
			"Y"
		})] ValueTuple<int, int> values)
		{
			return new Vector2i(values.Item1, values.Item2);
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x000251C9 File Offset: 0x000233C9
		public static explicit operator Point(Vector2i vec)
		{
			return new Point(vec.X, vec.Y);
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x000251DC File Offset: 0x000233DC
		public static explicit operator Size(Vector2i vec)
		{
			return new Size(vec.X, vec.Y);
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x000251EF File Offset: 0x000233EF
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x000251F9 File Offset: 0x000233F9
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x00025203 File Offset: 0x00023403
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0002520D File Offset: 0x0002340D
		public readonly string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("({0}{2} {1})", this.X.ToString(format, formatProvider), this.Y.ToString(format, formatProvider), MathHelper.GetListSeparator(formatProvider));
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x00025239 File Offset: 0x00023439
		public override bool Equals(object obj)
		{
			return obj is Vector2i && this.Equals((Vector2i)obj);
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x00025251 File Offset: 0x00023451
		public readonly bool Equals(Vector2i other)
		{
			return this.X == other.X && this.Y == other.Y;
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x00025271 File Offset: 0x00023471
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<int, int>(this.X, this.Y);
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x00025284 File Offset: 0x00023484
		public readonly void Deconstruct(out int x, out int y)
		{
			x = this.X;
			y = this.Y;
		}

		// Token: 0x0400009B RID: 155
		public int X;

		// Token: 0x0400009C RID: 156
		public int Y;

		// Token: 0x0400009D RID: 157
		public static readonly Vector2i UnitX = new Vector2i(1, 0);

		// Token: 0x0400009E RID: 158
		public static readonly Vector2i UnitY = new Vector2i(0, 1);

		// Token: 0x0400009F RID: 159
		public static readonly Vector2i Zero = new Vector2i(0, 0);

		// Token: 0x040000A0 RID: 160
		public static readonly Vector2i One = new Vector2i(1, 1);

		// Token: 0x040000A1 RID: 161
		public static readonly int SizeInBytes = Unsafe.SizeOf<Vector2i>();
	}
}
