using System;

namespace Standard
{
	// Token: 0x020001FF RID: 511
	internal struct RECT
	{
		// Token: 0x06000F08 RID: 3848 RVA: 0x0002B691 File Offset: 0x00029891
		public void Offset(int dx, int dy)
		{
			this._left += dx;
			this._top += dy;
			this._right += dx;
			this._bottom += dy;
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000F09 RID: 3849 RVA: 0x0002B6CB File Offset: 0x000298CB
		// (set) Token: 0x06000F0A RID: 3850 RVA: 0x0002B6D3 File Offset: 0x000298D3
		public int Left
		{
			get
			{
				return this._left;
			}
			set
			{
				this._left = value;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000F0B RID: 3851 RVA: 0x0002B6DC File Offset: 0x000298DC
		// (set) Token: 0x06000F0C RID: 3852 RVA: 0x0002B6E4 File Offset: 0x000298E4
		public int Right
		{
			get
			{
				return this._right;
			}
			set
			{
				this._right = value;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000F0D RID: 3853 RVA: 0x0002B6ED File Offset: 0x000298ED
		// (set) Token: 0x06000F0E RID: 3854 RVA: 0x0002B6F5 File Offset: 0x000298F5
		public int Top
		{
			get
			{
				return this._top;
			}
			set
			{
				this._top = value;
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000F0F RID: 3855 RVA: 0x0002B6FE File Offset: 0x000298FE
		// (set) Token: 0x06000F10 RID: 3856 RVA: 0x0002B706 File Offset: 0x00029906
		public int Bottom
		{
			get
			{
				return this._bottom;
			}
			set
			{
				this._bottom = value;
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000F11 RID: 3857 RVA: 0x0002B70F File Offset: 0x0002990F
		public int Width
		{
			get
			{
				return this._right - this._left;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000F12 RID: 3858 RVA: 0x0002B71E File Offset: 0x0002991E
		public int Height
		{
			get
			{
				return this._bottom - this._top;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000F13 RID: 3859 RVA: 0x0002B730 File Offset: 0x00029930
		public POINT Position
		{
			get
			{
				return new POINT
				{
					x = this._left,
					y = this._top
				};
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000F14 RID: 3860 RVA: 0x0002B760 File Offset: 0x00029960
		public SIZE Size
		{
			get
			{
				return new SIZE
				{
					cx = this.Width,
					cy = this.Height
				};
			}
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x0002B790 File Offset: 0x00029990
		public static RECT Union(RECT rect1, RECT rect2)
		{
			return new RECT
			{
				Left = Math.Min(rect1.Left, rect2.Left),
				Top = Math.Min(rect1.Top, rect2.Top),
				Right = Math.Max(rect1.Right, rect2.Right),
				Bottom = Math.Max(rect1.Bottom, rect2.Bottom)
			};
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x0002B810 File Offset: 0x00029A10
		public override bool Equals(object obj)
		{
			bool result;
			try
			{
				RECT rect = (RECT)obj;
				result = (rect._bottom == this._bottom && rect._left == this._left && rect._right == this._right && rect._top == this._top);
			}
			catch (InvalidCastException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x0002B878 File Offset: 0x00029A78
		public override int GetHashCode()
		{
			return (this._left << 16 | Utility.LOWORD(this._right)) ^ (this._top << 16 | Utility.LOWORD(this._bottom));
		}

		// Token: 0x0400031E RID: 798
		private int _left;

		// Token: 0x0400031F RID: 799
		private int _top;

		// Token: 0x04000320 RID: 800
		private int _right;

		// Token: 0x04000321 RID: 801
		private int _bottom;
	}
}
