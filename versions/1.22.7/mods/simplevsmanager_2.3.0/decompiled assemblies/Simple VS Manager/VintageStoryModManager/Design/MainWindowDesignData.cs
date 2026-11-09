using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Data;
using CommunityToolkit.Mvvm.Input;
using VintageStoryModManager.ViewModels;

namespace VintageStoryModManager.Design
{
	// Token: 0x020000C9 RID: 201
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MainWindowDesignData
	{
		// Token: 0x06001024 RID: 4132 RVA: 0x000564E8 File Offset: 0x000546E8
		public MainWindowDesignData()
		{
			this.DataDirectory = "C:\\\\Users\\\\Player\\\\AppData\\\\Roaming\\\\VintagestoryData";
			this._mods = new ObservableCollection<ModListItemViewModel>(DesignDataFactory.CreateSampleMods());
			this.ModsView = CollectionViewSource.GetDefaultView(this._mods);
			ObservableCollection<SortOption> observableCollection = new ObservableCollection<SortOption>();
			observableCollection.Add(new SortOption("Active (Active → Inactive)", new ValueTuple<string, ListSortDirection>[]
			{
				new ValueTuple<string, ListSortDirection>("ActiveSortOrder", 0),
				new ValueTuple<string, ListSortDirection>("NameSortKey", 0)
			}));
			observableCollection.Add(new SortOption("Name", new ValueTuple<string, ListSortDirection>[]
			{
				new ValueTuple<string, ListSortDirection>("NameSortKey", 0)
			}));
			observableCollection.Add(new SortOption("Version", new ValueTuple<string, ListSortDirection>[]
			{
				new ValueTuple<string, ListSortDirection>("VersionDisplay", 0)
			}));
			this._sortOptions = observableCollection;
			this.SortOptions = new ReadOnlyObservableCollection<SortOption>(this._sortOptions);
			this.RefreshCommand = new AsyncRelayCommand(() => Task.CompletedTask);
			this.SelectedSortOption = Enumerable.FirstOrDefault<SortOption>(this.SortOptions);
			this.TotalMods = this._mods.Count;
			this.ActiveMods = Enumerable.Count<ModListItemViewModel>(this._mods, (ModListItemViewModel mod) => mod.IsActive);
			this.UpdatableModsCount = Enumerable.Count<ModListItemViewModel>(this._mods, (ModListItemViewModel mod) => mod.CanUpdate);
			this.StatusMessage = "Loaded sample mods.";
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x06001025 RID: 4133 RVA: 0x00056686 File Offset: 0x00054886
		public string DataDirectory { get; }

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x06001026 RID: 4134 RVA: 0x0005668E File Offset: 0x0005488E
		public ICollectionView ModsView { get; }

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x06001027 RID: 4135 RVA: 0x00056696 File Offset: 0x00054896
		public ReadOnlyObservableCollection<SortOption> SortOptions { get; }

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06001028 RID: 4136 RVA: 0x0005669E File Offset: 0x0005489E
		// (set) Token: 0x06001029 RID: 4137 RVA: 0x000566A6 File Offset: 0x000548A6
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
				this._selectedSortOption = value;
				SortOption selectedSortOption = this._selectedSortOption;
				if (selectedSortOption == null)
				{
					return;
				}
				selectedSortOption.Apply(this.ModsView);
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x0600102A RID: 4138 RVA: 0x000566C5 File Offset: 0x000548C5
		public IAsyncRelayCommand RefreshCommand { get; }

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x0600102B RID: 4139 RVA: 0x000566CD File Offset: 0x000548CD
		public bool IsBusy
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x0600102C RID: 4140 RVA: 0x000566D0 File Offset: 0x000548D0
		public string StatusMessage { get; }

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x0600102D RID: 4141 RVA: 0x000566D8 File Offset: 0x000548D8
		public bool HasStatusMessage
		{
			get
			{
				return !string.IsNullOrWhiteSpace(this.StatusMessage);
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x0600102E RID: 4142 RVA: 0x000566E8 File Offset: 0x000548E8
		public bool IsErrorStatus
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x0600102F RID: 4143 RVA: 0x000566EB File Offset: 0x000548EB
		public int TotalMods { get; }

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06001030 RID: 4144 RVA: 0x000566F3 File Offset: 0x000548F3
		public int ActiveMods { get; }

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06001031 RID: 4145 RVA: 0x000566FC File Offset: 0x000548FC
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

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06001032 RID: 4146 RVA: 0x0005675A File Offset: 0x0005495A
		public int UpdatableModsCount { get; }

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06001033 RID: 4147 RVA: 0x00056764 File Offset: 0x00054964
		public string UpdateAllButtonLabel
		{
			get
			{
				if (this.UpdatableModsCount != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(13, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Update All (");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.UpdatableModsCount);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					return defaultInterpolatedStringHandler.ToStringAndClear();
				}
				return "Update All";
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06001034 RID: 4148 RVA: 0x000567B8 File Offset: 0x000549B8
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

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001035 RID: 4149 RVA: 0x00056809 File Offset: 0x00054A09
		public string NoModsFoundMessage
		{
			get
			{
				return "No mods found. If this is unexpected, verify that your VintageStoryData folder is correctly set: " + this.DataDirectory + ". You can change it in the File Menu.";
			}
		}

		// Token: 0x0400070C RID: 1804
		private readonly ObservableCollection<ModListItemViewModel> _mods;

		// Token: 0x0400070D RID: 1805
		private readonly ObservableCollection<SortOption> _sortOptions;

		// Token: 0x0400070E RID: 1806
		[Nullable(2)]
		private SortOption _selectedSortOption;
	}
}
