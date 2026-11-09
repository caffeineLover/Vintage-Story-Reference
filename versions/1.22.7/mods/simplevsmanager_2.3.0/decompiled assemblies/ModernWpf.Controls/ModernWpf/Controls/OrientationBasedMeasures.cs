using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000B5 RID: 181
	internal class OrientationBasedMeasures
	{
		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600090C RID: 2316 RVA: 0x000206C2 File Offset: 0x0001E8C2
		// (set) Token: 0x0600090D RID: 2317 RVA: 0x000206CA File Offset: 0x0001E8CA
		public ScrollOrientation ScrollOrientation { get; set; }

		// Token: 0x0600090E RID: 2318 RVA: 0x000206D3 File Offset: 0x0001E8D3
		public double Major(Size size)
		{
			if (this.ScrollOrientation != ScrollOrientation.Vertical)
			{
				return size.Width;
			}
			return size.Height;
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x000206EC File Offset: 0x0001E8EC
		public double Minor(Size size)
		{
			if (this.ScrollOrientation != ScrollOrientation.Vertical)
			{
				return size.Height;
			}
			return size.Width;
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x00020705 File Offset: 0x0001E905
		public double MajorSize(Rect rect)
		{
			if (this.ScrollOrientation != ScrollOrientation.Vertical)
			{
				return rect.Width;
			}
			return rect.Height;
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0002071E File Offset: 0x0001E91E
		public double MinorSize(Rect rect)
		{
			if (this.ScrollOrientation != ScrollOrientation.Vertical)
			{
				return rect.Height;
			}
			return rect.Width;
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x00020737 File Offset: 0x0001E937
		public double MajorStart(Rect rect)
		{
			if (this.ScrollOrientation != ScrollOrientation.Vertical)
			{
				return rect.X;
			}
			return rect.Y;
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x00020750 File Offset: 0x0001E950
		public double MajorEnd(Rect rect)
		{
			if (this.ScrollOrientation != ScrollOrientation.Vertical)
			{
				return rect.X + rect.Width;
			}
			return rect.Y + rect.Height;
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x00020779 File Offset: 0x0001E979
		public double MinorStart(Rect rect)
		{
			if (this.ScrollOrientation != ScrollOrientation.Vertical)
			{
				return rect.Y;
			}
			return rect.X;
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x00020792 File Offset: 0x0001E992
		public double MinorEnd(Rect rect)
		{
			if (this.ScrollOrientation != ScrollOrientation.Vertical)
			{
				return rect.Y + rect.Height;
			}
			return rect.X + rect.Width;
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x000207BB File Offset: 0x0001E9BB
		public void SetMajorSize(ref Rect rect, double value)
		{
			if (this.ScrollOrientation == ScrollOrientation.Vertical)
			{
				rect.Height = value;
				return;
			}
			rect.Width = value;
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x000207D4 File Offset: 0x0001E9D4
		public void SetMinorSize(ref Rect rect, double value)
		{
			if (this.ScrollOrientation == ScrollOrientation.Vertical)
			{
				rect.Width = value;
				return;
			}
			rect.Height = value;
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x000207ED File Offset: 0x0001E9ED
		public void SetMajorStart(ref Rect rect, double value)
		{
			if (this.ScrollOrientation == ScrollOrientation.Vertical)
			{
				rect.Y = value;
				return;
			}
			rect.X = value;
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x00020806 File Offset: 0x0001EA06
		public void SetMinorStart(ref Rect rect, double value)
		{
			if (this.ScrollOrientation == ScrollOrientation.Vertical)
			{
				rect.X = value;
				return;
			}
			rect.Y = value;
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0002081F File Offset: 0x0001EA1F
		public Rect MinorMajorRect(double minor, double major, double minorSize, double majorSize)
		{
			if (this.ScrollOrientation != ScrollOrientation.Vertical)
			{
				return new Rect(major, minor, majorSize, minorSize);
			}
			return new Rect(minor, major, minorSize, majorSize);
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0002083E File Offset: 0x0001EA3E
		public Point MinorMajorPoint(double minor, double major)
		{
			if (this.ScrollOrientation != ScrollOrientation.Vertical)
			{
				return new Point(major, minor);
			}
			return new Point(minor, major);
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x00020857 File Offset: 0x0001EA57
		public Size MinorMajorSize(double minor, double major)
		{
			if (this.ScrollOrientation != ScrollOrientation.Vertical)
			{
				return new Size(major, minor);
			}
			return new Size(minor, major);
		}
	}
}
