using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Exceptions;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Encryption
{
	// Token: 0x020001D0 RID: 464
	internal class EncryptionDictionary
	{
		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x000376A2 File Offset: 0x000358A2
		public string Filter { get; }

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x000376AA File Offset: 0x000358AA
		public EncryptionAlgorithmCode EncryptionAlgorithmCode { get; }

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x000376B2 File Offset: 0x000358B2
		public int? KeyLength { get; }

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000AF9 RID: 2809 RVA: 0x000376BA File Offset: 0x000358BA
		public int Revision { get; }

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000AFA RID: 2810 RVA: 0x000376C2 File Offset: 0x000358C2
		public byte[] OwnerBytes { get; }

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000AFB RID: 2811 RVA: 0x000376CA File Offset: 0x000358CA
		public byte[] UserBytes { get; }

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000AFC RID: 2812 RVA: 0x000376D2 File Offset: 0x000358D2
		[CanBeNull]
		public byte[] OwnerEncryptionBytes { get; }

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000AFD RID: 2813 RVA: 0x000376DA File Offset: 0x000358DA
		[CanBeNull]
		public byte[] UserEncryptionBytes { get; }

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000AFE RID: 2814 RVA: 0x000376E2 File Offset: 0x000358E2
		public UserAccessPermissions UserAccessPermissions { get; }

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x000376EA File Offset: 0x000358EA
		public bool IsStandardFilter
		{
			get
			{
				return string.Equals(this.Filter, "Standard", 5);
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000B00 RID: 2816 RVA: 0x000376FD File Offset: 0x000358FD
		public bool EncryptMetadata { get; }

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000B01 RID: 2817 RVA: 0x00037705 File Offset: 0x00035905
		public DictionaryToken Dictionary { get; }

		// Token: 0x06000B02 RID: 2818 RVA: 0x00037710 File Offset: 0x00035910
		public EncryptionDictionary(string filter, EncryptionAlgorithmCode encryptionAlgorithmCode, int? keyLength, int revision, byte[] ownerBytes, byte[] userBytes, byte[] ownerEncryptionBytes, byte[] userEncryptionBytes, UserAccessPermissions userAccessPermissions, DictionaryToken dictionary, bool encryptMetadata)
		{
			this.Filter = filter;
			this.EncryptionAlgorithmCode = encryptionAlgorithmCode;
			this.KeyLength = keyLength;
			this.Revision = revision;
			this.OwnerBytes = ownerBytes;
			this.UserBytes = userBytes;
			this.OwnerEncryptionBytes = ownerEncryptionBytes;
			this.UserEncryptionBytes = userEncryptionBytes;
			this.UserAccessPermissions = userAccessPermissions;
			this.Dictionary = dictionary;
			this.EncryptMetadata = encryptMetadata;
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x00037778 File Offset: 0x00035978
		public bool TryGetCryptHandler(out CryptHandler cryptHandler)
		{
			cryptHandler = null;
			if (this.EncryptionAlgorithmCode != EncryptionAlgorithmCode.SecurityHandlerInDocument && this.EncryptionAlgorithmCode != EncryptionAlgorithmCode.SecurityHandlerInDocument256)
			{
				return false;
			}
			DictionaryToken dictionaryToken;
			if (!this.Dictionary.TryGet<DictionaryToken>(NameToken.Cf, ref dictionaryToken))
			{
				return false;
			}
			DictionaryToken dictionaryToken2 = dictionaryToken;
			NameToken nameToken2;
			NameToken nameToken = this.Dictionary.TryGet<NameToken>(NameToken.StmF, ref nameToken2) ? nameToken2 : NameToken.Identity;
			NameToken nameToken4;
			NameToken nameToken3 = this.Dictionary.TryGet<NameToken>(NameToken.StrF, ref nameToken4) ? nameToken4 : NameToken.Identity;
			IToken token;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (nameToken != NameToken.Identity && !dictionaryToken2.TryGet(nameToken, ref token))
			{
				defaultInterpolatedStringHandler..ctor(47, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Stream filter ");
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(nameToken);
				defaultInterpolatedStringHandler.AppendLiteral(" not found in crypt dictionary: ");
				defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionaryToken);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentEncryptedException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (nameToken3 != NameToken.Identity && !dictionaryToken2.TryGet(nameToken3, ref token))
			{
				defaultInterpolatedStringHandler..ctor(47, 2);
				defaultInterpolatedStringHandler.AppendLiteral("String filter ");
				defaultInterpolatedStringHandler.AppendFormatted<NameToken>(nameToken3);
				defaultInterpolatedStringHandler.AppendLiteral(" not found in crypt dictionary: ");
				defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionaryToken);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentEncryptedException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			cryptHandler = new CryptHandler(dictionaryToken2, nameToken, nameToken3);
			return true;
		}
	}
}
