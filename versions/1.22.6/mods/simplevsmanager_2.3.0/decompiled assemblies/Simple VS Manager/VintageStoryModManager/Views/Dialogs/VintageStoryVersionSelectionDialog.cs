using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using VintageStoryModManager.Services;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x0200003F RID: 63
	[NullableContext(1)]
	[Nullable(0)]
	public class VintageStoryVersionSelectionDialog : Window, IComponentConnector
	{
		// Token: 0x0600055F RID: 1375 RVA: 0x000255F8 File Offset: 0x000237F8
		public VintageStoryVersionSelectionDialog(Window owner, IEnumerable<string> versions, [Nullable(2)] string currentVersion = null)
		{
			this.InitializeComponent();
			base.Owner = owner;
			string normalizedCurrent = VersionStringUtility.Normalize(currentVersion);
			List<VintageStoryVersionSelectionDialog.VersionListItem> list;
			if (versions == null)
			{
				list = null;
			}
			else
			{
				list = Enumerable.ToList<VintageStoryVersionSelectionDialog.VersionListItem>(Enumerable.Select<string, VintageStoryVersionSelectionDialog.VersionListItem>(Enumerable.Distinct<string>(Enumerable.Select<string, string>(Enumerable.Where<string>(versions, (string version) => !string.IsNullOrWhiteSpace(version)), (string version) => version.Trim()), StringComparer.OrdinalIgnoreCase), delegate(string version)
				{
					string text = VersionStringUtility.Normalize(version);
					bool isCurrent = normalizedCurrent != null && text != null && string.Equals(text, normalizedCurrent, 5);
					return new VintageStoryVersionSelectionDialog.VersionListItem(version, isCurrent);
				}));
			}
			this._versions = (list ?? new List<VintageStoryVersionSelectionDialog.VersionListItem>());
			this.VersionsListBox.ItemsSource = this._versions;
			if (this._versions.Count > 0)
			{
				this.VersionsListBox.SelectedIndex = 0;
			}
			this.UpdateSelectButtonState();
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x000256D9 File Offset: 0x000238D9
		[Nullable(2)]
		public string SelectedVersion
		{
			[NullableContext(2)]
			get
			{
				VintageStoryVersionSelectionDialog.VersionListItem versionListItem = this.VersionsListBox.SelectedItem as VintageStoryVersionSelectionDialog.VersionListItem;
				if (versionListItem == null)
				{
					return null;
				}
				return versionListItem.Version;
			}
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x000256F6 File Offset: 0x000238F6
		private void SelectButton_OnClick(object sender, RoutedEventArgs e)
		{
			if (this.SelectedVersion == null)
			{
				return;
			}
			base.DialogResult = new bool?(true);
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0002570D File Offset: 0x0002390D
		private void VersionsListBox_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (this.SelectedVersion == null)
			{
				return;
			}
			base.DialogResult = new bool?(true);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00025724 File Offset: 0x00023924
		private void VersionsListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.UpdateSelectButtonState();
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0002572C File Offset: 0x0002392C
		private void UpdateSelectButtonState()
		{
			this.SelectButton.IsEnabled = (this.SelectedVersion != null);
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00025748 File Offset: 0x00023948
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/vintagestoryversionselectiondialog.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00025778 File Offset: 0x00023978
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				this.VersionsListBox = (ListBox)target;
				this.VersionsListBox.MouseDoubleClick += new MouseButtonEventHandler(this.VersionsListBox_OnMouseDoubleClick);
				this.VersionsListBox.SelectionChanged += new SelectionChangedEventHandler(this.VersionsListBox_OnSelectionChanged);
				return;
			}
			if (connectionId != 2)
			{
				this._contentLoaded = true;
				return;
			}
			this.SelectButton = (Button)target;
			this.SelectButton.Click += new RoutedEventHandler(this.SelectButton_OnClick);
		}

		// Token: 0x040002DE RID: 734
		private readonly List<VintageStoryVersionSelectionDialog.VersionListItem> _versions;

		// Token: 0x040002DF RID: 735
		[Nullable(0)]
		internal ListBox VersionsListBox;

		// Token: 0x040002E0 RID: 736
		[Nullable(0)]
		internal Button SelectButton;

		// Token: 0x040002E1 RID: 737
		private bool _contentLoaded;

		// Token: 0x020001E3 RID: 483
		[Nullable(0)]
		private sealed class VersionListItem
		{
			// Token: 0x060013DA RID: 5082 RVA: 0x00072C80 File Offset: 0x00070E80
			public VersionListItem(string version, bool isCurrent)
			{
				this.Version = version;
				this.Display = (isCurrent ? (version + " (current)") : version);
			}

			// Token: 0x17000452 RID: 1106
			// (get) Token: 0x060013DB RID: 5083 RVA: 0x00072CA6 File Offset: 0x00070EA6
			public string Version { get; }

			// Token: 0x17000453 RID: 1107
			// (get) Token: 0x060013DC RID: 5084 RVA: 0x00072CAE File Offset: 0x00070EAE
			public string Display { get; }
		}
	}
}
