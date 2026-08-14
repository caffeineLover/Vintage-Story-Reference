using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services;

namespace VintageStoryModManager.ViewModels
{
	// Token: 0x0200004C RID: 76
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModListItemViewModel : ObservableObject
	{
		// Token: 0x0600077D RID: 1917 RVA: 0x0002F73C File Offset: 0x0002D93C
		[NullableContext(2)]
		public ModListItemViewModel([Nullable(1)] ModEntry entry, bool isActive, [Nullable(1)] string location, [Nullable(1)] Func<ModListItemViewModel, bool, Task<ActivationResult>> activationHandler, string installedGameVersion = null, bool isInstalled = false, Func<string, string, bool> shouldSkipVersion = null, Func<bool> requireExactVersionMatch = null, bool initializeUserReportState = true, ModLoadingTimingService timingService = null, Func<string, string, bool> isDependencyWarningOverridden = null)
		{
			ArgumentNullException.ThrowIfNull(entry, "entry");
			if (activationHandler == null)
			{
				throw new ArgumentNullException("activationHandler");
			}
			this._activationHandler = activationHandler;
			this._installedGameVersion = installedGameVersion;
			this._shouldSkipVersion = shouldSkipVersion;
			this._requireExactVersionMatch = requireExactVersionMatch;
			this._isDependencyWarningOverridden = isDependencyWarningOverridden;
			this._timingService = timingService;
			this._modEntry = entry;
			this._constructorLocation = location;
			this.ModId = entry.ModId;
			this.DisplayName = (string.IsNullOrWhiteSpace(entry.Name) ? entry.ModId : entry.Name);
			this.NameSortKey = (string.IsNullOrWhiteSpace(entry.ManifestName) ? this.DisplayName : entry.ManifestName);
			this.Version = entry.Version;
			this.NetworkVersion = entry.NetworkVersion;
			this.Website = entry.Website;
			this.SourcePath = entry.SourcePath;
			this.DateModified = ModListItemViewModel.GetDateModifiedTimestamp(this.SourcePath);
			this.Location = location;
			this.IsModDatabaseEntry = string.Equals(location, "Mod Database", 4);
			this.SourceKind = entry.SourceKind;
			this._authors = entry.Authors;
			this._contributors = entry.Contributors;
			ModDatabaseInfo databaseInfo = entry.DatabaseInfo;
			this.DatabaseTags = (((databaseInfo != null) ? databaseInfo.Tags : null) ?? Array.Empty<string>());
			this._databaseRequiredGameVersions = (((databaseInfo != null) ? databaseInfo.RequiredGameVersions : null) ?? Array.Empty<string>());
			this.LatestRelease = ((databaseInfo != null) ? databaseInfo.LatestRelease : null);
			this.LatestCompatibleRelease = ((databaseInfo != null) ? databaseInfo.LatestCompatibleRelease : null);
			this._releases = (((databaseInfo != null) ? databaseInfo.Releases : null) ?? Array.Empty<ModReleaseInfo>());
			this.ModDatabaseAssetId = ((databaseInfo != null) ? databaseInfo.AssetId : null);
			this.ModDatabasePageUrl = ((databaseInfo != null) ? databaseInfo.ModPageUrl : null);
			this.ModDatabasePageUri = ModListItemViewModel.TryCreateHttpUri(this.ModDatabasePageUrl);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(44, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Initial database page URL '");
			defaultInterpolatedStringHandler.AppendFormatted(ModListItemViewModel.FormatValue(this.ModDatabasePageUrl));
			defaultInterpolatedStringHandler.AppendLiteral("' resolved to '");
			defaultInterpolatedStringHandler.AppendFormatted(ModListItemViewModel.FormatUri(this.ModDatabasePageUri));
			defaultInterpolatedStringHandler.AppendLiteral("'.");
			ModListItemViewModel.LogDebug(defaultInterpolatedStringHandler.ToStringAndClear());
			if (this.ModDatabasePageUri != null)
			{
				Uri commandUri = this.ModDatabasePageUri;
				this._openModDatabasePageCommand = new RelayCommand(delegate()
				{
					ModListItemViewModel.LaunchUri(commandUri);
				});
			}
			this._databaseDownloads = ((databaseInfo != null) ? databaseInfo.Downloads : default(int?));
			this._databaseComments = ((databaseInfo != null) ? databaseInfo.Comments : default(int?));
			this._databaseRecentDownloads = ((databaseInfo != null) ? databaseInfo.DownloadsLastThirtyDays : default(int?));
			this._databaseTenDayDownloads = ((databaseInfo != null) ? databaseInfo.DownloadsLastTenDays : default(int?));
			this._modDatabaseLogoSource = ((databaseInfo != null) ? databaseInfo.LogoUrlSource : null);
			this._modDatabaseLogoUrl = ((databaseInfo != null) ? databaseInfo.LogoUrl : null);
			this._modDatabaseLogo = null;
			ModListItemViewModel.LogDebug("Deferred database logo creation. Source URL: '" + ModListItemViewModel.FormatValue(this._modDatabaseLogoUrl) + "'.");
			this.ModDatabaseRelevancySortKey = entry.ModDatabaseSearchScore.GetValueOrDefault();
			DateTime? dateTime = (databaseInfo != null) ? databaseInfo.LastReleasedUtc : default(DateTime?);
			this._modDatabaseLastUpdatedUtc = ((dateTime != null) ? dateTime : ModListItemViewModel.DetermineLastUpdatedFromReleases(this._releases));
			int? num = this._databaseRecentDownloads;
			if (num == null)
			{
				this._databaseRecentDownloads = ModListItemViewModel.CalculateDownloadsLastThirtyDaysFromReleases(this._releases);
			}
			num = this._databaseTenDayDownloads;
			if (num == null)
			{
				this._databaseTenDayDownloads = ModListItemViewModel.CalculateDownloadsLastTenDaysFromReleases(this._releases);
			}
			ModReleaseInfo latestCompatibleRelease = this.LatestCompatibleRelease;
			string latestDatabaseVersion;
			if ((latestDatabaseVersion = ((latestCompatibleRelease != null) ? latestCompatibleRelease.Version : null)) == null)
			{
				latestDatabaseVersion = (((databaseInfo != null) ? databaseInfo.LatestCompatibleVersion : null) ?? this.Version);
			}
			this.LatestDatabaseVersion = latestDatabaseVersion;
			string version = (!string.IsNullOrWhiteSpace(entry.Version)) ? entry.Version : ModListItemViewModel.SelectPreferredUserReportVersion(this.LatestRelease, this.LatestCompatibleRelease, databaseInfo);
			this.SetUserReportVersion(version, false);
			this._loadError = entry.LoadError;
			this.IsInstalled = isInstalled;
			this.WebsiteUri = ModListItemViewModel.TryCreateHttpUri(this.Website);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
			defaultInterpolatedStringHandler2..ctor(30, 2);
			defaultInterpolatedStringHandler2.AppendLiteral("Website URL '");
			defaultInterpolatedStringHandler2.AppendFormatted(ModListItemViewModel.FormatValue(this.Website));
			defaultInterpolatedStringHandler2.AppendLiteral("' resolved to '");
			defaultInterpolatedStringHandler2.AppendFormatted(ModListItemViewModel.FormatUri(this.WebsiteUri));
			defaultInterpolatedStringHandler2.AppendLiteral("'.");
			ModListItemViewModel.LogDebug(defaultInterpolatedStringHandler2.ToStringAndClear());
			this.OpenWebsiteCommand = ((this.WebsiteUri != null) ? new RelayCommand(delegate()
			{
				ModListItemViewModel.LaunchUri(this.WebsiteUri);
			}) : null);
			IReadOnlyList<ModDependencyInfo> readOnlyList = entry.Dependencies ?? Array.Empty<ModDependencyInfo>();
			ModDependencyInfo gameDependency;
			if ((gameDependency = Enumerable.FirstOrDefault<ModDependencyInfo>(readOnlyList, (ModDependencyInfo d) => string.Equals(d.ModId, "game", 5))) == null)
			{
				gameDependency = Enumerable.FirstOrDefault<ModDependencyInfo>(readOnlyList, (ModDependencyInfo d) => d.IsGameOrCoreDependency);
			}
			this._gameDependency = gameDependency;
			if (readOnlyList.Count == 0)
			{
				this.Dependencies = Array.Empty<ModDependencyInfo>();
			}
			else
			{
				ModDependencyInfo[] array = Enumerable.ToArray<ModDependencyInfo>(Enumerable.Where<ModDependencyInfo>(readOnlyList, (ModDependencyInfo d) => !d.IsGameOrCoreDependency));
				this.Dependencies = ((array.Length == 0) ? Array.Empty<ModDependencyInfo>() : array);
			}
			IReadOnlyList<ModDependencyInfo> missingDependencies = entry.MissingDependencies;
			this.MissingDependencies = ((missingDependencies != null && missingDependencies.Count > 0) ? Enumerable.ToArray<ModDependencyInfo>(entry.MissingDependencies) : Array.Empty<ModDependencyInfo>());
			this.DependencyHasErrors = entry.DependencyHasErrors;
			this._description = entry.Description;
			this._metadataError = entry.Error;
			this.Side = entry.Side;
			this.RequiredOnClient = entry.RequiredOnClient;
			this.RequiredOnServer = entry.RequiredOnServer;
			ModDatabaseInfo databaseInfo2 = entry.DatabaseInfo;
			this._modDatabaseSide = ((databaseInfo2 != null) ? databaseInfo2.Side : null);
			this._icon = null;
			this._iconInitialized = false;
			ModListItemViewModel.LogDebug("Deferred icon image creation. Will be created on first access.");
			this._isActive = isActive;
			this.HasErrors = entry.HasErrors;
			this._dependencyWarningOverridden = this.ResolveDependencyWarningOverride();
			this.InitializeUpdateAvailability();
			this.InitializeVersionOptions();
			this.InitializeVersionWarning(this._installedGameVersion);
			this.UpdateNewerReleaseChangelogs();
			this.UpdateStatusFromErrors();
			this.UpdateTooltip();
			if (initializeUserReportState)
			{
				this.InitializeUserReportState(this._installedGameVersion, this.UserReportModVersion);
			}
			else
			{
				this._userReportDisplay = string.Empty;
				this._userReportTooltip = null;
				this._hasInitializedUserReportState = false;
			}
			this._searchIndex = null;
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600077E RID: 1918 RVA: 0x0002FE3B File Offset: 0x0002E03B
		public string ModId { get; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x0002FE43 File Offset: 0x0002E043
		public string DisplayName { get; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000780 RID: 1920 RVA: 0x0002FE4B File Offset: 0x0002E04B
		public string NameSortKey { get; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x0002FE53 File Offset: 0x0002E053
		[Nullable(2)]
		public string Version { [NullableContext(2)] get; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000782 RID: 1922 RVA: 0x0002FE5B File Offset: 0x0002E05B
		public string VersionDisplay
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.Version))
				{
					return this.Version;
				}
				return "—";
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x0002FE76 File Offset: 0x0002E076
		[Nullable(2)]
		public string NetworkVersion { [NullableContext(2)] get; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000784 RID: 1924 RVA: 0x0002FE80 File Offset: 0x0002E080
		public string GameVersionDisplay
		{
			get
			{
				ModDependencyInfo gameDependency = this._gameDependency;
				if (gameDependency != null)
				{
					string version = gameDependency.Version;
					string text = ((version != null) ? version.Trim() : null) ?? string.Empty;
					if (text.Length > 0)
					{
						return text;
					}
				}
				if (!string.IsNullOrWhiteSpace(this.NetworkVersion))
				{
					return this.NetworkVersion;
				}
				return "—";
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x0002FED7 File Offset: 0x0002E0D7
		public string AuthorsDisplay
		{
			get
			{
				if (this._authors.Count != 0)
				{
					return string.Join(", ", this._authors);
				}
				return "—";
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000786 RID: 1926 RVA: 0x0002FEFC File Offset: 0x0002E0FC
		public string ContributorsDisplay
		{
			get
			{
				if (this._contributors.Count != 0)
				{
					return string.Join(", ", this._contributors);
				}
				return "—";
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x0002FF24 File Offset: 0x0002E124
		public string DependenciesDisplay
		{
			get
			{
				if (this.Dependencies.Count != 0)
				{
					return string.Join(", ", Enumerable.Select<ModDependencyInfo, string>(this.Dependencies, (ModDependencyInfo dependency) => dependency.Display));
				}
				return "—";
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000788 RID: 1928 RVA: 0x0002FF78 File Offset: 0x0002E178
		public IReadOnlyList<ModDependencyInfo> Dependencies { get; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x0002FF80 File Offset: 0x0002E180
		// (set) Token: 0x0600078A RID: 1930 RVA: 0x0002FF88 File Offset: 0x0002E188
		public IReadOnlyList<ModDependencyInfo> MissingDependencies { get; private set; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x0002FF91 File Offset: 0x0002E191
		// (set) Token: 0x0600078C RID: 1932 RVA: 0x0002FF99 File Offset: 0x0002E199
		public bool DependencyHasErrors { get; private set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x0002FFA2 File Offset: 0x0002E1A2
		public bool HasDependencyIssues
		{
			get
			{
				return this.DependencyHasErrors || this.MissingDependencies.Count > 0;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600078E RID: 1934 RVA: 0x0002FFBC File Offset: 0x0002E1BC
		public bool CanFixDependencyIssues
		{
			get
			{
				return this.HasDependencyIssues || this.HasLoadError;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x0600078F RID: 1935 RVA: 0x0002FFCE File Offset: 0x0002E1CE
		// (set) Token: 0x06000790 RID: 1936 RVA: 0x0002FFD6 File Offset: 0x0002E1D6
		public IReadOnlyList<string> DatabaseTags { get; private set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000791 RID: 1937 RVA: 0x0002FFE0 File Offset: 0x0002E1E0
		public string DatabaseTagsDisplay
		{
			get
			{
				string text = this._cachedDatabaseTagsDisplay;
				if (text != null)
				{
					return text;
				}
				text = ((this.DatabaseTags.Count == 0) ? "—" : string.Join(", ", this.DatabaseTags));
				this._cachedDatabaseTagsDisplay = text;
				return text;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x00030026 File Offset: 0x0002E226
		// (set) Token: 0x06000793 RID: 1939 RVA: 0x0003002E File Offset: 0x0002E22E
		[Nullable(2)]
		public string ModDatabaseAssetId { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x00030037 File Offset: 0x0002E237
		// (set) Token: 0x06000795 RID: 1941 RVA: 0x0003003F File Offset: 0x0002E23F
		[Nullable(2)]
		public string ModDatabasePageUrl { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x00030048 File Offset: 0x0002E248
		// (set) Token: 0x06000797 RID: 1943 RVA: 0x00030050 File Offset: 0x0002E250
		[Nullable(2)]
		public Uri ModDatabasePageUri { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000798 RID: 1944 RVA: 0x00030059 File Offset: 0x0002E259
		public bool HasModDatabasePageLink
		{
			get
			{
				return this.ModDatabasePageUri != null;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000799 RID: 1945 RVA: 0x00030067 File Offset: 0x0002E267
		public string ModDatabasePageUrlDisplay
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.ModDatabasePageUrl))
				{
					return this.ModDatabasePageUrl;
				}
				return "—";
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x00030084 File Offset: 0x0002E284
		public string DownloadsDisplay
		{
			get
			{
				if (this._databaseDownloads == null)
				{
					return "—";
				}
				return this._databaseDownloads.Value.ToString("N0", CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x0600079B RID: 1947 RVA: 0x000300C1 File Offset: 0x0002E2C1
		public int ModDatabaseDownloadsSortKey
		{
			get
			{
				return this._databaseDownloads.GetValueOrDefault();
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x0600079C RID: 1948 RVA: 0x000300D0 File Offset: 0x0002E2D0
		public string RecentDownloadsDisplay
		{
			get
			{
				if (this._databaseRecentDownloads == null)
				{
					return "—";
				}
				return this._databaseRecentDownloads.Value.ToString("N0", CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600079D RID: 1949 RVA: 0x00030110 File Offset: 0x0002E310
		public string TenDayDownloadsDisplay
		{
			get
			{
				if (this._databaseTenDayDownloads == null)
				{
					return "—";
				}
				return this._databaseTenDayDownloads.Value.ToString("N0", CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x0003014D File Offset: 0x0002E34D
		public int ModDatabaseRecentDownloadsSortKey
		{
			get
			{
				return this._databaseRecentDownloads.GetValueOrDefault();
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600079F RID: 1951 RVA: 0x0003015A File Offset: 0x0002E35A
		public double ModDatabaseRelevancySortKey { get; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x00030164 File Offset: 0x0002E364
		public long ModDatabaseLastUpdatedSortKey
		{
			get
			{
				if (this._modDatabaseLastUpdatedUtc == null)
				{
					return 0L;
				}
				return this._modDatabaseLastUpdatedUtc.GetValueOrDefault().Ticks;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x00030190 File Offset: 0x0002E390
		public string CommentsDisplay
		{
			get
			{
				if (this._databaseComments == null)
				{
					return "—";
				}
				return this._databaseComments.Value.ToString("N0", CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x000301D0 File Offset: 0x0002E3D0
		[Nullable(2)]
		public ImageSource ModDatabasePreviewImage
		{
			[NullableContext(2)]
			get
			{
				ImageSource icon = this.Icon;
				if (icon != null)
				{
					return icon;
				}
				if (this._modDatabaseLogo == null && !string.IsNullOrWhiteSpace(this._modDatabaseLogoUrl))
				{
					this._modDatabaseLogo = this.CreateModDatabaseLogoImage();
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(50, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Lazy database logo loaded. URL='");
					defaultInterpolatedStringHandler.AppendFormatted(ModListItemViewModel.FormatValue(this._modDatabaseLogoUrl));
					defaultInterpolatedStringHandler.AppendLiteral("', Image created=");
					defaultInterpolatedStringHandler.AppendFormatted<bool>(this._modDatabaseLogo != null);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					ModListItemViewModel.LogDebug(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				return this._modDatabaseLogo;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060007A3 RID: 1955 RVA: 0x0003026F File Offset: 0x0002E46F
		public bool HasModDatabasePreviewImage
		{
			get
			{
				return this.ModDatabasePreviewImage != null;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x0003027A File Offset: 0x0002E47A
		// (set) Token: 0x060007A5 RID: 1957 RVA: 0x00030282 File Offset: 0x0002E482
		[Nullable(2)]
		public string LatestDatabaseVersion { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x0003028B File Offset: 0x0002E48B
		public string LatestDatabaseVersionDisplay
		{
			get
			{
				if (InternetAccessManager.IsInternetAccessDisabled)
				{
					return "?";
				}
				if (!string.IsNullOrWhiteSpace(this.LatestDatabaseVersion))
				{
					return this.LatestDatabaseVersion;
				}
				return "—";
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x000302B4 File Offset: 0x0002E4B4
		public unsafe string LatestVersionSortKey
		{
			get
			{
				string text = string.IsNullOrWhiteSpace(this.LatestDatabaseVersion) ? this.LatestDatabaseVersionDisplay : this.LatestDatabaseVersion;
				int num = (!this.CanUpdate) ? 1 : 0;
				return string.Create<ValueTuple<int, string>>(text.Length + 2, new ValueTuple<int, string>(num, text), delegate(Span<char> span, [TupleElementNames(new string[]
				{
					"Order",
					"Version"
				})] ValueTuple<int, string> state)
				{
					*span[0] = (char)(48 + state.Item1);
					*span[1] = '|';
					MemoryExtensions.AsSpan(state.Item2).CopyTo(span.Slice(2));
				});
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x0003031A File Offset: 0x0002E51A
		public bool IsInstalled { get; }

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x00030322 File Offset: 0x0002E522
		// (set) Token: 0x060007AA RID: 1962 RVA: 0x0003032A File Offset: 0x0002E52A
		public bool CanUpdate { get; private set; }

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x00030334 File Offset: 0x0002E534
		public bool RequiresCompatibilitySelection
		{
			get
			{
				return this.CanUpdate && this.LatestRelease != null && !this.LatestRelease.IsCompatibleWithInstalledGame && this.LatestCompatibleRelease != null && !string.Equals(this.LatestRelease.Version, this.LatestCompatibleRelease.Version, 5);
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x00030387 File Offset: 0x0002E587
		public bool HasCompatibleUpdate
		{
			get
			{
				return this.LatestCompatibleRelease != null;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x00030392 File Offset: 0x0002E592
		public bool LatestReleaseIsCompatible
		{
			get
			{
				ModReleaseInfo latestRelease = this.LatestRelease;
				return latestRelease != null && latestRelease.IsCompatibleWithInstalledGame;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060007AE RID: 1966 RVA: 0x000303A5 File Offset: 0x0002E5A5
		public bool ShouldHighlightLatestVersion
		{
			get
			{
				return this.CanUpdate;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x000303AD File Offset: 0x0002E5AD
		// (set) Token: 0x060007B0 RID: 1968 RVA: 0x000303B5 File Offset: 0x0002E5B5
		[Nullable(2)]
		public ModReleaseInfo LatestRelease { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x000303BE File Offset: 0x0002E5BE
		// (set) Token: 0x060007B2 RID: 1970 RVA: 0x000303C6 File Offset: 0x0002E5C6
		[Nullable(2)]
		public ModReleaseInfo LatestCompatibleRelease { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x000303CF File Offset: 0x0002E5CF
		// (set) Token: 0x060007B4 RID: 1972 RVA: 0x000303D7 File Offset: 0x0002E5D7
		public IReadOnlyList<ModVersionOptionViewModel> VersionOptions { get; private set; } = Array.Empty<ModVersionOptionViewModel>();

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x000303E0 File Offset: 0x0002E5E0
		public bool HasVersionOptions
		{
			get
			{
				return this.VersionOptions.Count > 0;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x000303F0 File Offset: 0x0002E5F0
		public bool HasDownloadableRelease
		{
			get
			{
				return this.LatestCompatibleRelease != null;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x000303FB File Offset: 0x0002E5FB
		// (set) Token: 0x060007B8 RID: 1976 RVA: 0x00030403 File Offset: 0x0002E603
		public IReadOnlyList<ModListItemViewModel.ReleaseChangelog> NewerReleaseChangelogs { get; private set; } = Array.Empty<ModListItemViewModel.ReleaseChangelog>();

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060007B9 RID: 1977 RVA: 0x0003040C File Offset: 0x0002E60C
		public bool HasNewerReleaseChangelogs
		{
			get
			{
				return this.NewerReleaseChangelogs.Count > 0;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x0003041C File Offset: 0x0002E61C
		// (set) Token: 0x060007BB RID: 1979 RVA: 0x00030424 File Offset: 0x0002E624
		[Nullable(2)]
		public ModVersionVoteSummary LatestReleaseUserReportSummary { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x0003042D File Offset: 0x0002E62D
		// (set) Token: 0x060007BD RID: 1981 RVA: 0x00030435 File Offset: 0x0002E635
		[Nullable(2)]
		public string LatestReleaseUserReportVersion { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x0003043E File Offset: 0x0002E63E
		// (set) Token: 0x060007BF RID: 1983 RVA: 0x00030446 File Offset: 0x0002E646
		[Nullable(2)]
		public string LatestReleaseUserReportDisplay
		{
			[NullableContext(2)]
			get
			{
				return this._latestReleaseUserReportDisplay;
			}
			[NullableContext(2)]
			private set
			{
				if (base.SetProperty<string>(ref this._latestReleaseUserReportDisplay, value, "LatestReleaseUserReportDisplay"))
				{
					base.OnPropertyChanged("HasLatestReleaseUserReport");
				}
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x00030467 File Offset: 0x0002E667
		// (set) Token: 0x060007C1 RID: 1985 RVA: 0x0003046F File Offset: 0x0002E66F
		[Nullable(2)]
		public string LatestReleaseUserReportTooltip
		{
			[NullableContext(2)]
			get
			{
				return this._latestReleaseUserReportTooltip;
			}
			[NullableContext(2)]
			private set
			{
				base.SetProperty<string>(ref this._latestReleaseUserReportTooltip, value, "LatestReleaseUserReportTooltip");
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x00030484 File Offset: 0x0002E684
		public bool HasLatestReleaseUserReport
		{
			get
			{
				return !string.IsNullOrWhiteSpace(this._latestReleaseUserReportDisplay);
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x00030494 File Offset: 0x0002E694
		// (set) Token: 0x060007C4 RID: 1988 RVA: 0x0003049C File Offset: 0x0002E69C
		[Nullable(2)]
		public ModVersionVoteSummary UserReportSummary { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x000304A5 File Offset: 0x0002E6A5
		// (set) Token: 0x060007C6 RID: 1990 RVA: 0x000304AD File Offset: 0x0002E6AD
		[Nullable(2)]
		public string UserReportModVersion { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060007C7 RID: 1991 RVA: 0x000304B6 File Offset: 0x0002E6B6
		public ModVersionVoteCounts UserReportCounts
		{
			get
			{
				ModVersionVoteSummary userReportSummary = this.UserReportSummary;
				return ((userReportSummary != null) ? userReportSummary.Counts : null) ?? ModVersionVoteCounts.Empty;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060007C8 RID: 1992 RVA: 0x000304D3 File Offset: 0x0002E6D3
		public bool HasNegativeUserReports
		{
			get
			{
				return this.UserReportCounts.NotFunctional > 0 || this.UserReportCounts.CrashesOrFreezesGame > 0;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060007C9 RID: 1993 RVA: 0x000304F4 File Offset: 0x0002E6F4
		public ModVersionVoteOption? UserVoteOption
		{
			get
			{
				ModVersionVoteSummary userReportSummary = this.UserReportSummary;
				if (userReportSummary == null)
				{
					return default(ModVersionVoteOption?);
				}
				return userReportSummary.UserVote;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060007CA RID: 1994 RVA: 0x0003051C File Offset: 0x0002E71C
		public bool HasUserVoted
		{
			get
			{
				return this.UserVoteOption != null;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x00030537 File Offset: 0x0002E737
		// (set) Token: 0x060007CC RID: 1996 RVA: 0x0003053F File Offset: 0x0002E73F
		public string UserReportDisplay
		{
			get
			{
				return this._userReportDisplay;
			}
			private set
			{
				base.SetProperty<string>(ref this._userReportDisplay, value, "UserReportDisplay");
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x00030554 File Offset: 0x0002E754
		// (set) Token: 0x060007CE RID: 1998 RVA: 0x0003055C File Offset: 0x0002E75C
		[Nullable(2)]
		public string UserReportTooltip
		{
			[NullableContext(2)]
			get
			{
				return this._userReportTooltip;
			}
			[NullableContext(2)]
			private set
			{
				base.SetProperty<string>(ref this._userReportTooltip, value, "UserReportTooltip");
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x00030571 File Offset: 0x0002E771
		// (set) Token: 0x060007D0 RID: 2000 RVA: 0x00030579 File Offset: 0x0002E779
		public bool IsUserReportLoading
		{
			get
			{
				return this._isUserReportLoading;
			}
			private set
			{
				base.SetProperty<bool>(ref this._isUserReportLoading, value, "IsUserReportLoading");
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x0003058E File Offset: 0x0002E78E
		// (set) Token: 0x060007D2 RID: 2002 RVA: 0x00030596 File Offset: 0x0002E796
		public bool UserReportHasError
		{
			get
			{
				return this._userReportHasError;
			}
			private set
			{
				base.SetProperty<bool>(ref this._userReportHasError, value, "UserReportHasError");
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x000305AB File Offset: 0x0002E7AB
		public bool CanSubmitUserReport
		{
			get
			{
				return !string.IsNullOrWhiteSpace(this._installedGameVersion) && !string.IsNullOrWhiteSpace(this.UserReportModVersion);
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x000305CA File Offset: 0x0002E7CA
		public string InstallButtonToolTip
		{
			get
			{
				if (!this.HasDownloadableRelease)
				{
					return "No downloadable releases are available.";
				}
				if (this.LatestCompatibleRelease != null)
				{
					return "Install compatible version " + this.LatestCompatibleRelease.Version + ".";
				}
				return "No releases are marked compatible with the current target Vintage Story version.";
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x00030602 File Offset: 0x0002E802
		// (set) Token: 0x060007D6 RID: 2006 RVA: 0x0003060A File Offset: 0x0002E80A
		[Nullable(2)]
		public ModVersionOptionViewModel SelectedVersionOption
		{
			[NullableContext(2)]
			get
			{
				return this._selectedVersionOption;
			}
			[NullableContext(2)]
			set
			{
				base.SetProperty<ModVersionOptionViewModel>(ref this._selectedVersionOption, value, "SelectedVersionOption");
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x00030620 File Offset: 0x0002E820
		public string UpdateButtonToolTip
		{
			get
			{
				if (this.LatestCompatibleRelease == null)
				{
					return "No compatible updates available.";
				}
				if (this.LatestRelease != null && !string.Equals(this.LatestRelease.Version, this.LatestCompatibleRelease.Version, 5))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(23, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Latest: ");
					defaultInterpolatedStringHandler.AppendFormatted(this.LatestRelease.Version);
					defaultInterpolatedStringHandler.AppendLiteral(". Compatible: ");
					defaultInterpolatedStringHandler.AppendFormatted(this.LatestCompatibleRelease.Version);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}
				return "Install version " + this.LatestCompatibleRelease.Version + ".";
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060007D8 RID: 2008 RVA: 0x000306D5 File Offset: 0x0002E8D5
		public string DescriptionDisplay
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this._description))
				{
					return this._description;
				}
				return "No description available.";
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x000306F0 File Offset: 0x0002E8F0
		[Nullable(2)]
		public string Website { [NullableContext(2)] get; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060007DA RID: 2010 RVA: 0x000306F8 File Offset: 0x0002E8F8
		[Nullable(2)]
		public Uri WebsiteUri { [NullableContext(2)] get; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x00030700 File Offset: 0x0002E900
		public bool HasWebsiteLink
		{
			get
			{
				return this.WebsiteUri != null;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x0003070E File Offset: 0x0002E90E
		[Nullable(2)]
		public ICommand OpenWebsiteCommand { [NullableContext(2)] get; }

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x00030716 File Offset: 0x0002E916
		[Nullable(2)]
		public ICommand OpenModDatabasePageCommand
		{
			[NullableContext(2)]
			get
			{
				return this._openModDatabasePageCommand;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x0003071E File Offset: 0x0002E91E
		public string SourcePath { get; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060007DF RID: 2015 RVA: 0x00030726 File Offset: 0x0002E926
		// (set) Token: 0x060007E0 RID: 2016 RVA: 0x0003072E File Offset: 0x0002E92E
		public DateTime DateModified { get; private set; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x00030738 File Offset: 0x0002E938
		public string DateModifiedDisplay
		{
			get
			{
				if (!(this.DateModified == DateTime.MinValue))
				{
					return this.DateModified.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
				}
				return string.Empty;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x00030775 File Offset: 0x0002E975
		public string Location { get; }

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x0003077D File Offset: 0x0002E97D
		public bool IsModDatabaseEntry { get; }

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060007E4 RID: 2020 RVA: 0x00030785 File Offset: 0x0002E985
		public ModSourceKind SourceKind { get; }

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x00030790 File Offset: 0x0002E990
		public string SourceKindDisplay
		{
			get
			{
				string result;
				switch (this.SourceKind)
				{
				case ModSourceKind.Folder:
					result = "Folder";
					break;
				case ModSourceKind.ZipArchive:
					result = "Zip";
					break;
				case ModSourceKind.Assembly:
					result = "Assembly";
					break;
				case ModSourceKind.SourceCode:
					result = "Source";
					break;
				default:
					result = this.SourceKind.ToString();
					break;
				}
				return result;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060007E6 RID: 2022 RVA: 0x000307F2 File Offset: 0x0002E9F2
		[Nullable(2)]
		public string Side { [NullableContext(2)] get; }

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x000307FC File Offset: 0x0002E9FC
		public string SideDisplay
		{
			get
			{
				string preferredSide = this.GetPreferredSide();
				if (!string.IsNullOrWhiteSpace(preferredSide))
				{
					return ModListItemViewModel.GetCapitalizedSide(preferredSide);
				}
				return "—";
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060007E8 RID: 2024 RVA: 0x00030824 File Offset: 0x0002EA24
		[Nullable(2)]
		public string SideSortValue
		{
			[NullableContext(2)]
			get
			{
				return ModListItemViewModel.GetCapitalizedSide(this.GetPreferredSide());
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x00030831 File Offset: 0x0002EA31
		public bool? RequiredOnClient { get; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060007EA RID: 2026 RVA: 0x0003083C File Offset: 0x0002EA3C
		public string RequiredOnClientDisplay
		{
			get
			{
				if (this.RequiredOnClient == null)
				{
					return "—";
				}
				if (!this.RequiredOnClient.Value)
				{
					return "No";
				}
				return "Yes";
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x0003087A File Offset: 0x0002EA7A
		public bool? RequiredOnServer { get; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x00030884 File Offset: 0x0002EA84
		public string RequiredOnServerDisplay
		{
			get
			{
				if (this.RequiredOnServer == null)
				{
					return "—";
				}
				if (!this.RequiredOnServer.Value)
				{
					return "No";
				}
				return "Yes";
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060007ED RID: 2029 RVA: 0x000308C4 File Offset: 0x0002EAC4
		[Nullable(2)]
		public ImageSource Icon
		{
			[NullableContext(2)]
			get
			{
				if (!this._iconInitialized)
				{
					this._iconInitialized = true;
					ModLoadingTimingService timingService = this._timingService;
					using ((timingService != null) ? timingService.MeasureIconLoad() : null)
					{
						this._icon = this.CreateImage(this._modEntry.IconBytes, "Icon bytes");
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(69, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Lazy icon image created: ");
						defaultInterpolatedStringHandler.AppendFormatted<bool>(this._icon != null);
						defaultInterpolatedStringHandler.AppendLiteral(". Will fall back to database logo when null.");
						ModListItemViewModel.LogDebug(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
				return this._icon;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x00030974 File Offset: 0x0002EB74
		public bool HasErrors { get; }

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x0003097C File Offset: 0x0002EB7C
		public bool HasLoadError
		{
			get
			{
				return !string.IsNullOrWhiteSpace(this._loadError);
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060007F0 RID: 2032 RVA: 0x0003098C File Offset: 0x0002EB8C
		public bool IsDependencyWarningOverridden
		{
			get
			{
				return this._dependencyWarningOverridden;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x00030994 File Offset: 0x0002EB94
		public bool CanToggle
		{
			get
			{
				return !this.HasErrors && (!this.HasLoadError || this.CanOverrideDependencyWarning);
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060007F2 RID: 2034 RVA: 0x000309B0 File Offset: 0x0002EBB0
		private bool CanOverrideDependencyWarning
		{
			get
			{
				return this.IsDependencyWarningOverridden && this.HasDependencyIssues;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x000309C2 File Offset: 0x0002EBC2
		// (set) Token: 0x060007F4 RID: 2036 RVA: 0x000309CA File Offset: 0x0002EBCA
		public bool HasActivationError
		{
			get
			{
				return this._hasActivationError;
			}
			private set
			{
				base.SetProperty<bool>(ref this._hasActivationError, value, "HasActivationError");
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x000309DF File Offset: 0x0002EBDF
		// (set) Token: 0x060007F6 RID: 2038 RVA: 0x000309E7 File Offset: 0x0002EBE7
		[Nullable(2)]
		public string ActivationError
		{
			[NullableContext(2)]
			get
			{
				return this._activationError;
			}
			[NullableContext(2)]
			private set
			{
				if (base.SetProperty<string>(ref this._activationError, value, "ActivationError"))
				{
					this.HasActivationError = !string.IsNullOrWhiteSpace(value);
					this.UpdateStatusFromErrors();
					this.UpdateTooltip();
				}
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x00030A18 File Offset: 0x0002EC18
		// (set) Token: 0x060007F8 RID: 2040 RVA: 0x00030A20 File Offset: 0x0002EC20
		public string StatusText
		{
			get
			{
				return this._statusText;
			}
			private set
			{
				if (base.SetProperty<string>(ref this._statusText, value, "StatusText"))
				{
					base.OnPropertyChanged("StatusSortOrder");
				}
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x00030A44 File Offset: 0x0002EC44
		public int StatusSortOrder
		{
			get
			{
				string statusText = this.StatusText;
				int result;
				if (!(statusText == "Error"))
				{
					if (!(statusText == "Warning"))
					{
						result = 2;
					}
					else
					{
						result = 1;
					}
				}
				else
				{
					result = 0;
				}
				return result;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x00030A7F File Offset: 0x0002EC7F
		// (set) Token: 0x060007FB RID: 2043 RVA: 0x00030A87 File Offset: 0x0002EC87
		public string StatusDetails
		{
			get
			{
				return this._statusDetails;
			}
			private set
			{
				base.SetProperty<string>(ref this._statusDetails, value, "StatusDetails");
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x00030A9C File Offset: 0x0002EC9C
		// (set) Token: 0x060007FD RID: 2045 RVA: 0x00030AA4 File Offset: 0x0002ECA4
		public string Tooltip
		{
			get
			{
				return this._tooltip;
			}
			private set
			{
				base.SetProperty<string>(ref this._tooltip, value, "Tooltip");
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060007FE RID: 2046 RVA: 0x00030AB9 File Offset: 0x0002ECB9
		// (set) Token: 0x060007FF RID: 2047 RVA: 0x00030AC4 File Offset: 0x0002ECC4
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (this._suppressState)
				{
					if (base.SetProperty<bool>(ref this._isActive, value, "IsActive"))
					{
						base.OnPropertyChanged("ActiveSortOrder");
						base.OnPropertyChanged("CanFixDependencyIssues");
					}
					return;
				}
				if (this._isActive == value)
				{
					return;
				}
				bool isActive = this._isActive;
				if (!base.SetProperty<bool>(ref this._isActive, value, "IsActive"))
				{
					return;
				}
				base.OnPropertyChanged("ActiveSortOrder");
				base.OnPropertyChanged("CanFixDependencyIssues");
				this.ApplyActivationChangeAsync(isActive, value);
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000800 RID: 2048 RVA: 0x00030B49 File Offset: 0x0002ED49
		public int ActiveSortOrder
		{
			get
			{
				return (!this._isActive) ? 1 : 0;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000801 RID: 2049 RVA: 0x00030B54 File Offset: 0x0002ED54
		// (set) Token: 0x06000802 RID: 2050 RVA: 0x00030B5C File Offset: 0x0002ED5C
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				base.SetProperty<bool>(ref this._isSelected, value, "IsSelected");
			}
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00030B74 File Offset: 0x0002ED74
		public void RefreshDateModified()
		{
			DateTime dateModifiedTimestamp = ModListItemViewModel.GetDateModifiedTimestamp(this.SourcePath);
			if (this.DateModified == dateModifiedTimestamp)
			{
				return;
			}
			this.DateModified = dateModifiedTimestamp;
			base.OnPropertyChanged("DateModified");
			base.OnPropertyChanged("DateModifiedDisplay");
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00030BB9 File Offset: 0x0002EDB9
		public void RefreshInternetAccessDependentState()
		{
			base.OnPropertyChanged("LatestDatabaseVersionDisplay");
			base.OnPropertyChanged("LatestVersionSortKey");
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00030BD4 File Offset: 0x0002EDD4
		public IReadOnlyList<ModListItemViewModel.ReleaseChangelog> GetChangelogEntriesForUpgrade([Nullable(2)] string targetVersion)
		{
			if (this._releases.Count == 0 || string.IsNullOrWhiteSpace(targetVersion))
			{
				return Array.Empty<ModListItemViewModel.ReleaseChangelog>();
			}
			string trimmedVersion = targetVersion.Trim();
			string normalizedVersion = VersionStringUtility.Normalize(targetVersion);
			string version = this.Version;
			string normalizedVersion2 = VersionStringUtility.Normalize(version);
			List<ModListItemViewModel.ReleaseChangelog> list = new List<ModListItemViewModel.ReleaseChangelog>();
			bool flag = false;
			foreach (ModReleaseInfo modReleaseInfo in this._releases)
			{
				if (!flag && ModListItemViewModel.DoesReleaseMatchVersion(modReleaseInfo, trimmedVersion, normalizedVersion))
				{
					flag = true;
				}
				if (flag)
				{
					if (!string.IsNullOrWhiteSpace(version) && ModListItemViewModel.DoesReleaseMatchVersion(modReleaseInfo, version.Trim(), normalizedVersion2))
					{
						break;
					}
					string changelog = modReleaseInfo.Changelog;
					string text = (changelog != null) ? changelog.Trim() : null;
					if (!string.IsNullOrEmpty(text))
					{
						list.Add(new ModListItemViewModel.ReleaseChangelog(modReleaseInfo.Version, text));
					}
				}
			}
			if (list.Count != 0)
			{
				return list;
			}
			return Array.Empty<ModListItemViewModel.ReleaseChangelog>();
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00030CE0 File Offset: 0x0002EEE0
		[NullableContext(2)]
		private bool SetUserReportVersion(string version, bool reinitializeState)
		{
			string text = ModListItemViewModel.NormalizeVersion(version);
			if (string.Equals(this.UserReportModVersion, text, 4))
			{
				return false;
			}
			this.UserReportModVersion = text;
			base.OnPropertyChanged("UserReportModVersion");
			base.OnPropertyChanged("CanSubmitUserReport");
			if (reinitializeState)
			{
				this.InitializeUserReportState(this._installedGameVersion, this.UserReportModVersion);
			}
			return true;
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00030D38 File Offset: 0x0002EF38
		[NullableContext(2)]
		private static string NormalizeVersion(string value)
		{
			string text = (value != null) ? value.Trim() : null;
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text;
			}
			return null;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00030D5D File Offset: 0x0002EF5D
		[NullableContext(2)]
		private static string SelectPreferredUserReportVersion(ModReleaseInfo latestRelease, ModReleaseInfo latestCompatibleRelease, ModDatabaseInfo databaseInfo)
		{
			string result;
			if ((result = ((latestRelease != null) ? latestRelease.Version : null)) == null && (result = ((databaseInfo != null) ? databaseInfo.LatestVersion : null)) == null && (result = ((latestCompatibleRelease != null) ? latestCompatibleRelease.Version : null)) == null)
			{
				if (databaseInfo == null)
				{
					return null;
				}
				result = databaseInfo.LatestCompatibleVersion;
			}
			return result;
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00030D9A File Offset: 0x0002EF9A
		[NullableContext(2)]
		private void InitializeUserReportState(string installedGameVersion, string modVersion)
		{
			this._hasInitializedUserReportState = true;
			if (string.IsNullOrWhiteSpace(installedGameVersion) || string.IsNullOrWhiteSpace(modVersion))
			{
				this.SetUserReportUnavailable("User reports require a known Vintage Story and mod version.");
				return;
			}
			this.SetUserReportLoading();
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00030DC5 File Offset: 0x0002EFC5
		public void SetUserReportLoading()
		{
			this._hasInitializedUserReportState = true;
			this.ClearUserReportSummary();
			this.UserReportHasError = false;
			this.IsUserReportLoading = true;
			this.UserReportDisplay = "Loading…";
			this.UserReportTooltip = "Fetching user reports…";
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x00030DF8 File Offset: 0x0002EFF8
		public void SetUserReportOffline()
		{
			this._hasInitializedUserReportState = true;
			this.IsUserReportLoading = false;
			this.UserReportHasError = false;
			this.UserReportDisplay = "Offline";
			this.UserReportTooltip = "Enable Internet Access in the File menu to load user reports.";
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00030E25 File Offset: 0x0002F025
		public void SetUserReportUnavailable(string message)
		{
			this._hasInitializedUserReportState = true;
			this.ClearUserReportSummary();
			this.UserReportHasError = false;
			this.IsUserReportLoading = false;
			this.UserReportDisplay = "Unavailable";
			this.UserReportTooltip = (string.IsNullOrWhiteSpace(message) ? this.BuildUserReportTooltip(null) : message);
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x00030E65 File Offset: 0x0002F065
		public void SetUserReportError(string message)
		{
			this._hasInitializedUserReportState = true;
			this.IsUserReportLoading = false;
			this.UserReportHasError = true;
			this.UserReportTooltip = (string.IsNullOrWhiteSpace(message) ? "Failed to load user reports. Try again later." : message);
			if (this.UserReportSummary == null)
			{
				this.UserReportDisplay = "Error";
			}
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00030EA8 File Offset: 0x0002F0A8
		public void ApplyUserReportSummary(ModVersionVoteSummary summary)
		{
			ModLoadingTimingService timingService = this._timingService;
			using ((timingService != null) ? timingService.MeasureUserReportsLoad() : null)
			{
				if (summary == null)
				{
					throw new ArgumentNullException("summary");
				}
				this.UserReportSummary = summary;
				this._hasInitializedUserReportState = true;
				this.IsUserReportLoading = false;
				this.UserReportHasError = false;
				this.UserReportDisplay = ModListItemViewModel.BuildUserReportDisplay(summary);
				this.UserReportTooltip = this.BuildUserReportTooltip(summary);
				base.OnPropertyChanged("UserReportSummary");
				base.OnPropertyChanged("UserReportCounts");
				base.OnPropertyChanged("HasNegativeUserReports");
				base.OnPropertyChanged("UserVoteOption");
				base.OnPropertyChanged("HasUserVoted");
				this.UpdateStatusFromErrors();
			}
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x00030F68 File Offset: 0x0002F168
		private void ClearUserReportSummary()
		{
			if (this.UserReportSummary == null)
			{
				base.OnPropertyChanged("UserReportSummary");
				base.OnPropertyChanged("UserReportCounts");
				base.OnPropertyChanged("HasNegativeUserReports");
				base.OnPropertyChanged("UserVoteOption");
				base.OnPropertyChanged("HasUserVoted");
				this.UpdateStatusFromErrors();
				return;
			}
			this.UserReportSummary = null;
			base.OnPropertyChanged("UserReportSummary");
			base.OnPropertyChanged("UserReportCounts");
			base.OnPropertyChanged("HasNegativeUserReports");
			base.OnPropertyChanged("UserVoteOption");
			base.OnPropertyChanged("HasUserVoted");
			this.UpdateStatusFromErrors();
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00031000 File Offset: 0x0002F200
		public void EnsureUserReportStateInitialized()
		{
			if (this._hasInitializedUserReportState)
			{
				return;
			}
			Application application = Application.Current;
			if (application != null && application.Dispatcher.CheckAccess())
			{
				this.InitializeUserReportState(this._installedGameVersion, this.UserReportModVersion);
				return;
			}
			Application application2 = Application.Current;
			if (application2 == null)
			{
				return;
			}
			application2.Dispatcher.Invoke(delegate()
			{
				this.EnsureUserReportStateInitialized();
			});
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00031064 File Offset: 0x0002F264
		private unsafe static string BuildUserReportDisplay([Nullable(2)] ModVersionVoteSummary summary)
		{
			if (summary == null || summary.TotalVotes == 0)
			{
				return "No votes";
			}
			ModVersionVoteOption? majorityOption = summary.GetMajorityOption();
			if (majorityOption == null)
			{
				return string.Create<int>(8 + summary.TotalVotes.ToString(CultureInfo.CurrentCulture).Length, summary.TotalVotes, delegate(Span<char> span, int total)
				{
					MemoryExtensions.AsSpan("Mixed (").CopyTo(span);
					int length = "Mixed (".Length;
					string text3 = total.ToString(CultureInfo.CurrentCulture);
					MemoryExtensions.AsSpan(text3).CopyTo(span.Slice(length));
					*span[length + text3.Length] = ')';
				});
			}
			int count = summary.Counts.GetCount(majorityOption.Value);
			string text = majorityOption.Value.ToDisplayString();
			string text2 = count.ToString(CultureInfo.CurrentCulture);
			return text + " (" + text2 + ")";
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00031114 File Offset: 0x0002F314
		private string BuildUserReportTooltip([Nullable(2)] ModVersionVoteSummary summary)
		{
			string text = string.IsNullOrWhiteSpace(this._installedGameVersion) ? "this VS version" : string.Format(CultureInfo.CurrentCulture, "VS {0}", this._installedGameVersion);
			if (summary == null)
			{
				return string.Format(CultureInfo.CurrentCulture, "No votes recorded yet for {0}. Click to share your experience.", text);
			}
			ModVersionVoteCounts counts = summary.Counts;
			string text2 = ModListItemViewModel.BuildCountsSummary(text, counts);
			string text3 = ModListItemViewModel.BuildNegativeCommentsText(summary, false);
			if (!string.IsNullOrEmpty(text3))
			{
				return text2 + Environment.NewLine + Environment.NewLine + text3;
			}
			return text2;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x00031194 File Offset: 0x0002F394
		private static string BuildCountsSummary(string versionText, ModVersionVoteCounts counts)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(CultureInfo.CurrentCulture, "User reports for {0}:{1}", versionText, Environment.NewLine);
			stringBuilder.AppendFormat(CultureInfo.CurrentCulture, "Fully functional ({0}){1}", counts.FullyFunctional, Environment.NewLine);
			stringBuilder.AppendFormat(CultureInfo.CurrentCulture, "No issues noticed ({0}){1}", counts.NoIssuesSoFar, Environment.NewLine);
			stringBuilder.AppendFormat(CultureInfo.CurrentCulture, "Some issues but works ({0}){1}", counts.SomeIssuesButWorks, Environment.NewLine);
			stringBuilder.AppendFormat(CultureInfo.CurrentCulture, "Not functional ({0}){1}", counts.NotFunctional, Environment.NewLine);
			stringBuilder.AppendFormat(CultureInfo.CurrentCulture, "Crashes/Freezes game ({0})", counts.CrashesOrFreezesGame);
			return stringBuilder.ToString();
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x00031264 File Offset: 0x0002F464
		private static string BuildNegativeCommentsText(ModVersionVoteSummary summary, bool requireNegativeMajority)
		{
			if (summary == null)
			{
				return string.Empty;
			}
			if (requireNegativeMajority)
			{
				ModVersionVoteOption? majorityOption = summary.GetMajorityOption();
				if (!(majorityOption == ModVersionVoteOption.NotFunctional) && !(majorityOption == ModVersionVoteOption.CrashesOrFreezesGame))
				{
					return string.Empty;
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			ModListItemViewModel.AppendCommentSection(stringBuilder, "Not functional reports", summary.Comments.NotFunctional);
			ModListItemViewModel.AppendCommentSection(stringBuilder, "Crashes/Freezes game reports", summary.Comments.CrashesOrFreezesGame);
			return stringBuilder.ToString();
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x000312E8 File Offset: 0x0002F4E8
		private static void AppendCommentSection(StringBuilder builder, string heading, IReadOnlyList<string> comments)
		{
			if (comments == null || comments.Count == 0)
			{
				return;
			}
			List<string> list = new List<string>(comments.Count);
			Dictionary<string, int> dictionary = new Dictionary<string, int>(StringComparer.Ordinal);
			foreach (string text in comments)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					string text2 = text.Trim();
					int num;
					if (dictionary.TryGetValue(text2, ref num))
					{
						dictionary[text2] = num + 1;
					}
					else
					{
						dictionary.Add(text2, 1);
						list.Add(text2);
					}
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			if (builder.Length > 0)
			{
				builder.AppendLine();
				builder.AppendLine();
			}
			builder.Append(heading);
			builder.Append(':');
			foreach (string text3 in list)
			{
				builder.AppendLine();
				builder.Append(" • ");
				builder.Append(text3);
				if (dictionary[text3] > 1)
				{
					builder.Append(' ');
					builder.Append('(');
					builder.Append(dictionary[text3].ToString(CultureInfo.CurrentCulture));
					builder.Append(')');
				}
			}
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x00031450 File Offset: 0x0002F650
		public void ApplyLatestReleaseUserReportSummary(ModVersionVoteSummary summary)
		{
			if (summary == null)
			{
				return;
			}
			this.LatestReleaseUserReportSummary = summary;
			this.LatestReleaseUserReportVersion = summary.ModVersion;
			this.LatestReleaseUserReportDisplay = ModListItemViewModel.BuildLatestReleaseUserReportDisplay(summary);
			this.LatestReleaseUserReportTooltip = ModListItemViewModel.BuildLatestReleaseUserReportTooltip(summary);
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x00031481 File Offset: 0x0002F681
		public void ClearLatestReleaseUserReport()
		{
			if (this.LatestReleaseUserReportSummary == null && this.LatestReleaseUserReportVersion == null && this._latestReleaseUserReportDisplay == null)
			{
				return;
			}
			this.LatestReleaseUserReportSummary = null;
			this.LatestReleaseUserReportVersion = null;
			this.LatestReleaseUserReportDisplay = null;
			this.LatestReleaseUserReportTooltip = null;
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x000314B8 File Offset: 0x0002F6B8
		[NullableContext(2)]
		private static string BuildLatestReleaseUserReportDisplay(ModVersionVoteSummary summary)
		{
			if (summary == null || summary.TotalVotes == 0)
			{
				return null;
			}
			string text = ModListItemViewModel.BuildUserReportDisplay(summary);
			if (!string.Equals(text, "No votes", 4))
			{
				return text;
			}
			return null;
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x000314EC File Offset: 0x0002F6EC
		[NullableContext(2)]
		private static string BuildLatestReleaseUserReportTooltip(ModVersionVoteSummary summary)
		{
			if (summary == null || summary.TotalVotes == 0)
			{
				return null;
			}
			string text = ModListItemViewModel.BuildCountsSummary(string.IsNullOrWhiteSpace(summary.VintageStoryVersion) ? "this Vintage Story version" : string.Format(CultureInfo.CurrentCulture, "Vintage Story {0}", summary.VintageStoryVersion), summary.Counts);
			string text2 = ModListItemViewModel.BuildNegativeCommentsText(summary, true);
			if (!string.IsNullOrEmpty(text2))
			{
				return text + Environment.NewLine + Environment.NewLine + text2;
			}
			return text;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x00031560 File Offset: 0x0002F760
		[NullableContext(2)]
		public void UpdateLoadError(string loadError)
		{
			string text = string.IsNullOrWhiteSpace(loadError) ? null : loadError;
			if (string.Equals(this._loadError, text, 4))
			{
				return;
			}
			this._loadError = text;
			base.OnPropertyChanged("HasLoadError");
			base.OnPropertyChanged("CanToggle");
			base.OnPropertyChanged("CanFixDependencyIssues");
			this.UpdateStatusFromErrors();
			this.UpdateTooltip();
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x000315C0 File Offset: 0x0002F7C0
		public void RefreshDependencyWarningOverride()
		{
			bool flag = this.ResolveDependencyWarningOverride();
			if (this._dependencyWarningOverridden == flag)
			{
				return;
			}
			this._dependencyWarningOverridden = flag;
			base.OnPropertyChanged("IsDependencyWarningOverridden");
			base.OnPropertyChanged("CanToggle");
			this.UpdateStatusFromErrors();
			this.UpdateTooltip();
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00031608 File Offset: 0x0002F808
		public void UpdateDatabaseInfo(ModDatabaseInfo info, bool loadLogoImmediately = true)
		{
			if (info == null)
			{
				return;
			}
			using (this.SuspendPropertyChangeNotifications())
			{
				string text = ModListItemViewModel.NormalizeSide(this._modDatabaseSide);
				string text2 = ModListItemViewModel.NormalizeSide(info.Side);
				this._modDatabaseSide = info.Side;
				if (!string.Equals(text, text2, 4))
				{
					base.OnPropertyChanged("SideDisplay");
					base.OnPropertyChanged("SideSortValue");
				}
				IReadOnlyList<string> readOnlyList = info.Tags ?? Array.Empty<string>();
				if (ModListItemViewModel.HasDifferentContent<string>(this.DatabaseTags, readOnlyList))
				{
					ModLoadingTimingService timingService = this._timingService;
					using ((timingService != null) ? timingService.MeasureTagsLoad() : null)
					{
						this.DatabaseTags = readOnlyList;
						this._cachedDatabaseTagsDisplay = null;
						base.OnPropertyChanged("DatabaseTags");
						base.OnPropertyChanged("DatabaseTagsDisplay");
					}
				}
				IReadOnlyList<string> readOnlyList2 = info.RequiredGameVersions ?? Array.Empty<string>();
				if (ModListItemViewModel.HasDifferentContent<string>(this._databaseRequiredGameVersions, readOnlyList2))
				{
					this._databaseRequiredGameVersions = readOnlyList2;
				}
				ModReleaseInfo latestRelease = info.LatestRelease;
				ModReleaseInfo latestCompatibleRelease = info.LatestCompatibleRelease;
				IReadOnlyList<ModReleaseInfo> releases = info.Releases ?? Array.Empty<ModReleaseInfo>();
				if (string.IsNullOrWhiteSpace(this.Version))
				{
					string version = ModListItemViewModel.SelectPreferredUserReportVersion(latestRelease, latestCompatibleRelease, info);
					this.SetUserReportVersion(version, true);
				}
				string text3 = (latestRelease != null) ? latestRelease.Version : null;
				if (!string.Equals(this.LatestReleaseUserReportVersion, text3, 5))
				{
					this.ClearLatestReleaseUserReport();
				}
				this.LatestRelease = latestRelease;
				this.LatestCompatibleRelease = latestCompatibleRelease;
				this._releases = releases;
				this.UpdateModDatabaseMetrics(info, releases);
				int? downloads = info.Downloads;
				int? num = this._databaseDownloads;
				int? num2 = downloads;
				if (!(num.GetValueOrDefault() == num2.GetValueOrDefault() & num != null == (num2 != null)))
				{
					this._databaseDownloads = downloads;
					base.OnPropertyChanged("DownloadsDisplay");
					base.OnPropertyChanged("ModDatabaseDownloadsSortKey");
				}
				int? comments = info.Comments;
				num2 = this._databaseComments;
				num = comments;
				if (!(num2.GetValueOrDefault() == num.GetValueOrDefault() & num2 != null == (num != null)))
				{
					this._databaseComments = comments;
					base.OnPropertyChanged("CommentsDisplay");
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(63, 3);
				defaultInterpolatedStringHandler.AppendLiteral("UpdateDatabaseInfo invoked. AssetId='");
				defaultInterpolatedStringHandler.AppendFormatted(ModListItemViewModel.FormatValue(info.AssetId));
				defaultInterpolatedStringHandler.AppendLiteral("', PageUrl='");
				defaultInterpolatedStringHandler.AppendFormatted(ModListItemViewModel.FormatValue(info.ModPageUrl));
				defaultInterpolatedStringHandler.AppendLiteral("', LogoUrl='");
				defaultInterpolatedStringHandler.AppendFormatted(ModListItemViewModel.FormatValue(info.LogoUrl));
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				ModListItemViewModel.LogDebug(defaultInterpolatedStringHandler.ToStringAndClear());
				string logoUrl = info.LogoUrl;
				string logoUrlSource = info.LogoUrlSource;
				bool flag = !string.Equals(this._modDatabaseLogoUrl, logoUrl, 4);
				this._modDatabaseLogoSource = (logoUrlSource ?? this._modDatabaseLogoSource);
				if (flag)
				{
					this._modDatabaseLogoUrl = logoUrl;
					bool flag2 = loadLogoImmediately && this.Icon == null;
					if (flag2)
					{
						this._modDatabaseLogo = this.CreateModDatabaseLogoImage();
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(50, 2);
						defaultInterpolatedStringHandler2.AppendLiteral("Updated database logo. New URL='");
						defaultInterpolatedStringHandler2.AppendFormatted(ModListItemViewModel.FormatValue(this._modDatabaseLogoUrl));
						defaultInterpolatedStringHandler2.AppendLiteral("', Image created=");
						defaultInterpolatedStringHandler2.AppendFormatted<bool>(this._modDatabaseLogo != null);
						defaultInterpolatedStringHandler2.AppendLiteral(".");
						ModListItemViewModel.LogDebug(defaultInterpolatedStringHandler2.ToStringAndClear());
					}
					else
					{
						if (this._modDatabaseLogo != null)
						{
							ModListItemViewModel.LogDebug("Clearing previously loaded database logo to defer image refresh.");
						}
						this._modDatabaseLogo = null;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(66, 2);
						defaultInterpolatedStringHandler3.AppendLiteral("Deferred database logo update. New URL='");
						defaultInterpolatedStringHandler3.AppendFormatted(ModListItemViewModel.FormatValue(this._modDatabaseLogoUrl));
						defaultInterpolatedStringHandler3.AppendLiteral("'. Logo creation skipped=");
						defaultInterpolatedStringHandler3.AppendFormatted<bool>(!flag2);
						defaultInterpolatedStringHandler3.AppendLiteral(".");
						ModListItemViewModel.LogDebug(defaultInterpolatedStringHandler3.ToStringAndClear());
					}
					base.OnPropertyChanged("ModDatabasePreviewImage");
					base.OnPropertyChanged("HasModDatabasePreviewImage");
				}
				else if (loadLogoImmediately && this.Icon == null && this._modDatabaseLogo == null && !string.IsNullOrWhiteSpace(this._modDatabaseLogoUrl))
				{
					this._modDatabaseLogoSource = (logoUrlSource ?? this._modDatabaseLogoSource);
					this._modDatabaseLogo = this.CreateModDatabaseLogoImage();
					base.OnPropertyChanged("ModDatabasePreviewImage");
					base.OnPropertyChanged("HasModDatabasePreviewImage");
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(54, 2);
					defaultInterpolatedStringHandler4.AppendLiteral("Loaded deferred database logo. URL='");
					defaultInterpolatedStringHandler4.AppendFormatted(ModListItemViewModel.FormatValue(this._modDatabaseLogoUrl));
					defaultInterpolatedStringHandler4.AppendLiteral("', Image created=");
					defaultInterpolatedStringHandler4.AppendFormatted<bool>(this._modDatabaseLogo != null);
					defaultInterpolatedStringHandler4.AppendLiteral(".");
					ModListItemViewModel.LogDebug(defaultInterpolatedStringHandler4.ToStringAndClear());
				}
				if (!string.Equals(this.ModDatabaseAssetId, info.AssetId, 4))
				{
					this.ModDatabaseAssetId = info.AssetId;
					base.OnPropertyChanged("ModDatabaseAssetId");
					ModListItemViewModel.LogDebug("Database asset id updated to '" + ModListItemViewModel.FormatValue(this.ModDatabaseAssetId) + "'.");
				}
				string modPageUrl = info.ModPageUrl;
				if (!string.Equals(this.ModDatabasePageUrl, modPageUrl, 4))
				{
					this.ModDatabasePageUrl = modPageUrl;
					base.OnPropertyChanged("ModDatabasePageUrl");
					base.OnPropertyChanged("ModDatabasePageUrlDisplay");
					ModListItemViewModel.LogDebug("Database page URL updated to '" + ModListItemViewModel.FormatValue(this.ModDatabasePageUrl) + "'.");
				}
				Uri uri = ModListItemViewModel.TryCreateHttpUri(modPageUrl);
				if (this.ModDatabasePageUri != uri)
				{
					this.ModDatabasePageUri = uri;
					base.OnPropertyChanged("ModDatabasePageUri");
					base.OnPropertyChanged("HasModDatabasePageLink");
					ModListItemViewModel.LogDebug("Database page URI resolved to '" + ModListItemViewModel.FormatUri(this.ModDatabasePageUri) + "'.");
				}
				ICommand command = null;
				if (uri != null)
				{
					Uri commandUri = uri;
					command = new RelayCommand(delegate()
					{
						ModListItemViewModel.LaunchUri(commandUri);
					});
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5 = new DefaultInterpolatedStringHandler(41, 1);
					defaultInterpolatedStringHandler5.AppendLiteral("Database page command initialized for '");
					defaultInterpolatedStringHandler5.AppendFormatted<Uri>(commandUri);
					defaultInterpolatedStringHandler5.AppendLiteral("'.");
					ModListItemViewModel.LogDebug(defaultInterpolatedStringHandler5.ToStringAndClear());
				}
				base.SetProperty<ICommand>(ref this._openModDatabasePageCommand, command, "OpenModDatabasePageCommand");
				string text4;
				if ((text4 = ((latestCompatibleRelease != null) ? latestCompatibleRelease.Version : null)) == null)
				{
					text4 = (info.LatestCompatibleVersion ?? this.Version);
				}
				string text5 = text4;
				if (!string.Equals(this.LatestDatabaseVersion, text5, 4))
				{
					this.LatestDatabaseVersion = text5;
					base.OnPropertyChanged("LatestDatabaseVersion");
					base.OnPropertyChanged("LatestDatabaseVersionDisplay");
					base.OnPropertyChanged("LatestVersionSortKey");
					ModListItemViewModel.LogDebug("Latest database version updated to '" + ModListItemViewModel.FormatValue(this.LatestDatabaseVersion) + "'.");
				}
				this.InitializeUpdateAvailability();
				this.InitializeVersionOptions();
				this.InitializeVersionWarning(this._installedGameVersion);
				this.UpdateNewerReleaseChangelogs();
				base.OnPropertyChanged("LatestRelease");
				base.OnPropertyChanged("LatestCompatibleRelease");
				base.OnPropertyChanged("LatestReleaseIsCompatible");
				base.OnPropertyChanged("ShouldHighlightLatestVersion");
				base.OnPropertyChanged("CanUpdate");
				base.OnPropertyChanged("LatestVersionSortKey");
				base.OnPropertyChanged("RequiresCompatibilitySelection");
				base.OnPropertyChanged("HasCompatibleUpdate");
				base.OnPropertyChanged("HasDownloadableRelease");
				base.OnPropertyChanged("InstallButtonToolTip");
				base.OnPropertyChanged("UpdateButtonToolTip");
				this.UpdateStatusFromErrors();
				this.UpdateTooltip();
			}
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00031D74 File Offset: 0x0002FF74
		public void ClearDatabaseTags()
		{
			if (this.DatabaseTags.Count == 0)
			{
				return;
			}
			this.DatabaseTags = Array.Empty<string>();
			this._cachedDatabaseTagsDisplay = null;
			base.OnPropertyChanged("DatabaseTags");
			base.OnPropertyChanged("DatabaseTagsDisplay");
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00031DAC File Offset: 0x0002FFAC
		public void RefreshSkippedUpdateState()
		{
			bool canUpdate = this.CanUpdate;
			string updateMessage = this._updateMessage;
			this.InitializeUpdateAvailability();
			if (canUpdate != this.CanUpdate)
			{
				base.OnPropertyChanged("CanUpdate");
				base.OnPropertyChanged("ShouldHighlightLatestVersion");
				base.OnPropertyChanged("LatestVersionSortKey");
				base.OnPropertyChanged("RequiresCompatibilitySelection");
				base.OnPropertyChanged("HasCompatibleUpdate");
				base.OnPropertyChanged("HasDownloadableRelease");
				base.OnPropertyChanged("UpdateButtonToolTip");
				this.UpdateStatusFromErrors();
				this.UpdateTooltip();
				return;
			}
			if (!string.Equals(updateMessage, this._updateMessage, 4))
			{
				this.UpdateStatusFromErrors();
				this.UpdateTooltip();
			}
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x00031E4C File Offset: 0x0003004C
		public void EnsureModDatabaseLogoLoaded()
		{
			if (this._modDatabaseLogo != null)
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(this._modDatabaseLogoUrl))
			{
				return;
			}
			this._modDatabaseLogo = this.CreateModDatabaseLogoImage();
			base.OnPropertyChanged("ModDatabasePreviewImage");
			base.OnPropertyChanged("HasModDatabasePreviewImage");
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(61, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Deferred database logo load complete. URL='");
			defaultInterpolatedStringHandler.AppendFormatted(ModListItemViewModel.FormatValue(this._modDatabaseLogoUrl));
			defaultInterpolatedStringHandler.AppendLiteral("', Image created=");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this._modDatabaseLogo != null);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			ModListItemViewModel.LogDebug(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00031EF4 File Offset: 0x000300F4
		public Task LoadModDatabaseLogoAsync(CancellationToken cancellationToken)
		{
			ModListItemViewModel.<LoadModDatabaseLogoAsync>d__353 <LoadModDatabaseLogoAsync>d__;
			<LoadModDatabaseLogoAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadModDatabaseLogoAsync>d__.<>4__this = this;
			<LoadModDatabaseLogoAsync>d__.cancellationToken = cancellationToken;
			<LoadModDatabaseLogoAsync>d__.<>1__state = -1;
			<LoadModDatabaseLogoAsync>d__.<>t__builder.Start<ModListItemViewModel.<LoadModDatabaseLogoAsync>d__353>(ref <LoadModDatabaseLogoAsync>d__);
			return <LoadModDatabaseLogoAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00031F40 File Offset: 0x00030140
		public void SetIsActiveSilently(bool isActive)
		{
			if (this._isActive == isActive)
			{
				return;
			}
			this._suppressState = true;
			try
			{
				if (base.SetProperty<bool>(ref this._isActive, isActive, "SetIsActiveSilently"))
				{
					this.ActivationError = null;
					base.OnPropertyChanged("ActiveSortOrder");
					base.OnPropertyChanged("CanFixDependencyIssues");
				}
			}
			finally
			{
				this._suppressState = false;
			}
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00031FAC File Offset: 0x000301AC
		public void UpdateDependencyIssues(bool hasDependencyErrors, IReadOnlyList<ModDependencyInfo> missingDependencies)
		{
			ModLoadingTimingService timingService = this._timingService;
			using ((timingService != null) ? timingService.MeasureDependencyCheck() : null)
			{
				bool flag = false;
				if (this.DependencyHasErrors != hasDependencyErrors)
				{
					this.DependencyHasErrors = hasDependencyErrors;
					base.OnPropertyChanged("DependencyHasErrors");
					flag = true;
				}
				IReadOnlyList<ModDependencyInfo> readOnlyList = (missingDependencies != null && missingDependencies.Count > 0) ? Enumerable.ToArray<ModDependencyInfo>(missingDependencies) : Array.Empty<ModDependencyInfo>();
				if (this.MissingDependencies != readOnlyList && (this.MissingDependencies.Count != readOnlyList.Count || !Enumerable.SequenceEqual<ModDependencyInfo>(this.MissingDependencies, readOnlyList)))
				{
					this.MissingDependencies = readOnlyList;
					base.OnPropertyChanged("MissingDependencies");
					flag = true;
				}
				if (flag)
				{
					base.OnPropertyChanged("HasDependencyIssues");
					base.OnPropertyChanged("CanFixDependencyIssues");
					base.OnPropertyChanged("CanToggle");
					this.RefreshDependencyWarningOverride();
				}
			}
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0003208C File Offset: 0x0003028C
		private bool ResolveDependencyWarningOverride()
		{
			Func<string, string, bool> isDependencyWarningOverridden = this._isDependencyWarningOverridden;
			return isDependencyWarningOverridden != null && isDependencyWarningOverridden.Invoke(this.ModId, this.Version);
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x000320AC File Offset: 0x000302AC
		private Task ApplyActivationChangeAsync(bool previous, bool current)
		{
			ModListItemViewModel.<ApplyActivationChangeAsync>d__357 <ApplyActivationChangeAsync>d__;
			<ApplyActivationChangeAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ApplyActivationChangeAsync>d__.<>4__this = this;
			<ApplyActivationChangeAsync>d__.previous = previous;
			<ApplyActivationChangeAsync>d__.current = current;
			<ApplyActivationChangeAsync>d__.<>1__state = -1;
			<ApplyActivationChangeAsync>d__.<>t__builder.Start<ModListItemViewModel.<ApplyActivationChangeAsync>d__357>(ref <ApplyActivationChangeAsync>d__);
			return <ApplyActivationChangeAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00032100 File Offset: 0x00030300
		private void UpdateStatusFromErrors()
		{
			if (this.HasErrors)
			{
				this.StatusText = "Error";
				this.StatusDetails = this.AppendWarningText(this._metadataError ?? "Metadata error.");
				return;
			}
			if (this.HasLoadError)
			{
				if (this.CanOverrideDependencyWarning)
				{
					this.StatusText = "Warning";
					this.StatusDetails = this.AppendWarningText(("Dependency warning overridden. " + (this._loadError ?? string.Empty)).Trim());
					return;
				}
				this.StatusText = "Error";
				this.StatusDetails = this.AppendWarningText(this._loadError ?? string.Empty);
				return;
			}
			else
			{
				if (this.HasActivationError)
				{
					this.StatusText = "Error";
					this.StatusDetails = this.AppendWarningText(this.ActivationError ?? string.Empty);
					return;
				}
				if (!string.IsNullOrWhiteSpace(this._versionWarningMessage))
				{
					ModVersionVoteSummary userReportSummary = this.UserReportSummary;
					if (userReportSummary == null || !userReportSummary.HasPositiveCompatibilityConsensus())
					{
						this.StatusText = "Warning";
						this.StatusDetails = this._versionWarningMessage;
						return;
					}
				}
				if (this.CanUpdate)
				{
					this.StatusText = "Update";
					this.StatusDetails = (string.IsNullOrWhiteSpace(this._updateMessage) ? "Update available." : this._updateMessage);
					return;
				}
				this.StatusText = string.Empty;
				this.StatusDetails = string.Empty;
				return;
			}
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00032260 File Offset: 0x00030460
		private void UpdateTooltip()
		{
			string text = this.DisplayName;
			if (!string.IsNullOrWhiteSpace(this._description))
			{
				text = text + Environment.NewLine + Environment.NewLine + this._description.Trim();
			}
			if (!string.IsNullOrWhiteSpace(this._versionWarningMessage))
			{
				text = text + Environment.NewLine + Environment.NewLine + this._versionWarningMessage;
			}
			if (this.CanUpdate && !string.IsNullOrWhiteSpace(this._updateMessage))
			{
				text = text + Environment.NewLine + Environment.NewLine + this._updateMessage;
			}
			this.Tooltip = text;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x000322F4 File Offset: 0x000304F4
		internal bool MatchesSearchTokens(IReadOnlyList<string> tokens)
		{
			if (tokens.Count == 0)
			{
				return true;
			}
			if (this._searchIndex == null)
			{
				this._searchIndex = this.BuildSearchIndex(this._modEntry, this._constructorLocation);
				ModListItemViewModel.LogDebug("Lazy search index built for mod '" + this.DisplayName + "'.");
			}
			ReadOnlySpan<char> readOnlySpan = MemoryExtensions.AsSpan(this._searchIndex);
			foreach (string text in tokens)
			{
				if (MemoryExtensions.IndexOf(readOnlySpan, MemoryExtensions.AsSpan(text), 5) < 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x0003239C File Offset: 0x0003059C
		private void InitializeUpdateAvailability()
		{
			ModLoadingTimingService timingService = this._timingService;
			using ((timingService != null) ? timingService.MeasureUpdateCheck() : null)
			{
				ModReleaseInfo modReleaseInfo = this.SelectUpdateCandidateRelease();
				if (modReleaseInfo == null)
				{
					this.CanUpdate = false;
					this._updateMessage = null;
				}
				else if (!VersionStringUtility.IsCandidateVersionNewer(modReleaseInfo.Version, this.Version))
				{
					this.CanUpdate = false;
					this._updateMessage = null;
				}
				else
				{
					Func<string, string, bool> shouldSkipVersion = this._shouldSkipVersion;
					if (shouldSkipVersion != null && shouldSkipVersion.Invoke(this.ModId, modReleaseInfo.Version))
					{
						this.CanUpdate = false;
						this._updateMessage = null;
					}
					else
					{
						this.CanUpdate = true;
						this._updateMessage = this.BuildUpdateMessage(modReleaseInfo);
					}
				}
			}
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x0003245C File Offset: 0x0003065C
		[NullableContext(2)]
		private ModReleaseInfo SelectUpdateCandidateRelease()
		{
			if (this.LatestCompatibleRelease != null)
			{
				return this.LatestCompatibleRelease;
			}
			ModReleaseInfo latestRelease = this.LatestRelease;
			if (latestRelease != null && latestRelease.GameVersionTags.Count == 0)
			{
				return this.LatestRelease;
			}
			return null;
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x00032494 File Offset: 0x00030694
		private string BuildUpdateMessage(ModReleaseInfo release)
		{
			if (release.IsCompatibleWithInstalledGame)
			{
				return "Update available: " + release.Version;
			}
			if (this.LatestCompatibleRelease != null && !string.Equals(this.LatestCompatibleRelease.Version, release.Version, 5))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(40, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Update available: ");
				defaultInterpolatedStringHandler.AppendFormatted(release.Version);
				defaultInterpolatedStringHandler.AppendLiteral(" (latest compatible: ");
				defaultInterpolatedStringHandler.AppendFormatted(this.LatestCompatibleRelease.Version);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			return "Update available: " + release.Version + " (may be incompatible with your Vintage Story version)";
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x00032548 File Offset: 0x00030748
		private void InitializeVersionOptions()
		{
			if (this._releases.Count == 0 && string.IsNullOrWhiteSpace(this.Version))
			{
				this.VersionOptions = Array.Empty<ModVersionOptionViewModel>();
				base.OnPropertyChanged("VersionOptions");
				base.OnPropertyChanged("HasVersionOptions");
				base.SetProperty<ModVersionOptionViewModel>(ref this._selectedVersionOption, null, "InitializeVersionOptions");
				return;
			}
			string text = VersionStringUtility.Normalize(this.Version);
			List<ModVersionOptionViewModel> list = new List<ModVersionOptionViewModel>(this._releases.Count + 1);
			bool flag = false;
			foreach (ModReleaseInfo release in this._releases)
			{
				bool flag2 = ModListItemViewModel.IsReleaseInstalled(release, this.Version, text);
				if (flag2)
				{
					flag = true;
				}
				list.Add(ModVersionOptionViewModel.FromRelease(release, flag2));
			}
			if (!flag && !string.IsNullOrWhiteSpace(this.Version))
			{
				list.Insert(0, ModVersionOptionViewModel.FromInstalledVersion(this.Version, text));
			}
			IReadOnlyList<ModVersionOptionViewModel> readOnlyList2;
			if (list.Count != 0)
			{
				IReadOnlyList<ModVersionOptionViewModel> readOnlyList = list.AsReadOnly();
				readOnlyList2 = readOnlyList;
			}
			else
			{
				IReadOnlyList<ModVersionOptionViewModel> readOnlyList = Array.Empty<ModVersionOptionViewModel>();
				readOnlyList2 = readOnlyList;
			}
			IReadOnlyList<ModVersionOptionViewModel> readOnlyList3 = readOnlyList2;
			this.VersionOptions = readOnlyList3;
			base.OnPropertyChanged("VersionOptions");
			base.OnPropertyChanged("HasVersionOptions");
			ModVersionOptionViewModel modVersionOptionViewModel = Enumerable.FirstOrDefault<ModVersionOptionViewModel>(readOnlyList3, (ModVersionOptionViewModel option) => option.IsInstalled) ?? Enumerable.FirstOrDefault<ModVersionOptionViewModel>(readOnlyList3);
			base.SetProperty<ModVersionOptionViewModel>(ref this._selectedVersionOption, modVersionOptionViewModel, "InitializeVersionOptions");
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x000326CC File Offset: 0x000308CC
		private void UpdateNewerReleaseChangelogs()
		{
			ModLoadingTimingService timingService = this._timingService;
			using ((timingService != null) ? timingService.MeasureChangelogLoad() : null)
			{
				IReadOnlyList<ModListItemViewModel.ReleaseChangelog> readOnlyList = this.BuildNewerReleaseChangelogList();
				if (this.NewerReleaseChangelogs != readOnlyList)
				{
					this.NewerReleaseChangelogs = readOnlyList;
					base.OnPropertyChanged("NewerReleaseChangelogs");
					base.OnPropertyChanged("HasNewerReleaseChangelogs");
				}
			}
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x00032738 File Offset: 0x00030938
		private IReadOnlyList<ModListItemViewModel.ReleaseChangelog> BuildNewerReleaseChangelogList()
		{
			if (this._releases.Count == 0)
			{
				return Array.Empty<ModListItemViewModel.ReleaseChangelog>();
			}
			int num = this.FindInstalledReleaseIndex();
			int num2 = (num >= 0) ? num : this._releases.Count;
			if (num2 <= 0)
			{
				return Array.Empty<ModListItemViewModel.ReleaseChangelog>();
			}
			List<ModListItemViewModel.ReleaseChangelog> list = new List<ModListItemViewModel.ReleaseChangelog>();
			for (int i = 0; i < num2; i++)
			{
				ModReleaseInfo modReleaseInfo = this._releases[i];
				if (!string.IsNullOrWhiteSpace(modReleaseInfo.Changelog))
				{
					string text = modReleaseInfo.Changelog.Trim();
					if (text.Length != 0)
					{
						list.Add(new ModListItemViewModel.ReleaseChangelog(modReleaseInfo.Version, text));
					}
				}
			}
			if (list.Count != 0)
			{
				return list;
			}
			return Array.Empty<ModListItemViewModel.ReleaseChangelog>();
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x000327EC File Offset: 0x000309EC
		private int FindInstalledReleaseIndex()
		{
			if (this._releases.Count == 0 || string.IsNullOrWhiteSpace(this.Version))
			{
				return -1;
			}
			string text = this.Version.Trim();
			string text2 = VersionStringUtility.Normalize(this.Version);
			for (int i = 0; i < this._releases.Count; i++)
			{
				ModReleaseInfo modReleaseInfo = this._releases[i];
				if (!string.IsNullOrWhiteSpace(modReleaseInfo.Version) && string.Equals(modReleaseInfo.Version.Trim(), text, 5))
				{
					return i;
				}
				if (text2 != null && !string.IsNullOrWhiteSpace(modReleaseInfo.NormalizedVersion) && string.Equals(modReleaseInfo.NormalizedVersion, text2, 5))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x00032898 File Offset: 0x00030A98
		[NullableContext(2)]
		private static bool IsReleaseInstalled([Nullable(1)] ModReleaseInfo release, string installedVersion, string normalizedInstalled)
		{
			if (string.IsNullOrWhiteSpace(installedVersion))
			{
				return false;
			}
			string text = installedVersion.Trim();
			return (!string.IsNullOrWhiteSpace(release.Version) && string.Equals(release.Version.Trim(), text, 5)) || (normalizedInstalled != null && !string.IsNullOrWhiteSpace(release.NormalizedVersion) && string.Equals(release.NormalizedVersion, normalizedInstalled, 5));
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x000328F8 File Offset: 0x00030AF8
		private static bool DoesReleaseMatchVersion(ModReleaseInfo release, string trimmedVersion, [Nullable(2)] string normalizedVersion)
		{
			return (!string.IsNullOrWhiteSpace(release.Version) && string.Equals(release.Version.Trim(), trimmedVersion, 5)) || (!string.IsNullOrWhiteSpace(release.NormalizedVersion) && normalizedVersion != null && string.Equals(release.NormalizedVersion, normalizedVersion, 5));
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0003294C File Offset: 0x00030B4C
		[NullableContext(2)]
		private void InitializeVersionWarning(string installedGameVersion)
		{
			string installedVersion = VersionStringUtility.Normalize(installedGameVersion);
			IReadOnlyCollection<ValueTuple<string, string>> requiredVersions = this.BuildRequiredVersionCandidates();
			Func<bool> requireExactVersionMatch = this._requireExactVersionMatch;
			bool requireExact = requireExactVersionMatch != null && requireExactVersionMatch.Invoke();
			string versionWarningMessage;
			if (ModListItemViewModel.TryCreateVersionWarning(requiredVersions, installedVersion, requireExact, out versionWarningMessage))
			{
				this._versionWarningMessage = versionWarningMessage;
				return;
			}
			this._versionWarningMessage = null;
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00032994 File Offset: 0x00030B94
		[return: TupleElementNames(new string[]
		{
			"Normalized",
			"Original"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		private IReadOnlyList<ValueTuple<string, string>> BuildRequiredVersionCandidates()
		{
			if (this._databaseRequiredGameVersions.Count > 0)
			{
				List<ValueTuple<string, string>> list = new List<ValueTuple<string, string>>(this._databaseRequiredGameVersions.Count);
				foreach (string text in this._databaseRequiredGameVersions)
				{
					string text2 = VersionStringUtility.Normalize(text);
					if (!string.IsNullOrWhiteSpace(text2))
					{
						list.Add(new ValueTuple<string, string>(text2, text));
					}
				}
				if (list.Count > 0)
				{
					return list;
				}
			}
			ModDependencyInfo gameDependency = this._gameDependency;
			if (gameDependency != null && gameDependency.Version != null)
			{
				string text3 = VersionStringUtility.Normalize(gameDependency.Version);
				if (!string.IsNullOrWhiteSpace(text3))
				{
					return new ValueTuple<string, string>[]
					{
						new ValueTuple<string, string>(text3, gameDependency.Version)
					};
				}
			}
			return Array.Empty<ValueTuple<string, string>>();
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00032A6C File Offset: 0x00030C6C
		private string AppendWarningText(string baseText)
		{
			if (string.IsNullOrWhiteSpace(this._versionWarningMessage))
			{
				return baseText;
			}
			if (string.IsNullOrWhiteSpace(baseText))
			{
				return this._versionWarningMessage;
			}
			return baseText + Environment.NewLine + Environment.NewLine + this._versionWarningMessage;
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00032AA4 File Offset: 0x00030CA4
		[NullableContext(2)]
		private static bool TryCreateVersionWarning([TupleElementNames(new string[]
		{
			"Normalized",
			"Original"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})] IReadOnlyCollection<ValueTuple<string, string>> requiredVersions, string installedVersion, bool requireExact, out string message)
		{
			message = null;
			if (requiredVersions.Count == 0 || string.IsNullOrWhiteSpace(installedVersion))
			{
				return false;
			}
			int num;
			int num2;
			if (!ModListItemViewModel.TryGetMajorMinor(installedVersion, out num, out num2))
			{
				return false;
			}
			bool flag = false;
			foreach (ValueTuple<string, string> valueTuple in requiredVersions)
			{
				string item = valueTuple.Item1;
				int num3;
				int num4;
				if (ModListItemViewModel.TryGetMajorMinor(item, out num3, out num4))
				{
					flag = true;
					if (requireExact)
					{
						if (VersionStringUtility.MatchesFirstThreeDigits(item, installedVersion))
						{
							return false;
						}
					}
					else if (num3 == num && num4 == num2)
					{
						return false;
					}
				}
			}
			if (!flag)
			{
				return false;
			}
			message = "This mod isn't marked as compatible with your Vintage Story version, but might work anyway. Check user reports column or read the comments on the Mod DB page for more info.";
			return true;
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00032B50 File Offset: 0x00030D50
		private void UpdateModDatabaseMetrics(ModDatabaseInfo info, IReadOnlyList<ModReleaseInfo> releases)
		{
			int? num = (info != null) ? info.DownloadsLastThirtyDays : default(int?);
			int? num2 = (num != null) ? num : ModListItemViewModel.CalculateDownloadsLastThirtyDaysFromReleases(releases);
			num = this._databaseRecentDownloads;
			int? num3 = num2;
			if (!(num.GetValueOrDefault() == num3.GetValueOrDefault() & num != null == (num3 != null)))
			{
				this._databaseRecentDownloads = num2;
				base.OnPropertyChanged("RecentDownloadsDisplay");
				base.OnPropertyChanged("ModDatabaseRecentDownloadsSortKey");
			}
			num3 = ((info != null) ? info.DownloadsLastTenDays : default(int?));
			int? num4 = (num3 != null) ? num3 : ModListItemViewModel.CalculateDownloadsLastTenDaysFromReleases(releases);
			num3 = this._databaseTenDayDownloads;
			num = num4;
			if (!(num3.GetValueOrDefault() == num.GetValueOrDefault() & num3 != null == (num != null)))
			{
				this._databaseTenDayDownloads = num4;
				base.OnPropertyChanged("TenDayDownloadsDisplay");
			}
			DateTime? dateTime = ModListItemViewModel.DetermineLastUpdated(info, releases);
			if (this._modDatabaseLastUpdatedUtc != dateTime)
			{
				this._modDatabaseLastUpdatedUtc = dateTime;
				base.OnPropertyChanged("ModDatabaseLastUpdatedSortKey");
			}
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00032C92 File Offset: 0x00030E92
		private static int? CalculateDownloadsLastThirtyDaysFromReleases(IReadOnlyList<ModReleaseInfo> releases)
		{
			return ModListItemViewModel.CalculateRecentDownloadsFromReleases(releases, 30);
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x00032C9C File Offset: 0x00030E9C
		private static int? CalculateDownloadsLastTenDaysFromReleases(IReadOnlyList<ModReleaseInfo> releases)
		{
			return ModListItemViewModel.CalculateRecentDownloadsFromReleases(releases, 10);
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00032CA8 File Offset: 0x00030EA8
		private static int? CalculateRecentDownloadsFromReleases(IReadOnlyList<ModReleaseInfo> releases, int days)
		{
			if (releases.Count == 0)
			{
				return default(int?);
			}
			DateTime dateTime = DateTime.UtcNow.AddDays((double)(-(double)days));
			int num = 0;
			bool flag = false;
			foreach (ModReleaseInfo modReleaseInfo in releases)
			{
				DateTime? dateTime2 = (modReleaseInfo != null) ? modReleaseInfo.CreatedUtc : default(DateTime?);
				if (dateTime2 != null)
				{
					DateTime valueOrDefault = dateTime2.GetValueOrDefault();
					if (!(valueOrDefault < dateTime) && modReleaseInfo.Downloads != null)
					{
						flag = true;
						num += Math.Max(0, modReleaseInfo.Downloads.Value);
					}
				}
			}
			if (!flag)
			{
				return default(int?);
			}
			return new int?(num);
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00032D8C File Offset: 0x00030F8C
		private static DateTime? DetermineLastUpdated([Nullable(2)] ModDatabaseInfo info, IReadOnlyList<ModReleaseInfo> releases)
		{
			DateTime? dateTime = (info != null) ? info.LastReleasedUtc : default(DateTime?);
			if (dateTime != null)
			{
				DateTime valueOrDefault = dateTime.GetValueOrDefault();
				return new DateTime?(valueOrDefault);
			}
			return ModListItemViewModel.DetermineLastUpdatedFromReleases(releases);
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x00032DCC File Offset: 0x00030FCC
		[NullableContext(2)]
		private static DateTime GetDateModifiedTimestamp(string sourcePath)
		{
			if (string.IsNullOrWhiteSpace(sourcePath))
			{
				return DateTime.MinValue;
			}
			DateTime result;
			try
			{
				result = File.GetLastWriteTime(sourcePath);
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
			{
				result = DateTime.MinValue;
			}
			return result;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x00032E34 File Offset: 0x00031034
		private static DateTime? DetermineLastUpdatedFromReleases(IReadOnlyList<ModReleaseInfo> releases)
		{
			DateTime? result = default(DateTime?);
			foreach (ModReleaseInfo modReleaseInfo in releases)
			{
				DateTime? dateTime = (modReleaseInfo != null) ? modReleaseInfo.CreatedUtc : default(DateTime?);
				if (dateTime != null)
				{
					DateTime valueOrDefault = dateTime.GetValueOrDefault();
					if (result == null || valueOrDefault > result.Value)
					{
						result = new DateTime?(valueOrDefault);
					}
				}
			}
			return result;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x00032EC4 File Offset: 0x000310C4
		private static bool TryGetMajorMinor(string version, out int major, out int minor)
		{
			major = 0;
			minor = 0;
			ReadOnlySpan<char> readOnlySpan = MemoryExtensions.AsSpan(version);
			int num = MemoryExtensions.IndexOf<char>(readOnlySpan, '.');
			if (!int.TryParse(MemoryExtensions.Trim((num >= 0) ? readOnlySpan.Slice(0, num) : readOnlySpan), ref major))
			{
				return false;
			}
			if (num >= 0 && num + 1 < readOnlySpan.Length)
			{
				ReadOnlySpan<char> readOnlySpan2 = readOnlySpan.Slice(num + 1);
				int num2 = MemoryExtensions.IndexOf<char>(readOnlySpan2, '.');
				if (!int.TryParse(MemoryExtensions.Trim((num2 >= 0) ? readOnlySpan2.Slice(0, num2) : readOnlySpan2), ref minor))
				{
					return false;
				}
			}
			else
			{
				minor = 0;
			}
			return true;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x00032F50 File Offset: 0x00031150
		private string BuildSearchIndex(ModEntry entry, string location)
		{
			StringBuilder stringBuilder = new StringBuilder();
			ModListItemViewModel.AppendText(stringBuilder, this.DisplayName);
			ModListItemViewModel.AppendText(stringBuilder, entry.Name);
			ModListItemViewModel.AppendText(stringBuilder, entry.ModId);
			ModListItemViewModel.AppendText(stringBuilder, this.ModId);
			ModListItemViewModel.AppendText(stringBuilder, entry.Version);
			ModListItemViewModel.AppendText(stringBuilder, this.Version);
			ModListItemViewModel.AppendText(stringBuilder, entry.NetworkVersion);
			ModListItemViewModel.AppendText(stringBuilder, this.NetworkVersion);
			ModListItemViewModel.AppendText(stringBuilder, entry.Description);
			ModListItemViewModel.AppendText(stringBuilder, this._description);
			ModListItemViewModel.AppendText(stringBuilder, entry.Error);
			ModListItemViewModel.AppendText(stringBuilder, this._metadataError);
			ModListItemViewModel.AppendText(stringBuilder, entry.LoadError);
			ModListItemViewModel.AppendText(stringBuilder, this._loadError);
			ModListItemViewModel.AppendText(stringBuilder, entry.Side);
			ModListItemViewModel.AppendText(stringBuilder, this.Side);
			ModListItemViewModel.AppendText(stringBuilder, location);
			ModListItemViewModel.AppendText(stringBuilder, entry.SourcePath);
			ModListItemViewModel.AppendText(stringBuilder, entry.SourceKind.ToString());
			ModListItemViewModel.AppendText(stringBuilder, this.Website);
			ModListItemViewModel.AppendText(stringBuilder, this.ModDatabasePageUrl);
			ModListItemViewModel.AppendText(stringBuilder, this.ModDatabaseAssetId);
			ModListItemViewModel.AppendText(stringBuilder, this.LatestDatabaseVersion);
			ModListItemViewModel.AppendText(stringBuilder, this.LatestDatabaseVersionDisplay);
			ModReleaseInfo latestRelease = this.LatestRelease;
			ModListItemViewModel.AppendText(stringBuilder, (latestRelease != null) ? latestRelease.Version : null);
			ModReleaseInfo latestCompatibleRelease = this.LatestCompatibleRelease;
			ModListItemViewModel.AppendText(stringBuilder, (latestCompatibleRelease != null) ? latestCompatibleRelease.Version : null);
			ModListItemViewModel.AppendCollection(stringBuilder, this.DatabaseTags);
			ModListItemViewModel.AppendCollection(stringBuilder, this._databaseRequiredGameVersions);
			ModListItemViewModel.AppendCollection(stringBuilder, this._authors);
			ModListItemViewModel.AppendCollection(stringBuilder, this._contributors);
			ModListItemViewModel.AppendDependencies(stringBuilder, this.Dependencies);
			ModListItemViewModel.AppendReleases(stringBuilder, this._releases);
			return stringBuilder.ToString();
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x00033108 File Offset: 0x00031308
		private static void AppendText(StringBuilder builder, [Nullable(2)] string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return;
			}
			ModListItemViewModel.AppendSegment(builder, value);
			string text = ModListItemViewModel.NormalizeSearchText(value);
			if (!string.IsNullOrEmpty(text) && !string.Equals(text, value, 4))
			{
				ModListItemViewModel.AppendSegment(builder, text);
			}
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x00033148 File Offset: 0x00031348
		private static void AppendCollection(StringBuilder builder, IEnumerable<string> values)
		{
			foreach (string value in values)
			{
				ModListItemViewModel.AppendText(builder, value);
			}
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x00033190 File Offset: 0x00031390
		private static void AppendSegment(StringBuilder builder, string value)
		{
			if (builder.Length > 0)
			{
				builder.Append(' ');
			}
			builder.Append(value);
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x000331AC File Offset: 0x000313AC
		private static bool HasDifferentContent<[Nullable(2)] T>(IReadOnlyList<T> current, IReadOnlyList<T> updated)
		{
			if (current == updated)
			{
				return false;
			}
			if (current.Count != updated.Count)
			{
				return true;
			}
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < current.Count; i++)
			{
				if (!@default.Equals(current[i], updated[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x00033200 File Offset: 0x00031400
		[NullableContext(2)]
		private string GetPreferredSide()
		{
			string text = ModListItemViewModel.NormalizeSide(this._modDatabaseSide);
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text;
			}
			return ModListItemViewModel.NormalizeSide(this.Side);
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00033230 File Offset: 0x00031430
		[NullableContext(2)]
		private static string NormalizeSide(string side)
		{
			if (string.IsNullOrWhiteSpace(side))
			{
				return null;
			}
			string text = side.Trim();
			if (text.Length == 0)
			{
				return null;
			}
			string text2 = text.ToLowerInvariant();
			string result;
			if (!(text2 == "both"))
			{
				if (!(text2 == "client"))
				{
					if (!(text2 == "server"))
					{
						if (!(text2 == "universal") && !(text2 == "all") && !(text2 == "any"))
						{
							result = text;
						}
						else
						{
							result = "both";
						}
					}
					else
					{
						result = "server";
					}
				}
				else
				{
					result = "client";
				}
			}
			else
			{
				result = "both";
			}
			return result;
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x000332D4 File Offset: 0x000314D4
		[NullableContext(2)]
		private unsafe static string GetCapitalizedSide(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}
			ReadOnlySpan<char> readOnlySpan = MemoryExtensions.Trim(MemoryExtensions.AsSpan(value));
			if (readOnlySpan.IsEmpty)
			{
				return null;
			}
			if (readOnlySpan.Length == 1)
			{
				return char.ToUpperInvariant((char)(*readOnlySpan[0])).ToString(CultureInfo.InvariantCulture);
			}
			return char.ToUpperInvariant((char)(*readOnlySpan[0])).ToString(CultureInfo.InvariantCulture) + readOnlySpan.Slice(1).ToString();
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x00033360 File Offset: 0x00031560
		private static string NormalizeSearchText(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return string.Empty;
			}
			string text = value.Normalize(2);
			StringBuilder stringBuilder = new StringBuilder(text.Length);
			string text2 = text;
			for (int i = 0; i < text2.Length; i++)
			{
				char c = text2.get_Chars(i);
				UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
				if (unicodeCategory != 5 && unicodeCategory != 6 && unicodeCategory != 7 && char.IsLetterOrDigit(c))
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x000333D8 File Offset: 0x000315D8
		private static void AppendDependencies(StringBuilder builder, IReadOnlyList<ModDependencyInfo> dependencies)
		{
			foreach (ModDependencyInfo modDependencyInfo in dependencies)
			{
				ModListItemViewModel.AppendText(builder, modDependencyInfo.ModId);
				ModListItemViewModel.AppendText(builder, modDependencyInfo.Display);
				ModListItemViewModel.AppendText(builder, modDependencyInfo.Version);
			}
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x00033440 File Offset: 0x00031640
		private static void AppendReleases(StringBuilder builder, IReadOnlyList<ModReleaseInfo> releases)
		{
			foreach (ModReleaseInfo modReleaseInfo in releases)
			{
				ModListItemViewModel.AppendText(builder, modReleaseInfo.Version);
				ModListItemViewModel.AppendCollection(builder, modReleaseInfo.GameVersionTags);
			}
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0003349C File Offset: 0x0003169C
		[NullableContext(2)]
		private static Uri TryCreateHttpUri(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}
			Uri uri;
			if (Uri.TryCreate(value, 1, ref uri) && ModListItemViewModel.IsSupportedScheme(uri))
			{
				return uri;
			}
			if (Uri.TryCreate("https://" + value, 1, ref uri) && ModListItemViewModel.IsSupportedScheme(uri))
			{
				return uri;
			}
			return null;
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x000334E8 File Offset: 0x000316E8
		private static bool IsSupportedScheme(Uri uri)
		{
			return string.Equals(uri.Scheme, Uri.UriSchemeHttp, 5) || string.Equals(uri.Scheme, Uri.UriSchemeHttps, 5);
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x00033510 File Offset: 0x00031710
		private static void LaunchUri(Uri uri)
		{
			if (InternetAccessManager.IsInternetAccessDisabled)
			{
				ModManagerMessageBox.Show("Internet access is disabled. Enable Internet Access in the File menu to open web links.", "Simple VS Manager", 0, 64, null, null, null);
				return;
			}
			try
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = uri.AbsoluteUri,
					UseShellExecute = true
				});
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x00033570 File Offset: 0x00031770
		[NullableContext(2)]
		private ImageSource CreateModDatabaseLogoImage()
		{
			string modDatabaseLogoUrl = this._modDatabaseLogoUrl;
			if (string.IsNullOrWhiteSpace(modDatabaseLogoUrl))
			{
				return null;
			}
			try
			{
				byte[] array = ModImageCacheService.TryGetCachedImage(modDatabaseLogoUrl, this.GetModLogoCacheDescriptor());
				if (array != null && array.Length > 0)
				{
					Uri uri = ModListItemViewModel.TryCreateHttpUri(modDatabaseLogoUrl);
					if (uri != null)
					{
						ImageSource imageSource = this.CreateBitmapFromBytes(array, uri);
						if (imageSource != null)
						{
							ModListItemViewModel.LogDebug("Loaded database logo from cache for '" + ModListItemViewModel.FormatValue(modDatabaseLogoUrl) + "'.");
							return imageSource;
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return this.CreateImageFromUri(modDatabaseLogoUrl, "Mod database logo", false);
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x00033600 File Offset: 0x00031800
		private ModImageCacheDescriptor GetModLogoCacheDescriptor()
		{
			return new ModImageCacheDescriptor(this.ModId, this.DisplayName, this._modDatabaseLogoSource);
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0003361C File Offset: 0x0003181C
		[return: Nullable(2)]
		private ImageSource CreateBitmapFromBytes(byte[] payload, Uri sourceUri)
		{
			ImageSource result;
			try
			{
				using (MemoryStream memoryStream = new MemoryStream(payload, false))
				{
					BitmapImage bitmapImage = new BitmapImage();
					bitmapImage.BeginInit();
					bitmapImage.CacheOption = 1;
					bitmapImage.StreamSource = memoryStream;
					bitmapImage.EndInit();
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Async mod database logo (");
					defaultInterpolatedStringHandler.AppendFormatted<Uri>(sourceUri);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					string context = defaultInterpolatedStringHandler.ToStringAndClear();
					Action<string> log;
					if ((log = ModListItemViewModel.<>O.<0>__LogDebug) == null)
					{
						log = (ModListItemViewModel.<>O.<0>__LogDebug = new Action<string>(ModListItemViewModel.LogDebug));
					}
					ModListItemViewModel.TryFreezeImageSource(bitmapImage, context, log);
					result = bitmapImage;
				}
			}
			catch (Exception ex)
			{
				ModListItemViewModel.LogDebug("Async database logo load failed to create bitmap: " + ex.Message + ".");
				result = null;
			}
			return result;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x000336EC File Offset: 0x000318EC
		[NullableContext(2)]
		private ImageSource CreateImageFromUri(string url, [Nullable(1)] string context, bool enableLogging = true)
		{
			string text = ModListItemViewModel.FormatValue(url);
			if (enableLogging)
			{
				ModListItemViewModel.LogDebug(context + ": Attempting to create image from URL " + text + ".");
			}
			Uri uri = ModListItemViewModel.TryCreateHttpUri(url);
			if (uri == null)
			{
				if (enableLogging)
				{
					ModListItemViewModel.LogDebug(context + ": Unable to resolve absolute URI from " + text + ".");
				}
				return null;
			}
			if (enableLogging)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(18, 2);
				defaultInterpolatedStringHandler.AppendFormatted(context);
				defaultInterpolatedStringHandler.AppendLiteral(": Resolved URI '");
				defaultInterpolatedStringHandler.AppendFormatted<Uri>(uri);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				ModListItemViewModel.LogDebug(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (InternetAccessManager.IsInternetAccessDisabled)
			{
				if (enableLogging)
				{
					ModListItemViewModel.LogDebug(context + ": Skipping image load because internet access is disabled.");
				}
				return null;
			}
			Func<ImageSource> factory = delegate()
			{
				BitmapImage bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.UriSource = uri;
				bitmapImage.CacheOption = 1;
				bitmapImage.EndInit();
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5;
				defaultInterpolatedStringHandler5..ctor(3, 2);
				defaultInterpolatedStringHandler5.AppendFormatted(context);
				defaultInterpolatedStringHandler5.AppendLiteral(" (");
				defaultInterpolatedStringHandler5.AppendFormatted<Uri>(uri);
				defaultInterpolatedStringHandler5.AppendLiteral(")");
				string context2 = defaultInterpolatedStringHandler5.ToStringAndClear();
				object log;
				if (!enableLogging)
				{
					log = null;
				}
				else if ((log = ModListItemViewModel.<>O.<0>__LogDebug) == null)
				{
					log = (ModListItemViewModel.<>O.<0>__LogDebug = new Action<string>(ModListItemViewModel.LogDebug));
				}
				ModListItemViewModel.TryFreezeImageSource(bitmapImage, context2, log);
				return bitmapImage;
			};
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
			defaultInterpolatedStringHandler2..ctor(3, 2);
			defaultInterpolatedStringHandler2.AppendFormatted(context);
			defaultInterpolatedStringHandler2.AppendLiteral(" (");
			defaultInterpolatedStringHandler2.AppendFormatted<Uri>(uri);
			defaultInterpolatedStringHandler2.AppendLiteral(")");
			ImageSource imageSource = this.CreateImageSafely(factory, defaultInterpolatedStringHandler2.ToStringAndClear(), enableLogging);
			if (enableLogging)
			{
				string message;
				if (imageSource != null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3;
					defaultInterpolatedStringHandler3..ctor(36, 2);
					defaultInterpolatedStringHandler3.AppendFormatted(context);
					defaultInterpolatedStringHandler3.AppendLiteral(": Successfully loaded image from '");
					defaultInterpolatedStringHandler3.AppendFormatted<Uri>(uri);
					defaultInterpolatedStringHandler3.AppendLiteral("'.");
					message = defaultInterpolatedStringHandler3.ToStringAndClear();
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4;
					defaultInterpolatedStringHandler4..ctor(31, 2);
					defaultInterpolatedStringHandler4.AppendFormatted(context);
					defaultInterpolatedStringHandler4.AppendLiteral(": Failed to load image from '");
					defaultInterpolatedStringHandler4.AppendFormatted<Uri>(uri);
					defaultInterpolatedStringHandler4.AppendLiteral("'.");
					message = defaultInterpolatedStringHandler4.ToStringAndClear();
				}
				ModListItemViewModel.LogDebug(message);
			}
			return imageSource;
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x000338E4 File Offset: 0x00031AE4
		[NullableContext(2)]
		private ImageSource CreateImage(byte[] bytes, [Nullable(1)] string context)
		{
			int num = (bytes != null) ? bytes.Length : 0;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(39, 2);
			defaultInterpolatedStringHandler.AppendFormatted(context);
			defaultInterpolatedStringHandler.AppendLiteral(": Received ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler.AppendLiteral(" byte(s) for image creation.");
			ModListItemViewModel.LogDebug(defaultInterpolatedStringHandler.ToStringAndClear());
			if (bytes == null || num == 0)
			{
				ModListItemViewModel.LogDebug(context + ": No bytes available; skipping image creation.");
				return null;
			}
			byte[] buffer = bytes;
			ImageSource imageSource = this.CreateImageSafely(delegate
			{
				ImageSource result;
				using (MemoryStream memoryStream = new MemoryStream(buffer))
				{
					BitmapImage bitmapImage = new BitmapImage();
					bitmapImage.BeginInit();
					bitmapImage.CacheOption = 1;
					bitmapImage.StreamSource = memoryStream;
					bitmapImage.CreateOptions = 5;
					bitmapImage.EndInit();
					string context2 = context + " (byte stream)";
					Action<string> log;
					if ((log = ModListItemViewModel.<>O.<0>__LogDebug) == null)
					{
						log = (ModListItemViewModel.<>O.<0>__LogDebug = new Action<string>(ModListItemViewModel.LogDebug));
					}
					ModListItemViewModel.TryFreezeImageSource(bitmapImage, context2, log);
					result = bitmapImage;
				}
				return result;
			}, context + " (byte stream)", true);
			ModListItemViewModel.LogDebug((imageSource == null) ? (context + ": Failed to create bitmap from bytes.") : (context + ": Successfully created bitmap from bytes."));
			return imageSource;
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x000339BC File Offset: 0x00031BBC
		[return: Nullable(2)]
		private ImageSource CreateImageSafely([Nullable(new byte[]
		{
			1,
			2
		})] Func<ImageSource> factory, string context, bool enableLogging = true)
		{
			ImageSource result;
			try
			{
				ImageSource imageSource = factory.Invoke();
				if (enableLogging)
				{
					ModListItemViewModel.LogDebug(context + ": Image creation completed on current thread with result " + ((imageSource == null) ? "null" : "available") + ".");
				}
				result = imageSource;
			}
			catch (Exception ex)
			{
				if (enableLogging)
				{
					ModListItemViewModel.LogDebug(context + ": Exception during image creation: " + ex.Message + ".");
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x00033A30 File Offset: 0x00031C30
		private static Task InvokeOnDispatcherAsync(Action action, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.CompletedTask;
			}
			Application application = Application.Current;
			Dispatcher dispatcher = (application != null) ? application.Dispatcher : null;
			if (dispatcher == null)
			{
				action.Invoke();
				return Task.CompletedTask;
			}
			if (dispatcher.CheckAccess())
			{
				action.Invoke();
				return Task.CompletedTask;
			}
			return dispatcher.InvokeAsync(action, 9, cancellationToken).Task;
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x00033A90 File Offset: 0x00031C90
		private static void LogDebug(string message)
		{
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x00033A94 File Offset: 0x00031C94
		private static void TryFreezeImageSource(ImageSource image, string context, [Nullable(new byte[]
		{
			2,
			1
		})] Action<string> log)
		{
			if (image == null)
			{
				return;
			}
			if (image.IsFrozen)
			{
				return;
			}
			if (!image.CanFreeze)
			{
				if (log != null)
				{
					log.Invoke(context + ": Image cannot be frozen; continuing without freezing.");
				}
				return;
			}
			try
			{
				image.Freeze();
			}
			catch (Exception ex)
			{
				if (log != null)
				{
					log.Invoke(context + ": Failed to freeze image: " + ex.Message + ". Continuing without freezing.");
				}
			}
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x00033B08 File Offset: 0x00031D08
		private static string FormatValue([Nullable(2)] string value)
		{
			if (value == null)
			{
				return "<null>";
			}
			if (string.IsNullOrWhiteSpace(value))
			{
				return "<empty>";
			}
			return value;
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x00033B22 File Offset: 0x00031D22
		private static string FormatUri([Nullable(2)] Uri uri)
		{
			return ((uri != null) ? uri.AbsoluteUri : null) ?? "<null>";
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x00033B39 File Offset: 0x00031D39
		private IDisposable SuspendPropertyChangeNotifications()
		{
			return new ModListItemViewModel.PropertyChangeSuspensionScope(this);
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x00033B41 File Offset: 0x00031D41
		protected override void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (this._propertyChangeSuspendCount > 0)
			{
				if (e.PropertyName != null)
				{
					this._pendingPropertyChanges.Add(e.PropertyName);
				}
				return;
			}
			base.OnPropertyChanged(e);
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00033B6E File Offset: 0x00031D6E
		private void BeginPropertyChangeSuspension()
		{
			this._propertyChangeSuspendCount++;
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x00033B80 File Offset: 0x00031D80
		private void EndPropertyChangeSuspension()
		{
			if (this._propertyChangeSuspendCount == 0)
			{
				return;
			}
			this._propertyChangeSuspendCount--;
			if (this._propertyChangeSuspendCount == 0 && this._pendingPropertyChanges.Count > 0)
			{
				foreach (string text in this._pendingPropertyChanges)
				{
					base.OnPropertyChanged(new PropertyChangedEventArgs(text));
				}
				this._pendingPropertyChanges.Clear();
			}
		}

		// Token: 0x040003CB RID: 971
		private static readonly HttpClient HttpClient = new HttpClient
		{
			Timeout = TimeSpan.FromSeconds(15.0)
		};

		// Token: 0x040003CC RID: 972
		private readonly Func<ModListItemViewModel, bool, Task<ActivationResult>> _activationHandler;

		// Token: 0x040003CD RID: 973
		private readonly IReadOnlyList<string> _authors;

		// Token: 0x040003CE RID: 974
		private readonly IReadOnlyList<string> _contributors;

		// Token: 0x040003CF RID: 975
		[Nullable(2)]
		private readonly string _description;

		// Token: 0x040003D0 RID: 976
		[Nullable(2)]
		private readonly ModDependencyInfo _gameDependency;

		// Token: 0x040003D1 RID: 977
		[Nullable(2)]
		private readonly string _installedGameVersion;

		// Token: 0x040003D2 RID: 978
		[Nullable(2)]
		private readonly string _metadataError;

		// Token: 0x040003D3 RID: 979
		[Nullable(2)]
		private readonly Func<bool> _requireExactVersionMatch;

		// Token: 0x040003D4 RID: 980
		[Nullable(2)]
		private readonly Func<string, string, bool> _isDependencyWarningOverridden;

		// Token: 0x040003D5 RID: 981
		[Nullable(2)]
		private readonly ModLoadingTimingService _timingService;

		// Token: 0x040003D6 RID: 982
		[Nullable(2)]
		private string _searchIndex;

		// Token: 0x040003D7 RID: 983
		[Nullable(2)]
		private readonly Func<string, string, bool> _shouldSkipVersion;

		// Token: 0x040003D8 RID: 984
		private readonly ModEntry _modEntry;

		// Token: 0x040003D9 RID: 985
		private readonly string _constructorLocation;

		// Token: 0x040003DA RID: 986
		[Nullable(2)]
		private string _activationError;

		// Token: 0x040003DB RID: 987
		private int? _databaseComments;

		// Token: 0x040003DC RID: 988
		private int? _databaseDownloads;

		// Token: 0x040003DD RID: 989
		private int? _databaseRecentDownloads;

		// Token: 0x040003DE RID: 990
		private IReadOnlyList<string> _databaseRequiredGameVersions;

		// Token: 0x040003DF RID: 991
		private int? _databaseTenDayDownloads;

		// Token: 0x040003E0 RID: 992
		private bool _hasActivationError;

		// Token: 0x040003E1 RID: 993
		private bool _isActive;

		// Token: 0x040003E2 RID: 994
		private bool _dependencyWarningOverridden;

		// Token: 0x040003E3 RID: 995
		private bool _isSelected;

		// Token: 0x040003E4 RID: 996
		private bool _isUserReportLoading;

		// Token: 0x040003E5 RID: 997
		[Nullable(2)]
		private string _latestReleaseUserReportDisplay;

		// Token: 0x040003E6 RID: 998
		[Nullable(2)]
		private string _latestReleaseUserReportTooltip;

		// Token: 0x040003E7 RID: 999
		[Nullable(2)]
		private string _loadError;

		// Token: 0x040003E8 RID: 1000
		private DateTime? _modDatabaseLastUpdatedUtc;

		// Token: 0x040003E9 RID: 1001
		[Nullable(2)]
		private ImageSource _modDatabaseLogo;

		// Token: 0x040003EA RID: 1002
		[Nullable(2)]
		private string _modDatabaseLogoSource;

		// Token: 0x040003EB RID: 1003
		[Nullable(2)]
		private string _modDatabaseLogoUrl;

		// Token: 0x040003EC RID: 1004
		[Nullable(2)]
		private string _modDatabaseSide;

		// Token: 0x040003ED RID: 1005
		[Nullable(2)]
		private ICommand _openModDatabasePageCommand;

		// Token: 0x040003EE RID: 1006
		private IReadOnlyList<ModReleaseInfo> _releases;

		// Token: 0x040003EF RID: 1007
		[Nullable(2)]
		private ModVersionOptionViewModel _selectedVersionOption;

		// Token: 0x040003F0 RID: 1008
		private string _statusDetails = string.Empty;

		// Token: 0x040003F1 RID: 1009
		private string _statusText = string.Empty;

		// Token: 0x040003F2 RID: 1010
		private bool _suppressState;

		// Token: 0x040003F3 RID: 1011
		private string _tooltip = string.Empty;

		// Token: 0x040003F4 RID: 1012
		[Nullable(2)]
		private string _updateMessage;

		// Token: 0x040003F5 RID: 1013
		private string _userReportDisplay = "—";

		// Token: 0x040003F6 RID: 1014
		private bool _userReportHasError;

		// Token: 0x040003F7 RID: 1015
		[Nullable(2)]
		private string _userReportTooltip;

		// Token: 0x040003F8 RID: 1016
		[Nullable(2)]
		private string _versionWarningMessage;

		// Token: 0x040003F9 RID: 1017
		private bool _hasInitializedUserReportState;

		// Token: 0x040003FA RID: 1018
		[Nullable(2)]
		private ImageSource _icon;

		// Token: 0x040003FB RID: 1019
		private bool _iconInitialized;

		// Token: 0x040003FC RID: 1020
		[Nullable(2)]
		private string _cachedDatabaseTagsDisplay;

		// Token: 0x040003FD RID: 1021
		private int _propertyChangeSuspendCount;

		// Token: 0x040003FE RID: 1022
		private readonly HashSet<string> _pendingPropertyChanges = new HashSet<string>();

		// Token: 0x02000257 RID: 599
		[NullableContext(0)]
		private sealed class PropertyChangeSuspensionScope : IDisposable
		{
			// Token: 0x06001527 RID: 5415 RVA: 0x0007D321 File Offset: 0x0007B521
			[NullableContext(1)]
			public PropertyChangeSuspensionScope(ModListItemViewModel viewModel)
			{
				this._viewModel = viewModel;
				this._viewModel.BeginPropertyChangeSuspension();
			}

			// Token: 0x06001528 RID: 5416 RVA: 0x0007D33B File Offset: 0x0007B53B
			public void Dispose()
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
				this._viewModel.EndPropertyChangeSuspension();
			}

			// Token: 0x04000E9B RID: 3739
			[Nullable(1)]
			private readonly ModListItemViewModel _viewModel;

			// Token: 0x04000E9C RID: 3740
			private bool _disposed;
		}

		// Token: 0x02000258 RID: 600
		[Nullable(0)]
		public sealed class ReleaseChangelog : IEquatable<ModListItemViewModel.ReleaseChangelog>
		{
			// Token: 0x06001529 RID: 5417 RVA: 0x0007D358 File Offset: 0x0007B558
			public ReleaseChangelog(string Version, string Changelog)
			{
				this.Version = Version;
				this.Changelog = Changelog;
				base..ctor();
			}

			// Token: 0x1700046A RID: 1130
			// (get) Token: 0x0600152A RID: 5418 RVA: 0x0007D36E File Offset: 0x0007B56E
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(ModListItemViewModel.ReleaseChangelog);
				}
			}

			// Token: 0x1700046B RID: 1131
			// (get) Token: 0x0600152B RID: 5419 RVA: 0x0007D37A File Offset: 0x0007B57A
			// (set) Token: 0x0600152C RID: 5420 RVA: 0x0007D382 File Offset: 0x0007B582
			public string Version { get; set; }

			// Token: 0x1700046C RID: 1132
			// (get) Token: 0x0600152D RID: 5421 RVA: 0x0007D38B File Offset: 0x0007B58B
			// (set) Token: 0x0600152E RID: 5422 RVA: 0x0007D393 File Offset: 0x0007B593
			public string Changelog { get; set; }

			// Token: 0x0600152F RID: 5423 RVA: 0x0007D39C File Offset: 0x0007B59C
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("ReleaseChangelog");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06001530 RID: 5424 RVA: 0x0007D3E8 File Offset: 0x0007B5E8
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Version = ");
				builder.Append(this.Version);
				builder.Append(", Changelog = ");
				builder.Append(this.Changelog);
				return true;
			}

			// Token: 0x06001531 RID: 5425 RVA: 0x0007D422 File Offset: 0x0007B622
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(ModListItemViewModel.ReleaseChangelog left, ModListItemViewModel.ReleaseChangelog right)
			{
				return !(left == right);
			}

			// Token: 0x06001532 RID: 5426 RVA: 0x0007D42E File Offset: 0x0007B62E
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(ModListItemViewModel.ReleaseChangelog left, ModListItemViewModel.ReleaseChangelog right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x06001533 RID: 5427 RVA: 0x0007D442 File Offset: 0x0007B642
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Version>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Changelog>k__BackingField);
			}

			// Token: 0x06001534 RID: 5428 RVA: 0x0007D482 File Offset: 0x0007B682
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as ModListItemViewModel.ReleaseChangelog);
			}

			// Token: 0x06001535 RID: 5429 RVA: 0x0007D490 File Offset: 0x0007B690
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(ModListItemViewModel.ReleaseChangelog other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Version>k__BackingField, other.<Version>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Changelog>k__BackingField, other.<Changelog>k__BackingField));
			}

			// Token: 0x06001537 RID: 5431 RVA: 0x0007D4F1 File Offset: 0x0007B6F1
			[CompilerGenerated]
			private ReleaseChangelog(ModListItemViewModel.ReleaseChangelog original)
			{
				this.Version = original.<Version>k__BackingField;
				this.Changelog = original.<Changelog>k__BackingField;
			}

			// Token: 0x06001538 RID: 5432 RVA: 0x0007D511 File Offset: 0x0007B711
			[CompilerGenerated]
			public void Deconstruct(out string Version, out string Changelog)
			{
				Version = this.Version;
				Changelog = this.Changelog;
			}
		}

		// Token: 0x02000259 RID: 601
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000E9F RID: 3743
			[Nullable(new byte[]
			{
				0,
				1
			})]
			public static Action<string> <0>__LogDebug;
		}
	}
}
