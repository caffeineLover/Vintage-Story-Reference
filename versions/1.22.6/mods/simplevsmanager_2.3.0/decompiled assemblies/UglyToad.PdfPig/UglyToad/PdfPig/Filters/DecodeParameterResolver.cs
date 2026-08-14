using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.Filters
{
	// Token: 0x020001BF RID: 447
	internal static class DecodeParameterResolver
	{
		// Token: 0x06000AB8 RID: 2744 RVA: 0x0003611C File Offset: 0x0003431C
		public static DictionaryToken GetFilterParameters(DictionaryToken streamDictionary, int index)
		{
			if (streamDictionary == null)
			{
				throw new ArgumentNullException("streamDictionary");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Index must be 0 or greater");
			}
			IToken objectOrDefault = streamDictionary.GetObjectOrDefault(NameToken.Filter, NameToken.F);
			IToken objectOrDefault2 = streamDictionary.GetObjectOrDefault(NameToken.DecodeParms, NameToken.Dp);
			if (!(objectOrDefault is NameToken))
			{
				if (objectOrDefault is ArrayToken)
				{
					ArrayToken arrayToken = objectOrDefault2 as ArrayToken;
					if (arrayToken != null && index < arrayToken.Data.Count)
					{
						DictionaryToken dictionaryToken = arrayToken.Data[index] as DictionaryToken;
						if (dictionaryToken != null)
						{
							return dictionaryToken;
						}
					}
				}
			}
			else
			{
				DictionaryToken dictionaryToken2 = objectOrDefault2 as DictionaryToken;
				if (dictionaryToken2 != null)
				{
					return dictionaryToken2;
				}
			}
			return new DictionaryToken(new Dictionary<NameToken, IToken>());
		}
	}
}
