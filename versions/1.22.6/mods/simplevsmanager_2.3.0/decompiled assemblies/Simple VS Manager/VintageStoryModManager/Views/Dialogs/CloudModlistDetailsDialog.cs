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
	// Token: 0x0200001F RID: 31
	[NullableContext(1)]
	[Nullable(0)]
	public class CloudModlistDetailsDialog : Window, IComponentConnector
	{
		// Token: 0x06000414 RID: 1044 RVA: 0x0001FEF8 File Offset: 0x0001E0F8
		[NullableContext(2)]
		public CloudModlistDetailsDialog([Nullable(1)] Window owner, string suggestedName, [Nullable(new byte[]
		{
			2,
			1
		})] IEnumerable<ModConfigOption> configOptions, string defaultGameVersion = null)
		{
			this.ConfigOptions = new ObservableCollection<ModConfigOption>(Enumerable.OrderBy<ModConfigOption, string>(Enumerable.Where<ModConfigOption>(configOptions ?? Enumerable.Empty<ModConfigOption>(), (ModConfigOption option) => option != null), (ModConfigOption option) => option.DisplayName, StringComparer.OrdinalIgnoreCase));
			this.InitializeComponent();
			foreach (ModConfigOption modConfigOption in this.ConfigOptions)
			{
				modConfigOption.PropertyChanged += new PropertyChangedEventHandler(this.ConfigOption_OnPropertyChanged);
			}
			this.UpdateSelectAllState();
			base.Owner = owner;
			this.NameTextBox.Text = (string.IsNullOrWhiteSpace(suggestedName) ? string.Empty : suggestedName);
			if (!string.IsNullOrWhiteSpace(defaultGameVersion))
			{
				this.GameVersionTextBox.Text = defaultGameVersion.Trim();
			}
			this.NameTextBox.SelectAll();
			this.UpdateConfirmButtonState();
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x00020010 File Offset: 0x0001E210
		public ObservableCollection<ModConfigOption> ConfigOptions { get; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x00020018 File Offset: 0x0001E218
		public bool HasConfigOptions
		{
			get
			{
				return this.ConfigOptions.Count > 0;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x00020028 File Offset: 0x0001E228
		public string ModlistName
		{
			get
			{
				return this.NameTextBox.Text.Trim();
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x0002003A File Offset: 0x0001E23A
		[Nullable(2)]
		public string ModlistDescription
		{
			[NullableContext(2)]
			get
			{
				if (!string.IsNullOrWhiteSpace(this.DescriptionTextBox.Text))
				{
					return this.DescriptionTextBox.Text.Trim();
				}
				return null;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x00020060 File Offset: 0x0001E260
		[Nullable(2)]
		public string ModlistVersion
		{
			[NullableContext(2)]
			get
			{
				if (!string.IsNullOrWhiteSpace(this.VersionTextBox.Text))
				{
					return this.VersionTextBox.Text.Trim();
				}
				return null;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x00020086 File Offset: 0x0001E286
		[Nullable(2)]
		public string ModlistGameVersion
		{
			[NullableContext(2)]
			get
			{
				if (!string.IsNullOrWhiteSpace(this.GameVersionTextBox.Text))
				{
					return this.GameVersionTextBox.Text.Trim();
				}
				return null;
			}
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x000200AC File Offset: 0x0001E2AC
		public IReadOnlyList<ModConfigOption> GetSelectedConfigOptions()
		{
			return Enumerable.ToList<ModConfigOption>(Enumerable.Where<ModConfigOption>(this.ConfigOptions, (ModConfigOption option) => option.IsSelected));
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x000200DD File Offset: 0x0001E2DD
		private void ConfirmButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(this.NameTextBox.Text))
			{
				return;
			}
			base.DialogResult = new bool?(true);
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x000200FE File Offset: 0x0001E2FE
		private void NameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			this.UpdateConfirmButtonState();
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00020108 File Offset: 0x0001E308
		private void UpdateConfirmButtonState()
		{
			if (this.ConfirmButton == null)
			{
				return;
			}
			TextBox nameTextBox = this.NameTextBox;
			string text = (nameTextBox != null) ? nameTextBox.Text : null;
			this.ConfirmButton.IsEnabled = !string.IsNullOrWhiteSpace(text);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00020145 File Offset: 0x0001E345
		private void Window_OnLoaded(object sender, RoutedEventArgs e)
		{
			this.NameTextBox.Focus();
			this.NameTextBox.SelectAll();
			this.UpdateSelectAllState();
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00020164 File Offset: 0x0001E364
		private void SelectAllCheckBox_OnClick(object sender, RoutedEventArgs e)
		{
			if (this._isUpdatingSelectAllCheckBox)
			{
				return;
			}
			this._isUpdatingConfigOptionSelection = true;
			bool valueOrDefault = this.SelectAllCheckBox.IsChecked.GetValueOrDefault();
			foreach (ModConfigOption modConfigOption in this.ConfigOptions)
			{
				modConfigOption.IsSelected = valueOrDefault;
			}
			this._isUpdatingConfigOptionSelection = false;
			this.UpdateSelectAllState();
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x000201E0 File Offset: 0x0001E3E0
		private void ConfigOption_OnPropertyChanged([Nullable(2)] object sender, PropertyChangedEventArgs e)
		{
			if (!string.Equals(e.PropertyName, "IsSelected", 4))
			{
				return;
			}
			if (this._isUpdatingConfigOptionSelection)
			{
				return;
			}
			this.UpdateSelectAllState();
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00020208 File Offset: 0x0001E408
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

		// Token: 0x06000423 RID: 1059 RVA: 0x000202C0 File Offset: 0x0001E4C0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/cloudmodlistdetailsdialog.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x000202F0 File Offset: 0x0001E4F0
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CloudModlistDetailsDialog)target).Loaded += new RoutedEventHandler(this.Window_OnLoaded);
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
				this.DescriptionTextBox = (TextBox)target;
				return;
			case 6:
				this.SelectAllCheckBox = (CheckBox)target;
				this.SelectAllCheckBox.Click += new RoutedEventHandler(this.SelectAllCheckBox_OnClick);
				return;
			case 7:
				this.ConfirmButton = (Button)target;
				this.ConfirmButton.Click += new RoutedEventHandler(this.ConfirmButton_OnClick);
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x0400022D RID: 557
		private bool _isUpdatingConfigOptionSelection;

		// Token: 0x0400022E RID: 558
		private bool _isUpdatingSelectAllCheckBox;

		// Token: 0x04000230 RID: 560
		[Nullable(0)]
		internal TextBox NameTextBox;

		// Token: 0x04000231 RID: 561
		[Nullable(0)]
		internal TextBox VersionTextBox;

		// Token: 0x04000232 RID: 562
		[Nullable(0)]
		internal TextBox GameVersionTextBox;

		// Token: 0x04000233 RID: 563
		[Nullable(0)]
		internal TextBox DescriptionTextBox;

		// Token: 0x04000234 RID: 564
		[Nullable(0)]
		internal CheckBox SelectAllCheckBox;

		// Token: 0x04000235 RID: 565
		[Nullable(0)]
		internal Button ConfirmButton;

		// Token: 0x04000236 RID: 566
		private bool _contentLoaded;
	}
}
