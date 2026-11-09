using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000F3 RID: 243
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class SourceCodePointer : ContainerElement
	{
		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060007E8 RID: 2024 RVA: 0x00013324 File Offset: 0x00011524
		// (set) Token: 0x060007E9 RID: 2025 RVA: 0x0001332C File Offset: 0x0001152C
		public string MethodName { get; set; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060007EA RID: 2026 RVA: 0x00013335 File Offset: 0x00011535
		// (set) Token: 0x060007EB RID: 2027 RVA: 0x0001333D File Offset: 0x0001153D
		public string CalledFrom { get; set; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x00013346 File Offset: 0x00011546
		// (set) Token: 0x060007ED RID: 2029 RVA: 0x0001334E File Offset: 0x0001154E
		public string FilePath { get; set; }

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x00013357 File Offset: 0x00011557
		// (set) Token: 0x060007EF RID: 2031 RVA: 0x0001335F File Offset: 0x0001155F
		public int LineNumber { get; set; }

		// Token: 0x060007F0 RID: 2032 RVA: 0x00013368 File Offset: 0x00011568
		[NullableContext(2)]
		internal override string GetCompanionSearchableContent()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 3);
			defaultInterpolatedStringHandler.AppendFormatted(this.MethodName);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(this.CalledFrom);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(this.FilePath);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x000133C4 File Offset: 0x000115C4
		[return: Nullable(new byte[]
		{
			2,
			0,
			1,
			1
		})]
		internal override IEnumerable<KeyValuePair<string, string>> GetCompanionProperties()
		{
			SourceCodePointer.<GetCompanionProperties>d__17 <GetCompanionProperties>d__ = new SourceCodePointer.<GetCompanionProperties>d__17(-2);
			<GetCompanionProperties>d__.<>4__this = this;
			return <GetCompanionProperties>d__;
		}
	}
}
