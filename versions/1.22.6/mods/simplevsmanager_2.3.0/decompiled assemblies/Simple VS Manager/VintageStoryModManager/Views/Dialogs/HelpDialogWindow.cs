using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Navigation;
using VintageStoryModManager.Services;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x02000029 RID: 41
	[NullableContext(1)]
	[Nullable(0)]
	public class HelpDialogWindow : Window, IComponentConnector
	{
		// Token: 0x0600047C RID: 1148 RVA: 0x00021A25 File Offset: 0x0001FC25
		public HelpDialogWindow(string managerDirectory, [Nullable(2)] string cachedModsDirectory)
		{
			this.InitializeComponent();
			this.ConfigureCachedModsHyperlink(this.ModDBlink, cachedModsDirectory, true);
			this.ConfigureFirebaseHyperlink(this.FirebaseFileHyperlink, managerDirectory, true);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00021A4F File Offset: 0x0001FC4F
		private void ConfigureCachedModsHyperlink(Hyperlink hyperlink, [Nullable(2)] string path, bool ensureDirectory)
		{
			this.ConfigureHyperlinkCommon(hyperlink, path, ensureDirectory);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00021A5A File Offset: 0x0001FC5A
		private void ConfigureFirebaseHyperlink(Hyperlink hyperlink, [Nullable(2)] string path, bool ensureDirectory)
		{
			this.ConfigureHyperlinkCommon(hyperlink, path, ensureDirectory);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00021A68 File Offset: 0x0001FC68
		private void ConfigureHyperlinkCommon(Hyperlink hyperlink, [Nullable(2)] string path, bool ensureDirectory)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				hyperlink.IsEnabled = false;
				hyperlink.Tag = null;
				hyperlink.NavigateUri = null;
				hyperlink.ToolTip = "Location not available";
				hyperlink.Foreground = SystemColors.GrayTextBrush;
				hyperlink.TextDecorations = null;
				return;
			}
			if (ensureDirectory)
			{
				try
				{
					Directory.CreateDirectory(path);
				}
				catch (Exception)
				{
				}
			}
			hyperlink.Tag = path;
			hyperlink.NavigateUri = HelpDialogWindow.TryCreateUri(path);
			hyperlink.ToolTip = path;
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00021AEC File Offset: 0x0001FCEC
		[return: Nullable(2)]
		private static Uri TryCreateUri(string path)
		{
			Uri result;
			if (!Uri.TryCreate(path, 1, ref result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00021B07 File Offset: 0x0001FD07
		private void OnFirebaseHyperlinkRequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			this.HandleHyperlinkNavigation(sender, e);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00021B11 File Offset: 0x0001FD11
		private void OnBackupHyperlinkRequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			this.HandleHyperlinkNavigation(sender, e);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00021B1B File Offset: 0x0001FD1B
		private void OnCachedModsHyperlinkRequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			this.HandleHyperlinkNavigation(sender, e);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00021B28 File Offset: 0x0001FD28
		private void OnModDBlinkNavigate(object sender, RequestNavigateEventArgs e)
		{
			e.Handled = true;
			string managerModDatabaseUrl = DevConfig.ManagerModDatabaseUrl;
			if (InternetAccessManager.IsInternetAccessDisabled)
			{
				ModManagerMessageBox.Show(this, "Internet access is disabled. Enable Internet Access in the File menu to open web links.", "Simple VS Manager", 0, 64, null, null, null);
				return;
			}
			try
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = managerModDatabaseUrl,
					UseShellExecute = true
				});
			}
			catch (Exception ex)
			{
				ModManagerMessageBox.Show(this, "Failed to open the web page:\n" + ex.Message, "Simple VS Manager", 0, 16, null, null, null);
			}
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00021BB4 File Offset: 0x0001FDB4
		private void HandleHyperlinkNavigation(object sender, RequestNavigateEventArgs e)
		{
			e.Handled = true;
			Hyperlink hyperlink = sender as Hyperlink;
			if (hyperlink != null)
			{
				string text = hyperlink.Tag as string;
				if (text != null && !string.IsNullOrWhiteSpace(text))
				{
					string text2 = HelpDialogWindow.ResolveDestinationPath(text);
					if (text2 == null)
					{
						ModManagerMessageBox.Show(this, "The location could not be found:\n" + text, "Simple VS Manager", 0, 64, null, null, null);
						return;
					}
					try
					{
						Process.Start(new ProcessStartInfo
						{
							FileName = text2,
							UseShellExecute = true
						});
					}
					catch (Exception ex)
					{
						ModManagerMessageBox.Show(this, "Failed to open the location:\n" + ex.Message, "Simple VS Manager", 0, 16, null, null, null);
					}
					return;
				}
			}
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00021C64 File Offset: 0x0001FE64
		[return: Nullable(2)]
		private static string ResolveDestinationPath(string path)
		{
			if (Directory.Exists(path) || File.Exists(path))
			{
				return path;
			}
			string directoryName = Path.GetDirectoryName(path);
			if (!string.IsNullOrWhiteSpace(directoryName) && Directory.Exists(directoryName))
			{
				return directoryName;
			}
			return null;
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00021C9D File Offset: 0x0001FE9D
		private void OnHelpCloseButtonClick(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(true);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00021CAC File Offset: 0x0001FEAC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/helpdialogwindow.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00021CDC File Offset: 0x0001FEDC
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.DiscordLink = (Hyperlink)target;
				this.DiscordLink.RequestNavigate += new RequestNavigateEventHandler(this.OnBackupHyperlinkRequestNavigate);
				return;
			case 2:
				this.FirebaseFileHyperlink = (Hyperlink)target;
				this.FirebaseFileHyperlink.RequestNavigate += new RequestNavigateEventHandler(this.OnFirebaseHyperlinkRequestNavigate);
				return;
			case 3:
				this.ModDBlink = (Hyperlink)target;
				this.ModDBlink.RequestNavigate += new RequestNavigateEventHandler(this.OnModDBlinkNavigate);
				return;
			case 4:
				((Button)target).Click += new RoutedEventHandler(this.OnHelpCloseButtonClick);
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x04000262 RID: 610
		[Nullable(0)]
		internal Hyperlink DiscordLink;

		// Token: 0x04000263 RID: 611
		[Nullable(0)]
		internal Hyperlink FirebaseFileHyperlink;

		// Token: 0x04000264 RID: 612
		[Nullable(0)]
		internal Hyperlink ModDBlink;

		// Token: 0x04000265 RID: 613
		private bool _contentLoaded;
	}
}
