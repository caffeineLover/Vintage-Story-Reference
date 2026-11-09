using System;
using System.Text.Json.Serialization;

namespace Xabe.FFmpeg.Downloader
{
	// Token: 0x02000013 RID: 19
	internal class Bin
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002799 File Offset: 0x00000999
		// (set) Token: 0x0600002F RID: 47 RVA: 0x000027A1 File Offset: 0x000009A1
		[JsonPropertyName("windows-32")]
		public Windows32 Windows32 { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000027AA File Offset: 0x000009AA
		// (set) Token: 0x06000031 RID: 49 RVA: 0x000027B2 File Offset: 0x000009B2
		[JsonPropertyName("windows-64")]
		public Windows64 Windows64 { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000027BB File Offset: 0x000009BB
		// (set) Token: 0x06000033 RID: 51 RVA: 0x000027C3 File Offset: 0x000009C3
		[JsonPropertyName("linux-32")]
		public Linux32 Linux32 { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000034 RID: 52 RVA: 0x000027CC File Offset: 0x000009CC
		// (set) Token: 0x06000035 RID: 53 RVA: 0x000027D4 File Offset: 0x000009D4
		[JsonPropertyName("linux-64")]
		public Linux64 Linux64 { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000036 RID: 54 RVA: 0x000027DD File Offset: 0x000009DD
		// (set) Token: 0x06000037 RID: 55 RVA: 0x000027E5 File Offset: 0x000009E5
		[JsonPropertyName("linux-armhf")]
		public LinuxArmhf LinuxArmhf { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000038 RID: 56 RVA: 0x000027EE File Offset: 0x000009EE
		// (set) Token: 0x06000039 RID: 57 RVA: 0x000027F6 File Offset: 0x000009F6
		[JsonPropertyName("linux-armel")]
		public LinuxArmel LinuxArmel { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003A RID: 58 RVA: 0x000027FF File Offset: 0x000009FF
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002807 File Offset: 0x00000A07
		[JsonPropertyName("linux-arm64")]
		public LinuxArm64 LinuxArm64 { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002810 File Offset: 0x00000A10
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002818 File Offset: 0x00000A18
		[JsonPropertyName("osx-64")]
		public Osx64 Osx64 { get; set; }
	}
}
