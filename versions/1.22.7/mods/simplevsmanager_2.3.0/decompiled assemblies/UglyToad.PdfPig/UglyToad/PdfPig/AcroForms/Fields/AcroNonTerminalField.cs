using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.AcroForms.Fields
{
	// Token: 0x02000219 RID: 537
	public class AcroNonTerminalField : AcroFieldBase
	{
		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x06000CAA RID: 3242 RVA: 0x0003F5F3 File Offset: 0x0003D7F3
		public IReadOnlyList<AcroFieldBase> Children { get; }

		// Token: 0x06000CAB RID: 3243 RVA: 0x0003F5FC File Offset: 0x0003D7FC
		internal AcroNonTerminalField(DictionaryToken dictionary, string fieldType, uint fieldFlags, AcroFieldCommonInformation information, AcroFieldType acroFieldType, IReadOnlyList<AcroFieldBase> children) : base(dictionary, fieldType, fieldFlags, acroFieldType, information, default(int?), default(PdfRectangle?))
		{
			if (children == null)
			{
				throw new ArgumentNullException("children");
			}
			this.Children = children;
		}
	}
}
