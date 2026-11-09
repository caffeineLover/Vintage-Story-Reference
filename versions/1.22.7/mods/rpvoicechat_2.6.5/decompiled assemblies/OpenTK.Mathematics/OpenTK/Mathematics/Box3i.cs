using System;
using System.Xml.Serialization;

namespace OpenTK.Mathematics
{
	// Token: 0x0200000E RID: 14
	[Serializable]
	public struct Box3i : IEquatable<Box3i>, IFormattable
	{
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600020A RID: 522 RVA: 0x000083DB File Offset: 0x000065DB
		// (set) Token: 0x0600020B RID: 523 RVA: 0x000083E3 File Offset: 0x000065E3
		public Vector3i Min
		{
			readonly get
			{
				return this._min;
			}
			set
			{
				this._max = Vector3i.ComponentMax(this._max, value);
				this._min = value;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600020C RID: 524 RVA: 0x000083FE File Offset: 0x000065FE
		// (set) Token: 0x0600020D RID: 525 RVA: 0x00008406 File Offset: 0x00006606
		public Vector3i Max
		{
			readonly get
			{
				return this._max;
			}
			set
			{
				this._min = Vector3i.ComponentMin(this._min, value);
				this._max = value;
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00008421 File Offset: 0x00006621
		public Box3i(Vector3i min, Vector3i max)
		{
			this._min = Vector3i.ComponentMin(min, max);
			this._max = Vector3i.ComponentMax(min, max);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000843D File Offset: 0x0000663D
		public Box3i(int minX, int minY, int minZ, int maxX, int maxY, int maxZ)
		{
			this = new Box3i(new Vector3i(minX, minY, minZ), new Vector3i(maxX, maxY, maxZ));
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000210 RID: 528 RVA: 0x00008458 File Offset: 0x00006658
		[XmlIgnore]
		public Vector3i Size
		{
			get
			{
				return this.Max - this.Min;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000846B File Offset: 0x0000666B
		// (set) Token: 0x06000212 RID: 530 RVA: 0x0000847C File Offset: 0x0000667C
		[XmlIgnore]
		public Vector3i HalfSize
		{
			get
			{
				return this.Size / 2;
			}
			set
			{
				Vector3i left = new Vector3i((int)this.Center.X, (int)this.Center.Y, (int)this.Center.Z);
				this._min = left - value;
				this._max = left + value;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000213 RID: 531 RVA: 0x000084D0 File Offset: 0x000066D0
		[XmlIgnore]
		public readonly Vector3 Center
		{
			get
			{
				return this._min + (this._max - this._min).ToVector3() * 0.5f;
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00008510 File Offset: 0x00006710
		[Obsolete("This function excludes borders even though it's documentation says otherwise. Use ContainsInclusive and ContainsExclusive for the desired behaviour.")]
		public readonly bool Contains(Vector3i point)
		{
			return this._min.X < point.X && point.X < this._max.X && this._min.Y < point.Y && point.Y < this._max.Y && this._min.Z < point.Z && point.Z < this._max.Z;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00008594 File Offset: 0x00006794
		public readonly bool ContainsInclusive(Vector3i point)
		{
			return this._min.X <= point.X && point.X <= this._max.X && this._min.Y <= point.Y && point.Y <= this._max.Y && this._min.Z <= point.Z && point.Z <= this._max.Z;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00008618 File Offset: 0x00006818
		public readonly bool ContainsExclusive(Vector3i point)
		{
			return this._min.X < point.X && point.X < this._max.X && this._min.Y < point.Y && point.Y < this._max.Y && this._min.Z < point.Z && point.Z < this._max.Z;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00008699 File Offset: 0x00006899
		public bool Contains(Vector3i point, bool boundaryInclusive)
		{
			if (boundaryInclusive)
			{
				return this.ContainsInclusive(point);
			}
			return this.ContainsExclusive(point);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x000086B0 File Offset: 0x000068B0
		public readonly bool Contains(Box3i other)
		{
			return this._max.X >= other._min.X && this._min.X <= other._max.X && this._max.Y >= other._min.Y && this._min.Y <= other._max.Y && this._max.Z >= other._min.Z && this._min.Z <= other._max.Z;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00008754 File Offset: 0x00006954
		public readonly float DistanceToNearestEdge(Vector3i point)
		{
			Vector3 vector = new Vector3(Math.Max(0f, (float)Math.Max(this._min.X - point.X, point.X - this._max.X)), Math.Max(0f, (float)Math.Max(this._min.Y - point.Y, point.Y - this._max.Y)), Math.Max(0f, (float)Math.Max(this._min.Z - point.Z, point.Z - this._max.Z)));
			return vector.Length;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000880B File Offset: 0x00006A0B
		public void Translate(Vector3i distance)
		{
			this._min += distance;
			this._max += distance;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00008834 File Offset: 0x00006A34
		public readonly Box3i Translated(Vector3i distance)
		{
			Box3i result = this;
			result.Translate(distance);
			return result;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00008851 File Offset: 0x00006A51
		public void Scale(Vector3i scale, Vector3i anchor)
		{
			this._min = anchor + (this._min - anchor) * scale;
			this._max = anchor + (this._max - anchor) * scale;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00008890 File Offset: 0x00006A90
		public readonly Box3i Scaled(Vector3i scale, Vector3i anchor)
		{
			Box3i result = this;
			result.Scale(scale, anchor);
			return result;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x000088AE File Offset: 0x00006AAE
		public void Inflate(Vector3i size)
		{
			size = Vector3i.ComponentMax(size, -this.HalfSize);
			this._min -= size;
			this._max += size;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x000088E8 File Offset: 0x00006AE8
		public readonly Box3i Inflated(Vector3i size)
		{
			Box3i result = this;
			result.Inflate(size);
			return result;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00008905 File Offset: 0x00006B05
		public void Extend(Vector3i point)
		{
			this._min = Vector3i.ComponentMin(this._min, point);
			this._max = Vector3i.ComponentMax(this._max, point);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000892C File Offset: 0x00006B2C
		public readonly Box3i Extended(Vector3i point)
		{
			Box3i result = this;
			result.Extend(point);
			return result;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00008949 File Offset: 0x00006B49
		public static bool operator ==(Box3i left, Box3i right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00008953 File Offset: 0x00006B53
		public static bool operator !=(Box3i left, Box3i right)
		{
			return !(left == right);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000895F File Offset: 0x00006B5F
		public override bool Equals(object obj)
		{
			return obj is Box3i && this.Equals((Box3i)obj);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00008977 File Offset: 0x00006B77
		public bool Equals(Box3i other)
		{
			return this._min.Equals(other._min) && this._max.Equals(other._max);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000899F File Offset: 0x00006B9F
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector3i, Vector3i>(this._min, this._max);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x000089B2 File Offset: 0x00006BB2
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x000089BC File Offset: 0x00006BBC
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x000089C6 File Offset: 0x00006BC6
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x000089D0 File Offset: 0x00006BD0
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return this.Min.ToString(format, formatProvider) + " - " + this.Max.ToString(format, formatProvider);
		}

		// Token: 0x04000027 RID: 39
		public static readonly Box3i Empty = new Box3i(0, 0, 0, 0, 0, 0);

		// Token: 0x04000028 RID: 40
		private Vector3i _min;

		// Token: 0x04000029 RID: 41
		private Vector3i _max;
	}
}
