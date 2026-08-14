using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace VintageStoryModManager.Views.Controls
{
	// Token: 0x02000041 RID: 65
	public class NavigationHeader : UserControl, IComponentConnector
	{
		// Token: 0x0600056A RID: 1386 RVA: 0x00025861 File Offset: 0x00023A61
		public NavigationHeader()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00025870 File Offset: 0x00023A70
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/controls/navigationheader.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x000258A0 File Offset: 0x00023AA0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.Root = (NavigationHeader)target;
				return;
			case 2:
				this.ModDatabaseTabButton = (Button)target;
				return;
			case 3:
				this.ModlistsTabButton = (Button)target;
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x040002E5 RID: 741
		internal NavigationHeader Root;

		// Token: 0x040002E6 RID: 742
		internal Button ModDatabaseTabButton;

		// Token: 0x040002E7 RID: 743
		internal Button ModlistsTabButton;

		// Token: 0x040002E8 RID: 744
		private bool _contentLoaded;
	}
}
