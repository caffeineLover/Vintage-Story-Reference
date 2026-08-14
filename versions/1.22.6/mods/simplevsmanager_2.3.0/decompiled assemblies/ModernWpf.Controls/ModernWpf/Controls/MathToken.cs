using System;

namespace ModernWpf.Controls
{
	// Token: 0x0200007A RID: 122
	internal struct MathToken
	{
		// Token: 0x060005FD RID: 1533 RVA: 0x000156B1 File Offset: 0x000138B1
		public MathToken(MathTokenType t, char c)
		{
			this.Type = t;
			this.Char = c;
			this.Value = double.NaN;
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x000156D0 File Offset: 0x000138D0
		public MathToken(MathTokenType t, double d)
		{
			this.Type = t;
			this.Value = d;
			this.Char = '\0';
		}

		// Token: 0x040002C6 RID: 710
		public MathTokenType Type;

		// Token: 0x040002C7 RID: 711
		public char Char;

		// Token: 0x040002C8 RID: 712
		public double Value;
	}
}
