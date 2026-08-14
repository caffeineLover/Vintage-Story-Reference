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
	// Token: 0x02000028 RID: 40
	[NullableContext(1)]
	[Nullable(0)]
	public class GuideDialogWindow : Window, IComponentConnector
	{
		// Token: 0x06000475 RID: 1141 RVA: 0x000218AE File Offset: 0x0001FAAE
		public GuideDialogWindow(string managerDirectory, [Nullable(2)] string cachedModsDirectory, string configurationFilePath)
		{
			this.InitializeComponent();
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x000218BC File Offset: 0x0001FABC
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

		// Token: 0x06000477 RID: 1143 RVA: 0x000218D8 File Offset: 0x0001FAD8
		private void OnHyperlinkRequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			e.Handled = true;
			Hyperlink hyperlink = sender as Hyperlink;
			if (hyperlink != null)
			{
				string text = hyperlink.Tag as string;
				if (text != null && !string.IsNullOrWhiteSpace(text))
				{
					string text2 = GuideDialogWindow.ResolveDestinationPath(text);
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

		// Token: 0x06000478 RID: 1144 RVA: 0x00021988 File Offset: 0x0001FB88
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

		// Token: 0x06000479 RID: 1145 RVA: 0x000219C1 File Offset: 0x0001FBC1
		private void OnCloseButtonClick(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(true);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x000219D0 File Offset: 0x0001FBD0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/guidedialogwindow.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00021A00 File Offset: 0x0001FC00
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				((Button)target).Click += new RoutedEventHandler(this.OnCloseButtonClick);
				return;
			}
			this._contentLoaded = true;
		}

		// Token: 0x04000261 RID: 609
		private bool _contentLoaded;
	}
}
