using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace VintageStoryModManager.Models
{
	// Token: 0x0200009D RID: 157
	[NullableContext(1)]
	[Nullable(0)]
	public class DownloadableMod
	{
		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000D5A RID: 3418 RVA: 0x00050E46 File Offset: 0x0004F046
		// (set) Token: 0x06000D5B RID: 3419 RVA: 0x00050E4E File Offset: 0x0004F04E
		[JsonPropertyName("modid")]
		public int ModId { get; set; }

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000D5C RID: 3420 RVA: 0x00050E57 File Offset: 0x0004F057
		// (set) Token: 0x06000D5D RID: 3421 RVA: 0x00050E5F File Offset: 0x0004F05F
		[Nullable(2)]
		[JsonPropertyName("modidstr")]
		public string ModIdStr { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x00050E68 File Offset: 0x0004F068
		// (set) Token: 0x06000D5F RID: 3423 RVA: 0x00050E70 File Offset: 0x0004F070
		[JsonPropertyName("assetid")]
		public int AssetId { get; set; }

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000D60 RID: 3424 RVA: 0x00050E79 File Offset: 0x0004F079
		// (set) Token: 0x06000D61 RID: 3425 RVA: 0x00050E81 File Offset: 0x0004F081
		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000D62 RID: 3426 RVA: 0x00050E8A File Offset: 0x0004F08A
		// (set) Token: 0x06000D63 RID: 3427 RVA: 0x00050E92 File Offset: 0x0004F092
		[JsonPropertyName("text")]
		public string Text { get; set; } = string.Empty;

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000D64 RID: 3428 RVA: 0x00050E9B File Offset: 0x0004F09B
		// (set) Token: 0x06000D65 RID: 3429 RVA: 0x00050EA3 File Offset: 0x0004F0A3
		[JsonPropertyName("author")]
		public string Author { get; set; } = string.Empty;

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000D66 RID: 3430 RVA: 0x00050EAC File Offset: 0x0004F0AC
		// (set) Token: 0x06000D67 RID: 3431 RVA: 0x00050EB4 File Offset: 0x0004F0B4
		[Nullable(2)]
		[JsonPropertyName("urlalias")]
		public string UrlAlias { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000D68 RID: 3432 RVA: 0x00050EBD File Offset: 0x0004F0BD
		// (set) Token: 0x06000D69 RID: 3433 RVA: 0x00050EC5 File Offset: 0x0004F0C5
		[Nullable(2)]
		[JsonPropertyName("logofiledb")]
		public string LogoFileDatabase { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000D6A RID: 3434 RVA: 0x00050ECE File Offset: 0x0004F0CE
		// (set) Token: 0x06000D6B RID: 3435 RVA: 0x00050ED6 File Offset: 0x0004F0D6
		[Nullable(2)]
		[JsonPropertyName("homepageurl")]
		public string HomepageUrl { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000D6C RID: 3436 RVA: 0x00050EDF File Offset: 0x0004F0DF
		// (set) Token: 0x06000D6D RID: 3437 RVA: 0x00050EE7 File Offset: 0x0004F0E7
		[Nullable(2)]
		[JsonPropertyName("sourcecodeurl")]
		public string SourceCodeUrl { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000D6E RID: 3438 RVA: 0x00050EF0 File Offset: 0x0004F0F0
		// (set) Token: 0x06000D6F RID: 3439 RVA: 0x00050EF8 File Offset: 0x0004F0F8
		[Nullable(2)]
		[JsonPropertyName("trailervideourl")]
		public string TrailerVideoUrl { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000D70 RID: 3440 RVA: 0x00050F01 File Offset: 0x0004F101
		// (set) Token: 0x06000D71 RID: 3441 RVA: 0x00050F09 File Offset: 0x0004F109
		[Nullable(2)]
		[JsonPropertyName("issuetrackerurl")]
		public string IssueTrackerUrl { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x00050F12 File Offset: 0x0004F112
		// (set) Token: 0x06000D73 RID: 3443 RVA: 0x00050F1A File Offset: 0x0004F11A
		[Nullable(2)]
		[JsonPropertyName("wikiurl")]
		public string WikiUrl { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000D74 RID: 3444 RVA: 0x00050F23 File Offset: 0x0004F123
		// (set) Token: 0x06000D75 RID: 3445 RVA: 0x00050F2B File Offset: 0x0004F12B
		[JsonPropertyName("downloads")]
		public int Downloads { get; set; }

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000D76 RID: 3446 RVA: 0x00050F34 File Offset: 0x0004F134
		// (set) Token: 0x06000D77 RID: 3447 RVA: 0x00050F3C File Offset: 0x0004F13C
		[JsonPropertyName("follows")]
		public int Follows { get; set; }

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000D78 RID: 3448 RVA: 0x00050F45 File Offset: 0x0004F145
		// (set) Token: 0x06000D79 RID: 3449 RVA: 0x00050F4D File Offset: 0x0004F14D
		[JsonPropertyName("trendingpoints")]
		public int TrendingPoints { get; set; }

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000D7A RID: 3450 RVA: 0x00050F56 File Offset: 0x0004F156
		// (set) Token: 0x06000D7B RID: 3451 RVA: 0x00050F5E File Offset: 0x0004F15E
		[JsonPropertyName("comments")]
		public int Comments { get; set; }

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000D7C RID: 3452 RVA: 0x00050F67 File Offset: 0x0004F167
		// (set) Token: 0x06000D7D RID: 3453 RVA: 0x00050F6F File Offset: 0x0004F16F
		[JsonPropertyName("side")]
		public string Side { get; set; } = string.Empty;

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000D7E RID: 3454 RVA: 0x00050F78 File Offset: 0x0004F178
		// (set) Token: 0x06000D7F RID: 3455 RVA: 0x00050F80 File Offset: 0x0004F180
		[JsonPropertyName("type")]
		public string Type { get; set; } = string.Empty;

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000D80 RID: 3456 RVA: 0x00050F89 File Offset: 0x0004F189
		// (set) Token: 0x06000D81 RID: 3457 RVA: 0x00050F91 File Offset: 0x0004F191
		[JsonPropertyName("createdat")]
		public string CreatedAt { get; set; } = string.Empty;

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000D82 RID: 3458 RVA: 0x00050F9A File Offset: 0x0004F19A
		// (set) Token: 0x06000D83 RID: 3459 RVA: 0x00050FA2 File Offset: 0x0004F1A2
		[JsonPropertyName("lastmodified")]
		public string LastModified { get; set; } = string.Empty;

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000D84 RID: 3460 RVA: 0x00050FAB File Offset: 0x0004F1AB
		// (set) Token: 0x06000D85 RID: 3461 RVA: 0x00050FB3 File Offset: 0x0004F1B3
		[JsonPropertyName("tags")]
		public List<string> Tags { get; set; } = new List<string>();

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000D86 RID: 3462 RVA: 0x00050FBC File Offset: 0x0004F1BC
		// (set) Token: 0x06000D87 RID: 3463 RVA: 0x00050FC4 File Offset: 0x0004F1C4
		[JsonPropertyName("releases")]
		public List<DownloadableModRelease> Releases { get; set; } = new List<DownloadableModRelease>();

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000D88 RID: 3464 RVA: 0x00050FCD File Offset: 0x0004F1CD
		// (set) Token: 0x06000D89 RID: 3465 RVA: 0x00050FD5 File Offset: 0x0004F1D5
		[JsonPropertyName("screenshots")]
		public List<DownloadableModScreenshot> Screenshots { get; set; } = new List<DownloadableModScreenshot>();
	}
}
