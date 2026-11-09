using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using CommunityToolkit.Mvvm.Input;
using SimpleVsManager.Cloud;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services;

namespace VintageStoryModManager.ViewModels
{
	// Token: 0x02000042 RID: 66
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MainViewModel : ObservableObject, IDisposable
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600056D RID: 1389 RVA: 0x000258F4 File Offset: 0x00023AF4
		// (remove) Token: 0x0600056E RID: 1390 RVA: 0x0002592C File Offset: 0x00023B2C
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public event EventHandler<MainViewModel.ModUserReportChangedEventArgs> UserReportVoteSubmitted;

		// Token: 0x0600056F RID: 1391 RVA: 0x00025964 File Offset: 0x00023B64
		public MainViewModel(string dataDirectory, UserConfigurationService configuration, [Nullable(2)] string gameDirectory = null)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(dataDirectory, "dataDirectory");
			ArgumentNullException.ThrowIfNull(configuration, "configuration");
			this.DataDirectory = Path.GetFullPath(dataDirectory);
			this._voteEtagCachePath = Path.Combine(this.DataDirectory, "voteEtags.json");
			this._configuration = configuration;
			this._settingsStore = new ClientSettingsStore(this.DataDirectory);
			this.PerformClientSettingsCleanupIfNeeded();
			this._discoveryService = new ModDiscoveryService(this._settingsStore);
			this._databaseService = new ModDatabaseService();
			this._tagFilterService = new TagFilterService(this._tagCache);
			this.InstalledGameVersion = VintageStoryVersionLocator.GetInstalledVersion(gameDirectory);
			this._modsWatcher = new ModDirectoryWatcher(this._discoveryService);
			this._clientSettingsWatcher = new ClientSettingsWatcher(this._settingsStore.SettingsPath);
			this.LoadVoteEtagsFromDisk();
			this.ModsView = CollectionViewSource.GetDefaultView(this._mods);
			this.ModsView.Filter = new Predicate<object>(this.FilterMod);
			this.SearchResultsView = CollectionViewSource.GetDefaultView(this._searchResults);
			this.CloudModlistsView = CollectionViewSource.GetDefaultView(this._cloudModlists);
			this.LocalModlistsView = CollectionViewSource.GetDefaultView(this._localModlists);
			this.InstalledTagFilters = new ReadOnlyObservableCollection<TagFilterOptionViewModel>(this._installedTagFilters);
			this._mods.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnModsCollectionChanged);
			this._searchResults.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnSearchResultsCollectionChanged);
			this._sortOptions = new ObservableCollection<SortOption>(MainViewModel.CreateSortOptions());
			this.SortOptions = new ReadOnlyObservableCollection<SortOption>(this._sortOptions);
			this.SelectedSortOption = Enumerable.FirstOrDefault<SortOption>(this.SortOptions);
			SortOption selectedSortOption = this.SelectedSortOption;
			if (selectedSortOption != null)
			{
				selectedSortOption.Apply(this.ModsView);
			}
			this._hasEnabledUserReportFetching = FirebaseAnonymousAuthenticator.HasPersistedState();
			this._isAutoRefreshDisabled = configuration.DisableAutoRefresh;
			this._allowModDetailsRefresh = !this._isAutoRefreshDisabled;
			this._clearSearchCommand = new RelayCommand(delegate()
			{
				this.SearchText = string.Empty;
			}, () => this.HasSearchText);
			this.ClearSearchCommand = this._clearSearchCommand;
			this._clearInstalledTagFiltersCommand = new RelayCommand(new Action(this.ClearInstalledTagFilters), () => this.HasSelectedTags);
			this.ClearInstalledTagFiltersCommand = this._clearInstalledTagFiltersCommand;
			this._showMainTabCommand = new RelayCommand(delegate()
			{
				this.SetViewSection(MainViewModel.ViewSection.MainTab);
			});
			this._showDatabaseTabCommand = new RelayCommand(delegate()
			{
				this.SetViewSection(MainViewModel.ViewSection.DatabaseTab);
			}, () => !InternetAccessManager.IsInternetAccessDisabled);
			this._showModlistTabCommand = new RelayCommand(delegate()
			{
				this.SetViewSection(MainViewModel.ViewSection.ModlistTab);
			}, () => !InternetAccessManager.IsInternetAccessDisabled);
			this.ShowMainTabCommand = this._showMainTabCommand;
			this.ShowDatabaseTabCommand = this._showDatabaseTabCommand;
			this.ShowModlistTabCommand = this._showModlistTabCommand;
			this.RefreshCommand = new AsyncRelayCommand(new Func<Task>(this.LoadModsAsync));
			this.SetStatus("Ready.", false, false);
			InternetAccessManager.InternetAccessChanged += new EventHandler(this.OnInternetAccessChanged);
			this.ResetFastCheckTimer();
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x00025E38 File Offset: 0x00024038
		public string DataDirectory { get; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x00025E40 File Offset: 0x00024040
		[Nullable(2)]
		public string PlayerUid
		{
			[NullableContext(2)]
			get
			{
				return this._settingsStore.PlayerUid;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x00025E4D File Offset: 0x0002404D
		[Nullable(2)]
		public string PlayerName
		{
			[NullableContext(2)]
			get
			{
				return this._settingsStore.PlayerName;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x00025E5A File Offset: 0x0002405A
		public ICollectionView ModsView { get; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x00025E62 File Offset: 0x00024062
		public ICollectionView SearchResultsView { get; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x00025E6A File Offset: 0x0002406A
		public ICollectionView CloudModlistsView { get; }

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x00025E72 File Offset: 0x00024072
		public ICollectionView LocalModlistsView { get; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x00025E7A File Offset: 0x0002407A
		public ModDirectoryWatcher ModsWatcher
		{
			get
			{
				return this._modsWatcher;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x00025E82 File Offset: 0x00024082
		public ModLoadingTimingService TimingService
		{
			get
			{
				return this._timingService;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x00025E8A File Offset: 0x0002408A
		public ReadOnlyObservableCollection<TagFilterOptionViewModel> InstalledTagFilters { get; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x00025E94 File Offset: 0x00024094
		public ICollectionView CurrentModsView
		{
			get
			{
				MainViewModel.ViewSection viewSection = this._viewSection;
				ICollectionView result;
				if (viewSection != MainViewModel.ViewSection.DatabaseTab)
				{
					if (viewSection != MainViewModel.ViewSection.ModlistTab)
					{
						result = this.ModsView;
					}
					else
					{
						result = this.CloudModlistsView;
					}
				}
				else
				{
					result = this.SearchResultsView;
				}
				return result;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x00025ECC File Offset: 0x000240CC
		public bool CanAccessCloudModlists
		{
			get
			{
				return !InternetAccessManager.IsInternetAccessDisabled;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x00025ED6 File Offset: 0x000240D6
		public ReadOnlyObservableCollection<SortOption> SortOptions { get; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x00025EDE File Offset: 0x000240DE
		// (set) Token: 0x0600057E RID: 1406 RVA: 0x00025EE6 File Offset: 0x000240E6
		[Nullable(2)]
		public SortOption SelectedSortOption
		{
			[NullableContext(2)]
			get
			{
				return this._selectedSortOption;
			}
			[NullableContext(2)]
			set
			{
				if (base.SetProperty<SortOption>(ref this._selectedSortOption, value, "SelectedSortOption") && value != null)
				{
					value.Apply(this.ModsView);
				}
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x00025F0B File Offset: 0x0002410B
		// (set) Token: 0x06000580 RID: 1408 RVA: 0x00025F13 File Offset: 0x00024113
		public bool IsBusy
		{
			get
			{
				return this._isBusy;
			}
			private set
			{
				base.SetProperty<bool>(ref this._isBusy, value, "IsBusy");
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x00025F28 File Offset: 0x00024128
		// (set) Token: 0x06000582 RID: 1410 RVA: 0x00025F30 File Offset: 0x00024130
		public bool IsLoadingMods
		{
			get
			{
				return this._isLoadingMods;
			}
			private set
			{
				if (base.SetProperty<bool>(ref this._isLoadingMods, value, "IsLoadingMods"))
				{
					this.RecalculateIsBusy();
				}
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x00025F4C File Offset: 0x0002414C
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x00025F54 File Offset: 0x00024154
		public double LoadingProgress
		{
			get
			{
				return this._loadingProgress;
			}
			private set
			{
				base.SetProperty<double>(ref this._loadingProgress, value, "LoadingProgress");
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x00025F69 File Offset: 0x00024169
		// (set) Token: 0x06000586 RID: 1414 RVA: 0x00025F71 File Offset: 0x00024171
		public string LoadingStatusText
		{
			get
			{
				return this._loadingStatusText;
			}
			private set
			{
				base.SetProperty<string>(ref this._loadingStatusText, value, "LoadingStatusText");
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x00025F86 File Offset: 0x00024186
		// (set) Token: 0x06000588 RID: 1416 RVA: 0x00025F8E File Offset: 0x0002418E
		public bool IsLoadingModDetails
		{
			get
			{
				return this._isLoadingModDetails;
			}
			private set
			{
				if (base.SetProperty<bool>(ref this._isLoadingModDetails, value, "IsLoadingModDetails"))
				{
					this.RecalculateIsBusy();
					this.UpdateModDetailsProgressVisibility();
				}
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x00025FB0 File Offset: 0x000241B0
		// (set) Token: 0x0600058A RID: 1418 RVA: 0x00025FB8 File Offset: 0x000241B8
		public bool IsModDetailsProgressVisible
		{
			get
			{
				return this._isModDetailsProgressVisible;
			}
			private set
			{
				base.SetProperty<bool>(ref this._isModDetailsProgressVisible, value, "IsModDetailsProgressVisible");
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x00025FCD File Offset: 0x000241CD
		// (set) Token: 0x0600058C RID: 1420 RVA: 0x00025FD5 File Offset: 0x000241D5
		public bool IsFastCheckInProgress
		{
			get
			{
				return this._isFastCheckInProgress;
			}
			private set
			{
				if (base.SetProperty<bool>(ref this._isFastCheckInProgress, value, "IsFastCheckInProgress"))
				{
					this.UpdateModDetailsProgressVisibility();
				}
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x00025FF1 File Offset: 0x000241F1
		// (set) Token: 0x0600058E RID: 1422 RVA: 0x00025FF9 File Offset: 0x000241F9
		public double ModDetailsProgress
		{
			get
			{
				return this._modDetailsProgress;
			}
			private set
			{
				base.SetProperty<double>(ref this._modDetailsProgress, value, "ModDetailsProgress");
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x0002600E File Offset: 0x0002420E
		// (set) Token: 0x06000590 RID: 1424 RVA: 0x00026016 File Offset: 0x00024216
		public string ModDetailsStatusText
		{
			get
			{
				return this._modDetailsStatusText;
			}
			private set
			{
				base.SetProperty<string>(ref this._modDetailsStatusText, value, "ModDetailsStatusText");
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x0002602B File Offset: 0x0002422B
		// (set) Token: 0x06000592 RID: 1426 RVA: 0x00026033 File Offset: 0x00024233
		public bool IsCompactView
		{
			get
			{
				return this._isCompactView;
			}
			set
			{
				base.SetProperty<bool>(ref this._isCompactView, value, "IsCompactView");
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x00026048 File Offset: 0x00024248
		// (set) Token: 0x06000594 RID: 1428 RVA: 0x00026050 File Offset: 0x00024250
		public bool HasSelectedTags
		{
			get
			{
				return this._hasSelectedTags;
			}
			private set
			{
				if (base.SetProperty<bool>(ref this._hasSelectedTags, value, "HasSelectedTags"))
				{
					base.OnPropertyChanged("TagsColumnHeader");
				}
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x00026071 File Offset: 0x00024271
		public string TagsColumnHeader
		{
			get
			{
				if (!this.HasSelectedTags)
				{
					return "Tags";
				}
				return "Tags (*)";
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x00026086 File Offset: 0x00024286
		// (set) Token: 0x06000597 RID: 1431 RVA: 0x0002608E File Offset: 0x0002428E
		public bool IsModInfoExpanded
		{
			get
			{
				return this._isModInfoExpanded;
			}
			set
			{
				base.SetProperty<bool>(ref this._isModInfoExpanded, value, "IsModInfoExpanded");
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000598 RID: 1432 RVA: 0x000260A3 File Offset: 0x000242A3
		// (set) Token: 0x06000599 RID: 1433 RVA: 0x000260AB File Offset: 0x000242AB
		public string StatusMessage
		{
			get
			{
				return this._statusMessage;
			}
			private set
			{
				if (base.SetProperty<string>(ref this._statusMessage, value, "StatusMessage"))
				{
					base.OnPropertyChanged("HasStatusMessage");
				}
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600059A RID: 1434 RVA: 0x000260CC File Offset: 0x000242CC
		public bool HasStatusMessage
		{
			get
			{
				return !string.IsNullOrWhiteSpace(this.StatusMessage);
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x000260DC File Offset: 0x000242DC
		// (set) Token: 0x0600059C RID: 1436 RVA: 0x000260E4 File Offset: 0x000242E4
		[Nullable(2)]
		public ModListItemViewModel SelectedMod
		{
			[NullableContext(2)]
			get
			{
				return this._selectedMod;
			}
			[NullableContext(2)]
			private set
			{
				if (base.SetProperty<ModListItemViewModel>(ref this._selectedMod, value, "SelectedMod"))
				{
					base.OnPropertyChanged("HasSelectedMod");
				}
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x00026105 File Offset: 0x00024305
		public bool HasSelectedMod
		{
			get
			{
				return this.SelectedMod != null;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x00026110 File Offset: 0x00024310
		// (set) Token: 0x0600059F RID: 1439 RVA: 0x00026118 File Offset: 0x00024318
		public bool HasSelectedMods
		{
			get
			{
				return this._hasSelectedMods;
			}
			private set
			{
				base.SetProperty<bool>(ref this._hasSelectedMods, value, "HasSelectedMods");
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x0002612D File Offset: 0x0002432D
		// (set) Token: 0x060005A1 RID: 1441 RVA: 0x00026135 File Offset: 0x00024335
		public bool HasMultipleSelectedMods
		{
			get
			{
				return this._hasMultipleSelectedMods;
			}
			private set
			{
				base.SetProperty<bool>(ref this._hasMultipleSelectedMods, value, "HasMultipleSelectedMods");
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x0002614A File Offset: 0x0002434A
		// (set) Token: 0x060005A3 RID: 1443 RVA: 0x00026152 File Offset: 0x00024352
		public bool IsErrorStatus
		{
			get
			{
				return this._isErrorStatus;
			}
			private set
			{
				base.SetProperty<bool>(ref this._isErrorStatus, value, "IsErrorStatus");
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x00026167 File Offset: 0x00024367
		public IRelayCommand ShowMainTabCommand { get; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x0002616F File Offset: 0x0002436F
		public IRelayCommand ShowDatabaseTabCommand { get; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060005A6 RID: 1446 RVA: 0x00026177 File Offset: 0x00024377
		public IRelayCommand ShowModlistTabCommand { get; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x0002617F File Offset: 0x0002437F
		public bool IsViewingModlistTab
		{
			get
			{
				return this._viewSection == MainViewModel.ViewSection.ModlistTab;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x0002618A File Offset: 0x0002438A
		public bool IsViewingMainTab
		{
			get
			{
				return this._viewSection == MainViewModel.ViewSection.MainTab;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060005A9 RID: 1449 RVA: 0x00026195 File Offset: 0x00024395
		public bool SearchModDatabase
		{
			get
			{
				return this._viewSection == MainViewModel.ViewSection.DatabaseTab;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x000261A0 File Offset: 0x000243A0
		// (set) Token: 0x060005AB RID: 1451 RVA: 0x000261A8 File Offset: 0x000243A8
		public bool UseModDbDesignView
		{
			get
			{
				return this._useModDbDesignView;
			}
			set
			{
				base.SetProperty<bool>(ref this._useModDbDesignView, value, "UseModDbDesignView");
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x000261BD File Offset: 0x000243BD
		public bool HasCloudModlists
		{
			get
			{
				return this._cloudModlists.Count > 0;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x000261CD File Offset: 0x000243CD
		public bool HasLocalModlists
		{
			get
			{
				return this._localModlists.Count > 0;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x000261DD File Offset: 0x000243DD
		// (set) Token: 0x060005AF RID: 1455 RVA: 0x000261E8 File Offset: 0x000243E8
		public string SearchText
		{
			get
			{
				return this._searchText;
			}
			set
			{
				string text = value ?? string.Empty;
				if (!base.SetProperty<string>(ref this._searchText, text, "SearchText"))
				{
					return;
				}
				bool flag = this._searchTokens.Length != 0;
				this._searchTokens = MainViewModel.CreateSearchTokens(text);
				bool flag2 = this._searchTokens.Length != 0;
				base.OnPropertyChanged("HasSearchText");
				this._clearSearchCommand.NotifyCanExecuteChanged();
				if (flag || flag2)
				{
					this.TriggerDebouncedInstalledModsSearch();
				}
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x00026256 File Offset: 0x00024456
		public bool HasSearchText
		{
			get
			{
				return this._searchTokens.Length != 0;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060005B1 RID: 1457 RVA: 0x00026262 File Offset: 0x00024462
		// (set) Token: 0x060005B2 RID: 1458 RVA: 0x0002626A File Offset: 0x0002446A
		public int TotalMods
		{
			get
			{
				return this._totalMods;
			}
			private set
			{
				if (base.SetProperty<int>(ref this._totalMods, value, "TotalMods"))
				{
					base.OnPropertyChanged("SummaryText");
				}
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060005B3 RID: 1459 RVA: 0x0002628B File Offset: 0x0002448B
		// (set) Token: 0x060005B4 RID: 1460 RVA: 0x00026293 File Offset: 0x00024493
		public int ActiveMods
		{
			get
			{
				return this._activeMods;
			}
			private set
			{
				if (base.SetProperty<int>(ref this._activeMods, value, "ActiveMods"))
				{
					base.OnPropertyChanged("SummaryText");
				}
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x000262B4 File Offset: 0x000244B4
		// (set) Token: 0x060005B6 RID: 1462 RVA: 0x000262BC File Offset: 0x000244BC
		public int UpdatableModsCount
		{
			get
			{
				return this._updatableModsCount;
			}
			private set
			{
				if (base.SetProperty<int>(ref this._updatableModsCount, value, "UpdatableModsCount"))
				{
					base.OnPropertyChanged("UpdateAllButtonLabel");
					base.OnPropertyChanged("UpdateAllModsMenuHeader");
				}
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060005B7 RID: 1463 RVA: 0x000262E8 File Offset: 0x000244E8
		public string SummaryText
		{
			get
			{
				if (this.TotalMods != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(16, 2);
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.ActiveMods);
					defaultInterpolatedStringHandler.AppendLiteral(" active of ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.TotalMods);
					defaultInterpolatedStringHandler.AppendLiteral(" mods");
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}
				return "No mods found.";
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x00026348 File Offset: 0x00024548
		public string UpdateAllButtonLabel
		{
			get
			{
				if (this.UpdatableModsCount != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(17, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Manage Updates (");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.UpdatableModsCount);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}
				return "Manage Updates";
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x0002639C File Offset: 0x0002459C
		public string UpdateAllModsMenuHeader
		{
			get
			{
				if (this.UpdatableModsCount != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(19, 1);
					defaultInterpolatedStringHandler.AppendLiteral("_Update All Mods (");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.UpdatableModsCount);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}
				return "_Update All Mods";
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x000263ED File Offset: 0x000245ED
		public string NoModsFoundMessage
		{
			get
			{
				return "No mods found. If this is unexpected, verify that your VintageStoryData folder is correctly set: " + this.DataDirectory + ". You can change it in the File Menu.";
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060005BB RID: 1467 RVA: 0x00026404 File Offset: 0x00024604
		public IRelayCommand ClearSearchCommand { get; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x0002640C File Offset: 0x0002460C
		public IRelayCommand ClearInstalledTagFiltersCommand { get; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x00026414 File Offset: 0x00024614
		public IAsyncRelayCommand RefreshCommand { get; }

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060005BE RID: 1470 RVA: 0x0002641C File Offset: 0x0002461C
		[Nullable(2)]
		public string InstalledGameVersion { [NullableContext(2)] get; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x00026424 File Offset: 0x00024624
		[Nullable(2)]
		public string EffectiveGameVersion
		{
			[NullableContext(2)]
			get
			{
				return TargetGameVersionPreference.ResolveEffectiveGameVersion(this.InstalledGameVersion, this._configuration.TargetGameVersionOverride);
			}
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0002643C File Offset: 0x0002463C
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			InternetAccessManager.InternetAccessChanged -= new EventHandler(this.OnInternetAccessChanged);
			this._mods.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnModsCollectionChanged);
			this._searchResults.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnSearchResultsCollectionChanged);
			this.DetachAllInstalledMods();
			this.DetachAllSearchResults();
			foreach (TagFilterOptionViewModel tagFilterOptionViewModel in this._installedTagFilters)
			{
				tagFilterOptionViewModel.PropertyChanged -= new PropertyChangedEventHandler(this.OnInstalledTagFilterPropertyChanged);
			}
			this._installedTagFilters.Clear();
			object obj = this._fastCheckTimerLock;
			lock (obj)
			{
				Timer fastCheckTimer = this._fastCheckTimer;
				if (fastCheckTimer != null)
				{
					fastCheckTimer.Dispose();
				}
				this._fastCheckTimer = null;
			}
			obj = this._searchDebounceLock;
			lock (obj)
			{
				Timer searchDebounceTimer = this._searchDebounceTimer;
				if (searchDebounceTimer != null)
				{
					searchDebounceTimer.Dispose();
				}
				this._searchDebounceTimer = null;
				CancellationTokenSource pendingSearchCts = this._pendingSearchCts;
				if (pendingSearchCts != null)
				{
					pendingSearchCts.Cancel();
				}
				CancellationTokenSource pendingSearchCts2 = this._pendingSearchCts;
				if (pendingSearchCts2 != null)
				{
					pendingSearchCts2.Dispose();
				}
				this._pendingSearchCts = null;
			}
			obj = this._databaseInfoBatchLock;
			lock (obj)
			{
				Timer databaseInfoBatchTimer = this._databaseInfoBatchTimer;
				if (databaseInfoBatchTimer != null)
				{
					databaseInfoBatchTimer.Dispose();
				}
				this._databaseInfoBatchTimer = null;
				this._pendingDatabaseInfoUpdates.Clear();
			}
			obj = this._databaseRefreshLock;
			lock (obj)
			{
				CancellationTokenSource databaseRefreshCts = this._databaseRefreshCts;
				if (databaseRefreshCts != null)
				{
					databaseRefreshCts.Cancel();
				}
				CancellationTokenSource databaseRefreshCts2 = this._databaseRefreshCts;
				if (databaseRefreshCts2 != null)
				{
					databaseRefreshCts2.Dispose();
				}
				this._databaseRefreshCts = null;
			}
			if (this._databaseRefreshTask != null)
			{
				try
				{
					this._databaseRefreshTask.Wait(TimeSpan.FromSeconds(2.0));
				}
				catch (AggregateException)
				{
				}
			}
			this._clientSettingsWatcher.Dispose();
			IDisposable modDetailsBusyScope = this._modDetailsBusyScope;
			if (modDetailsBusyScope != null)
			{
				modDetailsBusyScope.Dispose();
			}
			this._modDetailsBusyScope = null;
			this._userReportRefreshLimiter.Dispose();
			this._voteService.Dispose();
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x000266B0 File Offset: 0x000248B0
		public IDisposable EnterBusyScope()
		{
			return this.BeginBusyScope();
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x000266B8 File Offset: 0x000248B8
		public void SetInstalledColumnVisibility(string columnName, bool isVisible)
		{
			if (string.IsNullOrWhiteSpace(columnName))
			{
				return;
			}
			if (string.Equals(columnName, "Tags", 5))
			{
				this.SetTagsColumnVisibility(isVisible);
				return;
			}
			if (string.Equals(columnName, "UserReports", 5))
			{
				this.SetUserReportsColumnVisibility(isVisible);
			}
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x000266F0 File Offset: 0x000248F0
		private void SetViewSection(MainViewModel.ViewSection section)
		{
			if (this._viewSection == section)
			{
				return;
			}
			if (section == MainViewModel.ViewSection.DatabaseTab && InternetAccessManager.IsInternetAccessDisabled)
			{
				this.SetStatus("Enable Internet Access in the File menu to use.", false, false);
				return;
			}
			if (section == MainViewModel.ViewSection.ModlistTab && InternetAccessManager.IsInternetAccessDisabled)
			{
				this.SetStatus("Enable Internet Access in the File menu to use.", false, false);
				return;
			}
			this._viewSection = section;
			if (!string.IsNullOrEmpty(this._searchText))
			{
				this.SearchText = string.Empty;
			}
			switch (section)
			{
			case MainViewModel.ViewSection.MainTab:
				this.SelectedMod = null;
				this.SetStatus("Showing installed mods.", false, false);
				break;
			case MainViewModel.ViewSection.DatabaseTab:
				this.SelectedMod = null;
				this.SetStatus("Showing mod database.", false, false);
				break;
			case MainViewModel.ViewSection.ModlistTab:
				this.SelectedMod = null;
				this.SetStatus("Showing cloud modlists.", false, false);
				break;
			}
			base.OnPropertyChanged("IsViewingModlistTab");
			base.OnPropertyChanged("IsViewingMainTab");
			base.OnPropertyChanged("SearchModDatabase");
			base.OnPropertyChanged("CurrentModsView");
			Application application = Application.Current;
			if (application == null)
			{
				return;
			}
			application.Dispatcher.BeginInvoke(delegate()
			{
				if (this._viewSection == MainViewModel.ViewSection.MainTab)
				{
					this.FastCheck();
				}
			}, 4, Array.Empty<object>());
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00026800 File Offset: 0x00024A00
		public void FastCheck()
		{
			this.ResetFastCheckTimer();
			if (this._isAutoRefreshDisabled)
			{
				return;
			}
			if (InternetAccessManager.IsInternetAccessDisabled)
			{
				return;
			}
			this._hasPendingFastCheck = true;
			if (Interlocked.CompareExchange(ref this._isFastCheckRunning, 1, 0) == 0)
			{
				Task.Run(new Func<Task>(this.RunFastCheckAsync));
			}
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00026850 File Offset: 0x00024A50
		private Task RunFastCheckAsync()
		{
			MainViewModel.<RunFastCheckAsync>d__276 <RunFastCheckAsync>d__;
			<RunFastCheckAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RunFastCheckAsync>d__.<>4__this = this;
			<RunFastCheckAsync>d__.<>1__state = -1;
			<RunFastCheckAsync>d__.<>t__builder.Start<MainViewModel.<RunFastCheckAsync>d__276>(ref <RunFastCheckAsync>d__);
			return <RunFastCheckAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00026894 File Offset: 0x00024A94
		private void ResetFastCheckTimer()
		{
			if (this._disposed || this._isAutoRefreshDisabled)
			{
				return;
			}
			object fastCheckTimerLock = this._fastCheckTimerLock;
			lock (fastCheckTimerLock)
			{
				if (this._fastCheckTimer == null)
				{
					this._fastCheckTimer = new Timer(new TimerCallback(this.OnFastCheckTimerElapsed), null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
				}
				this._fastCheckTimer.Change(MainViewModel.FastCheckInterval, Timeout.InfiniteTimeSpan);
			}
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00026920 File Offset: 0x00024B20
		private void StopFastCheckTimer()
		{
			object fastCheckTimerLock = this._fastCheckTimerLock;
			lock (fastCheckTimerLock)
			{
				Timer fastCheckTimer = this._fastCheckTimer;
				if (fastCheckTimer != null)
				{
					fastCheckTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
				}
			}
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00026978 File Offset: 0x00024B78
		[NullableContext(2)]
		private void OnFastCheckTimerElapsed(object state)
		{
			if (this._disposed || this._isAutoRefreshDisabled)
			{
				return;
			}
			this.FastCheck();
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00026994 File Offset: 0x00024B94
		private Task<IReadOnlyList<ModEntry>> CheckForNewModReleasesAsync(CancellationToken cancellationToken, bool showProgress = true)
		{
			MainViewModel.<CheckForNewModReleasesAsync>d__280 <CheckForNewModReleasesAsync>d__;
			<CheckForNewModReleasesAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IReadOnlyList<ModEntry>>.Create();
			<CheckForNewModReleasesAsync>d__.<>4__this = this;
			<CheckForNewModReleasesAsync>d__.cancellationToken = cancellationToken;
			<CheckForNewModReleasesAsync>d__.showProgress = showProgress;
			<CheckForNewModReleasesAsync>d__.<>1__state = -1;
			<CheckForNewModReleasesAsync>d__.<>t__builder.Start<MainViewModel.<CheckForNewModReleasesAsync>d__280>(ref <CheckForNewModReleasesAsync>d__);
			return <CheckForNewModReleasesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x000269E8 File Offset: 0x00024BE8
		private Task CheckForVoteChangesAsync(CancellationToken cancellationToken)
		{
			MainViewModel.<CheckForVoteChangesAsync>d__281 <CheckForVoteChangesAsync>d__;
			<CheckForVoteChangesAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CheckForVoteChangesAsync>d__.<>4__this = this;
			<CheckForVoteChangesAsync>d__.cancellationToken = cancellationToken;
			<CheckForVoteChangesAsync>d__.<>1__state = -1;
			<CheckForVoteChangesAsync>d__.<>t__builder.Start<MainViewModel.<CheckForVoteChangesAsync>d__281>(ref <CheckForVoteChangesAsync>d__);
			return <CheckForVoteChangesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00026A34 File Offset: 0x00024C34
		private Task CheckVoteChangesForModAsync(MainViewModel.VoteCheckTarget target, SemaphoreSlim limiter, CancellationToken cancellationToken)
		{
			MainViewModel.<CheckVoteChangesForModAsync>d__282 <CheckVoteChangesForModAsync>d__;
			<CheckVoteChangesForModAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CheckVoteChangesForModAsync>d__.<>4__this = this;
			<CheckVoteChangesForModAsync>d__.target = target;
			<CheckVoteChangesForModAsync>d__.limiter = limiter;
			<CheckVoteChangesForModAsync>d__.cancellationToken = cancellationToken;
			<CheckVoteChangesForModAsync>d__.<>1__state = -1;
			<CheckVoteChangesForModAsync>d__.<>t__builder.Start<MainViewModel.<CheckVoteChangesForModAsync>d__282>(ref <CheckVoteChangesForModAsync>d__);
			return <CheckVoteChangesForModAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00026A8F File Offset: 0x00024C8F
		[NullableContext(2)]
		private void StoreUserReportEtag([Nullable(1)] string modId, string modVersion, string etag)
		{
			this.UpdateVoteEtag(this._userReportEtags, "current", modId, modVersion, etag);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00026AA5 File Offset: 0x00024CA5
		[NullableContext(2)]
		private void StoreLatestReleaseUserReportEtag([Nullable(1)] string modId, string modVersion, string etag)
		{
			this.UpdateVoteEtag(this._latestReleaseUserReportEtags, "latest", modId, modVersion, etag);
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00026ABC File Offset: 0x00024CBC
		[return: Nullable(2)]
		private string GetVoteEtag(Dictionary<string, string> source, string prefix, string modId, [Nullable(2)] string modVersion)
		{
			if (string.IsNullOrWhiteSpace(modId) || string.IsNullOrWhiteSpace(modVersion) || string.IsNullOrWhiteSpace(this.InstalledGameVersion))
			{
				return null;
			}
			string text = MainViewModel.BuildVoteEtagKey(prefix, modId, modVersion, this.InstalledGameVersion);
			string result;
			if (!source.TryGetValue(text, ref result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00026B08 File Offset: 0x00024D08
		private void UpdateVoteEtag(Dictionary<string, string> target, string prefix, string modId, [Nullable(2)] string modVersion, [Nullable(2)] string etag)
		{
			if (string.IsNullOrWhiteSpace(modId) || string.IsNullOrWhiteSpace(modVersion) || string.IsNullOrWhiteSpace(this.InstalledGameVersion))
			{
				return;
			}
			string text = MainViewModel.BuildVoteEtagKey(prefix, modId, modVersion, this.InstalledGameVersion);
			object voteEtagPersistenceLock = this._voteEtagPersistenceLock;
			lock (voteEtagPersistenceLock)
			{
				bool flag2 = false;
				string text2;
				if (string.IsNullOrEmpty(etag))
				{
					flag2 = target.Remove(text);
				}
				else if (!target.TryGetValue(text, ref text2) || !string.Equals(text2, etag, 4))
				{
					target[text] = etag;
					flag2 = true;
				}
				if (flag2)
				{
					this.PersistVoteEtagsLocked();
				}
			}
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00026BB4 File Offset: 0x00024DB4
		private static string BuildVoteEtagKey(string prefix, string modId, string modVersion, [Nullable(2)] string gameVersion)
		{
			return string.Concat(new object[]
			{
				prefix,
				'|',
				modId,
				'|',
				modVersion,
				'|',
				gameVersion ?? string.Empty
			});
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00026C04 File Offset: 0x00024E04
		private void LoadVoteEtagsFromDisk()
		{
			object voteEtagPersistenceLock = this._voteEtagPersistenceLock;
			lock (voteEtagPersistenceLock)
			{
				try
				{
					if (File.Exists(this._voteEtagCachePath))
					{
						using (FileStream fileStream = File.OpenRead(this._voteEtagCachePath))
						{
							MainViewModel.VoteEtagCacheState voteEtagCacheState = JsonSerializer.Deserialize<MainViewModel.VoteEtagCacheState>(fileStream, null);
							this._userReportEtags.Clear();
							this._latestReleaseUserReportEtags.Clear();
							Dictionary<string, string> dictionary = (voteEtagCacheState != null) ? voteEtagCacheState.Current : null;
							if (dictionary != null)
							{
								foreach (KeyValuePair<string, string> keyValuePair in dictionary)
								{
									this._userReportEtags[keyValuePair.Key] = keyValuePair.Value;
								}
							}
							Dictionary<string, string> dictionary2 = (voteEtagCacheState != null) ? voteEtagCacheState.Latest : null;
							if (dictionary2 != null)
							{
								foreach (KeyValuePair<string, string> keyValuePair2 in dictionary2)
								{
									this._latestReleaseUserReportEtags[keyValuePair2.Key] = keyValuePair2.Value;
								}
							}
						}
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00026DA8 File Offset: 0x00024FA8
		private void PersistVoteEtagsLocked()
		{
			try
			{
				string directoryName = Path.GetDirectoryName(this._voteEtagCachePath);
				if (!string.IsNullOrEmpty(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				MainViewModel.VoteEtagCacheState voteEtagCacheState = new MainViewModel.VoteEtagCacheState
				{
					Current = new Dictionary<string, string>(this._userReportEtags, StringComparer.OrdinalIgnoreCase),
					Latest = new Dictionary<string, string>(this._latestReleaseUserReportEtags, StringComparer.OrdinalIgnoreCase)
				};
				using (FileStream fileStream = File.Create(this._voteEtagCachePath))
				{
					JsonSerializer.Serialize<MainViewModel.VoteEtagCacheState>(fileStream, voteEtagCacheState, null);
				}
			}
			catch
			{
			}
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00026E44 File Offset: 0x00025044
		[NullableContext(2)]
		private static bool IsDifferentVersion(string installedVersion, string latestVersion)
		{
			if (string.IsNullOrWhiteSpace(latestVersion) || string.IsNullOrWhiteSpace(installedVersion))
			{
				return false;
			}
			string text = VersionStringUtility.Normalize(installedVersion);
			string text2 = VersionStringUtility.Normalize(latestVersion);
			if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(text2))
			{
				return !string.Equals(text, text2, 5);
			}
			return !string.Equals(installedVersion.Trim(), latestVersion.Trim(), 5);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00026EA2 File Offset: 0x000250A2
		public Task InitializeAsync()
		{
			return this.LoadModsAsync();
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00026EAC File Offset: 0x000250AC
		public void ReplaceCloudModlists([Nullable(new byte[]
		{
			2,
			1
		})] IEnumerable<CloudModlistListEntry> entries)
		{
			this._cloudModlists.Clear();
			if (entries != null)
			{
				foreach (CloudModlistListEntry cloudModlistListEntry in entries)
				{
					if (cloudModlistListEntry != null)
					{
						this._cloudModlists.Add(cloudModlistListEntry);
					}
				}
			}
			this.CloudModlistsView.Refresh();
			base.OnPropertyChanged("HasCloudModlists");
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00026F20 File Offset: 0x00025120
		public bool TryReplaceCloudModlist(CloudModlistListEntry existing, CloudModlistListEntry replacement)
		{
			int num = this._cloudModlists.IndexOf(existing);
			if (num < 0)
			{
				return false;
			}
			this._cloudModlists[num] = replacement;
			this.CloudModlistsView.Refresh();
			base.OnPropertyChanged("HasCloudModlists");
			return true;
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00026F64 File Offset: 0x00025164
		public void ReplaceLocalModlists([Nullable(new byte[]
		{
			2,
			1
		})] IEnumerable<LocalModlistListEntry> entries)
		{
			this._localModlists.Clear();
			if (entries != null)
			{
				foreach (LocalModlistListEntry localModlistListEntry in entries)
				{
					if (localModlistListEntry != null)
					{
						this._localModlists.Add(localModlistListEntry);
					}
				}
			}
			this.LocalModlistsView.Refresh();
			base.OnPropertyChanged("HasLocalModlists");
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00026FD8 File Offset: 0x000251D8
		public IReadOnlyList<string> GetCurrentDisabledEntries()
		{
			return this._settingsStore.GetDisabledEntriesSnapshot();
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00026FE5 File Offset: 0x000251E5
		public IReadOnlyList<ModPresetModState> GetCurrentModStates()
		{
			return Enumerable.ToList<ModPresetModState>(Enumerable.Select<ModListItemViewModel, ModPresetModState>(this._mods, (ModListItemViewModel mod) => new ModPresetModState(mod.ModId, mod.Version, new bool?(mod.IsActive), null, null, null)));
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00027016 File Offset: 0x00025216
		public IReadOnlyList<ModListItemViewModel> GetInstalledModsSnapshot()
		{
			return Enumerable.ToList<ModListItemViewModel>(this._mods);
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00027024 File Offset: 0x00025224
		public IReadOnlyList<ModUsageTrackingEntry> GetActiveModUsageSnapshot()
		{
			List<ModUsageTrackingEntry> list = new List<ModUsageTrackingEntry>();
			if (string.IsNullOrWhiteSpace(this.InstalledGameVersion))
			{
				return list;
			}
			string gameVersion = this.InstalledGameVersion.Trim();
			HashSet<ModUsageTrackingKey> hashSet = new HashSet<ModUsageTrackingKey>();
			foreach (ModListItemViewModel modListItemViewModel in this._mods)
			{
				if (modListItemViewModel != null && modListItemViewModel.IsActive && !string.IsNullOrWhiteSpace(modListItemViewModel.ModId) && !string.IsNullOrWhiteSpace(modListItemViewModel.Version))
				{
					string modId = modListItemViewModel.ModId.Trim();
					string modVersion = modListItemViewModel.Version.Trim();
					ModUsageTrackingKey modUsageTrackingKey = new ModUsageTrackingKey(modId, modVersion, gameVersion);
					if (hashSet.Add(modUsageTrackingKey))
					{
						list.Add(new ModUsageTrackingEntry(modId, modVersion, gameVersion, modListItemViewModel.CanSubmitUserReport, modListItemViewModel.UserVoteOption != null));
					}
				}
			}
			return list;
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0002711C File Offset: 0x0002531C
		public IReadOnlyList<string> GetActiveModIdsSnapshot()
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			List<string> list = new List<string>();
			foreach (ModListItemViewModel modListItemViewModel in this._mods)
			{
				if (modListItemViewModel != null && modListItemViewModel.IsActive && !string.IsNullOrWhiteSpace(modListItemViewModel.ModId))
				{
					string text = modListItemViewModel.ModId.Trim();
					if (text.Length != 0 && hashSet.Add(text))
					{
						list.Add(text);
					}
				}
			}
			return list;
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x000271B4 File Offset: 0x000253B4
		[NullableContext(2)]
		public bool TryGetInstalledModDisplayName(string modId, out string displayName)
		{
			displayName = null;
			if (string.IsNullOrWhiteSpace(modId))
			{
				return false;
			}
			foreach (ModListItemViewModel modListItemViewModel in this._mods)
			{
				if (modListItemViewModel != null && !string.IsNullOrWhiteSpace(modListItemViewModel.ModId) && string.Equals(modListItemViewModel.ModId, modId, 5))
				{
					displayName = modListItemViewModel.DisplayName;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00027238 File Offset: 0x00025438
		public Task<bool> ApplyPresetAsync(ModPreset preset)
		{
			MainViewModel.<ApplyPresetAsync>d__302 <ApplyPresetAsync>d__;
			<ApplyPresetAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<ApplyPresetAsync>d__.<>4__this = this;
			<ApplyPresetAsync>d__.preset = preset;
			<ApplyPresetAsync>d__.<>1__state = -1;
			<ApplyPresetAsync>d__.<>t__builder.Start<MainViewModel.<ApplyPresetAsync>d__302>(ref <ApplyPresetAsync>d__);
			return <ApplyPresetAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00027283 File Offset: 0x00025483
		public void ReportStatus(string message, bool isError = false)
		{
			this.SetStatus(message, isError, false);
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0002728E File Offset: 0x0002548E
		public void OnInternetAccessStateChanged()
		{
			if (this._allowModDetailsRefresh && this._modEntriesBySourcePath.Count > 0)
			{
				this.QueueDatabaseInfoRefresh(Enumerable.ToArray<ModEntry>(this._modEntriesBySourcePath.Values), false);
			}
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x000272BD File Offset: 0x000254BD
		internal void SetAutoRefreshDisabled(bool disabled)
		{
			this._isAutoRefreshDisabled = disabled;
			this._allowModDetailsRefresh = !this._isAutoRefreshDisabled;
			if (disabled)
			{
				this.StopFastCheckTimer();
				return;
			}
			this.ResetFastCheckTimer();
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x000272E5 File Offset: 0x000254E5
		internal void ForceNextRefreshToLoadDetails()
		{
			this._isModDetailsRefreshForced = true;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x000272EE File Offset: 0x000254EE
		internal void RefreshInstalledModDetails()
		{
			if (this._modEntriesBySourcePath.Count > 0)
			{
				this.QueueDatabaseInfoRefresh(Enumerable.ToArray<ModEntry>(this._modEntriesBySourcePath.Values), true);
			}
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00027315 File Offset: 0x00025515
		[NullableContext(2)]
		internal void SetSelectedMod(ModListItemViewModel mod, int selectionCount)
		{
			this.HasSelectedMods = (selectionCount > 0);
			this.HasMultipleSelectedMods = (selectionCount > 1);
			this.SelectedMod = mod;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00027332 File Offset: 0x00025532
		internal void RemoveSearchResult(ModListItemViewModel mod)
		{
			if (mod == null)
			{
				return;
			}
			this._searchResults.Remove(mod);
			if (this.SelectedMod == mod)
			{
				this.SelectedMod = null;
			}
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00027358 File Offset: 0x00025558
		[NullableContext(2)]
		public ModListItemViewModel FindInstalledModById(string modId)
		{
			if (string.IsNullOrWhiteSpace(modId))
			{
				return null;
			}
			string text = modId.Trim();
			foreach (ModListItemViewModel modListItemViewModel in this._mods)
			{
				if (modListItemViewModel != null && !string.IsNullOrWhiteSpace(modListItemViewModel.ModId) && string.Equals(modListItemViewModel.ModId.Trim(), text, 5))
				{
					return modListItemViewModel;
				}
			}
			return null;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x000273DC File Offset: 0x000255DC
		[NullableContext(2)]
		internal ModListItemViewModel FindModBySourcePath(string sourcePath)
		{
			if (string.IsNullOrWhiteSpace(sourcePath))
			{
				return null;
			}
			ModListItemViewModel result;
			if (!this._modViewModelsBySourcePath.TryGetValue(sourcePath, ref result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00027408 File Offset: 0x00025608
		internal Task<bool> PreserveActivationStateAsync(string modId, [Nullable(2)] string previousVersion, [Nullable(2)] string newVersion, bool wasActive)
		{
			MainViewModel.<PreserveActivationStateAsync>d__312 <PreserveActivationStateAsync>d__;
			<PreserveActivationStateAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<PreserveActivationStateAsync>d__.<>4__this = this;
			<PreserveActivationStateAsync>d__.modId = modId;
			<PreserveActivationStateAsync>d__.previousVersion = previousVersion;
			<PreserveActivationStateAsync>d__.newVersion = newVersion;
			<PreserveActivationStateAsync>d__.wasActive = wasActive;
			<PreserveActivationStateAsync>d__.<>1__state = -1;
			<PreserveActivationStateAsync>d__.<>t__builder.Start<MainViewModel.<PreserveActivationStateAsync>d__312>(ref <PreserveActivationStateAsync>d__);
			return <PreserveActivationStateAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0002746C File Offset: 0x0002566C
		internal Task<ActivationResult> ApplyActivationChangeAsync(ModListItemViewModel mod, bool isActive)
		{
			MainViewModel.<ApplyActivationChangeAsync>d__313 <ApplyActivationChangeAsync>d__;
			<ApplyActivationChangeAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ActivationResult>.Create();
			<ApplyActivationChangeAsync>d__.<>4__this = this;
			<ApplyActivationChangeAsync>d__.mod = mod;
			<ApplyActivationChangeAsync>d__.isActive = isActive;
			<ApplyActivationChangeAsync>d__.<>1__state = -1;
			<ApplyActivationChangeAsync>d__.<>t__builder.Start<MainViewModel.<ApplyActivationChangeAsync>d__313>(ref <ApplyActivationChangeAsync>d__);
			return <ApplyActivationChangeAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x000274C0 File Offset: 0x000256C0
		internal IReadOnlyCollection<string> GetSourcePathsForModsWithErrors()
		{
			if (this._mods.Count == 0)
			{
				return Array.Empty<string>();
			}
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (ModListItemViewModel modListItemViewModel in this._mods)
			{
				if (modListItemViewModel != null)
				{
					string sourcePath = modListItemViewModel.SourcePath;
					if (!string.IsNullOrWhiteSpace(sourcePath) && (modListItemViewModel.HasLoadError || modListItemViewModel.DependencyHasErrors || modListItemViewModel.MissingDependencies.Count > 0))
					{
						hashSet.Add(sourcePath);
					}
				}
			}
			if (hashSet.Count != 0)
			{
				return Enumerable.ToArray<string>(hashSet);
			}
			return Array.Empty<string>();
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00027570 File Offset: 0x00025770
		internal Task RefreshModsWithErrorsAsync([Nullable(new byte[]
		{
			2,
			1
		})] IReadOnlyCollection<string> includeSourcePaths = null)
		{
			MainViewModel.<RefreshModsWithErrorsAsync>d__315 <RefreshModsWithErrorsAsync>d__;
			<RefreshModsWithErrorsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RefreshModsWithErrorsAsync>d__.<>4__this = this;
			<RefreshModsWithErrorsAsync>d__.includeSourcePaths = includeSourcePaths;
			<RefreshModsWithErrorsAsync>d__.<>1__state = -1;
			<RefreshModsWithErrorsAsync>d__.<>t__builder.Start<MainViewModel.<RefreshModsWithErrorsAsync>d__315>(ref <RefreshModsWithErrorsAsync>d__);
			return <RefreshModsWithErrorsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x000275BC File Offset: 0x000257BC
		private Task LoadModsAsync()
		{
			MainViewModel.<LoadModsAsync>d__316 <LoadModsAsync>d__;
			<LoadModsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadModsAsync>d__.<>4__this = this;
			<LoadModsAsync>d__.<>1__state = -1;
			<LoadModsAsync>d__.<>t__builder.Start<MainViewModel.<LoadModsAsync>d__316>(ref <LoadModsAsync>d__);
			return <LoadModsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00027600 File Offset: 0x00025800
		private Task PerformFullReloadAsync([Nullable(2)] string previousSelection)
		{
			MainViewModel.<PerformFullReloadAsync>d__317 <PerformFullReloadAsync>d__;
			<PerformFullReloadAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<PerformFullReloadAsync>d__.<>4__this = this;
			<PerformFullReloadAsync>d__.previousSelection = previousSelection;
			<PerformFullReloadAsync>d__.<>1__state = -1;
			<PerformFullReloadAsync>d__.<>t__builder.Start<MainViewModel.<PerformFullReloadAsync>d__317>(ref <PerformFullReloadAsync>d__);
			return <PerformFullReloadAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0002764C File Offset: 0x0002584C
		public Task<bool> CheckForModStateChangesAsync()
		{
			MainViewModel.<CheckForModStateChangesAsync>d__319 <CheckForModStateChangesAsync>d__;
			<CheckForModStateChangesAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<CheckForModStateChangesAsync>d__.<>4__this = this;
			<CheckForModStateChangesAsync>d__.<>1__state = -1;
			<CheckForModStateChangesAsync>d__.<>t__builder.Start<MainViewModel.<CheckForModStateChangesAsync>d__319>(ref <CheckForModStateChangesAsync>d__);
			return <CheckForModStateChangesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00027690 File Offset: 0x00025890
		[return: TupleElementNames(new string[]
		{
			"success",
			"modStatesChanged"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0
		})]
		private Task<ValueTuple<bool, bool>> ApplyClientSettingsChangesAsync()
		{
			MainViewModel.<ApplyClientSettingsChangesAsync>d__320 <ApplyClientSettingsChangesAsync>d__;
			<ApplyClientSettingsChangesAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ValueTuple<bool, bool>>.Create();
			<ApplyClientSettingsChangesAsync>d__.<>4__this = this;
			<ApplyClientSettingsChangesAsync>d__.<>1__state = -1;
			<ApplyClientSettingsChangesAsync>d__.<>t__builder.Start<MainViewModel.<ApplyClientSettingsChangesAsync>d__320>(ref <ApplyClientSettingsChangesAsync>d__);
			return <ApplyClientSettingsChangesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x000276D4 File Offset: 0x000258D4
		private ModListItemViewModel CreateModViewModel(ModEntry entry)
		{
			bool isActive = !this._settingsStore.IsDisabled(entry.ModId, entry.Version);
			string displayPath = this.GetDisplayPath(entry.SourcePath);
			return new ModListItemViewModel(entry, isActive, displayPath, new Func<ModListItemViewModel, bool, Task<ActivationResult>>(this.ApplyActivationChangeAsync), this.EffectiveGameVersion, true, new Func<string, string, bool>(this._configuration.ShouldSkipModVersion), () => this._configuration.RequireExactVsVersionMatch, this._allowModDetailsRefresh, this._timingService, new Func<string, string, bool>(this._configuration.IsDependencyWarningOverridden));
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00027760 File Offset: 0x00025960
		private Task UpdateModsStateSnapshotAsync()
		{
			MainViewModel.<UpdateModsStateSnapshotAsync>d__322 <UpdateModsStateSnapshotAsync>d__;
			<UpdateModsStateSnapshotAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<UpdateModsStateSnapshotAsync>d__.<>4__this = this;
			<UpdateModsStateSnapshotAsync>d__.<>1__state = -1;
			<UpdateModsStateSnapshotAsync>d__.<>t__builder.Start<MainViewModel.<UpdateModsStateSnapshotAsync>d__322>(ref <UpdateModsStateSnapshotAsync>d__);
			return <UpdateModsStateSnapshotAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x000277A3 File Offset: 0x000259A3
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		private Task<string> CaptureModsStateFingerprintAsync()
		{
			return Task.Run<string>(delegate()
			{
				string result;
				try
				{
					result = this._discoveryService.GetModsStateFingerprint();
				}
				catch (Exception)
				{
					result = null;
				}
				return result;
			});
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x000277B6 File Offset: 0x000259B6
		private void UpdateActiveCount()
		{
			this.ActiveMods = Enumerable.Count<ModListItemViewModel>(this._mods, (ModListItemViewModel item) => item.IsActive);
			this.UpdateUpdatableCount();
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x000277EE File Offset: 0x000259EE
		private void UpdateUpdatableCount()
		{
			this.UpdatableModsCount = Enumerable.Count<ModListItemViewModel>(this._mods, (ModListItemViewModel item) => item.CanUpdate);
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00027820 File Offset: 0x00025A20
		private void ClearSearchResults()
		{
			if (this._searchResults.Count == 0)
			{
				this.SelectedMod = null;
				return;
			}
			Application application = Application.Current;
			Dispatcher dispatcher = (application != null) ? application.Dispatcher : null;
			if (dispatcher != null && !dispatcher.CheckAccess())
			{
				dispatcher.Invoke(delegate()
				{
					this._searchResults.Clear();
					this.SelectedMod = null;
				});
				return;
			}
			this._searchResults.Clear();
			this.SelectedMod = null;
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00027884 File Offset: 0x00025A84
		private void OnModsCollectionChanged([Nullable(2)] object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case 0:
				using (IEnumerator<ModListItemViewModel> enumerator = MainViewModel.EnumerateModItems(e.NewItems).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ModListItemViewModel mod = enumerator.Current;
						this.AttachInstalledMod(mod);
					}
					goto IL_13C;
				}
				break;
			case 1:
				break;
			case 2:
				goto IL_96;
			case 3:
				goto IL_13C;
			case 4:
				goto IL_104;
			default:
				goto IL_13C;
			}
			using (IEnumerator<ModListItemViewModel> enumerator = MainViewModel.EnumerateModItems(e.OldItems).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ModListItemViewModel mod2 = enumerator.Current;
					this.DetachInstalledMod(mod2);
				}
				goto IL_13C;
			}
			IL_96:
			foreach (ModListItemViewModel mod3 in MainViewModel.EnumerateModItems(e.OldItems))
			{
				this.DetachInstalledMod(mod3);
			}
			using (IEnumerator<ModListItemViewModel> enumerator = MainViewModel.EnumerateModItems(e.NewItems).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ModListItemViewModel mod4 = enumerator.Current;
					this.AttachInstalledMod(mod4);
				}
				goto IL_13C;
			}
			IL_104:
			this.DetachAllInstalledMods();
			foreach (ModListItemViewModel mod5 in this._mods)
			{
				this.AttachInstalledMod(mod5);
			}
			IL_13C:
			this.ScheduleInstalledTagFilterRefresh();
			this.UpdateActiveCount();
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00027A1C File Offset: 0x00025C1C
		private void OnSearchResultsCollectionChanged([Nullable(2)] object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case 0:
				using (IEnumerator<ModListItemViewModel> enumerator = MainViewModel.EnumerateModItems(e.NewItems).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ModListItemViewModel mod = enumerator.Current;
						this.AttachSearchResult(mod);
					}
					return;
				}
				break;
			case 1:
				break;
			case 2:
				goto IL_92;
			case 3:
				return;
			case 4:
				goto IL_100;
			default:
				return;
			}
			using (IEnumerator<ModListItemViewModel> enumerator = MainViewModel.EnumerateModItems(e.OldItems).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ModListItemViewModel mod2 = enumerator.Current;
					this.DetachSearchResult(mod2);
				}
				return;
			}
			IL_92:
			foreach (ModListItemViewModel mod3 in MainViewModel.EnumerateModItems(e.OldItems))
			{
				this.DetachSearchResult(mod3);
			}
			using (IEnumerator<ModListItemViewModel> enumerator = MainViewModel.EnumerateModItems(e.NewItems).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ModListItemViewModel mod4 = enumerator.Current;
					this.AttachSearchResult(mod4);
				}
				return;
			}
			IL_100:
			this.DetachAllSearchResults();
			foreach (ModListItemViewModel mod5 in this._searchResults)
			{
				this.AttachSearchResult(mod5);
			}
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00027BA4 File Offset: 0x00025DA4
		private static IEnumerable<ModListItemViewModel> EnumerateModItems([Nullable(2)] IList items)
		{
			MainViewModel.<EnumerateModItems>d__329 <EnumerateModItems>d__ = new MainViewModel.<EnumerateModItems>d__329(-2);
			<EnumerateModItems>d__.<>3__items = items;
			return <EnumerateModItems>d__;
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00027BB4 File Offset: 0x00025DB4
		private void AttachInstalledMod(ModListItemViewModel mod)
		{
			if (this._installedModSubscriptions.Add(mod))
			{
				mod.PropertyChanged += new PropertyChangedEventHandler(this.OnInstalledModPropertyChanged);
			}
			if (this._allowModDetailsRefresh)
			{
				this.QueueUserReportRefresh(mod);
			}
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00027BE5 File Offset: 0x00025DE5
		private void DetachInstalledMod(ModListItemViewModel mod)
		{
			if (this._installedModSubscriptions.Remove(mod))
			{
				mod.PropertyChanged -= new PropertyChangedEventHandler(this.OnInstalledModPropertyChanged);
			}
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00027C08 File Offset: 0x00025E08
		private void DetachAllInstalledMods()
		{
			if (this._installedModSubscriptions.Count == 0)
			{
				return;
			}
			foreach (ModListItemViewModel modListItemViewModel in this._installedModSubscriptions)
			{
				modListItemViewModel.PropertyChanged -= new PropertyChangedEventHandler(this.OnInstalledModPropertyChanged);
			}
			this._installedModSubscriptions.Clear();
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00027C80 File Offset: 0x00025E80
		private void AttachSearchResult(ModListItemViewModel mod)
		{
			if (this._searchResultSubscriptions.Add(mod))
			{
				mod.PropertyChanged += new PropertyChangedEventHandler(this.OnSearchResultPropertyChanged);
			}
			if (mod.CanSubmitUserReport)
			{
				this.QueueUserReportRefresh(mod);
			}
			this.QueueLatestReleaseUserReportRefresh(mod);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00027CB8 File Offset: 0x00025EB8
		private void DetachSearchResult(ModListItemViewModel mod)
		{
			if (this._searchResultSubscriptions.Remove(mod))
			{
				mod.PropertyChanged -= new PropertyChangedEventHandler(this.OnSearchResultPropertyChanged);
			}
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00027CDC File Offset: 0x00025EDC
		private void DetachAllSearchResults()
		{
			if (this._searchResultSubscriptions.Count == 0)
			{
				return;
			}
			foreach (ModListItemViewModel modListItemViewModel in this._searchResultSubscriptions)
			{
				modListItemViewModel.PropertyChanged -= new PropertyChangedEventHandler(this.OnSearchResultPropertyChanged);
			}
			this._searchResultSubscriptions.Clear();
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00027D54 File Offset: 0x00025F54
		private void OnInstalledModPropertyChanged([Nullable(2)] object sender, PropertyChangedEventArgs e)
		{
			if (string.Equals(e.PropertyName, "DatabaseTags", 4))
			{
				this.ScheduleInstalledTagFilterRefresh();
				return;
			}
			if (string.Equals(e.PropertyName, "IsActive", 4))
			{
				this.UpdateActiveCount();
				return;
			}
			if (string.Equals(e.PropertyName, "CanUpdate", 4))
			{
				this.UpdateUpdatableCount();
			}
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00027DB0 File Offset: 0x00025FB0
		private void OnSearchResultPropertyChanged([Nullable(2)] object sender, PropertyChangedEventArgs e)
		{
			ModListItemViewModel modListItemViewModel = sender as ModListItemViewModel;
			if (modListItemViewModel == null)
			{
				return;
			}
			if (string.Equals(e.PropertyName, "DatabaseTags", 4))
			{
				return;
			}
			if (string.Equals(e.PropertyName, "UserReportModVersion", 4) && modListItemViewModel.CanSubmitUserReport)
			{
				this.QueueUserReportRefresh(modListItemViewModel);
			}
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00027E00 File Offset: 0x00026000
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		private Task<ModVersionVoteSummary> RunUserReportOperationAsync([Nullable(new byte[]
		{
			1,
			1,
			2
		})] Func<CancellationToken, Task<ModVersionVoteSummary>> operation, CancellationToken cancellationToken)
		{
			MainViewModel.<RunUserReportOperationAsync>d__338 <RunUserReportOperationAsync>d__;
			<RunUserReportOperationAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ModVersionVoteSummary>.Create();
			<RunUserReportOperationAsync>d__.<>4__this = this;
			<RunUserReportOperationAsync>d__.operation = operation;
			<RunUserReportOperationAsync>d__.cancellationToken = cancellationToken;
			<RunUserReportOperationAsync>d__.<>1__state = -1;
			<RunUserReportOperationAsync>d__.<>t__builder.Start<MainViewModel.<RunUserReportOperationAsync>d__338>(ref <RunUserReportOperationAsync>d__);
			return <RunUserReportOperationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00027E54 File Offset: 0x00026054
		private IDisposable BeginUserReportOperation()
		{
			object userReportOperationLock = this._userReportOperationLock;
			lock (userReportOperationLock)
			{
				this._activeUserReportOperations++;
				int activeUserReportOperations = this._activeUserReportOperations;
			}
			IDisposable busyScope = this.BeginBusyScope();
			return new MainViewModel.UserReportOperationScope(this, busyScope);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00027EB4 File Offset: 0x000260B4
		private void EndUserReportOperation()
		{
			object userReportOperationLock = this._userReportOperationLock;
			lock (userReportOperationLock)
			{
				if (this._activeUserReportOperations > 0)
				{
					this._activeUserReportOperations--;
				}
				int activeUserReportOperations = this._activeUserReportOperations;
			}
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00027F0C File Offset: 0x0002610C
		private void QueueLatestReleaseUserReportRefresh(ModListItemViewModel mod)
		{
			if (mod == null)
			{
				return;
			}
			if (!this._areUserReportsVisible)
			{
				return;
			}
			this.RunUserReportOperationAsync((CancellationToken ct) => this.RefreshLatestReleaseUserReportCoreAsync(mod, true, ct), CancellationToken.None);
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x00027F58 File Offset: 0x00026158
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		public Task<ModVersionVoteSummary> RefreshLatestReleaseUserReportAsync(ModListItemViewModel mod, CancellationToken cancellationToken = default(CancellationToken))
		{
			return this.RunUserReportOperationAsync((CancellationToken ct) => this.RefreshLatestReleaseUserReportCoreAsync(mod, false, ct), cancellationToken);
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00027F8C File Offset: 0x0002618C
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		private Task<ModVersionVoteSummary> RefreshLatestReleaseUserReportCoreAsync(ModListItemViewModel mod, bool suppressErrors, CancellationToken cancellationToken)
		{
			MainViewModel.<RefreshLatestReleaseUserReportCoreAsync>d__343 <RefreshLatestReleaseUserReportCoreAsync>d__;
			<RefreshLatestReleaseUserReportCoreAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ModVersionVoteSummary>.Create();
			<RefreshLatestReleaseUserReportCoreAsync>d__.<>4__this = this;
			<RefreshLatestReleaseUserReportCoreAsync>d__.mod = mod;
			<RefreshLatestReleaseUserReportCoreAsync>d__.suppressErrors = suppressErrors;
			<RefreshLatestReleaseUserReportCoreAsync>d__.cancellationToken = cancellationToken;
			<RefreshLatestReleaseUserReportCoreAsync>d__.<>1__state = -1;
			<RefreshLatestReleaseUserReportCoreAsync>d__.<>t__builder.Start<MainViewModel.<RefreshLatestReleaseUserReportCoreAsync>d__343>(ref <RefreshLatestReleaseUserReportCoreAsync>d__);
			return <RefreshLatestReleaseUserReportCoreAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00027FE8 File Offset: 0x000261E8
		private void QueueUserReportRefresh(ModListItemViewModel mod)
		{
			if (mod == null)
			{
				return;
			}
			if (!this._areUserReportsVisible)
			{
				return;
			}
			this.RunUserReportOperationAsync((CancellationToken ct) => this.RefreshUserReportCoreAsync(mod, true, ct), CancellationToken.None);
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00028034 File Offset: 0x00026234
		public void EnableUserReportFetching(bool includeInstalledWhenAutoRefreshDisabled = false)
		{
			if (!this._areUserReportsVisible)
			{
				return;
			}
			if (this._hasFetchedUserReportsThisSession)
			{
				return;
			}
			this._hasEnabledUserReportFetching = true;
			bool flag = this._allowModDetailsRefresh || includeInstalledWhenAutoRefreshDisabled;
			bool flag2 = false;
			if (flag)
			{
				foreach (ModListItemViewModel modListItemViewModel in this._installedModSubscriptions)
				{
					modListItemViewModel.EnsureUserReportStateInitialized();
					this.QueueUserReportRefresh(modListItemViewModel);
					flag2 = true;
				}
			}
			foreach (ModListItemViewModel modListItemViewModel2 in this._searchResultSubscriptions)
			{
				modListItemViewModel2.EnsureUserReportStateInitialized();
				if (modListItemViewModel2.CanSubmitUserReport)
				{
					this.QueueUserReportRefresh(modListItemViewModel2);
					flag2 = true;
				}
				this.QueueLatestReleaseUserReportRefresh(modListItemViewModel2);
				flag2 = true;
			}
			if (flag2)
			{
				this._hasFetchedUserReportsThisSession = true;
			}
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0002811C File Offset: 0x0002631C
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		public Task<ModVersionVoteSummary> RefreshUserReportAsync(ModListItemViewModel mod, CancellationToken cancellationToken = default(CancellationToken))
		{
			return this.RunUserReportOperationAsync((CancellationToken ct) => this.RefreshUserReportCoreAsync(mod, false, ct), cancellationToken);
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00028150 File Offset: 0x00026350
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		public Task<ModVersionVoteSummary> SubmitUserReportVoteAsync(ModListItemViewModel mod, ModVersionVoteOption? option, [Nullable(2)] string comment, CancellationToken cancellationToken = default(CancellationToken))
		{
			MainViewModel.<SubmitUserReportVoteAsync>d__347 <SubmitUserReportVoteAsync>d__;
			<SubmitUserReportVoteAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ModVersionVoteSummary>.Create();
			<SubmitUserReportVoteAsync>d__.<>4__this = this;
			<SubmitUserReportVoteAsync>d__.mod = mod;
			<SubmitUserReportVoteAsync>d__.option = option;
			<SubmitUserReportVoteAsync>d__.comment = comment;
			<SubmitUserReportVoteAsync>d__.cancellationToken = cancellationToken;
			<SubmitUserReportVoteAsync>d__.<>1__state = -1;
			<SubmitUserReportVoteAsync>d__.<>t__builder.Start<MainViewModel.<SubmitUserReportVoteAsync>d__347>(ref <SubmitUserReportVoteAsync>d__);
			return <SubmitUserReportVoteAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x000281B4 File Offset: 0x000263B4
		private void RaiseUserReportVoteSubmitted(ModListItemViewModel mod, ModVersionVoteSummary summary)
		{
			int? numericModId = default(int?);
			int num;
			if (int.TryParse(mod.ModId, 7, CultureInfo.InvariantCulture, ref num))
			{
				numericModId = new int?(num);
			}
			EventHandler<MainViewModel.ModUserReportChangedEventArgs> userReportVoteSubmitted = this.UserReportVoteSubmitted;
			if (userReportVoteSubmitted == null)
			{
				return;
			}
			userReportVoteSubmitted.Invoke(this, new MainViewModel.ModUserReportChangedEventArgs(mod.ModId, mod.UserReportModVersion, numericModId, summary));
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0002820C File Offset: 0x0002640C
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		private Task<ModVersionVoteSummary> RefreshUserReportCoreAsync(ModListItemViewModel mod, bool suppressErrors, CancellationToken cancellationToken)
		{
			MainViewModel.<RefreshUserReportCoreAsync>d__349 <RefreshUserReportCoreAsync>d__;
			<RefreshUserReportCoreAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ModVersionVoteSummary>.Create();
			<RefreshUserReportCoreAsync>d__.<>4__this = this;
			<RefreshUserReportCoreAsync>d__.mod = mod;
			<RefreshUserReportCoreAsync>d__.suppressErrors = suppressErrors;
			<RefreshUserReportCoreAsync>d__.cancellationToken = cancellationToken;
			<RefreshUserReportCoreAsync>d__.<>1__state = -1;
			<RefreshUserReportCoreAsync>d__.<>t__builder.Start<MainViewModel.<RefreshUserReportCoreAsync>d__349>(ref <RefreshUserReportCoreAsync>d__);
			return <RefreshUserReportCoreAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00028268 File Offset: 0x00026468
		private void SetTagsColumnVisibility(bool isVisible)
		{
			if (this._isTagsColumnVisible == isVisible)
			{
				return;
			}
			this._isTagsColumnVisible = isVisible;
			if (!isVisible)
			{
				foreach (ModListItemViewModel modListItemViewModel in this._mods)
				{
					modListItemViewModel.ClearDatabaseTags();
				}
				foreach (ModListItemViewModel modListItemViewModel2 in this._searchResults)
				{
					modListItemViewModel2.ClearDatabaseTags();
				}
				return;
			}
			this.ScheduleInstalledTagFilterRefresh();
			if (this._allowModDetailsRefresh && this._modEntriesBySourcePath.Count > 0)
			{
				this.QueueDatabaseInfoRefresh(Enumerable.ToArray<ModEntry>(this._modEntriesBySourcePath.Values), false);
			}
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00028334 File Offset: 0x00026534
		private void SetUserReportsColumnVisibility(bool isVisible)
		{
			if (this._areUserReportsVisible == isVisible)
			{
				return;
			}
			this._areUserReportsVisible = isVisible;
			if (!isVisible)
			{
				this._hasEnabledUserReportFetching = false;
				this._hasFetchedUserReportsThisSession = false;
				return;
			}
			if (this._allowModDetailsRefresh)
			{
				this.EnableUserReportFetching(false);
			}
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00028368 File Offset: 0x00026568
		private void ScheduleInstalledTagFilterRefresh()
		{
			if (!this._isTagsColumnVisible)
			{
				return;
			}
			if (this._isInstalledTagRefreshPending)
			{
				return;
			}
			this._isInstalledTagRefreshPending = true;
			Application application = Application.Current;
			Dispatcher dispatcher = (application != null) ? application.Dispatcher : null;
			if (dispatcher != null)
			{
				dispatcher.BeginInvoke(3, new Action(this.<ScheduleInstalledTagFilterRefresh>g__ExecuteAsync|352_0));
				return;
			}
			this.<ScheduleInstalledTagFilterRefresh>g__ExecuteAsync|352_0();
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x000283BE File Offset: 0x000265BE
		private void ResetInstalledTagFilters(IEnumerable<string> tags)
		{
			if (!this._isTagsColumnVisible)
			{
				return;
			}
			this._tagFilterService.SetInstalledAvailableTags(Enumerable.Concat<string>(tags, this._tagFilterService.GetSelectedInstalledTags()));
			this.ApplyInstalledTagFilters(this._tagFilterService.GetInstalledAvailableTags());
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x000283F8 File Offset: 0x000265F8
		private void ApplyInstalledTagFilters(IReadOnlyList<string> normalized)
		{
			if (!this._isTagsColumnVisible)
			{
				return;
			}
			string[] array = Enumerable.ToArray<string>(Enumerable.OrderBy<string, string>(Enumerable.Distinct<string>(Enumerable.Select<string, string>(Enumerable.Where<string>(normalized, (string tag) => !string.IsNullOrWhiteSpace(tag)), (string tag) => tag.Trim()), StringComparer.OrdinalIgnoreCase), (string tag) => tag, StringComparer.OrdinalIgnoreCase));
			if (Enumerable.SequenceEqual<string>(this._lastInstalledAvailableTags, array, StringComparer.OrdinalIgnoreCase))
			{
				return;
			}
			this._lastInstalledAvailableTags = array;
			this._suppressInstalledTagFilterSelectionChanges = true;
			try
			{
				foreach (TagFilterOptionViewModel tagFilterOptionViewModel in this._installedTagFilters)
				{
					tagFilterOptionViewModel.PropertyChanged -= new PropertyChangedEventHandler(this.OnInstalledTagFilterPropertyChanged);
				}
				this._installedTagFilters.Clear();
				foreach (string text in array)
				{
					bool isSelected = this._tagFilterService.IsInstalledTagSelected(text);
					TagFilterOptionViewModel tagFilterOptionViewModel2 = new TagFilterOptionViewModel(text, isSelected);
					tagFilterOptionViewModel2.PropertyChanged += new PropertyChangedEventHandler(this.OnInstalledTagFilterPropertyChanged);
					this._installedTagFilters.Add(tagFilterOptionViewModel2);
				}
			}
			finally
			{
				this._suppressInstalledTagFilterSelectionChanges = false;
			}
			this.SyncSelectedTagsToService(this._installedTagFilters, true);
			this.UpdateHasSelectedTags();
			this.ModsView.Refresh();
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0002858C File Offset: 0x0002678C
		private void OnInstalledTagFilterPropertyChanged([Nullable(2)] object sender, PropertyChangedEventArgs e)
		{
			if (this._suppressInstalledTagFilterSelectionChanges)
			{
				return;
			}
			if (!string.Equals(e.PropertyName, "IsSelected", 4))
			{
				return;
			}
			if (this.SyncSelectedTagsToService(this._installedTagFilters, true))
			{
				this.UpdateHasSelectedTags();
				this.ModsView.Refresh();
			}
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x000285CC File Offset: 0x000267CC
		private void ClearInstalledTagFilters()
		{
			if (!this.HasSelectedTags)
			{
				return;
			}
			this._suppressInstalledTagFilterSelectionChanges = true;
			try
			{
				foreach (TagFilterOptionViewModel tagFilterOptionViewModel in this._installedTagFilters)
				{
					tagFilterOptionViewModel.IsSelected = false;
				}
			}
			finally
			{
				this._suppressInstalledTagFilterSelectionChanges = false;
			}
			if (this._tagFilterService.SetSelectedInstalledTags(Array.Empty<string>()))
			{
				this.UpdateHasSelectedTags();
				this.ModsView.Refresh();
			}
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00028660 File Offset: 0x00026860
		private void OnModDatabaseTagFilterPropertyChanged([Nullable(2)] object sender, PropertyChangedEventArgs e)
		{
			string.Equals(e.PropertyName, "IsSelected", 4);
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00028674 File Offset: 0x00026874
		private bool SyncSelectedTagsToService(IEnumerable<TagFilterOptionViewModel> filters, bool isInstalled)
		{
			List<string> selectedInstalledTags = Enumerable.ToList<string>(Enumerable.Select<TagFilterOptionViewModel, string>(Enumerable.Where<TagFilterOptionViewModel>(filters, (TagFilterOptionViewModel filter) => filter.IsSelected), (TagFilterOptionViewModel filter) => filter.Name));
			return this._tagFilterService.SetSelectedInstalledTags(selectedInstalledTags);
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x000286DC File Offset: 0x000268DC
		private void UpdateHasSelectedTags()
		{
			this.HasSelectedTags = this._tagFilterService.HasSelectedTags;
			this._clearInstalledTagFiltersCommand.NotifyCanExecuteChanged();
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x000286FC File Offset: 0x000268FC
		private IDisposable BeginBusyScope()
		{
			CancellationTokenSource cancellationTokenSource = null;
			object busyStateLock = this._busyStateLock;
			bool isBusy;
			lock (busyStateLock)
			{
				this._busyOperationCount++;
				isBusy = (this._busyOperationCount > 0);
				if (this._busyReleaseCts != null)
				{
					cancellationTokenSource = this._busyReleaseCts;
					this._busyReleaseCts = null;
				}
			}
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Cancel();
			}
			this.UpdateIsBusy(isBusy);
			return new MainViewModel.BusyScope(this);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x0002877C File Offset: 0x0002697C
		private void EndBusyScope()
		{
			CancellationTokenSource cancellationTokenSource = null;
			CancellationTokenSource cancellationTokenSource2 = null;
			object busyStateLock = this._busyStateLock;
			bool flag2;
			lock (busyStateLock)
			{
				if (this._busyOperationCount > 0)
				{
					this._busyOperationCount--;
				}
				flag2 = (this._busyOperationCount > 0);
				if (!flag2)
				{
					cancellationTokenSource = this._busyReleaseCts;
					cancellationTokenSource2 = new CancellationTokenSource();
					this._busyReleaseCts = cancellationTokenSource2;
				}
			}
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Cancel();
			}
			if (flag2)
			{
				this.UpdateIsBusy(true);
				return;
			}
			this.ScheduleBusyRelease(cancellationTokenSource2);
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00028810 File Offset: 0x00026A10
		[NullableContext(2)]
		private void ScheduleBusyRelease(CancellationTokenSource releaseCts)
		{
			MainViewModel.<>c__DisplayClass362_0 CS$<>8__locals1 = new MainViewModel.<>c__DisplayClass362_0();
			CS$<>8__locals1.releaseCts = releaseCts;
			CS$<>8__locals1.<>4__this = this;
			if (CS$<>8__locals1.releaseCts == null)
			{
				this.UpdateIsBusy(false);
				return;
			}
			Task.Run(delegate()
			{
				MainViewModel.<>c__DisplayClass362_0.<<ScheduleBusyRelease>b__0>d <<ScheduleBusyRelease>b__0>d;
				<<ScheduleBusyRelease>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<ScheduleBusyRelease>b__0>d.<>4__this = CS$<>8__locals1;
				<<ScheduleBusyRelease>b__0>d.<>1__state = -1;
				<<ScheduleBusyRelease>b__0>d.<>t__builder.Start<MainViewModel.<>c__DisplayClass362_0.<<ScheduleBusyRelease>b__0>d>(ref <<ScheduleBusyRelease>b__0>d);
				return <<ScheduleBusyRelease>b__0>d.<>t__builder.Task;
			});
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00028854 File Offset: 0x00026A54
		private void UpdateIsBusy(bool isBusy)
		{
			Application application = Application.Current;
			Dispatcher dispatcher = (application != null) ? application.Dispatcher : null;
			if (dispatcher == null)
			{
				this._hasActiveBusyScope = isBusy;
				this.RecalculateIsBusy();
				return;
			}
			if (dispatcher.CheckAccess())
			{
				this._hasActiveBusyScope = isBusy;
				this.RecalculateIsBusy();
				return;
			}
			dispatcher.BeginInvoke(delegate()
			{
				this._hasActiveBusyScope = isBusy;
				this.RecalculateIsBusy();
			}, Array.Empty<object>());
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x000288D0 File Offset: 0x00026AD0
		private void RecalculateIsBusy()
		{
			bool isBusy = this._hasActiveBusyScope || this._isLoadingMods || this._isLoadingModDetails;
			Application application = Application.Current;
			Dispatcher dispatcher = (application != null) ? application.Dispatcher : null;
			if (dispatcher == null)
			{
				this.IsBusy = isBusy;
				return;
			}
			if (dispatcher.CheckAccess())
			{
				this.IsBusy = isBusy;
				return;
			}
			dispatcher.BeginInvoke(delegate()
			{
				this.IsBusy = isBusy;
			}, Array.Empty<object>());
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00028958 File Offset: 0x00026B58
		private void UpdateIsLoadingModDetails(bool isLoading)
		{
			Application application = Application.Current;
			Dispatcher dispatcher = (application != null) ? application.Dispatcher : null;
			if (dispatcher == null)
			{
				this.IsLoadingModDetails = isLoading;
				return;
			}
			if (dispatcher.CheckAccess())
			{
				this.IsLoadingModDetails = isLoading;
				return;
			}
			dispatcher.BeginInvoke(delegate()
			{
				this.IsLoadingModDetails = isLoading;
			}, Array.Empty<object>());
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x000289C8 File Offset: 0x00026BC8
		private void UpdateModDetailsProgressVisibility()
		{
			this.IsModDetailsProgressVisible = (this._isLoadingModDetails && !this._isFastCheckInProgress);
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x000289E4 File Offset: 0x00026BE4
		private void UpdateLoadedModsStatus()
		{
			if (this.IsModDetailsRefreshPending())
			{
				if (!this._isModDetailsStatusActive)
				{
					this.SetStatus(this.BuildModDetailsLoadingStatusMessage(), false, true);
					return;
				}
			}
			else
			{
				this.SetStatus(this.BuildModDetailsReadyStatusMessage(), false, false);
			}
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x00028A13 File Offset: 0x00026C13
		private bool IsModDetailsRefreshPending()
		{
			return Interlocked.CompareExchange(ref this._pendingModDetailsRefreshCount, 0, 0) > 0;
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00028A28 File Offset: 0x00026C28
		[NullableContext(2)]
		private void OnModDetailsRefreshEnqueued(int count, string statusText = null)
		{
			if (count <= 0)
			{
				return;
			}
			int num = Interlocked.Add(ref this._pendingModDetailsRefreshCount, count);
			if (num <= 0)
			{
				Interlocked.Exchange(ref this._pendingModDetailsRefreshCount, 0);
				return;
			}
			if (num == count)
			{
				this.ResetModDetailsProgress();
			}
			this.EnsureModDetailsBusyScope();
			this.UpdateIsLoadingModDetails(true);
			this.AddModDetailsWork(count, statusText);
			if (num == count || !this._isModDetailsStatusActive)
			{
				this.SetStatus(this.BuildModDetailsLoadingStatusMessage(), false, true);
			}
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00028A94 File Offset: 0x00026C94
		private void OnModDetailsRefreshCompleted(int completedCount = 1)
		{
			if (completedCount <= 0)
			{
				return;
			}
			int num = Interlocked.Add(ref this._pendingModDetailsRefreshCount, -completedCount);
			if (num < 0)
			{
				Interlocked.Exchange(ref this._pendingModDetailsRefreshCount, 0);
				num = 0;
			}
			Interlocked.Add(ref this._modDetailsRefreshCompletedWork, completedCount);
			this.UpdateModDetailsProgress(null);
			if (num <= 0)
			{
				Interlocked.Exchange(ref this._pendingModDetailsRefreshCount, 0);
				this.ReleaseModDetailsBusyScope();
				this.UpdateIsLoadingModDetails(false);
				if (this._isModDetailsStatusActive)
				{
					this.SetStatus(this.BuildModDetailsReadyStatusMessage(), false, false);
				}
				this.ResetModDetailsProgress();
			}
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00028B18 File Offset: 0x00026D18
		private void EnsureModDetailsBusyScope()
		{
			object modDetailsBusyScopeLock = this._modDetailsBusyScopeLock;
			lock (modDetailsBusyScopeLock)
			{
				if (this._modDetailsBusyScope == null)
				{
					this._modDetailsBusyScope = this.BeginBusyScope();
				}
			}
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00028B68 File Offset: 0x00026D68
		private void ReleaseModDetailsBusyScope()
		{
			object modDetailsBusyScopeLock = this._modDetailsBusyScopeLock;
			lock (modDetailsBusyScopeLock)
			{
				IDisposable modDetailsBusyScope = this._modDetailsBusyScope;
				if (modDetailsBusyScope != null)
				{
					modDetailsBusyScope.Dispose();
				}
				this._modDetailsBusyScope = null;
			}
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00028BBC File Offset: 0x00026DBC
		private string BuildModDetailsLoadingStatusMessage()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(36, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Loaded ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.TotalMods);
			defaultInterpolatedStringHandler.AppendLiteral(" mods. Loading mod details...");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00028C00 File Offset: 0x00026E00
		private string BuildModDetailsReadyStatusMessage()
		{
			if (this._hasShownModDetailsLoadingStatus)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(37, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Loaded ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.TotalMods);
				defaultInterpolatedStringHandler.AppendLiteral(" mods. Mod details up to date.");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
			defaultInterpolatedStringHandler2..ctor(13, 1);
			defaultInterpolatedStringHandler2.AppendLiteral("Loaded ");
			defaultInterpolatedStringHandler2.AppendFormatted<int>(this.TotalMods);
			defaultInterpolatedStringHandler2.AppendLiteral(" mods.");
			return defaultInterpolatedStringHandler2.ToStringAndClear();
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00028C82 File Offset: 0x00026E82
		[NullableContext(2)]
		private void AddModDetailsWork(int count, string statusText)
		{
			if (count <= 0)
			{
				return;
			}
			Interlocked.Add(ref this._modDetailsRefreshTotalWork, count);
			this.UpdateModDetailsProgress(statusText);
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00028CA0 File Offset: 0x00026EA0
		[NullableContext(2)]
		private void UpdateModDetailsProgress(string statusText = null)
		{
			if (!string.IsNullOrWhiteSpace(statusText))
			{
				this._modDetailsProgressStage = statusText;
			}
			int num = Interlocked.CompareExchange(ref this._modDetailsRefreshTotalWork, 0, 0);
			if (num <= 0)
			{
				this.ModDetailsProgress = 0.0;
				this.ModDetailsStatusText = string.Empty;
				return;
			}
			int num2 = Interlocked.CompareExchange(ref this._modDetailsRefreshCompletedWork, 0, 0);
			num2 = Math.Clamp(num2, 0, num);
			this.ModDetailsProgress = (double)num2 / (double)num * 100.0;
			string text = string.IsNullOrWhiteSpace(this._modDetailsProgressStage) ? this.BuildModDetailsLoadingStatusMessage() : this._modDetailsProgressStage;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(4, 3);
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
			defaultInterpolatedStringHandler.AppendLiteral("/");
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			this.ModDetailsStatusText = defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00028D84 File Offset: 0x00026F84
		private void ResetModDetailsProgress()
		{
			Interlocked.Exchange(ref this._modDetailsRefreshCompletedWork, 0);
			Interlocked.Exchange(ref this._modDetailsRefreshTotalWork, 0);
			this._modDetailsProgressStage = string.Empty;
			this.ModDetailsProgress = 0.0;
			this.ModDetailsStatusText = string.Empty;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00028DD0 File Offset: 0x00026FD0
		private Task UpdateSearchResultsAsync(IReadOnlyList<ModListItemViewModel> items, CancellationToken cancellationToken)
		{
			return MainViewModel.InvokeOnDispatcherAsync(delegate()
			{
				this._searchResults.Clear();
				foreach (ModListItemViewModel modListItemViewModel in items)
				{
					this._searchResults.Add(modListItemViewModel);
				}
				this.SelectedMod = null;
			}, cancellationToken, 9);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00028DF8 File Offset: 0x00026FF8
		private Task LoadModDatabaseLogosAsync(IReadOnlyList<ModListItemViewModel> viewModels, CancellationToken cancellationToken)
		{
			MainViewModel.<LoadModDatabaseLogosAsync>d__379 <LoadModDatabaseLogosAsync>d__;
			<LoadModDatabaseLogosAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadModDatabaseLogosAsync>d__.viewModels = viewModels;
			<LoadModDatabaseLogosAsync>d__.cancellationToken = cancellationToken;
			<LoadModDatabaseLogosAsync>d__.<>1__state = -1;
			<LoadModDatabaseLogosAsync>d__.<>t__builder.Start<MainViewModel.<LoadModDatabaseLogosAsync>d__379>(ref <LoadModDatabaseLogosAsync>d__);
			return <LoadModDatabaseLogosAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00028E43 File Offset: 0x00027043
		private Task<HashSet<string>> GetInstalledModIdsAsync(CancellationToken cancellationToken)
		{
			return MainViewModel.InvokeOnDispatcherAsync<HashSet<string>>(delegate()
			{
				HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				foreach (ModListItemViewModel modListItemViewModel in this._mods)
				{
					if (!string.IsNullOrWhiteSpace(modListItemViewModel.ModId))
					{
						hashSet.Add(modListItemViewModel.ModId);
					}
				}
				return hashSet;
			}, cancellationToken, 9);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00028E5C File Offset: 0x0002705C
		private static bool IsResultInstalled(ModDatabaseSearchResult result, HashSet<string> installedModIds)
		{
			if (installedModIds.Contains(result.ModId))
			{
				return true;
			}
			foreach (string text in result.AlternateIds)
			{
				if (!string.IsNullOrWhiteSpace(text) && installedModIds.Contains(text))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00028ECC File Offset: 0x000270CC
		private static Task InvokeOnDispatcherAsync(Action action, CancellationToken cancellationToken, DispatcherPriority priority = 9)
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
			return dispatcher.InvokeAsync(action, priority, cancellationToken).Task;
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00028F2C File Offset: 0x0002712C
		private static Task<T> InvokeOnDispatcherAsync<[Nullable(2)] T>(Func<T> function, CancellationToken cancellationToken, DispatcherPriority priority = 9)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<T>(cancellationToken);
			}
			Application application = Application.Current;
			Dispatcher dispatcher = (application != null) ? application.Dispatcher : null;
			if (dispatcher == null)
			{
				return Task.FromResult<T>(function.Invoke());
			}
			if (dispatcher.CheckAccess())
			{
				return Task.FromResult<T>(function.Invoke());
			}
			return dispatcher.InvokeAsync<T>(function, priority, cancellationToken).Task;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00028F8C File Offset: 0x0002718C
		private static ModEntry CreateSearchResultEntry(ModDatabaseSearchResult result)
		{
			string[] array;
			if (!string.IsNullOrWhiteSpace(result.Author))
			{
				(array = new string[1])[0] = result.Author;
			}
			else
			{
				array = Array.Empty<string>();
			}
			string[] authors = array;
			string description = MainViewModel.BuildSearchResultDescription(result);
			string text = MainViewModel.BuildModDatabasePageUrl(result);
			ModDatabaseInfo modDatabaseInfo;
			if ((modDatabaseInfo = result.DetailedInfo) == null)
			{
				ModDatabaseInfo modDatabaseInfo2 = new ModDatabaseInfo();
				modDatabaseInfo2.Tags = result.Tags;
				modDatabaseInfo2.AssetId = result.AssetId;
				modDatabaseInfo2.ModPageUrl = text;
				modDatabaseInfo2.Downloads = new int?(result.Downloads);
				modDatabaseInfo2.Comments = new int?(result.Comments);
				modDatabaseInfo2.Follows = new int?(result.Follows);
				modDatabaseInfo2.TrendingPoints = new int?(result.TrendingPoints);
				modDatabaseInfo2.LogoUrl = result.LogoUrl;
				modDatabaseInfo2.LogoUrlSource = result.LogoUrlSource;
				modDatabaseInfo2.LastReleasedUtc = result.LastReleasedUtc;
				modDatabaseInfo = modDatabaseInfo2;
				modDatabaseInfo2.Side = result.Side;
			}
			ModDatabaseInfo databaseInfo = modDatabaseInfo;
			return new ModEntry
			{
				ModId = result.ModId,
				Name = result.Name,
				ManifestName = result.Name,
				Description = description,
				Authors = authors,
				Website = text,
				SourceKind = ModSourceKind.SourceCode,
				SourcePath = string.Empty,
				Side = result.Side,
				ModDatabaseSearchScore = new double?(result.Score),
				DatabaseInfo = databaseInfo
			};
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x000290E8 File Offset: 0x000272E8
		private ModListItemViewModel CreateSearchResultViewModel(ModEntry entry, bool isInstalled)
		{
			bool isActive = false;
			string location = "Mod Database";
			Func<ModListItemViewModel, bool, Task<ActivationResult>> activationHandler;
			if ((activationHandler = MainViewModel.<>O.<0>__RejectActivationChangeAsync) == null)
			{
				activationHandler = (MainViewModel.<>O.<0>__RejectActivationChangeAsync = new Func<ModListItemViewModel, bool, Task<ActivationResult>>(MainViewModel.RejectActivationChangeAsync));
			}
			return new ModListItemViewModel(entry, isActive, location, activationHandler, this.EffectiveGameVersion, isInstalled, null, () => this._configuration.RequireExactVsVersionMatch, this._allowModDetailsRefresh, null, null);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00029138 File Offset: 0x00027338
		[return: Nullable(2)]
		private static string BuildSearchResultDescription(ModDatabaseSearchResult result)
		{
			if (!string.IsNullOrWhiteSpace(result.Summary))
			{
				return result.Summary.Trim();
			}
			return null;
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00029154 File Offset: 0x00027354
		[return: Nullable(2)]
		private static string BuildModDatabasePageUrl(ModDatabaseSearchResult result)
		{
			if (!string.IsNullOrWhiteSpace(result.AssetId))
			{
				return "https://mods.vintagestory.at/show/mod/" + result.AssetId;
			}
			if (string.IsNullOrWhiteSpace(result.UrlAlias))
			{
				return null;
			}
			string text = result.UrlAlias.TrimStart('/');
			if (!string.IsNullOrWhiteSpace(text))
			{
				return "https://mods.vintagestory.at/" + text;
			}
			return null;
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x000291B1 File Offset: 0x000273B1
		private static Task<ActivationResult> RejectActivationChangeAsync(ModListItemViewModel mod, bool isActive)
		{
			return Task.FromResult<ActivationResult>(new ActivationResult(false, "Install this mod locally to manage its activation state."));
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x000291C4 File Offset: 0x000273C4
		[NullableContext(2)]
		private bool FilterMod(object item)
		{
			ModListItemViewModel modListItemViewModel = item as ModListItemViewModel;
			return modListItemViewModel != null && this._tagFilterService.PassesInstalledTagFilter(modListItemViewModel.DatabaseTags) && (this._searchTokens.Length == 0 || modListItemViewModel.MatchesSearchTokens(this._searchTokens));
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0002920C File Offset: 0x0002740C
		[NullableContext(2)]
		private bool FilterModlist(object item)
		{
			if (string.IsNullOrWhiteSpace(this.ModlistSearchText))
			{
				return true;
			}
			LocalModlistListEntry localModlistListEntry = item as LocalModlistListEntry;
			bool result;
			if (localModlistListEntry == null)
			{
				CloudModlistListEntry cloudModlistListEntry = item as CloudModlistListEntry;
				result = (cloudModlistListEntry == null || cloudModlistListEntry.DisplayName.Contains(this.ModlistSearchText, 5) || (!string.IsNullOrWhiteSpace(cloudModlistListEntry.Description) && cloudModlistListEntry.Description.Contains(this.ModlistSearchText, 5)));
			}
			else
			{
				result = (localModlistListEntry.DisplayName.Contains(this.ModlistSearchText, 5) || (!string.IsNullOrWhiteSpace(localModlistListEntry.Description) && localModlistListEntry.Description.Contains(this.ModlistSearchText, 5)));
			}
			return result;
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x000292B9 File Offset: 0x000274B9
		private static string[] CreateSearchTokens(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return Array.Empty<string>();
			}
			return value.Split(new char[]
			{
				' ',
				'\t',
				'\r',
				'\n'
			}, 3);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x000292E4 File Offset: 0x000274E4
		private string GetDisplayPath([Nullable(2)] string fullPath)
		{
			if (string.IsNullOrWhiteSpace(fullPath))
			{
				return string.Empty;
			}
			string text;
			try
			{
				text = Path.GetFullPath(fullPath);
			}
			catch (Exception)
			{
				return fullPath;
			}
			List<string> list;
			if ((list = this._cachedBasePaths) == null)
			{
				list = (this._cachedBasePaths = this.GetBasePathsList());
			}
			foreach (string text2 in list)
			{
				try
				{
					string relativePath = Path.GetRelativePath(text2, text);
					if (!relativePath.StartsWith("..", 4) && relativePath.Length < text.Length)
					{
						text = relativePath;
					}
				}
				catch (Exception)
				{
				}
			}
			return text.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x000293C0 File Offset: 0x000275C0
		private List<string> GetBasePathsList()
		{
			MainViewModel.<>c__DisplayClass393_0 CS$<>8__locals1;
			CS$<>8__locals1.set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			MainViewModel.<GetBasePathsList>g__TryAdd|393_0(this._settingsStore.DataDirectory, ref CS$<>8__locals1);
			foreach (string candidate in this._settingsStore.SearchBaseCandidates)
			{
				MainViewModel.<GetBasePathsList>g__TryAdd|393_0(candidate, ref CS$<>8__locals1);
			}
			MainViewModel.<GetBasePathsList>g__TryAdd|393_0(Directory.GetCurrentDirectory(), ref CS$<>8__locals1);
			return Enumerable.ToList<string>(CS$<>8__locals1.set);
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0002944C File Offset: 0x0002764C
		private IEnumerable<string> EnumerateBasePaths()
		{
			List<string> result;
			if ((result = this._cachedBasePaths) == null)
			{
				result = (this._cachedBasePaths = this.GetBasePathsList());
			}
			return result;
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00029472 File Offset: 0x00027672
		private static IEnumerable<SortOption> CreateSortOptions()
		{
			return new MainViewModel.<CreateSortOptions>d__395(-2);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0002947C File Offset: 0x0002767C
		private void ReapplyActiveSortIfNeeded()
		{
			SortOption selectedSortOption = this.SelectedSortOption;
			IReadOnlyList<ValueTuple<string, ListSortDirection>> readOnlyList = (selectedSortOption != null) ? selectedSortOption.SortDescriptions : null;
			if (readOnlyList == null || readOnlyList.Count <= 0)
			{
				return;
			}
			if (!MainViewModel.IsActiveSortProperty(readOnlyList[0].Item1))
			{
				return;
			}
			this.SelectedSortOption.Apply(this.ModsView);
			this.ModsView.Refresh();
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x000294D9 File Offset: 0x000276D9
		[NullableContext(2)]
		private static bool IsActiveSortProperty(string propertyName)
		{
			return !string.IsNullOrWhiteSpace(propertyName) && (string.Equals(propertyName, "IsActive", 5) || string.Equals(propertyName, "ActiveSortOrder", 5));
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00029501 File Offset: 0x00027701
		private void SetStatus(string message, bool isError, bool isModDetailsStatus = false)
		{
			StatusLogService.AppendStatus(message, isError);
			this.StatusMessage = message;
			this.IsErrorStatus = isError;
			this._isModDetailsStatusActive = isModDetailsStatus;
			this._hasShownModDetailsLoadingStatus = isModDetailsStatus;
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00029528 File Offset: 0x00027728
		[return: Nullable(new byte[]
		{
			1,
			1,
			2
		})]
		private Dictionary<string, ModEntry> LoadChangedModEntries(IReadOnlyCollection<string> paths, [Nullable(new byte[]
		{
			2,
			1,
			1
		})] IReadOnlyDictionary<string, ModEntry> existingEntries)
		{
			Dictionary<string, ModEntry> dictionary = new Dictionary<string, ModEntry>(StringComparer.OrdinalIgnoreCase);
			foreach (string text in paths)
			{
				ModEntry modEntry = this._discoveryService.LoadModFromPath(text);
				if (modEntry != null)
				{
					MainViewModel.ResetCalculatedModState(modEntry);
					ModEntry source;
					if (existingEntries != null && existingEntries.TryGetValue(text, ref source))
					{
						MainViewModel.CopyTransientModState(source, modEntry);
					}
				}
				dictionary[text] = modEntry;
			}
			return dictionary;
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x000295AC File Offset: 0x000277AC
		private static void ResetCalculatedModState(ModEntry entry)
		{
			entry.LoadError = null;
			entry.DependencyHasErrors = false;
			entry.MissingDependencies = Array.Empty<ModDependencyInfo>();
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x000295C8 File Offset: 0x000277C8
		private static void CopyTransientModState(ModEntry source, ModEntry target)
		{
			if (source == null || target == null)
			{
				return;
			}
			bool flag = string.Equals(source.ModId, target.ModId, 5);
			bool flag2 = string.Equals(source.Version, target.Version, 5) || (string.IsNullOrWhiteSpace(source.Version) && string.IsNullOrWhiteSpace(target.Version));
			if (!flag || !flag2)
			{
				return;
			}
			if (target.DatabaseInfo == null && source.DatabaseInfo != null)
			{
				target.DatabaseInfo = source.DatabaseInfo;
			}
			if (source.ModDatabaseSearchScore != null)
			{
				target.ModDatabaseSearchScore = source.ModDatabaseSearchScore;
			}
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00029660 File Offset: 0x00027860
		[NullableContext(2)]
		private void ApplyPartialUpdates([Nullable(new byte[]
		{
			1,
			1,
			2
		})] IReadOnlyDictionary<string, ModEntry> changes, string previousSelection, [Nullable(new byte[]
		{
			2,
			1
		})] IReadOnlyCollection<ModEntry> statusChanges = null)
		{
			bool flag = statusChanges != null && statusChanges.Count > 0;
			if (changes.Count == 0 && !flag)
			{
				this.SetStatus("Mods are up to date.", false, false);
				ModListItemViewModel selectedMod;
				if (!string.IsNullOrWhiteSpace(previousSelection) && this._modViewModelsBySourcePath.TryGetValue(previousSelection, ref selectedMod))
				{
					this.SelectedMod = selectedMod;
				}
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			using (this._mods.SuspendNotifications())
			{
				foreach (KeyValuePair<string, ModEntry> keyValuePair in changes)
				{
					string key = keyValuePair.Key;
					ModEntry value = keyValuePair.Value;
					if (value == null)
					{
						ModListItemViewModel modListItemViewModel;
						if (this._modViewModelsBySourcePath.TryGetValue(key, ref modListItemViewModel))
						{
							this._mods.Remove(modListItemViewModel);
							this._modViewModelsBySourcePath.Remove(key);
							num3++;
							if (this.SelectedMod == modListItemViewModel)
							{
								this.SelectedMod = null;
							}
						}
						this._modEntriesBySourcePath.Remove(key);
					}
					else
					{
						ModListItemViewModel modListItemViewModel2 = this.CreateModViewModel(value);
						ModListItemViewModel modListItemViewModel3;
						if (this._modViewModelsBySourcePath.TryGetValue(key, ref modListItemViewModel3))
						{
							int num4 = this._mods.IndexOf(modListItemViewModel3);
							if (num4 >= 0)
							{
								this._mods[num4] = modListItemViewModel2;
							}
							else
							{
								this._mods.Add(modListItemViewModel2);
							}
							this._modViewModelsBySourcePath[key] = modListItemViewModel2;
							this._modEntriesBySourcePath[key] = value;
							num2++;
							if (this.SelectedMod == modListItemViewModel3)
							{
								this.SelectedMod = modListItemViewModel2;
							}
						}
						else
						{
							this._mods.Add(modListItemViewModel2);
							this._modViewModelsBySourcePath[key] = modListItemViewModel2;
							this._modEntriesBySourcePath[key] = value;
							num++;
						}
					}
				}
			}
			if (statusChanges != null && statusChanges.Count > 0)
			{
				foreach (ModEntry modEntry in statusChanges)
				{
					ModListItemViewModel modListItemViewModel4;
					if (modEntry != null && !string.IsNullOrWhiteSpace(modEntry.SourcePath) && !changes.ContainsKey(modEntry.SourcePath) && this._modViewModelsBySourcePath.TryGetValue(modEntry.SourcePath, ref modListItemViewModel4))
					{
						modListItemViewModel4.UpdateLoadError(modEntry.LoadError);
						modListItemViewModel4.UpdateDependencyIssues(modEntry.DependencyHasErrors, modEntry.MissingDependencies ?? Array.Empty<ModDependencyInfo>());
					}
				}
			}
			ModListItemViewModel selectedMod2;
			if (!string.IsNullOrWhiteSpace(previousSelection) && this._modViewModelsBySourcePath.TryGetValue(previousSelection, ref selectedMod2))
			{
				this.SelectedMod = selectedMod2;
			}
			else if (this.SelectedMod != null && !this._mods.Contains(this.SelectedMod))
			{
				this.SelectedMod = null;
			}
			int num5 = num + num2 + num3;
			if (num5 == 0)
			{
				this.SetStatus(flag ? "Updated mod statuses." : "Mods are up to date.", false, false);
				return;
			}
			List<string> list = new List<string>();
			if (num > 0)
			{
				List<string> list2 = list;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(6, 1);
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral(" added");
				list2.Add(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			if (num2 > 0)
			{
				List<string> list3 = list;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(8, 1);
				defaultInterpolatedStringHandler2.AppendFormatted<int>(num2);
				defaultInterpolatedStringHandler2.AppendLiteral(" updated");
				list3.Add(defaultInterpolatedStringHandler2.ToStringAndClear());
			}
			if (num3 > 0)
			{
				List<string> list4 = list;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3;
				defaultInterpolatedStringHandler3..ctor(8, 1);
				defaultInterpolatedStringHandler3.AppendFormatted<int>(num3);
				defaultInterpolatedStringHandler3.AppendLiteral(" removed");
				list4.Add(defaultInterpolatedStringHandler3.ToStringAndClear());
			}
			string text;
			if (list.Count != 0)
			{
				text = string.Join(", ", list);
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4;
				defaultInterpolatedStringHandler4..ctor(8, 1);
				defaultInterpolatedStringHandler4.AppendFormatted<int>(num5);
				defaultInterpolatedStringHandler4.AppendLiteral(" changed");
				text = defaultInterpolatedStringHandler4.ToStringAndClear();
			}
			string text2 = text;
			this.SetStatus("Applied changes to mods (" + text2 + ").", false, false);
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x00029A60 File Offset: 0x00027C60
		private void PruneDependencyWarningOverridesToCurrentMods()
		{
			this.PruneDependencyWarningOverrides(this._modEntriesBySourcePath.Values);
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x00029A74 File Offset: 0x00027C74
		private void PruneDependencyWarningOverrides(IEnumerable<ModEntry> entries)
		{
			this._configuration.PruneDependencyWarningOverrides(Enumerable.Select<ModEntry, ValueTuple<string, string>>(Enumerable.Where<ModEntry>(entries, (ModEntry entry) => entry != null && !string.IsNullOrWhiteSpace(entry.ModId)), (ModEntry entry) => new ValueTuple<string, string>(entry.ModId, entry.Version)));
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00029AD8 File Offset: 0x00027CD8
		private void QueueDatabaseInfoRefresh(IEnumerable<ModEntry> entries, bool forceRefresh = false)
		{
			if (entries == null)
			{
				return;
			}
			if (!this._allowModDetailsRefresh && !forceRefresh)
			{
				return;
			}
			ModEntry[] pending = Enumerable.ToArray<ModEntry>(Enumerable.Where<ModEntry>(entries, (ModEntry entry) => entry != null && !string.IsNullOrWhiteSpace(entry.ModId) && (forceRefresh || this.NeedsDatabaseRefresh(entry))));
			if (pending.Length == 0)
			{
				return;
			}
			this.OnModDetailsRefreshEnqueued(pending.Length, null);
			object databaseRefreshLock = this._databaseRefreshLock;
			CancellationToken refreshToken;
			lock (databaseRefreshLock)
			{
				CancellationTokenSource databaseRefreshCts = this._databaseRefreshCts;
				if (databaseRefreshCts != null)
				{
					databaseRefreshCts.Cancel();
				}
				CancellationTokenSource databaseRefreshCts2 = this._databaseRefreshCts;
				if (databaseRefreshCts2 != null)
				{
					databaseRefreshCts2.Dispose();
				}
				this._databaseRefreshCts = new CancellationTokenSource();
				refreshToken = this._databaseRefreshCts.Token;
			}
			this._databaseRefreshTask = Task.Run(() => this.RefreshDatabaseInfoBatchAsync(pending, refreshToken), refreshToken);
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00029BCC File Offset: 0x00027DCC
		private Task RefreshDatabaseInfoBatchAsync(ModEntry[] pending, CancellationToken cancellationToken)
		{
			MainViewModel.<RefreshDatabaseInfoBatchAsync>d__406 <RefreshDatabaseInfoBatchAsync>d__;
			<RefreshDatabaseInfoBatchAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RefreshDatabaseInfoBatchAsync>d__.<>4__this = this;
			<RefreshDatabaseInfoBatchAsync>d__.pending = pending;
			<RefreshDatabaseInfoBatchAsync>d__.cancellationToken = cancellationToken;
			<RefreshDatabaseInfoBatchAsync>d__.<>1__state = -1;
			<RefreshDatabaseInfoBatchAsync>d__.<>t__builder.Start<MainViewModel.<RefreshDatabaseInfoBatchAsync>d__406>(ref <RefreshDatabaseInfoBatchAsync>d__);
			return <RefreshDatabaseInfoBatchAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00029C20 File Offset: 0x00027E20
		private bool NeedsDatabaseRefresh(ModEntry entry)
		{
			string text;
			return entry != null && ((this._isTagsColumnVisible && MainViewModel.TryGetTagSuppressionKey(entry, out text) && text != null && this._suppressedTagEntries.Contains(text)) || entry.DatabaseInfo == null || entry.DatabaseInfo.IsOfflineOnly);
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x00029C6C File Offset: 0x00027E6C
		[NullableContext(2)]
		private bool ShouldSkipOnlineDatabaseRefresh(ModDatabaseInfo cachedInfo)
		{
			return !this._isTagsColumnVisible && !this._areUserReportsVisible && cachedInfo != null && !cachedInfo.IsOfflineOnly;
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00029C90 File Offset: 0x00027E90
		private static bool TryGetTagSuppressionKey(ModEntry entry, [Nullable(2)] out string key)
		{
			key = null;
			if (entry == null)
			{
				return false;
			}
			if (!string.IsNullOrWhiteSpace(entry.SourcePath))
			{
				key = entry.SourcePath;
				return true;
			}
			if (!string.IsNullOrWhiteSpace(entry.ModId))
			{
				key = entry.ModId;
				return true;
			}
			return false;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x00029CCC File Offset: 0x00027ECC
		private Task RefreshDatabaseInfoProgressivelyAsync(ModEntry[] entries, CancellationToken cancellationToken)
		{
			MainViewModel.<RefreshDatabaseInfoProgressivelyAsync>d__410 <RefreshDatabaseInfoProgressivelyAsync>d__;
			<RefreshDatabaseInfoProgressivelyAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RefreshDatabaseInfoProgressivelyAsync>d__.<>4__this = this;
			<RefreshDatabaseInfoProgressivelyAsync>d__.entries = entries;
			<RefreshDatabaseInfoProgressivelyAsync>d__.cancellationToken = cancellationToken;
			<RefreshDatabaseInfoProgressivelyAsync>d__.<>1__state = -1;
			<RefreshDatabaseInfoProgressivelyAsync>d__.<>t__builder.Start<MainViewModel.<RefreshDatabaseInfoProgressivelyAsync>d__410>(ref <RefreshDatabaseInfoProgressivelyAsync>d__);
			return <RefreshDatabaseInfoProgressivelyAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00029D20 File Offset: 0x00027F20
		private Task RefreshDatabaseInfoAsync(ModEntry entry, SemaphoreSlim limiter, CancellationToken cancellationToken)
		{
			MainViewModel.<RefreshDatabaseInfoAsync>d__411 <RefreshDatabaseInfoAsync>d__;
			<RefreshDatabaseInfoAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RefreshDatabaseInfoAsync>d__.<>4__this = this;
			<RefreshDatabaseInfoAsync>d__.entry = entry;
			<RefreshDatabaseInfoAsync>d__.limiter = limiter;
			<RefreshDatabaseInfoAsync>d__.cancellationToken = cancellationToken;
			<RefreshDatabaseInfoAsync>d__.<>1__state = -1;
			<RefreshDatabaseInfoAsync>d__.<>t__builder.Start<MainViewModel.<RefreshDatabaseInfoAsync>d__411>(ref <RefreshDatabaseInfoAsync>d__);
			return <RefreshDatabaseInfoAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00029D7C File Offset: 0x00027F7C
		private Task PopulateOfflineDatabaseInfoAsync(IEnumerable<ModEntry> entries, CancellationToken cancellationToken)
		{
			MainViewModel.<PopulateOfflineDatabaseInfoAsync>d__412 <PopulateOfflineDatabaseInfoAsync>d__;
			<PopulateOfflineDatabaseInfoAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<PopulateOfflineDatabaseInfoAsync>d__.<>4__this = this;
			<PopulateOfflineDatabaseInfoAsync>d__.entries = entries;
			<PopulateOfflineDatabaseInfoAsync>d__.cancellationToken = cancellationToken;
			<PopulateOfflineDatabaseInfoAsync>d__.<>1__state = -1;
			<PopulateOfflineDatabaseInfoAsync>d__.<>t__builder.Start<MainViewModel.<PopulateOfflineDatabaseInfoAsync>d__412>(ref <PopulateOfflineDatabaseInfoAsync>d__);
			return <PopulateOfflineDatabaseInfoAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00029DD0 File Offset: 0x00027FD0
		private Task PopulateOfflineInfoForEntryAsync(ModEntry entry)
		{
			MainViewModel.<PopulateOfflineInfoForEntryAsync>d__413 <PopulateOfflineInfoForEntryAsync>d__;
			<PopulateOfflineInfoForEntryAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<PopulateOfflineInfoForEntryAsync>d__.<>4__this = this;
			<PopulateOfflineInfoForEntryAsync>d__.entry = entry;
			<PopulateOfflineInfoForEntryAsync>d__.<>1__state = -1;
			<PopulateOfflineInfoForEntryAsync>d__.<>t__builder.Start<MainViewModel.<PopulateOfflineInfoForEntryAsync>d__413>(ref <PopulateOfflineInfoForEntryAsync>d__);
			return <PopulateOfflineInfoForEntryAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00029E1C File Offset: 0x0002801C
		private Task ApplyDatabaseInfoAsync(ModEntry entry, ModDatabaseInfo info, bool loadLogoImmediately = true)
		{
			MainViewModel.<ApplyDatabaseInfoAsync>d__414 <ApplyDatabaseInfoAsync>d__;
			<ApplyDatabaseInfoAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ApplyDatabaseInfoAsync>d__.<>4__this = this;
			<ApplyDatabaseInfoAsync>d__.entry = entry;
			<ApplyDatabaseInfoAsync>d__.info = info;
			<ApplyDatabaseInfoAsync>d__.loadLogoImmediately = loadLogoImmediately;
			<ApplyDatabaseInfoAsync>d__.<>1__state = -1;
			<ApplyDatabaseInfoAsync>d__.<>t__builder.Start<MainViewModel.<ApplyDatabaseInfoAsync>d__414>(ref <ApplyDatabaseInfoAsync>d__);
			return <ApplyDatabaseInfoAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00029E78 File Offset: 0x00028078
		private bool ShouldUseBatchedUpdates()
		{
			object databaseInfoBatchLock = this._databaseInfoBatchLock;
			bool result;
			lock (databaseInfoBatchLock)
			{
				result = (this._pendingDatabaseInfoUpdates.Count > 0);
			}
			return result;
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00029EC4 File Offset: 0x000280C4
		private void QueueDatabaseInfoUpdate(ModEntry entry, ModDatabaseInfo info, bool loadLogoImmediately)
		{
			object databaseInfoBatchLock = this._databaseInfoBatchLock;
			lock (databaseInfoBatchLock)
			{
				this._pendingDatabaseInfoUpdates.Add(new ValueTuple<ModEntry, ModDatabaseInfo, bool>(entry, info, loadLogoImmediately));
				if (this._pendingDatabaseInfoUpdates.Count >= 20)
				{
					Timer databaseInfoBatchTimer = this._databaseInfoBatchTimer;
					if (databaseInfoBatchTimer != null)
					{
						databaseInfoBatchTimer.Dispose();
					}
					this._databaseInfoBatchTimer = null;
					this.FlushDatabaseInfoBatchLocked();
				}
				else
				{
					Timer databaseInfoBatchTimer2 = this._databaseInfoBatchTimer;
					if (databaseInfoBatchTimer2 != null)
					{
						databaseInfoBatchTimer2.Dispose();
					}
					this._databaseInfoBatchTimer = new Timer(delegate(object _)
					{
						object databaseInfoBatchLock2 = this._databaseInfoBatchLock;
						lock (databaseInfoBatchLock2)
						{
							if (this._databaseInfoBatchTimer != null)
							{
								this.FlushDatabaseInfoBatchLocked();
							}
						}
					}, null, 50, -1);
				}
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00029F6C File Offset: 0x0002816C
		private void FlushDatabaseInfoBatch()
		{
			object databaseInfoBatchLock = this._databaseInfoBatchLock;
			lock (databaseInfoBatchLock)
			{
				this.FlushDatabaseInfoBatchLocked();
			}
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x00029FAC File Offset: 0x000281AC
		private void FlushDatabaseInfoBatchLocked()
		{
			if (this._pendingDatabaseInfoUpdates.Count == 0)
			{
				return;
			}
			List<ValueTuple<ModEntry, ModDatabaseInfo, bool>> batch = new List<ValueTuple<ModEntry, ModDatabaseInfo, bool>>(this._pendingDatabaseInfoUpdates);
			this._pendingDatabaseInfoUpdates.Clear();
			Timer databaseInfoBatchTimer = this._databaseInfoBatchTimer;
			if (databaseInfoBatchTimer != null)
			{
				databaseInfoBatchTimer.Dispose();
			}
			this._databaseInfoBatchTimer = null;
			this.ApplyDatabaseInfoBatchAsync(batch);
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0002A000 File Offset: 0x00028200
		private Task ApplyDatabaseInfoBatchAsync([TupleElementNames(new string[]
		{
			"entry",
			"info",
			"loadLogoImmediately"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})] List<ValueTuple<ModEntry, ModDatabaseInfo, bool>> batch)
		{
			MainViewModel.<ApplyDatabaseInfoBatchAsync>d__419 <ApplyDatabaseInfoBatchAsync>d__;
			<ApplyDatabaseInfoBatchAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ApplyDatabaseInfoBatchAsync>d__.<>4__this = this;
			<ApplyDatabaseInfoBatchAsync>d__.batch = batch;
			<ApplyDatabaseInfoBatchAsync>d__.<>1__state = -1;
			<ApplyDatabaseInfoBatchAsync>d__.<>t__builder.Start<MainViewModel.<ApplyDatabaseInfoBatchAsync>d__419>(ref <ApplyDatabaseInfoBatchAsync>d__);
			return <ApplyDatabaseInfoBatchAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0002A04C File Offset: 0x0002824C
		private Task ApplyDatabaseInfoImmediateAsync(ModEntry entry, ModDatabaseInfo info, bool loadLogoImmediately)
		{
			MainViewModel.<ApplyDatabaseInfoImmediateAsync>d__420 <ApplyDatabaseInfoImmediateAsync>d__;
			<ApplyDatabaseInfoImmediateAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ApplyDatabaseInfoImmediateAsync>d__.<>4__this = this;
			<ApplyDatabaseInfoImmediateAsync>d__.entry = entry;
			<ApplyDatabaseInfoImmediateAsync>d__.info = info;
			<ApplyDatabaseInfoImmediateAsync>d__.loadLogoImmediately = loadLogoImmediately;
			<ApplyDatabaseInfoImmediateAsync>d__.<>1__state = -1;
			<ApplyDatabaseInfoImmediateAsync>d__.<>t__builder.Start<MainViewModel.<ApplyDatabaseInfoImmediateAsync>d__420>(ref <ApplyDatabaseInfoImmediateAsync>d__);
			return <ApplyDatabaseInfoImmediateAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0002A0A8 File Offset: 0x000282A8
		private ModDatabaseInfo PrepareDatabaseInfoForVisibility(ModEntry entry, ModDatabaseInfo info)
		{
			if (!this._isTagsColumnVisible)
			{
				string text;
				if (MainViewModel.TryGetTagSuppressionKey(entry, out text) && text != null)
				{
					this._suppressedTagEntries.Add(text);
				}
				return MainViewModel.CreateInfoWithoutTags(info);
			}
			string text2;
			if (MainViewModel.TryGetTagSuppressionKey(entry, out text2) && text2 != null)
			{
				this._suppressedTagEntries.Remove(text2);
			}
			return info;
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0002A0FC File Offset: 0x000282FC
		private static ModDatabaseInfo CreateInfoWithoutTags(ModDatabaseInfo source)
		{
			return new ModDatabaseInfo
			{
				Tags = Array.Empty<string>(),
				CachedTagsVersion = null,
				AssetId = source.AssetId,
				ModPageUrl = source.ModPageUrl,
				LatestCompatibleVersion = source.LatestCompatibleVersion,
				LatestVersion = source.LatestVersion,
				RequiredGameVersions = source.RequiredGameVersions,
				Downloads = source.Downloads,
				Comments = source.Comments,
				Follows = source.Follows,
				TrendingPoints = source.TrendingPoints,
				LogoUrl = source.LogoUrl,
				LogoUrlSource = source.LogoUrlSource,
				DownloadsLastThirtyDays = source.DownloadsLastThirtyDays,
				DownloadsLastTenDays = source.DownloadsLastTenDays,
				LastReleasedUtc = source.LastReleasedUtc,
				CreatedUtc = source.CreatedUtc,
				LatestRelease = source.LatestRelease,
				LatestCompatibleRelease = source.LatestCompatibleRelease,
				Releases = source.Releases,
				IsOfflineOnly = source.IsOfflineOnly,
				Side = source.Side
			};
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0002A210 File Offset: 0x00028410
		[return: Nullable(2)]
		private ModDatabaseInfo CreateOfflineDatabaseInfo(ModEntry entry)
		{
			if (entry == null)
			{
				return null;
			}
			IReadOnlyList<ModDependencyInfo> readOnlyList = entry.Dependencies ?? Array.Empty<ModDependencyInfo>();
			IReadOnlyList<string> installedRequiredGameVersions = MainViewModel.ExtractRequiredGameVersions(readOnlyList);
			IReadOnlyList<ModReleaseInfo> readOnlyList2 = this.CreateOfflineReleases(entry, installedRequiredGameVersions, readOnlyList);
			IReadOnlyList<string> requiredGameVersions = MainViewModel.AggregateRequiredGameVersions(installedRequiredGameVersions, readOnlyList2);
			ModReleaseInfo modReleaseInfo = (readOnlyList2.Count > 0) ? readOnlyList2[0] : null;
			ModReleaseInfo modReleaseInfo2 = Enumerable.FirstOrDefault<ModReleaseInfo>(readOnlyList2, (ModReleaseInfo release) => release.IsCompatibleWithInstalledGame);
			DateTime? lastReleasedUtc = MainViewModel.DetermineOfflineLastUpdatedUtc(entry, readOnlyList2);
			string latestVersion = ((modReleaseInfo != null) ? modReleaseInfo.Version : null) ?? entry.Version;
			string latestCompatibleVersion = ((modReleaseInfo2 != null) ? modReleaseInfo2.Version : null) ?? entry.Version;
			return new ModDatabaseInfo
			{
				RequiredGameVersions = requiredGameVersions,
				LatestVersion = latestVersion,
				LatestCompatibleVersion = latestCompatibleVersion,
				LatestRelease = modReleaseInfo,
				LatestCompatibleRelease = modReleaseInfo2,
				Releases = readOnlyList2,
				LastReleasedUtc = lastReleasedUtc,
				IsOfflineOnly = true,
				Side = entry.Side
			};
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x0002A314 File Offset: 0x00028514
		[NullableContext(2)]
		private static ModDatabaseInfo MergeOfflineAndCachedInfo(ModDatabaseInfo offlineInfo, ModDatabaseInfo cachedInfo)
		{
			if (offlineInfo == null)
			{
				return cachedInfo;
			}
			if (cachedInfo == null)
			{
				return offlineInfo;
			}
			IReadOnlyList<ModReleaseInfo> readOnlyList = MainViewModel.MergeReleases(offlineInfo.Releases, cachedInfo.Releases);
			ModReleaseInfo modReleaseInfo = offlineInfo.LatestRelease ?? cachedInfo.LatestRelease;
			if (modReleaseInfo == null && readOnlyList != null && readOnlyList.Count > 0)
			{
				modReleaseInfo = readOnlyList[0];
			}
			ModReleaseInfo modReleaseInfo2 = offlineInfo.LatestCompatibleRelease ?? cachedInfo.LatestCompatibleRelease;
			if (modReleaseInfo2 == null && readOnlyList != null && readOnlyList.Count > 0)
			{
				modReleaseInfo2 = Enumerable.FirstOrDefault<ModReleaseInfo>(readOnlyList, (ModReleaseInfo release) => release != null && release.IsCompatibleWithInstalledGame);
			}
			IReadOnlyList<string> requiredGameVersions = offlineInfo.RequiredGameVersions;
			IReadOnlyList<string> requiredGameVersions2 = (requiredGameVersions != null && requiredGameVersions.Count > 0) ? offlineInfo.RequiredGameVersions : cachedInfo.RequiredGameVersions;
			ModDatabaseInfo modDatabaseInfo = new ModDatabaseInfo();
			IReadOnlyList<string> tags;
			if ((tags = cachedInfo.Tags) == null)
			{
				tags = (offlineInfo.Tags ?? Array.Empty<string>());
			}
			modDatabaseInfo.Tags = tags;
			modDatabaseInfo.CachedTagsVersion = (cachedInfo.CachedTagsVersion ?? offlineInfo.CachedTagsVersion);
			modDatabaseInfo.AssetId = (cachedInfo.AssetId ?? offlineInfo.AssetId);
			modDatabaseInfo.ModPageUrl = (cachedInfo.ModPageUrl ?? offlineInfo.ModPageUrl);
			modDatabaseInfo.LatestCompatibleVersion = (offlineInfo.LatestCompatibleVersion ?? cachedInfo.LatestCompatibleVersion);
			modDatabaseInfo.LatestVersion = (offlineInfo.LatestVersion ?? cachedInfo.LatestVersion);
			modDatabaseInfo.RequiredGameVersions = requiredGameVersions2;
			int? num = cachedInfo.Downloads;
			modDatabaseInfo.Downloads = ((num != null) ? num : offlineInfo.Downloads);
			num = cachedInfo.Comments;
			modDatabaseInfo.Comments = ((num != null) ? num : offlineInfo.Comments);
			num = cachedInfo.Follows;
			modDatabaseInfo.Follows = ((num != null) ? num : offlineInfo.Follows);
			num = cachedInfo.TrendingPoints;
			modDatabaseInfo.TrendingPoints = ((num != null) ? num : offlineInfo.TrendingPoints);
			modDatabaseInfo.LogoUrl = (cachedInfo.LogoUrl ?? offlineInfo.LogoUrl);
			modDatabaseInfo.LogoUrlSource = (cachedInfo.LogoUrlSource ?? offlineInfo.LogoUrlSource);
			num = cachedInfo.DownloadsLastThirtyDays;
			modDatabaseInfo.DownloadsLastThirtyDays = ((num != null) ? num : offlineInfo.DownloadsLastThirtyDays);
			DateTime? dateTime = offlineInfo.LastReleasedUtc;
			modDatabaseInfo.LastReleasedUtc = ((dateTime != null) ? dateTime : cachedInfo.LastReleasedUtc);
			dateTime = cachedInfo.CreatedUtc;
			modDatabaseInfo.CreatedUtc = ((dateTime != null) ? dateTime : offlineInfo.CreatedUtc);
			modDatabaseInfo.LatestRelease = modReleaseInfo;
			modDatabaseInfo.LatestCompatibleRelease = modReleaseInfo2;
			modDatabaseInfo.Releases = readOnlyList;
			modDatabaseInfo.IsOfflineOnly = (offlineInfo.IsOfflineOnly && (cachedInfo == null || cachedInfo.IsOfflineOnly));
			modDatabaseInfo.Side = (((cachedInfo != null) ? cachedInfo.Side : null) ?? offlineInfo.Side);
			return modDatabaseInfo;
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x0002A5CC File Offset: 0x000287CC
		private static IReadOnlyList<ModReleaseInfo> MergeReleases([Nullable(new byte[]
		{
			2,
			1
		})] IReadOnlyList<ModReleaseInfo> offlineReleases, [Nullable(new byte[]
		{
			2,
			1
		})] IReadOnlyList<ModReleaseInfo> cachedReleases)
		{
			if (offlineReleases == null || offlineReleases.Count <= 0)
			{
				if (cachedReleases == null || cachedReleases.Count <= 0)
				{
					return Array.Empty<ModReleaseInfo>();
				}
				return cachedReleases;
			}
			else
			{
				if (cachedReleases == null || cachedReleases.Count <= 0)
				{
					return offlineReleases;
				}
				Dictionary<string, ModReleaseInfo> dictionary = new Dictionary<string, ModReleaseInfo>(StringComparer.OrdinalIgnoreCase);
				foreach (ModReleaseInfo modReleaseInfo in cachedReleases)
				{
					if (modReleaseInfo != null && !string.IsNullOrWhiteSpace(modReleaseInfo.Version) && !dictionary.ContainsKey(modReleaseInfo.Version))
					{
						dictionary[modReleaseInfo.Version] = modReleaseInfo;
					}
				}
				foreach (ModReleaseInfo modReleaseInfo2 in offlineReleases)
				{
					if (modReleaseInfo2 != null && !string.IsNullOrWhiteSpace(modReleaseInfo2.Version))
					{
						dictionary[modReleaseInfo2.Version] = modReleaseInfo2;
					}
				}
				if (dictionary.Count == 0)
				{
					return Array.Empty<ModReleaseInfo>();
				}
				List<ModReleaseInfo> list = Enumerable.ToList<ModReleaseInfo>(dictionary.Values);
				Comparison<ModReleaseInfo> comparison;
				if ((comparison = MainViewModel.<>O.<1>__CompareOfflineReleases) == null)
				{
					comparison = (MainViewModel.<>O.<1>__CompareOfflineReleases = new Comparison<ModReleaseInfo>(MainViewModel.CompareOfflineReleases));
				}
				list.Sort(comparison);
				return list;
			}
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0002A700 File Offset: 0x00028900
		private IReadOnlyList<ModReleaseInfo> CreateOfflineReleases(ModEntry entry, IReadOnlyList<string> installedRequiredGameVersions, IReadOnlyList<ModDependencyInfo> installedDependencies)
		{
			List<ModReleaseInfo> list = new List<ModReleaseInfo>();
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			ModReleaseInfo modReleaseInfo = this.CreateOfflineRelease(entry, installedRequiredGameVersions, installedDependencies);
			if (modReleaseInfo != null)
			{
				list.Add(modReleaseInfo);
				hashSet.Add(modReleaseInfo.Version);
			}
			foreach (ModReleaseInfo modReleaseInfo2 in this.EnumerateCachedModReleases(entry.ModId))
			{
				if (hashSet.Add(modReleaseInfo2.Version))
				{
					list.Add(modReleaseInfo2);
				}
			}
			if (list.Count == 0)
			{
				return Array.Empty<ModReleaseInfo>();
			}
			List<ModReleaseInfo> list2 = list;
			Comparison<ModReleaseInfo> comparison;
			if ((comparison = MainViewModel.<>O.<1>__CompareOfflineReleases) == null)
			{
				Comparison<ModReleaseInfo> comparison2 = MainViewModel.<>O.<1>__CompareOfflineReleases = new Comparison<ModReleaseInfo>(MainViewModel.CompareOfflineReleases);
				comparison = comparison2;
			}
			list2.Sort(comparison);
			return list.AsReadOnly();
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0002A7D0 File Offset: 0x000289D0
		[return: Nullable(2)]
		private ModReleaseInfo CreateOfflineRelease(ModEntry entry, IReadOnlyList<string> requiredGameVersions, IReadOnlyList<ModDependencyInfo> dependencies)
		{
			if (string.IsNullOrWhiteSpace(entry.Version))
			{
				return null;
			}
			Uri downloadUri;
			if (!MainViewModel.TryCreateFileUri(entry.SourcePath, entry.SourceKind, out downloadUri))
			{
				return null;
			}
			DateTime? createdUtc = MainViewModel.TryGetLastWriteTimeUtc(entry.SourcePath, entry.SourceKind);
			return new ModReleaseInfo
			{
				Version = entry.Version,
				NormalizedVersion = VersionStringUtility.Normalize(entry.Version),
				DownloadUri = downloadUri,
				FileName = MainViewModel.TryGetReleaseFileName(entry.SourcePath),
				GameVersionTags = requiredGameVersions,
				IsCompatibleWithInstalledGame = this.DetermineInstalledGameCompatibility(dependencies),
				CreatedUtc = createdUtc
			};
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0002A86A File Offset: 0x00028A6A
		private IEnumerable<ModReleaseInfo> EnumerateCachedModReleases(string modId)
		{
			MainViewModel.<EnumerateCachedModReleases>d__428 <EnumerateCachedModReleases>d__ = new MainViewModel.<EnumerateCachedModReleases>d__428(-2);
			<EnumerateCachedModReleases>d__.<>4__this = this;
			<EnumerateCachedModReleases>d__.<>3__modId = modId;
			return <EnumerateCachedModReleases>d__;
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0002A884 File Offset: 0x00028A84
		[return: Nullable(2)]
		private ModReleaseInfo TryCreateCachedRelease(string archivePath, string expectedModId)
		{
			DateTime lastWriteTimeUtc = DateTime.MinValue;
			long length = 0L;
			try
			{
				FileInfo fileInfo = new FileInfo(archivePath);
				if (fileInfo.Exists)
				{
					lastWriteTimeUtc = fileInfo.LastWriteTimeUtc;
					length = fileInfo.Length;
				}
			}
			catch (Exception)
			{
			}
			string text;
			byte[] array;
			if (ModManifestCacheService.TryGetManifest(archivePath, lastWriteTimeUtc, length, out text, out array))
			{
				try
				{
					using (JsonDocument jsonDocument = JsonDocument.Parse(text, default(JsonDocumentOptions)))
					{
						JsonElement rootElement = jsonDocument.RootElement;
						string actualModId = MainViewModel.GetString(rootElement, "modid") ?? MainViewModel.GetString(rootElement, "modID");
						if (!MainViewModel.IsModIdMatch(expectedModId, actualModId))
						{
							ModManifestCacheService.Invalidate(archivePath);
						}
						else
						{
							string text2 = MainViewModel.GetString(rootElement, "version") ?? MainViewModel.TryResolveVersionFromMap(rootElement);
							if (string.IsNullOrWhiteSpace(text2))
							{
								return null;
							}
							IReadOnlyList<ModDependencyInfo> dependencies = MainViewModel.ParseDependencies(rootElement);
							IReadOnlyList<string> gameVersionTags = MainViewModel.ExtractRequiredGameVersions(dependencies);
							Uri downloadUri;
							if (!MainViewModel.TryCreateFileUri(archivePath, ModSourceKind.ZipArchive, out downloadUri))
							{
								return null;
							}
							DateTime? createdUtc = MainViewModel.TryGetLastWriteTimeUtc(archivePath, ModSourceKind.ZipArchive);
							return new ModReleaseInfo
							{
								Version = text2,
								NormalizedVersion = VersionStringUtility.Normalize(text2),
								DownloadUri = downloadUri,
								FileName = Path.GetFileName(archivePath),
								GameVersionTags = gameVersionTags,
								IsCompatibleWithInstalledGame = this.DetermineInstalledGameCompatibility(dependencies),
								CreatedUtc = createdUtc
							};
						}
					}
				}
				catch (Exception)
				{
					ModManifestCacheService.Invalidate(archivePath);
				}
			}
			ModReleaseInfo result;
			try
			{
				using (ZipArchive zipArchive = ZipFile.OpenRead(archivePath))
				{
					ZipArchiveEntry zipArchiveEntry = MainViewModel.FindArchiveEntry(zipArchive, "modinfo.json");
					if (zipArchiveEntry == null)
					{
						result = null;
					}
					else
					{
						string text3;
						using (Stream stream = zipArchiveEntry.Open())
						{
							using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8, true))
							{
								text3 = streamReader.ReadToEnd();
							}
						}
						using (JsonDocument jsonDocument2 = JsonDocument.Parse(text3, default(JsonDocumentOptions)))
						{
							JsonElement rootElement2 = jsonDocument2.RootElement;
							string text4 = MainViewModel.GetString(rootElement2, "modid") ?? MainViewModel.GetString(rootElement2, "modID");
							if (!MainViewModel.IsModIdMatch(expectedModId, text4))
							{
								result = null;
							}
							else
							{
								string text5 = MainViewModel.GetString(rootElement2, "version") ?? MainViewModel.TryResolveVersionFromMap(rootElement2);
								if (string.IsNullOrWhiteSpace(text5))
								{
									result = null;
								}
								else
								{
									IReadOnlyList<ModDependencyInfo> dependencies2 = MainViewModel.ParseDependencies(rootElement2);
									IReadOnlyList<string> gameVersionTags2 = MainViewModel.ExtractRequiredGameVersions(dependencies2);
									Uri downloadUri2;
									if (!MainViewModel.TryCreateFileUri(archivePath, ModSourceKind.ZipArchive, out downloadUri2))
									{
										result = null;
									}
									else
									{
										DateTime? createdUtc2 = MainViewModel.TryGetLastWriteTimeUtc(archivePath, ModSourceKind.ZipArchive);
										string modId = (!string.IsNullOrWhiteSpace(text4)) ? text4 : ((!string.IsNullOrWhiteSpace(expectedModId)) ? expectedModId : Path.GetFileNameWithoutExtension(archivePath));
										ModManifestCacheService.StoreManifest(archivePath, lastWriteTimeUtc, length, modId, text5, text3, null);
										result = new ModReleaseInfo
										{
											Version = text5,
											NormalizedVersion = VersionStringUtility.Normalize(text5),
											DownloadUri = downloadUri2,
											FileName = Path.GetFileName(archivePath),
											GameVersionTags = gameVersionTags2,
											IsCompatibleWithInstalledGame = this.DetermineInstalledGameCompatibility(dependencies2),
											CreatedUtc = createdUtc2
										};
									}
								}
							}
						}
					}
				}
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0002AC48 File Offset: 0x00028E48
		private static bool IsModIdMatch(string expectedModId, [Nullable(2)] string actualModId)
		{
			return string.IsNullOrWhiteSpace(expectedModId) || string.IsNullOrWhiteSpace(actualModId) || string.Equals(actualModId, expectedModId, 5);
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0002AC64 File Offset: 0x00028E64
		[return: Nullable(2)]
		private static ZipArchiveEntry FindArchiveEntry(ZipArchive archive, string entryName)
		{
			foreach (ZipArchiveEntry zipArchiveEntry in archive.Entries)
			{
				if (string.Equals(zipArchiveEntry.FullName, entryName, 5))
				{
					return zipArchiveEntry;
				}
			}
			return Enumerable.FirstOrDefault<ZipArchiveEntry>(archive.Entries, (ZipArchiveEntry entry) => string.Equals(Path.GetFileName(entry.FullName), entryName, 5));
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x0002ACE8 File Offset: 0x00028EE8
		[return: Nullable(2)]
		private static string GetString(JsonElement element, string propertyName)
		{
			JsonElement jsonElement;
			if (MainViewModel.TryGetProperty(element, propertyName, out jsonElement) && jsonElement.ValueKind == 3)
			{
				return jsonElement.GetString();
			}
			return null;
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0002AD14 File Offset: 0x00028F14
		private static bool TryGetProperty(JsonElement element, string propertyName, out JsonElement value)
		{
			foreach (JsonProperty jsonProperty in element.EnumerateObject())
			{
				if (string.Equals(jsonProperty.Name, propertyName, 5))
				{
					value = jsonProperty.Value;
					return true;
				}
			}
			value = default(JsonElement);
			return false;
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0002AD90 File Offset: 0x00028F90
		[NullableContext(2)]
		private static string TryResolveVersionFromMap(JsonElement root)
		{
			JsonElement jsonElement;
			if (!MainViewModel.TryGetProperty(root, "versionmap", out jsonElement) && !MainViewModel.TryGetProperty(root, "VersionMap", out jsonElement))
			{
				return null;
			}
			if (jsonElement.ValueKind != 1)
			{
				return null;
			}
			string text = null;
			string text2 = null;
			foreach (JsonProperty jsonProperty in jsonElement.EnumerateObject())
			{
				string @string = jsonProperty.Value.GetString();
				if (@string != null)
				{
					text2 = @string;
					if (jsonProperty.Name.Contains("1.21", 5))
					{
						text = @string;
					}
				}
			}
			return text ?? text2;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0002AE48 File Offset: 0x00029048
		private static IReadOnlyList<ModDependencyInfo> ParseDependencies(JsonElement root)
		{
			JsonElement jsonElement;
			if (!MainViewModel.TryGetProperty(root, "dependencies", out jsonElement) || jsonElement.ValueKind != 1)
			{
				return Array.Empty<ModDependencyInfo>();
			}
			List<ModDependencyInfo> list = new List<ModDependencyInfo>();
			foreach (JsonProperty jsonProperty in jsonElement.EnumerateObject())
			{
				string version = (jsonProperty.Value.ValueKind == 3) ? (jsonProperty.Value.GetString() ?? string.Empty) : string.Empty;
				list.Add(new ModDependencyInfo(jsonProperty.Name, version));
			}
			if (list.Count != 0)
			{
				return list;
			}
			return Array.Empty<ModDependencyInfo>();
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0002AF20 File Offset: 0x00029120
		private static IReadOnlyList<string> AggregateRequiredGameVersions(IReadOnlyList<string> installedRequiredGameVersions, IReadOnlyList<ModReleaseInfo> releases)
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (string text in installedRequiredGameVersions)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					hashSet.Add(text);
				}
			}
			foreach (ModReleaseInfo modReleaseInfo in releases)
			{
				if (((modReleaseInfo != null) ? modReleaseInfo.GameVersionTags : null) != null)
				{
					foreach (string text2 in modReleaseInfo.GameVersionTags)
					{
						if (!string.IsNullOrWhiteSpace(text2))
						{
							hashSet.Add(text2);
						}
					}
				}
			}
			if (hashSet.Count != 0)
			{
				return Enumerable.ToArray<string>(hashSet);
			}
			return Array.Empty<string>();
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0002B01C File Offset: 0x0002921C
		private static DateTime? DetermineOfflineLastUpdatedUtc(ModEntry entry, IReadOnlyList<ModReleaseInfo> releases)
		{
			DateTime? dateTime = default(DateTime?);
			DateTime? result;
			foreach (ModReleaseInfo modReleaseInfo in releases)
			{
				result = ((modReleaseInfo != null) ? modReleaseInfo.CreatedUtc : default(DateTime?));
				if (result != null)
				{
					DateTime valueOrDefault = result.GetValueOrDefault();
					if (dateTime == null || valueOrDefault > dateTime)
					{
						dateTime = new DateTime?(valueOrDefault);
					}
				}
			}
			result = dateTime;
			if (result == null)
			{
				return MainViewModel.TryGetLastWriteTimeUtc(entry.SourcePath, entry.SourceKind);
			}
			return result;
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0002B0DC File Offset: 0x000292DC
		private void OnInternetAccessChanged([Nullable(2)] object sender, EventArgs e)
		{
			Application application = Application.Current;
			Dispatcher dispatcher = (application != null) ? application.Dispatcher : null;
			if (dispatcher == null)
			{
				this.RefreshInternetAccessDependentState();
				return;
			}
			if (dispatcher.CheckAccess())
			{
				this.RefreshInternetAccessDependentState();
				return;
			}
			dispatcher.BeginInvoke(9, new Action(this.RefreshInternetAccessDependentState));
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x0002B12C File Offset: 0x0002932C
		private void RefreshInternetAccessDependentState()
		{
			bool isInternetAccessDisabled = InternetAccessManager.IsInternetAccessDisabled;
			foreach (ModListItemViewModel modListItemViewModel in this._mods)
			{
				modListItemViewModel.RefreshInternetAccessDependentState();
				if (isInternetAccessDisabled)
				{
					modListItemViewModel.SetUserReportOffline();
				}
				else
				{
					this.QueueUserReportRefresh(modListItemViewModel);
				}
			}
			foreach (ModListItemViewModel modListItemViewModel2 in this._searchResults)
			{
				modListItemViewModel2.RefreshInternetAccessDependentState();
			}
			this._showDatabaseTabCommand.NotifyCanExecuteChanged();
			this._showModlistTabCommand.NotifyCanExecuteChanged();
			base.OnPropertyChanged("CanAccessCloudModlists");
			if (InternetAccessManager.IsInternetAccessDisabled && this._viewSection == MainViewModel.ViewSection.DatabaseTab)
			{
				this.SetStatus("Enable Internet Access in the File menu to use.", false, false);
				this.SetViewSection(MainViewModel.ViewSection.MainTab);
				return;
			}
			if (InternetAccessManager.IsInternetAccessDisabled && this._viewSection == MainViewModel.ViewSection.ModlistTab)
			{
				this.SetStatus("Enable Internet Access in the File menu to use.", false, false);
				this.SetViewSection(MainViewModel.ViewSection.MainTab);
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0002B234 File Offset: 0x00029434
		[NullableContext(2)]
		private static int CompareOfflineReleases(ModReleaseInfo left, ModReleaseInfo right)
		{
			if (left == right)
			{
				return 0;
			}
			if (left == null)
			{
				return 1;
			}
			if (right == null)
			{
				return -1;
			}
			if (VersionStringUtility.IsCandidateVersionNewer(left.Version, right.Version))
			{
				return -1;
			}
			if (VersionStringUtility.IsCandidateVersionNewer(right.Version, left.Version))
			{
				return 1;
			}
			DateTime dateTime = left.CreatedUtc ?? DateTime.MinValue;
			int num = (right.CreatedUtc ?? DateTime.MinValue).CompareTo(dateTime);
			if (num != 0)
			{
				return num;
			}
			return string.Compare(left.Version, right.Version, 5);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0002B2DC File Offset: 0x000294DC
		private static IReadOnlyList<string> ExtractRequiredGameVersions(IReadOnlyList<ModDependencyInfo> dependencies)
		{
			if (dependencies == null || dependencies.Count == 0)
			{
				return Array.Empty<string>();
			}
			string[] array = Enumerable.ToArray<string>(Enumerable.Distinct<string>(Enumerable.Select<string, string>(Enumerable.Where<string>(Enumerable.Select<ModDependencyInfo, string>(Enumerable.Where<ModDependencyInfo>(dependencies, (ModDependencyInfo dependency) => dependency != null && dependency.IsGameOrCoreDependency), delegate(ModDependencyInfo dependency)
			{
				string version = dependency.Version;
				if (version == null)
				{
					return null;
				}
				return version.Trim();
			}), (string version) => !string.IsNullOrWhiteSpace(version)), (string version) => version), StringComparer.OrdinalIgnoreCase));
			if (array.Length != 0)
			{
				return array;
			}
			return Array.Empty<string>();
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0002B3A8 File Offset: 0x000295A8
		private bool DetermineInstalledGameCompatibility(IReadOnlyList<ModDependencyInfo> dependencies)
		{
			if (dependencies == null || dependencies.Count == 0)
			{
				return true;
			}
			if (string.IsNullOrWhiteSpace(this.EffectiveGameVersion))
			{
				return true;
			}
			foreach (ModDependencyInfo modDependencyInfo in dependencies)
			{
				if (modDependencyInfo != null && modDependencyInfo.IsGameOrCoreDependency)
				{
					if (!VersionStringUtility.SatisfiesMinimumVersion(modDependencyInfo.Version, this.EffectiveGameVersion))
					{
						return false;
					}
					if (this._configuration.RequireExactVsVersionMatch && !VersionStringUtility.MatchesFirstThreeDigits(modDependencyInfo.Version, this.EffectiveGameVersion))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0002B450 File Offset: 0x00029650
		[NullableContext(2)]
		private static bool TryCreateFileUri(string path, ModSourceKind kind, out Uri uri)
		{
			uri = null;
			if (string.IsNullOrWhiteSpace(path))
			{
				return false;
			}
			bool result;
			try
			{
				string text = Path.GetFullPath(path);
				if (kind == ModSourceKind.Folder)
				{
					ReadOnlySpan<char> readOnlySpan = Path.TrimEndingDirectorySeparator(text);
					char directorySeparatorChar = Path.DirectorySeparatorChar;
					text = readOnlySpan + new ReadOnlySpan<char>(ref directorySeparatorChar);
				}
				result = Uri.TryCreate(text, 1, ref uri);
			}
			catch (Exception)
			{
				uri = null;
				result = false;
			}
			return result;
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0002B4B8 File Offset: 0x000296B8
		[NullableContext(2)]
		private static string TryGetReleaseFileName(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return null;
			}
			string result;
			try
			{
				string text = Path.GetFullPath(path);
				if (Directory.Exists(text))
				{
					text = Path.TrimEndingDirectorySeparator(text);
				}
				string fileName = Path.GetFileName(text);
				result = (string.IsNullOrWhiteSpace(fileName) ? null : fileName);
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0002B514 File Offset: 0x00029714
		[NullableContext(2)]
		private static DateTime? TryGetLastWriteTimeUtc(string path, ModSourceKind kind)
		{
			DateTime? dateTime;
			if (string.IsNullOrWhiteSpace(path))
			{
				dateTime = default(DateTime?);
				return dateTime;
			}
			try
			{
				DateTime? dateTime2;
				if (kind != ModSourceKind.Folder)
				{
					if (!File.Exists(path))
					{
						dateTime = default(DateTime?);
						dateTime2 = dateTime;
					}
					else
					{
						dateTime2 = new DateTime?(File.GetLastWriteTimeUtc(path));
					}
				}
				else if (!Directory.Exists(path))
				{
					dateTime = default(DateTime?);
					dateTime2 = dateTime;
				}
				else
				{
					dateTime2 = new DateTime?(Directory.GetLastWriteTimeUtc(path));
				}
				dateTime = dateTime2;
			}
			catch (Exception)
			{
				dateTime = default(DateTime?);
			}
			return dateTime;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0002B598 File Offset: 0x00029798
		private void PerformClientSettingsCleanupIfNeeded()
		{
			if (this._configuration.ClientSettingsCleanupCompleted)
			{
				return;
			}
			try
			{
				this._settingsStore.RemoveInvalidDisabledEntries();
				this._configuration.SetClientSettingsCleanupCompleted();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x0002B5E0 File Offset: 0x000297E0
		private TimeSpan CalculateAdaptiveSearchDebounce()
		{
			int count = this._mods.Count;
			if (count <= 200)
			{
				return MainViewModel.InstalledModsSearchDebounceMin;
			}
			int num = 300;
			double num2 = Math.Min(1.0, (double)(count - 200) / (double)num);
			return TimeSpan.FromMilliseconds(MainViewModel.InstalledModsSearchDebounceMin.TotalMilliseconds + (MainViewModel.InstalledModsSearchDebounceMax.TotalMilliseconds - MainViewModel.InstalledModsSearchDebounceMin.TotalMilliseconds) * num2);
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0002B650 File Offset: 0x00029850
		private void TriggerDebouncedInstalledModsSearch()
		{
			object searchDebounceLock = this._searchDebounceLock;
			lock (searchDebounceLock)
			{
				if (!this._disposed)
				{
					CancellationTokenSource pendingSearchCts = this._pendingSearchCts;
					if (pendingSearchCts != null)
					{
						pendingSearchCts.Cancel();
					}
					CancellationTokenSource pendingSearchCts2 = this._pendingSearchCts;
					if (pendingSearchCts2 != null)
					{
						pendingSearchCts2.Dispose();
					}
					this._pendingSearchCts = new CancellationTokenSource();
					TimeSpan timeSpan = this.CalculateAdaptiveSearchDebounce();
					if (this._searchDebounceTimer == null)
					{
						this._searchDebounceTimer = new Timer(new TimerCallback(this.OnSearchDebounceTimerElapsed), null, timeSpan, Timeout.InfiniteTimeSpan);
					}
					else
					{
						this._searchDebounceTimer.Change(timeSpan, Timeout.InfiniteTimeSpan);
					}
				}
			}
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0002B704 File Offset: 0x00029904
		[NullableContext(2)]
		private void OnSearchDebounceTimerElapsed(object state)
		{
			object searchDebounceLock = this._searchDebounceLock;
			CancellationTokenSource pendingSearchCts;
			lock (searchDebounceLock)
			{
				if (this._disposed)
				{
					return;
				}
				pendingSearchCts = this._pendingSearchCts;
			}
			if (pendingSearchCts == null)
			{
				return;
			}
			this.ExecuteInstalledModsSearch(pendingSearchCts);
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0002B75C File Offset: 0x0002995C
		private void ExecuteInstalledModsSearch(CancellationTokenSource cts)
		{
			if (cts.IsCancellationRequested)
			{
				return;
			}
			object searchDebounceLock = this._searchDebounceLock;
			bool flag2;
			lock (searchDebounceLock)
			{
				if (this._disposed)
				{
					return;
				}
				flag2 = (cts == this._pendingSearchCts);
			}
			if (!flag2)
			{
				return;
			}
			Application application = Application.Current;
			Dispatcher dispatcher = (application != null) ? application.Dispatcher : null;
			if (dispatcher != null)
			{
				dispatcher.BeginInvoke(delegate()
				{
					this.RefreshModsViewIfNotCancelled(cts);
				}, 5, Array.Empty<object>());
				return;
			}
			this.RefreshModsViewIfNotCancelled(cts);
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0002B814 File Offset: 0x00029A14
		private void RefreshModsViewIfNotCancelled(CancellationTokenSource cts)
		{
			if (!cts.IsCancellationRequested)
			{
				this.ModsView.Refresh();
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x0002B829 File Offset: 0x00029A29
		// (set) Token: 0x06000674 RID: 1652 RVA: 0x0002B831 File Offset: 0x00029A31
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string ModlistSearchText
		{
			get
			{
				return this._modlistSearchText;
			}
			[MemberNotNull("_modlistSearchText")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(this._modlistSearchText, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ModlistSearchText);
					this._modlistSearchText = value;
					this.OnModlistSearchTextChanged(value);
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ModlistSearchText);
				}
			}
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0002B86C File Offset: 0x00029A6C
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnModlistSearchTextChanged(string value)
		{
			bool flag = !string.IsNullOrWhiteSpace(value);
			this.LocalModlistsView.Filter = (flag ? new Predicate<object>(this.FilterModlist) : null);
			this.CloudModlistsView.Filter = (flag ? new Predicate<object>(this.FilterModlist) : null);
			this.LocalModlistsView.Refresh();
			this.CloudModlistsView.Refresh();
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0002BA74 File Offset: 0x00029C74
		[CompilerGenerated]
		private void <ScheduleInstalledTagFilterRefresh>g__ExecuteAsync|352_0()
		{
			MainViewModel.<<ScheduleInstalledTagFilterRefresh>g__ExecuteAsync|352_0>d <<ScheduleInstalledTagFilterRefresh>g__ExecuteAsync|352_0>d;
			<<ScheduleInstalledTagFilterRefresh>g__ExecuteAsync|352_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
			<<ScheduleInstalledTagFilterRefresh>g__ExecuteAsync|352_0>d.<>4__this = this;
			<<ScheduleInstalledTagFilterRefresh>g__ExecuteAsync|352_0>d.<>1__state = -1;
			<<ScheduleInstalledTagFilterRefresh>g__ExecuteAsync|352_0>d.<>t__builder.Start<MainViewModel.<<ScheduleInstalledTagFilterRefresh>g__ExecuteAsync|352_0>d>(ref <<ScheduleInstalledTagFilterRefresh>g__ExecuteAsync|352_0>d);
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0002BB50 File Offset: 0x00029D50
		[NullableContext(2)]
		[CompilerGenerated]
		internal static void <GetBasePathsList>g__TryAdd|393_0(string candidate, ref MainViewModel.<>c__DisplayClass393_0 A_1)
		{
			if (string.IsNullOrWhiteSpace(candidate))
			{
				return;
			}
			try
			{
				string fullPath = Path.GetFullPath(candidate);
				A_1.set.Add(fullPath);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x040002E9 RID: 745
		private const string InternetAccessDisabledStatusMessage = "Enable Internet Access in the File menu to use.";

		// Token: 0x040002EA RID: 746
		private const string TagsColumnName = "Tags";

		// Token: 0x040002EB RID: 747
		private const string UserReportsColumnName = "UserReports";

		// Token: 0x040002EC RID: 748
		private static readonly TimeSpan InstalledModsSearchDebounceMin = TimeSpan.FromMilliseconds(100.0);

		// Token: 0x040002ED RID: 749
		private static readonly TimeSpan InstalledModsSearchDebounceMax = TimeSpan.FromMilliseconds(300.0);

		// Token: 0x040002EE RID: 750
		private const int LargeModListThreshold = 200;

		// Token: 0x040002EF RID: 751
		private const int VeryLargeModListThreshold = 500;

		// Token: 0x040002F0 RID: 752
		private static readonly TimeSpan BusyStateReleaseDelay = TimeSpan.FromMilliseconds(600.0);

		// Token: 0x040002F1 RID: 753
		private static readonly TimeSpan FastCheckInterval = TimeSpan.FromMinutes(2.0);

		// Token: 0x040002F2 RID: 754
		private static readonly int MaxConcurrentDatabaseRefreshes = DevConfig.MaxConcurrentDatabaseRefreshes;

		// Token: 0x040002F3 RID: 755
		private static readonly int MaxConcurrentUserReportRefreshes = DevConfig.MaxConcurrentUserReportRefreshes;

		// Token: 0x040002F4 RID: 756
		private static readonly int MaxNewModsRecentMonths = DevConfig.MaxNewModsRecentMonths;

		// Token: 0x040002F5 RID: 757
		private static readonly int InstalledModsIncrementalBatchSize = DevConfig.InstalledModsIncrementalBatchSize;

		// Token: 0x040002F6 RID: 758
		private const int InitialRefreshDelayMs = 500;

		// Token: 0x040002F7 RID: 759
		private const int IncrementalRefreshDelayMs = 300;

		// Token: 0x040002F8 RID: 760
		private readonly object _busyStateLock = new object();

		// Token: 0x040002F9 RID: 761
		private readonly RelayCommand _clearInstalledTagFiltersCommand;

		// Token: 0x040002FA RID: 762
		private readonly RelayCommand _clearSearchCommand;

		// Token: 0x040002FB RID: 763
		private readonly ClientSettingsWatcher _clientSettingsWatcher;

		// Token: 0x040002FC RID: 764
		private readonly ObservableCollection<CloudModlistListEntry> _cloudModlists = new ObservableCollection<CloudModlistListEntry>();

		// Token: 0x040002FD RID: 765
		private readonly ObservableCollection<LocalModlistListEntry> _localModlists = new ObservableCollection<LocalModlistListEntry>();

		// Token: 0x040002FE RID: 766
		private readonly UserConfigurationService _configuration;

		// Token: 0x040002FF RID: 767
		private readonly ModDatabaseService _databaseService;

		// Token: 0x04000300 RID: 768
		private readonly ModDiscoveryService _discoveryService;

		// Token: 0x04000301 RID: 769
		private readonly object _fastCheckTimerLock = new object();

		// Token: 0x04000302 RID: 770
		private readonly object _searchDebounceLock = new object();

		// Token: 0x04000303 RID: 771
		private readonly HashSet<ModListItemViewModel> _installedModSubscriptions = new HashSet<ModListItemViewModel>();

		// Token: 0x04000304 RID: 772
		private readonly TagCacheService _tagCache = new TagCacheService();

		// Token: 0x04000305 RID: 773
		private readonly TagFilterService _tagFilterService;

		// Token: 0x04000306 RID: 774
		private readonly ObservableCollection<TagFilterOptionViewModel> _installedTagFilters = new ObservableCollection<TagFilterOptionViewModel>();

		// Token: 0x04000307 RID: 775
		private string[] _lastInstalledAvailableTags = Array.Empty<string>();

		// Token: 0x04000308 RID: 776
		private readonly Dictionary<string, string> _latestReleaseUserReportEtags = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000309 RID: 777
		private readonly object _modDetailsBusyScopeLock = new object();

		// Token: 0x0400030A RID: 778
		private readonly object _databaseRefreshLock = new object();

		// Token: 0x0400030B RID: 779
		private readonly Dictionary<string, ModEntry> _modEntriesBySourcePath = new Dictionary<string, ModEntry>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400030C RID: 780
		private readonly BatchedObservableCollection<ModListItemViewModel> _mods = new BatchedObservableCollection<ModListItemViewModel>();

		// Token: 0x0400030D RID: 781
		private readonly object _modsStateLock = new object();

		// Token: 0x0400030E RID: 782
		private readonly ModDirectoryWatcher _modsWatcher;

		// Token: 0x0400030F RID: 783
		private readonly Dictionary<string, ModListItemViewModel> _modViewModelsBySourcePath = new Dictionary<string, ModListItemViewModel>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000310 RID: 784
		private readonly ObservableCollection<ModListItemViewModel> _searchResults = new ObservableCollection<ModListItemViewModel>();

		// Token: 0x04000311 RID: 785
		private readonly HashSet<ModListItemViewModel> _searchResultSubscriptions = new HashSet<ModListItemViewModel>();

		// Token: 0x04000312 RID: 786
		private readonly ClientSettingsStore _settingsStore;

		// Token: 0x04000313 RID: 787
		private readonly RelayCommand _showModlistTabCommand;

		// Token: 0x04000314 RID: 788
		private readonly RelayCommand _showMainTabCommand;

		// Token: 0x04000315 RID: 789
		private readonly RelayCommand _showDatabaseTabCommand;

		// Token: 0x04000316 RID: 790
		private readonly ObservableCollection<SortOption> _sortOptions;

		// Token: 0x04000317 RID: 791
		private readonly HashSet<string> _suppressedTagEntries = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000318 RID: 792
		private readonly Dictionary<string, string> _userReportEtags = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000319 RID: 793
		private readonly object _userReportOperationLock = new object();

		// Token: 0x0400031A RID: 794
		private readonly string _voteEtagCachePath;

		// Token: 0x0400031B RID: 795
		private readonly object _voteEtagPersistenceLock = new object();

		// Token: 0x0400031C RID: 796
		private readonly SemaphoreSlim _userReportRefreshLimiter = new SemaphoreSlim(MainViewModel.MaxConcurrentUserReportRefreshes, MainViewModel.MaxConcurrentUserReportRefreshes);

		// Token: 0x0400031D RID: 797
		private readonly ModVersionVoteService _voteService = new ModVersionVoteService();

		// Token: 0x0400031E RID: 798
		private readonly ModLoadingTimingService _timingService = new ModLoadingTimingService();

		// Token: 0x0400031F RID: 799
		private readonly object _databaseInfoBatchLock = new object();

		// Token: 0x04000320 RID: 800
		[TupleElementNames(new string[]
		{
			"entry",
			"info",
			"loadLogoImmediately"
		})]
		[Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		private readonly List<ValueTuple<ModEntry, ModDatabaseInfo, bool>> _pendingDatabaseInfoUpdates = new List<ValueTuple<ModEntry, ModDatabaseInfo, bool>>();

		// Token: 0x04000321 RID: 801
		[Nullable(2)]
		private Timer _databaseInfoBatchTimer;

		// Token: 0x04000322 RID: 802
		private const int DatabaseInfoBatchDelayMs = 50;

		// Token: 0x04000323 RID: 803
		private const int DatabaseInfoBatchSize = 20;

		// Token: 0x04000324 RID: 804
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private List<string> _cachedBasePaths;

		// Token: 0x04000325 RID: 805
		private int _activeMods;

		// Token: 0x04000326 RID: 806
		private int _activeUserReportOperations;

		// Token: 0x04000327 RID: 807
		private bool _allowModDetailsRefresh = true;

		// Token: 0x04000328 RID: 808
		private bool _areUserReportsVisible = true;

		// Token: 0x04000329 RID: 809
		private int _busyOperationCount;

		// Token: 0x0400032A RID: 810
		[Nullable(2)]
		private CancellationTokenSource _busyReleaseCts;

		// Token: 0x0400032B RID: 811
		private bool _isInitialLoad = true;

		// Token: 0x0400032C RID: 812
		private bool _disposed;

		// Token: 0x0400032D RID: 813
		[Nullable(2)]
		private Timer _searchDebounceTimer;

		// Token: 0x0400032E RID: 814
		[Nullable(2)]
		private CancellationTokenSource _pendingSearchCts;

		// Token: 0x0400032F RID: 815
		[Nullable(2)]
		private Timer _fastCheckTimer;

		// Token: 0x04000330 RID: 816
		private bool _hasActiveBusyScope;

		// Token: 0x04000331 RID: 817
		private bool _hasEnabledUserReportFetching;

		// Token: 0x04000332 RID: 818
		private bool _hasFetchedUserReportsThisSession;

		// Token: 0x04000333 RID: 819
		private bool _hasMultipleSelectedMods;

		// Token: 0x04000334 RID: 820
		private volatile bool _hasPendingFastCheck;

		// Token: 0x04000335 RID: 821
		private bool _hasSelectedMods;

		// Token: 0x04000336 RID: 822
		private bool _hasSelectedTags;

		// Token: 0x04000337 RID: 823
		private bool _hasShownModDetailsLoadingStatus;

		// Token: 0x04000338 RID: 824
		private bool _isAutoRefreshDisabled;

		// Token: 0x04000339 RID: 825
		private bool _isBusy;

		// Token: 0x0400033A RID: 826
		private bool _isCompactView;

		// Token: 0x0400033B RID: 827
		private bool _isErrorStatus;

		// Token: 0x0400033C RID: 828
		private bool _isFastCheckInProgress;

		// Token: 0x0400033D RID: 829
		private int _isFastCheckRunning;

		// Token: 0x0400033E RID: 830
		private bool _isInstalledTagRefreshPending;

		// Token: 0x0400033F RID: 831
		private bool _isLoadingModDetails;

		// Token: 0x04000340 RID: 832
		private bool _isLoadingMods;

		// Token: 0x04000341 RID: 833
		private bool _isModDetailsProgressVisible;

		// Token: 0x04000342 RID: 834
		private double _modDetailsProgress;

		// Token: 0x04000343 RID: 835
		private int _modDetailsRefreshCompletedWork;

		// Token: 0x04000344 RID: 836
		private int _modDetailsRefreshTotalWork;

		// Token: 0x04000345 RID: 837
		private string _modDetailsProgressStage = string.Empty;

		// Token: 0x04000346 RID: 838
		private string _modDetailsStatusText = string.Empty;

		// Token: 0x04000347 RID: 839
		private double _loadingProgress;

		// Token: 0x04000348 RID: 840
		private string _loadingStatusText = string.Empty;

		// Token: 0x04000349 RID: 841
		private bool _isModDetailsRefreshForced;

		// Token: 0x0400034A RID: 842
		private bool _isModDetailsStatusActive;

		// Token: 0x0400034B RID: 843
		[Nullable(2)]
		private Task _databaseRefreshTask;

		// Token: 0x0400034C RID: 844
		[Nullable(2)]
		private CancellationTokenSource _databaseRefreshCts;

		// Token: 0x0400034D RID: 845
		private bool _isModInfoExpanded = true;

		// Token: 0x0400034E RID: 846
		private bool _isTagsColumnVisible = true;

		// Token: 0x0400034F RID: 847
		private bool _useModDbDesignView;

		// Token: 0x04000350 RID: 848
		[Nullable(2)]
		private IDisposable _modDetailsBusyScope;

		// Token: 0x04000351 RID: 849
		[Nullable(2)]
		private string _modsStateFingerprint;

		// Token: 0x04000352 RID: 850
		private int _pendingModDetailsRefreshCount;

		// Token: 0x04000353 RID: 851
		private string _searchText = string.Empty;

		// Token: 0x04000354 RID: 852
		private string[] _searchTokens = Array.Empty<string>();

		// Token: 0x04000355 RID: 853
		[ObservableProperty]
		private string _modlistSearchText = string.Empty;

		// Token: 0x04000356 RID: 854
		[Nullable(2)]
		private ModListItemViewModel _selectedMod;

		// Token: 0x04000357 RID: 855
		[Nullable(2)]
		private SortOption _selectedSortOption;

		// Token: 0x04000358 RID: 856
		private string _statusMessage = string.Empty;

		// Token: 0x04000359 RID: 857
		private bool _suppressInstalledTagFilterSelectionChanges;

		// Token: 0x0400035A RID: 858
		private int _totalMods;

		// Token: 0x0400035B RID: 859
		private int _updatableModsCount;

		// Token: 0x0400035C RID: 860
		private MainViewModel.ViewSection _viewSection;

		// Token: 0x020001E6 RID: 486
		[NullableContext(0)]
		private sealed class VoteEtagCacheState
		{
			// Token: 0x17000454 RID: 1108
			// (get) Token: 0x060013E3 RID: 5091 RVA: 0x00072D1F File Offset: 0x00070F1F
			// (set) Token: 0x060013E4 RID: 5092 RVA: 0x00072D27 File Offset: 0x00070F27
			[Nullable(new byte[]
			{
				2,
				1,
				1
			})]
			public Dictionary<string, string> Current { [return: Nullable(new byte[]
			{
				2,
				1,
				1
			})] get; [param: Nullable(new byte[]
			{
				2,
				1,
				1
			})] set; }

			// Token: 0x17000455 RID: 1109
			// (get) Token: 0x060013E5 RID: 5093 RVA: 0x00072D30 File Offset: 0x00070F30
			// (set) Token: 0x060013E6 RID: 5094 RVA: 0x00072D38 File Offset: 0x00070F38
			[Nullable(new byte[]
			{
				2,
				1,
				1
			})]
			public Dictionary<string, string> Latest { [return: Nullable(new byte[]
			{
				2,
				1,
				1
			})] get; [param: Nullable(new byte[]
			{
				2,
				1,
				1
			})] set; }
		}

		// Token: 0x020001E7 RID: 487
		[Nullable(0)]
		private readonly struct LoadingProgressUpdate : IEquatable<MainViewModel.LoadingProgressUpdate>
		{
			// Token: 0x060013E8 RID: 5096 RVA: 0x00072D49 File Offset: 0x00070F49
			public LoadingProgressUpdate(double Progress, string Status)
			{
				this.Progress = Progress;
				this.Status = Status;
			}

			// Token: 0x17000456 RID: 1110
			// (get) Token: 0x060013E9 RID: 5097 RVA: 0x00072D59 File Offset: 0x00070F59
			// (set) Token: 0x060013EA RID: 5098 RVA: 0x00072D61 File Offset: 0x00070F61
			public double Progress { get; set; }

			// Token: 0x17000457 RID: 1111
			// (get) Token: 0x060013EB RID: 5099 RVA: 0x00072D6A File Offset: 0x00070F6A
			// (set) Token: 0x060013EC RID: 5100 RVA: 0x00072D72 File Offset: 0x00070F72
			public string Status { get; set; }

			// Token: 0x060013ED RID: 5101 RVA: 0x00072D7C File Offset: 0x00070F7C
			[NullableContext(0)]
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("LoadingProgressUpdate");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x060013EE RID: 5102 RVA: 0x00072DC8 File Offset: 0x00070FC8
			[NullableContext(0)]
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				builder.Append("Progress = ");
				builder.Append(this.Progress.ToString());
				builder.Append(", Status = ");
				builder.Append(this.Status);
				return true;
			}

			// Token: 0x060013EF RID: 5103 RVA: 0x00072E16 File Offset: 0x00071016
			[CompilerGenerated]
			public static bool operator !=(MainViewModel.LoadingProgressUpdate left, MainViewModel.LoadingProgressUpdate right)
			{
				return !(left == right);
			}

			// Token: 0x060013F0 RID: 5104 RVA: 0x00072E22 File Offset: 0x00071022
			[CompilerGenerated]
			public static bool operator ==(MainViewModel.LoadingProgressUpdate left, MainViewModel.LoadingProgressUpdate right)
			{
				return left.Equals(right);
			}

			// Token: 0x060013F1 RID: 5105 RVA: 0x00072E2C File Offset: 0x0007102C
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return EqualityComparer<double>.Default.GetHashCode(this.<Progress>k__BackingField) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Status>k__BackingField);
			}

			// Token: 0x060013F2 RID: 5106 RVA: 0x00072E55 File Offset: 0x00071055
			[NullableContext(0)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return obj is MainViewModel.LoadingProgressUpdate && this.Equals((MainViewModel.LoadingProgressUpdate)obj);
			}

			// Token: 0x060013F3 RID: 5107 RVA: 0x00072E6D File Offset: 0x0007106D
			[CompilerGenerated]
			public bool Equals(MainViewModel.LoadingProgressUpdate other)
			{
				return EqualityComparer<double>.Default.Equals(this.<Progress>k__BackingField, other.<Progress>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Status>k__BackingField, other.<Status>k__BackingField);
			}

			// Token: 0x060013F4 RID: 5108 RVA: 0x00072E9F File Offset: 0x0007109F
			[CompilerGenerated]
			public void Deconstruct(out double Progress, out string Status)
			{
				Progress = this.Progress;
				Status = this.Status;
			}
		}

		// Token: 0x020001E8 RID: 488
		[Nullable(0)]
		public sealed class ModUserReportChangedEventArgs : EventArgs
		{
			// Token: 0x060013F5 RID: 5109 RVA: 0x00072EB1 File Offset: 0x000710B1
			public ModUserReportChangedEventArgs(string modId, [Nullable(2)] string modVersion, int? numericModId, ModVersionVoteSummary summary)
			{
				this.ModId = modId;
				this.ModVersion = modVersion;
				this.NumericModId = numericModId;
				if (summary == null)
				{
					throw new ArgumentNullException("summary");
				}
				this.Summary = summary;
			}

			// Token: 0x17000458 RID: 1112
			// (get) Token: 0x060013F6 RID: 5110 RVA: 0x00072EE5 File Offset: 0x000710E5
			public string ModId { get; }

			// Token: 0x17000459 RID: 1113
			// (get) Token: 0x060013F7 RID: 5111 RVA: 0x00072EED File Offset: 0x000710ED
			[Nullable(2)]
			public string ModVersion { [NullableContext(2)] get; }

			// Token: 0x1700045A RID: 1114
			// (get) Token: 0x060013F8 RID: 5112 RVA: 0x00072EF5 File Offset: 0x000710F5
			public int? NumericModId { get; }

			// Token: 0x1700045B RID: 1115
			// (get) Token: 0x060013F9 RID: 5113 RVA: 0x00072EFD File Offset: 0x000710FD
			public ModVersionVoteSummary Summary { get; }
		}

		// Token: 0x020001E9 RID: 489
		[NullableContext(0)]
		private enum ViewSection
		{
			// Token: 0x04000C88 RID: 3208
			MainTab,
			// Token: 0x04000C89 RID: 3209
			DatabaseTab,
			// Token: 0x04000C8A RID: 3210
			ModlistTab
		}

		// Token: 0x020001EA RID: 490
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class VoteCheckTarget : IEquatable<MainViewModel.VoteCheckTarget>
		{
			// Token: 0x060013FA RID: 5114 RVA: 0x00072F05 File Offset: 0x00071105
			public VoteCheckTarget([Nullable(1)] ModListItemViewModel Mod, string UserReportVersion, bool HasUserReportSummary, string LatestReleaseVersion, bool HasLatestReleaseSummary)
			{
				this.Mod = Mod;
				this.UserReportVersion = UserReportVersion;
				this.HasUserReportSummary = HasUserReportSummary;
				this.LatestReleaseVersion = LatestReleaseVersion;
				this.HasLatestReleaseSummary = HasLatestReleaseSummary;
				base..ctor();
			}

			// Token: 0x1700045C RID: 1116
			// (get) Token: 0x060013FB RID: 5115 RVA: 0x00072F32 File Offset: 0x00071132
			[Nullable(1)]
			[CompilerGenerated]
			private Type EqualityContract
			{
				[NullableContext(1)]
				[CompilerGenerated]
				get
				{
					return typeof(MainViewModel.VoteCheckTarget);
				}
			}

			// Token: 0x1700045D RID: 1117
			// (get) Token: 0x060013FC RID: 5116 RVA: 0x00072F3E File Offset: 0x0007113E
			// (set) Token: 0x060013FD RID: 5117 RVA: 0x00072F46 File Offset: 0x00071146
			[Nullable(1)]
			public ModListItemViewModel Mod { [NullableContext(1)] get; [NullableContext(1)] set; }

			// Token: 0x1700045E RID: 1118
			// (get) Token: 0x060013FE RID: 5118 RVA: 0x00072F4F File Offset: 0x0007114F
			// (set) Token: 0x060013FF RID: 5119 RVA: 0x00072F57 File Offset: 0x00071157
			public string UserReportVersion { get; set; }

			// Token: 0x1700045F RID: 1119
			// (get) Token: 0x06001400 RID: 5120 RVA: 0x00072F60 File Offset: 0x00071160
			// (set) Token: 0x06001401 RID: 5121 RVA: 0x00072F68 File Offset: 0x00071168
			public bool HasUserReportSummary { get; set; }

			// Token: 0x17000460 RID: 1120
			// (get) Token: 0x06001402 RID: 5122 RVA: 0x00072F71 File Offset: 0x00071171
			// (set) Token: 0x06001403 RID: 5123 RVA: 0x00072F79 File Offset: 0x00071179
			public string LatestReleaseVersion { get; set; }

			// Token: 0x17000461 RID: 1121
			// (get) Token: 0x06001404 RID: 5124 RVA: 0x00072F82 File Offset: 0x00071182
			// (set) Token: 0x06001405 RID: 5125 RVA: 0x00072F8A File Offset: 0x0007118A
			public bool HasLatestReleaseSummary { get; set; }

			// Token: 0x06001406 RID: 5126 RVA: 0x00072F94 File Offset: 0x00071194
			[NullableContext(1)]
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("VoteCheckTarget");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06001407 RID: 5127 RVA: 0x00072FE0 File Offset: 0x000711E0
			[NullableContext(1)]
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Mod = ");
				builder.Append(this.Mod);
				builder.Append(", UserReportVersion = ");
				builder.Append(this.UserReportVersion);
				builder.Append(", HasUserReportSummary = ");
				builder.Append(this.HasUserReportSummary.ToString());
				builder.Append(", LatestReleaseVersion = ");
				builder.Append(this.LatestReleaseVersion);
				builder.Append(", HasLatestReleaseSummary = ");
				builder.Append(this.HasLatestReleaseSummary.ToString());
				return true;
			}

			// Token: 0x06001408 RID: 5128 RVA: 0x0007308C File Offset: 0x0007128C
			[CompilerGenerated]
			public static bool operator !=(MainViewModel.VoteCheckTarget left, MainViewModel.VoteCheckTarget right)
			{
				return !(left == right);
			}

			// Token: 0x06001409 RID: 5129 RVA: 0x00073098 File Offset: 0x00071298
			[CompilerGenerated]
			public static bool operator ==(MainViewModel.VoteCheckTarget left, MainViewModel.VoteCheckTarget right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x0600140A RID: 5130 RVA: 0x000730AC File Offset: 0x000712AC
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<ModListItemViewModel>.Default.GetHashCode(this.<Mod>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<UserReportVersion>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<HasUserReportSummary>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<LatestReleaseVersion>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<HasLatestReleaseSummary>k__BackingField);
			}

			// Token: 0x0600140B RID: 5131 RVA: 0x0007313C File Offset: 0x0007133C
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as MainViewModel.VoteCheckTarget);
			}

			// Token: 0x0600140C RID: 5132 RVA: 0x0007314C File Offset: 0x0007134C
			[CompilerGenerated]
			public bool Equals(MainViewModel.VoteCheckTarget other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<ModListItemViewModel>.Default.Equals(this.<Mod>k__BackingField, other.<Mod>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<UserReportVersion>k__BackingField, other.<UserReportVersion>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<HasUserReportSummary>k__BackingField, other.<HasUserReportSummary>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<LatestReleaseVersion>k__BackingField, other.<LatestReleaseVersion>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<HasLatestReleaseSummary>k__BackingField, other.<HasLatestReleaseSummary>k__BackingField));
			}

			// Token: 0x0600140E RID: 5134 RVA: 0x000731FC File Offset: 0x000713FC
			[CompilerGenerated]
			private VoteCheckTarget([Nullable(1)] MainViewModel.VoteCheckTarget original)
			{
				this.Mod = original.<Mod>k__BackingField;
				this.UserReportVersion = original.<UserReportVersion>k__BackingField;
				this.HasUserReportSummary = original.<HasUserReportSummary>k__BackingField;
				this.LatestReleaseVersion = original.<LatestReleaseVersion>k__BackingField;
				this.HasLatestReleaseSummary = original.<HasLatestReleaseSummary>k__BackingField;
			}

			// Token: 0x0600140F RID: 5135 RVA: 0x0007324B File Offset: 0x0007144B
			[CompilerGenerated]
			public void Deconstruct([Nullable(1)] out ModListItemViewModel Mod, out string UserReportVersion, out bool HasUserReportSummary, out string LatestReleaseVersion, out bool HasLatestReleaseSummary)
			{
				Mod = this.Mod;
				UserReportVersion = this.UserReportVersion;
				HasUserReportSummary = this.HasUserReportSummary;
				LatestReleaseVersion = this.LatestReleaseVersion;
				HasLatestReleaseSummary = this.HasLatestReleaseSummary;
			}
		}

		// Token: 0x020001EB RID: 491
		[Nullable(0)]
		private sealed class UserReportOperationScope : IDisposable
		{
			// Token: 0x06001410 RID: 5136 RVA: 0x00073277 File Offset: 0x00071477
			public UserReportOperationScope(MainViewModel owner, IDisposable busyScope)
			{
				this._owner = owner;
				this._busyScope = busyScope;
			}

			// Token: 0x06001411 RID: 5137 RVA: 0x0007328D File Offset: 0x0007148D
			public void Dispose()
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
				this._busyScope.Dispose();
				this._owner.EndUserReportOperation();
			}

			// Token: 0x04000C90 RID: 3216
			private readonly IDisposable _busyScope;

			// Token: 0x04000C91 RID: 3217
			private readonly MainViewModel _owner;

			// Token: 0x04000C92 RID: 3218
			private bool _disposed;
		}

		// Token: 0x020001EC RID: 492
		[NullableContext(0)]
		private sealed class BusyScope : IDisposable
		{
			// Token: 0x06001412 RID: 5138 RVA: 0x000732B5 File Offset: 0x000714B5
			[NullableContext(1)]
			public BusyScope(MainViewModel owner)
			{
				this._owner = owner;
			}

			// Token: 0x06001413 RID: 5139 RVA: 0x000732C4 File Offset: 0x000714C4
			public void Dispose()
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
				this._owner.EndBusyScope();
			}

			// Token: 0x04000C93 RID: 3219
			[Nullable(1)]
			private readonly MainViewModel _owner;

			// Token: 0x04000C94 RID: 3220
			private bool _disposed;
		}

		// Token: 0x020001EE RID: 494
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000C99 RID: 3225
			[Nullable(new byte[]
			{
				0,
				1,
				1
			})]
			public static Func<ModListItemViewModel, bool, Task<ActivationResult>> <0>__RejectActivationChangeAsync;

			// Token: 0x04000C9A RID: 3226
			[Nullable(0)]
			public static Comparison<ModReleaseInfo> <1>__CompareOfflineReleases;
		}
	}
}
