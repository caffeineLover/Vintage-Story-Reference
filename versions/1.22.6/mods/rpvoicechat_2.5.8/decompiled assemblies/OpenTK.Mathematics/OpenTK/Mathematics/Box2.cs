using System;
using System.Drawing;
using System.Xml.Serialization;

namespace OpenTK.Mathematics
{
	// Token: 0x02000009 RID: 9
	[Serializable]
	public struct Box2 : IEquatable<Box2>, IFormattable
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00006443 File Offset: 0x00004643
		// (set) Token: 0x0600015A RID: 346 RVA: 0x0000644C File Offset: 0x0000464C
		public Vector2 Min
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
				this._min = value;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600015B RID: 347 RVA: 0x000064A8 File Offset: 0x000046A8
		// (set) Token: 0x0600015C RID: 348 RVA: 0x000064B0 File Offset: 0x000046B0
		public Vector2 Max
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
				this._max = value;
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000650C File Offset: 0x0000470C
		public Box2(Vector2 min, Vector2 max)
		{
			this._min = Vector2.ComponentMin(min, max);
			this._max = Vector2.ComponentMax(min, max);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00006528 File Offset: 0x00004728
		public Box2(float minX, float minY, float maxX, float maxY)
		{
			this = new Box2(new Vector2(minX, minY), new Vector2(maxX, maxY));
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600015F RID: 351 RVA: 0x0000653F File Offset: 0x0000473F
		// (set) Token: 0x06000160 RID: 352 RVA: 0x00006554 File Offset: 0x00004754
		[XmlIgnore]
		public Vector2 Size
		{
			get
			{
				return this.Max - this.Min;
			}
			set
			{
				Vector2 center = this.Center;
				this._min = center - value * 0.5f;
				this._max = center + value * 0.5f;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00006596 File Offset: 0x00004796
		// (set) Token: 0x06000162 RID: 354 RVA: 0x000065A8 File Offset: 0x000047A8
		[XmlIgnore]
		public Vector2 HalfSize
		{
			get
			{
				return this.Size / 2f;
			}
			set
			{
				this.Size = value * 2f;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000163 RID: 355 RVA: 0x000065BB File Offset: 0x000047BB
		// (set) Token: 0x06000164 RID: 356 RVA: 0x000065CE File Offset: 0x000047CE
		[XmlIgnore]
		public Vector2 Center
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

		// Token: 0x06000165 RID: 357 RVA: 0x000065E4 File Offset: 0x000047E4
		[Obsolete("This function used to exclude borders, but to follow changes from the other Box structs it's deprecated. Use ContainsInclusive and ContainsExclusive for the desired behaviour.")]
		public readonly bool Contains(Vector2 point)
		{
			return this._min.X < point.X && point.X < this._max.X && this._min.Y < point.Y && point.Y < this._max.Y;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00006640 File Offset: 0x00004840
		public readonly bool ContainsInclusive(Vector2 point)
		{
			return this._min.X <= point.X && point.X <= this._max.X && this._min.Y <= point.Y && point.Y <= this._max.Y;
		}

		// Token: 0x06000167 RID: 359 RVA: 0x000066A0 File Offset: 0x000048A0
		public readonly bool ContainsExclusive(Vector2 point)
		{
			return this._min.X < point.X && point.X < this._max.X && this._min.Y < point.Y && point.Y < this._max.Y;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x000066FB File Offset: 0x000048FB
		public bool Contains(Vector2 point, bool boundaryInclusive)
		{
			if (boundaryInclusive)
			{
				return this.ContainsInclusive(point);
			}
			return this.ContainsExclusive(point);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00006710 File Offset: 0x00004910
		public readonly bool Contains(Box2 other)
		{
			return this._max.X >= other._min.X && this._min.X <= other._max.X && this._max.Y >= other._min.Y && this._min.Y <= other._max.Y;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00006784 File Offset: 0x00004984
		public readonly float DistanceToNearestEdge(Vector2 point)
		{
			Vector2 vector = new Vector2(Math.Max(0f, Math.Max(this._min.X - point.X, point.X - this._max.X)), Math.Max(0f, Math.Max(this._min.Y - point.Y, point.Y - this._max.Y)));
			return vector.Length;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00006805 File Offset: 0x00004A05
		public void Translate(Vector2 distance)
		{
			this._min += distance;
			this._max += distance;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000682C File Offset: 0x00004A2C
		public readonly Box2 Translated(Vector2 distance)
		{
			Box2 result = this;
			result.Translate(distance);
			return result;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00006849 File Offset: 0x00004A49
		public void Scale(Vector2 scale, Vector2 anchor)
		{
			this._min = anchor + (this._min - anchor) * scale;
			this._max = anchor + (this._max - anchor) * scale;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00006888 File Offset: 0x00004A88
		public readonly Box2 Scaled(Vector2 scale, Vector2 anchor)
		{
			Box2 result = this;
			result.Scale(scale, anchor);
			return result;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x000068A8 File Offset: 0x00004AA8
		public void Inflate(Vector2 size)
		{
			size = Vector2.ComponentMax(size, -this.HalfSize);
			Vector2 a = this._min - size;
			Vector2 b = this._max + size;
			this._min = Vector2.ComponentMin(a, b);
			this._max = Vector2.ComponentMax(a, b);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x000068FC File Offset: 0x00004AFC
		public readonly Box2 Inflated(Vector2 size)
		{
			Box2 result = this;
			result.Inflate(size);
			return result;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00006919 File Offset: 0x00004B19
		public void Extend(Vector2 point)
		{
			this._min = Vector2.ComponentMin(this._min, point);
			this._max = Vector2.ComponentMax(this._max, point);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00006940 File Offset: 0x00004B40
		public readonly Box2 Extended(Vector2 point)
		{
			Box2 result = this;
			result.Extend(point);
			return result;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000695D File Offset: 0x00004B5D
		public static bool operator ==(Box2 left, Box2 right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00006967 File Offset: 0x00004B67
		public static bool operator !=(Box2 left, Box2 right)
		{
			return !(left == right);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00006973 File Offset: 0x00004B73
		public static explicit operator RectangleF(Box2 box)
		{
			return new RectangleF((PointF)box.Min, (SizeF)box.Size);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00006992 File Offset: 0x00004B92
		public override bool Equals(object obj)
		{
			return obj is Box2 && this.Equals((Box2)obj);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x000069AA File Offset: 0x00004BAA
		public bool Equals(Box2 other)
		{
			return this._min.Equals(other._min) && this._max.Equals(other._max);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000069D2 File Offset: 0x00004BD2
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector2, Vector2>(this._min, this._max);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x000069E5 File Offset: 0x00004BE5
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000069EF File Offset: 0x00004BEF
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x000069F9 File Offset: 0x00004BF9
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00006A04 File Offset: 0x00004C04
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return this.Min.ToString(format, formatProvider) + " - " + this.Max.ToString(format, formatProvider);
		}

		// Token: 0x0400001C RID: 28
		private Vector2 _min;

		// Token: 0x0400001D RID: 29
		private Vector2 _max;
	}
}
