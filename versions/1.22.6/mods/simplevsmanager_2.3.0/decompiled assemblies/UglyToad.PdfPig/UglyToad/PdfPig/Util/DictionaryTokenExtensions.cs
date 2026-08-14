using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Parser.Parts;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Util
{
	// Token: 0x0200002C RID: 44
	internal static class DictionaryTokenExtensions
	{
		// Token: 0x06000125 RID: 293 RVA: 0x0000B928 File Offset: 0x00009B28
		[CanBeNull]
		public static IToken GetObjectOrDefault(this DictionaryToken token, NameToken name)
		{
			IToken result;
			if (token.TryGet(name, ref result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000B944 File Offset: 0x00009B44
		[CanBeNull]
		public static IToken GetObjectOrDefault(this DictionaryToken token, NameToken first, NameToken second)
		{
			IToken result;
			if (token.TryGet(first, ref result))
			{
				return result;
			}
			if (token.TryGet(second, ref result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000B96C File Offset: 0x00009B6C
		public static int GetInt(this DictionaryToken token, NameToken name)
		{
			if (token == null)
			{
				throw new ArgumentNullException("token");
			}
			NumericToken numericToken = token.GetObjectOrDefault(name) as NumericToken;
			if (numericToken == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(72, 2);
				defaultInterpolatedStringHandler.AppendLiteral("The dictionary did not contain a number with the key ");
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(name);
				defaultInterpolatedStringHandler.AppendLiteral(". Dictionary way: ");
				defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(token);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return numericToken.Int;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000B9E7 File Offset: 0x00009BE7
		public static int GetIntOrDefault(this DictionaryToken token, NameToken name, int defaultValue = 0)
		{
			if (token == null)
			{
				throw new ArgumentNullException("token");
			}
			NumericToken numericToken = token.GetObjectOrDefault(name) as NumericToken;
			if (numericToken == null)
			{
				return defaultValue;
			}
			return numericToken.Int;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000BA0E File Offset: 0x00009C0E
		public static int GetIntOrDefault(this DictionaryToken token, NameToken first, NameToken second, int defaultValue = 0)
		{
			if (token == null)
			{
				throw new ArgumentNullException("token");
			}
			NumericToken numericToken = token.GetObjectOrDefault(first, second) as NumericToken;
			if (numericToken == null)
			{
				return 0;
			}
			return numericToken.Int;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000BA38 File Offset: 0x00009C38
		public static long? GetLongOrDefault(this DictionaryToken token, NameToken name)
		{
			if (token == null)
			{
				throw new ArgumentNullException("token");
			}
			NumericToken numericToken = token.GetObjectOrDefault(name) as NumericToken;
			if (numericToken == null)
			{
				return default(long?);
			}
			return new long?(numericToken.Long);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000BA77 File Offset: 0x00009C77
		public static bool GetBooleanOrDefault(this DictionaryToken token, NameToken name, bool defaultValue)
		{
			if (token == null)
			{
				throw new ArgumentNullException("token");
			}
			BooleanToken booleanToken = token.GetObjectOrDefault(name) as BooleanToken;
			if (booleanToken == null)
			{
				return defaultValue;
			}
			return booleanToken.Data;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000BA9E File Offset: 0x00009C9E
		[CanBeNull]
		public static NameToken GetNameOrDefault(this DictionaryToken token, NameToken name)
		{
			if (token == null)
			{
				throw new ArgumentNullException("token");
			}
			return token.GetObjectOrDefault(name) as NameToken;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000BABC File Offset: 0x00009CBC
		public static bool TryGetOptionalTokenDirect<T>(this DictionaryToken token, NameToken name, IPdfTokenScanner scanner, out T result) where T : IToken
		{
			result = default(T);
			IToken token2;
			T t;
			if (token.TryGet(name, ref token2) && DirectObjectFinder.TryGet<T>(token2, scanner, out t))
			{
				result = t;
				return true;
			}
			return false;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000BAF0 File Offset: 0x00009CF0
		public static bool TryGetOptionalStringDirect(this DictionaryToken token, NameToken name, IPdfTokenScanner scanner, out string result)
		{
			result = null;
			StringToken stringToken;
			if (token.TryGetOptionalTokenDirect(name, scanner, out stringToken))
			{
				result = stringToken.Data;
				return true;
			}
			HexToken hexToken;
			if (token.TryGetOptionalTokenDirect(name, scanner, out hexToken))
			{
				result = hexToken.Data;
				return true;
			}
			return false;
		}
	}
}
