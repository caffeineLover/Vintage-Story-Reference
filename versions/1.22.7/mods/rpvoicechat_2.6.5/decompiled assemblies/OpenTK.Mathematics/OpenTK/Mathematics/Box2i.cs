using System;
using System.Drawing;
using System.Xml.Serialization;

namespace OpenTK.Mathematics
{
	// Token: 0x0200000B RID: 11
	[Serializable]
	public struct Box2i : IEquatable<Box2i>, IFormattable
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000700F File Offset: 0x0000520F
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x00007017 File Offset: 0x00005217
		public Vector2i Min
		{
			readonly get
			{
				return this._min;
			}
			set
			{
				this._max = Vector2i.ComponentMax(this._max, value);
				this._min = value;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00007032 File Offset: 0x00005232
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x0000703A File Offset: 0x0000523A
		public Vector2i Max
		{
			readonly get
			{
				return this._max;
			}
			set
			{
				this._min = Vector2i.ComponentMin(this._min, value);
				this._max = value;
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00007055 File Offset: 0x00005255
		public Box2i(Vector2i min, Vector2i max)
		{
			this._min = Vector2i.ComponentMin(min, max);
			this._max = Vector2i.ComponentMax(min, max);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00007071 File Offset: 0x00005271
		public Box2i(int minX, int minY, int maxX, int maxY)
		{
			this = new Box2i(new Vector2i(minX, minY), new Vector2i(maxX, maxY));
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00007088 File Offset: 0x00005288
		[XmlIgnore]
		public Vector2i Size
		{
			get
			{
				return this.Max - this.Min;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x0000709B File Offset: 0x0000529B
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x000070AC File Offset: 0x000052AC
		[XmlIgnore]
		public Vector2i HalfSize
		{
			get
			{
				return this.Size / 2;
			}
			set
			{
				Vector2i left = new Vector2i((int)this.Center.X, (int)this.Center.Y);
				this._min = left - value;
				this._max = left + value;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x000070F4 File Offset: 0x000052F4
		[XmlIgnore]
		public readonly Vector2 Center
		{
			get
			{
				return this._min + (this._max - this._min).ToVector2() * 0.5f;
			}
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00007134 File Offset: 0x00005334
		[Obsolete("This function excludes borders even though it's documentation says otherwise. Use ContainsInclusive and ContainsExclusive for the desired behaviour.")]
		public readonly bool Contains(Vector2i point)
		{
			return this._min.X < point.X && point.X < this._max.X && this._min.Y < point.Y && point.Y < this._max.Y;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00007190 File Offset: 0x00005390
		public readonly bool ContainsInclusive(Vector2i point)
		{
			return this._min.X <= point.X && point.X <= this._max.X && this._min.Y <= point.Y && point.Y <= this._max.Y;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000071F0 File Offset: 0x000053F0
		public readonly bool ContainsExclusive(Vector2i point)
		{
			return this._min.X < point.X && point.X < this._max.X && this._min.Y < point.Y && point.Y < this._max.Y;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000724B File Offset: 0x0000544B
		public bool Contains(Vector2i point, bool boundaryInclusive)
		{
			if (boundaryInclusive)
			{
				return this.ContainsInclusive(point);
			}
			return this.ContainsExclusive(point);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00007260 File Offset: 0x00005460
		public readonly bool Contains(Box2i other)
		{
			return this._max.X >= other._min.X && this._min.X <= other._max.X && this._max.Y >= other._min.Y && this._min.Y <= other._max.Y;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x000072D4 File Offset: 0x000054D4
		public static Box2i Intersect(Box2i a, Box2i b)
		{
			Vector2i vector2i = Vector2i.ComponentMax(a.Min, b.Min);
			Vector2i vector2i2 = Vector2i.ComponentMin(a.Max, b.Max);
			if (vector2i2.X >= vector2i.X && vector2i2.Y >= vector2i.Y)
			{
				return new Box2i(vector2i, vector2i2);
			}
			return Box2i.Empty;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00007334 File Offset: 0x00005534
		public readonly float DistanceToNearestEdge(Vector2i point)
		{
			Vector2 vector = new Vector2(Math.Max(0f, (float)Math.Max(this._min.X - point.X, point.X - this._max.X)), Math.Max(0f, (float)Math.Max(this._min.Y - point.Y, point.Y - this._max.Y)));
			return vector.Length;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x000073B7 File Offset: 0x000055B7
		public void Translate(Vector2i distance)
		{
			this._min += distance;
			this._max += distance;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x000073E0 File Offset: 0x000055E0
		public readonly Box2i Translated(Vector2i distance)
		{
			Box2i result = this;
			result.Translate(distance);
			return result;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x000073FD File Offset: 0x000055FD
		public void Scale(Vector2i scale, Vector2i anchor)
		{
			this._min = anchor + (this._min - anchor) * scale;
			this._max = anchor + (this._max - anchor) * scale;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000743C File Offset: 0x0000563C
		public readonly Box2i Scaled(Vector2i scale, Vector2i anchor)
		{
			Box2i result = this;
			result.Scale(scale, anchor);
			return result;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000745A File Offset: 0x0000565A
		public void Inflate(Vector2i size)
		{
			size = Vector2i.ComponentMax(size, -this.HalfSize);
			this._min -= size;
			this._max += size;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00007494 File Offset: 0x00005694
		public readonly Box2i Inflated(Vector2i size)
		{
			Box2i result = this;
			result.Inflate(size);
			return result;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x000074B1 File Offset: 0x000056B1
		public void Extend(Vector2i point)
		{
			this._min = Vector2i.ComponentMin(this._min, point);
			this._max = Vector2i.ComponentMax(this._max, point);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x000074D8 File Offset: 0x000056D8
		public readonly Box2i Extended(Vector2i point)
		{
			Box2i result = this;
			result.Extend(point);
			return result;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x000074F5 File Offset: 0x000056F5
		public static bool operator ==(Box2i left, Box2i right)
		{
			return left.Equals(right);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x000074FF File Offset: 0x000056FF
		public static bool operator !=(Box2i left, Box2i right)
		{
			return !(left == right);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000750B File Offset: 0x0000570B
		public static explicit operator Rectangle(Box2i box)
		{
			return new Rectangle((Point)box.Min, (Size)box.Size);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000752A File Offset: 0x0000572A
		public override bool Equals(object obj)
		{
			return obj is Box2i && this.Equals((Box2i)obj);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00007542 File Offset: 0x00005742
		public bool Equals(Box2i other)
		{
			return this._min.Equals(other._min) && this._max.Equals(other._max);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000756A File Offset: 0x0000576A
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector2i, Vector2i>(this._min, this._max);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000757D File Offset: 0x0000577D
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00007587 File Offset: 0x00005787
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00007591 File Offset: 0x00005791
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000759C File Offset: 0x0000579C
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return this.Min.ToString(format, formatProvider) + " - " + this.Max.ToString(format, formatProvider);
		}

		// Token: 0x04000020 RID: 32
		public static readonly Box2i Empty = new Box2i(0, 0, 0, 0);

		// Token: 0x04000021 RID: 33
		private Vector2i _min;

		// Token: 0x04000022 RID: 34
		private Vector2i _max;
	}
}
