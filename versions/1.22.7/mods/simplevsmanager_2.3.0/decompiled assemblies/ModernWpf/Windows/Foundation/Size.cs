using System;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x0200002B RID: 43
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	internal struct Size
	{
		// Token: 0x060002B1 RID: 689 RVA: 0x000053FC File Offset: 0x000035FC
		public Size(float width, float height)
		{
			if (width < 0f)
			{
				throw new ArgumentOutOfRangeException("width", GSR.ArgumentOutOfRange_NeedNonNegNum);
			}
			if (height < 0f)
			{
				throw new ArgumentOutOfRangeException("height", GSR.ArgumentOutOfRange_NeedNonNegNum);
			}
			this._width = width;
			this._height = height;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000543C File Offset: 0x0000363C
		public Size(double width, double height)
		{
			this = new Size((float)width, (float)height);
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x00005448 File Offset: 0x00003648
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x00005451 File Offset: 0x00003651
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

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x00005477 File Offset: 0x00003677
		// (set) Token: 0x060002B6 RID: 694 RVA: 0x00005480 File Offset: 0x00003680
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

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x000054A6 File Offset: 0x000036A6
		public static Size Empty
		{
			get
			{
				return Size.s_empty;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x000054AD File Offset: 0x000036AD
		public bool IsEmpty
		{
			get
			{
				return this.Width < 0.0;
			}
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x000054C0 File Offset: 0x000036C0
		private static Size CreateEmptySize()
		{
			return new Size
			{
				_width = float.NegativeInfinity,
				_height = float.NegativeInfinity
			};
		}

		// Token: 0x060002BA RID: 698 RVA: 0x000054EE File Offset: 0x000036EE
		public static bool operator ==(Size size1, Size size2)
		{
			return size1._width == size2._width && size1._height == size2._height;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000550E File Offset: 0x0000370E
		public static bool operator !=(Size size1, Size size2)
		{
			return !(size1 == size2);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000551A File Offset: 0x0000371A
		public override bool Equals(object o)
		{
			return o is Size && Size.Equals(this, (Size)o);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00005537 File Offset: 0x00003737
		public bool Equals(Size value)
		{
			return Size.Equals(this, value);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00005548 File Offset: 0x00003748
		public override int GetHashCode()
		{
			if (this.IsEmpty)
			{
				return 0;
			}
			return this.Width.GetHashCode() ^ this.Height.GetHashCode();
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000557C File Offset: 0x0000377C
		private static bool Equals(Size size1, Size size2)
		{
			if (size1.IsEmpty)
			{
				return size2.IsEmpty;
			}
			return size1._width.Equals(size2._width) && size1._height.Equals(size2._height);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x000055B7 File Offset: 0x000037B7
		public override string ToString()
		{
			if (this.IsEmpty)
			{
				return "Empty";
			}
			return string.Format("{0},{1}", this._width, this._height);
		}

		// Token: 0x04000087 RID: 135
		public float _width;

		// Token: 0x04000088 RID: 136
		public float _height;

		// Token: 0x04000089 RID: 137
		private static readonly Size s_empty = Size.CreateEmptySize();
	}
}
