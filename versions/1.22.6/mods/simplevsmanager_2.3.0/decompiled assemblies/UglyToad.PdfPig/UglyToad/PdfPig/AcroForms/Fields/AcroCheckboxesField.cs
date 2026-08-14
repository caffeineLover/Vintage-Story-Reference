using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.AcroForms.Fields
{
	// Token: 0x02000210 RID: 528
	public class AcroCheckboxesField : AcroNonTerminalField
	{
		// Token: 0x06000C81 RID: 3201 RVA: 0x0003F193 File Offset: 0x0003D393
		internal AcroCheckboxesField(DictionaryToken dictionary, string fieldType, AcroButtonFieldFlags fieldFlags, AcroFieldCommonInformation information, IReadOnlyList<AcroFieldBase> children) : base(dictionary, fieldType, (uint)fieldFlags, information, AcroFieldType.Checkboxes, children)
		{
		}
	}
}
