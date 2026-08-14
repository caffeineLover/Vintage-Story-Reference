using System;
using System.Globalization;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Parser.FileStructure
{
	// Token: 0x020000D4 RID: 212
	internal static class FileHeaderParser
	{
		// Token: 0x060003F9 RID: 1017 RVA: 0x00018E34 File Offset: 0x00017034
		[NotNull]
		public static HeaderVersion Parse([NotNull] ISeekableTokenScanner scanner, IInputBytes inputBytes, bool isLenientParsing, ILog log)
		{
			if (scanner == null)
			{
				throw new ArgumentNullException("scanner");
			}
			long currentPosition = scanner.CurrentPosition;
			int num = 0;
			while (num != 30 && scanner.MoveNext())
			{
				CommentToken commentToken = scanner.CurrentToken as CommentToken;
				num++;
				if (commentToken != null)
				{
					return FileHeaderParser.GetHeaderVersionAndResetScanner(commentToken, scanner, isLenientParsing, log);
				}
			}
			HeaderVersion result;
			if (!FileHeaderParser.TryBruteForceVersionLocation(currentPosition, inputBytes, out result))
			{
				throw new PdfDocumentFormatException("Could not find the version header comment at the start of the document.");
			}
			scanner.Seek(currentPosition);
			return result;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00018EA0 File Offset: 0x000170A0
		private static HeaderVersion GetHeaderVersionAndResetScanner(CommentToken comment, ISeekableTokenScanner scanner, bool isLenientParsing, ILog log)
		{
			if (comment.Data.IndexOf("PDF-1.", 5) != 0 && comment.Data.IndexOf("FDF-1.", 5) != 0)
			{
				return FileHeaderParser.HandleMissingVersion(comment, isLenientParsing, log);
			}
			decimal version;
			if (!decimal.TryParse(comment.Data.Substring(4), 111, CultureInfo.InvariantCulture, ref version))
			{
				return FileHeaderParser.HandleMissingVersion(comment, isLenientParsing, log);
			}
			int num = (scanner.CurrentPosition == scanner.Length) ? 1 : 2;
			long offsetInFile = scanner.CurrentPosition - (long)comment.Data.Length - (long)num;
			scanner.Seek(0L);
			return new HeaderVersion(version, comment.Data, offsetInFile);
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00018F44 File Offset: 0x00017144
		private static bool TryBruteForceVersionLocation(long startPosition, IInputBytes inputBytes, out HeaderVersion headerVersion)
		{
			headerVersion = null;
			inputBytes.Seek(startPosition);
			byte[] array = new byte[64];
			long num = startPosition;
			string text;
			int num5;
			decimal version;
			for (;;)
			{
				int num2 = inputBytes.Read(array, new int?(64));
				text = OtherEncodings.BytesAsLatin1String(array);
				int num3 = text.IndexOf("%PDF-", 5);
				int num4 = text.IndexOf("%FDF-", 5);
				num5 = ((num3 >= 0) ? num3 : num4);
				if (num5 >= 0 && text.Length - num5 >= 8 && decimal.TryParse(text.Substring(num5 + 5, 3), 111, CultureInfo.InvariantCulture, ref version))
				{
					break;
				}
				num += (long)(num2 - 8);
				inputBytes.Seek(num);
				if (num2 != 64)
				{
					return false;
				}
			}
			int num6 = num5 + 1;
			headerVersion = new HeaderVersion(version, text.Substring(num6, 7), num + (long)num5);
			inputBytes.Seek(startPosition);
			return true;
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00019010 File Offset: 0x00017210
		private static HeaderVersion HandleMissingVersion(CommentToken comment, bool isLenientParsing, ILog log)
		{
			if (isLenientParsing)
			{
				log.Warn("Did not find a version header of the correct format, defaulting to 1.4 since lenient. Header was: " + comment.Data + ".");
				return new HeaderVersion(1.4m, "PDF-1.4", 0L);
			}
			throw new PdfDocumentFormatException("The comment which should have provided the version was in the wrong format: " + comment.Data + ".");
		}
	}
}
