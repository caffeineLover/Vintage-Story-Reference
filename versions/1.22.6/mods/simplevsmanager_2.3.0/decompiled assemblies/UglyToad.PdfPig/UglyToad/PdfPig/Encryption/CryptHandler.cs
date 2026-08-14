using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Exceptions;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Encryption
{
	// Token: 0x020001CE RID: 462
	internal class CryptHandler
	{
		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x000373A1 File Offset: 0x000355A1
		public CryptDictionary StreamDictionary { get; }

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x000373A9 File Offset: 0x000355A9
		public CryptDictionary StringDictionary { get; }

		// Token: 0x06000AF3 RID: 2803 RVA: 0x000373B4 File Offset: 0x000355B4
		public CryptHandler(DictionaryToken cryptDictionary, NameToken streamName, NameToken stringName)
		{
			if (streamName == null)
			{
				throw new ArgumentNullException("streamName");
			}
			if (stringName == null)
			{
				throw new ArgumentNullException("stringName");
			}
			if (cryptDictionary == null)
			{
				throw new ArgumentNullException("cryptDictionary");
			}
			this.cryptDictionary = cryptDictionary;
			this.StreamDictionary = CryptHandler.ParseCryptDictionary(cryptDictionary, streamName);
			this.StringDictionary = CryptHandler.ParseCryptDictionary(cryptDictionary, stringName);
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0003741F File Offset: 0x0003561F
		public CryptDictionary GetNamedCryptDictionary(NameToken name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return CryptHandler.ParseCryptDictionary(this.cryptDictionary, name);
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x00037444 File Offset: 0x00035644
		private static CryptDictionary ParseCryptDictionary(DictionaryToken cryptDictionary, NameToken name)
		{
			if (name == NameToken.Identity)
			{
				return CryptDictionary.Identity;
			}
			DictionaryToken dictionaryToken;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (!cryptDictionary.TryGet<DictionaryToken>(name, ref dictionaryToken))
			{
				defaultInterpolatedStringHandler..ctor(72, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Could not find named crypt filter ");
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(name);
				defaultInterpolatedStringHandler.AppendLiteral(" for decryption in crypt dictionary: ");
				defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionaryToken);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentEncryptedException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			NameToken nameToken;
			if (dictionaryToken.TryGet<NameToken>(NameToken.Type, ref nameToken) && nameToken != NameToken.CryptFilter && nameToken != NameToken.CryptAlgorithm)
			{
				defaultInterpolatedStringHandler..ctor(51, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid crypt dictionary type ");
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(nameToken);
				defaultInterpolatedStringHandler.AppendLiteral(" for crypt filter ");
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(name);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionaryToken);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentEncryptedException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			NameToken nameToken3;
			NameToken nameToken2 = dictionaryToken.TryGet<NameToken>(NameToken.Cfm, ref nameToken3) ? nameToken3 : NameToken.None;
			CryptDictionary.Method name2;
			if (nameToken2 == NameToken.None)
			{
				name2 = CryptDictionary.Method.None;
			}
			else if (nameToken2 == NameToken.V2)
			{
				name2 = CryptDictionary.Method.V2;
			}
			else if (nameToken2 == NameToken.Aesv2)
			{
				name2 = CryptDictionary.Method.AesV2;
			}
			else
			{
				if (!(nameToken2 == NameToken.Aesv3))
				{
					defaultInterpolatedStringHandler..ctor(44, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Unrecognized CFM option for crypt filter ");
					defaultInterpolatedStringHandler.AppendFormatted<NameToken>(nameToken3);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionaryToken);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new PdfDocumentEncryptedException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				name2 = CryptDictionary.Method.AesV3;
			}
			NameToken nameToken5;
			NameToken nameToken4 = dictionaryToken.TryGet<NameToken>(NameToken.AuthEvent, ref nameToken5) ? nameToken5 : NameToken.DocOpen;
			CryptDictionary.TriggerEvent @event;
			if (nameToken4 == NameToken.DocOpen)
			{
				@event = CryptDictionary.TriggerEvent.DocumentOpen;
			}
			else
			{
				if (!(nameToken4 == NameToken.EfOpen))
				{
					defaultInterpolatedStringHandler..ctor(50, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Unrecognized AuthEvent option for crypt filter ");
					defaultInterpolatedStringHandler.AppendFormatted<NameToken>(nameToken4);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionaryToken);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new PdfDocumentEncryptedException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				@event = CryptDictionary.TriggerEvent.EmbeddedFileOpen;
			}
			NumericToken numericToken;
			int length = dictionaryToken.TryGet<NumericToken>(NameToken.Length, ref numericToken) ? numericToken.Int : 0;
			return new CryptDictionary(name2, @event, length);
		}

		// Token: 0x04000595 RID: 1429
		private readonly DictionaryToken cryptDictionary;
	}
}
