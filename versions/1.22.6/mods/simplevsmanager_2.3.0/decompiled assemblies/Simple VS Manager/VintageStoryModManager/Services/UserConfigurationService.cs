using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
using VintageStoryModManager.ViewModels;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000091 RID: 145
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class UserConfigurationService
	{
		// Token: 0x06000BCF RID: 3023 RVA: 0x00048A0C File Offset: 0x00046C0C
		public UserConfigurationService()
		{
			this._configurationPath = UserConfigurationService.DetermineConfigurationPath();
			this._modConfigPathsPath = UserConfigurationService.DetermineModConfigPathsPath(UserConfigurationService.ModConfigPathsFileName);
			this.Load();
			if (string.IsNullOrWhiteSpace(this._currentThemeName))
			{
				this._currentThemeName = UserConfigurationService.GetThemeDisplayName(this.ColorTheme);
			}
			this._hasPendingSave |= !File.Exists(this._configurationPath);
			this._hasPendingModConfigPathSave = false;
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000BD0 RID: 3024 RVA: 0x00048B8C File Offset: 0x00046D8C
		[Nullable(2)]
		public string DataDirectory
		{
			[NullableContext(2)]
			get
			{
				return this.ActiveProfile.DataDirectory;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000BD1 RID: 3025 RVA: 0x00048B99 File Offset: 0x00046D99
		[Nullable(2)]
		public string GameDirectory
		{
			[NullableContext(2)]
			get
			{
				return this.ActiveProfile.GameDirectory;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000BD2 RID: 3026 RVA: 0x00048BA6 File Offset: 0x00046DA6
		[Nullable(2)]
		public string TargetGameVersionOverride
		{
			[NullableContext(2)]
			get
			{
				return this.ActiveProfile.TargetGameVersionOverride;
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000BD3 RID: 3027 RVA: 0x00048BB3 File Offset: 0x00046DB3
		public bool RequiresDataDirectorySelection
		{
			get
			{
				return this.ActiveProfile.RequiresDataDirectorySelection;
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000BD4 RID: 3028 RVA: 0x00048BC0 File Offset: 0x00046DC0
		public bool RequiresGameDirectorySelection
		{
			get
			{
				return this.ActiveProfile.RequiresGameDirectorySelection;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000BD5 RID: 3029 RVA: 0x00048BCD File Offset: 0x00046DCD
		// (set) Token: 0x06000BD6 RID: 3030 RVA: 0x00048BD5 File Offset: 0x00046DD5
		public string ConfigurationVersion { get; private set; } = UserConfigurationService.CurrentConfigurationVersion;

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000BD7 RID: 3031 RVA: 0x00048BDE File Offset: 0x00046DDE
		// (set) Token: 0x06000BD8 RID: 3032 RVA: 0x00048BE6 File Offset: 0x00046DE6
		public string ModManagerVersion { get; private set; } = UserConfigurationService.CurrentModManagerVersion;

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000BD9 RID: 3033 RVA: 0x00048BEF File Offset: 0x00046DEF
		// (set) Token: 0x06000BDA RID: 3034 RVA: 0x00048BF7 File Offset: 0x00046DF7
		public bool HasVersionMismatch { get; private set; }

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000BDB RID: 3035 RVA: 0x00048C00 File Offset: 0x00046E00
		// (set) Token: 0x06000BDC RID: 3036 RVA: 0x00048C08 File Offset: 0x00046E08
		[Nullable(2)]
		public string PreviousConfigurationVersion { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000BDD RID: 3037 RVA: 0x00048C11 File Offset: 0x00046E11
		// (set) Token: 0x06000BDE RID: 3038 RVA: 0x00048C19 File Offset: 0x00046E19
		[Nullable(2)]
		public string PreviousModManagerVersion { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000BDF RID: 3039 RVA: 0x00048C22 File Offset: 0x00046E22
		// (set) Token: 0x06000BE0 RID: 3040 RVA: 0x00048C2A File Offset: 0x00046E2A
		public bool IsCompactView { get; private set; }

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000BE1 RID: 3041 RVA: 0x00048C33 File Offset: 0x00046E33
		// (set) Token: 0x06000BE2 RID: 3042 RVA: 0x00048C3B File Offset: 0x00046E3B
		public bool UseModDbDesignView { get; private set; } = true;

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x00048C44 File Offset: 0x00046E44
		// (set) Token: 0x06000BE4 RID: 3044 RVA: 0x00048C4C File Offset: 0x00046E4C
		public bool CacheAllVersionsLocally { get; private set; } = true;

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x00048C55 File Offset: 0x00046E55
		// (set) Token: 0x06000BE6 RID: 3046 RVA: 0x00048C5D File Offset: 0x00046E5D
		public bool CacheModDatabaseResults { get; private set; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000BE7 RID: 3047 RVA: 0x00048C66 File Offset: 0x00046E66
		// (set) Token: 0x06000BE8 RID: 3048 RVA: 0x00048C6E File Offset: 0x00046E6E
		public ColorTheme ColorTheme { get; private set; }

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000BE9 RID: 3049 RVA: 0x00048C77 File Offset: 0x00046E77
		// (set) Token: 0x06000BEA RID: 3050 RVA: 0x00048C7F File Offset: 0x00046E7F
		public bool ExcludeInstalledModDatabaseResults { get; private set; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000BEB RID: 3051 RVA: 0x00048C88 File Offset: 0x00046E88
		// (set) Token: 0x06000BEC RID: 3052 RVA: 0x00048C90 File Offset: 0x00046E90
		public bool OnlyShowCompatibleModDatabaseResults { get; private set; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000BED RID: 3053 RVA: 0x00048C99 File Offset: 0x00046E99
		// (set) Token: 0x06000BEE RID: 3054 RVA: 0x00048CA1 File Offset: 0x00046EA1
		public bool DisableAutoRefresh { get; private set; }

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000BEF RID: 3055 RVA: 0x00048CAA File Offset: 0x00046EAA
		// (set) Token: 0x06000BF0 RID: 3056 RVA: 0x00048CB2 File Offset: 0x00046EB2
		public bool DisableAutoRefreshWarningAcknowledged { get; private set; }

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000BF1 RID: 3057 RVA: 0x00048CBB File Offset: 0x00046EBB
		// (set) Token: 0x06000BF2 RID: 3058 RVA: 0x00048CC3 File Offset: 0x00046EC3
		public bool DisableInternetAccess { get; private set; }

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000BF3 RID: 3059 RVA: 0x00048CCC File Offset: 0x00046ECC
		// (set) Token: 0x06000BF4 RID: 3060 RVA: 0x00048CD4 File Offset: 0x00046ED4
		public bool EnableServerOptions { get; private set; }

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000BF5 RID: 3061 RVA: 0x00048CDD File Offset: 0x00046EDD
		// (set) Token: 0x06000BF6 RID: 3062 RVA: 0x00048CE5 File Offset: 0x00046EE5
		public bool LogModUpdates { get; private set; }

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000BF7 RID: 3063 RVA: 0x00048CEE File Offset: 0x00046EEE
		// (set) Token: 0x06000BF8 RID: 3064 RVA: 0x00048CF6 File Offset: 0x00046EF6
		public bool LogModInstalls { get; private set; }

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000BF9 RID: 3065 RVA: 0x00048CFF File Offset: 0x00046EFF
		// (set) Token: 0x06000BFA RID: 3066 RVA: 0x00048D07 File Offset: 0x00046F07
		public bool LogModDeletions { get; private set; }

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000BFB RID: 3067 RVA: 0x00048D10 File Offset: 0x00046F10
		// (set) Token: 0x06000BFC RID: 3068 RVA: 0x00048D18 File Offset: 0x00046F18
		public bool LogAppLaunchAndExit { get; private set; }

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000BFD RID: 3069 RVA: 0x00048D21 File Offset: 0x00046F21
		// (set) Token: 0x06000BFE RID: 3070 RVA: 0x00048D29 File Offset: 0x00046F29
		public bool LogErrorsAndExceptions { get; private set; }

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000BFF RID: 3071 RVA: 0x00048D32 File Offset: 0x00046F32
		// (set) Token: 0x06000C00 RID: 3072 RVA: 0x00048D3A File Offset: 0x00046F3A
		public bool AutomaticDataBackupsEnabled { get; private set; }

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000C01 RID: 3073 RVA: 0x00048D43 File Offset: 0x00046F43
		// (set) Token: 0x06000C02 RID: 3074 RVA: 0x00048D4B File Offset: 0x00046F4B
		public bool AutomaticDataBackupsWarningAcknowledged { get; private set; }

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000C03 RID: 3075 RVA: 0x00048D54 File Offset: 0x00046F54
		// (set) Token: 0x06000C04 RID: 3076 RVA: 0x00048D5C File Offset: 0x00046F5C
		public bool CloseManagerAfterGameLaunch { get; private set; }

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000C05 RID: 3077 RVA: 0x00048D65 File Offset: 0x00046F65
		// (set) Token: 0x06000C06 RID: 3078 RVA: 0x00048D6D File Offset: 0x00046F6D
		[Nullable(2)]
		public string CustomDataBackupLocation { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000C07 RID: 3079 RVA: 0x00048D76 File Offset: 0x00046F76
		// (set) Token: 0x06000C08 RID: 3080 RVA: 0x00048D7E File Offset: 0x00046F7E
		public int MaximumDataBackups { get; private set; }

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000C09 RID: 3081 RVA: 0x00048D87 File Offset: 0x00046F87
		// (set) Token: 0x06000C0A RID: 3082 RVA: 0x00048D8F File Offset: 0x00046F8F
		public bool SuppressModlistSavePrompt { get; private set; }

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000C0B RID: 3083 RVA: 0x00048D98 File Offset: 0x00046F98
		public bool SuppressRefreshCachePrompt
		{
			get
			{
				return this._suppressRefreshCachePrompt && (this._suppressRefreshCachePromptVersion == null || string.Equals(this._suppressRefreshCachePromptVersion, this.ModManagerVersion, 5));
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000C0C RID: 3084 RVA: 0x00048DC0 File Offset: 0x00046FC0
		// (set) Token: 0x06000C0D RID: 3085 RVA: 0x00048DC8 File Offset: 0x00046FC8
		public bool GameProfileCreationWarningAcknowledged { get; private set; }

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000C0E RID: 3086 RVA: 0x00048DD1 File Offset: 0x00046FD1
		// (set) Token: 0x06000C0F RID: 3087 RVA: 0x00048DD9 File Offset: 0x00046FD9
		public ModlistAutoLoadBehavior ModlistAutoLoadBehavior { get; private set; }

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000C10 RID: 3088 RVA: 0x00048DE2 File Offset: 0x00046FE2
		// (set) Token: 0x06000C11 RID: 3089 RVA: 0x00048DEA File Offset: 0x00046FEA
		public ModlistsTabSelection PreferredModlistsTab
		{
			get
			{
				return this._preferredModlistsTab;
			}
			private set
			{
				this._preferredModlistsTab = value;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000C12 RID: 3090 RVA: 0x00048DF3 File Offset: 0x00046FF3
		// (set) Token: 0x06000C13 RID: 3091 RVA: 0x00048DFB File Offset: 0x00046FFB
		public ModInfoPanePlacement ModInfoPanePlacement { get; private set; } = ModInfoPanePlacement.Right;

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000C14 RID: 3092 RVA: 0x00048E04 File Offset: 0x00047004
		// (set) Token: 0x06000C15 RID: 3093 RVA: 0x00048E0C File Offset: 0x0004700C
		public int ModDatabaseSearchResultLimit { get; private set; } = UserConfigurationService.DefaultModDatabaseSearchResultLimit;

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000C16 RID: 3094 RVA: 0x00048E15 File Offset: 0x00047015
		// (set) Token: 0x06000C17 RID: 3095 RVA: 0x00048E1D File Offset: 0x0004701D
		public int ModDatabaseNewModsRecentMonths { get; private set; } = UserConfigurationService.DefaultModDatabaseNewModsRecentMonths;

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000C18 RID: 3096 RVA: 0x00048E26 File Offset: 0x00047026
		// (set) Token: 0x06000C19 RID: 3097 RVA: 0x00048E2E File Offset: 0x0004702E
		public ModDatabaseAutoLoadMode ModDatabaseAutoLoadMode { get; private set; }

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000C1A RID: 3098 RVA: 0x00048E37 File Offset: 0x00047037
		// (set) Token: 0x06000C1B RID: 3099 RVA: 0x00048E3F File Offset: 0x0004703F
		public string ModBrowserOrderBy { get; private set; } = "follows";

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000C1C RID: 3100 RVA: 0x00048E48 File Offset: 0x00047048
		// (set) Token: 0x06000C1D RID: 3101 RVA: 0x00048E50 File Offset: 0x00047050
		public string ModBrowserOrderByDirection { get; private set; } = "desc";

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000C1E RID: 3102 RVA: 0x00048E59 File Offset: 0x00047059
		// (set) Token: 0x06000C1F RID: 3103 RVA: 0x00048E61 File Offset: 0x00047061
		public string ModBrowserSelectedSide { get; private set; } = "any";

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000C20 RID: 3104 RVA: 0x00048E6A File Offset: 0x0004706A
		// (set) Token: 0x06000C21 RID: 3105 RVA: 0x00048E72 File Offset: 0x00047072
		public string ModBrowserSelectedInstalledFilter { get; private set; } = "all";

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000C22 RID: 3106 RVA: 0x00048E7B File Offset: 0x0004707B
		// (set) Token: 0x06000C23 RID: 3107 RVA: 0x00048E83 File Offset: 0x00047083
		public bool ModBrowserOnlyFavorites { get; private set; }

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000C24 RID: 3108 RVA: 0x00048E8C File Offset: 0x0004708C
		// (set) Token: 0x06000C25 RID: 3109 RVA: 0x00048E94 File Offset: 0x00047094
		public bool ModBrowserRelevantSearch { get; private set; } = true;

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000C26 RID: 3110 RVA: 0x00048E9D File Offset: 0x0004709D
		// (set) Token: 0x06000C27 RID: 3111 RVA: 0x00048EA5 File Offset: 0x000470A5
		public List<int> ModBrowserFavoriteModIds { get; private set; } = new List<int>();

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000C28 RID: 3112 RVA: 0x00048EAE File Offset: 0x000470AE
		// (set) Token: 0x06000C29 RID: 3113 RVA: 0x00048EB6 File Offset: 0x000470B6
		public List<string> ModBrowserSelectedVersionIds { get; private set; } = new List<string>();

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000C2A RID: 3114 RVA: 0x00048EBF File Offset: 0x000470BF
		// (set) Token: 0x06000C2B RID: 3115 RVA: 0x00048EC7 File Offset: 0x000470C7
		public List<int> ModBrowserSelectedTagIds { get; private set; } = new List<int>();

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000C2C RID: 3116 RVA: 0x00048ED0 File Offset: 0x000470D0
		public double? WindowWidth
		{
			get
			{
				return this._windowWidth;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000C2D RID: 3117 RVA: 0x00048ED8 File Offset: 0x000470D8
		public double? WindowHeight
		{
			get
			{
				return this._windowHeight;
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000C2E RID: 3118 RVA: 0x00048EE0 File Offset: 0x000470E0
		public double? WindowLeft
		{
			get
			{
				return this._windowLeft;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000C2F RID: 3119 RVA: 0x00048EE8 File Offset: 0x000470E8
		public double? WindowTop
		{
			get
			{
				return this._windowTop;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x00048EF0 File Offset: 0x000470F0
		public double? ModInfoPanelLeft
		{
			get
			{
				return this._modInfoPanelLeft;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000C31 RID: 3121 RVA: 0x00048EF8 File Offset: 0x000470F8
		public double? ModInfoPanelTop
		{
			get
			{
				return this._modInfoPanelTop;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x00048F00 File Offset: 0x00047100
		[Nullable(2)]
		public string CustomShortcutPath
		{
			[NullableContext(2)]
			get
			{
				return this.ActiveProfile.CustomShortcutPath;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000C33 RID: 3123 RVA: 0x00048F0D File Offset: 0x0004710D
		// (set) Token: 0x06000C34 RID: 3124 RVA: 0x00048F15 File Offset: 0x00047115
		[Nullable(2)]
		public string CloudUploaderName { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000C35 RID: 3125 RVA: 0x00048F1E File Offset: 0x0004711E
		public bool HasPendingModUsagePrompt
		{
			get
			{
				return !this._isModUsageTrackingDisabled && this.ActiveProfile.HasPendingModUsagePrompt && this.ActiveProfile.ModUsageSessionCounts.Count > 0;
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000C36 RID: 3126 RVA: 0x00048F4A File Offset: 0x0004714A
		public bool IsModUsageTrackingEnabled
		{
			get
			{
				return !this._isModUsageTrackingDisabled;
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000C37 RID: 3127 RVA: 0x00048F55 File Offset: 0x00047155
		public string ActiveGameProfileName
		{
			get
			{
				return this.ActiveProfile.Name;
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000C38 RID: 3128 RVA: 0x00048F62 File Offset: 0x00047162
		// (set) Token: 0x06000C39 RID: 3129 RVA: 0x00048F6A File Offset: 0x0004716A
		private UserConfigurationService.GameProfileState ActiveProfile { get; set; } = new UserConfigurationService.GameProfileState("Default");

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000C3A RID: 3130 RVA: 0x00048F73 File Offset: 0x00047173
		private Dictionary<string, List<string>> ActiveModConfigPaths
		{
			get
			{
				return this.ActiveProfile.ModConfigPaths;
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000C3B RID: 3131 RVA: 0x00048F80 File Offset: 0x00047180
		private Dictionary<string, bool> ActiveBulkUpdateModExclusions
		{
			get
			{
				return this.ActiveProfile.BulkUpdateModExclusions;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000C3C RID: 3132 RVA: 0x00048F8D File Offset: 0x0004718D
		private Dictionary<string, string> ActiveSkippedModVersions
		{
			get
			{
				return this.ActiveProfile.SkippedModVersions;
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000C3D RID: 3133 RVA: 0x00048F9A File Offset: 0x0004719A
		private Dictionary<string, string> ActiveDependencyWarningOverrides
		{
			get
			{
				return this.ActiveProfile.DependencyWarningOverrides;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000C3E RID: 3134 RVA: 0x00048FA7 File Offset: 0x000471A7
		private Dictionary<ModUsageTrackingKey, int> ActiveModUsageSessionCounts
		{
			get
			{
				return this.ActiveProfile.ModUsageSessionCounts;
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000C3F RID: 3135 RVA: 0x00048FB4 File Offset: 0x000471B4
		// (set) Token: 0x06000C40 RID: 3136 RVA: 0x00048FC1 File Offset: 0x000471C1
		private int ActiveLongRunningSessionCount
		{
			get
			{
				return this.ActiveProfile.LongRunningSessionCount;
			}
			set
			{
				this.ActiveProfile.LongRunningSessionCount = value;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000C41 RID: 3137 RVA: 0x00048FCF File Offset: 0x000471CF
		// (set) Token: 0x06000C42 RID: 3138 RVA: 0x00048FDC File Offset: 0x000471DC
		private bool ActiveHasPendingModUsagePrompt
		{
			get
			{
				return this.ActiveProfile.HasPendingModUsagePrompt;
			}
			set
			{
				this.ActiveProfile.HasPendingModUsagePrompt = value;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000C43 RID: 3139 RVA: 0x00048FEA File Offset: 0x000471EA
		// (set) Token: 0x06000C44 RID: 3140 RVA: 0x00048FF2 File Offset: 0x000471F2
		public bool MigrationCheckCompleted { get; private set; }

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000C45 RID: 3141 RVA: 0x00048FFB File Offset: 0x000471FB
		// (set) Token: 0x06000C46 RID: 3142 RVA: 0x00049003 File Offset: 0x00047203
		public bool RequireExactVsVersionMatch { get; private set; }

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000C47 RID: 3143 RVA: 0x0004900C File Offset: 0x0004720C
		// (set) Token: 0x06000C48 RID: 3144 RVA: 0x00049014 File Offset: 0x00047214
		public bool FirebaseAuthBackupCreated { get; private set; }

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000C49 RID: 3145 RVA: 0x0004901D File Offset: 0x0004721D
		// (set) Token: 0x06000C4A RID: 3146 RVA: 0x00049025 File Offset: 0x00047225
		public bool ClientSettingsCleanupCompleted { get; private set; }

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000C4B RID: 3147 RVA: 0x0004902E File Offset: 0x0004722E
		// (set) Token: 0x06000C4C RID: 3148 RVA: 0x00049036 File Offset: 0x00047236
		public bool RebuiltModlistMigrationCompleted { get; private set; }

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000C4D RID: 3149 RVA: 0x0004903F File Offset: 0x0004723F
		// (set) Token: 0x06000C4E RID: 3150 RVA: 0x00049047 File Offset: 0x00047247
		public bool UseFasterThumbnails { get; private set; } = true;

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000C4F RID: 3151 RVA: 0x00049050 File Offset: 0x00047250
		// (set) Token: 0x06000C50 RID: 3152 RVA: 0x00049058 File Offset: 0x00047258
		public bool DisableHoverEffects { get; private set; }

		// Token: 0x06000C51 RID: 3153 RVA: 0x00049064 File Offset: 0x00047264
		public IReadOnlyList<string> GetGameProfileNames()
		{
			return Enumerable.ToArray<string>(Enumerable.ThenBy<string, string>(Enumerable.OrderBy<string, int>(this._gameProfiles.Keys, (string name) => (!string.Equals(name, "Default", 5)) ? 1 : 0), (string name) => name, StringComparer.OrdinalIgnoreCase));
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x000490D0 File Offset: 0x000472D0
		[NullableContext(2)]
		public bool IsDefaultGameProfile(string name)
		{
			string text = UserConfigurationService.NormalizeGameProfileName(name);
			return text != null && string.Equals(text, "Default", 5);
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x000490F8 File Offset: 0x000472F8
		[NullableContext(2)]
		public bool TryCreateGameProfile(string name, out string normalizedName, out string errorMessage)
		{
			normalizedName = UserConfigurationService.NormalizeGameProfileName(name);
			if (normalizedName == null)
			{
				errorMessage = "Enter a profile name.";
				return false;
			}
			if (string.Equals(normalizedName, "Default", 5))
			{
				errorMessage = "The Default profile already exists.";
				return false;
			}
			if (this._gameProfiles.ContainsKey(normalizedName))
			{
				errorMessage = "A profile with that name already exists.";
				return false;
			}
			UserConfigurationService.GameProfileState gameProfileState = new UserConfigurationService.GameProfileState(normalizedName);
			gameProfileState.RequiresDataDirectorySelection = true;
			gameProfileState.RequiresGameDirectorySelection = true;
			gameProfileState.DataDirectory = null;
			gameProfileState.GameDirectory = null;
			gameProfileState.CustomShortcutPath = null;
			this._gameProfiles[normalizedName] = gameProfileState;
			this.Save();
			errorMessage = null;
			return true;
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x00049190 File Offset: 0x00047390
		[NullableContext(2)]
		public bool TrySetActiveGameProfile(string name)
		{
			string text = UserConfigurationService.NormalizeGameProfileName(name);
			if (text == null)
			{
				return false;
			}
			UserConfigurationService.GameProfileState gameProfileState;
			if (!this._gameProfiles.TryGetValue(text, ref gameProfileState))
			{
				return false;
			}
			if (this.ActiveProfile == gameProfileState)
			{
				return true;
			}
			this.ActiveProfile = gameProfileState;
			this.RefreshActiveModConfigPathsFromHistory();
			this.Save();
			return true;
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x000491DC File Offset: 0x000473DC
		public bool TryDeleteGameProfiles(IReadOnlyCollection<string> profileNames, [Nullable(2)] out string errorMessage, out bool activeProfileChanged)
		{
			activeProfileChanged = false;
			if (profileNames == null || profileNames.Count == 0)
			{
				errorMessage = "Select at least one profile to delete.";
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (string text in profileNames)
			{
				if (text != null)
				{
					string text2 = UserConfigurationService.NormalizeGameProfileName(text);
					if (text2 != null)
					{
						if (string.Equals(text2, "Default", 5))
						{
							errorMessage = "The Default profile cannot be deleted.";
							return false;
						}
						hashSet.Add(text2);
					}
				}
			}
			if (hashSet.Count == 0)
			{
				errorMessage = "Select at least one profile to delete.";
				return false;
			}
			bool flag = false;
			List<string> list = new List<string>();
			foreach (string text3 in hashSet)
			{
				if (this._gameProfiles.Remove(text3))
				{
					flag = true;
					list.Add(text3);
				}
			}
			if (!flag)
			{
				errorMessage = "No matching game profiles were found.";
				return false;
			}
			if (hashSet.Contains(this.ActiveProfile.Name))
			{
				UserConfigurationService.GameProfileState activeProfile = this.EnsureProfile("Default");
				this.ActiveProfile = activeProfile;
				activeProfileChanged = true;
			}
			this.Save();
			this.DeleteProfileBackupDirectories(list);
			errorMessage = null;
			return true;
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x00049330 File Offset: 0x00047530
		public string GetActiveGameProfileBackupDirectoryName()
		{
			return UserConfigurationService.BuildBackupDirectoryName(this.ActiveProfile.Name);
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x00049344 File Offset: 0x00047544
		private void DeleteProfileBackupDirectories(IEnumerable<string> profileNames)
		{
			if (profileNames == null)
			{
				return;
			}
			string configurationDirectory = this.GetConfigurationDirectory();
			foreach (string text in profileNames)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					string text2 = UserConfigurationService.BuildBackupDirectoryName(text);
					UserConfigurationService.TryDeleteDirectory(Path.Combine(configurationDirectory, text2));
				}
			}
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x000493AC File Offset: 0x000475AC
		private UserConfigurationService.GameProfileState EnsureProfile(string name)
		{
			UserConfigurationService.GameProfileState gameProfileState;
			if (this._gameProfiles.TryGetValue(name, ref gameProfileState))
			{
				return gameProfileState;
			}
			gameProfileState = new UserConfigurationService.GameProfileState(name);
			this._gameProfiles[name] = gameProfileState;
			return gameProfileState;
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x000493E0 File Offset: 0x000475E0
		[NullableContext(2)]
		private static string NormalizeGameProfileName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return null;
			}
			string text = name.Trim();
			if (text.Length > 128)
			{
				text = text.Substring(0, 128);
			}
			if (text.Length != 0)
			{
				return text;
			}
			return null;
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x00049424 File Offset: 0x00047624
		private static string BuildBackupDirectoryName(string profileName)
		{
			string text = new string(Enumerable.ToArray<char>(Enumerable.Where<char>(profileName, (char ch) => !Enumerable.Contains<char>(Path.GetInvalidFileNameChars(), ch))));
			if (string.IsNullOrWhiteSpace(text))
			{
				text = "Default";
			}
			if (string.Equals(text, "Default", 5))
			{
				return DevConfig.BackupDirectoryName;
			}
			return DevConfig.BackupDirectoryName + "_" + text;
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x00049494 File Offset: 0x00047694
		[NullableContext(2)]
		private static void TryDeleteDirectory(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return;
			}
			try
			{
				if (Directory.Exists(path))
				{
					Directory.Delete(path, true);
				}
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x000494E0 File Offset: 0x000476E0
		private void LoadGameProfile(UserConfigurationService.GameProfileState profile, JsonObject obj)
		{
			string text = UserConfigurationService.NormalizePath(UserConfigurationService.GetOptionalString(obj["dataDirectory"]));
			if (text != null)
			{
				profile.DataDirectory = text;
			}
			string text2 = UserConfigurationService.NormalizePath(UserConfigurationService.GetOptionalString(obj["gameDirectory"]));
			if (text2 != null)
			{
				profile.GameDirectory = text2;
			}
			string optionalString = UserConfigurationService.GetOptionalString(obj["targetGameVersionOverride"]);
			if (!string.IsNullOrWhiteSpace(optionalString))
			{
				profile.TargetGameVersionOverride = optionalString;
			}
			string text3 = UserConfigurationService.NormalizePath(UserConfigurationService.GetOptionalString(obj["customShortcutPath"]));
			if (text3 != null)
			{
				profile.CustomShortcutPath = text3;
			}
			JsonNode jsonNode = obj["requiresDataDirectorySelection"];
			profile.RequiresDataDirectorySelection = ((jsonNode != null) ? jsonNode.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
			JsonNode jsonNode2 = obj["requiresGameDirectorySelection"];
			profile.RequiresGameDirectorySelection = ((jsonNode2 != null) ? jsonNode2.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
			this.LoadBulkUpdateModExclusions(obj["bulkUpdateModExclusions"], profile.BulkUpdateModExclusions);
			this.LoadSkippedModVersions(obj["skippedModVersions"], profile.SkippedModVersions);
			this.LoadDependencyWarningOverrides(obj["dependencyWarningOverrides"], profile.DependencyWarningOverrides);
			this.LoadModUsageTracking(obj["modUsageTracking"], profile);
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x00049624 File Offset: 0x00047824
		private void ApplyLegacyProfileData(JsonObject root, UserConfigurationService.GameProfileState profile)
		{
			string customShortcutPath = UserConfigurationService.NormalizePath(UserConfigurationService.GetOptionalString(root["customShortcutPath"]));
			if (profile.CustomShortcutPath == null)
			{
				profile.CustomShortcutPath = customShortcutPath;
			}
			if (profile.BulkUpdateModExclusions.Count == 0)
			{
				this.LoadBulkUpdateModExclusions(root["bulkUpdateModExclusions"], profile.BulkUpdateModExclusions);
			}
			if (profile.SkippedModVersions.Count == 0)
			{
				this.LoadSkippedModVersions(root["skippedModVersions"], profile.SkippedModVersions);
			}
			if (profile.DependencyWarningOverrides.Count == 0)
			{
				this.LoadDependencyWarningOverrides(root["dependencyWarningOverrides"], profile.DependencyWarningOverrides);
			}
			if (profile.ModUsageSessionCounts.Count == 0)
			{
				this.LoadModUsageTracking(root["modUsageTracking"], profile);
			}
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x000496E1 File Offset: 0x000478E1
		public void SetMigrationCheckCompleted()
		{
			if (this.MigrationCheckCompleted)
			{
				return;
			}
			this.MigrationCheckCompleted = true;
			this.Save();
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x000496F9 File Offset: 0x000478F9
		public void SetFirebaseAuthBackupCreated()
		{
			if (this.FirebaseAuthBackupCreated)
			{
				return;
			}
			this.FirebaseAuthBackupCreated = true;
			this.Save();
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x00049711 File Offset: 0x00047911
		public void ResetFirebaseAuthBackupFlag()
		{
			if (!this.FirebaseAuthBackupCreated)
			{
				return;
			}
			this.FirebaseAuthBackupCreated = false;
			this.Save();
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x00049729 File Offset: 0x00047929
		public void SetClientSettingsCleanupCompleted()
		{
			if (this.ClientSettingsCleanupCompleted)
			{
				return;
			}
			this.ClientSettingsCleanupCompleted = true;
			this.Save();
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x00049741 File Offset: 0x00047941
		public void SetRebuiltModlistMigrationCompleted()
		{
			if (this.RebuiltModlistMigrationCompleted)
			{
				return;
			}
			this.RebuiltModlistMigrationCompleted = true;
			this.Save();
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x00049759 File Offset: 0x00047959
		public IReadOnlyDictionary<string, string> GetThemePaletteColors()
		{
			return new Dictionary<string, string>(this._themePaletteColors, StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x0004976B File Offset: 0x0004796B
		public IReadOnlyDictionary<ModUsageTrackingKey, int> GetPendingModUsageCounts()
		{
			if (this._isModUsageTrackingDisabled)
			{
				return new Dictionary<ModUsageTrackingKey, int>();
			}
			return new Dictionary<ModUsageTrackingKey, int>(this.ActiveModUsageSessionCounts);
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x00049788 File Offset: 0x00047988
		[NullableContext(2)]
		public bool RecordLongRunningSession(IReadOnlyList<ModUsageTrackingEntry> activeMods, out bool recordedUsage)
		{
			recordedUsage = false;
			if (this._isModUsageTrackingDisabled)
			{
				return false;
			}
			bool hasPendingModUsagePrompt = this.HasPendingModUsagePrompt;
			if (activeMods != null && activeMods.Count > 0)
			{
				foreach (ModUsageTrackingEntry modUsageTrackingEntry in activeMods)
				{
					if (!string.IsNullOrEmpty(modUsageTrackingEntry.ModId) && !string.IsNullOrEmpty(modUsageTrackingEntry.ModVersion) && !string.IsNullOrEmpty(modUsageTrackingEntry.GameVersion) && modUsageTrackingEntry.CanSubmitVote && !modUsageTrackingEntry.HasUserVote)
					{
						ModUsageTrackingKey modUsageTrackingKey = new ModUsageTrackingKey(modUsageTrackingEntry.ModId, modUsageTrackingEntry.ModVersion, modUsageTrackingEntry.GameVersion);
						int num;
						if (this.ActiveModUsageSessionCounts.TryGetValue(modUsageTrackingKey, ref num))
						{
							this.ActiveModUsageSessionCounts[modUsageTrackingKey] = ((num >= 2147483646) ? int.MaxValue : (num + 1));
						}
						else
						{
							this.ActiveModUsageSessionCounts[modUsageTrackingKey] = 1;
						}
						recordedUsage = true;
					}
				}
			}
			if (this.ActiveLongRunningSessionCount < 2147483647)
			{
				int activeLongRunningSessionCount = this.ActiveLongRunningSessionCount;
				this.ActiveLongRunningSessionCount = activeLongRunningSessionCount + 1;
			}
			if (this.ActiveLongRunningSessionCount > UserConfigurationService.GameSessionVoteThreshold)
			{
				this.ActiveLongRunningSessionCount = UserConfigurationService.GameSessionVoteThreshold;
			}
			if (this.ActiveLongRunningSessionCount >= UserConfigurationService.GameSessionVoteThreshold && this.ActiveModUsageSessionCounts.Count > 0)
			{
				this.ActiveHasPendingModUsagePrompt = true;
			}
			this.Save();
			bool hasPendingModUsagePrompt2 = this.HasPendingModUsagePrompt;
			return !hasPendingModUsagePrompt && hasPendingModUsagePrompt2;
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x00049904 File Offset: 0x00047B04
		public void DisableModUsageTracking()
		{
			if (this._isModUsageTrackingDisabled)
			{
				return;
			}
			this._isModUsageTrackingDisabled = true;
			bool flag = this.ActiveLongRunningSessionCount != 0 || this.ActiveModUsageSessionCounts.Count > 0 || this.ActiveHasPendingModUsagePrompt;
			this.ResetModUsageTracking();
			if (!flag)
			{
				this.Save();
			}
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x00049944 File Offset: 0x00047B44
		[NullableContext(2)]
		public void ResetModUsageCounts(IEnumerable<ModUsageTrackingKey> keys)
		{
			if (keys == null)
			{
				return;
			}
			bool flag = false;
			foreach (ModUsageTrackingKey modUsageTrackingKey in keys)
			{
				if (modUsageTrackingKey.IsValid && this.ActiveModUsageSessionCounts.Remove(modUsageTrackingKey))
				{
					flag = true;
				}
			}
			if (this.ActiveModUsageSessionCounts.Count == 0)
			{
				if (this.ActiveLongRunningSessionCount != 0 || this.ActiveHasPendingModUsagePrompt)
				{
					this.ActiveLongRunningSessionCount = 0;
					this.ActiveHasPendingModUsagePrompt = false;
					flag = true;
				}
			}
			else
			{
				bool flag2 = this.ActiveLongRunningSessionCount >= UserConfigurationService.GameSessionVoteThreshold;
				if (this.ActiveHasPendingModUsagePrompt != flag2)
				{
					this.ActiveHasPendingModUsagePrompt = flag2;
					flag = true;
				}
			}
			if (flag)
			{
				this.Save();
			}
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x00049A00 File Offset: 0x00047C00
		[NullableContext(2)]
		public void CompleteModUsageVotes(IEnumerable<ModUsageTrackingKey> completedKeys)
		{
			bool flag = false;
			if (completedKeys != null)
			{
				foreach (ModUsageTrackingKey modUsageTrackingKey in completedKeys)
				{
					if (modUsageTrackingKey.IsValid && this.ActiveModUsageSessionCounts.Remove(modUsageTrackingKey))
					{
						flag = true;
					}
				}
			}
			if (this.ActiveModUsageSessionCounts.Count == 0)
			{
				if (this.ActiveLongRunningSessionCount != 0 || this.ActiveHasPendingModUsagePrompt)
				{
					this.ActiveLongRunningSessionCount = 0;
					this.ActiveHasPendingModUsagePrompt = false;
					flag = true;
				}
			}
			else
			{
				this.ActiveHasPendingModUsagePrompt = true;
				if (this.ActiveLongRunningSessionCount < UserConfigurationService.GameSessionVoteThreshold)
				{
					this.ActiveLongRunningSessionCount = UserConfigurationService.GameSessionVoteThreshold;
					flag = true;
				}
			}
			if (flag)
			{
				this.Save();
			}
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x00049AB8 File Offset: 0x00047CB8
		public void ResetModUsageTracking()
		{
			if (this.ActiveLongRunningSessionCount == 0 && this.ActiveModUsageSessionCounts.Count == 0 && !this.ActiveHasPendingModUsagePrompt)
			{
				return;
			}
			this.ActiveLongRunningSessionCount = 0;
			this.ActiveModUsageSessionCounts.Clear();
			this.ActiveHasPendingModUsagePrompt = false;
			this.Save();
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x00049AF8 File Offset: 0x00047CF8
		public bool TrySetThemePaletteColor(string key, string color)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				return false;
			}
			string text = key.Trim();
			if (!this._themePaletteColors.ContainsKey(text))
			{
				return false;
			}
			string text2;
			if (!UserConfigurationService.TryNormalizeHexColor(color, out text2))
			{
				return false;
			}
			string text3;
			if (this._themePaletteColors.TryGetValue(text, ref text3) && string.Equals(text3, text2, 5))
			{
				return true;
			}
			this._themePaletteColors[text] = text2;
			if (this.ColorTheme == ColorTheme.Custom)
			{
				this.EnsureCustomThemePaletteInitialized();
				this._customThemePaletteColors[text] = text2;
			}
			this.Save();
			return true;
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x00049B7E File Offset: 0x00047D7E
		public IReadOnlyList<string> GetCustomThemeNames()
		{
			return Enumerable.ToList<string>(Enumerable.OrderBy<string, string>(this._savedCustomThemes.Keys, (string name) => name, StringComparer.OrdinalIgnoreCase));
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x00049BBC File Offset: 0x00047DBC
		public IReadOnlyList<string> GetAllThemeNames()
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			hashSet.Add(UserConfigurationService.GetThemeDisplayName(ColorTheme.VintageStory));
			hashSet.Add(UserConfigurationService.GetThemeDisplayName(ColorTheme.Dark));
			hashSet.Add(UserConfigurationService.GetThemeDisplayName(ColorTheme.Light));
			HashSet<string> hashSet2 = hashSet;
			foreach (string text in this.GetCustomThemeNames())
			{
				hashSet2.Add(text);
			}
			return Enumerable.ToList<string>(Enumerable.OrderBy<string, string>(hashSet2, (string name) => name, StringComparer.OrdinalIgnoreCase));
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x00049C6C File Offset: 0x00047E6C
		public string GetCurrentThemeName()
		{
			if (!string.IsNullOrWhiteSpace(this._currentThemeName))
			{
				return this._currentThemeName;
			}
			return UserConfigurationService.GetThemeDisplayName(this.ColorTheme);
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x00049C90 File Offset: 0x00047E90
		[NullableContext(2)]
		public bool TryActivateTheme(string name)
		{
			string text = UserConfigurationService.NormalizeThemeName(name);
			if (text == null)
			{
				return false;
			}
			Dictionary<string, string> paletteOverride;
			if (this._savedCustomThemes.TryGetValue(text, ref paletteOverride))
			{
				this._currentThemeName = text;
				this.SetColorTheme(ColorTheme.Custom, paletteOverride);
				this.SyncCustomThemePaletteWithCurrentTheme();
				return true;
			}
			ColorTheme theme;
			if (UserConfigurationService.TryGetBuiltInTheme(text, out theme))
			{
				this._currentThemeName = UserConfigurationService.GetThemeDisplayName(theme);
				this.SetColorTheme(theme, null);
				return true;
			}
			return false;
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x00049CF4 File Offset: 0x00047EF4
		public bool TryGetThemePalette([Nullable(2)] string name, out IReadOnlyDictionary<string, string> palette)
		{
			palette = Enumerable.ToDictionary<KeyValuePair<string, string>, string, string>(Array.Empty<KeyValuePair<string, string>>(), (KeyValuePair<string, string> entry) => entry.Key, (KeyValuePair<string, string> entry) => entry.Value, StringComparer.OrdinalIgnoreCase);
			string text = UserConfigurationService.NormalizeThemeName(name);
			if (text == null)
			{
				return false;
			}
			Dictionary<string, string> dictionary;
			if (this._savedCustomThemes.TryGetValue(text, ref dictionary))
			{
				palette = new Dictionary<string, string>(dictionary, StringComparer.OrdinalIgnoreCase);
				return true;
			}
			ColorTheme theme;
			if (UserConfigurationService.TryGetBuiltInTheme(text, out theme))
			{
				palette = UserConfigurationService.GetDefaultThemePalette(theme);
				return true;
			}
			return false;
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x00049D90 File Offset: 0x00047F90
		[NullableContext(2)]
		public bool SaveCustomTheme(string name)
		{
			string text = UserConfigurationService.NormalizeThemeName(name);
			if (text == null)
			{
				return false;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>(this._themePaletteColors, StringComparer.OrdinalIgnoreCase);
			UserConfigurationService.EnsurePaletteDefaults(dictionary, UserConfigurationService.GetDefaultPalette(ColorTheme.Custom));
			this._savedCustomThemes[text] = dictionary;
			this._currentThemeName = text;
			this.SetColorTheme(ColorTheme.Custom, dictionary);
			this.SyncCustomThemePaletteWithCurrentTheme();
			this.Save();
			return true;
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x00049DF0 File Offset: 0x00047FF0
		[NullableContext(2)]
		public bool DeleteCustomTheme(string name)
		{
			string text = UserConfigurationService.NormalizeThemeName(name);
			if (text == null)
			{
				return false;
			}
			if (!this._savedCustomThemes.Remove(text))
			{
				return false;
			}
			if (string.Equals(this._currentThemeName, text, 5))
			{
				this._currentThemeName = UserConfigurationService.GetThemeDisplayName(ColorTheme.VintageStory);
				this.SetColorTheme(ColorTheme.VintageStory, null);
			}
			this.Save();
			return true;
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x00049E44 File Offset: 0x00048044
		public static string GetThemeDisplayName(ColorTheme theme)
		{
			string result;
			if (UserConfigurationService.BuiltInThemeNames.TryGetValue(theme, ref result))
			{
				return result;
			}
			return theme.ToString();
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x00049E6F File Offset: 0x0004806F
		public void ResetThemePalette()
		{
			if (this.ColorTheme == ColorTheme.Custom)
			{
				this.ResetCustomThemePaletteToDefaults();
			}
			this.ResetThemePaletteToDefaults();
			this.Save();
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x00049E8C File Offset: 0x0004808C
		public static IReadOnlyDictionary<string, string> GetDefaultThemePalette(ColorTheme theme)
		{
			return new Dictionary<string, string>(UserConfigurationService.GetDefaultPalette(theme), StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x00049E9E File Offset: 0x0004809E
		[return: TupleElementNames(new string[]
		{
			"SortMemberPath",
			"Direction"
		})]
		[return: Nullable(new byte[]
		{
			0,
			2
		})]
		public ValueTuple<string, ListSortDirection> GetModListSortPreference()
		{
			return new ValueTuple<string, ListSortDirection>(this._modsSortMemberPath, this._modsSortDirection);
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x00049EB1 File Offset: 0x000480B1
		[return: TupleElementNames(new string[]
		{
			"SortMemberPath",
			"Direction"
		})]
		[return: Nullable(new byte[]
		{
			0,
			2
		})]
		public ValueTuple<string, ListSortDirection> GetLocalModlistsSortPreference()
		{
			return new ValueTuple<string, ListSortDirection>(this._localModlistsSortMemberPath, this._localModlistsSortDirection);
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x00049EC4 File Offset: 0x000480C4
		[return: TupleElementNames(new string[]
		{
			"SortMemberPath",
			"Direction"
		})]
		[return: Nullable(new byte[]
		{
			0,
			2
		})]
		public ValueTuple<string, ListSortDirection> GetOnlineModlistsSortPreference()
		{
			return new ValueTuple<string, ListSortDirection>(this._onlineModlistsSortMemberPath, this._onlineModlistsSortDirection);
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x00049ED8 File Offset: 0x000480D8
		public bool? GetInstalledColumnVisibility(string columnName)
		{
			string text = UserConfigurationService.NormalizeInstalledColumnKey(columnName);
			if (text == null)
			{
				return default(bool?);
			}
			bool flag;
			if (!this._installedColumnVisibility.TryGetValue(text, ref flag))
			{
				return default(bool?);
			}
			return new bool?(flag);
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x00049F1C File Offset: 0x0004811C
		public void SetInstalledColumnVisibility(string columnName, bool isVisible)
		{
			string text = UserConfigurationService.NormalizeInstalledColumnKey(columnName);
			if (text == null)
			{
				throw new ArgumentException("Column name cannot be empty.", "columnName");
			}
			bool flag;
			if (this._installedColumnVisibility.TryGetValue(text, ref flag) && flag == isVisible)
			{
				return;
			}
			this._installedColumnVisibility[text] = isVisible;
			this.Save();
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x00049F6B File Offset: 0x0004816B
		public string GetConfigurationDirectory()
		{
			string text = Path.GetDirectoryName(this._configurationPath) ?? AppContext.BaseDirectory;
			Directory.CreateDirectory(text);
			return text;
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x00049F88 File Offset: 0x00048188
		public void EnablePersistence()
		{
			if (this._isPersistenceEnabled)
			{
				return;
			}
			this._isPersistenceEnabled = true;
			if (this._hasPendingSave)
			{
				this.PersistConfiguration();
			}
			if (this._hasPendingModConfigPathSave)
			{
				this.PersistModConfigPathHistory();
			}
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x00049FB6 File Offset: 0x000481B6
		[NullableContext(2)]
		public string GetLastSelectedPresetName()
		{
			return this._selectedPresetName;
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x00049FC0 File Offset: 0x000481C0
		[NullableContext(2)]
		public void SetLastSelectedPresetName(string name)
		{
			string text = UserConfigurationService.NormalizePresetName(name);
			if (string.Equals(this._selectedPresetName, text, 5))
			{
				return;
			}
			this._selectedPresetName = text;
			this.Save();
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x00049FF4 File Offset: 0x000481F4
		[NullableContext(2)]
		public bool TryGetModConfigPath(string modId, out string path)
		{
			IReadOnlyList<string> modConfigPaths = this.GetModConfigPaths(modId);
			path = ((modConfigPaths.Count > 0) ? modConfigPaths[0] : null);
			return path != null;
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x0004A028 File Offset: 0x00048228
		public IReadOnlyList<string> GetModConfigPaths([Nullable(2)] string modId)
		{
			if (string.IsNullOrWhiteSpace(modId))
			{
				return Array.Empty<string>();
			}
			string text = modId.Trim();
			List<string> result;
			if (this.ActiveModConfigPaths.TryGetValue(text, ref result))
			{
				return result;
			}
			List<UserConfigurationService.ModConfigPathEntry> list;
			if (this._storedModConfigPaths.TryGetValue(text, ref list) && list != null)
			{
				List<string> list2 = Enumerable.ToList<string>(Enumerable.Select<string, string>(Enumerable.Where<string>(Enumerable.Select<UserConfigurationService.ModConfigPathEntry, string>(list, new Func<UserConfigurationService.ModConfigPathEntry, string>(this.BuildFullConfigPath)), (string combinedPath) => !string.IsNullOrWhiteSpace(combinedPath)), (string path) => path));
				if (list2.Count > 0)
				{
					this.ActiveModConfigPaths[text] = list2;
					return list2;
				}
			}
			return Array.Empty<string>();
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x0004A0EC File Offset: 0x000482EC
		public void SetModConfigPath(string modId, string path, [Nullable(2)] string configName = null)
		{
			this.SetModConfigPaths(modId, new string[]
			{
				path
			}, new string[]
			{
				configName
			});
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x0004A10C File Offset: 0x0004830C
		public void SetModConfigPaths(string modId, IEnumerable<string> paths, [Nullable(2)] IEnumerable<string> configNames = null)
		{
			if (string.IsNullOrWhiteSpace(modId))
			{
				throw new ArgumentException("Mod ID cannot be empty.", "modId");
			}
			List<string> list = Enumerable.ToList<string>(Enumerable.Distinct<string>(this.NormalizeAndValidateConfigPaths(paths), StringComparer.OrdinalIgnoreCase));
			if (list.Count == 0)
			{
				throw new ArgumentException("At least one valid configuration path is required.", "paths");
			}
			List<string> configNames2 = UserConfigurationService.NormalizeConfigNames(configNames, list);
			string text = modId.Trim();
			this.ActiveModConfigPaths[text] = list;
			this.UpdatePersistentModConfigPaths(text, list, configNames2, false);
			this.Save();
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x0004A18C File Offset: 0x0004838C
		public void AddModConfigPath(string modId, string path, [Nullable(2)] string configName = null)
		{
			if (string.IsNullOrWhiteSpace(modId))
			{
				throw new ArgumentException("Mod ID cannot be empty.", "modId");
			}
			string normalizedPath = this.NormalizeAndValidateConfigPath(path2);
			if (UserConfigurationService.NormalizeConfigName(configName) == null)
			{
				UserConfigurationService.ExtractFileName(normalizedPath);
			}
			string text = modId.Trim();
			List<string> list;
			if (!this.ActiveModConfigPaths.TryGetValue(text, ref list))
			{
				list = new List<string>();
				this.ActiveModConfigPaths[text] = list;
			}
			if (!Enumerable.Contains<string>(list, normalizedPath, StringComparer.OrdinalIgnoreCase))
			{
				list.Add(normalizedPath);
			}
			List<string> configNames = Enumerable.ToList<string>(Enumerable.Cast<string>(Enumerable.Select<string, string>(list, (string path) => UserConfigurationService.NormalizeConfigName(string.Equals(path, normalizedPath, UserConfigurationService.PathComparison) ? configName : null) ?? UserConfigurationService.ExtractFileName(path))));
			this.UpdatePersistentModConfigPaths(text, list, configNames, true);
			this.Save();
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x0004A25C File Offset: 0x0004845C
		[NullableContext(2)]
		public void RemoveModConfigPath(string modId, bool preserveHistory = false)
		{
			if (string.IsNullOrWhiteSpace(modId))
			{
				return;
			}
			string text = modId.Trim();
			bool flag = this.ActiveModConfigPaths.Remove(text);
			bool flag2 = false;
			if (!preserveHistory)
			{
				flag2 = this._storedModConfigPaths.Remove(text);
			}
			if (flag)
			{
				this.Save();
			}
			if (flag2)
			{
				this.SaveModConfigPathHistory();
			}
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x0004A2A8 File Offset: 0x000484A8
		public void RemoveModConfigPath(string modId, string path, bool preserveHistory = false)
		{
			if (string.IsNullOrWhiteSpace(modId) || string.IsNullOrWhiteSpace(path))
			{
				return;
			}
			string text = modId.Trim();
			List<string> list;
			if (this.ActiveModConfigPaths.TryGetValue(text, ref list))
			{
				list.RemoveAll((string candidate) => string.Equals(candidate, path, UserConfigurationService.PathComparison));
				if (list.Count == 0)
				{
					this.ActiveModConfigPaths.Remove(text);
				}
				this.Save();
			}
			List<UserConfigurationService.ModConfigPathEntry> list2;
			if (!preserveHistory && this._storedModConfigPaths.TryGetValue(text, ref list2))
			{
				list2.RemoveAll((UserConfigurationService.ModConfigPathEntry entry) => string.Equals(this.BuildFullConfigPath(entry), path, UserConfigurationService.PathComparison));
				if (list2.Count == 0)
				{
					this._storedModConfigPaths.Remove(text);
				}
				this.SaveModConfigPathHistory();
			}
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x0004A365 File Offset: 0x00048565
		private IEnumerable<string> NormalizeAndValidateConfigPaths(IEnumerable<string> paths)
		{
			UserConfigurationService.<NormalizeAndValidateConfigPaths>d__361 <NormalizeAndValidateConfigPaths>d__ = new UserConfigurationService.<NormalizeAndValidateConfigPaths>d__361(-2);
			<NormalizeAndValidateConfigPaths>d__.<>4__this = this;
			<NormalizeAndValidateConfigPaths>d__.<>3__paths = paths;
			return <NormalizeAndValidateConfigPaths>d__;
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x0004A37C File Offset: 0x0004857C
		private string NormalizeAndValidateConfigPath(string path)
		{
			string text = UserConfigurationService.NormalizePath(path);
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new ArgumentException("The configuration path is invalid.", "path");
			}
			string activeModConfigDirectory = this.GetActiveModConfigDirectory();
			if (!string.IsNullOrWhiteSpace(activeModConfigDirectory) && !UserConfigurationService.IsPathInDirectory(text, activeModConfigDirectory))
			{
				throw new ArgumentException("The configuration path must be inside the ModConfig directory.", "path");
			}
			return text;
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x0004A3D4 File Offset: 0x000485D4
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		private static List<string> NormalizeConfigNames([Nullable(2)] IEnumerable<string> configNames, IReadOnlyList<string> paths)
		{
			List<string> list = new List<string>(paths.Count);
			List<string> list2 = Enumerable.ToList<string>(configNames ?? Array.Empty<string>());
			for (int i = 0; i < paths.Count; i++)
			{
				string text = UserConfigurationService.NormalizeConfigName((i < list2.Count) ? list2[i] : null) ?? UserConfigurationService.ExtractFileName(paths[i]);
				list.Add(text);
			}
			return list;
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x0004A43F File Offset: 0x0004863F
		public void SetCompactViewMode(bool isCompact)
		{
			if (this.IsCompactView == isCompact)
			{
				return;
			}
			this.IsCompactView = isCompact;
			this.Save();
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x0004A458 File Offset: 0x00048658
		public void SetModDbDesignViewMode(bool useModDbDesignView)
		{
			if (this.UseModDbDesignView == useModDbDesignView)
			{
				return;
			}
			this.UseModDbDesignView = useModDbDesignView;
			this.Save();
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x0004A474 File Offset: 0x00048674
		public void SetColorTheme(ColorTheme theme, [Nullable(new byte[]
		{
			2,
			1,
			1
		})] IReadOnlyDictionary<string, string> paletteOverride = null)
		{
			bool flag = false;
			if (this.ColorTheme != theme)
			{
				this.ColorTheme = theme;
				this._currentThemeName = UserConfigurationService.GetThemeDisplayName(theme);
				this.ResetThemePaletteToDefaults();
				flag = true;
			}
			if (paletteOverride != null)
			{
				flag |= this.ApplyThemePaletteOverride(paletteOverride);
				if (theme == ColorTheme.Custom && string.IsNullOrWhiteSpace(this._currentThemeName))
				{
					this._currentThemeName = UserConfigurationService.GetThemeDisplayName(ColorTheme.Custom);
				}
			}
			if (flag)
			{
				this.Save();
			}
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x0004A4DA File Offset: 0x000486DA
		public void SetEnableServerOptions(bool enableServerOptions)
		{
			if (this.EnableServerOptions == enableServerOptions)
			{
				return;
			}
			this.EnableServerOptions = enableServerOptions;
			this.Save();
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x0004A4F3 File Offset: 0x000486F3
		public void SetLogModUpdates(bool logModUpdates)
		{
			if (this.LogModUpdates == logModUpdates)
			{
				return;
			}
			this.LogModUpdates = logModUpdates;
			this.Save();
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x0004A50C File Offset: 0x0004870C
		public void SetLogModInstalls(bool logModInstalls)
		{
			if (this.LogModInstalls == logModInstalls)
			{
				return;
			}
			this.LogModInstalls = logModInstalls;
			this.Save();
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x0004A525 File Offset: 0x00048725
		public void SetLogModDeletions(bool logModDeletions)
		{
			if (this.LogModDeletions == logModDeletions)
			{
				return;
			}
			this.LogModDeletions = logModDeletions;
			this.Save();
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x0004A53E File Offset: 0x0004873E
		public void SetLogAppLaunchAndExit(bool logAppLaunchAndExit)
		{
			if (this.LogAppLaunchAndExit == logAppLaunchAndExit)
			{
				return;
			}
			this.LogAppLaunchAndExit = logAppLaunchAndExit;
			this.Save();
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x0004A557 File Offset: 0x00048757
		public void SetLogErrorsAndExceptions(bool logErrorsAndExceptions)
		{
			if (this.LogErrorsAndExceptions == logErrorsAndExceptions)
			{
				return;
			}
			this.LogErrorsAndExceptions = logErrorsAndExceptions;
			this.Save();
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x0004A570 File Offset: 0x00048770
		public void SetAutomaticDataBackupsEnabled(bool isEnabled)
		{
			if (this.AutomaticDataBackupsEnabled == isEnabled)
			{
				return;
			}
			this.AutomaticDataBackupsEnabled = isEnabled;
			this.Save();
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x0004A589 File Offset: 0x00048789
		public void SetAutomaticDataBackupsWarningAcknowledged(bool acknowledged)
		{
			if (this.AutomaticDataBackupsWarningAcknowledged == acknowledged)
			{
				return;
			}
			this.AutomaticDataBackupsWarningAcknowledged = acknowledged;
			this.Save();
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x0004A5A2 File Offset: 0x000487A2
		public void SetCloseManagerAfterGameLaunch(bool isEnabled)
		{
			if (this.CloseManagerAfterGameLaunch == isEnabled)
			{
				return;
			}
			this.CloseManagerAfterGameLaunch = isEnabled;
			this.Save();
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x0004A5BC File Offset: 0x000487BC
		[NullableContext(2)]
		public void SetCustomDataBackupLocation(string path)
		{
			string text = string.IsNullOrWhiteSpace(path) ? null : UserConfigurationService.NormalizePath(path);
			if (string.Equals(this.CustomDataBackupLocation, text, 5))
			{
				return;
			}
			this.CustomDataBackupLocation = text;
			this.Save();
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x0004A5F8 File Offset: 0x000487F8
		public void ClearCustomDataBackupLocation()
		{
			if (this.CustomDataBackupLocation == null)
			{
				return;
			}
			this.CustomDataBackupLocation = null;
			this.Save();
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x0004A610 File Offset: 0x00048810
		public void SetMaximumDataBackups(int maximumBackups)
		{
			int num = UserConfigurationService.NormalizeMaximumDataBackups(new int?(maximumBackups));
			if (this.MaximumDataBackups == num)
			{
				return;
			}
			this.MaximumDataBackups = num;
			this.Save();
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x0004A640 File Offset: 0x00048840
		public void SetModlistAutoLoadBehavior(ModlistAutoLoadBehavior behavior)
		{
			if (this.ModlistAutoLoadBehavior == behavior)
			{
				return;
			}
			this.ModlistAutoLoadBehavior = behavior;
			this.Save();
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x0004A659 File Offset: 0x00048859
		public void SetPreferredModlistsTab(ModlistsTabSelection selection)
		{
			if (this.PreferredModlistsTab == selection)
			{
				return;
			}
			this.PreferredModlistsTab = selection;
			this.Save();
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x0004A672 File Offset: 0x00048872
		public void SetCacheAllVersionsLocally(bool cacheAllVersionsLocally)
		{
			if (this.CacheAllVersionsLocally == cacheAllVersionsLocally)
			{
				return;
			}
			this.CacheAllVersionsLocally = cacheAllVersionsLocally;
			this.Save();
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x0004A68B File Offset: 0x0004888B
		public void SetModDatabaseAutoLoadMode(ModDatabaseAutoLoadMode mode)
		{
			if (this.ModDatabaseAutoLoadMode == mode)
			{
				return;
			}
			this.ModDatabaseAutoLoadMode = mode;
			this.Save();
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x0004A6A4 File Offset: 0x000488A4
		public void SetModDatabaseSearchResultLimit(int limit)
		{
			int num = UserConfigurationService.NormalizeModDatabaseSearchResultLimit(new int?(limit));
			if (this.ModDatabaseSearchResultLimit == num)
			{
				return;
			}
			this.ModDatabaseSearchResultLimit = num;
			this.Save();
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x0004A6D4 File Offset: 0x000488D4
		[NullableContext(2)]
		public void SetModListSortPreference(string sortMemberPath, ListSortDirection direction)
		{
			string text = string.IsNullOrWhiteSpace(sortMemberPath) ? null : sortMemberPath.Trim();
			if (string.Equals(this._modsSortMemberPath, text, 4) && this._modsSortDirection == direction)
			{
				return;
			}
			this._modsSortMemberPath = text;
			this._modsSortDirection = direction;
			this.Save();
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x0004A720 File Offset: 0x00048920
		public void SetModBrowserOrderBy(string orderBy)
		{
			string text = string.IsNullOrWhiteSpace(orderBy) ? "follows" : orderBy.Trim();
			if (string.Equals(this.ModBrowserOrderBy, text, 4))
			{
				return;
			}
			this.ModBrowserOrderBy = text;
			this.Save();
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x0004A760 File Offset: 0x00048960
		public void SetModBrowserOrderByDirection(string direction)
		{
			string text = string.IsNullOrWhiteSpace(direction) ? "desc" : direction.Trim();
			if (string.Equals(this.ModBrowserOrderByDirection, text, 4))
			{
				return;
			}
			this.ModBrowserOrderByDirection = text;
			this.Save();
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x0004A7A0 File Offset: 0x000489A0
		public void SetModBrowserSelectedSide(string side)
		{
			string text = string.IsNullOrWhiteSpace(side) ? "any" : side.Trim();
			if (string.Equals(this.ModBrowserSelectedSide, text, 4))
			{
				return;
			}
			this.ModBrowserSelectedSide = text;
			this.Save();
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x0004A7E0 File Offset: 0x000489E0
		public void SetModBrowserSelectedInstalledFilter(string filter)
		{
			string text = string.IsNullOrWhiteSpace(filter) ? "all" : filter.Trim();
			if (string.Equals(this.ModBrowserSelectedInstalledFilter, text, 4))
			{
				return;
			}
			this.ModBrowserSelectedInstalledFilter = text;
			this.Save();
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x0004A820 File Offset: 0x00048A20
		public void SetModBrowserOnlyFavorites(bool onlyFavorites)
		{
			if (this.ModBrowserOnlyFavorites == onlyFavorites)
			{
				return;
			}
			this.ModBrowserOnlyFavorites = onlyFavorites;
			this.Save();
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x0004A839 File Offset: 0x00048A39
		public void SetModBrowserRelevantSearch(bool enabled)
		{
			if (this.ModBrowserRelevantSearch == enabled)
			{
				return;
			}
			this.ModBrowserRelevantSearch = enabled;
			this.Save();
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x0004A854 File Offset: 0x00048A54
		public void SetModBrowserFavoriteModIds(IEnumerable<int> favoriteIds)
		{
			List<int> list = ((favoriteIds != null) ? Enumerable.ToList<int>(favoriteIds) : null) ?? new List<int>();
			if (this.ModBrowserFavoriteModIds.Count == list.Count && Enumerable.SequenceEqual<int>(this.ModBrowserFavoriteModIds, list))
			{
				return;
			}
			this.ModBrowserFavoriteModIds = list;
			this.Save();
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x0004A8A8 File Offset: 0x00048AA8
		public void SetModBrowserSelectedVersionIds(IEnumerable<string> versionIds)
		{
			List<string> list;
			if (versionIds == null)
			{
				list = null;
			}
			else
			{
				list = Enumerable.ToList<string>(Enumerable.Select<string, string>(Enumerable.Where<string>(versionIds, (string id) => !string.IsNullOrWhiteSpace(id)), (string id) => id.Trim()));
			}
			List<string> list2 = list ?? new List<string>();
			if (this.ModBrowserSelectedVersionIds.Count == list2.Count && Enumerable.SequenceEqual<string>(this.ModBrowserSelectedVersionIds, list2))
			{
				return;
			}
			this.ModBrowserSelectedVersionIds = list2;
			this.Save();
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x0004A944 File Offset: 0x00048B44
		public void SetModBrowserSelectedTagIds(IEnumerable<int> tagIds)
		{
			List<int> list = ((tagIds != null) ? Enumerable.ToList<int>(tagIds) : null) ?? new List<int>();
			if (this.ModBrowserSelectedTagIds.Count == list.Count && Enumerable.SequenceEqual<int>(this.ModBrowserSelectedTagIds, list))
			{
				return;
			}
			this.ModBrowserSelectedTagIds = list;
			this.Save();
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x0004A998 File Offset: 0x00048B98
		public void SetWindowDimensions(double width, double height)
		{
			double? windowWidth = UserConfigurationService.NormalizeWindowDimension(new double?(width));
			double? windowHeight = UserConfigurationService.NormalizeWindowDimension(new double?(height));
			if (windowWidth == null || windowHeight == null)
			{
				return;
			}
			bool flag = this._windowWidth == null || Math.Abs(this._windowWidth.Value - windowWidth.Value) > 0.1;
			bool flag2 = this._windowHeight == null || Math.Abs(this._windowHeight.Value - windowHeight.Value) > 0.1;
			if (!flag && !flag2)
			{
				return;
			}
			this._windowWidth = windowWidth;
			this._windowHeight = windowHeight;
			this.Save();
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x0004AA54 File Offset: 0x00048C54
		public void SetWindowPosition(double left, double top)
		{
			double? windowLeft = UserConfigurationService.NormalizeWindowCoordinate(new double?(left));
			double? windowTop = UserConfigurationService.NormalizeWindowCoordinate(new double?(top));
			if (windowLeft == null || windowTop == null)
			{
				return;
			}
			bool flag = this._windowLeft == null || Math.Abs(this._windowLeft.Value - windowLeft.Value) > 0.1;
			bool flag2 = this._windowTop == null || Math.Abs(this._windowTop.Value - windowTop.Value) > 0.1;
			if (!flag && !flag2)
			{
				return;
			}
			this._windowLeft = windowLeft;
			this._windowTop = windowTop;
			this.Save();
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x0004AB10 File Offset: 0x00048D10
		public void SetModInfoPanelPosition(double left, double top)
		{
			double? modInfoPanelLeft = UserConfigurationService.NormalizeModInfoCoordinate(new double?(left));
			double? modInfoPanelTop = UserConfigurationService.NormalizeModInfoCoordinate(new double?(top));
			if (modInfoPanelLeft == null || modInfoPanelTop == null)
			{
				return;
			}
			bool flag = this._modInfoPanelLeft == null || Math.Abs(this._modInfoPanelLeft.Value - modInfoPanelLeft.Value) > 0.1;
			bool flag2 = this._modInfoPanelTop == null || Math.Abs(this._modInfoPanelTop.Value - modInfoPanelTop.Value) > 0.1;
			if (!flag && !flag2)
			{
				return;
			}
			this._modInfoPanelLeft = modInfoPanelLeft;
			this._modInfoPanelTop = modInfoPanelTop;
			this.Save();
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x0004ABC9 File Offset: 0x00048DC9
		public void SetDataDirectory(string path)
		{
			this.ActiveProfile.DataDirectory = UserConfigurationService.NormalizePath(path);
			this.ActiveProfile.RequiresDataDirectorySelection = false;
			UserConfigurationService.EnsureModConfigDirectoryExists(this.ActiveProfile.DataDirectory);
			this.RefreshActiveModConfigPathsFromHistory();
			this.Save();
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x0004AC04 File Offset: 0x00048E04
		public void ClearDataDirectory()
		{
			if (this.ActiveProfile.DataDirectory == null)
			{
				this.ActiveProfile.RequiresDataDirectorySelection = true;
				return;
			}
			this.ActiveProfile.DataDirectory = null;
			this.ActiveProfile.RequiresDataDirectorySelection = true;
			this.Save();
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x0004AC3E File Offset: 0x00048E3E
		public void SetGameDirectory(string path)
		{
			this.ActiveProfile.GameDirectory = UserConfigurationService.NormalizePath(path);
			this.ActiveProfile.RequiresGameDirectorySelection = false;
			this.Save();
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x0004AC64 File Offset: 0x00048E64
		[NullableContext(2)]
		public void SetTargetGameVersionOverride(string value)
		{
			string text = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
			if (string.Equals(this.ActiveProfile.TargetGameVersionOverride, text, 4))
			{
				return;
			}
			this.ActiveProfile.TargetGameVersionOverride = text;
			this.Save();
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x0004ACAA File Offset: 0x00048EAA
		public void ClearGameDirectory()
		{
			if (this.ActiveProfile.GameDirectory == null)
			{
				this.ActiveProfile.RequiresGameDirectorySelection = true;
				return;
			}
			this.ActiveProfile.GameDirectory = null;
			this.ActiveProfile.RequiresGameDirectorySelection = true;
			this.Save();
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x0004ACE4 File Offset: 0x00048EE4
		public void SetCustomShortcutPath(string path)
		{
			string text = UserConfigurationService.NormalizePath(path);
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new ArgumentException("The shortcut path is invalid.", "path");
			}
			if (string.Equals(this.ActiveProfile.CustomShortcutPath, text, 5))
			{
				return;
			}
			this.ActiveProfile.CustomShortcutPath = text;
			this.Save();
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x0004AD37 File Offset: 0x00048F37
		public void ClearCustomShortcutPath()
		{
			if (this.ActiveProfile.CustomShortcutPath == null)
			{
				return;
			}
			this.ActiveProfile.CustomShortcutPath = null;
			this.Save();
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x0004AD59 File Offset: 0x00048F59
		public void SetDisableAutoRefresh(bool disable)
		{
			if (this.DisableAutoRefresh == disable)
			{
				return;
			}
			this.DisableAutoRefresh = disable;
			this.Save();
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x0004AD72 File Offset: 0x00048F72
		public void SetDisableAutoRefreshWarningAcknowledged(bool acknowledged)
		{
			if (this.DisableAutoRefreshWarningAcknowledged == acknowledged)
			{
				return;
			}
			this.DisableAutoRefreshWarningAcknowledged = acknowledged;
			this.Save();
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x0004AD8B File Offset: 0x00048F8B
		public void SetGameProfileCreationWarningAcknowledged(bool acknowledged)
		{
			if (this.GameProfileCreationWarningAcknowledged == acknowledged)
			{
				return;
			}
			this.GameProfileCreationWarningAcknowledged = acknowledged;
			this.Save();
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x0004ADA4 File Offset: 0x00048FA4
		public void SetDisableInternetAccess(bool disable)
		{
			if (this.DisableInternetAccess == disable)
			{
				return;
			}
			this.DisableInternetAccess = disable;
			this.Save();
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x0004ADBD File Offset: 0x00048FBD
		public void SetSuppressModlistSavePrompt(bool suppress)
		{
			if (this.SuppressModlistSavePrompt == suppress)
			{
				return;
			}
			this.SuppressModlistSavePrompt = suppress;
			this.Save();
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x0004ADD8 File Offset: 0x00048FD8
		public void SetSuppressRefreshCachePrompt(bool suppress)
		{
			string text = suppress ? (UserConfigurationService.NormalizeVersion(this.ModManagerVersion) ?? this.ModManagerVersion) : null;
			if (this._suppressRefreshCachePrompt == suppress && string.Equals(this._suppressRefreshCachePromptVersion, text, 5))
			{
				return;
			}
			this._suppressRefreshCachePrompt = suppress;
			this._suppressRefreshCachePromptVersion = text;
			this.Save();
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x0004AE30 File Offset: 0x00049030
		[NullableContext(2)]
		public void SetCloudUploaderName(string name)
		{
			string text = UserConfigurationService.NormalizeUploaderName(name);
			if (string.Equals(this.CloudUploaderName, text, 4))
			{
				return;
			}
			this.CloudUploaderName = text;
			this.Save();
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x0004AE61 File Offset: 0x00049061
		public void SetExcludeInstalledModDatabaseResults(bool exclude)
		{
			if (this.ExcludeInstalledModDatabaseResults == exclude)
			{
				return;
			}
			this.ExcludeInstalledModDatabaseResults = exclude;
			this.Save();
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x0004AE7A File Offset: 0x0004907A
		public void SetOnlyShowCompatibleModDatabaseResults(bool onlyCompatible)
		{
			if (this.OnlyShowCompatibleModDatabaseResults == onlyCompatible)
			{
				return;
			}
			this.OnlyShowCompatibleModDatabaseResults = onlyCompatible;
			this.Save();
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x0004AE93 File Offset: 0x00049093
		public void SetRequireExactVsVersionMatch(bool requireExact)
		{
			if (this.RequireExactVsVersionMatch == requireExact)
			{
				return;
			}
			this.RequireExactVsVersionMatch = requireExact;
			this.Save();
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x0004AEAC File Offset: 0x000490AC
		public void SetUseFasterThumbnails(bool useFasterThumbnails)
		{
			if (this.UseFasterThumbnails == useFasterThumbnails)
			{
				return;
			}
			this.UseFasterThumbnails = useFasterThumbnails;
			this.Save();
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0004AEC5 File Offset: 0x000490C5
		public void SetDisableHoverEffects(bool disableHoverEffects)
		{
			if (this.DisableHoverEffects == disableHoverEffects)
			{
				return;
			}
			this.DisableHoverEffects = disableHoverEffects;
			this.Save();
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0004AEE0 File Offset: 0x000490E0
		[NullableContext(2)]
		public bool IsModExcludedFromBulkUpdates(string modId)
		{
			string text = UserConfigurationService.NormalizeModId(modId);
			bool flag;
			return text != null && (this.ActiveBulkUpdateModExclusions.TryGetValue(text, ref flag) && flag);
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x0004AF0C File Offset: 0x0004910C
		[NullableContext(2)]
		public void SetModExcludedFromBulkUpdates(string modId, bool isExcluded)
		{
			string text = UserConfigurationService.NormalizeModId(modId);
			if (text == null)
			{
				return;
			}
			if (isExcluded)
			{
				bool flag;
				if (this.ActiveBulkUpdateModExclusions.TryGetValue(text, ref flag) && flag)
				{
					return;
				}
				this.ActiveBulkUpdateModExclusions[text] = true;
				this.Save();
				return;
			}
			else
			{
				if (!this.ActiveBulkUpdateModExclusions.Remove(text))
				{
					return;
				}
				this.Save();
				return;
			}
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x0004AF64 File Offset: 0x00049164
		[NullableContext(2)]
		public void SkipModVersion(string modId, string version)
		{
			string text = UserConfigurationService.NormalizeModId(modId);
			if (text == null)
			{
				return;
			}
			string text2 = UserConfigurationService.NormalizeVersion(version);
			if (text2 == null)
			{
				return;
			}
			string text3;
			if (this.ActiveSkippedModVersions.TryGetValue(text, ref text3) && string.Equals(text3, text2, 5))
			{
				return;
			}
			this.ActiveSkippedModVersions[text] = text2;
			this.Save();
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x0004AFB8 File Offset: 0x000491B8
		[NullableContext(2)]
		public bool ShouldSkipModVersion(string modId, string version)
		{
			string text = UserConfigurationService.NormalizeModId(modId);
			if (text == null)
			{
				return false;
			}
			string text2;
			if (!this.ActiveSkippedModVersions.TryGetValue(text, ref text2))
			{
				return false;
			}
			string text3 = UserConfigurationService.NormalizeVersion(version);
			if ((text3 == null || !string.Equals(text2, text3, 5)) && this.ActiveSkippedModVersions.Remove(text))
			{
				this.Save();
			}
			return string.Equals(text2, text3, 5);
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0004B014 File Offset: 0x00049214
		[NullableContext(2)]
		public bool IsDependencyWarningOverridden(string modId, string version)
		{
			string text = UserConfigurationService.NormalizeModId(modId);
			if (text == null)
			{
				return false;
			}
			string text2;
			if (!this.ActiveDependencyWarningOverrides.TryGetValue(text, ref text2))
			{
				return false;
			}
			string text3 = UserConfigurationService.NormalizeVersion(version);
			if (text3 == null || !string.Equals(text2, text3, 5))
			{
				if (this.ActiveDependencyWarningOverrides.Remove(text))
				{
					this.Save();
				}
				return false;
			}
			return true;
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x0004B06C File Offset: 0x0004926C
		[NullableContext(2)]
		public void SetDependencyWarningOverride(string modId, string version, bool isOverridden)
		{
			string text = UserConfigurationService.NormalizeModId(modId);
			if (text == null)
			{
				return;
			}
			if (!isOverridden)
			{
				if (this.ActiveDependencyWarningOverrides.Remove(text))
				{
					this.Save();
				}
				return;
			}
			string text2 = UserConfigurationService.NormalizeVersion(version);
			if (text2 == null)
			{
				return;
			}
			string text3;
			if (this.ActiveDependencyWarningOverrides.TryGetValue(text, ref text3) && string.Equals(text3, text2, 5))
			{
				return;
			}
			this.ActiveDependencyWarningOverrides[text] = text2;
			this.Save();
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x0004B0D8 File Offset: 0x000492D8
		[NullableContext(2)]
		public void SetLocalModlistsSortPreference(string sortMemberPath, ListSortDirection direction)
		{
			string text = UserConfigurationService.NormalizeOptionalSortMemberPath(sortMemberPath);
			if (string.Equals(this._localModlistsSortMemberPath, text, 4) && this._localModlistsSortDirection == direction)
			{
				return;
			}
			this._localModlistsSortMemberPath = text;
			this._localModlistsSortDirection = direction;
			this.Save();
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x0004B11C File Offset: 0x0004931C
		[NullableContext(2)]
		public void SetOnlineModlistsSortPreference(string sortMemberPath, ListSortDirection direction)
		{
			string text = UserConfigurationService.NormalizeOptionalSortMemberPath(sortMemberPath);
			if (string.Equals(this._onlineModlistsSortMemberPath, text, 4) && this._onlineModlistsSortDirection == direction)
			{
				return;
			}
			this._onlineModlistsSortMemberPath = text;
			this._onlineModlistsSortDirection = direction;
			this.Save();
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x0004B15D File Offset: 0x0004935D
		public void SetModInfoPanePlacement(ModInfoPanePlacement placement)
		{
			if (this.ModInfoPanePlacement == placement)
			{
				return;
			}
			this.ModInfoPanePlacement = placement;
			this.Save();
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x0004B176 File Offset: 0x00049376
		public void SetCacheModDatabaseResults(bool cacheModDatabaseResults)
		{
			if (this.CacheModDatabaseResults == cacheModDatabaseResults)
			{
				return;
			}
			this.CacheModDatabaseResults = cacheModDatabaseResults;
			this.Save();
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x0004B190 File Offset: 0x00049390
		[NullableContext(2)]
		public void ClearDependencyWarningOverride(string modId)
		{
			string text = UserConfigurationService.NormalizeModId(modId);
			if (text == null)
			{
				return;
			}
			if (this.ActiveDependencyWarningOverrides.Remove(text))
			{
				this.Save();
			}
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x0004B1BC File Offset: 0x000493BC
		public void PruneDependencyWarningOverrides([TupleElementNames(new string[]
		{
			"ModId",
			"Version"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1,
			2
		})] IEnumerable<ValueTuple<string, string>> installedMods)
		{
			if (this.ActiveDependencyWarningOverrides.Count == 0)
			{
				return;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			foreach (ValueTuple<string, string> valueTuple in installedMods)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				string text = UserConfigurationService.NormalizeModId(item);
				string text2 = UserConfigurationService.NormalizeVersion(item2);
				if (text != null && text2 != null)
				{
					dictionary[text] = text2;
				}
			}
			bool flag = false;
			foreach (KeyValuePair<string, string> keyValuePair in Enumerable.ToArray<KeyValuePair<string, string>>(this.ActiveDependencyWarningOverrides))
			{
				string text3;
				if (!dictionary.TryGetValue(keyValuePair.Key, ref text3) || !string.Equals(text3, keyValuePair.Value, 5))
				{
					this.ActiveDependencyWarningOverrides.Remove(keyValuePair.Key);
					flag = true;
				}
			}
			if (flag)
			{
				this.Save();
			}
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x0004B2B0 File Offset: 0x000494B0
		private void Load()
		{
			this._gameProfiles.Clear();
			UserConfigurationService.GameProfileState gameProfileState = new UserConfigurationService.GameProfileState("Default");
			this._gameProfiles[gameProfileState.Name] = gameProfileState;
			this.ActiveProfile = gameProfileState;
			this.ResetCustomThemePaletteToDefaults();
			this.ColorTheme = ColorTheme.VintageStory;
			this.ResetThemePaletteToDefaults();
			this._selectedPresetName = null;
			this.GameProfileCreationWarningAcknowledged = false;
			this.AutomaticDataBackupsWarningAcknowledged = false;
			this.CloseManagerAfterGameLaunch = false;
			try
			{
				if (!File.Exists(this._configurationPath))
				{
					return;
				}
				using (FileStream fileStream = File.OpenRead(this._configurationPath))
				{
					JsonObject jsonObject = JsonNode.Parse(fileStream, default(JsonNodeOptions?), default(JsonDocumentOptions)) as JsonObject;
					if (jsonObject == null)
					{
						return;
					}
					string configurationVersion = UserConfigurationService.NormalizeVersion(UserConfigurationService.GetOptionalString(jsonObject["configurationVersion"]));
					string modManagerVersion = UserConfigurationService.NormalizeVersion(UserConfigurationService.GetOptionalString(jsonObject["modManagerVersion"]));
					this.InitializeVersionMetadata(configurationVersion, modManagerVersion);
					string dataDirectory = UserConfigurationService.NormalizePath(UserConfigurationService.GetOptionalString(jsonObject["dataDirectory"]));
					string gameDirectory = UserConfigurationService.NormalizePath(UserConfigurationService.GetOptionalString(jsonObject["gameDirectory"]));
					gameProfileState.DataDirectory = dataDirectory;
					gameProfileState.GameDirectory = gameDirectory;
					JsonNode jsonNode = jsonObject["isCompactView"];
					this.IsCompactView = ((jsonNode != null) ? jsonNode.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					JsonNode jsonNode2 = jsonObject["useModDbDesignView"];
					this.UseModDbDesignView = ((jsonNode2 != null) ? jsonNode2.GetValue<bool?>() : default(bool?)).GetValueOrDefault(true);
					JsonNode jsonNode3 = jsonObject["cacheAllVersionsLocally"];
					this.CacheAllVersionsLocally = ((jsonNode3 != null) ? jsonNode3.GetValue<bool?>() : default(bool?)).GetValueOrDefault(true);
					JsonNode jsonNode4 = jsonObject["cacheModDatabaseResults"];
					this.CacheModDatabaseResults = ((jsonNode4 != null) ? jsonNode4.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					JsonNode jsonNode5 = jsonObject["disableAutoRefresh"];
					this.DisableAutoRefresh = ((jsonNode5 != null) ? jsonNode5.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					JsonNode jsonNode6 = jsonObject["disableAutoRefreshWarningAcknowledged"];
					this.DisableAutoRefreshWarningAcknowledged = ((jsonNode6 != null) ? jsonNode6.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					JsonNode jsonNode7 = jsonObject["disableInternetAccess"];
					this.DisableInternetAccess = ((jsonNode7 != null) ? jsonNode7.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					JsonNode jsonNode8 = jsonObject["modUsageTrackingDisabled"];
					this._isModUsageTrackingDisabled = ((jsonNode8 != null) ? jsonNode8.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					JsonNode jsonNode9 = jsonObject["enableServerOptions"];
					this.EnableServerOptions = ((jsonNode9 != null) ? jsonNode9.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					JsonNode jsonNode10 = jsonObject["logModUpdates"];
					this.LogModUpdates = ((jsonNode10 != null) ? jsonNode10.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					JsonNode jsonNode11 = jsonObject["logModInstalls"];
					this.LogModInstalls = ((jsonNode11 != null) ? jsonNode11.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					JsonNode jsonNode12 = jsonObject["logModDeletions"];
					this.LogModDeletions = ((jsonNode12 != null) ? jsonNode12.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					JsonNode jsonNode13 = jsonObject["logAppLaunchAndExit"];
					this.LogAppLaunchAndExit = ((jsonNode13 != null) ? jsonNode13.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					JsonNode jsonNode14 = jsonObject["logErrorsAndExceptions"];
					this.LogErrorsAndExceptions = ((jsonNode14 != null) ? jsonNode14.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					JsonNode jsonNode15 = jsonObject["automaticDataBackupsEnabled"];
					this.AutomaticDataBackupsEnabled = ((jsonNode15 != null) ? jsonNode15.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					JsonNode jsonNode16 = jsonObject["automaticDataBackupsWarningAcknowledged"];
					this.AutomaticDataBackupsWarningAcknowledged = ((jsonNode16 != null) ? jsonNode16.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					JsonNode jsonNode17 = jsonObject["closeManagerAfterGameLaunch"];
					this.CloseManagerAfterGameLaunch = ((jsonNode17 != null) ? jsonNode17.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					this.CustomDataBackupLocation = UserConfigurationService.NormalizePath(UserConfigurationService.GetOptionalString(jsonObject["customDataBackupLocation"]));
					JsonNode jsonNode18 = jsonObject["maximumDataBackups"];
					this.MaximumDataBackups = UserConfigurationService.NormalizeMaximumDataBackups((jsonNode18 != null) ? jsonNode18.GetValue<int?>() : default(int?));
					JsonNode jsonNode19 = jsonObject["suppressModlistSavePrompt"];
					this.SuppressModlistSavePrompt = ((jsonNode19 != null) ? jsonNode19.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					JsonNode jsonNode20 = jsonObject["suppressRefreshCachePrompt"];
					this._suppressRefreshCachePrompt = ((jsonNode20 != null) ? jsonNode20.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					this._suppressRefreshCachePromptVersion = UserConfigurationService.NormalizeVersion(UserConfigurationService.GetOptionalString(jsonObject["suppressRefreshCachePromptVersion"]));
					JsonNode jsonNode21 = jsonObject["gameProfileCreationWarningAcknowledged"];
					this.GameProfileCreationWarningAcknowledged = ((jsonNode21 != null) ? jsonNode21.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					if (this._suppressRefreshCachePrompt)
					{
						if (this._suppressRefreshCachePromptVersion == null)
						{
							this._suppressRefreshCachePromptVersion = (this.PreviousModManagerVersion ?? this.ModManagerVersion);
							this._hasPendingSave = true;
						}
					}
					else
					{
						this._suppressRefreshCachePromptVersion = null;
					}
					string optionalString = UserConfigurationService.GetOptionalString(jsonObject["colorTheme"]);
					JsonNode jsonNode22 = jsonObject["useDarkVsMode"];
					bool? flag = (jsonNode22 != null) ? jsonNode22.GetValue<bool?>() : default(bool?);
					ColorTheme colorTheme;
					if (!string.IsNullOrWhiteSpace(optionalString) && Enum.TryParse<ColorTheme>(optionalString.Trim(), true, ref colorTheme))
					{
						this.ColorTheme = colorTheme;
					}
					else
					{
						this.ColorTheme = ((flag != null && !flag.Value) ? ColorTheme.Light : ColorTheme.VintageStory);
						this._hasPendingSave = true;
					}
					this._currentThemeName = (UserConfigurationService.NormalizeThemeName(UserConfigurationService.GetOptionalString(jsonObject["currentThemeName"])) ?? UserConfigurationService.GetThemeDisplayName(this.ColorTheme));
					this.LoadCustomThemes(jsonObject["customThemes"]);
					bool flag2 = this.LoadCustomThemePalette(jsonObject["customThemePalette"]);
					this.ResetThemePaletteToDefaults();
					this.ModlistAutoLoadBehavior = UserConfigurationService.ParseModlistAutoLoadBehavior(UserConfigurationService.GetOptionalString(jsonObject["modlistAutoLoadBehavior"]));
					this.PreferredModlistsTab = UserConfigurationService.ParseModlistsTabSelection(UserConfigurationService.GetOptionalString(jsonObject["preferredModlistsTab"]));
					this.ModInfoPanePlacement = UserConfigurationService.ParseModInfoPanePlacement(UserConfigurationService.GetOptionalString(jsonObject["modInfoPanePlacement"]));
					this._modsSortMemberPath = UserConfigurationService.NormalizeSortMemberPath(UserConfigurationService.GetOptionalString(jsonObject["modsSortMemberPath"]));
					this._modsSortDirection = UserConfigurationService.ParseSortDirection(UserConfigurationService.GetOptionalString(jsonObject["modsSortDirection"]));
					this._localModlistsSortMemberPath = UserConfigurationService.NormalizeOptionalSortMemberPath(UserConfigurationService.GetOptionalString(jsonObject["localModlistsSortMemberPath"]));
					this._localModlistsSortDirection = UserConfigurationService.ParseSortDirection(UserConfigurationService.GetOptionalString(jsonObject["localModlistsSortDirection"]));
					this._onlineModlistsSortMemberPath = UserConfigurationService.NormalizeOptionalSortMemberPath(UserConfigurationService.GetOptionalString(jsonObject["onlineModlistsSortMemberPath"]));
					this._onlineModlistsSortDirection = UserConfigurationService.ParseSortDirection(UserConfigurationService.GetOptionalString(jsonObject["onlineModlistsSortDirection"]));
					JsonNode jsonNode23 = jsonObject["modDatabaseSearchResultLimit"];
					this.ModDatabaseSearchResultLimit = UserConfigurationService.NormalizeModDatabaseSearchResultLimit((jsonNode23 != null) ? jsonNode23.GetValue<int?>() : default(int?));
					JsonNode jsonNode24 = jsonObject["modDatabaseNewModsRecentMonths"];
					this.ModDatabaseNewModsRecentMonths = UserConfigurationService.NormalizeModDatabaseNewModsRecentMonths((jsonNode24 != null) ? jsonNode24.GetValue<int?>() : default(int?));
					this.ModDatabaseAutoLoadMode = UserConfigurationService.ParseModDatabaseAutoLoadMode(UserConfigurationService.GetOptionalString(jsonObject["modDatabaseAutoLoadMode"]));
					JsonNode jsonNode25 = jsonObject["excludeInstalledModDatabaseResults"];
					this.ExcludeInstalledModDatabaseResults = ((jsonNode25 != null) ? jsonNode25.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					JsonNode jsonNode26 = jsonObject["onlyShowCompatibleModDatabaseResults"];
					this.OnlyShowCompatibleModDatabaseResults = ((jsonNode26 != null) ? jsonNode26.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					JsonNode jsonNode27 = jsonObject["requireExactVsVersionMatch"];
					this.RequireExactVsVersionMatch = ((jsonNode27 != null) ? jsonNode27.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					this.ModBrowserOrderBy = (UserConfigurationService.GetOptionalString(jsonObject["modBrowserOrderBy"]) ?? "follows");
					this.ModBrowserOrderByDirection = (UserConfigurationService.GetOptionalString(jsonObject["modBrowserOrderByDirection"]) ?? "desc");
					this.ModBrowserSelectedSide = (UserConfigurationService.GetOptionalString(jsonObject["modBrowserSelectedSide"]) ?? "any");
					this.ModBrowserSelectedInstalledFilter = (UserConfigurationService.GetOptionalString(jsonObject["modBrowserSelectedInstalledFilter"]) ?? "all");
					JsonNode jsonNode28 = jsonObject["modBrowserOnlyFavorites"];
					this.ModBrowserOnlyFavorites = ((jsonNode28 != null) ? jsonNode28.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					JsonNode jsonNode29 = jsonObject["modBrowserRelevantSearch"];
					this.ModBrowserRelevantSearch = ((jsonNode29 != null) ? jsonNode29.GetValue<bool?>() : default(bool?)).GetValueOrDefault(true);
					this.ModBrowserFavoriteModIds = this.LoadIntList(jsonObject["modBrowserFavoriteModIds"]);
					this.ModBrowserSelectedVersionIds = this.LoadStringList(jsonObject["modBrowserSelectedVersionIds"]);
					this.ModBrowserSelectedTagIds = this.LoadIntList(jsonObject["modBrowserSelectedTagIds"]);
					JsonNode jsonNode30 = jsonObject["windowWidth"];
					this._windowWidth = UserConfigurationService.NormalizeWindowDimension((jsonNode30 != null) ? jsonNode30.GetValue<double?>() : default(double?));
					JsonNode jsonNode31 = jsonObject["windowHeight"];
					this._windowHeight = UserConfigurationService.NormalizeWindowDimension((jsonNode31 != null) ? jsonNode31.GetValue<double?>() : default(double?));
					JsonNode jsonNode32 = jsonObject["windowLeft"];
					this._windowLeft = UserConfigurationService.NormalizeWindowCoordinate((jsonNode32 != null) ? jsonNode32.GetValue<double?>() : default(double?));
					JsonNode jsonNode33 = jsonObject["windowTop"];
					this._windowTop = UserConfigurationService.NormalizeWindowCoordinate((jsonNode33 != null) ? jsonNode33.GetValue<double?>() : default(double?));
					JsonNode jsonNode34 = jsonObject["modInfoPanelLeft"];
					this._modInfoPanelLeft = UserConfigurationService.NormalizeModInfoCoordinate((jsonNode34 != null) ? jsonNode34.GetValue<double?>() : default(double?));
					JsonNode jsonNode35 = jsonObject["modInfoPanelTop"];
					this._modInfoPanelTop = UserConfigurationService.NormalizeModInfoCoordinate((jsonNode35 != null) ? jsonNode35.GetValue<double?>() : default(double?));
					this.LoadInstalledColumnVisibility(jsonObject["installedColumnVisibility"]);
					this.LoadThemePalette(jsonObject["themePalette"] ?? jsonObject["darkVsPalette"]);
					if (this.ColorTheme == ColorTheme.Custom)
					{
						this.SyncCustomThemePaletteWithCurrentTheme();
						if (!flag2)
						{
							this._hasPendingSave = true;
						}
					}
					this._selectedPresetName = UserConfigurationService.NormalizePresetName(UserConfigurationService.GetOptionalString(jsonObject["selectedPreset"]));
					this.CloudUploaderName = UserConfigurationService.NormalizeUploaderName(UserConfigurationService.GetOptionalString(jsonObject["cloudUploaderName"]));
					JsonNode jsonNode36 = jsonObject["migrationCheckCompleted"];
					this.MigrationCheckCompleted = ((jsonNode36 != null) ? jsonNode36.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					JsonNode jsonNode37 = jsonObject["firebaseAuthBackupCreated"];
					this.FirebaseAuthBackupCreated = ((jsonNode37 != null) ? jsonNode37.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					JsonNode jsonNode38 = jsonObject["clientSettingsCleanupCompleted"];
					this.ClientSettingsCleanupCompleted = ((jsonNode38 != null) ? jsonNode38.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					JsonNode jsonNode39 = jsonObject["rebuiltModlistMigrationCompleted"];
					this.RebuiltModlistMigrationCompleted = ((jsonNode39 != null) ? jsonNode39.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					JsonNode jsonNode40 = jsonObject["useFasterThumbnails"];
					bool? flag3 = (jsonNode40 != null) ? jsonNode40.GetValue<bool?>() : default(bool?);
					bool useFasterThumbnails;
					if (flag3 == null)
					{
						JsonNode jsonNode41 = jsonObject["useCorrectThumbnails"];
						useFasterThumbnails = !((jsonNode41 != null) ? jsonNode41.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					}
					else
					{
						useFasterThumbnails = flag3.GetValueOrDefault();
					}
					this.UseFasterThumbnails = useFasterThumbnails;
					JsonNode jsonNode42 = jsonObject["disableHoverEffects"];
					this.DisableHoverEffects = ((jsonNode42 != null) ? jsonNode42.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
					bool flag4 = false;
					JsonObject jsonObject2 = jsonObject["gameProfiles"] as JsonObject;
					if (jsonObject2 != null)
					{
						flag4 = true;
						foreach (KeyValuePair<string, JsonNode> keyValuePair in jsonObject2)
						{
							string text;
							JsonNode jsonNode43;
							keyValuePair.Deconstruct(ref text, ref jsonNode43);
							string name = text;
							JsonNode jsonNode44 = jsonNode43;
							string text2 = UserConfigurationService.NormalizeGameProfileName(name);
							if (text2 != null)
							{
								JsonObject jsonObject3 = jsonNode44 as JsonObject;
								if (jsonObject3 != null)
								{
									UserConfigurationService.GameProfileState profile = this.EnsureProfile(text2);
									this.LoadGameProfile(profile, jsonObject3);
								}
							}
						}
					}
					if (!flag4)
					{
						this.ApplyLegacyProfileData(jsonObject, gameProfileState);
					}
					else
					{
						UserConfigurationService.GameProfileState gameProfileState2;
						if (!this._gameProfiles.TryGetValue("Default", ref gameProfileState2) || gameProfileState2 == null)
						{
							gameProfileState = new UserConfigurationService.GameProfileState("Default");
							this._gameProfiles[gameProfileState.Name] = gameProfileState;
						}
						else
						{
							gameProfileState = gameProfileState2;
						}
						UserConfigurationService.GameProfileState gameProfileState3 = gameProfileState;
						if (gameProfileState3.DataDirectory == null)
						{
							string text = gameProfileState3.DataDirectory = dataDirectory;
						}
						gameProfileState3 = gameProfileState;
						if (gameProfileState3.GameDirectory == null)
						{
							string text = gameProfileState3.GameDirectory = gameDirectory;
						}
						this.ApplyLegacyProfileData(jsonObject, gameProfileState);
					}
					string customShortcutPath = UserConfigurationService.NormalizePath(UserConfigurationService.GetOptionalString(jsonObject["customShortcutPath"]));
					foreach (UserConfigurationService.GameProfileState gameProfileState4 in this._gameProfiles.Values)
					{
						if (gameProfileState4.DataDirectory == null && !gameProfileState4.RequiresDataDirectorySelection)
						{
							gameProfileState4.DataDirectory = dataDirectory;
						}
						if (gameProfileState4.DataDirectory != null)
						{
							gameProfileState4.RequiresDataDirectorySelection = false;
						}
						if (gameProfileState4.GameDirectory == null && !gameProfileState4.RequiresGameDirectorySelection)
						{
							gameProfileState4.GameDirectory = gameDirectory;
						}
						if (gameProfileState4.GameDirectory != null)
						{
							gameProfileState4.RequiresGameDirectorySelection = false;
						}
						if (gameProfileState4.CustomShortcutPath == null && !gameProfileState4.RequiresDataDirectorySelection && !gameProfileState4.RequiresGameDirectorySelection)
						{
							gameProfileState4.CustomShortcutPath = customShortcutPath;
						}
					}
					string text3 = UserConfigurationService.NormalizeGameProfileName(UserConfigurationService.GetOptionalString(jsonObject["activeGameProfile"]));
					UserConfigurationService.GameProfileState activeProfile;
					if (text3 != null && this._gameProfiles.TryGetValue(text3, ref activeProfile))
					{
						this.ActiveProfile = activeProfile;
					}
					else
					{
						this.ActiveProfile = gameProfileState;
					}
					if (this._isModUsageTrackingDisabled)
					{
						foreach (UserConfigurationService.GameProfileState gameProfileState5 in this._gameProfiles.Values)
						{
							gameProfileState5.ModUsageSessionCounts.Clear();
							gameProfileState5.LongRunningSessionCount = 0;
							gameProfileState5.HasPendingModUsagePrompt = false;
						}
					}
				}
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is JsonException)
			{
				this._gameProfiles.Clear();
				UserConfigurationService.GameProfileState gameProfileState6 = new UserConfigurationService.GameProfileState("Default");
				this._gameProfiles[gameProfileState6.Name] = gameProfileState6;
				this.ActiveProfile = gameProfileState6;
				this.ColorTheme = ColorTheme.VintageStory;
				this.ResetCustomThemePaletteToDefaults();
				this.ResetThemePaletteToDefaults();
				this.ConfigurationVersion = UserConfigurationService.CurrentConfigurationVersion;
				this.ModManagerVersion = UserConfigurationService.CurrentModManagerVersion;
				this.IsCompactView = false;
				this.UseModDbDesignView = true;
				this.CacheAllVersionsLocally = true;
				this.DisableAutoRefresh = false;
				this.DisableAutoRefreshWarningAcknowledged = false;
				this.DisableInternetAccess = false;
				this.EnableServerOptions = false;
				this.LogModUpdates = false;
				this.LogModInstalls = false;
				this.LogModDeletions = false;
				this.AutomaticDataBackupsWarningAcknowledged = false;
				this.CloseManagerAfterGameLaunch = false;
				this.CustomDataBackupLocation = null;
				this.MaximumDataBackups = 0;
				this.SuppressModlistSavePrompt = false;
				this._suppressRefreshCachePrompt = false;
				this._suppressRefreshCachePromptVersion = null;
				this.ModlistAutoLoadBehavior = ModlistAutoLoadBehavior.Prompt;
				this.PreferredModlistsTab = ModlistsTabSelection.Local;
				this.ModInfoPanePlacement = ModInfoPanePlacement.Right;
				this._modsSortMemberPath = null;
				this._modsSortDirection = 0;
				this._localModlistsSortMemberPath = null;
				this._localModlistsSortDirection = 0;
				this._onlineModlistsSortMemberPath = null;
				this._onlineModlistsSortDirection = 0;
				this._selectedPresetName = null;
				this.ModDatabaseSearchResultLimit = UserConfigurationService.DefaultModDatabaseSearchResultLimit;
				this.ModDatabaseNewModsRecentMonths = UserConfigurationService.DefaultModDatabaseNewModsRecentMonths;
				this.ModDatabaseAutoLoadMode = ModDatabaseAutoLoadMode.TotalDownloads;
				this.CacheModDatabaseResults = false;
				this.ExcludeInstalledModDatabaseResults = false;
				this.OnlyShowCompatibleModDatabaseResults = false;
				this.RequireExactVsVersionMatch = false;
				this.ModBrowserOrderBy = "follows";
				this.ModBrowserOrderByDirection = "desc";
				this.ModBrowserSelectedSide = "any";
				this.ModBrowserSelectedInstalledFilter = "all";
				this.ModBrowserOnlyFavorites = false;
				this.ModBrowserRelevantSearch = true;
				this.ModBrowserFavoriteModIds = new List<int>();
				this.ModBrowserSelectedVersionIds = new List<string>();
				this.ModBrowserSelectedTagIds = new List<int>();
				this._windowWidth = default(double?);
				this._windowHeight = default(double?);
				this._windowLeft = default(double?);
				this._windowTop = default(double?);
				this._modInfoPanelLeft = default(double?);
				this._modInfoPanelTop = default(double?);
				this.CloudUploaderName = null;
				this._installedColumnVisibility.Clear();
				this._isModUsageTrackingDisabled = false;
				this.MigrationCheckCompleted = false;
				this.FirebaseAuthBackupCreated = false;
				this.GameProfileCreationWarningAcknowledged = false;
				this.ClientSettingsCleanupCompleted = false;
				this.RebuiltModlistMigrationCompleted = false;
				this.UseFasterThumbnails = true;
			}
			this.LoadPersistentModConfigPaths();
			this.BackfillModConfigPathHistory();
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x0004C440 File Offset: 0x0004A640
		private void Save()
		{
			this._hasPendingSave = true;
			if (!this._isPersistenceEnabled)
			{
				return;
			}
			this.PersistConfiguration();
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x0004C458 File Offset: 0x0004A658
		private void PersistConfiguration()
		{
			try
			{
				Directory.CreateDirectory(Path.GetDirectoryName(this._configurationPath));
				JsonObject jsonObject = new JsonObject(default(JsonNodeOptions?));
				jsonObject["configurationVersion"] = this.ConfigurationVersion;
				jsonObject["modManagerVersion"] = this.ModManagerVersion;
				jsonObject["dataDirectory"] = this.DataDirectory;
				jsonObject["gameDirectory"] = this.GameDirectory;
				jsonObject["activeGameProfile"] = this.ActiveProfile.Name;
				jsonObject["isCompactView"] = this.IsCompactView;
				jsonObject["useModDbDesignView"] = this.UseModDbDesignView;
				jsonObject["cacheAllVersionsLocally"] = this.CacheAllVersionsLocally;
				jsonObject["cacheModDatabaseResults"] = this.CacheModDatabaseResults;
				jsonObject["disableAutoRefresh"] = this.DisableAutoRefresh;
				jsonObject["disableAutoRefreshWarningAcknowledged"] = this.DisableAutoRefreshWarningAcknowledged;
				jsonObject["disableInternetAccess"] = this.DisableInternetAccess;
				jsonObject["modUsageTrackingDisabled"] = this._isModUsageTrackingDisabled;
				jsonObject["enableServerOptions"] = this.EnableServerOptions;
				jsonObject["logModUpdates"] = this.LogModUpdates;
				jsonObject["logModInstalls"] = this.LogModInstalls;
				jsonObject["logModDeletions"] = this.LogModDeletions;
				jsonObject["logAppLaunchAndExit"] = this.LogAppLaunchAndExit;
				jsonObject["logErrorsAndExceptions"] = this.LogErrorsAndExceptions;
				jsonObject["automaticDataBackupsEnabled"] = this.AutomaticDataBackupsEnabled;
				jsonObject["automaticDataBackupsWarningAcknowledged"] = this.AutomaticDataBackupsWarningAcknowledged;
				jsonObject["closeManagerAfterGameLaunch"] = this.CloseManagerAfterGameLaunch;
				jsonObject["customDataBackupLocation"] = this.CustomDataBackupLocation;
				jsonObject["maximumDataBackups"] = this.MaximumDataBackups;
				jsonObject["suppressModlistSavePrompt"] = this.SuppressModlistSavePrompt;
				jsonObject["suppressRefreshCachePrompt"] = this._suppressRefreshCachePrompt;
				jsonObject["suppressRefreshCachePromptVersion"] = this._suppressRefreshCachePromptVersion;
				jsonObject["gameProfileCreationWarningAcknowledged"] = this.GameProfileCreationWarningAcknowledged;
				jsonObject["useDarkVsMode"] = (this.ColorTheme != ColorTheme.Light);
				jsonObject["colorTheme"] = this.ColorTheme.ToString();
				jsonObject["currentThemeName"] = this._currentThemeName;
				jsonObject["modlistAutoLoadBehavior"] = this.ModlistAutoLoadBehavior.ToString();
				jsonObject["preferredModlistsTab"] = this.PreferredModlistsTab.ToString();
				jsonObject["modInfoPanePlacement"] = this.ModInfoPanePlacement.ToString();
				jsonObject["modsSortMemberPath"] = this._modsSortMemberPath;
				jsonObject["modsSortDirection"] = this._modsSortDirection.ToString();
				jsonObject["localModlistsSortMemberPath"] = this._localModlistsSortMemberPath;
				jsonObject["localModlistsSortDirection"] = this._localModlistsSortDirection.ToString();
				jsonObject["onlineModlistsSortMemberPath"] = this._onlineModlistsSortMemberPath;
				jsonObject["onlineModlistsSortDirection"] = this._onlineModlistsSortDirection.ToString();
				jsonObject["modDatabaseSearchResultLimit"] = this.ModDatabaseSearchResultLimit;
				jsonObject["modDatabaseNewModsRecentMonths"] = this.ModDatabaseNewModsRecentMonths;
				jsonObject["modDatabaseAutoLoadMode"] = this.ModDatabaseAutoLoadMode.ToString();
				jsonObject["excludeInstalledModDatabaseResults"] = this.ExcludeInstalledModDatabaseResults;
				jsonObject["onlyShowCompatibleModDatabaseResults"] = this.OnlyShowCompatibleModDatabaseResults;
				jsonObject["requireExactVsVersionMatch"] = this.RequireExactVsVersionMatch;
				jsonObject["modBrowserOrderBy"] = this.ModBrowserOrderBy;
				jsonObject["modBrowserOrderByDirection"] = this.ModBrowserOrderByDirection;
				jsonObject["modBrowserSelectedSide"] = this.ModBrowserSelectedSide;
				jsonObject["modBrowserSelectedInstalledFilter"] = this.ModBrowserSelectedInstalledFilter;
				jsonObject["modBrowserOnlyFavorites"] = this.ModBrowserOnlyFavorites;
				jsonObject["modBrowserRelevantSearch"] = this.ModBrowserRelevantSearch;
				jsonObject["modBrowserFavoriteModIds"] = UserConfigurationService.BuildIntListJson(this.ModBrowserFavoriteModIds);
				jsonObject["modBrowserSelectedVersionIds"] = UserConfigurationService.BuildStringListJson(this.ModBrowserSelectedVersionIds);
				jsonObject["modBrowserSelectedTagIds"] = UserConfigurationService.BuildIntListJson(this.ModBrowserSelectedTagIds);
				jsonObject["windowWidth"] = this._windowWidth;
				jsonObject["windowHeight"] = this._windowHeight;
				jsonObject["windowLeft"] = this._windowLeft;
				jsonObject["windowTop"] = this._windowTop;
				jsonObject["modInfoPanelLeft"] = this._modInfoPanelLeft;
				jsonObject["modInfoPanelTop"] = this._modInfoPanelTop;
				jsonObject["bulkUpdateModExclusions"] = UserConfigurationService.BuildBulkUpdateModExclusionsJson(this.ActiveProfile.BulkUpdateModExclusions);
				jsonObject["skippedModVersions"] = UserConfigurationService.BuildSkippedModVersionsJson(this.ActiveProfile.SkippedModVersions);
				jsonObject["dependencyWarningOverrides"] = UserConfigurationService.BuildDependencyWarningOverridesJson(this.ActiveProfile.DependencyWarningOverrides);
				jsonObject["installedColumnVisibility"] = this.BuildInstalledColumnVisibilityJson();
				jsonObject["modUsageTracking"] = UserConfigurationService.BuildModUsageTrackingJson(this.ActiveProfile);
				jsonObject["themePalette"] = this.BuildThemePaletteJson();
				jsonObject["darkVsPalette"] = this.BuildThemePaletteJson();
				jsonObject["customThemePalette"] = this.BuildCustomThemePaletteJson();
				jsonObject["customThemes"] = this.BuildCustomThemesJson();
				jsonObject["selectedPreset"] = this._selectedPresetName;
				jsonObject["customShortcutPath"] = this.ActiveProfile.CustomShortcutPath;
				jsonObject["cloudUploaderName"] = this.CloudUploaderName;
				jsonObject["migrationCheckCompleted"] = this.MigrationCheckCompleted;
				jsonObject["firebaseAuthBackupCreated"] = this.FirebaseAuthBackupCreated;
				jsonObject["clientSettingsCleanupCompleted"] = this.ClientSettingsCleanupCompleted;
				jsonObject["rebuiltModlistMigrationCompleted"] = this.RebuiltModlistMigrationCompleted;
				jsonObject["useFasterThumbnails"] = this.UseFasterThumbnails;
				jsonObject["disableHoverEffects"] = this.DisableHoverEffects;
				jsonObject["gameProfiles"] = this.BuildGameProfilesJson();
				JsonObject jsonObject2 = jsonObject;
				this.RemoveRedundantRootProfileValues(jsonObject2);
				File.WriteAllText(this._configurationPath, jsonObject2.ToJsonString(UserConfigurationService.JsonOptions));
				this._hasPendingSave = false;
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
			{
			}
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x0004CC44 File Offset: 0x0004AE44
		private void RemoveRedundantRootProfileValues(JsonObject root)
		{
			JsonObject jsonObject = root["gameProfiles"] as JsonObject;
			if (jsonObject == null || !jsonObject.ContainsKey("Default"))
			{
				return;
			}
			foreach (string text in UserConfigurationService.RedundantRootProfilePropertyNames)
			{
				root.Remove(text);
			}
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x0004CC94 File Offset: 0x0004AE94
		[NullableContext(2)]
		private void InitializeVersionMetadata(string configurationVersion, string modManagerVersion)
		{
			bool flag = false;
			this.PreviousConfigurationVersion = configurationVersion;
			this.PreviousModManagerVersion = modManagerVersion;
			bool flag2 = !string.IsNullOrWhiteSpace(configurationVersion) && !string.Equals(configurationVersion, UserConfigurationService.CurrentConfigurationVersion, 5);
			bool flag3 = !string.IsNullOrWhiteSpace(modManagerVersion) && !string.Equals(modManagerVersion, UserConfigurationService.CurrentModManagerVersion, 5);
			this.HasVersionMismatch = (flag2 || flag3);
			string text = string.IsNullOrWhiteSpace(configurationVersion) ? UserConfigurationService.CurrentConfigurationVersion : configurationVersion;
			string text2 = string.IsNullOrWhiteSpace(modManagerVersion) ? UserConfigurationService.CurrentModManagerVersion : modManagerVersion;
			if (!string.Equals(text, UserConfigurationService.CurrentConfigurationVersion, 5))
			{
				text = UserConfigurationService.CurrentConfigurationVersion;
				flag = true;
			}
			if (!string.Equals(text2, UserConfigurationService.CurrentModManagerVersion, 5))
			{
				text2 = UserConfigurationService.CurrentModManagerVersion;
				flag = true;
			}
			this.ConfigurationVersion = text;
			this.ModManagerVersion = text2;
			if (flag)
			{
				this._hasPendingSave = true;
			}
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x0004CD5C File Offset: 0x0004AF5C
		private static int NormalizeModDatabaseSearchResultLimit(int? value)
		{
			if (value == null)
			{
				return UserConfigurationService.DefaultModDatabaseSearchResultLimit;
			}
			int value2 = value.Value;
			if (value2 <= 0)
			{
				return UserConfigurationService.DefaultModDatabaseSearchResultLimit;
			}
			return Math.Max(value2, 1);
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x0004CD91 File Offset: 0x0004AF91
		private static int NormalizeMaximumDataBackups(int? value)
		{
			if (value == null)
			{
				return 0;
			}
			return Math.Max(value.Value, 0);
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x0004CDAC File Offset: 0x0004AFAC
		private static int NormalizeModDatabaseNewModsRecentMonths(int? value)
		{
			if (value == null)
			{
				return UserConfigurationService.DefaultModDatabaseNewModsRecentMonths;
			}
			int value2 = value.Value;
			if (value2 <= 0)
			{
				return UserConfigurationService.DefaultModDatabaseNewModsRecentMonths;
			}
			return Math.Clamp(value2, 1, UserConfigurationService.MaxModDatabaseNewModsRecentMonths);
		}

		// Token: 0x06000CD0 RID: 3280 RVA: 0x0004CDE8 File Offset: 0x0004AFE8
		[NullableContext(2)]
		private static ModlistAutoLoadBehavior ParseModlistAutoLoadBehavior(string value)
		{
			ModlistAutoLoadBehavior result;
			if (Enum.TryParse<ModlistAutoLoadBehavior>(value, true, ref result))
			{
				return result;
			}
			return ModlistAutoLoadBehavior.Prompt;
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x0004CE04 File Offset: 0x0004B004
		[NullableContext(2)]
		private static ModlistsTabSelection ParseModlistsTabSelection(string value)
		{
			ModlistsTabSelection result;
			if (Enum.TryParse<ModlistsTabSelection>(value, true, ref result))
			{
				return result;
			}
			return ModlistsTabSelection.Local;
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x0004CE20 File Offset: 0x0004B020
		[NullableContext(2)]
		private static ModInfoPanePlacement ParseModInfoPanePlacement(string value)
		{
			ModInfoPanePlacement result;
			if (Enum.TryParse<ModInfoPanePlacement>(value, true, ref result))
			{
				return result;
			}
			return ModInfoPanePlacement.Right;
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x0004CE3C File Offset: 0x0004B03C
		[NullableContext(2)]
		private static ModDatabaseAutoLoadMode ParseModDatabaseAutoLoadMode(string value)
		{
			ModDatabaseAutoLoadMode result;
			if (Enum.TryParse<ModDatabaseAutoLoadMode>(value, true, ref result))
			{
				return result;
			}
			return ModDatabaseAutoLoadMode.TotalDownloads;
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x0004CE58 File Offset: 0x0004B058
		private static double? NormalizeWindowDimension(double? dimension)
		{
			if (dimension == null)
			{
				return default(double?);
			}
			double value = dimension.Value;
			if (double.IsNaN(value) || double.IsInfinity(value) || value <= 0.0)
			{
				return default(double?);
			}
			return new double?(value);
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0004CEAC File Offset: 0x0004B0AC
		private static double? NormalizeModInfoCoordinate(double? coordinate)
		{
			if (coordinate == null)
			{
				return default(double?);
			}
			double value = coordinate.Value;
			if (double.IsNaN(value) || double.IsInfinity(value))
			{
				return default(double?);
			}
			return new double?(value);
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x0004CEF4 File Offset: 0x0004B0F4
		private static double? NormalizeWindowCoordinate(double? coordinate)
		{
			if (coordinate == null)
			{
				return default(double?);
			}
			double value = coordinate.Value;
			if (double.IsNaN(value) || double.IsInfinity(value))
			{
				return default(double?);
			}
			return new double?(value);
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x0004CF3C File Offset: 0x0004B13C
		private static string DetermineConfigurationPath()
		{
			return Path.Combine(UserConfigurationService.GetPreferredConfigurationDirectory(), UserConfigurationService.ConfigurationFileName);
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x0004CF4D File Offset: 0x0004B14D
		private static string DetermineModConfigPathsPath(string fileName)
		{
			return Path.Combine(UserConfigurationService.GetPreferredConfigurationDirectory(), fileName);
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x0004CF5C File Offset: 0x0004B15C
		private JsonObject BuildInstalledColumnVisibilityJson()
		{
			JsonObject jsonObject = new JsonObject(default(JsonNodeOptions?));
			foreach (KeyValuePair<string, bool> keyValuePair in Enumerable.OrderBy<KeyValuePair<string, bool>, string>(this._installedColumnVisibility, (KeyValuePair<string, bool> entry) => entry.Key, StringComparer.OrdinalIgnoreCase))
			{
				if (!string.IsNullOrWhiteSpace(keyValuePair.Key))
				{
					jsonObject[keyValuePair.Key] = keyValuePair.Value;
				}
			}
			return jsonObject;
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x0004D004 File Offset: 0x0004B204
		private static JsonObject BuildBulkUpdateModExclusionsJson(IReadOnlyDictionary<string, bool> source)
		{
			JsonObject jsonObject = new JsonObject(default(JsonNodeOptions?));
			foreach (KeyValuePair<string, bool> keyValuePair in Enumerable.OrderBy<KeyValuePair<string, bool>, string>(source, (KeyValuePair<string, bool> entry) => entry.Key, StringComparer.OrdinalIgnoreCase))
			{
				if (!string.IsNullOrWhiteSpace(keyValuePair.Key) && keyValuePair.Value)
				{
					jsonObject[keyValuePair.Key] = true;
				}
			}
			return jsonObject;
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0004D0A8 File Offset: 0x0004B2A8
		private static JsonObject BuildSkippedModVersionsJson(IReadOnlyDictionary<string, string> source)
		{
			JsonObject jsonObject = new JsonObject(default(JsonNodeOptions?));
			foreach (KeyValuePair<string, string> keyValuePair in Enumerable.OrderBy<KeyValuePair<string, string>, string>(source, (KeyValuePair<string, string> entry) => entry.Key, StringComparer.OrdinalIgnoreCase))
			{
				if (!string.IsNullOrWhiteSpace(keyValuePair.Key) && !string.IsNullOrWhiteSpace(keyValuePair.Value))
				{
					jsonObject[keyValuePair.Key] = keyValuePair.Value;
				}
			}
			return jsonObject;
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0004D158 File Offset: 0x0004B358
		private static JsonObject BuildDependencyWarningOverridesJson(IReadOnlyDictionary<string, string> source)
		{
			JsonObject jsonObject = new JsonObject(default(JsonNodeOptions?));
			foreach (KeyValuePair<string, string> keyValuePair in Enumerable.OrderBy<KeyValuePair<string, string>, string>(source, (KeyValuePair<string, string> entry) => entry.Key, StringComparer.OrdinalIgnoreCase))
			{
				if (!string.IsNullOrWhiteSpace(keyValuePair.Key) && !string.IsNullOrWhiteSpace(keyValuePair.Value))
				{
					jsonObject[keyValuePair.Key] = keyValuePair.Value;
				}
			}
			return jsonObject;
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0004D208 File Offset: 0x0004B408
		private static JsonObject BuildModUsageTrackingJson(UserConfigurationService.GameProfileState profile)
		{
			JsonArray jsonArray = new JsonArray(default(JsonNodeOptions?));
			foreach (KeyValuePair<ModUsageTrackingKey, int> keyValuePair in Enumerable.ThenBy<KeyValuePair<ModUsageTrackingKey, int>, string>(Enumerable.ThenBy<KeyValuePair<ModUsageTrackingKey, int>, string>(Enumerable.OrderBy<KeyValuePair<ModUsageTrackingKey, int>, string>(profile.ModUsageSessionCounts, (KeyValuePair<ModUsageTrackingKey, int> entry) => entry.Key.ModId, StringComparer.OrdinalIgnoreCase), (KeyValuePair<ModUsageTrackingKey, int> entry) => entry.Key.ModVersion, StringComparer.OrdinalIgnoreCase), (KeyValuePair<ModUsageTrackingKey, int> entry) => entry.Key.GameVersion, StringComparer.OrdinalIgnoreCase))
			{
				if (keyValuePair.Key.IsValid && keyValuePair.Value > 0)
				{
					JsonArray jsonArray2 = jsonArray;
					JsonObject jsonObject = new JsonObject(default(JsonNodeOptions?));
					jsonObject["modId"] = keyValuePair.Key.ModId;
					jsonObject["modVersion"] = keyValuePair.Key.ModVersion;
					jsonObject["gameVersion"] = keyValuePair.Key.GameVersion;
					jsonObject["count"] = keyValuePair.Value;
					jsonArray2.Add<JsonObject>(jsonObject);
				}
			}
			JsonObject jsonObject2 = new JsonObject(default(JsonNodeOptions?));
			jsonObject2["longSessionCount"] = profile.LongRunningSessionCount;
			jsonObject2["pendingPrompt"] = (profile.HasPendingModUsagePrompt && jsonArray.Count > 0);
			jsonObject2["modCounts"] = jsonArray;
			return jsonObject2;
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0004D3E0 File Offset: 0x0004B5E0
		private JsonObject BuildThemePaletteJson()
		{
			IReadOnlyDictionary<string, string> readOnlyDictionary;
			if (this.ColorTheme != ColorTheme.Custom)
			{
				readOnlyDictionary = UserConfigurationService.GetDefaultPalette(this.ColorTheme);
			}
			else
			{
				IReadOnlyDictionary<string, string> customThemePaletteColors = this._customThemePaletteColors;
				readOnlyDictionary = customThemePaletteColors;
			}
			IReadOnlyDictionary<string, string> defaults = readOnlyDictionary;
			return UserConfigurationService.BuildPaletteJson(this._themePaletteColors, defaults);
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x0004D418 File Offset: 0x0004B618
		private JsonObject BuildCustomThemePaletteJson()
		{
			return UserConfigurationService.BuildPaletteJson(this._customThemePaletteColors, UserConfigurationService.GetDefaultPalette(ColorTheme.Custom));
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x0004D42C File Offset: 0x0004B62C
		private JsonObject BuildCustomThemesJson()
		{
			JsonObject jsonObject = new JsonObject(default(JsonNodeOptions?));
			foreach (KeyValuePair<string, Dictionary<string, string>> keyValuePair in Enumerable.OrderBy<KeyValuePair<string, Dictionary<string, string>>, string>(this._savedCustomThemes, (KeyValuePair<string, Dictionary<string, string>> entry) => entry.Key, StringComparer.OrdinalIgnoreCase))
			{
				if (!string.IsNullOrWhiteSpace(keyValuePair.Key) && keyValuePair.Value.Count != 0)
				{
					jsonObject[keyValuePair.Key] = UserConfigurationService.BuildPaletteJson(keyValuePair.Value, UserConfigurationService.GetDefaultPalette(ColorTheme.Custom));
				}
			}
			return jsonObject;
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x0004D4E8 File Offset: 0x0004B6E8
		private static JsonArray BuildStringListJson(List<string> strings)
		{
			JsonArray jsonArray = new JsonArray(default(JsonNodeOptions?));
			foreach (string text in strings)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					jsonArray.Add<string>(text);
				}
			}
			return jsonArray;
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x0004D550 File Offset: 0x0004B750
		private static JsonArray BuildIntListJson(List<int> ints)
		{
			JsonArray jsonArray = new JsonArray(default(JsonNodeOptions?));
			foreach (int num in ints)
			{
				jsonArray.Add<int>(num);
			}
			return jsonArray;
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x0004D5B0 File Offset: 0x0004B7B0
		private JsonObject BuildGameProfilesJson()
		{
			JsonObject jsonObject = new JsonObject(default(JsonNodeOptions?));
			foreach (UserConfigurationService.GameProfileState gameProfileState in Enumerable.OrderBy<UserConfigurationService.GameProfileState, string>(this._gameProfiles.Values, (UserConfigurationService.GameProfileState p) => p.Name, StringComparer.OrdinalIgnoreCase))
			{
				JsonObject jsonObject2 = new JsonObject(default(JsonNodeOptions?));
				jsonObject2["dataDirectory"] = gameProfileState.DataDirectory;
				jsonObject2["gameDirectory"] = gameProfileState.GameDirectory;
				jsonObject2["targetGameVersionOverride"] = gameProfileState.TargetGameVersionOverride;
				jsonObject2["customShortcutPath"] = gameProfileState.CustomShortcutPath;
				jsonObject2["bulkUpdateModExclusions"] = UserConfigurationService.BuildBulkUpdateModExclusionsJson(gameProfileState.BulkUpdateModExclusions);
				jsonObject2["skippedModVersions"] = UserConfigurationService.BuildSkippedModVersionsJson(gameProfileState.SkippedModVersions);
				jsonObject2["dependencyWarningOverrides"] = UserConfigurationService.BuildDependencyWarningOverridesJson(gameProfileState.DependencyWarningOverrides);
				jsonObject2["modUsageTracking"] = UserConfigurationService.BuildModUsageTrackingJson(gameProfileState);
				JsonObject jsonObject3 = jsonObject2;
				if (gameProfileState.RequiresDataDirectorySelection)
				{
					jsonObject3["requiresDataDirectorySelection"] = true;
				}
				if (gameProfileState.RequiresGameDirectorySelection)
				{
					jsonObject3["requiresGameDirectorySelection"] = true;
				}
				jsonObject[gameProfileState.Name] = jsonObject3;
			}
			return jsonObject;
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x0004D744 File Offset: 0x0004B944
		private static JsonObject BuildPaletteJson(IReadOnlyDictionary<string, string> palette, IReadOnlyDictionary<string, string> defaults)
		{
			JsonObject jsonObject = new JsonObject(default(JsonNodeOptions?));
			foreach (KeyValuePair<string, string> keyValuePair in Enumerable.OrderBy<KeyValuePair<string, string>, string>(palette, (KeyValuePair<string, string> entry) => entry.Key, StringComparer.OrdinalIgnoreCase))
			{
				if (!string.IsNullOrWhiteSpace(keyValuePair.Key) && !string.IsNullOrWhiteSpace(keyValuePair.Value))
				{
					jsonObject[keyValuePair.Key] = keyValuePair.Value;
				}
			}
			foreach (KeyValuePair<string, string> keyValuePair2 in Enumerable.OrderBy<KeyValuePair<string, string>, string>(defaults, (KeyValuePair<string, string> entry) => entry.Key, StringComparer.OrdinalIgnoreCase))
			{
				if (!jsonObject.ContainsKey(keyValuePair2.Key))
				{
					jsonObject[keyValuePair2.Key] = keyValuePair2.Value;
				}
			}
			return jsonObject;
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x0004D878 File Offset: 0x0004BA78
		private void LoadBulkUpdateModExclusions([Nullable(2)] JsonNode node, Dictionary<string, bool> target)
		{
			target.Clear();
			JsonObject jsonObject = node as JsonObject;
			if (jsonObject == null)
			{
				return;
			}
			bool flag = false;
			foreach (KeyValuePair<string, JsonNode> keyValuePair in jsonObject)
			{
				string text;
				JsonNode jsonNode;
				keyValuePair.Deconstruct(ref text, ref jsonNode);
				string modId = text;
				JsonNode jsonNode2 = jsonNode;
				string text2 = UserConfigurationService.NormalizeModId(modId);
				if (text2 == null)
				{
					flag = true;
				}
				else if (!((jsonNode2 != null) ? jsonNode2.GetValue<bool?>() : default(bool?)).GetValueOrDefault())
				{
					flag = true;
				}
				else if (!target.ContainsKey(text2))
				{
					target[text2] = true;
				}
			}
			if (flag)
			{
				this._hasPendingSave = true;
			}
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x0004D930 File Offset: 0x0004BB30
		private void LoadSkippedModVersions([Nullable(2)] JsonNode node, Dictionary<string, string> target)
		{
			target.Clear();
			JsonObject jsonObject = node as JsonObject;
			if (jsonObject == null)
			{
				return;
			}
			bool flag = false;
			foreach (KeyValuePair<string, JsonNode> keyValuePair in jsonObject)
			{
				string text;
				JsonNode jsonNode;
				keyValuePair.Deconstruct(ref text, ref jsonNode);
				string modId = text;
				JsonNode node2 = jsonNode;
				string text2 = UserConfigurationService.NormalizeModId(modId);
				if (text2 == null)
				{
					flag = true;
				}
				else
				{
					string text3 = UserConfigurationService.NormalizeVersion(UserConfigurationService.GetOptionalString(node2));
					if (text3 == null)
					{
						flag = true;
					}
					else if (!target.ContainsKey(text2))
					{
						target[text2] = text3;
					}
				}
			}
			if (flag)
			{
				this._hasPendingSave = true;
			}
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x0004D9D8 File Offset: 0x0004BBD8
		private void LoadDependencyWarningOverrides([Nullable(2)] JsonNode node, Dictionary<string, string> target)
		{
			target.Clear();
			JsonObject jsonObject = node as JsonObject;
			if (jsonObject == null)
			{
				return;
			}
			bool flag = false;
			foreach (KeyValuePair<string, JsonNode> keyValuePair in jsonObject)
			{
				string text;
				JsonNode jsonNode;
				keyValuePair.Deconstruct(ref text, ref jsonNode);
				string modId = text;
				JsonNode node2 = jsonNode;
				string text2 = UserConfigurationService.NormalizeModId(modId);
				if (text2 == null)
				{
					flag = true;
				}
				else
				{
					string text3 = UserConfigurationService.NormalizeVersion(UserConfigurationService.GetOptionalString(node2));
					if (text3 == null)
					{
						flag = true;
					}
					else if (!target.ContainsKey(text2))
					{
						target[text2] = text3;
					}
				}
			}
			if (flag)
			{
				this._hasPendingSave = true;
			}
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x0004DA80 File Offset: 0x0004BC80
		[NullableContext(2)]
		private void LoadInstalledColumnVisibility(JsonNode node)
		{
			this._installedColumnVisibility.Clear();
			JsonObject jsonObject = node as JsonObject;
			if (jsonObject == null)
			{
				return;
			}
			foreach (KeyValuePair<string, JsonNode> keyValuePair in jsonObject)
			{
				string text = UserConfigurationService.NormalizeInstalledColumnKey(keyValuePair.Key);
				JsonNode value = keyValuePair.Value;
				bool? flag = (value != null) ? value.GetValue<bool?>() : default(bool?);
				if (text != null && flag != null)
				{
					this._installedColumnVisibility[text] = flag.Value;
				}
			}
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x0004DB24 File Offset: 0x0004BD24
		private List<string> LoadStringList([Nullable(2)] JsonNode node)
		{
			List<string> list = new List<string>();
			JsonArray jsonArray = node as JsonArray;
			if (jsonArray == null)
			{
				return list;
			}
			foreach (JsonNode node2 in jsonArray)
			{
				string optionalString = UserConfigurationService.GetOptionalString(node2);
				if (!string.IsNullOrWhiteSpace(optionalString))
				{
					list.Add(optionalString.Trim());
				}
			}
			return list;
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x0004DB94 File Offset: 0x0004BD94
		private List<int> LoadIntList([Nullable(2)] JsonNode node)
		{
			List<int> list = new List<int>();
			JsonArray jsonArray = node as JsonArray;
			if (jsonArray == null)
			{
				return list;
			}
			foreach (JsonNode jsonNode in jsonArray)
			{
				int? num = (jsonNode != null) ? jsonNode.GetValue<int?>() : default(int?);
				if (num != null)
				{
					list.Add(num.Value);
				}
			}
			return list;
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x0004DC14 File Offset: 0x0004BE14
		[NullableContext(2)]
		private void LoadThemePalette(JsonNode node)
		{
			IReadOnlyDictionary<string, string> currentThemeDefaults = this.GetCurrentThemeDefaults();
			JsonObject jsonObject = node as JsonObject;
			if (jsonObject == null)
			{
				UserConfigurationService.EnsurePaletteDefaults(this._themePaletteColors, currentThemeDefaults);
				this._hasPendingSave = true;
				return;
			}
			bool flag = false;
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (KeyValuePair<string, JsonNode> keyValuePair in jsonObject)
			{
				string key = keyValuePair.Key;
				if (!string.IsNullOrWhiteSpace(key))
				{
					string text = key.Trim();
					string text2;
					if (!this._themePaletteColors.ContainsKey(text))
					{
						if (currentThemeDefaults.ContainsKey(text))
						{
							this._themePaletteColors[text] = currentThemeDefaults[text];
						}
					}
					else if (!UserConfigurationService.TryNormalizeHexColor(UserConfigurationService.GetOptionalString(keyValuePair.Value), out text2))
					{
						flag = true;
					}
					else
					{
						this._themePaletteColors[text] = text2;
						hashSet.Add(text);
					}
				}
			}
			foreach (KeyValuePair<string, string> keyValuePair2 in currentThemeDefaults)
			{
				if (!hashSet.Contains(keyValuePair2.Key))
				{
					flag = true;
				}
				if (!this._themePaletteColors.ContainsKey(keyValuePair2.Key))
				{
					this._themePaletteColors[keyValuePair2.Key] = keyValuePair2.Value;
				}
			}
			if (flag)
			{
				this._hasPendingSave = true;
			}
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x0004DD90 File Offset: 0x0004BF90
		[NullableContext(2)]
		private void LoadCustomThemes(JsonNode node)
		{
			this._savedCustomThemes.Clear();
			JsonObject jsonObject = node as JsonObject;
			if (jsonObject == null)
			{
				return;
			}
			foreach (KeyValuePair<string, JsonNode> keyValuePair in jsonObject)
			{
				string text;
				JsonNode jsonNode;
				keyValuePair.Deconstruct(ref text, ref jsonNode);
				string name = text;
				JsonNode jsonNode2 = jsonNode;
				string text2 = UserConfigurationService.NormalizeThemeName(name);
				if (text2 != null)
				{
					JsonObject jsonObject2 = jsonNode2 as JsonObject;
					if (jsonObject2 != null)
					{
						IReadOnlyDictionary<string, string> defaultPalette = UserConfigurationService.GetDefaultPalette(ColorTheme.Custom);
						bool flag = false;
						HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
						Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
						foreach (KeyValuePair<string, JsonNode> keyValuePair2 in jsonObject2)
						{
							string key = keyValuePair2.Key;
							if (!string.IsNullOrWhiteSpace(key))
							{
								string text3 = key.Trim();
								string text4;
								if (!UserConfigurationService.TryNormalizeHexColor(UserConfigurationService.GetOptionalString(keyValuePair2.Value), out text4))
								{
									flag = true;
								}
								else
								{
									dictionary[text3] = text4;
									hashSet.Add(text3);
								}
							}
						}
						foreach (KeyValuePair<string, string> keyValuePair3 in defaultPalette)
						{
							if (!hashSet.Contains(keyValuePair3.Key))
							{
								flag = true;
							}
							if (!dictionary.ContainsKey(keyValuePair3.Key))
							{
								dictionary[keyValuePair3.Key] = keyValuePair3.Value;
							}
						}
						if (dictionary.Count == 0)
						{
							continue;
						}
						this._savedCustomThemes[text2] = dictionary;
						if (flag)
						{
							this._hasPendingSave = true;
							continue;
						}
						continue;
					}
				}
				this._hasPendingSave = true;
			}
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x0004DF7C File Offset: 0x0004C17C
		private void LoadModUsageTracking([Nullable(2)] JsonNode node, UserConfigurationService.GameProfileState profile)
		{
			profile.LongRunningSessionCount = 0;
			profile.HasPendingModUsagePrompt = false;
			profile.ModUsageSessionCounts.Clear();
			JsonObject jsonObject = node as JsonObject;
			if (jsonObject == null)
			{
				return;
			}
			JsonNode jsonNode = jsonObject["longSessionCount"];
			int? num = (jsonNode != null) ? jsonNode.GetValue<int?>() : default(int?);
			profile.LongRunningSessionCount = ((num != null && num.Value > 0) ? num.Value : 0);
			JsonNode jsonNode2 = jsonObject["pendingPrompt"];
			profile.HasPendingModUsagePrompt = ((jsonNode2 != null) ? jsonNode2.GetValue<bool?>() : default(bool?)).GetValueOrDefault();
			JsonNode jsonNode3 = jsonObject["modCounts"];
			JsonArray jsonArray = jsonNode3 as JsonArray;
			if (jsonArray != null)
			{
				using (IEnumerator<JsonNode> enumerator = jsonArray.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						JsonNode jsonNode4 = enumerator.Current;
						JsonObject jsonObject2 = jsonNode4 as JsonObject;
						if (jsonObject2 != null)
						{
							string optionalString = UserConfigurationService.GetOptionalString(jsonObject2["modId"]);
							string optionalString2 = UserConfigurationService.GetOptionalString(jsonObject2["modVersion"]);
							string optionalString3 = UserConfigurationService.GetOptionalString(jsonObject2["gameVersion"]);
							JsonNode jsonNode5 = jsonObject2["count"];
							int? num2 = (jsonNode5 != null) ? jsonNode5.GetValue<int?>() : default(int?);
							if (!string.IsNullOrWhiteSpace(optionalString) && !string.IsNullOrWhiteSpace(optionalString2) && !string.IsNullOrWhiteSpace(optionalString3) && num2 != null && num2.Value > 0)
							{
								ModUsageTrackingKey modUsageTrackingKey = new ModUsageTrackingKey(optionalString, optionalString2, optionalString3);
								if (modUsageTrackingKey.IsValid)
								{
									profile.ModUsageSessionCounts[modUsageTrackingKey] = num2.Value;
								}
							}
						}
					}
					goto IL_226;
				}
			}
			JsonObject jsonObject3 = jsonNode3 as JsonObject;
			if (jsonObject3 != null)
			{
				foreach (KeyValuePair<string, JsonNode> keyValuePair in jsonObject3)
				{
					string text;
					JsonNode jsonNode6;
					keyValuePair.Deconstruct(ref text, ref jsonNode6);
					string text2 = text;
					JsonNode jsonNode7 = jsonNode6;
					if (!string.IsNullOrWhiteSpace(text2) && jsonNode7 != null)
					{
						int? value = jsonNode7.GetValue<int?>();
						if (value != null && value.Value > 0 && UserConfigurationService.NormalizeModId(text2) != null)
						{
							this._hasPendingSave = true;
						}
					}
				}
			}
			IL_226:
			if (profile.ModUsageSessionCounts.Count == 0)
			{
				profile.LongRunningSessionCount = 0;
				profile.HasPendingModUsagePrompt = false;
				return;
			}
			if (profile.LongRunningSessionCount >= UserConfigurationService.GameSessionVoteThreshold)
			{
				profile.HasPendingModUsagePrompt = true;
			}
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x0004E1FC File Offset: 0x0004C3FC
		[NullableContext(2)]
		private bool LoadCustomThemePalette(JsonNode node)
		{
			IReadOnlyDictionary<string, string> defaultPalette = UserConfigurationService.GetDefaultPalette(ColorTheme.Custom);
			this._customThemePaletteColors.Clear();
			Dictionary<string, string> dictionary;
			if (!string.IsNullOrWhiteSpace(this._currentThemeName) && this._savedCustomThemes.TryGetValue(this._currentThemeName, ref dictionary))
			{
				foreach (KeyValuePair<string, string> keyValuePair in dictionary)
				{
					this._customThemePaletteColors[keyValuePair.Key] = keyValuePair.Value;
				}
				UserConfigurationService.EnsurePaletteDefaults(this._customThemePaletteColors, defaultPalette);
				return true;
			}
			bool flag = false;
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			JsonObject jsonObject = node as JsonObject;
			bool result;
			if (jsonObject != null)
			{
				result = true;
				using (IEnumerator<KeyValuePair<string, JsonNode>> enumerator2 = jsonObject.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<string, JsonNode> keyValuePair2 = enumerator2.Current;
						string key = keyValuePair2.Key;
						if (!string.IsNullOrWhiteSpace(key))
						{
							string text = key.Trim();
							if (defaultPalette.ContainsKey(text))
							{
								string text2;
								if (!UserConfigurationService.TryNormalizeHexColor(UserConfigurationService.GetOptionalString(keyValuePair2.Value), out text2))
								{
									flag = true;
								}
								else
								{
									this._customThemePaletteColors[text] = text2;
									hashSet.Add(text);
								}
							}
						}
					}
					goto IL_12C;
				}
			}
			result = false;
			flag = true;
			IL_12C:
			foreach (KeyValuePair<string, string> keyValuePair3 in defaultPalette)
			{
				if (!hashSet.Contains(keyValuePair3.Key))
				{
					flag = true;
				}
				if (!this._customThemePaletteColors.ContainsKey(keyValuePair3.Key))
				{
					this._customThemePaletteColors[keyValuePair3.Key] = keyValuePair3.Value;
				}
			}
			if (flag)
			{
				this._hasPendingSave = true;
			}
			return result;
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x0004E3D4 File Offset: 0x0004C5D4
		private void SyncCustomThemePaletteWithCurrentTheme()
		{
			this.EnsureCustomThemePaletteInitialized();
			foreach (KeyValuePair<string, string> keyValuePair in this._themePaletteColors)
			{
				this._customThemePaletteColors[keyValuePair.Key] = keyValuePair.Value;
			}
			UserConfigurationService.EnsurePaletteDefaults(this._customThemePaletteColors, UserConfigurationService.GetDefaultPalette(ColorTheme.Custom));
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x0004E450 File Offset: 0x0004C650
		private void ResetThemePaletteToDefaults()
		{
			this._themePaletteColors.Clear();
			if (this.ColorTheme == ColorTheme.Custom)
			{
				this.EnsureCustomThemePaletteInitialized();
				foreach (KeyValuePair<string, string> keyValuePair in this._customThemePaletteColors)
				{
					this._themePaletteColors[keyValuePair.Key] = keyValuePair.Value;
				}
				UserConfigurationService.EnsurePaletteDefaults(this._themePaletteColors, UserConfigurationService.GetDefaultPalette(ColorTheme.Custom));
				return;
			}
			foreach (KeyValuePair<string, string> keyValuePair2 in UserConfigurationService.GetDefaultPalette(this.ColorTheme))
			{
				this._themePaletteColors[keyValuePair2.Key] = keyValuePair2.Value;
			}
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x0004E534 File Offset: 0x0004C734
		private bool ApplyThemePaletteOverride(IReadOnlyDictionary<string, string> paletteOverride)
		{
			bool result = false;
			foreach (KeyValuePair<string, string> keyValuePair in paletteOverride)
			{
				if (!string.IsNullOrWhiteSpace(keyValuePair.Key))
				{
					string text = keyValuePair.Key.Trim();
					string text2;
					string text3;
					if (this._themePaletteColors.ContainsKey(text) && UserConfigurationService.TryNormalizeHexColor(keyValuePair.Value, out text2) && (!this._themePaletteColors.TryGetValue(text, ref text3) || !string.Equals(text3, text2, 5)))
					{
						this._themePaletteColors[text] = text2;
						if (this.ColorTheme == ColorTheme.Custom)
						{
							this.EnsureCustomThemePaletteInitialized();
							this._customThemePaletteColors[text] = text2;
						}
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x0004E600 File Offset: 0x0004C800
		private void ResetCustomThemePaletteToDefaults()
		{
			this._customThemePaletteColors.Clear();
			foreach (KeyValuePair<string, string> keyValuePair in UserConfigurationService.GetDefaultPalette(ColorTheme.Custom))
			{
				this._customThemePaletteColors[keyValuePair.Key] = keyValuePair.Value;
			}
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0004E66C File Offset: 0x0004C86C
		private void EnsureCustomThemePaletteInitialized()
		{
			if (this._customThemePaletteColors.Count == 0)
			{
				this.ResetCustomThemePaletteToDefaults();
				return;
			}
			UserConfigurationService.EnsurePaletteDefaults(this._customThemePaletteColors, UserConfigurationService.GetDefaultPalette(ColorTheme.Custom));
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x0004E693 File Offset: 0x0004C893
		private IReadOnlyDictionary<string, string> GetCurrentThemeDefaults()
		{
			if (this.ColorTheme == ColorTheme.Custom)
			{
				this.EnsureCustomThemePaletteInitialized();
				return this._customThemePaletteColors;
			}
			return UserConfigurationService.GetDefaultPalette(this.ColorTheme);
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x0004E6B8 File Offset: 0x0004C8B8
		private static void EnsurePaletteDefaults(IDictionary<string, string> palette, IReadOnlyDictionary<string, string> defaults)
		{
			foreach (KeyValuePair<string, string> keyValuePair in defaults)
			{
				if (!palette.ContainsKey(keyValuePair.Key))
				{
					palette[keyValuePair.Key] = keyValuePair.Value;
				}
			}
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0004E71C File Offset: 0x0004C91C
		[NullableContext(2)]
		private static string NormalizeThemeName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return null;
			}
			return name.Trim();
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0004E730 File Offset: 0x0004C930
		private static bool TryGetBuiltInTheme(string name, out ColorTheme theme)
		{
			foreach (KeyValuePair<ColorTheme, string> keyValuePair in UserConfigurationService.BuiltInThemeNames)
			{
				if (string.Equals(keyValuePair.Value, name, 5))
				{
					theme = keyValuePair.Key;
					return true;
				}
			}
			theme = ColorTheme.Custom;
			return false;
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0004E798 File Offset: 0x0004C998
		private static IReadOnlyDictionary<string, string> GetDefaultPalette(ColorTheme theme)
		{
			IReadOnlyDictionary<string, string> result;
			switch (theme)
			{
			case ColorTheme.VintageStory:
				result = UserConfigurationService.VintageStoryPaletteColors;
				break;
			case ColorTheme.Dark:
				result = UserConfigurationService.DarkPaletteColors;
				break;
			case ColorTheme.Light:
				result = UserConfigurationService.LightPaletteColors;
				break;
			case ColorTheme.SurpriseMe:
				result = UserConfigurationService.VintageStoryPaletteColors;
				break;
			case ColorTheme.Custom:
				result = UserConfigurationService.VintageStoryPaletteColors;
				break;
			default:
				result = UserConfigurationService.VintageStoryPaletteColors;
				break;
			}
			return result;
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x0004E7F0 File Offset: 0x0004C9F0
		private void LoadPersistentModConfigPaths()
		{
			this._storedModConfigPaths.Clear();
			bool flag = false;
			try
			{
				if (!File.Exists(this._modConfigPathsPath))
				{
					return;
				}
				using (FileStream fileStream = File.OpenRead(this._modConfigPathsPath))
				{
					JsonObject jsonObject = JsonNode.Parse(fileStream, default(JsonNodeOptions?), default(JsonDocumentOptions)) as JsonObject;
					if (jsonObject == null)
					{
						return;
					}
					string text = UserConfigurationService.NormalizeVersion(this.ModManagerVersion) ?? this.ModManagerVersion;
					string text2 = UserConfigurationService.NormalizeVersion(UserConfigurationService.GetOptionalString(jsonObject["version"]));
					flag = (string.IsNullOrWhiteSpace(text2) || !string.Equals(text2, text, 4));
					JsonObject jsonObject2 = jsonObject["modConfigPaths"] as JsonObject;
					bool flag2 = jsonObject2 == null;
					if (jsonObject2 == null)
					{
						jsonObject2 = jsonObject;
					}
					string activeModConfigDirectory = this.GetActiveModConfigDirectory();
					foreach (KeyValuePair<string, JsonNode> keyValuePair in jsonObject2)
					{
						string text3;
						JsonNode jsonNode;
						keyValuePair.Deconstruct(ref text3, ref jsonNode);
						string text4 = text3;
						JsonNode value = jsonNode;
						if ((!flag2 || !string.Equals(text4, "version", 5)) && !string.IsNullOrWhiteSpace(text4))
						{
							string text5 = text4.Trim();
							if (text5.Length != 0)
							{
								List<UserConfigurationService.ModConfigPathEntry> list = this.ParseModConfigEntries(value, activeModConfigDirectory, ref flag);
								if (list.Count != 0)
								{
									this._storedModConfigPaths[text5] = list;
									List<string> list2 = Enumerable.ToList<string>(Enumerable.Select<string, string>(Enumerable.Where<string>(Enumerable.Select<UserConfigurationService.ModConfigPathEntry, string>(list, new Func<UserConfigurationService.ModConfigPathEntry, string>(this.BuildFullConfigPath)), (string path) => !string.IsNullOrWhiteSpace(path)), (string path) => path));
									if (list2.Count > 0)
									{
										this.ActiveModConfigPaths[text5] = list2;
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is JsonException || ex is ArgumentException || ex is NotSupportedException)
			{
				this._storedModConfigPaths.Clear();
				flag = false;
			}
			if (flag)
			{
				this.SaveModConfigPathHistory();
			}
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x0004EA9C File Offset: 0x0004CC9C
		[NullableContext(2)]
		[return: Nullable(1)]
		private List<UserConfigurationService.ModConfigPathEntry> ParseModConfigEntries(JsonNode value, string modConfigDirectory, ref bool conversionNeeded)
		{
			List<UserConfigurationService.ModConfigPathEntry> list = new List<UserConfigurationService.ModConfigPathEntry>();
			JsonArray jsonArray = value as JsonArray;
			if (jsonArray != null)
			{
				using (IEnumerator<JsonObject> enumerator = Enumerable.OfType<JsonObject>(jsonArray).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						JsonObject entryObj = enumerator.Current;
						UserConfigurationService.ModConfigPathEntry modConfigPathEntry;
						if (this.TryParseModConfigEntry(entryObj, modConfigDirectory, ref conversionNeeded, out modConfigPathEntry))
						{
							list.Add(modConfigPathEntry);
						}
					}
					return list;
				}
			}
			JsonObject jsonObject = value as JsonObject;
			UserConfigurationService.ModConfigPathEntry modConfigPathEntry2;
			if (jsonObject != null && this.TryParseModConfigEntry(jsonObject, modConfigDirectory, ref conversionNeeded, out modConfigPathEntry2))
			{
				list.Add(modConfigPathEntry2);
			}
			return list;
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x0004EB2C File Offset: 0x0004CD2C
		private bool TryParseModConfigEntry(JsonObject entryObj, [Nullable(2)] string modConfigDirectory, ref bool conversionNeeded, out UserConfigurationService.ModConfigPathEntry entry)
		{
			string optionalString = UserConfigurationService.GetOptionalString(entryObj["directoryPath"]);
			string optionalString2 = UserConfigurationService.GetOptionalString(entryObj["fileName"]);
			string optionalString3 = UserConfigurationService.GetOptionalString(entryObj["configName"]);
			string text = UserConfigurationService.NormalizeStoredModConfigDirectory(optionalString, modConfigDirectory);
			string text2 = UserConfigurationService.NormalizeStoredModConfigFileName(optionalString2);
			string text3 = UserConfigurationService.NormalizeStoredModConfigName(optionalString3);
			if (text == null || text2 == null)
			{
				entry = null;
				return false;
			}
			if (!conversionNeeded)
			{
				string text4 = string.IsNullOrWhiteSpace(optionalString) ? string.Empty : optionalString.Trim();
				if (Path.IsPathRooted(text4))
				{
					conversionNeeded = true;
				}
				else
				{
					string text5 = UserConfigurationService.NormalizeRelativeModConfigDirectory(text4);
					if (text5 == null || !string.Equals(text5, text, UserConfigurationService.PathComparison))
					{
						conversionNeeded = true;
					}
				}
				if (!conversionNeeded && !string.IsNullOrWhiteSpace(optionalString2) && !string.Equals(optionalString2.Trim(), text2, UserConfigurationService.PathComparison))
				{
					conversionNeeded = true;
				}
				if (!conversionNeeded && optionalString3 != null)
				{
					string text6 = optionalString3.Trim();
					if (string.IsNullOrWhiteSpace(text3))
					{
						if (!string.IsNullOrWhiteSpace(text6))
						{
							conversionNeeded = true;
						}
					}
					else if (!string.Equals(text6, text3, 4))
					{
						conversionNeeded = true;
					}
				}
			}
			entry = new UserConfigurationService.ModConfigPathEntry(text, text2, text3);
			return true;
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x0004EC44 File Offset: 0x0004CE44
		private void BackfillModConfigPathHistory()
		{
			bool flag = false;
			foreach (UserConfigurationService.GameProfileState gameProfileState in this._gameProfiles.Values)
			{
				if (gameProfileState.ModConfigPaths.Count != 0)
				{
					string dataDirectory = gameProfileState.DataDirectory;
					if (!string.IsNullOrWhiteSpace(dataDirectory))
					{
						foreach (KeyValuePair<string, List<string>> keyValuePair in gameProfileState.ModConfigPaths)
						{
							if (!string.IsNullOrWhiteSpace(keyValuePair.Key))
							{
								foreach (string path in keyValuePair.Value)
								{
									string text = UserConfigurationService.NormalizePath(path);
									if (!string.IsNullOrWhiteSpace(text))
									{
										string configName = UserConfigurationService.ExtractFileName(text);
										if (this.TryStorePersistentModConfigPath(keyValuePair.Key, text, dataDirectory, configName, false, true))
										{
											flag = true;
										}
									}
								}
							}
						}
					}
				}
			}
			if (flag)
			{
				this.SaveModConfigPathHistory();
				this.RefreshActiveModConfigPathsFromHistory();
			}
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x0004ED8C File Offset: 0x0004CF8C
		private void UpdatePersistentModConfigPaths(string modId, IReadOnlyList<string> normalizedPaths, [Nullable(new byte[]
		{
			1,
			2
		})] IReadOnlyList<string> configNames, bool append)
		{
			string dataDirectory = this.ActiveProfile.DataDirectory;
			bool flag = false;
			if (!append)
			{
				this._storedModConfigPaths.Remove(modId);
			}
			for (int i = 0; i < normalizedPaths.Count; i++)
			{
				string normalizedPath = normalizedPaths[i];
				string configName = (i < configNames.Count) ? configNames[i] : null;
				bool append2 = append || i > 0;
				if (this.TryStorePersistentModConfigPath(modId, normalizedPath, dataDirectory, configName, true, append2))
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.SaveModConfigPathHistory();
			}
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x0004EE10 File Offset: 0x0004D010
		private void RefreshActiveModConfigPathsFromHistory()
		{
			this.ActiveModConfigPaths.Clear();
			if (this._storedModConfigPaths.Count == 0)
			{
				return;
			}
			foreach (KeyValuePair<string, List<UserConfigurationService.ModConfigPathEntry>> keyValuePair in this._storedModConfigPaths)
			{
				List<string> list = Enumerable.ToList<string>(Enumerable.Select<string, string>(Enumerable.Where<string>(Enumerable.Select<UserConfigurationService.ModConfigPathEntry, string>(keyValuePair.Value, new Func<UserConfigurationService.ModConfigPathEntry, string>(this.BuildFullConfigPath)), (string path) => !string.IsNullOrWhiteSpace(path)), (string path) => path));
				if (list.Count != 0)
				{
					this.ActiveModConfigPaths[keyValuePair.Key] = list;
				}
			}
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x0004EEFC File Offset: 0x0004D0FC
		private void SaveModConfigPathHistory()
		{
			this._hasPendingModConfigPathSave = true;
			if (!this._isPersistenceEnabled)
			{
				return;
			}
			this.PersistModConfigPathHistory();
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x0004EF14 File Offset: 0x0004D114
		private void PersistModConfigPathHistory()
		{
			try
			{
				Directory.CreateDirectory(Path.GetDirectoryName(this._modConfigPathsPath));
				string text = UserConfigurationService.NormalizeVersion(this.ModManagerVersion) ?? this.ModManagerVersion;
				JsonObject jsonObject = new JsonObject(default(JsonNodeOptions?));
				foreach (KeyValuePair<string, List<UserConfigurationService.ModConfigPathEntry>> keyValuePair in Enumerable.OrderBy<KeyValuePair<string, List<UserConfigurationService.ModConfigPathEntry>>, string>(this._storedModConfigPaths, (KeyValuePair<string, List<UserConfigurationService.ModConfigPathEntry>> entry) => entry.Key, StringComparer.OrdinalIgnoreCase))
				{
					JsonArray jsonArray = new JsonArray(default(JsonNodeOptions?));
					foreach (UserConfigurationService.ModConfigPathEntry modConfigPathEntry in keyValuePair.Value)
					{
						JsonObject jsonObject2 = new JsonObject(default(JsonNodeOptions?));
						jsonObject2["directoryPath"] = modConfigPathEntry.RelativeDirectoryPath;
						jsonObject2["fileName"] = modConfigPathEntry.FileName;
						JsonObject jsonObject3 = jsonObject2;
						if (!string.IsNullOrWhiteSpace(modConfigPathEntry.ConfigName))
						{
							jsonObject3["configName"] = modConfigPathEntry.ConfigName;
						}
						jsonArray.Add<JsonObject>(jsonObject3);
					}
					jsonObject[keyValuePair.Key] = jsonArray;
				}
				JsonObject jsonObject4 = new JsonObject(default(JsonNodeOptions?));
				jsonObject4["version"] = text;
				jsonObject4["modConfigPaths"] = jsonObject;
				JsonObject jsonObject5 = jsonObject4;
				File.WriteAllText(this._modConfigPathsPath, jsonObject5.ToJsonString(UserConfigurationService.JsonOptions));
				this._hasPendingModConfigPathSave = false;
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException || ex is NotSupportedException)
			{
			}
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x0004F150 File Offset: 0x0004D350
		[return: Nullable(2)]
		private string BuildFullConfigPath(UserConfigurationService.ModConfigPathEntry entry)
		{
			if (entry == null || string.IsNullOrWhiteSpace(entry.FileName))
			{
				return null;
			}
			string activeModConfigDirectory = this.GetActiveModConfigDirectory();
			if (string.IsNullOrWhiteSpace(activeModConfigDirectory))
			{
				return null;
			}
			string text = Path.Combine(string.IsNullOrWhiteSpace(entry.RelativeDirectoryPath) ? activeModConfigDirectory : Path.Combine(activeModConfigDirectory, entry.RelativeDirectoryPath), entry.FileName);
			return UserConfigurationService.NormalizePath(text) ?? text;
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x0004F1B4 File Offset: 0x0004D3B4
		private bool TryStorePersistentModConfigPath(string modId, string normalizedPath, [Nullable(2)] string dataDirectory, [Nullable(2)] string configName, bool removeOnFailure, bool append)
		{
			if (string.IsNullOrWhiteSpace(modId) || string.IsNullOrWhiteSpace(normalizedPath))
			{
				return false;
			}
			string text = modId.Trim();
			if (text.Length == 0)
			{
				return false;
			}
			string text2;
			string text3;
			if (!this.TryGetRelativeModConfigLocation(normalizedPath, dataDirectory, out text2, out text3))
			{
				return removeOnFailure && !string.IsNullOrWhiteSpace(dataDirectory) && this._storedModConfigPaths.Remove(text);
			}
			string text4 = UserConfigurationService.NormalizeStoredModConfigName(configName);
			List<UserConfigurationService.ModConfigPathEntry> list;
			if (!this._storedModConfigPaths.TryGetValue(text, ref list) || list == null)
			{
				list = new List<UserConfigurationService.ModConfigPathEntry>();
				this._storedModConfigPaths[text] = list;
			}
			if (!append)
			{
				list.Clear();
			}
			foreach (UserConfigurationService.ModConfigPathEntry modConfigPathEntry in list)
			{
				if (text4 == null && !string.IsNullOrWhiteSpace(modConfigPathEntry.ConfigName))
				{
					text4 = modConfigPathEntry.ConfigName;
				}
				if (string.Equals(modConfigPathEntry.RelativeDirectoryPath, text2, UserConfigurationService.PathComparison) && string.Equals(modConfigPathEntry.FileName, text3, UserConfigurationService.PathComparison) && string.Equals(modConfigPathEntry.ConfigName, text4, 4))
				{
					return false;
				}
			}
			UserConfigurationService.ModConfigPathEntry modConfigPathEntry2 = new UserConfigurationService.ModConfigPathEntry(text2, text3, text4);
			list.Add(modConfigPathEntry2);
			return true;
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x0004F2F8 File Offset: 0x0004D4F8
		private bool TryGetRelativeModConfigLocation(string normalizedPath, [Nullable(2)] string dataDirectory, out string relativeDirectory, out string fileName)
		{
			relativeDirectory = string.Empty;
			fileName = string.Empty;
			if (string.IsNullOrWhiteSpace(normalizedPath))
			{
				return false;
			}
			string modConfigDirectory = UserConfigurationService.GetModConfigDirectory(dataDirectory);
			if (string.IsNullOrWhiteSpace(modConfigDirectory))
			{
				return false;
			}
			if (!UserConfigurationService.IsPathInDirectory(normalizedPath, modConfigDirectory))
			{
				return false;
			}
			string text = UserConfigurationService.ExtractFileName(normalizedPath);
			if (string.IsNullOrWhiteSpace(text))
			{
				return false;
			}
			fileName = text;
			string text2 = UserConfigurationService.ExtractDirectoryPath(normalizedPath) ?? modConfigDirectory;
			string text3 = UserConfigurationService.NormalizeRelativeModConfigDirectory(Path.GetRelativePath(modConfigDirectory, text2));
			if (text3 == null)
			{
				return false;
			}
			relativeDirectory = text3;
			return true;
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x0004F371 File Offset: 0x0004D571
		[NullableContext(2)]
		private string GetActiveModConfigDirectory()
		{
			return UserConfigurationService.GetModConfigDirectory(this.ActiveProfile.DataDirectory);
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x0004F384 File Offset: 0x0004D584
		[NullableContext(2)]
		private static void EnsureModConfigDirectoryExists(string dataDirectory)
		{
			if (string.IsNullOrWhiteSpace(dataDirectory))
			{
				return;
			}
			try
			{
				Directory.CreateDirectory(Path.Combine(dataDirectory, "ModConfig"));
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (NotSupportedException)
			{
			}
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x0004F3E0 File Offset: 0x0004D5E0
		[NullableContext(2)]
		private static string GetModConfigDirectory(string dataDirectory)
		{
			if (string.IsNullOrWhiteSpace(dataDirectory))
			{
				return null;
			}
			return UserConfigurationService.NormalizePath(Path.Combine(dataDirectory, "ModConfig"));
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x0004F3FC File Offset: 0x0004D5FC
		[NullableContext(2)]
		private static string NormalizeStoredModConfigDirectory(string storedDirectory, string modConfigDirectory)
		{
			if (string.IsNullOrWhiteSpace(storedDirectory))
			{
				return string.Empty;
			}
			string text = storedDirectory.Trim();
			if (!Path.IsPathRooted(text))
			{
				return UserConfigurationService.NormalizeRelativeModConfigDirectory(text);
			}
			string text2 = UserConfigurationService.NormalizePath(text);
			if (text2 == null)
			{
				return null;
			}
			if (!string.IsNullOrWhiteSpace(modConfigDirectory) && UserConfigurationService.IsPathInDirectory(text2, modConfigDirectory))
			{
				return UserConfigurationService.NormalizeRelativeModConfigDirectory(Path.GetRelativePath(modConfigDirectory, text2));
			}
			string text3 = UserConfigurationService.ExtractRelativePathFromModConfigFolder(text2);
			if (text3 == null)
			{
				return null;
			}
			return UserConfigurationService.NormalizeRelativeModConfigDirectory(text3);
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x0004F46C File Offset: 0x0004D66C
		[NullableContext(2)]
		private static string NormalizeStoredModConfigFileName(string fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				return null;
			}
			string fileName2 = Path.GetFileName(fileName.Trim());
			if (string.IsNullOrWhiteSpace(fileName2))
			{
				return null;
			}
			return fileName2;
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x0004F49C File Offset: 0x0004D69C
		[NullableContext(2)]
		private static string NormalizeStoredModConfigName(string configName)
		{
			if (string.IsNullOrWhiteSpace(configName))
			{
				return null;
			}
			string text = configName.Trim();
			if (text.Length == 0)
			{
				return null;
			}
			string text2 = Path.GetFileName(text).Trim();
			if (text2.Length == 0)
			{
				text2 = text;
			}
			if (text2.Length != 0)
			{
				return text2;
			}
			return null;
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x0004F4E5 File Offset: 0x0004D6E5
		[NullableContext(2)]
		private static string NormalizeConfigName(string configName)
		{
			return UserConfigurationService.NormalizeStoredModConfigName(configName);
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x0004F4F0 File Offset: 0x0004D6F0
		[NullableContext(2)]
		private static string NormalizeRelativeModConfigDirectory(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return string.Empty;
			}
			string text = path.Trim();
			if (text.Length == 0 || string.Equals(text, ".", 4))
			{
				return string.Empty;
			}
			string text2 = text.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
			text2 = text2.Trim(UserConfigurationService.DirectorySeparators);
			if (text2.Length == 0)
			{
				return string.Empty;
			}
			string[] array = text2.Split(Path.DirectorySeparatorChar, 1);
			if (array.Length == 0)
			{
				return string.Empty;
			}
			foreach (string text3 in array)
			{
				if (string.Equals(text3, ".", 4) || string.Equals(text3, "..", 4))
				{
					return null;
				}
			}
			return string.Join(Path.DirectorySeparatorChar, array);
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x0004F5B8 File Offset: 0x0004D7B8
		[return: Nullable(2)]
		private static string ExtractRelativePathFromModConfigFolder(string normalizedPath)
		{
			if (string.IsNullOrWhiteSpace(normalizedPath))
			{
				return null;
			}
			string text = normalizedPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
			char directorySeparatorChar = Path.DirectorySeparatorChar;
			string text2 = new ReadOnlySpan<char>(ref directorySeparatorChar) + "ModConfig";
			int i = text.IndexOf(text2, UserConfigurationService.PathComparison);
			while (i >= 0)
			{
				int num = i + text2.Length;
				if (num == text.Length || text.get_Chars(num) == Path.DirectorySeparatorChar)
				{
					if (num < text.Length && text.get_Chars(num) == Path.DirectorySeparatorChar)
					{
						num++;
					}
					if (num >= text.Length)
					{
						return string.Empty;
					}
					return text.Substring(num);
				}
				else
				{
					i = text.IndexOf(text2, num, UserConfigurationService.PathComparison);
				}
			}
			return null;
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0004F680 File Offset: 0x0004D880
		private static bool IsPathInDirectory(string path, string directory)
		{
			if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(directory))
			{
				return false;
			}
			string text = UserConfigurationService.NormalizePath(path);
			string text2 = UserConfigurationService.NormalizePath(directory);
			if (text == null || text2 == null)
			{
				return false;
			}
			if (!text2.EndsWith(Path.DirectorySeparatorChar))
			{
				ReadOnlySpan<char> readOnlySpan = text2;
				char directorySeparatorChar = Path.DirectorySeparatorChar;
				text2 = readOnlySpan + new ReadOnlySpan<char>(ref directorySeparatorChar);
			}
			return text.StartsWith(text2, UserConfigurationService.PathComparison);
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x0004F6E8 File Offset: 0x0004D8E8
		[return: Nullable(2)]
		private static string ExtractDirectoryPath(string normalizedPath)
		{
			string text = Path.GetDirectoryName(normalizedPath);
			if (string.IsNullOrWhiteSpace(text))
			{
				text = Path.GetPathRoot(normalizedPath);
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				return null;
			}
			return UserConfigurationService.NormalizePath(text);
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x0004F71C File Offset: 0x0004D91C
		[return: Nullable(2)]
		private static string ExtractFileName(string normalizedPath)
		{
			string text = normalizedPath.Trim();
			string fileName = Path.GetFileName(text);
			if (string.IsNullOrWhiteSpace(fileName))
			{
				fileName = Path.GetFileName(text.TrimEnd(new char[]
				{
					Path.DirectorySeparatorChar,
					Path.AltDirectorySeparatorChar
				}));
			}
			if (string.IsNullOrWhiteSpace(fileName))
			{
				return null;
			}
			return fileName;
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x0004F76C File Offset: 0x0004D96C
		private static string GetPreferredConfigurationDirectory()
		{
			string customConfigFolder = CustomConfigFolderManager.GetCustomConfigFolder();
			if (!string.IsNullOrWhiteSpace(customConfigFolder))
			{
				return customConfigFolder;
			}
			string folder = UserConfigurationService.GetFolder(28);
			if (!string.IsNullOrWhiteSpace(folder))
			{
				return Path.Combine(folder, "Simple VS Manager");
			}
			string folder2 = UserConfigurationService.GetFolder(26);
			if (!string.IsNullOrWhiteSpace(folder2))
			{
				return Path.Combine(folder2, "Simple VS Manager");
			}
			string folder3 = UserConfigurationService.GetFolder(5);
			if (!string.IsNullOrWhiteSpace(folder3))
			{
				return Path.Combine(folder3, "Simple VS Manager");
			}
			string folder4 = UserConfigurationService.GetFolder(5);
			if (!string.IsNullOrWhiteSpace(folder4))
			{
				return Path.Combine(folder4, "Simple VS Manager");
			}
			string folder5 = UserConfigurationService.GetFolder(40);
			if (!string.IsNullOrWhiteSpace(folder5))
			{
				return Path.Combine(folder5, ".simple-vs-manager");
			}
			return Path.Combine(AppContext.BaseDirectory, "Simple VS Manager");
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0004F828 File Offset: 0x0004DA28
		[NullableContext(2)]
		private static string GetFolder(Environment.SpecialFolder folder)
		{
			try
			{
				string folderPath = Environment.GetFolderPath(folder, 16384);
				if (!string.IsNullOrWhiteSpace(folderPath))
				{
					return folderPath;
				}
			}
			catch (PlatformNotSupportedException)
			{
				return null;
			}
			return null;
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x0004F868 File Offset: 0x0004DA68
		private static bool TryNormalizeHexColor([Nullable(2)] string value, out string normalized)
		{
			normalized = string.Empty;
			if (string.IsNullOrWhiteSpace(value))
			{
				return false;
			}
			string text = value.Trim();
			if (!text.StartsWith('#') || text.Length <= 1)
			{
				return false;
			}
			string text2 = text.Substring(1);
			int i = text2.Length;
			if (i != 6 && i != 8)
			{
				return false;
			}
			string text3 = text2;
			for (i = 0; i < text3.Length; i++)
			{
				if (!UserConfigurationService.IsHexDigit(text3.get_Chars(i)))
				{
					return false;
				}
			}
			normalized = "#" + text2.ToUpperInvariant();
			return true;
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x0004F8F1 File Offset: 0x0004DAF1
		private static bool IsHexDigit(char c)
		{
			return (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f');
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0004F918 File Offset: 0x0004DB18
		[NullableContext(2)]
		private static string GetOptionalString(JsonNode node)
		{
			if (node == null)
			{
				return null;
			}
			string result;
			try
			{
				result = node.GetValue<string>();
			}
			catch (InvalidOperationException)
			{
				result = null;
			}
			catch (FormatException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x0004F95C File Offset: 0x0004DB5C
		private static string ResolveCurrentVersion()
		{
			Assembly assembly = typeof(UserConfigurationService).Assembly;
			AssemblyInformationalVersionAttribute customAttribute = CustomAttributeExtensions.GetCustomAttribute<AssemblyInformationalVersionAttribute>(assembly);
			string text = UserConfigurationService.TrimToSemanticVersion(VersionStringUtility.Normalize((customAttribute != null) ? customAttribute.InformationalVersion : null));
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text;
			}
			AssemblyFileVersionAttribute customAttribute2 = CustomAttributeExtensions.GetCustomAttribute<AssemblyFileVersionAttribute>(assembly);
			text = UserConfigurationService.TrimToSemanticVersion(VersionStringUtility.Normalize((customAttribute2 != null) ? customAttribute2.Version : null));
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text;
			}
			Version version = assembly.GetName().Version;
			text = UserConfigurationService.TrimToSemanticVersion(VersionStringUtility.Normalize((version != null) ? version.ToString() : null));
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text;
			}
			return "0.0.0";
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x0004F9F8 File Offset: 0x0004DBF8
		[NullableContext(2)]
		private static string TrimToSemanticVersion(string version)
		{
			if (string.IsNullOrWhiteSpace(version))
			{
				return null;
			}
			string[] array = version.Split('.', 1);
			if (array.Length == 0)
			{
				return null;
			}
			if (array.Length <= 3)
			{
				return string.Join('.', array);
			}
			return string.Join<string>('.', Enumerable.Take<string>(array, 3));
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x0004FA3C File Offset: 0x0004DC3C
		[NullableContext(2)]
		private static string NormalizeVersion(string version)
		{
			if (string.IsNullOrWhiteSpace(version))
			{
				return null;
			}
			string text = VersionStringUtility.Normalize(version);
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text;
			}
			return null;
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x0004FA68 File Offset: 0x0004DC68
		[NullableContext(2)]
		private static string NormalizePath(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return null;
			}
			string result;
			try
			{
				result = Path.GetFullPath(path);
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0004FAA0 File Offset: 0x0004DCA0
		[NullableContext(2)]
		private static string NormalizeSortMemberPath(string sortMemberPath)
		{
			if (string.IsNullOrWhiteSpace(sortMemberPath))
			{
				return null;
			}
			string text = sortMemberPath.Trim();
			if (string.Equals(text, "DisplayName", 5))
			{
				return "NameSortKey";
			}
			return text;
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x0004FAD3 File Offset: 0x0004DCD3
		[NullableContext(2)]
		private static string NormalizeInstalledColumnKey(string columnName)
		{
			if (string.IsNullOrWhiteSpace(columnName))
			{
				return null;
			}
			return columnName.Trim();
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x0004FAE5 File Offset: 0x0004DCE5
		[NullableContext(2)]
		private static string NormalizeModId(string modId)
		{
			if (!string.IsNullOrWhiteSpace(modId))
			{
				return modId.Trim();
			}
			return null;
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x0004FAF7 File Offset: 0x0004DCF7
		[NullableContext(2)]
		private static string NormalizeOptionalSortMemberPath(string sortMemberPath)
		{
			if (!string.IsNullOrWhiteSpace(sortMemberPath))
			{
				return sortMemberPath.Trim();
			}
			return null;
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x0004FB0C File Offset: 0x0004DD0C
		[NullableContext(2)]
		private static ListSortDirection ParseSortDirection(string value)
		{
			ListSortDirection result;
			if (Enum.TryParse<ListSortDirection>(value, true, ref result))
			{
				return result;
			}
			return 0;
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x0004FB27 File Offset: 0x0004DD27
		[NullableContext(2)]
		private static string NormalizePresetName(string name)
		{
			if (!string.IsNullOrWhiteSpace(name))
			{
				return name.Trim();
			}
			return null;
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0004FB39 File Offset: 0x0004DD39
		[NullableContext(2)]
		private static string NormalizeUploaderName(string name)
		{
			if (!string.IsNullOrWhiteSpace(name))
			{
				return name.Trim();
			}
			return null;
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0004FB4C File Offset: 0x0004DD4C
		// Note: this type is marked as 'beforefieldinit'.
		static UserConfigurationService()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			dictionary["Palette.BaseSurface.Brighter"] = "#FF735B43";
			dictionary["Palette.BaseSurface.Shadowed"] = "#FF403529";
			dictionary["Palette.BaseSurface.Raised"] = "#FF4D3D2D";
			dictionary["Palette.BaseSurface.HoverGlow"] = "#FF5A4530";
			dictionary["Palette.Interactive.Surface"] = "#FF453525";
			dictionary["Palette.Accent.Primary"] = "#FF479BBE";
			dictionary["Palette.Interactive.DisabledSurface"] = "#FF332A21";
			dictionary["Palette.Text.Primary"] = "#FFC8BCAE";
			dictionary["Palette.Text.Link"] = "#FF479BBE";
			dictionary["Palette.Bevel.Highlight"] = "#80FFFFFF";
			dictionary["Palette.Bevel.Shadow"] = "#40000000";
			dictionary["Palette.Overlay.HoverTint"] = "#10FFFFFF";
			dictionary["Palette.White"] = "#FFfffcf5";
			dictionary["Palette.Grey"] = "#FF867e74";
			dictionary["Palette.DarkGrey"] = "#FF6e675f";
			dictionary["Palette.Error"] = "#FFED4337";
			UserConfigurationService.VintageStoryPaletteColors = dictionary;
			Dictionary<ColorTheme, string> dictionary2 = new Dictionary<ColorTheme, string>();
			dictionary2[ColorTheme.VintageStory] = "Vintage Story";
			dictionary2[ColorTheme.Dark] = "Dark";
			dictionary2[ColorTheme.Light] = "Light";
			dictionary2[ColorTheme.SurpriseMe] = "Surprise Me";
			dictionary2[ColorTheme.Custom] = "Custom";
			UserConfigurationService.BuiltInThemeNames = dictionary2;
			Dictionary<string, string> dictionary3 = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			dictionary3["Palette.Accent.Primary"] = "#FF0078D4";
			dictionary3["Palette.BaseSurface.Brighter"] = "#FF525252";
			dictionary3["Palette.BaseSurface.Shadowed"] = "#FF202020";
			dictionary3["Palette.BaseSurface.HoverGlow"] = "#FF323232";
			dictionary3["Palette.BaseSurface.Raised"] = "#FF2B2B2B";
			dictionary3["Palette.Bevel.Shadow"] = "#26000000";
			dictionary3["Palette.Bevel.Highlight"] = "#21FFFFFF";
			dictionary3["Palette.Interactive.DisabledSurface"] = "#FF2A2A2A";
			dictionary3["Palette.Interactive.Surface"] = "#FF2E2E2E";
			dictionary3["Palette.Overlay.HoverTint"] = "#14FFFFFF";
			dictionary3["Palette.Text.Link"] = "#FF0F6CBD";
			dictionary3["Palette.Text.Primary"] = "#FFEDEDED";
			dictionary3["Palette.White"] = "#FFFFFFFF";
			dictionary3["Palette.Grey"] = "#FF808080";
			dictionary3["Palette.DarkGrey"] = "#FF505050";
			dictionary3["Palette.Error"] = "#FFED4337";
			UserConfigurationService.DarkPaletteColors = dictionary3;
			Dictionary<string, string> dictionary4 = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			dictionary4["Palette.Accent.Primary"] = "#FF0078D4";
			dictionary4["Palette.BaseSurface.Brighter"] = "#FFA6AAAD";
			dictionary4["Palette.BaseSurface.HoverGlow"] = "#FFE0EAF5";
			dictionary4["Palette.BaseSurface.Raised"] = "#FFFFFFFF";
			dictionary4["Palette.BaseSurface.Shadowed"] = "#FFD0DBE5";
			dictionary4["Palette.Bevel.Highlight"] = "#80FFFFFF";
			dictionary4["Palette.Bevel.Shadow"] = "#66000000";
			dictionary4["Palette.Interactive.DisabledSurface"] = "#FFBAC5D0";
			dictionary4["Palette.Interactive.Surface"] = "#FFE5F0FA";
			dictionary4["Palette.Overlay.HoverTint"] = "#20000000";
			dictionary4["Palette.Text.Link"] = "#FF0078D4";
			dictionary4["Palette.Text.Primary"] = "#FF000000";
			dictionary4["Palette.White"] = "#FFFFFFFF";
			dictionary4["Palette.Grey"] = "#FF9E9E9E";
			dictionary4["Palette.DarkGrey"] = "#FF6E6E6E";
			dictionary4["Palette.Error"] = "#FFED4337";
			UserConfigurationService.LightPaletteColors = dictionary4;
			UserConfigurationService.DefaultModDatabaseSearchResultLimit = DevConfig.DefaultModDatabaseSearchResultLimit;
			UserConfigurationService.DefaultModDatabaseNewModsRecentMonths = DevConfig.DefaultModDatabaseNewModsRecentMonths;
			UserConfigurationService.MaxModDatabaseNewModsRecentMonths = DevConfig.MaxModDatabaseNewModsRecentMonths;
			UserConfigurationService.GameSessionVoteThreshold = DevConfig.GameSessionVoteThreshold;
			UserConfigurationService.RedundantRootProfilePropertyNames = new string[]
			{
				"dataDirectory",
				"gameDirectory",
				"bulkUpdateModExclusions",
				"skippedModVersions",
				"dependencyWarningOverrides",
				"modConfigPaths",
				"modUsageTracking",
				"customShortcutPath"
			};
		}

		// Token: 0x04000564 RID: 1380
		private const string ModConfigDirectoryName = "ModConfig";

		// Token: 0x04000565 RID: 1381
		private const string ModConfigPathHistoryVersionPropertyName = "version";

		// Token: 0x04000566 RID: 1382
		private const string ModConfigPathHistoryEntriesPropertyName = "modConfigPaths";

		// Token: 0x04000567 RID: 1383
		private const string ModConfigPathHistoryDirectoryPropertyName = "directoryPath";

		// Token: 0x04000568 RID: 1384
		private const string ModConfigPathHistoryFileNamePropertyName = "fileName";

		// Token: 0x04000569 RID: 1385
		private const string ModConfigPathHistoryConfigNamePropertyName = "configName";

		// Token: 0x0400056A RID: 1386
		private const string DefaultGameProfileName = "Default";

		// Token: 0x0400056B RID: 1387
		public const string DefaultProfileName = "Default";

		// Token: 0x0400056C RID: 1388
		private static readonly string ConfigurationFileName = DevConfig.ConfigurationFileName;

		// Token: 0x0400056D RID: 1389
		private static readonly string ModConfigPathsFileName = DevConfig.ModConfigPathsFileName;

		// Token: 0x0400056E RID: 1390
		private static readonly char[] DirectorySeparators = new char[]
		{
			Path.DirectorySeparatorChar,
			Path.AltDirectorySeparatorChar
		};

		// Token: 0x0400056F RID: 1391
		private static readonly StringComparison PathComparison = OperatingSystem.IsWindows() ? 5 : 4;

		// Token: 0x04000570 RID: 1392
		private static readonly string CurrentModManagerVersion = UserConfigurationService.ResolveCurrentVersion();

		// Token: 0x04000571 RID: 1393
		private static readonly string CurrentConfigurationVersion = UserConfigurationService.CurrentModManagerVersion;

		// Token: 0x04000572 RID: 1394
		private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
		{
			WriteIndented = true,
			TypeInfoResolver = new DefaultJsonTypeInfoResolver()
		};

		// Token: 0x04000573 RID: 1395
		private static readonly IReadOnlyDictionary<string, string> VintageStoryPaletteColors;

		// Token: 0x04000574 RID: 1396
		private static readonly IReadOnlyDictionary<ColorTheme, string> BuiltInThemeNames;

		// Token: 0x04000575 RID: 1397
		private static readonly IReadOnlyDictionary<string, string> DarkPaletteColors;

		// Token: 0x04000576 RID: 1398
		private static readonly IReadOnlyDictionary<string, string> LightPaletteColors;

		// Token: 0x04000577 RID: 1399
		private static readonly int DefaultModDatabaseSearchResultLimit;

		// Token: 0x04000578 RID: 1400
		private static readonly int DefaultModDatabaseNewModsRecentMonths;

		// Token: 0x04000579 RID: 1401
		private static readonly int MaxModDatabaseNewModsRecentMonths;

		// Token: 0x0400057A RID: 1402
		private static readonly int GameSessionVoteThreshold;

		// Token: 0x0400057B RID: 1403
		private static readonly string[] RedundantRootProfilePropertyNames;

		// Token: 0x0400057C RID: 1404
		private readonly string _configurationPath;

		// Token: 0x0400057D RID: 1405
		private readonly Dictionary<string, string> _customThemePaletteColors = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400057E RID: 1406
		private readonly Dictionary<string, UserConfigurationService.GameProfileState> _gameProfiles = new Dictionary<string, UserConfigurationService.GameProfileState>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400057F RID: 1407
		private readonly Dictionary<string, bool> _installedColumnVisibility = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000580 RID: 1408
		private readonly string _modConfigPathsPath;

		// Token: 0x04000581 RID: 1409
		private readonly Dictionary<string, Dictionary<string, string>> _savedCustomThemes = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000582 RID: 1410
		private readonly Dictionary<string, List<UserConfigurationService.ModConfigPathEntry>> _storedModConfigPaths = new Dictionary<string, List<UserConfigurationService.ModConfigPathEntry>>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000583 RID: 1411
		private readonly Dictionary<string, string> _themePaletteColors = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000584 RID: 1412
		private bool _hasPendingModConfigPathSave;

		// Token: 0x04000585 RID: 1413
		private bool _hasPendingSave;

		// Token: 0x04000586 RID: 1414
		private bool _isModUsageTrackingDisabled;

		// Token: 0x04000587 RID: 1415
		private bool _isPersistenceEnabled;

		// Token: 0x04000588 RID: 1416
		private ModlistsTabSelection _preferredModlistsTab;

		// Token: 0x04000589 RID: 1417
		private double? _modInfoPanelLeft;

		// Token: 0x0400058A RID: 1418
		private double? _modInfoPanelTop;

		// Token: 0x0400058B RID: 1419
		private ListSortDirection _localModlistsSortDirection;

		// Token: 0x0400058C RID: 1420
		[Nullable(2)]
		private string _localModlistsSortMemberPath;

		// Token: 0x0400058D RID: 1421
		private ListSortDirection _modsSortDirection;

		// Token: 0x0400058E RID: 1422
		[Nullable(2)]
		private string _modsSortMemberPath;

		// Token: 0x0400058F RID: 1423
		private ListSortDirection _onlineModlistsSortDirection;

		// Token: 0x04000590 RID: 1424
		[Nullable(2)]
		private string _onlineModlistsSortMemberPath;

		// Token: 0x04000591 RID: 1425
		[Nullable(2)]
		private string _currentThemeName;

		// Token: 0x04000592 RID: 1426
		[Nullable(2)]
		private string _selectedPresetName;

		// Token: 0x04000593 RID: 1427
		private bool _suppressRefreshCachePrompt;

		// Token: 0x04000594 RID: 1428
		[Nullable(2)]
		private string _suppressRefreshCachePromptVersion;

		// Token: 0x04000595 RID: 1429
		private double? _windowHeight;

		// Token: 0x04000596 RID: 1430
		private double? _windowLeft;

		// Token: 0x04000597 RID: 1431
		private double? _windowTop;

		// Token: 0x04000598 RID: 1432
		private double? _windowWidth;

		// Token: 0x02000309 RID: 777
		[Nullable(0)]
		private sealed class GameProfileState
		{
			// Token: 0x06001928 RID: 6440 RVA: 0x00092130 File Offset: 0x00090330
			public GameProfileState(string name)
			{
				this.Name = name;
			}

			// Token: 0x17000539 RID: 1337
			// (get) Token: 0x06001929 RID: 6441 RVA: 0x00092195 File Offset: 0x00090395
			public string Name { get; }

			// Token: 0x1700053A RID: 1338
			// (get) Token: 0x0600192A RID: 6442 RVA: 0x0009219D File Offset: 0x0009039D
			// (set) Token: 0x0600192B RID: 6443 RVA: 0x000921A5 File Offset: 0x000903A5
			[Nullable(2)]
			public string DataDirectory { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x1700053B RID: 1339
			// (get) Token: 0x0600192C RID: 6444 RVA: 0x000921AE File Offset: 0x000903AE
			// (set) Token: 0x0600192D RID: 6445 RVA: 0x000921B6 File Offset: 0x000903B6
			[Nullable(2)]
			public string GameDirectory { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x1700053C RID: 1340
			// (get) Token: 0x0600192E RID: 6446 RVA: 0x000921BF File Offset: 0x000903BF
			// (set) Token: 0x0600192F RID: 6447 RVA: 0x000921C7 File Offset: 0x000903C7
			[Nullable(2)]
			public string TargetGameVersionOverride { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x1700053D RID: 1341
			// (get) Token: 0x06001930 RID: 6448 RVA: 0x000921D0 File Offset: 0x000903D0
			// (set) Token: 0x06001931 RID: 6449 RVA: 0x000921D8 File Offset: 0x000903D8
			[Nullable(2)]
			public string CustomShortcutPath { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x1700053E RID: 1342
			// (get) Token: 0x06001932 RID: 6450 RVA: 0x000921E1 File Offset: 0x000903E1
			// (set) Token: 0x06001933 RID: 6451 RVA: 0x000921E9 File Offset: 0x000903E9
			public bool RequiresDataDirectorySelection { get; set; }

			// Token: 0x1700053F RID: 1343
			// (get) Token: 0x06001934 RID: 6452 RVA: 0x000921F2 File Offset: 0x000903F2
			// (set) Token: 0x06001935 RID: 6453 RVA: 0x000921FA File Offset: 0x000903FA
			public bool RequiresGameDirectorySelection { get; set; }

			// Token: 0x17000540 RID: 1344
			// (get) Token: 0x06001936 RID: 6454 RVA: 0x00092203 File Offset: 0x00090403
			public Dictionary<string, List<string>> ModConfigPaths { get; } = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

			// Token: 0x17000541 RID: 1345
			// (get) Token: 0x06001937 RID: 6455 RVA: 0x0009220B File Offset: 0x0009040B
			public Dictionary<string, bool> BulkUpdateModExclusions { get; } = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

			// Token: 0x17000542 RID: 1346
			// (get) Token: 0x06001938 RID: 6456 RVA: 0x00092213 File Offset: 0x00090413
			public Dictionary<string, string> SkippedModVersions { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

			// Token: 0x17000543 RID: 1347
			// (get) Token: 0x06001939 RID: 6457 RVA: 0x0009221B File Offset: 0x0009041B
			public Dictionary<string, string> DependencyWarningOverrides { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

			// Token: 0x17000544 RID: 1348
			// (get) Token: 0x0600193A RID: 6458 RVA: 0x00092223 File Offset: 0x00090423
			public Dictionary<ModUsageTrackingKey, int> ModUsageSessionCounts { get; } = new Dictionary<ModUsageTrackingKey, int>();

			// Token: 0x17000545 RID: 1349
			// (get) Token: 0x0600193B RID: 6459 RVA: 0x0009222B File Offset: 0x0009042B
			// (set) Token: 0x0600193C RID: 6460 RVA: 0x00092233 File Offset: 0x00090433
			public int LongRunningSessionCount { get; set; }

			// Token: 0x17000546 RID: 1350
			// (get) Token: 0x0600193D RID: 6461 RVA: 0x0009223C File Offset: 0x0009043C
			// (set) Token: 0x0600193E RID: 6462 RVA: 0x00092244 File Offset: 0x00090444
			public bool HasPendingModUsagePrompt { get; set; }
		}

		// Token: 0x0200030A RID: 778
		[Nullable(0)]
		private sealed class ModConfigPathEntry
		{
			// Token: 0x0600193F RID: 6463 RVA: 0x0009224D File Offset: 0x0009044D
			public ModConfigPathEntry(string relativeDirectoryPath, string fileName, [Nullable(2)] string configName)
			{
				this.RelativeDirectoryPath = relativeDirectoryPath;
				this.FileName = fileName;
				this.ConfigName = configName;
			}

			// Token: 0x17000547 RID: 1351
			// (get) Token: 0x06001940 RID: 6464 RVA: 0x0009226A File Offset: 0x0009046A
			public string RelativeDirectoryPath { get; }

			// Token: 0x17000548 RID: 1352
			// (get) Token: 0x06001941 RID: 6465 RVA: 0x00092272 File Offset: 0x00090472
			public string FileName { get; }

			// Token: 0x17000549 RID: 1353
			// (get) Token: 0x06001942 RID: 6466 RVA: 0x0009227A File Offset: 0x0009047A
			[Nullable(2)]
			public string ConfigName { [NullableContext(2)] get; }
		}
	}
}
