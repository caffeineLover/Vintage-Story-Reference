using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.AcroForms.Fields
{
	// Token: 0x0200021B RID: 539
	public class AcroRadioButtonField : AcroFieldBase
	{
		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000CAE RID: 3246 RVA: 0x0003F660 File Offset: 0x0003D860
		public AcroButtonFieldFlags Flags { get; }

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000CAF RID: 3247 RVA: 0x0003F668 File Offset: 0x0003D868
		public NameToken CurrentValue { get; }

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x0003F670 File Offset: 0x0003D870
		public bool IsSelected { get; }

		// Token: 0x06000CB1 RID: 3249 RVA: 0x0003F678 File Offset: 0x0003D878
		public AcroRadioButtonField(DictionaryToken dictionary, string fieldType, AcroButtonFieldFlags fieldFlags, AcroFieldCommonInformation information, int? pageNumber, PdfRectangle? bounds, NameToken currentValue, bool isSelected) : base(dictionary, fieldType, (uint)fieldFlags, AcroFieldType.RadioButton, information, pageNumber, bounds)
		{
			this.Flags = fieldFlags;
			this.CurrentValue = currentValue;
			this.IsSelected = isSelected;
		}
	}
}
