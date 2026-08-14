using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Graphics.Colors;
using UglyToad.PdfPig.PdfFonts;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001E7 RID: 487
	internal interface IResourceStore
	{
		// Token: 0x06000B9D RID: 2973
		void LoadResourceDictionary(DictionaryToken resourceDictionary, InternalParsingOptions parsingOptions);

		// Token: 0x06000B9E RID: 2974
		void UnloadResourceDictionary();

		// Token: 0x06000B9F RID: 2975
		IFont GetFont(NameToken name);

		// Token: 0x06000BA0 RID: 2976
		bool TryGetXObject(NameToken name, out StreamToken stream);

		// Token: 0x06000BA1 RID: 2977
		DictionaryToken GetExtendedGraphicsStateDictionary(NameToken name);

		// Token: 0x06000BA2 RID: 2978
		IFont GetFontDirectly(IndirectReferenceToken fontReferenceToken);

		// Token: 0x06000BA3 RID: 2979
		bool TryGetNamedColorSpace(NameToken name, out ResourceColorSpace namedColorSpace);

		// Token: 0x06000BA4 RID: 2980
		ColorSpaceDetails GetColorSpaceDetails(NameToken name, DictionaryToken dictionary);

		// Token: 0x06000BA5 RID: 2981
		DictionaryToken GetMarkedContentPropertiesDictionary(NameToken name);

		// Token: 0x06000BA6 RID: 2982
		IReadOnlyDictionary<NameToken, PatternColor> GetPatterns();

		// Token: 0x06000BA7 RID: 2983
		Shading GetShading(NameToken name);
	}
}
