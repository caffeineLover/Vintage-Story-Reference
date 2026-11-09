using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Logging;
using UglyToad.PdfPig.PdfFonts.Parser.Handlers;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.PdfFonts
{
	// Token: 0x02000090 RID: 144
	internal class FontFactory : IFontFactory
	{
		// Token: 0x0600025E RID: 606 RVA: 0x00010060 File Offset: 0x0000E260
		public FontFactory(ILog log, Type0FontHandler type0FontHandler, TrueTypeFontHandler trueTypeFontHandler, Type1FontHandler type1FontHandler, Type3FontHandler type3FontHandler)
		{
			this.log = log;
			Dictionary<NameToken, IFontHandler> dictionary = new Dictionary<NameToken, IFontHandler>();
			dictionary.Add(NameToken.Type0, type0FontHandler);
			dictionary.Add(NameToken.TrueType, trueTypeFontHandler);
			dictionary.Add(NameToken.Type1, type1FontHandler);
			dictionary.Add(NameToken.MmType1, type1FontHandler);
			dictionary.Add(NameToken.Type3, type3FontHandler);
			this.handlers = dictionary;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x000100C4 File Offset: 0x0000E2C4
		public IFont Get(DictionaryToken dictionary)
		{
			NameToken nameOrDefault = dictionary.GetNameOrDefault(NameToken.Type);
			if (nameOrDefault != null && !nameOrDefault.Equals(NameToken.Font))
			{
				string message = "The font dictionary did not have type 'Font'. " + ((dictionary != null) ? dictionary.ToString() : null);
				ILog log = this.log;
				if (log != null)
				{
					log.Error(message);
				}
			}
			NameToken nameOrDefault2 = dictionary.GetNameOrDefault(NameToken.Subtype);
			IFontHandler fontHandler;
			if (this.handlers.TryGetValue(nameOrDefault2, ref fontHandler))
			{
				return fontHandler.Generate(dictionary);
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(86, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Parsing not implemented for fonts of type: ");
			defaultInterpolatedStringHandler.AppendFormatted<NameToken>(nameOrDefault2);
			defaultInterpolatedStringHandler.AppendLiteral(", please submit a pull request or an issue.");
			throw new NotImplementedException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0400013C RID: 316
		private readonly ILog log;

		// Token: 0x0400013D RID: 317
		private readonly IReadOnlyDictionary<NameToken, IFontHandler> handlers;
	}
}
