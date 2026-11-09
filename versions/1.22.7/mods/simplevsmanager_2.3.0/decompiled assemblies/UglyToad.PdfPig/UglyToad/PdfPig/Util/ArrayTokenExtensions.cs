using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Util
{
	// Token: 0x0200002D RID: 45
	internal static class ArrayTokenExtensions
	{
		// Token: 0x0600012F RID: 303 RVA: 0x0000BB30 File Offset: 0x00009D30
		public static NumericToken GetNumeric(this ArrayToken array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (index < 0 || index >= array.Data.Count)
			{
				defaultInterpolatedStringHandler..ctor(47, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot index into array at index ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(index);
				defaultInterpolatedStringHandler.AppendLiteral(". Array was: ");
				defaultInterpolatedStringHandler.AppendFormatted<ArrayToken>(array);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			NumericToken numericToken = array.Data[index] as NumericToken;
			if (numericToken != null)
			{
				return numericToken;
			}
			defaultInterpolatedStringHandler..ctor(58, 2);
			defaultInterpolatedStringHandler.AppendLiteral("The array did not contain a number at index ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(index);
			defaultInterpolatedStringHandler.AppendLiteral(". Array was: ");
			defaultInterpolatedStringHandler.AppendFormatted<ArrayToken>(array);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000BC0C File Offset: 0x00009E0C
		public static PdfRectangle ToRectangle(this ArrayToken array, IPdfTokenScanner tokenScanner)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Data.Count != 4)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(67, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot convert array to rectangle, expected 4 values instead got: ");
				defaultInterpolatedStringHandler.AppendFormatted<ArrayToken>(array);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return new PdfRectangle(DirectObjectFinder.Get<NumericToken>(array[0], tokenScanner).Double, DirectObjectFinder.Get<NumericToken>(array[1], tokenScanner).Double, DirectObjectFinder.Get<NumericToken>(array[2], tokenScanner).Double, DirectObjectFinder.Get<NumericToken>(array[3], tokenScanner).Double);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000BCBC File Offset: 0x00009EBC
		public static PdfRectangle ToIntRectangle(this ArrayToken array, IPdfTokenScanner tokenScanner)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Data.Count != 4)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(67, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot convert array to rectangle, expected 4 values instead got: ");
				defaultInterpolatedStringHandler.AppendFormatted<ArrayToken>(array);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return new PdfRectangle((double)DirectObjectFinder.Get<NumericToken>(array[0], tokenScanner).Int, (double)DirectObjectFinder.Get<NumericToken>(array[1], tokenScanner).Int, (double)DirectObjectFinder.Get<NumericToken>(array[2], tokenScanner).Int, (double)DirectObjectFinder.Get<NumericToken>(array[3], tokenScanner).Int);
		}
	}
}
