using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Writer.Fonts
{
	// Token: 0x0200001F RID: 31
	internal interface IWritingFont
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000F2 RID: 242
		bool HasWidths { get; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000F3 RID: 243
		string Name { get; }

		// Token: 0x060000F4 RID: 244
		bool TryGetBoundingBox(char character, out PdfRectangle boundingBox);

		// Token: 0x060000F5 RID: 245
		bool TryGetAdvanceWidth(char character, out double width);

		// Token: 0x060000F6 RID: 246
		TransformationMatrix GetFontMatrix();

		// Token: 0x060000F7 RID: 247
		IndirectReferenceToken WriteFont(IPdfStreamWriter writer, [CanBeNull] IndirectReferenceToken reservedIndirect = null);

		// Token: 0x060000F8 RID: 248
		byte GetValueForCharacter(char character);
	}
}
