using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.AcroForms.Fields
{
	// Token: 0x0200021D RID: 541
	public class AcroSignatureField : AcroFieldBase
	{
		// Token: 0x06000CB4 RID: 3252 RVA: 0x0003F6C0 File Offset: 0x0003D8C0
		public AcroSignatureField(DictionaryToken dictionary, string fieldType, uint fieldFlags, AcroFieldCommonInformation information, int? pageNumber, PdfRectangle? bounds) : base(dictionary, fieldType, fieldFlags, AcroFieldType.Signature, information, pageNumber, bounds)
		{
		}
	}
}
