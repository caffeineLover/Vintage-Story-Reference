using System;
using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig.AcroForms.Fields;

namespace UglyToad.PdfPig.AcroForms
{
	// Token: 0x0200020C RID: 524
	public static class AcroFormExtensions
	{
		// Token: 0x06000C75 RID: 3189 RVA: 0x0003E0F0 File Offset: 0x0003C2F0
		public static IEnumerable<AcroFieldBase> GetFields(this AcroForm form)
		{
			return Enumerable.SelectMany<AcroFieldBase, AcroFieldBase>(form.Fields, (AcroFieldBase f) => f.GetFields());
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x0003E11C File Offset: 0x0003C31C
		public static IEnumerable<AcroFieldBase> GetFields(this AcroFieldBase fieldBase)
		{
			if (fieldBase.FieldType != AcroFieldType.Unknown)
			{
				yield return fieldBase;
			}
			AcroNonTerminalField acroNonTerminalField = fieldBase as AcroNonTerminalField;
			if (acroNonTerminalField != null)
			{
				foreach (AcroFieldBase fieldBase2 in acroNonTerminalField.Children)
				{
					foreach (AcroFieldBase acroFieldBase in fieldBase2.GetFields())
					{
						yield return acroFieldBase;
					}
					IEnumerator<AcroFieldBase> enumerator2 = null;
				}
				IEnumerator<AcroFieldBase> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x0003E12C File Offset: 0x0003C32C
		public static KeyValuePair<string, string> GetFieldValue(this AcroFieldBase fieldBase)
		{
			AcroTextField acroTextField = fieldBase as AcroTextField;
			KeyValuePair<string, string> result;
			if (acroTextField == null)
			{
				AcroCheckboxField acroCheckboxField = fieldBase as AcroCheckboxField;
				if (acroCheckboxField == null)
				{
					result = new KeyValuePair<string, string>(fieldBase.Information.PartialName, "");
				}
				else
				{
					result = new KeyValuePair<string, string>(acroCheckboxField.Information.PartialName, acroCheckboxField.IsChecked.ToString());
				}
			}
			else
			{
				result = new KeyValuePair<string, string>(acroTextField.Information.PartialName, acroTextField.Value);
			}
			return result;
		}
	}
}
