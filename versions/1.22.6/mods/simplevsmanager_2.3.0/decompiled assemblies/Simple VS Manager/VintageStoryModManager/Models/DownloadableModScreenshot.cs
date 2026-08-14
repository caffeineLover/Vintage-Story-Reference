using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace VintageStoryModManager.Models
{
	// Token: 0x0200009F RID: 159
	[NullableContext(1)]
	[Nullable(0)]
	public class DownloadableModScreenshot
	{
		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x000511A8 File Offset: 0x0004F3A8
		// (set) Token: 0x06000DA3 RID: 3491 RVA: 0x000511B0 File Offset: 0x0004F3B0
		[JsonPropertyName("fileid")]
		[JsonConverter(typeof(FlexibleIntConverter))]
		public int FileId { get; set; }

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x000511B9 File Offset: 0x0004F3B9
		// (set) Token: 0x06000DA5 RID: 3493 RVA: 0x000511C1 File Offset: 0x0004F3C1
		[JsonPropertyName("mainfile")]
		public string MainFile { get; set; } = string.Empty;

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x000511CA File Offset: 0x0004F3CA
		// (set) Token: 0x06000DA7 RID: 3495 RVA: 0x000511D2 File Offset: 0x0004F3D2
		[JsonPropertyName("filename")]
		public string Filename { get; set; } = string.Empty;

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000DA8 RID: 3496 RVA: 0x000511DB File Offset: 0x0004F3DB
		// (set) Token: 0x06000DA9 RID: 3497 RVA: 0x000511E3 File Offset: 0x0004F3E3
		[JsonPropertyName("thumbnailfile")]
		public string ThumbnailFile { get; set; } = string.Empty;

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000DAA RID: 3498 RVA: 0x000511EC File Offset: 0x0004F3EC
		// (set) Token: 0x06000DAB RID: 3499 RVA: 0x000511F4 File Offset: 0x0004F3F4
		[JsonPropertyName("createdat")]
		public string CreatedAt { get; set; } = string.Empty;
	}
}
