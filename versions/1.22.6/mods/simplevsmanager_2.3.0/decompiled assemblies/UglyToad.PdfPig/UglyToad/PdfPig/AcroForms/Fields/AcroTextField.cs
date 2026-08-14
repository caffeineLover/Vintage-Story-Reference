using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.AcroForms.Fields
{
	// Token: 0x0200021E RID: 542
	public class AcroTextField : AcroFieldBase
	{
		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x0003F6D2 File Offset: 0x0003D8D2
		public AcroTextFieldFlags Flags { get; }

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x0003F6DA File Offset: 0x0003D8DA
		public string Value { get; }

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x0003F6E2 File Offset: 0x0003D8E2
		public int? MaxLength { get; }

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x0003F6EA File Offset: 0x0003D8EA
		public bool IsRichText { get; }

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x0003F6F2 File Offset: 0x0003D8F2
		public bool IsMultiline { get; }

		// Token: 0x06000CBA RID: 3258 RVA: 0x0003F6FC File Offset: 0x0003D8FC
		public AcroTextField(DictionaryToken dictionary, string fieldType, AcroTextFieldFlags fieldFlags, AcroFieldCommonInformation information, string value, int? maxLength, int? pageNumber, PdfRectangle? bounds) : base(dictionary, fieldType, (uint)fieldFlags, AcroFieldType.Text, information, pageNumber, bounds)
		{
			this.Flags = fieldFlags;
			this.Value = value;
			this.MaxLength = maxLength;
			this.IsRichText = this.Flags.HasFlag(AcroTextFieldFlags.RichText);
			this.IsMultiline = this.Flags.HasFlag(AcroTextFieldFlags.Multiline);
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0003F770 File Offset: 0x0003D970
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<AcroFieldType>(base.FieldType);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted(this.Value ?? string.Empty);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
