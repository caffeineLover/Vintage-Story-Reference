using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Parser.Parts
{
	// Token: 0x020000CC RID: 204
	internal static class ObjectHelper
	{
		// Token: 0x060003DA RID: 986 RVA: 0x00017900 File Offset: 0x00015B00
		public static long ReadObjectNumber(IInputBytes bytes)
		{
			long num = ReadHelper.ReadLong(bytes);
			if (num < 0L || num >= 10000000000L)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(55, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Object Number '");
				defaultInterpolatedStringHandler.AppendFormatted<long>(num);
				defaultInterpolatedStringHandler.AppendLiteral("' has more than 10 digits or is negative");
				throw new FormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return num;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00017960 File Offset: 0x00015B60
		public static int ReadGenerationNumber(IInputBytes bytes)
		{
			int num = ReadHelper.ReadInt(bytes);
			if (num < 0 || (long)num > 65535L)
			{
				throw new FormatException("Generation Number '" + num.ToString() + "' has more than 5 digits");
			}
			return num;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000179A0 File Offset: 0x00015BA0
		public static string CreateObjectString(long objectId, long genId)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(5, 2);
			defaultInterpolatedStringHandler.AppendFormatted<long>(objectId);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<long>(genId);
			defaultInterpolatedStringHandler.AppendLiteral(" obj");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x040001F6 RID: 502
		private const long ObjectNumberThreshold = 10000000000L;

		// Token: 0x040001F7 RID: 503
		private const long GenerationNumberThreshold = 65535L;
	}
}
