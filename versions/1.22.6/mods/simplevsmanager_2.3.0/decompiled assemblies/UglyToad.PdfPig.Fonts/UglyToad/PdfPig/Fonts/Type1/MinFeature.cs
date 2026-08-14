using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Fonts.Type1
{
	// Token: 0x02000008 RID: 8
	public class MinFeature
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002895 File Offset: 0x00000A95
		public int First { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600002E RID: 46 RVA: 0x0000289D File Offset: 0x00000A9D
		public int Second { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600002F RID: 47 RVA: 0x000028A5 File Offset: 0x00000AA5
		public static MinFeature Default { get; } = new MinFeature(16, 16);

		// Token: 0x06000030 RID: 48 RVA: 0x000028AC File Offset: 0x00000AAC
		public MinFeature(int first, int second)
		{
			this.First = first;
			this.Second = second;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000028C4 File Offset: 0x00000AC4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(3, 2);
			defaultInterpolatedStringHandler.AppendLiteral("{");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.First);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Second);
			defaultInterpolatedStringHandler.AppendLiteral("}");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
