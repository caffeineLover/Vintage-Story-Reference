using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.AcroForms.Fields
{
	// Token: 0x02000211 RID: 529
	public class AcroCheckboxField : AcroFieldBase
	{
		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000C82 RID: 3202 RVA: 0x0003F1A3 File Offset: 0x0003D3A3
		public AcroButtonFieldFlags Flags { get; }

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000C83 RID: 3203 RVA: 0x0003F1AB File Offset: 0x0003D3AB
		public NameToken CurrentValue { get; }

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000C84 RID: 3204 RVA: 0x0003F1B3 File Offset: 0x0003D3B3
		public bool IsChecked { get; }

		// Token: 0x06000C85 RID: 3205 RVA: 0x0003F1BB File Offset: 0x0003D3BB
		public AcroCheckboxField(DictionaryToken dictionary, string fieldType, AcroButtonFieldFlags fieldFlags, AcroFieldCommonInformation information, NameToken currentValue, bool isChecked, int? pageNumber, PdfRectangle? bounds) : base(dictionary, fieldType, (uint)fieldFlags, AcroFieldType.Checkbox, information, pageNumber, bounds)
		{
			this.Flags = fieldFlags;
			this.CurrentValue = currentValue;
			this.IsChecked = isChecked;
		}
	}
}
