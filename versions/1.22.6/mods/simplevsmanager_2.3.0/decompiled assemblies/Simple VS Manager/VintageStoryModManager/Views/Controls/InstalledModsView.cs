using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VintageStoryModManager.Views.Controls
{
	// Token: 0x02000040 RID: 64
	public class InstalledModsView : UserControl, IComponentConnector
	{
		// Token: 0x06000567 RID: 1383 RVA: 0x000257F5 File Offset: 0x000239F5
		public InstalledModsView()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00025804 File Offset: 0x00023A04
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/controls/installedmodsview.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00025834 File Offset: 0x00023A34
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				this.Root = (InstalledModsView)target;
				return;
			}
			if (connectionId != 2)
			{
				this._contentLoaded = true;
				return;
			}
			this.Installed_and_ModDB = (Border)target;
		}

		// Token: 0x040002E2 RID: 738
		internal InstalledModsView Root;

		// Token: 0x040002E3 RID: 739
		internal Border Installed_and_ModDB;

		// Token: 0x040002E4 RID: 740
		private bool _contentLoaded;
	}
}
