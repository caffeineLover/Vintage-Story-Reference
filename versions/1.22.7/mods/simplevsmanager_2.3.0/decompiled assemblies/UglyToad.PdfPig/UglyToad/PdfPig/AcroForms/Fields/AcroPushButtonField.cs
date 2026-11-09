using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.AcroForms.Fields
{
	// Token: 0x0200021A RID: 538
	public class AcroPushButtonField : AcroFieldBase
	{
		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000CAC RID: 3244 RVA: 0x0003F63F File Offset: 0x0003D83F
		public AcroButtonFieldFlags Flags { get; }

		// Token: 0x06000CAD RID: 3245 RVA: 0x0003F647 File Offset: 0x0003D847
		public AcroPushButtonField(DictionaryToken dictionary, string fieldType, AcroButtonFieldFlags fieldFlags, AcroFieldCommonInformation information, int? pageNumber, PdfRectangle? bounds) : base(dictionary, fieldType, (uint)fieldFlags, AcroFieldType.PushButton, information, pageNumber, bounds)
		{
			this.Flags = fieldFlags;
		}
	}
}
