using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.AcroForms.Fields
{
	// Token: 0x02000214 RID: 532
	public class AcroComboBoxField : AcroFieldBase
	{
		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000C8D RID: 3213 RVA: 0x0003F2A4 File Offset: 0x0003D4A4
		public AcroChoiceFieldFlags Flags { get; }

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x0003F2AC File Offset: 0x0003D4AC
		[NotNull]
		public IReadOnlyList<AcroChoiceOption> Options { get; }

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000C8F RID: 3215 RVA: 0x0003F2B4 File Offset: 0x0003D4B4
		[NotNull]
		public IReadOnlyList<string> SelectedOptions { get; }

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000C90 RID: 3216 RVA: 0x0003F2BC File Offset: 0x0003D4BC
		[CanBeNull]
		public IReadOnlyList<int> SelectedOptionIndices { get; }

		// Token: 0x06000C91 RID: 3217 RVA: 0x0003F2C4 File Offset: 0x0003D4C4
		public AcroComboBoxField(DictionaryToken dictionary, string fieldType, AcroChoiceFieldFlags fieldFlags, AcroFieldCommonInformation information, IReadOnlyList<AcroChoiceOption> options, IReadOnlyList<string> selectedOptions, IReadOnlyList<int> selectedOptionIndices, int? pageNumber, PdfRectangle? bounds) : base(dictionary, fieldType, (uint)fieldFlags, AcroFieldType.ComboBox, information, pageNumber, bounds)
		{
			this.Flags = fieldFlags;
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			this.Options = options;
			if (selectedOptions == null)
			{
				throw new ArgumentNullException("selectedOptions");
			}
			this.SelectedOptions = selectedOptions;
			this.SelectedOptionIndices = selectedOptionIndices;
		}
	}
}
