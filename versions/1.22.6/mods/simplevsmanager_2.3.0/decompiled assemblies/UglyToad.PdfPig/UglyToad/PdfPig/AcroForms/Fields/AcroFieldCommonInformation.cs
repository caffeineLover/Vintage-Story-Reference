using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.AcroForms.Fields
{
	// Token: 0x02000216 RID: 534
	public class AcroFieldCommonInformation
	{
		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x0003F3FE File Offset: 0x0003D5FE
		// (set) Token: 0x06000C9C RID: 3228 RVA: 0x0003F406 File Offset: 0x0003D606
		public IndirectReference? Parent { get; set; }

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000C9D RID: 3229 RVA: 0x0003F40F File Offset: 0x0003D60F
		[CanBeNull]
		public string PartialName { get; }

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000C9E RID: 3230 RVA: 0x0003F417 File Offset: 0x0003D617
		[CanBeNull]
		public string AlternateName { get; }

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000C9F RID: 3231 RVA: 0x0003F41F File Offset: 0x0003D61F
		[CanBeNull]
		public string MappingName { get; }

		// Token: 0x06000CA0 RID: 3232 RVA: 0x0003F427 File Offset: 0x0003D627
		public AcroFieldCommonInformation(IndirectReference? parent, string partialName, string alternateName, string mappingName)
		{
			this.Parent = parent;
			this.PartialName = partialName;
			this.AlternateName = alternateName;
			this.MappingName = mappingName;
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x0003F44C File Offset: 0x0003D64C
		public override string ToString()
		{
			string text = string.Empty;
			if (this.Parent != null)
			{
				string text2 = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(9, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Parent: ");
				defaultInterpolatedStringHandler.AppendFormatted<IndirectReference?>(this.Parent);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				text = text2 + defaultInterpolatedStringHandler.ToStringAndClear();
			}
			text = AcroFieldCommonInformation.<ToString>g__AppendIfNotNull|14_0(this.PartialName, "Partial Name", text);
			text = AcroFieldCommonInformation.<ToString>g__AppendIfNotNull|14_0(this.AlternateName, "Alternate Name", text);
			return AcroFieldCommonInformation.<ToString>g__AppendIfNotNull|14_0(this.MappingName, "Mapping Name", text);
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x0003F4E4 File Offset: 0x0003D6E4
		[CompilerGenerated]
		internal static string <ToString>g__AppendIfNotNull|14_0(string val, string label, string result)
		{
			if (val == null)
			{
				return result;
			}
			if (result.Length > 0)
			{
				result += " ";
			}
			result = string.Concat(new string[]
			{
				result,
				label,
				": ",
				val,
				"."
			});
			return result;
		}
	}
}
