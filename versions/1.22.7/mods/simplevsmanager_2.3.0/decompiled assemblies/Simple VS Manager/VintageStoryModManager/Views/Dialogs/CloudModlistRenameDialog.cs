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
	// Token: 0x02000021 RID: 33
	[NullableContext(1)]
	[Nullable(0)]
	public class CloudModlistRenameDialog : Window, IComponentConnector
	{
		// Token: 0x06000431 RID: 1073 RVA: 0x00020710 File Offset: 0x0001E910
		public CloudModlistRenameDialog(Window owner, [Nullable(2)] string currentName)
		{
			this.InitializeComponent();
			base.Owner = owner;
			this.NameTextBox.Text = (string.IsNullOrWhiteSpace(currentName) ? string.Empty : currentName);
			this.NameTextBox.SelectAll();
			this.UpdateConfirmButtonState();
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x0002075C File Offset: 0x0001E95C
		public string ModlistName
		{
			get
			{
				return this.NameTextBox.Text.Trim();
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0002076E File Offset: 0x0001E96E
		private void ConfirmButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(this.NameTextBox.Text))
			{
				return;
			}
			base.DialogResult = new bool?(true);
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0002078F File Offset: 0x0001E98F
		private void NameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			this.UpdateConfirmButtonState();
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00020798 File Offset: 0x0001E998
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

		// Token: 0x06000436 RID: 1078 RVA: 0x000207D5 File Offset: 0x0001E9D5
		private void Window_OnLoaded(object sender, RoutedEventArgs e)
		{
			this.NameTextBox.Focus();
			this.NameTextBox.SelectAll();
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x000207F0 File Offset: 0x0001E9F0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/cloudmodlistrenamedialog.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00020820 File Offset: 0x0001EA20
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CloudModlistRenameDialog)target).Loaded += new RoutedEventHandler(this.Window_OnLoaded);
				return;
			case 2:
				this.NameTextBox = (TextBox)target;
				this.NameTextBox.TextChanged += new TextChangedEventHandler(this.NameTextBox_OnTextChanged);
				return;
			case 3:
				this.ConfirmButton = (Button)target;
				this.ConfirmButton.Click += new RoutedEventHandler(this.ConfirmButton_OnClick);
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x04000240 RID: 576
		[Nullable(0)]
		internal TextBox NameTextBox;

		// Token: 0x04000241 RID: 577
		[Nullable(0)]
		internal Button ConfirmButton;

		// Token: 0x04000242 RID: 578
		private bool _contentLoaded;
	}
}
