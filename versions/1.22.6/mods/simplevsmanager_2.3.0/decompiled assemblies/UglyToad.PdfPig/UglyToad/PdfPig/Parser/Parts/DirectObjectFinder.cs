using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Parser.Parts
{
	// Token: 0x020000CA RID: 202
	internal static class DirectObjectFinder
	{
		// Token: 0x060003D5 RID: 981 RVA: 0x00017588 File Offset: 0x00015788
		public static bool TryGet<T>(IToken token, IPdfTokenScanner scanner, out T tokenResult) where T : IToken
		{
			tokenResult = default(T);
			if (token is T)
			{
				T t = (T)((object)token);
				tokenResult = t;
				return true;
			}
			IndirectReferenceToken indirectReferenceToken = token as IndirectReferenceToken;
			if (indirectReferenceToken == null)
			{
				return false;
			}
			try
			{
				ObjectToken objectToken = scanner.Get(indirectReferenceToken.Data);
				IToken data = objectToken.Data;
				if (data is T)
				{
					T t2 = (T)((object)data);
					tokenResult = t2;
					return true;
				}
				IndirectReferenceToken indirectReferenceToken2 = objectToken.Data as IndirectReferenceToken;
				if (indirectReferenceToken2 != null)
				{
					return DirectObjectFinder.TryGet<T>(indirectReferenceToken2, scanner, out tokenResult);
				}
			}
			catch
			{
				return false;
			}
			return false;
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00017630 File Offset: 0x00015830
		public static T Get<T>(IndirectReference reference, IPdfTokenScanner scanner) where T : class, IToken
		{
			ObjectToken objectToken = scanner.Get(reference);
			if (objectToken == null || objectToken.Data is NullToken)
			{
				return default(T);
			}
			T t = objectToken.Data as T;
			if (t != null)
			{
				return t;
			}
			IndirectReferenceToken indirectReferenceToken = objectToken.Data as IndirectReferenceToken;
			if (indirectReferenceToken != null)
			{
				return DirectObjectFinder.Get<T>(indirectReferenceToken, scanner);
			}
			ArrayToken arrayToken = objectToken.Data as ArrayToken;
			if (arrayToken != null && arrayToken.Data.Count == 1)
			{
				IToken token = arrayToken.Data[0];
				IndirectReferenceToken indirectReferenceToken2 = token as IndirectReferenceToken;
				if (indirectReferenceToken2 != null)
				{
					return DirectObjectFinder.Get<T>(indirectReferenceToken2, scanner);
				}
				T t2 = token as T;
				if (t2 != null)
				{
					return t2;
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(78, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Could not find the object number ");
			defaultInterpolatedStringHandler.AppendFormatted<IndirectReference>(reference);
			defaultInterpolatedStringHandler.AppendLiteral(" with type ");
			defaultInterpolatedStringHandler.AppendFormatted(typeof(T).Name);
			defaultInterpolatedStringHandler.AppendLiteral(" instead, it was found with type ");
			defaultInterpolatedStringHandler.AppendFormatted(objectToken.GetType().Name);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00017764 File Offset: 0x00015964
		public static T Get<T>(IToken token, IPdfTokenScanner scanner) where T : class, IToken
		{
			T t = token as T;
			if (t != null)
			{
				return t;
			}
			IndirectReferenceToken indirectReferenceToken = token as IndirectReferenceToken;
			if (indirectReferenceToken != null)
			{
				return DirectObjectFinder.Get<T>(indirectReferenceToken.Data, scanner);
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(71, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Could not find the object ");
			defaultInterpolatedStringHandler.AppendFormatted<IToken>(token);
			defaultInterpolatedStringHandler.AppendLiteral(" with type ");
			defaultInterpolatedStringHandler.AppendFormatted(typeof(T).Name);
			defaultInterpolatedStringHandler.AppendLiteral(" instead, it was found with type ");
			defaultInterpolatedStringHandler.AppendFormatted(token.GetType().Name);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
		}
	}
}
