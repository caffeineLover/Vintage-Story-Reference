using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ModernWpf.Controls;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x02000038 RID: 56
	public class RestoreBackupDialog : Window, IComponentConnector
	{
		// Token: 0x060004FC RID: 1276 RVA: 0x00023745 File Offset: 0x00021945
		public RestoreBackupDialog()
		{
			this.InitializeComponent();
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x00023753 File Offset: 0x00021953
		public bool RestoreConfigurations
		{
			get
			{
				return this.RestoreConfigsToggle.IsOn;
			}
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00023760 File Offset: 0x00021960
		[NullableContext(1)]
		private void RestoreButton_OnClick(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(true);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0002376E File Offset: 0x0002196E
		[NullableContext(1)]
		private void CancelButton_OnClick(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(false);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0002377C File Offset: 0x0002197C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/restorebackupdialog.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x000237AC File Offset: 0x000219AC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.RestoreConfigsToggle = (ToggleSwitch)target;
				return;
			case 2:
				((Button)target).Click += new RoutedEventHandler(this.RestoreButton_OnClick);
				return;
			case 3:
				((Button)target).Click += new RoutedEventHandler(this.CancelButton_OnClick);
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x040002B1 RID: 689
		internal ToggleSwitch RestoreConfigsToggle;

		// Token: 0x040002B2 RID: 690
		private bool _contentLoaded;
	}
}
