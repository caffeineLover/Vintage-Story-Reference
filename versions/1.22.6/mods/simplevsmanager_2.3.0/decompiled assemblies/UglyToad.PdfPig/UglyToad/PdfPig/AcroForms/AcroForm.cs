using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.AcroForms.Fields;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.AcroForms
{
	// Token: 0x0200020B RID: 523
	public class AcroForm
	{
		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000C6E RID: 3182 RVA: 0x0003E04A File Offset: 0x0003C24A
		[NotNull]
		public DictionaryToken Dictionary { get; }

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x0003E052 File Offset: 0x0003C252
		public SignatureFlags SignatureFlags { get; }

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x0003E05A File Offset: 0x0003C25A
		public bool NeedAppearances { get; }

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000C71 RID: 3185 RVA: 0x0003E062 File Offset: 0x0003C262
		public IReadOnlyList<AcroFieldBase> Fields { get; }

		// Token: 0x06000C72 RID: 3186 RVA: 0x0003E06C File Offset: 0x0003C26C
		internal AcroForm(DictionaryToken dictionary, SignatureFlags signatureFlags, bool needAppearances, IReadOnlyDictionary<IndirectReference, AcroFieldBase> fieldsWithReferences)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			this.Dictionary = dictionary;
			this.SignatureFlags = signatureFlags;
			this.NeedAppearances = needAppearances;
			if (fieldsWithReferences == null)
			{
				throw new ArgumentNullException("fieldsWithReferences");
			}
			this.fieldsWithReferences = fieldsWithReferences;
			this.Fields = Enumerable.ToList<AcroFieldBase>(fieldsWithReferences.Values);
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x0003E0CC File Offset: 0x0003C2CC
		public IEnumerable<AcroFieldBase> GetFieldsForPage(int pageNumber)
		{
			if (pageNumber <= 0)
			{
				string text = "pageNumber";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Page number starts at 1, instead got ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(pageNumber);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentOutOfRangeException(text, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Func<AcroFieldBase, bool> <>9__0;
			foreach (AcroFieldBase acroFieldBase in this.Fields)
			{
				int? pageNumber2 = acroFieldBase.PageNumber;
				int pageNumber3 = pageNumber;
				if (pageNumber2.GetValueOrDefault() == pageNumber3 & pageNumber2 != null)
				{
					yield return acroFieldBase;
				}
				else
				{
					AcroNonTerminalField acroNonTerminalField = acroFieldBase as AcroNonTerminalField;
					if (acroNonTerminalField != null)
					{
						IEnumerable<AcroFieldBase> children = acroNonTerminalField.Children;
						Func<AcroFieldBase, bool> func;
						if ((func = <>9__0) == null)
						{
							func = (<>9__0 = delegate(AcroFieldBase x)
							{
								int? pageNumber4 = x.PageNumber;
								int pageNumber5 = pageNumber;
								return pageNumber4.GetValueOrDefault() == pageNumber5 & pageNumber4 != null;
							});
						}
						if (Enumerable.Any<AcroFieldBase>(children, func))
						{
							yield return acroFieldBase;
						}
					}
				}
			}
			IEnumerator<AcroFieldBase> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x0003E0E3 File Offset: 0x0003C2E3
		public override string ToString()
		{
			return this.Dictionary.ToString();
		}

		// Token: 0x040006D9 RID: 1753
		private readonly IReadOnlyDictionary<IndirectReference, AcroFieldBase> fieldsWithReferences;
	}
}
