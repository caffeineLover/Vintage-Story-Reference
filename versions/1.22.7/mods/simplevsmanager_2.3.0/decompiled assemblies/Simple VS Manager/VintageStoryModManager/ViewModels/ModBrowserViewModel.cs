using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using CommunityToolkit.Mvvm.Input;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services;

namespace VintageStoryModManager.ViewModels
{
	// Token: 0x02000043 RID: 67
	[NullableContext(1)]
	[Nullable(0)]
	public class ModBrowserViewModel : ObservableObject
	{
		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x0002BBD8 File Offset: 0x00029DD8
		public bool HasSearchText
		{
			get
			{
				return !string.IsNullOrWhiteSpace(this.TextFilter);
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x0002BBE8 File Offset: 0x00029DE8
		public bool HasScreenshots
		{
			get
			{
				return this.ScreenshotUrls.Count > 0;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x0002BBF8 File Offset: 0x00029DF8
		public bool HasMultipleScreenshots
		{
			get
			{
				return this.ScreenshotUrls.Count > 1;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x0002BC08 File Offset: 0x00029E08
		[Nullable(2)]
		public string CurrentScreenshotUrl
		{
			[NullableContext(2)]
			get
			{
				if (this.CurrentScreenshotIndex < 0 || this.CurrentScreenshotIndex >= this.ScreenshotUrls.Count)
				{
					return null;
				}
				return this.ScreenshotUrls[this.CurrentScreenshotIndex];
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x0002BC3C File Offset: 0x00029E3C
		public string ScreenshotCounterText
		{
			get
			{
				if (!this.HasScreenshots)
				{
					return string.Empty;
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(3, 2);
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.CurrentScreenshotIndex + 1);
				defaultInterpolatedStringHandler.AppendLiteral(" / ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.ScreenshotUrls.Count);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0002BC94 File Offset: 0x00029E94
		private void BuildScreenshotGallery()
		{
			List<string> list = new List<string>();
			DownloadableMod modDetails = this.ModDetails;
			List<DownloadableModScreenshot> list2 = (modDetails != null) ? modDetails.Screenshots : null;
			if (list2 != null)
			{
				foreach (DownloadableModScreenshot downloadableModScreenshot in list2)
				{
					string text = ModBrowserViewModel.ResolveScreenshotUrl(downloadableModScreenshot.MainFile);
					if (!string.IsNullOrWhiteSpace(text))
					{
						list.Add(text);
					}
				}
			}
			if (list.Count == 0)
			{
				DownloadableModOnList selectedMod = this.SelectedMod;
				string text2 = (selectedMod != null) ? selectedMod.LogoUrl : null;
				if (!string.IsNullOrWhiteSpace(text2))
				{
					list.Add(text2);
				}
			}
			this.CurrentScreenshotIndex = 0;
			this.ScreenshotUrls = list;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0002BD4C File Offset: 0x00029F4C
		[NullableContext(2)]
		private static string ResolveScreenshotUrl(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}
			string text = value.Trim();
			if (text.StartsWith("//", 4))
			{
				return "https:" + text;
			}
			if (text.StartsWith("/", 4))
			{
				return "https://mods.vintagestory.at" + text;
			}
			return text;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0002BD9F File Offset: 0x00029F9F
		[RelayCommand]
		private void NextScreenshot()
		{
			if (this.ScreenshotUrls.Count == 0)
			{
				return;
			}
			this.CurrentScreenshotIndex = (this.CurrentScreenshotIndex + 1) % this.ScreenshotUrls.Count;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0002BDC9 File Offset: 0x00029FC9
		[RelayCommand]
		private void PreviousScreenshot()
		{
			if (this.ScreenshotUrls.Count == 0)
			{
				return;
			}
			this.CurrentScreenshotIndex = (this.CurrentScreenshotIndex - 1 + this.ScreenshotUrls.Count) % this.ScreenshotUrls.Count;
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000692 RID: 1682 RVA: 0x0002BDFF File Offset: 0x00029FFF
		private bool ShouldUseCorrectThumbnails
		{
			get
			{
				UserConfigurationService userConfigService = this._userConfigService;
				return userConfigService != null && !userConfigService.UseFasterThumbnails;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000693 RID: 1683 RVA: 0x0002BE16 File Offset: 0x0002A016
		[Nullable(2)]
		private string EffectiveGameVersion
		{
			[NullableContext(2)]
			get
			{
				string installedGameVersion = this._installedGameVersion;
				UserConfigurationService userConfigService = this._userConfigService;
				return TargetGameVersionPreference.ResolveEffectiveGameVersion(installedGameVersion, (userConfigService != null) ? userConfigService.TargetGameVersionOverride : null);
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000694 RID: 1684 RVA: 0x0002BE38 File Offset: 0x0002A038
		[Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		public unsafe static List<KeyValuePair<string, string>> SideOptions
		{
			[return: Nullable(new byte[]
			{
				1,
				0,
				1,
				1
			})]
			get
			{
				int num = 4;
				List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>(num);
				CollectionsMarshal.SetCount<KeyValuePair<string, string>>(list, num);
				Span<KeyValuePair<string, string>> span = CollectionsMarshal.AsSpan<KeyValuePair<string, string>>(list);
				*span[0] = new KeyValuePair<string, string>("any", "Any");
				*span[1] = new KeyValuePair<string, string>("both", "Both");
				*span[2] = new KeyValuePair<string, string>("server", "Server");
				*span[3] = new KeyValuePair<string, string>("client", "Client");
				return list;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000695 RID: 1685 RVA: 0x0002BECC File Offset: 0x0002A0CC
		[Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		public unsafe static List<KeyValuePair<string, string>> InstalledFilterOptions
		{
			[return: Nullable(new byte[]
			{
				1,
				0,
				1,
				1
			})]
			get
			{
				int num = 3;
				List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>(num);
				CollectionsMarshal.SetCount<KeyValuePair<string, string>>(list, num);
				Span<KeyValuePair<string, string>> span = CollectionsMarshal.AsSpan<KeyValuePair<string, string>>(list);
				*span[0] = new KeyValuePair<string, string>("all", "All");
				*span[1] = new KeyValuePair<string, string>("installed", "Installed");
				*span[2] = new KeyValuePair<string, string>("not-installed", "Not Installed");
				return list;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x0002BF44 File Offset: 0x0002A144
		public unsafe static List<OrderByOption> OrderByOptions
		{
			get
			{
				int num = 6;
				List<OrderByOption> list = new List<OrderByOption>(num);
				CollectionsMarshal.SetCount<OrderByOption>(list, num);
				Span<OrderByOption> span = CollectionsMarshal.AsSpan<OrderByOption>(list);
				*span[0] = new OrderByOption("trendingpoints", "Trending", "IconFire");
				*span[1] = new OrderByOption("downloads", "Downloads", "IconDownload");
				*span[2] = new OrderByOption("comments", "Comments", "IconMessage");
				*span[3] = new OrderByOption("lastreleased", "Updated", "IconHistory");
				*span[4] = new OrderByOption("asset.created", "Created", "IconCalendar");
				*span[5] = new OrderByOption("follows", "Follows", "IconHeartSolid");
				return list;
			}
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0002C018 File Offset: 0x0002A218
		public ModBrowserViewModel(IModApiService modApiService, [Nullable(2)] UserConfigurationService userConfigService = null)
		{
			this._modApiService = modApiService;
			this._userConfigService = userConfigService;
			this._voteService = new ModVersionVoteService();
			this.RefreshInstalledGameVersion();
			this._isInitializing = true;
			this.SelectedVersions.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnFilterCollectionChanged);
			this.SelectedTags.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnFilterCollectionChanged);
			if (this._userConfigService != null)
			{
				this.OrderBy = this._userConfigService.ModBrowserOrderBy;
				this.OrderByDirection = this._userConfigService.ModBrowserOrderByDirection;
				this.SelectedSide = this._userConfigService.ModBrowserSelectedSide;
				this.SelectedInstalledFilter = this._userConfigService.ModBrowserSelectedInstalledFilter;
				this.OnlyFavorites = this._userConfigService.ModBrowserOnlyFavorites;
				this.FavoriteMods = new ObservableCollection<int>(this._userConfigService.ModBrowserFavoriteModIds);
				this.UseRelevantSearchResults = this._userConfigService.ModBrowserRelevantSearch;
			}
			this.FavoriteMods.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnFavoriteModsCollectionChanged);
			this._isInitializing = false;
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0002C213 File Offset: 0x0002A413
		public void SetInstallModCallback(Func<DownloadableMod, Task> callback)
		{
			this._installModCallback = callback;
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x0002C21C File Offset: 0x0002A41C
		public void RefreshInstalledGameVersion()
		{
			UserConfigurationService userConfigService = this._userConfigService;
			this._installedGameVersion = VintageStoryVersionLocator.GetInstalledVersion((userConfigService != null) ? userConfigService.GameDirectory : null);
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0002C23C File Offset: 0x0002A43C
		public void SetTabVisibility(bool isVisible)
		{
			this._isTabVisible = isVisible;
			if (!isVisible)
			{
				CancellationTokenSource searchCts = this._searchCts;
				if (searchCts != null)
				{
					searchCts.Cancel();
				}
				CancellationTokenSource pendingLoadMoreCts = this._pendingLoadMoreCts;
				if (pendingLoadMoreCts != null)
				{
					pendingLoadMoreCts.Cancel();
				}
				this._pendingLoadMoreCts = null;
				if (this.IsModDetailsOpen || this.SelectedMod != null || this.ModDetails != null || this._modDetailsLoadCancellation != null)
				{
					this.CloseModDetails();
				}
			}
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0002C2A4 File Offset: 0x0002A4A4
		private void OnFilterCollectionChanged([Nullable(2)] object sender, NotifyCollectionChangedEventArgs e)
		{
			ModBrowserViewModel.<OnFilterCollectionChanged>d__82 <OnFilterCollectionChanged>d__;
			<OnFilterCollectionChanged>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<OnFilterCollectionChanged>d__.<>4__this = this;
			<OnFilterCollectionChanged>d__.sender = sender;
			<OnFilterCollectionChanged>d__.<>1__state = -1;
			<OnFilterCollectionChanged>d__.<>t__builder.Start<ModBrowserViewModel.<OnFilterCollectionChanged>d__82>(ref <OnFilterCollectionChanged>d__);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0002C2E4 File Offset: 0x0002A4E4
		private void OnFavoriteModsCollectionChanged([Nullable(2)] object sender, NotifyCollectionChangedEventArgs e)
		{
			this.DownloadAllFavoritesCommand.NotifyCanExecuteChanged();
			if (this._isInitializing)
			{
				return;
			}
			UserConfigurationService userConfigService = this._userConfigService;
			if (userConfigService != null)
			{
				userConfigService.SetModBrowserFavoriteModIds(this.FavoriteMods);
			}
			if (this.OnlyFavorites)
			{
				this.IsSearching = true;
				this.SearchModsAsync();
			}
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0002C334 File Offset: 0x0002A534
		private List<DownloadableModOnList> GetPrefetchMods()
		{
			int num = Math.Min(this.VisibleModsCount + 10, this.ModsList.Count);
			return Enumerable.ToList<DownloadableModOnList>(Enumerable.Take<DownloadableModOnList>(this.ModsList, num));
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0002C36C File Offset: 0x0002A56C
		public bool IsModFavorite(int modId)
		{
			return this.FavoriteMods.Contains(modId);
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0002C37A File Offset: 0x0002A57A
		public bool IsModInstalled(int modId)
		{
			return this.IsModInstalledById(modId.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0002C390 File Offset: 0x0002A590
		public void UpdateInstalledMods(IEnumerable<string> modIds, [Nullable(2)] IEnumerable<int> numericModIds = null)
		{
			string[] array = Enumerable.ToArray<string>(Enumerable.OrderBy<string, string>(this._normalizedInstalledModIds, (string id) => id, StringComparer.OrdinalIgnoreCase));
			this._normalizedInstalledModIds.Clear();
			foreach (string modId in modIds)
			{
				this.AddInstalledModId(modId);
			}
			this.InstalledMods.Clear();
			if (numericModIds != null)
			{
				foreach (int num in Enumerable.Distinct<int>(numericModIds))
				{
					this.InstalledMods.Add(num);
				}
			}
			this.RefreshInstalledFlags();
			if (!Enumerable.SequenceEqual<string>(array, Enumerable.OrderBy<string, string>(this._normalizedInstalledModIds, (string id) => id, StringComparer.OrdinalIgnoreCase), StringComparer.OrdinalIgnoreCase))
			{
				this._installedModsVersion++;
			}
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0002C4BC File Offset: 0x0002A6BC
		public void AddInstalledMod(string modId, int? numericModId = null)
		{
			this.AddInstalledModId(modId);
			if (numericModId != null && !this.InstalledMods.Contains(numericModId.Value))
			{
				this.InstalledMods.Add(numericModId.Value);
			}
			this.RefreshInstalledFlags();
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0002C4FC File Offset: 0x0002A6FC
		[NullableContext(2)]
		public void InvalidateUserReport(int modId, ModVersionVoteSummary summary = null)
		{
			HashSet<int> userReportsLoaded = this._userReportsLoaded;
			lock (userReportsLoaded)
			{
				this._userReportsLoaded.Remove(modId);
			}
			DownloadableModOnList downloadableModOnList = Enumerable.FirstOrDefault<DownloadableModOnList>(this.ModsList, (DownloadableModOnList m) => m.ModId == modId);
			if (downloadableModOnList == null)
			{
				return;
			}
			if (summary != null)
			{
				this.ApplyUserReportSummary(downloadableModOnList, summary);
				userReportsLoaded = this._userReportsLoaded;
				lock (userReportsLoaded)
				{
					this._userReportsLoaded.Add(modId);
				}
				return;
			}
			this.PopulateUserReportsAsync(new DownloadableModOnList[]
			{
				downloadableModOnList
			}, CancellationToken.None);
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0002C5D0 File Offset: 0x0002A7D0
		public void InvalidateAllVisibleUserReports()
		{
			HashSet<int> userReportsLoaded = this._userReportsLoaded;
			List<DownloadableModOnList> list;
			lock (userReportsLoaded)
			{
				list = Enumerable.ToList<DownloadableModOnList>(Enumerable.Where<DownloadableModOnList>(this.VisibleMods, (DownloadableModOnList m) => this._userReportsLoaded.Contains(m.ModId)));
				foreach (DownloadableModOnList downloadableModOnList in list)
				{
					this._userReportsLoaded.Remove(downloadableModOnList.ModId);
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			this.PopulateUserReportsAsync(list, CancellationToken.None);
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0002C688 File Offset: 0x0002A888
		[RelayCommand]
		private void GoBack()
		{
			Application application = Application.Current;
			if (application == null)
			{
				return;
			}
			Window mainWindow = application.MainWindow;
			if (mainWindow == null)
			{
				return;
			}
			mainWindow.Close();
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0002C6A4 File Offset: 0x0002A8A4
		[RelayCommand]
		private Task InitializeAsync()
		{
			ModBrowserViewModel.<InitializeAsync>d__92 <InitializeAsync>d__;
			<InitializeAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<InitializeAsync>d__.<>4__this = this;
			<InitializeAsync>d__.<>1__state = -1;
			<InitializeAsync>d__.<>t__builder.Start<ModBrowserViewModel.<InitializeAsync>d__92>(ref <InitializeAsync>d__);
			return <InitializeAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0002C6E7 File Offset: 0x0002A8E7
		public Task RefreshSearchAsync(bool useCachedResults = false)
		{
			return this.SearchModsCoreAsync(useCachedResults);
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0002C6F0 File Offset: 0x0002A8F0
		public Task LoadAvailableGameVersionsAsync()
		{
			ModBrowserViewModel.<LoadAvailableGameVersionsAsync>d__94 <LoadAvailableGameVersionsAsync>d__;
			<LoadAvailableGameVersionsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadAvailableGameVersionsAsync>d__.<>4__this = this;
			<LoadAvailableGameVersionsAsync>d__.<>1__state = -1;
			<LoadAvailableGameVersionsAsync>d__.<>t__builder.Start<ModBrowserViewModel.<LoadAvailableGameVersionsAsync>d__94>(ref <LoadAvailableGameVersionsAsync>d__);
			return <LoadAvailableGameVersionsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0002C733 File Offset: 0x0002A933
		[RelayCommand]
		private Task SearchModsAsync()
		{
			return this.SearchModsCoreAsync(false);
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0002C73C File Offset: 0x0002A93C
		private Task SearchModsCoreAsync(bool useCachedResults)
		{
			ModBrowserViewModel.<SearchModsCoreAsync>d__96 <SearchModsCoreAsync>d__;
			<SearchModsCoreAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SearchModsCoreAsync>d__.<>4__this = this;
			<SearchModsCoreAsync>d__.useCachedResults = useCachedResults;
			<SearchModsCoreAsync>d__.<>1__state = -1;
			<SearchModsCoreAsync>d__.<>t__builder.Start<ModBrowserViewModel.<SearchModsCoreAsync>d__96>(ref <SearchModsCoreAsync>d__);
			return <SearchModsCoreAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0002C788 File Offset: 0x0002A988
		private string CreateSearchFingerprint()
		{
			string text = string.Join<long>(",", Enumerable.Order<long>(Enumerable.Select<GameVersion, long>(this.SelectedVersions, (GameVersion version) => version.TagId)));
			string text2 = string.Join<int>(",", Enumerable.Order<int>(Enumerable.Select<ModTag, int>(this.SelectedTags, (ModTag tag) => tag.TagId)));
			string text3 = this.OnlyFavorites ? string.Join<int>(",", Enumerable.Order<int>(this.FavoriteMods)) : string.Empty;
			string text4 = "|";
			object[] array = new object[12];
			array[0] = this.TextFilter.Trim();
			int num = 1;
			ModAuthor selectedAuthor = this.SelectedAuthor;
			array[num] = (((selectedAuthor != null) ? selectedAuthor.UserId : null) ?? string.Empty);
			array[2] = text;
			array[3] = text2;
			array[4] = this.SelectedSide;
			array[5] = this.SelectedInstalledFilter;
			array[6] = this.OnlyFavorites;
			array[7] = text3;
			array[8] = this.OrderBy;
			array[9] = this.OrderByDirection;
			array[10] = this.UseRelevantSearchResults;
			array[11] = this._installedModsVersion;
			return string.Join(text4, array);
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0002C8C8 File Offset: 0x0002AAC8
		[RelayCommand]
		private void ClearSearch()
		{
			this.TextFilter = string.Empty;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0002C8D8 File Offset: 0x0002AAD8
		[RelayCommand]
		private Task LoadMore()
		{
			ModBrowserViewModel.<LoadMore>d__99 <LoadMore>d__;
			<LoadMore>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadMore>d__.<>4__this = this;
			<LoadMore>d__.<>1__state = -1;
			<LoadMore>d__.<>t__builder.Start<ModBrowserViewModel.<LoadMore>d__99>(ref <LoadMore>d__);
			return <LoadMore>d__.<>t__builder.Task;
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0002C91C File Offset: 0x0002AB1C
		[RelayCommand]
		private void ClearFilters()
		{
			this.TextFilter = string.Empty;
			this.SelectedAuthor = null;
			this.SelectedVersions.Clear();
			this.SelectedTags.Clear();
			this.SelectedSide = "any";
			this.SelectedInstalledFilter = "all";
			this.OnlyFavorites = false;
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0002C96E File Offset: 0x0002AB6E
		[RelayCommand]
		private void ToggleFavoriteFilter()
		{
			this.OnlyFavorites = !this.OnlyFavorites;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x0002C980 File Offset: 0x0002AB80
		[RelayCommand(CanExecute = "CanDownloadAllFavorites")]
		private Task DownloadAllFavoritesAsync()
		{
			ModBrowserViewModel.<DownloadAllFavoritesAsync>d__102 <DownloadAllFavoritesAsync>d__;
			<DownloadAllFavoritesAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DownloadAllFavoritesAsync>d__.<>4__this = this;
			<DownloadAllFavoritesAsync>d__.<>1__state = -1;
			<DownloadAllFavoritesAsync>d__.<>t__builder.Start<ModBrowserViewModel.<DownloadAllFavoritesAsync>d__102>(ref <DownloadAllFavoritesAsync>d__);
			return <DownloadAllFavoritesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0002C9C3 File Offset: 0x0002ABC3
		private bool CanDownloadAllFavorites()
		{
			return this.FavoriteMods.Count > 0;
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0002C9D3 File Offset: 0x0002ABD3
		[RelayCommand]
		private void ToggleRelevantSearchResults()
		{
			this.UseRelevantSearchResults = !this.UseRelevantSearchResults;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0002C9E4 File Offset: 0x0002ABE4
		[RelayCommand]
		private Task OpenModDetailsAsync(DownloadableModOnList mod)
		{
			ModBrowserViewModel.<OpenModDetailsAsync>d__105 <OpenModDetailsAsync>d__;
			<OpenModDetailsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OpenModDetailsAsync>d__.<>4__this = this;
			<OpenModDetailsAsync>d__.mod = mod;
			<OpenModDetailsAsync>d__.<>1__state = -1;
			<OpenModDetailsAsync>d__.<>t__builder.Start<ModBrowserViewModel.<OpenModDetailsAsync>d__105>(ref <OpenModDetailsAsync>d__);
			return <OpenModDetailsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x0002CA2F File Offset: 0x0002AC2F
		[RelayCommand]
		private void CloseModDetails()
		{
			this.CancelModDetailsLoad();
			this.IsModDetailsOpen = false;
			this.IsModDetailsLoading = false;
			this.ModDetails = null;
			this.SelectedMod = null;
			this.CurrentScreenshotIndex = 0;
			this.ScreenshotUrls = Array.Empty<string>();
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0002CA65 File Offset: 0x0002AC65
		private void CancelModDetailsLoad()
		{
			this._modDetailsLoadVersion++;
			CancellationTokenSource modDetailsLoadCancellation = this._modDetailsLoadCancellation;
			if (modDetailsLoadCancellation != null)
			{
				modDetailsLoadCancellation.Cancel();
			}
			CancellationTokenSource modDetailsLoadCancellation2 = this._modDetailsLoadCancellation;
			if (modDetailsLoadCancellation2 != null)
			{
				modDetailsLoadCancellation2.Dispose();
			}
			this._modDetailsLoadCancellation = null;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0002CAA0 File Offset: 0x0002ACA0
		[RelayCommand]
		private Task DownloadFromDetailsAsync()
		{
			ModBrowserViewModel.<DownloadFromDetailsAsync>d__108 <DownloadFromDetailsAsync>d__;
			<DownloadFromDetailsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DownloadFromDetailsAsync>d__.<>4__this = this;
			<DownloadFromDetailsAsync>d__.<>1__state = -1;
			<DownloadFromDetailsAsync>d__.<>t__builder.Start<ModBrowserViewModel.<DownloadFromDetailsAsync>d__108>(ref <DownloadFromDetailsAsync>d__);
			return <DownloadFromDetailsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0002CAE3 File Offset: 0x0002ACE3
		[RelayCommand]
		private void ToggleFavorite(int modId)
		{
			if (this.FavoriteMods.Contains(modId))
			{
				this.FavoriteMods.Remove(modId);
			}
			else
			{
				this.FavoriteMods.Add(modId);
			}
			base.OnPropertyChanged("FavoriteMods");
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0002CB1C File Offset: 0x0002AD1C
		[RelayCommand]
		private Task InstallModAsync(int modId)
		{
			ModBrowserViewModel.<InstallModAsync>d__110 <InstallModAsync>d__;
			<InstallModAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<InstallModAsync>d__.<>4__this = this;
			<InstallModAsync>d__.modId = modId;
			<InstallModAsync>d__.<>1__state = -1;
			<InstallModAsync>d__.<>t__builder.Start<ModBrowserViewModel.<InstallModAsync>d__110>(ref <InstallModAsync>d__);
			return <InstallModAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0002CB68 File Offset: 0x0002AD68
		[RelayCommand]
		private void OpenModInBrowser(int assetId)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(38, 1);
			defaultInterpolatedStringHandler.AppendLiteral("https://mods.vintagestory.at/show/mod/");
			defaultInterpolatedStringHandler.AppendFormatted<int>(assetId);
			Process.Start(new ProcessStartInfo(defaultInterpolatedStringHandler.ToStringAndClear())
			{
				UseShellExecute = true
			});
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0002CBAC File Offset: 0x0002ADAC
		[RelayCommand]
		private void ChangeOrderBy(string newOrderBy)
		{
			if (this.OrderBy == newOrderBy)
			{
				this.OrderByDirection = ((this.OrderByDirection == "desc") ? "asc" : "desc");
				return;
			}
			this.OrderBy = newOrderBy;
			this.OrderByDirection = "desc";
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0002CBFE File Offset: 0x0002ADFE
		[RelayCommand]
		private void ToggleOrderDirection()
		{
			this.OrderByDirection = ((this.OrderByDirection == "desc") ? "asc" : "desc");
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0002CC24 File Offset: 0x0002AE24
		[RelayCommand]
		private void ToggleVersion(GameVersion version)
		{
			if (this.SelectedVersions.Contains(version))
			{
				this.SelectedVersions.Remove(version);
				return;
			}
			this.SelectedVersions.Add(version);
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0002CC4E File Offset: 0x0002AE4E
		[RelayCommand]
		private void ToggleTag(ModTag tag)
		{
			if (this.SelectedTags.Contains(tag))
			{
				this.SelectedTags.Remove(tag);
				return;
			}
			this.SelectedTags.Add(tag);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0002CC78 File Offset: 0x0002AE78
		private void RestoreSavedSelections()
		{
			if (this._userConfigService == null)
			{
				return;
			}
			this.SelectedVersions.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnFilterCollectionChanged);
			this.SelectedTags.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnFilterCollectionChanged);
			try
			{
				foreach (string text in this._userConfigService.ModBrowserSelectedVersionIds)
				{
					long tagId;
					if (long.TryParse(text, ref tagId))
					{
						GameVersion gameVersion = Enumerable.FirstOrDefault<GameVersion>(this.AvailableVersions, (GameVersion v) => v.TagId == tagId);
						if (gameVersion != null && !this.SelectedVersions.Contains(gameVersion))
						{
							this.SelectedVersions.Add(gameVersion);
						}
					}
				}
				using (List<int>.Enumerator enumerator2 = this._userConfigService.ModBrowserSelectedTagIds.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						int tagId = enumerator2.Current;
						ModTag modTag = Enumerable.FirstOrDefault<ModTag>(this.AvailableTags, (ModTag t) => t.TagId == tagId);
						if (modTag != null && !this.SelectedTags.Contains(modTag))
						{
							this.SelectedTags.Add(modTag);
						}
					}
				}
			}
			finally
			{
				this.SelectedVersions.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnFilterCollectionChanged);
				this.SelectedTags.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnFilterCollectionChanged);
			}
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0002CE0C File Offset: 0x0002B00C
		private Task LoadAuthorsAsync()
		{
			ModBrowserViewModel.<LoadAuthorsAsync>d__117 <LoadAuthorsAsync>d__;
			<LoadAuthorsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadAuthorsAsync>d__.<>4__this = this;
			<LoadAuthorsAsync>d__.<>1__state = -1;
			<LoadAuthorsAsync>d__.<>t__builder.Start<ModBrowserViewModel.<LoadAuthorsAsync>d__117>(ref <LoadAuthorsAsync>d__);
			return <LoadAuthorsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0002CE50 File Offset: 0x0002B050
		private Task LoadGameVersionsAsync()
		{
			ModBrowserViewModel.<LoadGameVersionsAsync>d__118 <LoadGameVersionsAsync>d__;
			<LoadGameVersionsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadGameVersionsAsync>d__.<>4__this = this;
			<LoadGameVersionsAsync>d__.<>1__state = -1;
			<LoadGameVersionsAsync>d__.<>t__builder.Start<ModBrowserViewModel.<LoadGameVersionsAsync>d__118>(ref <LoadGameVersionsAsync>d__);
			return <LoadGameVersionsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0002CE94 File Offset: 0x0002B094
		private Task LoadTagsAsync()
		{
			ModBrowserViewModel.<LoadTagsAsync>d__119 <LoadTagsAsync>d__;
			<LoadTagsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadTagsAsync>d__.<>4__this = this;
			<LoadTagsAsync>d__.<>1__state = -1;
			<LoadTagsAsync>d__.<>t__builder.Start<ModBrowserViewModel.<LoadTagsAsync>d__119>(ref <LoadTagsAsync>d__);
			return <LoadTagsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0002CED8 File Offset: 0x0002B0D8
		private bool IsModInstalled(DownloadableModOnList mod)
		{
			foreach (string modId in ModBrowserViewModel.GetCandidateModIds(mod))
			{
				if (this.IsModInstalledById(modId))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0002CF30 File Offset: 0x0002B130
		[NullableContext(2)]
		private bool IsModInstalledById(string modId)
		{
			string text = ModBrowserViewModel.NormalizeModId(modId);
			return !string.IsNullOrWhiteSpace(text) && this._normalizedInstalledModIds.Contains(text);
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0002CF5C File Offset: 0x0002B15C
		[NullableContext(2)]
		private void AddInstalledModId(string modId)
		{
			string text = ModBrowserViewModel.NormalizeModId(modId);
			if (string.IsNullOrWhiteSpace(text))
			{
				return;
			}
			this._normalizedInstalledModIds.Add(text);
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0002CF88 File Offset: 0x0002B188
		private void RefreshInstalledFlags()
		{
			foreach (DownloadableModOnList downloadableModOnList in this.ModsList)
			{
				downloadableModOnList.IsInstalled = this.IsModInstalled(downloadableModOnList);
			}
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0002CFDC File Offset: 0x0002B1DC
		private static IEnumerable<string> GetCandidateModIds(DownloadableModOnList mod)
		{
			ModBrowserViewModel.<GetCandidateModIds>d__124 <GetCandidateModIds>d__ = new ModBrowserViewModel.<GetCandidateModIds>d__124(-2);
			<GetCandidateModIds>d__.<>3__mod = mod;
			return <GetCandidateModIds>d__;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0002CFEC File Offset: 0x0002B1EC
		[return: Nullable(2)]
		private static string GetPreferredUserReportModId(DownloadableModOnList mod)
		{
			List<string> modIdStrings = mod.ModIdStrings;
			if (modIdStrings != null && modIdStrings.Count > 0)
			{
				foreach (string text in mod.ModIdStrings)
				{
					if (!string.IsNullOrWhiteSpace(text))
					{
						return text;
					}
				}
			}
			if (mod.ModId > 0)
			{
				return mod.ModId.ToString(CultureInfo.InvariantCulture);
			}
			return null;
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0002D078 File Offset: 0x0002B278
		[NullableContext(2)]
		private static string NormalizeModId(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder(value.Length);
			for (int i = 0; i < value.Length; i++)
			{
				char c = value.get_Chars(i);
				if (char.IsLetter(c))
				{
					stringBuilder.Append(char.ToLowerInvariant(c));
				}
				else if (char.IsDigit(c))
				{
					if (stringBuilder.Length == 0)
					{
						stringBuilder.Append('m');
					}
					stringBuilder.Append(c);
				}
			}
			if (stringBuilder.Length != 0)
			{
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0002D100 File Offset: 0x0002B300
		private List<DownloadableModOnList> ApplyClientSideFilters(List<DownloadableModOnList> mods)
		{
			foreach (DownloadableModOnList downloadableModOnList in mods)
			{
				downloadableModOnList.IsInstalled = this.IsModInstalled(downloadableModOnList);
			}
			IEnumerable<DownloadableModOnList> enumerable = Enumerable.AsEnumerable<DownloadableModOnList>(mods);
			if (this.SelectedSide != "any")
			{
				enumerable = Enumerable.Where<DownloadableModOnList>(enumerable, (DownloadableModOnList m) => m.Side.Equals(this.SelectedSide, 5));
			}
			if (this.SelectedInstalledFilter == "installed")
			{
				enumerable = Enumerable.Where<DownloadableModOnList>(enumerable, (DownloadableModOnList m) => m.IsInstalled);
			}
			else if (this.SelectedInstalledFilter == "not-installed")
			{
				enumerable = Enumerable.Where<DownloadableModOnList>(enumerable, (DownloadableModOnList m) => !m.IsInstalled);
			}
			if (this.OnlyFavorites)
			{
				enumerable = Enumerable.Where<DownloadableModOnList>(enumerable, (DownloadableModOnList m) => this.FavoriteMods.Contains(m.ModId));
			}
			return Enumerable.ToList<DownloadableModOnList>(enumerable);
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0002D214 File Offset: 0x0002B414
		private bool ShouldApplyRelevantSorting()
		{
			return this.UseRelevantSearchResults && !string.IsNullOrWhiteSpace(this.TextFilter);
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0002D230 File Offset: 0x0002B430
		private List<DownloadableModOnList> SortByRelevance(IEnumerable<DownloadableModOnList> mods, string query)
		{
			List<string> searchTerms = ModBrowserViewModel.ExtractSearchTerms(query);
			if (searchTerms.Count == 0)
			{
				return Enumerable.ToList<DownloadableModOnList>(mods);
			}
			string normalizedPhrase = string.Join<string>(' ', searchTerms);
			return Enumerable.ToList<DownloadableModOnList>(Enumerable.Select(Enumerable.ThenBy(Enumerable.OrderByDescending(Enumerable.Select(mods, (DownloadableModOnList mod, int index) => new <>f__AnonymousType8<DownloadableModOnList, double, int>(mod, this.CalculateRelevanceScore(mod, searchTerms, normalizedPhrase), index)), entry => entry.Score), entry => entry.Index), entry => entry.Mod));
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0002D300 File Offset: 0x0002B500
		private static List<string> ExtractSearchTerms(string query)
		{
			return Enumerable.ToList<string>(Enumerable.Take<string>(Enumerable.Where<string>(query.Split(' ', 3), (string term) => term.Length > 0), 10));
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0002D33C File Offset: 0x0002B53C
		private double CalculateRelevanceScore(DownloadableModOnList mod, IReadOnlyList<string> terms, string normalizedPhrase)
		{
			double num = 0.0;
			foreach (string term in terms)
			{
				num += ModBrowserViewModel.ScoreField(mod.Name, term, 6.0);
				num += ModBrowserViewModel.ScoreField(mod.Author, term, 4.5);
				num += ModBrowserViewModel.ScoreField(mod.Summary, term, 2.5);
			}
			num += ModBrowserViewModel.ScoreField(mod.Name, normalizedPhrase, 7.5);
			num += ModBrowserViewModel.ScoreField(mod.Author, normalizedPhrase, 5.175);
			num += ModBrowserViewModel.ScoreField(mod.Summary, normalizedPhrase, 2.25);
			return num;
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0002D418 File Offset: 0x0002B618
		private static double ScoreField([Nullable(2)] string field, string term, double weight)
		{
			if (string.IsNullOrWhiteSpace(field) || string.IsNullOrWhiteSpace(term))
			{
				return 0.0;
			}
			double num = 0.0;
			int num3;
			for (int i = 0; i < field.Length; i = num3)
			{
				int num2 = field.IndexOf(term, i, 5);
				if (num2 < 0)
				{
					break;
				}
				num += weight;
				if (num2 == 0 || char.IsWhiteSpace(field.get_Chars(num2 - 1)))
				{
					num += weight * 0.5;
				}
				num3 = num2 + term.Length;
				if (num3 == field.Length || char.IsWhiteSpace(field.get_Chars(num3)))
				{
					num += weight * 0.25;
				}
			}
			return num;
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0002D4BC File Offset: 0x0002B6BC
		private Task PopulateModThumbnailsAsync(IEnumerable<DownloadableModOnList> mods, CancellationToken cancellationToken)
		{
			ModBrowserViewModel.<PopulateModThumbnailsAsync>d__133 <PopulateModThumbnailsAsync>d__;
			<PopulateModThumbnailsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<PopulateModThumbnailsAsync>d__.<>4__this = this;
			<PopulateModThumbnailsAsync>d__.mods = mods;
			<PopulateModThumbnailsAsync>d__.cancellationToken = cancellationToken;
			<PopulateModThumbnailsAsync>d__.<>1__state = -1;
			<PopulateModThumbnailsAsync>d__.<>t__builder.Start<ModBrowserViewModel.<PopulateModThumbnailsAsync>d__133>(ref <PopulateModThumbnailsAsync>d__);
			return <PopulateModThumbnailsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0002D510 File Offset: 0x0002B710
		private Task PopulateUserReportsAsync(IEnumerable<DownloadableModOnList> mods, CancellationToken cancellationToken)
		{
			ModBrowserViewModel.<PopulateUserReportsAsync>d__134 <PopulateUserReportsAsync>d__;
			<PopulateUserReportsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<PopulateUserReportsAsync>d__.<>4__this = this;
			<PopulateUserReportsAsync>d__.mods = mods;
			<PopulateUserReportsAsync>d__.cancellationToken = cancellationToken;
			<PopulateUserReportsAsync>d__.<>1__state = -1;
			<PopulateUserReportsAsync>d__.<>t__builder.Start<ModBrowserViewModel.<PopulateUserReportsAsync>d__134>(ref <PopulateUserReportsAsync>d__);
			return <PopulateUserReportsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0002D564 File Offset: 0x0002B764
		private Task LoadUserReportAsync(DownloadableModOnList mod, CancellationToken cancellationToken)
		{
			ModBrowserViewModel.<LoadUserReportAsync>d__135 <LoadUserReportAsync>d__;
			<LoadUserReportAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadUserReportAsync>d__.<>4__this = this;
			<LoadUserReportAsync>d__.mod = mod;
			<LoadUserReportAsync>d__.cancellationToken = cancellationToken;
			<LoadUserReportAsync>d__.<>1__state = -1;
			<LoadUserReportAsync>d__.<>t__builder.Start<ModBrowserViewModel.<LoadUserReportAsync>d__135>(ref <LoadUserReportAsync>d__);
			return <LoadUserReportAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0002D5B8 File Offset: 0x0002B7B8
		private void ApplyUserReportSummary(DownloadableModOnList mod, ModVersionVoteSummary summary)
		{
			string effectiveGameVersion = this.EffectiveGameVersion;
			string text = ModBrowserViewModel.BuildUserReportDisplay(summary);
			mod.ShowUserReportBadge = !string.IsNullOrWhiteSpace(text);
			mod.UserReportDisplay = (text ?? string.Empty);
			mod.UserReportTooltip = ModBrowserViewModel.BuildUserReportTooltip(summary, effectiveGameVersion ?? string.Empty);
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0002D608 File Offset: 0x0002B808
		[NullableContext(2)]
		private static string BuildUserReportDisplay(ModVersionVoteSummary summary)
		{
			if (summary == null || summary.TotalVotes == 0)
			{
				return null;
			}
			ModVersionVoteOption? majorityOption = summary.GetMajorityOption();
			if (majorityOption == null)
			{
				return "Mixed (" + summary.TotalVotes.ToString(CultureInfo.CurrentCulture) + ")";
			}
			int count = summary.Counts.GetCount(majorityOption.Value);
			return majorityOption.Value.ToDisplayString() + " (" + count.ToString(CultureInfo.CurrentCulture) + ")";
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0002D690 File Offset: 0x0002B890
		private static string BuildUserReportTooltip([Nullable(2)] ModVersionVoteSummary summary, string vintageStoryVersion)
		{
			if (summary != null && summary.TotalVotes != 0)
			{
				ModVersionVoteCounts counts = summary.Counts;
				string text = string.IsNullOrWhiteSpace(vintageStoryVersion) ? "User reports:" : string.Format(CultureInfo.CurrentCulture, "User reports for Vintage Story {0}:", vintageStoryVersion);
				return string.Join(Environment.NewLine, new string[]
				{
					text,
					string.Format(CultureInfo.CurrentCulture, "Fully functional ({0})", counts.FullyFunctional),
					string.Format(CultureInfo.CurrentCulture, "No issues noticed ({0})", counts.NoIssuesSoFar),
					string.Format(CultureInfo.CurrentCulture, "Some issues but works ({0})", counts.SomeIssuesButWorks),
					string.Format(CultureInfo.CurrentCulture, "Not functional ({0})", counts.NotFunctional),
					string.Format(CultureInfo.CurrentCulture, "Crashes/Freezes game ({0})", counts.CrashesOrFreezesGame)
				});
			}
			if (!string.IsNullOrWhiteSpace(vintageStoryVersion))
			{
				return string.Format(CultureInfo.CurrentCulture, "No user reports yet for Vintage Story {0}. Click to share your experience.", vintageStoryVersion);
			}
			return "No user reports yet. Click to share your experience.";
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060006D4 RID: 1748 RVA: 0x0002D793 File Offset: 0x0002B993
		// (set) Token: 0x060006D5 RID: 1749 RVA: 0x0002D79B File Offset: 0x0002B99B
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public ObservableCollection<DownloadableModOnList> ModsList
		{
			get
			{
				return this._modsList;
			}
			[MemberNotNull("_modsList")]
			set
			{
				if (!EqualityComparer<ObservableCollection<DownloadableModOnList>>.Default.Equals(this._modsList, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ModsList);
					this._modsList = value;
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ModsList);
				}
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060006D6 RID: 1750 RVA: 0x0002D7CD File Offset: 0x0002B9CD
		// (set) Token: 0x060006D7 RID: 1751 RVA: 0x0002D7D5 File Offset: 0x0002B9D5
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public ObservableCollection<DownloadableModOnList> VisibleMods
		{
			get
			{
				return this._visibleMods;
			}
			[MemberNotNull("_visibleMods")]
			set
			{
				if (!EqualityComparer<ObservableCollection<DownloadableModOnList>>.Default.Equals(this._visibleMods, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.VisibleMods);
					this._visibleMods = value;
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.VisibleMods);
				}
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x0002D807 File Offset: 0x0002BA07
		// (set) Token: 0x060006D9 RID: 1753 RVA: 0x0002D80F File Offset: 0x0002BA0F
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public int VisibleModsCount
		{
			get
			{
				return this._visibleModsCount;
			}
			set
			{
				if (!EqualityComparer<int>.Default.Equals(this._visibleModsCount, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.VisibleModsCount);
					this._visibleModsCount = value;
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.VisibleModsCount);
				}
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x0002D841 File Offset: 0x0002BA41
		// (set) Token: 0x060006DB RID: 1755 RVA: 0x0002D849 File Offset: 0x0002BA49
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool IsSearching
		{
			get
			{
				return this._isSearching;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(this._isSearching, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsSearching);
					this._isSearching = value;
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsSearching);
				}
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x0002D87B File Offset: 0x0002BA7B
		// (set) Token: 0x060006DD RID: 1757 RVA: 0x0002D883 File Offset: 0x0002BA83
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string LoadingMessage
		{
			get
			{
				return this._loadingMessage;
			}
			[MemberNotNull("_loadingMessage")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(this._loadingMessage, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.LoadingMessage);
					this._loadingMessage = value;
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.LoadingMessage);
				}
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x0002D8B5 File Offset: 0x0002BAB5
		// (set) Token: 0x060006DF RID: 1759 RVA: 0x0002D8BD File Offset: 0x0002BABD
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string TextFilter
		{
			get
			{
				return this._textFilter;
			}
			[MemberNotNull("_textFilter")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(this._textFilter, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.TextFilter);
					this._textFilter = value;
					this.OnTextFilterChanged(value);
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.TextFilter);
				}
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x0002D8F6 File Offset: 0x0002BAF6
		// (set) Token: 0x060006E1 RID: 1761 RVA: 0x0002D8FE File Offset: 0x0002BAFE
		[Nullable(2)]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public ModAuthor SelectedAuthor
		{
			[NullableContext(2)]
			get
			{
				return this._selectedAuthor;
			}
			[NullableContext(2)]
			set
			{
				if (!EqualityComparer<ModAuthor>.Default.Equals(this._selectedAuthor, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SelectedAuthor);
					this._selectedAuthor = value;
					this.OnSelectedAuthorChanged(value);
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SelectedAuthor);
				}
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x0002D937 File Offset: 0x0002BB37
		// (set) Token: 0x060006E3 RID: 1763 RVA: 0x0002D93F File Offset: 0x0002BB3F
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public ObservableCollection<ModAuthor> AvailableAuthors
		{
			get
			{
				return this._availableAuthors;
			}
			[MemberNotNull("_availableAuthors")]
			set
			{
				if (!EqualityComparer<ObservableCollection<ModAuthor>>.Default.Equals(this._availableAuthors, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.AvailableAuthors);
					this._availableAuthors = value;
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.AvailableAuthors);
				}
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0002D971 File Offset: 0x0002BB71
		// (set) Token: 0x060006E5 RID: 1765 RVA: 0x0002D979 File Offset: 0x0002BB79
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public ObservableCollection<GameVersion> SelectedVersions
		{
			get
			{
				return this._selectedVersions;
			}
			[MemberNotNull("_selectedVersions")]
			set
			{
				if (!EqualityComparer<ObservableCollection<GameVersion>>.Default.Equals(this._selectedVersions, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SelectedVersions);
					this._selectedVersions = value;
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SelectedVersions);
				}
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x0002D9AB File Offset: 0x0002BBAB
		// (set) Token: 0x060006E7 RID: 1767 RVA: 0x0002D9B3 File Offset: 0x0002BBB3
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public ObservableCollection<GameVersion> AvailableVersions
		{
			get
			{
				return this._availableVersions;
			}
			[MemberNotNull("_availableVersions")]
			set
			{
				if (!EqualityComparer<ObservableCollection<GameVersion>>.Default.Equals(this._availableVersions, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.AvailableVersions);
					this._availableVersions = value;
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.AvailableVersions);
				}
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x0002D9E5 File Offset: 0x0002BBE5
		// (set) Token: 0x060006E9 RID: 1769 RVA: 0x0002D9ED File Offset: 0x0002BBED
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public ObservableCollection<ModTag> SelectedTags
		{
			get
			{
				return this._selectedTags;
			}
			[MemberNotNull("_selectedTags")]
			set
			{
				if (!EqualityComparer<ObservableCollection<ModTag>>.Default.Equals(this._selectedTags, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SelectedTags);
					this._selectedTags = value;
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SelectedTags);
				}
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x0002DA1F File Offset: 0x0002BC1F
		// (set) Token: 0x060006EB RID: 1771 RVA: 0x0002DA27 File Offset: 0x0002BC27
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public ObservableCollection<ModTag> AvailableTags
		{
			get
			{
				return this._availableTags;
			}
			[MemberNotNull("_availableTags")]
			set
			{
				if (!EqualityComparer<ObservableCollection<ModTag>>.Default.Equals(this._availableTags, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.AvailableTags);
					this._availableTags = value;
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.AvailableTags);
				}
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x0002DA59 File Offset: 0x0002BC59
		// (set) Token: 0x060006ED RID: 1773 RVA: 0x0002DA61 File Offset: 0x0002BC61
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string SelectedSide
		{
			get
			{
				return this._selectedSide;
			}
			[MemberNotNull("_selectedSide")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(this._selectedSide, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SelectedSide);
					this._selectedSide = value;
					this.OnSelectedSideChanged(value);
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SelectedSide);
				}
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x0002DA9A File Offset: 0x0002BC9A
		// (set) Token: 0x060006EF RID: 1775 RVA: 0x0002DAA2 File Offset: 0x0002BCA2
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string SelectedInstalledFilter
		{
			get
			{
				return this._selectedInstalledFilter;
			}
			[MemberNotNull("_selectedInstalledFilter")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(this._selectedInstalledFilter, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SelectedInstalledFilter);
					this._selectedInstalledFilter = value;
					this.OnSelectedInstalledFilterChanged(value);
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SelectedInstalledFilter);
				}
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x0002DADB File Offset: 0x0002BCDB
		// (set) Token: 0x060006F1 RID: 1777 RVA: 0x0002DAE3 File Offset: 0x0002BCE3
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool OnlyFavorites
		{
			get
			{
				return this._onlyFavorites;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(this._onlyFavorites, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.OnlyFavorites);
					this._onlyFavorites = value;
					this.OnOnlyFavoritesChanged(value);
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.OnlyFavorites);
				}
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x0002DB1C File Offset: 0x0002BD1C
		// (set) Token: 0x060006F3 RID: 1779 RVA: 0x0002DB24 File Offset: 0x0002BD24
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string OrderBy
		{
			get
			{
				return this._orderBy;
			}
			[MemberNotNull("_orderBy")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(this._orderBy, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.OrderBy);
					this._orderBy = value;
					this.OnOrderByChanged(value);
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.OrderBy);
				}
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x0002DB5D File Offset: 0x0002BD5D
		// (set) Token: 0x060006F5 RID: 1781 RVA: 0x0002DB65 File Offset: 0x0002BD65
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string OrderByDirection
		{
			get
			{
				return this._orderByDirection;
			}
			[MemberNotNull("_orderByDirection")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(this._orderByDirection, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.OrderByDirection);
					this._orderByDirection = value;
					this.OnOrderByDirectionChanged(value);
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.OrderByDirection);
				}
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x0002DB9E File Offset: 0x0002BD9E
		// (set) Token: 0x060006F7 RID: 1783 RVA: 0x0002DBA6 File Offset: 0x0002BDA6
		[Nullable(2)]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public DownloadableModOnList SelectedMod
		{
			[NullableContext(2)]
			get
			{
				return this._selectedMod;
			}
			[NullableContext(2)]
			set
			{
				if (!EqualityComparer<DownloadableModOnList>.Default.Equals(this._selectedMod, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SelectedMod);
					this._selectedMod = value;
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SelectedMod);
				}
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x0002DBD8 File Offset: 0x0002BDD8
		// (set) Token: 0x060006F9 RID: 1785 RVA: 0x0002DBE0 File Offset: 0x0002BDE0
		[Nullable(2)]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public DownloadableMod ModDetails
		{
			[NullableContext(2)]
			get
			{
				return this._modDetails;
			}
			[NullableContext(2)]
			set
			{
				if (!EqualityComparer<DownloadableMod>.Default.Equals(this._modDetails, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ModDetails);
					this._modDetails = value;
					this.OnModDetailsChanged(value);
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ModDetails);
				}
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x0002DC19 File Offset: 0x0002BE19
		// (set) Token: 0x060006FB RID: 1787 RVA: 0x0002DC21 File Offset: 0x0002BE21
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool IsModDetailsOpen
		{
			get
			{
				return this._isModDetailsOpen;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(this._isModDetailsOpen, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsModDetailsOpen);
					this._isModDetailsOpen = value;
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsModDetailsOpen);
				}
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x0002DC53 File Offset: 0x0002BE53
		// (set) Token: 0x060006FD RID: 1789 RVA: 0x0002DC5B File Offset: 0x0002BE5B
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool IsModDetailsLoading
		{
			get
			{
				return this._isModDetailsLoading;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(this._isModDetailsLoading, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsModDetailsLoading);
					this._isModDetailsLoading = value;
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsModDetailsLoading);
				}
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x0002DC8D File Offset: 0x0002BE8D
		// (set) Token: 0x060006FF RID: 1791 RVA: 0x0002DC95 File Offset: 0x0002BE95
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IReadOnlyList<string> ScreenshotUrls
		{
			get
			{
				return this._screenshotUrls;
			}
			[MemberNotNull("_screenshotUrls")]
			set
			{
				if (!EqualityComparer<IReadOnlyList<string>>.Default.Equals(this._screenshotUrls, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ScreenshotUrls);
					this._screenshotUrls = value;
					this.OnScreenshotUrlsChanged(value);
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ScreenshotUrls);
				}
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x0002DCCE File Offset: 0x0002BECE
		// (set) Token: 0x06000701 RID: 1793 RVA: 0x0002DCD6 File Offset: 0x0002BED6
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public int CurrentScreenshotIndex
		{
			get
			{
				return this._currentScreenshotIndex;
			}
			set
			{
				if (!EqualityComparer<int>.Default.Equals(this._currentScreenshotIndex, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CurrentScreenshotIndex);
					this._currentScreenshotIndex = value;
					this.OnCurrentScreenshotIndexChanged(value);
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CurrentScreenshotIndex);
				}
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x0002DD0F File Offset: 0x0002BF0F
		// (set) Token: 0x06000703 RID: 1795 RVA: 0x0002DD17 File Offset: 0x0002BF17
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public ObservableCollection<int> FavoriteMods
		{
			get
			{
				return this._favoriteMods;
			}
			[MemberNotNull("_favoriteMods")]
			set
			{
				if (!EqualityComparer<ObservableCollection<int>>.Default.Equals(this._favoriteMods, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.FavoriteMods);
					this._favoriteMods = value;
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.FavoriteMods);
					this.DownloadAllFavoritesCommand.NotifyCanExecuteChanged();
				}
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x0002DD54 File Offset: 0x0002BF54
		// (set) Token: 0x06000705 RID: 1797 RVA: 0x0002DD5C File Offset: 0x0002BF5C
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public ObservableCollection<int> InstalledMods
		{
			get
			{
				return this._installedMods;
			}
			[MemberNotNull("_installedMods")]
			set
			{
				if (!EqualityComparer<ObservableCollection<int>>.Default.Equals(this._installedMods, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.InstalledMods);
					this._installedMods = value;
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.InstalledMods);
				}
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x0002DD8E File Offset: 0x0002BF8E
		// (set) Token: 0x06000707 RID: 1799 RVA: 0x0002DD96 File Offset: 0x0002BF96
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool UseRelevantSearchResults
		{
			get
			{
				return this._useRelevantSearchResults;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(this._useRelevantSearchResults, value))
				{
					this.OnPropertyChanging(__KnownINotifyPropertyChangingArgs.UseRelevantSearchResults);
					this._useRelevantSearchResults = value;
					this.OnUseRelevantSearchResultsChanged(value);
					this.OnPropertyChanged(__KnownINotifyPropertyChangedArgs.UseRelevantSearchResults);
				}
			}
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0002DDCF File Offset: 0x0002BFCF
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnTextFilterChanged(string value)
		{
			base.OnPropertyChanged("HasSearchText");
			if (!this._isInitializing)
			{
				this.IsSearching = true;
				this.SearchModsAsync();
			}
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0002DDF2 File Offset: 0x0002BFF2
		[NullableContext(2)]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnSelectedAuthorChanged(ModAuthor value)
		{
			if (!this._isInitializing)
			{
				this.IsSearching = true;
				this.SearchModsAsync();
			}
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0002DE0A File Offset: 0x0002C00A
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnSelectedSideChanged(string value)
		{
			if (!this._isInitializing)
			{
				this.IsSearching = true;
				UserConfigurationService userConfigService = this._userConfigService;
				if (userConfigService != null)
				{
					userConfigService.SetModBrowserSelectedSide(value);
				}
				this.SearchModsAsync();
			}
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0002DE34 File Offset: 0x0002C034
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnSelectedInstalledFilterChanged(string value)
		{
			if (!this._isInitializing)
			{
				this.IsSearching = true;
				UserConfigurationService userConfigService = this._userConfigService;
				if (userConfigService != null)
				{
					userConfigService.SetModBrowserSelectedInstalledFilter(value);
				}
				this.SearchModsAsync();
			}
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0002DE5E File Offset: 0x0002C05E
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnOnlyFavoritesChanged(bool value)
		{
			if (!this._isInitializing)
			{
				this.IsSearching = true;
				UserConfigurationService userConfigService = this._userConfigService;
				if (userConfigService != null)
				{
					userConfigService.SetModBrowserOnlyFavorites(value);
				}
				this.SearchModsAsync();
			}
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0002DE88 File Offset: 0x0002C088
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnOrderByChanged(string value)
		{
			if (!this._isInitializing)
			{
				this.IsSearching = true;
				UserConfigurationService userConfigService = this._userConfigService;
				if (userConfigService != null)
				{
					userConfigService.SetModBrowserOrderBy(value);
				}
				this.SearchModsAsync();
			}
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0002DEB2 File Offset: 0x0002C0B2
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnOrderByDirectionChanged(string value)
		{
			if (!this._isInitializing)
			{
				this.IsSearching = true;
				UserConfigurationService userConfigService = this._userConfigService;
				if (userConfigService != null)
				{
					userConfigService.SetModBrowserOrderByDirection(value);
				}
				this.SearchModsAsync();
			}
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0002DEDC File Offset: 0x0002C0DC
		[NullableContext(2)]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnModDetailsChanged(DownloadableMod value)
		{
			this.BuildScreenshotGallery();
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0002DEE4 File Offset: 0x0002C0E4
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnScreenshotUrlsChanged(IReadOnlyList<string> value)
		{
			base.OnPropertyChanged("HasScreenshots");
			base.OnPropertyChanged("HasMultipleScreenshots");
			base.OnPropertyChanged("CurrentScreenshotUrl");
			base.OnPropertyChanged("ScreenshotCounterText");
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0002DF12 File Offset: 0x0002C112
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnCurrentScreenshotIndexChanged(int value)
		{
			base.OnPropertyChanged("CurrentScreenshotUrl");
			base.OnPropertyChanged("ScreenshotCounterText");
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0002DF2A File Offset: 0x0002C12A
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnUseRelevantSearchResultsChanged(bool value)
		{
			if (!this._isInitializing)
			{
				this.IsSearching = true;
				UserConfigurationService userConfigService = this._userConfigService;
				if (userConfigService != null)
				{
					userConfigService.SetModBrowserRelevantSearch(value);
				}
				this.SearchModsAsync();
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x0002DF54 File Offset: 0x0002C154
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IRelayCommand NextScreenshotCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this.nextScreenshotCommand) == null)
				{
					result = (this.nextScreenshotCommand = new RelayCommand(new Action(this.NextScreenshot)));
				}
				return result;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x0002DF88 File Offset: 0x0002C188
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IRelayCommand PreviousScreenshotCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this.previousScreenshotCommand) == null)
				{
					result = (this.previousScreenshotCommand = new RelayCommand(new Action(this.PreviousScreenshot)));
				}
				return result;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x0002DFBC File Offset: 0x0002C1BC
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IRelayCommand GoBackCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this.goBackCommand) == null)
				{
					result = (this.goBackCommand = new RelayCommand(new Action(this.GoBack)));
				}
				return result;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x0002DFF0 File Offset: 0x0002C1F0
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IAsyncRelayCommand InitializeCommand
		{
			get
			{
				AsyncRelayCommand result;
				if ((result = this.initializeCommand) == null)
				{
					result = (this.initializeCommand = new AsyncRelayCommand(new Func<Task>(this.InitializeAsync)));
				}
				return result;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x0002E024 File Offset: 0x0002C224
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IAsyncRelayCommand SearchModsCommand
		{
			get
			{
				AsyncRelayCommand result;
				if ((result = this.searchModsCommand) == null)
				{
					result = (this.searchModsCommand = new AsyncRelayCommand(new Func<Task>(this.SearchModsAsync)));
				}
				return result;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000718 RID: 1816 RVA: 0x0002E058 File Offset: 0x0002C258
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IRelayCommand ClearSearchCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this.clearSearchCommand) == null)
				{
					result = (this.clearSearchCommand = new RelayCommand(new Action(this.ClearSearch)));
				}
				return result;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x0002E08C File Offset: 0x0002C28C
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IAsyncRelayCommand LoadMoreCommand
		{
			get
			{
				AsyncRelayCommand result;
				if ((result = this.loadMoreCommand) == null)
				{
					result = (this.loadMoreCommand = new AsyncRelayCommand(new Func<Task>(this.LoadMore)));
				}
				return result;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x0002E0C0 File Offset: 0x0002C2C0
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IRelayCommand ClearFiltersCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this.clearFiltersCommand) == null)
				{
					result = (this.clearFiltersCommand = new RelayCommand(new Action(this.ClearFilters)));
				}
				return result;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600071B RID: 1819 RVA: 0x0002E0F4 File Offset: 0x0002C2F4
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IRelayCommand ToggleFavoriteFilterCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this.toggleFavoriteFilterCommand) == null)
				{
					result = (this.toggleFavoriteFilterCommand = new RelayCommand(new Action(this.ToggleFavoriteFilter)));
				}
				return result;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x0002E128 File Offset: 0x0002C328
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IAsyncRelayCommand DownloadAllFavoritesCommand
		{
			get
			{
				AsyncRelayCommand result;
				if ((result = this.downloadAllFavoritesCommand) == null)
				{
					result = (this.downloadAllFavoritesCommand = new AsyncRelayCommand(new Func<Task>(this.DownloadAllFavoritesAsync), new Func<bool>(this.CanDownloadAllFavorites)));
				}
				return result;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x0600071D RID: 1821 RVA: 0x0002E168 File Offset: 0x0002C368
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IRelayCommand ToggleRelevantSearchResultsCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this.toggleRelevantSearchResultsCommand) == null)
				{
					result = (this.toggleRelevantSearchResultsCommand = new RelayCommand(new Action(this.ToggleRelevantSearchResults)));
				}
				return result;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x0002E19C File Offset: 0x0002C39C
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IAsyncRelayCommand<DownloadableModOnList> OpenModDetailsCommand
		{
			get
			{
				AsyncRelayCommand<DownloadableModOnList> result;
				if ((result = this.openModDetailsCommand) == null)
				{
					result = (this.openModDetailsCommand = new AsyncRelayCommand<DownloadableModOnList>(new Func<DownloadableModOnList, Task>(this.OpenModDetailsAsync)));
				}
				return result;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x0002E1D0 File Offset: 0x0002C3D0
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IRelayCommand CloseModDetailsCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this.closeModDetailsCommand) == null)
				{
					result = (this.closeModDetailsCommand = new RelayCommand(new Action(this.CloseModDetails)));
				}
				return result;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x0002E204 File Offset: 0x0002C404
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IAsyncRelayCommand DownloadFromDetailsCommand
		{
			get
			{
				AsyncRelayCommand result;
				if ((result = this.downloadFromDetailsCommand) == null)
				{
					result = (this.downloadFromDetailsCommand = new AsyncRelayCommand(new Func<Task>(this.DownloadFromDetailsAsync)));
				}
				return result;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x0002E238 File Offset: 0x0002C438
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IRelayCommand<int> ToggleFavoriteCommand
		{
			get
			{
				RelayCommand<int> result;
				if ((result = this.toggleFavoriteCommand) == null)
				{
					result = (this.toggleFavoriteCommand = new RelayCommand<int>(new Action<int>(this.ToggleFavorite)));
				}
				return result;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x0002E26C File Offset: 0x0002C46C
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IAsyncRelayCommand<int> InstallModCommand
		{
			get
			{
				AsyncRelayCommand<int> result;
				if ((result = this.installModCommand) == null)
				{
					result = (this.installModCommand = new AsyncRelayCommand<int>(new Func<int, Task>(this.InstallModAsync)));
				}
				return result;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x0002E2A0 File Offset: 0x0002C4A0
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IRelayCommand<int> OpenModInBrowserCommand
		{
			get
			{
				RelayCommand<int> result;
				if ((result = this.openModInBrowserCommand) == null)
				{
					result = (this.openModInBrowserCommand = new RelayCommand<int>(new Action<int>(this.OpenModInBrowser)));
				}
				return result;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x0002E2D4 File Offset: 0x0002C4D4
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IRelayCommand<string> ChangeOrderByCommand
		{
			get
			{
				RelayCommand<string> result;
				if ((result = this.changeOrderByCommand) == null)
				{
					result = (this.changeOrderByCommand = new RelayCommand<string>(new Action<string>(this.ChangeOrderBy)));
				}
				return result;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x0002E308 File Offset: 0x0002C508
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IRelayCommand ToggleOrderDirectionCommand
		{
			get
			{
				RelayCommand result;
				if ((result = this.toggleOrderDirectionCommand) == null)
				{
					result = (this.toggleOrderDirectionCommand = new RelayCommand(new Action(this.ToggleOrderDirection)));
				}
				return result;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x0002E33C File Offset: 0x0002C53C
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IRelayCommand<GameVersion> ToggleVersionCommand
		{
			get
			{
				RelayCommand<GameVersion> result;
				if ((result = this.toggleVersionCommand) == null)
				{
					result = (this.toggleVersionCommand = new RelayCommand<GameVersion>(new Action<GameVersion>(this.ToggleVersion)));
				}
				return result;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x0002E370 File Offset: 0x0002C570
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IRelayCommand<ModTag> ToggleTagCommand
		{
			get
			{
				RelayCommand<ModTag> result;
				if ((result = this.toggleTagCommand) == null)
				{
					result = (this.toggleTagCommand = new RelayCommand<ModTag>(new Action<ModTag>(this.ToggleTag)));
				}
				return result;
			}
		}

		// Token: 0x0400036C RID: 876
		private readonly IModApiService _modApiService;

		// Token: 0x0400036D RID: 877
		[Nullable(2)]
		private readonly UserConfigurationService _userConfigService;

		// Token: 0x0400036E RID: 878
		private readonly ModVersionVoteService _voteService;

		// Token: 0x0400036F RID: 879
		[Nullable(2)]
		private string _installedGameVersion;

		// Token: 0x04000370 RID: 880
		[Nullable(2)]
		private CancellationTokenSource _searchCts;

		// Token: 0x04000371 RID: 881
		private const int DefaultLoadedMods = 45;

		// Token: 0x04000372 RID: 882
		private const int LoadMoreCount = 15;

		// Token: 0x04000373 RID: 883
		private const int PrefetchBufferCount = 10;

		// Token: 0x04000374 RID: 884
		private const double TitleWeight = 6.0;

		// Token: 0x04000375 RID: 885
		private const double AuthorWeight = 4.5;

		// Token: 0x04000376 RID: 886
		private const double SummaryWeight = 2.5;

		// Token: 0x04000377 RID: 887
		private bool _isInitializing;

		// Token: 0x04000378 RID: 888
		private bool _isTabVisible;

		// Token: 0x04000379 RID: 889
		[Nullable(new byte[]
		{
			2,
			1,
			1
		})]
		private Func<DownloadableMod, Task> _installModCallback;

		// Token: 0x0400037A RID: 890
		private int _modDetailsLoadVersion;

		// Token: 0x0400037B RID: 891
		[Nullable(2)]
		private CancellationTokenSource _modDetailsLoadCancellation;

		// Token: 0x0400037C RID: 892
		private readonly HashSet<int> _userReportsLoaded = new HashSet<int>();

		// Token: 0x0400037D RID: 893
		private readonly HashSet<int> _modsWithLoadedLogos = new HashSet<int>();

		// Token: 0x0400037E RID: 894
		private readonly HashSet<string> _normalizedInstalledModIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400037F RID: 895
		private long _lastLoadMoreTicks;

		// Token: 0x04000380 RID: 896
		private const int LoadMoreThrottleMs = 300;

		// Token: 0x04000381 RID: 897
		private int _activeSearchCount;

		// Token: 0x04000382 RID: 898
		[Nullable(2)]
		private CancellationTokenSource _pendingLoadMoreCts;

		// Token: 0x04000383 RID: 899
		private bool _isLoadingMore;

		// Token: 0x04000384 RID: 900
		private readonly SemaphoreSlim _gameVersionsLoadGate = new SemaphoreSlim(1, 1);

		// Token: 0x04000385 RID: 901
		[Nullable(2)]
		private string _lastSuccessfulSearchFingerprint;

		// Token: 0x04000386 RID: 902
		private bool _hasCachedSearchResults;

		// Token: 0x04000387 RID: 903
		private int _installedModsVersion;

		// Token: 0x04000388 RID: 904
		[ObservableProperty]
		private ObservableCollection<DownloadableModOnList> _modsList = new ObservableCollection<DownloadableModOnList>();

		// Token: 0x04000389 RID: 905
		[ObservableProperty]
		private ObservableCollection<DownloadableModOnList> _visibleMods = new ObservableCollection<DownloadableModOnList>();

		// Token: 0x0400038A RID: 906
		[ObservableProperty]
		private int _visibleModsCount = 45;

		// Token: 0x0400038B RID: 907
		[ObservableProperty]
		private bool _isSearching;

		// Token: 0x0400038C RID: 908
		[ObservableProperty]
		private string _loadingMessage = "Loading mods...";

		// Token: 0x0400038D RID: 909
		[ObservableProperty]
		private string _textFilter = string.Empty;

		// Token: 0x0400038E RID: 910
		[Nullable(2)]
		[ObservableProperty]
		private ModAuthor _selectedAuthor;

		// Token: 0x0400038F RID: 911
		[ObservableProperty]
		private ObservableCollection<ModAuthor> _availableAuthors = new ObservableCollection<ModAuthor>();

		// Token: 0x04000390 RID: 912
		[ObservableProperty]
		private ObservableCollection<GameVersion> _selectedVersions = new ObservableCollection<GameVersion>();

		// Token: 0x04000391 RID: 913
		[ObservableProperty]
		private ObservableCollection<GameVersion> _availableVersions = new ObservableCollection<GameVersion>();

		// Token: 0x04000392 RID: 914
		[ObservableProperty]
		private ObservableCollection<ModTag> _selectedTags = new ObservableCollection<ModTag>();

		// Token: 0x04000393 RID: 915
		[ObservableProperty]
		private ObservableCollection<ModTag> _availableTags = new ObservableCollection<ModTag>();

		// Token: 0x04000394 RID: 916
		[ObservableProperty]
		private string _selectedSide = "any";

		// Token: 0x04000395 RID: 917
		[ObservableProperty]
		private string _selectedInstalledFilter = "all";

		// Token: 0x04000396 RID: 918
		[ObservableProperty]
		private bool _onlyFavorites;

		// Token: 0x04000397 RID: 919
		[ObservableProperty]
		private string _orderBy = "follows";

		// Token: 0x04000398 RID: 920
		[ObservableProperty]
		private string _orderByDirection = "desc";

		// Token: 0x04000399 RID: 921
		[Nullable(2)]
		[ObservableProperty]
		private DownloadableModOnList _selectedMod;

		// Token: 0x0400039A RID: 922
		[Nullable(2)]
		[ObservableProperty]
		private DownloadableMod _modDetails;

		// Token: 0x0400039B RID: 923
		[ObservableProperty]
		private bool _isModDetailsOpen;

		// Token: 0x0400039C RID: 924
		[ObservableProperty]
		private bool _isModDetailsLoading;

		// Token: 0x0400039D RID: 925
		[ObservableProperty]
		private IReadOnlyList<string> _screenshotUrls = Array.Empty<string>();

		// Token: 0x0400039E RID: 926
		[ObservableProperty]
		private int _currentScreenshotIndex;

		// Token: 0x0400039F RID: 927
		[ObservableProperty]
		[NotifyCanExecuteChangedFor("DownloadAllFavoritesCommand")]
		private ObservableCollection<int> _favoriteMods = new ObservableCollection<int>();

		// Token: 0x040003A0 RID: 928
		[ObservableProperty]
		private ObservableCollection<int> _installedMods = new ObservableCollection<int>();

		// Token: 0x040003A1 RID: 929
		[ObservableProperty]
		private bool _useRelevantSearchResults = true;

		// Token: 0x040003A2 RID: 930
		[Nullable(2)]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private RelayCommand nextScreenshotCommand;

		// Token: 0x040003A3 RID: 931
		[Nullable(2)]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private RelayCommand previousScreenshotCommand;

		// Token: 0x040003A4 RID: 932
		[Nullable(2)]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private RelayCommand goBackCommand;

		// Token: 0x040003A5 RID: 933
		[Nullable(2)]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private AsyncRelayCommand initializeCommand;

		// Token: 0x040003A6 RID: 934
		[Nullable(2)]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private AsyncRelayCommand searchModsCommand;

		// Token: 0x040003A7 RID: 935
		[Nullable(2)]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private RelayCommand clearSearchCommand;

		// Token: 0x040003A8 RID: 936
		[Nullable(2)]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private AsyncRelayCommand loadMoreCommand;

		// Token: 0x040003A9 RID: 937
		[Nullable(2)]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private RelayCommand clearFiltersCommand;

		// Token: 0x040003AA RID: 938
		[Nullable(2)]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private RelayCommand toggleFavoriteFilterCommand;

		// Token: 0x040003AB RID: 939
		[Nullable(2)]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private AsyncRelayCommand downloadAllFavoritesCommand;

		// Token: 0x040003AC RID: 940
		[Nullable(2)]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private RelayCommand toggleRelevantSearchResultsCommand;

		// Token: 0x040003AD RID: 941
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private AsyncRelayCommand<DownloadableModOnList> openModDetailsCommand;

		// Token: 0x040003AE RID: 942
		[Nullable(2)]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private RelayCommand closeModDetailsCommand;

		// Token: 0x040003AF RID: 943
		[Nullable(2)]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private AsyncRelayCommand downloadFromDetailsCommand;

		// Token: 0x040003B0 RID: 944
		[Nullable(2)]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private RelayCommand<int> toggleFavoriteCommand;

		// Token: 0x040003B1 RID: 945
		[Nullable(2)]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private AsyncRelayCommand<int> installModCommand;

		// Token: 0x040003B2 RID: 946
		[Nullable(2)]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private RelayCommand<int> openModInBrowserCommand;

		// Token: 0x040003B3 RID: 947
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private RelayCommand<string> changeOrderByCommand;

		// Token: 0x040003B4 RID: 948
		[Nullable(2)]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private RelayCommand toggleOrderDirectionCommand;

		// Token: 0x040003B5 RID: 949
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private RelayCommand<GameVersion> toggleVersionCommand;

		// Token: 0x040003B6 RID: 950
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private RelayCommand<ModTag> toggleTagCommand;
	}
}
