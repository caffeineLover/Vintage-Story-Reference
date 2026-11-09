using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VintageStoryModManager.Views
{
	// Token: 0x02000013 RID: 19
	public class BulkCompatibilityPromptWindow : Window, IComponentConnector
	{
		// Token: 0x06000119 RID: 281 RVA: 0x00005D88 File Offset: 0x00003F88
		[NullableContext(1)]
		public BulkCompatibilityPromptWindow(string message, string latestVersion, [Nullable(2)] string compatibleVersion, bool hasCompatible)
		{
			this.InitializeComponent();
			this.MessageTextBlock.Text = message;
			this.InstallLatestButton.Content = (string.IsNullOrWhiteSpace(latestVersion) ? "Install latest release" : ("Install latest release (" + latestVersion + ")"));
			this.InstallLatestAllButton.Content = (string.IsNullOrWhiteSpace(latestVersion) ? "Install latest release for all mods" : ("Install latest release for all mods (" + latestVersion + ")"));
			if (hasCompatible && !string.IsNullOrWhiteSpace(compatibleVersion) && !string.Equals(latestVersion, compatibleVersion, 5))
			{
				this.InstallCompatibleButton.Content = "Install latest compatible version (" + compatibleVersion + ")";
				this.InstallCompatibleAllButton.Content = "Install latest compatible version for all mods (" + compatibleVersion + ")";
				this.InstallCompatibleButton.Visibility = 0;
				this.InstallCompatibleAllButton.Visibility = 0;
			}
			else
			{
				this.InstallCompatibleButton.Visibility = 2;
				this.InstallCompatibleAllButton.Visibility = 2;
			}
			this.SkipButton.Content = "Skip this mod";
			this.CancelButton.Content = "Cancel updates";
			this.Decision = CompatibilityDecisionKind.Skip;
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00005EAC File Offset: 0x000040AC
		// (set) Token: 0x0600011B RID: 283 RVA: 0x00005EB4 File Offset: 0x000040B4
		internal CompatibilityDecisionKind Decision { get; private set; }

		// Token: 0x0600011C RID: 284 RVA: 0x00005EBD File Offset: 0x000040BD
		[NullableContext(1)]
		private void InstallLatestButton_OnClick(object sender, RoutedEventArgs e)
		{
			this.Decision = CompatibilityDecisionKind.Latest;
			base.DialogResult = new bool?(true);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005ED2 File Offset: 0x000040D2
		[NullableContext(1)]
		private void InstallLatestAllButton_OnClick(object sender, RoutedEventArgs e)
		{
			this.Decision = CompatibilityDecisionKind.LatestForAll;
			base.DialogResult = new bool?(true);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00005EE7 File Offset: 0x000040E7
		[NullableContext(1)]
		private void InstallCompatibleButton_OnClick(object sender, RoutedEventArgs e)
		{
			this.Decision = CompatibilityDecisionKind.LatestCompatible;
			base.DialogResult = new bool?(true);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005EFC File Offset: 0x000040FC
		[NullableContext(1)]
		private void InstallCompatibleAllButton_OnClick(object sender, RoutedEventArgs e)
		{
			this.Decision = CompatibilityDecisionKind.LatestCompatibleForAll;
			base.DialogResult = new bool?(true);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005F11 File Offset: 0x00004111
		[NullableContext(1)]
		private void SkipButton_OnClick(object sender, RoutedEventArgs e)
		{
			this.Decision = CompatibilityDecisionKind.Skip;
			base.DialogResult = new bool?(true);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00005F26 File Offset: 0x00004126
		[NullableContext(1)]
		private void CancelButton_OnClick(object sender, RoutedEventArgs e)
		{
			this.Decision = CompatibilityDecisionKind.Abort;
			base.DialogResult = new bool?(true);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00005F3C File Offset: 0x0000413C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/bulkcompatibilitypromptwindow.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00005F6C File Offset: 0x0000416C
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
				this.InstallLatestButton = (Button)target;
				this.InstallLatestButton.Click += new RoutedEventHandler(this.InstallLatestButton_OnClick);
				return;
			case 3:
				this.InstallLatestAllButton = (Button)target;
				this.InstallLatestAllButton.Click += new RoutedEventHandler(this.InstallLatestAllButton_OnClick);
				return;
			case 4:
				this.InstallCompatibleButton = (Button)target;
				this.InstallCompatibleButton.Click += new RoutedEventHandler(this.InstallCompatibleButton_OnClick);
				return;
			case 5:
				this.InstallCompatibleAllButton = (Button)target;
				this.InstallCompatibleAllButton.Click += new RoutedEventHandler(this.InstallCompatibleAllButton_OnClick);
				return;
			case 6:
				this.SkipButton = (Button)target;
				this.SkipButton.Click += new RoutedEventHandler(this.SkipButton_OnClick);
				return;
			case 7:
				this.CancelButton = (Button)target;
				this.CancelButton.Click += new RoutedEventHandler(this.CancelButton_OnClick);
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x040000C5 RID: 197
		internal TextBlock MessageTextBlock;

		// Token: 0x040000C6 RID: 198
		internal Button InstallLatestButton;

		// Token: 0x040000C7 RID: 199
		internal Button InstallLatestAllButton;

		// Token: 0x040000C8 RID: 200
		internal Button InstallCompatibleButton;

		// Token: 0x040000C9 RID: 201
		internal Button InstallCompatibleAllButton;

		// Token: 0x040000CA RID: 202
		internal Button SkipButton;

		// Token: 0x040000CB RID: 203
		internal Button CancelButton;

		// Token: 0x040000CC RID: 204
		private bool _contentLoaded;
	}
}
