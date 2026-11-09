using System;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace OpenTK.Mathematics
{
	// Token: 0x0200002D RID: 45
	[Serializable]
	public struct Vector4i : IEquatable<Vector4i>, IFormattable
	{
		// Token: 0x06000DFB RID: 3579 RVA: 0x0003234F File Offset: 0x0003054F
		public Vector4i(int value)
		{
			this.X = value;
			this.Y = value;
			this.Z = value;
			this.W = value;
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0003236D File Offset: 0x0003056D
		public Vector4i(int x, int y, int z, int w)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0003238C File Offset: 0x0003058C
		public Vector4i(Vector2i v)
		{
			this.X = v.X;
			this.Y = v.Y;
			this.Z = 0;
			this.W = 0;
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x000323B4 File Offset: 0x000305B4
		public Vector4i(Vector2i v1, Vector2i v2)
		{
			this.X = v1.X;
			this.Y = v1.Y;
			this.Z = v2.X;
			this.W = v2.Y;
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x000323E6 File Offset: 0x000305E6
		public Vector4i(Vector3i v)
		{
			this.X = v.X;
			this.Y = v.Y;
			this.Z = v.Z;
			this.W = 0;
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x00032413 File Offset: 0x00030613
		public Vector4i(Vector3i v, int w)
		{
			this.X = v.X;
			this.Y = v.Y;
			this.Z = v.Z;
			this.W = w;
		}

		// Token: 0x170002E8 RID: 744
		public unsafe int this[int index]
		{
			readonly get
			{
				if (index < 4)
				{
					return *this.GetElementUnsafe(this, index);
				}
				throw new IndexOutOfRangeException("You tried to access this vector at index: " + index.ToString());
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

		// Token: 0x06000E03 RID: 3587 RVA: 0x0003248D File Offset: 0x0003068D
		[MethodImpl(256)]
		private readonly ref int GetElementUnsafe(in Vector4i v, int index)
		{
			return Unsafe.Add<int>(Unsafe.AsRef<int>(ref v.X), index);
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000E04 RID: 3588 RVA: 0x000324A0 File Offset: 0x000306A0
		public readonly int ManhattanLength
		{
			get
			{
				return Math.Abs(this.X) + Math.Abs(this.Y) + Math.Abs(this.Z) + Math.Abs(this.W);
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000E05 RID: 3589 RVA: 0x000324D1 File Offset: 0x000306D1
		public readonly int EuclideanLengthSquared
		{
			get
			{
				return this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W;
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000E06 RID: 3590 RVA: 0x0003250A File Offset: 0x0003070A
		public readonly float EuclideanLength
		{
			get
			{
				return MathF.Sqrt((float)(this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W));
			}
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0003254C File Offset: 0x0003074C
		public readonly Vector4i Abs()
		{
			Vector4i vector4i = this;
			vector4i.X = Math.Abs(vector4i.X);
			vector4i.Y = Math.Abs(vector4i.Y);
			vector4i.Z = Math.Abs(vector4i.Z);
			vector4i.W = Math.Abs(vector4i.W);
			return vector4i;
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x000325A9 File Offset: 0x000307A9
		public static Vector4i Add(Vector4i a, Vector4i b)
		{
			Vector4i.Add(a, b, out a);
			return a;
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x000325B8 File Offset: 0x000307B8
		public static void Add(in Vector4i a, in Vector4i b, out Vector4i result)
		{
			result.X = a.X + b.X;
			result.Y = a.Y + b.Y;
			result.Z = a.Z + b.Z;
			result.W = a.W + b.W;
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x00032611 File Offset: 0x00030811
		public static Vector4i Subtract(Vector4i a, Vector4i b)
		{
			Vector4i.Subtract(a, b, out a);
			return a;
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x00032620 File Offset: 0x00030820
		public static void Subtract(in Vector4i a, in Vector4i b, out Vector4i result)
		{
			result.X = a.X - b.X;
			result.Y = a.Y - b.Y;
			result.Z = a.Z - b.Z;
			result.W = a.W - b.W;
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x00032679 File Offset: 0x00030879
		public static Vector4i Multiply(Vector4i vector, int scale)
		{
			Vector4i.Multiply(vector, scale, out vector);
			return vector;
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x00032686 File Offset: 0x00030886
		public static void Multiply(in Vector4i vector, int scale, out Vector4i result)
		{
			result.X = vector.X * scale;
			result.Y = vector.Y * scale;
			result.Z = vector.Z * scale;
			result.W = vector.W * scale;
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x000326C0 File Offset: 0x000308C0
		public static Vector4i Multiply(Vector4i vector, Vector4i scale)
		{
			Vector4i.Multiply(vector, scale, out vector);
			return vector;
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x000326D0 File Offset: 0x000308D0
		public static void Multiply(in Vector4i vector, in Vector4i scale, out Vector4i result)
		{
			result.X = vector.X * scale.X;
			result.Y = vector.Y * scale.Y;
			result.Z = vector.Z * scale.Z;
			result.W = vector.W * scale.W;
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00032729 File Offset: 0x00030929
		public static Vector4i Divide(Vector4i vector, int scale)
		{
			Vector4i.Divide(vector, scale, out vector);
			return vector;
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x00032736 File Offset: 0x00030936
		public static void Divide(in Vector4i vector, int scale, out Vector4i result)
		{
			result.X = vector.X / scale;
			result.Y = vector.Y / scale;
			result.Z = vector.Z / scale;
			result.W = vector.W / scale;
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x00032770 File Offset: 0x00030970
		public static Vector4i Divide(Vector4i vector, Vector4i scale)
		{
			Vector4i.Divide(vector, scale, out vector);
			return vector;
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x00032780 File Offset: 0x00030980
		public static void Divide(in Vector4i vector, in Vector4i scale, out Vector4i result)
		{
			result.X = vector.X / scale.X;
			result.Y = vector.Y / scale.Y;
			result.Z = vector.Z / scale.Z;
			result.W = vector.W / scale.W;
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x000327DC File Offset: 0x000309DC
		public static Vector4i ComponentMin(Vector4i a, Vector4i b)
		{
			Vector4i result;
			result.X = Math.Min(a.X, b.X);
			result.Y = Math.Min(a.Y, b.Y);
			result.Z = Math.Min(a.Z, b.Z);
			result.W = Math.Min(a.W, b.W);
			return result;
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0003284C File Offset: 0x00030A4C
		public static void ComponentMin(in Vector4i a, in Vector4i b, out Vector4i result)
		{
			result.X = Math.Min(a.X, b.X);
			result.Y = Math.Min(a.Y, b.Y);
			result.Z = Math.Min(a.Z, b.Z);
			result.W = Math.Min(a.W, b.W);
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x000328B8 File Offset: 0x00030AB8
		public static Vector4i ComponentMax(Vector4i a, Vector4i b)
		{
			Vector4i result;
			result.X = Math.Max(a.X, b.X);
			result.Y = Math.Max(a.Y, b.Y);
			result.Z = Math.Max(a.Z, b.Z);
			result.W = Math.Max(a.W, b.W);
			return result;
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x00032928 File Offset: 0x00030B28
		public static void ComponentMax(in Vector4i a, in Vector4i b, out Vector4i result)
		{
			result.X = Math.Max(a.X, b.X);
			result.Y = Math.Max(a.Y, b.Y);
			result.Z = Math.Max(a.Z, b.Z);
			result.W = Math.Max(a.W, b.W);
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x00032994 File Offset: 0x00030B94
		public static Vector4i Clamp(Vector4i vec, Vector4i min, Vector4i max)
		{
			Vector4i result;
			result.X = MathHelper.Clamp(vec.X, min.X, max.X);
			result.Y = MathHelper.Clamp(vec.Y, min.Y, max.Y);
			result.Z = MathHelper.Clamp(vec.Z, min.Z, max.Z);
			result.W = MathHelper.Clamp(vec.W, min.W, max.W);
			return result;
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x00032A1C File Offset: 0x00030C1C
		public static void Clamp(in Vector4i vec, in Vector4i min, in Vector4i max, out Vector4i result)
		{
			result.X = MathHelper.Clamp(vec.X, min.X, max.X);
			result.Y = MathHelper.Clamp(vec.Y, min.Y, max.Y);
			result.Z = MathHelper.Clamp(vec.Z, min.Z, max.Z);
			result.W = MathHelper.Clamp(vec.W, min.W, max.W);
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x00032AA0 File Offset: 0x00030CA0
		public static Vector4i Abs(Vector4i vec)
		{
			vec.X = Math.Abs(vec.X);
			vec.Y = Math.Abs(vec.Y);
			vec.Z = Math.Abs(vec.Z);
			vec.W = Math.Abs(vec.W);
			return vec;
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x00032AF8 File Offset: 0x00030CF8
		public static void Abs(in Vector4i vec, out Vector4i result)
		{
			result.X = Math.Abs(vec.X);
			result.Y = Math.Abs(vec.Y);
			result.Z = Math.Abs(vec.Z);
			result.W = Math.Abs(vec.W);
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000E1C RID: 3612 RVA: 0x00032B49 File Offset: 0x00030D49
		// (set) Token: 0x06000E1D RID: 3613 RVA: 0x00032B56 File Offset: 0x00030D56
		[XmlIgnore]
		public unsafe Vector2i Xy
		{
			get
			{
				return *Unsafe.As<Vector4i, Vector2i>(ref this);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000E1E RID: 3614 RVA: 0x00032B70 File Offset: 0x00030D70
		// (set) Token: 0x06000E1F RID: 3615 RVA: 0x00032B83 File Offset: 0x00030D83
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

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000E20 RID: 3616 RVA: 0x00032B9D File Offset: 0x00030D9D
		// (set) Token: 0x06000E21 RID: 3617 RVA: 0x00032BB0 File Offset: 0x00030DB0
		[XmlIgnore]
		public Vector2i Xw
		{
			readonly get
			{
				return new Vector2i(this.X, this.W);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000E22 RID: 3618 RVA: 0x00032BCA File Offset: 0x00030DCA
		// (set) Token: 0x06000E23 RID: 3619 RVA: 0x00032BDD File Offset: 0x00030DDD
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

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000E24 RID: 3620 RVA: 0x00032BF7 File Offset: 0x00030DF7
		// (set) Token: 0x06000E25 RID: 3621 RVA: 0x00032C0A File Offset: 0x00030E0A
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

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000E26 RID: 3622 RVA: 0x00032C24 File Offset: 0x00030E24
		// (set) Token: 0x06000E27 RID: 3623 RVA: 0x00032C37 File Offset: 0x00030E37
		[XmlIgnore]
		public Vector2i Yw
		{
			readonly get
			{
				return new Vector2i(this.Y, this.W);
			}
			set
			{
				this.Y = value.X;
				this.W = value.Y;
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000E28 RID: 3624 RVA: 0x00032C51 File Offset: 0x00030E51
		// (set) Token: 0x06000E29 RID: 3625 RVA: 0x00032C64 File Offset: 0x00030E64
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

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000E2A RID: 3626 RVA: 0x00032C7E File Offset: 0x00030E7E
		// (set) Token: 0x06000E2B RID: 3627 RVA: 0x00032C91 File Offset: 0x00030E91
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

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000E2C RID: 3628 RVA: 0x00032CAB File Offset: 0x00030EAB
		// (set) Token: 0x06000E2D RID: 3629 RVA: 0x00032CBE File Offset: 0x00030EBE
		[XmlIgnore]
		public Vector2i Zw
		{
			readonly get
			{
				return new Vector2i(this.Z, this.W);
			}
			set
			{
				this.Z = value.X;
				this.W = value.Y;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000E2E RID: 3630 RVA: 0x00032CD8 File Offset: 0x00030ED8
		// (set) Token: 0x06000E2F RID: 3631 RVA: 0x00032CEB File Offset: 0x00030EEB
		[XmlIgnore]
		public Vector2i Wx
		{
			readonly get
			{
				return new Vector2i(this.W, this.X);
			}
			set
			{
				this.W = value.X;
				this.X = value.Y;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000E30 RID: 3632 RVA: 0x00032D05 File Offset: 0x00030F05
		// (set) Token: 0x06000E31 RID: 3633 RVA: 0x00032D18 File Offset: 0x00030F18
		[XmlIgnore]
		public Vector2i Wy
		{
			readonly get
			{
				return new Vector2i(this.W, this.Y);
			}
			set
			{
				this.W = value.X;
				this.Y = value.Y;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000E32 RID: 3634 RVA: 0x00032D32 File Offset: 0x00030F32
		// (set) Token: 0x06000E33 RID: 3635 RVA: 0x00032D45 File Offset: 0x00030F45
		[XmlIgnore]
		public Vector2i Wz
		{
			readonly get
			{
				return new Vector2i(this.W, this.Z);
			}
			set
			{
				this.W = value.X;
				this.Z = value.Y;
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000E34 RID: 3636 RVA: 0x00032D5F File Offset: 0x00030F5F
		// (set) Token: 0x06000E35 RID: 3637 RVA: 0x00032D6C File Offset: 0x00030F6C
		[XmlIgnore]
		public unsafe Vector3i Xyz
		{
			get
			{
				return *Unsafe.As<Vector4i, Vector3i>(ref this);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000E36 RID: 3638 RVA: 0x00032D92 File Offset: 0x00030F92
		// (set) Token: 0x06000E37 RID: 3639 RVA: 0x00032DAB File Offset: 0x00030FAB
		[XmlIgnore]
		public Vector3i Xyw
		{
			readonly get
			{
				return new Vector3i(this.X, this.Y, this.W);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000E38 RID: 3640 RVA: 0x00032DD1 File Offset: 0x00030FD1
		// (set) Token: 0x06000E39 RID: 3641 RVA: 0x00032DEA File Offset: 0x00030FEA
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

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000E3A RID: 3642 RVA: 0x00032E10 File Offset: 0x00031010
		// (set) Token: 0x06000E3B RID: 3643 RVA: 0x00032E29 File Offset: 0x00031029
		[XmlIgnore]
		public Vector3i Xzw
		{
			readonly get
			{
				return new Vector3i(this.X, this.Z, this.W);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000E3C RID: 3644 RVA: 0x00032E4F File Offset: 0x0003104F
		// (set) Token: 0x06000E3D RID: 3645 RVA: 0x00032E68 File Offset: 0x00031068
		[XmlIgnore]
		public Vector3i Xwy
		{
			readonly get
			{
				return new Vector3i(this.X, this.W, this.Y);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000E3E RID: 3646 RVA: 0x00032E8E File Offset: 0x0003108E
		// (set) Token: 0x06000E3F RID: 3647 RVA: 0x00032EA7 File Offset: 0x000310A7
		[XmlIgnore]
		public Vector3i Xwz
		{
			readonly get
			{
				return new Vector3i(this.X, this.W, this.Z);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000E40 RID: 3648 RVA: 0x00032ECD File Offset: 0x000310CD
		// (set) Token: 0x06000E41 RID: 3649 RVA: 0x00032EE6 File Offset: 0x000310E6
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

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000E42 RID: 3650 RVA: 0x00032F0C File Offset: 0x0003110C
		// (set) Token: 0x06000E43 RID: 3651 RVA: 0x00032F25 File Offset: 0x00031125
		[XmlIgnore]
		public Vector3i Yxw
		{
			readonly get
			{
				return new Vector3i(this.Y, this.X, this.W);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000E44 RID: 3652 RVA: 0x00032F4B File Offset: 0x0003114B
		// (set) Token: 0x06000E45 RID: 3653 RVA: 0x00032F64 File Offset: 0x00031164
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

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000E46 RID: 3654 RVA: 0x00032F8A File Offset: 0x0003118A
		// (set) Token: 0x06000E47 RID: 3655 RVA: 0x00032FA3 File Offset: 0x000311A3
		[XmlIgnore]
		public Vector3i Yzw
		{
			readonly get
			{
				return new Vector3i(this.Y, this.Z, this.W);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000E48 RID: 3656 RVA: 0x00032FC9 File Offset: 0x000311C9
		// (set) Token: 0x06000E49 RID: 3657 RVA: 0x00032FE2 File Offset: 0x000311E2
		[XmlIgnore]
		public Vector3i Ywx
		{
			readonly get
			{
				return new Vector3i(this.Y, this.W, this.X);
			}
			set
			{
				this.Y = value.X;
				this.W = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000E4A RID: 3658 RVA: 0x00033008 File Offset: 0x00031208
		// (set) Token: 0x06000E4B RID: 3659 RVA: 0x00033021 File Offset: 0x00031221
		[XmlIgnore]
		public Vector3i Ywz
		{
			readonly get
			{
				return new Vector3i(this.Y, this.W, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.W = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000E4C RID: 3660 RVA: 0x00033047 File Offset: 0x00031247
		// (set) Token: 0x06000E4D RID: 3661 RVA: 0x00033060 File Offset: 0x00031260
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

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000E4E RID: 3662 RVA: 0x00033086 File Offset: 0x00031286
		// (set) Token: 0x06000E4F RID: 3663 RVA: 0x0003309F File Offset: 0x0003129F
		[XmlIgnore]
		public Vector3i Zxw
		{
			readonly get
			{
				return new Vector3i(this.Z, this.X, this.W);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000E50 RID: 3664 RVA: 0x000330C5 File Offset: 0x000312C5
		// (set) Token: 0x06000E51 RID: 3665 RVA: 0x000330DE File Offset: 0x000312DE
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

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000E52 RID: 3666 RVA: 0x00033104 File Offset: 0x00031304
		// (set) Token: 0x06000E53 RID: 3667 RVA: 0x0003311D File Offset: 0x0003131D
		[XmlIgnore]
		public Vector3i Zyw
		{
			readonly get
			{
				return new Vector3i(this.Z, this.Y, this.W);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
				this.W = value.Z;
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000E54 RID: 3668 RVA: 0x00033143 File Offset: 0x00031343
		// (set) Token: 0x06000E55 RID: 3669 RVA: 0x0003315C File Offset: 0x0003135C
		[XmlIgnore]
		public Vector3i Zwx
		{
			readonly get
			{
				return new Vector3i(this.Z, this.W, this.X);
			}
			set
			{
				this.Z = value.X;
				this.W = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000E56 RID: 3670 RVA: 0x00033182 File Offset: 0x00031382
		// (set) Token: 0x06000E57 RID: 3671 RVA: 0x0003319B File Offset: 0x0003139B
		[XmlIgnore]
		public Vector3i Zwy
		{
			readonly get
			{
				return new Vector3i(this.Z, this.W, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.W = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000E58 RID: 3672 RVA: 0x000331C1 File Offset: 0x000313C1
		// (set) Token: 0x06000E59 RID: 3673 RVA: 0x000331DA File Offset: 0x000313DA
		[XmlIgnore]
		public Vector3i Wxy
		{
			readonly get
			{
				return new Vector3i(this.W, this.X, this.Y);
			}
			set
			{
				this.W = value.X;
				this.X = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000E5A RID: 3674 RVA: 0x00033200 File Offset: 0x00031400
		// (set) Token: 0x06000E5B RID: 3675 RVA: 0x00033219 File Offset: 0x00031419
		[XmlIgnore]
		public Vector3i Wxz
		{
			readonly get
			{
				return new Vector3i(this.W, this.X, this.Z);
			}
			set
			{
				this.W = value.X;
				this.X = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000E5C RID: 3676 RVA: 0x0003323F File Offset: 0x0003143F
		// (set) Token: 0x06000E5D RID: 3677 RVA: 0x00033258 File Offset: 0x00031458
		[XmlIgnore]
		public Vector3i Wyx
		{
			readonly get
			{
				return new Vector3i(this.W, this.Y, this.X);
			}
			set
			{
				this.W = value.X;
				this.Y = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000E5E RID: 3678 RVA: 0x0003327E File Offset: 0x0003147E
		// (set) Token: 0x06000E5F RID: 3679 RVA: 0x00033297 File Offset: 0x00031497
		[XmlIgnore]
		public Vector3i Wyz
		{
			readonly get
			{
				return new Vector3i(this.W, this.Y, this.Z);
			}
			set
			{
				this.W = value.X;
				this.Y = value.Y;
				this.Z = value.Z;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000E60 RID: 3680 RVA: 0x000332BD File Offset: 0x000314BD
		// (set) Token: 0x06000E61 RID: 3681 RVA: 0x000332D6 File Offset: 0x000314D6
		[XmlIgnore]
		public Vector3i Wzx
		{
			readonly get
			{
				return new Vector3i(this.W, this.Z, this.X);
			}
			set
			{
				this.W = value.X;
				this.Z = value.Y;
				this.X = value.Z;
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000E62 RID: 3682 RVA: 0x000332FC File Offset: 0x000314FC
		// (set) Token: 0x06000E63 RID: 3683 RVA: 0x00033315 File Offset: 0x00031515
		[XmlIgnore]
		public Vector3i Wzy
		{
			readonly get
			{
				return new Vector3i(this.W, this.Z, this.Y);
			}
			set
			{
				this.W = value.X;
				this.Z = value.Y;
				this.Y = value.Z;
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000E64 RID: 3684 RVA: 0x0003333B File Offset: 0x0003153B
		// (set) Token: 0x06000E65 RID: 3685 RVA: 0x0003335A File Offset: 0x0003155A
		[XmlIgnore]
		public Vector4i Xywz
		{
			readonly get
			{
				return new Vector4i(this.X, this.Y, this.W, this.Z);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
				this.W = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000E66 RID: 3686 RVA: 0x0003338C File Offset: 0x0003158C
		// (set) Token: 0x06000E67 RID: 3687 RVA: 0x000333AB File Offset: 0x000315AB
		[XmlIgnore]
		public Vector4i Xzyw
		{
			readonly get
			{
				return new Vector4i(this.X, this.Z, this.Y, this.W);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
				this.Y = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000E68 RID: 3688 RVA: 0x000333DD File Offset: 0x000315DD
		// (set) Token: 0x06000E69 RID: 3689 RVA: 0x000333FC File Offset: 0x000315FC
		[XmlIgnore]
		public Vector4i Xzwy
		{
			readonly get
			{
				return new Vector4i(this.X, this.Z, this.W, this.Y);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
				this.W = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000E6A RID: 3690 RVA: 0x0003342E File Offset: 0x0003162E
		// (set) Token: 0x06000E6B RID: 3691 RVA: 0x0003344D File Offset: 0x0003164D
		[XmlIgnore]
		public Vector4i Xwyz
		{
			readonly get
			{
				return new Vector4i(this.X, this.W, this.Y, this.Z);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
				this.Y = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000E6C RID: 3692 RVA: 0x0003347F File Offset: 0x0003167F
		// (set) Token: 0x06000E6D RID: 3693 RVA: 0x0003349E File Offset: 0x0003169E
		[XmlIgnore]
		public Vector4i Xwzy
		{
			readonly get
			{
				return new Vector4i(this.X, this.W, this.Z, this.Y);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
				this.Z = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000E6E RID: 3694 RVA: 0x000334D0 File Offset: 0x000316D0
		// (set) Token: 0x06000E6F RID: 3695 RVA: 0x000334EF File Offset: 0x000316EF
		[XmlIgnore]
		public Vector4i Yxzw
		{
			readonly get
			{
				return new Vector4i(this.Y, this.X, this.Z, this.W);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
				this.Z = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000E70 RID: 3696 RVA: 0x00033521 File Offset: 0x00031721
		// (set) Token: 0x06000E71 RID: 3697 RVA: 0x00033540 File Offset: 0x00031740
		[XmlIgnore]
		public Vector4i Yxwz
		{
			readonly get
			{
				return new Vector4i(this.Y, this.X, this.W, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.X = value.Y;
				this.W = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000E72 RID: 3698 RVA: 0x00033572 File Offset: 0x00031772
		// (set) Token: 0x06000E73 RID: 3699 RVA: 0x00033591 File Offset: 0x00031791
		[XmlIgnore]
		public Vector4i Yyzw
		{
			readonly get
			{
				return new Vector4i(this.Y, this.Y, this.Z, this.W);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
				this.Z = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000E74 RID: 3700 RVA: 0x000335C3 File Offset: 0x000317C3
		// (set) Token: 0x06000E75 RID: 3701 RVA: 0x000335E2 File Offset: 0x000317E2
		[XmlIgnore]
		public Vector4i Yywz
		{
			readonly get
			{
				return new Vector4i(this.Y, this.Y, this.W, this.Z);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
				this.W = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x00033614 File Offset: 0x00031814
		// (set) Token: 0x06000E77 RID: 3703 RVA: 0x00033633 File Offset: 0x00031833
		[XmlIgnore]
		public Vector4i Yzxw
		{
			readonly get
			{
				return new Vector4i(this.Y, this.Z, this.X, this.W);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
				this.X = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000E78 RID: 3704 RVA: 0x00033665 File Offset: 0x00031865
		// (set) Token: 0x06000E79 RID: 3705 RVA: 0x00033684 File Offset: 0x00031884
		[XmlIgnore]
		public Vector4i Yzwx
		{
			readonly get
			{
				return new Vector4i(this.Y, this.Z, this.W, this.X);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
				this.W = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000E7A RID: 3706 RVA: 0x000336B6 File Offset: 0x000318B6
		// (set) Token: 0x06000E7B RID: 3707 RVA: 0x000336D5 File Offset: 0x000318D5
		[XmlIgnore]
		public Vector4i Ywxz
		{
			readonly get
			{
				return new Vector4i(this.Y, this.W, this.X, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.W = value.Y;
				this.X = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000E7C RID: 3708 RVA: 0x00033707 File Offset: 0x00031907
		// (set) Token: 0x06000E7D RID: 3709 RVA: 0x00033726 File Offset: 0x00031926
		[XmlIgnore]
		public Vector4i Ywzx
		{
			readonly get
			{
				return new Vector4i(this.Y, this.W, this.Z, this.X);
			}
			set
			{
				this.Y = value.X;
				this.W = value.Y;
				this.Z = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000E7E RID: 3710 RVA: 0x00033758 File Offset: 0x00031958
		// (set) Token: 0x06000E7F RID: 3711 RVA: 0x00033777 File Offset: 0x00031977
		[XmlIgnore]
		public Vector4i Zxyw
		{
			readonly get
			{
				return new Vector4i(this.Z, this.X, this.Y, this.W);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
				this.Y = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000E80 RID: 3712 RVA: 0x000337A9 File Offset: 0x000319A9
		// (set) Token: 0x06000E81 RID: 3713 RVA: 0x000337C8 File Offset: 0x000319C8
		[XmlIgnore]
		public Vector4i Zxwy
		{
			readonly get
			{
				return new Vector4i(this.Z, this.X, this.W, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.X = value.Y;
				this.W = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x000337FA File Offset: 0x000319FA
		// (set) Token: 0x06000E83 RID: 3715 RVA: 0x00033819 File Offset: 0x00031A19
		[XmlIgnore]
		public Vector4i Zyxw
		{
			readonly get
			{
				return new Vector4i(this.Z, this.Y, this.X, this.W);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
				this.X = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000E84 RID: 3716 RVA: 0x0003384B File Offset: 0x00031A4B
		// (set) Token: 0x06000E85 RID: 3717 RVA: 0x0003386A File Offset: 0x00031A6A
		[XmlIgnore]
		public Vector4i Zywx
		{
			readonly get
			{
				return new Vector4i(this.Z, this.Y, this.W, this.X);
			}
			set
			{
				this.Z = value.X;
				this.Y = value.Y;
				this.W = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x0003389C File Offset: 0x00031A9C
		// (set) Token: 0x06000E87 RID: 3719 RVA: 0x000338BB File Offset: 0x00031ABB
		[XmlIgnore]
		public Vector4i Zwxy
		{
			readonly get
			{
				return new Vector4i(this.Z, this.W, this.X, this.Y);
			}
			set
			{
				this.Z = value.X;
				this.W = value.Y;
				this.X = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000E88 RID: 3720 RVA: 0x000338ED File Offset: 0x00031AED
		// (set) Token: 0x06000E89 RID: 3721 RVA: 0x0003390C File Offset: 0x00031B0C
		[XmlIgnore]
		public Vector4i Zwyx
		{
			readonly get
			{
				return new Vector4i(this.Z, this.W, this.Y, this.X);
			}
			set
			{
				this.Z = value.X;
				this.W = value.Y;
				this.Y = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x0003393E File Offset: 0x00031B3E
		// (set) Token: 0x06000E8B RID: 3723 RVA: 0x0003395D File Offset: 0x00031B5D
		[XmlIgnore]
		public Vector4i Zwzy
		{
			readonly get
			{
				return new Vector4i(this.Z, this.W, this.Z, this.Y);
			}
			set
			{
				this.X = value.X;
				this.W = value.Y;
				this.Z = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000E8C RID: 3724 RVA: 0x0003398F File Offset: 0x00031B8F
		// (set) Token: 0x06000E8D RID: 3725 RVA: 0x000339AE File Offset: 0x00031BAE
		[XmlIgnore]
		public Vector4i Wxyz
		{
			readonly get
			{
				return new Vector4i(this.W, this.X, this.Y, this.Z);
			}
			set
			{
				this.W = value.X;
				this.X = value.Y;
				this.Y = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000E8E RID: 3726 RVA: 0x000339E0 File Offset: 0x00031BE0
		// (set) Token: 0x06000E8F RID: 3727 RVA: 0x000339FF File Offset: 0x00031BFF
		[XmlIgnore]
		public Vector4i Wxzy
		{
			readonly get
			{
				return new Vector4i(this.W, this.X, this.Z, this.Y);
			}
			set
			{
				this.W = value.X;
				this.X = value.Y;
				this.Z = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000E90 RID: 3728 RVA: 0x00033A31 File Offset: 0x00031C31
		// (set) Token: 0x06000E91 RID: 3729 RVA: 0x00033A50 File Offset: 0x00031C50
		[XmlIgnore]
		public Vector4i Wyxz
		{
			readonly get
			{
				return new Vector4i(this.W, this.Y, this.X, this.Z);
			}
			set
			{
				this.W = value.X;
				this.Y = value.Y;
				this.X = value.Z;
				this.Z = value.W;
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000E92 RID: 3730 RVA: 0x00033A82 File Offset: 0x00031C82
		// (set) Token: 0x06000E93 RID: 3731 RVA: 0x00033AA1 File Offset: 0x00031CA1
		[XmlIgnore]
		public Vector4i Wyzx
		{
			readonly get
			{
				return new Vector4i(this.W, this.Y, this.Z, this.X);
			}
			set
			{
				this.W = value.X;
				this.Y = value.Y;
				this.Z = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000E94 RID: 3732 RVA: 0x00033AD3 File Offset: 0x00031CD3
		// (set) Token: 0x06000E95 RID: 3733 RVA: 0x00033AF2 File Offset: 0x00031CF2
		[XmlIgnore]
		public Vector4i Wzxy
		{
			readonly get
			{
				return new Vector4i(this.W, this.Z, this.X, this.Y);
			}
			set
			{
				this.W = value.X;
				this.Z = value.Y;
				this.X = value.Z;
				this.Y = value.W;
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000E96 RID: 3734 RVA: 0x00033B24 File Offset: 0x00031D24
		// (set) Token: 0x06000E97 RID: 3735 RVA: 0x00033B43 File Offset: 0x00031D43
		[XmlIgnore]
		public Vector4i Wzyx
		{
			readonly get
			{
				return new Vector4i(this.W, this.Z, this.Y, this.X);
			}
			set
			{
				this.W = value.X;
				this.Z = value.Y;
				this.Y = value.Z;
				this.X = value.W;
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000E98 RID: 3736 RVA: 0x00033B75 File Offset: 0x00031D75
		// (set) Token: 0x06000E99 RID: 3737 RVA: 0x00033B94 File Offset: 0x00031D94
		[XmlIgnore]
		public Vector4i Wzyw
		{
			readonly get
			{
				return new Vector4i(this.W, this.Z, this.Y, this.W);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
				this.Y = value.Z;
				this.W = value.W;
			}
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00033BC6 File Offset: 0x00031DC6
		public readonly Vector4 ToVector4()
		{
			return new Vector4((float)this.X, (float)this.Y, (float)this.Z, (float)this.W);
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00033BE9 File Offset: 0x00031DE9
		public static void ToVector4(in Vector4i input, out Vector4 result)
		{
			result.X = (float)input.X;
			result.Y = (float)input.Y;
			result.Z = (float)input.Z;
			result.W = (float)input.W;
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00033C20 File Offset: 0x00031E20
		public static Vector4i operator +(Vector4i left, Vector4i right)
		{
			left.X += right.X;
			left.Y += right.Y;
			left.Z += right.Z;
			left.W += right.W;
			return left;
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x00033C74 File Offset: 0x00031E74
		public static Vector4i operator -(Vector4i left, Vector4i right)
		{
			left.X -= right.X;
			left.Y -= right.Y;
			left.Z -= right.Z;
			left.W -= right.W;
			return left;
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x00033CC6 File Offset: 0x00031EC6
		public static Vector4i operator -(Vector4i vec)
		{
			vec.X = -vec.X;
			vec.Y = -vec.Y;
			vec.Z = -vec.Z;
			vec.W = -vec.W;
			return vec;
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x00033D01 File Offset: 0x00031F01
		public static Vector4i operator *(Vector4i vec, int scale)
		{
			vec.X *= scale;
			vec.Y *= scale;
			vec.Z *= scale;
			vec.W *= scale;
			return vec;
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x00033D34 File Offset: 0x00031F34
		public static Vector4i operator *(int scale, Vector4i vec)
		{
			vec.X *= scale;
			vec.Y *= scale;
			vec.Z *= scale;
			vec.W *= scale;
			return vec;
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x00033D68 File Offset: 0x00031F68
		public static Vector4i operator *(Vector4i vec, Vector4i scale)
		{
			vec.X *= scale.X;
			vec.Y *= scale.Y;
			vec.Z *= scale.Z;
			vec.W *= scale.W;
			return vec;
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x00033DBA File Offset: 0x00031FBA
		public static Vector4i operator /(Vector4i vec, int scale)
		{
			vec.X /= scale;
			vec.Y /= scale;
			vec.Z /= scale;
			vec.W /= scale;
			return vec;
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x00033DF0 File Offset: 0x00031FF0
		public static Vector4i operator /(Vector4i vec, Vector4i scale)
		{
			vec.X /= scale.X;
			vec.Y /= scale.Y;
			vec.Z /= scale.Z;
			vec.W /= scale.W;
			return vec;
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x00033E42 File Offset: 0x00032042
		public static bool operator ==(Vector4i left, Vector4i right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x00033E4C File Offset: 0x0003204C
		public static bool operator !=(Vector4i left, Vector4i right)
		{
			return !(left == right);
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x00033E58 File Offset: 0x00032058
		public unsafe static explicit operator int*(Vector4i v)
		{
			return &v.X;
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x00033E62 File Offset: 0x00032062
		public unsafe static explicit operator IntPtr(Vector4i v)
		{
			return (IntPtr)((void*)(&v.X));
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x00033E71 File Offset: 0x00032071
		public static implicit operator Vector4(Vector4i vec)
		{
			return new Vector4((float)vec.X, (float)vec.Y, (float)vec.Z, (float)vec.W);
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x00033E94 File Offset: 0x00032094
		public static implicit operator Vector4d(Vector4i vec)
		{
			return new Vector4d((double)vec.X, (double)vec.Y, (double)vec.Z, (double)vec.W);
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x00033EB7 File Offset: 0x000320B7
		public static explicit operator Vector4h(Vector4i vec)
		{
			return new Vector4h((float)vec.X, (float)vec.Y, (float)vec.Z, (float)vec.W);
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x00033EDA File Offset: 0x000320DA
		public static implicit operator Vector4i([TupleElementNames(new string[]
		{
			"X",
			"Y",
			"Z",
			"W"
		})] ValueTuple<int, int, int, int> values)
		{
			return new Vector4i(values.Item1, values.Item2, values.Item3, values.Item4);
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x00033EF9 File Offset: 0x000320F9
		public override readonly string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x00033F03 File Offset: 0x00032103
		public readonly string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x00033F0D File Offset: 0x0003210D
		public readonly string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x00033F18 File Offset: 0x00032118
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

		// Token: 0x06000EB0 RID: 3760 RVA: 0x00033F7E File Offset: 0x0003217E
		public override readonly bool Equals(object obj)
		{
			return obj is Vector4i && this.Equals((Vector4i)obj);
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x00033F96 File Offset: 0x00032196
		public readonly bool Equals(Vector4i other)
		{
			return this.X == other.X && this.Y == other.Y && this.Z == other.Z && this.W == other.W;
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x00033FD2 File Offset: 0x000321D2
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<int, int, int, int>(this.X, this.Y, this.Z, this.W);
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x00033FF1 File Offset: 0x000321F1
		public readonly void Deconstruct(out int x, out int y, out int z, out int w)
		{
			x = this.X;
			y = this.Y;
			z = this.Z;
			w = this.W;
		}

		// Token: 0x040000E4 RID: 228
		public int X;

		// Token: 0x040000E5 RID: 229
		public int Y;

		// Token: 0x040000E6 RID: 230
		public int Z;

		// Token: 0x040000E7 RID: 231
		public int W;

		// Token: 0x040000E8 RID: 232
		public static readonly Vector4i UnitX = new Vector4i(1, 0, 0, 0);

		// Token: 0x040000E9 RID: 233
		public static readonly Vector4i UnitY = new Vector4i(0, 1, 0, 0);

		// Token: 0x040000EA RID: 234
		public static readonly Vector4i UnitZ = new Vector4i(0, 0, 1, 0);

		// Token: 0x040000EB RID: 235
		public static readonly Vector4i UnitW = new Vector4i(0, 0, 0, 1);

		// Token: 0x040000EC RID: 236
		public static readonly Vector4i Zero = new Vector4i(0, 0, 0, 0);

		// Token: 0x040000ED RID: 237
		public static readonly Vector4i One = new Vector4i(1, 1, 1, 1);

		// Token: 0x040000EE RID: 238
		public static readonly int SizeInBytes = Unsafe.SizeOf<Vector4i>();
	}
}
