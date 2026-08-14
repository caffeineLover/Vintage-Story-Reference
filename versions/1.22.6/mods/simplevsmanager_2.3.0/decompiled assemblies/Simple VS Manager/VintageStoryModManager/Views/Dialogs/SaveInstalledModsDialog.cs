using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x02000039 RID: 57
	[NullableContext(1)]
	[Nullable(0)]
	public class SaveInstalledModsDialog : Window, IComponentConnector
	{
		// Token: 0x06000502 RID: 1282 RVA: 0x00023814 File Offset: 0x00021A14
		[NullableContext(2)]
		public SaveInstalledModsDialog(string defaultListName = null, [Nullable(new byte[]
		{
			2,
			1
		})] IEnumerable<ModConfigOption> configOptions = null, string defaultCreatedBy = null, string defaultVersion = null, string defaultGameVersion = null, SaveInstalledModsDialogResult defaultAction = SaveInstalledModsDialogResult.SaveJson)
		{
			this.ConfigOptions = new ObservableCollection<ModConfigOption>(Enumerable.OrderBy<ModConfigOption, string>(Enumerable.Where<ModConfigOption>(configOptions ?? Array.Empty<ModConfigOption>(), (ModConfigOption option) => option != null), (ModConfigOption option) => option.DisplayName, StringComparer.OrdinalIgnoreCase));
			this.InitializeComponent();
			if (!string.IsNullOrWhiteSpace(defaultListName))
			{
				this.NameTextBox.Text = defaultListName.Trim();
			}
			if (!string.IsNullOrWhiteSpace(defaultVersion))
			{
				this.VersionTextBox.Text = defaultVersion.Trim();
			}
			if (!string.IsNullOrWhiteSpace(defaultGameVersion))
			{
				this.GameVersionTextBox.Text = defaultGameVersion.Trim();
			}
			if (!string.IsNullOrWhiteSpace(defaultCreatedBy))
			{
				this.CreatedByTextBox.Text = defaultCreatedBy.Trim();
			}
			else if (!string.IsNullOrWhiteSpace(Environment.UserName))
			{
				this.CreatedByTextBox.Text = Environment.UserName.Trim();
			}
			foreach (ModConfigOption modConfigOption in this.ConfigOptions)
			{
				modConfigOption.PropertyChanged += new PropertyChangedEventHandler(this.ConfigOption_OnPropertyChanged);
			}
			this.SetDefaultAction(defaultAction);
			this.UpdateActionButtonsState();
			this.UpdateSelectAllState();
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x00023978 File Offset: 0x00021B78
		public ObservableCollection<ModConfigOption> ConfigOptions { get; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x00023980 File Offset: 0x00021B80
		public bool HasConfigOptions
		{
			get
			{
				return this.ConfigOptions.Count > 0;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000505 RID: 1285 RVA: 0x00023990 File Offset: 0x00021B90
		public string ListName
		{
			get
			{
				return this.NameTextBox.Text.Trim();
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000506 RID: 1286 RVA: 0x000239A2 File Offset: 0x00021BA2
		[Nullable(2)]
		public string Version
		{
			[NullableContext(2)]
			get
			{
				return SaveInstalledModsDialog.NormalizeOptionalText(this.VersionTextBox.Text);
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000507 RID: 1287 RVA: 0x000239B4 File Offset: 0x00021BB4
		[Nullable(2)]
		public string VintageStoryVersion
		{
			[NullableContext(2)]
			get
			{
				return SaveInstalledModsDialog.NormalizeOptionalText(this.GameVersionTextBox.Text);
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000508 RID: 1288 RVA: 0x000239C6 File Offset: 0x00021BC6
		[Nullable(2)]
		public string Description
		{
			[NullableContext(2)]
			get
			{
				return SaveInstalledModsDialog.NormalizeOptionalText(this.DescriptionTextBox.Text);
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x000239D8 File Offset: 0x00021BD8
		[Nullable(2)]
		public string CreatedBy
		{
			[NullableContext(2)]
			get
			{
				return SaveInstalledModsDialog.NormalizeOptionalText(this.CreatedByTextBox.Text);
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x000239EA File Offset: 0x00021BEA
		// (set) Token: 0x0600050B RID: 1291 RVA: 0x000239F2 File Offset: 0x00021BF2
		public SaveInstalledModsDialogResult SelectedAction { get; private set; }

		// Token: 0x0600050C RID: 1292 RVA: 0x000239FB File Offset: 0x00021BFB
		public IReadOnlyList<ModConfigOption> GetSelectedConfigOptions()
		{
			return Enumerable.ToList<ModConfigOption>(Enumerable.Where<ModConfigOption>(this.ConfigOptions, (ModConfigOption option) => option.IsSelected));
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00023A2C File Offset: 0x00021C2C
		[NullableContext(2)]
		private static string NormalizeOptionalText(string text)
		{
			if (!string.IsNullOrWhiteSpace(text))
			{
				return text.Trim();
			}
			return null;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00023A3E File Offset: 0x00021C3E
		private void Window_OnLoaded(object sender, RoutedEventArgs e)
		{
			this.UpdateActionButtonsState();
			this.NameTextBox.Focus();
			this.NameTextBox.SelectAll();
			this.UpdateSelectAllState();
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00023A63 File Offset: 0x00021C63
		private void NameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			this.UpdateActionButtonsState();
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00023A6C File Offset: 0x00021C6C
		private void UpdateActionButtonsState()
		{
			TextBox nameTextBox = this.NameTextBox;
			bool isEnabled = !string.IsNullOrWhiteSpace((nameTextBox != null) ? nameTextBox.Text : null);
			if (this.ConfirmButton != null)
			{
				this.ConfirmButton.IsEnabled = isEnabled;
			}
			if (this.SaveAsPdfButton != null)
			{
				this.SaveAsPdfButton.IsEnabled = isEnabled;
			}
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00023ABC File Offset: 0x00021CBC
		private void ConfirmButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(this.NameTextBox.Text))
			{
				return;
			}
			this.SelectedAction = SaveInstalledModsDialogResult.SaveJson;
			base.DialogResult = new bool?(true);
			base.Close();
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00023AEA File Offset: 0x00021CEA
		private void SaveAsPdfButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(this.NameTextBox.Text))
			{
				return;
			}
			this.SelectedAction = SaveInstalledModsDialogResult.SavePdf;
			base.DialogResult = new bool?(true);
			base.Close();
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00023B18 File Offset: 0x00021D18
		private void SetDefaultAction(SaveInstalledModsDialogResult action)
		{
			if (this.ConfirmButton == null || this.SaveAsPdfButton == null)
			{
				return;
			}
			this.ConfirmButton.IsDefault = (action == SaveInstalledModsDialogResult.SaveJson);
			this.SaveAsPdfButton.IsDefault = (action == SaveInstalledModsDialogResult.SavePdf);
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00023B4C File Offset: 0x00021D4C
		private void SelectAllCheckBox_OnClick(object sender, RoutedEventArgs e)
		{
			if (this._isUpdatingSelectAllCheckBox)
			{
				return;
			}
			bool valueOrDefault = this.SelectAllCheckBox.IsChecked.GetValueOrDefault();
			this._isUpdatingConfigSelection = true;
			foreach (ModConfigOption modConfigOption in this.ConfigOptions)
			{
				modConfigOption.IsSelected = valueOrDefault;
			}
			this._isUpdatingConfigSelection = false;
			this.UpdateSelectAllState();
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00023BC8 File Offset: 0x00021DC8
		private void ConfigOption_OnPropertyChanged([Nullable(2)] object sender, PropertyChangedEventArgs e)
		{
			if (!string.Equals(e.PropertyName, "IsSelected", 4))
			{
				return;
			}
			if (this._isUpdatingConfigSelection)
			{
				return;
			}
			this.UpdateSelectAllState();
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00023BF0 File Offset: 0x00021DF0
		private void UpdateSelectAllState()
		{
			if (this.SelectAllCheckBox == null)
			{
				return;
			}
			if (!this.HasConfigOptions)
			{
				this._isUpdatingSelectAllCheckBox = true;
				this.SelectAllCheckBox.IsChecked = new bool?(false);
				this._isUpdatingSelectAllCheckBox = false;
				return;
			}
			int num = Enumerable.Count<ModConfigOption>(this.ConfigOptions, (ModConfigOption option) => option.IsSelected);
			int count = this.ConfigOptions.Count;
			this._isUpdatingSelectAllCheckBox = true;
			CheckBox selectAllCheckBox = this.SelectAllCheckBox;
			bool? isChecked;
			if (num == 0)
			{
				isChecked = new bool?(false);
			}
			else if (num == count)
			{
				isChecked = new bool?(true);
			}
			else
			{
				isChecked = default(bool?);
			}
			selectAllCheckBox.IsChecked = isChecked;
			this._isUpdatingSelectAllCheckBox = false;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00023CA8 File Offset: 0x00021EA8
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/saveinstalledmodsdialog.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00023CD8 File Offset: 0x00021ED8
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((SaveInstalledModsDialog)target).Loaded += new RoutedEventHandler(this.Window_OnLoaded);
				return;
			case 2:
				this.NameTextBox = (TextBox)target;
				this.NameTextBox.TextChanged += new TextChangedEventHandler(this.NameTextBox_OnTextChanged);
				return;
			case 3:
				this.VersionTextBox = (TextBox)target;
				return;
			case 4:
				this.GameVersionTextBox = (TextBox)target;
				return;
			case 5:
				this.CreatedByTextBox = (TextBox)target;
				return;
			case 6:
				this.DescriptionTextBox = (TextBox)target;
				return;
			case 7:
				this.SelectAllCheckBox = (CheckBox)target;
				this.SelectAllCheckBox.Click += new RoutedEventHandler(this.SelectAllCheckBox_OnClick);
				return;
			case 8:
				this.ConfirmButton = (Button)target;
				this.ConfirmButton.Click += new RoutedEventHandler(this.ConfirmButton_OnClick);
				return;
			case 9:
				this.SaveAsPdfButton = (Button)target;
				this.SaveAsPdfButton.Click += new RoutedEventHandler(this.SaveAsPdfButton_OnClick);
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x040002B3 RID: 691
		private bool _isUpdatingConfigSelection;

		// Token: 0x040002B4 RID: 692
		private bool _isUpdatingSelectAllCheckBox;

		// Token: 0x040002B7 RID: 695
		[Nullable(0)]
		internal TextBox NameTextBox;

		// Token: 0x040002B8 RID: 696
		[Nullable(0)]
		internal TextBox VersionTextBox;

		// Token: 0x040002B9 RID: 697
		[Nullable(0)]
		internal TextBox GameVersionTextBox;

		// Token: 0x040002BA RID: 698
		[Nullable(0)]
		internal TextBox CreatedByTextBox;

		// Token: 0x040002BB RID: 699
		[Nullable(0)]
		internal TextBox DescriptionTextBox;

		// Token: 0x040002BC RID: 700
		[Nullable(0)]
		internal CheckBox SelectAllCheckBox;

		// Token: 0x040002BD RID: 701
		[Nullable(0)]
		internal Button ConfirmButton;

		// Token: 0x040002BE RID: 702
		[Nullable(0)]
		internal Button SaveAsPdfButton;

		// Token: 0x040002BF RID: 703
		private bool _contentLoaded;
	}
}
