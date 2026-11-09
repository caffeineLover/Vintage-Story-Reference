using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.AcroForms.Fields
{
	// Token: 0x02000213 RID: 531
	public class AcroChoiceOption
	{
		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000C86 RID: 3206 RVA: 0x0003F1E4 File Offset: 0x0003D3E4
		public int Index { get; }

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000C87 RID: 3207 RVA: 0x0003F1EC File Offset: 0x0003D3EC
		public bool IsSelected { get; }

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000C88 RID: 3208 RVA: 0x0003F1F4 File Offset: 0x0003D3F4
		public string Name { get; }

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x0003F1FC File Offset: 0x0003D3FC
		public string ExportValue { get; }

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000C8A RID: 3210 RVA: 0x0003F204 File Offset: 0x0003D404
		public bool HasExportValue { get; }

		// Token: 0x06000C8B RID: 3211 RVA: 0x0003F20C File Offset: 0x0003D40C
		public AcroChoiceOption(int index, bool isSelected, string name, string exportValue = null)
		{
			this.Index = index;
			this.IsSelected = isSelected;
			this.Name = name;
			this.ExportValue = exportValue;
			this.HasExportValue = (exportValue != null);
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x0003F23C File Offset: 0x0003D43C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(6, 3);
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Index);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted(this.Name);
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.IsSelected);
			defaultInterpolatedStringHandler.AppendLiteral(").");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
