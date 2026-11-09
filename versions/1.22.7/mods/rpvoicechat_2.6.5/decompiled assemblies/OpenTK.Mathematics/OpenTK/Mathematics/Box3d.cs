using System;
using System.Xml.Serialization;

namespace OpenTK.Mathematics
{
	// Token: 0x0200000D RID: 13
	[Serializable]
	public struct Box3d : IEquatable<Box3d>, IFormattable
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x00007CDF File Offset: 0x00005EDF
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x00007CE8 File Offset: 0x00005EE8
		public Vector3d Min
		{
			readonly get
			{
				return this._min;
			}
			set
			{
				if (value.X > this._max.X)
				{
					this._max.X = value.X;
				}
				if (value.Y > this._max.Y)
				{
					this._max.Y = value.Y;
				}
				if (value.Z > this._max.Z)
				{
					this._max.Z = value.Z;
				}
				this._min = value;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x00007D68 File Offset: 0x00005F68
		// (set) Token: 0x060001EA RID: 490 RVA: 0x00007D70 File Offset: 0x00005F70
		public Vector3d Max
		{
			readonly get
			{
				return this._max;
			}
			set
			{
				if (value.X < this._min.X)
				{
					this._min.X = value.X;
				}
				if (value.Y < this._min.Y)
				{
					this._min.Y = value.Y;
				}
				if (value.Z < this._min.Z)
				{
					this._min.Z = value.Z;
				}
				this._max = value;
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00007DF0 File Offset: 0x00005FF0
		public Box3d(Vector3d min, Vector3d max)
		{
			this._min = Vector3d.ComponentMin(min, max);
			this._max = Vector3d.ComponentMax(min, max);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00007E0C File Offset: 0x0000600C
		public Box3d(double minX, double minY, double minZ, double maxX, double maxY, double maxZ)
		{
			this = new Box3d(new Vector3d(minX, minY, minZ), new Vector3d(maxX, maxY, maxZ));
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060001ED RID: 493 RVA: 0x00007E27 File Offset: 0x00006027
		// (set) Token: 0x060001EE RID: 494 RVA: 0x00007E3C File Offset: 0x0000603C
		[XmlIgnore]
		public Vector3d Size
		{
			get
			{
				return this.Max - this.Min;
			}
			set
			{
				Vector3d center = this.Center;
				this._min = center - value * 0.5;
				this._max = center + value * 0.5;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060001EF RID: 495 RVA: 0x00007E86 File Offset: 0x00006086
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x00007E9C File Offset: 0x0000609C
		[XmlIgnore]
		public Vector3d HalfSize
		{
			get
			{
				return this.Size / 2.0;
			}
			set
			{
				this.Size = value * 2.0;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x00007EB3 File Offset: 0x000060B3
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x00007EC6 File Offset: 0x000060C6
		[XmlIgnore]
		public Vector3d Center
		{
			get
			{
				return this.HalfSize + this._min;
			}
			set
			{
				this.Translate(value - this.Center);
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00007EDC File Offset: 0x000060DC
		[Obsolete("This function excludes borders even though it's documentation says otherwise. Use ContainsInclusive and ContainsExclusive for the desired behaviour.")]
		public readonly bool Contains(Vector3d point)
		{
			return this._min.X < point.X && point.X < this._max.X && this._min.Y < point.Y && point.Y < this._max.Y && this._min.Z < point.Z && point.Z < this._max.Z;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00007F60 File Offset: 0x00006160
		public readonly bool ContainsInclusive(Vector3d point)
		{
			return this._min.X <= point.X && point.X <= this._max.X && this._min.Y <= point.Y && point.Y <= this._max.Y && this._min.Z <= point.Z && point.Z <= this._max.Z;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00007FE4 File Offset: 0x000061E4
		public readonly bool ContainsExclusive(Vector3d point)
		{
			return this._min.X < point.X && point.X < this._max.X && this._min.Y < point.Y && point.Y < this._max.Y && this._min.Z < point.Z && point.Z < this._max.Z;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00008065 File Offset: 0x00006265
		public bool Contains(Vector3d point, bool boundaryInclusive)
		{
			if (boundaryInclusive)
			{
				return this.ContainsInclusive(point);
			}
			return this.ContainsExclusive(point);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000807C File Offset: 0x0000627C
		public readonly bool Contains(Box3d other)
		{
			return this._max.X >= other._min.X && this._min.X <= other._max.X && this._max.Y >= other._min.Y && this._min.Y <= other._max.Y && this._max.Z >= other._min.Z && this._min.Z <= other._max.Z;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00008120 File Offset: 0x00006320
		public readonly double DistanceToNearestEdge(Vector3d point)
		{
			Vector3d vector3d = new Vector3d(Math.Max(0.0, Math.Max(this._min.X - point.X, point.X - this._max.X)), Math.Max(0.0, Math.Max(this._min.Y - point.Y, point.Y - this._max.Y)), Math.Max(0.0, Math.Max(this._min.Z - point.Z, point.Z - this._max.Z)));
			return vector3d.Length;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x000081E0 File Offset: 0x000063E0
		public void Translate(Vector3d distance)
		{
			this._min += distance;
			this._max += distance;
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00008208 File Offset: 0x00006408
		public readonly Box3d Translated(Vector3d distance)
		{
			Box3d result = this;
			result.Translate(distance);
			return result;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00008225 File Offset: 0x00006425
		public void Scale(Vector3d scale, Vector3d anchor)
		{
			this._min = anchor + (this._min - anchor) * scale;
			this._max = anchor + (this._max - anchor) * scale;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00008264 File Offset: 0x00006464
		public readonly Box3d Scaled(Vector3d scale, Vector3d anchor)
		{
			Box3d result = this;
			result.Scale(scale, anchor);
			return result;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00008282 File Offset: 0x00006482
		public void Inflate(Vector3d size)
		{
			size = Vector3d.ComponentMax(size, -this.HalfSize);
			this._min -= size;
			this._max += size;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x000082BC File Offset: 0x000064BC
		public readonly Box3d Inflated(Vector3d size)
		{
			Box3d result = this;
			result.Inflate(size);
			return result;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x000082D9 File Offset: 0x000064D9
		public void Extend(Vector3d point)
		{
			this._min = Vector3d.ComponentMin(this._min, point);
			this._max = Vector3d.ComponentMax(this._max, point);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00008300 File Offset: 0x00006500
		public readonly Box3d Extended(Vector3d point)
		{
			Box3d result = this;
			result.Extend(point);
			return result;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000831D File Offset: 0x0000651D
		public static bool operator ==(Box3d left, Box3d right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00008327 File Offset: 0x00006527
		public static bool operator !=(Box3d left, Box3d right)
		{
			return !(left == right);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00008333 File Offset: 0x00006533
		public override bool Equals(object obj)
		{
			return obj is Box3d && this.Equals((Box3d)obj);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000834B File Offset: 0x0000654B
		public bool Equals(Box3d other)
		{
			return this._min.Equals(other._min) && this._max.Equals(other._max);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00008373 File Offset: 0x00006573
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector3d, Vector3d>(this._min, this._max);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00008386 File Offset: 0x00006586
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00008390 File Offset: 0x00006590
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000839A File Offset: 0x0000659A
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x000083A4 File Offset: 0x000065A4
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return this.Min.ToString(format, formatProvider) + " - " + this.Max.ToString(format, formatProvider);
		}

		// Token: 0x04000025 RID: 37
		private Vector3d _min;

		// Token: 0x04000026 RID: 38
		private Vector3d _max;
	}
}
