using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat.Dictionaries
{
	// Token: 0x02000099 RID: 153
	internal class CompactFontFormatTopLevelDictionary
	{
		// Token: 0x17000187 RID: 391
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00019C79 File Offset: 0x00017E79
		// (set) Token: 0x0600036E RID: 878 RVA: 0x00019C81 File Offset: 0x00017E81
		public string Version { get; set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600036F RID: 879 RVA: 0x00019C8A File Offset: 0x00017E8A
		// (set) Token: 0x06000370 RID: 880 RVA: 0x00019C92 File Offset: 0x00017E92
		public string Notice { get; set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000371 RID: 881 RVA: 0x00019C9B File Offset: 0x00017E9B
		// (set) Token: 0x06000372 RID: 882 RVA: 0x00019CA3 File Offset: 0x00017EA3
		public string Copyright { get; set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000373 RID: 883 RVA: 0x00019CAC File Offset: 0x00017EAC
		// (set) Token: 0x06000374 RID: 884 RVA: 0x00019CB4 File Offset: 0x00017EB4
		public string FullName { get; set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000375 RID: 885 RVA: 0x00019CBD File Offset: 0x00017EBD
		// (set) Token: 0x06000376 RID: 886 RVA: 0x00019CC5 File Offset: 0x00017EC5
		public string FamilyName { get; set; }

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000377 RID: 887 RVA: 0x00019CCE File Offset: 0x00017ECE
		// (set) Token: 0x06000378 RID: 888 RVA: 0x00019CD6 File Offset: 0x00017ED6
		public string Weight { get; set; }

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000379 RID: 889 RVA: 0x00019CDF File Offset: 0x00017EDF
		// (set) Token: 0x0600037A RID: 890 RVA: 0x00019CE7 File Offset: 0x00017EE7
		public bool IsFixedPitch { get; set; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x0600037B RID: 891 RVA: 0x00019CF0 File Offset: 0x00017EF0
		// (set) Token: 0x0600037C RID: 892 RVA: 0x00019CF8 File Offset: 0x00017EF8
		public decimal ItalicAngle { get; set; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x0600037D RID: 893 RVA: 0x00019D01 File Offset: 0x00017F01
		// (set) Token: 0x0600037E RID: 894 RVA: 0x00019D09 File Offset: 0x00017F09
		public decimal UnderlinePosition { get; set; } = -100m;

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x0600037F RID: 895 RVA: 0x00019D12 File Offset: 0x00017F12
		// (set) Token: 0x06000380 RID: 896 RVA: 0x00019D1A File Offset: 0x00017F1A
		public decimal UnderlineThickness { get; set; } = 50m;

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000381 RID: 897 RVA: 0x00019D23 File Offset: 0x00017F23
		// (set) Token: 0x06000382 RID: 898 RVA: 0x00019D2B File Offset: 0x00017F2B
		public decimal PaintType { get; set; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000383 RID: 899 RVA: 0x00019D34 File Offset: 0x00017F34
		// (set) Token: 0x06000384 RID: 900 RVA: 0x00019D3C File Offset: 0x00017F3C
		public CompactFontFormatCharStringType CharStringType { get; set; } = CompactFontFormatCharStringType.Type2;

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000385 RID: 901 RVA: 0x00019D45 File Offset: 0x00017F45
		// (set) Token: 0x06000386 RID: 902 RVA: 0x00019D4D File Offset: 0x00017F4D
		public TransformationMatrix FontMatrix { get; set; } = TransformationMatrix.FromValues(0.001, 0.0, 0.0, 0.001, 0.0, 0.0);

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000387 RID: 903 RVA: 0x00019D56 File Offset: 0x00017F56
		// (set) Token: 0x06000388 RID: 904 RVA: 0x00019D5E File Offset: 0x00017F5E
		public decimal StrokeWidth { get; set; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000389 RID: 905 RVA: 0x00019D67 File Offset: 0x00017F67
		// (set) Token: 0x0600038A RID: 906 RVA: 0x00019D6F File Offset: 0x00017F6F
		public decimal UniqueId { get; set; }

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x0600038B RID: 907 RVA: 0x00019D78 File Offset: 0x00017F78
		// (set) Token: 0x0600038C RID: 908 RVA: 0x00019D80 File Offset: 0x00017F80
		public PdfRectangle FontBoundingBox { get; set; } = new PdfRectangle(0, 0, 0, 0);

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00019D89 File Offset: 0x00017F89
		// (set) Token: 0x0600038E RID: 910 RVA: 0x00019D91 File Offset: 0x00017F91
		public decimal[] Xuid { get; set; }

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00019D9A File Offset: 0x00017F9A
		// (set) Token: 0x06000390 RID: 912 RVA: 0x00019DA2 File Offset: 0x00017FA2
		public int CharSetOffset { get; set; } = -1;

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000391 RID: 913 RVA: 0x00019DAB File Offset: 0x00017FAB
		// (set) Token: 0x06000392 RID: 914 RVA: 0x00019DB3 File Offset: 0x00017FB3
		public int EncodingOffset { get; set; } = -1;

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000393 RID: 915 RVA: 0x00019DBC File Offset: 0x00017FBC
		// (set) Token: 0x06000394 RID: 916 RVA: 0x00019DC4 File Offset: 0x00017FC4
		public CompactFontFormatTopLevelDictionary.SizeAndOffset? PrivateDictionaryLocation { get; set; }

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000395 RID: 917 RVA: 0x00019DCD File Offset: 0x00017FCD
		// (set) Token: 0x06000396 RID: 918 RVA: 0x00019DD5 File Offset: 0x00017FD5
		public int CharStringsOffset { get; set; } = -1;

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000397 RID: 919 RVA: 0x00019DDE File Offset: 0x00017FDE
		// (set) Token: 0x06000398 RID: 920 RVA: 0x00019DE6 File Offset: 0x00017FE6
		public int SyntheticBaseFontIndex { get; set; }

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000399 RID: 921 RVA: 0x00019DEF File Offset: 0x00017FEF
		// (set) Token: 0x0600039A RID: 922 RVA: 0x00019DF7 File Offset: 0x00017FF7
		public string PostScript { get; set; }

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x0600039B RID: 923 RVA: 0x00019E00 File Offset: 0x00018000
		// (set) Token: 0x0600039C RID: 924 RVA: 0x00019E08 File Offset: 0x00018008
		public string BaseFontName { get; set; }

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600039D RID: 925 RVA: 0x00019E11 File Offset: 0x00018011
		// (set) Token: 0x0600039E RID: 926 RVA: 0x00019E19 File Offset: 0x00018019
		public decimal[] BaseFontBlend { get; set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x0600039F RID: 927 RVA: 0x00019E22 File Offset: 0x00018022
		// (set) Token: 0x060003A0 RID: 928 RVA: 0x00019E2A File Offset: 0x0001802A
		public bool IsCidFont { get; set; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x00019E33 File Offset: 0x00018033
		// (set) Token: 0x060003A2 RID: 930 RVA: 0x00019E3B File Offset: 0x0001803B
		public CidFontOperators CidFontOperators { get; set; } = new CidFontOperators();

		// Token: 0x0400032E RID: 814
		public const int UnsetOffset = -1;

		// Token: 0x020000DE RID: 222
		public struct SizeAndOffset
		{
			// Token: 0x17000271 RID: 625
			// (get) Token: 0x06000585 RID: 1413 RVA: 0x0002043D File Offset: 0x0001E63D
			public readonly int Size { get; }

			// Token: 0x17000272 RID: 626
			// (get) Token: 0x06000586 RID: 1414 RVA: 0x00020445 File Offset: 0x0001E645
			public readonly int Offset { get; }

			// Token: 0x06000587 RID: 1415 RVA: 0x0002044D File Offset: 0x0001E64D
			public SizeAndOffset(int size, int offset)
			{
				this.Size = size;
				this.Offset = offset;
			}

			// Token: 0x06000588 RID: 1416 RVA: 0x00020460 File Offset: 0x0001E660
			public override string ToString()
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(16, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.Size);
				defaultInterpolatedStringHandler.AppendLiteral(", Offset: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.Offset);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}
	}
}
