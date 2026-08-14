using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.AcroForms.Fields
{
	// Token: 0x02000218 RID: 536
	public class AcroListBoxField : AcroFieldBase
	{
		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06000CA3 RID: 3235 RVA: 0x0003F536 File Offset: 0x0003D736
		public AcroChoiceFieldFlags Flags { get; }

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x0003F53E File Offset: 0x0003D73E
		[NotNull]
		public IReadOnlyList<AcroChoiceOption> Options { get; }

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x0003F546 File Offset: 0x0003D746
		[NotNull]
		public IReadOnlyList<string> SelectedOptions { get; }

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x0003F54E File Offset: 0x0003D74E
		[CanBeNull]
		public IReadOnlyList<int> SelectedOptionIndices { get; }

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x0003F556 File Offset: 0x0003D756
		public int TopIndex { get; }

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000CA8 RID: 3240 RVA: 0x0003F560 File Offset: 0x0003D760
		public bool SupportsMultiSelect
		{
			get
			{
				return this.Flags.Equals(AcroChoiceFieldFlags.MultiSelect);
			}
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x0003F58C File Offset: 0x0003D78C
		public AcroListBoxField(DictionaryToken dictionary, string fieldType, AcroChoiceFieldFlags fieldFlags, AcroFieldCommonInformation information, IReadOnlyList<AcroChoiceOption> options, IReadOnlyList<string> selectedOptions, IReadOnlyList<int> selectedOptionIndices, int? topIndex, int? pageNumber, PdfRectangle? bounds) : base(dictionary, fieldType, (uint)fieldFlags, AcroFieldType.ListBox, information, pageNumber, bounds)
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
			this.TopIndex = topIndex.GetValueOrDefault();
		}
	}
}
