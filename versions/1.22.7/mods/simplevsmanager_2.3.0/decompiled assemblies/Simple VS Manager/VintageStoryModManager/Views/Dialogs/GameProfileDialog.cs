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
	// Token: 0x02000027 RID: 39
	[NullableContext(1)]
	[Nullable(0)]
	public class GameProfileDialog : Window, IComponentConnector
	{
		// Token: 0x0600046D RID: 1133 RVA: 0x00021760 File Offset: 0x0001F960
		public GameProfileDialog(Window owner)
		{
			this.InitializeComponent();
			base.Owner = owner;
			this.UpdateConfirmButtonState();
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x0002177B File Offset: 0x0001F97B
		public string ProfileName
		{
			get
			{
				return this.NameTextBox.Text.Trim();
			}
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0002178D File Offset: 0x0001F98D
		private void ConfirmButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(this.ProfileName))
			{
				return;
			}
			base.DialogResult = new bool?(true);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x000217A9 File Offset: 0x0001F9A9
		private void NameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			this.UpdateConfirmButtonState();
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x000217B1 File Offset: 0x0001F9B1
		private void UpdateConfirmButtonState()
		{
			if (this.ConfirmButton == null)
			{
				return;
			}
			this.ConfirmButton.IsEnabled = !string.IsNullOrWhiteSpace(this.NameTextBox.Text);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x000217DA File Offset: 0x0001F9DA
		private void Window_OnLoaded(object sender, RoutedEventArgs e)
		{
			this.NameTextBox.Focus();
			this.NameTextBox.SelectAll();
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x000217F4 File Offset: 0x0001F9F4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/gameprofiledialog.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00021824 File Offset: 0x0001FA24
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((GameProfileDialog)target).Loaded += new RoutedEventHandler(this.Window_OnLoaded);
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

		// Token: 0x0400025E RID: 606
		[Nullable(0)]
		internal TextBox NameTextBox;

		// Token: 0x0400025F RID: 607
		[Nullable(0)]
		internal Button ConfirmButton;

		// Token: 0x04000260 RID: 608
		private bool _contentLoaded;
	}
}
