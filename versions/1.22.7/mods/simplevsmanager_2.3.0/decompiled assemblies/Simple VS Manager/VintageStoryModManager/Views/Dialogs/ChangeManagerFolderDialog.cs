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
	// Token: 0x0200001D RID: 29
	public class ChangeManagerFolderDialog : Window, IComponentConnector
	{
		// Token: 0x0600040C RID: 1036 RVA: 0x0001FD8C File Offset: 0x0001DF8C
		[NullableContext(1)]
		public ChangeManagerFolderDialog(Window owner, string currentFolder)
		{
			this.InitializeComponent();
			base.Owner = owner;
			string text = "Current manager folder:\n" + currentFolder + "\n\nThis will allow you to move the entire \"Simple VS Manager\" folder to a new location.\n\nThe manager will:\n• Move all configuration files, cached mods, backups, and presets\n• Update the configuration to use the new location\n• Require a restart to complete the change\n\nNote: The Firebase authentication backup (SVSM Backup folder) will remain in its original location.\n\nDo not proceed unless you really need to move the folder. Do you want to continue?";
			this.MessageTextBlock.Text = text;
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x0001FDD0 File Offset: 0x0001DFD0
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x0001FDD8 File Offset: 0x0001DFD8
		public ChangeManagerFolderDialogResult Result { get; private set; } = ChangeManagerFolderDialogResult.No;

		// Token: 0x0600040F RID: 1039 RVA: 0x0001FDE1 File Offset: 0x0001DFE1
		[NullableContext(1)]
		private void YesButton_OnClick(object sender, RoutedEventArgs e)
		{
			this.Result = ChangeManagerFolderDialogResult.Yes;
			base.DialogResult = new bool?(true);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001FDF6 File Offset: 0x0001DFF6
		[NullableContext(1)]
		private void NoButton_OnClick(object sender, RoutedEventArgs e)
		{
			this.Result = ChangeManagerFolderDialogResult.No;
			base.DialogResult = new bool?(false);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0001FE0B File Offset: 0x0001E00B
		[NullableContext(1)]
		private void ResetButton_OnClick(object sender, RoutedEventArgs e)
		{
			this.Result = ChangeManagerFolderDialogResult.Reset;
			base.DialogResult = new bool?(true);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0001FE20 File Offset: 0x0001E020
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/changemanagerfolderdialog.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0001FE50 File Offset: 0x0001E050
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.MessageTextBlock = (TextBlock)target;
				return;
			case 2:
				this.ResetButton = (Button)target;
				this.ResetButton.Click += new RoutedEventHandler(this.ResetButton_OnClick);
				return;
			case 3:
				this.YesButton = (Button)target;
				this.YesButton.Click += new RoutedEventHandler(this.YesButton_OnClick);
				return;
			case 4:
				this.NoButton = (Button)target;
				this.NoButton.Click += new RoutedEventHandler(this.NoButton_OnClick);
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x04000224 RID: 548
		internal TextBlock MessageTextBlock;

		// Token: 0x04000225 RID: 549
		internal Button ResetButton;

		// Token: 0x04000226 RID: 550
		internal Button YesButton;

		// Token: 0x04000227 RID: 551
		internal Button NoButton;

		// Token: 0x04000228 RID: 552
		private bool _contentLoaded;
	}
}
