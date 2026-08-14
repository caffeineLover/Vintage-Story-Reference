using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Fonts.TrueType.Names
{
	// Token: 0x02000060 RID: 96
	public class TrueTypeNameRecord
	{
		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000CD2F File Offset: 0x0000AF2F
		public TrueTypePlatformIdentifier PlatformId { get; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000276 RID: 630 RVA: 0x0000CD37 File Offset: 0x0000AF37
		public ushort PlatformEncodingId { get; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000CD3F File Offset: 0x0000AF3F
		public ushort LanguageId { get; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000278 RID: 632 RVA: 0x0000CD47 File Offset: 0x0000AF47
		public ushort NameId { get; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000CD4F File Offset: 0x0000AF4F
		public string Value { get; }

		// Token: 0x0600027A RID: 634 RVA: 0x0000CD57 File Offset: 0x0000AF57
		public TrueTypeNameRecord(TrueTypePlatformIdentifier platformId, ushort platformEncodingId, ushort languageId, ushort nameId, string value)
		{
			this.PlatformId = platformId;
			this.PlatformEncodingId = platformEncodingId;
			this.LanguageId = languageId;
			this.NameId = nameId;
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.Value = value;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000CD94 File Offset: 0x0000AF94
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(21, 3);
			defaultInterpolatedStringHandler.AppendLiteral("(Platform: ");
			defaultInterpolatedStringHandler.AppendFormatted<TrueTypePlatformIdentifier>(this.PlatformId);
			defaultInterpolatedStringHandler.AppendLiteral(", Id: ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(this.NameId);
			defaultInterpolatedStringHandler.AppendLiteral(") - ");
			defaultInterpolatedStringHandler.AppendFormatted(this.Value);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
