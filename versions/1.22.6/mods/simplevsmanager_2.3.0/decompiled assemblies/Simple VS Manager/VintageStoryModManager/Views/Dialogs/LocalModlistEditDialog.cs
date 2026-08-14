using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x0200002A RID: 42
	[NullableContext(2)]
	[Nullable(0)]
	public class LocalModlistEditDialog : Window, IComponentConnector
	{
		// Token: 0x0600048A RID: 1162 RVA: 0x00021D94 File Offset: 0x0001FF94
		public LocalModlistEditDialog([Nullable(1)] Window owner, string name, string description, string version, string gameVersion, string dialogTitle = null, string confirmButtonText = null)
		{
			this.InitializeComponent();
			base.Owner = owner;
			if (!string.IsNullOrWhiteSpace(dialogTitle))
			{
				base.Title = dialogTitle;
			}
			if (!string.IsNullOrWhiteSpace(confirmButtonText))
			{
				this.ConfirmButton.Content = confirmButtonText;
			}
			this.NameTextBox.Text = (name ?? string.Empty);
			this.DescriptionTextBox.Text = (description ?? string.Empty);
			this.VersionTextBox.Text = (version ?? string.Empty);
			this.GameVersionTextBox.Text = (gameVersion ?? string.Empty);
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x00021E31 File Offset: 0x00020031
		public string ModlistName
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.NameTextBox.Text))
				{
					return this.NameTextBox.Text.Trim();
				}
				return null;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600048C RID: 1164 RVA: 0x00021E57 File Offset: 0x00020057
		public string ModlistDescription
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.DescriptionTextBox.Text))
				{
					return this.DescriptionTextBox.Text.Trim();
				}
				return null;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x00021E7D File Offset: 0x0002007D
		public string ModlistVersion
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.VersionTextBox.Text))
				{
					return this.VersionTextBox.Text.Trim();
				}
				return null;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x00021EA3 File Offset: 0x000200A3
		public string ModlistGameVersion
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.GameVersionTextBox.Text))
				{
					return this.GameVersionTextBox.Text.Trim();
				}
				return null;
			}
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00021EC9 File Offset: 0x000200C9
		[NullableContext(1)]
		private void NameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			this.UpdateConfirmButtonState();
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00021ED1 File Offset: 0x000200D1
		[NullableContext(1)]
		private void ConfirmButton_OnClick(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(true);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00021EDF File Offset: 0x000200DF
		[NullableContext(1)]
		private void Window_OnLoaded(object sender, RoutedEventArgs e)
		{
			this.UpdateConfirmButtonState();
			this.NameTextBox.Focus();
			this.NameTextBox.SelectAll();
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00021EFE File Offset: 0x000200FE
		private void UpdateConfirmButtonState()
		{
			if (this.ConfirmButton == null)
			{
				return;
			}
			this.ConfirmButton.IsEnabled = !string.IsNullOrWhiteSpace(this.NameTextBox.Text);
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00021F28 File Offset: 0x00020128
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/localmodlisteditdialog.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00021F58 File Offset: 0x00020158
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((LocalModlistEditDialog)target).Loaded += new RoutedEventHandler(this.Window_OnLoaded);
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
				this.ConfirmButton = (Button)target;
				this.ConfirmButton.Click += new RoutedEventHandler(this.ConfirmButton_OnClick);
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x04000266 RID: 614
		[Nullable(0)]
		internal TextBox NameTextBox;

		// Token: 0x04000267 RID: 615
		[Nullable(0)]
		internal TextBox VersionTextBox;

		// Token: 0x04000268 RID: 616
		[Nullable(0)]
		internal TextBox GameVersionTextBox;

		// Token: 0x04000269 RID: 617
		[Nullable(0)]
		internal TextBox DescriptionTextBox;

		// Token: 0x0400026A RID: 618
		[Nullable(0)]
		internal Button ConfirmButton;

		// Token: 0x0400026B RID: 619
		private bool _contentLoaded;
	}
}
