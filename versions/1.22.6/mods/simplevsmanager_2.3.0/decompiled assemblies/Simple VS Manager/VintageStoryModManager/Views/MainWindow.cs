using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using ModernWpf.Controls;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SimpleVsManager.Cloud;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using VintageStoryModManager.Helpers;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services;
using VintageStoryModManager.ViewModels;
using VintageStoryModManager.Views.Dialogs;
using YamlDotNet.Core;

namespace VintageStoryModManager.Views
{
	// Token: 0x02000016 RID: 22
	[NullableContext(1)]
	[Nullable(0)]
	public class MainWindow : Window, IComponentConnector, IStyleConnector
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00006145 File Offset: 0x00004345
		// (set) Token: 0x0600012A RID: 298 RVA: 0x00006157 File Offset: 0x00004357
		public bool IsDataBackupInProgress
		{
			get
			{
				return (bool)base.GetValue(MainWindow.IsDataBackupInProgressProperty);
			}
			set
			{
				base.SetValue(MainWindow.IsDataBackupInProgressProperty, value);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600012B RID: 299 RVA: 0x0000616A File Offset: 0x0000436A
		// (set) Token: 0x0600012C RID: 300 RVA: 0x0000617C File Offset: 0x0000437C
		public double DataBackupProgress
		{
			get
			{
				return (double)base.GetValue(MainWindow.DataBackupProgressProperty);
			}
			set
			{
				base.SetValue(MainWindow.DataBackupProgressProperty, value);
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600012D RID: 301 RVA: 0x0000618F File Offset: 0x0000438F
		// (set) Token: 0x0600012E RID: 302 RVA: 0x000061A1 File Offset: 0x000043A1
		public string DataBackupStatusMessage
		{
			get
			{
				return (string)base.GetValue(MainWindow.DataBackupStatusMessageProperty);
			}
			set
			{
				base.SetValue(MainWindow.DataBackupStatusMessageProperty, value);
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600012F RID: 303 RVA: 0x000061AF File Offset: 0x000043AF
		// (set) Token: 0x06000130 RID: 304 RVA: 0x000061C1 File Offset: 0x000043C1
		public bool IsModlistInstallInProgress
		{
			get
			{
				return (bool)base.GetValue(MainWindow.IsModlistInstallInProgressProperty);
			}
			set
			{
				base.SetValue(MainWindow.IsModlistInstallInProgressProperty, value);
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000131 RID: 305 RVA: 0x000061D4 File Offset: 0x000043D4
		// (set) Token: 0x06000132 RID: 306 RVA: 0x000061E6 File Offset: 0x000043E6
		public double ModlistInstallProgress
		{
			get
			{
				return (double)base.GetValue(MainWindow.ModlistInstallProgressProperty);
			}
			set
			{
				base.SetValue(MainWindow.ModlistInstallProgressProperty, value);
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000133 RID: 307 RVA: 0x000061F9 File Offset: 0x000043F9
		// (set) Token: 0x06000134 RID: 308 RVA: 0x0000620B File Offset: 0x0000440B
		public string ModlistInstallStatusMessage
		{
			get
			{
				return (string)base.GetValue(MainWindow.ModlistInstallStatusMessageProperty);
			}
			set
			{
				base.SetValue(MainWindow.ModlistInstallStatusMessageProperty, value);
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00006219 File Offset: 0x00004419
		// (set) Token: 0x06000136 RID: 310 RVA: 0x0000622B File Offset: 0x0000442B
		public string ModlistDownloadSpeed
		{
			get
			{
				return (string)base.GetValue(MainWindow.ModlistDownloadSpeedProperty);
			}
			set
			{
				base.SetValue(MainWindow.ModlistDownloadSpeedProperty, value);
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00006239 File Offset: 0x00004439
		// (set) Token: 0x06000138 RID: 312 RVA: 0x0000624B File Offset: 0x0000444B
		public bool HasModlistDownloadSpeed
		{
			get
			{
				return (bool)base.GetValue(MainWindow.HasModlistDownloadSpeedProperty);
			}
			set
			{
				base.SetValue(MainWindow.HasModlistDownloadSpeedProperty, value);
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00006260 File Offset: 0x00004460
		public MainWindow()
		{
			this.RefreshModsUiCommand = new AsyncRelayCommand(new Func<Task>(this.RefreshModsWithErrorHandlingAsync), 1);
			this._userConfiguration = new UserConfigurationService();
			this._dataBackupService = new DataBackupService(this._userConfiguration.GetConfigurationDirectory(), this._userConfiguration.CustomDataBackupLocation);
			this._modActivityLoggingService = new ModActivityLoggingService(this._userConfiguration);
			this.InitializeComponent();
			this.InitializeModInfoPane();
			this.InitializeModBrowserView();
			DeveloperProfileManager.CurrentProfileChanged += new EventHandler<DeveloperProfileChangedEventArgs>(this.DeveloperProfileManager_OnCurrentProfileChanged);
			this.UpdateModlistLoadingUiState();
			this.InitializeColumnVisibilityMenu();
			this.ApplyStoredWindowDimensions();
			this.CacheAllVersionsMenuItem.IsChecked = this._userConfiguration.CacheAllVersionsLocally;
			this.RequireExactVsVersionMenuItem.IsChecked = this._userConfiguration.RequireExactVsVersionMatch;
			this.DisableAutoRefreshMenuItem.IsChecked = this._userConfiguration.DisableAutoRefresh;
			this.DisableInternetAccessMenuItem.IsChecked = this._userConfiguration.DisableInternetAccess;
			this.AutomaticDataBackupsMenuItem.IsChecked = this._userConfiguration.AutomaticDataBackupsEnabled;
			this.CloseManagerAfterGameLaunchMenuItem.IsChecked = this._userConfiguration.CloseManagerAfterGameLaunch;
			InternetAccessManager.SetInternetAccessDisabled(this._userConfiguration.DisableInternetAccess);
			this.UpdateServerOptionsState(this._userConfiguration.EnableServerOptions);
			this.UseFasterThumbnailsMenuItem.IsChecked = this._userConfiguration.UseFasterThumbnails;
			this.CacheModDatabaseResultsMenuItem.IsChecked = this._userConfiguration.CacheModDatabaseResults;
			this.DisableHoverEffectsMenuItem.IsChecked = this._userConfiguration.DisableHoverEffects;
			HoverEffectHelper.SetDisableHoverEffects(this, this._userConfiguration.DisableHoverEffects);
			this.UpdateLoggingMenuState();
			this.InitializeTraceListener();
			this._modActivityLoggingService.LogAppLaunch();
			this.RefreshCustomThemeMenuItems();
			this.UpdateThemeMenuSelection(this._userConfiguration.ColorTheme, this._userConfiguration.GetCurrentThemeName());
			if (this.ManagerVersionMenuItem != null)
			{
				string managerInformationalVersion = MainWindow.GetManagerInformationalVersion();
				if (string.IsNullOrWhiteSpace(managerInformationalVersion))
				{
					this.ManagerVersionMenuItem.Visibility = 2;
				}
				else
				{
					this.ManagerVersionMenuItem.Header = "Version: " + managerInformationalVersion;
					this.ManagerVersionMenuItem.Visibility = 0;
				}
			}
			this.UpdateModlistAutoLoadMenu(this._userConfiguration.ModlistAutoLoadBehavior);
			this.TryInitializePaths();
			this.RefreshDeveloperProfilesMenuEntries();
			this.UpdateGameProfileMenuChecks();
			this.UpdateActiveGameProfileDisplay();
			this.UpdateGameVersionMenuItem(VintageStoryVersionLocator.GetInstalledVersion(this._gameDirectory));
			if (!string.IsNullOrWhiteSpace(this._dataDirectory))
			{
				try
				{
					this.InitializeViewModel();
				}
				catch (Exception exception)
				{
					this.HandleViewModelInitializationFailure(exception);
				}
			}
			base.Loaded += new RoutedEventHandler(this.MainWindow_Loaded);
			base.Closing += new CancelEventHandler(this.MainWindow_OnClosing);
			InternetAccessManager.InternetAccessChanged += new EventHandler(this.InternetAccessManager_OnInternetAccessChanged);
			this.UpdateCloudModlistControlsEnabledState();
			this.UpdateLocalModlistControlsEnabledState();
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600013A RID: 314 RVA: 0x000065B8 File Offset: 0x000047B8
		public IAsyncRelayCommand RefreshModsUiCommand { get; }

		// Token: 0x0600013B RID: 315 RVA: 0x000065C0 File Offset: 0x000047C0
		public void ReportStatus(string message, bool isError = false)
		{
			MainViewModel viewModel = this._viewModel;
			if (viewModel == null)
			{
				return;
			}
			viewModel.ReportStatus(message, isError);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000065D4 File Offset: 0x000047D4
		private void InitializeColumnVisibilityMenu()
		{
			this.RegisterColumnMenuItem(this.ActiveColumnMenuItem, MainWindow.InstalledModsColumn.Active);
			this.RegisterColumnMenuItem(this.IconColumnMenuItem, MainWindow.InstalledModsColumn.Icon);
			this.RegisterColumnMenuItem(this.NameColumnMenuItem, MainWindow.InstalledModsColumn.Name);
			this.RegisterColumnMenuItem(this.VersionColumnMenuItem, MainWindow.InstalledModsColumn.Version);
			this.RegisterColumnMenuItem(this.LatestVersionColumnMenuItem, MainWindow.InstalledModsColumn.LatestVersion);
			this.RegisterColumnMenuItem(this.DateModifiedColumnMenuItem, MainWindow.InstalledModsColumn.DateModified);
			this.RegisterColumnMenuItem(this.AuthorsColumnMenuItem, MainWindow.InstalledModsColumn.Authors);
			this.RegisterColumnMenuItem(this.TagsColumnMenuItem, MainWindow.InstalledModsColumn.Tags);
			this.RegisterColumnMenuItem(this.UserReportsColumnMenuItem, MainWindow.InstalledModsColumn.UserReports);
			this.RegisterColumnMenuItem(this.StatusColumnMenuItem, MainWindow.InstalledModsColumn.Status);
			this.RegisterColumnMenuItem(this.SideColumnMenuItem, MainWindow.InstalledModsColumn.Side);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00006674 File Offset: 0x00004874
		[NullableContext(2)]
		private void RegisterColumnMenuItem(MenuItem menuItem, MainWindow.InstalledModsColumn column)
		{
			if (menuItem == null)
			{
				return;
			}
			menuItem.Tag = column;
			bool? installedColumnVisibility = this._userConfiguration.GetInstalledColumnVisibility(column.ToString());
			if (installedColumnVisibility != null)
			{
				bool valueOrDefault = installedColumnVisibility.GetValueOrDefault();
				menuItem.IsChecked = valueOrDefault;
			}
			this._installedColumnVisibilityPreferences[column] = menuItem.IsChecked;
			this.NotifyViewModelOfInstalledColumnVisibility(column, menuItem.IsChecked);
			this.ApplyInstalledColumnVisibility(column, menuItem.IsChecked);
			menuItem.Checked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
			menuItem.Unchecked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00006714 File Offset: 0x00004914
		private void InstalledModsColumnMenuItem_OnChecked(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem != null)
			{
				object tag = menuItem.Tag;
				if (tag is MainWindow.InstalledModsColumn)
				{
					MainWindow.InstalledModsColumn installedModsColumn = (MainWindow.InstalledModsColumn)tag;
					this._installedColumnVisibilityPreferences[installedModsColumn] = menuItem.IsChecked;
					this._userConfiguration.SetInstalledColumnVisibility(installedModsColumn.ToString(), menuItem.IsChecked);
					this.NotifyViewModelOfInstalledColumnVisibility(installedModsColumn, menuItem.IsChecked);
					this.ApplyInstalledColumnVisibility(installedModsColumn, menuItem.IsChecked);
					return;
				}
			}
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00006790 File Offset: 0x00004990
		private void ApplyInstalledColumnVisibility(MainWindow.InstalledModsColumn columnKey, bool isVisible)
		{
			DataGridColumn dataGridColumn;
			switch (columnKey)
			{
			case MainWindow.InstalledModsColumn.Active:
				dataGridColumn = this.ActiveColumn;
				goto IL_A1;
			case MainWindow.InstalledModsColumn.Icon:
				dataGridColumn = this.IconColumn;
				goto IL_A1;
			case MainWindow.InstalledModsColumn.Name:
				dataGridColumn = this.NameColumn;
				goto IL_A1;
			case MainWindow.InstalledModsColumn.Version:
				dataGridColumn = this.VersionColumn;
				goto IL_A1;
			case MainWindow.InstalledModsColumn.LatestVersion:
				dataGridColumn = this.LatestVersionColumn;
				goto IL_A1;
			case MainWindow.InstalledModsColumn.DateModified:
				dataGridColumn = this.DateModifiedColumn;
				goto IL_A1;
			case MainWindow.InstalledModsColumn.Authors:
				dataGridColumn = this.AuthorsColumn;
				goto IL_A1;
			case MainWindow.InstalledModsColumn.Tags:
				dataGridColumn = this.TagsColumn;
				goto IL_A1;
			case MainWindow.InstalledModsColumn.UserReports:
				dataGridColumn = this.UserReportsColumn;
				goto IL_A1;
			case MainWindow.InstalledModsColumn.Status:
				dataGridColumn = this.StatusColumn;
				goto IL_A1;
			case MainWindow.InstalledModsColumn.Side:
				dataGridColumn = this.SideColumn;
				goto IL_A1;
			}
			dataGridColumn = null;
			IL_A1:
			DataGridColumn dataGridColumn2 = dataGridColumn;
			if (dataGridColumn2 == null)
			{
				return;
			}
			dataGridColumn2.Visibility = (isVisible ? 0 : 2);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00006851 File Offset: 0x00004A51
		private void NotifyViewModelOfInstalledColumnVisibility(MainWindow.InstalledModsColumn column, bool isVisible)
		{
			MainViewModel viewModel = this._viewModel;
			if (viewModel == null)
			{
				return;
			}
			viewModel.SetInstalledColumnVisibility(column.ToString(), isVisible);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00006874 File Offset: 0x00004A74
		private void ApplyColumnVisibilityPreferencesToViewModel()
		{
			if (this._viewModel == null)
			{
				return;
			}
			foreach (KeyValuePair<MainWindow.InstalledModsColumn, bool> keyValuePair in this._installedColumnVisibilityPreferences)
			{
				this.NotifyViewModelOfInstalledColumnVisibility(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000068E0 File Offset: 0x00004AE0
		private void ModsDataGrid_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			DependencyObject source = e.OriginalSource as DependencyObject;
			DataGridColumnHeader dataGridColumnHeader = MainWindow.FindAncestor<DataGridColumnHeader>(source);
			this._installedContextColumn = ((dataGridColumnHeader != null) ? dataGridColumnHeader.Column : null);
			if (dataGridColumnHeader != null || MainWindow.FindAncestor<ScrollBar>(source) != null)
			{
				return;
			}
			DataGridRow dataGridRow = MainWindow.FindAncestor<DataGridRow>(source);
			ModListItemViewModel modListItemViewModel = ((dataGridRow != null) ? dataGridRow.Item : null) as ModListItemViewModel;
			if (modListItemViewModel != null)
			{
				if (!this._selectedMods.Contains(modListItemViewModel))
				{
					this.ClearSelection(false);
					this.AddToSelection(modListItemViewModel);
					this._selectionAnchor = modListItemViewModel;
				}
				return;
			}
			this.ClearSelection(true);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00006964 File Offset: 0x00004B64
		private void ModsDataGrid_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			ContextMenu installedModsContextMenu = this.InstalledModsContextMenu;
			DataGridColumn installedContextColumn = this._installedContextColumn;
			this._installedContextColumn = null;
			installedModsContextMenu.Items.Clear();
			if (installedContextColumn != null)
			{
				this.PopulateInstalledColumnHeaderContextMenu(installedModsContextMenu, installedContextColumn);
				return;
			}
			this.PopulateInstalledModsContextMenu(installedModsContextMenu);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000069A4 File Offset: 0x00004BA4
		private void PopulateInstalledColumnHeaderContextMenu(ContextMenu menu, DataGridColumn column)
		{
			MainWindow.InstalledModsColumn columnKey;
			if (this.TryGetInstalledColumn(column, out columnKey))
			{
				string installedColumnLabel = MainWindow.GetInstalledColumnLabel(columnKey);
				menu.Items.Add(this.CreateContextMenuItem("Hide " + installedColumnLabel + " column", delegate(object _, RoutedEventArgs _)
				{
					this.SetInstalledColumnVisibility(columnKey, false);
				}, column.Visibility == 0, null, null));
				menu.Items.Add(new Separator());
			}
			menu.Items.Add(this.CreateInstalledColumnsSubmenu());
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00006A38 File Offset: 0x00004C38
		private void PopulateInstalledModsContextMenu(ContextMenu menu)
		{
			List<ModListItemViewModel> list = Enumerable.ToList<ModListItemViewModel>(this._selectedMods);
			ModListItemViewModel modListItemViewModel = (list.Count == 1) ? list[0] : null;
			int num = Enumerable.Count<ModListItemViewModel>(list, (ModListItemViewModel mod) => mod.CanToggle && !mod.IsActive);
			int num2 = Enumerable.Count<ModListItemViewModel>(list, (ModListItemViewModel mod) => mod.CanToggle && mod.IsActive);
			int num3 = Enumerable.Count<ModListItemViewModel>(list, (ModListItemViewModel mod) => mod.CanUpdate);
			if (num > 0)
			{
				ItemCollection items = menu.Items;
				string header;
				if (list.Count <= 1)
				{
					header = "Activate";
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(25, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Activate selected mods (");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					header = defaultInterpolatedStringHandler.ToStringAndClear();
				}
				items.Add(this.CreateContextMenuItem(header, delegate(object _, RoutedEventArgs _)
				{
					this.SetSelectedModsActivation(true);
				}, true, null, null));
			}
			if (num2 > 0)
			{
				ItemCollection items2 = menu.Items;
				string header2;
				if (list.Count <= 1)
				{
					header2 = "Deactivate";
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
					defaultInterpolatedStringHandler2..ctor(27, 1);
					defaultInterpolatedStringHandler2.AppendLiteral("Deactivate selected mods (");
					defaultInterpolatedStringHandler2.AppendFormatted<int>(num2);
					defaultInterpolatedStringHandler2.AppendLiteral(")");
					header2 = defaultInterpolatedStringHandler2.ToStringAndClear();
				}
				items2.Add(this.CreateContextMenuItem(header2, delegate(object _, RoutedEventArgs _)
				{
					this.SetSelectedModsActivation(false);
				}, true, null, null));
			}
			if (num3 > 0)
			{
				string text;
				if (list.Count != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3;
					defaultInterpolatedStringHandler3..ctor(19, 2);
					defaultInterpolatedStringHandler3.AppendLiteral("Update selected ");
					defaultInterpolatedStringHandler3.AppendFormatted((num3 == 1) ? "mod" : "mods");
					defaultInterpolatedStringHandler3.AppendLiteral(" (");
					defaultInterpolatedStringHandler3.AppendFormatted<int>(num3);
					defaultInterpolatedStringHandler3.AppendLiteral(")");
					text = defaultInterpolatedStringHandler3.ToStringAndClear();
				}
				else
				{
					text = "Update to latest";
				}
				string header3 = text;
				menu.Items.Add(this.CreateContextMenuItem(header3, new RoutedEventHandler(this.UpdateSelectedModsContextMenuItem_OnClick), !this._isModUpdateInProgress, null, null));
			}
			if (modListItemViewModel != null && modListItemViewModel.CanFixDependencyIssues)
			{
				menu.Items.Add(this.CreateModContextMenuItem("Resolve dependency issues", modListItemViewModel, new RoutedEventHandler(this.FixModButton_OnClick), !this._isModUpdateInProgress));
			}
			MainWindow.AddSeparatorIfNeeded(menu);
			if (modListItemViewModel != null)
			{
				string text2;
				bool flag = !string.IsNullOrWhiteSpace(modListItemViewModel.ModId) && this._userConfiguration.TryGetModConfigPath(modListItemViewModel.ModId, out text2) && !string.IsNullOrWhiteSpace(text2);
				menu.Items.Add(this.CreateModContextMenuItem(flag ? "Edit config" : "Set config...", modListItemViewModel, new RoutedEventHandler(this.EditConfigButton_OnClick), true));
				if (modListItemViewModel.HasModDatabasePageLink)
				{
					menu.Items.Add(this.CreateModContextMenuItem("Open mod page", modListItemViewModel, new RoutedEventHandler(this.ModDatabasePageButton_OnClick), true));
				}
				if (modListItemViewModel.CanSubmitUserReport && !InternetAccessManager.IsInternetAccessDisabled)
				{
					menu.Items.Add(this.CreateModContextMenuItem("View user reports...", modListItemViewModel, new RoutedEventHandler(this.UserReportsButton_OnClick), true));
				}
				if (this._userConfiguration.EnableServerOptions && !string.IsNullOrWhiteSpace(ServerCommandBuilder.TryBuildInstallCommand(modListItemViewModel.ModId, modListItemViewModel.Version)))
				{
					menu.Items.Add(this.CreateModContextMenuItem("Copy install command for server", modListItemViewModel, new RoutedEventHandler(this.SelectedModCopyForServerButton_OnClick), true));
				}
				if (!string.IsNullOrWhiteSpace(modListItemViewModel.ModId))
				{
					menu.Items.Add(this.CreateModContextMenuItem("Copy mod ID", modListItemViewModel, new RoutedEventHandler(this.CopyModIdContextMenuItem_OnClick), true));
				}
				menu.Items.Add(this.CreateModContextMenuItem("Open mod location", modListItemViewModel, new RoutedEventHandler(this.OpenModLocationContextMenuItem_OnClick), true));
			}
			if (list.Count > 0)
			{
				MainWindow.AddSeparatorIfNeeded(menu);
				ItemCollection items3 = menu.Items;
				string header4;
				if (list.Count != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4;
					defaultInterpolatedStringHandler4..ctor(23, 1);
					defaultInterpolatedStringHandler4.AppendLiteral("Remove selected mods (");
					defaultInterpolatedStringHandler4.AppendFormatted<int>(list.Count);
					defaultInterpolatedStringHandler4.AppendLiteral(")");
					header4 = defaultInterpolatedStringHandler4.ToStringAndClear();
				}
				else
				{
					header4 = "Remove mod";
				}
				items3.Add(this.CreateContextMenuItem(header4, new RoutedEventHandler(this.DeleteModButton_OnClick), true, (list.Count == 1) ? modListItemViewModel : null, "Delete"));
			}
			MainWindow.AddSeparatorIfNeeded(menu);
			menu.Items.Add(this.CreateContextMenuItem("Select all", delegate(object _, RoutedEventArgs _)
			{
				this.SelectAllModsInCurrentView();
			}, this.GetModsInViewOrder().Count > 0, null, "Ctrl+A"));
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00006EAC File Offset: 0x000050AC
		private void LocalModlistsDataGrid_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			DataGrid dataGrid = sender as DataGrid;
			if (dataGrid == null)
			{
				return;
			}
			DependencyObject source = e.OriginalSource as DependencyObject;
			Button button = MainWindow.FindAncestor<Button>(source);
			if (button != null && button.DataContext is ModlistGroupMemberListEntry)
			{
				return;
			}
			if (MainWindow.FindAncestor<DataGridColumnHeader>(source) != null || MainWindow.FindAncestor<ScrollBar>(source) != null)
			{
				return;
			}
			DataGridRow dataGridRow = MainWindow.FindAncestor<DataGridRow>(source);
			if (dataGridRow != null && dataGridRow.Item is LocalModlistListEntry)
			{
				if (!dataGridRow.IsSelected)
				{
					dataGrid.UnselectAll();
					dataGridRow.IsSelected = true;
				}
				dataGridRow.Focus();
				return;
			}
			dataGrid.UnselectAll();
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00006F34 File Offset: 0x00005134
		private void LocalModlistsDataGrid_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			Button button = MainWindow.FindAncestor<Button>(e.OriginalSource as DependencyObject);
			if (button != null && button.DataContext is ModlistGroupMemberListEntry)
			{
				return;
			}
			ContextMenu localModlistsContextMenu = this.LocalModlistsContextMenu;
			localModlistsContextMenu.Items.Clear();
			int count = this._selectedLocalModlists.Count;
			if (count == 0)
			{
				e.Handled = true;
				return;
			}
			if (count == 1)
			{
				LocalModlistListEntry localModlistListEntry = this._selectedLocalModlists[0];
				localModlistsContextMenu.Items.Add(this.CreateContextMenuItem(localModlistListEntry.IsGroup ? "Install modlist group" : "Install modlist", new RoutedEventHandler(this.InstallLocalModlistButton_OnClick), true, null, null));
				localModlistsContextMenu.Items.Add(this.CreateContextMenuItem(localModlistListEntry.IsGroup ? "Modify modlist group..." : "Modify modlist...", new RoutedEventHandler(this.ModifyLocalModlistButton_OnClick), true, null, null));
				if (localModlistListEntry.IsGroup)
				{
					ItemCollection items = localModlistsContextMenu.Items;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Fix duplicate mods (");
					defaultInterpolatedStringHandler.AppendFormatted<int>(localModlistListEntry.GroupConflicts.Count);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					items.Add(this.CreateContextMenuItem(defaultInterpolatedStringHandler.ToStringAndClear(), new RoutedEventHandler(this.FixDuplicateModsButton_OnClick), localModlistListEntry.GroupConflicts.Count > 0, null, null));
				}
			}
			ItemCollection items2 = localModlistsContextMenu.Items;
			string header;
			if (count <= 1)
			{
				header = (this._selectedLocalModlists[0].IsGroup ? "Delete modlist group" : "Delete modlist");
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(34, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("Delete selected modlists/groups (");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(count);
				defaultInterpolatedStringHandler2.AppendLiteral(")");
				header = defaultInterpolatedStringHandler2.ToStringAndClear();
			}
			items2.Add(this.CreateContextMenuItem(header, new RoutedEventHandler(this.DeleteLocalModlistsButton_OnClick), true, null, null));
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000070F4 File Offset: 0x000052F4
		private void CloudModlistsDataGrid_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			DataGrid dataGrid = sender as DataGrid;
			if (dataGrid == null)
			{
				return;
			}
			DependencyObject source = e.OriginalSource as DependencyObject;
			if (MainWindow.FindAncestor<DataGridColumnHeader>(source) != null || MainWindow.FindAncestor<ScrollBar>(source) != null)
			{
				return;
			}
			DataGridRow dataGridRow = MainWindow.FindAncestor<DataGridRow>(source);
			if (dataGridRow != null && dataGridRow.Item is CloudModlistListEntry)
			{
				dataGrid.SelectedItem = dataGridRow.Item;
				dataGridRow.Focus();
				return;
			}
			dataGrid.UnselectAll();
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000715C File Offset: 0x0000535C
		private void CloudModlistsDataGrid_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			ContextMenu cloudModlistsContextMenu = this.CloudModlistsContextMenu;
			cloudModlistsContextMenu.Items.Clear();
			bool flag = !InternetAccessManager.IsInternetAccessDisabled;
			cloudModlistsContextMenu.Items.Add(this.CreateContextMenuItem("Refresh online modlists", new RoutedEventHandler(this.RefreshCloudModlistsButton_OnClick), flag && !this._isCloudModlistRefreshInProgress, null, null));
			cloudModlistsContextMenu.Items.Add(new Separator());
			cloudModlistsContextMenu.Items.Add(this.CreateContextMenuItem("Install selected modlist", new RoutedEventHandler(this.InstallCloudModlistButton_OnClick), flag && this._selectedCloudModlist != null, null, null));
			cloudModlistsContextMenu.Items.Add(this.CreateContextMenuItem("Add to your local modlists", new RoutedEventHandler(this.AddCloudModlistToLocal_OnClick), flag && this._selectedCloudModlist != null, null, null));
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00007238 File Offset: 0x00005438
		private MenuItem CreateInstalledColumnsSubmenu()
		{
			MenuItem menuItem = new MenuItem
			{
				Header = "Columns",
				MinWidth = 220.0,
				Style = (base.TryFindResource("ToggleMenuItemStyle") as Style)
			};
			Style style = base.TryFindResource("ToggleMenuItemStyle") as Style;
			foreach (ValueTuple<MainWindow.InstalledModsColumn, string> valueTuple in MainWindow.GetInstalledColumns())
			{
				MainWindow.InstalledModsColumn column = valueTuple.Item1;
				string item = valueTuple.Item2;
				MenuItem columnMenuItem = new MenuItem
				{
					Header = item,
					IsCheckable = true,
					IsChecked = CollectionExtensions.GetValueOrDefault<MainWindow.InstalledModsColumn, bool>(this._installedColumnVisibilityPreferences, column, true),
					MinHeight = 32.0,
					Style = style,
					StaysOpenOnClick = true
				};
				columnMenuItem.Click += delegate(object _, RoutedEventArgs _)
				{
					this.SetInstalledColumnVisibility(column, columnMenuItem.IsChecked);
				};
				menuItem.Items.Add(columnMenuItem);
			}
			return menuItem;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00007368 File Offset: 0x00005568
		private MenuItem CreateContextMenuItem(string header, RoutedEventHandler clickHandler, bool isEnabled = true, [Nullable(2)] object dataContext = null, [Nullable(2)] string inputGestureText = null)
		{
			MenuItem menuItem = new MenuItem
			{
				Header = header,
				IsEnabled = isEnabled,
				MinHeight = 32.0,
				DataContext = dataContext,
				Style = (base.TryFindResource("ToggleMenuItemStyle") as Style)
			};
			if (!string.IsNullOrWhiteSpace(inputGestureText))
			{
				menuItem.InputGestureText = inputGestureText;
			}
			menuItem.Click += clickHandler;
			return menuItem;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000073CF File Offset: 0x000055CF
		private MenuItem CreateModContextMenuItem(string header, ModListItemViewModel mod, RoutedEventHandler clickHandler, bool isEnabled = true)
		{
			return this.CreateContextMenuItem(header, clickHandler, isEnabled, mod, null);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x000073DD File Offset: 0x000055DD
		private static void AddSeparatorIfNeeded(ContextMenu menu)
		{
			if (menu.Items.Count > 0)
			{
				ItemCollection items = menu.Items;
				if (!(items[items.Count - 1] is Separator))
				{
					menu.Items.Add(new Separator());
				}
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00007418 File Offset: 0x00005618
		private void SetSelectedModsActivation(bool isActive)
		{
			if (this._selectedMods.Count == 0)
			{
				return;
			}
			try
			{
				this._isApplyingMultiToggle = true;
				foreach (ModListItemViewModel modListItemViewModel in Enumerable.ToList<ModListItemViewModel>(this._selectedMods))
				{
					if (modListItemViewModel.CanToggle && modListItemViewModel.IsActive != isActive)
					{
						modListItemViewModel.IsActive = isActive;
					}
				}
			}
			finally
			{
				this._isApplyingMultiToggle = false;
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x000074AC File Offset: 0x000056AC
		private void UpdateSelectedModsContextMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<UpdateSelectedModsContextMenuItem_OnClick>d__151 <UpdateSelectedModsContextMenuItem_OnClick>d__;
			<UpdateSelectedModsContextMenuItem_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<UpdateSelectedModsContextMenuItem_OnClick>d__.<>4__this = this;
			<UpdateSelectedModsContextMenuItem_OnClick>d__.e = e;
			<UpdateSelectedModsContextMenuItem_OnClick>d__.<>1__state = -1;
			<UpdateSelectedModsContextMenuItem_OnClick>d__.<>t__builder.Start<MainWindow.<UpdateSelectedModsContextMenuItem_OnClick>d__151>(ref <UpdateSelectedModsContextMenuItem_OnClick>d__);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000074EC File Offset: 0x000056EC
		private void CopyModIdContextMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement != null)
			{
				ModListItemViewModel modListItemViewModel = frameworkElement.DataContext as ModListItemViewModel;
				if (modListItemViewModel != null && !string.IsNullOrWhiteSpace(modListItemViewModel.ModId))
				{
					try
					{
						Clipboard.SetDataObject(modListItemViewModel.ModId, true, 10, 100);
						MainViewModel viewModel = this._viewModel;
						if (viewModel != null)
						{
							viewModel.ReportStatus("Copied mod ID " + modListItemViewModel.ModId + ".", false);
						}
					}
					catch (ExternalException ex)
					{
						MainViewModel viewModel2 = this._viewModel;
						if (viewModel2 != null)
						{
							viewModel2.ReportStatus("Failed to copy mod ID for " + modListItemViewModel.DisplayName + ": " + ex.Message, true);
						}
						ModManagerMessageBox.Show(this, "Failed to copy the mod ID. Please try again.", "Simple VS Manager", 0, 16, null, null, null);
					}
					return;
				}
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000075B4 File Offset: 0x000057B4
		private void OpenModLocationContextMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement != null)
			{
				ModListItemViewModel modListItemViewModel = frameworkElement.DataContext as ModListItemViewModel;
				if (modListItemViewModel != null)
				{
					string text;
					string text2;
					if (!this.TryGetManagedModPath(modListItemViewModel, out text, out text2))
					{
						ModManagerMessageBox.Show(this, text2 ?? "The mod location could not be determined.", "Simple VS Manager", 0, 64, null, null, null);
						return;
					}
					if (!File.Exists(text) && !Directory.Exists(text))
					{
						ModManagerMessageBox.Show(this, "The mod could not be found at:" + Environment.NewLine + text, "Simple VS Manager", 0, 64, null, null, null);
						return;
					}
					try
					{
						MainWindow.RevealPathWithShell(text);
					}
					catch (Exception ex)
					{
						ModManagerMessageBox.Show(this, "Failed to open the mod location:" + Environment.NewLine + ex.Message, "Simple VS Manager", 0, 16, null, null, null);
					}
					return;
				}
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00007680 File Offset: 0x00005880
		private static void RevealPathWithShell(string path)
		{
			if (OperatingSystem.IsWindows())
			{
				ProcessStartInfo processStartInfo = new ProcessStartInfo();
				processStartInfo.FileName = "explorer.exe";
				processStartInfo.UseShellExecute = true;
				processStartInfo.ArgumentList.Add("/select,");
				processStartInfo.ArgumentList.Add(path);
				Process.Start(processStartInfo);
				return;
			}
			string text = Directory.Exists(path) ? path : Path.GetDirectoryName(path);
			if (!string.IsNullOrWhiteSpace(text))
			{
				MainWindow.OpenFolderWithShell(text);
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x000076F0 File Offset: 0x000058F0
		private void SetInstalledColumnVisibility(MainWindow.InstalledModsColumn column, bool isVisible)
		{
			MenuItem installedColumnMenuItem = this.GetInstalledColumnMenuItem(column);
			if (installedColumnMenuItem != null && installedColumnMenuItem.IsChecked != isVisible)
			{
				installedColumnMenuItem.IsChecked = isVisible;
				return;
			}
			this._installedColumnVisibilityPreferences[column] = isVisible;
			this._userConfiguration.SetInstalledColumnVisibility(column.ToString(), isVisible);
			this.NotifyViewModelOfInstalledColumnVisibility(column, isVisible);
			this.ApplyInstalledColumnVisibility(column, isVisible);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00007750 File Offset: 0x00005950
		[NullableContext(2)]
		private MenuItem GetInstalledColumnMenuItem(MainWindow.InstalledModsColumn column)
		{
			switch (column)
			{
			case MainWindow.InstalledModsColumn.Active:
				return this.ActiveColumnMenuItem;
			case MainWindow.InstalledModsColumn.Icon:
				return this.IconColumnMenuItem;
			case MainWindow.InstalledModsColumn.Name:
				return this.NameColumnMenuItem;
			case MainWindow.InstalledModsColumn.Version:
				return this.VersionColumnMenuItem;
			case MainWindow.InstalledModsColumn.LatestVersion:
				return this.LatestVersionColumnMenuItem;
			case MainWindow.InstalledModsColumn.DateModified:
				return this.DateModifiedColumnMenuItem;
			case MainWindow.InstalledModsColumn.Authors:
				return this.AuthorsColumnMenuItem;
			case MainWindow.InstalledModsColumn.Tags:
				return this.TagsColumnMenuItem;
			case MainWindow.InstalledModsColumn.UserReports:
				return this.UserReportsColumnMenuItem;
			case MainWindow.InstalledModsColumn.Status:
				return this.StatusColumnMenuItem;
			case MainWindow.InstalledModsColumn.Side:
				return this.SideColumnMenuItem;
			}
			return null;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00007800 File Offset: 0x00005A00
		private bool TryGetInstalledColumn(DataGridColumn column, out MainWindow.InstalledModsColumn columnKey)
		{
			if (column == this.ActiveColumn)
			{
				columnKey = MainWindow.InstalledModsColumn.Active;
			}
			else if (column == this.IconColumn)
			{
				columnKey = MainWindow.InstalledModsColumn.Icon;
			}
			else if (column == this.NameColumn)
			{
				columnKey = MainWindow.InstalledModsColumn.Name;
			}
			else if (column == this.VersionColumn)
			{
				columnKey = MainWindow.InstalledModsColumn.Version;
			}
			else if (column == this.LatestVersionColumn)
			{
				columnKey = MainWindow.InstalledModsColumn.LatestVersion;
			}
			else if (column == this.DateModifiedColumn)
			{
				columnKey = MainWindow.InstalledModsColumn.DateModified;
			}
			else if (column == this.AuthorsColumn)
			{
				columnKey = MainWindow.InstalledModsColumn.Authors;
			}
			else if (column == this.TagsColumn)
			{
				columnKey = MainWindow.InstalledModsColumn.Tags;
			}
			else if (column == this.UserReportsColumn)
			{
				columnKey = MainWindow.InstalledModsColumn.UserReports;
			}
			else if (column == this.StatusColumn)
			{
				columnKey = MainWindow.InstalledModsColumn.Status;
			}
			else
			{
				if (column != this.SideColumn)
				{
					columnKey = MainWindow.InstalledModsColumn.Active;
					return false;
				}
				columnKey = MainWindow.InstalledModsColumn.Side;
			}
			return true;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x000078B8 File Offset: 0x00005AB8
		[return: TupleElementNames(new string[]
		{
			"Column",
			"Label"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		private static IReadOnlyList<ValueTuple<MainWindow.InstalledModsColumn, string>> GetInstalledColumns()
		{
			return new <>z__ReadOnlyArray<ValueTuple<MainWindow.InstalledModsColumn, string>>(new ValueTuple<MainWindow.InstalledModsColumn, string>[]
			{
				new ValueTuple<MainWindow.InstalledModsColumn, string>(MainWindow.InstalledModsColumn.Active, "Active"),
				new ValueTuple<MainWindow.InstalledModsColumn, string>(MainWindow.InstalledModsColumn.Icon, "Icon"),
				new ValueTuple<MainWindow.InstalledModsColumn, string>(MainWindow.InstalledModsColumn.Name, "Name"),
				new ValueTuple<MainWindow.InstalledModsColumn, string>(MainWindow.InstalledModsColumn.Version, "Version"),
				new ValueTuple<MainWindow.InstalledModsColumn, string>(MainWindow.InstalledModsColumn.LatestVersion, "Latest Version"),
				new ValueTuple<MainWindow.InstalledModsColumn, string>(MainWindow.InstalledModsColumn.DateModified, "Date Modified"),
				new ValueTuple<MainWindow.InstalledModsColumn, string>(MainWindow.InstalledModsColumn.Authors, "Authors"),
				new ValueTuple<MainWindow.InstalledModsColumn, string>(MainWindow.InstalledModsColumn.Tags, "Tags"),
				new ValueTuple<MainWindow.InstalledModsColumn, string>(MainWindow.InstalledModsColumn.UserReports, "User Reports"),
				new ValueTuple<MainWindow.InstalledModsColumn, string>(MainWindow.InstalledModsColumn.Status, "Status"),
				new ValueTuple<MainWindow.InstalledModsColumn, string>(MainWindow.InstalledModsColumn.Side, "Side")
			});
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000079A0 File Offset: 0x00005BA0
		private static string GetInstalledColumnLabel(MainWindow.InstalledModsColumn column)
		{
			return Enumerable.First<ValueTuple<MainWindow.InstalledModsColumn, string>>(MainWindow.GetInstalledColumns(), ([TupleElementNames(new string[]
			{
				"Column",
				"Label"
			})] ValueTuple<MainWindow.InstalledModsColumn, string> entry) => entry.Item1 == column).Item2;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000079D8 File Offset: 0x00005BD8
		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			MainWindow.<MainWindow_Loaded>d__160 <MainWindow_Loaded>d__;
			<MainWindow_Loaded>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<MainWindow_Loaded>d__.<>4__this = this;
			<MainWindow_Loaded>d__.<>1__state = -1;
			<MainWindow_Loaded>d__.<>t__builder.Start<MainWindow.<MainWindow_Loaded>d__160>(ref <MainWindow_Loaded>d__);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00007A10 File Offset: 0x00005C10
		private void MigrateLegacyRebuiltModlistsIfNeeded()
		{
			if (this._userConfiguration.RebuiltModlistMigrationCompleted)
			{
				return;
			}
			try
			{
				string text = this.EnsureModListDirectory();
				string text2 = Path.Combine(text, MainWindow.RebuiltModListDirectoryName);
				Directory.CreateDirectory(text2);
				bool flag = false;
				foreach (string text3 in Directory.EnumerateFileSystemEntries(text, "Rebuilt_*", 0))
				{
					if (Directory.Exists(text3))
					{
						string text4 = Path.Combine(text2, Path.GetFileName(text3));
						text4 = MainWindow.EnsureUniqueDirectoryPath(text4);
						Directory.Move(text3, text4);
						flag = true;
					}
					else if (File.Exists(text3))
					{
						string text5 = Path.Combine(text2, Path.GetFileName(text3));
						text5 = MainWindow.EnsureUniqueFilePath(text5);
						File.Move(text3, text5);
						flag = true;
					}
				}
				if (flag)
				{
					MainViewModel viewModel = this._viewModel;
					if (viewModel != null)
					{
						viewModel.ReportStatus("Moved rebuilt modlists into \"" + MainWindow.RebuiltModListDirectoryName + "\" folder.", false);
					}
				}
				this._userConfiguration.SetRebuiltModlistMigrationCompleted();
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
			{
				this._modActivityLoggingService.LogError("Failed to prepare the Rebuilt modlists folder", ex);
				ModManagerMessageBox.Show("Failed to prepare the Rebuilt modlists folder:\n" + ex.Message, "Simple VS Manager", 0, 48, null, null, null);
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00007B84 File Offset: 0x00005D84
		private Task PromptCacheRefreshIfNeededAsync()
		{
			MainWindow.<PromptCacheRefreshIfNeededAsync>d__162 <PromptCacheRefreshIfNeededAsync>d__;
			<PromptCacheRefreshIfNeededAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<PromptCacheRefreshIfNeededAsync>d__.<>4__this = this;
			<PromptCacheRefreshIfNeededAsync>d__.<>1__state = -1;
			<PromptCacheRefreshIfNeededAsync>d__.<>t__builder.Start<MainWindow.<PromptCacheRefreshIfNeededAsync>d__162>(ref <PromptCacheRefreshIfNeededAsync>d__);
			return <PromptCacheRefreshIfNeededAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00007BC8 File Offset: 0x00005DC8
		private Task ClearManagerCachesForVersionUpdateAsync()
		{
			MainWindow.<ClearManagerCachesForVersionUpdateAsync>d__163 <ClearManagerCachesForVersionUpdateAsync>d__;
			<ClearManagerCachesForVersionUpdateAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ClearManagerCachesForVersionUpdateAsync>d__.<>4__this = this;
			<ClearManagerCachesForVersionUpdateAsync>d__.<>1__state = -1;
			<ClearManagerCachesForVersionUpdateAsync>d__.<>t__builder.Start<MainWindow.<ClearManagerCachesForVersionUpdateAsync>d__163>(ref <ClearManagerCachesForVersionUpdateAsync>d__);
			return <ClearManagerCachesForVersionUpdateAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00007C0C File Offset: 0x00005E0C
		private Task CheckAndPromptMigrationAsync()
		{
			MainWindow.<CheckAndPromptMigrationAsync>d__164 <CheckAndPromptMigrationAsync>d__;
			<CheckAndPromptMigrationAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CheckAndPromptMigrationAsync>d__.<>4__this = this;
			<CheckAndPromptMigrationAsync>d__.<>1__state = -1;
			<CheckAndPromptMigrationAsync>d__.<>t__builder.Start<MainWindow.<CheckAndPromptMigrationAsync>d__164>(ref <CheckAndPromptMigrationAsync>d__);
			return <CheckAndPromptMigrationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00007C50 File Offset: 0x00005E50
		private Task PerformMigrationAsync()
		{
			MainWindow.<>c__DisplayClass165_0 CS$<>8__locals1 = new MainWindow.<>c__DisplayClass165_0();
			StackPanel stackPanel = new StackPanel();
			stackPanel.VerticalAlignment = 1;
			stackPanel.HorizontalAlignment = 1;
			TextBlock textBlock = new TextBlock();
			textBlock.Text = "Migrating configuration and cache files...";
			textBlock.FontSize = 14.0;
			textBlock.Margin = new Thickness(20.0);
			textBlock.TextAlignment = 2;
			ProgressBar progressBar = new ProgressBar();
			progressBar.IsIndeterminate = true;
			progressBar.Width = 300.0;
			progressBar.Height = 20.0;
			progressBar.Margin = new Thickness(20.0);
			stackPanel.Children.Add(textBlock);
			stackPanel.Children.Add(progressBar);
			CS$<>8__locals1.progressWindow = new Window();
			CS$<>8__locals1.progressWindow.Title = "Migrating Configuration";
			CS$<>8__locals1.progressWindow.Width = 400.0;
			CS$<>8__locals1.progressWindow.Height = 150.0;
			CS$<>8__locals1.progressWindow.WindowStartupLocation = 2;
			CS$<>8__locals1.progressWindow.Owner = this;
			CS$<>8__locals1.progressWindow.ResizeMode = 0;
			CS$<>8__locals1.progressWindow.WindowStyle = 3;
			CS$<>8__locals1.progressWindow.Content = stackPanel;
			CS$<>8__locals1.migrationSuccess = false;
			CS$<>8__locals1.progressWindow.Loaded += delegate(object s, RoutedEventArgs e)
			{
				MainWindow.<>c__DisplayClass165_0.<<PerformMigrationAsync>b__0>d <<PerformMigrationAsync>b__0>d;
				<<PerformMigrationAsync>b__0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
				<<PerformMigrationAsync>b__0>d.<>4__this = CS$<>8__locals1;
				<<PerformMigrationAsync>b__0>d.<>1__state = -1;
				<<PerformMigrationAsync>b__0>d.<>t__builder.Start<MainWindow.<>c__DisplayClass165_0.<<PerformMigrationAsync>b__0>d>(ref <<PerformMigrationAsync>b__0>d);
			};
			CS$<>8__locals1.progressWindow.ShowDialog();
			if (CS$<>8__locals1.migrationSuccess)
			{
				ModManagerMessageBox.Show(this, "Configuration and cache files have been successfully migrated to the new location.", "Simple VS Manager", 0, 64, null, null, null);
			}
			else
			{
				ModManagerMessageBox.Show(this, "Migration could not be completed. Starting with default settings.\n\nYou can manually copy files from Documents\\Simple VS Manager to AppData\\Local\\Simple VS Manager if needed.", "Simple VS Manager", 0, 48, null, null, null);
			}
			return Task.CompletedTask;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00007DF4 File Offset: 0x00005FF4
		private Task TryShowModUsagePromptAsync()
		{
			if (this._isModUsageDialogOpen)
			{
				return Task.CompletedTask;
			}
			if (this._viewModel == null || !this._userConfiguration.IsModUsageTrackingEnabled)
			{
				this._modUsagePromptData = null;
				this.UpdateModUsagePromptIndicator(false);
				return Task.CompletedTask;
			}
			if (!this._userConfiguration.HasPendingModUsagePrompt)
			{
				this._modUsagePromptData = null;
				this.UpdateModUsagePromptIndicator(false);
				return Task.CompletedTask;
			}
			MainWindow.ModUsagePromptData modUsagePromptData = this.PrepareModUsagePromptData();
			if (modUsagePromptData == null || modUsagePromptData.Candidates.Count == 0)
			{
				this._modUsagePromptData = null;
				this.UpdateModUsagePromptIndicator(false);
				GameSessionMonitor gameSessionMonitor = this._gameSessionMonitor;
				if (gameSessionMonitor != null)
				{
					gameSessionMonitor.RefreshPromptState();
				}
				return Task.CompletedTask;
			}
			MainWindow.ModUsagePromptData modUsagePromptData2 = this._modUsagePromptData;
			this._modUsagePromptData = modUsagePromptData;
			this.UpdateModUsagePromptIndicator(true);
			if (modUsagePromptData.SkippedCount > 0 && (modUsagePromptData2 == null || modUsagePromptData2.SkippedCount != modUsagePromptData.SkippedCount || modUsagePromptData2.Candidates.Count != modUsagePromptData.Candidates.Count))
			{
				StatusLogService.AppendStatus(string.Format(CultureInfo.CurrentCulture, "Skipped {0} mod(s) that cannot receive automatic votes.", modUsagePromptData.SkippedCount), false);
			}
			return Task.CompletedTask;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00007F0C File Offset: 0x0000610C
		[NullableContext(2)]
		private MainWindow.ModUsagePromptData PrepareModUsagePromptData()
		{
			if (this._viewModel == null || !this._userConfiguration.IsModUsageTrackingEnabled)
			{
				return null;
			}
			IReadOnlyDictionary<ModUsageTrackingKey, int> pendingModUsageCounts = this._userConfiguration.GetPendingModUsageCounts();
			if (pendingModUsageCounts.Count == 0)
			{
				this._userConfiguration.ResetModUsageTracking();
				return null;
			}
			string text = this._viewModel.InstalledGameVersion;
			if (string.IsNullOrWhiteSpace(text))
			{
				this._userConfiguration.ResetModUsageTracking();
				return null;
			}
			text = text.Trim();
			List<ModUsageVoteCandidateViewModel> list = new List<ModUsageVoteCandidateViewModel>();
			List<ModUsageTrackingKey> list2 = new List<ModUsageTrackingKey>();
			List<ModUsageTrackingKey> list3 = new List<ModUsageTrackingKey>();
			int num = 0;
			foreach (KeyValuePair<ModUsageTrackingKey, int> keyValuePair in Enumerable.OrderByDescending<KeyValuePair<ModUsageTrackingKey, int>, int>(pendingModUsageCounts, (KeyValuePair<ModUsageTrackingKey, int> pair) => pair.Value))
			{
				ModUsageTrackingKey key = keyValuePair.Key;
				if (!key.IsValid)
				{
					list3.Add(key);
				}
				else
				{
					ModListItemViewModel modListItemViewModel = this._viewModel.FindInstalledModById(key.ModId);
					if (modListItemViewModel == null)
					{
						list3.Add(key);
						num++;
					}
					else if (!modListItemViewModel.CanSubmitUserReport)
					{
						list3.Add(key);
						num++;
					}
					else
					{
						string version = modListItemViewModel.Version;
						if (string.IsNullOrWhiteSpace(version) || !string.Equals(version.Trim(), key.ModVersion, 5))
						{
							list3.Add(key);
							num++;
						}
						else if (!string.Equals(text, key.GameVersion, 5))
						{
							list3.Add(key);
							num++;
						}
						else if (modListItemViewModel.UserVoteOption != null)
						{
							list3.Add(key);
						}
						else
						{
							list.Add(new ModUsageVoteCandidateViewModel(modListItemViewModel, keyValuePair.Value, key));
							list2.Add(key);
						}
					}
				}
			}
			if (list3.Count > 0)
			{
				this._userConfiguration.ResetModUsageCounts(list3);
			}
			if (list.Count == 0)
			{
				if (num > 0)
				{
					StatusLogService.AppendStatus("No mods were eligible for automatic \"No issues\" votes.", false);
				}
				if (this._userConfiguration.GetPendingModUsageCounts().Count == 0)
				{
					this._userConfiguration.ResetModUsageTracking();
				}
				return null;
			}
			return new MainWindow.ModUsagePromptData(list, list2, num);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00008154 File Offset: 0x00006354
		private void UpdateModUsagePromptIndicator(bool isVisible)
		{
			if (this.ModUsagePromptTextBlock == null)
			{
				return;
			}
			this.ModUsagePromptTextBlock.Visibility = (isVisible ? 0 : 2);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00008174 File Offset: 0x00006374
		private Task ShowModUsagePromptDialogAsync()
		{
			MainWindow.<ShowModUsagePromptDialogAsync>d__169 <ShowModUsagePromptDialogAsync>d__;
			<ShowModUsagePromptDialogAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ShowModUsagePromptDialogAsync>d__.<>4__this = this;
			<ShowModUsagePromptDialogAsync>d__.<>1__state = -1;
			<ShowModUsagePromptDialogAsync>d__.<>t__builder.Start<MainWindow.<ShowModUsagePromptDialogAsync>d__169>(ref <ShowModUsagePromptDialogAsync>d__);
			return <ShowModUsagePromptDialogAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x000081B8 File Offset: 0x000063B8
		private Task EnsureInstalledModsCachedAsync(MainViewModel viewModel, bool ignoreUserSetting = false)
		{
			if (viewModel == null || (!this._userConfiguration.CacheAllVersionsLocally && !ignoreUserSetting))
			{
				return Task.CompletedTask;
			}
			IReadOnlyList<ModListItemViewModel> installedMods = viewModel.GetInstalledModsSnapshot();
			if (installedMods.Count == 0)
			{
				return Task.CompletedTask;
			}
			return Task.Run(delegate()
			{
				foreach (ModListItemViewModel modListItemViewModel in installedMods)
				{
					if (modListItemViewModel != null && modListItemViewModel.IsInstalled)
					{
						ModCacheService.EnsureModCached(modListItemViewModel.ModId, modListItemViewModel.Version, modListItemViewModel.SourcePath, modListItemViewModel.SourceKind);
					}
				}
			});
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00008214 File Offset: 0x00006414
		private void MainWindow_OnClosing([Nullable(2)] object sender, CancelEventArgs e)
		{
			if (!this._isApplyingPreset)
			{
				MainViewModel viewModel = this._viewModel;
				if (viewModel == null || !viewModel.IsLoadingMods)
				{
					goto IL_3C;
				}
			}
			if (ModManagerMessageBox.Show("A modlist is still being applied. Exiting now may leave some mods missing or disabled. Do you want to exit anyway?", "Simple VS Manager", 4, 48, null, null, null) != 6)
			{
				e.Cancel = true;
				return;
			}
			IL_3C:
			this.SaveWindowDimensions();
			this.SaveUploaderName();
			if (this._viewModel != null && this._userConfiguration.LogErrorsAndExceptions)
			{
				string timingSummary = this._viewModel.TimingService.GetTimingSummary();
				this._modActivityLoggingService.LogModLoadingTimingSummary(timingSummary);
			}
			this._modActivityLoggingService.LogAppExit();
			if (this._traceListener != null)
			{
				Trace.Listeners.Remove(this._traceListener);
				this._traceListener.Dispose();
				this._traceListener = null;
			}
			this.DisposeCurrentViewModel();
			InternetAccessManager.InternetAccessChanged -= new EventHandler(this.InternetAccessManager_OnInternetAccessChanged);
			DeveloperProfileManager.CurrentProfileChanged -= new EventHandler<DeveloperProfileChangedEventArgs>(this.DeveloperProfileManager_OnCurrentProfileChanged);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000082F8 File Offset: 0x000064F8
		private void InitializeModInfoPane()
		{
			this.ApplyModInfoPanePlacement(this._userConfiguration.ModInfoPanePlacement);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000830C File Offset: 0x0000650C
		private void ModInfoPanePlacementButton_OnClick(object sender, RoutedEventArgs e)
		{
			ModInfoPanePlacement modInfoPanePlacement = (this._userConfiguration.ModInfoPanePlacement == ModInfoPanePlacement.Left) ? ModInfoPanePlacement.Right : ModInfoPanePlacement.Left;
			this._userConfiguration.SetModInfoPanePlacement(modInfoPanePlacement);
			this.ApplyModInfoPanePlacement(modInfoPanePlacement);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00008340 File Offset: 0x00006540
		private void ApplyModInfoPanePlacement(ModInfoPanePlacement placement)
		{
			bool flag = placement == ModInfoPanePlacement.Left;
			DockPanel.SetDock(this.ModInfoPaneRoot, flag ? 0 : 2);
			this.ModInfoPanePlacementButton.Content = (flag ? "→" : "←");
			this.ModInfoPanePlacementButton.ToolTip = (flag ? "Move details pane to the right" : "Move details pane to the left");
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00008398 File Offset: 0x00006598
		private void DisableAutoRefreshMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			bool isChecked = menuItem.IsChecked;
			if (isChecked && !this._userConfiguration.DisableAutoRefreshWarningAcknowledged)
			{
				if (ModManagerMessageBox.Show("This will disable automatic refresh functions such as update checks, loading of tags and other mod details, user reports and other similar functions." + Environment.NewLine + Environment.NewLine + "This will decrease loading times on start for example. Use the \"Refresh\" button to choose when you want to fetch details from cache and/or Mod DB. This dialog will not be shown again.", "Simple VS Manager", 4, 48, null, null, null) != 6)
				{
					menuItem.IsChecked = false;
					return;
				}
				this._userConfiguration.SetDisableAutoRefreshWarningAcknowledged(true);
			}
			this._userConfiguration.SetDisableAutoRefresh(isChecked);
			MainViewModel viewModel = this._viewModel;
			if (viewModel == null)
			{
				return;
			}
			viewModel.SetAutoRefreshDisabled(isChecked);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00008424 File Offset: 0x00006624
		private void AutomaticDataBackupsMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			if (menuItem.IsChecked && (string.IsNullOrWhiteSpace(this._dataDirectory) || !Directory.Exists(this._dataDirectory)))
			{
				ModManagerMessageBox.Show("Please configure a valid VintagestoryData folder before enabling automatic backups.", "Simple VS Manager", 0, 48, null, null, null);
				menuItem.IsChecked = false;
				return;
			}
			if (menuItem.IsChecked && !this._userConfiguration.AutomaticDataBackupsWarningAcknowledged)
			{
				ModManagerMessageBox.Show("This is an experimental feature and will increase your start up time depending on your files. Depending on your mods, saves and other files it could also end up taking up a lot of disk space.\n\nYou can set the maximum number of backups in the menu, the oldest one will be deleted. Remember to launch with the \"Launch Vintage Story\" button, it backups at launch ONLY.", "Simple VS Manager", 0, 64, null, null, null);
				this._userConfiguration.SetAutomaticDataBackupsWarningAcknowledged(true);
			}
			this._userConfiguration.SetAutomaticDataBackupsEnabled(menuItem.IsChecked);
			menuItem.IsChecked = this._userConfiguration.AutomaticDataBackupsEnabled;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x000084D8 File Offset: 0x000066D8
		private void CloseManagerAfterGameLaunchMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			this._userConfiguration.SetCloseManagerAfterGameLaunch(menuItem.IsChecked);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00008504 File Offset: 0x00006704
		private void DisableInternetAccessMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			bool isChecked = menuItem.IsChecked;
			InternetAccessManager.SetInternetAccessDisabled(isChecked);
			this._userConfiguration.SetDisableInternetAccess(isChecked);
			MainViewModel viewModel = this._viewModel;
			if (viewModel == null)
			{
				return;
			}
			viewModel.OnInternetAccessStateChanged();
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00008548 File Offset: 0x00006748
		private void ThemeMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			string text = menuItem.Tag as string;
			if (text != null)
			{
				if (!this._userConfiguration.TryActivateTheme(text))
				{
					return;
				}
				IReadOnlyDictionary<string, string> themePaletteColors = this._userConfiguration.GetThemePaletteColors();
				this.UpdateThemeMenuSelection(this._userConfiguration.ColorTheme, text);
				App.ApplyTheme(this._userConfiguration.ColorTheme, (themePaletteColors.Count > 0) ? themePaletteColors : null);
				this.ClearScrollViewerCache();
				return;
			}
			else
			{
				object tag = menuItem.Tag;
				if (!(tag is ColorTheme))
				{
					return;
				}
				ColorTheme colorTheme = (ColorTheme)tag;
				ColorTheme colorTheme2 = this._userConfiguration.ColorTheme;
				IReadOnlyDictionary<string, string> readOnlyDictionary = null;
				if (colorTheme == ColorTheme.SurpriseMe)
				{
					readOnlyDictionary = MainWindow.GenerateSurprisePalette();
				}
				if (colorTheme == colorTheme2 && readOnlyDictionary == null)
				{
					this.UpdateThemeMenuSelection(colorTheme2, this._userConfiguration.GetCurrentThemeName());
					return;
				}
				this.UpdateThemeMenuSelection(colorTheme, UserConfigurationService.GetThemeDisplayName(colorTheme));
				this._userConfiguration.SetColorTheme(colorTheme, readOnlyDictionary);
				IReadOnlyDictionary<string, string> themePaletteColors2 = this._userConfiguration.GetThemePaletteColors();
				App.ApplyTheme(colorTheme, (themePaletteColors2.Count > 0) ? themePaletteColors2 : null);
				this.ClearScrollViewerCache();
				return;
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00008657 File Offset: 0x00006857
		private void EditThemeMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			this.ShowCustomThemeEditor();
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00008660 File Offset: 0x00006860
		[NullableContext(2)]
		private void UpdateThemeMenuSelection(ColorTheme theme, string themeName = null)
		{
			if (this.VintageStoryThemeMenuItem != null)
			{
				this.VintageStoryThemeMenuItem.IsChecked = (theme == ColorTheme.VintageStory);
			}
			if (this.DarkThemeMenuItem != null)
			{
				this.DarkThemeMenuItem.IsChecked = (theme == ColorTheme.Dark);
			}
			if (this.LightThemeMenuItem != null)
			{
				this.LightThemeMenuItem.IsChecked = (theme == ColorTheme.Light);
			}
			string text = themeName ?? this._userConfiguration.GetCurrentThemeName();
			foreach (MenuItem menuItem in this._customThemeMenuItems)
			{
				object header = menuItem.Header;
				string text2 = (header != null) ? header.ToString() : null;
				menuItem.IsChecked = (theme == ColorTheme.Custom && !string.IsNullOrWhiteSpace(text2) && string.Equals(text2, text, 5));
			}
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00008730 File Offset: 0x00006930
		private static IReadOnlyDictionary<string, string> GenerateSurprisePalette()
		{
			IEnumerable<KeyValuePair<string, string>> defaultThemePalette = UserConfigurationService.GetDefaultThemePalette(ColorTheme.VintageStory);
			Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			foreach (KeyValuePair<string, string> keyValuePair in defaultThemePalette)
			{
				dictionary[keyValuePair.Key] = MainWindow.GenerateRandomColor(keyValuePair.Value);
			}
			return dictionary;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000879C File Offset: 0x0000699C
		private unsafe static string GenerateRandomColor(string baseColor)
		{
			byte b = byte.MaxValue;
			byte b2;
			if (!string.IsNullOrWhiteSpace(baseColor) && baseColor.Length == 9 && byte.TryParse(MemoryExtensions.AsSpan(baseColor, 1, 2), 515, CultureInfo.InvariantCulture, ref b2))
			{
				b = b2;
			}
			Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)3], 3);
			RandomNumberGenerator.Fill(span);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 4);
			defaultInterpolatedStringHandler.AppendLiteral("#");
			defaultInterpolatedStringHandler.AppendFormatted<byte>(b, "X2");
			defaultInterpolatedStringHandler.AppendFormatted<byte>(*span[0], "X2");
			defaultInterpolatedStringHandler.AppendFormatted<byte>(*span[1], "X2");
			defaultInterpolatedStringHandler.AppendFormatted<byte>(*span[2], "X2");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00008858 File Offset: 0x00006A58
		private void RefreshCustomThemeMenuItems()
		{
			if (this.ThemesMenuItem == null)
			{
				return;
			}
			foreach (MenuItem menuItem in this._customThemeMenuItems)
			{
				menuItem.Click -= new RoutedEventHandler(this.ThemeMenuItem_OnClick);
				this.ThemesMenuItem.Items.Remove(menuItem);
			}
			this._customThemeMenuItems.Clear();
			IReadOnlyList<string> customThemeNames = this._userConfiguration.GetCustomThemeNames();
			if (customThemeNames.Count == 0)
			{
				if (this.CustomThemesSeparator != null)
				{
					this.CustomThemesSeparator.Visibility = 2;
				}
				return;
			}
			if (this.CustomThemesSeparator != null)
			{
				this.CustomThemesSeparator.Visibility = 0;
			}
			Style style = base.TryFindResource("ToggleMenuItemStyle") as Style;
			int num = (this.CustomThemesSeparator != null && this.ThemesMenuItem.Items.Contains(this.CustomThemesSeparator)) ? this.ThemesMenuItem.Items.IndexOf(this.CustomThemesSeparator) : this.ThemesMenuItem.Items.Count;
			foreach (string text in customThemeNames)
			{
				MenuItem menuItem2 = new MenuItem
				{
					Header = text,
					Height = 35.0,
					IsCheckable = true,
					Tag = text,
					Style = style
				};
				menuItem2.Click += new RoutedEventHandler(this.ThemeMenuItem_OnClick);
				this._customThemeMenuItems.Add(menuItem2);
				this.ThemesMenuItem.Items.Insert(num, menuItem2);
				num++;
			}
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00008A18 File Offset: 0x00006C18
		private void ShowCustomThemeEditor()
		{
			ColorTheme colorTheme = this._userConfiguration.ColorTheme;
			string currentThemeName = this._userConfiguration.GetCurrentThemeName();
			this.UpdateThemeMenuSelection(colorTheme, currentThemeName);
			IReadOnlyDictionary<string, string> themePaletteColors = this._userConfiguration.GetThemePaletteColors();
			App.ApplyTheme(colorTheme, (themePaletteColors.Count > 0) ? themePaletteColors : null);
			this.ClearScrollViewerCache();
			new ThemePaletteEditorDialog(this._userConfiguration)
			{
				Owner = this
			}.ShowDialog();
			this.RefreshCustomThemeMenuItems();
			this.UpdateThemeMenuSelection(this._userConfiguration.ColorTheme, this._userConfiguration.GetCurrentThemeName());
			this.ClearScrollViewerCache();
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00008AAA File Offset: 0x00006CAA
		private void AlwaysClearModlistsMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			this.HandleModlistAutoLoadMenuClick(sender, ModlistAutoLoadBehavior.Replace, ModlistAutoLoadBehavior.Prompt);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00008AB5 File Offset: 0x00006CB5
		private void AlwaysAddModlistsMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			this.HandleModlistAutoLoadMenuClick(sender, ModlistAutoLoadBehavior.Add, ModlistAutoLoadBehavior.Prompt);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00008AC0 File Offset: 0x00006CC0
		private void HandleModlistAutoLoadMenuClick(object sender, ModlistAutoLoadBehavior enabledBehavior, ModlistAutoLoadBehavior disabledBehavior)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			ModlistAutoLoadBehavior modlistAutoLoadBehavior = menuItem.IsChecked ? enabledBehavior : disabledBehavior;
			this.SetModlistAutoLoadBehavior(modlistAutoLoadBehavior);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00008AEC File Offset: 0x00006CEC
		private void SetModlistAutoLoadBehavior(ModlistAutoLoadBehavior behavior)
		{
			this.UpdateModlistAutoLoadMenu(behavior);
			this._userConfiguration.SetModlistAutoLoadBehavior(behavior);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00008B01 File Offset: 0x00006D01
		private void UpdateModlistAutoLoadMenu(ModlistAutoLoadBehavior behavior)
		{
			if (this.AlwaysClearModlistsMenuItem != null)
			{
				this.AlwaysClearModlistsMenuItem.IsChecked = (behavior == ModlistAutoLoadBehavior.Replace);
			}
			if (this.AlwaysAddModlistsMenuItem != null)
			{
				this.AlwaysAddModlistsMenuItem.IsChecked = (behavior == ModlistAutoLoadBehavior.Add);
			}
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00008B34 File Offset: 0x00006D34
		[NullableContext(2)]
		private void UpdateGameVersionMenuItem(string gameVersion)
		{
			string targetGameVersionOverride = this._userConfiguration.TargetGameVersionOverride;
			if (this.GameVersionMenuItem == null)
			{
				this.UpdateTargetGameVersionMenuHeader(gameVersion, targetGameVersionOverride);
				return;
			}
			bool flag = !string.IsNullOrWhiteSpace(gameVersion);
			bool flag2 = !string.IsNullOrWhiteSpace(targetGameVersionOverride);
			if (!flag && !flag2)
			{
				this.GameVersionMenuItem.Visibility = 2;
				this.UpdateTargetGameVersionMenuHeader(gameVersion, targetGameVersionOverride);
				return;
			}
			string text = flag ? gameVersion.Trim() : "Unknown";
			if (TargetGameVersionPreference.IsLatest(targetGameVersionOverride))
			{
				this.GameVersionMenuItem.Header = "Vintage Story: " + text + " (targeting latest)";
			}
			else if (flag2)
			{
				HeaderedItemsControl gameVersionMenuItem = this.GameVersionMenuItem;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(28, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Vintage Story: ");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral(" (targeting ");
				defaultInterpolatedStringHandler.AppendFormatted(targetGameVersionOverride);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				gameVersionMenuItem.Header = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			else
			{
				this.GameVersionMenuItem.Header = "Vintage Story: " + text;
			}
			this.GameVersionMenuItem.Visibility = 0;
			this.UpdateTargetGameVersionMenuHeader(gameVersion, targetGameVersionOverride);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00008C44 File Offset: 0x00006E44
		[NullableContext(2)]
		private void UpdateTargetGameVersionMenuHeader(string detectedVersion, string overrideVersion)
		{
			if (this.TargetGameVersionMenuItem == null)
			{
				return;
			}
			string text = string.IsNullOrWhiteSpace(detectedVersion) ? "unknown" : detectedVersion.Trim();
			if (TargetGameVersionPreference.IsLatest(overrideVersion))
			{
				this.TargetGameVersionMenuItem.Header = "Target version: Latest (detected: " + text + ")";
				return;
			}
			if (string.IsNullOrWhiteSpace(overrideVersion))
			{
				this.TargetGameVersionMenuItem.Header = "Target version: Auto (" + text + ")";
				return;
			}
			HeaderedItemsControl targetGameVersionMenuItem = this.TargetGameVersionMenuItem;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(29, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Target version: ");
			defaultInterpolatedStringHandler.AppendFormatted(overrideVersion);
			defaultInterpolatedStringHandler.AppendLiteral(" (detected: ");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			targetGameVersionMenuItem.Header = defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00008D08 File Offset: 0x00006F08
		private void TargetGameVersionMenuItem_OnSubmenuOpened(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			string installedVersion = VintageStoryVersionLocator.GetInstalledVersion(this._gameDirectory);
			string targetGameVersionOverride = this._userConfiguration.TargetGameVersionOverride;
			string text = VersionStringUtility.Normalize(targetGameVersionOverride);
			bool flag = TargetGameVersionPreference.IsLatest(targetGameVersionOverride);
			IEnumerable<string> enumerable = this.BuildTargetVersionMenuCandidates(installedVersion, targetGameVersionOverride);
			menuItem.Items.Clear();
			string header = string.IsNullOrWhiteSpace(installedVersion) ? "Auto (detected: unknown)" : ("Auto (detected: " + installedVersion + ")");
			MenuItem menuItem2 = new MenuItem
			{
				Header = header,
				Tag = "__target_auto__",
				IsCheckable = true,
				IsChecked = string.IsNullOrWhiteSpace(targetGameVersionOverride)
			};
			menuItem2.Click += new RoutedEventHandler(this.TargetGameVersionChoiceMenuItem_OnClick);
			menuItem.Items.Add(menuItem2);
			MenuItem menuItem3 = new MenuItem
			{
				Header = "Latest",
				Tag = "__target_latest__",
				IsCheckable = true,
				IsChecked = flag
			};
			menuItem3.Click += new RoutedEventHandler(this.TargetGameVersionChoiceMenuItem_OnClick);
			menuItem.Items.Add(menuItem3);
			menuItem.Items.Add(new Separator());
			foreach (string text2 in enumerable)
			{
				string text3 = VersionStringUtility.Normalize(text2);
				MenuItem menuItem4 = new MenuItem
				{
					Header = text2,
					Tag = text2,
					IsCheckable = true,
					IsChecked = (!flag && !string.IsNullOrWhiteSpace(text) && string.Equals(text3, text, 5))
				};
				menuItem4.Click += new RoutedEventHandler(this.TargetGameVersionChoiceMenuItem_OnClick);
				menuItem.Items.Add(menuItem4);
			}
			MenuItem menuItem5 = new MenuItem
			{
				Header = "Custom...",
				Tag = "__target_custom__",
				IsCheckable = false
			};
			menuItem5.Click += new RoutedEventHandler(this.TargetGameVersionChoiceMenuItem_OnClick);
			menuItem.Items.Add(menuItem5);
			this.UpdateTargetGameVersionMenuHeader(installedVersion, targetGameVersionOverride);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00008F14 File Offset: 0x00007114
		private void TargetGameVersionChoiceMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<TargetGameVersionChoiceMenuItem_OnClick>d__194 <TargetGameVersionChoiceMenuItem_OnClick>d__;
			<TargetGameVersionChoiceMenuItem_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<TargetGameVersionChoiceMenuItem_OnClick>d__.<>4__this = this;
			<TargetGameVersionChoiceMenuItem_OnClick>d__.sender = sender;
			<TargetGameVersionChoiceMenuItem_OnClick>d__.<>1__state = -1;
			<TargetGameVersionChoiceMenuItem_OnClick>d__.<>t__builder.Start<MainWindow.<TargetGameVersionChoiceMenuItem_OnClick>d__194>(ref <TargetGameVersionChoiceMenuItem_OnClick>d__);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00008F54 File Offset: 0x00007154
		public static void ApplyTargetGameVersionMenuCheckedState(MenuItem selectedMenuItem)
		{
			if (!selectedMenuItem.IsCheckable)
			{
				return;
			}
			ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(selectedMenuItem) ?? (selectedMenuItem.Parent as ItemsControl);
			if (itemsControl == null)
			{
				return;
			}
			foreach (MenuItem menuItem in Enumerable.OfType<MenuItem>(itemsControl.Items))
			{
				if (menuItem.IsCheckable)
				{
					menuItem.IsChecked = (menuItem == selectedMenuItem);
				}
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00008FD4 File Offset: 0x000071D4
		[NullableContext(2)]
		private string PromptForTargetGameVersionOverride()
		{
			string text = TargetGameVersionPreference.HasConcreteOverride(this._userConfiguration.TargetGameVersionOverride) ? this._userConfiguration.TargetGameVersionOverride : (MainWindow.ExtractMajorMinorVersion(VintageStoryVersionLocator.GetInstalledVersion(this._gameDirectory)) ?? string.Empty);
			string text2 = Interaction.InputBox("Enter the target Vintage Story version (for example: 1.22 or 1.22.0).", "Set Target Game Version", text, -1, -1);
			if (string.IsNullOrWhiteSpace(text2))
			{
				return null;
			}
			string text3 = text2.Trim();
			string text4 = VersionStringUtility.Normalize(text3);
			if (string.IsNullOrWhiteSpace(text4))
			{
				ModManagerMessageBox.Show("The entered target version is not valid.", "Simple VS Manager", 0, 48, null, null, null);
				return null;
			}
			return MainWindow.ExtractMajorMinorVersion(text3) ?? text4;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00009070 File Offset: 0x00007270
		[NullableContext(2)]
		[return: Nullable(1)]
		private IReadOnlyList<string> BuildTargetVersionMenuCandidates(string detectedVersion, string currentOverride)
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			MainWindow.AddTargetVersionCandidate(hashSet, detectedVersion);
			MainWindow.AddTargetVersionCandidate(hashSet, currentOverride);
			ModBrowserViewModel modBrowserViewModel = this._modBrowserViewModel;
			ObservableCollection<GameVersion> observableCollection = (modBrowserViewModel != null) ? modBrowserViewModel.AvailableVersions : null;
			if (observableCollection != null && observableCollection.Count > 0)
			{
				foreach (GameVersion gameVersion in observableCollection)
				{
					MainWindow.AddTargetVersionCandidate(hashSet, (gameVersion != null) ? gameVersion.Name : null);
				}
			}
			if (hashSet.Count == 0 && this._viewModel != null)
			{
				foreach (ModListItemViewModel modListItemViewModel in this._viewModel.GetInstalledModsSnapshot())
				{
					foreach (ModVersionOptionViewModel modVersionOptionViewModel in modListItemViewModel.VersionOptions)
					{
						ModReleaseInfo release = modVersionOptionViewModel.Release;
						IReadOnlyList<string> readOnlyList = (release != null) ? release.GameVersionTags : null;
						if (readOnlyList != null && readOnlyList.Count > 0)
						{
							foreach (string value2 in readOnlyList)
							{
								MainWindow.AddTargetVersionCandidate(hashSet, value2);
							}
						}
					}
				}
			}
			return Enumerable.ToArray<string>(Enumerable.ThenByDescending<string, string>(Enumerable.ThenByDescending<string, int>(Enumerable.ThenByDescending<string, int>(Enumerable.OrderByDescending<string, int>(hashSet, (string value) => MainWindow.ParseTargetVersionSortKey(value).Item1), (string value) => MainWindow.ParseTargetVersionSortKey(value).Item2), (string value) => MainWindow.ParseTargetVersionSortKey(value).Item3), (string value) => value, StringComparer.OrdinalIgnoreCase));
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00009288 File Offset: 0x00007488
		private static void AddTargetVersionCandidate(ISet<string> candidates, [Nullable(2)] string value)
		{
			string text = MainWindow.ExtractMajorMinorVersion(value);
			if (string.IsNullOrWhiteSpace(text))
			{
				return;
			}
			candidates.Add(text);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x000092B0 File Offset: 0x000074B0
		[NullableContext(2)]
		private static string ExtractMajorMinorVersion(string value)
		{
			string text = VersionStringUtility.Normalize(value);
			if (string.IsNullOrWhiteSpace(text))
			{
				return null;
			}
			string[] array = text.Split('.', 1);
			if (array.Length < 2)
			{
				return null;
			}
			return array[0] + "." + array[1];
		}

		// Token: 0x06000180 RID: 384 RVA: 0x000092F0 File Offset: 0x000074F0
		[NullableContext(0)]
		[return: TupleElementNames(new string[]
		{
			"Major",
			"Minor",
			"Patch"
		})]
		private static ValueTuple<int, int, int> ParseTargetVersionSortKey([Nullable(1)] string value)
		{
			string text = VersionStringUtility.Normalize(value);
			if (string.IsNullOrWhiteSpace(text))
			{
				return new ValueTuple<int, int, int>(-1, -1, -1);
			}
			string[] array = text.Split('.', 1);
			int num2;
			int num = (array.Length != 0 && int.TryParse(array[0], 7, CultureInfo.InvariantCulture, ref num2)) ? num2 : -1;
			int num4;
			int num3 = (array.Length > 1 && int.TryParse(array[1], 7, CultureInfo.InvariantCulture, ref num4)) ? num4 : -1;
			int num6;
			int num5 = (array.Length > 2 && int.TryParse(array[2], 7, CultureInfo.InvariantCulture, ref num6)) ? num6 : -1;
			return new ValueTuple<int, int, int>(num, num3, num5);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00009380 File Offset: 0x00007580
		private void ApplyStoredWindowDimensions()
		{
			double? windowWidth = this._userConfiguration.WindowWidth;
			double? windowHeight = this._userConfiguration.WindowHeight;
			double? windowLeft = this._userConfiguration.WindowLeft;
			double? windowTop = this._userConfiguration.WindowTop;
			if (windowWidth == null && windowHeight == null && windowLeft == null && windowTop == null)
			{
				return;
			}
			base.SizeToContent = 0;
			if (windowWidth != null)
			{
				base.Width = windowWidth.Value;
			}
			if (windowHeight != null)
			{
				base.Height = windowHeight.Value;
			}
			if (windowLeft != null && windowTop != null && MainWindow.IsWindowPositionOnScreen(windowLeft.Value, windowTop.Value))
			{
				base.WindowStartupLocation = 0;
				base.Left = windowLeft.Value;
				base.Top = windowTop.Value;
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00009460 File Offset: 0x00007660
		private static bool IsWindowPositionOnScreen(double left, double top)
		{
			Point point;
			point..ctor((int)left + 50, (int)top + 50);
			Screen[] allScreens = Screen.AllScreens;
			for (int i = 0; i < allScreens.Length; i++)
			{
				if (allScreens[i].WorkingArea.Contains(point))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000094A8 File Offset: 0x000076A8
		private void SaveWindowDimensions()
		{
			if (this._userConfiguration == null)
			{
				return;
			}
			Rect rect = (base.WindowState == null) ? new Rect(base.Left, base.Top, base.ActualWidth, base.ActualHeight) : base.RestoreBounds;
			this._userConfiguration.SetWindowDimensions(rect.Width, rect.Height);
			this._userConfiguration.SetWindowPosition(rect.Left, rect.Top);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00009520 File Offset: 0x00007720
		[NullableContext(2)]
		private void SetUsernameDisplay(string name)
		{
			string cloudUploaderName = string.IsNullOrWhiteSpace(name) ? null : name.Trim();
			this._userConfiguration.SetCloudUploaderName(cloudUploaderName);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000954C File Offset: 0x0000774C
		private string ResolveUploaderName([Nullable(2)] string fallbackUserId = null)
		{
			MainViewModel viewModel = this._viewModel;
			string text = (viewModel != null) ? viewModel.PlayerName : null;
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text.Trim();
			}
			MainViewModel viewModel2 = this._viewModel;
			string text2 = (viewModel2 != null) ? viewModel2.PlayerUid : null;
			if (string.IsNullOrWhiteSpace(text2))
			{
				text2 = fallbackUserId;
			}
			if (string.IsNullOrWhiteSpace(text2))
			{
				FirebaseModlistStore cloudModlistStore = this._cloudModlistStore;
				text2 = ((cloudModlistStore != null) ? cloudModlistStore.CurrentUserId : null);
			}
			if (!string.IsNullOrWhiteSpace(text2))
			{
				ReadOnlySpan<char> readOnlySpan = MemoryExtensions.Trim(MemoryExtensions.AsSpan(text2));
				if (!readOnlySpan.IsEmpty)
				{
					string text3 = (readOnlySpan.Length <= 4) ? readOnlySpan.ToString() : readOnlySpan.Slice(readOnlySpan.Length - 4, 4).ToString();
					if (string.IsNullOrWhiteSpace(text3))
					{
						text3 = "0000";
					}
					return "Anonymous" + text3;
				}
			}
			return "Anonymous0000";
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00009629 File Offset: 0x00007829
		private void ApplyPlayerIdentityToUiAndCloudStore()
		{
			this.SetUsernameDisplay(this.ResolveUploaderName(null));
			if (this._cloudModlistStore != null)
			{
				this.ApplyPlayerIdentityToCloudStore(this._cloudModlistStore);
			}
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000964C File Offset: 0x0000784C
		[NullableContext(2)]
		private void ApplyPlayerIdentityToCloudStore(FirebaseModlistStore store)
		{
			if (store == null)
			{
				return;
			}
			MainViewModel viewModel = this._viewModel;
			string playerUid = (viewModel != null) ? viewModel.PlayerUid : null;
			MainViewModel viewModel2 = this._viewModel;
			store.SetPlayerIdentity(playerUid, (viewModel2 != null) ? viewModel2.PlayerName : null);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000967C File Offset: 0x0000787C
		private void SaveUploaderName()
		{
			if (this._userConfiguration == null)
			{
				return;
			}
			FirebaseModlistStore cloudModlistStore = this._cloudModlistStore;
			string usernameDisplay = this.ResolveUploaderName((cloudModlistStore != null) ? cloudModlistStore.CurrentUserId : null);
			this.SetUsernameDisplay(usernameDisplay);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000096B4 File Offset: 0x000078B4
		private void EnableServerOptionsMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			this._userConfiguration.SetEnableServerOptions(menuItem.IsChecked);
			bool enableServerOptions = this._userConfiguration.EnableServerOptions;
			this.UpdateServerOptionsState(enableServerOptions);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000096F0 File Offset: 0x000078F0
		private void UseFasterThumbnailsMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			this._userConfiguration.SetUseFasterThumbnails(menuItem.IsChecked);
			menuItem.IsChecked = this._userConfiguration.UseFasterThumbnails;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000972C File Offset: 0x0000792C
		private void CacheModDatabaseResultsMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			this._userConfiguration.SetCacheModDatabaseResults(menuItem.IsChecked);
			menuItem.IsChecked = this._userConfiguration.CacheModDatabaseResults;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00009768 File Offset: 0x00007968
		private void DisableHoverEffectsMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			this._userConfiguration.SetDisableHoverEffects(menuItem.IsChecked);
			menuItem.IsChecked = this._userConfiguration.DisableHoverEffects;
			HoverEffectHelper.SetDisableHoverEffects(this, menuItem.IsChecked);
			this.RefreshHoverOverlayState();
		}

		// Token: 0x0600018D RID: 397 RVA: 0x000097B4 File Offset: 0x000079B4
		private void LogModUpdateMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			this._userConfiguration.SetLogModUpdates(menuItem.IsChecked);
			menuItem.IsChecked = this._userConfiguration.LogModUpdates;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000097F0 File Offset: 0x000079F0
		private void LogModInstallMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			this._userConfiguration.SetLogModInstalls(menuItem.IsChecked);
			menuItem.IsChecked = this._userConfiguration.LogModInstalls;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000982C File Offset: 0x00007A2C
		private void LogModDeletionMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			this._userConfiguration.SetLogModDeletions(menuItem.IsChecked);
			menuItem.IsChecked = this._userConfiguration.LogModDeletions;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00009868 File Offset: 0x00007A68
		private void LogAppLifecycleMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			this._userConfiguration.SetLogAppLaunchAndExit(menuItem.IsChecked);
			menuItem.IsChecked = this._userConfiguration.LogAppLaunchAndExit;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000098A4 File Offset: 0x00007AA4
		private void LogErrorsAndExceptionsMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			this._userConfiguration.SetLogErrorsAndExceptions(menuItem.IsChecked);
			menuItem.IsChecked = this._userConfiguration.LogErrorsAndExceptions;
			this.InitializeTraceListener();
		}

		// Token: 0x06000192 RID: 402 RVA: 0x000098E4 File Offset: 0x00007AE4
		private void InitializeTraceListener()
		{
			if (this._traceListener != null)
			{
				Trace.Listeners.Remove(this._traceListener);
				this._traceListener.Dispose();
				this._traceListener = null;
			}
			if (this._userConfiguration.LogErrorsAndExceptions)
			{
				this._traceListener = new ModManagerTraceListener(this._modActivityLoggingService);
				Trace.Listeners.Add(this._traceListener);
				this._modActivityLoggingService.LogDiagnostic("Error and exception logging enabled");
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000995C File Offset: 0x00007B5C
		private void UpdateLoggingMenuState()
		{
			if (this.LogModUpdateMenuItem != null)
			{
				this.LogModUpdateMenuItem.IsChecked = this._userConfiguration.LogModUpdates;
			}
			if (this.LogModInstallMenuItem != null)
			{
				this.LogModInstallMenuItem.IsChecked = this._userConfiguration.LogModInstalls;
			}
			if (this.LogModDeletionMenuItem != null)
			{
				this.LogModDeletionMenuItem.IsChecked = this._userConfiguration.LogModDeletions;
			}
			if (this.LogAppLifecycleMenuItem != null)
			{
				this.LogAppLifecycleMenuItem.IsChecked = this._userConfiguration.LogAppLaunchAndExit;
			}
			if (this.LogErrorsAndExceptionsMenuItem != null)
			{
				this.LogErrorsAndExceptionsMenuItem.IsChecked = this._userConfiguration.LogErrorsAndExceptions;
			}
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00009A00 File Offset: 0x00007C00
		private void GenerateServerInstallMacroMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			if (this._viewModel == null)
			{
				ModManagerMessageBox.Show(this, "The manager is still initializing. Please try again once the installed mods have finished loading.", "Simple VS Manager", 0, 64, null, null, null);
				return;
			}
			IEnumerable<ModListItemViewModel> installedModsSnapshot = this._viewModel.GetInstalledModsSnapshot();
			List<ValueTuple<string, string>> list = new List<ValueTuple<string, string>>();
			foreach (ModListItemViewModel modListItemViewModel in installedModsSnapshot)
			{
				if (modListItemViewModel != null && modListItemViewModel.IsInstalled && !string.IsNullOrWhiteSpace(modListItemViewModel.ModId) && !string.IsNullOrWhiteSpace(modListItemViewModel.Version))
				{
					list.Add(new ValueTuple<string, string>(modListItemViewModel.ModId.Trim(), modListItemViewModel.Version.Trim()));
				}
			}
			if (list.Count == 0)
			{
				ModManagerMessageBox.Show(this, "No installed mods with a known version were found. Install mods before generating a server macro.", "Simple VS Manager", 0, 64, null, null, null);
				return;
			}
			string text = ServerMacroGenerator.CreateDefaultMacroName();
			SaveFileDialog saveFileDialog = new SaveFileDialog
			{
				Title = "Save server macro",
				Filter = "Server macro (*.json)|*.json|All files (*.*)|*.*",
				AddExtension = true,
				DefaultExt = ".json",
				FileName = text + ".json"
			};
			try
			{
				string configurationDirectory = this._userConfiguration.GetConfigurationDirectory();
				if (!string.IsNullOrWhiteSpace(configurationDirectory))
				{
					Directory.CreateDirectory(configurationDirectory);
					saveFileDialog.InitialDirectory = configurationDirectory;
				}
			}
			catch (Exception)
			{
			}
			if (!saveFileDialog.ShowDialog(this).GetValueOrDefault())
			{
				return;
			}
			string fileName = saveFileDialog.FileName;
			string description = string.Format(CultureInfo.InvariantCulture, "Install {0} mods generated by Simple VS Manager on {1:yyyy-MM-dd HH:mm} UTC.", list.Count, DateTime.UtcNow);
			ServerMacroGenerator.ServerMacroGenerationResult serverMacroGenerationResult;
			try
			{
				serverMacroGenerationResult = ServerMacroGenerator.CreateInstallMacro(fileName, text, list, description, null);
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException || ex is NotSupportedException || ex is SecurityException)
			{
				StatusLogService.AppendStatus("Failed to create server macro: " + ex.Message, true);
				ModManagerMessageBox.Show(this, "Failed to create the server macro:\n" + ex.Message, "Simple VS Manager", 0, 16, null, null, null);
				return;
			}
			if (!serverMacroGenerationResult.HasMacro)
			{
				ModManagerMessageBox.Show(this, "Unable to generate server commands for the installed mods.", "Simple VS Manager", 0, 48, null, null, null);
				return;
			}
			string command = serverMacroGenerationResult.Command;
			bool flag = false;
			try
			{
				Clipboard.SetText(command);
				flag = true;
			}
			catch (ExternalException)
			{
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(47, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Created server macro '");
			defaultInterpolatedStringHandler.AppendFormatted(serverMacroGenerationResult.MacroName);
			defaultInterpolatedStringHandler.AppendLiteral("' with ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(serverMacroGenerationResult.CommandCount);
			defaultInterpolatedStringHandler.AppendLiteral(" install commands.");
			StatusLogService.AppendStatus(defaultInterpolatedStringHandler.ToStringAndClear(), false);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Saved server macro file:");
			stringBuilder.AppendLine(fileName);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(string.Format(CultureInfo.InvariantCulture, "Place this file in your server's config directory as servermacros.json (or merge it with your existing macros) and run {0} to install {1} mods.", command, serverMacroGenerationResult.CommandCount));
			if (flag)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("The command was copied to your clipboard.");
			}
			else
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("Copy this command before running it.");
			}
			ModManagerMessageBox.Show(this, stringBuilder.ToString(), "Simple VS Manager", 0, 64, null, null, null);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00009D78 File Offset: 0x00007F78
		private void CacheAllVersionsMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			this._userConfiguration.SetCacheAllVersionsLocally(menuItem.IsChecked);
			menuItem.IsChecked = this._userConfiguration.CacheAllVersionsLocally;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00009DB4 File Offset: 0x00007FB4
		private void RequireExactVsVersionMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			bool requireExactVsVersionMatch = this._userConfiguration.RequireExactVsVersionMatch;
			this._userConfiguration.SetRequireExactVsVersionMatch(menuItem.IsChecked);
			menuItem.IsChecked = this._userConfiguration.RequireExactVsVersionMatch;
			if (requireExactVsVersionMatch != this._userConfiguration.RequireExactVsVersionMatch)
			{
				MainViewModel viewModel = this._viewModel;
				if (viewModel == null)
				{
					return;
				}
				viewModel.RefreshInstalledModDetails();
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00009E18 File Offset: 0x00008018
		private void ExperimentalModDebuggingMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<ExperimentalModDebuggingMenuItem_OnClick>d__223 <ExperimentalModDebuggingMenuItem_OnClick>d__;
			<ExperimentalModDebuggingMenuItem_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<ExperimentalModDebuggingMenuItem_OnClick>d__.<>4__this = this;
			<ExperimentalModDebuggingMenuItem_OnClick>d__.<>1__state = -1;
			<ExperimentalModDebuggingMenuItem_OnClick>d__.<>t__builder.Start<MainWindow.<ExperimentalModDebuggingMenuItem_OnClick>d__223>(ref <ExperimentalModDebuggingMenuItem_OnClick>d__);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00009E50 File Offset: 0x00008050
		private void ExperimentalAllModsDebuggingMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<ExperimentalAllModsDebuggingMenuItem_OnClick>d__224 <ExperimentalAllModsDebuggingMenuItem_OnClick>d__;
			<ExperimentalAllModsDebuggingMenuItem_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<ExperimentalAllModsDebuggingMenuItem_OnClick>d__.<>4__this = this;
			<ExperimentalAllModsDebuggingMenuItem_OnClick>d__.<>1__state = -1;
			<ExperimentalAllModsDebuggingMenuItem_OnClick>d__.<>t__builder.Start<MainWindow.<ExperimentalAllModsDebuggingMenuItem_OnClick>d__224>(ref <ExperimentalAllModsDebuggingMenuItem_OnClick>d__);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00009E88 File Offset: 0x00008088
		private static List<ExperimentalModDebugLogLine> CollectExperimentalModDebugLines(string logsDirectory, string modId)
		{
			List<ExperimentalModDebugLogLine> list = new List<ExperimentalModDebugLogLine>();
			foreach (string filePath in MainWindow.GetExperimentalModDebugFilePaths(logsDirectory))
			{
				MainWindow.AppendExperimentalModDebugLines(list, filePath, modId);
			}
			return list;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00009EE4 File Offset: 0x000080E4
		private static List<ExperimentalModDebugLogLine> CollectInstalledModDebugLines(string logsDirectory, IReadOnlyList<MainWindow.InstalledModLogIdentifier> modIdentifiers)
		{
			List<ExperimentalModDebugLogLine> list = new List<ExperimentalModDebugLogLine>();
			if (modIdentifiers.Count == 0)
			{
				return list;
			}
			foreach (string filePath in MainWindow.GetExperimentalModDebugFilePaths(logsDirectory))
			{
				MainWindow.AppendInstalledModDebugLines(list, filePath, modIdentifiers);
			}
			return list;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00009F4C File Offset: 0x0000814C
		private static List<string> GetExperimentalModDebugFilePaths(string logsDirectory)
		{
			List<string> list = new List<string>();
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (string text in MainWindow.ExperimentalModDebugLogPrefixes)
			{
				foreach (string text2 in MainWindow.ExperimentalModDebugLogExtensions)
				{
					string text3 = text2.StartsWith('.') ? (text + "*" + text2) : (text + "*." + text2);
					try
					{
						foreach (string text4 in Directory.EnumerateFiles(logsDirectory, text3, 0))
						{
							if (hashSet.Add(text4))
							{
								list.Add(text4);
							}
						}
					}
					catch (IOException)
					{
					}
					catch (UnauthorizedAccessException)
					{
					}
				}
				string text5 = Path.Combine(logsDirectory, text);
				if (File.Exists(text5) && hashSet.Add(text5))
				{
					list.Add(text5);
				}
			}
			list.Sort(StringComparer.OrdinalIgnoreCase);
			return list;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000A080 File Offset: 0x00008280
		private static void AppendExperimentalModDebugLines(List<ExperimentalModDebugLogLine> logLines, string filePath, string modId)
		{
			try
			{
				string fileName = Path.GetFileName(filePath);
				List<ValueTuple<string, int>> list = new List<ValueTuple<string, int>>();
				int num = 0;
				foreach (string text in File.ReadLines(filePath))
				{
					num++;
					if (!MainWindow.ShouldIgnoreExperimentalModDebugLine(text) && text.IndexOf(modId, 5) >= 0)
					{
						list.Add(new ValueTuple<string, int>(text, num));
					}
				}
				MainWindow.AppendExperimentalModDebugFileSectionWithModId(logLines, filePath, fileName, list, modId);
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000A128 File Offset: 0x00008328
		private static void AppendInstalledModDebugLines(List<ExperimentalModDebugLogLine> logLines, string filePath, IReadOnlyList<MainWindow.InstalledModLogIdentifier> modIdentifiers)
		{
			try
			{
				string fileName = Path.GetFileName(filePath);
				List<ValueTuple<string, string, int>> list = new List<ValueTuple<string, string, int>>();
				int num = 0;
				foreach (string text in File.ReadLines(filePath))
				{
					num++;
					if (!MainWindow.ShouldIgnoreExperimentalModDebugLine(text))
					{
						foreach (MainWindow.InstalledModLogIdentifier installedModLogIdentifier in modIdentifiers)
						{
							if (text.IndexOf(installedModLogIdentifier.SearchValue, 5) >= 0)
							{
								list.Add(new ValueTuple<string, string, int>(text, installedModLogIdentifier.DisplayLabel, num));
								break;
							}
						}
					}
				}
				MainWindow.AppendExperimentalModDebugFileSectionWithMods(logLines, filePath, fileName, list);
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000A214 File Offset: 0x00008414
		private static void AppendExperimentalModDebugFileSection(List<ExperimentalModDebugLogLine> logLines, string fileName, List<string> matchedLines)
		{
			if (matchedLines.Count == 0)
			{
				return;
			}
			List<string> list = MainWindow.SummarizePatchMissingLines(matchedLines);
			if (list.Count == 0)
			{
				return;
			}
			logLines.Add(ExperimentalModDebugLogLine.FromPlainText("**" + fileName + "**", null, null, 0));
			foreach (string rawText in list)
			{
				logLines.Add(ExperimentalModDebugLogLine.FromLogEntry(rawText, null, null, 0));
			}
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000A2A4 File Offset: 0x000084A4
		private static void AppendExperimentalModDebugFileSectionWithModId(List<ExperimentalModDebugLogLine> logLines, string filePath, string fileName, [TupleElementNames(new string[]
		{
			"Line",
			"LineNumber"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1
		})] List<ValueTuple<string, int>> matchedLines, string modId)
		{
			if (matchedLines.Count == 0)
			{
				return;
			}
			List<ValueTuple<string, int>> list = MainWindow.SummarizePatchMissingLinesWithLineNumbers(matchedLines);
			if (list.Count == 0)
			{
				return;
			}
			logLines.Add(ExperimentalModDebugLogLine.FromPlainText("**" + fileName + "**", null, null, 0));
			foreach (ValueTuple<string, int> valueTuple in list)
			{
				string item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				logLines.Add(ExperimentalModDebugLogLine.FromLogEntry(item, modId, filePath, item2));
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000A340 File Offset: 0x00008540
		private static void AppendExperimentalModDebugFileSectionWithMods(List<ExperimentalModDebugLogLine> logLines, string filePath, string fileName, [TupleElementNames(new string[]
		{
			"Line",
			"ModName",
			"LineNumber"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})] List<ValueTuple<string, string, int>> matchedLines)
		{
			if (matchedLines.Count == 0)
			{
				return;
			}
			List<ValueTuple<string, string, int>> list = MainWindow.SummarizePatchMissingLinesWithModAndLineNumbers(matchedLines);
			if (list.Count == 0)
			{
				return;
			}
			logLines.Add(ExperimentalModDebugLogLine.FromPlainText("**" + fileName + "**", null, null, 0));
			foreach (ValueTuple<string, string, int> valueTuple in list)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				int item3 = valueTuple.Item3;
				logLines.Add(ExperimentalModDebugLogLine.FromLogEntry(item, item2, filePath, item3));
			}
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000A3E4 File Offset: 0x000085E4
		private static bool ShouldIgnoreExperimentalModDebugLine(string line)
		{
			foreach (string text in MainWindow.ExperimentalModDebugIgnoredLinePhrases)
			{
				if (line.IndexOf(text, 5) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000A418 File Offset: 0x00008618
		private static List<string> SummarizePatchMissingLines(List<string> matchedLines)
		{
			if (matchedLines.Count == 0)
			{
				return matchedLines;
			}
			List<string> list = new List<string>(matchedLines.Count);
			Dictionary<string, ValueTuple<int, int>> dictionary = new Dictionary<string, ValueTuple<int, int>>(StringComparer.OrdinalIgnoreCase);
			foreach (string text in matchedLines)
			{
				Match match = MainWindow.PatchAssetMissingRegex.Match(text);
				if (match.Success)
				{
					string value = match.Groups["mod"].Value;
					if (!string.IsNullOrEmpty(value))
					{
						string summaryKey = "__PATCH_MOD__" + value.Trim();
						MainWindow.AddOrIncrementSummary(list, dictionary, text, summaryKey);
						continue;
					}
				}
				string text2 = null;
				foreach (string text3 in MainWindow.SummarizableLinePrefixes)
				{
					if (text.IndexOf(text3, 5) >= 0)
					{
						text2 = text3;
						break;
					}
				}
				if (text2 != null)
				{
					string summaryKey2 = "__PREFIX__" + text2;
					MainWindow.AddOrIncrementSummary(list, dictionary, text, summaryKey2);
				}
				else
				{
					list.Add(text);
				}
			}
			foreach (ValueTuple<int, int> valueTuple in dictionary.Values)
			{
				if (valueTuple.Item2 > 0)
				{
					int item = valueTuple.Item1;
					if (item >= 0 && item < list.Count)
					{
						List<string> list2 = list;
						int num = item;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 2);
						defaultInterpolatedStringHandler.AppendFormatted(list[item]);
						defaultInterpolatedStringHandler.AppendLiteral(" (");
						defaultInterpolatedStringHandler.AppendFormatted<int>(valueTuple.Item2);
						defaultInterpolatedStringHandler.AppendLiteral(" similar lines hidden...)");
						list2[num] = defaultInterpolatedStringHandler.ToStringAndClear();
					}
				}
			}
			return list;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000A5E8 File Offset: 0x000087E8
		[return: TupleElementNames(new string[]
		{
			"Line",
			"LineNumber"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		private static List<ValueTuple<string, int>> SummarizePatchMissingLinesWithLineNumbers([TupleElementNames(new string[]
		{
			"Line",
			"LineNumber"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1
		})] List<ValueTuple<string, int>> matchedLines)
		{
			if (matchedLines.Count == 0)
			{
				return matchedLines;
			}
			List<ValueTuple<string, int>> list = new List<ValueTuple<string, int>>(matchedLines.Count);
			Dictionary<string, ValueTuple<int, int>> dictionary = new Dictionary<string, ValueTuple<int, int>>(StringComparer.OrdinalIgnoreCase);
			foreach (ValueTuple<string, int> valueTuple in matchedLines)
			{
				string item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				Match match = MainWindow.PatchAssetMissingRegex.Match(item);
				if (match.Success)
				{
					string value = match.Groups["mod"].Value;
					if (!string.IsNullOrEmpty(value))
					{
						string summaryKey = "__PATCH_MOD__" + value.Trim();
						MainWindow.AddOrIncrementSummaryWithLineNumber(list, dictionary, item, item2, summaryKey);
						continue;
					}
				}
				string text = null;
				foreach (string text2 in MainWindow.SummarizableLinePrefixes)
				{
					if (item.IndexOf(text2, 5) >= 0)
					{
						text = text2;
						break;
					}
				}
				if (text != null)
				{
					string summaryKey2 = "__PREFIX__" + text;
					MainWindow.AddOrIncrementSummaryWithLineNumber(list, dictionary, item, item2, summaryKey2);
				}
				else
				{
					list.Add(new ValueTuple<string, int>(item, item2));
				}
			}
			foreach (ValueTuple<int, int> valueTuple2 in dictionary.Values)
			{
				if (valueTuple2.Item2 > 0)
				{
					int item3 = valueTuple2.Item1;
					if (item3 >= 0 && item3 < list.Count)
					{
						ValueTuple<string, int> valueTuple3 = list[item3];
						string item4 = valueTuple3.Item1;
						int item5 = valueTuple3.Item2;
						List<ValueTuple<string, int>> list2 = list;
						int num = item3;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 2);
						defaultInterpolatedStringHandler.AppendFormatted(item4);
						defaultInterpolatedStringHandler.AppendLiteral(" (");
						defaultInterpolatedStringHandler.AppendFormatted<int>(valueTuple2.Item2);
						defaultInterpolatedStringHandler.AppendLiteral(" similar lines hidden...)");
						list2[num] = new ValueTuple<string, int>(defaultInterpolatedStringHandler.ToStringAndClear(), item5);
					}
				}
			}
			return list;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000A7EC File Offset: 0x000089EC
		private static void AddOrIncrementSummary(List<string> summarized, [TupleElementNames(new string[]
		{
			"Index",
			"HiddenCount"
		})] [Nullable(new byte[]
		{
			1,
			1,
			0
		})] Dictionary<string, ValueTuple<int, int>> summaries, string line, string summaryKey)
		{
			ValueTuple<int, int> valueTuple;
			if (summaries.TryGetValue(summaryKey, ref valueTuple))
			{
				summaries[summaryKey] = new ValueTuple<int, int>(valueTuple.Item1, valueTuple.Item2 + 1);
				return;
			}
			summaries[summaryKey] = new ValueTuple<int, int>(summarized.Count, 0);
			summarized.Add(line);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000A83C File Offset: 0x00008A3C
		private static void AddOrIncrementSummaryWithLineNumber([TupleElementNames(new string[]
		{
			"Line",
			"LineNumber"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1
		})] List<ValueTuple<string, int>> summarized, [TupleElementNames(new string[]
		{
			"Index",
			"HiddenCount"
		})] [Nullable(new byte[]
		{
			1,
			1,
			0
		})] Dictionary<string, ValueTuple<int, int>> summaries, string line, int lineNumber, string summaryKey)
		{
			ValueTuple<int, int> valueTuple;
			if (summaries.TryGetValue(summaryKey, ref valueTuple))
			{
				summaries[summaryKey] = new ValueTuple<int, int>(valueTuple.Item1, valueTuple.Item2 + 1);
				return;
			}
			summaries[summaryKey] = new ValueTuple<int, int>(summarized.Count, 0);
			summarized.Add(new ValueTuple<string, int>(line, lineNumber));
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000A894 File Offset: 0x00008A94
		[return: TupleElementNames(new string[]
		{
			"Line",
			"ModName",
			"LineNumber"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		private static List<ValueTuple<string, string, int>> SummarizePatchMissingLinesWithModAndLineNumbers([TupleElementNames(new string[]
		{
			"Line",
			"ModName",
			"LineNumber"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})] List<ValueTuple<string, string, int>> matchedLines)
		{
			if (matchedLines.Count == 0)
			{
				return matchedLines;
			}
			List<ValueTuple<string, string, int>> list = new List<ValueTuple<string, string, int>>(matchedLines.Count);
			Dictionary<string, ValueTuple<int, int>> dictionary = new Dictionary<string, ValueTuple<int, int>>(StringComparer.OrdinalIgnoreCase);
			foreach (ValueTuple<string, string, int> valueTuple in matchedLines)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				int item3 = valueTuple.Item3;
				Match match = MainWindow.PatchAssetMissingRegex.Match(item);
				if (match.Success)
				{
					string value = match.Groups["mod"].Value;
					if (!string.IsNullOrEmpty(value))
					{
						string summaryKey = "__PATCH_MOD__" + value.Trim();
						MainWindow.AddOrIncrementSummaryWithModAndLineNumber(list, dictionary, item, item2, item3, summaryKey);
						continue;
					}
				}
				string text = null;
				foreach (string text2 in MainWindow.SummarizableLinePrefixes)
				{
					if (item.IndexOf(text2, 5) >= 0)
					{
						text = text2;
						break;
					}
				}
				if (text != null)
				{
					string summaryKey2 = "__PREFIX__" + text;
					MainWindow.AddOrIncrementSummaryWithModAndLineNumber(list, dictionary, item, item2, item3, summaryKey2);
				}
				else
				{
					list.Add(new ValueTuple<string, string, int>(item, item2, item3));
				}
			}
			foreach (ValueTuple<int, int> valueTuple2 in dictionary.Values)
			{
				if (valueTuple2.Item2 > 0)
				{
					int item4 = valueTuple2.Item1;
					if (item4 >= 0 && item4 < list.Count)
					{
						ValueTuple<string, string, int> valueTuple3 = list[item4];
						string item5 = valueTuple3.Item1;
						string item6 = valueTuple3.Item2;
						int item7 = valueTuple3.Item3;
						List<ValueTuple<string, string, int>> list2 = list;
						int num = item4;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 2);
						defaultInterpolatedStringHandler.AppendFormatted(item5);
						defaultInterpolatedStringHandler.AppendLiteral(" (");
						defaultInterpolatedStringHandler.AppendFormatted<int>(valueTuple2.Item2);
						defaultInterpolatedStringHandler.AppendLiteral(" similar lines hidden...)");
						list2[num] = new ValueTuple<string, string, int>(defaultInterpolatedStringHandler.ToStringAndClear(), item6, item7);
					}
				}
			}
			return list;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000AAB4 File Offset: 0x00008CB4
		private static void AddOrIncrementSummaryWithModAndLineNumber([TupleElementNames(new string[]
		{
			"Line",
			"ModName",
			"LineNumber"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})] List<ValueTuple<string, string, int>> summarized, [TupleElementNames(new string[]
		{
			"Index",
			"HiddenCount"
		})] [Nullable(new byte[]
		{
			1,
			1,
			0
		})] Dictionary<string, ValueTuple<int, int>> summaries, string line, string modName, int lineNumber, string summaryKey)
		{
			ValueTuple<int, int> valueTuple;
			if (summaries.TryGetValue(summaryKey, ref valueTuple))
			{
				summaries[summaryKey] = new ValueTuple<int, int>(valueTuple.Item1, valueTuple.Item2 + 1);
				return;
			}
			summaries[summaryKey] = new ValueTuple<int, int>(summarized.Count, 0);
			summarized.Add(new ValueTuple<string, string, int>(line, modName, lineNumber));
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000AB0C File Offset: 0x00008D0C
		private void InitializeViewModel()
		{
			if (string.IsNullOrWhiteSpace(this._dataDirectory))
			{
				throw new InvalidOperationException("The data directory is not set.");
			}
			this._viewModel = new MainViewModel(this._dataDirectory, this._userConfiguration, this._gameDirectory)
			{
				IsCompactView = this._userConfiguration.IsCompactView,
				UseModDbDesignView = this._userConfiguration.UseModDbDesignView
			};
			this._viewModel.PropertyChanged += new PropertyChangedEventHandler(this.ViewModelOnPropertyChanged);
			this._viewModel.UserReportVoteSubmitted += new EventHandler<MainViewModel.ModUserReportChangedEventArgs>(this.OnUserReportVoteSubmitted);
			base.DataContext = this._viewModel;
			this.ApplyPlayerIdentityToUiAndCloudStore();
			this._cloudModlistsLoaded = false;
			this._localModlistsLoaded = false;
			this._selectedCloudModlist = null;
			this.AttachToModsView(this._viewModel.CurrentModsView, false);
			this.RestoreSortPreference();
			this.RestoreModlistsSortPreferences();
			this.UpdateGameVersionMenuItem(this._viewModel.InstalledGameVersion);
			this.ApplyColumnVisibilityPreferencesToViewModel();
			this.SubscribeModBrowserToDirectoryWatcher();
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000AC00 File Offset: 0x00008E00
		private void InitializeModBrowserView()
		{
			if (this.ModBrowserView != null)
			{
				ModApiService modApiService = new ModApiService(new HttpClient(new HttpClientHandler
				{
					AutomaticDecompression = 3
				})
				{
					Timeout = TimeSpan.FromSeconds(30.0)
				});
				this._modBrowserViewModel = new ModBrowserViewModel(modApiService, this._userConfiguration);
				this._modBrowserViewModel.SetInstallModCallback(new Func<DownloadableMod, Task>(this.InstallModFromBrowserAsync));
				this.ModBrowserView.DataContext = this._modBrowserViewModel;
				TabItem databaseTab = this.DatabaseTab;
				this.UpdateModBrowserTabVisibilityState(databaseTab != null && databaseTab.IsSelected);
				if (!this._userConfiguration.DisableInternetAccess)
				{
					this.LoadTargetGameVersionsForMenuAsync();
				}
				this.InitializeVotesCacheWatcher();
			}
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000ACB0 File Offset: 0x00008EB0
		private Task LoadTargetGameVersionsForMenuAsync()
		{
			MainWindow.<LoadTargetGameVersionsForMenuAsync>d__242 <LoadTargetGameVersionsForMenuAsync>d__;
			<LoadTargetGameVersionsForMenuAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadTargetGameVersionsForMenuAsync>d__.<>4__this = this;
			<LoadTargetGameVersionsForMenuAsync>d__.<>1__state = -1;
			<LoadTargetGameVersionsForMenuAsync>d__.<>t__builder.Start<MainWindow.<LoadTargetGameVersionsForMenuAsync>d__242>(ref <LoadTargetGameVersionsForMenuAsync>d__);
			return <LoadTargetGameVersionsForMenuAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000ACF4 File Offset: 0x00008EF4
		private void InitializeVotesCacheWatcher()
		{
			try
			{
				string voteCachePath = ModVersionVoteService.GetVoteCachePath();
				this._votesCacheWatcher = new VotesCacheWatcher(voteCachePath);
				this._votesCacheWatcher.CacheChanged += new EventHandler(this.OnVotesCacheChanged);
				this._votesCacheWatcher.EnsureWatcher();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000AD4C File Offset: 0x00008F4C
		private void OnVotesCacheChanged([Nullable(2)] object sender, EventArgs e)
		{
			if (this._modBrowserViewModel == null)
			{
				return;
			}
			base.Dispatcher.InvokeAsync(delegate()
			{
				try
				{
					VotesCacheWatcher votesCacheWatcher = this._votesCacheWatcher;
					if (votesCacheWatcher != null && votesCacheWatcher.TryConsumePendingChanges())
					{
						this._modBrowserViewModel.InvalidateAllVisibleUserReports();
					}
				}
				catch (Exception)
				{
				}
			}, 4);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000AD70 File Offset: 0x00008F70
		private void SubscribeModBrowserToDirectoryWatcher()
		{
			if (!this._isModBrowserWatcherSubscribed)
			{
				MainViewModel viewModel = this._viewModel;
				if (((viewModel != null) ? viewModel.ModsWatcher : null) != null && this._modBrowserViewModel != null)
				{
					this._viewModel.ModsWatcher.ChangesDetected += new EventHandler(this.ModsWatcherOnChangesDetected);
					this._isModBrowserWatcherSubscribed = true;
					return;
				}
			}
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000ADC8 File Offset: 0x00008FC8
		private void UnsubscribeModBrowserFromDirectoryWatcher()
		{
			if (this._isModBrowserWatcherSubscribed)
			{
				MainViewModel viewModel = this._viewModel;
				if (((viewModel != null) ? viewModel.ModsWatcher : null) != null)
				{
					this._viewModel.ModsWatcher.ChangesDetected -= new EventHandler(this.ModsWatcherOnChangesDetected);
					this._isModBrowserWatcherSubscribed = false;
					return;
				}
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000AE15 File Offset: 0x00009015
		private void ModsWatcherOnChangesDetected([Nullable(2)] object sender, EventArgs e)
		{
			if (this._modBrowserViewModel == null || this._isInstallingMod)
			{
				return;
			}
			base.Dispatcher.InvokeAsync<Task>(delegate()
			{
				MainWindow.<<ModsWatcherOnChangesDetected>b__247_0>d <<ModsWatcherOnChangesDetected>b__247_0>d;
				<<ModsWatcherOnChangesDetected>b__247_0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<ModsWatcherOnChangesDetected>b__247_0>d.<>4__this = this;
				<<ModsWatcherOnChangesDetected>b__247_0>d.<>1__state = -1;
				<<ModsWatcherOnChangesDetected>b__247_0>d.<>t__builder.Start<MainWindow.<<ModsWatcherOnChangesDetected>b__247_0>d>(ref <<ModsWatcherOnChangesDetected>b__247_0>d);
				return <<ModsWatcherOnChangesDetected>b__247_0>d.<>t__builder.Task;
			}, 4);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000AE44 File Offset: 0x00009044
		private void SyncInstalledModsToModBrowser()
		{
			if (this._modBrowserViewModel == null || this._viewModel == null)
			{
				return;
			}
			IEnumerable<ModListItemViewModel> installedModsSnapshot = this._viewModel.GetInstalledModsSnapshot();
			List<string> list = new List<string>();
			List<int> list2 = new List<int>();
			foreach (ModListItemViewModel modListItemViewModel in installedModsSnapshot)
			{
				if (!string.IsNullOrWhiteSpace(modListItemViewModel.ModId))
				{
					list.Add(modListItemViewModel.ModId);
					int num;
					if (int.TryParse(modListItemViewModel.ModId, ref num) && !list2.Contains(num))
					{
						list2.Add(num);
					}
				}
			}
			this._modBrowserViewModel.UpdateInstalledMods(list, list2);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000AEF4 File Offset: 0x000090F4
		private void AddModToInstalledAndRemoveFromSearch(int modId)
		{
			if (this._modBrowserViewModel == null)
			{
				return;
			}
			this._modBrowserViewModel.AddInstalledMod(modId.ToString(CultureInfo.InvariantCulture), new int?(modId));
			DownloadableModOnList downloadableModOnList = Enumerable.FirstOrDefault<DownloadableModOnList>(this._modBrowserViewModel.ModsList, (DownloadableModOnList m) => m.ModId == modId);
			if (downloadableModOnList != null)
			{
				this._modBrowserViewModel.ModsList.Remove(downloadableModOnList);
				if (this._modBrowserViewModel.SelectedMod == downloadableModOnList)
				{
					this._modBrowserViewModel.SelectedMod = null;
				}
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000AF8C File Offset: 0x0000918C
		private Task InstallModFromBrowserAsync(DownloadableMod mod)
		{
			MainWindow.<InstallModFromBrowserAsync>d__250 <InstallModFromBrowserAsync>d__;
			<InstallModFromBrowserAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<InstallModFromBrowserAsync>d__.<>4__this = this;
			<InstallModFromBrowserAsync>d__.mod = mod;
			<InstallModFromBrowserAsync>d__.<>1__state = -1;
			<InstallModFromBrowserAsync>d__.<>t__builder.Start<MainWindow.<InstallModFromBrowserAsync>d__250>(ref <InstallModFromBrowserAsync>d__);
			return <InstallModFromBrowserAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000AFD8 File Offset: 0x000091D8
		private ModListItemViewModel ConvertToModListItemViewModel(DownloadableMod mod)
		{
			MainWindow.<>c__DisplayClass251_0 CS$<>8__locals1 = new MainWindow.<>c__DisplayClass251_0();
			CS$<>8__locals1.<>4__this = this;
			MainWindow.<>c__DisplayClass251_0 CS$<>8__locals2 = CS$<>8__locals1;
			MainViewModel viewModel = this._viewModel;
			CS$<>8__locals2.effectiveGameVersion = ((viewModel != null) ? viewModel.EffectiveGameVersion : null);
			CS$<>8__locals1.requireExactVersionMatch = this._userConfiguration.RequireExactVsVersionMatch;
			List<DownloadableModRelease> releases = mod.Releases;
			List<ModReleaseInfo> list;
			if (releases == null)
			{
				list = null;
			}
			else
			{
				list = Enumerable.ToList<ModReleaseInfo>(Enumerable.Select<ModReleaseInfo, ModReleaseInfo>(Enumerable.Where<ModReleaseInfo>(Enumerable.Select<DownloadableModRelease, ModReleaseInfo>(Enumerable.OrderByDescending<DownloadableModRelease, DateTime>(releases, delegate(DownloadableModRelease r)
				{
					DateTime result;
					if (!DateTime.TryParse(r.Created, ref result))
					{
						return DateTime.MinValue;
					}
					return result;
				}), (DownloadableModRelease release) => MainWindow.ConvertToModReleaseInfo(release, CS$<>8__locals1.effectiveGameVersion, CS$<>8__locals1.requireExactVersionMatch)), (ModReleaseInfo release) => release != null), (ModReleaseInfo release) => release));
			}
			List<ModReleaseInfo> list2 = list ?? new List<ModReleaseInfo>();
			ModReleaseInfo modReleaseInfo = Enumerable.FirstOrDefault<ModReleaseInfo>(list2);
			ModReleaseInfo modReleaseInfo2 = TargetGameVersionReleaseSelector.SelectLatestCompatibleRelease(list2, CS$<>8__locals1.effectiveGameVersion, CS$<>8__locals1.requireExactVersionMatch);
			string logoUrlSource = (!string.IsNullOrWhiteSpace(mod.LogoFileDatabase)) ? "logofiledb" : null;
			ModDatabaseInfo modDatabaseInfo = new ModDatabaseInfo();
			List<string> tags = mod.Tags;
			modDatabaseInfo.Tags = (((tags != null) ? tags.ToArray() : null) ?? Array.Empty<string>());
			modDatabaseInfo.AssetId = mod.AssetId.ToString();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(38, 1);
			defaultInterpolatedStringHandler.AppendLiteral("https://mods.vintagestory.at/show/mod/");
			defaultInterpolatedStringHandler.AppendFormatted<int>(mod.AssetId);
			modDatabaseInfo.ModPageUrl = defaultInterpolatedStringHandler.ToStringAndClear();
			modDatabaseInfo.LatestVersion = ((modReleaseInfo != null) ? modReleaseInfo.Version : null);
			modDatabaseInfo.LatestCompatibleVersion = ((modReleaseInfo2 != null) ? modReleaseInfo2.Version : null);
			modDatabaseInfo.RequiredGameVersions = Enumerable.ToArray<string>(Enumerable.Distinct<string>(Enumerable.SelectMany<ModReleaseInfo, string>(list2, (ModReleaseInfo r) => r.GameVersionTags)));
			modDatabaseInfo.Downloads = new int?(mod.Downloads);
			modDatabaseInfo.Comments = new int?(mod.Comments);
			modDatabaseInfo.Follows = new int?(mod.Follows);
			modDatabaseInfo.TrendingPoints = new int?(mod.TrendingPoints);
			modDatabaseInfo.LogoUrl = mod.LogoFileDatabase;
			modDatabaseInfo.LogoUrlSource = logoUrlSource;
			modDatabaseInfo.LastReleasedUtc = ((modReleaseInfo != null) ? modReleaseInfo.CreatedUtc : default(DateTime?));
			modDatabaseInfo.LatestRelease = modReleaseInfo;
			modDatabaseInfo.LatestCompatibleRelease = modReleaseInfo2;
			modDatabaseInfo.Releases = list2;
			modDatabaseInfo.Side = mod.Side;
			ModDatabaseInfo databaseInfo = modDatabaseInfo;
			ModEntry modEntry = new ModEntry();
			modEntry.ModId = (mod.ModIdStr ?? mod.ModId.ToString());
			modEntry.Name = mod.Name;
			modEntry.Version = null;
			ModEntry modEntry2 = modEntry;
			string[] authors;
			if (string.IsNullOrWhiteSpace(mod.Author))
			{
				authors = Array.Empty<string>();
			}
			else
			{
				(authors = new string[1])[0] = mod.Author;
			}
			modEntry2.Authors = authors;
			modEntry.Website = mod.HomepageUrl;
			modEntry.SourcePath = string.Empty;
			modEntry.SourceKind = ModSourceKind.ZipArchive;
			modEntry.DatabaseInfo = databaseInfo;
			return new ModListItemViewModel(modEntry, false, "Mod Database", (ModListItemViewModel _, bool _) => Task.FromResult<ActivationResult>(new ActivationResult(false, null)), CS$<>8__locals1.effectiveGameVersion, false, null, () => CS$<>8__locals1.<>4__this._userConfiguration.RequireExactVsVersionMatch, true, null, null);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000B318 File Offset: 0x00009518
		[NullableContext(2)]
		private static ModReleaseInfo ConvertToModReleaseInfo([Nullable(1)] DownloadableModRelease release, string targetGameVersion, bool requireExactVersionMatch)
		{
			if (string.IsNullOrWhiteSpace(release.MainFile) || string.IsNullOrWhiteSpace(release.Filename))
			{
				return null;
			}
			Uri downloadUri = new Uri(release.MainFile);
			DateTime? createdUtc = default(DateTime?);
			DateTime dateTime;
			if (DateTime.TryParse(release.Created, ref dateTime))
			{
				createdUtc = new DateTime?(dateTime.ToUniversalTime());
			}
			List<string> tags = release.Tags;
			string[] gameVersionTags = ((tags != null) ? tags.ToArray() : null) ?? Array.Empty<string>();
			return new ModReleaseInfo
			{
				Version = release.ModVersion,
				NormalizedVersion = VersionStringUtility.Normalize(release.ModVersion),
				DownloadUri = downloadUri,
				FileName = release.Filename,
				GameVersionTags = gameVersionTags,
				IsCompatibleWithInstalledGame = TargetGameVersionReleaseSelector.TagsSupportTarget(gameVersionTags, targetGameVersion, requireExactVersionMatch),
				Changelog = release.Changelog,
				Downloads = new int?(release.Downloads),
				CreatedUtc = createdUtc
			};
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000B3FC File Offset: 0x000095FC
		private void OnUserReportVoteSubmitted([Nullable(2)] object sender, MainViewModel.ModUserReportChangedEventArgs e)
		{
			if (this._modBrowserViewModel == null)
			{
				return;
			}
			int value;
			if (e.NumericModId != null)
			{
				value = e.NumericModId.Value;
			}
			else if (!int.TryParse(e.ModId, 7, CultureInfo.InvariantCulture, ref value))
			{
				return;
			}
			this._modBrowserViewModel.InvalidateUserReport(value, e.Summary);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000B45C File Offset: 0x0000965C
		private void ViewModelOnPropertyChanged([Nullable(2)] object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "SelectedSortOption")
			{
				if (this._viewModel != null)
				{
					base.Dispatcher.InvokeAsync(delegate()
					{
						if (this._viewModel != null)
						{
							this.UpdateSortPreferenceFromSelectedOption(!this._suppressSortPreferenceSave);
						}
					}, 4);
					return;
				}
			}
			else if (e.PropertyName == "IsCompactView")
			{
				if (this._viewModel != null)
				{
					this._userConfiguration.SetCompactViewMode(this._viewModel.IsCompactView);
					return;
				}
			}
			else if (e.PropertyName == "UseModDbDesignView")
			{
				if (this._viewModel != null)
				{
					this._userConfiguration.SetModDbDesignViewMode(this._viewModel.UseModDbDesignView);
					return;
				}
			}
			else if (e.PropertyName == "IsViewingModlistTab")
			{
				if (this._viewModel != null)
				{
					base.Dispatcher.InvokeAsync(delegate()
					{
						if (this._viewModel != null)
						{
							this.HandleModlistsVisibilityChanged(this._viewModel.IsViewingModlistTab);
							this.SyncMiddleTabControlToViewModel();
						}
					}, 4);
					return;
				}
			}
			else if (e.PropertyName == "IsViewingMainTab")
			{
				if (this._viewModel != null)
				{
					base.Dispatcher.InvokeAsync(delegate()
					{
						if (this._viewModel != null)
						{
							this.SyncMiddleTabControlToViewModel();
						}
					}, 4);
					return;
				}
			}
			else if (e.PropertyName == "CurrentModsView")
			{
				if (this._viewModel != null)
				{
					base.Dispatcher.InvokeAsync(delegate()
					{
						if (this._viewModel != null)
						{
							ICollectionView currentModsView = this._viewModel.CurrentModsView;
							ICollectionView currentModsView2 = this._currentModsView;
							bool preserveState = this.ShouldPreserveModsViewState(currentModsView2, currentModsView);
							this.AttachToModsView(currentModsView, preserveState);
						}
					}, 4);
					return;
				}
			}
			else
			{
				if (e.PropertyName == "IsLoadingMods")
				{
					base.Dispatcher.InvokeAsync(delegate()
					{
						this.RefreshHoverOverlayState();
						this.ScheduleRefreshAfterModlistLoadIfReady();
					}, 4);
					return;
				}
				if (e.PropertyName == "IsLoadingModDetails")
				{
					base.Dispatcher.InvokeAsync(delegate()
					{
						this.RefreshHoverOverlayState();
						this.ScheduleRefreshAfterModlistLoadIfReady();
					}, 4);
					return;
				}
				if (e.PropertyName == "StatusMessage")
				{
					MainViewModel viewModel = this._viewModel;
					if (MainWindow.ShouldRefreshAfterDependencyResolution((viewModel != null) ? viewModel.StatusMessage : null))
					{
						base.Dispatcher.InvokeAsync<Task>(delegate()
						{
							MainWindow.<<ViewModelOnPropertyChanged>b__254_6>d <<ViewModelOnPropertyChanged>b__254_6>d;
							<<ViewModelOnPropertyChanged>b__254_6>d.<>t__builder = AsyncTaskMethodBuilder.Create();
							<<ViewModelOnPropertyChanged>b__254_6>d.<>4__this = this;
							<<ViewModelOnPropertyChanged>b__254_6>d.<>1__state = -1;
							<<ViewModelOnPropertyChanged>b__254_6>d.<>t__builder.Start<MainWindow.<<ViewModelOnPropertyChanged>b__254_6>d>(ref <<ViewModelOnPropertyChanged>b__254_6>d);
							return <<ViewModelOnPropertyChanged>b__254_6>d.<>t__builder.Task;
						}, 4);
					}
					base.Dispatcher.InvokeAsync(new Action(this.ScheduleRefreshAfterModlistLoadIfReady), 4);
				}
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000B662 File Offset: 0x00009862
		private void UpdateSearchColumnVisibility(bool isSearchingModDatabase)
		{
			this.UpdateSearchSortingBehavior(isSearchingModDatabase);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000B66C File Offset: 0x0000986C
		private void HandleModlistsVisibilityChanged(bool isVisible)
		{
			if (isVisible)
			{
				this.ApplyPreferredModlistsTabSelection();
				this.RefreshLocalModlists(true, null);
				if (this.ModlistsTabControl != null && this.OnlineModlistsTabItem != null && object.Equals(this.ModlistsTabControl.SelectedItem, this.OnlineModlistsTabItem))
				{
					this.RefreshCloudModlistsAsync(!this._cloudModlistsLoaded);
				}
				return;
			}
			this.SetLocalModlistSelection(Array.Empty<LocalModlistListEntry>());
			if (this.LocalModlistsDataGrid != null)
			{
				this.LocalModlistsDataGrid.SelectedItems.Clear();
			}
			this.SetCloudModlistSelection(null);
			if (this.CloudModlistsDataGrid != null)
			{
				this.CloudModlistsDataGrid.SelectedItem = null;
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000B704 File Offset: 0x00009904
		private void MiddleTabControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			MainWindow.<MiddleTabControl_OnSelectionChanged>d__257 <MiddleTabControl_OnSelectionChanged>d__;
			<MiddleTabControl_OnSelectionChanged>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<MiddleTabControl_OnSelectionChanged>d__.<>4__this = this;
			<MiddleTabControl_OnSelectionChanged>d__.sender = sender;
			<MiddleTabControl_OnSelectionChanged>d__.<>1__state = -1;
			<MiddleTabControl_OnSelectionChanged>d__.<>t__builder.Start<MainWindow.<MiddleTabControl_OnSelectionChanged>d__257>(ref <MiddleTabControl_OnSelectionChanged>d__);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000B744 File Offset: 0x00009944
		private void UpdateModBrowserTabVisibilityState(bool isSelected)
		{
			if (this._modBrowserViewModel == null)
			{
				return;
			}
			bool flag;
			if (isSelected)
			{
				TabItem databaseTab = this.DatabaseTab;
				flag = (databaseTab != null && databaseTab.IsVisible);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			this._modBrowserViewModel.SetTabVisibility(flag2);
			if (flag2)
			{
				ModBrowserView modBrowserView = this.ModBrowserView;
				if (modBrowserView != null && modBrowserView.IsModBrowserInitialized)
				{
					this._modBrowserViewModel.RefreshSearchAsync(this._userConfiguration.CacheModDatabaseResults);
				}
			}
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000B7B0 File Offset: 0x000099B0
		private void SyncMiddleTabControlToViewModel()
		{
			if (this.MiddleTabControl == null || this._viewModel == null)
			{
				return;
			}
			TabItem tabItem;
			if (this._viewModel.IsViewingMainTab)
			{
				tabItem = this.MainTab;
			}
			else if (this._viewModel.IsViewingModlistTab)
			{
				tabItem = this.ModlistTab;
			}
			else if (this._viewModel.SearchModDatabase)
			{
				tabItem = this.DatabaseTab;
			}
			else
			{
				tabItem = this.MainTab;
			}
			if (tabItem == null || object.Equals(this.MiddleTabControl.SelectedItem, tabItem))
			{
				return;
			}
			this._isUpdatingMiddleTabSelection = true;
			try
			{
				this.MiddleTabControl.SelectedItem = tabItem;
			}
			finally
			{
				this._isUpdatingMiddleTabSelection = false;
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000B85C File Offset: 0x00009A5C
		private void ModlistsTabControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this._isUpdatingModlistsTabSelection)
			{
				return;
			}
			MainViewModel viewModel = this._viewModel;
			if (viewModel == null || !viewModel.IsViewingModlistTab)
			{
				return;
			}
			TabControl tabControl = sender as TabControl;
			if (tabControl == null)
			{
				return;
			}
			if (this.OnlineModlistsTabItem == null || this.LocalModlistsTabItem == null)
			{
				return;
			}
			if (object.Equals(tabControl.SelectedItem, this.LocalModlistsTabItem))
			{
				this._userConfiguration.SetPreferredModlistsTab(ModlistsTabSelection.Local);
				return;
			}
			if (!object.Equals(tabControl.SelectedItem, this.OnlineModlistsTabItem))
			{
				return;
			}
			if (MainWindow.HasFirebaseAuthStateFile())
			{
				this.EnsureFirebaseAuthBackedUpIfAvailable();
			}
			if (!this.EnsureCloudModlistsConsent())
			{
				this._isUpdatingModlistsTabSelection = true;
				try
				{
					tabControl.SelectedItem = this.LocalModlistsTabItem;
				}
				finally
				{
					this._isUpdatingModlistsTabSelection = false;
				}
				this._userConfiguration.SetPreferredModlistsTab(ModlistsTabSelection.Local);
				return;
			}
			this._userConfiguration.SetPreferredModlistsTab(ModlistsTabSelection.Online);
			this.RefreshCloudModlistsAsync(!this._cloudModlistsLoaded);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000B948 File Offset: 0x00009B48
		private void ModlistsTabControl_OnLoaded(object sender, RoutedEventArgs e)
		{
			this.ApplyPreferredModlistsTabSelection();
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000B950 File Offset: 0x00009B50
		private void ApplyPreferredModlistsTabSelection()
		{
			if (this.ModlistsTabControl == null || this.LocalModlistsTabItem == null || this.OnlineModlistsTabItem == null)
			{
				return;
			}
			TabItem tabItem = (this._userConfiguration.PreferredModlistsTab == ModlistsTabSelection.Online) ? this.OnlineModlistsTabItem : this.LocalModlistsTabItem;
			if (!object.Equals(this.ModlistsTabControl.SelectedItem, tabItem))
			{
				this.ModlistsTabControl.SelectedItem = tabItem;
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000B9B2 File Offset: 0x00009BB2
		private void InternetAccessManager_OnInternetAccessChanged([Nullable(2)] object sender, EventArgs e)
		{
			if (base.Dispatcher.CheckAccess())
			{
				this.<InternetAccessManager_OnInternetAccessChanged>g__Update|263_0();
				return;
			}
			base.Dispatcher.BeginInvoke(9, new Action(this.<InternetAccessManager_OnInternetAccessChanged>g__Update|263_0));
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000B9E4 File Offset: 0x00009BE4
		private bool EnsureCloudModlistsConsent()
		{
			string message = string.Concat(new string[]
			{
				"In this tab you can easily save and load Modlists from an online database (Google Firebase), for free.",
				Environment.NewLine,
				Environment.NewLine,
				"When you continue, Simple VS Manager will create a firebase-auth.json (basically just a code that identifies you as the owner of your uploaded modlists) file in its configuration folder. If you lose this file you will not be able to delete or modify your uploaded online modlists.",
				Environment.NewLine,
				Environment.NewLine,
				"You will not need to sign in or provide any account information or do anything really :) Press OK to continue and never show this again!"
			});
			return this.EnsureFirebaseAuthConsent(message);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000BA3C File Offset: 0x00009C3C
		private bool EnsureFirebaseAuthConsent(string message)
		{
			string stateFilePath = FirebaseAnonymousAuthenticator.GetStateFilePath();
			if (string.IsNullOrWhiteSpace(stateFilePath))
			{
				return true;
			}
			if (File.Exists(stateFilePath))
			{
				this.EnsureFirebaseAuthBackedUpIfAvailable();
				return true;
			}
			MessageDialogButtonContentOverrides buttonContentOverrides = new MessageDialogButtonContentOverrides
			{
				Cancel = "No thanks"
			};
			return ModManagerMessageBox.Show(this, message, "Simple VS Manager", 1, 64, null, buttonContentOverrides, null) == 1;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000BA90 File Offset: 0x00009C90
		private static bool HasFirebaseAuthStateFile()
		{
			string stateFilePath = FirebaseAnonymousAuthenticator.GetStateFilePath();
			return !string.IsNullOrWhiteSpace(stateFilePath) && File.Exists(stateFilePath);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000BAB4 File Offset: 0x00009CB4
		private bool EnsureUserReportVotingConsent()
		{
			string message = "To enable voting, Simple VS Manager will create a firebase-auth.json (basically just a code that identifies you as the owner of your mod compatibility votes) file in its configuration folder. If you lose this file you will not be able to manage or remove your mod compatibility votes." + Environment.NewLine + Environment.NewLine + "You will not need to sign in or provide any account information or do anything really :) Press OK to continue and never show this again!";
			return this.EnsureFirebaseAuthConsent(message);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000BAE4 File Offset: 0x00009CE4
		private void EnsureFirebaseAuthBackedUpIfAvailable()
		{
			string stateFilePath = FirebaseAnonymousAuthenticator.GetStateFilePath();
			if (string.IsNullOrWhiteSpace(stateFilePath) || !File.Exists(stateFilePath))
			{
				return;
			}
			string dataDirectory = this._dataDirectory;
			if (string.IsNullOrWhiteSpace(dataDirectory))
			{
				return;
			}
			string text = Path.Combine(Path.Combine(dataDirectory, "ModData"), "SimpleVSManager");
			string text2 = Path.Combine(text, "firebase-auth.json");
			try
			{
				Directory.CreateDirectory(text);
				File.Copy(stateFilePath, text2, true);
			}
			catch (IOException ex)
			{
				this._modActivityLoggingService.LogError("Failed to back up Firebase auth state", ex);
				StatusLogService.AppendStatus("Failed to back up Firebase auth state: " + ex.Message, true);
			}
			catch (UnauthorizedAccessException ex2)
			{
				this._modActivityLoggingService.LogError("Failed to back up Firebase auth state", ex2);
				StatusLogService.AppendStatus("Failed to back up Firebase auth state: " + ex2.Message, true);
			}
			catch (NotSupportedException ex3)
			{
				this._modActivityLoggingService.LogError("Failed to back up Firebase auth state", ex3);
				StatusLogService.AppendStatus("Failed to back up Firebase auth state: " + ex3.Message, true);
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000BC00 File Offset: 0x00009E00
		private void DeleteFirebaseAuthFiles()
		{
			string stateFilePath = FirebaseAnonymousAuthenticator.GetStateFilePath();
			if (!string.IsNullOrWhiteSpace(stateFilePath))
			{
				MainWindow.TryDeleteFirebaseAuthFile(stateFilePath);
			}
			string dataDirectory = this._dataDirectory;
			if (string.IsNullOrWhiteSpace(dataDirectory))
			{
				return;
			}
			MainWindow.TryDeleteFirebaseAuthFile(Path.Combine(Path.Combine(dataDirectory, "ModData", "SimpleVSManager"), "firebase-auth.json"));
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000BC50 File Offset: 0x00009E50
		private static void TryDeleteFirebaseAuthFile(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return;
			}
			try
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is NotSupportedException || ex is SecurityException)
			{
				StatusLogService.AppendStatus("Failed to delete Firebase auth file " + path + ": " + ex.Message, true);
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000BCE0 File Offset: 0x00009EE0
		private void UpdateSearchSortingBehavior(bool isSearchingModDatabase)
		{
			if (this.ModsDataGrid == null)
			{
				return;
			}
			if (!isSearchingModDatabase)
			{
				this.ModsDataGrid.CanUserSortColumns = true;
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000BCFA File Offset: 0x00009EFA
		[NullableContext(2)]
		private static bool ShouldRefreshAfterDependencyResolution(string statusMessage)
		{
			return !string.IsNullOrWhiteSpace(statusMessage) && statusMessage.StartsWith("Resolved dependencies for ", 4);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000BD14 File Offset: 0x00009F14
		private Task RefreshModsAfterDependencyResolutionAsync()
		{
			MainWindow.<RefreshModsAfterDependencyResolutionAsync>d__273 <RefreshModsAfterDependencyResolutionAsync>d__;
			<RefreshModsAfterDependencyResolutionAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RefreshModsAfterDependencyResolutionAsync>d__.<>4__this = this;
			<RefreshModsAfterDependencyResolutionAsync>d__.<>1__state = -1;
			<RefreshModsAfterDependencyResolutionAsync>d__.<>t__builder.Start<MainWindow.<RefreshModsAfterDependencyResolutionAsync>d__273>(ref <RefreshModsAfterDependencyResolutionAsync>d__);
			return <RefreshModsAfterDependencyResolutionAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000BD58 File Offset: 0x00009F58
		private void ScheduleRefreshAfterModlistLoadIfReady()
		{
			if (!this._refreshAfterModlistLoadPending || this._isRefreshingAfterModlistLoad)
			{
				return;
			}
			MainViewModel viewModel = this._viewModel;
			if (((viewModel != null) ? viewModel.RefreshCommand : null) == null)
			{
				return;
			}
			if (viewModel.IsLoadingMods || viewModel.IsLoadingModDetails)
			{
				return;
			}
			this._isRefreshingAfterModlistLoad = true;
			base.Dispatcher.InvokeAsync<Task>(delegate()
			{
				MainWindow.<<ScheduleRefreshAfterModlistLoadIfReady>b__274_0>d <<ScheduleRefreshAfterModlistLoadIfReady>b__274_0>d;
				<<ScheduleRefreshAfterModlistLoadIfReady>b__274_0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<ScheduleRefreshAfterModlistLoadIfReady>b__274_0>d.<>4__this = this;
				<<ScheduleRefreshAfterModlistLoadIfReady>b__274_0>d.<>1__state = -1;
				<<ScheduleRefreshAfterModlistLoadIfReady>b__274_0>d.<>t__builder.Start<MainWindow.<<ScheduleRefreshAfterModlistLoadIfReady>b__274_0>d>(ref <<ScheduleRefreshAfterModlistLoadIfReady>b__274_0>d);
				return <<ScheduleRefreshAfterModlistLoadIfReady>b__274_0>d.<>t__builder.Task;
			}, 4);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000BDC0 File Offset: 0x00009FC0
		private void RestoreSortPreference()
		{
			if (this._viewModel == null)
			{
				return;
			}
			ValueTuple<string, ListSortDirection> modListSortPreference = this._userConfiguration.GetModListSortPreference();
			string item = modListSortPreference.Item1;
			if (!string.IsNullOrWhiteSpace(item))
			{
				this.ApplyModListSort(item, modListSortPreference.Item2, false);
				return;
			}
			this.UpdateSortPreferenceFromSelectedOption(false);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000BE08 File Offset: 0x0000A008
		private void ApplyModListSort(string sortMemberPath, ListSortDirection direction, bool persistPreference)
		{
			if (this._viewModel == null)
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(sortMemberPath))
			{
				return;
			}
			sortMemberPath = MainWindow.NormalizeSortMemberPath(sortMemberPath.Trim());
			SortOption sortOption = this.FindMatchingSortOption(sortMemberPath, direction);
			if (sortOption == null)
			{
				ValueTuple<string, ListSortDirection>[] sorts = MainWindow.BuildSortDescriptions(sortMemberPath, direction);
				sortOption = new SortOption(MainWindow.BuildSortDisplayName(sortMemberPath, direction), sorts);
			}
			this.ApplySortOption(sortOption, persistPreference);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000BE60 File Offset: 0x0000A060
		private void ApplySortOption(SortOption option, bool persistPreference)
		{
			if (this._viewModel == null)
			{
				return;
			}
			bool flag = this._viewModel.SelectedSortOption != option;
			bool suppressSortPreferenceSave = this._suppressSortPreferenceSave;
			this._suppressSortPreferenceSave = !persistPreference;
			try
			{
				if (flag)
				{
					this._viewModel.SelectedSortOption = option;
				}
				else
				{
					option.Apply(this._viewModel.ModsView);
					this.UpdateSortPreferenceFromSelectedOption(persistPreference);
				}
			}
			finally
			{
				this._suppressSortPreferenceSave = suppressSortPreferenceSave;
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000BEDC File Offset: 0x0000A0DC
		[NullableContext(2)]
		private static bool IsActiveSortMember(string sortMemberPath)
		{
			return !string.IsNullOrWhiteSpace(sortMemberPath) && (string.Equals(sortMemberPath, "IsActive", 5) || string.Equals(sortMemberPath, "ActiveSortOrder", 5));
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000BF04 File Offset: 0x0000A104
		private static string NormalizeSortMemberPath(string sortMemberPath)
		{
			if (string.IsNullOrWhiteSpace(sortMemberPath))
			{
				return sortMemberPath;
			}
			string text = sortMemberPath.Trim();
			if (string.Equals(text, "DisplayName", 5))
			{
				return "NameSortKey";
			}
			if (!MainWindow.IsActiveSortMember(text))
			{
				return text;
			}
			return "ActiveSortOrder";
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000BF45 File Offset: 0x0000A145
		private static bool SortMemberMatches([Nullable(2)] string columnSortMemberPath, string sortMemberPath)
		{
			return !string.IsNullOrWhiteSpace(columnSortMemberPath) && string.Equals(MainWindow.NormalizeSortMemberPath(columnSortMemberPath), MainWindow.NormalizeSortMemberPath(sortMemberPath), 5);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000BF64 File Offset: 0x0000A164
		[return: Nullable(2)]
		private SortOption FindMatchingSortOption(string sortMemberPath, ListSortDirection direction)
		{
			if (this._viewModel == null)
			{
				return null;
			}
			sortMemberPath = MainWindow.NormalizeSortMemberPath(sortMemberPath);
			foreach (SortOption sortOption in this._viewModel.SortOptions)
			{
				if (MainWindow.SortOptionMatches(sortOption, sortMemberPath, direction))
				{
					return sortOption;
				}
			}
			if (this._viewModel.SelectedSortOption != null && MainWindow.SortOptionMatches(this._viewModel.SelectedSortOption, sortMemberPath, direction))
			{
				return this._viewModel.SelectedSortOption;
			}
			return null;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000C000 File Offset: 0x0000A200
		private static bool SortOptionMatches(SortOption option, string sortMemberPath, ListSortDirection direction)
		{
			if (option.SortDescriptions.Count == 0)
			{
				return false;
			}
			ValueTuple<string, ListSortDirection> valueTuple = option.SortDescriptions[0];
			if (!string.Equals(valueTuple.Item1, sortMemberPath, 5) || valueTuple.Item2 != direction)
			{
				return false;
			}
			if (!MainWindow.IsActiveSortMember(sortMemberPath))
			{
				return true;
			}
			if (option.SortDescriptions.Count < 2)
			{
				return false;
			}
			ValueTuple<string, ListSortDirection> valueTuple2 = option.SortDescriptions[1];
			return string.Equals(valueTuple2.Item1, "NameSortKey", 5) && valueTuple2.Item2 == 0;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000C088 File Offset: 0x0000A288
		[return: TupleElementNames(new string[]
		{
			"Property",
			"Direction"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		private static ValueTuple<string, ListSortDirection>[] BuildSortDescriptions(string sortMemberPath, ListSortDirection direction)
		{
			List<ValueTuple<string, ListSortDirection>> list2;
			if (MainWindow.IsActiveSortMember(sortMemberPath))
			{
				List<ValueTuple<string, ListSortDirection>> list = new List<ValueTuple<string, ListSortDirection>>();
				list.Add(new ValueTuple<string, ListSortDirection>("ActiveSortOrder", direction));
				list.Add(new ValueTuple<string, ListSortDirection>("NameSortKey", 0));
				list2 = list;
			}
			else if (string.Equals(sortMemberPath, "LatestVersionSortKey", 5))
			{
				List<ValueTuple<string, ListSortDirection>> list3 = new List<ValueTuple<string, ListSortDirection>>();
				list3.Add(new ValueTuple<string, ListSortDirection>("LatestVersionSortKey", direction));
				list3.Add(new ValueTuple<string, ListSortDirection>("NameSortKey", 0));
				list2 = list3;
			}
			else
			{
				List<ValueTuple<string, ListSortDirection>> list4 = new List<ValueTuple<string, ListSortDirection>>();
				list4.Add(new ValueTuple<string, ListSortDirection>(sortMemberPath, direction));
				list2 = list4;
			}
			return list2.ToArray();
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000C118 File Offset: 0x0000A318
		private static string BuildSortDisplayName(string sortMemberPath, ListSortDirection direction)
		{
			if (MainWindow.IsActiveSortMember(sortMemberPath))
			{
				if (direction != null)
				{
					return "Active (Inactive → Active)";
				}
				return "Active (Active → Inactive)";
			}
			else if (string.Equals(sortMemberPath, "NameSortKey", 5))
			{
				if (direction != null)
				{
					return "Name (Z → A)";
				}
				return "Name (A → Z)";
			}
			else
			{
				if (!string.Equals(sortMemberPath, "LatestVersionSortKey", 5))
				{
					return sortMemberPath + " (" + ((direction == null) ? "Ascending" : "Descending") + ")";
				}
				if (direction != null)
				{
					return "Latest Version (Updates Last)";
				}
				return "Latest Version (Updates First)";
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000C198 File Offset: 0x0000A398
		private void UpdateSortPreferenceFromSelectedOption(bool persistPreference)
		{
			MainViewModel viewModel = this._viewModel;
			SortOption sortOption = (viewModel != null) ? viewModel.SelectedSortOption : null;
			if (sortOption == null)
			{
				this.ClearColumnSortIndicators();
				if (persistPreference)
				{
					this._userConfiguration.SetModListSortPreference(null, 0);
				}
				return;
			}
			if (sortOption.SortDescriptions.Count == 0)
			{
				this.ClearColumnSortIndicators();
				if (persistPreference)
				{
					this._userConfiguration.SetModListSortPreference(null, 0);
				}
				return;
			}
			ValueTuple<string, ListSortDirection> valueTuple = sortOption.SortDescriptions[0];
			this.UpdateColumnSortVisuals(valueTuple.Item1, valueTuple.Item2);
			if (persistPreference)
			{
				this._userConfiguration.SetModListSortPreference(valueTuple.Item1, valueTuple.Item2);
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000C230 File Offset: 0x0000A430
		private void UpdateColumnSortVisuals(string sortMemberPath, ListSortDirection direction)
		{
			if (this.ModsDataGrid == null)
			{
				return;
			}
			foreach (DataGridColumn dataGridColumn in this.ModsDataGrid.Columns)
			{
				if (MainWindow.SortMemberMatches(dataGridColumn.SortMemberPath, sortMemberPath))
				{
					dataGridColumn.SortDirection = new ListSortDirection?(direction);
				}
				else
				{
					dataGridColumn.SortDirection = default(ListSortDirection?);
				}
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000C2B0 File Offset: 0x0000A4B0
		private void ClearColumnSortIndicators()
		{
			if (this.ModsDataGrid == null)
			{
				return;
			}
			foreach (DataGridColumn dataGridColumn in this.ModsDataGrid.Columns)
			{
				dataGridColumn.SortDirection = default(ListSortDirection?);
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000C314 File Offset: 0x0000A514
		private Task InitializeViewModelAsync(MainViewModel viewModel)
		{
			MainWindow.<InitializeViewModelAsync>d__288 <InitializeViewModelAsync>d__;
			<InitializeViewModelAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<InitializeViewModelAsync>d__.<>4__this = this;
			<InitializeViewModelAsync>d__.viewModel = viewModel;
			<InitializeViewModelAsync>d__.<>1__state = -1;
			<InitializeViewModelAsync>d__.<>t__builder.Start<MainWindow.<InitializeViewModelAsync>d__288>(ref <InitializeViewModelAsync>d__);
			return <InitializeViewModelAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000C360 File Offset: 0x0000A560
		private void DisposeCurrentViewModel()
		{
			if (this._viewModel == null)
			{
				return;
			}
			MainViewModel viewModel = this._viewModel;
			this._viewModel = null;
			base.DataContext = null;
			this.StopGameSessionMonitor();
			this.DisposeViewModel(viewModel);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000C398 File Offset: 0x0000A598
		[NullableContext(2)]
		private void DisposeViewModel(MainViewModel viewModel)
		{
			if (viewModel == null)
			{
				return;
			}
			viewModel.PropertyChanged -= new PropertyChangedEventHandler(this.ViewModelOnPropertyChanged);
			viewModel.UserReportVoteSubmitted -= new EventHandler<MainViewModel.ModUserReportChangedEventArgs>(this.OnUserReportVoteSubmitted);
			viewModel.Dispose();
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000C3C8 File Offset: 0x0000A5C8
		private void StartModsWatcher()
		{
			if (this._viewModel == null)
			{
				return;
			}
			this.StopModsWatcher();
			this._modsWatcherTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(1.0)
			};
			this._modsWatcherTimer.Tick += new EventHandler(this.ModsWatcherTimerOnTick);
			this._modsWatcherTimer.Start();
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000C428 File Offset: 0x0000A628
		private void StartGameSessionMonitor()
		{
			if (string.IsNullOrWhiteSpace(this._dataDirectory) || this._viewModel == null)
			{
				return;
			}
			if (!this._userConfiguration.IsModUsageTrackingEnabled)
			{
				return;
			}
			this.StopGameSessionMonitor();
			string logsDirectory = Path.Combine(this._dataDirectory, "Logs");
			try
			{
				this._gameSessionMonitor = new GameSessionMonitor(logsDirectory, base.Dispatcher, this._userConfiguration, () => this._viewModel.GetActiveModUsageSnapshot());
				this._gameSessionMonitor.PromptRequired += new EventHandler(this.GameSessionMonitor_OnPromptRequired);
				this._gameSessionMonitor.RefreshPromptState();
				if (this._userConfiguration.HasPendingModUsagePrompt)
				{
					base.Dispatcher.BeginInvoke(4, new Func<Task>(this.TryShowModUsagePromptAsync));
				}
			}
			catch (Exception ex)
			{
				StatusLogService.AppendStatus(string.Format(CultureInfo.CurrentCulture, "Failed to initialize log monitor: {0}", ex.Message), true);
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000C50C File Offset: 0x0000A70C
		private void StopGameSessionMonitor()
		{
			if (this._gameSessionMonitor == null)
			{
				return;
			}
			this._gameSessionMonitor.PromptRequired -= new EventHandler(this.GameSessionMonitor_OnPromptRequired);
			this._gameSessionMonitor.Dispose();
			this._gameSessionMonitor = null;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000C540 File Offset: 0x0000A740
		private void GameSessionMonitor_OnPromptRequired([Nullable(2)] object sender, EventArgs e)
		{
			base.Dispatcher.BeginInvoke(4, new Func<Task>(this.TryShowModUsagePromptAsync));
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000C55B File Offset: 0x0000A75B
		[return: TupleElementNames(new string[]
		{
			"ModId",
			"DisplayName",
			"ConfigPath"
		})]
		[return: Nullable(new byte[]
		{
			1,
			1,
			0,
			1,
			1,
			1
		})]
		private Task<IReadOnlyList<ValueTuple<string, string, string>>> ScanForModConfigFilesAsync(MainViewModel viewModel)
		{
			return this.ScanForModConfigFilesAsync(viewModel, null);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000C568 File Offset: 0x0000A768
		[return: TupleElementNames(new string[]
		{
			"ModId",
			"DisplayName",
			"ConfigPath"
		})]
		[return: Nullable(new byte[]
		{
			1,
			1,
			0,
			1,
			1,
			1
		})]
		private Task<IReadOnlyList<ValueTuple<string, string, string>>> ScanForModConfigFilesAsync(MainViewModel viewModel, [Nullable(new byte[]
		{
			2,
			1
		})] IReadOnlyCollection<string> modIds)
		{
			MainWindow.<ScanForModConfigFilesAsync>d__296 <ScanForModConfigFilesAsync>d__;
			<ScanForModConfigFilesAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IReadOnlyList<ValueTuple<string, string, string>>>.Create();
			<ScanForModConfigFilesAsync>d__.<>4__this = this;
			<ScanForModConfigFilesAsync>d__.viewModel = viewModel;
			<ScanForModConfigFilesAsync>d__.modIds = modIds;
			<ScanForModConfigFilesAsync>d__.<>1__state = -1;
			<ScanForModConfigFilesAsync>d__.<>t__builder.Start<MainWindow.<ScanForModConfigFilesAsync>d__296>(ref <ScanForModConfigFilesAsync>d__);
			return <ScanForModConfigFilesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000C5BC File Offset: 0x0000A7BC
		[return: TupleElementNames(new string[]
		{
			"ModId",
			"DisplayName",
			"ConfigPath"
		})]
		[return: Nullable(new byte[]
		{
			1,
			1,
			0,
			1,
			1,
			1
		})]
		private Task<IReadOnlyList<ValueTuple<string, string, string>>> ScanForModConfigFilesAsync(MainViewModel viewModel, IReadOnlyList<ModListItemViewModel> candidateMods)
		{
			MainWindow.<ScanForModConfigFilesAsync>d__297 <ScanForModConfigFilesAsync>d__;
			<ScanForModConfigFilesAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IReadOnlyList<ValueTuple<string, string, string>>>.Create();
			<ScanForModConfigFilesAsync>d__.<>4__this = this;
			<ScanForModConfigFilesAsync>d__.viewModel = viewModel;
			<ScanForModConfigFilesAsync>d__.candidateMods = candidateMods;
			<ScanForModConfigFilesAsync>d__.<>1__state = -1;
			<ScanForModConfigFilesAsync>d__.<>t__builder.Start<MainWindow.<ScanForModConfigFilesAsync>d__297>(ref <ScanForModConfigFilesAsync>d__);
			return <ScanForModConfigFilesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000C610 File Offset: 0x0000A810
		[return: TupleElementNames(new string[]
		{
			"ModId",
			"ConfigPath"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		private static List<ValueTuple<string, string>> FindConfigMatches([TupleElementNames(new string[]
		{
			"ModId",
			"DisplayName"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})] IReadOnlyList<ValueTuple<string, string>> mods, IReadOnlyList<string> configPaths)
		{
			List<ValueTuple<string, string>> list = new List<ValueTuple<string, string>>();
			if (mods.Count == 0 || configPaths.Count == 0)
			{
				return list;
			}
			List<ValueTuple<string, List<string>>> list2 = Enumerable.ToList<ValueTuple<string, List<string>>>(Enumerable.Where<ValueTuple<string, List<string>>>(Enumerable.Select<string, ValueTuple<string, List<string>>>(configPaths, (string path) => new ValueTuple<string, List<string>>(path, MainWindow.BuildSearchTokens(Path.GetFileNameWithoutExtension(path) ?? string.Empty, true))), ([TupleElementNames(new string[]
			{
				"Path",
				"Tokens"
			})] ValueTuple<string, List<string>> candidate) => candidate.Item2.Count > 0));
			if (list2.Count == 0)
			{
				return list;
			}
			foreach (ValueTuple<string, string> valueTuple in mods)
			{
				List<IReadOnlyList<string>> list3 = MainWindow.BuildModSearchTokenSets(valueTuple.Item1, valueTuple.Item2);
				if (list3.Count != 0)
				{
					string text = null;
					int num = int.MaxValue;
					int num2 = -1;
					int num3 = 0;
					for (int i = 0; i < list2.Count; i++)
					{
						ValueTuple<string, List<string>> valueTuple2 = list2[i];
						bool flag = false;
						int num4 = int.MaxValue;
						int num5 = 0;
						foreach (IReadOnlyList<string> readOnlyList in list3)
						{
							int num6;
							if (MainWindow.TryCalculateMatchScore(readOnlyList, valueTuple2.Item2, out num6))
							{
								flag = true;
								if (num6 < num4)
								{
									num4 = num6;
									num5 = readOnlyList.Count;
									if (num6 == 0)
									{
										break;
									}
								}
							}
						}
						if (flag && num4 < num)
						{
							num = num4;
							text = valueTuple2.Item1;
							num2 = i;
							num3 = num5;
							if (num4 == 0)
							{
								break;
							}
						}
					}
					if (text != null && num2 >= 0 && num3 > 0 && num < num3 * MainWindow.AutomaticConfigMaxWordDistance)
					{
						list.Add(new ValueTuple<string, string>(valueTuple.Item1, text));
						list2.RemoveAt(num2);
						if (list2.Count == 0)
						{
							break;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000C80C File Offset: 0x0000AA0C
		private static bool TryCalculateMatchScore(IReadOnlyList<string> words, IReadOnlyList<string> candidateTokens, out int score)
		{
			score = 0;
			if (words.Count == 0 || candidateTokens.Count == 0)
			{
				return false;
			}
			foreach (string word in words)
			{
				int num = int.MaxValue;
				foreach (string token in candidateTokens)
				{
					int num2 = MainWindow.CalculateBestDistance(token, word, MainWindow.AutomaticConfigMaxWordDistance);
					if (num2 < num)
					{
						num = num2;
						if (num == 0)
						{
							break;
						}
					}
				}
				if (num > MainWindow.AutomaticConfigMaxWordDistance)
				{
					score = int.MaxValue;
					return false;
				}
				score += num;
			}
			return true;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000C8D0 File Offset: 0x0000AAD0
		private static int CalculateBestDistance(string token, string word, int maxDistance)
		{
			if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(word))
			{
				return int.MaxValue;
			}
			ReadOnlySpan<char> target = MemoryExtensions.AsSpan(token);
			return MainWindow.CalculateLevenshteinDistance(MemoryExtensions.AsSpan(word), target, maxDistance);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000C908 File Offset: 0x0000AB08
		[NullableContext(0)]
		private unsafe static int CalculateLevenshteinDistance(ReadOnlySpan<char> source, ReadOnlySpan<char> target, int maxDistance)
		{
			if (Math.Abs(source.Length - target.Length) > maxDistance)
			{
				return maxDistance + 1;
			}
			int length = target.Length;
			int num = length + 1;
			Span<int> span = new Span<int>(stackalloc byte[checked(unchecked((UIntPtr)num) * 4)], num);
			num = length + 1;
			Span<int> span2 = new Span<int>(stackalloc byte[checked(unchecked((UIntPtr)num) * 4)], num);
			for (int i = 0; i <= length; i++)
			{
				*span[i] = i;
			}
			for (int j = 1; j <= source.Length; j++)
			{
				*span2[0] = j;
				int num2 = *span2[0];
				char c = (char)(*source[j - 1]);
				for (int k = 1; k <= length; k++)
				{
					int num3 = (c != (char)(*target[k - 1])) ? 1 : 0;
					int num4 = *span[k] + 1;
					int num5 = *span2[k - 1] + 1;
					int num6 = *span[k - 1] + num3;
					int num7 = Math.Min(Math.Min(num4, num5), num6);
					*span2[k] = num7;
					if (num7 < num2)
					{
						num2 = num7;
					}
				}
				if (num2 > maxDistance)
				{
					return maxDistance + 1;
				}
				Span<int> span3 = span;
				span = span2;
				span2 = span3;
			}
			return *span[length];
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000CA44 File Offset: 0x0000AC44
		[NullableContext(2)]
		[return: Nullable(1)]
		private static List<IReadOnlyList<string>> BuildModSearchTokenSets(string modId, string displayName)
		{
			MainWindow.<>c__DisplayClass302_0 CS$<>8__locals1;
			CS$<>8__locals1.tokenSets = new List<IReadOnlyList<string>>();
			MainWindow.<BuildModSearchTokenSets>g__AddTokenVariations|302_0(displayName, ref CS$<>8__locals1);
			MainWindow.<BuildModSearchTokenSets>g__AddTokenVariations|302_0(modId, ref CS$<>8__locals1);
			return CS$<>8__locals1.tokenSets;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000CA74 File Offset: 0x0000AC74
		private static bool AreTokenListsEqual(IReadOnlyList<string> first, IReadOnlyList<string> second)
		{
			if (first.Count != second.Count)
			{
				return false;
			}
			for (int i = 0; i < first.Count; i++)
			{
				if (!string.Equals(first[i], second[i], 4))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000CABC File Offset: 0x0000ACBC
		private static List<string> BuildSearchTokens(string value, bool includeCombinedToken = true)
		{
			List<string> list = MainWindow.ExtractWords(value);
			if (list.Count == 0 && !string.IsNullOrWhiteSpace(value))
			{
				list.Add(value.ToLowerInvariant());
			}
			if (includeCombinedToken && list.Count > 1)
			{
				string text = string.Concat(list);
				if (!string.IsNullOrEmpty(text) && !list.Contains(text))
				{
					list.Add(text);
				}
			}
			return list;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000CB18 File Offset: 0x0000AD18
		private static List<string> ExtractWords(string value)
		{
			MainWindow.<>c__DisplayClass305_0 CS$<>8__locals1;
			CS$<>8__locals1.results = new List<string>();
			if (string.IsNullOrWhiteSpace(value))
			{
				return CS$<>8__locals1.results;
			}
			CS$<>8__locals1.seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			CS$<>8__locals1.builder = new StringBuilder();
			bool flag = false;
			char c = '\0';
			for (int i = 0; i < value.Length; i++)
			{
				char c2 = value.get_Chars(i);
				if (char.IsLetterOrDigit(c2))
				{
					if (CS$<>8__locals1.builder.Length > 0 && char.IsUpper(c2) && flag && char.IsLetter(c) && !char.IsUpper(c))
					{
						MainWindow.<ExtractWords>g__FlushBuilder|305_0(ref CS$<>8__locals1);
					}
					CS$<>8__locals1.builder.Append(char.ToLowerInvariant(c2));
					flag = true;
					c = c2;
				}
				else
				{
					MainWindow.<ExtractWords>g__FlushBuilder|305_0(ref CS$<>8__locals1);
					flag = false;
					c = '\0';
				}
			}
			MainWindow.<ExtractWords>g__FlushBuilder|305_0(ref CS$<>8__locals1);
			return CS$<>8__locals1.results;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000CBE9 File Offset: 0x0000ADE9
		private void StopModsWatcher()
		{
			if (this._modsWatcherTimer == null)
			{
				return;
			}
			this._modsWatcherTimer.Stop();
			this._modsWatcherTimer.Tick -= new EventHandler(this.ModsWatcherTimerOnTick);
			this._modsWatcherTimer = null;
			this._isAutomaticRefreshRunning = false;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000CC24 File Offset: 0x0000AE24
		private void ModsWatcherTimerOnTick([Nullable(2)] object sender, EventArgs e)
		{
			MainWindow.<ModsWatcherTimerOnTick>d__307 <ModsWatcherTimerOnTick>d__;
			<ModsWatcherTimerOnTick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<ModsWatcherTimerOnTick>d__.<>4__this = this;
			<ModsWatcherTimerOnTick>d__.<>1__state = -1;
			<ModsWatcherTimerOnTick>d__.<>t__builder.Start<MainWindow.<ModsWatcherTimerOnTick>d__307>(ref <ModsWatcherTimerOnTick>d__);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000CC5C File Offset: 0x0000AE5C
		private Task RefreshModsAsync(bool allowModDetailsRefresh = false)
		{
			MainWindow.<RefreshModsAsync>d__308 <RefreshModsAsync>d__;
			<RefreshModsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RefreshModsAsync>d__.<>4__this = this;
			<RefreshModsAsync>d__.allowModDetailsRefresh = allowModDetailsRefresh;
			<RefreshModsAsync>d__.<>1__state = -1;
			<RefreshModsAsync>d__.<>t__builder.Start<MainWindow.<RefreshModsAsync>d__308>(ref <RefreshModsAsync>d__);
			return <RefreshModsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000CCA7 File Offset: 0x0000AEA7
		private void RefreshModDetailsOnly()
		{
			if (this._viewModel == null)
			{
				return;
			}
			this._viewModel.ForceNextRefreshToLoadDetails();
			this._viewModel.RefreshInstalledModDetails();
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000CCC8 File Offset: 0x0000AEC8
		private bool TryInitializePaths()
		{
			bool flag = this.TryResolveDataDirectory();
			bool flag2 = this.TryResolveGameDirectory();
			this.TryResolveCustomShortcut();
			if (flag)
			{
				this._userConfiguration.SetDataDirectory(this._dataDirectory);
				DeveloperProfileManager.UpdateOriginalProfile(this._dataDirectory);
			}
			if (flag2)
			{
				this._userConfiguration.SetGameDirectory(this._gameDirectory);
			}
			return flag && flag2;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000CD20 File Offset: 0x0000AF20
		private bool TryResolveDataDirectory()
		{
			bool requiresDataDirectorySelection = this._userConfiguration.RequiresDataDirectorySelection;
			string dataDirectory = this._userConfiguration.DataDirectory;
			string text;
			if (!requiresDataDirectorySelection && MainWindow.TryValidateDataDirectory(dataDirectory, out this._dataDirectory, out text))
			{
				return true;
			}
			if (!string.IsNullOrWhiteSpace(dataDirectory))
			{
				this._userConfiguration.ClearDataDirectory();
			}
			string text2 = DataDirectoryLocator.Resolve();
			if (!requiresDataDirectorySelection && MainWindow.TryValidateDataDirectory(text2, out this._dataDirectory, out text))
			{
				return true;
			}
			ModManagerMessageBox.Show(requiresDataDirectorySelection ? "Select the Vintage Story data folder for this profile to enable mod management." : "The Vintage Story data folder could not be located. Please select it to enable mod management.", "Simple VS Manager", 0, 64, null, null, null);
			string description = "Select your VintagestoryData folder";
			string initialPath = this._userConfiguration.DataDirectory ?? text2;
			MainWindow.PathValidator validator;
			if ((validator = MainWindow.<>O.<0>__TryValidateDataDirectory) == null)
			{
				validator = (MainWindow.<>O.<0>__TryValidateDataDirectory = new MainWindow.PathValidator(MainWindow.TryValidateDataDirectory));
			}
			this._dataDirectory = this.PromptForDirectory(description, initialPath, validator, true);
			if (this._dataDirectory == null)
			{
				ModManagerMessageBox.Show("Mods cannot be managed until a VintagestoryData folder is selected. You can set it later from File > Set Data Folder.", "Simple VS Manager", 0, 64, null, null, null);
				return false;
			}
			return true;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000CE08 File Offset: 0x0000B008
		private bool TryResolveGameDirectory()
		{
			bool requiresGameDirectorySelection = this._userConfiguration.RequiresGameDirectorySelection;
			string gameDirectory = this._userConfiguration.GameDirectory;
			string text;
			if (!requiresGameDirectorySelection && MainWindow.TryValidateGameDirectory(gameDirectory, out this._gameDirectory, out text))
			{
				return true;
			}
			if (!string.IsNullOrWhiteSpace(gameDirectory))
			{
				this._userConfiguration.ClearGameDirectory();
			}
			string text2 = GameDirectoryLocator.Resolve();
			if (!requiresGameDirectorySelection && !string.IsNullOrWhiteSpace(text2) && MainWindow.TryValidateGameDirectory(text2, out this._gameDirectory, out text))
			{
				return true;
			}
			ModManagerMessageBox.Show(requiresGameDirectorySelection ? "Select the Vintage Story installation folder for this profile to enable game-related features." : "The Vintage Story installation folder could not be located. Please select it to enable game-related features.", "Simple VS Manager", 0, 64, null, null, null);
			string description = "Select your Vintage Story installation folder";
			string initialPath = this._userConfiguration.GameDirectory ?? (string.IsNullOrWhiteSpace(text2) ? null : text2);
			MainWindow.PathValidator validator;
			if ((validator = MainWindow.<>O.<1>__TryValidateGameDirectory) == null)
			{
				validator = (MainWindow.<>O.<1>__TryValidateGameDirectory = new MainWindow.PathValidator(MainWindow.TryValidateGameDirectory));
			}
			this._gameDirectory = this.PromptForDirectory(description, initialPath, validator, true);
			if (this._gameDirectory == null)
			{
				ModManagerMessageBox.Show("Game-related features will be unavailable until a Vintage Story installation folder is selected. You can set it later from File > Set Game Folder.", "Simple VS Manager", 0, 64, null, null, null);
				return false;
			}
			return true;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000CF04 File Offset: 0x0000B104
		private void TryResolveCustomShortcut()
		{
			string customShortcutPath = this._userConfiguration.CustomShortcutPath;
			if (string.IsNullOrWhiteSpace(customShortcutPath))
			{
				this._customShortcutPath = null;
				return;
			}
			if (File.Exists(customShortcutPath))
			{
				this._customShortcutPath = customShortcutPath;
				return;
			}
			this._customShortcutPath = null;
			this._userConfiguration.ClearCustomShortcutPath();
			ModManagerMessageBox.Show("The previously selected Vintage Story shortcut could not be found and has been cleared.", "Simple VS Manager", 0, 64, null, null, null);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000CF68 File Offset: 0x0000B168
		private void HandleViewModelInitializationFailure(Exception exception)
		{
			this._modActivityLoggingService.LogError("Failed to initialize view model", exception);
			this.DisposeCurrentViewModel();
			if (this._dataDirectory != null)
			{
				this._userConfiguration.ClearDataDirectory();
			}
			this._dataDirectory = null;
			ModManagerMessageBox.Show("Failed to initialize the mod manager:\n" + exception.Message + "\n\nYou can set the Vintage Story folders from the File menu once the application has loaded.", "Simple VS Manager", 0, 48, null, null, null);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000CFCC File Offset: 0x0000B1CC
		private Task ReloadViewModelAsync()
		{
			MainWindow.<ReloadViewModelAsync>d__315 <ReloadViewModelAsync>d__;
			<ReloadViewModelAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ReloadViewModelAsync>d__.<>4__this = this;
			<ReloadViewModelAsync>d__.<>1__state = -1;
			<ReloadViewModelAsync>d__.<>t__builder.Start<MainWindow.<ReloadViewModelAsync>d__315>(ref <ReloadViewModelAsync>d__);
			return <ReloadViewModelAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000D010 File Offset: 0x0000B210
		[return: Nullable(2)]
		private string PromptForDirectory(string description, [Nullable(2)] string initialPath, MainWindow.PathValidator validator, bool allowCancel)
		{
			string text = initialPath;
			string result;
			for (;;)
			{
				using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
				{
					Description = description,
					UseDescriptionForTitle = true,
					ShowNewFolderButton = false
				})
				{
					if (!string.IsNullOrWhiteSpace(text) && Directory.Exists(text))
					{
						folderBrowserDialog.SelectedPath = text;
					}
					if (folderBrowserDialog.ShowDialog() != 1)
					{
						if (allowCancel)
						{
							result = null;
						}
						else
						{
							if (ModManagerMessageBox.Show("You must select a folder to continue. Do you want to exit the application?", "Simple VS Manager", 4, 48, null, null, null) != 6)
							{
								continue;
							}
							result = null;
						}
					}
					else
					{
						text = folderBrowserDialog.SelectedPath;
						string text2;
						string text3;
						if (!validator(text, out text2, out text3))
						{
							ModManagerMessageBox.Show(text3 ?? "The selected folder is not valid.", "Simple VS Manager", 0, 48, null, null, null);
							continue;
						}
						result = text2;
					}
				}
				break;
			}
			return result;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000D0D8 File Offset: 0x0000B2D8
		[NullableContext(2)]
		private string PromptForConfigFile([Nullable(1)] ModListItemViewModel mod, string previousPath)
		{
			string initialConfigDirectory = this.GetInitialConfigDirectory(previousPath);
			string result;
			using (OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Title = "Select config file for " + mod.DisplayName,
				Filter = "Config files (*.json;*.yaml;*.yml)|*.json;*.yaml;*.yml|All files (*.*)|*.*",
				CheckFileExists = true,
				Multiselect = false,
				RestoreDirectory = true
			})
			{
				if (!string.IsNullOrWhiteSpace(initialConfigDirectory) && Directory.Exists(initialConfigDirectory))
				{
					openFileDialog.InitialDirectory = initialConfigDirectory;
				}
				if (!string.IsNullOrWhiteSpace(previousPath))
				{
					openFileDialog.FileName = Path.GetFileName(previousPath);
				}
				if (openFileDialog.ShowDialog() != 1)
				{
					result = null;
				}
				else
				{
					try
					{
						result = Path.GetFullPath(openFileDialog.FileName);
					}
					catch (Exception ex) when (ex is ArgumentException || ex is NotSupportedException || ex is PathTooLongException)
					{
						throw new ArgumentException("The selected configuration file path is invalid.", "previousPath", ex);
					}
				}
			}
			return result;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000D1E0 File Offset: 0x0000B3E0
		[NullableContext(2)]
		private string GetInitialConfigDirectory(string previousPath)
		{
			if (!string.IsNullOrWhiteSpace(previousPath))
			{
				try
				{
					string directoryName = Path.GetDirectoryName(previousPath);
					if (!string.IsNullOrWhiteSpace(directoryName) && Directory.Exists(directoryName))
					{
						return directoryName;
					}
				}
				catch (Exception)
				{
				}
			}
			if (string.IsNullOrWhiteSpace(this._dataDirectory))
			{
				return null;
			}
			string text = Path.Combine(this._dataDirectory, "ModConfig");
			if (Directory.Exists(text))
			{
				return text;
			}
			return this._dataDirectory;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000D258 File Offset: 0x0000B458
		[NullableContext(2)]
		private static bool TryValidateDataDirectory(string path, out string normalizedPath, out string errorMessage)
		{
			normalizedPath = null;
			errorMessage = null;
			if (string.IsNullOrWhiteSpace(path))
			{
				errorMessage = "No folder was selected.";
				return false;
			}
			try
			{
				normalizedPath = Path.GetFullPath(path);
			}
			catch (Exception)
			{
				errorMessage = "The folder path is invalid.";
				return false;
			}
			if (!Directory.Exists(normalizedPath))
			{
				errorMessage = "The folder does not exist.";
				return false;
			}
			bool flag = File.Exists(Path.Combine(normalizedPath, "clientsettings.json"));
			bool flag2 = Directory.Exists(Path.Combine(normalizedPath, "Mods"));
			bool flag3 = Directory.Exists(Path.Combine(normalizedPath, "ModConfig"));
			if (!flag && !flag2 && !flag3)
			{
				errorMessage = "The folder does not appear to be a VintagestoryData directory.";
				return false;
			}
			return true;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000D300 File Offset: 0x0000B500
		[NullableContext(2)]
		private static bool TryValidateGameDirectory(string path, out string normalizedPath, out string errorMessage)
		{
			normalizedPath = null;
			errorMessage = null;
			if (string.IsNullOrWhiteSpace(path))
			{
				errorMessage = "No folder was selected.";
				return false;
			}
			string text;
			try
			{
				text = Path.GetFullPath(path);
			}
			catch (Exception)
			{
				errorMessage = "The folder path is invalid.";
				return false;
			}
			if (File.Exists(text))
			{
				string directoryName = Path.GetDirectoryName(text);
				if (string.IsNullOrWhiteSpace(directoryName))
				{
					errorMessage = "The folder path is invalid.";
					return false;
				}
				text = directoryName;
			}
			if (!Directory.Exists(text))
			{
				errorMessage = "The folder does not exist.";
				return false;
			}
			if (GameDirectoryLocator.FindExecutable(text) == null)
			{
				errorMessage = "The folder does not contain a Vintage Story executable.";
				return false;
			}
			normalizedPath = text;
			return true;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000D394 File Offset: 0x0000B594
		private void ModsDataGrid_OnSorting(object sender, DataGridSortingEventArgs e)
		{
			if (this._viewModel == null)
			{
				return;
			}
			string columnSortMemberPath = MainWindow.GetColumnSortMemberPath(e.Column);
			if (string.IsNullOrWhiteSpace(columnSortMemberPath))
			{
				e.Handled = true;
				return;
			}
			e.Handled = true;
			ListSortDirection nextColumnSortDirection = MainWindow.GetNextColumnSortDirection(e.Column);
			this.ApplyModListSort(columnSortMemberPath, nextColumnSortDirection, true);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000D3E4 File Offset: 0x0000B5E4
		private void ModlistsDataGrid_OnSorting(object sender, DataGridSortingEventArgs e)
		{
			e.Handled = true;
			DataGrid dataGrid = sender as DataGrid;
			if (dataGrid == null)
			{
				return;
			}
			string columnSortMemberPath = MainWindow.GetColumnSortMemberPath(e.Column);
			if (!string.IsNullOrWhiteSpace(columnSortMemberPath))
			{
				ICollectionView collectionView = dataGrid.ItemsSource as ICollectionView;
				if (collectionView != null)
				{
					ListSortDirection nextColumnSortDirection = MainWindow.GetNextColumnSortDirection(e.Column);
					MainWindow.ApplyModlistsSort(dataGrid, collectionView, columnSortMemberPath, nextColumnSortDirection);
					if (dataGrid == this.LocalModlistsDataGrid)
					{
						this._userConfiguration.SetLocalModlistsSortPreference(columnSortMemberPath, nextColumnSortDirection);
						return;
					}
					if (dataGrid == this.CloudModlistsDataGrid)
					{
						this._userConfiguration.SetOnlineModlistsSortPreference(columnSortMemberPath, nextColumnSortDirection);
					}
					return;
				}
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000D46C File Offset: 0x0000B66C
		private void RestoreModlistsSortPreferences()
		{
			if (this._viewModel == null)
			{
				return;
			}
			MainWindow.RestoreModlistsSortPreference(this.LocalModlistsDataGrid, this._viewModel.LocalModlistsView, this._userConfiguration.GetLocalModlistsSortPreference());
			MainWindow.RestoreModlistsSortPreference(this.CloudModlistsDataGrid, this._viewModel.CloudModlistsView, this._userConfiguration.GetOnlineModlistsSortPreference());
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000D4C4 File Offset: 0x0000B6C4
		private static void RestoreModlistsSortPreference(DataGrid dataGrid, ICollectionView view, [TupleElementNames(new string[]
		{
			"SortMemberPath",
			"Direction"
		})] [Nullable(new byte[]
		{
			0,
			2
		})] ValueTuple<string, ListSortDirection> preference)
		{
			if (string.IsNullOrWhiteSpace(preference.Item1))
			{
				return;
			}
			string sortMemberPath = preference.Item1;
			if (!Enumerable.Any<DataGridColumn>(dataGrid.Columns, (DataGridColumn column) => string.Equals(MainWindow.GetColumnSortMemberPath(column), sortMemberPath, 4)))
			{
				return;
			}
			MainWindow.ApplyModlistsSort(dataGrid, view, sortMemberPath, preference.Item2);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000D520 File Offset: 0x0000B720
		private static void ApplyModlistsSort(DataGrid dataGrid, ICollectionView view, string sortMemberPath, ListSortDirection direction)
		{
			using (view.DeferRefresh())
			{
				view.SortDescriptions.Clear();
				view.SortDescriptions.Add(new SortDescription(sortMemberPath, direction));
			}
			foreach (DataGridColumn dataGridColumn in dataGrid.Columns)
			{
				dataGridColumn.SortDirection = (string.Equals(MainWindow.GetColumnSortMemberPath(dataGridColumn), sortMemberPath, 4) ? new ListSortDirection?(direction) : default(ListSortDirection?));
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000D5C8 File Offset: 0x0000B7C8
		[return: Nullable(2)]
		private static string GetColumnSortMemberPath(DataGridColumn column)
		{
			if (!string.IsNullOrWhiteSpace(column.SortMemberPath))
			{
				return column.SortMemberPath;
			}
			DataGridBoundColumn dataGridBoundColumn = column as DataGridBoundColumn;
			if (dataGridBoundColumn != null)
			{
				Binding binding = dataGridBoundColumn.Binding as Binding;
				if (binding != null)
				{
					PropertyPath path = binding.Path;
					if (path == null)
					{
						return null;
					}
					return path.Path;
				}
			}
			return null;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000D618 File Offset: 0x0000B818
		private static ListSortDirection GetNextColumnSortDirection(DataGridColumn column)
		{
			ListSortDirection? sortDirection = column.SortDirection;
			if (sortDirection != null)
			{
				ListSortDirection valueOrDefault = sortDirection.GetValueOrDefault();
				if (valueOrDefault != 1)
				{
					return 1;
				}
				return 0;
			}
			else
			{
				if (!MainWindow.IsDescendingFirstColumn(column))
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000D650 File Offset: 0x0000B850
		private static bool IsDescendingFirstColumn(DataGridColumn column)
		{
			object header = column.Header;
			string text = (header != null) ? header.ToString() : null;
			if (string.Equals(text, "Version", 4) || string.Equals(text, "Latest Version", 4) || string.Equals(text, "Date Modified", 4) || string.Equals(text, "VS Version", 4))
			{
				return true;
			}
			string columnSortMemberPath = MainWindow.GetColumnSortMemberPath(column);
			return string.Equals(columnSortMemberPath, "LatestVersionSortKey", 4) || string.Equals(columnSortMemberPath, "DateModified", 4) || string.Equals(columnSortMemberPath, "LastModified", 4) || string.Equals(columnSortMemberPath, "DateAdded", 4);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000D6EC File Offset: 0x0000B8EC
		private void ModsDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			DataGrid dataGrid = sender as DataGrid;
			if (dataGrid != null)
			{
				dataGrid.SelectedIndex = -1;
				dataGrid.UnselectAll();
				dataGrid.UnselectAllCells();
			}
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000D718 File Offset: 0x0000B918
		private void ModsDataGrid_OnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			MainWindow.<ModsDataGrid_OnPreviewKeyDown>d__330 <ModsDataGrid_OnPreviewKeyDown>d__;
			<ModsDataGrid_OnPreviewKeyDown>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<ModsDataGrid_OnPreviewKeyDown>d__.<>4__this = this;
			<ModsDataGrid_OnPreviewKeyDown>d__.e = e;
			<ModsDataGrid_OnPreviewKeyDown>d__.<>1__state = -1;
			<ModsDataGrid_OnPreviewKeyDown>d__.<>t__builder.Start<MainWindow.<ModsDataGrid_OnPreviewKeyDown>d__330>(ref <ModsDataGrid_OnPreviewKeyDown>d__);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000D758 File Offset: 0x0000B958
		private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			MainWindow.<MainWindow_OnPreviewKeyDown>d__331 <MainWindow_OnPreviewKeyDown>d__;
			<MainWindow_OnPreviewKeyDown>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<MainWindow_OnPreviewKeyDown>d__.<>4__this = this;
			<MainWindow_OnPreviewKeyDown>d__.e = e;
			<MainWindow_OnPreviewKeyDown>d__.<>1__state = -1;
			<MainWindow_OnPreviewKeyDown>d__.<>t__builder.Start<MainWindow.<MainWindow_OnPreviewKeyDown>d__331>(ref <MainWindow_OnPreviewKeyDown>d__);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000D798 File Offset: 0x0000B998
		private void UserReportsButton_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<UserReportsButton_OnClick>d__332 <UserReportsButton_OnClick>d__;
			<UserReportsButton_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<UserReportsButton_OnClick>d__.<>4__this = this;
			<UserReportsButton_OnClick>d__.sender = sender;
			<UserReportsButton_OnClick>d__.e = e;
			<UserReportsButton_OnClick>d__.<>1__state = -1;
			<UserReportsButton_OnClick>d__.<>t__builder.Start<MainWindow.<UserReportsButton_OnClick>d__332>(ref <UserReportsButton_OnClick>d__);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000D7E0 File Offset: 0x0000B9E0
		private void UserReportReasonsButton_OnClick(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			if (button != null)
			{
				ModListItemViewModel modListItemViewModel = button.DataContext as ModListItemViewModel;
				if (modListItemViewModel != null)
				{
					ModVersionVoteSummary userReportSummary = modListItemViewModel.UserReportSummary;
					if (userReportSummary != null)
					{
						e.Handled = true;
						new ModVoteReasonDetailsDialog(modListItemViewModel.DisplayName, userReportSummary)
						{
							Owner = this
						}.ShowDialog();
						return;
					}
				}
			}
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000D834 File Offset: 0x0000BA34
		private Task<bool> TryHandleModListKeyDownAsync(KeyEventArgs e)
		{
			MainWindow.<TryHandleModListKeyDownAsync>d__334 <TryHandleModListKeyDownAsync>d__;
			<TryHandleModListKeyDownAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<TryHandleModListKeyDownAsync>d__.<>4__this = this;
			<TryHandleModListKeyDownAsync>d__.e = e;
			<TryHandleModListKeyDownAsync>d__.<>1__state = -1;
			<TryHandleModListKeyDownAsync>d__.<>t__builder.Start<MainWindow.<TryHandleModListKeyDownAsync>d__334>(ref <TryHandleModListKeyDownAsync>d__);
			return <TryHandleModListKeyDownAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000D880 File Offset: 0x0000BA80
		private void ModsDataGrid_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (this._isApplyingPreset)
			{
				e.Handled = true;
				return;
			}
			if (e.Handled)
			{
				return;
			}
			if (!(sender is DataGrid))
			{
				return;
			}
			if (this.ShouldIgnoreRowSelection(e.OriginalSource as DependencyObject))
			{
				return;
			}
			DependencyObject source = e.OriginalSource as DependencyObject;
			if (MainWindow.FindAncestor<DataGridRow>(source) != null)
			{
				return;
			}
			if (MainWindow.FindAncestor<DataGridColumnHeader>(source) != null)
			{
				return;
			}
			if (MainWindow.FindAncestor<ScrollBar>(source) != null)
			{
				return;
			}
			this.ClearSelection(true);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000D8F4 File Offset: 0x0000BAF4
		private void ModsDataGridRow_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (this._isApplyingPreset)
			{
				e.Handled = true;
				return;
			}
			if (this.ShouldIgnoreRowSelection(e.OriginalSource as DependencyObject))
			{
				return;
			}
			DataGridRow dataGridRow = sender as DataGridRow;
			if (dataGridRow != null)
			{
				ModListItemViewModel modListItemViewModel = dataGridRow.DataContext as ModListItemViewModel;
				if (modListItemViewModel != null)
				{
					dataGridRow.Focus();
					this.HandleModRowSelection(modListItemViewModel);
					e.Handled = true;
					return;
				}
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000D954 File Offset: 0x0000BB54
		private void ModDatabaseCard_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (this._isApplyingPreset)
			{
				e.Handled = true;
				return;
			}
			if (this.ShouldIgnoreRowSelection(e.OriginalSource as DependencyObject))
			{
				return;
			}
			ListViewItem listViewItem = sender as ListViewItem;
			if (listViewItem != null)
			{
				ModListItemViewModel modListItemViewModel = listViewItem.DataContext as ModListItemViewModel;
				if (modListItemViewModel != null)
				{
					listViewItem.Focus();
					this.HandleModRowSelection(modListItemViewModel);
					e.Handled = true;
					return;
				}
			}
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000D9B4 File Offset: 0x0000BBB4
		private void ModsDataGridRow_OnLoaded(object sender, RoutedEventArgs e)
		{
			DataGridRow dataGridRow = sender as DataGridRow;
			if (dataGridRow == null)
			{
				return;
			}
			dataGridRow.DataContextChanged -= new DependencyPropertyChangedEventHandler(this.ModsDataGridRow_OnDataContextChanged);
			dataGridRow.DataContextChanged += new DependencyPropertyChangedEventHandler(this.ModsDataGridRow_OnDataContextChanged);
			MainWindow.UpdateRowModSubscription(dataGridRow, dataGridRow.DataContext as ModListItemViewModel);
			MainWindow.SetRowIsHovered(dataGridRow, dataGridRow.IsMouseOver);
			if (!this.AreHoverOverlaysSuppressed())
			{
				MainWindow.ResetRowOverlays(dataGridRow);
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000DA1C File Offset: 0x0000BC1C
		private void ModsDataGridRow_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			DataGridRow dataGridRow = sender as DataGridRow;
			if (dataGridRow == null)
			{
				return;
			}
			MainWindow.UpdateRowModSubscription(dataGridRow, e.NewValue as ModListItemViewModel);
			if (!this.AreHoverOverlaysSuppressed())
			{
				MainWindow.ResetRowOverlays(dataGridRow);
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000DA54 File Offset: 0x0000BC54
		private void ModsDataGridRow_OnUnloaded(object sender, RoutedEventArgs e)
		{
			DataGridRow dataGridRow = sender as DataGridRow;
			if (dataGridRow == null)
			{
				return;
			}
			dataGridRow.DataContextChanged -= new DependencyPropertyChangedEventHandler(this.ModsDataGridRow_OnDataContextChanged);
			MainWindow.UpdateRowModSubscription(dataGridRow, null);
			if (!this.AreHoverOverlaysSuppressed())
			{
				MainWindow.ClearRowOverlayValues(dataGridRow);
			}
			MainWindow.SetRowIsHovered(dataGridRow, false);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000DA9C File Offset: 0x0000BC9C
		private void ModsDataGridRow_OnMouseEnter(object sender, MouseEventArgs e)
		{
			DataGridRow dataGridRow = sender as DataGridRow;
			if (dataGridRow == null)
			{
				return;
			}
			MainWindow.SetRowIsHovered(dataGridRow, true);
			if (this.AreHoverOverlaysSuppressed())
			{
				return;
			}
			MainWindow.ResetRowOverlays(dataGridRow);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000DACC File Offset: 0x0000BCCC
		private void ModsDataGridRow_OnMouseLeave(object sender, MouseEventArgs e)
		{
			DataGridRow dataGridRow = sender as DataGridRow;
			if (dataGridRow == null)
			{
				return;
			}
			MainWindow.SetRowIsHovered(dataGridRow, false);
			if (this.AreHoverOverlaysSuppressed())
			{
				return;
			}
			MainWindow.ResetRowOverlays(dataGridRow);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000DAFC File Offset: 0x0000BCFC
		private static void ResetRowOverlays(DataGridRow row)
		{
			if (row.Template == null)
			{
				return;
			}
			Border border = row.Template.FindName("SelectionOverlay", row) as Border;
			Border border2 = row.Template.FindName("HoverOverlay", row) as Border;
			if (border == null && border2 == null)
			{
				return;
			}
			ModListItemViewModel modListItemViewModel = row.DataContext as ModListItemViewModel;
			if (modListItemViewModel == null)
			{
				if (border != null)
				{
					border.ClearValue(UIElement.OpacityProperty);
				}
				if (border2 != null)
				{
					border2.ClearValue(UIElement.OpacityProperty);
				}
				return;
			}
			bool flag = modListItemViewModel.IsSelected || row.IsSelected;
			bool rowIsHovered = MainWindow.GetRowIsHovered(row);
			if (border != null)
			{
				double opacity = flag ? MainWindow.SelectionOverlayOpacity : 0.0;
				border.Opacity = opacity;
			}
			if (border2 != null)
			{
				bool flag2 = rowIsHovered && !flag && !MainWindow.AreHoverEffectsDisabled(row) && !MainWindow.AreHoverOverlaysSuppressed(row);
				border2.Opacity = (flag2 ? MainWindow.HoverOverlayOpacity : 0.0);
			}
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000DBE7 File Offset: 0x0000BDE7
		private bool AreHoverOverlaysSuppressed()
		{
			MainViewModel viewModel = this._viewModel;
			if (viewModel == null || !viewModel.IsLoadingMods)
			{
				MainViewModel viewModel2 = this._viewModel;
				return viewModel2 != null && viewModel2.IsLoadingModDetails;
			}
			return true;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000DC10 File Offset: 0x0000BE10
		private static bool AreHoverOverlaysSuppressed(DataGridRow row)
		{
			MainWindow mainWindow = Window.GetWindow(row) as MainWindow;
			return mainWindow != null && mainWindow.AreHoverOverlaysSuppressed();
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000DC34 File Offset: 0x0000BE34
		private static bool AreHoverEffectsDisabled(DependencyObject dependencyObject)
		{
			return HoverEffectHelper.GetDisableHoverEffects(dependencyObject);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000DC3C File Offset: 0x0000BE3C
		private static void ClearRowOverlayValues(DataGridRow row)
		{
			if (row.Template == null)
			{
				return;
			}
			Border border = row.Template.FindName("SelectionOverlay", row) as Border;
			if (border != null)
			{
				border.ClearValue(UIElement.OpacityProperty);
			}
			Border border2 = row.Template.FindName("HoverOverlay", row) as Border;
			if (border2 != null)
			{
				border2.ClearValue(UIElement.OpacityProperty);
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000DC9C File Offset: 0x0000BE9C
		private void RefreshHoverOverlayState()
		{
			MainWindow.RefreshRowHoverOverlays(this.ModsDataGrid);
			MainWindow.RefreshRowHoverOverlays(this.CloudModlistsDataGrid);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000DCB4 File Offset: 0x0000BEB4
		[NullableContext(2)]
		private static void RefreshRowHoverOverlays(DataGrid dataGrid)
		{
			if (dataGrid == null)
			{
				return;
			}
			ItemContainerGenerator itemContainerGenerator = dataGrid.ItemContainerGenerator;
			foreach (object obj in dataGrid.Items)
			{
				DataGridRow dataGridRow = itemContainerGenerator.ContainerFromItem(obj) as DataGridRow;
				if (dataGridRow != null)
				{
					MainWindow.ResetRowOverlays(dataGridRow);
				}
			}
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000DD28 File Offset: 0x0000BF28
		private static void UpdateRowModSubscription(DataGridRow row, [Nullable(2)] ModListItemViewModel newMod)
		{
			ModListItemViewModel boundMod = MainWindow.GetBoundMod(row);
			if (boundMod != null)
			{
				PropertyChangedEventHandler boundModHandler = MainWindow.GetBoundModHandler(row);
				if (boundModHandler != null)
				{
					boundMod.PropertyChanged -= boundModHandler;
				}
			}
			if (newMod != null)
			{
				Action <>9__1;
				PropertyChangedEventHandler propertyChangedEventHandler = delegate([Nullable(2)] object _, PropertyChangedEventArgs args)
				{
					if (args.PropertyName == "IsSelected")
					{
						if (MainWindow.AreHoverOverlaysSuppressed(row))
						{
							return;
						}
						if (row.Dispatcher.CheckAccess())
						{
							MainWindow.ResetRowOverlays(row);
							return;
						}
						Dispatcher dispatcher = row.Dispatcher;
						Action action;
						if ((action = <>9__1) == null)
						{
							action = (<>9__1 = delegate()
							{
								MainWindow.ResetRowOverlays(row);
							});
						}
						dispatcher.Invoke(action);
					}
				};
				newMod.PropertyChanged += propertyChangedEventHandler;
				MainWindow.SetBoundModHandler(row, propertyChangedEventHandler);
			}
			else
			{
				MainWindow.SetBoundModHandler(row, null);
			}
			MainWindow.SetBoundMod(row, newMod);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000DDA4 File Offset: 0x0000BFA4
		private static void SetBoundMod(DataGridRow row, [Nullable(2)] ModListItemViewModel mod)
		{
			row.SetValue(MainWindow.BoundModProperty, mod);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000DDB2 File Offset: 0x0000BFB2
		[return: Nullable(2)]
		private static ModListItemViewModel GetBoundMod(DataGridRow row)
		{
			return (ModListItemViewModel)row.GetValue(MainWindow.BoundModProperty);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000DDC4 File Offset: 0x0000BFC4
		private static void SetBoundModHandler(DataGridRow row, [Nullable(2)] PropertyChangedEventHandler handler)
		{
			row.SetValue(MainWindow.BoundModHandlerProperty, handler);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000DDD2 File Offset: 0x0000BFD2
		[return: Nullable(2)]
		private static PropertyChangedEventHandler GetBoundModHandler(DataGridRow row)
		{
			return (PropertyChangedEventHandler)row.GetValue(MainWindow.BoundModHandlerProperty);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000DDE4 File Offset: 0x0000BFE4
		private static void SetRowIsHovered(DataGridRow row, bool value)
		{
			row.SetValue(MainWindow.RowIsHoveredProperty, value);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000DDF7 File Offset: 0x0000BFF7
		private static bool GetRowIsHovered(DataGridRow row)
		{
			return (bool)row.GetValue(MainWindow.RowIsHoveredProperty);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000DE0C File Offset: 0x0000C00C
		private void ModDatabasePageButton_OnClick(object sender, RoutedEventArgs e)
		{
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement != null)
			{
				ModListItemViewModel modListItemViewModel = frameworkElement.DataContext as ModListItemViewModel;
				if (modListItemViewModel != null)
				{
					ICommand openModDatabasePageCommand = modListItemViewModel.OpenModDatabasePageCommand;
					if (openModDatabasePageCommand != null && openModDatabasePageCommand.CanExecute(null))
					{
						openModDatabasePageCommand.Execute(null);
					}
				}
			}
			e.Handled = true;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000DE54 File Offset: 0x0000C054
		private void SelectedModCopyForServerButton_OnClick(object sender, RoutedEventArgs e)
		{
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement != null)
			{
				ModListItemViewModel modListItemViewModel = frameworkElement.DataContext as ModListItemViewModel;
				if (modListItemViewModel != null)
				{
					string text = ServerCommandBuilder.TryBuildInstallCommand(modListItemViewModel.ModId, modListItemViewModel.Version);
					if (string.IsNullOrWhiteSpace(text))
					{
						return;
					}
					try
					{
						Clipboard.SetDataObject(text, true, 10, 100);
						string text2 = text.Trim();
						string message = "Copied " + text2;
						MainViewModel viewModel = this._viewModel;
						if (viewModel != null)
						{
							viewModel.ReportStatus(message, false);
						}
					}
					catch (ExternalException ex)
					{
						string message2 = "Failed to copy server install command for " + modListItemViewModel.DisplayName + ": " + ex.Message;
						MainViewModel viewModel2 = this._viewModel;
						if (viewModel2 != null)
						{
							viewModel2.ReportStatus(message2, true);
						}
						ModManagerMessageBox.Show(this, "Failed to copy the server install command. Please try again.", "Simple VS Manager", 0, 16, null, null, null);
					}
					return;
				}
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000DF2C File Offset: 0x0000C12C
		private void EditConfigButton_OnClick(object sender, RoutedEventArgs e)
		{
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement != null)
			{
				ModListItemViewModel modListItemViewModel = frameworkElement.DataContext as ModListItemViewModel;
				if (modListItemViewModel != null)
				{
					e.Handled = true;
					if (string.IsNullOrWhiteSpace(modListItemViewModel.ModId))
					{
						return;
					}
					List<string> list = Enumerable.ToList<string>(Enumerable.Distinct<string>(Enumerable.Where<string>(this._userConfiguration.GetModConfigPaths(modListItemViewModel.ModId), (string path) => !string.IsNullOrWhiteSpace(path)), StringComparer.OrdinalIgnoreCase));
					try
					{
						for (int i = list.Count - 1; i >= 0; i--)
						{
							string text = list[i];
							if (!File.Exists(text))
							{
								list.RemoveAt(i);
								this._userConfiguration.RemoveModConfigPath(modListItemViewModel.ModId, text, false);
							}
						}
						if (list.Count == 0)
						{
							string text2 = this.PromptForConfigFile(modListItemViewModel, Enumerable.FirstOrDefault<string>(list));
							if (text2 == null)
							{
								return;
							}
							list.Add(text2);
							this._userConfiguration.SetModConfigPaths(modListItemViewModel.ModId, list, null);
							this.UpdateSelectedModEditConfigButton(modListItemViewModel);
						}
					}
					catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException)
					{
						ModManagerMessageBox.Show("Failed to store the configuration path:\n" + ex.Message, "Simple VS Manager", 0, 16, null, null, null);
						return;
					}
					try
					{
						ModConfigEditorViewModel modConfigEditorViewModel = new ModConfigEditorViewModel(modListItemViewModel.DisplayName, list);
						if (new ModConfigEditorWindow(modConfigEditorViewModel)
						{
							Owner = this
						}.ShowDialog().GetValueOrDefault())
						{
							List<string> list2 = Enumerable.ToList<string>(Enumerable.Where<string>(modConfigEditorViewModel.ConfigPaths, (string path) => !string.IsNullOrWhiteSpace(path)));
							try
							{
								if (list2.Count == 0)
								{
									this._userConfiguration.RemoveModConfigPath(modListItemViewModel.ModId, false);
								}
								else
								{
									this._userConfiguration.SetModConfigPaths(modListItemViewModel.ModId, list2, null);
								}
								this.UpdateSelectedModEditConfigButton(modListItemViewModel);
								MainViewModel viewModel = this._viewModel;
								if (viewModel != null)
								{
									viewModel.ReportStatus("Saved config for " + modListItemViewModel.DisplayName + ".", false);
								}
							}
							catch (Exception ex2) when (ex2 is IOException || ex2 is UnauthorizedAccessException || ex2 is ArgumentException)
							{
								ModManagerMessageBox.Show("Failed to store the configuration path:\n" + ex2.Message, "Simple VS Manager", 0, 16, null, null, null);
							}
						}
					}
					catch (Exception ex3) when (ex3 is IOException || ex3 is UnauthorizedAccessException || ex3 is JsonException || ex3 is YamlException)
					{
						ModManagerMessageBox.Show("Failed to open the configuration file:\n" + ex3.Message, "Simple VS Manager", 0, 16, null, null, null);
						this._userConfiguration.RemoveModConfigPath(modListItemViewModel.ModId, false);
						this.UpdateSelectedModEditConfigButton(modListItemViewModel);
					}
					return;
				}
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000E280 File Offset: 0x0000C480
		private void DeleteModButton_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<DeleteModButton_OnClick>d__360 <DeleteModButton_OnClick>d__;
			<DeleteModButton_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<DeleteModButton_OnClick>d__.<>4__this = this;
			<DeleteModButton_OnClick>d__.sender = sender;
			<DeleteModButton_OnClick>d__.e = e;
			<DeleteModButton_OnClick>d__.<>1__state = -1;
			<DeleteModButton_OnClick>d__.<>t__builder.Start<MainWindow.<DeleteModButton_OnClick>d__360>(ref <DeleteModButton_OnClick>d__);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000E2C7 File Offset: 0x0000C4C7
		private void CloseModInfoButton_OnClick(object sender, RoutedEventArgs e)
		{
			this.ClearSelection(true);
			e.Handled = true;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000E2D8 File Offset: 0x0000C4D8
		private Task DeleteSelectedModsAsync()
		{
			MainWindow.<DeleteSelectedModsAsync>d__362 <DeleteSelectedModsAsync>d__;
			<DeleteSelectedModsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DeleteSelectedModsAsync>d__.<>4__this = this;
			<DeleteSelectedModsAsync>d__.<>1__state = -1;
			<DeleteSelectedModsAsync>d__.<>t__builder.Start<MainWindow.<DeleteSelectedModsAsync>d__362>(ref <DeleteSelectedModsAsync>d__);
			return <DeleteSelectedModsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000E31C File Offset: 0x0000C51C
		private Task DeleteSingleModAsync(ModListItemViewModel mod)
		{
			MainWindow.<DeleteSingleModAsync>d__363 <DeleteSingleModAsync>d__;
			<DeleteSingleModAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DeleteSingleModAsync>d__.<>4__this = this;
			<DeleteSingleModAsync>d__.mod = mod;
			<DeleteSingleModAsync>d__.<>1__state = -1;
			<DeleteSingleModAsync>d__.<>t__builder.Start<MainWindow.<DeleteSingleModAsync>d__363>(ref <DeleteSingleModAsync>d__);
			return <DeleteSingleModAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000E368 File Offset: 0x0000C568
		private Task DeleteMultipleModsAsync(IReadOnlyList<ModListItemViewModel> mods)
		{
			MainWindow.<DeleteMultipleModsAsync>d__364 <DeleteMultipleModsAsync>d__;
			<DeleteMultipleModsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DeleteMultipleModsAsync>d__.<>4__this = this;
			<DeleteMultipleModsAsync>d__.mods = mods;
			<DeleteMultipleModsAsync>d__.<>1__state = -1;
			<DeleteMultipleModsAsync>d__.<>t__builder.Start<MainWindow.<DeleteMultipleModsAsync>d__364>(ref <DeleteMultipleModsAsync>d__);
			return <DeleteMultipleModsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000E3B4 File Offset: 0x0000C5B4
		private bool TryDeleteModAtPath(ModListItemViewModel mod, string modPath)
		{
			bool flag = false;
			try
			{
				if (Directory.Exists(modPath))
				{
					Directory.Delete(modPath, true);
					flag = true;
				}
				else if (File.Exists(modPath))
				{
					File.Delete(modPath);
					flag = true;
				}
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(18, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to delete ");
				defaultInterpolatedStringHandler.AppendFormatted(mod.DisplayName);
				defaultInterpolatedStringHandler.AppendLiteral(":");
				defaultInterpolatedStringHandler.AppendFormatted(Environment.NewLine);
				defaultInterpolatedStringHandler.AppendFormatted(ex.Message);
				ModManagerMessageBox.Show(defaultInterpolatedStringHandler.ToStringAndClear(), "Simple VS Manager", 0, 16, null, null, null);
				return false;
			}
			if (!flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(63, 3);
				defaultInterpolatedStringHandler2.AppendLiteral("The mod could not be found at:");
				defaultInterpolatedStringHandler2.AppendFormatted(Environment.NewLine);
				defaultInterpolatedStringHandler2.AppendFormatted(modPath);
				defaultInterpolatedStringHandler2.AppendFormatted(Environment.NewLine);
				defaultInterpolatedStringHandler2.AppendLiteral("It may have already been removed.");
				ModManagerMessageBox.Show(defaultInterpolatedStringHandler2.ToStringAndClear(), "Simple VS Manager", 0, 64, null, null, null);
				return false;
			}
			this._userConfiguration.RemoveModConfigPath(mod.ModId, true);
			this._userConfiguration.ClearDependencyWarningOverride(mod.ModId);
			ModActivityLoggingService modActivityLoggingService = this._modActivityLoggingService;
			string modName;
			if ((modName = mod.DisplayName) == null)
			{
				modName = (mod.ModId ?? "Unknown");
			}
			modActivityLoggingService.LogModDeletion(modName);
			return true;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000E538 File Offset: 0x0000C738
		private void FixModButton_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<FixModButton_OnClick>d__366 <FixModButton_OnClick>d__;
			<FixModButton_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<FixModButton_OnClick>d__.<>4__this = this;
			<FixModButton_OnClick>d__.sender = sender;
			<FixModButton_OnClick>d__.e = e;
			<FixModButton_OnClick>d__.<>1__state = -1;
			<FixModButton_OnClick>d__.<>t__builder.Start<MainWindow.<FixModButton_OnClick>d__366>(ref <FixModButton_OnClick>d__);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000E580 File Offset: 0x0000C780
		[return: TupleElementNames(new string[]
		{
			"Success",
			"Changed",
			"Message"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		private Task<ValueTuple<bool, bool, string>> ResolveDependencyTreeAsync(ModDependencyInfo dependency, ISet<string> modsToRefresh)
		{
			MainWindow.<ResolveDependencyTreeAsync>d__367 <ResolveDependencyTreeAsync>d__;
			<ResolveDependencyTreeAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ValueTuple<bool, bool, string>>.Create();
			<ResolveDependencyTreeAsync>d__.<>4__this = this;
			<ResolveDependencyTreeAsync>d__.dependency = dependency;
			<ResolveDependencyTreeAsync>d__.modsToRefresh = modsToRefresh;
			<ResolveDependencyTreeAsync>d__.<>1__state = -1;
			<ResolveDependencyTreeAsync>d__.<>t__builder.Start<MainWindow.<ResolveDependencyTreeAsync>d__367>(ref <ResolveDependencyTreeAsync>d__);
			return <ResolveDependencyTreeAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000E5D4 File Offset: 0x0000C7D4
		private Task EnableModDespiteDependencyWarningAsync(ModListItemViewModel mod)
		{
			MainWindow.<EnableModDespiteDependencyWarningAsync>d__368 <EnableModDespiteDependencyWarningAsync>d__;
			<EnableModDespiteDependencyWarningAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<EnableModDespiteDependencyWarningAsync>d__.<>4__this = this;
			<EnableModDespiteDependencyWarningAsync>d__.mod = mod;
			<EnableModDespiteDependencyWarningAsync>d__.<>1__state = -1;
			<EnableModDespiteDependencyWarningAsync>d__.<>t__builder.Start<MainWindow.<EnableModDespiteDependencyWarningAsync>d__368>(ref <EnableModDespiteDependencyWarningAsync>d__);
			return <EnableModDespiteDependencyWarningAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000E620 File Offset: 0x0000C820
		private void InstallModButton_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<InstallModButton_OnClick>d__369 <InstallModButton_OnClick>d__;
			<InstallModButton_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<InstallModButton_OnClick>d__.<>4__this = this;
			<InstallModButton_OnClick>d__.sender = sender;
			<InstallModButton_OnClick>d__.e = e;
			<InstallModButton_OnClick>d__.<>1__state = -1;
			<InstallModButton_OnClick>d__.<>t__builder.Start<MainWindow.<InstallModButton_OnClick>d__369>(ref <InstallModButton_OnClick>d__);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000E668 File Offset: 0x0000C868
		[return: TupleElementNames(new string[]
		{
			"Success",
			"Message"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		private Task<ValueTuple<bool, string>> InstallOrUpdateDependencyAsync(ModDependencyInfo dependency, [Nullable(2)] ModListItemViewModel installedMod)
		{
			MainWindow.<InstallOrUpdateDependencyAsync>d__370 <InstallOrUpdateDependencyAsync>d__;
			<InstallOrUpdateDependencyAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ValueTuple<bool, string>>.Create();
			<InstallOrUpdateDependencyAsync>d__.<>4__this = this;
			<InstallOrUpdateDependencyAsync>d__.dependency = dependency;
			<InstallOrUpdateDependencyAsync>d__.installedMod = installedMod;
			<InstallOrUpdateDependencyAsync>d__.<>1__state = -1;
			<InstallOrUpdateDependencyAsync>d__.<>t__builder.Start<MainWindow.<InstallOrUpdateDependencyAsync>d__370>(ref <InstallOrUpdateDependencyAsync>d__);
			return <InstallOrUpdateDependencyAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000E6BC File Offset: 0x0000C8BC
		[return: Nullable(2)]
		private static ModReleaseInfo SelectReleaseForDependency(ModDependencyInfo dependency, ModDatabaseInfo info)
		{
			if (info == null)
			{
				return null;
			}
			IReadOnlyList<ModReleaseInfo> readOnlyList = info.Releases ?? Array.Empty<ModReleaseInfo>();
			if (readOnlyList.Count == 0)
			{
				return null;
			}
			foreach (ModReleaseInfo modReleaseInfo in readOnlyList)
			{
				if (modReleaseInfo.IsCompatibleWithInstalledGame && VersionStringUtility.SatisfiesMinimumVersion(dependency.Version, modReleaseInfo.Version))
				{
					return modReleaseInfo;
				}
			}
			return null;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000E740 File Offset: 0x0000C940
		[return: Nullable(2)]
		private static ModReleaseInfo SelectReleaseForInstall(ModListItemViewModel mod)
		{
			return mod.LatestCompatibleRelease;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000E748 File Offset: 0x0000C948
		private void UpdateModButton_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<UpdateModButton_OnClick>d__373 <UpdateModButton_OnClick>d__;
			<UpdateModButton_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<UpdateModButton_OnClick>d__.<>4__this = this;
			<UpdateModButton_OnClick>d__.sender = sender;
			<UpdateModButton_OnClick>d__.e = e;
			<UpdateModButton_OnClick>d__.<>1__state = -1;
			<UpdateModButton_OnClick>d__.<>t__builder.Start<MainWindow.<UpdateModButton_OnClick>d__373>(ref <UpdateModButton_OnClick>d__);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000E790 File Offset: 0x0000C990
		private void SelectedModVersionComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			MainWindow.<SelectedModVersionComboBox_OnSelectionChanged>d__374 <SelectedModVersionComboBox_OnSelectionChanged>d__;
			<SelectedModVersionComboBox_OnSelectionChanged>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<SelectedModVersionComboBox_OnSelectionChanged>d__.<>4__this = this;
			<SelectedModVersionComboBox_OnSelectionChanged>d__.sender = sender;
			<SelectedModVersionComboBox_OnSelectionChanged>d__.<>1__state = -1;
			<SelectedModVersionComboBox_OnSelectionChanged>d__.<>t__builder.Start<MainWindow.<SelectedModVersionComboBox_OnSelectionChanged>d__374>(ref <SelectedModVersionComboBox_OnSelectionChanged>d__);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000E7D0 File Offset: 0x0000C9D0
		private void SelectedModVersionComboBox_OnDropDownOpened(object sender, EventArgs e)
		{
			MainWindow.<>c__DisplayClass375_0 CS$<>8__locals1 = new MainWindow.<>c__DisplayClass375_0();
			CS$<>8__locals1.comboBox = (sender as ComboBox);
			if (CS$<>8__locals1.comboBox == null)
			{
				return;
			}
			CS$<>8__locals1.comboBox.Dispatcher.BeginInvoke(new Action(CS$<>8__locals1.<SelectedModVersionComboBox_OnDropDownOpened>g__ScrollToTop|0), 4, Array.Empty<object>());
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000E81C File Offset: 0x0000CA1C
		private void UpdateAllModsMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<UpdateAllModsMenuItem_OnClick>d__376 <UpdateAllModsMenuItem_OnClick>d__;
			<UpdateAllModsMenuItem_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<UpdateAllModsMenuItem_OnClick>d__.<>4__this = this;
			<UpdateAllModsMenuItem_OnClick>d__.<>1__state = -1;
			<UpdateAllModsMenuItem_OnClick>d__.<>t__builder.Start<MainWindow.<UpdateAllModsMenuItem_OnClick>d__376>(ref <UpdateAllModsMenuItem_OnClick>d__);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000E854 File Offset: 0x0000CA54
		private void CheckModsCompatibilityMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<CheckModsCompatibilityMenuItem_OnClick>d__377 <CheckModsCompatibilityMenuItem_OnClick>d__;
			<CheckModsCompatibilityMenuItem_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<CheckModsCompatibilityMenuItem_OnClick>d__.<>4__this = this;
			<CheckModsCompatibilityMenuItem_OnClick>d__.<>1__state = -1;
			<CheckModsCompatibilityMenuItem_OnClick>d__.<>t__builder.Start<MainWindow.<CheckModsCompatibilityMenuItem_OnClick>d__377>(ref <CheckModsCompatibilityMenuItem_OnClick>d__);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000E88C File Offset: 0x0000CA8C
		private static MainWindow.CompatibilityEvaluation EvaluateCompatibility(ModListItemViewModel mod, string targetVersion, string displayName, bool requireExactMatch)
		{
			string text = string.IsNullOrWhiteSpace(mod.Version) ? "Unknown" : mod.Version;
			ModVersionOptionViewModel modVersionOptionViewModel = Enumerable.FirstOrDefault<ModVersionOptionViewModel>(mod.VersionOptions, (ModVersionOptionViewModel option) => option != null && option.IsInstalled);
			ModReleaseInfo modReleaseInfo = (modVersionOptionViewModel != null) ? modVersionOptionViewModel.Release : null;
			ModReleaseInfo modReleaseInfo2 = Enumerable.FirstOrDefault<ModReleaseInfo>(Enumerable.ToList<ModReleaseInfo>(Enumerable.Select<IGrouping<string, ModReleaseInfo>, ModReleaseInfo>(Enumerable.GroupBy<ModReleaseInfo, string>(Enumerable.Where<ModReleaseInfo>(Enumerable.Select<ModVersionOptionViewModel, ModReleaseInfo>(mod.VersionOptions, (ModVersionOptionViewModel option) => option.Release), (ModReleaseInfo release) => release != null), (ModReleaseInfo release) => release.Version, StringComparer.OrdinalIgnoreCase), (IGrouping<string, ModReleaseInfo> group) => Enumerable.First<ModReleaseInfo>(group))), (ModReleaseInfo release) => MainWindow.ReleaseSupportsVersion(release, targetVersion, requireExactMatch));
			if (modReleaseInfo != null)
			{
				if (MainWindow.ReleaseSupportsVersion(modReleaseInfo, targetVersion, requireExactMatch))
				{
					return MainWindow.CompatibilityEvaluation.Compatible;
				}
				if (modReleaseInfo2 != null && !string.Equals(modReleaseInfo2.Version, modReleaseInfo.Version, 5))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(98, 4);
					defaultInterpolatedStringHandler.AppendFormatted(displayName);
					defaultInterpolatedStringHandler.AppendLiteral(": Installed version ");
					defaultInterpolatedStringHandler.AppendFormatted(modReleaseInfo.Version);
					defaultInterpolatedStringHandler.AppendLiteral(" is not marked as compatible with Vintage Story ");
					defaultInterpolatedStringHandler.AppendFormatted(targetVersion);
					defaultInterpolatedStringHandler.AppendLiteral(". Update to version ");
					defaultInterpolatedStringHandler.AppendFormatted(modReleaseInfo2.Version);
					defaultInterpolatedStringHandler.AppendLiteral(" or later.");
					return MainWindow.CompatibilityEvaluation.Incompatible(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				IReadOnlyList<string> gameVersionTags = modReleaseInfo.GameVersionTags;
				if (gameVersionTags != null && gameVersionTags.Count > 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
					defaultInterpolatedStringHandler2..ctor(101, 3);
					defaultInterpolatedStringHandler2.AppendFormatted(displayName);
					defaultInterpolatedStringHandler2.AppendLiteral(": Installed version ");
					defaultInterpolatedStringHandler2.AppendFormatted(text);
					defaultInterpolatedStringHandler2.AppendLiteral(" is not marked as compatible with Vintage Story ");
					defaultInterpolatedStringHandler2.AppendFormatted(targetVersion);
					defaultInterpolatedStringHandler2.AppendLiteral(". No compatible update was found.");
					return MainWindow.CompatibilityEvaluation.Incompatible(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
			}
			if (modReleaseInfo2 != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3;
				defaultInterpolatedStringHandler3..ctor(98, 4);
				defaultInterpolatedStringHandler3.AppendFormatted(displayName);
				defaultInterpolatedStringHandler3.AppendLiteral(": Installed version ");
				defaultInterpolatedStringHandler3.AppendFormatted(text);
				defaultInterpolatedStringHandler3.AppendLiteral(" is not marked as compatible with Vintage Story ");
				defaultInterpolatedStringHandler3.AppendFormatted(targetVersion);
				defaultInterpolatedStringHandler3.AppendLiteral(". Update to version ");
				defaultInterpolatedStringHandler3.AppendFormatted(modReleaseInfo2.Version);
				defaultInterpolatedStringHandler3.AppendLiteral(" or later.");
				return MainWindow.CompatibilityEvaluation.Incompatible(defaultInterpolatedStringHandler3.ToStringAndClear());
			}
			IEnumerable<ModDependencyInfo> enumerable = mod.Dependencies ?? Array.Empty<ModDependencyInfo>();
			bool flag = false;
			foreach (ModDependencyInfo modDependencyInfo in enumerable)
			{
				if (modDependencyInfo != null && modDependencyInfo.IsGameOrCoreDependency && !string.IsNullOrWhiteSpace(modDependencyInfo.Version))
				{
					flag = true;
					if (!VersionStringUtility.SatisfiesMinimumVersion(modDependencyInfo.Version, targetVersion))
					{
						return MainWindow.CompatibilityEvaluation.Incompatible(displayName + ": Requires Vintage Story " + modDependencyInfo.Version + " or newer.");
					}
				}
			}
			if (flag)
			{
				return MainWindow.CompatibilityEvaluation.Compatible;
			}
			return MainWindow.CompatibilityEvaluation.Unknown(displayName + ": No compatibility metadata is available for Vintage Story " + targetVersion + ".");
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000EC08 File Offset: 0x0000CE08
		private static bool ReleaseSupportsVersion(ModReleaseInfo release, string targetVersion, bool requireExactMatch)
		{
			IReadOnlyList<string> gameVersionTags = release.GameVersionTags;
			if (gameVersionTags == null || gameVersionTags.Count <= 0)
			{
				return false;
			}
			foreach (string text in release.GameVersionTags)
			{
				if (!string.IsNullOrWhiteSpace(text) && VersionStringUtility.SupportsVersion(text, targetVersion, requireExactMatch))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000EC7C File Offset: 0x0000CE7C
		private void ModsMenuItem_OnSubmenuOpened(object sender, RoutedEventArgs e)
		{
			MainWindow.<ModsMenuItem_OnSubmenuOpened>d__380 <ModsMenuItem_OnSubmenuOpened>d__;
			<ModsMenuItem_OnSubmenuOpened>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<ModsMenuItem_OnSubmenuOpened>d__.<>4__this = this;
			<ModsMenuItem_OnSubmenuOpened>d__.<>1__state = -1;
			<ModsMenuItem_OnSubmenuOpened>d__.<>t__builder.Start<MainWindow.<ModsMenuItem_OnSubmenuOpened>d__380>(ref <ModsMenuItem_OnSubmenuOpened>d__);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000ECB4 File Offset: 0x0000CEB4
		private void DeleteCachedModsMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<DeleteCachedModsMenuItem_OnClick>d__381 <DeleteCachedModsMenuItem_OnClick>d__;
			<DeleteCachedModsMenuItem_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<DeleteCachedModsMenuItem_OnClick>d__.<>4__this = this;
			<DeleteCachedModsMenuItem_OnClick>d__.<>1__state = -1;
			<DeleteCachedModsMenuItem_OnClick>d__.<>t__builder.Start<MainWindow.<DeleteCachedModsMenuItem_OnClick>d__381>(ref <DeleteCachedModsMenuItem_OnClick>d__);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000ECEC File Offset: 0x0000CEEC
		private void SaveInstalledModsPdfMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			if (this._viewModel == null)
			{
				ModManagerMessageBox.Show("Mods are still loading. Please try again once loading is complete.", "Simple VS Manager", 0, 64, null, null, null);
				return;
			}
			IReadOnlyList<ModListItemViewModel> installedModsSnapshot = this._viewModel.GetInstalledModsSnapshot();
			if (installedModsSnapshot.Count == 0)
			{
				ModManagerMessageBox.Show("No installed mods were found to include in the PDF.", "Simple VS Manager", 0, 64, null, null, null);
				return;
			}
			List<ModConfigOption> list = this.BuildModConfigOptions(true);
			string defaultListName = MainWindow.BuildCloudModlistName();
			IEnumerable<ModConfigOption> configOptions = list;
			string uploaderNameForPdf = this.GetUploaderNameForPdf();
			string defaultVersion = null;
			MainViewModel viewModel = this._viewModel;
			SaveInstalledModsDialog saveInstalledModsDialog = new SaveInstalledModsDialog(defaultListName, configOptions, uploaderNameForPdf, defaultVersion, (viewModel != null) ? viewModel.InstalledGameVersion : null, SaveInstalledModsDialogResult.SavePdf)
			{
				Owner = this
			};
			if (!saveInstalledModsDialog.ShowDialog().GetValueOrDefault())
			{
				return;
			}
			string listName = saveInstalledModsDialog.ListName;
			string version = saveInstalledModsDialog.Version;
			string description = saveInstalledModsDialog.Description;
			string text = saveInstalledModsDialog.CreatedBy;
			if (string.IsNullOrWhiteSpace(text))
			{
				text = this.GetUploaderNameForPdf();
			}
			else
			{
				text = text.Trim();
			}
			string gameVersion = this.ResolveGameVersion(saveInstalledModsDialog.VintageStoryVersion);
			IReadOnlyList<ModConfigOption> selectedConfigOptions = saveInstalledModsDialog.GetSelectedConfigOptions();
			Dictionary<string, IReadOnlyList<ModConfigurationSnapshot>> includedConfigurations = this.TryReadModConfigurations(selectedConfigOptions);
			this.TrySaveInstalledModsPdf(listName, version, description, text, includedConfigurations, gameVersion, installedModsSnapshot);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000EDFC File Offset: 0x0000CFFC
		[NullableContext(2)]
		private bool TrySaveInstalledModsPdf([Nullable(1)] string listName, string version, string description, [Nullable(1)] string uploaderName, [Nullable(new byte[]
		{
			2,
			1,
			1,
			1
		})] IReadOnlyDictionary<string, IReadOnlyList<ModConfigurationSnapshot>> includedConfigurations, string gameVersion, [Nullable(new byte[]
		{
			2,
			1
		})] IReadOnlyList<ModListItemViewModel> preFetchedMods = null)
		{
			if (this._viewModel == null)
			{
				ModManagerMessageBox.Show("Mods are still loading. Please try again once loading is complete.", "Simple VS Manager", 0, 64, null, null, null);
				return false;
			}
			IReadOnlyList<ModListItemViewModel> readOnlyList = preFetchedMods ?? this._viewModel.GetInstalledModsSnapshot();
			if (readOnlyList.Count == 0)
			{
				ModManagerMessageBox.Show("No installed mods were found to include in the PDF.", "Simple VS Manager", 0, 64, null, null, null);
				return false;
			}
			string text3;
			try
			{
				string text = this.EnsureModListDirectory();
				string text2 = MainWindow.BuildSuggestedFileName(listName, "Modlist");
				text3 = Path.Combine(text, text2 + ".pdf");
				if (File.Exists(text3))
				{
					string messageBoxText = "A modlist PDF named \"" + Path.GetFileName(text3) + "\" already exists in the Modlists folder. Do you want to replace it?";
					if (ModManagerMessageBox.Show(this, messageBoxText, "Replace Modlist PDF", 4, 32, null, null, null) != 6)
					{
						return false;
					}
				}
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is NotSupportedException || ex is SecurityException)
			{
				ModManagerMessageBox.Show("Failed to prepare the Modlists folder:\n" + ex.Message, "Simple VS Manager", 0, 16, null, null, null);
				return false;
			}
			string entryName = string.IsNullOrWhiteSpace(listName) ? "Installed Mods" : listName.Trim();
			string gameVersion2 = this.ResolveGameVersion(gameVersion);
			SerializablePreset serializablePreset = this.BuildSerializablePreset(entryName, true, true, includedConfigurations, gameVersion2);
			serializablePreset.Description = (string.IsNullOrWhiteSpace(description) ? null : description.Trim());
			serializablePreset.Version = (string.IsNullOrWhiteSpace(version) ? null : version.Trim());
			serializablePreset.Uploader = (string.IsNullOrWhiteSpace(uploaderName) ? null : uploaderName.Trim());
			if (!string.IsNullOrWhiteSpace(listName))
			{
				serializablePreset.Name = listName.Trim();
			}
			SerializableConfigList configList = MainWindow.BuildSerializableConfigList(includedConfigurations);
			string uploaderName2 = string.IsNullOrWhiteSpace(uploaderName) ? this.GetUploaderNameForPdf() : uploaderName.Trim();
			try
			{
				MainWindow.GenerateInstalledModsPdf(text3, listName, version, description, uploaderName2, gameVersion2, readOnlyList, serializablePreset, configList);
				this._viewModel.ReportStatus("Saved installed mods PDF to \"" + text3 + "\".", false);
				ModManagerMessageBox.Show("Saved installed mods PDF successfully.", "Simple VS Manager", 0, 64, null, null, null);
				return true;
			}
			catch (Exception ex2) when (ex2 is IOException || ex2 is UnauthorizedAccessException || ex2 is NotSupportedException)
			{
				ModManagerMessageBox.Show("Failed to save the PDF:\n" + ex2.Message, "Simple VS Manager", 0, 16, null, null, null);
			}
			catch (Exception ex3)
			{
				ModManagerMessageBox.Show("Failed to generate the PDF:\n" + ex3.Message, "Simple VS Manager", 0, 16, null, null, null);
			}
			return false;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000F0D8 File Offset: 0x0000D2D8
		private void ManagerDataFolderMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			string managerDataDirectory = MainWindow.GetManagerDataDirectory();
			if (string.IsNullOrWhiteSpace(managerDataDirectory))
			{
				ModManagerMessageBox.Show("The manager data folder is not available on this system.", "Simple VS Manager", 0, 48, null, null, null);
				return;
			}
			try
			{
				Directory.CreateDirectory(managerDataDirectory);
			}
			catch (Exception ex)
			{
				ModManagerMessageBox.Show("Failed to open the manager data folder:\n" + ex.Message, "Simple VS Manager", 0, 16, null, null, null);
				return;
			}
			MainWindow.OpenFolder(managerDataDirectory, "manager data");
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000F154 File Offset: 0x0000D354
		private void ManagerModDbPageMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			this.OpenManagerModDatabasePage();
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000F15C File Offset: 0x0000D35C
		private void ChangeManagerFolderMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			string managerDataDirectory = ModCacheLocator.GetManagerDataDirectory();
			if (string.IsNullOrWhiteSpace(managerDataDirectory))
			{
				ModManagerMessageBox.Show("Cannot determine the current manager data folder location.", "Simple VS Manager", 0, 16, null, null, null);
				return;
			}
			ChangeManagerFolderDialog changeManagerFolderDialog = new ChangeManagerFolderDialog(this, managerDataDirectory);
			if (!changeManagerFolderDialog.ShowDialog().GetValueOrDefault())
			{
				return;
			}
			if (changeManagerFolderDialog.Result == ChangeManagerFolderDialogResult.Reset)
			{
				this.HandleResetManagerFolder(managerDataDirectory);
				return;
			}
			if (changeManagerFolderDialog.Result != ChangeManagerFolderDialogResult.Yes)
			{
				return;
			}
			using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
			{
				Description = "Select the new location for the \"Simple VS Manager\" folder.\nThe folder will be created if it doesn't exist.",
				ShowNewFolderButton = true,
				UseDescriptionForTitle = true
			})
			{
				if (folderBrowserDialog.ShowDialog() == 1)
				{
					string selectedPath = folderBrowserDialog.SelectedPath;
					if (!string.IsNullOrWhiteSpace(selectedPath))
					{
						string text = Path.Combine(selectedPath, "Simple VS Manager");
						if (!Directory.Exists(text) || string.Equals(managerDataDirectory, text, 5) || ModManagerMessageBox.Show("The folder \"" + text + "\" already exists.\n\nDo you want to merge with the existing folder?\n(Existing files with the same name will be overwritten)", "Folder Already Exists", 4, 48, null, null, null) == 6)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Move manager folder from:\n");
							defaultInterpolatedStringHandler.AppendFormatted(managerDataDirectory);
							defaultInterpolatedStringHandler.AppendLiteral("\n\nTo:\n");
							defaultInterpolatedStringHandler.AppendFormatted(text);
							defaultInterpolatedStringHandler.AppendLiteral("\n\n");
							if (ModManagerMessageBox.Show(defaultInterpolatedStringHandler.ToStringAndClear() + "The application will restart after the move is complete.\n\nContinue?", "Confirm Move", 4, 48, null, null, null) == 6)
							{
								try
								{
									if (string.Equals(managerDataDirectory, text, 5))
									{
										ModManagerMessageBox.Show("The selected location is the same as the current location.", "Simple VS Manager", 0, 64, null, null, null);
									}
									else
									{
										Directory.CreateDirectory(text);
										MainWindow.MoveDirectoryContents(managerDataDirectory, text);
										CustomConfigFolderManager.SetCustomConfigFolder(text);
										ModManagerMessageBox.Show("Manager folder moved successfully to:\n" + text + "\n\nThe application will now restart.", "Move Complete", 0, 64, null, null, null);
										MainWindow.RestartApplication();
									}
								}
								catch (Exception ex)
								{
									ModManagerMessageBox.Show("Failed to move the manager folder:\n\n" + ex.Message, "Error", 0, 16, null, null, null);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000F380 File Offset: 0x0000D580
		private static void MoveDirectoryContents(string sourceDir, string targetDir)
		{
			if (string.Equals(sourceDir, targetDir, 5))
			{
				return;
			}
			Directory.CreateDirectory(targetDir);
			foreach (string text in Directory.GetFiles(sourceDir))
			{
				string fileName = Path.GetFileName(text);
				string text2 = Path.Combine(targetDir, fileName);
				try
				{
					if (File.Exists(text2))
					{
						File.Delete(text2);
					}
					File.Move(text, text2);
				}
				catch (Exception)
				{
					try
					{
						File.Copy(text, text2, true);
						File.Delete(text);
					}
					catch (Exception)
					{
						throw;
					}
				}
			}
			foreach (string text3 in Directory.GetDirectories(sourceDir))
			{
				string fileName2 = Path.GetFileName(text3);
				string targetDir2 = Path.Combine(targetDir, fileName2);
				MainWindow.MoveDirectoryContents(text3, targetDir2);
			}
			try
			{
				if (Directory.GetFiles(sourceDir).Length == 0 && Directory.GetDirectories(sourceDir).Length == 0)
				{
					Directory.Delete(sourceDir, false);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000F474 File Offset: 0x0000D674
		private void HandleResetManagerFolder(string currentFolder)
		{
			string defaultManagerDataDirectory = ModCacheLocator.GetDefaultManagerDataDirectory();
			if (string.IsNullOrWhiteSpace(defaultManagerDataDirectory))
			{
				ModManagerMessageBox.Show("Cannot determine the default manager data folder location.", "Simple VS Manager", 0, 16, null, null, null);
				return;
			}
			if (string.Equals(currentFolder, defaultManagerDataDirectory, 5))
			{
				ModManagerMessageBox.Show("Already using the default manager folder location:\n" + defaultManagerDataDirectory, "Simple VS Manager", 0, 64, null, null, null);
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(83, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Reset manager folder to default location?\n\n");
			defaultInterpolatedStringHandler.AppendLiteral("Current location:\n");
			defaultInterpolatedStringHandler.AppendFormatted(currentFolder);
			defaultInterpolatedStringHandler.AppendLiteral("\n\n");
			defaultInterpolatedStringHandler.AppendLiteral("Default location:\n");
			defaultInterpolatedStringHandler.AppendFormatted(defaultManagerDataDirectory);
			defaultInterpolatedStringHandler.AppendLiteral("\n\n");
			if (ModManagerMessageBox.Show(defaultInterpolatedStringHandler.ToStringAndClear() + "The manager will:\n• Move all configuration files, cached mods, backups, and presets\n• Update the configuration to use the default location\n• Require a restart to complete the change\n\nNote: The Firebase authentication backup (SVSM Backup folder) will remain in its original location.\n\nContinue?", "Reset to Default Location", 4, 32, null, null, null) != 6)
			{
				return;
			}
			try
			{
				if (!Directory.Exists(defaultManagerDataDirectory) || ModManagerMessageBox.Show("The default folder \"" + defaultManagerDataDirectory + "\" already exists.\n\nDo you want to merge with the existing folder?\n(Existing files with the same name will be overwritten)", "Folder Already Exists", 4, 48, null, null, null) == 6)
				{
					Directory.CreateDirectory(defaultManagerDataDirectory);
					MainWindow.MoveDirectoryContents(currentFolder, defaultManagerDataDirectory);
					CustomConfigFolderManager.ClearCustomConfigFolder();
					ModManagerMessageBox.Show("Manager folder reset to default location:\n" + defaultManagerDataDirectory + "\n\nThe application will now restart.", "Reset Complete", 0, 64, null, null, null);
					MainWindow.RestartApplication();
				}
			}
			catch (Exception ex)
			{
				ModManagerMessageBox.Show("Failed to reset the manager folder:\n\n" + ex.Message, "Error", 0, 16, null, null, null);
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000F5E8 File Offset: 0x0000D7E8
		private static void RestartApplication()
		{
			string text = Environment.ProcessPath;
			if (string.IsNullOrWhiteSpace(text))
			{
				ProcessModule mainModule = Process.GetCurrentProcess().MainModule;
				if (mainModule != null)
				{
					text = mainModule.FileName;
				}
			}
			if (!string.IsNullOrWhiteSpace(text))
			{
				try
				{
					Process.Start(text);
					Application.Current.Shutdown();
					return;
				}
				catch (Exception)
				{
					Application.Current.Shutdown();
					return;
				}
			}
			Application.Current.Shutdown();
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000F65C File Offset: 0x0000D85C
		private void HelpMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			string configurationDirectory = this._userConfiguration.GetConfigurationDirectory();
			string cachedModsDirectory = ModCacheLocator.GetCachedModsDirectory();
			new HelpDialogWindow(configurationDirectory, cachedModsDirectory)
			{
				Owner = this
			}.ShowDialog();
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000F690 File Offset: 0x0000D890
		private void GuideMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			string configurationDirectory = this._userConfiguration.GetConfigurationDirectory();
			string configurationFilePath = Path.Combine(configurationDirectory, "SimpleVSManagerConfiguration.json");
			string cachedModsDirectory = ModCacheLocator.GetCachedModsDirectory();
			new GuideDialogWindow(configurationDirectory, cachedModsDirectory, configurationFilePath)
			{
				Owner = this
			}.ShowDialog();
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000F6D0 File Offset: 0x0000D8D0
		private void ScanForModConfigsMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<ScanForModConfigsMenuItem_OnClick>d__392 <ScanForModConfigsMenuItem_OnClick>d__;
			<ScanForModConfigsMenuItem_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<ScanForModConfigsMenuItem_OnClick>d__.<>4__this = this;
			<ScanForModConfigsMenuItem_OnClick>d__.<>1__state = -1;
			<ScanForModConfigsMenuItem_OnClick>d__.<>t__builder.Start<MainWindow.<ScanForModConfigsMenuItem_OnClick>d__392>(ref <ScanForModConfigsMenuItem_OnClick>d__);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000F707 File Offset: 0x0000D907
		private void ManagerUpdateLink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			e.Handled = true;
			this.OpenManagerModDatabasePage();
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000F718 File Offset: 0x0000D918
		private void DiscordButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (e != null)
			{
				e.Handled = true;
			}
			if (InternetAccessManager.IsInternetAccessDisabled)
			{
				ModManagerMessageBox.Show("Enable Internet Access in the File menu to open Discord.", "Simple VS Manager", 0, 64, null, null, null);
				return;
			}
			try
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = "https://discord.gg/Zhm3QnD2s9",
					UseShellExecute = true
				});
			}
			catch (Exception ex)
			{
				ModManagerMessageBox.Show("Failed to open Discord:\n" + ex.Message, "Simple VS Manager", 0, 16, null, null, null);
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000F7A4 File Offset: 0x0000D9A4
		private void ModUsagePromptLink_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<ModUsagePromptLink_OnClick>d__395 <ModUsagePromptLink_OnClick>d__;
			<ModUsagePromptLink_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<ModUsagePromptLink_OnClick>d__.<>4__this = this;
			<ModUsagePromptLink_OnClick>d__.e = e;
			<ModUsagePromptLink_OnClick>d__.<>1__state = -1;
			<ModUsagePromptLink_OnClick>d__.<>t__builder.Start<MainWindow.<ModUsagePromptLink_OnClick>d__395>(ref <ModUsagePromptLink_OnClick>d__);
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000F7E3 File Offset: 0x0000D9E3
		internal UserConfigurationService UserConfiguration
		{
			get
			{
				return this._userConfiguration;
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000F7EB File Offset: 0x0000D9EB
		private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (e != null)
			{
				e.Handled = true;
			}
			new SettingsWindow(this)
			{
				Owner = this
			}.ShowDialog();
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000F80C File Offset: 0x0000DA0C
		private void OpenManagerModDatabasePage()
		{
			if (InternetAccessManager.IsInternetAccessDisabled)
			{
				ModManagerMessageBox.Show("Internet access is disabled. Enable Internet Access in the File menu to open the mod database page.", "Simple VS Manager", 0, 64, null, null, null);
				return;
			}
			try
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = MainWindow.ManagerModDatabaseUrl,
					UseShellExecute = true
				});
			}
			catch (Exception ex)
			{
				ModManagerMessageBox.Show("Failed to open the mod database page:\n" + ex.Message, "Simple VS Manager", 0, 16, null, null, null);
			}
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000F88C File Offset: 0x0000DA8C
		private void ExperimentalCompReviewMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<ExperimentalCompReviewMenuItem_OnClick>d__400 <ExperimentalCompReviewMenuItem_OnClick>d__;
			<ExperimentalCompReviewMenuItem_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<ExperimentalCompReviewMenuItem_OnClick>d__.<>4__this = this;
			<ExperimentalCompReviewMenuItem_OnClick>d__.<>1__state = -1;
			<ExperimentalCompReviewMenuItem_OnClick>d__.<>t__builder.Start<MainWindow.<ExperimentalCompReviewMenuItem_OnClick>d__400>(ref <ExperimentalCompReviewMenuItem_OnClick>d__);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000F8C4 File Offset: 0x0000DAC4
		private static string BuildExperimentalCompReviewMessage(ModCompatibilityCommentsService.ExperimentalCompReviewResult result)
		{
			IReadOnlyList<ModCompatibilityCommentsService.ExperimentalCompReviewComment> top = result.Top3;
			if (top == null || top.Count <= 0)
			{
				return result.Reason ?? string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < result.Top3.Count; i++)
			{
				ModCompatibilityCommentsService.ExperimentalCompReviewComment experimentalCompReviewComment = result.Top3[i];
				IReadOnlyDictionary<string, double> scoreBreakdown = experimentalCompReviewComment.ScoreBreakdown;
				string text = MainWindow.FormatExperimentalCompReviewScore((scoreBreakdown != null) ? Enumerable.Sum(scoreBreakdown.Values) : 0.0);
				stringBuilder.Append(i + 1);
				stringBuilder.Append(". [");
				stringBuilder.Append(text);
				stringBuilder.Append("] ");
				stringBuilder.AppendLine(experimentalCompReviewComment.Snippet);
			}
			return stringBuilder.ToString().TrimEnd();
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000F988 File Offset: 0x0000DB88
		private static string FormatExperimentalCompReviewScore(double score)
		{
			return Math.Round(score, 2).ToString("+0.##;-0.##;0", CultureInfo.CurrentCulture);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000F9B0 File Offset: 0x0000DBB0
		private void DeleteCloudAuthMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<DeleteCloudAuthMenuItem_OnClick>d__403 <DeleteCloudAuthMenuItem_OnClick>d__;
			<DeleteCloudAuthMenuItem_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<DeleteCloudAuthMenuItem_OnClick>d__.<>4__this = this;
			<DeleteCloudAuthMenuItem_OnClick>d__.<>1__state = -1;
			<DeleteCloudAuthMenuItem_OnClick>d__.<>t__builder.Start<MainWindow.<DeleteCloudAuthMenuItem_OnClick>d__403>(ref <DeleteCloudAuthMenuItem_OnClick>d__);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000F9E8 File Offset: 0x0000DBE8
		private static string ResolveExperimentalCompReviewIdentifier(ModListItemViewModel selectedMod)
		{
			string text = MainWindow.TryExtractModSlug(selectedMod.ModDatabasePageUrl);
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text;
			}
			if (!string.IsNullOrWhiteSpace(selectedMod.ModDatabaseAssetId))
			{
				return selectedMod.ModDatabaseAssetId;
			}
			return selectedMod.ModId;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000FA28 File Offset: 0x0000DC28
		[NullableContext(2)]
		private static string TryExtractModSlug(string modDatabasePageUrl)
		{
			if (string.IsNullOrWhiteSpace(modDatabasePageUrl))
			{
				return null;
			}
			Uri uri;
			if (Uri.TryCreate(modDatabasePageUrl, 1, ref uri))
			{
				string text = MainWindow.<TryExtractModSlug>g__ExtractSlugFromPath|405_0(uri.AbsolutePath);
				if (!string.IsNullOrWhiteSpace(text))
				{
					return text;
				}
			}
			return MainWindow.<TryExtractModSlug>g__ExtractSlugFromPath|405_0(modDatabasePageUrl);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000FA68 File Offset: 0x0000DC68
		private void ClearAllCachesMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			if (ModManagerMessageBox.Show(this, "This will delete all cache folders used by Simple VS Manager:\n\n• Temp Cache (contains all cache data)\n\nYour settings, modlists and installed mods will NOT be affected.\n\nThis is useful when experiencing problems with the mod database or cached data.\n\nContinue?", "Simple VS Manager", 1, 32, null, null, null) != 1)
			{
				return;
			}
			try
			{
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				string managerDataDirectory = ModCacheLocator.GetManagerDataDirectory();
				if (string.IsNullOrWhiteSpace(managerDataDirectory))
				{
					ModManagerMessageBox.Show(this, "Could not locate the Simple VS Manager data directory.", "Simple VS Manager", 0, 16, null, null, null);
				}
				else
				{
					foreach (ValueTuple<string, string> valueTuple in new ValueTuple<string, string>[]
					{
						new ValueTuple<string, string>("Temp Cache", Path.Combine(managerDataDirectory, "Temp Cache"))
					})
					{
						string item = valueTuple.Item1;
						string item2 = valueTuple.Item2;
						try
						{
							if (Directory.Exists(item2))
							{
								Directory.Delete(item2, true);
								list.Add(item);
							}
						}
						catch (Exception ex)
						{
							list2.Add(item + ": " + ex.Message);
						}
					}
					foreach (string text in new string[]
					{
						Path.Combine(managerDataDirectory, "Mod Database Cache"),
						Path.Combine(managerDataDirectory, "Cached Mods"),
						Path.Combine(managerDataDirectory, "Mod Metadata"),
						Path.Combine(managerDataDirectory, "Firebase Cache")
					})
					{
						try
						{
							if (Directory.Exists(text))
							{
								Directory.Delete(text, true);
							}
						}
						catch
						{
						}
					}
					StringBuilder stringBuilder = new StringBuilder();
					if (list.Count > 0)
					{
						stringBuilder.AppendLine("Successfully deleted the following cache folders:");
						using (List<string>.Enumerator enumerator = list.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								string text2 = enumerator.Current;
								StringBuilder stringBuilder2 = stringBuilder;
								StringBuilder stringBuilder3 = stringBuilder2;
								StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder2);
								appendInterpolatedStringHandler.AppendLiteral("• ");
								appendInterpolatedStringHandler.AppendFormatted(text2);
								stringBuilder3.AppendLine(ref appendInterpolatedStringHandler);
							}
							goto IL_1D0;
						}
					}
					if (list2.Count == 0)
					{
						stringBuilder.AppendLine("No cache folders were found to delete.");
					}
					IL_1D0:
					if (list2.Count > 0)
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.AppendLine();
						}
						stringBuilder.AppendLine("Failed to delete the following cache folders:");
						foreach (string text3 in list2)
						{
							StringBuilder stringBuilder2 = stringBuilder;
							StringBuilder stringBuilder4 = stringBuilder2;
							StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder2);
							appendInterpolatedStringHandler.AppendLiteral("• ");
							appendInterpolatedStringHandler.AppendFormatted(text3);
							stringBuilder4.AppendLine(ref appendInterpolatedStringHandler);
						}
					}
					ModManagerMessageBox.Show(this, stringBuilder.ToString(), "Simple VS Manager", 0, (list2.Count > 0) ? 48 : 64, null, null, null);
				}
			}
			catch (Exception ex2)
			{
				ModManagerMessageBox.Show(this, "An error occurred while clearing caches:\n\n" + ex2.Message, "Simple VS Manager", 0, 16, null, null, null);
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000FD90 File Offset: 0x0000DF90
		private void RestoreFirebaseAuthBackupMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			if (ModManagerMessageBox.Show(this, "This will copy the firebase-auth.json file from the SVSM Backup folder (AppData/Local/SVSM Backup) into the Simple VS Manager folder and replace the current file.\n\nUse this if you lost access to online modlists or votes after moving or deleting files.\n\nContinue?", "Simple VS Manager", 1, 32, null, null, null) != 1)
			{
				return;
			}
			string backupFilePath = FirebaseAnonymousAuthenticator.GetBackupFilePath();
			if (string.IsNullOrWhiteSpace(backupFilePath))
			{
				ModManagerMessageBox.Show(this, "Could not determine the location of the firebase-auth.json backup.", "Simple VS Manager", 0, 16, null, null, null);
				return;
			}
			if (!File.Exists(backupFilePath))
			{
				ModManagerMessageBox.Show(this, "No firebase-auth.json backup was found in the SVSM Backup folder.", "Simple VS Manager", 0, 48, null, null, null);
				return;
			}
			string stateFilePath = FirebaseAnonymousAuthenticator.GetStateFilePath();
			if (string.IsNullOrWhiteSpace(stateFilePath))
			{
				ModManagerMessageBox.Show(this, "Could not determine the Simple VS Manager folder for firebase-auth.json.", "Simple VS Manager", 0, 16, null, null, null);
				return;
			}
			try
			{
				string directoryName = Path.GetDirectoryName(stateFilePath);
				if (!string.IsNullOrWhiteSpace(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				File.Copy(backupFilePath, stateFilePath, true);
				ModManagerMessageBox.Show(this, "Restored firebase-auth.json from the SVSM Backup folder.", "Simple VS Manager", 0, 64, null, null, null);
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is NotSupportedException || ex is SecurityException)
			{
				ModManagerMessageBox.Show(this, "Failed to restore firebase-auth.json: " + ex.Message, "Simple VS Manager", 0, 16, null, null, null);
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000FECC File Offset: 0x0000E0CC
		private void DeleteAllManagerFilesMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<DeleteAllManagerFilesMenuItem_OnClick>d__408 <DeleteAllManagerFilesMenuItem_OnClick>d__;
			<DeleteAllManagerFilesMenuItem_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<DeleteAllManagerFilesMenuItem_OnClick>d__.<>4__this = this;
			<DeleteAllManagerFilesMenuItem_OnClick>d__.<>1__state = -1;
			<DeleteAllManagerFilesMenuItem_OnClick>d__.<>t__builder.Start<MainWindow.<DeleteAllManagerFilesMenuItem_OnClick>d__408>(ref <DeleteAllManagerFilesMenuItem_OnClick>d__);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000FF04 File Offset: 0x0000E104
		private void SwitchToInstalledModsTab()
		{
			MainViewModel viewModel = this._viewModel;
			bool flag;
			if (viewModel == null)
			{
				flag = false;
			}
			else
			{
				IRelayCommand showMainTabCommand = viewModel.ShowMainTabCommand;
				flag = ((showMainTabCommand != null) ? new bool?(showMainTabCommand.CanExecute(null)) : default(bool?)).GetValueOrDefault();
			}
			if (flag)
			{
				this._viewModel.ShowMainTabCommand.Execute(null);
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000FF58 File Offset: 0x0000E158
		private void PrepareForModlistLoad()
		{
			this.SwitchToInstalledModsTab();
			this.ClearSelection(true);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000FF68 File Offset: 0x0000E168
		private void UpdateModlistLoadingUiState()
		{
			bool flag = !this._isApplyingPreset;
			if (this.UpdateAllButton != null)
			{
				this.UpdateAllButton.IsEnabled = flag;
			}
			if (this.LaunchGameButton != null)
			{
				this.LaunchGameButton.IsEnabled = flag;
			}
			if (this.PresetsAndModlistsMenuItem != null)
			{
				this.PresetsAndModlistsMenuItem.IsEnabled = flag;
			}
			if (this.ModsDataGrid != null)
			{
				if (flag)
				{
					this.ModsDataGrid.ClearValue(UIElement.IsEnabledProperty);
					return;
				}
				this.ModsDataGrid.IsEnabled = false;
			}
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000FFE4 File Offset: 0x0000E1E4
		private Task RefreshDeleteCachedModsMenuHeaderAsync()
		{
			MainWindow.<RefreshDeleteCachedModsMenuHeaderAsync>d__412 <RefreshDeleteCachedModsMenuHeaderAsync>d__;
			<RefreshDeleteCachedModsMenuHeaderAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RefreshDeleteCachedModsMenuHeaderAsync>d__.<>4__this = this;
			<RefreshDeleteCachedModsMenuHeaderAsync>d__.<>1__state = -1;
			<RefreshDeleteCachedModsMenuHeaderAsync>d__.<>t__builder.Start<MainWindow.<RefreshDeleteCachedModsMenuHeaderAsync>d__412>(ref <RefreshDeleteCachedModsMenuHeaderAsync>d__);
			return <RefreshDeleteCachedModsMenuHeaderAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00010028 File Offset: 0x0000E228
		private static long CalculateDirectorySize(string rootDirectory)
		{
			Stack<string> stack = new Stack<string>();
			stack.Push(rootDirectory);
			long num = 0L;
			while (stack.Count > 0)
			{
				string text = stack.Pop();
				if (!string.IsNullOrWhiteSpace(text) && Directory.Exists(text))
				{
					try
					{
						foreach (string text2 in Directory.EnumerateFiles(text))
						{
							try
							{
								FileInfo fileInfo = new FileInfo(text2);
								num += fileInfo.Length;
							}
							catch (IOException)
							{
							}
							catch (UnauthorizedAccessException)
							{
							}
							catch (SecurityException)
							{
							}
						}
					}
					catch (IOException)
					{
						continue;
					}
					catch (UnauthorizedAccessException)
					{
						continue;
					}
					catch (SecurityException)
					{
						continue;
					}
					try
					{
						foreach (string text3 in Directory.EnumerateDirectories(text))
						{
							stack.Push(text3);
						}
					}
					catch (IOException)
					{
					}
					catch (UnauthorizedAccessException)
					{
					}
					catch (SecurityException)
					{
					}
				}
			}
			return num;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00010190 File Offset: 0x0000E390
		[NullableContext(2)]
		private static string GetCachedModsDirectory()
		{
			return ModCacheLocator.GetCachedModsDirectory();
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00010197 File Offset: 0x0000E397
		[NullableContext(2)]
		private static string GetManagerDataDirectory()
		{
			return ModCacheLocator.GetManagerDataDirectory();
		}

		// Token: 0x06000257 RID: 599 RVA: 0x000101A0 File Offset: 0x0000E3A0
		[NullableContext(2)]
		private static MainWindow.ManagerDeletionResult DeleteAllManagerFiles(string dataDirectory)
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			HashSet<string> hashSet2 = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			MainWindow.AddCandidateDirectory(hashSet, ModCacheLocator.GetManagerDataDirectory());
			MainWindow.AddCandidateDirectory(hashSet, MainWindow.TryCombineSpecialFolder(5, "Simple VS Manager"));
			MainWindow.AddCandidateDirectory(hashSet, MainWindow.TryCombineSpecialFolder(5, "Simple VS Manager"));
			MainWindow.AddCandidateDirectory(hashSet, MainWindow.TryCombineSpecialFolder(26, "Simple VS Manager"));
			MainWindow.AddCandidateDirectory(hashSet, MainWindow.TryCombineSpecialFolder(28, "Simple VS Manager"));
			MainWindow.AddCandidateDirectory(hashSet, MainWindow.TryCombineSpecialFolder(40, ".simple-vs-manager"));
			MainWindow.AddCandidateDirectory(hashSet, Path.Combine(AppContext.BaseDirectory, "Simple VS Manager"));
			MainWindow.AddCandidateDirectory(hashSet, Path.Combine(Environment.CurrentDirectory, "Simple VS Manager"));
			if (!string.IsNullOrWhiteSpace(dataDirectory))
			{
				MainWindow.AddCandidateDirectory(hashSet, Path.Combine(dataDirectory, "ModData", "SimpleVSManager"));
			}
			MainWindow.AddCandidateFile(hashSet2, FirebaseAnonymousAuthenticator.GetStateFilePath());
			MainWindow.AddCandidateFile(hashSet2, Path.Combine(AppContext.BaseDirectory, "SimpleVSManagerStatus.log"));
			MainWindow.AddCandidateFile(hashSet2, Path.Combine(Environment.CurrentDirectory, "SimpleVSManagerStatus.log"));
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			foreach (string text in hashSet2)
			{
				try
				{
					if (File.Exists(text))
					{
						FileSystem.DeleteFile(text, 2, 3);
						list.Add(text);
					}
				}
				catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is NotSupportedException || ex is SecurityException || ex is ArgumentException)
				{
					list2.Add(text + " (" + ex.Message + ")");
				}
			}
			foreach (string text2 in Enumerable.OrderByDescending<string, int>(hashSet, (string path) => path.Length))
			{
				try
				{
					if (Directory.Exists(text2))
					{
						FileSystem.DeleteDirectory(text2, 2, 3);
						list.Add(text2);
					}
				}
				catch (Exception ex2) when (ex2 is IOException || ex2 is UnauthorizedAccessException || ex2 is NotSupportedException || ex2 is SecurityException || ex2 is ArgumentException)
				{
					list2.Add(text2 + " (" + ex2.Message + ")");
				}
			}
			list.Sort(StringComparer.OrdinalIgnoreCase);
			list2.Sort(StringComparer.OrdinalIgnoreCase);
			return new MainWindow.ManagerDeletionResult(list, list2);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00010498 File Offset: 0x0000E698
		private static void AddCandidateDirectory(ISet<string> directories, [Nullable(2)] string path)
		{
			string text = MainWindow.TryNormalizePath(path);
			if (text == null)
			{
				return;
			}
			directories.Add(text);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x000104B8 File Offset: 0x0000E6B8
		private static void AddCandidateFile(ISet<string> files, [Nullable(2)] string path)
		{
			string text = MainWindow.TryNormalizePath(path);
			if (text == null)
			{
				return;
			}
			files.Add(text);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x000104D8 File Offset: 0x0000E6D8
		[return: Nullable(2)]
		private static string TryCombineSpecialFolder(Environment.SpecialFolder folder, string relativePath)
		{
			string text = MainWindow.TryGetSpecialFolderPath(folder);
			if (string.IsNullOrWhiteSpace(text))
			{
				return null;
			}
			string result;
			try
			{
				result = Path.GetFullPath(Path.Combine(text, relativePath));
			}
			catch (Exception ex) when (ex is ArgumentException || ex is NotSupportedException || ex is PathTooLongException || ex is SecurityException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00010558 File Offset: 0x0000E758
		[NullableContext(2)]
		private static string TryGetSpecialFolderPath(Environment.SpecialFolder folder)
		{
			string result;
			try
			{
				string folderPath = Environment.GetFolderPath(folder, 16384);
				if (string.IsNullOrWhiteSpace(folderPath))
				{
					folderPath = Environment.GetFolderPath(folder);
				}
				result = (string.IsNullOrWhiteSpace(folderPath) ? null : folderPath);
			}
			catch (Exception ex) when (ex is PlatformNotSupportedException || ex is InvalidOperationException || ex is SecurityException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x000105D8 File Offset: 0x0000E7D8
		[NullableContext(2)]
		private static string TryNormalizePath(string path)
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
			catch (Exception ex) when (ex is ArgumentException || ex is NotSupportedException || ex is PathTooLongException || ex is SecurityException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00010648 File Offset: 0x0000E848
		private Task RefreshModsWithErrorHandlingAsync()
		{
			MainWindow.<RefreshModsWithErrorHandlingAsync>d__422 <RefreshModsWithErrorHandlingAsync>d__;
			<RefreshModsWithErrorHandlingAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RefreshModsWithErrorHandlingAsync>d__.<>4__this = this;
			<RefreshModsWithErrorHandlingAsync>d__.<>1__state = -1;
			<RefreshModsWithErrorHandlingAsync>d__.<>t__builder.Start<MainWindow.<RefreshModsWithErrorHandlingAsync>d__422>(ref <RefreshModsWithErrorHandlingAsync>d__);
			return <RefreshModsWithErrorHandlingAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0001068C File Offset: 0x0000E88C
		private static void ClearManagerCaches(bool preserveModCache)
		{
			List<string> list = new List<string>();
			try
			{
				ModManifestCacheService.ClearCache();
			}
			catch (Exception ex)
			{
				list.Add(MainWindow.BuildCacheClearErrorMessage("Mod metadata cache", ex));
			}
			string cachedModsDirectory = ModCacheLocator.GetCachedModsDirectory();
			if (!preserveModCache && !string.IsNullOrWhiteSpace(cachedModsDirectory))
			{
				try
				{
					MainWindow.ClearCachedModsDirectory(cachedModsDirectory);
				}
				catch (Exception ex2)
				{
					list.Add(MainWindow.BuildCacheClearErrorMessage("Cached mods at " + cachedModsDirectory, ex2));
				}
			}
			if (list.Count > 0)
			{
				throw new InvalidOperationException(string.Join(Environment.NewLine + Environment.NewLine, list));
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00010730 File Offset: 0x0000E930
		private static void ClearCachedModsDirectory(string cachedModsDirectory)
		{
			if (!Directory.Exists(cachedModsDirectory))
			{
				return;
			}
			Directory.Delete(cachedModsDirectory, true);
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00010744 File Offset: 0x0000E944
		private static string BuildCacheClearErrorMessage(string context, Exception ex)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(context);
			stringBuilder.Append(':');
			stringBuilder.AppendLine();
			stringBuilder.Append(ex.Message);
			for (Exception innerException = ex.InnerException; innerException != null; innerException = innerException.InnerException)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append(innerException.Message);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000261 RID: 609 RVA: 0x000107AC File Offset: 0x0000E9AC
		private Task OnActiveGameProfileChangedAsync()
		{
			MainWindow.<OnActiveGameProfileChangedAsync>d__426 <OnActiveGameProfileChangedAsync>d__;
			<OnActiveGameProfileChangedAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnActiveGameProfileChangedAsync>d__.<>4__this = this;
			<OnActiveGameProfileChangedAsync>d__.<>1__state = -1;
			<OnActiveGameProfileChangedAsync>d__.<>t__builder.Start<MainWindow.<OnActiveGameProfileChangedAsync>d__426>(ref <OnActiveGameProfileChangedAsync>d__);
			return <OnActiveGameProfileChangedAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x000107EF File Offset: 0x0000E9EF
		private void GameProfilesMenuItem_OnSubmenuOpened(object sender, RoutedEventArgs e)
		{
			this.RefreshGameProfileMenuItems();
			this.UpdateGameProfileMenuChecks();
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00010800 File Offset: 0x0000EA00
		private void CreateGameProfileMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<CreateGameProfileMenuItem_OnClick>d__428 <CreateGameProfileMenuItem_OnClick>d__;
			<CreateGameProfileMenuItem_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<CreateGameProfileMenuItem_OnClick>d__.<>4__this = this;
			<CreateGameProfileMenuItem_OnClick>d__.<>1__state = -1;
			<CreateGameProfileMenuItem_OnClick>d__.<>t__builder.Start<MainWindow.<CreateGameProfileMenuItem_OnClick>d__428>(ref <CreateGameProfileMenuItem_OnClick>d__);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00010838 File Offset: 0x0000EA38
		private void DeleteGameProfileMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<DeleteGameProfileMenuItem_OnClick>d__429 <DeleteGameProfileMenuItem_OnClick>d__;
			<DeleteGameProfileMenuItem_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<DeleteGameProfileMenuItem_OnClick>d__.<>4__this = this;
			<DeleteGameProfileMenuItem_OnClick>d__.<>1__state = -1;
			<DeleteGameProfileMenuItem_OnClick>d__.<>t__builder.Start<MainWindow.<DeleteGameProfileMenuItem_OnClick>d__429>(ref <DeleteGameProfileMenuItem_OnClick>d__);
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00010870 File Offset: 0x0000EA70
		private void GameProfileMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<GameProfileMenuItem_OnClick>d__430 <GameProfileMenuItem_OnClick>d__;
			<GameProfileMenuItem_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<GameProfileMenuItem_OnClick>d__.<>4__this = this;
			<GameProfileMenuItem_OnClick>d__.sender = sender;
			<GameProfileMenuItem_OnClick>d__.<>1__state = -1;
			<GameProfileMenuItem_OnClick>d__.<>t__builder.Start<MainWindow.<GameProfileMenuItem_OnClick>d__430>(ref <GameProfileMenuItem_OnClick>d__);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x000108B0 File Offset: 0x0000EAB0
		private void SelectDataFolderMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<SelectDataFolderMenuItem_OnClick>d__431 <SelectDataFolderMenuItem_OnClick>d__;
			<SelectDataFolderMenuItem_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<SelectDataFolderMenuItem_OnClick>d__.<>4__this = this;
			<SelectDataFolderMenuItem_OnClick>d__.<>1__state = -1;
			<SelectDataFolderMenuItem_OnClick>d__.<>t__builder.Start<MainWindow.<SelectDataFolderMenuItem_OnClick>d__431>(ref <SelectDataFolderMenuItem_OnClick>d__);
		}

		// Token: 0x06000267 RID: 615 RVA: 0x000108E8 File Offset: 0x0000EAE8
		private void SelectGameFolderMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			string description = "Select your Vintage Story installation folder";
			string gameDirectory = this._gameDirectory;
			MainWindow.PathValidator validator;
			if ((validator = MainWindow.<>O.<1>__TryValidateGameDirectory) == null)
			{
				validator = (MainWindow.<>O.<1>__TryValidateGameDirectory = new MainWindow.PathValidator(MainWindow.TryValidateGameDirectory));
			}
			string text = this.PromptForDirectory(description, gameDirectory, validator, true);
			if (text == null)
			{
				return;
			}
			if (string.Equals(text, this._gameDirectory, 5))
			{
				return;
			}
			this._gameDirectory = text;
			this._userConfiguration.SetGameDirectory(text);
			this.UpdateGameVersionMenuItem(VintageStoryVersionLocator.GetInstalledVersion(this._gameDirectory));
			if (this._modBrowserViewModel != null)
			{
				this._modBrowserViewModel.RefreshInstalledGameVersion();
				this._modBrowserViewModel.InvalidateAllVisibleUserReports();
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0001097C File Offset: 0x0000EB7C
		private void RefreshDeveloperProfilesMenuEntries()
		{
			if (this.DeveloperProfilesMenuItem == null)
			{
				return;
			}
			foreach (MenuItem menuItem in this._developerProfileMenuItems)
			{
				menuItem.Click -= new RoutedEventHandler(this.DeveloperProfileMenuItem_OnClick);
			}
			this._developerProfileMenuItems.Clear();
			this.DeveloperProfilesMenuItem.Items.Clear();
			if (!DeveloperProfileManager.DevDebug)
			{
				this.DeveloperProfilesMenuItem.Visibility = 2;
				return;
			}
			IReadOnlyList<DeveloperProfile> profiles = DeveloperProfileManager.GetProfiles();
			if (profiles.Count == 0)
			{
				this.DeveloperProfilesMenuItem.Visibility = 2;
				return;
			}
			foreach (DeveloperProfile developerProfile in profiles)
			{
				MenuItem menuItem2 = new MenuItem
				{
					Header = developerProfile.DisplayName,
					Tag = developerProfile,
					IsCheckable = true
				};
				menuItem2.Click += new RoutedEventHandler(this.DeveloperProfileMenuItem_OnClick);
				this.DeveloperProfilesMenuItem.Items.Add(menuItem2);
				this._developerProfileMenuItems.Add(menuItem2);
			}
			this.DeveloperProfilesMenuItem.Visibility = 0;
			this.UpdateDeveloperProfileMenuChecks();
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00010AC4 File Offset: 0x0000ECC4
		private void RefreshGameProfileMenuItems()
		{
			if (this.GameProfilesMenuItem == null || this.CreateGameProfileMenuItem == null)
			{
				return;
			}
			foreach (MenuItem menuItem in this._gameProfileMenuItems)
			{
				menuItem.Click -= new RoutedEventHandler(this.GameProfileMenuItem_OnClick);
			}
			this._gameProfileMenuItems.Clear();
			this.GameProfilesMenuItem.Items.Clear();
			this.GameProfilesMenuItem.Items.Add(this.CreateGameProfileMenuItem);
			if (this.DeleteGameProfileMenuItem != null)
			{
				this.GameProfilesMenuItem.Items.Add(this.DeleteGameProfileMenuItem);
			}
			IReadOnlyList<string> gameProfileNames = this._userConfiguration.GetGameProfileNames();
			if (this.DeleteGameProfileMenuItem != null)
			{
				this.DeleteGameProfileMenuItem.IsEnabled = Enumerable.Any<string>(gameProfileNames, (string name) => !this._userConfiguration.IsDefaultGameProfile(name));
			}
			if (gameProfileNames.Count > 0)
			{
				this.GameProfilesMenuItem.Items.Add(new Separator());
			}
			string activeGameProfileName = this._userConfiguration.ActiveGameProfileName;
			foreach (string text in gameProfileNames)
			{
				MenuItem menuItem2 = new MenuItem
				{
					Header = text,
					Tag = text,
					IsCheckable = true,
					Height = 35.0,
					IsChecked = string.Equals(text, activeGameProfileName, 5)
				};
				menuItem2.Click += new RoutedEventHandler(this.GameProfileMenuItem_OnClick);
				this.GameProfilesMenuItem.Items.Add(menuItem2);
				this._gameProfileMenuItems.Add(menuItem2);
			}
			this.UpdateActiveGameProfileDisplay();
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00010C84 File Offset: 0x0000EE84
		private void UpdateGameProfileMenuChecks()
		{
			string activeGameProfileName = this._userConfiguration.ActiveGameProfileName;
			foreach (MenuItem menuItem in this._gameProfileMenuItems)
			{
				string text = menuItem.Tag as string;
				if (text != null)
				{
					menuItem.IsChecked = string.Equals(text, activeGameProfileName, 5);
				}
			}
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00010CFC File Offset: 0x0000EEFC
		private void UpdateActiveGameProfileDisplay()
		{
			if (this.ActiveGameProfileTextBlock == null)
			{
				return;
			}
			if (!Enumerable.Any<string>(this._userConfiguration.GetGameProfileNames(), (string name) => !this._userConfiguration.IsDefaultGameProfile(name)))
			{
				this.ActiveGameProfileTextBlock.Visibility = 2;
				return;
			}
			string text = this._userConfiguration.ActiveGameProfileName;
			if (string.IsNullOrWhiteSpace(text))
			{
				text = "Default";
			}
			this.ActiveGameProfileTextBlock.Text = "Profile: " + text;
			this.ActiveGameProfileTextBlock.Visibility = 0;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00010D7C File Offset: 0x0000EF7C
		private void UpdateDeveloperProfileMenuChecks()
		{
			if (!DeveloperProfileManager.DevDebug)
			{
				return;
			}
			DeveloperProfile currentProfile = DeveloperProfileManager.CurrentProfile;
			foreach (MenuItem menuItem in this._developerProfileMenuItems)
			{
				DeveloperProfile developerProfile = menuItem.Tag as DeveloperProfile;
				if (developerProfile == null)
				{
					menuItem.IsChecked = false;
				}
				else
				{
					bool isChecked = currentProfile != null && string.Equals(developerProfile.Id, currentProfile.Id, 5);
					menuItem.IsChecked = isChecked;
				}
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00010E10 File Offset: 0x0000F010
		private void DeveloperProfileMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<DeveloperProfileMenuItem_OnClick>d__438 <DeveloperProfileMenuItem_OnClick>d__;
			<DeveloperProfileMenuItem_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<DeveloperProfileMenuItem_OnClick>d__.<>4__this = this;
			<DeveloperProfileMenuItem_OnClick>d__.sender = sender;
			<DeveloperProfileMenuItem_OnClick>d__.<>1__state = -1;
			<DeveloperProfileMenuItem_OnClick>d__.<>t__builder.Start<MainWindow.<DeveloperProfileMenuItem_OnClick>d__438>(ref <DeveloperProfileMenuItem_OnClick>d__);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00010E50 File Offset: 0x0000F050
		private void DeveloperProfileManager_OnCurrentProfileChanged([Nullable(2)] object sender, DeveloperProfileChangedEventArgs e)
		{
			if (!DeveloperProfileManager.DevDebug)
			{
				return;
			}
			if (!base.Dispatcher.CheckAccess())
			{
				base.Dispatcher.Invoke(delegate()
				{
					this.DeveloperProfileManager_OnCurrentProfileChanged(sender, e);
				});
				return;
			}
			if (e.ProfilesUpdated)
			{
				this.RefreshDeveloperProfilesMenuEntries();
				return;
			}
			this.UpdateDeveloperProfileMenuChecks();
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00010EBF File Offset: 0x0000F0BF
		private void ExitMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			base.Close();
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00010EC8 File Offset: 0x0000F0C8
		private Task<bool> TryEnsureDataBackupBeforeLaunchAsync()
		{
			MainWindow.<TryEnsureDataBackupBeforeLaunchAsync>d__441 <TryEnsureDataBackupBeforeLaunchAsync>d__;
			<TryEnsureDataBackupBeforeLaunchAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<TryEnsureDataBackupBeforeLaunchAsync>d__.<>4__this = this;
			<TryEnsureDataBackupBeforeLaunchAsync>d__.<>1__state = -1;
			<TryEnsureDataBackupBeforeLaunchAsync>d__.<>t__builder.Start<MainWindow.<TryEnsureDataBackupBeforeLaunchAsync>d__441>(ref <TryEnsureDataBackupBeforeLaunchAsync>d__);
			return <TryEnsureDataBackupBeforeLaunchAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00010F0C File Offset: 0x0000F10C
		private void LaunchGameButton_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<LaunchGameButton_OnClick>d__442 <LaunchGameButton_OnClick>d__;
			<LaunchGameButton_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<LaunchGameButton_OnClick>d__.<>4__this = this;
			<LaunchGameButton_OnClick>d__.<>1__state = -1;
			<LaunchGameButton_OnClick>d__.<>t__builder.Start<MainWindow.<LaunchGameButton_OnClick>d__442>(ref <LaunchGameButton_OnClick>d__);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00010F43 File Offset: 0x0000F143
		private void CloseManagerAfterGameLaunchIfEnabled()
		{
			if (this._userConfiguration.CloseManagerAfterGameLaunch)
			{
				base.Close();
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00010F58 File Offset: 0x0000F158
		private void SetCustomShortcutMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			string text = null;
			string text2 = null;
			if (!string.IsNullOrWhiteSpace(this._customShortcutPath))
			{
				try
				{
					text = Path.GetDirectoryName(this._customShortcutPath);
					text2 = Path.GetFileName(this._customShortcutPath);
				}
				catch (Exception)
				{
					text = null;
					text2 = null;
				}
			}
			using (OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Title = "Select Vintage Story shortcut",
				Filter = "Shortcut files (*.lnk)|*.lnk|All files (*.*)|*.*",
				CheckFileExists = true,
				Multiselect = false,
				RestoreDirectory = true
			})
			{
				if (!string.IsNullOrWhiteSpace(text) && Directory.Exists(text))
				{
					openFileDialog.InitialDirectory = text;
				}
				if (!string.IsNullOrWhiteSpace(text2))
				{
					openFileDialog.FileName = text2;
				}
				if (openFileDialog.ShowDialog() == 1)
				{
					string fileName = openFileDialog.FileName;
					if (!File.Exists(fileName))
					{
						ModManagerMessageBox.Show("The selected shortcut could not be found.", "Simple VS Manager", 0, 48, null, null, null);
						return;
					}
					try
					{
						this._userConfiguration.SetCustomShortcutPath(fileName);
						this._customShortcutPath = this._userConfiguration.CustomShortcutPath;
						return;
					}
					catch (ArgumentException ex)
					{
						ModManagerMessageBox.Show("The selected shortcut is not valid:\n" + ex.Message, "Simple VS Manager", 0, 48, null, null, null);
						return;
					}
				}
				if (!string.IsNullOrWhiteSpace(this._customShortcutPath))
				{
					if (ModManagerMessageBox.Show("Do you want to clear the custom Vintage Story shortcut?", "Simple VS Manager", 4, 32, null, null, null) == 6)
					{
						this._userConfiguration.ClearCustomShortcutPath();
						this._customShortcutPath = null;
					}
				}
			}
		}

		// Token: 0x06000274 RID: 628 RVA: 0x000110D8 File Offset: 0x0000F2D8
		private void RestoreDataFolderMenuItem_OnSubmenuOpened(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			menuItem.Items.Clear();
			MenuItem menuItem2 = new MenuItem
			{
				Header = "Open data backup directory..."
			};
			menuItem2.Click += new RoutedEventHandler(this.OpenDataBackupDirectoryMenuItem_OnClick);
			menuItem.Items.Add(menuItem2);
			MenuItem menuItem3 = new MenuItem
			{
				Header = "Change backup location..."
			};
			menuItem3.Click += new RoutedEventHandler(this.ChangeBackupLocationMenuItem_OnClick);
			menuItem.Items.Add(menuItem3);
			MenuItem menuItem4 = new MenuItem();
			object header;
			if (this._userConfiguration.MaximumDataBackups <= 0)
			{
				header = "Set maximum backups... (Unlimited)";
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(25, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Set maximum backups... (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._userConfiguration.MaximumDataBackups);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				header = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			menuItem4.Header = header;
			MenuItem menuItem5 = menuItem4;
			menuItem5.Click += new RoutedEventHandler(this.SetMaximumDataBackupsMenuItem_OnClick);
			menuItem.Items.Add(menuItem5);
			MenuItem menuItem6 = new MenuItem
			{
				Header = "Delete all data folder backups",
				IsEnabled = false
			};
			menuItem6.Click += new RoutedEventHandler(this.DeleteDataFolderBackupsMenuItem_OnClick);
			menuItem.Items.Add(menuItem6);
			menuItem.Items.Add(new Separator());
			IReadOnlyList<DataBackupSummary> availableBackups;
			try
			{
				availableBackups = this._dataBackupService.GetAvailableBackups();
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
			{
				Trace.TraceWarning("Failed to access data backups: {0}", new object[]
				{
					ex.Message
				});
				menuItem.Items.Add(new MenuItem
				{
					Header = "Backups unavailable",
					IsEnabled = false
				});
				return;
			}
			string dataDirectory = this._dataDirectory;
			DataBackupSummary[] array;
			if (string.IsNullOrWhiteSpace(dataDirectory))
			{
				array = Array.Empty<DataBackupSummary>();
			}
			else
			{
				array = Enumerable.ToArray<DataBackupSummary>(Enumerable.Where<DataBackupSummary>(availableBackups, (DataBackupSummary summary) => MainWindow.IsSameDirectory(summary.SourceDataDirectory, dataDirectory)));
			}
			string text = VersionStringUtility.Normalize(VintageStoryVersionLocator.GetInstalledVersion(this._gameDirectory));
			menuItem6.IsEnabled = (!string.IsNullOrWhiteSpace(dataDirectory) && !string.IsNullOrWhiteSpace(text) && array.Length != 0);
			if (array.Length == 0)
			{
				string header2 = string.IsNullOrWhiteSpace(dataDirectory) ? "Set VintagestoryData folder to restore backups" : "No backups for this VintagestoryData folder";
				menuItem.Items.Add(new MenuItem
				{
					Header = header2,
					IsEnabled = false
				});
				return;
			}
			DataBackupSummary[] array2 = Enumerable.ToArray<DataBackupSummary>(Enumerable.Take<DataBackupSummary>(array, 15));
			foreach (DataBackupSummary dataBackupSummary in array2)
			{
				string text2 = dataBackupSummary.CreatedOnUtc.ToLocalTime().ToString("dd MMM yyyy '•' HH:mm:ss", CultureInfo.CurrentCulture);
				string text3 = string.IsNullOrWhiteSpace(dataBackupSummary.Id) ? text2 : (text2 + " — " + dataBackupSummary.Id);
				if (!string.IsNullOrWhiteSpace(dataBackupSummary.VintageStoryVersion))
				{
					text3 = text3 + " — VS " + dataBackupSummary.VintageStoryVersion;
				}
				MenuItem menuItem7 = new MenuItem
				{
					Header = text3,
					Tag = dataBackupSummary
				};
				menuItem7.Click += new RoutedEventHandler(this.RestoreDataFolderMenuItem_OnBackupClick);
				menuItem.Items.Add(menuItem7);
			}
			if (array.Length > array2.Length)
			{
				ItemCollection items = menuItem.Items;
				MenuItem menuItem8 = new MenuItem();
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(27, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("Showing latest ");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(array2.Length);
				defaultInterpolatedStringHandler2.AppendLiteral(" of ");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(array.Length);
				defaultInterpolatedStringHandler2.AppendLiteral(" backups");
				menuItem8.Header = defaultInterpolatedStringHandler2.ToStringAndClear();
				menuItem8.IsEnabled = false;
				items.Add(menuItem8);
			}
		}

		// Token: 0x06000275 RID: 629 RVA: 0x000114C0 File Offset: 0x0000F6C0
		private void RestoreDataFolderMenuItem_OnBackupClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<RestoreDataFolderMenuItem_OnBackupClick>d__446 <RestoreDataFolderMenuItem_OnBackupClick>d__;
			<RestoreDataFolderMenuItem_OnBackupClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<RestoreDataFolderMenuItem_OnBackupClick>d__.<>4__this = this;
			<RestoreDataFolderMenuItem_OnBackupClick>d__.sender = sender;
			<RestoreDataFolderMenuItem_OnBackupClick>d__.<>1__state = -1;
			<RestoreDataFolderMenuItem_OnBackupClick>d__.<>t__builder.Start<MainWindow.<RestoreDataFolderMenuItem_OnBackupClick>d__446>(ref <RestoreDataFolderMenuItem_OnBackupClick>d__);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00011500 File Offset: 0x0000F700
		private void OpenDataBackupDirectoryMenuItem_OnClick([Nullable(2)] object sender, RoutedEventArgs e)
		{
			string backupRootDirectory = this._dataBackupService.GetBackupRootDirectory();
			try
			{
				if (string.IsNullOrWhiteSpace(backupRootDirectory))
				{
					ModManagerMessageBox.Show("The data backup directory is not available.", "Simple VS Manager", 0, 48, null, null, null);
				}
				else
				{
					Directory.CreateDirectory(backupRootDirectory);
					Process.Start(new ProcessStartInfo
					{
						FileName = backupRootDirectory,
						UseShellExecute = true
					});
				}
			}
			catch (Win32Exception ex)
			{
				ModManagerMessageBox.Show("Failed to open the data backup directory:\n" + ex.Message, "Simple VS Manager", 0, 16, null, null, null);
			}
			catch (Exception ex2) when (ex2 is IOException || ex2 is UnauthorizedAccessException || ex2 is InvalidOperationException)
			{
				ModManagerMessageBox.Show("Failed to open the data backup directory:\n" + ex2.Message, "Simple VS Manager", 0, 16, null, null, null);
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x000115F4 File Offset: 0x0000F7F4
		private void ChangeBackupLocationMenuItem_OnClick([Nullable(2)] object sender, RoutedEventArgs e)
		{
			string backupRootDirectory = this._dataBackupService.GetBackupRootDirectory();
			using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
			{
				Description = "Select a folder where backups will be saved and loaded from.",
				UseDescriptionForTitle = true,
				ShowNewFolderButton = true
			})
			{
				if (!string.IsNullOrWhiteSpace(backupRootDirectory) && Directory.Exists(backupRootDirectory))
				{
					folderBrowserDialog.InitialDirectory = backupRootDirectory;
					folderBrowserDialog.SelectedPath = backupRootDirectory;
				}
				if (folderBrowserDialog.ShowDialog() == 1)
				{
					string selectedPath = folderBrowserDialog.SelectedPath;
					if (!string.IsNullOrWhiteSpace(selectedPath))
					{
						try
						{
							Directory.CreateDirectory(selectedPath);
							this._userConfiguration.SetCustomDataBackupLocation(selectedPath);
							this._dataBackupService = new DataBackupService(this._userConfiguration.GetConfigurationDirectory(), this._userConfiguration.CustomDataBackupLocation);
							MainViewModel viewModel = this._viewModel;
							if (viewModel != null)
							{
								viewModel.ReportStatus("Backup location changed to: " + selectedPath, false);
							}
						}
						catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is NotSupportedException)
						{
							ModManagerMessageBox.Show("Failed to set the backup location:\n" + ex.Message, "Simple VS Manager", 0, 16, null, null, null);
						}
					}
				}
			}
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00011740 File Offset: 0x0000F940
		private void SetMaximumDataBackupsMenuItem_OnClick([Nullable(2)] object sender, RoutedEventArgs e)
		{
			string text = Interaction.InputBox("Enter the maximum number of VintagestoryData backups to keep for the current data folder and Vintage Story version. Use 0 for unlimited.", "Set Maximum Backups", this._userConfiguration.MaximumDataBackups.ToString(CultureInfo.InvariantCulture), -1, -1);
			if (string.IsNullOrWhiteSpace(text))
			{
				return;
			}
			int num;
			if (!int.TryParse(text.Trim(), 7, CultureInfo.InvariantCulture, ref num) || num < 0)
			{
				ModManagerMessageBox.Show("Enter 0 for unlimited backups, or a positive whole number.", "Simple VS Manager", 0, 48, null, null, null);
				return;
			}
			this._userConfiguration.SetMaximumDataBackups(num);
			if (num <= 0)
			{
				MainViewModel viewModel = this._viewModel;
				if (viewModel == null)
				{
					return;
				}
				viewModel.ReportStatus("VintagestoryData backup limit set to unlimited.", false);
				return;
			}
			else
			{
				int num2 = this.PruneDataBackupsForCurrentContext();
				MainViewModel viewModel2 = this._viewModel;
				if (viewModel2 == null)
				{
					return;
				}
				string message;
				if (num2 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(62, 2);
					defaultInterpolatedStringHandler.AppendLiteral("VintagestoryData backup limit set to ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num);
					defaultInterpolatedStringHandler.AppendLiteral("; deleted ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(num2);
					defaultInterpolatedStringHandler.AppendLiteral(" old backup(s).");
					message = defaultInterpolatedStringHandler.ToStringAndClear();
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
					defaultInterpolatedStringHandler2..ctor(38, 1);
					defaultInterpolatedStringHandler2.AppendLiteral("VintagestoryData backup limit set to ");
					defaultInterpolatedStringHandler2.AppendFormatted<int>(num);
					defaultInterpolatedStringHandler2.AppendLiteral(".");
					message = defaultInterpolatedStringHandler2.ToStringAndClear();
				}
				viewModel2.ReportStatus(message, false);
				return;
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0001186C File Offset: 0x0000FA6C
		private int PruneDataBackupsForCurrentContext()
		{
			int maximumDataBackups = this._userConfiguration.MaximumDataBackups;
			if (maximumDataBackups <= 0)
			{
				return 0;
			}
			if (string.IsNullOrWhiteSpace(this._dataDirectory))
			{
				return 0;
			}
			string installedVersion = VintageStoryVersionLocator.GetInstalledVersion(this._gameDirectory);
			string text = VersionStringUtility.Normalize(installedVersion);
			if (string.IsNullOrWhiteSpace(text))
			{
				return 0;
			}
			int result;
			try
			{
				result = this._dataBackupService.PruneBackups(this._dataDirectory, installedVersion ?? text, maximumDataBackups);
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is InvalidOperationException || ex is ArgumentException)
			{
				Trace.TraceWarning("Failed to prune VintagestoryData backups: {0}", new object[]
				{
					ex.Message
				});
				result = 0;
			}
			return result;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00011944 File Offset: 0x0000FB44
		private void DeleteDataFolderBackupsMenuItem_OnClick([Nullable(2)] object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(this._dataDirectory))
			{
				ModManagerMessageBox.Show("Set the VintagestoryData folder before deleting backups.", "Simple VS Manager", 0, 48, null, null, null);
				return;
			}
			string installedVersion = VintageStoryVersionLocator.GetInstalledVersion(this._gameDirectory);
			string text = VersionStringUtility.Normalize(installedVersion);
			if (string.IsNullOrWhiteSpace(text))
			{
				ModManagerMessageBox.Show("The installed Vintage Story version could not be determined, so backups cannot be deleted safely.", "Simple VS Manager", 0, 48, null, null, null);
				return;
			}
			string text2 = installedVersion ?? text;
			if (ModManagerMessageBox.Show("Delete all VintagestoryData backups for Vintage Story " + text2 + "? This action cannot be undone.", "Simple VS Manager", 4, 48, null, null, null) != 6)
			{
				return;
			}
			try
			{
				int num = this._dataBackupService.DeleteBackups(this._dataDirectory, text2);
				if (num == 0)
				{
					ModManagerMessageBox.Show("No backups matching the current data folder and Vintage Story version were found.", "Simple VS Manager", 0, 64, null, null, null);
				}
				else
				{
					MainViewModel viewModel = this._viewModel;
					if (viewModel != null)
					{
						string message;
						if (num != 1)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Deleted ");
							defaultInterpolatedStringHandler.AppendFormatted<int>(num);
							defaultInterpolatedStringHandler.AppendLiteral(" VintagestoryData backups.");
							message = defaultInterpolatedStringHandler.ToStringAndClear();
						}
						else
						{
							message = "Deleted 1 VintagestoryData backup.";
						}
						viewModel.ReportStatus(message, false);
					}
					string messageBoxText;
					if (num != 1)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(34, 1);
						defaultInterpolatedStringHandler2.AppendLiteral("Deleted ");
						defaultInterpolatedStringHandler2.AppendFormatted<int>(num);
						defaultInterpolatedStringHandler2.AppendLiteral(" VintagestoryData backups.");
						messageBoxText = defaultInterpolatedStringHandler2.ToStringAndClear();
					}
					else
					{
						messageBoxText = "Deleted 1 VintagestoryData backup.";
					}
					ModManagerMessageBox.Show(messageBoxText, "Simple VS Manager", 0, 64, null, null, null);
				}
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is InvalidOperationException || ex is ArgumentException)
			{
				ModManagerMessageBox.Show("Failed to delete the VintagestoryData backups:\n" + ex.Message, "Simple VS Manager", 0, 16, null, null, null);
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00011B18 File Offset: 0x0000FD18
		private void RestoreBackupMenuItem_OnSubmenuOpened(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			menuItem.Items.Clear();
			string text;
			try
			{
				text = this.EnsureBackupDirectory();
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
			{
				Trace.TraceWarning("Failed to access backup directory: {0}", new object[]
				{
					ex.Message
				});
				menuItem.Items.Add(new MenuItem
				{
					Header = "Backups unavailable",
					IsEnabled = false
				});
				return;
			}
			string[] files;
			try
			{
				files = Directory.GetFiles(text, "*.json");
			}
			catch (Exception ex2) when (ex2 is IOException || ex2 is UnauthorizedAccessException)
			{
				Trace.TraceWarning("Failed to enumerate backups: {0}", new object[]
				{
					ex2.Message
				});
				menuItem.Items.Add(new MenuItem
				{
					Header = "Backups unavailable",
					IsEnabled = false
				});
				return;
			}
			if (files.Length == 0)
			{
				menuItem.Items.Add(new MenuItem
				{
					Header = "No backups available",
					IsEnabled = false
				});
				return;
			}
			Array.Sort<string>(files, (string left, string right) => File.GetLastWriteTimeUtc(right).CompareTo(File.GetLastWriteTimeUtc(left)));
			bool flag = false;
			string[] array = files;
			int i = 0;
			while (i < array.Length)
			{
				string text2 = array[i];
				if (!MainWindow.IsAppStartedBackup(text2))
				{
					goto IL_17E;
				}
				if (!flag)
				{
					flag = true;
					goto IL_17E;
				}
				IL_1BF:
				i++;
				continue;
				IL_17E:
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text2);
				MenuItem menuItem2 = new MenuItem
				{
					Header = fileNameWithoutExtension,
					Tag = text2
				};
				menuItem2.Click += new RoutedEventHandler(this.RestoreBackupMenuItem_OnBackupClick);
				menuItem.Items.Add(menuItem2);
				goto IL_1BF;
			}
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00011D10 File Offset: 0x0000FF10
		private void RestoreBackupMenuItem_OnBackupClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<RestoreBackupMenuItem_OnBackupClick>d__453 <RestoreBackupMenuItem_OnBackupClick>d__;
			<RestoreBackupMenuItem_OnBackupClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<RestoreBackupMenuItem_OnBackupClick>d__.<>4__this = this;
			<RestoreBackupMenuItem_OnBackupClick>d__.sender = sender;
			<RestoreBackupMenuItem_OnBackupClick>d__.<>1__state = -1;
			<RestoreBackupMenuItem_OnBackupClick>d__.<>t__builder.Start<MainWindow.<RestoreBackupMenuItem_OnBackupClick>d__453>(ref <RestoreBackupMenuItem_OnBackupClick>d__);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00011D50 File Offset: 0x0000FF50
		private Task RestoreBackupAsync(string backupPath, bool restoreConfigurations)
		{
			MainWindow.<RestoreBackupAsync>d__454 <RestoreBackupAsync>d__;
			<RestoreBackupAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RestoreBackupAsync>d__.<>4__this = this;
			<RestoreBackupAsync>d__.backupPath = backupPath;
			<RestoreBackupAsync>d__.restoreConfigurations = restoreConfigurations;
			<RestoreBackupAsync>d__.<>1__state = -1;
			<RestoreBackupAsync>d__.<>t__builder.Start<MainWindow.<RestoreBackupAsync>d__454>(ref <RestoreBackupAsync>d__);
			return <RestoreBackupAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00011DA4 File Offset: 0x0000FFA4
		private Task RestoreDataBackupAsync(DataBackupSummary summary)
		{
			MainWindow.<RestoreDataBackupAsync>d__455 <RestoreDataBackupAsync>d__;
			<RestoreDataBackupAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RestoreDataBackupAsync>d__.<>4__this = this;
			<RestoreDataBackupAsync>d__.summary = summary;
			<RestoreDataBackupAsync>d__.<>1__state = -1;
			<RestoreDataBackupAsync>d__.<>t__builder.Start<MainWindow.<RestoreDataBackupAsync>d__455>(ref <RestoreDataBackupAsync>d__);
			return <RestoreDataBackupAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00011DEF File Offset: 0x0000FFEF
		private IProgress<DataBackupProgress> CreateDataBackupProgressReporter(string fallbackMessage)
		{
			return new Progress<DataBackupProgress>(delegate(DataBackupProgress value)
			{
				string dataBackupStatusMessage = string.IsNullOrWhiteSpace(value.Status) ? fallbackMessage : value.Status;
				this.DataBackupStatusMessage = dataBackupStatusMessage;
				this.DataBackupProgress = (double.IsNaN(value.Percent) ? 0.0 : Math.Clamp(value.Percent, 0.0, 100.0));
			});
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00011E14 File Offset: 0x00010014
		private void ShowDataBackupOverlay(string message)
		{
			this.DataBackupProgress = 0.0;
			this.DataBackupStatusMessage = message;
			this.IsDataBackupInProgress = true;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00011E33 File Offset: 0x00010033
		private void HideDataBackupOverlay()
		{
			this.IsDataBackupInProgress = false;
			this.DataBackupProgress = 0.0;
			this.DataBackupStatusMessage = string.Empty;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00011E58 File Offset: 0x00010058
		private void BeginModlistInstallUi(int totalSteps, string message)
		{
			this._modlistInstallTotalSteps = Math.Max(1, totalSteps);
			this._modlistInstallCompletedSteps = 0;
			this.ModlistInstallProgress = 0.0;
			this.ModlistInstallStatusMessage = message;
			this.UpdateModlistDownloadSpeed(default(double?));
			this.IsModlistInstallInProgress = true;
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00011EA8 File Offset: 0x000100A8
		private void UpdateModlistInstallUi(string status, double? stepPercent, double? bytesPerSecond)
		{
			if (!this.IsModlistInstallInProgress)
			{
				return;
			}
			double num = MainWindow.ClampPercent(stepPercent);
			double num2 = ((double)this._modlistInstallCompletedSteps + num / 100.0) / (double)this._modlistInstallTotalSteps * 100.0;
			this.ModlistInstallProgress = Math.Clamp(num2, 0.0, 100.0);
			this.ModlistInstallStatusMessage = status;
			this.UpdateModlistDownloadSpeed(bytesPerSecond);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00011F18 File Offset: 0x00010118
		private void CompleteModlistInstallStep(string status)
		{
			if (!this.IsModlistInstallInProgress)
			{
				return;
			}
			this._modlistInstallCompletedSteps = Math.Min(this._modlistInstallCompletedSteps + 1, this._modlistInstallTotalSteps);
			this.ModlistInstallProgress = (double)this._modlistInstallCompletedSteps / (double)this._modlistInstallTotalSteps * 100.0;
			this.ModlistInstallStatusMessage = status;
			this.UpdateModlistDownloadSpeed(default(double?));
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00011F7C File Offset: 0x0001017C
		private void EndModlistInstallUi()
		{
			this.IsModlistInstallInProgress = false;
			this.ModlistInstallProgress = 0.0;
			this.ModlistInstallStatusMessage = string.Empty;
			this.UpdateModlistDownloadSpeed(default(double?));
			this._modlistInstallCompletedSteps = 0;
			this._modlistInstallTotalSteps = 0;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00011FC8 File Offset: 0x000101C8
		private static double ClampPercent(double? value)
		{
			if (value == null || double.IsNaN(value.Value))
			{
				return 0.0;
			}
			return Math.Clamp(value.Value, 0.0, 100.0);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00012014 File Offset: 0x00010214
		private void UpdateModlistDownloadSpeed(double? bytesPerSecond)
		{
			if (bytesPerSecond != null && bytesPerSecond.Value > 0.0)
			{
				this.ModlistDownloadSpeed = MainWindow.FormatDownloadSpeed(bytesPerSecond.Value);
				this.HasModlistDownloadSpeed = true;
				return;
			}
			this.ModlistDownloadSpeed = string.Empty;
			this.HasModlistDownloadSpeed = false;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00012068 File Offset: 0x00010268
		private static string FormatDownloadSpeed(double bytesPerSecond)
		{
			string result;
			if (bytesPerSecond >= 1024.0)
			{
				if (bytesPerSecond >= 1048576.0)
				{
					if (bytesPerSecond >= 1073741824.0)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
						defaultInterpolatedStringHandler..ctor(5, 1);
						defaultInterpolatedStringHandler.AppendFormatted<double>(bytesPerSecond / 1073741824.0, "0.0");
						defaultInterpolatedStringHandler.AppendLiteral(" GB/s");
						result = defaultInterpolatedStringHandler.ToStringAndClear();
					}
					else
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
						defaultInterpolatedStringHandler2..ctor(5, 1);
						defaultInterpolatedStringHandler2.AppendFormatted<double>(bytesPerSecond / 1048576.0, "0.0");
						defaultInterpolatedStringHandler2.AppendLiteral(" MB/s");
						result = defaultInterpolatedStringHandler2.ToStringAndClear();
					}
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3;
					defaultInterpolatedStringHandler3..ctor(5, 1);
					defaultInterpolatedStringHandler3.AppendFormatted<double>(bytesPerSecond / 1024.0, "0.0");
					defaultInterpolatedStringHandler3.AppendLiteral(" KB/s");
					result = defaultInterpolatedStringHandler3.ToStringAndClear();
				}
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4;
				defaultInterpolatedStringHandler4..ctor(4, 1);
				defaultInterpolatedStringHandler4.AppendFormatted<double>(bytesPerSecond, "0");
				defaultInterpolatedStringHandler4.AppendLiteral(" B/s");
				result = defaultInterpolatedStringHandler4.ToStringAndClear();
			}
			return result;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0001216E File Offset: 0x0001036E
		private IProgress<ModUpdateProgress> CreateModlistInstallProgressReporter(string modDisplayName)
		{
			string display = string.IsNullOrWhiteSpace(modDisplayName) ? "Mod" : modDisplayName;
			return new Progress<ModUpdateProgress>(delegate(ModUpdateProgress p)
			{
				string status = display + ": " + p.Message;
				double? bytesPerSecond = (p.Stage == ModUpdateStage.Downloading) ? p.BytesPerSecond : default(double?);
				this.UpdateModlistInstallUi(status, p.Percent, bytesPerSecond);
			});
		}

		// Token: 0x0600028A RID: 650 RVA: 0x000121A2 File Offset: 0x000103A2
		private IProgress<ModUpdateProgress> CreateModUpdateProgressReporter(string modDisplayName, [Nullable(2)] IProgress<ModUpdateProgress> additionalProgress = null)
		{
			string display = string.IsNullOrWhiteSpace(modDisplayName) ? "Mod" : modDisplayName;
			return new Progress<ModUpdateProgress>(delegate(ModUpdateProgress p)
			{
				IProgress<ModUpdateProgress> additionalProgress2 = additionalProgress;
				if (additionalProgress2 != null)
				{
					additionalProgress2.Report(p);
				}
				MainViewModel viewModel = this._viewModel;
				if (viewModel == null)
				{
					return;
				}
				viewModel.ReportStatus(display + ": " + p.Message, false);
			});
		}

		// Token: 0x0600028B RID: 651 RVA: 0x000121DD File Offset: 0x000103DD
		private void OpenModFolderButton_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.OpenFolder((this._dataDirectory == null) ? null : Path.Combine(this._dataDirectory, "Mods"), "mods");
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00012204 File Offset: 0x00010404
		private void OpenConfigFolderButton_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.OpenFolder((this._dataDirectory == null) ? null : Path.Combine(this._dataDirectory, "ModConfig"), "config");
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0001222B File Offset: 0x0001042B
		private void OpenLogsFolderButton_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.OpenFolder((this._dataDirectory == null) ? null : Path.Combine(this._dataDirectory, "Logs"), "logs");
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00012254 File Offset: 0x00010454
		private static void OpenFolder([Nullable(2)] string path, string description)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				ModManagerMessageBox.Show("The " + description + " folder is not available. Please verify the VintagestoryData folder from File > Set Data Folder.", "Simple VS Manager", 0, 48, null, null, null);
				return;
			}
			if (!Directory.Exists(path))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(106, 2);
				defaultInterpolatedStringHandler.AppendLiteral("The ");
				defaultInterpolatedStringHandler.AppendFormatted(description);
				defaultInterpolatedStringHandler.AppendLiteral(" folder could not be found at:\n");
				defaultInterpolatedStringHandler.AppendFormatted(path);
				defaultInterpolatedStringHandler.AppendLiteral("\nPlease verify the VintagestoryData folder from File > Set Data Folder.");
				ModManagerMessageBox.Show(defaultInterpolatedStringHandler.ToStringAndClear(), "Simple VS Manager", 0, 48, null, null, null);
				return;
			}
			try
			{
				MainWindow.OpenFolderWithShell(path);
			}
			catch (Exception ex)
			{
				ModManagerMessageBox.Show("Failed to open the " + description + " folder:\n" + ex.Message, "Simple VS Manager", 0, 16, null, null, null);
			}
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00012330 File Offset: 0x00010530
		private static void OpenFolderWithShell(string path)
		{
			if (OperatingSystem.IsWindows())
			{
				ProcessStartInfo processStartInfo = new ProcessStartInfo();
				processStartInfo.FileName = "explorer.exe";
				processStartInfo.UseShellExecute = true;
				processStartInfo.ArgumentList.Add(path);
				Process.Start(processStartInfo);
				return;
			}
			Process.Start(new ProcessStartInfo
			{
				FileName = path,
				UseShellExecute = true
			});
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00012388 File Offset: 0x00010588
		private bool TryGetManagedModPath(ModListItemViewModel mod, out string fullPath, [Nullable(2)] out string errorMessage)
		{
			fullPath = string.Empty;
			errorMessage = null;
			if (this._dataDirectory == null)
			{
				errorMessage = "The VintagestoryData folder is not available. Please verify it from File > Set Data Folder.";
				return false;
			}
			if (string.IsNullOrWhiteSpace(mod.SourcePath))
			{
				errorMessage = "This mod does not have a known source path and cannot be deleted automatically.";
				return false;
			}
			try
			{
				fullPath = Path.GetFullPath(mod.SourcePath);
			}
			catch (Exception)
			{
				errorMessage = "The mod path is invalid and cannot be deleted automatically.";
				return false;
			}
			if (!this.IsPathWithinManagedMods(fullPath))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(92, 4);
				defaultInterpolatedStringHandler.AppendLiteral("This mod is located outside of the Mods folder and cannot be deleted automatically.");
				defaultInterpolatedStringHandler.AppendFormatted(Environment.NewLine);
				defaultInterpolatedStringHandler.AppendFormatted(Environment.NewLine);
				defaultInterpolatedStringHandler.AppendLiteral("Location:");
				defaultInterpolatedStringHandler.AppendFormatted(Environment.NewLine);
				defaultInterpolatedStringHandler.AppendFormatted(fullPath);
				errorMessage = defaultInterpolatedStringHandler.ToStringAndClear();
				return false;
			}
			return this.TryEnsureManagedModTargetIsSafe(fullPath, out errorMessage);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00012468 File Offset: 0x00010668
		private bool TryGetDependencyInstallTargetPath(string modId, ModReleaseInfo release, out string fullPath, [Nullable(2)] out string errorMessage)
		{
			fullPath = string.Empty;
			errorMessage = null;
			if (this._dataDirectory == null)
			{
				errorMessage = "The VintagestoryData folder is not available. Please verify it from File > Set Data Folder.";
				return false;
			}
			string text = Path.Combine(this._dataDirectory, "Mods");
			try
			{
				Directory.CreateDirectory(text);
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException || ex is NotSupportedException)
			{
				errorMessage = "The Mods folder could not be accessed:" + Environment.NewLine + ex.Message;
				return false;
			}
			string text2 = string.IsNullOrWhiteSpace(modId) ? "mod" : modId;
			string text3 = string.IsNullOrWhiteSpace(release.Version) ? "latest" : release.Version;
			string fallback = text2 + "-" + text3 + ".zip";
			string fileName = release.FileName;
			if (!string.IsNullOrWhiteSpace(fileName))
			{
				fileName = Path.GetFileName(fileName);
			}
			string text4 = MainWindow.SanitizeFileName(fileName, fallback);
			if (string.IsNullOrWhiteSpace(Path.GetExtension(text4)))
			{
				text4 += ".zip";
			}
			string path = Path.Combine(text, text4);
			fullPath = MainWindow.EnsureUniqueFilePath(path);
			return true;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x000125B0 File Offset: 0x000107B0
		private bool TryGetInstallTargetPath(ModListItemViewModel mod, ModReleaseInfo release, out string fullPath, [Nullable(2)] out string errorMessage)
		{
			fullPath = string.Empty;
			errorMessage = null;
			if (this._dataDirectory == null)
			{
				errorMessage = "The VintagestoryData folder is not available. Please verify it from File > Set Data Folder.";
				return false;
			}
			string text = Path.Combine(this._dataDirectory, "Mods");
			try
			{
				Directory.CreateDirectory(text);
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException || ex is NotSupportedException)
			{
				errorMessage = "The Mods folder could not be accessed:" + Environment.NewLine + ex.Message;
				return false;
			}
			string text2 = string.IsNullOrWhiteSpace(mod.ModId) ? "mod" : mod.ModId;
			string text3 = string.IsNullOrWhiteSpace(release.Version) ? "latest" : release.Version;
			string fallback = text2 + "-" + text3 + ".zip";
			string fileName = release.FileName;
			if (!string.IsNullOrWhiteSpace(fileName))
			{
				fileName = Path.GetFileName(fileName);
			}
			string text4 = MainWindow.SanitizeFileName(fileName, fallback);
			if (string.IsNullOrWhiteSpace(Path.GetExtension(text4)))
			{
				text4 += ".zip";
			}
			string path = Path.Combine(text, text4);
			fullPath = MainWindow.EnsureUniqueFilePath(path);
			return true;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00012700 File Offset: 0x00010900
		private bool TryGetUpdateTargetPath(ModListItemViewModel mod, ModReleaseInfo release, string existingPath, out string fullPath, [Nullable(2)] out string errorMessage)
		{
			fullPath = string.Empty;
			errorMessage = null;
			if (string.IsNullOrWhiteSpace(existingPath))
			{
				errorMessage = "The mod path could not be determined.";
				return false;
			}
			string directoryName;
			try
			{
				directoryName = Path.GetDirectoryName(existingPath);
			}
			catch (Exception ex) when (ex is ArgumentException || ex is PathTooLongException)
			{
				errorMessage = "The mod path is invalid:" + Environment.NewLine + ex.Message;
				return false;
			}
			if (string.IsNullOrWhiteSpace(directoryName))
			{
				errorMessage = "The mod path could not be determined.";
				return false;
			}
			string text = string.IsNullOrWhiteSpace(mod.ModId) ? "mod" : mod.ModId;
			string text2 = string.IsNullOrWhiteSpace(release.Version) ? "latest" : release.Version;
			string fallback = text + "-" + text2 + ".zip";
			string fileName = release.FileName;
			if (!string.IsNullOrWhiteSpace(fileName))
			{
				fileName = Path.GetFileName(fileName);
			}
			string text3 = MainWindow.SanitizeFileName(fileName, fallback);
			if (string.IsNullOrWhiteSpace(Path.GetExtension(text3)))
			{
				text3 += ".zip";
			}
			fullPath = Path.Combine(directoryName, text3);
			return true;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00012838 File Offset: 0x00010A38
		private bool IsPathWithinManagedMods(string fullPath)
		{
			if (this._dataDirectory == null)
			{
				return false;
			}
			string directory = Path.Combine(this._dataDirectory, "Mods");
			string directory2 = Path.Combine(this._dataDirectory, "ModsByServer");
			return MainWindow.IsPathUnderDirectory(fullPath, directory) || MainWindow.IsPathUnderDirectory(fullPath, directory2);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00012884 File Offset: 0x00010A84
		private static bool IsPathUnderDirectory(string path, [Nullable(2)] string directory)
		{
			if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(directory))
			{
				return false;
			}
			bool result;
			try
			{
				string text = Path.GetFullPath(path).TrimEnd(new char[]
				{
					Path.DirectorySeparatorChar,
					Path.AltDirectorySeparatorChar
				});
				string text2 = Path.GetFullPath(directory).TrimEnd(new char[]
				{
					Path.DirectorySeparatorChar,
					Path.AltDirectorySeparatorChar
				});
				if (text.Length < text2.Length)
				{
					result = false;
				}
				else if (!text.StartsWith(text2, 5))
				{
					result = false;
				}
				else if (text.Length == text2.Length)
				{
					result = true;
				}
				else
				{
					char c = text.get_Chars(text2.Length);
					result = (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar);
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00012954 File Offset: 0x00010B54
		[NullableContext(2)]
		private static bool IsSameDirectory(string first, string second)
		{
			if (string.IsNullOrWhiteSpace(first) || string.IsNullOrWhiteSpace(second))
			{
				return false;
			}
			bool result;
			try
			{
				string text = Path.TrimEndingDirectorySeparator(Path.GetFullPath(first));
				string text2 = Path.TrimEndingDirectorySeparator(Path.GetFullPath(second));
				StringComparison stringComparison = OperatingSystem.IsWindows() ? 5 : 4;
				result = string.Equals(text, text2, stringComparison);
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x000129B8 File Offset: 0x00010BB8
		private static string SanitizeFileName([Nullable(2)] string fileName, string fallback)
		{
			object obj = string.IsNullOrWhiteSpace(fileName) ? fallback : fileName;
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			object obj2 = obj;
			StringBuilder stringBuilder = new StringBuilder(obj2.Length);
			string text = obj2;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text.get_Chars(i);
				stringBuilder.Append((Array.IndexOf<char>(invalidFileNameChars, c) >= 0) ? '_' : c);
			}
			string text2 = stringBuilder.ToString().Trim();
			if (!string.IsNullOrWhiteSpace(text2))
			{
				return text2;
			}
			return fallback;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00012A34 File Offset: 0x00010C34
		private static string EnsureUniqueFilePath(string path)
		{
			if (!File.Exists(path))
			{
				return path;
			}
			string text = Path.GetDirectoryName(path);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
			string extension = Path.GetExtension(path);
			if (string.IsNullOrWhiteSpace(text))
			{
				text = Directory.GetCurrentDirectory();
			}
			int num = 1;
			string text3;
			do
			{
				string text2 = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(3, 3);
				defaultInterpolatedStringHandler.AppendFormatted(fileNameWithoutExtension);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				defaultInterpolatedStringHandler.AppendFormatted(extension);
				text3 = Path.Combine(text2, defaultInterpolatedStringHandler.ToStringAndClear());
				num++;
			}
			while (File.Exists(text3));
			return text3;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00012AC8 File Offset: 0x00010CC8
		private bool TryEnsureManagedModTargetIsSafe(string fullPath, [Nullable(2)] out string errorMessage)
		{
			errorMessage = null;
			FileSystemInfo fileSystemInfo = null;
			if (Directory.Exists(fullPath))
			{
				fileSystemInfo = new DirectoryInfo(fullPath);
			}
			else if (File.Exists(fullPath))
			{
				fileSystemInfo = new FileInfo(fullPath);
			}
			if (fileSystemInfo == null)
			{
				return true;
			}
			if (!fileSystemInfo.Attributes.HasFlag(1024))
			{
				return true;
			}
			bool result;
			try
			{
				FileSystemInfo fileSystemInfo2 = fileSystemInfo.ResolveLinkTarget(true);
				if (fileSystemInfo2 == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(112, 4);
					defaultInterpolatedStringHandler.AppendLiteral("This mod is a symbolic link and its target could not be resolved. It will not be deleted automatically.");
					defaultInterpolatedStringHandler.AppendFormatted(Environment.NewLine);
					defaultInterpolatedStringHandler.AppendFormatted(Environment.NewLine);
					defaultInterpolatedStringHandler.AppendLiteral("Location:");
					defaultInterpolatedStringHandler.AppendFormatted(Environment.NewLine);
					defaultInterpolatedStringHandler.AppendFormatted(fullPath);
					errorMessage = defaultInterpolatedStringHandler.ToStringAndClear();
					result = false;
				}
				else
				{
					string fullPath2 = Path.GetFullPath(fileSystemInfo2.FullName);
					if (!this.IsPathWithinManagedMods(fullPath2))
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(115, 4);
						defaultInterpolatedStringHandler2.AppendLiteral("This mod is a symbolic link that points outside of the Mods folder and cannot be deleted automatically.");
						defaultInterpolatedStringHandler2.AppendFormatted(Environment.NewLine);
						defaultInterpolatedStringHandler2.AppendFormatted(Environment.NewLine);
						defaultInterpolatedStringHandler2.AppendLiteral("Link target:");
						defaultInterpolatedStringHandler2.AppendFormatted(Environment.NewLine);
						defaultInterpolatedStringHandler2.AppendFormatted(fullPath2);
						errorMessage = defaultInterpolatedStringHandler2.ToStringAndClear();
						result = false;
					}
					else
					{
						result = true;
					}
				}
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is NotSupportedException)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3;
				defaultInterpolatedStringHandler3..ctor(95, 8);
				defaultInterpolatedStringHandler3.AppendLiteral("This mod is a symbolic link that could not be validated for automatic deletion.");
				defaultInterpolatedStringHandler3.AppendFormatted(Environment.NewLine);
				defaultInterpolatedStringHandler3.AppendFormatted(Environment.NewLine);
				defaultInterpolatedStringHandler3.AppendLiteral("Location:");
				defaultInterpolatedStringHandler3.AppendFormatted(Environment.NewLine);
				defaultInterpolatedStringHandler3.AppendFormatted(fullPath);
				defaultInterpolatedStringHandler3.AppendFormatted(Environment.NewLine);
				defaultInterpolatedStringHandler3.AppendFormatted(Environment.NewLine);
				defaultInterpolatedStringHandler3.AppendLiteral("Reason:");
				defaultInterpolatedStringHandler3.AppendFormatted(Environment.NewLine);
				defaultInterpolatedStringHandler3.AppendFormatted(ex.Message);
				errorMessage = defaultInterpolatedStringHandler3.ToStringAndClear();
				result = false;
			}
			return result;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00012CE8 File Offset: 0x00010EE8
		private bool TrySaveSnapshot(string directory, string title, string filter, string folderWarningMessage, string fallbackName, [Nullable(2)] Func<string> suggestedNameProvider, [Nullable(new byte[]
		{
			2,
			1
		})] Action<string> onSuccess, string failureContext, bool includeModVersions, bool exclusive, [Nullable(new byte[]
		{
			2,
			1,
			1,
			1
		})] IReadOnlyDictionary<string, IReadOnlyList<ModConfigurationSnapshot>> includedConfigurations = null, [Nullable(new byte[]
		{
			2,
			1
		})] Action<SerializablePreset> configureSerializable = null)
		{
			if (this._viewModel == null)
			{
				return false;
			}
			SaveFileDialog dialog = new SaveFileDialog
			{
				Title = title,
				Filter = filter,
				DefaultExt = ".json",
				AddExtension = true,
				OverwritePrompt = true,
				InitialDirectory = directory
			};
			dialog.FileOk += delegate([Nullable(2)] object _, CancelEventArgs args)
			{
				if (MainWindow.IsPathWithinDirectory(directory, dialog.FileName))
				{
					return;
				}
				ModManagerMessageBox.Show(folderWarningMessage, "Simple VS Manager", 0, 48, null, null, null);
				args.Cancel = true;
			};
			string text = (suggestedNameProvider != null) ? suggestedNameProvider.Invoke() : null;
			if (!string.IsNullOrWhiteSpace(text))
			{
				dialog.FileName = MainWindow.BuildSuggestedFileName(text, fallbackName);
			}
			if (!dialog.ShowDialog(this).GetValueOrDefault())
			{
				return false;
			}
			string text2 = dialog.FileName;
			string text3 = MainWindow.BuildSuggestedFileName(Path.GetFileNameWithoutExtension(text2), fallbackName);
			if (!string.Equals(text3, Path.GetFileNameWithoutExtension(text2), 4))
			{
				text2 = Path.Combine(directory, text3 + ".json");
			}
			SerializablePreset serializablePreset = this.BuildSerializablePreset(text3, includeModVersions, exclusive, includedConfigurations, this.ResolveGameVersion(null));
			if (configureSerializable != null)
			{
				configureSerializable.Invoke(serializablePreset);
			}
			bool result;
			try
			{
				JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
				{
					WriteIndented = true,
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
					DefaultIgnoreCondition = 3
				};
				string text4 = JsonSerializer.Serialize<SerializablePreset>(serializablePreset, jsonSerializerOptions);
				File.WriteAllText(text2, text4);
				if (onSuccess != null)
				{
					onSuccess.Invoke(text3);
				}
				result = true;
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
			{
				ModManagerMessageBox.Show("Failed to save the " + failureContext + ":\n" + ex.Message, "Simple VS Manager", 0, 16, null, null, null);
				result = false;
			}
			return result;
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00012EC8 File Offset: 0x000110C8
		private SerializablePreset BuildSerializablePreset(string entryName, bool includeModVersions, bool exclusive, [Nullable(new byte[]
		{
			2,
			1,
			1,
			1
		})] IReadOnlyDictionary<string, IReadOnlyList<ModConfigurationSnapshot>> includedConfigurations = null, [Nullable(2)] string gameVersion = null)
		{
			if (this._viewModel == null)
			{
				throw new InvalidOperationException("View model is not initialized.");
			}
			IReadOnlyList<ModPresetModState> currentModStates = this._viewModel.GetCurrentModStates();
			List<SerializablePresetModState> list = new List<SerializablePresetModState>(currentModStates.Count);
			foreach (ModPresetModState modPresetModState in currentModStates)
			{
				if (modPresetModState != null)
				{
					string text = string.IsNullOrWhiteSpace(modPresetModState.ModId) ? modPresetModState.ModId : modPresetModState.ModId.Trim();
					SerializablePresetModState serializablePresetModState = new SerializablePresetModState
					{
						ModId = text,
						Version = ((includeModVersions && !string.IsNullOrWhiteSpace(modPresetModState.Version)) ? modPresetModState.Version.Trim() : null),
						IsActive = modPresetModState.IsActive
					};
					if (!string.IsNullOrWhiteSpace(text))
					{
						string text2 = text;
						IReadOnlyList<ModConfigurationSnapshot> readOnlyList;
						if (includedConfigurations != null && includedConfigurations.TryGetValue(text2, ref readOnlyList) && readOnlyList != null && readOnlyList.Count > 0)
						{
							ModConfigurationSnapshot modConfigurationSnapshot = readOnlyList[0];
							serializablePresetModState.ConfigurationFileName = modConfigurationSnapshot.FileName;
							serializablePresetModState.ConfigurationContent = ModConfigurationEncoding.Encode(modConfigurationSnapshot.Content);
						}
						else if (modPresetModState.ConfigurationContent != null)
						{
							serializablePresetModState.ConfigurationFileName = MainWindow.GetSafeConfigFileName(modPresetModState.ConfigurationFileName, text2);
							serializablePresetModState.ConfigurationContent = ModConfigurationEncoding.Encode(ModConfigurationEncoding.Decode(modPresetModState.ConfigurationContent));
						}
					}
					list.Add(serializablePresetModState);
				}
			}
			SerializableConfigList serializableConfigList = MainWindow.BuildSerializableConfigList(includedConfigurations);
			return new SerializablePreset
			{
				Name = entryName,
				IncludeModStatus = new bool?(true),
				IncludeModVersions = (includeModVersions ? new bool?(true) : default(bool?)),
				Exclusive = (exclusive ? new bool?(true) : default(bool?)),
				Mods = list,
				GameVersion = (string.IsNullOrWhiteSpace(gameVersion) ? null : gameVersion.Trim()),
				Configurations = ((serializableConfigList != null) ? serializableConfigList.Configurations : null)
			};
		}

		// Token: 0x0600029C RID: 668 RVA: 0x000130CC File Offset: 0x000112CC
		[NullableContext(2)]
		private string ResolveGameVersion(string requestedVersion)
		{
			if (!string.IsNullOrWhiteSpace(requestedVersion))
			{
				return requestedVersion.Trim();
			}
			MainViewModel viewModel = this._viewModel;
			string text = (viewModel != null) ? viewModel.InstalledGameVersion : null;
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text.Trim();
			}
			return null;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0001310C File Offset: 0x0001130C
		[NullableContext(2)]
		private static SerializableConfigList BuildSerializableConfigList([Nullable(new byte[]
		{
			2,
			1,
			1,
			1
		})] IReadOnlyDictionary<string, IReadOnlyList<ModConfigurationSnapshot>> includedConfigurations)
		{
			if (includedConfigurations == null || includedConfigurations.Count == 0)
			{
				return null;
			}
			List<SerializableModConfiguration> list = new List<SerializableModConfiguration>();
			foreach (KeyValuePair<string, IReadOnlyList<ModConfigurationSnapshot>> keyValuePair in includedConfigurations)
			{
				if (keyValuePair.Key != null && keyValuePair.Value != null && keyValuePair.Value.Count != 0)
				{
					string text = keyValuePair.Key.Trim();
					if (!string.IsNullOrWhiteSpace(text))
					{
						foreach (ModConfigurationSnapshot modConfigurationSnapshot in keyValuePair.Value)
						{
							if (modConfigurationSnapshot != null)
							{
								string fileName = string.IsNullOrWhiteSpace(modConfigurationSnapshot.FileName) ? null : modConfigurationSnapshot.FileName.Trim();
								string content = modConfigurationSnapshot.Content ?? string.Empty;
								list.Add(new SerializableModConfiguration
								{
									ModId = text,
									FileName = fileName,
									RelativePath = modConfigurationSnapshot.RelativePath,
									Content = content
								});
							}
						}
					}
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			list.Sort(delegate(SerializableModConfiguration left, SerializableModConfiguration right)
			{
				int num = string.Compare((left != null) ? left.ModId : null, (right != null) ? right.ModId : null, 5);
				if (num != 0)
				{
					return num;
				}
				return string.Compare((left != null) ? left.FileName : null, (right != null) ? right.FileName : null, 5);
			});
			return new SerializableConfigList
			{
				Configurations = list
			};
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00013284 File Offset: 0x00011484
		private void SavePresetMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			string directory = this.EnsurePresetDirectory();
			this.TrySaveSnapshot(directory, "Save Mod Preset", "Preset files (*.json)|*.json|All files (*.*)|*.*", "Presets must be saved inside the Presets folder.", "Preset", () => this._userConfiguration.GetLastSelectedPresetName(), delegate(string name)
			{
				this._userConfiguration.SetLastSelectedPresetName(name);
				MainViewModel viewModel = this._viewModel;
				if (viewModel == null)
				{
					return;
				}
				viewModel.ReportStatus("Saved preset \"" + name + "\".", false);
			}, "preset", false, false, null, null);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x000132D8 File Offset: 0x000114D8
		private bool TrySaveModlist()
		{
			string text;
			return this.TrySaveModlist(null, out text);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x000132F0 File Offset: 0x000114F0
		[NullableContext(2)]
		private bool TrySaveModlist(Func<string> suggestedNameProvider, out string savedFilePath)
		{
			savedFilePath = null;
			List<ModConfigOption> list = this.BuildModConfigOptions(true);
			string text = (suggestedNameProvider != null) ? suggestedNameProvider.Invoke() : null;
			string defaultListName = text;
			IEnumerable<ModConfigOption> configOptions = list;
			string uploaderNameForPdf = this.GetUploaderNameForPdf();
			string defaultVersion = null;
			MainViewModel viewModel = this._viewModel;
			SaveInstalledModsDialog saveInstalledModsDialog = new SaveInstalledModsDialog(defaultListName, configOptions, uploaderNameForPdf, defaultVersion, (viewModel != null) ? viewModel.InstalledGameVersion : null, SaveInstalledModsDialogResult.SaveJson)
			{
				Owner = this
			};
			if (!saveInstalledModsDialog.ShowDialog().GetValueOrDefault())
			{
				return false;
			}
			string listName = saveInstalledModsDialog.ListName;
			string version = saveInstalledModsDialog.Version;
			string description = saveInstalledModsDialog.Description;
			string text2 = saveInstalledModsDialog.CreatedBy;
			text2 = (string.IsNullOrWhiteSpace(text2) ? this.GetUploaderNameForPdf() : text2.Trim());
			string gameVersion = this.ResolveGameVersion(saveInstalledModsDialog.VintageStoryVersion);
			IReadOnlyList<ModConfigOption> selectedConfigOptions = saveInstalledModsDialog.GetSelectedConfigOptions();
			Dictionary<string, IReadOnlyList<ModConfigurationSnapshot>> includedConfigurations = this.TryReadModConfigurations(selectedConfigOptions);
			if (saveInstalledModsDialog.SelectedAction == SaveInstalledModsDialogResult.SavePdf)
			{
				return this.TrySaveInstalledModsPdf(listName, version, description, text2, includedConfigurations, gameVersion, null);
			}
			bool result;
			try
			{
				string text3 = this.EnsureModListDirectory();
				string text4 = MainWindow.BuildSuggestedFileName((!string.IsNullOrWhiteSpace(listName)) ? listName : text, "Modlist");
				string text5 = Path.Combine(text3, text4 + ".json");
				if (File.Exists(text5))
				{
					string messageBoxText = "A modlist named \"" + Path.GetFileName(text5) + "\" already exists in the Modlists folder. Do you want to replace it?";
					if (ModManagerMessageBox.Show(this, messageBoxText, "Replace Modlist", 4, 32, null, null, null) != 6)
					{
						return false;
					}
				}
				SerializablePreset serializablePreset = this.BuildSerializablePreset(text4, true, true, includedConfigurations, gameVersion);
				if (!string.IsNullOrWhiteSpace(listName))
				{
					serializablePreset.Name = listName.Trim();
				}
				serializablePreset.Description = description;
				serializablePreset.Version = version;
				serializablePreset.Uploader = (string.IsNullOrWhiteSpace(text2) ? null : text2.Trim());
				JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
				{
					WriteIndented = true,
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
					DefaultIgnoreCondition = 3
				};
				string text6 = JsonSerializer.Serialize<SerializablePreset>(serializablePreset, jsonSerializerOptions);
				File.WriteAllText(text5, text6);
				MainViewModel viewModel2 = this._viewModel;
				if (viewModel2 != null)
				{
					viewModel2.ReportStatus("Saved modlist \"" + text4 + "\".", false);
				}
				savedFilePath = text5;
				result = true;
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is NotSupportedException)
			{
				ModManagerMessageBox.Show("Failed to save the modlist:\n" + ex.Message, "Simple VS Manager", 0, 16, null, null, null);
				result = false;
			}
			return result;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0001356C File Offset: 0x0001176C
		private bool TrySaveAutomaticModlist(string requestedName, out string savedName, out string filePath)
		{
			savedName = string.Empty;
			filePath = string.Empty;
			if (this._viewModel == null)
			{
				return false;
			}
			string text = this.EnsureRebuiltModListDirectory();
			savedName = MainWindow.BuildSuggestedFileName(requestedName, "Modlist");
			filePath = Path.Combine(text, savedName + ".json");
			SerializablePreset serializablePreset = this.BuildSerializablePreset(savedName, true, true, null, this.ResolveGameVersion(null));
			bool result;
			try
			{
				JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
				{
					WriteIndented = true,
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
					DefaultIgnoreCondition = 3
				};
				string text2 = JsonSerializer.Serialize<SerializablePreset>(serializablePreset, jsonSerializerOptions);
				File.WriteAllText(filePath, text2);
				this._viewModel.ReportStatus("Saved modlist \"" + savedName + "\".", false);
				result = true;
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
			{
				ModManagerMessageBox.Show("Failed to save the modlist:\n" + ex.Message, "Simple VS Manager", 0, 16, null, null, null);
				savedName = string.Empty;
				filePath = string.Empty;
				result = false;
			}
			return result;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00013694 File Offset: 0x00011894
		private MainWindow.ModlistLoadMode? PromptModlistLoadMode()
		{
			ModlistAutoLoadBehavior modlistAutoLoadBehavior = this._userConfiguration.ModlistAutoLoadBehavior;
			if (modlistAutoLoadBehavior == ModlistAutoLoadBehavior.Replace)
			{
				return new MainWindow.ModlistLoadMode?(MainWindow.ModlistLoadMode.Replace);
			}
			if (modlistAutoLoadBehavior != ModlistAutoLoadBehavior.Add)
			{
				MessageDialogButtonContentOverrides buttonContentOverrides = new MessageDialogButtonContentOverrides
				{
					Yes = "Only Modlist mods",
					No = "Add Modlist mods"
				};
				MessageBoxResult messageBoxResult = ModManagerMessageBox.Show(this, "How would you like to load the modlist?\n\nOnly Modlist mods: Delete your current mods and install only the mods from the modlist.\nAdd Modlist mods: Keep your current mods and add any missing mods from the modlist.\nCancel: Do nothing.", "Load Modlist", 3, 32, null, buttonContentOverrides, null);
				MainWindow.ModlistLoadMode? result;
				if (messageBoxResult != 6)
				{
					if (messageBoxResult != 7)
					{
						result = default(MainWindow.ModlistLoadMode?);
					}
					else
					{
						result = new MainWindow.ModlistLoadMode?(MainWindow.ModlistLoadMode.Add);
					}
				}
				else
				{
					result = new MainWindow.ModlistLoadMode?(MainWindow.ModlistLoadMode.Replace);
				}
				return result;
			}
			return new MainWindow.ModlistLoadMode?(MainWindow.ModlistLoadMode.Add);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00013720 File Offset: 0x00011920
		private MainWindow.PresetLoadOptions GetModlistLoadOptions(MainWindow.ModlistLoadMode mode)
		{
			if (mode == MainWindow.ModlistLoadMode.Replace)
			{
				return MainWindow.ModListLoadOptions;
			}
			return new MainWindow.PresetLoadOptions(MainWindow.ModListLoadOptions.ApplyModStatus, MainWindow.ModListLoadOptions.ApplyModVersions, false);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00013748 File Offset: 0x00011948
		private bool EnsureModlistBackupBeforeLoad()
		{
			MessageBoxResult messageBoxResult;
			if (this._userConfiguration.SuppressModlistSavePrompt)
			{
				messageBoxResult = 7;
			}
			else
			{
				MessageDialogExtraButton extraButton = new MessageDialogExtraButton("No, don't ask again", 7, delegate
				{
					this._userConfiguration.SetSuppressModlistSavePrompt(true);
				}, false, false);
				messageBoxResult = ModManagerMessageBox.Show("Would you like to backup your current mods as a Modlist before loading the selected Modlist? Your current mods will be deleted! ", "Simple VS Manager", 3, 32, extraButton, null, null);
			}
			if (messageBoxResult == 2)
			{
				return false;
			}
			if (messageBoxResult == 6)
			{
				string text;
				bool flag = this.TrySaveModlist(null, out text);
				if (flag)
				{
					if (!string.IsNullOrWhiteSpace(text))
					{
						this.RefreshLocalModlists(true, new string[]
						{
							text
						});
					}
					else
					{
						this.RefreshLocalModlists(true, null);
					}
				}
				return flag;
			}
			return true;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x000137D4 File Offset: 0x000119D4
		private bool TryBuildCurrentModlistJson(string modlistName, [Nullable(2)] string description, [Nullable(2)] string version, string uploader, [Nullable(new byte[]
		{
			2,
			1,
			1,
			1
		})] IReadOnlyDictionary<string, IReadOnlyList<ModConfigurationSnapshot>> includedConfigurations, [Nullable(2)] string gameVersion, out string json)
		{
			json = string.Empty;
			string text = string.IsNullOrWhiteSpace(modlistName) ? null : modlistName.Trim();
			if (string.IsNullOrEmpty(text) || this._viewModel == null)
			{
				return false;
			}
			SerializablePreset serializablePreset = this.BuildSerializablePreset(text, true, true, includedConfigurations, gameVersion);
			serializablePreset.Description = (string.IsNullOrWhiteSpace(description) ? null : description.Trim());
			serializablePreset.Version = (string.IsNullOrWhiteSpace(version) ? null : version.Trim());
			serializablePreset.Uploader = (string.IsNullOrWhiteSpace(uploader) ? null : uploader.Trim());
			JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
			{
				WriteIndented = true,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				DefaultIgnoreCondition = 3
			};
			json = JsonSerializer.Serialize<SerializablePreset>(serializablePreset, jsonSerializerOptions);
			return true;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0001388C File Offset: 0x00011A8C
		private Task CreateAutomaticBackupAsync(string trigger)
		{
			return this.CreateBackupAsync(trigger, "Backup", true, false);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0001389C File Offset: 0x00011A9C
		private Task CreateAppStartedBackupAsync()
		{
			return this.CreateBackupAsync("AppStarted", "Backup_AppStarted", false, true);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x000138B0 File Offset: 0x00011AB0
		private Task CreateBackupAsync(string trigger, string fallbackFileName, bool pruneAutomaticBackups, bool pruneAppStartedBackups)
		{
			MainWindow.<CreateBackupAsync>d__497 <CreateBackupAsync>d__;
			<CreateBackupAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CreateBackupAsync>d__.<>4__this = this;
			<CreateBackupAsync>d__.trigger = trigger;
			<CreateBackupAsync>d__.fallbackFileName = fallbackFileName;
			<CreateBackupAsync>d__.pruneAutomaticBackups = pruneAutomaticBackups;
			<CreateBackupAsync>d__.pruneAppStartedBackups = pruneAppStartedBackups;
			<CreateBackupAsync>d__.<>1__state = -1;
			<CreateBackupAsync>d__.<>t__builder.Start<MainWindow.<CreateBackupAsync>d__497>(ref <CreateBackupAsync>d__);
			return <CreateBackupAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00013914 File Offset: 0x00011B14
		[return: Nullable(new byte[]
		{
			2,
			1,
			1,
			1
		})]
		private IReadOnlyDictionary<string, IReadOnlyList<ModConfigurationSnapshot>> CaptureConfigurationsForBackup(IReadOnlyList<ModListItemViewModel> mods)
		{
			if (mods == null || mods.Count == 0)
			{
				return null;
			}
			Dictionary<string, List<ModConfigurationSnapshot>> dictionary = new Dictionary<string, List<ModConfigurationSnapshot>>(StringComparer.OrdinalIgnoreCase);
			foreach (ModListItemViewModel modListItemViewModel in mods)
			{
				if (modListItemViewModel != null && !string.IsNullOrWhiteSpace(modListItemViewModel.ModId))
				{
					string text = modListItemViewModel.ModId.Trim();
					if (!dictionary.ContainsKey(text))
					{
						IEnumerable<string> enumerable = Enumerable.Select<string, string>(Enumerable.Where<string>(this._userConfiguration.GetModConfigPaths(text), (string path) => !string.IsNullOrWhiteSpace(path)), (string path) => path.Trim());
						Func<string, bool> func;
						if ((func = MainWindow.<>O.<2>__Exists) == null)
						{
							func = (MainWindow.<>O.<2>__Exists = new Func<string, bool>(File.Exists));
						}
						List<string> list = Enumerable.ToList<string>(Enumerable.Where<string>(enumerable, func));
						if (list.Count != 0)
						{
							foreach (string text2 in list)
							{
								try
								{
									string content = File.ReadAllText(text2);
									string safeConfigFileName = MainWindow.GetSafeConfigFileName(Path.GetFileName(text2), text);
									string relativePath = this.TryGetRelativeConfigPath(text2, safeConfigFileName);
									List<ModConfigurationSnapshot> list2;
									if (!dictionary.TryGetValue(text, ref list2))
									{
										list2 = new List<ModConfigurationSnapshot>();
										dictionary[text] = list2;
									}
									list2.Add(new ModConfigurationSnapshot(safeConfigFileName, content, relativePath));
								}
								catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException || ex is NotSupportedException)
								{
									Trace.TraceWarning("Failed to include configuration file {0} for mod {1} in backup: {2}", new object[]
									{
										text2,
										text,
										ex.Message
									});
								}
							}
						}
					}
				}
			}
			if (dictionary.Count <= 0)
			{
				return null;
			}
			return Enumerable.ToDictionary<KeyValuePair<string, List<ModConfigurationSnapshot>>, string, IReadOnlyList<ModConfigurationSnapshot>>(dictionary, (KeyValuePair<string, List<ModConfigurationSnapshot>> pair) => pair.Key, (KeyValuePair<string, List<ModConfigurationSnapshot>> pair) => pair.Value, StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00013BA8 File Offset: 0x00011DA8
		[NullableContext(2)]
		private static bool IsAppStartedBackup(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return false;
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
			return fileNameWithoutExtension.EndsWith("_AppStarted", 5) || fileNameWithoutExtension.Contains("-- AppStarted", 5);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00013BE4 File Offset: 0x00011DE4
		private static void PruneAutomaticBackups(string directory)
		{
			try
			{
				IEnumerable<string> enumerable = Enumerable.Where<string>(Directory.GetFiles(directory, "*.json"), (string file) => !MainWindow.IsAppStartedBackup(file));
				Func<string, DateTime> func;
				if ((func = MainWindow.<>O.<3>__GetLastWriteTimeUtc) == null)
				{
					func = (MainWindow.<>O.<3>__GetLastWriteTimeUtc = new Func<string, DateTime>(File.GetLastWriteTimeUtc));
				}
				string[] array = Enumerable.ToArray<string>(Enumerable.OrderByDescending<string, DateTime>(enumerable, func));
				if (array.Length > 10)
				{
					for (int i = 10; i < array.Length; i++)
					{
						string text = array[i];
						try
						{
							File.Delete(text);
						}
						catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
						{
							Trace.TraceWarning("Failed to delete backup {0}: {1}", new object[]
							{
								text,
								ex.Message
							});
						}
					}
				}
			}
			catch (Exception ex2) when (ex2 is IOException || ex2 is UnauthorizedAccessException)
			{
				Trace.TraceWarning("Failed to prune backups in {0}: {1}", new object[]
				{
					directory,
					ex2.Message
				});
			}
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00013D28 File Offset: 0x00011F28
		private static void PruneAppStartedBackups(string directory)
		{
			try
			{
				IEnumerable<string> files = Directory.GetFiles(directory, "*.json");
				Func<string, bool> func;
				if ((func = MainWindow.<>O.<4>__IsAppStartedBackup) == null)
				{
					func = (MainWindow.<>O.<4>__IsAppStartedBackup = new Func<string, bool>(MainWindow.IsAppStartedBackup));
				}
				IEnumerable<string> enumerable = Enumerable.Where<string>(files, func);
				Func<string, DateTime> func2;
				if ((func2 = MainWindow.<>O.<3>__GetLastWriteTimeUtc) == null)
				{
					func2 = (MainWindow.<>O.<3>__GetLastWriteTimeUtc = new Func<string, DateTime>(File.GetLastWriteTimeUtc));
				}
				string[] array = Enumerable.ToArray<string>(Enumerable.OrderByDescending<string, DateTime>(enumerable, func2));
				if (array.Length > 10)
				{
					for (int i = 10; i < array.Length; i++)
					{
						string text = array[i];
						try
						{
							File.Delete(text);
						}
						catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
						{
							Trace.TraceWarning("Failed to delete backup {0}: {1}", new object[]
							{
								text,
								ex.Message
							});
						}
					}
				}
			}
			catch (Exception ex2) when (ex2 is IOException || ex2 is UnauthorizedAccessException)
			{
				Trace.TraceWarning("Failed to prune backups in {0}: {1}", new object[]
				{
					directory,
					ex2.Message
				});
			}
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00013E68 File Offset: 0x00012068
		private static string BuildCloudModlistName()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(8, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Modlist ");
			defaultInterpolatedStringHandler.AppendFormatted<DateTime>(DateTime.Now, "yyyy-MM-dd HH:mm");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00013EA2 File Offset: 0x000120A2
		private static void EnsureQuestPdfLicense()
		{
			if (MainWindow._isQuestPdfLicenseInitialized)
			{
				return;
			}
			Settings.License = new LicenseType?(0);
			MainWindow._isQuestPdfLicenseInitialized = true;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00013EBD File Offset: 0x000120BD
		private static string[] GetLines(string content)
		{
			if (string.IsNullOrEmpty(content))
			{
				return Array.Empty<string>();
			}
			return content.ReplaceLineEndings("\n").Split('\n', 0);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00013EE0 File Offset: 0x000120E0
		private static void GenerateInstalledModsPdf(string filePath, string listName, [Nullable(2)] string modlistVersion, [Nullable(2)] string description, string uploaderName, [Nullable(2)] string gameVersion, IReadOnlyList<ModListItemViewModel> mods, SerializablePreset serializable, [Nullable(2)] SerializableConfigList configList)
		{
			MainWindow.EnsureQuestPdfLicense();
			string normalizedListName = string.IsNullOrWhiteSpace(listName) ? "Installed Mods" : listName.Trim();
			string normalizedVersion = string.IsNullOrWhiteSpace(modlistVersion) ? null : modlistVersion.Trim();
			string normalizedDescription = ((description != null) ? description.Trim() : null) ?? string.Empty;
			string resolvedGameVersion = string.IsNullOrWhiteSpace(gameVersion) ? "Unknown" : gameVersion.Trim();
			string encodedModlist = PdfModlistSerializer.SerializeToBase64(serializable);
			string encodedConfigList = (configList == null) ? null : PdfModlistSerializer.SerializeConfigListToBase64(configList);
			string subject = PdfModlistSerializer.CreateModlistMetadataValue(encodedModlist);
			string keywords = PdfModlistSerializer.CreateConfigMetadataValue(encodedConfigList);
			DocumentMetadata documentMetadata = new DocumentMetadata
			{
				Title = normalizedListName,
				Author = uploaderName,
				Subject = subject,
				Keywords = keywords,
				Creator = "Simple VS Manager",
				Producer = "Simple VS Manager"
			};
			Action<TextDescriptor> <>9__5;
			Action<ColumnDescriptor> <>9__6;
			Action<ColumnDescriptor> <>9__3;
			Action<PageDescriptor> <>9__1;
			GenerateExtensions.GeneratePdf(Document.Create(delegate(IDocumentContainer container)
			{
				Action<PageDescriptor> action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate(PageDescriptor page)
					{
						page.Size(PageSizes.A4);
						page.Margin(40f, 0);
						page.PageColor(Colors.White);
						page.DefaultTextStyle((TextStyle style) => TextStyleExtensions.FontSize(style, 12f));
						IContainer container = page.Content();
						Action<ColumnDescriptor> action2;
						if ((action2 = <>9__3) == null)
						{
							action2 = (<>9__3 = delegate(ColumnDescriptor column)
							{
								column.Spacing(6f, 0);
								TextSpanDescriptorExtensions.Bold<TextBlockDescriptor>(TextSpanDescriptorExtensions.FontSize<TextBlockDescriptor>(TextExtensions.Text(column.Item(), normalizedListName), 32f));
								if (!string.IsNullOrEmpty(normalizedVersion))
								{
									TextSpanDescriptorExtensions.Italic<TextBlockDescriptor>(TextSpanDescriptorExtensions.FontSize<TextBlockDescriptor>(TextExtensions.Text(column.Item(), "Version: " + normalizedVersion), 12f), true);
								}
								TextExtensions.Text(column.Item(), delegate(TextDescriptor text)
								{
									text.DefaultTextStyle((TextStyle style) => TextStyleExtensions.FontSize(style, 10f));
									text.Span("Generated with Simple VS Manager.");
									text.EmptyLine();
									text.Span("Download the app from the ");
									TextSpanDescriptorExtensions.FontColor<TextSpanDescriptor>(text.Hyperlink("Vintage Story ModDB", "https://mods.vintagestory.at/simplevsmanager"), Colors.Blue.Medium);
									text.Span(" or ");
									TextSpanDescriptorExtensions.FontColor<TextSpanDescriptor>(text.Hyperlink("Github", "https://github.com/Interzoneism/Simple-Mod-Manager"), Colors.Blue.Medium);
									text.Span(" to easily load this pdf as a modlist!");
								});
								TextSpanDescriptorExtensions.FontSize<TextBlockDescriptor>(TextExtensions.Text(column.Item(), "Made for VS version " + resolvedGameVersion), 14f);
								TextSpanDescriptorExtensions.FontSize<TextBlockDescriptor>(TextExtensions.Text(column.Item(), "Modlist by " + uploaderName), 14f);
								IContainer container2 = column.Item();
								Action<TextDescriptor> action3;
								if ((action3 = <>9__5) == null)
								{
									action3 = (<>9__5 = delegate(TextDescriptor text)
									{
										text.DefaultTextStyle((TextStyle style) => TextStyleExtensions.FontSize(style, 10f));
										text.DefaultTextStyle((TextStyle style) => TextStyleExtensions.Italic(style, true));
										if (string.IsNullOrEmpty(normalizedDescription))
										{
											return;
										}
										string[] lines = MainWindow.GetLines(normalizedDescription);
										for (int i = 0; i < lines.Length; i++)
										{
											text.Line(lines[i]);
										}
									});
								}
								TextExtensions.Text(container2, action3);
								TextSpanDescriptorExtensions.Bold<TextBlockDescriptor>(TextSpanDescriptorExtensions.FontSize<TextBlockDescriptor>(TextExtensions.Text(column.Item(), "Mods in this list:"), 12f));
								IContainer container3 = column.Item();
								Action<ColumnDescriptor> action4;
								if ((action4 = <>9__6) == null)
								{
									action4 = (<>9__6 = delegate(ColumnDescriptor modColumn)
									{
										modColumn.Spacing(0f, 0);
										foreach (ModListItemViewModel modListItemViewModel in mods)
										{
											if (modListItemViewModel != null)
											{
												string text3 = string.IsNullOrWhiteSpace(modListItemViewModel.DisplayName) ? (string.IsNullOrWhiteSpace(modListItemViewModel.ModId) ? "Unknown Mod" : modListItemViewModel.ModId.Trim()) : modListItemViewModel.DisplayName.Trim();
												string text2 = string.IsNullOrWhiteSpace(modListItemViewModel.Version) ? string.Empty : modListItemViewModel.Version.Trim();
												string modLine = string.IsNullOrEmpty(text2) ? text3 : (text3 + " " + text2);
												string modDatabaseUrl = string.IsNullOrWhiteSpace(modListItemViewModel.ModDatabasePageUrl) ? null : modListItemViewModel.ModDatabasePageUrl.Trim();
												TextExtensions.Text(modColumn.Item(), delegate(TextDescriptor text)
												{
													text.DefaultTextStyle((TextStyle style) => TextStyleExtensions.FontSize(style, 10f));
													if (!string.IsNullOrEmpty(modDatabaseUrl))
													{
														TextSpanDescriptorExtensions.FontColor<TextSpanDescriptor>(text.Hyperlink(modLine, modDatabaseUrl), Colors.Blue.Medium);
														return;
													}
													text.Span(modLine);
												});
											}
										}
									});
								}
								ColumnExtensions.Column(container3, action4);
							});
						}
						ColumnExtensions.Column(container, action2);
					});
				}
				PageExtensions.Page(container, action);
			}).WithMetadata(documentMetadata), filePath);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00013FF8 File Offset: 0x000121F8
		private string GetUploaderNameForPdf()
		{
			UserConfigurationService userConfiguration = this._userConfiguration;
			string text = (userConfiguration != null) ? userConfiguration.CloudUploaderName : null;
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text.Trim();
			}
			MainViewModel viewModel = this._viewModel;
			string text2 = (viewModel != null) ? viewModel.PlayerName : null;
			if (!string.IsNullOrWhiteSpace(text2))
			{
				return text2.Trim();
			}
			MainViewModel viewModel2 = this._viewModel;
			string text3;
			if ((text3 = ((viewModel2 != null) ? viewModel2.PlayerUid : null)) == null)
			{
				FirebaseModlistStore cloudModlistStore = this._cloudModlistStore;
				text3 = ((cloudModlistStore != null) ? cloudModlistStore.CurrentUserId : null);
			}
			string text4 = text3;
			if (!string.IsNullOrWhiteSpace(text4))
			{
				return this.ResolveUploaderName(text4);
			}
			return "Anonymous";
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00014088 File Offset: 0x00012288
		private string DetermineUploaderName(FirebaseModlistStore store)
		{
			string text = this.ResolveUploaderName((store != null) ? store.CurrentUserId : null);
			this.SetUsernameDisplay(text);
			return text;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x000140B0 File Offset: 0x000122B0
		private void SaveModlistMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			string text;
			if (this.TrySaveModlist(null, out text))
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					this.RefreshLocalModlists(true, new string[]
					{
						text
					});
					return;
				}
				this.RefreshLocalModlists(true, null);
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x000140EA File Offset: 0x000122EA
		private Task SaveModlistToCloudAsync()
		{
			return this.ExecuteCloudOperationAsync(delegate(FirebaseModlistStore store)
			{
				MainWindow.<<SaveModlistToCloudAsync>b__509_0>d <<SaveModlistToCloudAsync>b__509_0>d;
				<<SaveModlistToCloudAsync>b__509_0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<SaveModlistToCloudAsync>b__509_0>d.<>4__this = this;
				<<SaveModlistToCloudAsync>b__509_0>d.store = store;
				<<SaveModlistToCloudAsync>b__509_0>d.<>1__state = -1;
				<<SaveModlistToCloudAsync>b__509_0>d.<>t__builder.Start<MainWindow.<<SaveModlistToCloudAsync>b__509_0>d>(ref <<SaveModlistToCloudAsync>b__509_0>d);
				return <<SaveModlistToCloudAsync>b__509_0>d.<>t__builder.Task;
			}, "save the modlist to the cloud");
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00014103 File Offset: 0x00012303
		[NullableContext(2)]
		private static string NormalizeCloudVersion(string version)
		{
			if (!string.IsNullOrWhiteSpace(version))
			{
				return version.Trim();
			}
			return null;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00014118 File Offset: 0x00012318
		private List<ModConfigOption> BuildModConfigOptions(bool selectByDefault = true)
		{
			List<ModConfigOption> list = new List<ModConfigOption>();
			if (this._viewModel == null)
			{
				return list;
			}
			IEnumerable<ModListItemViewModel> installedModsSnapshot = this._viewModel.GetInstalledModsSnapshot();
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (ModListItemViewModel modListItemViewModel in installedModsSnapshot)
			{
				if (modListItemViewModel != null && !string.IsNullOrWhiteSpace(modListItemViewModel.ModId))
				{
					string text = modListItemViewModel.ModId.Trim();
					if (hashSet.Add(text))
					{
						List<string> list2 = Enumerable.ToList<string>(Enumerable.Where<string>(this._userConfiguration.GetModConfigPaths(text), (string path) => !string.IsNullOrWhiteSpace(path) && File.Exists(path)));
						if (list2.Count > 0)
						{
							list.Add(new ModConfigOption(text, modListItemViewModel.DisplayName, list2, selectByDefault));
						}
					}
				}
			}
			list.Sort((ModConfigOption left, ModConfigOption right) => string.Compare(left.DisplayName, right.DisplayName, 5));
			return list;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0001422C File Offset: 0x0001242C
		[return: Nullable(new byte[]
		{
			2,
			1,
			1,
			1
		})]
		private Dictionary<string, IReadOnlyList<ModConfigurationSnapshot>> TryReadModConfigurations(IReadOnlyList<ModConfigOption> selectedConfigOptions)
		{
			if (selectedConfigOptions == null || selectedConfigOptions.Count == 0)
			{
				return null;
			}
			Dictionary<string, List<ModConfigurationSnapshot>> dictionary = new Dictionary<string, List<ModConfigurationSnapshot>>(StringComparer.OrdinalIgnoreCase);
			List<string> list = new List<string>();
			foreach (ModConfigOption modConfigOption in selectedConfigOptions)
			{
				if (modConfigOption != null && modConfigOption.ConfigPaths.Count != 0)
				{
					foreach (string text in modConfigOption.ConfigPaths)
					{
						try
						{
							string content = File.ReadAllText(text);
							string safeConfigFileName = MainWindow.GetSafeConfigFileName(Path.GetFileName(text), modConfigOption.ModId);
							string relativePath = this.TryGetRelativeConfigPath(text, safeConfigFileName);
							List<ModConfigurationSnapshot> list2;
							if (!dictionary.TryGetValue(modConfigOption.ModId, ref list2))
							{
								list2 = new List<ModConfigurationSnapshot>();
								dictionary[modConfigOption.ModId] = list2;
							}
							list2.Add(new ModConfigurationSnapshot(safeConfigFileName, content, relativePath));
						}
						catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException || ex is NotSupportedException)
						{
							list.Add(modConfigOption.DisplayName + ": " + ex.Message);
						}
					}
				}
			}
			if (list.Count > 0)
			{
				ModManagerMessageBox.Show("Some configuration files could not be included:\n" + string.Join("\n", list), "Simple VS Manager", 0, 48, null, null, null);
			}
			if (dictionary.Count == 0)
			{
				return null;
			}
			return Enumerable.ToDictionary<KeyValuePair<string, List<ModConfigurationSnapshot>>, string, IReadOnlyList<ModConfigurationSnapshot>>(dictionary, (KeyValuePair<string, List<ModConfigurationSnapshot>> pair) => pair.Key, (KeyValuePair<string, List<ModConfigurationSnapshot>> pair) => pair.Value, StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00014460 File Offset: 0x00012660
		[return: Nullable(2)]
		private string TryGetRelativeConfigPath(string path, string sanitizedFileName)
		{
			if (string.IsNullOrWhiteSpace(this._dataDirectory))
			{
				return null;
			}
			string text = Path.Combine(this._dataDirectory, "ModConfig");
			if (!MainWindow.IsPathWithinDirectory(text, path))
			{
				return null;
			}
			string result;
			try
			{
				result = MainWindow.NormalizeRelativeConfigPath(Path.GetRelativePath(Path.GetFullPath(text), Path.GetFullPath(path)), sanitizedFileName);
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x000144CC File Offset: 0x000126CC
		[NullableContext(2)]
		private static string NormalizeRelativeConfigPath(string relativePath, [Nullable(1)] string sanitizedFileName)
		{
			if (string.IsNullOrWhiteSpace(relativePath))
			{
				return null;
			}
			string text = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar);
			if (string.IsNullOrWhiteSpace(text))
			{
				return null;
			}
			string directoryName = Path.GetDirectoryName(text);
			string text2 = Path.GetFileName(text);
			if (string.IsNullOrWhiteSpace(text2))
			{
				text2 = sanitizedFileName;
			}
			if (!string.IsNullOrWhiteSpace(directoryName))
			{
				return Path.Combine(directoryName, text2);
			}
			return text2;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00014534 File Offset: 0x00012734
		private static string EnsureUniqueRelativePath(string relativePath, HashSet<string> usedRelativePaths)
		{
			string text = relativePath;
			int num = 1;
			while (!usedRelativePaths.Add(text))
			{
				string directoryName = Path.GetDirectoryName(relativePath);
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(relativePath);
				string extension = Path.GetExtension(relativePath);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(1, 3);
				defaultInterpolatedStringHandler.AppendFormatted(fileNameWithoutExtension);
				defaultInterpolatedStringHandler.AppendLiteral("_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num++);
				defaultInterpolatedStringHandler.AppendFormatted(extension);
				string text2 = defaultInterpolatedStringHandler.ToStringAndClear();
				text = (string.IsNullOrWhiteSpace(directoryName) ? text2 : Path.Combine(directoryName, text2));
			}
			return text;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x000145B7 File Offset: 0x000127B7
		private static string[] GetSupportedConfigFiles(string directory)
		{
			return Enumerable.ToArray<string>(Enumerable.Where<string>(Directory.EnumerateFiles(directory, "*", 0), (string file) => MainWindow.IsSupportedConfigExtension(Path.GetExtension(file))));
		}

		// Token: 0x060002BC RID: 700 RVA: 0x000145F0 File Offset: 0x000127F0
		[NullableContext(2)]
		private static bool IsSupportedConfigExtension(string extension)
		{
			if (string.IsNullOrWhiteSpace(extension))
			{
				return false;
			}
			foreach (string text in MainWindow.SupportedConfigExtensions)
			{
				if (extension.Equals(text, 5))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0001462C File Offset: 0x0001282C
		[NullableContext(2)]
		[return: Nullable(1)]
		private static string GetSafeConfigFileName(string candidate, string modId)
		{
			string text = MainWindow.SanitizeForFileName(string.IsNullOrWhiteSpace(modId) ? "modconfig" : modId.Trim()).Trim();
			if (string.IsNullOrWhiteSpace(text))
			{
				text = "modconfig";
			}
			string text2 = string.IsNullOrWhiteSpace(candidate) ? null : candidate.Trim();
			string text3 = MainWindow.ResolveConfigExtension(text2);
			string value;
			if (string.IsNullOrWhiteSpace(text2))
			{
				value = text;
			}
			else
			{
				string fileName = Path.GetFileName(text2);
				string text4 = string.IsNullOrWhiteSpace(fileName) ? null : Path.GetFileNameWithoutExtension(fileName);
				value = (string.IsNullOrWhiteSpace(text4) ? text : text4);
			}
			string text5 = MainWindow.SanitizeForFileName(value).Trim();
			if (string.IsNullOrWhiteSpace(text5))
			{
				text5 = text;
			}
			return text5 + text3;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x000146DC File Offset: 0x000128DC
		private static string ResolveConfigExtension([Nullable(2)] string candidate)
		{
			if (!string.IsNullOrWhiteSpace(candidate))
			{
				string extension = Path.GetExtension(candidate);
				if (MainWindow.IsSupportedConfigExtension(extension))
				{
					return extension;
				}
			}
			return ".json";
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00014708 File Offset: 0x00012908
		private static string SanitizeForFileName([Nullable(2)] string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			StringBuilder stringBuilder = new StringBuilder(value.Length);
			for (int i = 0; i < value.Length; i++)
			{
				char c = value.get_Chars(i);
				stringBuilder.Append((Array.IndexOf<char>(invalidFileNameChars, c) >= 0) ? '_' : c);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00014770 File Offset: 0x00012970
		private void SaveModlistToCloudMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<SaveModlistToCloudMenuItem_OnClick>d__521 <SaveModlistToCloudMenuItem_OnClick>d__;
			<SaveModlistToCloudMenuItem_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<SaveModlistToCloudMenuItem_OnClick>d__.<>4__this = this;
			<SaveModlistToCloudMenuItem_OnClick>d__.<>1__state = -1;
			<SaveModlistToCloudMenuItem_OnClick>d__.<>t__builder.Start<MainWindow.<SaveModlistToCloudMenuItem_OnClick>d__521>(ref <SaveModlistToCloudMenuItem_OnClick>d__);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x000147A8 File Offset: 0x000129A8
		private void SaveCloudModlistButton_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<SaveCloudModlistButton_OnClick>d__522 <SaveCloudModlistButton_OnClick>d__;
			<SaveCloudModlistButton_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<SaveCloudModlistButton_OnClick>d__.<>4__this = this;
			<SaveCloudModlistButton_OnClick>d__.<>1__state = -1;
			<SaveCloudModlistButton_OnClick>d__.<>t__builder.Start<MainWindow.<SaveCloudModlistButton_OnClick>d__522>(ref <SaveCloudModlistButton_OnClick>d__);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x000147E0 File Offset: 0x000129E0
		private void ModifyCloudModlistsButton_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<ModifyCloudModlistsButton_OnClick>d__523 <ModifyCloudModlistsButton_OnClick>d__;
			<ModifyCloudModlistsButton_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<ModifyCloudModlistsButton_OnClick>d__.<>4__this = this;
			<ModifyCloudModlistsButton_OnClick>d__.<>1__state = -1;
			<ModifyCloudModlistsButton_OnClick>d__.<>t__builder.Start<MainWindow.<ModifyCloudModlistsButton_OnClick>d__523>(ref <ModifyCloudModlistsButton_OnClick>d__);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00014818 File Offset: 0x00012A18
		private Task<bool> IsCloudUploaderNameAvailableAsync(FirebaseModlistStore store, string uploader)
		{
			MainWindow.<IsCloudUploaderNameAvailableAsync>d__524 <IsCloudUploaderNameAvailableAsync>d__;
			<IsCloudUploaderNameAvailableAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<IsCloudUploaderNameAvailableAsync>d__.store = store;
			<IsCloudUploaderNameAvailableAsync>d__.uploader = uploader;
			<IsCloudUploaderNameAvailableAsync>d__.<>1__state = -1;
			<IsCloudUploaderNameAvailableAsync>d__.<>t__builder.Start<MainWindow.<IsCloudUploaderNameAvailableAsync>d__524>(ref <IsCloudUploaderNameAvailableAsync>d__);
			return <IsCloudUploaderNameAvailableAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x00014864 File Offset: 0x00012A64
		private void LocalModlistsDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.LocalModlistsDataGrid == null)
			{
				this.SetLocalModlistSelection(Array.Empty<LocalModlistListEntry>());
				return;
			}
			List<LocalModlistListEntry> localModlistSelection = Enumerable.ToList<LocalModlistListEntry>(Enumerable.OfType<LocalModlistListEntry>(this.LocalModlistsDataGrid.SelectedItems));
			this.SetLocalModlistSelection(localModlistSelection);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x000148A4 File Offset: 0x00012AA4
		private void GroupReferenceItemButton_OnClick(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			if (button != null)
			{
				ContextMenu contextMenu = button.ContextMenu;
				if (contextMenu != null)
				{
					contextMenu.PlacementTarget = button;
					contextMenu.Placement = 8;
					contextMenu.IsOpen = true;
					e.Handled = true;
					return;
				}
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x000148E4 File Offset: 0x00012AE4
		private void RemoveReferencedModlistMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem != null)
			{
				ModlistGroupMemberListEntry modlistGroupMemberListEntry = menuItem.DataContext as ModlistGroupMemberListEntry;
				if (modlistGroupMemberListEntry != null)
				{
					this.RemoveGroupReference(modlistGroupMemberListEntry);
				}
			}
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00014914 File Offset: 0x00012B14
		private void SelectReferencedModlistMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem != null)
			{
				ModlistGroupMemberListEntry modlistGroupMemberListEntry = menuItem.DataContext as ModlistGroupMemberListEntry;
				if (modlistGroupMemberListEntry != null)
				{
					this.SelectReferencedModlist(modlistGroupMemberListEntry);
				}
			}
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00014944 File Offset: 0x00012B44
		private void RemoveGroupReference(ModlistGroupMemberListEntry member)
		{
			LocalModlistListEntry localModlistListEntry = Enumerable.FirstOrDefault<LocalModlistListEntry>(Enumerable.OfType<LocalModlistListEntry>(this.LocalModlistsDataGrid.Items), (LocalModlistListEntry entry) => entry.IsGroup && string.Equals(entry.FilePath, member.GroupFilePath, 5));
			ModlistGroupDefinition modlistGroupDefinition = (localModlistListEntry != null) ? localModlistListEntry.GroupDefinition : null;
			if (modlistGroupDefinition == null)
			{
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(77, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Remove the reference to \"");
			defaultInterpolatedStringHandler.AppendFormatted(member.DisplayName);
			defaultInterpolatedStringHandler.AppendLiteral("\" from \"");
			defaultInterpolatedStringHandler.AppendFormatted(localModlistListEntry.BaseDisplayName);
			defaultInterpolatedStringHandler.AppendLiteral("\"? The original modlist will not be deleted.");
			if (ModManagerMessageBox.Show(this, defaultInterpolatedStringHandler.ToStringAndClear(), "Remove from Modlist Group", 4, 32, null, null, null) != 6)
			{
				return;
			}
			if (modlistGroupDefinition.Members.RemoveAll((string reference) => string.Equals(reference, member.MemberReference, 5)) == 0)
			{
				return;
			}
			IEnumerable<KeyValuePair<string, string>> conflictResolutions = modlistGroupDefinition.ConflictResolutions;
			Func<KeyValuePair<string, string>, bool> <>9__2;
			Func<KeyValuePair<string, string>, bool> func;
			if ((func = <>9__2) == null)
			{
				func = (<>9__2 = ((KeyValuePair<string, string> resolution) => string.Equals(resolution.Value, member.MemberReference, 5)));
			}
			foreach (string text in Enumerable.ToList<string>(Enumerable.Select<KeyValuePair<string, string>, string>(Enumerable.Where<KeyValuePair<string, string>>(conflictResolutions, func), (KeyValuePair<string, string> resolution) => resolution.Key)))
			{
				modlistGroupDefinition.ConflictResolutions.Remove(text);
			}
			try
			{
				ModlistGroupService.Save(localModlistListEntry.FilePath, modlistGroupDefinition);
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is InvalidOperationException)
			{
				ModManagerMessageBox.Show(this, "Failed to remove the modlist reference:\n" + ex.Message, "Remove from Modlist Group", 0, 16, null, null, null);
				return;
			}
			MainViewModel viewModel = this._viewModel;
			if (viewModel != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(19, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("Removed \"");
				defaultInterpolatedStringHandler2.AppendFormatted(member.DisplayName);
				defaultInterpolatedStringHandler2.AppendLiteral("\" from \"");
				defaultInterpolatedStringHandler2.AppendFormatted(localModlistListEntry.BaseDisplayName);
				defaultInterpolatedStringHandler2.AppendLiteral("\".");
				viewModel.ReportStatus(defaultInterpolatedStringHandler2.ToStringAndClear(), false);
			}
			this.RefreshLocalModlists(true, new string[]
			{
				localModlistListEntry.FilePath
			});
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00014BA8 File Offset: 0x00012DA8
		private void SelectReferencedModlist(ModlistGroupMemberListEntry member)
		{
			MainWindow.<>c__DisplayClass530_0 CS$<>8__locals1 = new MainWindow.<>c__DisplayClass530_0();
			CS$<>8__locals1.member = member;
			if (!CS$<>8__locals1.member.IsAvailable)
			{
				ModManagerMessageBox.Show(this, "The referenced modlist file could not be found.", "Select Referenced Modlist", 0, 48, null, null, null);
				return;
			}
			LocalModlistListEntry localModlistListEntry = Enumerable.FirstOrDefault<LocalModlistListEntry>(Enumerable.OfType<LocalModlistListEntry>(this.LocalModlistsDataGrid.Items), (LocalModlistListEntry entry) => !entry.IsGroup && string.Equals(entry.FilePath, CS$<>8__locals1.member.SourceFilePath, 5));
			if (localModlistListEntry == null)
			{
				ModManagerMessageBox.Show(this, "The referenced modlist is no longer available in the local list.", "Select Referenced Modlist", 0, 48, null, null, null);
				return;
			}
			CS$<>8__locals1.scrollViewer = MainWindow.FindDescendantScrollViewer(this.LocalModlistsDataGrid);
			MainWindow.<>c__DisplayClass530_0 CS$<>8__locals2 = CS$<>8__locals1;
			ScrollViewer scrollViewer = CS$<>8__locals1.scrollViewer;
			CS$<>8__locals2.previousHorizontalOffset = ((scrollViewer != null) ? scrollViewer.HorizontalOffset : 0.0);
			this.LocalModlistsDataGrid.UnselectAll();
			this.LocalModlistsDataGrid.SelectedItem = localModlistListEntry;
			this.LocalModlistsDataGrid.ScrollIntoView(localModlistListEntry);
			if (CS$<>8__locals1.scrollViewer != null)
			{
				this.LocalModlistsDataGrid.Dispatcher.BeginInvoke(delegate()
				{
					CS$<>8__locals1.scrollViewer.ScrollToHorizontalOffset(CS$<>8__locals1.previousHorizontalOffset);
				}, 6, Array.Empty<object>());
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00014CA4 File Offset: 0x00012EA4
		private void LocalModlistsDataGrid_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			Button button = MainWindow.FindAncestor<Button>(e.OriginalSource as DependencyObject);
			if (button != null && button.DataContext is ModlistGroupMemberListEntry)
			{
				this._localModlistDragSource = null;
				return;
			}
			this._localModlistDragStart = e.GetPosition(this.LocalModlistsDataGrid);
			DataGridRow dataGridRow = MainWindow.FindAncestor<DataGridRow>(e.OriginalSource as DependencyObject);
			this._localModlistDragSource = (((dataGridRow != null) ? dataGridRow.Item : null) as LocalModlistListEntry);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00014D14 File Offset: 0x00012F14
		private void LocalModlistsDataGrid_OnPreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton != 1 || this._localModlistDragSource == null || this._localModlistDragSource.IsGroup)
			{
				return;
			}
			Point position = e.GetPosition(this.LocalModlistsDataGrid);
			if (Math.Abs(position.X - this._localModlistDragStart.X) < SystemParameters.MinimumHorizontalDragDistance && Math.Abs(position.Y - this._localModlistDragStart.Y) < SystemParameters.MinimumVerticalDragDistance)
			{
				return;
			}
			LocalModlistListEntry localModlistDragSource = this._localModlistDragSource;
			this._localModlistDragSource = null;
			DataObject dataObject = new DataObject("SimpleVSManager.LocalModlist", localModlistDragSource);
			DragDrop.DoDragDrop(this.LocalModlistsDataGrid, dataObject, 4);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00014DB4 File Offset: 0x00012FB4
		private void LocalModlistsDataGrid_OnDragOver(object sender, DragEventArgs e)
		{
			e.Handled = true;
			LocalModlistListEntry localModlistListEntry;
			LocalModlistListEntry localModlistListEntry2;
			e.Effects = (MainWindow.CanDropLocalModlistReference(e, out localModlistListEntry, out localModlistListEntry2) ? 4 : 0);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00014DE0 File Offset: 0x00012FE0
		private void LocalModlistsDataGrid_OnDrop(object sender, DragEventArgs e)
		{
			e.Handled = true;
			LocalModlistListEntry localModlistListEntry;
			LocalModlistListEntry localModlistListEntry2;
			if (!MainWindow.CanDropLocalModlistReference(e, out localModlistListEntry, out localModlistListEntry2))
			{
				e.Effects = 0;
				return;
			}
			e.Effects = 4;
			if (localModlistListEntry2.IsGroup)
			{
				this.AddModlistReferenceToGroup(localModlistListEntry2, localModlistListEntry);
				return;
			}
			this.CreateModlistGroup(new LocalModlistListEntry[]
			{
				localModlistListEntry2,
				localModlistListEntry
			});
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00014E34 File Offset: 0x00013034
		private void LocalModlistsDataGrid_OnLoadingRow(object sender, DataGridRowEventArgs e)
		{
			DataGridRow row = e.Row;
			LocalModlistListEntry localModlistListEntry = e.Row.Item as LocalModlistListEntry;
			row.DetailsVisibility = ((localModlistListEntry != null && localModlistListEntry.IsGroup && localModlistListEntry.IsGroupExpanded) ? 0 : 2);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00014E74 File Offset: 0x00013074
		private void ModlistGroupExpanderButton_OnClick(object sender, RoutedEventArgs e)
		{
			ToggleButton toggleButton = sender as ToggleButton;
			if (toggleButton != null)
			{
				LocalModlistListEntry localModlistListEntry = toggleButton.DataContext as LocalModlistListEntry;
				if (localModlistListEntry != null && localModlistListEntry.IsGroup)
				{
					DataGridRow dataGridRow = MainWindow.FindAncestor<DataGridRow>(toggleButton);
					if (dataGridRow != null)
					{
						bool valueOrDefault = toggleButton.IsChecked.GetValueOrDefault();
						localModlistListEntry.IsGroupExpanded = valueOrDefault;
						dataGridRow.DetailsVisibility = (valueOrDefault ? 0 : 2);
						if (valueOrDefault)
						{
							this._collapsedModlistGroups.Remove(localModlistListEntry.FilePath);
							return;
						}
						this._collapsedModlistGroups.Add(localModlistListEntry.FilePath);
						return;
					}
				}
			}
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x00014EFC File Offset: 0x000130FC
		[NullableContext(2)]
		private static bool CanDropLocalModlistReference([Nullable(1)] DragEventArgs e, out LocalModlistListEntry source, out LocalModlistListEntry target)
		{
			source = (e.Data.GetData("SimpleVSManager.LocalModlist") as LocalModlistListEntry);
			DataGridRow dataGridRow = MainWindow.FindAncestor<DataGridRow>(e.OriginalSource as DependencyObject);
			target = (((dataGridRow != null) ? dataGridRow.Item : null) as LocalModlistListEntry);
			LocalModlistListEntry localModlistListEntry = source;
			return localModlistListEntry != null && !localModlistListEntry.IsGroup && target != null && source != target && !string.Equals(source.FilePath, target.FilePath, 5);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00014F74 File Offset: 0x00013174
		private void CreateModlistGroupButton_OnClick(object sender, RoutedEventArgs e)
		{
			this.CreateModlistGroup(Array.Empty<LocalModlistListEntry>());
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00014F84 File Offset: 0x00013184
		private void CreateModlistGroup(IReadOnlyCollection<LocalModlistListEntry> initialMembers)
		{
			LocalModlistEditDialog localModlistEditDialog = new LocalModlistEditDialog(this, null, null, null, null, "Create Modlist Group", "Create group");
			if (!localModlistEditDialog.ShowDialog().GetValueOrDefault())
			{
				return;
			}
			string uniqueFilePath;
			try
			{
				string directory = this.EnsureModListDirectory();
				string baseFileName = MainWindow.BuildSuggestedFileName(localModlistEditDialog.ModlistName, "Modlist Group");
				uniqueFilePath = MainWindow.GetUniqueFilePath(directory, baseFileName, ".svsmgroup");
				ModlistGroupDefinition modlistGroupDefinition = new ModlistGroupDefinition
				{
					Name = localModlistEditDialog.ModlistName,
					Description = localModlistEditDialog.ModlistDescription,
					Version = localModlistEditDialog.ModlistVersion,
					GameVersion = localModlistEditDialog.ModlistGameVersion
				};
				foreach (LocalModlistListEntry localModlistListEntry in Enumerable.DistinctBy<LocalModlistListEntry, string>(Enumerable.Where<LocalModlistListEntry>(initialMembers, (LocalModlistListEntry member) => !member.IsGroup), (LocalModlistListEntry member) => member.FilePath, StringComparer.OrdinalIgnoreCase))
				{
					modlistGroupDefinition.Members.Add(ModlistGroupService.CreateReference(uniqueFilePath, localModlistListEntry.FilePath));
				}
				ModlistGroupService.Save(uniqueFilePath, modlistGroupDefinition);
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is InvalidOperationException || ex is NotSupportedException)
			{
				ModManagerMessageBox.Show(this, "Failed to create the modlist group:\n" + ex.Message, "Create Modlist Group", 0, 16, null, null, null);
				return;
			}
			MainViewModel viewModel = this._viewModel;
			if (viewModel != null)
			{
				viewModel.ReportStatus("Created modlist group \"" + localModlistEditDialog.ModlistName + "\".", false);
			}
			this.RefreshLocalModlists(true, new string[]
			{
				uniqueFilePath
			});
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00015168 File Offset: 0x00013368
		private void AddModlistReferenceToGroup(LocalModlistListEntry group, LocalModlistListEntry member)
		{
			ModlistGroupDefinition groupDefinition = group.GroupDefinition;
			if (groupDefinition == null || member.IsGroup)
			{
				return;
			}
			try
			{
				string text = ModlistGroupService.CreateReference(group.FilePath, member.FilePath);
				if (Enumerable.Any<string>(groupDefinition.Members, delegate(string existingReference)
				{
					bool result;
					try
					{
						result = string.Equals(ModlistGroupService.ResolveReference(group.FilePath, existingReference), member.FilePath, 5);
					}
					catch (Exception)
					{
						result = false;
					}
					return result;
				}))
				{
					MainViewModel viewModel = this._viewModel;
					if (viewModel != null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 2);
						defaultInterpolatedStringHandler.AppendLiteral("\"");
						defaultInterpolatedStringHandler.AppendFormatted(member.DisplayName);
						defaultInterpolatedStringHandler.AppendLiteral("\" is already referenced by \"");
						defaultInterpolatedStringHandler.AppendFormatted(group.BaseDisplayName);
						defaultInterpolatedStringHandler.AppendLiteral("\".");
						viewModel.ReportStatus(defaultInterpolatedStringHandler.ToStringAndClear(), false);
					}
					return;
				}
				groupDefinition.Members.Add(text);
				ModlistGroupService.Save(group.FilePath, groupDefinition);
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is InvalidOperationException || ex is NotSupportedException)
			{
				ModManagerMessageBox.Show(this, "Failed to add the modlist to the group:\n" + ex.Message, "Modlist Group", 0, 16, null, null, null);
				return;
			}
			MainViewModel viewModel2 = this._viewModel;
			if (viewModel2 != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(30, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("Added a reference to \"");
				defaultInterpolatedStringHandler2.AppendFormatted(member.DisplayName);
				defaultInterpolatedStringHandler2.AppendLiteral("\" in \"");
				defaultInterpolatedStringHandler2.AppendFormatted(group.BaseDisplayName);
				defaultInterpolatedStringHandler2.AppendLiteral("\".");
				viewModel2.ReportStatus(defaultInterpolatedStringHandler2.ToStringAndClear(), false);
			}
			this.RefreshLocalModlists(true, new string[]
			{
				group.FilePath
			});
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0001536C File Offset: 0x0001356C
		private void SaveLocalModlistButton_OnClick(object sender, RoutedEventArgs e)
		{
			List<string> list = Enumerable.ToList<string>(Enumerable.Select<LocalModlistListEntry, string>(Enumerable.Where<LocalModlistListEntry>(this._selectedLocalModlists, (LocalModlistListEntry entry) => entry != null && !string.IsNullOrWhiteSpace(entry.FilePath)), (LocalModlistListEntry entry) => entry.FilePath));
			string text;
			if (this.TrySaveModlist(null, out text))
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					list.Add(text);
				}
				this.RefreshLocalModlists(true, list);
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x000153F0 File Offset: 0x000135F0
		private void InstallLocalModlistButton_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<InstallLocalModlistButton_OnClick>d__542 <InstallLocalModlistButton_OnClick>d__;
			<InstallLocalModlistButton_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<InstallLocalModlistButton_OnClick>d__.<>4__this = this;
			<InstallLocalModlistButton_OnClick>d__.<>1__state = -1;
			<InstallLocalModlistButton_OnClick>d__.<>t__builder.Start<MainWindow.<InstallLocalModlistButton_OnClick>d__542>(ref <InstallLocalModlistButton_OnClick>d__);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00015428 File Offset: 0x00013628
		private Task LoadModlistGroupAsync(LocalModlistListEntry entry)
		{
			MainWindow.<LoadModlistGroupAsync>d__543 <LoadModlistGroupAsync>d__;
			<LoadModlistGroupAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadModlistGroupAsync>d__.<>4__this = this;
			<LoadModlistGroupAsync>d__.entry = entry;
			<LoadModlistGroupAsync>d__.<>1__state = -1;
			<LoadModlistGroupAsync>d__.<>t__builder.Start<MainWindow.<LoadModlistGroupAsync>d__543>(ref <LoadModlistGroupAsync>d__);
			return <LoadModlistGroupAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00015474 File Offset: 0x00013674
		private void OpenModlistsFolderButton_OnClick(object sender, RoutedEventArgs e)
		{
			string path;
			try
			{
				path = this.EnsureModListDirectory();
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is SecurityException)
			{
				ModManagerMessageBox.Show("Failed to open the Modlists folder:\n" + ex.Message, "Simple VS Manager", 0, 16, null, null, null);
				return;
			}
			MainWindow.OpenFolder(path, "Modlists");
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x000154FC File Offset: 0x000136FC
		private void DeleteLocalModlistsButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (this._selectedLocalModlists.Count == 0)
			{
				return;
			}
			List<LocalModlistListEntry> list = Enumerable.ToList<LocalModlistListEntry>(Enumerable.Where<LocalModlistListEntry>(this._selectedLocalModlists, (LocalModlistListEntry entry) => entry != null && !string.IsNullOrWhiteSpace(entry.FilePath)));
			if (list.Count == 0)
			{
				return;
			}
			string messageBoxText;
			if (list.Count == 1)
			{
				string displayName = list[0].DisplayName;
				string text = list[0].IsGroup ? "modlist group" : "modlist";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(63, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Are you sure you want to delete the ");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral(" \"");
				defaultInterpolatedStringHandler.AppendFormatted(displayName);
				defaultInterpolatedStringHandler.AppendLiteral("\"? This cannot be undone.");
				messageBoxText = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(78, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("Are you sure you want to delete the ");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(list.Count);
				defaultInterpolatedStringHandler2.AppendLiteral(" selected modlists? This cannot be undone.");
				messageBoxText = defaultInterpolatedStringHandler2.ToStringAndClear();
			}
			if (ModManagerMessageBox.Show(this, messageBoxText, "Delete Modlists", 4, 32, null, null, null) != 6)
			{
				return;
			}
			List<string> list2 = new List<string>();
			int num = 0;
			foreach (LocalModlistListEntry localModlistListEntry in list)
			{
				try
				{
					if (File.Exists(localModlistListEntry.FilePath))
					{
						File.Delete(localModlistListEntry.FilePath);
						num++;
					}
				}
				catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
				{
					list2.Add(localModlistListEntry.DisplayName + ": " + ex.Message);
				}
			}
			if (list2.Count > 0)
			{
				string text2 = string.Join("\n", Enumerable.Select<string, string>(list2, (string err) => "• " + err));
				ModManagerMessageBox.Show(this, "Some modlists could not be deleted:\n" + text2, "Simple VS Manager", 0, 16, null, null, null);
			}
			else if (num > 0)
			{
				string text3;
				if (num != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3;
					defaultInterpolatedStringHandler3..ctor(24, 1);
					defaultInterpolatedStringHandler3.AppendLiteral("Deleted ");
					defaultInterpolatedStringHandler3.AppendFormatted<int>(num);
					defaultInterpolatedStringHandler3.AppendLiteral(" local modlists.");
					text3 = defaultInterpolatedStringHandler3.ToStringAndClear();
				}
				else
				{
					text3 = "Deleted local modlist.";
				}
				string message = text3;
				MainViewModel viewModel = this._viewModel;
				if (viewModel != null)
				{
					viewModel.ReportStatus(message, false);
				}
			}
			this.RefreshLocalModlists(true, Array.Empty<string>());
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0001579C File Offset: 0x0001399C
		private void ModifyLocalModlistButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (this._selectedLocalModlists.Count != 1)
			{
				return;
			}
			LocalModlistListEntry localModlistListEntry = this._selectedLocalModlists[0];
			if (localModlistListEntry == null || string.IsNullOrWhiteSpace(localModlistListEntry.FilePath))
			{
				return;
			}
			ModlistGroupDefinition groupDefinition = localModlistListEntry.GroupDefinition;
			if (groupDefinition != null)
			{
				this.ModifyModlistGroup(localModlistListEntry, groupDefinition);
				return;
			}
			LocalModlistEditDialog localModlistEditDialog = new LocalModlistEditDialog(this, localModlistListEntry.DisplayName, localModlistListEntry.Description, localModlistListEntry.Version, localModlistListEntry.GameVersion, null, null);
			if (!localModlistEditDialog.ShowDialog().GetValueOrDefault())
			{
				return;
			}
			string json;
			try
			{
				json = File.ReadAllText(localModlistListEntry.FilePath);
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
			{
				ModManagerMessageBox.Show(this, "Failed to read the modlist:\n" + ex.Message, "Modify Modlist", 0, 16, null, null, null);
				return;
			}
			SerializablePreset serializablePreset;
			string text;
			if (!PdfModlistSerializer.TryDeserializeFromJson(json, out serializablePreset, out text) || serializablePreset == null)
			{
				string messageBoxText = string.IsNullOrWhiteSpace(text) ? "The modlist could not be read." : text;
				ModManagerMessageBox.Show(this, messageBoxText, "Modify Modlist", 0, 16, null, null, null);
				return;
			}
			string modlistName = localModlistEditDialog.ModlistName;
			string text2 = (modlistName != null) ? modlistName.Trim() : null;
			string description = string.IsNullOrWhiteSpace(localModlistEditDialog.ModlistDescription) ? null : localModlistEditDialog.ModlistDescription.Trim();
			string version = string.IsNullOrWhiteSpace(localModlistEditDialog.ModlistVersion) ? null : localModlistEditDialog.ModlistVersion.Trim();
			string gameVersion = string.IsNullOrWhiteSpace(localModlistEditDialog.ModlistGameVersion) ? null : localModlistEditDialog.ModlistGameVersion.Trim();
			serializablePreset.Name = text2;
			serializablePreset.Description = description;
			serializablePreset.Version = version;
			serializablePreset.GameVersion = gameVersion;
			JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
			{
				WriteIndented = true,
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				DefaultIgnoreCondition = 3
			};
			try
			{
				string text3 = JsonSerializer.Serialize<SerializablePreset>(serializablePreset, jsonSerializerOptions);
				File.WriteAllText(localModlistListEntry.FilePath, text3);
			}
			catch (Exception ex2) when (ex2 is IOException || ex2 is UnauthorizedAccessException)
			{
				ModManagerMessageBox.Show(this, "Failed to update the modlist:\n" + ex2.Message, "Modify Modlist", 0, 16, null, null, null);
				return;
			}
			string text4 = string.IsNullOrWhiteSpace(text2) ? localModlistListEntry.DisplayName : text2;
			MainViewModel viewModel = this._viewModel;
			if (viewModel != null)
			{
				viewModel.ReportStatus("Updated modlist \"" + text4 + "\".", false);
			}
			this.RefreshLocalModlists(true, new string[]
			{
				localModlistListEntry.FilePath
			});
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00015A48 File Offset: 0x00013C48
		private void ModifyModlistGroup(LocalModlistListEntry entry, ModlistGroupDefinition definition)
		{
			LocalModlistEditDialog localModlistEditDialog = new LocalModlistEditDialog(this, definition.Name, definition.Description, definition.Version, definition.GameVersion, "Modify Modlist Group", null);
			if (!localModlistEditDialog.ShowDialog().GetValueOrDefault())
			{
				return;
			}
			definition.Name = localModlistEditDialog.ModlistName;
			definition.Description = localModlistEditDialog.ModlistDescription;
			definition.Version = localModlistEditDialog.ModlistVersion;
			definition.GameVersion = localModlistEditDialog.ModlistGameVersion;
			try
			{
				ModlistGroupService.Save(entry.FilePath, definition);
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is InvalidOperationException)
			{
				ModManagerMessageBox.Show(this, "Failed to update the modlist group:\n" + ex.Message, "Modify Modlist Group", 0, 16, null, null, null);
				return;
			}
			MainViewModel viewModel = this._viewModel;
			if (viewModel != null)
			{
				viewModel.ReportStatus("Updated modlist group \"" + localModlistEditDialog.ModlistName + "\".", false);
			}
			this.RefreshLocalModlists(true, new string[]
			{
				entry.FilePath
			});
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00015B6C File Offset: 0x00013D6C
		private void FixDuplicateModsButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (this._selectedLocalModlists.Count == 1)
			{
				LocalModlistListEntry localModlistListEntry = this._selectedLocalModlists[0];
				if (localModlistListEntry != null)
				{
					ModlistGroupDefinition groupDefinition = localModlistListEntry.GroupDefinition;
					if (groupDefinition != null)
					{
						if (localModlistListEntry.GroupConflicts.Count == 0)
						{
							ModManagerMessageBox.Show(this, "This modlist group has no duplicate mods.", "Modlist Group", 0, 64, null, null, null);
							return;
						}
						ModlistGroupConflictDialog modlistGroupConflictDialog = new ModlistGroupConflictDialog(this, localModlistListEntry.GroupConflicts);
						if (!modlistGroupConflictDialog.ShowDialog().GetValueOrDefault())
						{
							return;
						}
						foreach (KeyValuePair<string, string> keyValuePair in modlistGroupConflictDialog.Resolutions)
						{
							groupDefinition.ConflictResolutions[keyValuePair.Key] = keyValuePair.Value;
						}
						try
						{
							ModlistGroupService.Save(localModlistListEntry.FilePath, groupDefinition);
						}
						catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is InvalidOperationException)
						{
							ModManagerMessageBox.Show(this, "Failed to save the duplicate-mod choices:\n" + ex.Message, "Fix Duplicate Mods", 0, 16, null, null, null);
							return;
						}
						MainViewModel viewModel = this._viewModel;
						if (viewModel != null)
						{
							viewModel.ReportStatus("Resolved duplicate mods in \"" + localModlistListEntry.BaseDisplayName + "\".", false);
						}
						this.RefreshLocalModlists(true, new string[]
						{
							localModlistListEntry.FilePath
						});
						return;
					}
				}
			}
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00015CF4 File Offset: 0x00013EF4
		private void RefreshCloudModlistsButton_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<RefreshCloudModlistsButton_OnClick>d__549 <RefreshCloudModlistsButton_OnClick>d__;
			<RefreshCloudModlistsButton_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<RefreshCloudModlistsButton_OnClick>d__.<>4__this = this;
			<RefreshCloudModlistsButton_OnClick>d__.<>1__state = -1;
			<RefreshCloudModlistsButton_OnClick>d__.<>t__builder.Start<MainWindow.<RefreshCloudModlistsButton_OnClick>d__549>(ref <RefreshCloudModlistsButton_OnClick>d__);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00015D2C File Offset: 0x00013F2C
		private void CloudModlistsDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			DataGrid cloudModlistsDataGrid = this.CloudModlistsDataGrid;
			CloudModlistListEntry cloudModlistListEntry = ((cloudModlistsDataGrid != null) ? cloudModlistsDataGrid.SelectedItem : null) as CloudModlistListEntry;
			if (cloudModlistListEntry != null)
			{
				this.SetCloudModlistSelection(cloudModlistListEntry);
				return;
			}
			this.SetCloudModlistSelection(null);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00015D64 File Offset: 0x00013F64
		private void InstallCloudModlistButton_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<InstallCloudModlistButton_OnClick>d__551 <InstallCloudModlistButton_OnClick>d__;
			<InstallCloudModlistButton_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<InstallCloudModlistButton_OnClick>d__.<>4__this = this;
			<InstallCloudModlistButton_OnClick>d__.<>1__state = -1;
			<InstallCloudModlistButton_OnClick>d__.<>t__builder.Start<MainWindow.<InstallCloudModlistButton_OnClick>d__551>(ref <InstallCloudModlistButton_OnClick>d__);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00015D9C File Offset: 0x00013F9C
		private void AddCloudModlistToLocal_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<AddCloudModlistToLocal_OnClick>d__552 <AddCloudModlistToLocal_OnClick>d__;
			<AddCloudModlistToLocal_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<AddCloudModlistToLocal_OnClick>d__.<>4__this = this;
			<AddCloudModlistToLocal_OnClick>d__.<>1__state = -1;
			<AddCloudModlistToLocal_OnClick>d__.<>t__builder.Start<MainWindow.<AddCloudModlistToLocal_OnClick>d__552>(ref <AddCloudModlistToLocal_OnClick>d__);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00015DD4 File Offset: 0x00013FD4
		private void LoadModlistFromCloudMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<LoadModlistFromCloudMenuItem_OnClick>d__553 <LoadModlistFromCloudMenuItem_OnClick>d__;
			<LoadModlistFromCloudMenuItem_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<LoadModlistFromCloudMenuItem_OnClick>d__.<>4__this = this;
			<LoadModlistFromCloudMenuItem_OnClick>d__.<>1__state = -1;
			<LoadModlistFromCloudMenuItem_OnClick>d__.<>t__builder.Start<MainWindow.<LoadModlistFromCloudMenuItem_OnClick>d__553>(ref <LoadModlistFromCloudMenuItem_OnClick>d__);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00015E0C File Offset: 0x0001400C
		private void DeleteCloudModlistMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<DeleteCloudModlistMenuItem_OnClick>d__554 <DeleteCloudModlistMenuItem_OnClick>d__;
			<DeleteCloudModlistMenuItem_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<DeleteCloudModlistMenuItem_OnClick>d__.<>4__this = this;
			<DeleteCloudModlistMenuItem_OnClick>d__.<>1__state = -1;
			<DeleteCloudModlistMenuItem_OnClick>d__.<>t__builder.Start<MainWindow.<DeleteCloudModlistMenuItem_OnClick>d__554>(ref <DeleteCloudModlistMenuItem_OnClick>d__);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00015E44 File Offset: 0x00014044
		private void LoadPresetMenuItem_OnSubmenuOpened(object sender, RoutedEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;
			if (menuItem == null)
			{
				return;
			}
			for (int i = menuItem.Items.Count - 1; i >= 1; i--)
			{
				menuItem.Items.RemoveAt(i);
			}
			string text;
			try
			{
				text = this.EnsurePresetDirectory();
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
			{
				Trace.TraceWarning("Failed to access preset directory: {0}", new object[]
				{
					ex.Message
				});
				menuItem.Items.Add(new MenuItem
				{
					Header = "Presets unavailable",
					IsEnabled = false,
					Focusable = false,
					Opacity = 0.75
				});
				return;
			}
			string[] files;
			try
			{
				files = Directory.GetFiles(text, "*.json");
			}
			catch (Exception ex2) when (ex2 is IOException || ex2 is UnauthorizedAccessException)
			{
				Trace.TraceWarning("Failed to enumerate presets: {0}", new object[]
				{
					ex2.Message
				});
				menuItem.Items.Add(new MenuItem
				{
					Header = "Presets unavailable",
					IsEnabled = false,
					Focusable = false,
					Opacity = 0.75
				});
				return;
			}
			if (files.Length == 0)
			{
				menuItem.Items.Add(new MenuItem
				{
					Header = "No presets available",
					IsEnabled = false,
					Focusable = false,
					Opacity = 0.75
				});
				return;
			}
			Array.Sort<string>(files, StringComparer.OrdinalIgnoreCase);
			menuItem.Items.Add(new Separator());
			foreach (string text2 in files)
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text2);
				MenuItem menuItem2 = new MenuItem
				{
					Header = fileNameWithoutExtension,
					Tag = text2
				};
				menuItem2.Click += new RoutedEventHandler(this.LoadPresetMenuItem_OnPresetClick);
				menuItem.Items.Add(menuItem2);
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00016084 File Offset: 0x00014284
		private void LoadPresetMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<LoadPresetMenuItem_OnClick>d__556 <LoadPresetMenuItem_OnClick>d__;
			<LoadPresetMenuItem_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<LoadPresetMenuItem_OnClick>d__.<>4__this = this;
			<LoadPresetMenuItem_OnClick>d__.<>1__state = -1;
			<LoadPresetMenuItem_OnClick>d__.<>t__builder.Start<MainWindow.<LoadPresetMenuItem_OnClick>d__556>(ref <LoadPresetMenuItem_OnClick>d__);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x000160BC File Offset: 0x000142BC
		private void LoadPresetMenuItem_OnPresetClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<LoadPresetMenuItem_OnPresetClick>d__557 <LoadPresetMenuItem_OnPresetClick>d__;
			<LoadPresetMenuItem_OnPresetClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<LoadPresetMenuItem_OnPresetClick>d__.<>4__this = this;
			<LoadPresetMenuItem_OnPresetClick>d__.sender = sender;
			<LoadPresetMenuItem_OnPresetClick>d__.<>1__state = -1;
			<LoadPresetMenuItem_OnPresetClick>d__.<>t__builder.Start<MainWindow.<LoadPresetMenuItem_OnPresetClick>d__557>(ref <LoadPresetMenuItem_OnPresetClick>d__);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x000160FC File Offset: 0x000142FC
		private Task LoadPresetFromFileAsync(string filePath)
		{
			MainWindow.<LoadPresetFromFileAsync>d__558 <LoadPresetFromFileAsync>d__;
			<LoadPresetFromFileAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadPresetFromFileAsync>d__.<>4__this = this;
			<LoadPresetFromFileAsync>d__.filePath = filePath;
			<LoadPresetFromFileAsync>d__.<>1__state = -1;
			<LoadPresetFromFileAsync>d__.<>t__builder.Start<MainWindow.<LoadPresetFromFileAsync>d__558>(ref <LoadPresetFromFileAsync>d__);
			return <LoadPresetFromFileAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00016148 File Offset: 0x00014348
		private void LoadModlistMenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			MainWindow.<LoadModlistMenuItem_OnClick>d__559 <LoadModlistMenuItem_OnClick>d__;
			<LoadModlistMenuItem_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<LoadModlistMenuItem_OnClick>d__.<>4__this = this;
			<LoadModlistMenuItem_OnClick>d__.<>1__state = -1;
			<LoadModlistMenuItem_OnClick>d__.<>t__builder.Start<MainWindow.<LoadModlistMenuItem_OnClick>d__559>(ref <LoadModlistMenuItem_OnClick>d__);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x00016180 File Offset: 0x00014380
		private Task LoadModlistFromFileAsync(string filePath)
		{
			MainWindow.<LoadModlistFromFileAsync>d__560 <LoadModlistFromFileAsync>d__;
			<LoadModlistFromFileAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadModlistFromFileAsync>d__.<>4__this = this;
			<LoadModlistFromFileAsync>d__.filePath = filePath;
			<LoadModlistFromFileAsync>d__.<>1__state = -1;
			<LoadModlistFromFileAsync>d__.<>t__builder.Start<MainWindow.<LoadModlistFromFileAsync>d__560>(ref <LoadModlistFromFileAsync>d__);
			return <LoadModlistFromFileAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x000161CB File Offset: 0x000143CB
		private void MainWindow_OnPreviewDragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent("SimpleVSManager.LocalModlist"))
			{
				return;
			}
			this.HandleModlistDragEvent(e);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x000161E7 File Offset: 0x000143E7
		private void MainWindow_OnPreviewDragOver(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent("SimpleVSManager.LocalModlist"))
			{
				return;
			}
			this.HandleModlistDragEvent(e);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00016204 File Offset: 0x00014404
		private void MainWindow_OnPreviewDrop(object sender, DragEventArgs e)
		{
			MainWindow.<MainWindow_OnPreviewDrop>d__563 <MainWindow_OnPreviewDrop>d__;
			<MainWindow_OnPreviewDrop>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<MainWindow_OnPreviewDrop>d__.<>4__this = this;
			<MainWindow_OnPreviewDrop>d__.e = e;
			<MainWindow_OnPreviewDrop>d__.<>1__state = -1;
			<MainWindow_OnPreviewDrop>d__.<>t__builder.Start<MainWindow.<MainWindow_OnPreviewDrop>d__563>(ref <MainWindow_OnPreviewDrop>d__);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00016244 File Offset: 0x00014444
		private void HandleModlistDragEvent(DragEventArgs e)
		{
			e.Handled = true;
			string text;
			if (MainWindow.TryGetDroppedModlistFile(e, out text))
			{
				e.Effects = 1;
				return;
			}
			e.Effects = 0;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00016274 File Offset: 0x00014474
		private static bool TryGetDroppedModlistFile(DragEventArgs e, [Nullable(2)] out string filePath)
		{
			filePath = null;
			if (!e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				return false;
			}
			string[] array = e.Data.GetData(DataFormats.FileDrop) as string[];
			if (array == null || array.Length == 0)
			{
				return false;
			}
			foreach (string text in array)
			{
				if (MainWindow.HasSupportedModlistExtension(text))
				{
					filePath = text;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x000162DC File Offset: 0x000144DC
		private static bool HasSupportedModlistExtension(string filePath)
		{
			string extension = Path.GetExtension(filePath);
			return string.Equals(extension, ".json", 5) || string.Equals(extension, ".pdf", 5);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0001630C File Offset: 0x0001450C
		private bool TryLoadPresetFromFile(string filePath, string fallbackName, MainWindow.PresetLoadOptions options, [Nullable(2)] out ModPreset preset, [Nullable(2)] out string errorMessage)
		{
			preset = null;
			errorMessage = null;
			if (!File.Exists(filePath))
			{
				errorMessage = "The selected file could not be found.";
				return false;
			}
			if (string.Equals(Path.GetExtension(filePath), ".pdf", 5))
			{
				return this.TryLoadPresetFromPdf(filePath, fallbackName, options, out preset, out errorMessage);
			}
			bool result;
			try
			{
				string json;
				using (FileStream fileStream = File.OpenRead(filePath))
				{
					using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
					{
						json = streamReader.ReadToEnd();
					}
				}
				SerializablePreset data;
				if (!PdfModlistSerializer.TryDeserializeFromJson(json, out data, out errorMessage))
				{
					result = false;
				}
				else
				{
					string snapshotNameFromFilePath = MainWindow.GetSnapshotNameFromFilePath(filePath, fallbackName);
					result = this.TryBuildPresetFromSerializable(data, fallbackName, options, out preset, out errorMessage, snapshotNameFromFilePath);
				}
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is JsonException)
			{
				errorMessage = ex.Message;
				result = false;
			}
			return result;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0001642C File Offset: 0x0001462C
		private bool TryLoadPresetFromPdf(string filePath, string fallbackName, MainWindow.PresetLoadOptions options, [Nullable(2)] out ModPreset preset, [Nullable(2)] out string errorMessage)
		{
			preset = null;
			errorMessage = null;
			bool result;
			try
			{
				using (PdfDocument pdfDocument = PdfDocument.Open(filePath, null))
				{
					string json = null;
					string configJson = null;
					DocumentInformation information = pdfDocument.Information;
					string text;
					bool flag = PdfModlistSerializer.TryExtractModlistJsonFromMetadata((information != null) ? information.Subject : null, out json, out text);
					if (!flag && text != null)
					{
						errorMessage = text;
						result = false;
					}
					else
					{
						string text2;
						bool flag2 = PdfModlistSerializer.TryExtractConfigJsonFromMetadata((information != null) ? information.Keywords : null, out configJson, out text2);
						if (!flag2 && text2 != null)
						{
							errorMessage = text2;
							result = false;
						}
						else
						{
							string text3 = null;
							if (!flag || !flag2)
							{
								StringBuilder stringBuilder = new StringBuilder();
								foreach (Page page in pdfDocument.GetPages())
								{
									StringBuilder stringBuilder2 = new StringBuilder();
									foreach (Letter letter in page.Letters)
									{
										string value = letter.Value;
										if (!string.IsNullOrEmpty(value) && !(value == "\r"))
										{
											stringBuilder2.Append(value);
										}
									}
									string text4 = stringBuilder2.ToString();
									if (!string.IsNullOrWhiteSpace(text4))
									{
										if (stringBuilder.Length > 0)
										{
											stringBuilder.Append('\n');
										}
										stringBuilder.Append(text4);
									}
								}
								text3 = stringBuilder.ToString();
								if (string.IsNullOrWhiteSpace(text3))
								{
									errorMessage = "The PDF did not contain any readable text.";
									return false;
								}
							}
							string text5;
							string text6;
							if (!flag && !PdfModlistSerializer.TryExtractModlistJson(text3, out json, out text5))
							{
								errorMessage = text5;
								result = false;
							}
							else if (!flag2 && !PdfModlistSerializer.TryExtractConfigJson(text3, out configJson, out text6))
							{
								errorMessage = text6;
								result = false;
							}
							else
							{
								string snapshotNameFromFilePath = MainWindow.GetSnapshotNameFromFilePath(filePath, fallbackName);
								result = this.TryLoadPresetFromJson(json, fallbackName, options, out preset, out errorMessage, snapshotNameFromFilePath, configJson);
							}
						}
					}
				}
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is InvalidOperationException)
			{
				errorMessage = ex.Message;
				result = false;
			}
			catch (Exception ex2)
			{
				errorMessage = ex2.Message;
				result = false;
			}
			return result;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x000166DC File Offset: 0x000148DC
		[NullableContext(2)]
		private bool TryLoadPresetFromJson([Nullable(1)] string json, [Nullable(1)] string fallbackName, MainWindow.PresetLoadOptions options, out ModPreset preset, out string errorMessage, string sourceName = null, string configJson = null)
		{
			preset = null;
			errorMessage = null;
			if (string.IsNullOrWhiteSpace(json))
			{
				errorMessage = "The selected file was empty.";
				return false;
			}
			SerializablePreset serializablePreset;
			if (!PdfModlistSerializer.TryDeserializeFromJson(json, out serializablePreset, out errorMessage))
			{
				return false;
			}
			SerializablePreset serializablePreset2 = serializablePreset;
			if (!string.IsNullOrWhiteSpace(configJson))
			{
				SerializableConfigList configList;
				string text;
				if (!PdfModlistSerializer.TryDeserializeConfigListFromJson(configJson, out configList, out text))
				{
					errorMessage = (string.IsNullOrWhiteSpace(text) ? "The PDF configuration data could not be read." : text);
					return false;
				}
				MainWindow.ApplyConfigListToPreset(serializablePreset2, configList);
			}
			return this.TryBuildPresetFromSerializable(serializablePreset2, fallbackName, options, out preset, out errorMessage, sourceName);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00016754 File Offset: 0x00014954
		[NullableContext(2)]
		private bool TryBuildPresetFromSerializable([Nullable(1)] SerializablePreset data, [Nullable(1)] string fallbackName, MainWindow.PresetLoadOptions options, out ModPreset preset, out string errorMessage, string fallbackNameFromSource = null)
		{
			preset = null;
			errorMessage = null;
			string name = (!string.IsNullOrWhiteSpace(data.Name)) ? data.Name.Trim() : ((!string.IsNullOrWhiteSpace(fallbackNameFromSource)) ? fallbackNameFromSource.Trim() : fallbackName);
			List<string> list = new List<string>();
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			if (data.DisabledEntries != null)
			{
				foreach (string text in data.DisabledEntries)
				{
					if (!string.IsNullOrWhiteSpace(text))
					{
						string text2 = text.Trim();
						if (hashSet.Add(text2))
						{
							list.Add(text2);
						}
					}
				}
			}
			bool? flag = data.IncludeModStatus;
			bool flag2;
			if (flag == null)
			{
				List<SerializablePresetModState> mods = data.Mods;
				if (mods == null)
				{
					flag2 = false;
				}
				else
				{
					flag2 = Enumerable.Any<SerializablePresetModState>(mods, delegate(SerializablePresetModState entry)
					{
						bool? flag7 = (entry != null) ? entry.IsActive : default(bool?);
						return flag7 != null;
					});
				}
			}
			else
			{
				flag2 = flag.GetValueOrDefault();
			}
			bool flag3 = flag2;
			flag = data.IncludeModVersions;
			bool flag4;
			if (flag == null)
			{
				List<SerializablePresetModState> mods2 = data.Mods;
				if (mods2 == null)
				{
					flag4 = false;
				}
				else
				{
					flag4 = Enumerable.Any<SerializablePresetModState>(mods2, (SerializablePresetModState entry) => !string.IsNullOrWhiteSpace((entry != null) ? entry.Version : null));
				}
			}
			else
			{
				flag4 = flag.GetValueOrDefault();
			}
			bool flag5 = flag4;
			bool includesModStatus = options.ApplyModStatus && flag3;
			bool includesModVersions = options.ApplyModVersions && flag5;
			bool forceExclusive = options.ForceExclusive;
			List<ModPresetModState> list2 = new List<ModPresetModState>();
			Dictionary<string, List<ModConfigurationSnapshot>> dictionary = new Dictionary<string, List<ModConfigurationSnapshot>>(StringComparer.OrdinalIgnoreCase);
			if (data.Configurations != null)
			{
				foreach (SerializableModConfiguration serializableModConfiguration in data.Configurations)
				{
					if (serializableModConfiguration != null && !string.IsNullOrWhiteSpace(serializableModConfiguration.ModId))
					{
						string text3 = serializableModConfiguration.ModId.Trim();
						if (!string.IsNullOrWhiteSpace(text3))
						{
							List<ModConfigurationSnapshot> list3;
							if (!dictionary.TryGetValue(text3, ref list3))
							{
								list3 = new List<ModConfigurationSnapshot>();
								dictionary[text3] = list3;
							}
							string text4 = string.IsNullOrWhiteSpace(serializableModConfiguration.FileName) ? null : serializableModConfiguration.FileName.Trim();
							string relativePath = string.IsNullOrWhiteSpace(serializableModConfiguration.RelativePath) ? null : serializableModConfiguration.RelativePath.Trim();
							string content = serializableModConfiguration.Content ?? string.Empty;
							list3.Add(new ModConfigurationSnapshot(text4 ?? string.Empty, content, relativePath));
						}
					}
				}
			}
			if (data.Mods != null)
			{
				foreach (SerializablePresetModState serializablePresetModState in data.Mods)
				{
					if (serializablePresetModState != null && !string.IsNullOrWhiteSpace(serializablePresetModState.ModId))
					{
						MainWindow.<>c__DisplayClass570_0 CS$<>8__locals1;
						CS$<>8__locals1.modId = serializablePresetModState.ModId.Trim();
						string version = string.IsNullOrWhiteSpace(serializablePresetModState.Version) ? null : serializablePresetModState.Version.Trim();
						string text5 = string.IsNullOrWhiteSpace(serializablePresetModState.ConfigurationFileName) ? null : serializablePresetModState.ConfigurationFileName.Trim();
						string text6 = ModConfigurationEncoding.Decode(serializablePresetModState.ConfigurationContent);
						if (string.IsNullOrWhiteSpace(text6))
						{
							text6 = null;
						}
						CS$<>8__locals1.mergedConfigurations = new List<ModConfigurationSnapshot>();
						CS$<>8__locals1.seenConfigs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
						List<ModConfigurationSnapshot> list4;
						bool flag6 = dictionary.TryGetValue(CS$<>8__locals1.modId, ref list4);
						if (!flag6)
						{
							MainWindow.<TryBuildPresetFromSerializable>g__AddConfiguration|570_2(text5, text6, null, ref CS$<>8__locals1);
						}
						if (flag6 && list4 != null)
						{
							foreach (ModConfigurationSnapshot modConfigurationSnapshot in list4)
							{
								MainWindow.<TryBuildPresetFromSerializable>g__AddConfiguration|570_2(modConfigurationSnapshot.FileName, modConfigurationSnapshot.Content, modConfigurationSnapshot.RelativePath, ref CS$<>8__locals1);
							}
						}
						if (CS$<>8__locals1.mergedConfigurations.Count > 0)
						{
							if (text5 == null)
							{
								text5 = CS$<>8__locals1.mergedConfigurations[0].FileName;
							}
							if (text6 == null)
							{
								text6 = CS$<>8__locals1.mergedConfigurations[0].Content;
							}
						}
						list2.Add(new ModPresetModState(CS$<>8__locals1.modId, version, serializablePresetModState.IsActive, text5, text6, (CS$<>8__locals1.mergedConfigurations.Count > 0) ? CS$<>8__locals1.mergedConfigurations : null));
					}
				}
			}
			preset = new ModPreset(name, list, list2, includesModStatus, includesModVersions, forceExclusive);
			return true;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00016C10 File Offset: 0x00014E10
		private static void ApplyConfigListToPreset(SerializablePreset preset, [Nullable(2)] SerializableConfigList configList)
		{
			if (preset == null || ((configList != null) ? configList.Configurations : null) == null || configList.Configurations.Count == 0)
			{
				return;
			}
			if (preset.Configurations == null)
			{
				preset.Configurations = new List<SerializableModConfiguration>();
			}
			IEnumerable<SerializableModConfiguration> configurations = preset.Configurations;
			Func<SerializableModConfiguration, string> func;
			if ((func = MainWindow.<>O.<5>__BuildConfigKey) == null)
			{
				func = (MainWindow.<>O.<5>__BuildConfigKey = new Func<SerializableModConfiguration, string>(MainWindow.BuildConfigKey));
			}
			HashSet<string> hashSet = new HashSet<string>(Enumerable.Select<SerializableModConfiguration, string>(configurations, func), StringComparer.OrdinalIgnoreCase);
			foreach (SerializableModConfiguration serializableModConfiguration in configList.Configurations)
			{
				if (serializableModConfiguration != null && !string.IsNullOrWhiteSpace(serializableModConfiguration.ModId) && serializableModConfiguration.Content != null)
				{
					string text = MainWindow.BuildConfigKey(serializableModConfiguration);
					if (hashSet.Add(text))
					{
						preset.Configurations.Add(serializableModConfiguration);
					}
				}
			}
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00016D00 File Offset: 0x00014F00
		private static string BuildConfigKey(SerializableModConfiguration configuration)
		{
			string text = string.IsNullOrWhiteSpace(configuration.ModId) ? string.Empty : configuration.ModId.Trim();
			string text2 = string.IsNullOrWhiteSpace(configuration.FileName) ? string.Empty : configuration.FileName.Trim();
			string text3 = string.IsNullOrWhiteSpace(configuration.RelativePath) ? string.Empty : configuration.RelativePath.Trim();
			string text4 = configuration.Content ?? string.Empty;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(6, 4);
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral("::");
			defaultInterpolatedStringHandler.AppendFormatted(text3);
			defaultInterpolatedStringHandler.AppendLiteral("::");
			defaultInterpolatedStringHandler.AppendFormatted(text2);
			defaultInterpolatedStringHandler.AppendLiteral("::");
			defaultInterpolatedStringHandler.AppendFormatted(text4);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00016DD4 File Offset: 0x00014FD4
		private Task ExecuteCloudOperationAsync(Func<FirebaseModlistStore, Task> operation, string actionDescription)
		{
			MainWindow.<ExecuteCloudOperationAsync>d__573 <ExecuteCloudOperationAsync>d__;
			<ExecuteCloudOperationAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ExecuteCloudOperationAsync>d__.<>4__this = this;
			<ExecuteCloudOperationAsync>d__.operation = operation;
			<ExecuteCloudOperationAsync>d__.actionDescription = actionDescription;
			<ExecuteCloudOperationAsync>d__.<>1__state = -1;
			<ExecuteCloudOperationAsync>d__.<>t__builder.Start<MainWindow.<ExecuteCloudOperationAsync>d__573>(ref <ExecuteCloudOperationAsync>d__);
			return <ExecuteCloudOperationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00016E28 File Offset: 0x00015028
		private Task MigrateLegacyFirebaseDataIfNeededAsync()
		{
			MainWindow.<MigrateLegacyFirebaseDataIfNeededAsync>d__574 <MigrateLegacyFirebaseDataIfNeededAsync>d__;
			<MigrateLegacyFirebaseDataIfNeededAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<MigrateLegacyFirebaseDataIfNeededAsync>d__.<>4__this = this;
			<MigrateLegacyFirebaseDataIfNeededAsync>d__.<>1__state = -1;
			<MigrateLegacyFirebaseDataIfNeededAsync>d__.<>t__builder.Start<MainWindow.<MigrateLegacyFirebaseDataIfNeededAsync>d__574>(ref <MigrateLegacyFirebaseDataIfNeededAsync>d__);
			return <MigrateLegacyFirebaseDataIfNeededAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00016E6C File Offset: 0x0001506C
		private Task<FirebaseModlistStore> EnsureCloudStoreInitializedAsync()
		{
			MainWindow.<EnsureCloudStoreInitializedAsync>d__575 <EnsureCloudStoreInitializedAsync>d__;
			<EnsureCloudStoreInitializedAsync>d__.<>t__builder = AsyncTaskMethodBuilder<FirebaseModlistStore>.Create();
			<EnsureCloudStoreInitializedAsync>d__.<>4__this = this;
			<EnsureCloudStoreInitializedAsync>d__.<>1__state = -1;
			<EnsureCloudStoreInitializedAsync>d__.<>t__builder.Start<MainWindow.<EnsureCloudStoreInitializedAsync>d__575>(ref <EnsureCloudStoreInitializedAsync>d__);
			return <EnsureCloudStoreInitializedAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00016EB0 File Offset: 0x000150B0
		[return: Nullable(2)]
		private CloudModlistSlot PromptForCloudSaveReplacement(IReadOnlyList<CloudModlistSlot> slots)
		{
			if (slots == null)
			{
				return null;
			}
			List<CloudModlistSlot> list = Enumerable.ToList<CloudModlistSlot>(Enumerable.Where<CloudModlistSlot>(slots, (CloudModlistSlot slot) => slot.IsOccupied));
			if (list.Count == 0)
			{
				return null;
			}
			CloudSlotSelectionDialog cloudSlotSelectionDialog = new CloudSlotSelectionDialog(this, list, "Replace Cloud Modlist", "Select a cloud modlist to replace.", null);
			if (!cloudSlotSelectionDialog.ShowDialog().GetValueOrDefault())
			{
				return null;
			}
			return cloudSlotSelectionDialog.SelectedSlot;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00016F24 File Offset: 0x00015124
		private Task ShowCloudModlistManagementDialogAsync(FirebaseModlistStore store)
		{
			MainWindow.<ShowCloudModlistManagementDialogAsync>d__577 <ShowCloudModlistManagementDialogAsync>d__;
			<ShowCloudModlistManagementDialogAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ShowCloudModlistManagementDialogAsync>d__.<>4__this = this;
			<ShowCloudModlistManagementDialogAsync>d__.store = store;
			<ShowCloudModlistManagementDialogAsync>d__.<>1__state = -1;
			<ShowCloudModlistManagementDialogAsync>d__.<>t__builder.Start<MainWindow.<ShowCloudModlistManagementDialogAsync>d__577>(ref <ShowCloudModlistManagementDialogAsync>d__);
			return <ShowCloudModlistManagementDialogAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00016F70 File Offset: 0x00015170
		private Task<IReadOnlyList<CloudModlistManagementEntry>> BuildCloudModlistManagementEntriesAsync(FirebaseModlistStore store)
		{
			MainWindow.<BuildCloudModlistManagementEntriesAsync>d__578 <BuildCloudModlistManagementEntriesAsync>d__;
			<BuildCloudModlistManagementEntriesAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IReadOnlyList<CloudModlistManagementEntry>>.Create();
			<BuildCloudModlistManagementEntriesAsync>d__.<>4__this = this;
			<BuildCloudModlistManagementEntriesAsync>d__.store = store;
			<BuildCloudModlistManagementEntriesAsync>d__.<>1__state = -1;
			<BuildCloudModlistManagementEntriesAsync>d__.<>t__builder.Start<MainWindow.<BuildCloudModlistManagementEntriesAsync>d__578>(ref <BuildCloudModlistManagementEntriesAsync>d__);
			return <BuildCloudModlistManagementEntriesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00016FBC File Offset: 0x000151BC
		private Task<bool> RenameCloudModlistAsync(FirebaseModlistStore store, CloudModlistManagementEntry entry, string newName)
		{
			MainWindow.<RenameCloudModlistAsync>d__579 <RenameCloudModlistAsync>d__;
			<RenameCloudModlistAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<RenameCloudModlistAsync>d__.<>4__this = this;
			<RenameCloudModlistAsync>d__.store = store;
			<RenameCloudModlistAsync>d__.entry = entry;
			<RenameCloudModlistAsync>d__.newName = newName;
			<RenameCloudModlistAsync>d__.<>1__state = -1;
			<RenameCloudModlistAsync>d__.<>t__builder.Start<MainWindow.<RenameCloudModlistAsync>d__579>(ref <RenameCloudModlistAsync>d__);
			return <RenameCloudModlistAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00017018 File Offset: 0x00015218
		private Task<bool> DeleteCloudModlistAsync(FirebaseModlistStore store, CloudModlistManagementEntry entry)
		{
			MainWindow.<DeleteCloudModlistAsync>d__580 <DeleteCloudModlistAsync>d__;
			<DeleteCloudModlistAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<DeleteCloudModlistAsync>d__.<>4__this = this;
			<DeleteCloudModlistAsync>d__.store = store;
			<DeleteCloudModlistAsync>d__.entry = entry;
			<DeleteCloudModlistAsync>d__.<>1__state = -1;
			<DeleteCloudModlistAsync>d__.<>t__builder.Start<MainWindow.<DeleteCloudModlistAsync>d__580>(ref <DeleteCloudModlistAsync>d__);
			return <DeleteCloudModlistAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0001706C File Offset: 0x0001526C
		private Task DeleteAllCloudModlistsAndAuthorizationAsync(FirebaseModlistStore store, bool showCompletionMessage = true)
		{
			MainWindow.<DeleteAllCloudModlistsAndAuthorizationAsync>d__581 <DeleteAllCloudModlistsAndAuthorizationAsync>d__;
			<DeleteAllCloudModlistsAndAuthorizationAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DeleteAllCloudModlistsAndAuthorizationAsync>d__.<>4__this = this;
			<DeleteAllCloudModlistsAndAuthorizationAsync>d__.store = store;
			<DeleteAllCloudModlistsAndAuthorizationAsync>d__.showCompletionMessage = showCompletionMessage;
			<DeleteAllCloudModlistsAndAuthorizationAsync>d__.<>1__state = -1;
			<DeleteAllCloudModlistsAndAuthorizationAsync>d__.<>t__builder.Start<MainWindow.<DeleteAllCloudModlistsAndAuthorizationAsync>d__581>(ref <DeleteAllCloudModlistsAndAuthorizationAsync>d__);
			return <DeleteAllCloudModlistsAndAuthorizationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x000170C0 File Offset: 0x000152C0
		private Task UpdateCloudModlistsAfterChangeAsync()
		{
			MainWindow.<UpdateCloudModlistsAfterChangeAsync>d__582 <UpdateCloudModlistsAfterChangeAsync>d__;
			<UpdateCloudModlistsAfterChangeAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<UpdateCloudModlistsAfterChangeAsync>d__.<>4__this = this;
			<UpdateCloudModlistsAfterChangeAsync>d__.<>1__state = -1;
			<UpdateCloudModlistsAfterChangeAsync>d__.<>t__builder.Start<MainWindow.<UpdateCloudModlistsAfterChangeAsync>d__582>(ref <UpdateCloudModlistsAfterChangeAsync>d__);
			return <UpdateCloudModlistsAfterChangeAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00017104 File Offset: 0x00015304
		private static string ReplaceCloudModlistName(string json, string newName)
		{
			string @string;
			using (JsonDocument jsonDocument = JsonDocument.Parse(json, default(JsonDocumentOptions)))
			{
				if (jsonDocument.RootElement.ValueKind != 1)
				{
					throw new InvalidOperationException("The cloud modlist content is not a valid object.");
				}
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (Utf8JsonWriter utf8JsonWriter = new Utf8JsonWriter(memoryStream, default(JsonWriterOptions)))
					{
						utf8JsonWriter.WriteStartObject();
						bool flag = false;
						foreach (JsonProperty jsonProperty in jsonDocument.RootElement.EnumerateObject())
						{
							if (jsonProperty.NameEquals("name"))
							{
								utf8JsonWriter.WriteString("name", newName);
								flag = true;
							}
							else
							{
								jsonProperty.WriteTo(utf8JsonWriter);
							}
						}
						if (!flag)
						{
							utf8JsonWriter.WriteString("name", newName);
						}
						utf8JsonWriter.WriteEndObject();
					}
					@string = Encoding.UTF8.GetString(memoryStream.ToArray());
				}
			}
			return @string;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0001724C File Offset: 0x0001544C
		private Task<List<CloudModlistSlot>> GetCloudModlistSlotsAsync(FirebaseModlistStore store, bool includeEmptySlots, bool captureContent)
		{
			MainWindow.<GetCloudModlistSlotsAsync>d__584 <GetCloudModlistSlotsAsync>d__;
			<GetCloudModlistSlotsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<List<CloudModlistSlot>>.Create();
			<GetCloudModlistSlotsAsync>d__.store = store;
			<GetCloudModlistSlotsAsync>d__.includeEmptySlots = includeEmptySlots;
			<GetCloudModlistSlotsAsync>d__.captureContent = captureContent;
			<GetCloudModlistSlotsAsync>d__.<>1__state = -1;
			<GetCloudModlistSlotsAsync>d__.<>t__builder.Start<MainWindow.<GetCloudModlistSlotsAsync>d__584>(ref <GetCloudModlistSlotsAsync>d__);
			return <GetCloudModlistSlotsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x000172A0 File Offset: 0x000154A0
		private void RefreshLocalModlists(bool force, [Nullable(new byte[]
		{
			2,
			1
		})] IReadOnlyCollection<string> preferredSelection = null)
		{
			if (!force && this._localModlistsLoaded)
			{
				return;
			}
			List<string> list;
			IReadOnlyList<LocalModlistListEntry> readOnlyList;
			try
			{
				readOnlyList = this.BuildLocalModlistEntries(out list);
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
			{
				ModManagerMessageBox.Show("Failed to read local modlists:\n" + ex.Message, "Simple VS Manager", 0, 16, null, null, null);
				readOnlyList = Array.Empty<LocalModlistListEntry>();
				list = new List<string>();
			}
			foreach (LocalModlistListEntry localModlistListEntry in Enumerable.Where<LocalModlistListEntry>(readOnlyList, (LocalModlistListEntry entry) => entry.IsGroup))
			{
				localModlistListEntry.IsGroupExpanded = !this._collapsedModlistGroups.Contains(localModlistListEntry.FilePath);
			}
			MainViewModel viewModel = this._viewModel;
			if (viewModel != null)
			{
				viewModel.ReplaceLocalModlists(readOnlyList);
			}
			this._localModlistsLoaded = true;
			HashSet<string> hashSet;
			if (preferredSelection == null)
			{
				hashSet = new HashSet<string>(Enumerable.Select<LocalModlistListEntry, string>(Enumerable.Where<LocalModlistListEntry>(this._selectedLocalModlists, (LocalModlistListEntry entry) => !string.IsNullOrWhiteSpace((entry != null) ? entry.FilePath : null)), (LocalModlistListEntry entry) => entry.FilePath), StringComparer.OrdinalIgnoreCase);
			}
			else
			{
				hashSet = new HashSet<string>(Enumerable.Where<string>(preferredSelection, (string path) => !string.IsNullOrWhiteSpace(path)), StringComparer.OrdinalIgnoreCase);
			}
			HashSet<string> hashSet2 = hashSet;
			if (this.LocalModlistsDataGrid != null)
			{
				this.LocalModlistsDataGrid.SelectedItems.Clear();
				if (hashSet2.Count > 0)
				{
					foreach (LocalModlistListEntry localModlistListEntry2 in Enumerable.OfType<LocalModlistListEntry>(this.LocalModlistsDataGrid.Items))
					{
						if (!string.IsNullOrWhiteSpace((localModlistListEntry2 != null) ? localModlistListEntry2.FilePath : null) && hashSet2.Contains(localModlistListEntry2.FilePath))
						{
							this.LocalModlistsDataGrid.SelectedItems.Add(localModlistListEntry2);
						}
					}
				}
			}
			if (this.LocalModlistsDataGrid != null && this.LocalModlistsDataGrid.SelectedItems.Count > 0)
			{
				List<LocalModlistListEntry> localModlistSelection = Enumerable.ToList<LocalModlistListEntry>(Enumerable.OfType<LocalModlistListEntry>(this.LocalModlistsDataGrid.SelectedItems));
				this.SetLocalModlistSelection(localModlistSelection);
			}
			else
			{
				this.SetLocalModlistSelection(Array.Empty<LocalModlistListEntry>());
			}
			if (list.Count > 0)
			{
				string text = string.Join("\n", Enumerable.Select<string, string>(list, (string error) => "• " + error));
				ModManagerMessageBox.Show(this, "Some local modlists could not be loaded:\n" + text, "Simple VS Manager", 0, 48, null, null, null);
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0001758C File Offset: 0x0001578C
		private IReadOnlyList<LocalModlistListEntry> BuildLocalModlistEntries(out List<string> errors)
		{
			errors = new List<string>();
			string text = this.EnsureModListDirectory();
			if (!Directory.Exists(text))
			{
				return Array.Empty<LocalModlistListEntry>();
			}
			List<LocalModlistListEntry> list = new List<LocalModlistListEntry>();
			foreach (string text2 in Directory.EnumerateFiles(text, "*", 0))
			{
				if (!string.Equals(Path.GetExtension(text2), ".svsmgroup", 5) && MainWindow.HasSupportedModlistExtension(text2))
				{
					LocalModlistListEntry localModlistListEntry;
					string text3;
					if (this.TryCreateLocalModlistEntry(text2, out localModlistListEntry, out text3))
					{
						if (localModlistListEntry != null)
						{
							list.Add(localModlistListEntry);
						}
					}
					else if (!string.IsNullOrWhiteSpace(text3))
					{
						errors.Add(Path.GetFileName(text2) + ": " + text3);
					}
				}
			}
			Dictionary<string, LocalModlistListEntry> sourceEntriesByPath = Enumerable.ToDictionary<LocalModlistListEntry, string>(list, (LocalModlistListEntry entry) => Path.GetFullPath(entry.FilePath), StringComparer.OrdinalIgnoreCase);
			foreach (string text4 in Directory.EnumerateFiles(text, "*.svsmgroup", 0))
			{
				LocalModlistListEntry localModlistListEntry2;
				string text5;
				if (MainWindow.TryCreateLocalModlistGroupEntry(text4, sourceEntriesByPath, out localModlistListEntry2, out text5))
				{
					if (localModlistListEntry2 != null)
					{
						list.Add(localModlistListEntry2);
					}
				}
				else if (!string.IsNullOrWhiteSpace(text5))
				{
					errors.Add(Path.GetFileName(text4) + ": " + text5);
				}
			}
			list.Sort(delegate(LocalModlistListEntry left, LocalModlistListEntry right)
			{
				int num = string.Compare(left.DisplayName, right.DisplayName, 5);
				if (num != 0)
				{
					return num;
				}
				return string.Compare(left.FilePath, right.FilePath, 5);
			});
			return list;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00017724 File Offset: 0x00015924
		private static bool TryCreateLocalModlistGroupEntry(string groupFilePath, IReadOnlyDictionary<string, LocalModlistListEntry> sourceEntriesByPath, [Nullable(2)] out LocalModlistListEntry entry, [Nullable(2)] out string error)
		{
			entry = null;
			error = null;
			bool result;
			try
			{
				ModlistGroupDefinition modlistGroupDefinition = ModlistGroupService.Load(groupFilePath);
				List<ModlistGroupSource> list = new List<ModlistGroupSource>();
				List<string> list2 = new List<string>();
				List<ModlistGroupMemberListEntry> list3 = new List<ModlistGroupMemberListEntry>();
				HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				string groupName = string.IsNullOrWhiteSpace(modlistGroupDefinition.Name) ? Path.GetFileNameWithoutExtension(groupFilePath) : modlistGroupDefinition.Name.Trim();
				foreach (string text in modlistGroupDefinition.Members)
				{
					if (!string.IsNullOrWhiteSpace(text))
					{
						string text2;
						try
						{
							text2 = ModlistGroupService.ResolveReference(groupFilePath, text);
						}
						catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException || ex is NotSupportedException || ex is PathTooLongException)
						{
							list2.Add(text);
							list3.Add(new ModlistGroupMemberListEntry(groupFilePath, groupName, text, null, text, true));
							continue;
						}
						if (hashSet.Add(text2))
						{
							LocalModlistListEntry localModlistListEntry;
							if (!sourceEntriesByPath.TryGetValue(text2, ref localModlistListEntry) || localModlistListEntry.SourcePreset == null)
							{
								list2.Add(text);
								list3.Add(new ModlistGroupMemberListEntry(groupFilePath, groupName, text, text2, Path.GetFileNameWithoutExtension(text2), true));
							}
							else
							{
								list.Add(new ModlistGroupSource(text, localModlistListEntry.FilePath, localModlistListEntry.BaseDisplayName, localModlistListEntry.SourcePreset));
								list3.Add(new ModlistGroupMemberListEntry(groupFilePath, groupName, text, localModlistListEntry.FilePath, localModlistListEntry.BaseDisplayName, false));
							}
						}
					}
				}
				ModlistGroupBuildResult modlistGroupBuildResult = ModlistGroupService.BuildMergedPreset(modlistGroupDefinition, list, list2);
				List<SerializablePresetModState> mods = modlistGroupBuildResult.MergedPreset.Mods;
				List<string> list4;
				if (mods == null)
				{
					list4 = null;
				}
				else
				{
					list4 = Enumerable.ToList<string>(Enumerable.Select<SerializablePresetModState, string>(Enumerable.Where<SerializablePresetModState>(mods, (SerializablePresetModState mod) => mod != null && !string.IsNullOrWhiteSpace(mod.ModId)), delegate(SerializablePresetModState mod)
					{
						if (!string.IsNullOrWhiteSpace(mod.Version))
						{
							return mod.ModId.Trim() + " (" + mod.Version.Trim() + ")";
						}
						return mod.ModId.Trim();
					}));
				}
				List<string> mods2 = list4 ?? new List<string>();
				IEnumerable<string> enumerable = Enumerable.Concat<string>(new string[]
				{
					groupFilePath
				}, Enumerable.Select<ModlistGroupSource, string>(list, (ModlistGroupSource source) => source.FilePath));
				Func<string, DateTime> func;
				if ((func = MainWindow.<>O.<3>__GetLastWriteTimeUtc) == null)
				{
					func = (MainWindow.<>O.<3>__GetLastWriteTimeUtc = new Func<string, DateTime>(File.GetLastWriteTimeUtc));
				}
				DateTime dateTime = Enumerable.Max<DateTime>(Enumerable.DefaultIfEmpty<DateTime>(Enumerable.Select<string, DateTime>(enumerable, func), DateTime.MinValue));
				DateTimeOffset? lastModified = (dateTime == DateTime.MinValue) ? default(DateTimeOffset?) : new DateTimeOffset?(new DateTimeOffset(dateTime, TimeSpan.Zero));
				entry = new LocalModlistListEntry(groupFilePath, modlistGroupBuildResult.MergedPreset.Name, modlistGroupBuildResult.MergedPreset.Description, modlistGroupBuildResult.MergedPreset.Version, null, mods2, lastModified, modlistGroupBuildResult.MergedPreset.GameVersion, modlistGroupBuildResult.MergedPreset, modlistGroupDefinition, modlistGroupBuildResult.Conflicts, modlistGroupBuildResult.MissingMembers, list3);
				result = true;
			}
			catch (Exception ex2) when (ex2 is IOException || ex2 is UnauthorizedAccessException || ex2 is JsonException || ex2 is InvalidDataException || ex2 is InvalidOperationException)
			{
				error = ex2.Message;
				result = false;
			}
			return result;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00017AC0 File Offset: 0x00015CC0
		[NullableContext(2)]
		private bool TryCreateLocalModlistEntry([Nullable(1)] string filePath, out LocalModlistListEntry entry, out string error)
		{
			entry = null;
			error = null;
			bool result;
			try
			{
				if (string.Equals(Path.GetExtension(filePath), ".pdf", 5))
				{
					using (PdfDocument pdfDocument = PdfDocument.Open(filePath, null))
					{
						string json = null;
						DocumentInformation information = pdfDocument.Information;
						string text;
						if (!PdfModlistSerializer.TryExtractModlistJsonFromMetadata((information != null) ? information.Subject : null, out json, out text))
						{
							if (text != null)
							{
								error = text;
								return false;
							}
							StringBuilder stringBuilder = new StringBuilder();
							foreach (Page page in pdfDocument.GetPages())
							{
								StringBuilder stringBuilder2 = new StringBuilder();
								foreach (Letter letter in page.Letters)
								{
									string value = letter.Value;
									if (!string.IsNullOrEmpty(value) && !(value == "\r"))
									{
										stringBuilder2.Append(value);
									}
								}
								string text2 = stringBuilder2.ToString();
								if (!string.IsNullOrWhiteSpace(text2))
								{
									if (stringBuilder.Length > 0)
									{
										stringBuilder.Append('\n');
									}
									stringBuilder.Append(text2);
								}
							}
							string text3 = stringBuilder.ToString();
							if (string.IsNullOrWhiteSpace(text3))
							{
								error = "The PDF did not contain any readable text.";
								return false;
							}
							string text4;
							if (!PdfModlistSerializer.TryExtractModlistJson(text3, out json, out text4))
							{
								error = text4;
								return false;
							}
						}
						string configJson = null;
						string text5;
						if (!PdfModlistSerializer.TryExtractConfigJsonFromMetadata((information != null) ? information.Keywords : null, out configJson, out text5) && text5 != null)
						{
							error = text5;
							return false;
						}
						return this.TryCreateLocalModlistEntryFromJson(filePath, json, out entry, out error, configJson);
					}
				}
				string json2 = File.ReadAllText(filePath);
				result = this.TryCreateLocalModlistEntryFromJson(filePath, json2, out entry, out error, null);
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is InvalidOperationException)
			{
				error = ex.Message;
				result = false;
			}
			return result;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00017D28 File Offset: 0x00015F28
		[NullableContext(2)]
		private bool TryCreateLocalModlistEntryFromJson([Nullable(1)] string filePath, [Nullable(1)] string json, out LocalModlistListEntry entry, out string error, string configJson = null)
		{
			entry = null;
			error = null;
			SerializablePreset serializablePreset;
			string text;
			if (!PdfModlistSerializer.TryDeserializeFromJson(json, out serializablePreset, out text))
			{
				error = (string.IsNullOrWhiteSpace(text) ? "The file is not a valid SVSM modlist." : text);
				return false;
			}
			if (!string.IsNullOrWhiteSpace(configJson))
			{
				SerializableConfigList configList;
				string text2;
				if (!PdfModlistSerializer.TryDeserializeConfigListFromJson(configJson, out configList, out text2))
				{
					error = (string.IsNullOrWhiteSpace(text2) ? "The PDF configuration data could not be read." : text2);
					return false;
				}
				MainWindow.ApplyConfigListToPreset(serializablePreset, configList);
			}
			MainWindow.ModlistMetadata modlistMetadata = MainWindow.ExtractModlistMetadata(json);
			IReadOnlyList<string> mods = modlistMetadata.Mods ?? Array.Empty<string>();
			DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(filePath);
			DateTimeOffset? lastModified = (lastWriteTimeUtc == DateTime.MinValue) ? default(DateTimeOffset?) : new DateTimeOffset?(new DateTimeOffset(lastWriteTimeUtc, TimeSpan.Zero));
			string name = modlistMetadata.Name ?? ((serializablePreset != null) ? serializablePreset.Name : null);
			string description = modlistMetadata.Description ?? ((serializablePreset != null) ? serializablePreset.Description : null);
			string version = modlistMetadata.Version ?? ((serializablePreset != null) ? serializablePreset.Version : null);
			string uploader = modlistMetadata.Uploader ?? ((serializablePreset != null) ? serializablePreset.Uploader : null);
			string gameVersion = modlistMetadata.GameVersion ?? ((serializablePreset != null) ? serializablePreset.GameVersion : null);
			entry = new LocalModlistListEntry(filePath, name, description, version, uploader, mods, lastModified, gameVersion, serializablePreset, null, null, null, null);
			return true;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00017E6C File Offset: 0x0001606C
		private Task RefreshCloudModlistsAsync(bool force)
		{
			MainWindow.<RefreshCloudModlistsAsync>d__590 <RefreshCloudModlistsAsync>d__;
			<RefreshCloudModlistsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RefreshCloudModlistsAsync>d__.<>4__this = this;
			<RefreshCloudModlistsAsync>d__.force = force;
			<RefreshCloudModlistsAsync>d__.<>1__state = -1;
			<RefreshCloudModlistsAsync>d__.<>t__builder.Start<MainWindow.<RefreshCloudModlistsAsync>d__590>(ref <RefreshCloudModlistsAsync>d__);
			return <RefreshCloudModlistsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00017EB8 File Offset: 0x000160B8
		private IReadOnlyList<CloudModlistListEntry> BuildCloudModlistEntries(IEnumerable<CloudModlistRegistryEntry> registryEntries)
		{
			List<CloudModlistListEntry> list = new List<CloudModlistListEntry>();
			if (registryEntries == null)
			{
				return list;
			}
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (CloudModlistRegistryEntry cloudModlistRegistryEntry in registryEntries)
			{
				if (cloudModlistRegistryEntry != null && hashSet.Add(cloudModlistRegistryEntry.RegistryKey))
				{
					string slotLabel = MainWindow.FormatCloudSlotLabel(cloudModlistRegistryEntry.SlotKey);
					MainWindow.ModlistMetadata modlistMetadata = MainWindow.ExtractModlistMetadata(cloudModlistRegistryEntry.ContentJson);
					list.Add(new CloudModlistListEntry(cloudModlistRegistryEntry.OwnerId, cloudModlistRegistryEntry.SlotKey, slotLabel, modlistMetadata.Name, modlistMetadata.Description, modlistMetadata.Version, modlistMetadata.Uploader, modlistMetadata.Mods, cloudModlistRegistryEntry.ContentJson, cloudModlistRegistryEntry.DateAdded, modlistMetadata.GameVersion, cloudModlistRegistryEntry.IsContentComplete));
				}
			}
			list.Sort(delegate(CloudModlistListEntry left, CloudModlistListEntry right)
			{
				int num = string.Compare(left.DisplayName, right.DisplayName, 5);
				if (num != 0)
				{
					return num;
				}
				num = string.Compare(left.OwnerId, right.OwnerId, 5);
				if (num != 0)
				{
					return num;
				}
				return string.Compare(left.SlotKey, right.SlotKey, 5);
			});
			return list;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00017FBC File Offset: 0x000161BC
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		private Task<CloudModlistListEntry> EnsureCloudModlistContentAsync(CloudModlistListEntry entry)
		{
			MainWindow.<EnsureCloudModlistContentAsync>d__592 <EnsureCloudModlistContentAsync>d__;
			<EnsureCloudModlistContentAsync>d__.<>t__builder = AsyncTaskMethodBuilder<CloudModlistListEntry>.Create();
			<EnsureCloudModlistContentAsync>d__.<>4__this = this;
			<EnsureCloudModlistContentAsync>d__.entry = entry;
			<EnsureCloudModlistContentAsync>d__.<>1__state = -1;
			<EnsureCloudModlistContentAsync>d__.<>t__builder.Start<MainWindow.<EnsureCloudModlistContentAsync>d__592>(ref <EnsureCloudModlistContentAsync>d__);
			return <EnsureCloudModlistContentAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00018008 File Offset: 0x00016208
		private void SetLocalModlistSelection(IReadOnlyList<LocalModlistListEntry> selection)
		{
			this._selectedLocalModlists.Clear();
			if (selection != null)
			{
				foreach (LocalModlistListEntry localModlistListEntry in selection)
				{
					if (localModlistListEntry != null)
					{
						this._selectedLocalModlists.Add(localModlistListEntry);
					}
				}
			}
			LocalModlistListEntry localModlistListEntry2 = Enumerable.FirstOrDefault<LocalModlistListEntry>(this._selectedLocalModlists);
			if (this.SelectedLocalModlistTitle != null)
			{
				this.SelectedLocalModlistTitle.Text = (((localModlistListEntry2 != null) ? localModlistListEntry2.DisplayName : null) ?? string.Empty);
			}
			if (this.SelectedLocalModlistDescription != null)
			{
				this.SelectedLocalModlistDescription.Text = (((localModlistListEntry2 != null) ? localModlistListEntry2.Description : null) ?? string.Empty);
			}
			if (this.InstallLocalModlistButton != null)
			{
				if (this._selectedLocalModlists.Count == 1 && localModlistListEntry2 != null)
				{
					this.InstallLocalModlistButton.ToolTip = "Install \"" + localModlistListEntry2.DisplayName + "\"";
					this.InstallLocalModlistButton.Content = (localModlistListEntry2.IsGroup ? "Install Selected Group" : "Install Selected Modlist");
				}
				else
				{
					this.InstallLocalModlistButton.ToolTip = null;
					this.InstallLocalModlistButton.Content = "Install Selected Modlist";
				}
			}
			if (this.ModifyLocalModlistButton != null)
			{
				this.ModifyLocalModlistButton.Content = ((localModlistListEntry2 != null && localModlistListEntry2.IsGroup) ? "Modify Group" : "Modify");
			}
			this.UpdateLocalModlistControlsEnabledState();
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00018168 File Offset: 0x00016368
		[NullableContext(2)]
		private void SetCloudModlistSelection(CloudModlistListEntry entry)
		{
			this._selectedCloudModlist = entry;
			if (this.SelectedModlistTitle != null)
			{
				this.SelectedModlistTitle.Text = (((entry != null) ? entry.DisplayName : null) ?? string.Empty);
			}
			if (this.SelectedModlistDescription != null)
			{
				this.SelectedModlistDescription.Text = (((entry != null) ? entry.Description : null) ?? string.Empty);
			}
			this.UpdateCloudModlistControlsEnabledState();
		}

		// Token: 0x0600030A RID: 778 RVA: 0x000181D4 File Offset: 0x000163D4
		private void UpdateCloudModlistControlsEnabledState()
		{
			bool flag = !InternetAccessManager.IsInternetAccessDisabled;
			if (this.SaveCloudModlistButton != null)
			{
				this.SaveCloudModlistButton.IsEnabled = flag;
			}
			if (this.ModifyCloudModlistsButton != null)
			{
				this.ModifyCloudModlistsButton.IsEnabled = flag;
			}
			if (this.RefreshCloudModlistsButton != null)
			{
				this.RefreshCloudModlistsButton.IsEnabled = (flag && !this._isCloudModlistRefreshInProgress);
			}
			if (this.InstallCloudModlistButton != null)
			{
				bool flag2 = this._selectedCloudModlist != null;
				this.InstallCloudModlistButton.IsEnabled = (flag && flag2);
			}
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00018258 File Offset: 0x00016458
		private void UpdateLocalModlistControlsEnabledState()
		{
			bool isEnabled = this._selectedLocalModlists.Count > 0;
			bool flag = this._selectedLocalModlists.Count == 1;
			this.UpdateLocalModlistControlVisibility();
			if (this.DeleteLocalModlistsButton != null)
			{
				this.DeleteLocalModlistsButton.IsEnabled = isEnabled;
			}
			if (this.ModifyLocalModlistButton != null)
			{
				this.ModifyLocalModlistButton.IsEnabled = (this._selectedLocalModlists.Count == 1);
			}
			if (this.InstallLocalModlistButton != null)
			{
				this.InstallLocalModlistButton.IsEnabled = flag;
			}
			if (this.FixDuplicateModsButton != null)
			{
				int num = (flag && this._selectedLocalModlists[0].IsGroup) ? this._selectedLocalModlists[0].GroupConflicts.Count : 0;
				ContentControl fixDuplicateModsButton = this.FixDuplicateModsButton;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Fix duplicate mods (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				fixDuplicateModsButton.Content = defaultInterpolatedStringHandler.ToStringAndClear();
				this.FixDuplicateModsButton.IsEnabled = (num > 0);
			}
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00018358 File Offset: 0x00016558
		private void UpdateLocalModlistControlVisibility()
		{
			if (this.SaveLocalModlistButton != null)
			{
				this.SaveLocalModlistButton.Visibility = 0;
			}
			if (this.InstallLocalModlistButton != null)
			{
				this.InstallLocalModlistButton.Visibility = 0;
			}
			if (this.DeleteLocalModlistsButton != null)
			{
				this.DeleteLocalModlistsButton.Visibility = 0;
			}
			if (this.ModifyLocalModlistButton != null)
			{
				this.ModifyLocalModlistButton.Visibility = 0;
			}
			if (this.CreateModlistGroupButton != null)
			{
				this.CreateModlistGroupButton.Visibility = 0;
			}
			if (this.FixDuplicateModsButton != null)
			{
				this.FixDuplicateModsButton.Visibility = ((this._selectedLocalModlists.Count == 1 && this._selectedLocalModlists[0].IsGroup) ? 0 : 2);
			}
			if (this.SelectedLocalModlistDetailsBorder != null)
			{
				this.SelectedLocalModlistDetailsBorder.Margin = new Thickness(390.0, 10.0, 10.0, 0.0);
			}
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00018440 File Offset: 0x00016640
		private Task RefreshManagerUpdateLinkAsync()
		{
			MainWindow.<RefreshManagerUpdateLinkAsync>d__598 <RefreshManagerUpdateLinkAsync>d__;
			<RefreshManagerUpdateLinkAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RefreshManagerUpdateLinkAsync>d__.<>4__this = this;
			<RefreshManagerUpdateLinkAsync>d__.<>1__state = -1;
			<RefreshManagerUpdateLinkAsync>d__.<>t__builder.Start<MainWindow.<RefreshManagerUpdateLinkAsync>d__598>(ref <RefreshManagerUpdateLinkAsync>d__);
			return <RefreshManagerUpdateLinkAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00018484 File Offset: 0x00016684
		[NullableContext(2)]
		private static string GetManagerInformationalVersion()
		{
			string result;
			try
			{
				Assembly assembly = typeof(MainWindow).Assembly;
				if (assembly == null)
				{
					result = null;
				}
				else
				{
					AssemblyInformationalVersionAttribute customAttribute = CustomAttributeExtensions.GetCustomAttribute<AssemblyInformationalVersionAttribute>(assembly);
					string text;
					if ((text = ((customAttribute != null) ? customAttribute.InformationalVersion : null)) == null)
					{
						Version version = assembly.GetName().Version;
						text = ((version != null) ? version.ToString() : null);
					}
					string text2 = text;
					if (string.IsNullOrWhiteSpace(text2))
					{
						result = null;
					}
					else
					{
						int num = text2.IndexOf('+');
						if (num >= 0)
						{
							text2 = text2.Substring(0, num);
						}
						result = text2.Trim();
					}
				}
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00018518 File Offset: 0x00016718
		private static string BuildCloudSlotDisplay(string slotKey, MainWindow.ModlistMetadata metadata, bool isOccupied)
		{
			if (!isOccupied)
			{
				return MainWindow.FormatCloudSlotLabel(slotKey) + " (Empty)";
			}
			string text = metadata.Name ?? "Unnamed Modlist";
			if (!string.IsNullOrWhiteSpace(metadata.Version))
			{
				return text + " (v" + metadata.Version + ")";
			}
			return text;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00018570 File Offset: 0x00016770
		private static string FormatCloudSlotLabel(string slotKey)
		{
			if (string.Equals(slotKey, "public", 5))
			{
				return "Public Entry";
			}
			if (slotKey.Length > 4 && slotKey.StartsWith("slot", 5))
			{
				return "Slot " + MemoryExtensions.AsSpan(slotKey, 4).ToString();
			}
			return slotKey;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x000185C9 File Offset: 0x000167C9
		[NullableContext(2)]
		private static string ExtractModlistName(string json)
		{
			return MainWindow.ExtractModlistMetadata(json).Name;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x000185D8 File Offset: 0x000167D8
		private static MainWindow.ModlistMetadata ExtractModlistMetadata([Nullable(2)] string json)
		{
			if (string.IsNullOrWhiteSpace(json))
			{
				return MainWindow.ModlistMetadata.Empty;
			}
			MainWindow.ModlistMetadata result;
			try
			{
				using (JsonDocument jsonDocument = JsonDocument.Parse(json, default(JsonDocumentOptions)))
				{
					if (jsonDocument.RootElement.ValueKind != 1)
					{
						result = MainWindow.ModlistMetadata.Empty;
					}
					else
					{
						JsonElement rootElement = jsonDocument.RootElement;
						string name = MainWindow.TryGetTrimmedProperty(rootElement, "name");
						string description = MainWindow.TryGetTrimmedProperty(rootElement, "description");
						string version = MainWindow.TryGetTrimmedProperty(rootElement, "version");
						string uploader = MainWindow.TryGetTrimmedProperty(rootElement, "uploader") ?? MainWindow.TryGetTrimmedProperty(rootElement, "uploaderName");
						string gameVersion = MainWindow.TryGetTrimmedProperty(rootElement, "gameVersion") ?? MainWindow.TryGetTrimmedProperty(rootElement, "vsVersion");
						List<string> list = new List<string>();
						JsonElement jsonElement;
						if (rootElement.TryGetProperty("mods", ref jsonElement) && jsonElement.ValueKind == 2)
						{
							foreach (JsonElement element in jsonElement.EnumerateArray())
							{
								if (element.ValueKind == 1)
								{
									string text = MainWindow.TryGetTrimmedProperty(element, "modId");
									if (!string.IsNullOrWhiteSpace(text))
									{
										string text2 = MainWindow.TryGetTrimmedProperty(element, "version");
										string text3 = text;
										if (!string.IsNullOrWhiteSpace(text2))
										{
											text3 = text3 + " (" + text2 + ")";
										}
										list.Add(text3);
									}
								}
							}
						}
						result = new MainWindow.ModlistMetadata(name, description, version, uploader, list, gameVersion);
					}
				}
			}
			catch (JsonException)
			{
				result = MainWindow.ModlistMetadata.Empty;
			}
			return result;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x000187B4 File Offset: 0x000169B4
		[return: Nullable(2)]
		private static string TryGetTrimmedProperty(JsonElement element, string propertyName)
		{
			JsonElement jsonElement;
			if (element.ValueKind != 1 || !element.TryGetProperty(propertyName, ref jsonElement) || jsonElement.ValueKind != 3)
			{
				return null;
			}
			string @string = jsonElement.GetString();
			if (!string.IsNullOrWhiteSpace(@string))
			{
				return @string.Trim();
			}
			return null;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x000187FB File Offset: 0x000169FB
		private string EnsurePresetDirectory()
		{
			string text = Path.Combine(this._userConfiguration.GetConfigurationDirectory(), MainWindow.PresetDirectoryName);
			Directory.CreateDirectory(text);
			return text;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0001881C File Offset: 0x00016A1C
		private string EnsureBackupDirectory()
		{
			string configurationDirectory = this._userConfiguration.GetConfigurationDirectory();
			string activeGameProfileBackupDirectoryName = this._userConfiguration.GetActiveGameProfileBackupDirectoryName();
			string text = Path.Combine(configurationDirectory, activeGameProfileBackupDirectoryName);
			Directory.CreateDirectory(text);
			return text;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0001884D File Offset: 0x00016A4D
		private string EnsureLocalModBackupRootDirectory()
		{
			string text = Path.Combine(this.EnsureBackupDirectory(), "Backup Local Mods");
			Directory.CreateDirectory(text);
			return text;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00018868 File Offset: 0x00016A68
		private string CreateLocalModBackupSessionDirectory()
		{
			string text = this.EnsureLocalModBackupRootDirectory();
			string text2 = DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
			string text3 = "Local Mods " + text2;
			string text4 = MainWindow.EnsureUniqueDirectoryPath(Path.Combine(text, text3));
			Directory.CreateDirectory(text4);
			return text4;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x000188B4 File Offset: 0x00016AB4
		private static string EnsureUniqueDirectoryPath(string path)
		{
			if (!Directory.Exists(path) && !File.Exists(path))
			{
				return path;
			}
			int num = 1;
			string text;
			do
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(3, 2);
				defaultInterpolatedStringHandler.AppendFormatted(path);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num++);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				text = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			while (Directory.Exists(text) || File.Exists(text));
			return text;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00018928 File Offset: 0x00016B28
		private static void CopyDirectoryContents(string sourceDirectory, string destinationDirectory)
		{
			Directory.CreateDirectory(destinationDirectory);
			foreach (string text in Directory.GetDirectories(sourceDirectory, "*", 1))
			{
				string relativePath = Path.GetRelativePath(sourceDirectory, text);
				Directory.CreateDirectory(Path.Combine(destinationDirectory, relativePath));
			}
			foreach (string text2 in Directory.GetFiles(sourceDirectory, "*", 1))
			{
				string relativePath2 = Path.GetRelativePath(sourceDirectory, text2);
				string text3 = Path.Combine(destinationDirectory, relativePath2);
				string directoryName = Path.GetDirectoryName(text3);
				if (!string.IsNullOrEmpty(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				File.Copy(text2, text3, false);
			}
		}

		// Token: 0x0600031A RID: 794 RVA: 0x000189C8 File Offset: 0x00016BC8
		private string GetLocalModBackupEntryDirectory(string sessionDirectory, ModListItemViewModel mod)
		{
			string text = string.IsNullOrWhiteSpace(mod.ModId) ? "Mod" : mod.ModId;
			string text2 = MainWindow.SanitizeFileName(string.IsNullOrWhiteSpace(mod.DisplayName) ? text : mod.DisplayName, text);
			return MainWindow.EnsureUniqueDirectoryPath(Path.Combine(sessionDirectory, text2));
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00018A1C File Offset: 0x00016C1C
		private static void BackupLocalModAtPath(string sourcePath, string destinationDirectory)
		{
			if (Directory.Exists(sourcePath))
			{
				MainWindow.CopyDirectoryContents(sourcePath, destinationDirectory);
				return;
			}
			if (File.Exists(sourcePath))
			{
				Directory.CreateDirectory(destinationDirectory);
				string fileName = Path.GetFileName(sourcePath);
				string text = Path.Combine(destinationDirectory, fileName);
				text = MainWindow.EnsureUniqueFilePath(text);
				File.Copy(sourcePath, text, false);
			}
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00018A66 File Offset: 0x00016C66
		private string EnsureModListDirectory()
		{
			string text = Path.Combine(this._userConfiguration.GetConfigurationDirectory(), MainWindow.ModListDirectoryName);
			Directory.CreateDirectory(text);
			return text;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x00018A84 File Offset: 0x00016C84
		private string EnsureRebuiltModListDirectory()
		{
			string text = Path.Combine(this.EnsureModListDirectory(), MainWindow.RebuiltModListDirectoryName);
			Directory.CreateDirectory(text);
			return text;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00018A9D File Offset: 0x00016C9D
		private string EnsureCloudModListCacheDirectory()
		{
			string text = Path.Combine(this._userConfiguration.GetConfigurationDirectory(), MainWindow.CloudModListCacheDirectoryName);
			Directory.CreateDirectory(text);
			return text;
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00018ABC File Offset: 0x00016CBC
		private static bool IsPathWithinDirectory(string directory, string candidatePath)
		{
			bool result;
			try
			{
				ReadOnlySpan<char> readOnlySpan = Path.TrimEndingDirectorySeparator(Path.GetFullPath(directory));
				char directorySeparatorChar = Path.DirectorySeparatorChar;
				string text = readOnlySpan + new ReadOnlySpan<char>(ref directorySeparatorChar);
				result = Path.GetFullPath(candidatePath).StartsWith(text, 5);
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00018B14 File Offset: 0x00016D14
		private static string BuildSuggestedFileName([Nullable(2)] string name, string fallback)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return fallback;
			}
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			StringBuilder stringBuilder = new StringBuilder(name.Length);
			for (int i = 0; i < name.Length; i++)
			{
				char c = name.get_Chars(i);
				stringBuilder.Append((Array.IndexOf<char>(invalidFileNameChars, c) >= 0) ? '_' : c);
			}
			string text = stringBuilder.ToString().Trim();
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text;
			}
			return fallback;
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00018B90 File Offset: 0x00016D90
		private static string GetUniqueFilePath(string directory, string baseFileName, string extension)
		{
			string text = string.IsNullOrWhiteSpace(baseFileName) ? "Modlist" : baseFileName;
			string text2 = text + extension;
			string text3 = Path.Combine(directory, text2);
			int num = 1;
			while (File.Exists(text3))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(3, 3);
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				defaultInterpolatedStringHandler.AppendFormatted(extension);
				text2 = defaultInterpolatedStringHandler.ToStringAndClear();
				text3 = Path.Combine(directory, text2);
				num++;
			}
			return text3;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00018C18 File Offset: 0x00016E18
		private static string GetSnapshotNameFromFilePath(string filePath, string fallback)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
			if (!string.IsNullOrWhiteSpace(fileNameWithoutExtension))
			{
				return fileNameWithoutExtension.Trim();
			}
			return fallback;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00018C3C File Offset: 0x00016E3C
		private Task ApplyPresetAsync(ModPreset preset, bool importConfigurations = true)
		{
			MainWindow.<ApplyPresetAsync>d__620 <ApplyPresetAsync>d__;
			<ApplyPresetAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ApplyPresetAsync>d__.<>4__this = this;
			<ApplyPresetAsync>d__.preset = preset;
			<ApplyPresetAsync>d__.importConfigurations = importConfigurations;
			<ApplyPresetAsync>d__.<>1__state = -1;
			<ApplyPresetAsync>d__.<>t__builder.Start<MainWindow.<ApplyPresetAsync>d__620>(ref <ApplyPresetAsync>d__);
			return <ApplyPresetAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00018C90 File Offset: 0x00016E90
		private Task ImportPresetConfigsAsync(ModPreset preset)
		{
			MainWindow.<ImportPresetConfigsAsync>d__621 <ImportPresetConfigsAsync>d__;
			<ImportPresetConfigsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ImportPresetConfigsAsync>d__.<>4__this = this;
			<ImportPresetConfigsAsync>d__.preset = preset;
			<ImportPresetConfigsAsync>d__.<>1__state = -1;
			<ImportPresetConfigsAsync>d__.<>t__builder.Start<MainWindow.<ImportPresetConfigsAsync>d__621>(ref <ImportPresetConfigsAsync>d__);
			return <ImportPresetConfigsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00018CDC File Offset: 0x00016EDC
		private Task<bool> ApplyPresetModVersionsAsync(ModPreset preset)
		{
			MainWindow.<ApplyPresetModVersionsAsync>d__622 <ApplyPresetModVersionsAsync>d__;
			<ApplyPresetModVersionsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<ApplyPresetModVersionsAsync>d__.<>4__this = this;
			<ApplyPresetModVersionsAsync>d__.preset = preset;
			<ApplyPresetModVersionsAsync>d__.<>1__state = -1;
			<ApplyPresetModVersionsAsync>d__.<>t__builder.Start<MainWindow.<ApplyPresetModVersionsAsync>d__622>(ref <ApplyPresetModVersionsAsync>d__);
			return <ApplyPresetModVersionsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00018D28 File Offset: 0x00016F28
		private Task<MainWindow.PresetModInstallResult> TryInstallPresetModAsync(ModPresetModState state, [Nullable(2)] IProgress<ModUpdateProgress> installProgress = null)
		{
			MainWindow.<TryInstallPresetModAsync>d__623 <TryInstallPresetModAsync>d__;
			<TryInstallPresetModAsync>d__.<>t__builder = AsyncTaskMethodBuilder<MainWindow.PresetModInstallResult>.Create();
			<TryInstallPresetModAsync>d__.<>4__this = this;
			<TryInstallPresetModAsync>d__.state = state;
			<TryInstallPresetModAsync>d__.installProgress = installProgress;
			<TryInstallPresetModAsync>d__.<>1__state = -1;
			<TryInstallPresetModAsync>d__.<>t__builder.Start<MainWindow.<TryInstallPresetModAsync>d__623>(ref <TryInstallPresetModAsync>d__);
			return <TryInstallPresetModAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00018D7C File Offset: 0x00016F7C
		private Task ApplyExclusivePresetAsync(ModPreset preset)
		{
			MainWindow.<ApplyExclusivePresetAsync>d__624 <ApplyExclusivePresetAsync>d__;
			<ApplyExclusivePresetAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ApplyExclusivePresetAsync>d__.<>4__this = this;
			<ApplyExclusivePresetAsync>d__.preset = preset;
			<ApplyExclusivePresetAsync>d__.<>1__state = -1;
			<ApplyExclusivePresetAsync>d__.<>t__builder.Start<MainWindow.<ApplyExclusivePresetAsync>d__624>(ref <ApplyExclusivePresetAsync>d__);
			return <ApplyExclusivePresetAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00018DC8 File Offset: 0x00016FC8
		[NullableContext(2)]
		private static bool VersionsMatch(string desiredVersion, string installedVersion)
		{
			if (string.IsNullOrWhiteSpace(desiredVersion) && string.IsNullOrWhiteSpace(installedVersion))
			{
				return true;
			}
			if (!string.IsNullOrWhiteSpace(desiredVersion) && !string.IsNullOrWhiteSpace(installedVersion))
			{
				if (string.Equals(desiredVersion.Trim(), installedVersion.Trim(), 5))
				{
					return true;
				}
				string text = VersionStringUtility.Normalize(desiredVersion);
				string text2 = VersionStringUtility.Normalize(installedVersion);
				if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(text2) && string.Equals(text, text2, 5))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00018E38 File Offset: 0x00017038
		private void LatestVersionTextBlock_OnToolTipOpening(object sender, ToolTipEventArgs e)
		{
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement == null)
			{
				return;
			}
			ToolTip toolTip = frameworkElement.ToolTip as ToolTip;
			if (toolTip == null)
			{
				return;
			}
			toolTip.PreviewMouseWheel -= new MouseWheelEventHandler(this.ChangelogToolTip_OnPreviewMouseWheel);
			toolTip.PreviewMouseWheel += new MouseWheelEventHandler(this.ChangelogToolTip_OnPreviewMouseWheel);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00018E84 File Offset: 0x00017084
		private void LatestVersionTextBlock_OnToolTipClosing(object sender, ToolTipEventArgs e)
		{
			FrameworkElement frameworkElement = sender as FrameworkElement;
			if (frameworkElement == null)
			{
				return;
			}
			ToolTip toolTip = frameworkElement.ToolTip as ToolTip;
			if (toolTip == null)
			{
				return;
			}
			toolTip.PreviewMouseWheel -= new MouseWheelEventHandler(this.ChangelogToolTip_OnPreviewMouseWheel);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00018EC0 File Offset: 0x000170C0
		private void ChangelogToolTip_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (!e.Handled && e.Delta != 0)
			{
				ToolTip toolTip = sender as ToolTip;
				if (toolTip != null)
				{
					ScrollViewer scrollViewer = toolTip.Content as ScrollViewer;
					if (scrollViewer == null)
					{
						return;
					}
					double num = (double)Math.Max(1, SystemParameters.WheelScrollLines);
					double num2 = (double)e.Delta / 120.0 * num * this.GetCurrentScrollMultiplier();
					if (Math.Abs(num2) < 5E-324)
					{
						return;
					}
					double num3 = scrollViewer.VerticalOffset - num2;
					double num4 = Math.Max(0.0, Math.Min(num3, scrollViewer.ScrollableHeight));
					scrollViewer.ScrollToVerticalOffset(num4);
					e.Handled = true;
					return;
				}
			}
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00018F68 File Offset: 0x00017168
		private void ModsDataGrid_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (e.Handled || e.Delta == 0)
			{
				return;
			}
			DependencyObject dependencyObject = e.OriginalSource as DependencyObject;
			if (dependencyObject != null && MainWindow.IsDescendantOfToolTip(dependencyObject))
			{
				return;
			}
			if (!(sender is DependencyObject))
			{
				return;
			}
			ScrollViewer modsScrollViewer = this.GetModsScrollViewer();
			if (modsScrollViewer == null)
			{
				return;
			}
			double num = (double)e.Delta / 120.0;
			if ((Keyboard.Modifiers & 4) == 4)
			{
				this.ResetModsVerticalWheelAccumulator();
				double num2 = modsScrollViewer.HorizontalOffset - num * 48.0;
				double num3 = Math.Max(0.0, Math.Min(num2, modsScrollViewer.ScrollableWidth));
				if (Math.Abs(num3 - modsScrollViewer.HorizontalOffset) < 5E-324)
				{
					return;
				}
				modsScrollViewer.ScrollToHorizontalOffset(num3);
				e.Handled = true;
				return;
			}
			else
			{
				double num4 = (double)Math.Max(1, SystemParameters.WheelScrollLines);
				double num5 = num * num4 * this.GetCurrentScrollMultiplier();
				if (Math.Abs(num5) < 5E-324)
				{
					return;
				}
				if (modsScrollViewer.CanContentScroll && Math.Abs(num5) < 1.0)
				{
					if (this._modsVerticalWheelRemainderSource != modsScrollViewer || Math.Sign(this._modsVerticalWheelRemainder) != Math.Sign(num5))
					{
						this._modsVerticalWheelRemainderSource = modsScrollViewer;
						this._modsVerticalWheelRemainder = 0.0;
					}
					this._modsVerticalWheelRemainder += num5;
					if (Math.Abs(this._modsVerticalWheelRemainder) < 1.0)
					{
						e.Handled = true;
						return;
					}
					num5 = Math.Truncate(this._modsVerticalWheelRemainder);
					this._modsVerticalWheelRemainder -= num5;
				}
				else
				{
					this.ResetModsVerticalWheelAccumulator();
				}
				double num6 = modsScrollViewer.VerticalOffset - num5;
				double num7 = Math.Max(0.0, Math.Min(num6, modsScrollViewer.ScrollableHeight));
				modsScrollViewer.ScrollToVerticalOffset(num7);
				e.Handled = true;
				return;
			}
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00019133 File Offset: 0x00017333
		[NullableContext(2)]
		private static bool IsDescendantOfToolTip(DependencyObject source)
		{
			while (source != null)
			{
				if (source is ToolTip || source is Popup)
				{
					return true;
				}
				source = MainWindow.GetParent(source);
			}
			return false;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00019158 File Offset: 0x00017358
		[return: Nullable(2)]
		private static DependencyObject GetParent(DependencyObject current)
		{
			Visual visual = current as Visual;
			if (visual != null)
			{
				DependencyObject parent = VisualTreeHelper.GetParent(visual);
				if (parent != null)
				{
					return parent;
				}
				FrameworkElement frameworkElement = visual as FrameworkElement;
				if (frameworkElement != null)
				{
					return frameworkElement.Parent;
				}
			}
			FrameworkContentElement frameworkContentElement = current as FrameworkContentElement;
			if (frameworkContentElement != null)
			{
				return frameworkContentElement.Parent ?? frameworkContentElement.TemplatedParent;
			}
			return LogicalTreeHelper.GetParent(current);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x000191B0 File Offset: 0x000173B0
		[NullableContext(2)]
		private static T FindAncestor<[Nullable(0)] T>(DependencyObject source) where T : DependencyObject
		{
			while (source != null)
			{
				T t = source as T;
				if (t != null)
				{
					return t;
				}
				source = MainWindow.GetParent(source);
			}
			return default(T);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x000191E9 File Offset: 0x000173E9
		private double GetCurrentScrollMultiplier()
		{
			return MainWindow.ModListScrollMultiplier;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x000191F0 File Offset: 0x000173F0
		[NullableContext(2)]
		private bool ShouldPreserveModsViewState(ICollectionView previousView, ICollectionView nextView)
		{
			if (this._viewModel == null || previousView == null || nextView == null)
			{
				return false;
			}
			bool flag = previousView == this._viewModel.ModsView;
			bool flag2 = previousView == this._viewModel.SearchResultsView;
			bool flag3 = nextView == this._viewModel.ModsView;
			return !flag && (flag2 && flag3);
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00019240 File Offset: 0x00017440
		[NullableContext(2)]
		private void AttachToModsView(ICollectionView modsView, bool preserveState = false)
		{
			if (modsView == null)
			{
				if (this._modsCollection != null)
				{
					this._modsCollection.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.ModsView_OnCollectionChanged);
					this._modsCollection = null;
				}
				if (!preserveState)
				{
					this.ClearSelection(true);
				}
				this._currentModsView = null;
				return;
			}
			if (this._modsCollection != null)
			{
				this._modsCollection.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.ModsView_OnCollectionChanged);
				this._modsCollection = null;
			}
			if (modsView != null)
			{
				this._modsCollection = modsView;
				modsView.CollectionChanged += new NotifyCollectionChangedEventHandler(this.ModsView_OnCollectionChanged);
			}
			if (!preserveState)
			{
				this.ClearSelection(true);
			}
			this._currentModsView = modsView;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x000192E0 File Offset: 0x000174E0
		private void ModsView_OnCollectionChanged([Nullable(2)] object sender, NotifyCollectionChangedEventArgs e)
		{
			MainViewModel viewModel = this._viewModel;
			if (sender != ((viewModel != null) ? viewModel.ModsView : null))
			{
				return;
			}
			MainViewModel viewModel2 = this._viewModel;
			if (viewModel2 == null || !viewModel2.IsLoadingMods)
			{
				MainViewModel viewModel3 = this._viewModel;
				if (viewModel3 == null || !viewModel3.IsLoadingModDetails)
				{
					base.Dispatcher.Invoke(delegate()
					{
						this.ClearSelection(true);
					});
					return;
				}
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00019344 File Offset: 0x00017544
		private void HandleModRowSelection(ModListItemViewModel mod)
		{
			if (this._isApplyingPreset)
			{
				return;
			}
			bool flag = Keyboard.Modifiers.HasFlag(4);
			bool flag2 = Keyboard.Modifiers.HasFlag(2);
			if (flag)
			{
				ModListItemViewModel selectionAnchor = this._selectionAnchor;
				if (selectionAnchor == null)
				{
					if (!flag2)
					{
						this.ClearSelection(false);
					}
					this.AddToSelection(mod);
					this._selectionAnchor = mod;
					return;
				}
				if (!this.ApplyRangeSelection(selectionAnchor, mod, flag2))
				{
					this._selectionAnchor = mod;
				}
				return;
			}
			else
			{
				if (!flag2)
				{
					this.ClearSelection(false);
					this.AddToSelection(mod);
					this._selectionAnchor = mod;
					return;
				}
				if (this._selectedMods.Contains(mod))
				{
					this.RemoveFromSelection(mod);
					this._selectionAnchor = mod;
					return;
				}
				this.AddToSelection(mod);
				this._selectionAnchor = mod;
				return;
			}
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00019404 File Offset: 0x00017604
		private void SelectAllModsInCurrentView()
		{
			if (this._isApplyingPreset)
			{
				return;
			}
			List<ModListItemViewModel> modsInViewOrder = this.GetModsInViewOrder();
			this.ClearSelection(true);
			if (modsInViewOrder.Count == 0)
			{
				return;
			}
			foreach (ModListItemViewModel mod in modsInViewOrder)
			{
				this.AddToSelection(mod);
			}
			this._selectionAnchor = modsInViewOrder[modsInViewOrder.Count - 1];
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00019488 File Offset: 0x00017688
		private bool ApplyRangeSelection(ModListItemViewModel start, ModListItemViewModel end, bool preserveExisting)
		{
			List<ModListItemViewModel> modsInViewOrder = this.GetModsInViewOrder();
			int num = modsInViewOrder.IndexOf(start);
			int num2 = modsInViewOrder.IndexOf(end);
			if (num < 0 || num2 < 0)
			{
				if (!preserveExisting)
				{
					this.ClearSelection(false);
				}
				this.AddToSelection(end);
				return false;
			}
			if (!preserveExisting)
			{
				this.ClearSelection(false);
			}
			if (num > num2)
			{
				int num3 = num2;
				num2 = num;
				num = num3;
			}
			for (int i = num; i <= num2; i++)
			{
				this.AddToSelection(modsInViewOrder[i]);
			}
			return true;
		}

		// Token: 0x06000337 RID: 823 RVA: 0x000194F4 File Offset: 0x000176F4
		private List<ModListItemViewModel> GetModsInViewOrder()
		{
			MainViewModel viewModel = this._viewModel;
			ICollectionView collectionView = (viewModel != null) ? viewModel.CurrentModsView : null;
			if (collectionView == null)
			{
				return new List<ModListItemViewModel>();
			}
			return Enumerable.ToList<ModListItemViewModel>(Enumerable.Cast<ModListItemViewModel>(collectionView));
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00019528 File Offset: 0x00017728
		private void AddToSelection(ModListItemViewModel mod)
		{
			if (this._selectedMods.Contains(mod))
			{
				return;
			}
			this._selectedMods.Add(mod);
			this.SubscribeToSelectedMod(mod);
			mod.IsSelected = true;
			this.UpdateSelectedModButtons();
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00019559 File Offset: 0x00017759
		private void RemoveFromSelection(ModListItemViewModel mod)
		{
			if (!this._selectedMods.Remove(mod))
			{
				return;
			}
			mod.IsSelected = false;
			this.UnsubscribeFromSelectedMod(mod);
			this.UpdateSelectedModButtons();
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00019580 File Offset: 0x00017780
		private void ClearSelection(bool resetAnchor = false)
		{
			if (this._selectedMods.Count > 0)
			{
				foreach (ModListItemViewModel modListItemViewModel in this._selectedMods)
				{
					modListItemViewModel.IsSelected = false;
					this.UnsubscribeFromSelectedMod(modListItemViewModel);
				}
				this._selectedMods.Clear();
			}
			if (resetAnchor)
			{
				this._selectionAnchor = null;
			}
			this.UpdateSelectedModButtons();
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00019604 File Offset: 0x00017804
		private void ClearModDatabaseSelections()
		{
			if (this._selectedMods.Count > 0)
			{
				bool flag = false;
				for (int i = this._selectedMods.Count - 1; i >= 0; i--)
				{
					ModListItemViewModel modListItemViewModel = this._selectedMods[i];
					if (modListItemViewModel.IsModDatabaseEntry)
					{
						this._selectedMods.RemoveAt(i);
						modListItemViewModel.IsSelected = false;
						this.UnsubscribeFromSelectedMod(modListItemViewModel);
						flag = true;
					}
				}
				if (flag)
				{
					ModListItemViewModel selectionAnchor = this._selectionAnchor;
					if (selectionAnchor != null && selectionAnchor.IsModDatabaseEntry)
					{
						this._selectionAnchor = null;
					}
					this.UpdateSelectedModButtons();
				}
			}
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0001968C File Offset: 0x0001788C
		private void RestoreSelectionFromSourcePaths(IReadOnlyList<string> sourcePaths, [Nullable(2)] string anchorSourcePath)
		{
			if (this._viewModel == null)
			{
				return;
			}
			List<ModListItemViewModel> list = new List<ModListItemViewModel>(sourcePaths.Count);
			foreach (string text in sourcePaths)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					ModListItemViewModel modListItemViewModel = this._viewModel.FindModBySourcePath(text);
					if (modListItemViewModel != null && !list.Contains(modListItemViewModel))
					{
						list.Add(modListItemViewModel);
					}
				}
			}
			bool flag = list.Count != this._selectedMods.Count;
			if (!flag)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] != this._selectedMods[i])
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				this.UpdateSelectionAnchorAfterRestore(list, anchorSourcePath);
				return;
			}
			foreach (ModListItemViewModel modListItemViewModel2 in this._selectedMods)
			{
				modListItemViewModel2.IsSelected = false;
				this.UnsubscribeFromSelectedMod(modListItemViewModel2);
			}
			this._selectedMods.Clear();
			foreach (ModListItemViewModel modListItemViewModel3 in list)
			{
				this._selectedMods.Add(modListItemViewModel3);
				modListItemViewModel3.IsSelected = true;
				this.SubscribeToSelectedMod(modListItemViewModel3);
			}
			this.UpdateSelectionAnchorAfterRestore(list, anchorSourcePath);
			this.UpdateSelectedModButtons();
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00019820 File Offset: 0x00017A20
		private void UpdateSelectionAnchorAfterRestore(IReadOnlyList<ModListItemViewModel> selection, [Nullable(2)] string anchorSourcePath)
		{
			if (selection.Count == 0)
			{
				this._selectionAnchor = null;
				return;
			}
			if (!string.IsNullOrWhiteSpace(anchorSourcePath))
			{
				foreach (ModListItemViewModel modListItemViewModel in selection)
				{
					if (string.Equals(modListItemViewModel.SourcePath, anchorSourcePath, 5))
					{
						this._selectionAnchor = modListItemViewModel;
						return;
					}
				}
			}
			this._selectionAnchor = selection[selection.Count - 1];
		}

		// Token: 0x0600033E RID: 830 RVA: 0x000198A8 File Offset: 0x00017AA8
		private void SubscribeToSelectedMod(ModListItemViewModel mod)
		{
			MainWindow.<>c__DisplayClass647_0 CS$<>8__locals1 = new MainWindow.<>c__DisplayClass647_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.mod = mod;
			if (this._selectedModPropertyHandlers.ContainsKey(CS$<>8__locals1.mod))
			{
				return;
			}
			PropertyChangedEventHandler propertyChangedEventHandler = delegate([Nullable(2)] object _, PropertyChangedEventArgs args)
			{
				MainWindow.<>c__DisplayClass647_1 CS$<>8__locals2 = new MainWindow.<>c__DisplayClass647_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.shouldRefreshFixButton = (string.IsNullOrEmpty(args.PropertyName) || args.PropertyName == "CanFixDependencyIssues" || args.PropertyName == "HasDependencyIssues" || args.PropertyName == "MissingDependencies" || args.PropertyName == "DependencyHasErrors");
				CS$<>8__locals2.shouldRefreshCopyButton = (string.IsNullOrEmpty(args.PropertyName) || args.PropertyName == "Version");
				if (!CS$<>8__locals2.shouldRefreshFixButton && !CS$<>8__locals2.shouldRefreshCopyButton)
				{
					return;
				}
				if (CS$<>8__locals1.<>4__this.Dispatcher.CheckAccess())
				{
					CS$<>8__locals2.<SubscribeToSelectedMod>g__RefreshButtons|1();
					return;
				}
				CS$<>8__locals1.<>4__this.Dispatcher.Invoke(new Action(CS$<>8__locals2.<SubscribeToSelectedMod>g__RefreshButtons|1));
			};
			CS$<>8__locals1.mod.PropertyChanged += propertyChangedEventHandler;
			this._selectedModPropertyHandlers[CS$<>8__locals1.mod] = propertyChangedEventHandler;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x00019908 File Offset: 0x00017B08
		private void UnsubscribeFromSelectedMod(ModListItemViewModel mod)
		{
			PropertyChangedEventHandler propertyChangedEventHandler;
			if (this._selectedModPropertyHandlers.TryGetValue(mod, ref propertyChangedEventHandler))
			{
				mod.PropertyChanged -= propertyChangedEventHandler;
				this._selectedModPropertyHandlers.Remove(mod);
			}
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00019939 File Offset: 0x00017B39
		private void RefreshSelectedModFixButton(ModListItemViewModel mod)
		{
			if (this._selectedMods.Count == 1 && this._selectedMods[0] == mod)
			{
				this.UpdateSelectedModFixButton(mod);
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0001995F File Offset: 0x00017B5F
		private void RefreshSelectedModCopyForServerButton(ModListItemViewModel mod)
		{
			if (this._selectedMods.Count == 1 && this._selectedMods[0] == mod)
			{
				this.UpdateSelectedModCopyForServerButton(mod);
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00019988 File Offset: 0x00017B88
		private void UpdateSelectedModButtons()
		{
			int count = this._selectedMods.Count;
			ModListItemViewModel mod = (count == 1) ? this._selectedMods[0] : null;
			if (count > 1)
			{
				MainWindow.UpdateSelectedModButton(this.SelectedModDatabasePageButton, null, true, false);
				MainWindow.UpdateSelectedModButton(this.SelectedModUpdateButton, null, false, true);
				this.UpdateSelectedModEditConfigButton(null);
				this.UpdateSelectedModFixButton(null);
				this.UpdateSelectedModCopyForServerButton(null);
				if (this.SelectedModDeleteButton != null)
				{
					bool flag = true;
					this.SelectedModDeleteButton.DataContext = null;
					this.SelectedModDeleteButton.Visibility = (flag ? 0 : 2);
					this.SelectedModDeleteButton.IsEnabled = flag;
				}
			}
			else
			{
				MainWindow.UpdateSelectedModButton(this.SelectedModDatabasePageButton, mod, true, false);
				MainWindow.UpdateSelectedModButton(this.SelectedModUpdateButton, mod, false, true);
				this.UpdateSelectedModEditConfigButton(mod);
				MainWindow.UpdateSelectedModButton(this.SelectedModDeleteButton, mod, false, false);
				this.UpdateSelectedModFixButton(mod);
				this.UpdateSelectedModCopyForServerButton(mod);
			}
			MainViewModel viewModel = this._viewModel;
			if (viewModel == null)
			{
				return;
			}
			viewModel.SetSelectedMod(mod, count);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00019A74 File Offset: 0x00017C74
		private void UpdateServerOptionsState(bool isEnabled)
		{
			if (this.EnableServerOptionsMenuItem != null)
			{
				this.EnableServerOptionsMenuItem.IsChecked = isEnabled;
			}
			ModListItemViewModel modListItemViewModel = (this._selectedMods.Count == 1) ? this._selectedMods[0] : null;
			this.UpdateSelectedModCopyForServerButton(isEnabled ? modListItemViewModel : null);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00019AC0 File Offset: 0x00017CC0
		[NullableContext(2)]
		private void UpdateSelectedModFixButton(ModListItemViewModel mod)
		{
			if (this.SelectedModFixButton == null)
			{
				return;
			}
			if (mod == null || !mod.CanFixDependencyIssues || this._isModUpdateInProgress)
			{
				this.SelectedModFixButton.DataContext = null;
				this.SelectedModFixButton.Visibility = 2;
				this.SelectedModFixButton.IsEnabled = false;
				return;
			}
			this.SelectedModFixButton.DataContext = mod;
			this.SelectedModFixButton.Visibility = 0;
			this.SelectedModFixButton.IsEnabled = true;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00019B34 File Offset: 0x00017D34
		[NullableContext(2)]
		private void UpdateSelectedModCopyForServerButton(ModListItemViewModel mod)
		{
			if (this.SelectedModCopyForServerButton == null)
			{
				return;
			}
			if (!this._userConfiguration.EnableServerOptions || mod == null)
			{
				this.SelectedModCopyForServerButton.DataContext = null;
				this.SelectedModCopyForServerButton.Visibility = 2;
				this.SelectedModCopyForServerButton.IsEnabled = false;
				return;
			}
			if (string.IsNullOrWhiteSpace(ServerCommandBuilder.TryBuildInstallCommand(mod.ModId, mod.Version)))
			{
				this.SelectedModCopyForServerButton.DataContext = null;
				this.SelectedModCopyForServerButton.Visibility = 2;
				this.SelectedModCopyForServerButton.IsEnabled = false;
				return;
			}
			this.SelectedModCopyForServerButton.DataContext = mod;
			this.SelectedModCopyForServerButton.Visibility = 0;
			this.SelectedModCopyForServerButton.IsEnabled = true;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00019BE0 File Offset: 0x00017DE0
		[NullableContext(2)]
		private static void UpdateSelectedModButton(Button button, ModListItemViewModel mod, bool requireModDatabaseLink, bool requireUpdate = false)
		{
			if (button == null)
			{
				return;
			}
			if (mod == null || (requireModDatabaseLink && !mod.HasModDatabasePageLink) || (requireUpdate && !mod.CanUpdate))
			{
				button.DataContext = null;
				button.Visibility = 2;
				button.IsEnabled = false;
				return;
			}
			button.DataContext = mod;
			button.Visibility = 0;
			button.IsEnabled = true;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00019C38 File Offset: 0x00017E38
		[NullableContext(2)]
		private void UpdateSelectedModEditConfigButton(ModListItemViewModel mod)
		{
			if (this.SelectedModEditConfigButton == null)
			{
				return;
			}
			MainWindow.UpdateSelectedModButton(this.SelectedModEditConfigButton, mod, false, false);
			ModListItemViewModel modListItemViewModel = this.SelectedModEditConfigButton.DataContext as ModListItemViewModel;
			if (modListItemViewModel == null)
			{
				this.SelectedModEditConfigButton.ToolTip = null;
				this.SelectedModEditConfigButton.Content = "Edit Config";
				return;
			}
			string text;
			bool flag = !string.IsNullOrWhiteSpace(modListItemViewModel.ModId) && this._userConfiguration.TryGetModConfigPath(modListItemViewModel.ModId, out text) && !string.IsNullOrWhiteSpace(text);
			this.SelectedModEditConfigButton.Content = (flag ? "Edit Config" : "Set Config...");
			this.SelectedModEditConfigButton.ToolTip = (flag ? ("Edit Config for " + modListItemViewModel.DisplayName) : ("Set Config for " + modListItemViewModel.DisplayName));
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00019D08 File Offset: 0x00017F08
		[NullableContext(2)]
		private bool ShouldIgnoreRowSelection(DependencyObject source)
		{
			while (source != null)
			{
				if (source is ButtonBase || source is ToggleSwitch)
				{
					return true;
				}
				FrameworkElement frameworkElement = source as FrameworkElement;
				if (frameworkElement != null && frameworkElement.TemplatedParent is ToggleSwitch)
				{
					return true;
				}
				DataGridCell dataGridCell = source as DataGridCell;
				if (dataGridCell != null && dataGridCell.Column == this.ActiveColumn)
				{
					return true;
				}
				source = VisualTreeHelper.GetParent(source);
			}
			return false;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00019D68 File Offset: 0x00017F68
		[NullableContext(2)]
		private ScrollViewer GetModsScrollViewer()
		{
			DataGrid modsDataGrid = this.ModsDataGrid;
			if (modsDataGrid == null)
			{
				return null;
			}
			if (this._modsScrollViewer != null && this._modsScrollViewerSource == modsDataGrid && this._modsScrollViewer.IsDescendantOf(modsDataGrid))
			{
				return this._modsScrollViewer;
			}
			this.ResetModsVerticalWheelAccumulator();
			this._modsScrollViewer = MainWindow.FindDescendantScrollViewer(modsDataGrid);
			this._modsScrollViewerSource = modsDataGrid;
			return this._modsScrollViewer;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00019DC6 File Offset: 0x00017FC6
		private void ClearScrollViewerCache()
		{
			this.ResetModsVerticalWheelAccumulator();
			this._modsScrollViewer = null;
			this._modsScrollViewerSource = null;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00019DDC File Offset: 0x00017FDC
		private void ResetModsVerticalWheelAccumulator()
		{
			this._modsVerticalWheelRemainderSource = null;
			this._modsVerticalWheelRemainder = 0.0;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00019DF4 File Offset: 0x00017FF4
		[NullableContext(2)]
		private static ScrollViewer FindDescendantScrollViewer(DependencyObject current)
		{
			if (current == null)
			{
				return null;
			}
			ScrollViewer scrollViewer = current as ScrollViewer;
			if (scrollViewer != null)
			{
				return scrollViewer;
			}
			int childrenCount = VisualTreeHelper.GetChildrenCount(current);
			for (int i = 0; i < childrenCount; i++)
			{
				ScrollViewer scrollViewer2 = MainWindow.FindDescendantScrollViewer(VisualTreeHelper.GetChild(current, i));
				if (scrollViewer2 != null)
				{
					return scrollViewer2;
				}
			}
			return null;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00019E38 File Offset: 0x00018038
		private Task UpdateModsAsync(IReadOnlyList<ModListItemViewModel> mods, bool isBulk, [Nullable(new byte[]
		{
			2,
			1,
			1
		})] IReadOnlyDictionary<ModListItemViewModel, ModReleaseInfo> releaseOverrides = null, bool showSummary = true, [Nullable(new byte[]
		{
			2,
			1,
			1,
			2
		})] Func<ModListItemViewModel, ModReleaseInfo, IProgress<ModUpdateProgress>> progressFactory = null, [Nullable(new byte[]
		{
			2,
			1,
			1,
			1
		})] Action<ModListItemViewModel, ModReleaseInfo, ModUpdateResult> completionCallback = null)
		{
			MainWindow.<UpdateModsAsync>d__662 <UpdateModsAsync>d__;
			<UpdateModsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<UpdateModsAsync>d__.<>4__this = this;
			<UpdateModsAsync>d__.mods = mods;
			<UpdateModsAsync>d__.isBulk = isBulk;
			<UpdateModsAsync>d__.releaseOverrides = releaseOverrides;
			<UpdateModsAsync>d__.showSummary = showSummary;
			<UpdateModsAsync>d__.progressFactory = progressFactory;
			<UpdateModsAsync>d__.completionCallback = completionCallback;
			<UpdateModsAsync>d__.<>1__state = -1;
			<UpdateModsAsync>d__.<>t__builder.Start<MainWindow.<UpdateModsAsync>d__662>(ref <UpdateModsAsync>d__);
			return <UpdateModsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00019EB0 File Offset: 0x000180B0
		[return: Nullable(2)]
		private ModReleaseInfo SelectReleaseForMod(ModListItemViewModel mod, List<MainWindow.ModUpdateOperationResult> results)
		{
			ModReleaseInfo latestCompatibleRelease = mod.LatestCompatibleRelease;
			if (latestCompatibleRelease == null)
			{
				results.Add(MainWindow.ModUpdateOperationResult.SkippedResult(mod, "No release marked compatible with the current target Vintage Story version was found."));
				return null;
			}
			return latestCompatibleRelease;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00019EDC File Offset: 0x000180DC
		private void ShowBulkUpdateChangelogDialog(IReadOnlyList<MainWindow.ModUpdateOperationResult> results)
		{
			if (results == null || results.Count <= 0)
			{
				return;
			}
			List<BulkUpdateChangelogWindow.BulkUpdateChangelogItem> list = new List<BulkUpdateChangelogWindow.BulkUpdateChangelogItem>();
			foreach (MainWindow.ModUpdateOperationResult modUpdateOperationResult in results)
			{
				if (modUpdateOperationResult.Success)
				{
					string text = string.IsNullOrWhiteSpace(modUpdateOperationResult.OldVersion) ? "Unknown" : modUpdateOperationResult.OldVersion;
					string text2 = string.IsNullOrWhiteSpace(modUpdateOperationResult.NewVersion) ? "Unknown" : modUpdateOperationResult.NewVersion;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 3);
					defaultInterpolatedStringHandler.AppendFormatted(modUpdateOperationResult.Mod.DisplayName);
					defaultInterpolatedStringHandler.AppendLiteral(" (");
					defaultInterpolatedStringHandler.AppendFormatted(text);
					defaultInterpolatedStringHandler.AppendLiteral(" → ");
					defaultInterpolatedStringHandler.AppendFormatted(text2);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					string title = defaultInterpolatedStringHandler.ToStringAndClear();
					string changelog = string.IsNullOrWhiteSpace(modUpdateOperationResult.ChangelogSummary) ? "No changelog entries were provided for this update." : modUpdateOperationResult.ChangelogSummary;
					list.Add(new BulkUpdateChangelogWindow.BulkUpdateChangelogItem(title, changelog));
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			new BulkUpdateChangelogWindow(list)
			{
				Owner = this
			}.ShowDialog();
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0001A020 File Offset: 0x00018220
		[return: Nullable(2)]
		private static string BuildChangelogSummary(IReadOnlyList<ModListItemViewModel.ReleaseChangelog> changelogEntries)
		{
			if (changelogEntries == null || changelogEntries.Count <= 0)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < changelogEntries.Count; i++)
			{
				ModListItemViewModel.ReleaseChangelog releaseChangelog = changelogEntries[i];
				if (i > 0)
				{
					stringBuilder.AppendLine();
				}
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder3 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler;
				appendInterpolatedStringHandler..ctor(1, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendFormatted(releaseChangelog.Version);
				appendInterpolatedStringHandler.AppendLiteral(":");
				stringBuilder3.AppendLine(ref appendInterpolatedStringHandler);
				stringBuilder.AppendLine(releaseChangelog.Changelog);
			}
			return stringBuilder.ToString().TrimEnd();
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0001A0AC File Offset: 0x000182AC
		private static void ShowUpdateSummary(IReadOnlyList<MainWindow.ModUpdateOperationResult> results, bool isBulk, bool aborted)
		{
			if (results.Count == 0)
			{
				return;
			}
			int num = Enumerable.Count<MainWindow.ModUpdateOperationResult>(results, (MainWindow.ModUpdateOperationResult result) => result.Success);
			int num2 = Enumerable.Count<MainWindow.ModUpdateOperationResult>(results, (MainWindow.ModUpdateOperationResult result) => !result.Success && !result.Skipped);
			int num3 = Enumerable.Count<MainWindow.ModUpdateOperationResult>(results, (MainWindow.ModUpdateOperationResult result) => result.Skipped);
			if (!isBulk && num2 == 0 && num3 == 0)
			{
				return;
			}
			if (isBulk && num2 == 0 && num3 == 0 && !aborted)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(isBulk ? "Bulk update completed." : "Update completed.");
			if (aborted)
			{
				stringBuilder.AppendLine("The operation was cancelled.");
			}
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder3 = stringBuilder2;
			StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler;
			appendInterpolatedStringHandler..ctor(9, 1, stringBuilder2);
			appendInterpolatedStringHandler.AppendLiteral("Updated: ");
			appendInterpolatedStringHandler.AppendFormatted<int>(num);
			stringBuilder3.AppendLine(ref appendInterpolatedStringHandler);
			if (num2 > 0)
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder4 = stringBuilder2;
				appendInterpolatedStringHandler..ctor(8, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("Failed: ");
				appendInterpolatedStringHandler.AppendFormatted<int>(num2);
				stringBuilder4.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (num3 > 0)
			{
				stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder5 = stringBuilder2;
				appendInterpolatedStringHandler..ctor(9, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("Skipped: ");
				appendInterpolatedStringHandler.AppendFormatted<int>(num3);
				stringBuilder5.AppendLine(ref appendInterpolatedStringHandler);
			}
			if (num2 > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Failures:");
				foreach (MainWindow.ModUpdateOperationResult modUpdateOperationResult in Enumerable.Where<MainWindow.ModUpdateOperationResult>(results, (MainWindow.ModUpdateOperationResult result) => !result.Success && !result.Skipped))
				{
					stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder6 = stringBuilder2;
					appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(5, 2, stringBuilder2);
					appendInterpolatedStringHandler.AppendLiteral(" • ");
					appendInterpolatedStringHandler.AppendFormatted(modUpdateOperationResult.Mod.DisplayName);
					appendInterpolatedStringHandler.AppendLiteral(": ");
					appendInterpolatedStringHandler.AppendFormatted(modUpdateOperationResult.Message);
					stringBuilder6.AppendLine(ref appendInterpolatedStringHandler);
				}
			}
			if (num3 > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Skipped:");
				foreach (MainWindow.ModUpdateOperationResult modUpdateOperationResult2 in Enumerable.Where<MainWindow.ModUpdateOperationResult>(results, (MainWindow.ModUpdateOperationResult result) => result.Skipped))
				{
					stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder7 = stringBuilder2;
					appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(5, 2, stringBuilder2);
					appendInterpolatedStringHandler.AppendLiteral(" • ");
					appendInterpolatedStringHandler.AppendFormatted(modUpdateOperationResult2.Mod.DisplayName);
					appendInterpolatedStringHandler.AppendLiteral(": ");
					appendInterpolatedStringHandler.AppendFormatted(modUpdateOperationResult2.Message);
					stringBuilder7.AppendLine(ref appendInterpolatedStringHandler);
				}
			}
			MessageBoxImage icon;
			if (isBulk)
			{
				icon = 0;
			}
			else
			{
				icon = ((num2 > 0) ? 48 : 64);
			}
			ModManagerMessageBox.Show(stringBuilder.ToString(), "Simple VS Manager", 0, icon, null, null, null);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0001A3C0 File Offset: 0x000185C0
		private void ActiveToggle_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			ToggleSwitch toggleSwitch = sender as ToggleSwitch;
			if (toggleSwitch == null)
			{
				return;
			}
			if (!toggleSwitch.IsEnabled)
			{
				return;
			}
			e.Handled = true;
			toggleSwitch.Focus();
			toggleSwitch.IsOn = !toggleSwitch.IsOn;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0001A3FE File Offset: 0x000185FE
		private void ActiveToggle_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (sender is ToggleSwitch)
			{
				e.Handled = true;
			}
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0001A40F File Offset: 0x0001860F
		private void ActiveToggle_OnPreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (!(sender is ToggleSwitch) || e.LeftButton != 1)
			{
				return;
			}
			e.Handled = true;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0001A42C File Offset: 0x0001862C
		private void ActiveToggle_OnToggled(object sender, RoutedEventArgs e)
		{
			if (this._isApplyingMultiToggle)
			{
				return;
			}
			ToggleSwitch toggleSwitch = sender as ToggleSwitch;
			if (toggleSwitch != null)
			{
				ModListItemViewModel modListItemViewModel = toggleSwitch.DataContext as ModListItemViewModel;
				if (modListItemViewModel != null)
				{
					if (!this._selectedMods.Contains(modListItemViewModel) || this._selectedMods.Count <= 1)
					{
						return;
					}
					bool isActive = modListItemViewModel.IsActive;
					try
					{
						this._isApplyingMultiToggle = true;
						foreach (ModListItemViewModel modListItemViewModel2 in this._selectedMods)
						{
							if (modListItemViewModel2 != modListItemViewModel && modListItemViewModel2.CanToggle && modListItemViewModel2.IsActive != isActive)
							{
								modListItemViewModel2.IsActive = isActive;
							}
						}
					}
					finally
					{
						this._isApplyingMultiToggle = false;
					}
					return;
				}
			}
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0001A4FC File Offset: 0x000186FC
		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			if (this._isWindowActive)
			{
				return;
			}
			this._isWindowActive = true;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0001A515 File Offset: 0x00018715
		protected override void OnDeactivated(EventArgs e)
		{
			base.OnDeactivated(e);
			this._isWindowActive = false;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0001A528 File Offset: 0x00018728
		protected override void OnClosed(EventArgs e)
		{
			this.UnsubscribeModBrowserFromDirectoryWatcher();
			this.StopModsWatcher();
			VotesCacheWatcher votesCacheWatcher = this._votesCacheWatcher;
			if (votesCacheWatcher != null)
			{
				votesCacheWatcher.Dispose();
			}
			this._backupSemaphore.Dispose();
			this._cloudStoreLock.Dispose();
			FirebaseModlistStore cloudModlistStore = this._cloudModlistStore;
			if (cloudModlistStore != null)
			{
				cloudModlistStore.Dispose();
			}
			base.OnClosed(e);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0001A580 File Offset: 0x00018780
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/mainwindow.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0001A5B0 File Offset: 0x000187B0
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0001A5BC File Offset: 0x000187BC
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.MainWindowRoot = (MainWindow)target;
				this.MainWindowRoot.PreviewDragEnter += new DragEventHandler(this.MainWindow_OnPreviewDragEnter);
				this.MainWindowRoot.PreviewDragOver += new DragEventHandler(this.MainWindow_OnPreviewDragOver);
				this.MainWindowRoot.PreviewDrop += new DragEventHandler(this.MainWindow_OnPreviewDrop);
				this.MainWindowRoot.PreviewKeyDown += new KeyEventHandler(this.MainWindow_OnPreviewKeyDown);
				return;
			case 2:
				this.RootGrid = (Grid)target;
				return;
			case 3:
				this.TOP = (Grid)target;
				return;
			case 4:
				this.SelectDataFolderMenuItem = (MenuItem)target;
				this.SelectDataFolderMenuItem.Click += new RoutedEventHandler(this.SelectDataFolderMenuItem_OnClick);
				return;
			case 5:
				this.SelectGameFolderMenuItem = (MenuItem)target;
				this.SelectGameFolderMenuItem.Click += new RoutedEventHandler(this.SelectGameFolderMenuItem_OnClick);
				return;
			case 6:
				this.GameProfilesMenuItem = (MenuItem)target;
				this.GameProfilesMenuItem.SubmenuOpened += new RoutedEventHandler(this.GameProfilesMenuItem_OnSubmenuOpened);
				return;
			case 7:
				this.CreateGameProfileMenuItem = (MenuItem)target;
				this.CreateGameProfileMenuItem.Click += new RoutedEventHandler(this.CreateGameProfileMenuItem_OnClick);
				return;
			case 8:
				this.DeleteGameProfileMenuItem = (MenuItem)target;
				this.DeleteGameProfileMenuItem.Click += new RoutedEventHandler(this.DeleteGameProfileMenuItem_OnClick);
				return;
			case 9:
				this.TargetGameVersionMenuItem = (MenuItem)target;
				this.TargetGameVersionMenuItem.SubmenuOpened += new RoutedEventHandler(this.TargetGameVersionMenuItem_OnSubmenuOpened);
				return;
			case 10:
				((MenuItem)target).SubmenuOpened += new RoutedEventHandler(this.RestoreBackupMenuItem_OnSubmenuOpened);
				return;
			case 11:
				this.RestoreDataFolderMenuItem = (MenuItem)target;
				this.RestoreDataFolderMenuItem.SubmenuOpened += new RoutedEventHandler(this.RestoreDataFolderMenuItem_OnSubmenuOpened);
				return;
			case 12:
				this.AutomaticDataBackupsMenuItem = (MenuItem)target;
				this.AutomaticDataBackupsMenuItem.Click += new RoutedEventHandler(this.AutomaticDataBackupsMenuItem_OnClick);
				return;
			case 13:
				this.CloseManagerAfterGameLaunchMenuItem = (MenuItem)target;
				this.CloseManagerAfterGameLaunchMenuItem.Click += new RoutedEventHandler(this.CloseManagerAfterGameLaunchMenuItem_OnClick);
				return;
			case 14:
				((MenuItem)target).Click += new RoutedEventHandler(this.ExitMenuItem_OnClick);
				return;
			case 15:
				((MenuItem)target).SubmenuOpened += new RoutedEventHandler(this.ModsMenuItem_OnSubmenuOpened);
				return;
			case 16:
				((MenuItem)target).Click += new RoutedEventHandler(this.ScanForModConfigsMenuItem_OnClick);
				return;
			case 17:
				((MenuItem)target).Click += new RoutedEventHandler(this.CheckModsCompatibilityMenuItem_OnClick);
				return;
			case 18:
				this.RequireExactVsVersionMenuItem = (MenuItem)target;
				this.RequireExactVsVersionMenuItem.Click += new RoutedEventHandler(this.RequireExactVsVersionMenuItem_OnClick);
				return;
			case 19:
				this.CacheAllVersionsMenuItem = (MenuItem)target;
				this.CacheAllVersionsMenuItem.Click += new RoutedEventHandler(this.CacheAllVersionsMenuItem_OnClick);
				return;
			case 20:
				this.DeleteCachedModsMenuItem = (MenuItem)target;
				this.DeleteCachedModsMenuItem.Click += new RoutedEventHandler(this.DeleteCachedModsMenuItem_OnClick);
				return;
			case 21:
				((MenuItem)target).Click += new RoutedEventHandler(this.OpenModFolderButton_OnClick);
				return;
			case 22:
				((MenuItem)target).Click += new RoutedEventHandler(this.OpenLogsFolderButton_OnClick);
				return;
			case 23:
				((MenuItem)target).Click += new RoutedEventHandler(this.OpenConfigFolderButton_OnClick);
				return;
			case 24:
				((MenuItem)target).Click += new RoutedEventHandler(this.OpenModlistsFolderButton_OnClick);
				return;
			case 25:
				((MenuItem)target).Click += new RoutedEventHandler(this.ManagerDataFolderMenuItem_OnClick);
				return;
			case 26:
				this.PresetsAndModlistsMenuItem = (MenuItem)target;
				return;
			case 27:
				((MenuItem)target).Click += new RoutedEventHandler(this.SaveModlistMenuItem_OnClick);
				return;
			case 28:
				((MenuItem)target).Click += new RoutedEventHandler(this.LoadModlistMenuItem_OnClick);
				return;
			case 29:
				((MenuItem)target).Click += new RoutedEventHandler(this.SavePresetMenuItem_OnClick);
				return;
			case 30:
				this.LoadPresetMenuItem = (MenuItem)target;
				this.LoadPresetMenuItem.SubmenuOpened += new RoutedEventHandler(this.LoadPresetMenuItem_OnSubmenuOpened);
				return;
			case 31:
				this.BrowsePresetMenuItem = (MenuItem)target;
				this.BrowsePresetMenuItem.Click += new RoutedEventHandler(this.LoadPresetMenuItem_OnClick);
				return;
			case 32:
				this.AlwaysClearModlistsMenuItem = (MenuItem)target;
				this.AlwaysClearModlistsMenuItem.Click += new RoutedEventHandler(this.AlwaysClearModlistsMenuItem_OnClick);
				return;
			case 33:
				this.AlwaysAddModlistsMenuItem = (MenuItem)target;
				this.AlwaysAddModlistsMenuItem.Click += new RoutedEventHandler(this.AlwaysAddModlistsMenuItem_OnClick);
				return;
			case 34:
				this.PerformanceMenuItem = (MenuItem)target;
				return;
			case 35:
				this.DisableAutoRefreshMenuItem = (MenuItem)target;
				this.DisableAutoRefreshMenuItem.Click += new RoutedEventHandler(this.DisableAutoRefreshMenuItem_OnClick);
				return;
			case 36:
				this.UseFasterThumbnailsMenuItem = (MenuItem)target;
				this.UseFasterThumbnailsMenuItem.Click += new RoutedEventHandler(this.UseFasterThumbnailsMenuItem_OnClick);
				return;
			case 37:
				this.CacheModDatabaseResultsMenuItem = (MenuItem)target;
				this.CacheModDatabaseResultsMenuItem.Click += new RoutedEventHandler(this.CacheModDatabaseResultsMenuItem_OnClick);
				return;
			case 38:
				this.DisableHoverEffectsMenuItem = (MenuItem)target;
				this.DisableHoverEffectsMenuItem.Click += new RoutedEventHandler(this.DisableHoverEffectsMenuItem_OnClick);
				return;
			case 39:
				this.ThemesMenuItem = (MenuItem)target;
				return;
			case 40:
				this.VintageStoryThemeMenuItem = (MenuItem)target;
				this.VintageStoryThemeMenuItem.Click += new RoutedEventHandler(this.ThemeMenuItem_OnClick);
				return;
			case 41:
				this.DarkThemeMenuItem = (MenuItem)target;
				this.DarkThemeMenuItem.Click += new RoutedEventHandler(this.ThemeMenuItem_OnClick);
				return;
			case 42:
				this.LightThemeMenuItem = (MenuItem)target;
				this.LightThemeMenuItem.Click += new RoutedEventHandler(this.ThemeMenuItem_OnClick);
				return;
			case 43:
				this.CustomThemesSeparator = (Separator)target;
				return;
			case 44:
				this.EditThemeMenuItem = (MenuItem)target;
				this.EditThemeMenuItem.Click += new RoutedEventHandler(this.EditThemeMenuItem_OnClick);
				return;
			case 45:
				this.ColumnsMenuItem = (MenuItem)target;
				return;
			case 46:
				this.ActiveColumnMenuItem = (MenuItem)target;
				this.ActiveColumnMenuItem.Checked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
				this.ActiveColumnMenuItem.Unchecked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
				return;
			case 47:
				this.IconColumnMenuItem = (MenuItem)target;
				this.IconColumnMenuItem.Checked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
				this.IconColumnMenuItem.Unchecked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
				return;
			case 48:
				this.NameColumnMenuItem = (MenuItem)target;
				this.NameColumnMenuItem.Checked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
				this.NameColumnMenuItem.Unchecked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
				return;
			case 49:
				this.VersionColumnMenuItem = (MenuItem)target;
				this.VersionColumnMenuItem.Checked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
				this.VersionColumnMenuItem.Unchecked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
				return;
			case 50:
				this.LatestVersionColumnMenuItem = (MenuItem)target;
				this.LatestVersionColumnMenuItem.Checked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
				this.LatestVersionColumnMenuItem.Unchecked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
				return;
			case 51:
				this.DateModifiedColumnMenuItem = (MenuItem)target;
				this.DateModifiedColumnMenuItem.Checked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
				this.DateModifiedColumnMenuItem.Unchecked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
				return;
			case 52:
				this.AuthorsColumnMenuItem = (MenuItem)target;
				this.AuthorsColumnMenuItem.Checked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
				this.AuthorsColumnMenuItem.Unchecked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
				return;
			case 53:
				this.TagsColumnMenuItem = (MenuItem)target;
				this.TagsColumnMenuItem.Checked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
				this.TagsColumnMenuItem.Unchecked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
				return;
			case 54:
				this.StatusColumnMenuItem = (MenuItem)target;
				this.StatusColumnMenuItem.Checked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
				this.StatusColumnMenuItem.Unchecked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
				return;
			case 55:
				this.UserReportsColumnMenuItem = (MenuItem)target;
				this.UserReportsColumnMenuItem.Checked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
				this.UserReportsColumnMenuItem.Unchecked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
				return;
			case 56:
				this.SideColumnMenuItem = (MenuItem)target;
				this.SideColumnMenuItem.Checked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
				this.SideColumnMenuItem.Unchecked += new RoutedEventHandler(this.InstalledModsColumnMenuItem_OnChecked);
				return;
			case 57:
				this.ManagerVersionMenuItem = (MenuItem)target;
				return;
			case 58:
				this.GameVersionMenuItem = (MenuItem)target;
				return;
			case 59:
				this.GuideMenuItem = (MenuItem)target;
				this.GuideMenuItem.Click += new RoutedEventHandler(this.GuideMenuItem_OnClick);
				return;
			case 60:
				this.HelpMenuItem = (MenuItem)target;
				this.HelpMenuItem.Click += new RoutedEventHandler(this.HelpMenuItem_OnClick);
				return;
			case 61:
				this.ManagerModDbPageMenuItem = (MenuItem)target;
				this.ManagerModDbPageMenuItem.Click += new RoutedEventHandler(this.ManagerModDbPageMenuItem_OnClick);
				return;
			case 62:
				this.ClearAllCachesMenuItem = (MenuItem)target;
				this.ClearAllCachesMenuItem.Click += new RoutedEventHandler(this.ClearAllCachesMenuItem_OnClick);
				return;
			case 63:
				this.SetCustomShortcutMenuItem = (MenuItem)target;
				this.SetCustomShortcutMenuItem.Click += new RoutedEventHandler(this.SetCustomShortcutMenuItem_OnClick);
				return;
			case 64:
				this.DeveloperProfilesMenuItem = (MenuItem)target;
				return;
			case 65:
				this.ChangeManagerFolderMenuItem = (MenuItem)target;
				this.ChangeManagerFolderMenuItem.Click += new RoutedEventHandler(this.ChangeManagerFolderMenuItem_OnClick);
				return;
			case 66:
				((MenuItem)target).Click += new RoutedEventHandler(this.RestoreFirebaseAuthBackupMenuItem_OnClick);
				return;
			case 67:
				this.DisableInternetAccessMenuItem = (MenuItem)target;
				this.DisableInternetAccessMenuItem.Click += new RoutedEventHandler(this.DisableInternetAccessMenuItem_OnClick);
				return;
			case 68:
				this.EnableServerOptionsMenuItem = (MenuItem)target;
				this.EnableServerOptionsMenuItem.Click += new RoutedEventHandler(this.EnableServerOptionsMenuItem_OnClick);
				return;
			case 69:
				this.LogModUpdateMenuItem = (MenuItem)target;
				this.LogModUpdateMenuItem.Click += new RoutedEventHandler(this.LogModUpdateMenuItem_OnClick);
				return;
			case 70:
				this.LogModInstallMenuItem = (MenuItem)target;
				this.LogModInstallMenuItem.Click += new RoutedEventHandler(this.LogModInstallMenuItem_OnClick);
				return;
			case 71:
				this.LogModDeletionMenuItem = (MenuItem)target;
				this.LogModDeletionMenuItem.Click += new RoutedEventHandler(this.LogModDeletionMenuItem_OnClick);
				return;
			case 72:
				this.LogAppLifecycleMenuItem = (MenuItem)target;
				this.LogAppLifecycleMenuItem.Click += new RoutedEventHandler(this.LogAppLifecycleMenuItem_OnClick);
				return;
			case 73:
				this.LogErrorsAndExceptionsMenuItem = (MenuItem)target;
				this.LogErrorsAndExceptionsMenuItem.Click += new RoutedEventHandler(this.LogErrorsAndExceptionsMenuItem_OnClick);
				return;
			case 74:
				((MenuItem)target).Click += new RoutedEventHandler(this.ExperimentalModDebuggingMenuItem_OnClick);
				return;
			case 75:
				((MenuItem)target).Click += new RoutedEventHandler(this.ExperimentalAllModsDebuggingMenuItem_OnClick);
				return;
			case 76:
				((MenuItem)target).Click += new RoutedEventHandler(this.DeleteAllManagerFilesMenuItem_OnClick);
				return;
			case 77:
				this.ActiveGameProfileTextBlock = (TextBlock)target;
				return;
			case 78:
				this.ManagerUpdateLinkTextBlock = (TextBlock)target;
				return;
			case 79:
				((Hyperlink)target).RequestNavigate += new RequestNavigateEventHandler(this.ManagerUpdateLink_OnRequestNavigate);
				return;
			case 80:
				this.ModUsagePromptTextBlock = (TextBlock)target;
				return;
			case 81:
				((Hyperlink)target).Click += new RoutedEventHandler(this.ModUsagePromptLink_OnClick);
				return;
			case 82:
				this.InstalledModDetailsLayout = (DockPanel)target;
				return;
			case 83:
				this.ModInfoPaneRoot = (Border)target;
				return;
			case 84:
				this.ModInfoHeader = (Grid)target;
				return;
			case 85:
				this.ModInfoPanePlacementButton = (Button)target;
				this.ModInfoPanePlacementButton.Click += new RoutedEventHandler(this.ModInfoPanePlacementButton_OnClick);
				return;
			case 86:
				((Button)target).Click += new RoutedEventHandler(this.CloseModInfoButton_OnClick);
				return;
			case 87:
				this.ModInfoContentScrollViewer = (ScrollViewer)target;
				return;
			case 88:
				this.ModInfoContentRoot = (Grid)target;
				return;
			case 89:
				this.ModInfoBatchSelectionCard = (Border)target;
				return;
			case 90:
				this.ModInfoSingleDetailsGrid = (Grid)target;
				return;
			case 91:
				this.ModInfoStatusCard = (Border)target;
				return;
			case 92:
				this.ModInfoDescriptionText = (TextBlock)target;
				return;
			case 93:
				this.ModInfoVersionSection = (StackPanel)target;
				return;
			case 94:
				this.ModInfoVersionHeading = (TextBlock)target;
				return;
			case 95:
				this.SelectedModVersionComboBox = (ComboBox)target;
				this.SelectedModVersionComboBox.DropDownOpened += new EventHandler(this.SelectedModVersionComboBox_OnDropDownOpened);
				this.SelectedModVersionComboBox.SelectionChanged += new SelectionChangedEventHandler(this.SelectedModVersionComboBox_OnSelectionChanged);
				return;
			case 96:
				this.SelectedModUpdateButton = (Button)target;
				this.SelectedModUpdateButton.Click += new RoutedEventHandler(this.UpdateModButton_OnClick);
				return;
			case 97:
				this.ModInfoMetadataSection = (StackPanel)target;
				return;
			case 98:
				this.ModInfoMetadataHeading = (TextBlock)target;
				return;
			case 99:
				this.ModInfoActionsSection = (StackPanel)target;
				return;
			case 100:
				this.ModInfoActionsHeading = (TextBlock)target;
				return;
			case 101:
				this.SelectedModEditConfigButton = (Button)target;
				this.SelectedModEditConfigButton.Click += new RoutedEventHandler(this.EditConfigButton_OnClick);
				return;
			case 102:
				this.SelectedModDatabasePageButton = (Button)target;
				this.SelectedModDatabasePageButton.Click += new RoutedEventHandler(this.ModDatabasePageButton_OnClick);
				return;
			case 103:
				this.SelectedModCopyForServerButton = (Button)target;
				this.SelectedModCopyForServerButton.Click += new RoutedEventHandler(this.SelectedModCopyForServerButton_OnClick);
				return;
			case 104:
				this.ModInfoFooter = (Border)target;
				return;
			case 105:
				this.ModInfoFooterButtons = (StackPanel)target;
				return;
			case 106:
				this.SelectedModFixButton = (Button)target;
				this.SelectedModFixButton.Click += new RoutedEventHandler(this.FixModButton_OnClick);
				return;
			case 107:
				this.SelectedModDeleteButton = (Button)target;
				this.SelectedModDeleteButton.Click += new RoutedEventHandler(this.DeleteModButton_OnClick);
				return;
			case 108:
				this.MIDDLE = (Grid)target;
				return;
			case 109:
				this.MiddleTabControl = (TabControl)target;
				this.MiddleTabControl.SelectionChanged += new SelectionChangedEventHandler(this.MiddleTabControl_OnSelectionChanged);
				return;
			case 110:
				this.MainTab = (TabItem)target;
				return;
			case 111:
				this.MainGrid = (Grid)target;
				return;
			case 112:
				this.MainControls = (Grid)target;
				return;
			case 113:
				this.MainSearch = (TextBox)target;
				return;
			case 114:
				this.MainBorder = (Border)target;
				return;
			case 115:
				this.ModsDataGrid = (DataGrid)target;
				this.ModsDataGrid.ContextMenuOpening += new ContextMenuEventHandler(this.ModsDataGrid_OnContextMenuOpening);
				this.ModsDataGrid.PreviewKeyDown += new KeyEventHandler(this.ModsDataGrid_OnPreviewKeyDown);
				this.ModsDataGrid.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.ModsDataGrid_OnPreviewMouseLeftButtonDown);
				this.ModsDataGrid.PreviewMouseRightButtonDown += new MouseButtonEventHandler(this.ModsDataGrid_OnPreviewMouseRightButtonDown);
				this.ModsDataGrid.PreviewMouseWheel += new MouseWheelEventHandler(this.ModsDataGrid_OnPreviewMouseWheel);
				this.ModsDataGrid.SelectionChanged += new SelectionChangedEventHandler(this.ModsDataGrid_OnSelectionChanged);
				this.ModsDataGrid.Sorting += new DataGridSortingEventHandler(this.ModsDataGrid_OnSorting);
				return;
			case 116:
				this.InstalledModsContextMenu = (ContextMenu)target;
				return;
			case 118:
				this.ActiveColumn = (DataGridTemplateColumn)target;
				return;
			case 120:
				this.IconColumn = (DataGridTemplateColumn)target;
				return;
			case 121:
				this.NameColumn = (DataGridTextColumn)target;
				return;
			case 122:
				this.VersionColumn = (DataGridTextColumn)target;
				return;
			case 123:
				this.LatestVersionColumn = (DataGridTextColumn)target;
				return;
			case 125:
				this.DateModifiedColumn = (DataGridTextColumn)target;
				return;
			case 126:
				this.AuthorsColumn = (DataGridTextColumn)target;
				return;
			case 127:
				this.TagsColumn = (DataGridTemplateColumn)target;
				return;
			case 128:
				this.StatusColumn = (DataGridTemplateColumn)target;
				return;
			case 129:
				this.UserReportsColumn = (DataGridTemplateColumn)target;
				return;
			case 132:
				this.SideColumn = (DataGridTextColumn)target;
				return;
			case 133:
				this.DatabaseTab = (TabItem)target;
				return;
			case 134:
				this.ModBrowserView = (ModBrowserView)target;
				return;
			case 135:
				this.ModlistTab = (TabItem)target;
				return;
			case 136:
				this.ModlistInnerTab = (Grid)target;
				return;
			case 137:
				this.ModlistsTabControl = (TabControl)target;
				this.ModlistsTabControl.Loaded += new RoutedEventHandler(this.ModlistsTabControl_OnLoaded);
				this.ModlistsTabControl.SelectionChanged += new SelectionChangedEventHandler(this.ModlistsTabControl_OnSelectionChanged);
				return;
			case 138:
				this.LocalModlistsTabItem = (TabItem)target;
				return;
			case 139:
				this.LocalModlistsGrid = (Grid)target;
				return;
			case 140:
				this.LocalModlistsDataGrid = (DataGrid)target;
				this.LocalModlistsDataGrid.ContextMenuOpening += new ContextMenuEventHandler(this.LocalModlistsDataGrid_OnContextMenuOpening);
				this.LocalModlistsDataGrid.DragOver += new DragEventHandler(this.LocalModlistsDataGrid_OnDragOver);
				this.LocalModlistsDataGrid.Drop += new DragEventHandler(this.LocalModlistsDataGrid_OnDrop);
				this.LocalModlistsDataGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(this.LocalModlistsDataGrid_OnLoadingRow);
				this.LocalModlistsDataGrid.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.LocalModlistsDataGrid_OnPreviewMouseLeftButtonDown);
				this.LocalModlistsDataGrid.PreviewMouseMove += new MouseEventHandler(this.LocalModlistsDataGrid_OnPreviewMouseMove);
				this.LocalModlistsDataGrid.PreviewMouseRightButtonDown += new MouseButtonEventHandler(this.LocalModlistsDataGrid_OnPreviewMouseRightButtonDown);
				this.LocalModlistsDataGrid.SelectionChanged += new SelectionChangedEventHandler(this.LocalModlistsDataGrid_OnSelectionChanged);
				this.LocalModlistsDataGrid.Sorting += new DataGridSortingEventHandler(this.ModlistsDataGrid_OnSorting);
				return;
			case 141:
				this.LocalModlistsContextMenu = (ContextMenu)target;
				return;
			case 146:
				this.PlaceholderText = (TextBlock)target;
				return;
			case 147:
				this.LocalModlistsControlsBorder = (Border)target;
				return;
			case 148:
				this.LocalModlistsControls = (Grid)target;
				return;
			case 149:
				this.SaveLocalModlistButton = (Button)target;
				this.SaveLocalModlistButton.Click += new RoutedEventHandler(this.SaveLocalModlistButton_OnClick);
				return;
			case 150:
				this.InstallLocalModlistButton = (Button)target;
				this.InstallLocalModlistButton.Click += new RoutedEventHandler(this.InstallLocalModlistButton_OnClick);
				return;
			case 151:
				this.DeleteLocalModlistsButton = (Button)target;
				this.DeleteLocalModlistsButton.Click += new RoutedEventHandler(this.DeleteLocalModlistsButton_OnClick);
				return;
			case 152:
				this.ModifyLocalModlistButton = (Button)target;
				this.ModifyLocalModlistButton.Click += new RoutedEventHandler(this.ModifyLocalModlistButton_OnClick);
				return;
			case 153:
				this.CreateModlistGroupButton = (Button)target;
				this.CreateModlistGroupButton.Click += new RoutedEventHandler(this.CreateModlistGroupButton_OnClick);
				return;
			case 154:
				this.FixDuplicateModsButton = (Button)target;
				this.FixDuplicateModsButton.Click += new RoutedEventHandler(this.FixDuplicateModsButton_OnClick);
				return;
			case 155:
				this.SelectedLocalModlistDetailsBorder = (Border)target;
				return;
			case 156:
				this.SelectedLocalModlistTitle = (TextBlock)target;
				return;
			case 157:
				this.SelectedLocalModlistDescription = (TextBlock)target;
				return;
			case 158:
				this.OnlineModlistsTabItem = (TabItem)target;
				return;
			case 159:
				this.OnlineModlistsGrid = (Grid)target;
				return;
			case 160:
				this.CloudModlistsDataGrid = (DataGrid)target;
				this.CloudModlistsDataGrid.ContextMenuOpening += new ContextMenuEventHandler(this.CloudModlistsDataGrid_OnContextMenuOpening);
				this.CloudModlistsDataGrid.PreviewMouseRightButtonDown += new MouseButtonEventHandler(this.CloudModlistsDataGrid_OnPreviewMouseRightButtonDown);
				this.CloudModlistsDataGrid.SelectionChanged += new SelectionChangedEventHandler(this.CloudModlistsDataGrid_OnSelectionChanged);
				this.CloudModlistsDataGrid.Sorting += new DataGridSortingEventHandler(this.ModlistsDataGrid_OnSorting);
				return;
			case 161:
				this.CloudModlistsContextMenu = (ContextMenu)target;
				return;
			case 162:
				this.OnlineModlistsButtonsBorder = (Border)target;
				return;
			case 163:
				this.OnlineModlistsButtons = (Grid)target;
				return;
			case 164:
				this.InstallCloudModlistButton = (Button)target;
				this.InstallCloudModlistButton.Click += new RoutedEventHandler(this.InstallCloudModlistButton_OnClick);
				return;
			case 165:
				this.SaveCloudModlistButton = (Button)target;
				this.SaveCloudModlistButton.Click += new RoutedEventHandler(this.SaveCloudModlistButton_OnClick);
				return;
			case 166:
				this.ModifyCloudModlistsButton = (Button)target;
				this.ModifyCloudModlistsButton.Click += new RoutedEventHandler(this.ModifyCloudModlistsButton_OnClick);
				return;
			case 167:
				this.RefreshCloudModlistsButton = (Button)target;
				this.RefreshCloudModlistsButton.Click += new RoutedEventHandler(this.RefreshCloudModlistsButton_OnClick);
				return;
			case 168:
				this.OnlineModlistsDescriptions = (Border)target;
				return;
			case 169:
				this.SelectedModlistTitle = (TextBlock)target;
				return;
			case 170:
				this.SelectedModlistDescription = (TextBlock)target;
				return;
			case 171:
				this.ModlistSearch = (TextBox)target;
				return;
			case 172:
				this.SettingsButton = (Button)target;
				this.SettingsButton.Click += new RoutedEventHandler(this.SettingsButton_OnClick);
				return;
			case 173:
				this.DiscordButton = (Button)target;
				this.DiscordButton.Click += new RoutedEventHandler(this.DiscordButton_OnClick);
				return;
			case 174:
				this.BOTTOM = (Grid)target;
				return;
			case 175:
				this.StatusTextBorder = (Border)target;
				return;
			case 176:
				this.UpdateAllButton = (Button)target;
				this.UpdateAllButton.Click += new RoutedEventHandler(this.UpdateAllModsMenuItem_OnClick);
				return;
			case 177:
				this.LaunchGameButton = (Button)target;
				this.LaunchGameButton.Click += new RoutedEventHandler(this.LaunchGameButton_OnClick);
				return;
			case 178:
				this.ModlistInstallOverlay = (Border)target;
				return;
			case 179:
				this.DataBackupOverlay = (Border)target;
				return;
			}
			this._contentLoaded = true;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0001BBF4 File Offset: 0x00019DF4
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId <= 124)
			{
				EventSetter eventSetter;
				if (connectionId == 117)
				{
					eventSetter = new EventSetter();
					eventSetter.Event = UIElement.PreviewMouseLeftButtonDownEvent;
					eventSetter.Handler = new MouseButtonEventHandler(this.ModsDataGridRow_OnPreviewMouseLeftButtonDown);
					((Style)target).Setters.Add(eventSetter);
					eventSetter = new EventSetter();
					eventSetter.Event = FrameworkElement.LoadedEvent;
					eventSetter.Handler = new RoutedEventHandler(this.ModsDataGridRow_OnLoaded);
					((Style)target).Setters.Add(eventSetter);
					eventSetter = new EventSetter();
					eventSetter.Event = FrameworkElement.UnloadedEvent;
					eventSetter.Handler = new RoutedEventHandler(this.ModsDataGridRow_OnUnloaded);
					((Style)target).Setters.Add(eventSetter);
					eventSetter = new EventSetter();
					eventSetter.Event = UIElement.MouseEnterEvent;
					eventSetter.Handler = new MouseEventHandler(this.ModsDataGridRow_OnMouseEnter);
					((Style)target).Setters.Add(eventSetter);
					eventSetter = new EventSetter();
					eventSetter.Event = UIElement.MouseLeaveEvent;
					eventSetter.Handler = new MouseEventHandler(this.ModsDataGridRow_OnMouseLeave);
					((Style)target).Setters.Add(eventSetter);
					return;
				}
				if (connectionId == 119)
				{
					((ToggleSwitch)target).PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.ActiveToggle_OnPreviewMouseLeftButtonDown);
					((ToggleSwitch)target).PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.ActiveToggle_OnPreviewMouseLeftButtonUp);
					((ToggleSwitch)target).PreviewMouseMove += new MouseEventHandler(this.ActiveToggle_OnPreviewMouseMove);
					((ToggleSwitch)target).Toggled += new RoutedEventHandler(this.ActiveToggle_OnToggled);
					return;
				}
				if (connectionId != 124)
				{
					return;
				}
				eventSetter = new EventSetter();
				eventSetter.Event = FrameworkElement.ToolTipOpeningEvent;
				eventSetter.Handler = new ToolTipEventHandler(this.LatestVersionTextBlock_OnToolTipOpening);
				((Style)target).Setters.Add(eventSetter);
				eventSetter = new EventSetter();
				eventSetter.Event = FrameworkElement.ToolTipClosingEvent;
				eventSetter.Handler = new ToolTipEventHandler(this.LatestVersionTextBlock_OnToolTipClosing);
				((Style)target).Setters.Add(eventSetter);
				return;
			}
			else
			{
				if (connectionId == 130)
				{
					((Button)target).Click += new RoutedEventHandler(this.UserReportReasonsButton_OnClick);
					return;
				}
				if (connectionId == 131)
				{
					((Button)target).Click += new RoutedEventHandler(this.UserReportsButton_OnClick);
					return;
				}
				switch (connectionId)
				{
				case 142:
					((Button)target).Click += new RoutedEventHandler(this.GroupReferenceItemButton_OnClick);
					return;
				case 143:
					((MenuItem)target).Click += new RoutedEventHandler(this.RemoveReferencedModlistMenuItem_OnClick);
					return;
				case 144:
					((MenuItem)target).Click += new RoutedEventHandler(this.SelectReferencedModlistMenuItem_OnClick);
					return;
				case 145:
					((ToggleButton)target).Click += new RoutedEventHandler(this.ModlistGroupExpanderButton_OnClick);
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0001C3E7 File Offset: 0x0001A5E7
		[CompilerGenerated]
		private void <InternetAccessManager_OnInternetAccessChanged>g__Update|263_0()
		{
			this.UpdateCloudModlistControlsEnabledState();
			this.RefreshManagerUpdateLinkAsync();
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0001C448 File Offset: 0x0001A648
		[NullableContext(2)]
		[CompilerGenerated]
		internal static void <BuildModSearchTokenSets>g__AddTokenVariations|302_0(string value, ref MainWindow.<>c__DisplayClass302_0 A_1)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return;
			}
			List<string> list = MainWindow.BuildSearchTokens(value, false);
			if (list.Count == 0)
			{
				return;
			}
			MainWindow.<BuildModSearchTokenSets>g__AddTokenSet|302_1(list, ref A_1);
			if (list.Count > 1)
			{
				string text = string.Concat(list);
				if (!string.IsNullOrWhiteSpace(text))
				{
					List<string> list2 = new List<string>();
					list2.Add(text);
					MainWindow.<BuildModSearchTokenSets>g__AddTokenSet|302_1(list2, ref A_1);
				}
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0001C4A0 File Offset: 0x0001A6A0
		[CompilerGenerated]
		internal static void <BuildModSearchTokenSets>g__AddTokenSet|302_1(List<string> tokens, ref MainWindow.<>c__DisplayClass302_0 A_1)
		{
			if (tokens.Count == 0)
			{
				return;
			}
			using (List<IReadOnlyList<string>>.Enumerator enumerator = A_1.tokenSets.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (MainWindow.AreTokenListsEqual(enumerator.Current, tokens))
					{
						return;
					}
				}
			}
			A_1.tokenSets.Add(tokens);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0001C50C File Offset: 0x0001A70C
		[CompilerGenerated]
		internal static void <ExtractWords>g__FlushBuilder|305_0(ref MainWindow.<>c__DisplayClass305_0 A_0)
		{
			if (A_0.builder.Length == 0)
			{
				return;
			}
			string text = A_0.builder.ToString();
			if (A_0.seen.Add(text))
			{
				A_0.results.Add(text);
			}
			A_0.builder.Clear();
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0001C564 File Offset: 0x0001A764
		[NullableContext(2)]
		[CompilerGenerated]
		internal static string <TryExtractModSlug>g__ExtractSlugFromPath|405_0(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return null;
			}
			string text = path.Trim();
			int num = text.IndexOf('#');
			if (num >= 0)
			{
				text = text.Substring(0, num);
			}
			int num2 = text.IndexOf('?');
			if (num2 >= 0)
			{
				text = text.Substring(0, num2);
			}
			text = text.Trim('/');
			if (string.IsNullOrWhiteSpace(text))
			{
				return null;
			}
			int num3 = text.LastIndexOf('/');
			if (num3 >= 0)
			{
				text = text.Substring(num3 + 1);
			}
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text;
			}
			return null;
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0001C780 File Offset: 0x0001A980
		[NullableContext(2)]
		[CompilerGenerated]
		internal static void <TryBuildPresetFromSerializable>g__AddConfiguration|570_2(string fileName, string content, string relativePath = null, ref MainWindow.<>c__DisplayClass570_0 A_3)
		{
			if (string.IsNullOrEmpty(content))
			{
				return;
			}
			string safeConfigFileName = MainWindow.GetSafeConfigFileName(fileName, A_3.modId);
			string text = MainWindow.NormalizeRelativeConfigPath(relativePath, safeConfigFileName);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(4, 3);
			defaultInterpolatedStringHandler.AppendFormatted(safeConfigFileName);
			defaultInterpolatedStringHandler.AppendLiteral("::");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral("::");
			defaultInterpolatedStringHandler.AppendFormatted(content);
			string text2 = defaultInterpolatedStringHandler.ToStringAndClear();
			if (!A_3.seenConfigs.Add(text2))
			{
				return;
			}
			A_3.mergedConfigurations.Add(new ModConfigurationSnapshot(safeConfigFileName, content, text));
		}

		// Token: 0x040000D6 RID: 214
		private const string SummaryKeyPatchModPrefix = "__PATCH_MOD__";

		// Token: 0x040000D7 RID: 215
		private const string SummaryKeyLinePrefix = "__PREFIX__";

		// Token: 0x040000D8 RID: 216
		private const string TargetGameVersionMenuAutoTag = "__target_auto__";

		// Token: 0x040000D9 RID: 217
		private const string TargetGameVersionMenuCustomTag = "__target_custom__";

		// Token: 0x040000DA RID: 218
		private const int MaxDataBackupsMenuItems = 15;

		// Token: 0x040000DB RID: 219
		private const int WindowPositionScreenMargin = 50;

		// Token: 0x040000DC RID: 220
		private const string LocalModlistDragDataFormat = "SimpleVSManager.LocalModlist";

		// Token: 0x040000DD RID: 221
		private const string DiscordInviteUrl = "https://discord.gg/Zhm3QnD2s9";

		// Token: 0x040000DE RID: 222
		private static readonly string ManagerModDatabaseUrl = DevConfig.ManagerModDatabaseUrl;

		// Token: 0x040000DF RID: 223
		private static readonly string ManagerModDatabaseModId = DevConfig.ManagerModDatabaseModId;

		// Token: 0x040000E0 RID: 224
		private static readonly string ModDatabaseUnavailableMessage = DevConfig.ModDatabaseUnavailableMessage;

		// Token: 0x040000E1 RID: 225
		private static readonly double ModListScrollMultiplier = DevConfig.ModListScrollMultiplier;

		// Token: 0x040000E2 RID: 226
		private static readonly double ModDbDesignScrollMultiplier = DevConfig.ModDbDesignScrollMultiplier;

		// Token: 0x040000E3 RID: 227
		private static readonly double LoadMoreScrollThreshold = DevConfig.LoadMoreScrollThreshold;

		// Token: 0x040000E4 RID: 228
		private static readonly double HoverOverlayOpacity = DevConfig.HoverOverlayOpacity;

		// Token: 0x040000E5 RID: 229
		private static readonly double SelectionOverlayOpacity = DevConfig.SelectionOverlayOpacity;

		// Token: 0x040000E6 RID: 230
		private static readonly string PresetDirectoryName = DevConfig.PresetDirectoryName;

		// Token: 0x040000E7 RID: 231
		private static readonly string ModListDirectoryName = DevConfig.ModListDirectoryName;

		// Token: 0x040000E8 RID: 232
		private static readonly string CloudModListCacheDirectoryName = DevConfig.CloudModListCacheDirectoryName;

		// Token: 0x040000E9 RID: 233
		private static readonly string RebuiltModListDirectoryName = DevConfig.RebuiltModListDirectoryName;

		// Token: 0x040000EA RID: 234
		private static readonly int AutomaticConfigMaxWordDistance = DevConfig.AutomaticConfigMaxWordDistance;

		// Token: 0x040000EB RID: 235
		private static readonly HttpClient ConnectivityTestHttpClient = new HttpClient
		{
			Timeout = TimeSpan.FromSeconds(10.0)
		};

		// Token: 0x040000EC RID: 236
		private static readonly string[] ExperimentalModDebugLogPrefixes = new string[]
		{
			"client-debug",
			"client-main",
			"server-debug",
			"server-main"
		};

		// Token: 0x040000ED RID: 237
		private static readonly string[] ExperimentalModDebugLogExtensions = new string[]
		{
			".txt",
			".log"
		};

		// Token: 0x040000EE RID: 238
		private static readonly string[] ExperimentalModDebugIgnoredLinePhrases = new string[]
		{
			"Check for mod systems in mod ",
			"Loaded assembly ",
			"Instantiate mod systems for ",
			"Starting system:",
			"Mods, sorted by dependency:",
			"External Origins in load order:"
		};

		// Token: 0x040000EF RID: 239
		private static readonly string[] SupportedConfigExtensions = new string[]
		{
			".json",
			".yaml",
			".yml"
		};

		// Token: 0x040000F0 RID: 240
		private static readonly string[] SummarizableLinePrefixes = new string[]
		{
			"Patch file",
			"Lang key not found:",
			"[Config lib] Values patched:",
			"Loading sound file, game may stutter",
			"[Config lib] Patched",
			"Block must have a unique code",
			"Failed resolving a blocks blockdrop or smeltedstack",
			"Missing mapping for texture code"
		};

		// Token: 0x040000F1 RID: 241
		private static bool _isQuestPdfLicenseInitialized;

		// Token: 0x040000F2 RID: 242
		private static readonly Regex PatchAssetMissingRegex = new Regex("\\bPatch \\d+ in (?<mod>[^:\\r\\n]+)", 521);

		// Token: 0x040000F3 RID: 243
		private static readonly MainWindow.PresetLoadOptions StandardPresetLoadOptions = new MainWindow.PresetLoadOptions(true, false, false);

		// Token: 0x040000F4 RID: 244
		private static readonly MainWindow.PresetLoadOptions ModListLoadOptions = new MainWindow.PresetLoadOptions(true, true, true);

		// Token: 0x040000F5 RID: 245
		private static readonly DependencyProperty BoundModProperty = DependencyProperty.RegisterAttached("BoundMod", typeof(ModListItemViewModel), typeof(MainWindow));

		// Token: 0x040000F6 RID: 246
		private static readonly DependencyProperty BoundModHandlerProperty = DependencyProperty.RegisterAttached("BoundModHandler", typeof(PropertyChangedEventHandler), typeof(MainWindow));

		// Token: 0x040000F7 RID: 247
		private static readonly DependencyProperty RowIsHoveredProperty = DependencyProperty.RegisterAttached("RowIsHovered", typeof(bool), typeof(MainWindow));

		// Token: 0x040000F8 RID: 248
		public static readonly DependencyProperty IsDataBackupInProgressProperty = DependencyProperty.Register("IsDataBackupInProgress", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

		// Token: 0x040000F9 RID: 249
		public static readonly DependencyProperty DataBackupProgressProperty = DependencyProperty.Register("DataBackupProgress", typeof(double), typeof(MainWindow), new PropertyMetadata(0.0));

		// Token: 0x040000FA RID: 250
		public static readonly DependencyProperty DataBackupStatusMessageProperty = DependencyProperty.Register("DataBackupStatusMessage", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));

		// Token: 0x040000FB RID: 251
		public static readonly DependencyProperty IsModlistInstallInProgressProperty = DependencyProperty.Register("IsModlistInstallInProgress", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

		// Token: 0x040000FC RID: 252
		public static readonly DependencyProperty ModlistInstallProgressProperty = DependencyProperty.Register("ModlistInstallProgress", typeof(double), typeof(MainWindow), new PropertyMetadata(0.0));

		// Token: 0x040000FD RID: 253
		public static readonly DependencyProperty ModlistInstallStatusMessageProperty = DependencyProperty.Register("ModlistInstallStatusMessage", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));

		// Token: 0x040000FE RID: 254
		public static readonly DependencyProperty ModlistDownloadSpeedProperty = DependencyProperty.Register("ModlistDownloadSpeed", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));

		// Token: 0x040000FF RID: 255
		public static readonly DependencyProperty HasModlistDownloadSpeedProperty = DependencyProperty.Register("HasModlistDownloadSpeed", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

		// Token: 0x04000100 RID: 256
		private readonly SemaphoreSlim _backupSemaphore = new SemaphoreSlim(1, 1);

		// Token: 0x04000101 RID: 257
		private readonly SemaphoreSlim _cloudStoreLock = new SemaphoreSlim(1, 1);

		// Token: 0x04000102 RID: 258
		private readonly List<MenuItem> _developerProfileMenuItems = new List<MenuItem>();

		// Token: 0x04000103 RID: 259
		private readonly List<MenuItem> _gameProfileMenuItems = new List<MenuItem>();

		// Token: 0x04000104 RID: 260
		private readonly Dictionary<MainWindow.InstalledModsColumn, bool> _installedColumnVisibilityPreferences = new Dictionary<MainWindow.InstalledModsColumn, bool>();

		// Token: 0x04000105 RID: 261
		private readonly ModCompatibilityCommentsService _modCompatibilityCommentsService = new ModCompatibilityCommentsService();

		// Token: 0x04000106 RID: 262
		private readonly ModDatabaseService _modDatabaseService = new ModDatabaseService();

		// Token: 0x04000107 RID: 263
		private readonly ModUpdateService _modUpdateService = new ModUpdateService();

		// Token: 0x04000108 RID: 264
		private DataBackupService _dataBackupService;

		// Token: 0x04000109 RID: 265
		private readonly ModActivityLoggingService _modActivityLoggingService;

		// Token: 0x0400010A RID: 266
		private readonly UserConfigurationService _userConfiguration;

		// Token: 0x0400010B RID: 267
		[Nullable(2)]
		private ModManagerTraceListener _traceListener;

		// Token: 0x0400010C RID: 268
		private readonly Dictionary<ModListItemViewModel, PropertyChangedEventHandler> _selectedModPropertyHandlers = new Dictionary<ModListItemViewModel, PropertyChangedEventHandler>();

		// Token: 0x0400010D RID: 269
		private readonly List<ModListItemViewModel> _selectedMods = new List<ModListItemViewModel>();

		// Token: 0x0400010E RID: 270
		private readonly List<LocalModlistListEntry> _selectedLocalModlists = new List<LocalModlistListEntry>();

		// Token: 0x0400010F RID: 271
		private readonly HashSet<string> _collapsedModlistGroups = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000110 RID: 272
		private Point _localModlistDragStart;

		// Token: 0x04000111 RID: 273
		[Nullable(2)]
		private LocalModlistListEntry _localModlistDragSource;

		// Token: 0x04000112 RID: 274
		[Nullable(2)]
		private CloudModlistListEntry _selectedCloudModlist;

		// Token: 0x04000113 RID: 275
		[Nullable(2)]
		private DataGridColumn _installedContextColumn;

		// Token: 0x04000114 RID: 276
		[Nullable(2)]
		private ModListItemViewModel _selectionAnchor;

		// Token: 0x04000115 RID: 277
		private bool _cloudModlistsLoaded;

		// Token: 0x04000116 RID: 278
		private bool _firebaseMigrationAttempted;

		// Token: 0x04000117 RID: 279
		[Nullable(2)]
		private FirebaseModlistStore _cloudModlistStore;

		// Token: 0x04000118 RID: 280
		[Nullable(2)]
		private ICollectionView _currentModsView;

		// Token: 0x04000119 RID: 281
		[Nullable(2)]
		private ScrollViewer _modsScrollViewer;

		// Token: 0x0400011A RID: 282
		[Nullable(2)]
		private DataGrid _modsScrollViewerSource;

		// Token: 0x0400011B RID: 283
		[Nullable(2)]
		private ScrollViewer _modsVerticalWheelRemainderSource;

		// Token: 0x0400011C RID: 284
		private double _modsVerticalWheelRemainder;

		// Token: 0x0400011D RID: 285
		[Nullable(2)]
		private INotifyCollectionChanged _modsCollection;

		// Token: 0x0400011E RID: 286
		[Nullable(2)]
		private string _customShortcutPath;

		// Token: 0x0400011F RID: 287
		[Nullable(2)]
		private string _dataDirectory;

		// Token: 0x04000120 RID: 288
		[Nullable(2)]
		private string _gameDirectory;

		// Token: 0x04000121 RID: 289
		[Nullable(2)]
		private GameSessionMonitor _gameSessionMonitor;

		// Token: 0x04000122 RID: 290
		[Nullable(2)]
		private DispatcherTimer _modsWatcherTimer;

		// Token: 0x04000123 RID: 291
		[Nullable(2)]
		private MainWindow.ModUsagePromptData _modUsagePromptData;

		// Token: 0x04000124 RID: 292
		[Nullable(2)]
		private VotesCacheWatcher _votesCacheWatcher;

		// Token: 0x04000125 RID: 293
		private bool _isApplyingMultiToggle;

		// Token: 0x04000126 RID: 294
		private bool _isApplyingPreset;

		// Token: 0x04000127 RID: 295
		private bool _isAutomaticRefreshRunning;

		// Token: 0x04000128 RID: 296
		private bool _isInstallingMod;

		// Token: 0x04000129 RID: 297
		private bool _isCloudModlistRefreshInProgress;

		// Token: 0x0400012A RID: 298
		private bool _isDependencyResolutionRefreshPending;

		// Token: 0x0400012B RID: 299
		private bool _isInitializing;

		// Token: 0x0400012C RID: 300
		private bool _isUpdatingModlistsTabSelection;

		// Token: 0x0400012D RID: 301
		private bool _isUpdatingMiddleTabSelection;

		// Token: 0x0400012E RID: 302
		private bool _isModUpdateInProgress;

		// Token: 0x0400012F RID: 303
		private bool _isModUsageDialogOpen;

		// Token: 0x04000130 RID: 304
		private bool _isRefreshingAfterModlistLoad;

		// Token: 0x04000131 RID: 305
		private bool _isWindowActive;

		// Token: 0x04000132 RID: 306
		private bool _refreshAfterModlistLoadPending;

		// Token: 0x04000133 RID: 307
		private bool _localModlistsLoaded;

		// Token: 0x04000134 RID: 308
		private bool _suppressSortPreferenceSave;

		// Token: 0x04000135 RID: 309
		private bool _isModBrowserWatcherSubscribed;

		// Token: 0x04000136 RID: 310
		[Nullable(2)]
		private string _recentLocalModBackupDirectory;

		// Token: 0x04000137 RID: 311
		[Nullable(new byte[]
		{
			2,
			1
		})]
		private List<string> _recentLocalModBackupModNames;

		// Token: 0x04000138 RID: 312
		private int _modlistInstallCompletedSteps;

		// Token: 0x04000139 RID: 313
		private int _modlistInstallTotalSteps;

		// Token: 0x0400013A RID: 314
		[Nullable(2)]
		private MainViewModel _viewModel;

		// Token: 0x0400013B RID: 315
		[Nullable(2)]
		private ModBrowserViewModel _modBrowserViewModel;

		// Token: 0x0400013C RID: 316
		private readonly List<MenuItem> _customThemeMenuItems = new List<MenuItem>();

		// Token: 0x0400013E RID: 318
		[Nullable(0)]
		internal MainWindow MainWindowRoot;

		// Token: 0x0400013F RID: 319
		[Nullable(0)]
		internal Grid RootGrid;

		// Token: 0x04000140 RID: 320
		[Nullable(0)]
		internal Grid TOP;

		// Token: 0x04000141 RID: 321
		[Nullable(0)]
		internal MenuItem SelectDataFolderMenuItem;

		// Token: 0x04000142 RID: 322
		[Nullable(0)]
		internal MenuItem SelectGameFolderMenuItem;

		// Token: 0x04000143 RID: 323
		[Nullable(0)]
		internal MenuItem GameProfilesMenuItem;

		// Token: 0x04000144 RID: 324
		[Nullable(0)]
		internal MenuItem CreateGameProfileMenuItem;

		// Token: 0x04000145 RID: 325
		[Nullable(0)]
		internal MenuItem DeleteGameProfileMenuItem;

		// Token: 0x04000146 RID: 326
		[Nullable(0)]
		internal MenuItem TargetGameVersionMenuItem;

		// Token: 0x04000147 RID: 327
		[Nullable(0)]
		internal MenuItem RestoreDataFolderMenuItem;

		// Token: 0x04000148 RID: 328
		[Nullable(0)]
		internal MenuItem AutomaticDataBackupsMenuItem;

		// Token: 0x04000149 RID: 329
		[Nullable(0)]
		internal MenuItem CloseManagerAfterGameLaunchMenuItem;

		// Token: 0x0400014A RID: 330
		[Nullable(0)]
		internal MenuItem RequireExactVsVersionMenuItem;

		// Token: 0x0400014B RID: 331
		[Nullable(0)]
		internal MenuItem CacheAllVersionsMenuItem;

		// Token: 0x0400014C RID: 332
		[Nullable(0)]
		internal MenuItem DeleteCachedModsMenuItem;

		// Token: 0x0400014D RID: 333
		[Nullable(0)]
		internal MenuItem PresetsAndModlistsMenuItem;

		// Token: 0x0400014E RID: 334
		[Nullable(0)]
		internal MenuItem LoadPresetMenuItem;

		// Token: 0x0400014F RID: 335
		[Nullable(0)]
		internal MenuItem BrowsePresetMenuItem;

		// Token: 0x04000150 RID: 336
		[Nullable(0)]
		internal MenuItem AlwaysClearModlistsMenuItem;

		// Token: 0x04000151 RID: 337
		[Nullable(0)]
		internal MenuItem AlwaysAddModlistsMenuItem;

		// Token: 0x04000152 RID: 338
		[Nullable(0)]
		internal MenuItem PerformanceMenuItem;

		// Token: 0x04000153 RID: 339
		[Nullable(0)]
		internal MenuItem DisableAutoRefreshMenuItem;

		// Token: 0x04000154 RID: 340
		[Nullable(0)]
		internal MenuItem UseFasterThumbnailsMenuItem;

		// Token: 0x04000155 RID: 341
		[Nullable(0)]
		internal MenuItem CacheModDatabaseResultsMenuItem;

		// Token: 0x04000156 RID: 342
		[Nullable(0)]
		internal MenuItem DisableHoverEffectsMenuItem;

		// Token: 0x04000157 RID: 343
		[Nullable(0)]
		internal MenuItem ThemesMenuItem;

		// Token: 0x04000158 RID: 344
		[Nullable(0)]
		internal MenuItem VintageStoryThemeMenuItem;

		// Token: 0x04000159 RID: 345
		[Nullable(0)]
		internal MenuItem DarkThemeMenuItem;

		// Token: 0x0400015A RID: 346
		[Nullable(0)]
		internal MenuItem LightThemeMenuItem;

		// Token: 0x0400015B RID: 347
		[Nullable(0)]
		internal Separator CustomThemesSeparator;

		// Token: 0x0400015C RID: 348
		[Nullable(0)]
		internal MenuItem EditThemeMenuItem;

		// Token: 0x0400015D RID: 349
		[Nullable(0)]
		internal MenuItem ColumnsMenuItem;

		// Token: 0x0400015E RID: 350
		[Nullable(0)]
		internal MenuItem ActiveColumnMenuItem;

		// Token: 0x0400015F RID: 351
		[Nullable(0)]
		internal MenuItem IconColumnMenuItem;

		// Token: 0x04000160 RID: 352
		[Nullable(0)]
		internal MenuItem NameColumnMenuItem;

		// Token: 0x04000161 RID: 353
		[Nullable(0)]
		internal MenuItem VersionColumnMenuItem;

		// Token: 0x04000162 RID: 354
		[Nullable(0)]
		internal MenuItem LatestVersionColumnMenuItem;

		// Token: 0x04000163 RID: 355
		[Nullable(0)]
		internal MenuItem DateModifiedColumnMenuItem;

		// Token: 0x04000164 RID: 356
		[Nullable(0)]
		internal MenuItem AuthorsColumnMenuItem;

		// Token: 0x04000165 RID: 357
		[Nullable(0)]
		internal MenuItem TagsColumnMenuItem;

		// Token: 0x04000166 RID: 358
		[Nullable(0)]
		internal MenuItem StatusColumnMenuItem;

		// Token: 0x04000167 RID: 359
		[Nullable(0)]
		internal MenuItem UserReportsColumnMenuItem;

		// Token: 0x04000168 RID: 360
		[Nullable(0)]
		internal MenuItem SideColumnMenuItem;

		// Token: 0x04000169 RID: 361
		[Nullable(0)]
		internal MenuItem ManagerVersionMenuItem;

		// Token: 0x0400016A RID: 362
		[Nullable(0)]
		internal MenuItem GameVersionMenuItem;

		// Token: 0x0400016B RID: 363
		[Nullable(0)]
		internal MenuItem GuideMenuItem;

		// Token: 0x0400016C RID: 364
		[Nullable(0)]
		internal MenuItem HelpMenuItem;

		// Token: 0x0400016D RID: 365
		[Nullable(0)]
		internal MenuItem ManagerModDbPageMenuItem;

		// Token: 0x0400016E RID: 366
		[Nullable(0)]
		internal MenuItem ClearAllCachesMenuItem;

		// Token: 0x0400016F RID: 367
		[Nullable(0)]
		internal MenuItem SetCustomShortcutMenuItem;

		// Token: 0x04000170 RID: 368
		[Nullable(0)]
		internal MenuItem DeveloperProfilesMenuItem;

		// Token: 0x04000171 RID: 369
		[Nullable(0)]
		internal MenuItem ChangeManagerFolderMenuItem;

		// Token: 0x04000172 RID: 370
		[Nullable(0)]
		internal MenuItem DisableInternetAccessMenuItem;

		// Token: 0x04000173 RID: 371
		[Nullable(0)]
		internal MenuItem EnableServerOptionsMenuItem;

		// Token: 0x04000174 RID: 372
		[Nullable(0)]
		internal MenuItem LogModUpdateMenuItem;

		// Token: 0x04000175 RID: 373
		[Nullable(0)]
		internal MenuItem LogModInstallMenuItem;

		// Token: 0x04000176 RID: 374
		[Nullable(0)]
		internal MenuItem LogModDeletionMenuItem;

		// Token: 0x04000177 RID: 375
		[Nullable(0)]
		internal MenuItem LogAppLifecycleMenuItem;

		// Token: 0x04000178 RID: 376
		[Nullable(0)]
		internal MenuItem LogErrorsAndExceptionsMenuItem;

		// Token: 0x04000179 RID: 377
		[Nullable(0)]
		internal TextBlock ActiveGameProfileTextBlock;

		// Token: 0x0400017A RID: 378
		[Nullable(0)]
		internal TextBlock ManagerUpdateLinkTextBlock;

		// Token: 0x0400017B RID: 379
		[Nullable(0)]
		internal TextBlock ModUsagePromptTextBlock;

		// Token: 0x0400017C RID: 380
		[Nullable(0)]
		internal DockPanel InstalledModDetailsLayout;

		// Token: 0x0400017D RID: 381
		[Nullable(0)]
		internal Border ModInfoPaneRoot;

		// Token: 0x0400017E RID: 382
		[Nullable(0)]
		internal Grid ModInfoHeader;

		// Token: 0x0400017F RID: 383
		[Nullable(0)]
		internal Button ModInfoPanePlacementButton;

		// Token: 0x04000180 RID: 384
		[Nullable(0)]
		internal ScrollViewer ModInfoContentScrollViewer;

		// Token: 0x04000181 RID: 385
		[Nullable(0)]
		internal Grid ModInfoContentRoot;

		// Token: 0x04000182 RID: 386
		[Nullable(0)]
		internal Border ModInfoBatchSelectionCard;

		// Token: 0x04000183 RID: 387
		[Nullable(0)]
		internal Grid ModInfoSingleDetailsGrid;

		// Token: 0x04000184 RID: 388
		[Nullable(0)]
		internal Border ModInfoStatusCard;

		// Token: 0x04000185 RID: 389
		[Nullable(0)]
		internal TextBlock ModInfoDescriptionText;

		// Token: 0x04000186 RID: 390
		[Nullable(0)]
		internal StackPanel ModInfoVersionSection;

		// Token: 0x04000187 RID: 391
		[Nullable(0)]
		internal TextBlock ModInfoVersionHeading;

		// Token: 0x04000188 RID: 392
		[Nullable(0)]
		internal ComboBox SelectedModVersionComboBox;

		// Token: 0x04000189 RID: 393
		[Nullable(0)]
		internal Button SelectedModUpdateButton;

		// Token: 0x0400018A RID: 394
		[Nullable(0)]
		internal StackPanel ModInfoMetadataSection;

		// Token: 0x0400018B RID: 395
		[Nullable(0)]
		internal TextBlock ModInfoMetadataHeading;

		// Token: 0x0400018C RID: 396
		[Nullable(0)]
		internal StackPanel ModInfoActionsSection;

		// Token: 0x0400018D RID: 397
		[Nullable(0)]
		internal TextBlock ModInfoActionsHeading;

		// Token: 0x0400018E RID: 398
		[Nullable(0)]
		internal Button SelectedModEditConfigButton;

		// Token: 0x0400018F RID: 399
		[Nullable(0)]
		internal Button SelectedModDatabasePageButton;

		// Token: 0x04000190 RID: 400
		[Nullable(0)]
		internal Button SelectedModCopyForServerButton;

		// Token: 0x04000191 RID: 401
		[Nullable(0)]
		internal Border ModInfoFooter;

		// Token: 0x04000192 RID: 402
		[Nullable(0)]
		internal StackPanel ModInfoFooterButtons;

		// Token: 0x04000193 RID: 403
		[Nullable(0)]
		internal Button SelectedModFixButton;

		// Token: 0x04000194 RID: 404
		[Nullable(0)]
		internal Button SelectedModDeleteButton;

		// Token: 0x04000195 RID: 405
		[Nullable(0)]
		internal Grid MIDDLE;

		// Token: 0x04000196 RID: 406
		[Nullable(0)]
		internal TabControl MiddleTabControl;

		// Token: 0x04000197 RID: 407
		[Nullable(0)]
		internal TabItem MainTab;

		// Token: 0x04000198 RID: 408
		[Nullable(0)]
		internal Grid MainGrid;

		// Token: 0x04000199 RID: 409
		[Nullable(0)]
		internal Grid MainControls;

		// Token: 0x0400019A RID: 410
		[Nullable(0)]
		internal TextBox MainSearch;

		// Token: 0x0400019B RID: 411
		[Nullable(0)]
		internal Border MainBorder;

		// Token: 0x0400019C RID: 412
		[Nullable(0)]
		internal DataGrid ModsDataGrid;

		// Token: 0x0400019D RID: 413
		[Nullable(0)]
		internal ContextMenu InstalledModsContextMenu;

		// Token: 0x0400019E RID: 414
		[Nullable(0)]
		internal DataGridTemplateColumn ActiveColumn;

		// Token: 0x0400019F RID: 415
		[Nullable(0)]
		internal DataGridTemplateColumn IconColumn;

		// Token: 0x040001A0 RID: 416
		[Nullable(0)]
		internal DataGridTextColumn NameColumn;

		// Token: 0x040001A1 RID: 417
		[Nullable(0)]
		internal DataGridTextColumn VersionColumn;

		// Token: 0x040001A2 RID: 418
		[Nullable(0)]
		internal DataGridTextColumn LatestVersionColumn;

		// Token: 0x040001A3 RID: 419
		[Nullable(0)]
		internal DataGridTextColumn DateModifiedColumn;

		// Token: 0x040001A4 RID: 420
		[Nullable(0)]
		internal DataGridTextColumn AuthorsColumn;

		// Token: 0x040001A5 RID: 421
		[Nullable(0)]
		internal DataGridTemplateColumn TagsColumn;

		// Token: 0x040001A6 RID: 422
		[Nullable(0)]
		internal DataGridTemplateColumn StatusColumn;

		// Token: 0x040001A7 RID: 423
		[Nullable(0)]
		internal DataGridTemplateColumn UserReportsColumn;

		// Token: 0x040001A8 RID: 424
		[Nullable(0)]
		internal DataGridTextColumn SideColumn;

		// Token: 0x040001A9 RID: 425
		[Nullable(0)]
		internal TabItem DatabaseTab;

		// Token: 0x040001AA RID: 426
		[Nullable(0)]
		internal ModBrowserView ModBrowserView;

		// Token: 0x040001AB RID: 427
		[Nullable(0)]
		internal TabItem ModlistTab;

		// Token: 0x040001AC RID: 428
		[Nullable(0)]
		internal Grid ModlistInnerTab;

		// Token: 0x040001AD RID: 429
		[Nullable(0)]
		internal TabControl ModlistsTabControl;

		// Token: 0x040001AE RID: 430
		[Nullable(0)]
		internal TabItem LocalModlistsTabItem;

		// Token: 0x040001AF RID: 431
		[Nullable(0)]
		internal Grid LocalModlistsGrid;

		// Token: 0x040001B0 RID: 432
		[Nullable(0)]
		internal DataGrid LocalModlistsDataGrid;

		// Token: 0x040001B1 RID: 433
		[Nullable(0)]
		internal ContextMenu LocalModlistsContextMenu;

		// Token: 0x040001B2 RID: 434
		[Nullable(0)]
		internal TextBlock PlaceholderText;

		// Token: 0x040001B3 RID: 435
		[Nullable(0)]
		internal Border LocalModlistsControlsBorder;

		// Token: 0x040001B4 RID: 436
		[Nullable(0)]
		internal Grid LocalModlistsControls;

		// Token: 0x040001B5 RID: 437
		[Nullable(0)]
		internal Button SaveLocalModlistButton;

		// Token: 0x040001B6 RID: 438
		[Nullable(0)]
		internal Button InstallLocalModlistButton;

		// Token: 0x040001B7 RID: 439
		[Nullable(0)]
		internal Button DeleteLocalModlistsButton;

		// Token: 0x040001B8 RID: 440
		[Nullable(0)]
		internal Button ModifyLocalModlistButton;

		// Token: 0x040001B9 RID: 441
		[Nullable(0)]
		internal Button CreateModlistGroupButton;

		// Token: 0x040001BA RID: 442
		[Nullable(0)]
		internal Button FixDuplicateModsButton;

		// Token: 0x040001BB RID: 443
		[Nullable(0)]
		internal Border SelectedLocalModlistDetailsBorder;

		// Token: 0x040001BC RID: 444
		[Nullable(0)]
		internal TextBlock SelectedLocalModlistTitle;

		// Token: 0x040001BD RID: 445
		[Nullable(0)]
		internal TextBlock SelectedLocalModlistDescription;

		// Token: 0x040001BE RID: 446
		[Nullable(0)]
		internal TabItem OnlineModlistsTabItem;

		// Token: 0x040001BF RID: 447
		[Nullable(0)]
		internal Grid OnlineModlistsGrid;

		// Token: 0x040001C0 RID: 448
		[Nullable(0)]
		internal DataGrid CloudModlistsDataGrid;

		// Token: 0x040001C1 RID: 449
		[Nullable(0)]
		internal ContextMenu CloudModlistsContextMenu;

		// Token: 0x040001C2 RID: 450
		[Nullable(0)]
		internal Border OnlineModlistsButtonsBorder;

		// Token: 0x040001C3 RID: 451
		[Nullable(0)]
		internal Grid OnlineModlistsButtons;

		// Token: 0x040001C4 RID: 452
		[Nullable(0)]
		internal Button InstallCloudModlistButton;

		// Token: 0x040001C5 RID: 453
		[Nullable(0)]
		internal Button SaveCloudModlistButton;

		// Token: 0x040001C6 RID: 454
		[Nullable(0)]
		internal Button ModifyCloudModlistsButton;

		// Token: 0x040001C7 RID: 455
		[Nullable(0)]
		internal Button RefreshCloudModlistsButton;

		// Token: 0x040001C8 RID: 456
		[Nullable(0)]
		internal Border OnlineModlistsDescriptions;

		// Token: 0x040001C9 RID: 457
		[Nullable(0)]
		internal TextBlock SelectedModlistTitle;

		// Token: 0x040001CA RID: 458
		[Nullable(0)]
		internal TextBlock SelectedModlistDescription;

		// Token: 0x040001CB RID: 459
		[Nullable(0)]
		internal TextBox ModlistSearch;

		// Token: 0x040001CC RID: 460
		[Nullable(0)]
		internal Button SettingsButton;

		// Token: 0x040001CD RID: 461
		[Nullable(0)]
		internal Button DiscordButton;

		// Token: 0x040001CE RID: 462
		[Nullable(0)]
		internal Grid BOTTOM;

		// Token: 0x040001CF RID: 463
		[Nullable(0)]
		internal Border StatusTextBorder;

		// Token: 0x040001D0 RID: 464
		[Nullable(0)]
		internal Button UpdateAllButton;

		// Token: 0x040001D1 RID: 465
		[Nullable(0)]
		internal Button LaunchGameButton;

		// Token: 0x040001D2 RID: 466
		[Nullable(0)]
		internal Border ModlistInstallOverlay;

		// Token: 0x040001D3 RID: 467
		[Nullable(0)]
		internal Border DataBackupOverlay;

		// Token: 0x040001D4 RID: 468
		private bool _contentLoaded;

		// Token: 0x0200011A RID: 282
		[NullableContext(0)]
		private readonly struct PresetLoadOptions : IEquatable<MainWindow.PresetLoadOptions>
		{
			// Token: 0x06001165 RID: 4453 RVA: 0x0005E6FF File Offset: 0x0005C8FF
			public PresetLoadOptions(bool ApplyModStatus, bool ApplyModVersions, bool ForceExclusive)
			{
				this.ApplyModStatus = ApplyModStatus;
				this.ApplyModVersions = ApplyModVersions;
				this.ForceExclusive = ForceExclusive;
			}

			// Token: 0x17000421 RID: 1057
			// (get) Token: 0x06001166 RID: 4454 RVA: 0x0005E716 File Offset: 0x0005C916
			// (set) Token: 0x06001167 RID: 4455 RVA: 0x0005E71E File Offset: 0x0005C91E
			public bool ApplyModStatus { get; set; }

			// Token: 0x17000422 RID: 1058
			// (get) Token: 0x06001168 RID: 4456 RVA: 0x0005E727 File Offset: 0x0005C927
			// (set) Token: 0x06001169 RID: 4457 RVA: 0x0005E72F File Offset: 0x0005C92F
			public bool ApplyModVersions { get; set; }

			// Token: 0x17000423 RID: 1059
			// (get) Token: 0x0600116A RID: 4458 RVA: 0x0005E738 File Offset: 0x0005C938
			// (set) Token: 0x0600116B RID: 4459 RVA: 0x0005E740 File Offset: 0x0005C940
			public bool ForceExclusive { get; set; }

			// Token: 0x0600116C RID: 4460 RVA: 0x0005E74C File Offset: 0x0005C94C
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("PresetLoadOptions");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x0600116D RID: 4461 RVA: 0x0005E798 File Offset: 0x0005C998
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				builder.Append("ApplyModStatus = ");
				builder.Append(this.ApplyModStatus.ToString());
				builder.Append(", ApplyModVersions = ");
				builder.Append(this.ApplyModVersions.ToString());
				builder.Append(", ForceExclusive = ");
				builder.Append(this.ForceExclusive.ToString());
				return true;
			}

			// Token: 0x0600116E RID: 4462 RVA: 0x0005E81B File Offset: 0x0005CA1B
			[CompilerGenerated]
			public static bool operator !=(MainWindow.PresetLoadOptions left, MainWindow.PresetLoadOptions right)
			{
				return !(left == right);
			}

			// Token: 0x0600116F RID: 4463 RVA: 0x0005E827 File Offset: 0x0005CA27
			[CompilerGenerated]
			public static bool operator ==(MainWindow.PresetLoadOptions left, MainWindow.PresetLoadOptions right)
			{
				return left.Equals(right);
			}

			// Token: 0x06001170 RID: 4464 RVA: 0x0005E831 File Offset: 0x0005CA31
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (EqualityComparer<bool>.Default.GetHashCode(this.<ApplyModStatus>k__BackingField) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<ApplyModVersions>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<ForceExclusive>k__BackingField);
			}

			// Token: 0x06001171 RID: 4465 RVA: 0x0005E871 File Offset: 0x0005CA71
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return obj is MainWindow.PresetLoadOptions && this.Equals((MainWindow.PresetLoadOptions)obj);
			}

			// Token: 0x06001172 RID: 4466 RVA: 0x0005E88C File Offset: 0x0005CA8C
			[CompilerGenerated]
			public bool Equals(MainWindow.PresetLoadOptions other)
			{
				return EqualityComparer<bool>.Default.Equals(this.<ApplyModStatus>k__BackingField, other.<ApplyModStatus>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<ApplyModVersions>k__BackingField, other.<ApplyModVersions>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<ForceExclusive>k__BackingField, other.<ForceExclusive>k__BackingField);
			}

			// Token: 0x06001173 RID: 4467 RVA: 0x0005E8E1 File Offset: 0x0005CAE1
			[CompilerGenerated]
			public void Deconstruct(out bool ApplyModStatus, out bool ApplyModVersions, out bool ForceExclusive)
			{
				ApplyModStatus = this.ApplyModStatus;
				ApplyModVersions = this.ApplyModVersions;
				ForceExclusive = this.ForceExclusive;
			}
		}

		// Token: 0x0200011B RID: 283
		[NullableContext(0)]
		private enum ModlistLoadMode
		{
			// Token: 0x04000888 RID: 2184
			Replace,
			// Token: 0x04000889 RID: 2185
			Add
		}

		// Token: 0x0200011C RID: 284
		[Nullable(0)]
		private readonly struct ManagerDeletionResult : IEquatable<MainWindow.ManagerDeletionResult>
		{
			// Token: 0x06001174 RID: 4468 RVA: 0x0005E8FB File Offset: 0x0005CAFB
			public ManagerDeletionResult(List<string> DeletedPaths, List<string> FailedPaths)
			{
				this.DeletedPaths = DeletedPaths;
				this.FailedPaths = FailedPaths;
			}

			// Token: 0x17000424 RID: 1060
			// (get) Token: 0x06001175 RID: 4469 RVA: 0x0005E90B File Offset: 0x0005CB0B
			// (set) Token: 0x06001176 RID: 4470 RVA: 0x0005E913 File Offset: 0x0005CB13
			public List<string> DeletedPaths { get; set; }

			// Token: 0x17000425 RID: 1061
			// (get) Token: 0x06001177 RID: 4471 RVA: 0x0005E91C File Offset: 0x0005CB1C
			// (set) Token: 0x06001178 RID: 4472 RVA: 0x0005E924 File Offset: 0x0005CB24
			public List<string> FailedPaths { get; set; }

			// Token: 0x06001179 RID: 4473 RVA: 0x0005E930 File Offset: 0x0005CB30
			[NullableContext(0)]
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("ManagerDeletionResult");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x0600117A RID: 4474 RVA: 0x0005E97C File Offset: 0x0005CB7C
			[NullableContext(0)]
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				builder.Append("DeletedPaths = ");
				builder.Append(this.DeletedPaths);
				builder.Append(", FailedPaths = ");
				builder.Append(this.FailedPaths);
				return true;
			}

			// Token: 0x0600117B RID: 4475 RVA: 0x0005E9B1 File Offset: 0x0005CBB1
			[CompilerGenerated]
			public static bool operator !=(MainWindow.ManagerDeletionResult left, MainWindow.ManagerDeletionResult right)
			{
				return !(left == right);
			}

			// Token: 0x0600117C RID: 4476 RVA: 0x0005E9BD File Offset: 0x0005CBBD
			[CompilerGenerated]
			public static bool operator ==(MainWindow.ManagerDeletionResult left, MainWindow.ManagerDeletionResult right)
			{
				return left.Equals(right);
			}

			// Token: 0x0600117D RID: 4477 RVA: 0x0005E9C7 File Offset: 0x0005CBC7
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return EqualityComparer<List<string>>.Default.GetHashCode(this.<DeletedPaths>k__BackingField) * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(this.<FailedPaths>k__BackingField);
			}

			// Token: 0x0600117E RID: 4478 RVA: 0x0005E9F0 File Offset: 0x0005CBF0
			[NullableContext(0)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return obj is MainWindow.ManagerDeletionResult && this.Equals((MainWindow.ManagerDeletionResult)obj);
			}

			// Token: 0x0600117F RID: 4479 RVA: 0x0005EA08 File Offset: 0x0005CC08
			[CompilerGenerated]
			public bool Equals(MainWindow.ManagerDeletionResult other)
			{
				return EqualityComparer<List<string>>.Default.Equals(this.<DeletedPaths>k__BackingField, other.<DeletedPaths>k__BackingField) && EqualityComparer<List<string>>.Default.Equals(this.<FailedPaths>k__BackingField, other.<FailedPaths>k__BackingField);
			}

			// Token: 0x06001180 RID: 4480 RVA: 0x0005EA3A File Offset: 0x0005CC3A
			[CompilerGenerated]
			public void Deconstruct(out List<string> DeletedPaths, out List<string> FailedPaths)
			{
				DeletedPaths = this.DeletedPaths;
				FailedPaths = this.FailedPaths;
			}
		}

		// Token: 0x0200011D RID: 285
		[Nullable(0)]
		private readonly struct InstalledModLogIdentifier : IEquatable<MainWindow.InstalledModLogIdentifier>
		{
			// Token: 0x06001181 RID: 4481 RVA: 0x0005EA4C File Offset: 0x0005CC4C
			public InstalledModLogIdentifier(string SearchValue, string DisplayLabel)
			{
				this.SearchValue = SearchValue;
				this.DisplayLabel = DisplayLabel;
			}

			// Token: 0x17000426 RID: 1062
			// (get) Token: 0x06001182 RID: 4482 RVA: 0x0005EA5C File Offset: 0x0005CC5C
			// (set) Token: 0x06001183 RID: 4483 RVA: 0x0005EA64 File Offset: 0x0005CC64
			public string SearchValue { get; set; }

			// Token: 0x17000427 RID: 1063
			// (get) Token: 0x06001184 RID: 4484 RVA: 0x0005EA6D File Offset: 0x0005CC6D
			// (set) Token: 0x06001185 RID: 4485 RVA: 0x0005EA75 File Offset: 0x0005CC75
			public string DisplayLabel { get; set; }

			// Token: 0x06001186 RID: 4486 RVA: 0x0005EA80 File Offset: 0x0005CC80
			[NullableContext(0)]
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("InstalledModLogIdentifier");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06001187 RID: 4487 RVA: 0x0005EACC File Offset: 0x0005CCCC
			[NullableContext(0)]
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				builder.Append("SearchValue = ");
				builder.Append(this.SearchValue);
				builder.Append(", DisplayLabel = ");
				builder.Append(this.DisplayLabel);
				return true;
			}

			// Token: 0x06001188 RID: 4488 RVA: 0x0005EB01 File Offset: 0x0005CD01
			[CompilerGenerated]
			public static bool operator !=(MainWindow.InstalledModLogIdentifier left, MainWindow.InstalledModLogIdentifier right)
			{
				return !(left == right);
			}

			// Token: 0x06001189 RID: 4489 RVA: 0x0005EB0D File Offset: 0x0005CD0D
			[CompilerGenerated]
			public static bool operator ==(MainWindow.InstalledModLogIdentifier left, MainWindow.InstalledModLogIdentifier right)
			{
				return left.Equals(right);
			}

			// Token: 0x0600118A RID: 4490 RVA: 0x0005EB17 File Offset: 0x0005CD17
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return EqualityComparer<string>.Default.GetHashCode(this.<SearchValue>k__BackingField) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<DisplayLabel>k__BackingField);
			}

			// Token: 0x0600118B RID: 4491 RVA: 0x0005EB40 File Offset: 0x0005CD40
			[NullableContext(0)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return obj is MainWindow.InstalledModLogIdentifier && this.Equals((MainWindow.InstalledModLogIdentifier)obj);
			}

			// Token: 0x0600118C RID: 4492 RVA: 0x0005EB58 File Offset: 0x0005CD58
			[CompilerGenerated]
			public bool Equals(MainWindow.InstalledModLogIdentifier other)
			{
				return EqualityComparer<string>.Default.Equals(this.<SearchValue>k__BackingField, other.<SearchValue>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<DisplayLabel>k__BackingField, other.<DisplayLabel>k__BackingField);
			}

			// Token: 0x0600118D RID: 4493 RVA: 0x0005EB8A File Offset: 0x0005CD8A
			[CompilerGenerated]
			public void Deconstruct(out string SearchValue, out string DisplayLabel)
			{
				SearchValue = this.SearchValue;
				DisplayLabel = this.DisplayLabel;
			}
		}

		// Token: 0x0200011E RID: 286
		[Nullable(0)]
		private sealed class ModUsagePromptData
		{
			// Token: 0x0600118E RID: 4494 RVA: 0x0005EB9C File Offset: 0x0005CD9C
			public ModUsagePromptData(IReadOnlyList<ModUsageVoteCandidateViewModel> candidates, IReadOnlyList<ModUsageTrackingKey> candidateKeys, int skippedCount)
			{
				this.Candidates = (candidates ?? Array.Empty<ModUsageVoteCandidateViewModel>());
				this.CandidateKeys = (candidateKeys ?? Array.Empty<ModUsageTrackingKey>());
				this.SkippedCount = ((skippedCount < 0) ? 0 : skippedCount);
			}

			// Token: 0x17000428 RID: 1064
			// (get) Token: 0x0600118F RID: 4495 RVA: 0x0005EBD2 File Offset: 0x0005CDD2
			public IReadOnlyList<ModUsageVoteCandidateViewModel> Candidates { get; }

			// Token: 0x17000429 RID: 1065
			// (get) Token: 0x06001190 RID: 4496 RVA: 0x0005EBDA File Offset: 0x0005CDDA
			public IReadOnlyList<ModUsageTrackingKey> CandidateKeys { get; }

			// Token: 0x1700042A RID: 1066
			// (get) Token: 0x06001191 RID: 4497 RVA: 0x0005EBE2 File Offset: 0x0005CDE2
			public int SkippedCount { get; }
		}

		// Token: 0x0200011F RID: 287
		[NullableContext(0)]
		private enum InstalledModsColumn
		{
			// Token: 0x04000892 RID: 2194
			Active,
			// Token: 0x04000893 RID: 2195
			Icon,
			// Token: 0x04000894 RID: 2196
			Name,
			// Token: 0x04000895 RID: 2197
			Installed,
			// Token: 0x04000896 RID: 2198
			Version,
			// Token: 0x04000897 RID: 2199
			LatestVersion,
			// Token: 0x04000898 RID: 2200
			DateModified,
			// Token: 0x04000899 RID: 2201
			Downloads,
			// Token: 0x0400089A RID: 2202
			Authors,
			// Token: 0x0400089B RID: 2203
			Tags,
			// Token: 0x0400089C RID: 2204
			UserReports,
			// Token: 0x0400089D RID: 2205
			Status,
			// Token: 0x0400089E RID: 2206
			Side
		}

		// Token: 0x02000120 RID: 288
		// (Invoke) Token: 0x06001193 RID: 4499
		[NullableContext(0)]
		private delegate bool PathValidator(string path, out string normalizedPath, out string errorMessage);

		// Token: 0x02000121 RID: 289
		[NullableContext(2)]
		[Nullable(0)]
		private readonly struct CompatibilityEvaluation
		{
			// Token: 0x06001196 RID: 4502 RVA: 0x0005EBEA File Offset: 0x0005CDEA
			private CompatibilityEvaluation(bool isCompatible, bool isUnknown, string message)
			{
				this.IsCompatible = isCompatible;
				this.IsUnknown = isUnknown;
				this.Message = message;
			}

			// Token: 0x1700042B RID: 1067
			// (get) Token: 0x06001197 RID: 4503 RVA: 0x0005EC01 File Offset: 0x0005CE01
			public bool IsCompatible { get; }

			// Token: 0x1700042C RID: 1068
			// (get) Token: 0x06001198 RID: 4504 RVA: 0x0005EC09 File Offset: 0x0005CE09
			public bool IsUnknown { get; }

			// Token: 0x1700042D RID: 1069
			// (get) Token: 0x06001199 RID: 4505 RVA: 0x0005EC11 File Offset: 0x0005CE11
			public string Message { get; }

			// Token: 0x1700042E RID: 1070
			// (get) Token: 0x0600119A RID: 4506 RVA: 0x0005EC19 File Offset: 0x0005CE19
			public static MainWindow.CompatibilityEvaluation Compatible { get; } = new MainWindow.CompatibilityEvaluation(true, false, null);

			// Token: 0x0600119B RID: 4507 RVA: 0x0005EC20 File Offset: 0x0005CE20
			[NullableContext(1)]
			public static MainWindow.CompatibilityEvaluation Incompatible(string message)
			{
				return new MainWindow.CompatibilityEvaluation(false, false, message);
			}

			// Token: 0x0600119C RID: 4508 RVA: 0x0005EC2A File Offset: 0x0005CE2A
			[NullableContext(1)]
			public static MainWindow.CompatibilityEvaluation Unknown(string message)
			{
				return new MainWindow.CompatibilityEvaluation(false, true, message);
			}
		}

		// Token: 0x02000122 RID: 290
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class ModlistMetadata
		{
			// Token: 0x0600119E RID: 4510 RVA: 0x0005EC44 File Offset: 0x0005CE44
			public ModlistMetadata(string name, string description, string version, string uploader, [Nullable(1)] IReadOnlyList<string> mods, string gameVersion)
			{
				this.Name = (string.IsNullOrWhiteSpace(name) ? null : name.Trim());
				this.Description = (string.IsNullOrWhiteSpace(description) ? null : description.Trim());
				this.Version = (string.IsNullOrWhiteSpace(version) ? null : version.Trim());
				this.Uploader = (string.IsNullOrWhiteSpace(uploader) ? null : uploader.Trim());
				this.Mods = (mods ?? Array.Empty<string>());
				this.GameVersion = (string.IsNullOrWhiteSpace(gameVersion) ? null : gameVersion.Trim());
			}

			// Token: 0x1700042F RID: 1071
			// (get) Token: 0x0600119F RID: 4511 RVA: 0x0005ECDF File Offset: 0x0005CEDF
			public string Name { get; }

			// Token: 0x17000430 RID: 1072
			// (get) Token: 0x060011A0 RID: 4512 RVA: 0x0005ECE7 File Offset: 0x0005CEE7
			public string Description { get; }

			// Token: 0x17000431 RID: 1073
			// (get) Token: 0x060011A1 RID: 4513 RVA: 0x0005ECEF File Offset: 0x0005CEEF
			public string Version { get; }

			// Token: 0x17000432 RID: 1074
			// (get) Token: 0x060011A2 RID: 4514 RVA: 0x0005ECF7 File Offset: 0x0005CEF7
			public string Uploader { get; }

			// Token: 0x17000433 RID: 1075
			// (get) Token: 0x060011A3 RID: 4515 RVA: 0x0005ECFF File Offset: 0x0005CEFF
			[Nullable(1)]
			public IReadOnlyList<string> Mods { [NullableContext(1)] get; }

			// Token: 0x17000434 RID: 1076
			// (get) Token: 0x060011A4 RID: 4516 RVA: 0x0005ED07 File Offset: 0x0005CF07
			public string GameVersion { get; }

			// Token: 0x040008A3 RID: 2211
			[Nullable(1)]
			public static readonly MainWindow.ModlistMetadata Empty = new MainWindow.ModlistMetadata(null, null, null, null, Array.Empty<string>(), null);
		}

		// Token: 0x02000123 RID: 291
		[NullableContext(2)]
		[Nullable(0)]
		private readonly struct PresetModInstallResult : IEquatable<MainWindow.PresetModInstallResult>
		{
			// Token: 0x060011A6 RID: 4518 RVA: 0x0005ED25 File Offset: 0x0005CF25
			public PresetModInstallResult(bool Success, bool ModMissing, bool VersionMissing, string ErrorMessage)
			{
				this.Success = Success;
				this.ModMissing = ModMissing;
				this.VersionMissing = VersionMissing;
				this.ErrorMessage = ErrorMessage;
			}

			// Token: 0x17000435 RID: 1077
			// (get) Token: 0x060011A7 RID: 4519 RVA: 0x0005ED44 File Offset: 0x0005CF44
			// (set) Token: 0x060011A8 RID: 4520 RVA: 0x0005ED4C File Offset: 0x0005CF4C
			public bool Success { get; set; }

			// Token: 0x17000436 RID: 1078
			// (get) Token: 0x060011A9 RID: 4521 RVA: 0x0005ED55 File Offset: 0x0005CF55
			// (set) Token: 0x060011AA RID: 4522 RVA: 0x0005ED5D File Offset: 0x0005CF5D
			public bool ModMissing { get; set; }

			// Token: 0x17000437 RID: 1079
			// (get) Token: 0x060011AB RID: 4523 RVA: 0x0005ED66 File Offset: 0x0005CF66
			// (set) Token: 0x060011AC RID: 4524 RVA: 0x0005ED6E File Offset: 0x0005CF6E
			public bool VersionMissing { get; set; }

			// Token: 0x17000438 RID: 1080
			// (get) Token: 0x060011AD RID: 4525 RVA: 0x0005ED77 File Offset: 0x0005CF77
			// (set) Token: 0x060011AE RID: 4526 RVA: 0x0005ED7F File Offset: 0x0005CF7F
			public string ErrorMessage { get; set; }

			// Token: 0x060011AF RID: 4527 RVA: 0x0005ED88 File Offset: 0x0005CF88
			[NullableContext(0)]
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("PresetModInstallResult");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x060011B0 RID: 4528 RVA: 0x0005EDD4 File Offset: 0x0005CFD4
			[NullableContext(0)]
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				builder.Append("Success = ");
				builder.Append(this.Success.ToString());
				builder.Append(", ModMissing = ");
				builder.Append(this.ModMissing.ToString());
				builder.Append(", VersionMissing = ");
				builder.Append(this.VersionMissing.ToString());
				builder.Append(", ErrorMessage = ");
				builder.Append(this.ErrorMessage);
				return true;
			}

			// Token: 0x060011B1 RID: 4529 RVA: 0x0005EE70 File Offset: 0x0005D070
			[CompilerGenerated]
			public static bool operator !=(MainWindow.PresetModInstallResult left, MainWindow.PresetModInstallResult right)
			{
				return !(left == right);
			}

			// Token: 0x060011B2 RID: 4530 RVA: 0x0005EE7C File Offset: 0x0005D07C
			[CompilerGenerated]
			public static bool operator ==(MainWindow.PresetModInstallResult left, MainWindow.PresetModInstallResult right)
			{
				return left.Equals(right);
			}

			// Token: 0x060011B3 RID: 4531 RVA: 0x0005EE88 File Offset: 0x0005D088
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((EqualityComparer<bool>.Default.GetHashCode(this.<Success>k__BackingField) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<ModMissing>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<VersionMissing>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ErrorMessage>k__BackingField);
			}

			// Token: 0x060011B4 RID: 4532 RVA: 0x0005EEEA File Offset: 0x0005D0EA
			[NullableContext(0)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return obj is MainWindow.PresetModInstallResult && this.Equals((MainWindow.PresetModInstallResult)obj);
			}

			// Token: 0x060011B5 RID: 4533 RVA: 0x0005EF04 File Offset: 0x0005D104
			[CompilerGenerated]
			public bool Equals(MainWindow.PresetModInstallResult other)
			{
				return EqualityComparer<bool>.Default.Equals(this.<Success>k__BackingField, other.<Success>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<ModMissing>k__BackingField, other.<ModMissing>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<VersionMissing>k__BackingField, other.<VersionMissing>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ErrorMessage>k__BackingField, other.<ErrorMessage>k__BackingField);
			}

			// Token: 0x060011B6 RID: 4534 RVA: 0x0005EF71 File Offset: 0x0005D171
			[CompilerGenerated]
			public void Deconstruct(out bool Success, out bool ModMissing, out bool VersionMissing, out string ErrorMessage)
			{
				Success = this.Success;
				ModMissing = this.ModMissing;
				VersionMissing = this.VersionMissing;
				ErrorMessage = this.ErrorMessage;
			}
		}

		// Token: 0x02000124 RID: 292
		[NullableContext(2)]
		[Nullable(0)]
		private readonly struct ModUpdateOperationResult : IEquatable<MainWindow.ModUpdateOperationResult>
		{
			// Token: 0x060011B7 RID: 4535 RVA: 0x0005EF94 File Offset: 0x0005D194
			public ModUpdateOperationResult([Nullable(1)] ModListItemViewModel Mod, bool Success, bool Skipped, [Nullable(1)] string Message, string OldVersion, string NewVersion, string ChangelogSummary)
			{
				this.Mod = Mod;
				this.Success = Success;
				this.Skipped = Skipped;
				this.Message = Message;
				this.OldVersion = OldVersion;
				this.NewVersion = NewVersion;
				this.ChangelogSummary = ChangelogSummary;
			}

			// Token: 0x17000439 RID: 1081
			// (get) Token: 0x060011B8 RID: 4536 RVA: 0x0005EFCB File Offset: 0x0005D1CB
			// (set) Token: 0x060011B9 RID: 4537 RVA: 0x0005EFD3 File Offset: 0x0005D1D3
			[Nullable(1)]
			public ModListItemViewModel Mod { [NullableContext(1)] get; [NullableContext(1)] set; }

			// Token: 0x1700043A RID: 1082
			// (get) Token: 0x060011BA RID: 4538 RVA: 0x0005EFDC File Offset: 0x0005D1DC
			// (set) Token: 0x060011BB RID: 4539 RVA: 0x0005EFE4 File Offset: 0x0005D1E4
			public bool Success { get; set; }

			// Token: 0x1700043B RID: 1083
			// (get) Token: 0x060011BC RID: 4540 RVA: 0x0005EFED File Offset: 0x0005D1ED
			// (set) Token: 0x060011BD RID: 4541 RVA: 0x0005EFF5 File Offset: 0x0005D1F5
			public bool Skipped { get; set; }

			// Token: 0x1700043C RID: 1084
			// (get) Token: 0x060011BE RID: 4542 RVA: 0x0005EFFE File Offset: 0x0005D1FE
			// (set) Token: 0x060011BF RID: 4543 RVA: 0x0005F006 File Offset: 0x0005D206
			[Nullable(1)]
			public string Message { [NullableContext(1)] get; [NullableContext(1)] set; }

			// Token: 0x1700043D RID: 1085
			// (get) Token: 0x060011C0 RID: 4544 RVA: 0x0005F00F File Offset: 0x0005D20F
			// (set) Token: 0x060011C1 RID: 4545 RVA: 0x0005F017 File Offset: 0x0005D217
			public string OldVersion { get; set; }

			// Token: 0x1700043E RID: 1086
			// (get) Token: 0x060011C2 RID: 4546 RVA: 0x0005F020 File Offset: 0x0005D220
			// (set) Token: 0x060011C3 RID: 4547 RVA: 0x0005F028 File Offset: 0x0005D228
			public string NewVersion { get; set; }

			// Token: 0x1700043F RID: 1087
			// (get) Token: 0x060011C4 RID: 4548 RVA: 0x0005F031 File Offset: 0x0005D231
			// (set) Token: 0x060011C5 RID: 4549 RVA: 0x0005F039 File Offset: 0x0005D239
			public string ChangelogSummary { get; set; }

			// Token: 0x060011C6 RID: 4550 RVA: 0x0005F042 File Offset: 0x0005D242
			[NullableContext(1)]
			public static MainWindow.ModUpdateOperationResult SuccessResult(ModListItemViewModel mod, string newVersion, [Nullable(2)] string previousVersion, [Nullable(2)] string changelogSummary)
			{
				return new MainWindow.ModUpdateOperationResult(mod, true, false, "Updated to " + newVersion + ".", previousVersion, newVersion, changelogSummary);
			}

			// Token: 0x060011C7 RID: 4551 RVA: 0x0005F05F File Offset: 0x0005D25F
			[NullableContext(1)]
			public static MainWindow.ModUpdateOperationResult Failure(ModListItemViewModel mod, string message)
			{
				return new MainWindow.ModUpdateOperationResult(mod, false, false, message, mod.Version, null, null);
			}

			// Token: 0x060011C8 RID: 4552 RVA: 0x0005F072 File Offset: 0x0005D272
			[NullableContext(1)]
			public static MainWindow.ModUpdateOperationResult SkippedResult(ModListItemViewModel mod, string message)
			{
				return new MainWindow.ModUpdateOperationResult(mod, false, true, message, mod.Version, null, null);
			}

			// Token: 0x060011C9 RID: 4553 RVA: 0x0005F088 File Offset: 0x0005D288
			[NullableContext(0)]
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("ModUpdateOperationResult");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x060011CA RID: 4554 RVA: 0x0005F0D4 File Offset: 0x0005D2D4
			[NullableContext(0)]
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				builder.Append("Mod = ");
				builder.Append(this.Mod);
				builder.Append(", Success = ");
				builder.Append(this.Success.ToString());
				builder.Append(", Skipped = ");
				builder.Append(this.Skipped.ToString());
				builder.Append(", Message = ");
				builder.Append(this.Message);
				builder.Append(", OldVersion = ");
				builder.Append(this.OldVersion);
				builder.Append(", NewVersion = ");
				builder.Append(this.NewVersion);
				builder.Append(", ChangelogSummary = ");
				builder.Append(this.ChangelogSummary);
				return true;
			}

			// Token: 0x060011CB RID: 4555 RVA: 0x0005F1AD File Offset: 0x0005D3AD
			[CompilerGenerated]
			public static bool operator !=(MainWindow.ModUpdateOperationResult left, MainWindow.ModUpdateOperationResult right)
			{
				return !(left == right);
			}

			// Token: 0x060011CC RID: 4556 RVA: 0x0005F1B9 File Offset: 0x0005D3B9
			[CompilerGenerated]
			public static bool operator ==(MainWindow.ModUpdateOperationResult left, MainWindow.ModUpdateOperationResult right)
			{
				return left.Equals(right);
			}

			// Token: 0x060011CD RID: 4557 RVA: 0x0005F1C4 File Offset: 0x0005D3C4
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (((((EqualityComparer<ModListItemViewModel>.Default.GetHashCode(this.<Mod>k__BackingField) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<Success>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<Skipped>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Message>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<OldVersion>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<NewVersion>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ChangelogSummary>k__BackingField);
			}

			// Token: 0x060011CE RID: 4558 RVA: 0x0005F26B File Offset: 0x0005D46B
			[NullableContext(0)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return obj is MainWindow.ModUpdateOperationResult && this.Equals((MainWindow.ModUpdateOperationResult)obj);
			}

			// Token: 0x060011CF RID: 4559 RVA: 0x0005F284 File Offset: 0x0005D484
			[CompilerGenerated]
			public bool Equals(MainWindow.ModUpdateOperationResult other)
			{
				return EqualityComparer<ModListItemViewModel>.Default.Equals(this.<Mod>k__BackingField, other.<Mod>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<Success>k__BackingField, other.<Success>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<Skipped>k__BackingField, other.<Skipped>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Message>k__BackingField, other.<Message>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<OldVersion>k__BackingField, other.<OldVersion>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<NewVersion>k__BackingField, other.<NewVersion>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ChangelogSummary>k__BackingField, other.<ChangelogSummary>k__BackingField);
			}

			// Token: 0x060011D0 RID: 4560 RVA: 0x0005F33C File Offset: 0x0005D53C
			[CompilerGenerated]
			public void Deconstruct([Nullable(1)] out ModListItemViewModel Mod, out bool Success, out bool Skipped, [Nullable(1)] out string Message, out string OldVersion, out string NewVersion, out string ChangelogSummary)
			{
				Mod = this.Mod;
				Success = this.Success;
				Skipped = this.Skipped;
				Message = this.Message;
				OldVersion = this.OldVersion;
				NewVersion = this.NewVersion;
				ChangelogSummary = this.ChangelogSummary;
			}
		}

		// Token: 0x0200012D RID: 301
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040008EA RID: 2282
			[Nullable(0)]
			public static MainWindow.PathValidator <0>__TryValidateDataDirectory;

			// Token: 0x040008EB RID: 2283
			[Nullable(0)]
			public static MainWindow.PathValidator <1>__TryValidateGameDirectory;

			// Token: 0x040008EC RID: 2284
			[Nullable(0)]
			public static Func<string, bool> <2>__Exists;

			// Token: 0x040008ED RID: 2285
			[Nullable(0)]
			public static Func<string, DateTime> <3>__GetLastWriteTimeUtc;

			// Token: 0x040008EE RID: 2286
			[Nullable(0)]
			public static Func<string, bool> <4>__IsAppStartedBackup;

			// Token: 0x040008EF RID: 2287
			[Nullable(0)]
			public static Func<SerializableModConfiguration, string> <5>__BuildConfigKey;
		}
	}
}
