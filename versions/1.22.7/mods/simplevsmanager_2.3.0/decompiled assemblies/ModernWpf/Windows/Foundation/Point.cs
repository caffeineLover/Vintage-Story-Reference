using System;
using System.Globalization;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x02000029 RID: 41
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	internal struct Point : IFormattable
	{
		// Token: 0x0600027E RID: 638 RVA: 0x00004B9E File Offset: 0x00002D9E
		public Point(float x, float y)
		{
			this._x = x;
			this._y = y;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00004BAE File Offset: 0x00002DAE
		public Point(double x, double y)
		{
			this = new Point((float)x, (float)y);
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000280 RID: 640 RVA: 0x00004BBA File Offset: 0x00002DBA
		// (set) Token: 0x06000281 RID: 641 RVA: 0x00004BC3 File Offset: 0x00002DC3
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

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000282 RID: 642 RVA: 0x00004BCD File Offset: 0x00002DCD
		// (set) Token: 0x06000283 RID: 643 RVA: 0x00004BD6 File Offset: 0x00002DD6
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

		// Token: 0x06000284 RID: 644 RVA: 0x00004BE0 File Offset: 0x00002DE0
		public override string ToString()
		{
			return this.ConvertToString(null, null);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00004BEA File Offset: 0x00002DEA
		public string ToString(IFormatProvider provider)
		{
			return this.ConvertToString(null, provider);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00004BF4 File Offset: 0x00002DF4
		string IFormattable.ToString(string format, IFormatProvider provider)
		{
			return this.ConvertToString(format, provider);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00004C00 File Offset: 0x00002E00
		private string ConvertToString(string format, IFormatProvider provider)
		{
			char numericListSeparator = Point.GetNumericListSeparator(provider);
			return string.Format(provider, string.Concat(new string[]
			{
				"{1:",
				format,
				"}{0}{2:",
				format,
				"}"
			}), numericListSeparator, this._x, this._y);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00004C64 File Offset: 0x00002E64
		private static char GetNumericListSeparator(IFormatProvider provider)
		{
			char c = ',';
			NumberFormatInfo instance = NumberFormatInfo.GetInstance(provider);
			if (instance.NumberDecimalSeparator.Length > 0 && instance.NumberDecimalSeparator.get_Chars(0) == c)
			{
				c = ';';
			}
			return c;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00004C9C File Offset: 0x00002E9C
		public static bool operator ==(Point point1, Point point2)
		{
			return point1._x == point2._x && point1._y == point2._y;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00004CBC File Offset: 0x00002EBC
		public static bool operator !=(Point point1, Point point2)
		{
			return !(point1 == point2);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00004CC8 File Offset: 0x00002EC8
		public override bool Equals(object o)
		{
			return o is Point && this == (Point)o;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00004CE5 File Offset: 0x00002EE5
		public bool Equals(Point value)
		{
			return this == value;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00004CF4 File Offset: 0x00002EF4
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode();
		}

		// Token: 0x0400007C RID: 124
		public float _x;

		// Token: 0x0400007D RID: 125
		public float _y;
	}
}
