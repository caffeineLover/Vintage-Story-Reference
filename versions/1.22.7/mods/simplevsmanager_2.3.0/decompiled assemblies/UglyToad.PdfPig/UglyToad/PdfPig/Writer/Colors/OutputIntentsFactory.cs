using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Writer.Colors
{
	// Token: 0x02000023 RID: 35
	internal static class OutputIntentsFactory
	{
		// Token: 0x06000110 RID: 272 RVA: 0x0000A40C File Offset: 0x0000860C
		public static ArrayToken GetOutputIntentsArray(Func<IToken, IndirectReferenceToken> objectWriter)
		{
			StringToken stringToken = new StringToken("sRGB IEC61966-2.1", 0);
			byte[] array = DataCompresser.CompressBytes(ProfileStreamReader.GetSRgb2014());
			Dictionary<NameToken, IToken> dictionary = new Dictionary<NameToken, IToken>();
			dictionary.Add(NameToken.Length, new NumericToken(array.Length));
			dictionary.Add(NameToken.N, new NumericToken(3m));
			dictionary.Add(NameToken.Filter, NameToken.FlateDecode);
			StreamToken streamToken = new StreamToken(new DictionaryToken(dictionary), array);
			IndirectReferenceToken indirectReferenceToken = objectWriter.Invoke(streamToken);
			IToken[] array2 = new IToken[1];
			int num = 0;
			Dictionary<NameToken, IToken> dictionary2 = new Dictionary<NameToken, IToken>();
			dictionary2.Add(NameToken.Type, NameToken.OutputIntent);
			dictionary2.Add(NameToken.S, NameToken.GtsPdfa1);
			dictionary2.Add(NameToken.OutputCondition, stringToken);
			dictionary2.Add(NameToken.OutputConditionIdentifier, stringToken);
			dictionary2.Add(NameToken.RegistryName, new StringToken("http://www.color.org", 0));
			dictionary2.Add(NameToken.Info, stringToken);
			dictionary2.Add(NameToken.DestOutputProfile, indirectReferenceToken);
			array2[num] = new DictionaryToken(dictionary2);
			return new ArrayToken(array2);
		}

		// Token: 0x040000A5 RID: 165
		private const string SrgbIec61966OutputCondition = "sRGB IEC61966-2.1";

		// Token: 0x040000A6 RID: 166
		private const string RegistryName = "http://www.color.org";
	}
}
