using System;
using System.Xml.Serialization;

namespace OpenTK.Mathematics
{
	// Token: 0x0200000A RID: 10
	[Serializable]
	public struct Box2d : IEquatable<Box2d>, IFormattable
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00006A3B File Offset: 0x00004C3B
		// (set) Token: 0x0600017E RID: 382 RVA: 0x00006A44 File Offset: 0x00004C44
		public Vector2d Min
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

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00006AA0 File Offset: 0x00004CA0
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00006AA8 File Offset: 0x00004CA8
		public Vector2d Max
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

		// Token: 0x06000181 RID: 385 RVA: 0x00006B04 File Offset: 0x00004D04
		public Box2d(Vector2d min, Vector2d max)
		{
			this._min = Vector2d.ComponentMin(min, max);
			this._max = Vector2d.ComponentMax(min, max);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00006B20 File Offset: 0x00004D20
		public Box2d(double minX, double minY, double maxX, double maxY)
		{
			this = new Box2d(new Vector2d(minX, minY), new Vector2d(maxX, maxY));
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00006B37 File Offset: 0x00004D37
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00006B4C File Offset: 0x00004D4C
		[XmlIgnore]
		public Vector2d Size
		{
			get
			{
				return this.Max - this.Min;
			}
			set
			{
				Vector2d center = this.Center;
				this._min = center - value * 0.5;
				this._max = center + value * 0.5;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00006B96 File Offset: 0x00004D96
		// (set) Token: 0x06000186 RID: 390 RVA: 0x00006BAC File Offset: 0x00004DAC
		[XmlIgnore]
		public Vector2d HalfSize
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

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00006BC3 File Offset: 0x00004DC3
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00006BD6 File Offset: 0x00004DD6
		[XmlIgnore]
		public Vector2d Center
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

		// Token: 0x06000189 RID: 393 RVA: 0x00006BEC File Offset: 0x00004DEC
		[Obsolete("This function excludes borders even though it's documentation says otherwise. Use ContainsInclusive and ContainsExclusive for the desired behaviour.")]
		public readonly bool Contains(Vector2d point)
		{
			return this._min.X < point.X && point.X < this._max.X && this._min.Y < point.Y && point.Y < this._max.Y;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00006C48 File Offset: 0x00004E48
		public readonly bool ContainsInclusive(Vector2d point)
		{
			return this._min.X <= point.X && point.X <= this._max.X && this._min.Y <= point.Y && point.Y <= this._max.Y;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00006CA8 File Offset: 0x00004EA8
		public readonly bool ContainsExclusive(Vector2d point)
		{
			return this._min.X < point.X && point.X < this._max.X && this._min.Y < point.Y && point.Y < this._max.Y;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00006D03 File Offset: 0x00004F03
		public bool Contains(Vector2d point, bool boundaryInclusive)
		{
			if (boundaryInclusive)
			{
				return this.ContainsInclusive(point);
			}
			return this.ContainsExclusive(point);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00006D18 File Offset: 0x00004F18
		public readonly bool Contains(Box2d other)
		{
			return this._max.X >= other._min.X && this._min.X <= other._max.X && this._max.Y >= other._min.Y && this._min.Y <= other._max.Y;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00006D8C File Offset: 0x00004F8C
		public readonly double DistanceToNearestEdge(Vector2d point)
		{
			Vector2d vector2d = new Vector2d(Math.Max(0.0, Math.Max(this._min.X - point.X, point.X - this._max.X)), Math.Max(0.0, Math.Max(this._min.Y - point.Y, point.Y - this._max.Y)));
			return vector2d.Length;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00006E15 File Offset: 0x00005015
		public void Translate(Vector2d distance)
		{
			this._min += distance;
			this._max += distance;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00006E3C File Offset: 0x0000503C
		public readonly Box2d Translated(Vector2d distance)
		{
			Box2d result = this;
			result.Translate(distance);
			return result;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00006E59 File Offset: 0x00005059
		public void Scale(Vector2d scale, Vector2d anchor)
		{
			this._min = anchor + (this._min - anchor) * scale;
			this._max = anchor + (this._max - anchor) * scale;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00006E98 File Offset: 0x00005098
		public readonly Box2d Scaled(Vector2d scale, Vector2d anchor)
		{
			Box2d result = this;
			result.Scale(scale, anchor);
			return result;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00006EB6 File Offset: 0x000050B6
		public void Inflate(Vector2d size)
		{
			size = Vector2d.ComponentMax(size, -this.HalfSize);
			this._min -= size;
			this._max += size;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00006EF0 File Offset: 0x000050F0
		public readonly Box2d Inflated(Vector2d size)
		{
			Box2d result = this;
			result.Inflate(size);
			return result;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00006F0D File Offset: 0x0000510D
		public void Extend(Vector2d point)
		{
			this._min = Vector2d.ComponentMin(this._min, point);
			this._max = Vector2d.ComponentMax(this._max, point);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00006F34 File Offset: 0x00005134
		public readonly Box2d Extended(Vector2d point)
		{
			Box2d result = this;
			result.Extend(point);
			return result;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00006F51 File Offset: 0x00005151
		public static bool operator ==(Box2d left, Box2d right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00006F5B File Offset: 0x0000515B
		public static bool operator !=(Box2d left, Box2d right)
		{
			return !(left == right);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00006F67 File Offset: 0x00005167
		public override bool Equals(object obj)
		{
			return obj is Box2d && this.Equals((Box2d)obj);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00006F7F File Offset: 0x0000517F
		public bool Equals(Box2d other)
		{
			return this._min.Equals(other._min) && this._max.Equals(other._max);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00006FA7 File Offset: 0x000051A7
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector2d, Vector2d>(this._min, this._max);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00006FBA File Offset: 0x000051BA
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00006FC4 File Offset: 0x000051C4
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00006FCE File Offset: 0x000051CE
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00006FD8 File Offset: 0x000051D8
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return this.Min.ToString(format, formatProvider) + " - " + this.Max.ToString(format, formatProvider);
		}

		// Token: 0x0400001E RID: 30
		private Vector2d _min;

		// Token: 0x0400001F RID: 31
		private Vector2d _max;
	}
}
