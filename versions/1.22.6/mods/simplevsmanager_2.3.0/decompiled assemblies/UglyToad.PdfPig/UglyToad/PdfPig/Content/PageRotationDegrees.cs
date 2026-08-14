using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001EE RID: 494
	public readonly struct PageRotationDegrees : IEquatable<PageRotationDegrees>
	{
		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000BF1 RID: 3057 RVA: 0x0003ABF9 File Offset: 0x00038DF9
		public int Value { get; }

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000BF2 RID: 3058 RVA: 0x0003AC01 File Offset: 0x00038E01
		public bool SwapsAxis
		{
			get
			{
				return this.Value == 90 || this.Value == 270;
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000BF3 RID: 3059 RVA: 0x0003AC1C File Offset: 0x00038E1C
		public decimal Radians
		{
			get
			{
				int value = this.Value;
				if (value <= 90)
				{
					if (value == 0)
					{
						return 0m;
					}
					if (value == 90)
					{
						return -1.5707963267949m;
					}
				}
				else
				{
					if (value == 180)
					{
						return -3.14159265358979m;
					}
					if (value == 270)
					{
						return -4.71238898038469m;
					}
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(29, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid value for rotation: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.Value);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x0003ACD0 File Offset: 0x00038ED0
		public PageRotationDegrees(int rotation)
		{
			if (rotation < 0)
			{
				rotation = 360 + rotation;
			}
			while (rotation >= 360)
			{
				rotation -= 360;
			}
			if (rotation != 0 && rotation != 90 && rotation != 180 && rotation != 270)
			{
				string text = "rotation";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Rotation must be 0, 90, 180 or 270. Got: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(rotation);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentOutOfRangeException(text, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.Value = rotation;
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x0003AD58 File Offset: 0x00038F58
		public override int GetHashCode()
		{
			return this.Value.GetHashCode();
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x0003AD74 File Offset: 0x00038F74
		public override string ToString()
		{
			return this.Value.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x0003AD94 File Offset: 0x00038F94
		public override bool Equals(object obj)
		{
			if (obj is PageRotationDegrees)
			{
				PageRotationDegrees other = (PageRotationDegrees)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x0003ADB9 File Offset: 0x00038FB9
		public bool Equals(PageRotationDegrees other)
		{
			return this.Value == other.Value;
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x0003ADCA File Offset: 0x00038FCA
		public static bool operator ==(PageRotationDegrees degrees1, PageRotationDegrees degrees2)
		{
			return degrees1.Equals(degrees2);
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x0003ADD4 File Offset: 0x00038FD4
		public static bool operator !=(PageRotationDegrees degrees1, PageRotationDegrees degrees2)
		{
			return !(degrees1 == degrees2);
		}
	}
}
