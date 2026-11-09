using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF
{
	// Token: 0x02000005 RID: 5
	[NullableContext(2)]
	[Nullable(0)]
	public static class Settings
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000022F6 File Offset: 0x000004F6
		// (set) Token: 0x0600000F RID: 15 RVA: 0x000022FD File Offset: 0x000004FD
		public static LicenseType? License { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002305 File Offset: 0x00000505
		// (set) Token: 0x06000011 RID: 17 RVA: 0x0000230C File Offset: 0x0000050C
		[Obsolete("This setting is ignored since the 2023.10 version. The new infinite layout detection algorithm works automatically. You can safely remove this setting from your codebase.")]
		public static int DocumentLayoutExceptionThreshold { get; set; } = 250;

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002314 File Offset: 0x00000514
		// (set) Token: 0x06000013 RID: 19 RVA: 0x0000231B File Offset: 0x0000051B
		public static bool EnableCaching { get; set; } = true;

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002323 File Offset: 0x00000523
		// (set) Token: 0x06000015 RID: 21 RVA: 0x0000232A File Offset: 0x0000052A
		public static bool EnableDebugging { get; set; } = Debugger.IsAttached;

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002332 File Offset: 0x00000532
		// (set) Token: 0x06000017 RID: 23 RVA: 0x00002339 File Offset: 0x00000539
		public static bool CheckIfAllTextGlyphsAreAvailable { get; set; } = Debugger.IsAttached;

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002341 File Offset: 0x00000541
		// (set) Token: 0x06000019 RID: 25 RVA: 0x00002348 File Offset: 0x00000548
		public static bool UseEnvironmentFonts { get; set; } = true;

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002350 File Offset: 0x00000550
		[Nullable(1)]
		public static ICollection<string> FontDiscoveryPaths { [NullableContext(1)] get; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002357 File Offset: 0x00000557
		// (set) Token: 0x0600001C RID: 28 RVA: 0x0000235E File Offset: 0x0000055E
		public static string TemporaryStoragePath { get; set; }

		// Token: 0x0600001D RID: 29 RVA: 0x00002368 File Offset: 0x00000568
		static Settings()
		{
			List<string> list = new List<string>();
			list.Add(Helpers.ApplicationFilesPath);
			Settings.FontDiscoveryPaths = list;
			SkNativeDependencyCompatibilityChecker.Test();
		}
	}
}
