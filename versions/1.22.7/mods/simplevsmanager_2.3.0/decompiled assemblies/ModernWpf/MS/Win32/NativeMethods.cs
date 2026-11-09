using System;
using System.Runtime.InteropServices;

namespace MS.Win32
{
	// Token: 0x02000206 RID: 518
	internal class NativeMethods
	{
		// Token: 0x04000332 RID: 818
		public const int SWP_NOSIZE = 1;

		// Token: 0x04000333 RID: 819
		public const int SWP_NOMOVE = 2;

		// Token: 0x04000334 RID: 820
		public const int SWP_NOZORDER = 4;

		// Token: 0x04000335 RID: 821
		public const int SWP_NOACTIVATE = 16;

		// Token: 0x04000336 RID: 822
		public const int SWP_SHOWWINDOW = 64;

		// Token: 0x04000337 RID: 823
		public const int SWP_HIDEWINDOW = 128;

		// Token: 0x04000338 RID: 824
		public const int SWP_DRAWFRAME = 32;

		// Token: 0x02000404 RID: 1028
		[StructLayout(0)]
		public class POINT
		{
			// Token: 0x06001B98 RID: 7064 RVA: 0x0004A191 File Offset: 0x00048391
			public POINT()
			{
			}

			// Token: 0x06001B99 RID: 7065 RVA: 0x0004A199 File Offset: 0x00048399
			public POINT(int x, int y)
			{
				this.x = x;
				this.y = y;
			}

			// Token: 0x040009E1 RID: 2529
			public int x;

			// Token: 0x040009E2 RID: 2530
			public int y;
		}

		// Token: 0x02000405 RID: 1029
		public struct RECT
		{
			// Token: 0x06001B9A RID: 7066 RVA: 0x0004A1AF File Offset: 0x000483AF
			public RECT(int left, int top, int right, int bottom)
			{
				this.left = left;
				this.top = top;
				this.right = right;
				this.bottom = bottom;
			}

			// Token: 0x170004FF RID: 1279
			// (get) Token: 0x06001B9B RID: 7067 RVA: 0x0004A1CE File Offset: 0x000483CE
			public int Width
			{
				get
				{
					return this.right - this.left;
				}
			}

			// Token: 0x17000500 RID: 1280
			// (get) Token: 0x06001B9C RID: 7068 RVA: 0x0004A1DD File Offset: 0x000483DD
			public int Height
			{
				get
				{
					return this.bottom - this.top;
				}
			}

			// Token: 0x06001B9D RID: 7069 RVA: 0x0004A1EC File Offset: 0x000483EC
			public void Offset(int dx, int dy)
			{
				this.left += dx;
				this.top += dy;
				this.right += dx;
				this.bottom += dy;
			}

			// Token: 0x17000501 RID: 1281
			// (get) Token: 0x06001B9E RID: 7070 RVA: 0x0004A226 File Offset: 0x00048426
			public bool IsEmpty
			{
				get
				{
					return this.left >= this.right || this.top >= this.bottom;
				}
			}

			// Token: 0x040009E3 RID: 2531
			public int left;

			// Token: 0x040009E4 RID: 2532
			public int top;

			// Token: 0x040009E5 RID: 2533
			public int right;

			// Token: 0x040009E6 RID: 2534
			public int bottom;
		}
	}
}
