using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace QuestPDF.Infrastructure
{
	// Token: 0x0200003C RID: 60
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class DocumentMetadata
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00004CCD File Offset: 0x00002ECD
		// (set) Token: 0x0600016D RID: 365 RVA: 0x00004CD5 File Offset: 0x00002ED5
		public string Title { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00004CDE File Offset: 0x00002EDE
		// (set) Token: 0x0600016F RID: 367 RVA: 0x00004CE6 File Offset: 0x00002EE6
		public string Author { get; set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00004CEF File Offset: 0x00002EEF
		// (set) Token: 0x06000171 RID: 369 RVA: 0x00004CF7 File Offset: 0x00002EF7
		public string Subject { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00004D00 File Offset: 0x00002F00
		// (set) Token: 0x06000173 RID: 371 RVA: 0x00004D08 File Offset: 0x00002F08
		public string Keywords { get; set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00004D11 File Offset: 0x00002F11
		// (set) Token: 0x06000175 RID: 373 RVA: 0x00004D19 File Offset: 0x00002F19
		public string Creator { get; set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00004D22 File Offset: 0x00002F22
		// (set) Token: 0x06000177 RID: 375 RVA: 0x00004D2A File Offset: 0x00002F2A
		public string Producer { get; set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00004D33 File Offset: 0x00002F33
		// (set) Token: 0x06000179 RID: 377 RVA: 0x00004D3B File Offset: 0x00002F3B
		public string Language { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00004D44 File Offset: 0x00002F44
		// (set) Token: 0x0600017B RID: 379 RVA: 0x00004D4C File Offset: 0x00002F4C
		public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.Now;

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00004D55 File Offset: 0x00002F55
		// (set) Token: 0x0600017D RID: 381 RVA: 0x00004D5D File Offset: 0x00002F5D
		public DateTimeOffset ModifiedDate { get; set; } = DateTimeOffset.Now;

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00004D66 File Offset: 0x00002F66
		[Nullable(1)]
		public static DocumentMetadata Default
		{
			[NullableContext(1)]
			get
			{
				return new DocumentMetadata();
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00004D6D File Offset: 0x00002F6D
		// (set) Token: 0x06000180 RID: 384 RVA: 0x00004D74 File Offset: 0x00002F74
		[Obsolete("This API has been moved since version 2022.9. Please use the QuestPDF.Settings.DocumentLayoutExceptionThreshold static property.")]
		[ExcludeFromCodeCoverage]
		public int DocumentLayoutExceptionThreshold
		{
			get
			{
				return Settings.DocumentLayoutExceptionThreshold;
			}
			set
			{
				Settings.DocumentLayoutExceptionThreshold = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00004D7C File Offset: 0x00002F7C
		// (set) Token: 0x06000182 RID: 386 RVA: 0x00004D83 File Offset: 0x00002F83
		[Obsolete("This API has been moved since version 2022.9. Please use the QuestPDF.Settings.EnableCaching static property.")]
		[ExcludeFromCodeCoverage]
		public bool ApplyCaching
		{
			get
			{
				return Settings.EnableCaching;
			}
			set
			{
				Settings.EnableCaching = value;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00004D8B File Offset: 0x00002F8B
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00004D92 File Offset: 0x00002F92
		[Obsolete("This API has been moved since version 2022.9. Please use the QuestPDF.Settings.EnableDebugging static property.")]
		[ExcludeFromCodeCoverage]
		public bool ApplyDebugging
		{
			get
			{
				return Settings.EnableDebugging;
			}
			set
			{
				Settings.EnableDebugging = value;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00004D9A File Offset: 0x00002F9A
		// (set) Token: 0x06000186 RID: 390 RVA: 0x00004DA2 File Offset: 0x00002FA2
		[Obsolete("This API has been moved since version 2023.5. Please use the QuestPDF.Infrastructure.DocumentSettings API.")]
		[ExcludeFromCodeCoverage]
		public int? ImageQuality { get; set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00004DAB File Offset: 0x00002FAB
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00004DB3 File Offset: 0x00002FB3
		[Obsolete("This API has been moved since version 2023.5. Please use the QuestPDF.Infrastructure.DocumentSettings API.")]
		[ExcludeFromCodeCoverage]
		public int? RasterDpi { get; set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00004DBC File Offset: 0x00002FBC
		// (set) Token: 0x0600018A RID: 394 RVA: 0x00004DC4 File Offset: 0x00002FC4
		[Obsolete("This API has been moved since version 2023.5. Please use the QuestPDF.Infrastructure.DocumentSettings API.")]
		[ExcludeFromCodeCoverage]
		public bool? PdfA { get; set; }
	}
}
