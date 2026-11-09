using System;
using System.Globalization;
using System.IO;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Graphics.Operations
{
	// Token: 0x02000113 RID: 275
	internal static class OperationWriteHelper
	{
		// Token: 0x060005F8 RID: 1528 RVA: 0x00022124 File Offset: 0x00020324
		public static void WriteText(this Stream stream, string text, bool appendWhitespace = false)
		{
			byte[] array = OtherEncodings.StringAsLatin1Bytes(text);
			stream.Write(array, 0, array.Length);
			if (appendWhitespace)
			{
				stream.WriteWhiteSpace();
			}
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0002214C File Offset: 0x0002034C
		public static void WriteHex(this Stream stream, byte[] bytes)
		{
			string text = BitConverter.ToString(bytes).Replace("-", string.Empty);
			stream.WriteText("<" + text + ">", false);
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00022186 File Offset: 0x00020386
		public static void WriteWhiteSpace(this Stream stream)
		{
			stream.WriteByte(OperationWriteHelper.WhiteSpace);
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00022193 File Offset: 0x00020393
		public static void WriteNewLine(this Stream stream)
		{
			stream.WriteByte(OperationWriteHelper.NewLine);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x000221A0 File Offset: 0x000203A0
		public static void WriteDecimal(this Stream stream, decimal value)
		{
			stream.WriteText(value.ToString("G", CultureInfo.InvariantCulture), false);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x000221BA File Offset: 0x000203BA
		public static void WriteNumberText(this Stream stream, decimal number, string text)
		{
			stream.WriteDecimal(number);
			stream.WriteWhiteSpace();
			stream.WriteText(text, false);
			stream.WriteNewLine();
		}

		// Token: 0x040002F8 RID: 760
		private static readonly byte WhiteSpace = OtherEncodings.StringAsLatin1Bytes(" ")[0];

		// Token: 0x040002F9 RID: 761
		private static readonly byte NewLine = OtherEncodings.StringAsLatin1Bytes("\n")[0];
	}
}
