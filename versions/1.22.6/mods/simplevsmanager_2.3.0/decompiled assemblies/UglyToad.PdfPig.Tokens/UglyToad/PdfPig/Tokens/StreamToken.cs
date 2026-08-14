using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Tokens
{
	// Token: 0x02000011 RID: 17
	public class StreamToken : IDataToken<IReadOnlyList<byte>>, IToken, IEquatable<IToken>
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000061 RID: 97 RVA: 0x000054CC File Offset: 0x000036CC
		public DictionaryToken StreamDictionary { get; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000062 RID: 98 RVA: 0x000054D4 File Offset: 0x000036D4
		public IReadOnlyList<byte> Data { get; }

		// Token: 0x06000063 RID: 99 RVA: 0x000054DC File Offset: 0x000036DC
		public StreamToken(DictionaryToken streamDictionary, IReadOnlyList<byte> data)
		{
			if (streamDictionary == null)
			{
				throw new ArgumentNullException("streamDictionary");
			}
			this.StreamDictionary = streamDictionary;
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			this.Data = data;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00005510 File Offset: 0x00003710
		public bool Equals(IToken obj)
		{
			if (this == obj)
			{
				return true;
			}
			StreamToken streamToken = obj as StreamToken;
			if (streamToken == null)
			{
				return false;
			}
			if (!this.StreamDictionary.Equals(streamToken.StreamDictionary))
			{
				return false;
			}
			if (this.Data.Count != streamToken.Data.Count)
			{
				return false;
			}
			for (int i = 0; i < this.Data.Count; i++)
			{
				if (this.Data[i] != streamToken.Data[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00005594 File Offset: 0x00003794
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(22, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Length: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Data.Count);
			defaultInterpolatedStringHandler.AppendLiteral(", Dictionary: ");
			defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(this.StreamDictionary);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
