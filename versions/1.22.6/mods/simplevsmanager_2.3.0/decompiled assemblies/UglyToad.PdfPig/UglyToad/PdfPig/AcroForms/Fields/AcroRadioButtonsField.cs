using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.AcroForms.Fields
{
	// Token: 0x0200021C RID: 540
	public class AcroRadioButtonsField : AcroNonTerminalField
	{
		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x06000CB2 RID: 3250 RVA: 0x0003F6A1 File Offset: 0x0003D8A1
		public AcroButtonFieldFlags Flags { get; }

		// Token: 0x06000CB3 RID: 3251 RVA: 0x0003F6A9 File Offset: 0x0003D8A9
		public AcroRadioButtonsField(DictionaryToken dictionary, string fieldType, AcroButtonFieldFlags fieldFlags, AcroFieldCommonInformation information, IReadOnlyList<AcroFieldBase> children) : base(dictionary, fieldType, (uint)fieldFlags, information, AcroFieldType.RadioButtons, children)
		{
			this.Flags = fieldFlags;
		}
	}
}
