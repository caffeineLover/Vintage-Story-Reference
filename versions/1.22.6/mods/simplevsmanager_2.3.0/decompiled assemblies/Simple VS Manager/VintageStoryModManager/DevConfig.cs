using System;
using System.IO;
using System.Runtime.CompilerServices;
using VintageStoryModManager.Services;

namespace VintageStoryModManager
{
	// Token: 0x02000012 RID: 18
	[NullableContext(1)]
	[Nullable(0)]
	public static class DevConfig
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00005808 File Offset: 0x00003A08
		public static string SingleInstanceMutexName { get; } = "VintageStoryModManager.SingleInstance";

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000CE RID: 206 RVA: 0x0000580F File Offset: 0x00003A0F
		public static int MaxConcurrentDatabaseRefreshes { get; } = 4;

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00005816 File Offset: 0x00003A16
		public static int MaxConcurrentUserReportRefreshes { get; } = 4;

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x0000581D File Offset: 0x00003A1D
		public static int MaxNewModsRecentMonths { get; } = 24;

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00005824 File Offset: 0x00003A24
		public static int InstalledModsIncrementalBatchSize { get; } = 32;

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x0000582B File Offset: 0x00003A2B
		public static int MaxModDatabaseResultLimit { get; } = int.MaxValue;

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00005832 File Offset: 0x00003A32
		public static double ModListScrollMultiplier { get; } = 0.5;

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00005839 File Offset: 0x00003A39
		public static double ModDbDesignScrollMultiplier { get; } = 20.0;

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00005840 File Offset: 0x00003A40
		public static double LoadMoreScrollThreshold { get; } = 0.98;

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00005847 File Offset: 0x00003A47
		public static double HoverOverlayOpacity { get; } = 0.1;

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x0000584E File Offset: 0x00003A4E
		public static double SelectionOverlayOpacity { get; } = 0.25;

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00005855 File Offset: 0x00003A55
		public static string ManagerModDatabaseUrl { get; } = "https://mods.vintagestory.at/simplevsmanager";

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x0000585C File Offset: 0x00003A5C
		public static string ManagerModDatabaseModId { get; } = "5545";

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00005863 File Offset: 0x00003A63
		public static string ModDatabaseUnavailableMessage { get; } = "Could not reach the online Mod Database, please check your internet connection";

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000DB RID: 219 RVA: 0x0000586A File Offset: 0x00003A6A
		public static string PresetDirectoryName { get; } = "Presets";

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00005871 File Offset: 0x00003A71
		public static string ModListDirectoryName { get; } = "Modlists";

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00005878 File Offset: 0x00003A78
		public static string RebuiltModListDirectoryName { get; } = "Rebuilt";

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000DE RID: 222 RVA: 0x0000587F File Offset: 0x00003A7F
		public static string CloudModListCacheDirectoryName { get; } = "Temp Cache/Modlists (Cloud Cache)";

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00005886 File Offset: 0x00003A86
		public static string BackupDirectoryName { get; } = "Backups";

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x0000588D File Offset: 0x00003A8D
		public static string DataFolderBackupDirectoryName { get; } = "Data Folder Backups";

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00005894 File Offset: 0x00003A94
		public static string DataFolderBackupManifestFileName { get; } = "data-backup.json";

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x0000589B File Offset: 0x00003A9B
		public static string DataFolderBackupSaveStoreName { get; } = ".saves";

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x000058A2 File Offset: 0x00003AA2
		public static int AutomaticConfigMaxWordDistance { get; } = 2;

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x000058A9 File Offset: 0x00003AA9
		public static string MetadataFolderName { get; } = "Temp Cache/Mod Metadata";

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x000058B0 File Offset: 0x00003AB0
		public static string MetadataIndexFileName { get; } = "metadata-index.json";

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x000058B7 File Offset: 0x00003AB7
		public static string IconCacheFolderName { get; } = "Temp Cache/Mod Icons";

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x000058BE File Offset: 0x00003ABE
		public static string FirebaseModlistDefaultDbUrl { get; } = "https://simplevsmanager-default-rtdb.europe-west1.firebasedatabase.app";

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x000058C5 File Offset: 0x00003AC5
		public static string FirebaseLegacyModlistDbUrl { get; } = "https://simple-vs-manager-default-rtdb.europe-west1.firebasedatabase.app";

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x000058CC File Offset: 0x00003ACC
		public static string FirebaseBackupDirectory
		{
			get
			{
				return Path.Combine(DevConfig.GetManagerDirectory(), DevConfig.FirebaseCacheDirectoryName);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000EA RID: 234 RVA: 0x000058DD File Offset: 0x00003ADD
		public static string StatusTimestampFormat { get; } = "yyyy-MM-dd HH:mm:ss.fff";

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000EB RID: 235 RVA: 0x000058E4 File Offset: 0x00003AE4
		public static int StatusLogMaxModNameLength { get; } = 10;

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000EC RID: 236 RVA: 0x000058EB File Offset: 0x00003AEB
		public static int StatusLogMaxLines { get; } = 5000;

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000ED RID: 237 RVA: 0x000058F2 File Offset: 0x00003AF2
		public static string ModCompatibilityApiUrlTemplate { get; } = "https://mods.vintagestory.at/api/mod/{0}";

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000EE RID: 238 RVA: 0x000058F9 File Offset: 0x00003AF9
		public static string ModCompatibilityPageUrlTemplate { get; } = "https://mods.vintagestory.at/{0}";

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00005900 File Offset: 0x00003B00
		public static string FirebaseSignInEndpoint { get; } = "https://identitytoolkit.googleapis.com/v1/accounts:signUp";

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00005907 File Offset: 0x00003B07
		public static string FirebaseRefreshEndpoint { get; } = "https://securetoken.googleapis.com/v1/token";

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x0000590E File Offset: 0x00003B0E
		public static string FirebaseDeleteEndpoint { get; } = "https://identitytoolkit.googleapis.com/v1/accounts:delete";

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00005915 File Offset: 0x00003B15
		public static string FirebaseAuthStateFileName { get; } = "firebase-auth.json";

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x0000591C File Offset: 0x00003B1C
		public static string FirebaseDefaultApiKey { get; } = "AIzaSyBjIEr_JbB-Fx9hMa9nAOwaUyPP82HEeG4";

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00005923 File Offset: 0x00003B23
		public static string FirebaseLegacyApiKey { get; } = "AIzaSyCmDJ9yC1ccUEUf41fC-SI8fuXFJzWWlHY";

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x0000592A File Offset: 0x00003B2A
		public static string FirebaseNewProjectId { get; } = "simplevsmanager";

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00005931 File Offset: 0x00003B31
		public static string FirebaseLegacyProjectId { get; } = "simple-vs-manager";

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00005938 File Offset: 0x00003B38
		public static string FirebaseAuthBackupDirectoryName { get; } = "SVSM Backup";

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x0000593F File Offset: 0x00003B3F
		public static string FirebaseCacheDirectoryName { get; } = "Temp Cache/Firebase Cache";

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00005946 File Offset: 0x00003B46
		public static string ConfigurationFileName { get; } = "SimpleVSManagerConfiguration.json";

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000FA RID: 250 RVA: 0x0000594D File Offset: 0x00003B4D
		public static string ModConfigPathsFileName { get; } = "SimpleVSManagerModConfigPaths.json";

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00005954 File Offset: 0x00003B54
		public static int DefaultModDatabaseSearchResultLimit { get; } = 30;

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000FC RID: 252 RVA: 0x0000595B File Offset: 0x00003B5B
		public static int DefaultModDatabaseNewModsRecentMonths { get; } = 3;

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00005962 File Offset: 0x00003B62
		public static int MaxModDatabaseNewModsRecentMonths { get; } = 24;

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00005969 File Offset: 0x00003B69
		public static int GameSessionVoteThreshold { get; } = 5;

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00005970 File Offset: 0x00003B70
		public static TimeSpan MinimumSessionDuration { get; } = TimeSpan.FromMinutes(30.0);

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00005977 File Offset: 0x00003B77
		public static string GameVersionsEndpoint { get; } = "https://mods.vintagestory.at/api/gameversions";

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000101 RID: 257 RVA: 0x0000597E File Offset: 0x00003B7E
		public static int ModDatabaseCacheSchemaVersion { get; } = 2;

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00005985 File Offset: 0x00003B85
		public static int ModDatabaseMinimumSupportedCacheSchemaVersion { get; } = 1;

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000103 RID: 259 RVA: 0x0000598C File Offset: 0x00003B8C
		public static string ModDatabaseAnyGameVersionToken { get; } = "any";

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00005993 File Offset: 0x00003B93
		public static int ModDiscoveryBatchSize { get; } = 32;

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000105 RID: 261 RVA: 0x0000599A File Offset: 0x00003B9A
		public static string ModDiscoveryGeneralLoadErrorMessage { get; } = "Unable to load mod. Check log files.";

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000106 RID: 262 RVA: 0x000059A1 File Offset: 0x00003BA1
		public static string ModDiscoveryDependencyErrorMessage { get; } = "Unable to load mod. A dependency has an error. Make sure they all load correctly.";

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000107 RID: 263 RVA: 0x000059A8 File Offset: 0x00003BA8
		public static string DataFolderName { get; } = "VintagestoryData";

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000108 RID: 264 RVA: 0x000059AF File Offset: 0x00003BAF
		public static string ModVersionVoteDefaultDbUrl { get; } = "https://simplevsmanager-default-rtdb.europe-west1.firebasedatabase.app";

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000109 RID: 265 RVA: 0x000059B6 File Offset: 0x00003BB6
		public static string ModVersionVoteRootPath { get; } = "compatVotes";

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600010A RID: 266 RVA: 0x000059BD File Offset: 0x00003BBD
		public static string InternetAccessDisabledMessage { get; } = "Internet access is disabled for the mod database and compatibility voting.";

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600010B RID: 267 RVA: 0x000059C4 File Offset: 0x00003BC4
		public static string ModDatabaseApiEndpointFormat { get; } = "https://mods.vintagestory.at/api/mod/{0}";

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600010C RID: 268 RVA: 0x000059CB File Offset: 0x00003BCB
		public static string ModDatabaseSearchEndpointFormat { get; } = "https://mods.vintagestory.at/api/mods?search={0}&limit={1}";

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600010D RID: 269 RVA: 0x000059D2 File Offset: 0x00003BD2
		public static string ModDatabaseMostDownloadedEndpointFormat { get; } = "https://mods.vintagestory.at/api/mods?sort=downloadsdesc&limit={0}";

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600010E RID: 270 RVA: 0x000059D9 File Offset: 0x00003BD9
		public static string ModDatabaseRecentlyCreatedEndpointFormat { get; } = "https://mods.vintagestory.at/api/mods?sortby=created&sortdir=d&limit={0}";

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600010F RID: 271 RVA: 0x000059E0 File Offset: 0x00003BE0
		public static string ModDatabaseRecentlyUpdatedEndpointFormat { get; } = "https://mods.vintagestory.at/api/mods?sortby=updated&sortdir=d&limit={0}";

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000110 RID: 272 RVA: 0x000059E7 File Offset: 0x00003BE7
		public static string ModDatabasePageBaseUrl { get; } = "https://mods.vintagestory.at/show/mod/";

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000111 RID: 273 RVA: 0x000059EE File Offset: 0x00003BEE
		public static string ModDatabasePageAliasBaseUrl { get; } = "https://mods.vintagestory.at/";

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000112 RID: 274 RVA: 0x000059F5 File Offset: 0x00003BF5
		public static int ModDatabaseMaxConcurrentMetadataRequests { get; } = 4;

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000113 RID: 275 RVA: 0x000059FC File Offset: 0x00003BFC
		public static int ModDatabaseMinimumTotalDownloadsForTrending { get; } = 500;

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00005A03 File Offset: 0x00003C03
		public static int ModDatabaseDefaultNewModsMonths { get; } = 3;

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00005A0A File Offset: 0x00003C0A
		public static int ModDatabaseMaxNewModsMonths { get; } = 24;

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00005A11 File Offset: 0x00003C11
		public static double ModDatabaseMinimumIntervalDays { get; } = 0.041666666666666664;

		// Token: 0x06000117 RID: 279 RVA: 0x00005A18 File Offset: 0x00003C18
		private static string GetManagerDirectory()
		{
			string customConfigFolder = CustomConfigFolderManager.GetCustomConfigFolder();
			if (!string.IsNullOrWhiteSpace(customConfigFolder))
			{
				return customConfigFolder;
			}
			string folderPath = Environment.GetFolderPath(28);
			if (!string.IsNullOrWhiteSpace(folderPath))
			{
				return Path.Combine(folderPath, "Simple VS Manager");
			}
			string folderPath2 = Environment.GetFolderPath(26);
			if (!string.IsNullOrWhiteSpace(folderPath2))
			{
				return Path.Combine(folderPath2, "Simple VS Manager");
			}
			string folderPath3 = Environment.GetFolderPath(5);
			if (!string.IsNullOrWhiteSpace(folderPath3))
			{
				return Path.Combine(folderPath3, "Simple VS Manager");
			}
			string folderPath4 = Environment.GetFolderPath(5);
			if (!string.IsNullOrWhiteSpace(folderPath4))
			{
				return Path.Combine(folderPath4, "Simple VS Manager");
			}
			return Path.Combine(AppContext.BaseDirectory, "Simple VS Manager");
		}
	}
}
