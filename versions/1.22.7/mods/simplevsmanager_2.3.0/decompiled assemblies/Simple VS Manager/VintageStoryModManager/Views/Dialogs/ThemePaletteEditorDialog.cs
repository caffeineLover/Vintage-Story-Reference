using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using VintageStoryModManager.Services;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x0200003C RID: 60
	[NullableContext(1)]
	[Nullable(0)]
	public class ThemePaletteEditorDialog : Window, INotifyPropertyChanged, IComponentConnector
	{
		// Token: 0x06000522 RID: 1314 RVA: 0x00024034 File Offset: 0x00022234
		public ThemePaletteEditorDialog(UserConfigurationService configuration)
		{
			this.InitializeComponent();
			if (configuration == null)
			{
				throw new ArgumentNullException("configuration");
			}
			this._configuration = configuration;
			this.PaletteItems = new ObservableCollection<PaletteColorEntry>();
			this.ThemeOptions = new ObservableCollection<ThemePaletteEditorDialog.ThemeOption>();
			base.DataContext = this;
			this._isInitializing = true;
			this.RefreshThemeOptions();
			this.CaptureThemePaletteSnapshots();
			if (this.SelectedThemeOption == null)
			{
				this.SelectedThemeOption = Enumerable.FirstOrDefault<ThemePaletteEditorDialog.ThemeOption>(this.ThemeOptions);
			}
			this.LoadPalette();
			this.UpdateResetAvailability();
			this._isInitializing = false;
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x000240D1 File Offset: 0x000222D1
		public ObservableCollection<PaletteColorEntry> PaletteItems { get; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x000240D9 File Offset: 0x000222D9
		public ObservableCollection<ThemePaletteEditorDialog.ThemeOption> ThemeOptions { get; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x000240E1 File Offset: 0x000222E1
		// (set) Token: 0x06000526 RID: 1318 RVA: 0x000240E9 File Offset: 0x000222E9
		[Nullable(2)]
		public ThemePaletteEditorDialog.ThemeOption SelectedThemeOption
		{
			[NullableContext(2)]
			get
			{
				return this._selectedThemeOption;
			}
			[NullableContext(2)]
			set
			{
				if (!this.SetProperty<ThemePaletteEditorDialog.ThemeOption>(ref this._selectedThemeOption, value, "SelectedThemeOption"))
				{
					return;
				}
				this.UpdateResetAvailability();
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00024108 File Offset: 0x00022308
		private void CaptureThemePaletteSnapshots()
		{
			this._themePaletteSnapshots.Clear();
			foreach (string text in this._configuration.GetAllThemeNames())
			{
				IReadOnlyDictionary<string, string> readOnlyDictionary;
				if (this._configuration.TryGetThemePalette(text, out readOnlyDictionary))
				{
					this._themePaletteSnapshots[text] = new Dictionary<string, string>(readOnlyDictionary, StringComparer.OrdinalIgnoreCase);
				}
			}
			string currentThemeName = this._configuration.GetCurrentThemeName();
			IReadOnlyDictionary<string, string> readOnlyDictionary2;
			if (!this._themePaletteSnapshots.ContainsKey(currentThemeName) && this._configuration.TryGetThemePalette(currentThemeName, out readOnlyDictionary2))
			{
				this._themePaletteSnapshots[currentThemeName] = new Dictionary<string, string>(readOnlyDictionary2, StringComparer.OrdinalIgnoreCase);
			}
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x000241C8 File Offset: 0x000223C8
		private void LoadPalette()
		{
			this.PaletteItems.Clear();
			foreach (KeyValuePair<string, string> keyValuePair in Enumerable.ThenBy<KeyValuePair<string, string>, string>(Enumerable.OrderByDescending<KeyValuePair<string, string>, int>(this._configuration.GetThemePaletteColors(), (KeyValuePair<string, string> entry) => ThemePaletteEditorDialog.GetPaletteUsageCount(entry.Key)), (KeyValuePair<string, string> entry) => ThemePaletteEditorDialog.GetPaletteDisplayName(entry.Key), StringComparer.OrdinalIgnoreCase))
			{
				string paletteDisplayName = ThemePaletteEditorDialog.GetPaletteDisplayName(keyValuePair.Key);
				this.PaletteItems.Add(new PaletteColorEntry(keyValuePair.Key, paletteDisplayName, keyValuePair.Value, new Action<PaletteColorEntry>(this.PickColor)));
			}
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x000242A4 File Offset: 0x000224A4
		private void RefreshThemeOptions()
		{
			ThemePaletteEditorDialog.<>c__DisplayClass18_0 CS$<>8__locals1 = new ThemePaletteEditorDialog.<>c__DisplayClass18_0();
			this._isInitializing = true;
			ThemePaletteEditorDialog.<>c__DisplayClass18_0 CS$<>8__locals2 = CS$<>8__locals1;
			ThemePaletteEditorDialog.ThemeOption selectedThemeOption = this.SelectedThemeOption;
			CS$<>8__locals2.currentName = (((selectedThemeOption != null) ? selectedThemeOption.Name : null) ?? this._configuration.GetCurrentThemeName());
			this.ThemeOptions.Clear();
			this.ThemeOptions.Add(new ThemePaletteEditorDialog.ThemeOption(UserConfigurationService.GetThemeDisplayName(ColorTheme.VintageStory), new ColorTheme?(ColorTheme.VintageStory), this._configuration));
			this.ThemeOptions.Add(new ThemePaletteEditorDialog.ThemeOption(UserConfigurationService.GetThemeDisplayName(ColorTheme.Dark), new ColorTheme?(ColorTheme.Dark), this._configuration));
			this.ThemeOptions.Add(new ThemePaletteEditorDialog.ThemeOption(UserConfigurationService.GetThemeDisplayName(ColorTheme.Light), new ColorTheme?(ColorTheme.Light), this._configuration));
			foreach (string name in this._configuration.GetCustomThemeNames())
			{
				if (!ThemePaletteEditorDialog.ThemeOption.IsBuiltInThemeName(name))
				{
					this.ThemeOptions.Add(new ThemePaletteEditorDialog.ThemeOption(name, default(ColorTheme?), this._configuration));
				}
			}
			if (this._configuration.ColorTheme == ColorTheme.Custom && Enumerable.All<ThemePaletteEditorDialog.ThemeOption>(this.ThemeOptions, (ThemePaletteEditorDialog.ThemeOption option) => !string.Equals(option.Name, CS$<>8__locals1.currentName, 5)))
			{
				this.ThemeOptions.Add(new ThemePaletteEditorDialog.ThemeOption(CS$<>8__locals1.currentName, default(ColorTheme?), this._configuration));
			}
			this.SelectedThemeOption = (Enumerable.FirstOrDefault<ThemePaletteEditorDialog.ThemeOption>(this.ThemeOptions, (ThemePaletteEditorDialog.ThemeOption option) => string.Equals(option.Name, CS$<>8__locals1.currentName, 5)) ?? this.SelectedThemeOption);
			this._isInitializing = false;
			this.UpdateResetAvailability();
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0002443C File Offset: 0x0002263C
		[NullableContext(2)]
		private void PickColor(PaletteColorEntry entry)
		{
			if (entry == null)
			{
				return;
			}
			using (ColorDialog colorDialog = new ColorDialog
			{
				FullOpen = true,
				AnyColor = true
			})
			{
				Color color;
				if (ThemePaletteEditorDialog.TryParseColor(entry.HexValue, out color))
				{
					colorDialog.Color = Color.FromArgb((int)color.A, (int)color.R, (int)color.G, (int)color.B);
				}
				IntPtr handle = new WindowInteropHelper(this).Handle;
				if (((handle != IntPtr.Zero) ? colorDialog.ShowDialog(new ThemePaletteEditorDialog.Win32Window(handle)) : colorDialog.ShowDialog()) == 1)
				{
					Color color2 = colorDialog.Color;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 4);
					defaultInterpolatedStringHandler.AppendLiteral("#");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(color2.A, "X2");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(color2.R, "X2");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(color2.G, "X2");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(color2.B, "X2");
					string hexValue = defaultInterpolatedStringHandler.ToStringAndClear();
					this.ApplyPaletteEntry(entry, hexValue);
				}
			}
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00024560 File Offset: 0x00022760
		private void ApplyPaletteEntry(PaletteColorEntry entry, string hexValue)
		{
			if (!this._configuration.TrySetThemePaletteColor(entry.Key, hexValue))
			{
				ModManagerMessageBox.Show(this, "Please enter a colour in #RRGGBB or #AARRGGBB format.", "Simple VS Manager", 0, 48, null, null, null);
				return;
			}
			entry.UpdateFromHex(hexValue);
			this.ApplyTheme();
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0002459C File Offset: 0x0002279C
		private void ApplyTheme()
		{
			IReadOnlyDictionary<string, string> themePaletteColors = this._configuration.GetThemePaletteColors();
			App.ApplyTheme(this._configuration.ColorTheme, (themePaletteColors.Count > 0) ? themePaletteColors : null);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x000245D4 File Offset: 0x000227D4
		private static string GetPaletteDisplayName(string key)
		{
			ThemePaletteEditorDialog.PaletteDisplayInfo paletteDisplayInfo;
			if (!ThemePaletteEditorDialog.PaletteDisplayInfos.TryGetValue(key, ref paletteDisplayInfo))
			{
				return key;
			}
			return paletteDisplayInfo.DisplayName;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x000245F8 File Offset: 0x000227F8
		private static int GetPaletteUsageCount(string key)
		{
			ThemePaletteEditorDialog.PaletteDisplayInfo paletteDisplayInfo;
			if (!ThemePaletteEditorDialog.PaletteDisplayInfos.TryGetValue(key, ref paletteDisplayInfo))
			{
				return 0;
			}
			return paletteDisplayInfo.UsageCount;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0002461C File Offset: 0x0002281C
		[NullableContext(2)]
		private static bool TryParseColor(string value, out Color color)
		{
			color = default(Color);
			if (string.IsNullOrWhiteSpace(value))
			{
				return false;
			}
			try
			{
				object obj = ColorConverter.ConvertFromString(value);
				if (obj is Color)
				{
					Color color2 = (Color)obj;
					color = color2;
					return true;
				}
			}
			catch
			{
				return false;
			}
			return false;
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00024678 File Offset: 0x00022878
		private void ApplySelectedTheme()
		{
			if (this._isInitializing || this.SelectedThemeOption == null)
			{
				return;
			}
			if (this._configuration.TryActivateTheme(this.SelectedThemeOption.Name))
			{
				this.LoadPalette();
				this.ApplyTheme();
			}
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x000246B0 File Offset: 0x000228B0
		private void ResetButton_OnClick(object sender, RoutedEventArgs e)
		{
			ThemePaletteEditorDialog.ThemeOption selectedThemeOption = this.SelectedThemeOption;
			if (selectedThemeOption == null || !selectedThemeOption.SupportsReset)
			{
				return;
			}
			if (this.SelectedThemeOption.Theme != null && this.SelectedThemeOption.HasCustomOverride)
			{
				this._configuration.DeleteCustomTheme(this.SelectedThemeOption.Name);
				this._themePaletteSnapshots.Remove(this.SelectedThemeOption.Name);
			}
			this._configuration.TryActivateTheme(this.SelectedThemeOption.Name);
			this._configuration.ResetThemePalette();
			this.RefreshThemeOptions();
			this.LoadPalette();
			this.ApplyTheme();
			this.CaptureThemePaletteSnapshots();
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0002475F File Offset: 0x0002295F
		private void CloseButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (!this.TryResolveUnsavedChanges(this.SelectedThemeOption, true))
			{
				return;
			}
			base.Close();
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00024777 File Offset: 0x00022977
		private void UpdateResetAvailability()
		{
			if (this.ResetButton != null)
			{
				UIElement resetButton = this.ResetButton;
				ThemePaletteEditorDialog.ThemeOption selectedThemeOption = this.SelectedThemeOption;
				resetButton.IsEnabled = (selectedThemeOption != null && selectedThemeOption.SupportsReset);
			}
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x000247A0 File Offset: 0x000229A0
		private void PaletteComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this._isInitializing || this.PaletteItems.Count == 0)
			{
				return;
			}
			ThemePaletteEditorDialog.ThemeOption themeOption = Enumerable.FirstOrDefault<ThemePaletteEditorDialog.ThemeOption>(Enumerable.OfType<ThemePaletteEditorDialog.ThemeOption>(e.RemovedItems));
			ThemePaletteEditorDialog.ThemeOption themeOption2 = Enumerable.FirstOrDefault<ThemePaletteEditorDialog.ThemeOption>(Enumerable.OfType<ThemePaletteEditorDialog.ThemeOption>(e.AddedItems)) ?? this.SelectedThemeOption;
			string requestedThemeName = (themeOption2 != null) ? themeOption2.Name : null;
			if (themeOption != null && this.SelectedThemeOption != themeOption)
			{
				this._isInitializing = true;
				this.SelectedThemeOption = themeOption;
				this._isInitializing = false;
				if (!this._isSwitchingAfterSave && !this.TryResolveUnsavedChanges(themeOption, false))
				{
					return;
				}
			}
			if (requestedThemeName != null)
			{
				themeOption2 = Enumerable.FirstOrDefault<ThemePaletteEditorDialog.ThemeOption>(this.ThemeOptions, (ThemePaletteEditorDialog.ThemeOption option) => string.Equals(option.Name, requestedThemeName, 5));
			}
			if (themeOption2 != null && this.SelectedThemeOption != themeOption2)
			{
				this._isInitializing = true;
				this.SelectedThemeOption = themeOption2;
				this._isInitializing = false;
			}
			this.ApplySelectedTheme();
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000535 RID: 1333 RVA: 0x00024884 File Offset: 0x00022A84
		// (remove) Token: 0x06000536 RID: 1334 RVA: 0x000248BC File Offset: 0x00022ABC
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06000537 RID: 1335 RVA: 0x000248F1 File Offset: 0x00022AF1
		[NullableContext(2)]
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0002490A File Offset: 0x00022B0A
		private bool SetProperty<[Nullable(2)] T>(ref T storage, T value, [Nullable(2)] [CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(storage, value))
			{
				return false;
			}
			storage = value;
			this.OnPropertyChanged(propertyName);
			return true;
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00024930 File Offset: 0x00022B30
		private void SaveButton_OnClick(object sender, RoutedEventArgs e)
		{
			string text;
			this.TrySaveTheme(out text, null);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00024948 File Offset: 0x00022B48
		private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (this.SelectedThemeOption == null || !this.SelectedThemeOption.IsCustom)
			{
				return;
			}
			if (ModManagerMessageBox.Show(this, "Delete the theme '" + this.SelectedThemeOption.Name + "'?", "Simple VS Manager", 4, 32, null, null, null) != 6)
			{
				return;
			}
			if (!this._configuration.DeleteCustomTheme(this.SelectedThemeOption.Name))
			{
				return;
			}
			this._themePaletteSnapshots.Remove(this.SelectedThemeOption.Name);
			this.RefreshThemeOptions();
			string currentThemeName = this._configuration.GetCurrentThemeName();
			this.SelectedThemeOption = (Enumerable.FirstOrDefault<ThemePaletteEditorDialog.ThemeOption>(this.ThemeOptions, (ThemePaletteEditorDialog.ThemeOption option) => string.Equals(option.Name, currentThemeName, 5)) ?? Enumerable.FirstOrDefault<ThemePaletteEditorDialog.ThemeOption>(this.ThemeOptions));
			this.ApplySelectedTheme();
			this.CaptureThemePaletteSnapshots();
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00024A20 File Offset: 0x00022C20
		[NullableContext(2)]
		private bool TryResolveUnsavedChanges(ThemePaletteEditorDialog.ThemeOption activeTheme, bool isClosing = false)
		{
			if (activeTheme == null || !this.HasUnsavedChanges(activeTheme.Name))
			{
				return true;
			}
			string messageBoxText = isClosing ? "Save changes to the current theme before closing?" : ("Save changes to the theme '" + activeTheme.Name + "' before switching?");
			MessageBoxResult messageBoxResult = ModManagerMessageBox.Show(this, messageBoxText, "Simple VS Manager", 3, 32, null, null, null);
			if (messageBoxResult == 6)
			{
				string text;
				return this.TrySaveTheme(out text, activeTheme.Name);
			}
			if (messageBoxResult != 7)
			{
				return false;
			}
			this.RestoreThemeFromSnapshot(activeTheme);
			return true;
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00024A98 File Offset: 0x00022C98
		private bool HasUnsavedChanges(string themeName)
		{
			IReadOnlyDictionary<string, string> second;
			return this._themePaletteSnapshots.TryGetValue(themeName, ref second) && !ThemePaletteEditorDialog.ArePalettesEqual(this._configuration.GetThemePaletteColors(), second);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00024ACC File Offset: 0x00022CCC
		private void RestoreThemeFromSnapshot(ThemePaletteEditorDialog.ThemeOption themeOption)
		{
			IReadOnlyDictionary<string, string> readOnlyDictionary;
			if (!this._themePaletteSnapshots.TryGetValue(themeOption.Name, ref readOnlyDictionary))
			{
				return;
			}
			Dictionary<string, string> paletteOverride = new Dictionary<string, string>(readOnlyDictionary, StringComparer.OrdinalIgnoreCase);
			ColorTheme valueOrDefault = themeOption.Theme.GetValueOrDefault(ColorTheme.Custom);
			this._configuration.SetColorTheme(valueOrDefault, paletteOverride);
			this.LoadPalette();
			this.ApplyTheme();
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00024B24 File Offset: 0x00022D24
		[NullableContext(2)]
		private bool TrySaveTheme(out string savedThemeName, string defaultThemeName = null)
		{
			savedThemeName = null;
			string text = defaultThemeName;
			if (defaultThemeName == null)
			{
				ThemePaletteEditorDialog.ThemeOption selectedThemeOption = this.SelectedThemeOption;
				text = (((selectedThemeOption != null) ? selectedThemeOption.Name : null) ?? this._configuration.GetCurrentThemeName());
			}
			string text2 = text;
			if (ThemePaletteEditorDialog.ThemeOption.IsBuiltInThemeName(text2))
			{
				text2 += " Custom";
			}
			ThemeNameDialog dialog = new ThemeNameDialog(text2, this._configuration)
			{
				Owner = this
			};
			if (!dialog.ShowDialog().GetValueOrDefault())
			{
				return false;
			}
			if (!this._configuration.SaveCustomTheme(dialog.ThemeName))
			{
				ModManagerMessageBox.Show(this, "Please enter a valid theme name.", "Simple VS Manager", 0, 48, null, null, null);
				return false;
			}
			savedThemeName = dialog.ThemeName;
			this.UpdateThemeSnapshot(savedThemeName);
			this.RefreshThemeOptions();
			this._isSwitchingAfterSave = true;
			try
			{
				this.SelectedThemeOption = Enumerable.FirstOrDefault<ThemePaletteEditorDialog.ThemeOption>(this.ThemeOptions, (ThemePaletteEditorDialog.ThemeOption option) => string.Equals(option.Name, dialog.ThemeName, 5));
				this.ApplySelectedTheme();
			}
			finally
			{
				this._isSwitchingAfterSave = false;
			}
			return true;
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00024C38 File Offset: 0x00022E38
		private void UpdateThemeSnapshot(string themeName)
		{
			IReadOnlyDictionary<string, string> themePaletteColors = this._configuration.GetThemePaletteColors();
			this._themePaletteSnapshots[themeName] = new Dictionary<string, string>(themePaletteColors, StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00024C68 File Offset: 0x00022E68
		private static bool ArePalettesEqual(IReadOnlyDictionary<string, string> first, IReadOnlyDictionary<string, string> second)
		{
			if (first == second)
			{
				return true;
			}
			if (first.Count != second.Count)
			{
				return false;
			}
			foreach (KeyValuePair<string, string> keyValuePair in first)
			{
				string text;
				if (!second.TryGetValue(keyValuePair.Key, ref text))
				{
					return false;
				}
				if (!string.Equals(keyValuePair.Value, text, 5))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00024CEC File Offset: 0x00022EEC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/themepaletteeditordialog.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00024D1C File Offset: 0x00022F1C
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.PaletteComboBox = (ComboBox)target;
				this.PaletteComboBox.SelectionChanged += new SelectionChangedEventHandler(this.PaletteComboBox_OnSelectionChanged);
				return;
			case 2:
				((Button)target).Click += new RoutedEventHandler(this.SaveButton_OnClick);
				return;
			case 3:
				((Button)target).Click += new RoutedEventHandler(this.DeleteButton_OnClick);
				return;
			case 4:
				this.ResetButton = (Button)target;
				this.ResetButton.Click += new RoutedEventHandler(this.ResetButton_OnClick);
				return;
			case 5:
				((Button)target).Click += new RoutedEventHandler(this.CloseButton_OnClick);
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00024DE4 File Offset: 0x00022FE4
		// Note: this type is marked as 'beforefieldinit'.
		static ThemePaletteEditorDialog()
		{
			Dictionary<string, ThemePaletteEditorDialog.PaletteDisplayInfo> dictionary = new Dictionary<string, ThemePaletteEditorDialog.PaletteDisplayInfo>(StringComparer.OrdinalIgnoreCase);
			dictionary["Palette.BaseSurface.Shadowed"] = new ThemePaletteEditorDialog.PaletteDisplayInfo("Palette.BaseSurface.Shadowed", "Main background", 33);
			dictionary["Palette.Text.Primary"] = new ThemePaletteEditorDialog.PaletteDisplayInfo("Palette.Text.Primary", "Primary text", 24);
			dictionary["Palette.Accent.Primary"] = new ThemePaletteEditorDialog.PaletteDisplayInfo("Palette.Accent.Primary", "Accent color", 24);
			dictionary["Palette.BaseSurface.Raised"] = new ThemePaletteEditorDialog.PaletteDisplayInfo("Palette.BaseSurface.Raised", "Panel background", 19);
			dictionary["Palette.BaseSurface.HoverGlow"] = new ThemePaletteEditorDialog.PaletteDisplayInfo("Palette.BaseSurface.HoverGlow", "Hover highlight", 14);
			dictionary["Palette.Interactive.Surface"] = new ThemePaletteEditorDialog.PaletteDisplayInfo("Palette.Interactive.Surface", "Active controls", 13);
			dictionary["Palette.Interactive.DisabledSurface"] = new ThemePaletteEditorDialog.PaletteDisplayInfo("Palette.Interactive.DisabledSurface", "Disabled controls", 11);
			dictionary["Palette.Bevel.Highlight"] = new ThemePaletteEditorDialog.PaletteDisplayInfo("Palette.Bevel.Highlight", "Highlight edges", 11);
			dictionary["Palette.BaseSurface.Brighter"] = new ThemePaletteEditorDialog.PaletteDisplayInfo("Palette.BaseSurface.Brighter", "Secondary background", 7);
			dictionary["Palette.Bevel.Shadow"] = new ThemePaletteEditorDialog.PaletteDisplayInfo("Palette.Bevel.Shadow", "Shadow edges", 7);
			dictionary["Palette.White"] = new ThemePaletteEditorDialog.PaletteDisplayInfo("Palette.White", "Bright highlight", 6);
			dictionary["Palette.DarkGrey"] = new ThemePaletteEditorDialog.PaletteDisplayInfo("Palette.DarkGrey", "Dark neutral", 6);
			dictionary["Palette.Overlay.HoverTint"] = new ThemePaletteEditorDialog.PaletteDisplayInfo("Palette.Overlay.HoverTint", "Hover overlay", 6);
			dictionary["Palette.Text.Link"] = new ThemePaletteEditorDialog.PaletteDisplayInfo("Palette.Text.Link", "Link text", 5);
			dictionary["Palette.Grey"] = new ThemePaletteEditorDialog.PaletteDisplayInfo("Palette.Grey", "Neutral grey", 5);
			dictionary["Palette.Error"] = new ThemePaletteEditorDialog.PaletteDisplayInfo("Palette.Error", "Error state", 5);
			ThemePaletteEditorDialog.PaletteDisplayInfos = dictionary;
		}

		// Token: 0x040002C8 RID: 712
		private static readonly IReadOnlyDictionary<string, ThemePaletteEditorDialog.PaletteDisplayInfo> PaletteDisplayInfos;

		// Token: 0x040002C9 RID: 713
		private readonly UserConfigurationService _configuration;

		// Token: 0x040002CA RID: 714
		private readonly Dictionary<string, IReadOnlyDictionary<string, string>> _themePaletteSnapshots = new Dictionary<string, IReadOnlyDictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040002CB RID: 715
		private bool _isInitializing;

		// Token: 0x040002CC RID: 716
		private bool _isSwitchingAfterSave;

		// Token: 0x040002CD RID: 717
		[Nullable(2)]
		private ThemePaletteEditorDialog.ThemeOption _selectedThemeOption;

		// Token: 0x040002D1 RID: 721
		[Nullable(0)]
		internal ComboBox PaletteComboBox;

		// Token: 0x040002D2 RID: 722
		[Nullable(0)]
		internal Button ResetButton;

		// Token: 0x040002D3 RID: 723
		private bool _contentLoaded;

		// Token: 0x020001DA RID: 474
		[Nullable(0)]
		public sealed class ThemeOption
		{
			// Token: 0x060013B0 RID: 5040 RVA: 0x000727D0 File Offset: 0x000709D0
			public ThemeOption(string name, ColorTheme? theme, UserConfigurationService configuration)
			{
				string name2 = name;
				if (name2 == null)
				{
					throw new ArgumentNullException("name");
				}
				this.Name = name2;
				this.Theme = theme;
				bool flag;
				if (theme != null)
				{
					ColorTheme valueOrDefault = theme.GetValueOrDefault();
					if (valueOrDefault <= ColorTheme.Light)
					{
						flag = true;
						goto IL_52;
					}
				}
				flag = false;
				IL_52:
				bool flag2 = flag;
				bool flag3 = ThemePaletteEditorDialog.ThemeOption.IsBuiltInThemeName(name);
				this.SupportsReset = (flag2 || flag3);
				this.HasCustomOverride = 0;
				if (flag2 && configuration != null)
				{
					IReadOnlyList<string> customThemeNames = configuration.GetCustomThemeNames();
					this.HasCustomOverride = Enumerable.Any<string>(customThemeNames, (string customName) => string.Equals(customName, name, 5));
				}
			}

			// Token: 0x17000448 RID: 1096
			// (get) Token: 0x060013B1 RID: 5041 RVA: 0x00072875 File Offset: 0x00070A75
			public string Name { get; }

			// Token: 0x17000449 RID: 1097
			// (get) Token: 0x060013B2 RID: 5042 RVA: 0x0007287D File Offset: 0x00070A7D
			public ColorTheme? Theme { get; }

			// Token: 0x1700044A RID: 1098
			// (get) Token: 0x060013B3 RID: 5043 RVA: 0x00072888 File Offset: 0x00070A88
			public bool IsCustom
			{
				get
				{
					return this.Theme == null;
				}
			}

			// Token: 0x1700044B RID: 1099
			// (get) Token: 0x060013B4 RID: 5044 RVA: 0x000728A6 File Offset: 0x00070AA6
			public bool SupportsReset { get; }

			// Token: 0x1700044C RID: 1100
			// (get) Token: 0x060013B5 RID: 5045 RVA: 0x000728AE File Offset: 0x00070AAE
			public bool HasCustomOverride { get; }

			// Token: 0x060013B6 RID: 5046 RVA: 0x000728B6 File Offset: 0x00070AB6
			public static bool IsBuiltInThemeName(string name)
			{
				return string.Equals(name, UserConfigurationService.GetThemeDisplayName(ColorTheme.VintageStory), 5) || string.Equals(name, UserConfigurationService.GetThemeDisplayName(ColorTheme.Dark), 5) || string.Equals(name, UserConfigurationService.GetThemeDisplayName(ColorTheme.Light), 5);
			}
		}

		// Token: 0x020001DB RID: 475
		[Nullable(0)]
		private sealed class PaletteDisplayInfo : IEquatable<ThemePaletteEditorDialog.PaletteDisplayInfo>
		{
			// Token: 0x060013B7 RID: 5047 RVA: 0x000728E5 File Offset: 0x00070AE5
			public PaletteDisplayInfo(string Key, string DisplayName, int UsageCount)
			{
				this.Key = Key;
				this.DisplayName = DisplayName;
				this.UsageCount = UsageCount;
				base..ctor();
			}

			// Token: 0x1700044D RID: 1101
			// (get) Token: 0x060013B8 RID: 5048 RVA: 0x00072902 File Offset: 0x00070B02
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(ThemePaletteEditorDialog.PaletteDisplayInfo);
				}
			}

			// Token: 0x1700044E RID: 1102
			// (get) Token: 0x060013B9 RID: 5049 RVA: 0x0007290E File Offset: 0x00070B0E
			// (set) Token: 0x060013BA RID: 5050 RVA: 0x00072916 File Offset: 0x00070B16
			public string Key { get; set; }

			// Token: 0x1700044F RID: 1103
			// (get) Token: 0x060013BB RID: 5051 RVA: 0x0007291F File Offset: 0x00070B1F
			// (set) Token: 0x060013BC RID: 5052 RVA: 0x00072927 File Offset: 0x00070B27
			public string DisplayName { get; set; }

			// Token: 0x17000450 RID: 1104
			// (get) Token: 0x060013BD RID: 5053 RVA: 0x00072930 File Offset: 0x00070B30
			// (set) Token: 0x060013BE RID: 5054 RVA: 0x00072938 File Offset: 0x00070B38
			public int UsageCount { get; set; }

			// Token: 0x060013BF RID: 5055 RVA: 0x00072944 File Offset: 0x00070B44
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("PaletteDisplayInfo");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x060013C0 RID: 5056 RVA: 0x00072990 File Offset: 0x00070B90
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Key = ");
				builder.Append(this.Key);
				builder.Append(", DisplayName = ");
				builder.Append(this.DisplayName);
				builder.Append(", UsageCount = ");
				builder.Append(this.UsageCount.ToString());
				return true;
			}

			// Token: 0x060013C1 RID: 5057 RVA: 0x000729FC File Offset: 0x00070BFC
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(ThemePaletteEditorDialog.PaletteDisplayInfo left, ThemePaletteEditorDialog.PaletteDisplayInfo right)
			{
				return !(left == right);
			}

			// Token: 0x060013C2 RID: 5058 RVA: 0x00072A08 File Offset: 0x00070C08
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(ThemePaletteEditorDialog.PaletteDisplayInfo left, ThemePaletteEditorDialog.PaletteDisplayInfo right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x060013C3 RID: 5059 RVA: 0x00072A1C File Offset: 0x00070C1C
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Key>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<DisplayName>k__BackingField)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.<UsageCount>k__BackingField);
			}

			// Token: 0x060013C4 RID: 5060 RVA: 0x00072A7E File Offset: 0x00070C7E
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as ThemePaletteEditorDialog.PaletteDisplayInfo);
			}

			// Token: 0x060013C5 RID: 5061 RVA: 0x00072A8C File Offset: 0x00070C8C
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(ThemePaletteEditorDialog.PaletteDisplayInfo other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Key>k__BackingField, other.<Key>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<DisplayName>k__BackingField, other.<DisplayName>k__BackingField) && EqualityComparer<int>.Default.Equals(this.<UsageCount>k__BackingField, other.<UsageCount>k__BackingField));
			}

			// Token: 0x060013C7 RID: 5063 RVA: 0x00072B05 File Offset: 0x00070D05
			[CompilerGenerated]
			private PaletteDisplayInfo(ThemePaletteEditorDialog.PaletteDisplayInfo original)
			{
				this.Key = original.<Key>k__BackingField;
				this.DisplayName = original.<DisplayName>k__BackingField;
				this.UsageCount = original.<UsageCount>k__BackingField;
			}

			// Token: 0x060013C8 RID: 5064 RVA: 0x00072B31 File Offset: 0x00070D31
			[CompilerGenerated]
			public void Deconstruct(out string Key, out string DisplayName, out int UsageCount)
			{
				Key = this.Key;
				DisplayName = this.DisplayName;
				UsageCount = this.UsageCount;
			}
		}

		// Token: 0x020001DC RID: 476
		[NullableContext(0)]
		private sealed class Win32Window : IWin32Window
		{
			// Token: 0x060013C9 RID: 5065 RVA: 0x00072B4B File Offset: 0x00070D4B
			public Win32Window(IntPtr handle)
			{
				this.Handle = handle;
			}

			// Token: 0x17000451 RID: 1105
			// (get) Token: 0x060013CA RID: 5066 RVA: 0x00072B5A File Offset: 0x00070D5A
			public IntPtr Handle { get; }
		}
	}
}
