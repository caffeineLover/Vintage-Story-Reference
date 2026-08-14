using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.PdfFonts.Cmap;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.PdfFonts.Composite
{
	// Token: 0x020000AD RID: 173
	internal class ToUnicodeCMap
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x000144B8 File Offset: 0x000126B8
		public bool CanMapToUnicode
		{
			get
			{
				return this.cMap != null;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x000144C3 File Offset: 0x000126C3
		public bool IsUsingIdentityAsUnicodeMap { get; }

		// Token: 0x060002F2 RID: 754 RVA: 0x000144CB File Offset: 0x000126CB
		public ToUnicodeCMap([CanBeNull] CMap cMap)
		{
			this.cMap = cMap;
			if (this.CanMapToUnicode)
			{
				string name = cMap.Name;
				this.IsUsingIdentityAsUnicodeMap = (name != null && name.StartsWith("Identity-", 3));
			}
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00014500 File Offset: 0x00012700
		public bool TryGet(int code, out string value)
		{
			value = null;
			return this.CanMapToUnicode && this.cMap.TryConvertToUnicode(code, out value);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0001451C File Offset: 0x0001271C
		public int ReadCode(IInputBytes inputBytes)
		{
			return this.cMap.ReadCode(inputBytes);
		}

		// Token: 0x04000193 RID: 403
		[CanBeNull]
		private readonly CMap cMap;
	}
}
