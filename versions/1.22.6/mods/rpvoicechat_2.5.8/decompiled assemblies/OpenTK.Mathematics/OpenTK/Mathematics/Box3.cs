using System;
using System.Xml.Serialization;

namespace OpenTK.Mathematics
{
	// Token: 0x0200000C RID: 12
	[Serializable]
	public struct Box3 : IEquatable<Box3>, IFormattable
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x000075E3 File Offset: 0x000057E3
		// (set) Token: 0x060001C5 RID: 453 RVA: 0x000075EC File Offset: 0x000057EC
		public Vector3 Min
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

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x0000766C File Offset: 0x0000586C
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x00007674 File Offset: 0x00005874
		public Vector3 Max
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

		// Token: 0x060001C8 RID: 456 RVA: 0x000076F4 File Offset: 0x000058F4
		public Box3(Vector3 min, Vector3 max)
		{
			this._min = Vector3.ComponentMin(min, max);
			this._max = Vector3.ComponentMax(min, max);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00007710 File Offset: 0x00005910
		public Box3(float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
		{
			this = new Box3(new Vector3(minX, minY, minZ), new Vector3(maxX, maxY, maxZ));
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000772B File Offset: 0x0000592B
		// (set) Token: 0x060001CB RID: 459 RVA: 0x00007740 File Offset: 0x00005940
		[XmlIgnore]
		public Vector3 Size
		{
			get
			{
				return this.Max - this.Min;
			}
			set
			{
				Vector3 center = this.Center;
				this._min = center - value * 0.5f;
				this._max = center + value * 0.5f;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060001CC RID: 460 RVA: 0x00007782 File Offset: 0x00005982
		// (set) Token: 0x060001CD RID: 461 RVA: 0x00007794 File Offset: 0x00005994
		[XmlIgnore]
		public Vector3 HalfSize
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

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060001CE RID: 462 RVA: 0x000077A7 File Offset: 0x000059A7
		// (set) Token: 0x060001CF RID: 463 RVA: 0x000077BA File Offset: 0x000059BA
		[XmlIgnore]
		public Vector3 Center
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

		// Token: 0x060001D0 RID: 464 RVA: 0x000077D0 File Offset: 0x000059D0
		[Obsolete("This function excludes borders even though it's documentation says otherwise. Use ContainsInclusive and ContainsExclusive for the desired behaviour.")]
		public readonly bool Contains(Vector3 point)
		{
			return this._min.X < point.X && point.X < this._max.X && this._min.Y < point.Y && point.Y < this._max.Y && this._min.Z < point.Z && point.Z < this._max.Z;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00007854 File Offset: 0x00005A54
		public readonly bool ContainsInclusive(Vector3 point)
		{
			return this._min.X <= point.X && point.X <= this._max.X && this._min.Y <= point.Y && point.Y <= this._max.Y && this._min.Z <= point.Z && point.Z <= this._max.Z;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x000078D8 File Offset: 0x00005AD8
		public readonly bool ContainsExclusive(Vector3 point)
		{
			return this._min.X < point.X && point.X < this._max.X && this._min.Y < point.Y && point.Y < this._max.Y && this._min.Z < point.Z && point.Z < this._max.Z;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00007959 File Offset: 0x00005B59
		public bool Contains(Vector3 point, bool boundaryInclusive)
		{
			if (boundaryInclusive)
			{
				return this.ContainsInclusive(point);
			}
			return this.ContainsExclusive(point);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00007970 File Offset: 0x00005B70
		public readonly bool Contains(Box3 other)
		{
			return this._max.X >= other._min.X && this._min.X <= other._max.X && this._max.Y >= other._min.Y && this._min.Y <= other._max.Y && this._max.Z >= other._min.Z && this._min.Z <= other._max.Z;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00007A14 File Offset: 0x00005C14
		public readonly float DistanceToNearestEdge(Vector3 point)
		{
			Vector3 vector = new Vector3(Math.Max(0f, Math.Max(this._min.X - point.X, point.X - this._max.X)), Math.Max(0f, Math.Max(this._min.Y - point.Y, point.Y - this._max.Y)), Math.Max(0f, Math.Max(this._min.Z - point.Z, point.Z - this._max.Z)));
			return vector.Length;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00007AC8 File Offset: 0x00005CC8
		public void Translate(Vector3 distance)
		{
			this._min += distance;
			this._max += distance;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00007AF0 File Offset: 0x00005CF0
		public readonly Box3 Translated(Vector3 distance)
		{
			Box3 result = this;
			result.Translate(distance);
			return result;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00007B0D File Offset: 0x00005D0D
		public void Scale(Vector3 scale, Vector3 anchor)
		{
			this._min = anchor + (this._min - anchor) * scale;
			this._max = anchor + (this._max - anchor) * scale;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00007B4C File Offset: 0x00005D4C
		public readonly Box3 Scaled(Vector3 scale, Vector3 anchor)
		{
			Box3 result = this;
			result.Scale(scale, anchor);
			return result;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00007B6C File Offset: 0x00005D6C
		public void Inflate(Vector3 size)
		{
			size = Vector3.ComponentMax(size, -this.HalfSize);
			Vector3 a = this._min - size;
			Vector3 b = this._max + size;
			this._min = Vector3.ComponentMin(a, b);
			this._max = Vector3.ComponentMax(a, b);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00007BC0 File Offset: 0x00005DC0
		public readonly Box3 Inflated(Vector3 size)
		{
			Box3 result = this;
			result.Inflate(size);
			return result;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00007BDD File Offset: 0x00005DDD
		public void Extend(Vector3 point)
		{
			this._min = Vector3.ComponentMin(this._min, point);
			this._max = Vector3.ComponentMax(this._max, point);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00007C04 File Offset: 0x00005E04
		public readonly Box3 Extended(Vector3 point)
		{
			Box3 result = this;
			result.Extend(point);
			return result;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00007C21 File Offset: 0x00005E21
		public static bool operator ==(Box3 left, Box3 right)
		{
			return left.Equals(right);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00007C2B File Offset: 0x00005E2B
		public static bool operator !=(Box3 left, Box3 right)
		{
			return !(left == right);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00007C37 File Offset: 0x00005E37
		public override bool Equals(object obj)
		{
			return obj is Box3 && this.Equals((Box3)obj);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00007C4F File Offset: 0x00005E4F
		public readonly bool Equals(Box3 other)
		{
			return this._min.Equals(other._min) && this._max.Equals(other._max);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00007C77 File Offset: 0x00005E77
		public override readonly int GetHashCode()
		{
			return HashCode.Combine<Vector3, Vector3>(this._min, this._max);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00007C8A File Offset: 0x00005E8A
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00007C94 File Offset: 0x00005E94
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00007C9E File Offset: 0x00005E9E
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00007CA8 File Offset: 0x00005EA8
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return this.Min.ToString(format, formatProvider) + " - " + this.Max.ToString(format, formatProvider);
		}

		// Token: 0x04000023 RID: 35
		private Vector3 _min;

		// Token: 0x04000024 RID: 36
		private Vector3 _max;
	}
}
