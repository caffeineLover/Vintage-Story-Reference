using System;
using System.Globalization;

namespace UglyToad.PdfPig.Geometry
{
	// Token: 0x0200018C RID: 396
	internal readonly struct UserSpaceUnit
	{
		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x0002C373 File Offset: 0x0002A573
		public int PointMultiples { get; }

		// Token: 0x06000972 RID: 2418 RVA: 0x0002C37B File Offset: 0x0002A57B
		public UserSpaceUnit(int pointMultiples)
		{
			if (pointMultiples <= 0)
			{
				throw new ArgumentOutOfRangeException("Cannot have a zero or negative value of point multiples: " + pointMultiples.ToString());
			}
			this.PointMultiples = pointMultiples;
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0002C3A0 File Offset: 0x0002A5A0
		public override string ToString()
		{
			return this.PointMultiples.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x04000483 RID: 1155
		public static readonly UserSpaceUnit Default = new UserSpaceUnit(1);
	}
}
