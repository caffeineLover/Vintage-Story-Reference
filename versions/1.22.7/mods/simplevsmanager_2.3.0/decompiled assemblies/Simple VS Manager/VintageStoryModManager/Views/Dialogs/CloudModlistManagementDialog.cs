using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using VintageStoryModManager.Models;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x02000020 RID: 32
	[NullableContext(1)]
	[Nullable(0)]
	public class CloudModlistManagementDialog : Window, IComponentConnector
	{
		// Token: 0x06000425 RID: 1061 RVA: 0x000203D8 File Offset: 0x0001E5D8
		public CloudModlistManagementDialog(Window owner, IEnumerable<CloudModlistManagementEntry> entries, Func<Task<IReadOnlyList<CloudModlistManagementEntry>>> refreshCallback, Func<CloudModlistManagementEntry, string, Task<bool>> renameCallback, Func<CloudModlistManagementEntry, Task<bool>> deleteCallback)
		{
			this.InitializeComponent();
			base.Owner = owner;
			if (refreshCallback == null)
			{
				throw new ArgumentNullException("refreshCallback");
			}
			this._refreshCallback = refreshCallback;
			if (renameCallback == null)
			{
				throw new ArgumentNullException("renameCallback");
			}
			this._renameCallback = renameCallback;
			if (deleteCallback == null)
			{
				throw new ArgumentNullException("deleteCallback");
			}
			this._deleteCallback = deleteCallback;
			this._entries = new ObservableCollection<CloudModlistManagementEntry>(entries ?? Enumerable.Empty<CloudModlistManagementEntry>());
			this.ModlistsListView.ItemsSource = this._entries;
			if (this._entries.Count > 0)
			{
				this.ModlistsListView.SelectedIndex = 0;
			}
			this.UpdateButtonStates();
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x00020482 File Offset: 0x0001E682
		[Nullable(2)]
		private CloudModlistManagementEntry SelectedEntry
		{
			[NullableContext(2)]
			get
			{
				return this.ModlistsListView.SelectedItem as CloudModlistManagementEntry;
			}
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00020494 File Offset: 0x0001E694
		private void RenameButton_OnClick(object sender, RoutedEventArgs e)
		{
			CloudModlistManagementDialog.<RenameButton_OnClick>d__8 <RenameButton_OnClick>d__;
			<RenameButton_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<RenameButton_OnClick>d__.<>4__this = this;
			<RenameButton_OnClick>d__.<>1__state = -1;
			<RenameButton_OnClick>d__.<>t__builder.Start<CloudModlistManagementDialog.<RenameButton_OnClick>d__8>(ref <RenameButton_OnClick>d__);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x000204CC File Offset: 0x0001E6CC
		private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
		{
			CloudModlistManagementDialog.<DeleteButton_OnClick>d__9 <DeleteButton_OnClick>d__;
			<DeleteButton_OnClick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<DeleteButton_OnClick>d__.<>4__this = this;
			<DeleteButton_OnClick>d__.<>1__state = -1;
			<DeleteButton_OnClick>d__.<>t__builder.Start<CloudModlistManagementDialog.<DeleteButton_OnClick>d__9>(ref <DeleteButton_OnClick>d__);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00020504 File Offset: 0x0001E704
		private Task RunOperationAsync(Func<Task<bool>> operation)
		{
			CloudModlistManagementDialog.<RunOperationAsync>d__10 <RunOperationAsync>d__;
			<RunOperationAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RunOperationAsync>d__.<>4__this = this;
			<RunOperationAsync>d__.operation = operation;
			<RunOperationAsync>d__.<>1__state = -1;
			<RunOperationAsync>d__.<>t__builder.Start<CloudModlistManagementDialog.<RunOperationAsync>d__10>(ref <RunOperationAsync>d__);
			return <RunOperationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00020550 File Offset: 0x0001E750
		private Task RefreshEntriesAsync()
		{
			CloudModlistManagementDialog.<RefreshEntriesAsync>d__11 <RefreshEntriesAsync>d__;
			<RefreshEntriesAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RefreshEntriesAsync>d__.<>4__this = this;
			<RefreshEntriesAsync>d__.<>1__state = -1;
			<RefreshEntriesAsync>d__.<>t__builder.Start<CloudModlistManagementDialog.<RefreshEntriesAsync>d__11>(ref <RefreshEntriesAsync>d__);
			return <RefreshEntriesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00020593 File Offset: 0x0001E793
		private void SetIsBusy(bool isBusy)
		{
			this._isBusy = isBusy;
			this.ModlistsListView.IsEnabled = !isBusy;
			if (isBusy)
			{
				this.RenameButton.IsEnabled = false;
				this.DeleteButton.IsEnabled = false;
				return;
			}
			this.UpdateButtonStates();
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x000205D0 File Offset: 0x0001E7D0
		private void UpdateButtonStates()
		{
			if (this._isBusy)
			{
				return;
			}
			bool isEnabled = this.SelectedEntry != null;
			this.RenameButton.IsEnabled = isEnabled;
			this.DeleteButton.IsEnabled = isEnabled;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0002060B File Offset: 0x0001E80B
		private void ModlistsListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.UpdateButtonStates();
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00020613 File Offset: 0x0001E813
		private void ModlistsListView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton != null || e.ClickCount < 2)
			{
				return;
			}
			this.RenameButton_OnClick(sender, e);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00020630 File Offset: 0x0001E830
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/cloudmodlistmanagementdialog.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00020660 File Offset: 0x0001E860
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.ModlistsListView = (ListView)target;
				this.ModlistsListView.SelectionChanged += new SelectionChangedEventHandler(this.ModlistsListView_OnSelectionChanged);
				this.ModlistsListView.MouseDoubleClick += new MouseButtonEventHandler(this.ModlistsListView_OnMouseDoubleClick);
				return;
			case 2:
				this.RenameButton = (Button)target;
				this.RenameButton.Click += new RoutedEventHandler(this.RenameButton_OnClick);
				return;
			case 3:
				this.DeleteButton = (Button)target;
				this.DeleteButton.Click += new RoutedEventHandler(this.DeleteButton_OnClick);
				return;
			default:
				this._contentLoaded = true;
				return;
			}
		}

		// Token: 0x04000237 RID: 567
		private readonly Func<CloudModlistManagementEntry, Task<bool>> _deleteCallback;

		// Token: 0x04000238 RID: 568
		private readonly ObservableCollection<CloudModlistManagementEntry> _entries;

		// Token: 0x04000239 RID: 569
		private readonly Func<Task<IReadOnlyList<CloudModlistManagementEntry>>> _refreshCallback;

		// Token: 0x0400023A RID: 570
		private readonly Func<CloudModlistManagementEntry, string, Task<bool>> _renameCallback;

		// Token: 0x0400023B RID: 571
		private bool _isBusy;

		// Token: 0x0400023C RID: 572
		[Nullable(0)]
		internal ListView ModlistsListView;

		// Token: 0x0400023D RID: 573
		[Nullable(0)]
		internal Button RenameButton;

		// Token: 0x0400023E RID: 574
		[Nullable(0)]
		internal Button DeleteButton;

		// Token: 0x0400023F RID: 575
		private bool _contentLoaded;
	}
}
