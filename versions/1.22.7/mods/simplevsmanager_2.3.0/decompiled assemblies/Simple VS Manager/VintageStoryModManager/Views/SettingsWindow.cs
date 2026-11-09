using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using ModernWpf.Controls;
using VintageStoryModManager.Services;

namespace VintageStoryModManager.Views
{
	// Token: 0x0200001C RID: 28
	public class SettingsWindow : Window, IComponentConnector, IStyleConnector
	{
		// Token: 0x060003F7 RID: 1015 RVA: 0x0001F2C8 File Offset: 0x0001D4C8
		[NullableContext(1)]
		public SettingsWindow(MainWindow mainWindow)
		{
			if (mainWindow == null)
			{
				throw new ArgumentNullException("mainWindow");
			}
			this._mainWindow = mainWindow;
			this.InitializeComponent();
			base.DataContext = mainWindow.DataContext;
			this.InitializeState();
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001F314 File Offset: 0x0001D514
		private void InitializeState()
		{
			this._isSyncing = true;
			UserConfigurationService userConfiguration = this._mainWindow.UserConfiguration;
			this.Register(this.SwDisableAutoRefresh, this._mainWindow.DisableAutoRefreshMenuItem, userConfiguration.DisableAutoRefresh);
			this.Register(this.SwFasterThumbnails, this._mainWindow.UseFasterThumbnailsMenuItem, userConfiguration.UseFasterThumbnails);
			this.Register(this.SwCacheModDb, this._mainWindow.CacheModDatabaseResultsMenuItem, userConfiguration.CacheModDatabaseResults);
			this.Register(this.SwCacheAllVersions, this._mainWindow.CacheAllVersionsMenuItem, userConfiguration.CacheAllVersionsLocally);
			this.Register(this.SwDisableHover, this._mainWindow.DisableHoverEffectsMenuItem, userConfiguration.DisableHoverEffects);
			this.Register(this.SwRequireExactVs, this._mainWindow.RequireExactVsVersionMenuItem, userConfiguration.RequireExactVsVersionMatch);
			this.Register(this.SwAutoDataBackup, this._mainWindow.AutomaticDataBackupsMenuItem, userConfiguration.AutomaticDataBackupsEnabled);
			this.Register(this.SwCloseManagerAfterGameLaunch, this._mainWindow.CloseManagerAfterGameLaunchMenuItem, userConfiguration.CloseManagerAfterGameLaunch);
			this.Register(this.SwAlwaysClear, this._mainWindow.AlwaysClearModlistsMenuItem, userConfiguration.ModlistAutoLoadBehavior == ModlistAutoLoadBehavior.Replace);
			this.Register(this.SwAlwaysAdd, this._mainWindow.AlwaysAddModlistsMenuItem, userConfiguration.ModlistAutoLoadBehavior == ModlistAutoLoadBehavior.Add);
			this.Register(this.SwDisableInternet, this._mainWindow.DisableInternetAccessMenuItem, userConfiguration.DisableInternetAccess);
			this.Register(this.SwEnableServerOptions, this._mainWindow.EnableServerOptionsMenuItem, userConfiguration.EnableServerOptions);
			this.Register(this.SwLogUpdate, this._mainWindow.LogModUpdateMenuItem, userConfiguration.LogModUpdates);
			this.Register(this.SwLogInstall, this._mainWindow.LogModInstallMenuItem, userConfiguration.LogModInstalls);
			this.Register(this.SwLogDeletion, this._mainWindow.LogModDeletionMenuItem, userConfiguration.LogModDeletions);
			this.Register(this.SwLogAppLifecycle, this._mainWindow.LogAppLifecycleMenuItem, userConfiguration.LogAppLaunchAndExit);
			this.Register(this.SwLogErrors, this._mainWindow.LogErrorsAndExceptionsMenuItem, userConfiguration.LogErrorsAndExceptions);
			switch (userConfiguration.ColorTheme)
			{
			case ColorTheme.VintageStory:
				this.RbThemeVintage.IsChecked = new bool?(true);
				break;
			case ColorTheme.Dark:
				this.RbThemeDark.IsChecked = new bool?(true);
				break;
			case ColorTheme.Light:
				this.RbThemeLight.IsChecked = new bool?(true);
				break;
			}
			TextBlock versionTextBlock = this.VersionTextBlock;
			MenuItem managerVersionMenuItem = this._mainWindow.ManagerVersionMenuItem;
			string text;
			if (managerVersionMenuItem == null)
			{
				text = null;
			}
			else
			{
				object header = managerVersionMenuItem.Header;
				text = ((header != null) ? header.ToString() : null);
			}
			versionTextBlock.Text = (text ?? "Version: --");
			this._isSyncing = false;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0001F5B7 File Offset: 0x0001D7B7
		[NullableContext(1)]
		private void Register(ToggleSwitch toggle, MenuItem menuItem, bool value)
		{
			this._toggleMap[toggle] = menuItem;
			toggle.IsOn = value;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0001F5D0 File Offset: 0x0001D7D0
		[NullableContext(1)]
		private void SettingToggle_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			ToggleSwitch toggleSwitch = sender as ToggleSwitch;
			if (toggleSwitch == null || !toggleSwitch.IsEnabled)
			{
				return;
			}
			e.Handled = true;
			toggleSwitch.Focus();
			toggleSwitch.IsOn = !toggleSwitch.IsOn;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0001F610 File Offset: 0x0001D810
		[NullableContext(1)]
		private void Toggle_OnToggled(object sender, RoutedEventArgs e)
		{
			if (this._isSyncing)
			{
				return;
			}
			ToggleSwitch toggleSwitch = sender as ToggleSwitch;
			if (toggleSwitch == null)
			{
				return;
			}
			MenuItem menuItem;
			if (!this._toggleMap.TryGetValue(toggleSwitch, ref menuItem))
			{
				return;
			}
			menuItem.IsChecked = toggleSwitch.IsOn;
			SettingsWindow.InvokeMenuItem(menuItem);
			this.ResyncToggles();
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0001F65C File Offset: 0x0001D85C
		private void ResyncToggles()
		{
			this._isSyncing = true;
			foreach (KeyValuePair<ToggleSwitch, MenuItem> keyValuePair in this._toggleMap)
			{
				keyValuePair.Key.IsOn = keyValuePair.Value.IsChecked;
			}
			this._isSyncing = false;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0001F6D0 File Offset: 0x0001D8D0
		[NullableContext(1)]
		private void ThemeRadio_OnChecked(object sender, RoutedEventArgs e)
		{
			if (this._isSyncing)
			{
				return;
			}
			RadioButton radioButton = sender as RadioButton;
			if (radioButton == null)
			{
				return;
			}
			string name = radioButton.Name;
			MenuItem menuItem;
			if (!(name == "RbThemeVintage"))
			{
				if (!(name == "RbThemeDark"))
				{
					if (!(name == "RbThemeLight"))
					{
						menuItem = null;
					}
					else
					{
						menuItem = this._mainWindow.LightThemeMenuItem;
					}
				}
				else
				{
					menuItem = this._mainWindow.DarkThemeMenuItem;
				}
			}
			else
			{
				menuItem = this._mainWindow.VintageStoryThemeMenuItem;
			}
			MenuItem menuItem2 = menuItem;
			if (menuItem2 == null)
			{
				return;
			}
			menuItem2.IsChecked = true;
			SettingsWindow.InvokeMenuItem(menuItem2);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0001F760 File Offset: 0x0001D960
		[NullableContext(1)]
		private void BtnEditTheme_OnClick(object sender, RoutedEventArgs e)
		{
			SettingsWindow.InvokeMenuItem(this._mainWindow.EditThemeMenuItem);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0001F772 File Offset: 0x0001D972
		[NullableContext(1)]
		private void BtnSetDataFolder_OnClick(object sender, RoutedEventArgs e)
		{
			SettingsWindow.InvokeMenuItem(this._mainWindow.SelectDataFolderMenuItem);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0001F784 File Offset: 0x0001D984
		[NullableContext(1)]
		private void BtnSetGameFolder_OnClick(object sender, RoutedEventArgs e)
		{
			SettingsWindow.InvokeMenuItem(this._mainWindow.SelectGameFolderMenuItem);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0001F796 File Offset: 0x0001D996
		[NullableContext(1)]
		private void BtnChangeManagerFolder_OnClick(object sender, RoutedEventArgs e)
		{
			SettingsWindow.InvokeMenuItem(this._mainWindow.ChangeManagerFolderMenuItem);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0001F7A8 File Offset: 0x0001D9A8
		[NullableContext(1)]
		private void BtnSetShortcut_OnClick(object sender, RoutedEventArgs e)
		{
			SettingsWindow.InvokeMenuItem(this._mainWindow.SetCustomShortcutMenuItem);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0001F7BA File Offset: 0x0001D9BA
		[NullableContext(1)]
		private void BtnMenuGuide_OnClick(object sender, RoutedEventArgs e)
		{
			SettingsWindow.InvokeMenuItem(this._mainWindow.GuideMenuItem);
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0001F7CC File Offset: 0x0001D9CC
		[NullableContext(1)]
		private void BtnHelp_OnClick(object sender, RoutedEventArgs e)
		{
			SettingsWindow.InvokeMenuItem(this._mainWindow.HelpMenuItem);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0001F7DE File Offset: 0x0001D9DE
		[NullableContext(1)]
		private void BtnModDbPage_OnClick(object sender, RoutedEventArgs e)
		{
			SettingsWindow.InvokeMenuItem(this._mainWindow.ManagerModDbPageMenuItem);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0001F7F0 File Offset: 0x0001D9F0
		[NullableContext(1)]
		private void BtnClearCache_OnClick(object sender, RoutedEventArgs e)
		{
			SettingsWindow.InvokeMenuItem(this._mainWindow.ClearAllCachesMenuItem);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0001F802 File Offset: 0x0001DA02
		[NullableContext(1)]
		private void BtnClose_OnClick(object sender, RoutedEventArgs e)
		{
			base.Close();
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0001F80A File Offset: 0x0001DA0A
		[NullableContext(2)]
		private static void InvokeMenuItem(MenuItem menuItem)
		{
			if (menuItem != null)
			{
				menuItem.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent, menuItem));
			}
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0001F820 File Offset: 0x0001DA20
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/settingswindow.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0001F850 File Offset: 0x0001DA50
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 2:
				this.RbThemeVintage = (RadioButton)target;
				this.RbThemeVintage.Checked += new RoutedEventHandler(this.ThemeRadio_OnChecked);
				return;
			case 3:
				this.RbThemeDark = (RadioButton)target;
				this.RbThemeDark.Checked += new RoutedEventHandler(this.ThemeRadio_OnChecked);
				return;
			case 4:
				this.RbThemeLight = (RadioButton)target;
				this.RbThemeLight.Checked += new RoutedEventHandler(this.ThemeRadio_OnChecked);
				return;
			case 5:
				this.BtnEditTheme = (Button)target;
				this.BtnEditTheme.Click += new RoutedEventHandler(this.BtnEditTheme_OnClick);
				return;
			case 6:
				this.SwCompactView = (ToggleSwitch)target;
				return;
			case 7:
				this.SwDisableHover = (ToggleSwitch)target;
				this.SwDisableHover.Toggled += new RoutedEventHandler(this.Toggle_OnToggled);
				return;
			case 8:
				this.SwDisableAutoRefresh = (ToggleSwitch)target;
				this.SwDisableAutoRefresh.Toggled += new RoutedEventHandler(this.Toggle_OnToggled);
				return;
			case 9:
				this.SwFasterThumbnails = (ToggleSwitch)target;
				this.SwFasterThumbnails.Toggled += new RoutedEventHandler(this.Toggle_OnToggled);
				return;
			case 10:
				this.SwCacheModDb = (ToggleSwitch)target;
				this.SwCacheModDb.Toggled += new RoutedEventHandler(this.Toggle_OnToggled);
				return;
			case 11:
				this.SwCacheAllVersions = (ToggleSwitch)target;
				this.SwCacheAllVersions.Toggled += new RoutedEventHandler(this.Toggle_OnToggled);
				return;
			case 12:
				this.SwRequireExactVs = (ToggleSwitch)target;
				this.SwRequireExactVs.Toggled += new RoutedEventHandler(this.Toggle_OnToggled);
				return;
			case 13:
				this.SwAutoDataBackup = (ToggleSwitch)target;
				this.SwAutoDataBackup.Toggled += new RoutedEventHandler(this.Toggle_OnToggled);
				return;
			case 14:
				this.SwCloseManagerAfterGameLaunch = (ToggleSwitch)target;
				this.SwCloseManagerAfterGameLaunch.Toggled += new RoutedEventHandler(this.Toggle_OnToggled);
				return;
			case 15:
				this.SwAlwaysClear = (ToggleSwitch)target;
				this.SwAlwaysClear.Toggled += new RoutedEventHandler(this.Toggle_OnToggled);
				return;
			case 16:
				this.SwAlwaysAdd = (ToggleSwitch)target;
				this.SwAlwaysAdd.Toggled += new RoutedEventHandler(this.Toggle_OnToggled);
				return;
			case 17:
				this.BtnSetDataFolder = (Button)target;
				this.BtnSetDataFolder.Click += new RoutedEventHandler(this.BtnSetDataFolder_OnClick);
				return;
			case 18:
				this.BtnSetGameFolder = (Button)target;
				this.BtnSetGameFolder.Click += new RoutedEventHandler(this.BtnSetGameFolder_OnClick);
				return;
			case 19:
				this.BtnChangeManagerFolder = (Button)target;
				this.BtnChangeManagerFolder.Click += new RoutedEventHandler(this.BtnChangeManagerFolder_OnClick);
				return;
			case 20:
				this.BtnSetShortcut = (Button)target;
				this.BtnSetShortcut.Click += new RoutedEventHandler(this.BtnSetShortcut_OnClick);
				return;
			case 21:
				this.SwDisableInternet = (ToggleSwitch)target;
				this.SwDisableInternet.Toggled += new RoutedEventHandler(this.Toggle_OnToggled);
				return;
			case 22:
				this.SwEnableServerOptions = (ToggleSwitch)target;
				this.SwEnableServerOptions.Toggled += new RoutedEventHandler(this.Toggle_OnToggled);
				return;
			case 23:
				this.SwLogUpdate = (ToggleSwitch)target;
				this.SwLogUpdate.Toggled += new RoutedEventHandler(this.Toggle_OnToggled);
				return;
			case 24:
				this.SwLogInstall = (ToggleSwitch)target;
				this.SwLogInstall.Toggled += new RoutedEventHandler(this.Toggle_OnToggled);
				return;
			case 25:
				this.SwLogDeletion = (ToggleSwitch)target;
				this.SwLogDeletion.Toggled += new RoutedEventHandler(this.Toggle_OnToggled);
				return;
			case 26:
				this.SwLogAppLifecycle = (ToggleSwitch)target;
				this.SwLogAppLifecycle.Toggled += new RoutedEventHandler(this.Toggle_OnToggled);
				return;
			case 27:
				this.SwLogErrors = (ToggleSwitch)target;
				this.SwLogErrors.Toggled += new RoutedEventHandler(this.Toggle_OnToggled);
				return;
			case 28:
				this.VersionTextBlock = (TextBlock)target;
				return;
			case 29:
				this.BtnMenuGuide = (Button)target;
				this.BtnMenuGuide.Click += new RoutedEventHandler(this.BtnMenuGuide_OnClick);
				return;
			case 30:
				this.BtnHelp = (Button)target;
				this.BtnHelp.Click += new RoutedEventHandler(this.BtnHelp_OnClick);
				return;
			case 31:
				this.BtnModDbPage = (Button)target;
				this.BtnModDbPage.Click += new RoutedEventHandler(this.BtnModDbPage_OnClick);
				return;
			case 32:
				this.BtnClearCache = (Button)target;
				this.BtnClearCache.Click += new RoutedEventHandler(this.BtnClearCache_OnClick);
				return;
			case 33:
				this.BtnClose = (Button)target;
				this.BtnClose.Click += new RoutedEventHandler(this.BtnClose_OnClick);
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0001FD44 File Offset: 0x0001DF44
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = UIElement.PreviewMouseLeftButtonDownEvent;
				eventSetter.Handler = new MouseButtonEventHandler(this.SettingToggle_OnPreviewMouseLeftButtonDown);
				((Style)target).Setters.Add(eventSetter);
			}
		}

		// Token: 0x040001FF RID: 511
		[Nullable(1)]
		private readonly MainWindow _mainWindow;

		// Token: 0x04000200 RID: 512
		[Nullable(1)]
		private readonly Dictionary<ToggleSwitch, MenuItem> _toggleMap = new Dictionary<ToggleSwitch, MenuItem>();

		// Token: 0x04000201 RID: 513
		private bool _isSyncing;

		// Token: 0x04000202 RID: 514
		internal RadioButton RbThemeVintage;

		// Token: 0x04000203 RID: 515
		internal RadioButton RbThemeDark;

		// Token: 0x04000204 RID: 516
		internal RadioButton RbThemeLight;

		// Token: 0x04000205 RID: 517
		internal Button BtnEditTheme;

		// Token: 0x04000206 RID: 518
		internal ToggleSwitch SwCompactView;

		// Token: 0x04000207 RID: 519
		internal ToggleSwitch SwDisableHover;

		// Token: 0x04000208 RID: 520
		internal ToggleSwitch SwDisableAutoRefresh;

		// Token: 0x04000209 RID: 521
		internal ToggleSwitch SwFasterThumbnails;

		// Token: 0x0400020A RID: 522
		internal ToggleSwitch SwCacheModDb;

		// Token: 0x0400020B RID: 523
		internal ToggleSwitch SwCacheAllVersions;

		// Token: 0x0400020C RID: 524
		internal ToggleSwitch SwRequireExactVs;

		// Token: 0x0400020D RID: 525
		internal ToggleSwitch SwAutoDataBackup;

		// Token: 0x0400020E RID: 526
		internal ToggleSwitch SwCloseManagerAfterGameLaunch;

		// Token: 0x0400020F RID: 527
		internal ToggleSwitch SwAlwaysClear;

		// Token: 0x04000210 RID: 528
		internal ToggleSwitch SwAlwaysAdd;

		// Token: 0x04000211 RID: 529
		internal Button BtnSetDataFolder;

		// Token: 0x04000212 RID: 530
		internal Button BtnSetGameFolder;

		// Token: 0x04000213 RID: 531
		internal Button BtnChangeManagerFolder;

		// Token: 0x04000214 RID: 532
		internal Button BtnSetShortcut;

		// Token: 0x04000215 RID: 533
		internal ToggleSwitch SwDisableInternet;

		// Token: 0x04000216 RID: 534
		internal ToggleSwitch SwEnableServerOptions;

		// Token: 0x04000217 RID: 535
		internal ToggleSwitch SwLogUpdate;

		// Token: 0x04000218 RID: 536
		internal ToggleSwitch SwLogInstall;

		// Token: 0x04000219 RID: 537
		internal ToggleSwitch SwLogDeletion;

		// Token: 0x0400021A RID: 538
		internal ToggleSwitch SwLogAppLifecycle;

		// Token: 0x0400021B RID: 539
		internal ToggleSwitch SwLogErrors;

		// Token: 0x0400021C RID: 540
		internal TextBlock VersionTextBlock;

		// Token: 0x0400021D RID: 541
		internal Button BtnMenuGuide;

		// Token: 0x0400021E RID: 542
		internal Button BtnHelp;

		// Token: 0x0400021F RID: 543
		internal Button BtnModDbPage;

		// Token: 0x04000220 RID: 544
		internal Button BtnClearCache;

		// Token: 0x04000221 RID: 545
		internal Button BtnClose;

		// Token: 0x04000222 RID: 546
		private bool _contentLoaded;
	}
}
