using System;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x0200002A RID: 42
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	internal struct Rect : IFormattable
	{
		// Token: 0x0600028E RID: 654 RVA: 0x00004D20 File Offset: 0x00002F20
		public Rect(float x, float y, float width, float height)
		{
			if (width < 0f)
			{
				throw new ArgumentOutOfRangeException("width", GSR.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (height < 0f)
			{
				throw new ArgumentOutOfRangeException("height", GSR.ArgumentOutOfRange_NeedNonNegNum);
			}
			this._x = x;
			this._y = y;
			this._width = width;
			this._height = height;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00004D7B File Offset: 0x00002F7B
		public Rect(double x, double y, double width, double height)
		{
			this = new Rect((float)x, (float)y, (float)width, (float)height);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00004D8C File Offset: 0x00002F8C
		public Rect(Point point1, Point point2)
		{
			this._x = Math.Min(point1._x, point2._x);
			this._y = Math.Min(point1._y, point2._y);
			this._width = Math.Max(Math.Max(point1._x, point2._x) - this._x, 0f);
			this._height = Math.Max(Math.Max(point1._y, point2._y) - this._y, 0f);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00004E18 File Offset: 0x00003018
		public Rect(Point location, Size size)
		{
			if (size.IsEmpty)
			{
				this = Rect.s_empty;
				return;
			}
			this._x = location._x;
			this._y = location._y;
			this._width = size._width;
			this._height = size._height;
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000292 RID: 658 RVA: 0x00004E6A File Offset: 0x0000306A
		// (set) Token: 0x06000293 RID: 659 RVA: 0x00004E73 File Offset: 0x00003073
		public double X
		{
			get
			{
				return (double)this._x;
			}
			set
			{
				this._x = (float)value;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000294 RID: 660 RVA: 0x00004E7D File Offset: 0x0000307D
		// (set) Token: 0x06000295 RID: 661 RVA: 0x00004E86 File Offset: 0x00003086
		public double Y
		{
			get
			{
				return (double)this._y;
			}
			set
			{
				this._y = (float)value;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000296 RID: 662 RVA: 0x00004E90 File Offset: 0x00003090
		// (set) Token: 0x06000297 RID: 663 RVA: 0x00004E99 File Offset: 0x00003099
		public double Width
		{
			get
			{
				return (double)this._width;
			}
			set
			{
				if (value < 0.0)
				{
					throw new ArgumentOutOfRangeException("Width", GSR.ArgumentOutOfRange_NeedNonNegNum);
				}
				this._width = (float)value;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000298 RID: 664 RVA: 0x00004EBF File Offset: 0x000030BF
		// (set) Token: 0x06000299 RID: 665 RVA: 0x00004EC8 File Offset: 0x000030C8
		public double Height
		{
			get
			{
				return (double)this._height;
			}
			set
			{
				if (value < 0.0)
				{
					throw new ArgumentOutOfRangeException("Height", GSR.ArgumentOutOfRange_NeedNonNegNum);
				}
				this._height = (float)value;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x0600029A RID: 666 RVA: 0x00004EEE File Offset: 0x000030EE
		public double Left
		{
			get
			{
				return (double)this._x;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x0600029B RID: 667 RVA: 0x00004EF7 File Offset: 0x000030F7
		public double Top
		{
			get
			{
				return (double)this._y;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x0600029C RID: 668 RVA: 0x00004F00 File Offset: 0x00003100
		public double Right
		{
			get
			{
				if (this.IsEmpty)
				{
					return double.NegativeInfinity;
				}
				return (double)(this._x + this._width);
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x0600029D RID: 669 RVA: 0x00004F22 File Offset: 0x00003122
		public double Bottom
		{
			get
			{
				if (this.IsEmpty)
				{
					return double.NegativeInfinity;
				}
				return (double)(this._y + this._height);
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600029E RID: 670 RVA: 0x00004F44 File Offset: 0x00003144
		public static Rect Empty
		{
			get
			{
				return Rect.s_empty;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600029F RID: 671 RVA: 0x00004F4B File Offset: 0x0000314B
		public bool IsEmpty
		{
			get
			{
				return this._width < 0f;
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00004F5A File Offset: 0x0000315A
		public bool Contains(Point point)
		{
			return this.ContainsInternal(point._x, point._y);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00004F70 File Offset: 0x00003170
		public void Intersect(Rect rect)
		{
			if (!this.IntersectsWith(rect))
			{
				this = Rect.s_empty;
				return;
			}
			double num = Math.Max(this.X, rect.X);
			double num2 = Math.Max(this.Y, rect.Y);
			this.Width = Math.Max(Math.Min(this.X + this.Width, rect.X + rect.Width) - num, 0.0);
			this.Height = Math.Max(Math.Min(this.Y + this.Height, rect.Y + rect.Height) - num2, 0.0);
			this.X = num;
			this.Y = num2;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00005034 File Offset: 0x00003234
		public void Union(Rect rect)
		{
			if (this.IsEmpty)
			{
				this = rect;
				return;
			}
			if (!rect.IsEmpty)
			{
				double num = Math.Min(this.Left, rect.Left);
				double num2 = Math.Min(this.Top, rect.Top);
				if (rect.Width == double.PositiveInfinity || this.Width == double.PositiveInfinity)
				{
					this.Width = double.PositiveInfinity;
				}
				else
				{
					double num3 = Math.Max(this.Right, rect.Right);
					this.Width = Math.Max(num3 - num, 0.0);
				}
				if (rect.Height == double.PositiveInfinity || this.Height == double.PositiveInfinity)
				{
					this.Height = double.PositiveInfinity;
				}
				else
				{
					double num4 = Math.Max(this.Bottom, rect.Bottom);
					this.Height = Math.Max(num4 - num2, 0.0);
				}
				this.X = num;
				this.Y = num2;
			}
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000514D File Offset: 0x0000334D
		public void Union(Point point)
		{
			this.Union(new Rect(point, point));
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000515C File Offset: 0x0000335C
		private bool ContainsInternal(float x, float y)
		{
			return x >= this._x && x - this._width <= this._x && y >= this._y && y - this._height <= this._y;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00005198 File Offset: 0x00003398
		internal bool IntersectsWith(Rect rect)
		{
			return this._width >= 0f && rect._width >= 0f && (rect._x <= this._x + this._width && rect._x + rect._width >= this._x && rect._y <= this._y + this._height) && rect._y + rect._height >= this._y;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000521C File Offset: 0x0000341C
		private static Rect CreateEmptyRect()
		{
			return new Rect
			{
				_x = float.PositiveInfinity,
				_y = float.PositiveInfinity,
				_width = float.NegativeInfinity,
				_height = float.NegativeInfinity
			};
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00005262 File Offset: 0x00003462
		public override string ToString()
		{
			return this.ConvertToString(null, null);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000526C File Offset: 0x0000346C
		public string ToString(IFormatProvider provider)
		{
			return this.ConvertToString(null, provider);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00005276 File Offset: 0x00003476
		string IFormattable.ToString(string format, IFormatProvider provider)
		{
			return this.ConvertToString(format, provider);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00005280 File Offset: 0x00003480
		internal string ConvertToString(string format, IFormatProvider provider)
		{
			if (this.IsEmpty)
			{
				return "Empty.";
			}
			char numericListSeparator = TokenizerHelper.GetNumericListSeparator(provider);
			return string.Format(provider, string.Concat(new string[]
			{
				"{1:",
				format,
				"}{0}{2:",
				format,
				"}{0}{3:",
				format,
				"}{0}{4:",
				format,
				"}"
			}), new object[]
			{
				numericListSeparator,
				this._x,
				this._y,
				this._width,
				this._height
			});
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00005333 File Offset: 0x00003533
		public bool Equals(Rect value)
		{
			return this == value;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00005341 File Offset: 0x00003541
		public static bool operator ==(Rect rect1, Rect rect2)
		{
			return rect1._x == rect2._x && rect1._y == rect2._y && rect1._width == rect2._width && rect1._height == rect2._height;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000537D File Offset: 0x0000357D
		public static bool operator !=(Rect rect1, Rect rect2)
		{
			return !(rect1 == rect2);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00005389 File Offset: 0x00003589
		public override bool Equals(object o)
		{
			return o is Rect && this == (Rect)o;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x000053A8 File Offset: 0x000035A8
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Width.GetHashCode() ^ this.Height.GetHashCode();
		}

		// Token: 0x0400007E RID: 126
		public float _x;

		// Token: 0x0400007F RID: 127
		public float _y;

		// Token: 0x04000080 RID: 128
		public float _width;

		// Token: 0x04000081 RID: 129
		public float _height;

		// Token: 0x04000082 RID: 130
		private const double EmptyX = double.PositiveInfinity;

		// Token: 0x04000083 RID: 131
		private const double EmptyY = double.PositiveInfinity;

		// Token: 0x04000084 RID: 132
		private const double EmptyWidth = double.NegativeInfinity;

		// Token: 0x04000085 RID: 133
		private const double EmptyHeight = double.NegativeInfinity;

		// Token: 0x04000086 RID: 134
		private static readonly Rect s_empty = Rect.CreateEmptyRect();
	}
}
