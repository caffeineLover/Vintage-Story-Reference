using System;
using System.Linq;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokenization.Scanner;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.Encryption
{
	// Token: 0x020001D1 RID: 465
	internal static class EncryptionDictionaryFactory
	{
		// Token: 0x06000B04 RID: 2820 RVA: 0x000378C8 File Offset: 0x00035AC8
		public static EncryptionDictionary Read(DictionaryToken encryptionDictionary, IPdfTokenScanner tokenScanner)
		{
			if (encryptionDictionary == null)
			{
				throw new ArgumentNullException("encryptionDictionary");
			}
			NameToken nameToken = encryptionDictionary.Get(NameToken.Filter, tokenScanner);
			EncryptionAlgorithmCode encryptionAlgorithmCode = EncryptionAlgorithmCode.Unrecognized;
			NumericToken numericToken;
			if (encryptionDictionary.TryGetOptionalTokenDirect(NameToken.V, tokenScanner, out numericToken))
			{
				encryptionAlgorithmCode = (EncryptionAlgorithmCode)numericToken.Int;
			}
			int? keyLength = default(int?);
			NumericToken numericToken2;
			if (encryptionDictionary.TryGetOptionalTokenDirect(NameToken.Length, tokenScanner, out numericToken2))
			{
				keyLength = new int?(numericToken2.Int);
			}
			int num = 0;
			NumericToken numericToken3;
			if (encryptionDictionary.TryGetOptionalTokenDirect(NameToken.R, tokenScanner, out numericToken3))
			{
				num = numericToken3.Int;
			}
			byte[] ownerBytes = null;
			IToken token;
			if (encryptionDictionary.TryGet(NameToken.O, ref token))
			{
				StringToken stringToken = token as StringToken;
				if (stringToken != null)
				{
					ownerBytes = stringToken.GetBytes();
				}
				else
				{
					HexToken hexToken = token as HexToken;
					if (hexToken != null)
					{
						ownerBytes = Enumerable.ToArray<byte>(hexToken.Bytes);
					}
				}
			}
			byte[] userBytes = null;
			IToken token2;
			if (encryptionDictionary.TryGet(NameToken.U, ref token2))
			{
				StringToken stringToken2 = token2 as StringToken;
				if (stringToken2 != null)
				{
					userBytes = stringToken2.GetBytes();
				}
				else
				{
					HexToken hexToken2 = token2 as HexToken;
					if (hexToken2 != null)
					{
						userBytes = Enumerable.ToArray<byte>(hexToken2.Bytes);
					}
				}
			}
			UserAccessPermissions userAccessPermissions = (UserAccessPermissions)0L;
			NumericToken numericToken4;
			if (encryptionDictionary.TryGetOptionalTokenDirect(NameToken.P, tokenScanner, out numericToken4))
			{
				userAccessPermissions = (UserAccessPermissions)numericToken4.Long;
			}
			byte[] userEncryptionBytes = null;
			byte[] ownerEncryptionBytes = null;
			if (num >= 5)
			{
				ownerEncryptionBytes = EncryptionDictionaryFactory.GetEncryptionBytesOrDefault(encryptionDictionary, tokenScanner, false);
				userEncryptionBytes = EncryptionDictionaryFactory.GetEncryptionBytesOrDefault(encryptionDictionary, tokenScanner, true);
			}
			BooleanToken booleanToken;
			encryptionDictionary.TryGetOptionalTokenDirect(NameToken.EncryptMetaData, tokenScanner, out booleanToken);
			return new EncryptionDictionary(nameToken.Data, encryptionAlgorithmCode, keyLength, num, ownerBytes, userBytes, ownerEncryptionBytes, userEncryptionBytes, userAccessPermissions, encryptionDictionary, booleanToken == null || booleanToken.Data);
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x00037A48 File Offset: 0x00035C48
		private static byte[] GetEncryptionBytesOrDefault(DictionaryToken encryptionDictionary, IPdfTokenScanner tokenScanner, bool isUser)
		{
			NameToken name = isUser ? NameToken.Ue : NameToken.Oe;
			StringToken stringToken;
			if (encryptionDictionary.TryGet(name, tokenScanner, out stringToken))
			{
				return OtherEncodings.StringAsLatin1Bytes(stringToken.Data);
			}
			HexToken hexToken;
			if (encryptionDictionary.TryGet(name, tokenScanner, out hexToken))
			{
				return Enumerable.ToArray<byte>(hexToken.Bytes);
			}
			return null;
		}
	}
}
