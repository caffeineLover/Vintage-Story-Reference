using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace VintageStoryModManager.Models
{
	// Token: 0x0200009E RID: 158
	[NullableContext(1)]
	[Nullable(0)]
	public class DownloadableModRelease
	{
		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000D8B RID: 3467 RVA: 0x00051061 File Offset: 0x0004F261
		// (set) Token: 0x06000D8C RID: 3468 RVA: 0x00051069 File Offset: 0x0004F269
		[JsonPropertyName("releaseid")]
		public int ReleaseId { get; set; }

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000D8D RID: 3469 RVA: 0x00051072 File Offset: 0x0004F272
		// (set) Token: 0x06000D8E RID: 3470 RVA: 0x0005107A File Offset: 0x0004F27A
		[JsonPropertyName("mainfile")]
		public string MainFile { get; set; } = string.Empty;

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000D8F RID: 3471 RVA: 0x00051083 File Offset: 0x0004F283
		// (set) Token: 0x06000D90 RID: 3472 RVA: 0x0005108B File Offset: 0x0004F28B
		[JsonPropertyName("filename")]
		public string Filename { get; set; } = string.Empty;

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000D91 RID: 3473 RVA: 0x00051094 File Offset: 0x0004F294
		// (set) Token: 0x06000D92 RID: 3474 RVA: 0x0005109C File Offset: 0x0004F29C
		[JsonPropertyName("fileid")]
		[JsonConverter(typeof(FlexibleIntConverter))]
		public int FileId { get; set; }

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000D93 RID: 3475 RVA: 0x000510A5 File Offset: 0x0004F2A5
		// (set) Token: 0x06000D94 RID: 3476 RVA: 0x000510AD File Offset: 0x0004F2AD
		[JsonPropertyName("downloads")]
		public int Downloads { get; set; }

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000D95 RID: 3477 RVA: 0x000510B6 File Offset: 0x0004F2B6
		// (set) Token: 0x06000D96 RID: 3478 RVA: 0x000510BE File Offset: 0x0004F2BE
		[JsonPropertyName("tags")]
		public List<string> Tags { get; set; } = new List<string>();

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000D97 RID: 3479 RVA: 0x000510C7 File Offset: 0x0004F2C7
		// (set) Token: 0x06000D98 RID: 3480 RVA: 0x000510CF File Offset: 0x0004F2CF
		[JsonPropertyName("modidstr")]
		public string ModIdStr { get; set; } = string.Empty;

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000D99 RID: 3481 RVA: 0x000510D8 File Offset: 0x0004F2D8
		// (set) Token: 0x06000D9A RID: 3482 RVA: 0x000510E0 File Offset: 0x0004F2E0
		[JsonPropertyName("modversion")]
		public string ModVersion { get; set; } = string.Empty;

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000D9B RID: 3483 RVA: 0x000510E9 File Offset: 0x0004F2E9
		// (set) Token: 0x06000D9C RID: 3484 RVA: 0x000510F1 File Offset: 0x0004F2F1
		[JsonPropertyName("created")]
		public string Created { get; set; } = string.Empty;

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000D9D RID: 3485 RVA: 0x000510FA File Offset: 0x0004F2FA
		// (set) Token: 0x06000D9E RID: 3486 RVA: 0x00051102 File Offset: 0x0004F302
		[JsonPropertyName("changelog")]
		public string Changelog { get; set; } = string.Empty;

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000D9F RID: 3487 RVA: 0x0005110C File Offset: 0x0004F30C
		public string FormattedDate
		{
			get
			{
				DateTime dateTime;
				if (!DateTime.TryParse(this.Created, ref dateTime))
				{
					return this.Created;
				}
				return dateTime.ToShortDateString();
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x00051136 File Offset: 0x0004F336
		public string GameVersionsDisplay
		{
			get
			{
				return string.Join(", ", this.Tags);
			}
		}
	}
}
