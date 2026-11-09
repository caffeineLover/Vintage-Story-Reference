using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Images.Png
{
	// Token: 0x020000F5 RID: 245
	internal readonly struct HeaderValidationResult
	{
		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x0001BBB2 File Offset: 0x00019DB2
		public int Byte1 { get; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x0001BBBA File Offset: 0x00019DBA
		public int Byte2 { get; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x0001BBC2 File Offset: 0x00019DC2
		public int Byte3 { get; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x0001BBCA File Offset: 0x00019DCA
		public int Byte4 { get; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x0001BBD2 File Offset: 0x00019DD2
		public int Byte5 { get; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x0001BBDA File Offset: 0x00019DDA
		public int Byte6 { get; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x0001BBE2 File Offset: 0x00019DE2
		public int Byte7 { get; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x0001BBEA File Offset: 0x00019DEA
		public int Byte8 { get; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x0001BBF2 File Offset: 0x00019DF2
		public bool IsValid { get; }

		// Token: 0x0600049C RID: 1180 RVA: 0x0001BBFC File Offset: 0x00019DFC
		public HeaderValidationResult(int byte1, int byte2, int byte3, int byte4, int byte5, int byte6, int byte7, int byte8)
		{
			this.Byte1 = byte1;
			this.Byte2 = byte2;
			this.Byte3 = byte3;
			this.Byte4 = byte4;
			this.Byte5 = byte5;
			this.Byte6 = byte6;
			this.Byte7 = byte7;
			this.Byte8 = byte8;
			this.IsValid = (byte1 == (int)HeaderValidationResult.ExpectedHeader[0] && byte2 == (int)HeaderValidationResult.ExpectedHeader[1] && byte3 == (int)HeaderValidationResult.ExpectedHeader[2] && byte4 == (int)HeaderValidationResult.ExpectedHeader[3] && byte5 == (int)HeaderValidationResult.ExpectedHeader[4] && byte6 == (int)HeaderValidationResult.ExpectedHeader[5] && byte7 == (int)HeaderValidationResult.ExpectedHeader[6] && byte8 == (int)HeaderValidationResult.ExpectedHeader[7]);
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0001BCA4 File Offset: 0x00019EA4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(7, 8);
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Byte1);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Byte2);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Byte3);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Byte4);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Byte5);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Byte6);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Byte7);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Byte8);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000273 RID: 627
		public static readonly byte[] ExpectedHeader = new byte[]
		{
			137,
			80,
			78,
			71,
			13,
			10,
			26,
			10
		};
	}
}
