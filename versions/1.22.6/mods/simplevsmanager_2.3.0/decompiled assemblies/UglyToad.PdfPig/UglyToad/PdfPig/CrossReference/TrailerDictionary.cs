using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.CrossReference
{
	// Token: 0x020001DC RID: 476
	public class TrailerDictionary
	{
		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000B40 RID: 2880 RVA: 0x000394B7 File Offset: 0x000376B7
		public int Size { get; }

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000B41 RID: 2881 RVA: 0x000394BF File Offset: 0x000376BF
		public long? PreviousCrossReferenceOffset { get; }

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000B42 RID: 2882 RVA: 0x000394C7 File Offset: 0x000376C7
		public IndirectReference Root { get; }

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000B43 RID: 2883 RVA: 0x000394CF File Offset: 0x000376CF
		public IndirectReference? Info { get; }

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000B44 RID: 2884 RVA: 0x000394D7 File Offset: 0x000376D7
		public IReadOnlyList<IDataToken<string>> Identifier { get; }

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000B45 RID: 2885 RVA: 0x000394DF File Offset: 0x000376DF
		[CanBeNull]
		public IToken EncryptionToken { get; }

		// Token: 0x06000B46 RID: 2886 RVA: 0x000394E8 File Offset: 0x000376E8
		internal TrailerDictionary(DictionaryToken dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			this.Size = dictionary.GetInt(NameToken.Size);
			this.PreviousCrossReferenceOffset = dictionary.GetLongOrDefault(NameToken.Prev);
			IndirectReferenceToken indirectReferenceToken;
			if (!dictionary.TryGet<IndirectReferenceToken>(NameToken.Root, ref indirectReferenceToken))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(52, 1);
				defaultInterpolatedStringHandler.AppendLiteral("No root token was found in the trailer dictionary: ");
				defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionary);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.Root = indirectReferenceToken.Data;
			IndirectReferenceToken indirectReferenceToken2;
			if (dictionary.TryGet<IndirectReferenceToken>(NameToken.Info, ref indirectReferenceToken2))
			{
				this.Info = new IndirectReference?(indirectReferenceToken2.Data);
			}
			ArrayToken arrayToken;
			if (dictionary.TryGet<ArrayToken>(NameToken.Id, ref arrayToken))
			{
				List<IDataToken<string>> list = new List<IDataToken<string>>(arrayToken.Data.Count);
				foreach (IToken token in arrayToken.Data)
				{
					StringToken stringToken = token as StringToken;
					if (stringToken != null)
					{
						list.Add(stringToken);
					}
					else
					{
						HexToken hexToken = token as HexToken;
						if (hexToken != null)
						{
							list.Add(hexToken);
						}
					}
				}
				this.Identifier = list;
			}
			else
			{
				this.Identifier = EmptyArray<IDataToken<string>>.Instance;
			}
			IToken token2;
			if (dictionary.TryGet(NameToken.Encrypt, ref token2))
			{
				this.EncryptionToken = token2;
			}
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00039658 File Offset: 0x00037858
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(14, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Size: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Size);
			defaultInterpolatedStringHandler.AppendLiteral(", Root: ");
			defaultInterpolatedStringHandler.AppendFormatted<IndirectReference>(this.Root);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
