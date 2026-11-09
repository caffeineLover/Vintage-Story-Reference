using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Markup;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services;
using VintageStoryModManager.ViewModels;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x0200003E RID: 62
	[NullableContext(1)]
	[Nullable(0)]
	public class UpdateModsDialog : Window, IComponentConnector, IStyleConnector
	{
		// Token: 0x0600054F RID: 1359 RVA: 0x000250E4 File Offset: 0x000232E4
		public UpdateModsDialog(UserConfigurationService configuration, IEnumerable<ModListItemViewModel> mods, [Nullable(new byte[]
		{
			2,
			1,
			1
		})] IReadOnlyDictionary<ModListItemViewModel, ModReleaseInfo> releaseOverrides)
		{
			UpdateModsDialog <>4__this = this;
			ArgumentNullException.ThrowIfNull(configuration, "configuration");
			ArgumentNullException.ThrowIfNull(mods, "mods");
			this.InitializeComponent();
			this._configuration = configuration;
			List<UpdateModSelectionViewModel> mods2 = Enumerable.ToList<UpdateModSelectionViewModel>(Enumerable.Select<ModListItemViewModel, UpdateModSelectionViewModel>(mods, delegate(ModListItemViewModel mod)
			{
				bool isSelected = !<>4__this._configuration.IsModExcludedFromBulkUpdates(mod.ModId);
				ModReleaseInfo overrideRelease = null;
				if (releaseOverrides != null)
				{
					releaseOverrides.TryGetValue(mod, ref overrideRelease);
				}
				return new UpdateModSelectionViewModel(mod, isSelected, overrideRelease, <>4__this._configuration.EnableServerOptions);
			}));
			this._viewModel = new UpdateModsDialogViewModel(mods2);
			base.DataContext = this._viewModel;
			foreach (UpdateModSelectionViewModel updateModSelectionViewModel in this._viewModel.Mods)
			{
				updateModSelectionViewModel.PropertyChanged += new PropertyChangedEventHandler(this.OnSelectionPropertyChanged);
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x000251AC File Offset: 0x000233AC
		public IReadOnlyList<ModListItemViewModel> SelectedMods
		{
			get
			{
				return this._viewModel.GetSelectedMods();
			}
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x000251B9 File Offset: 0x000233B9
		private void UpdateModsDialog_OnLoaded(object sender, RoutedEventArgs e)
		{
			if (base.Owner == null)
			{
				return;
			}
			this.ConstrainMaxHeightToOwner();
			base.Owner.SizeChanged += new SizeChangedEventHandler(this.Owner_OnSizeChanged);
			base.Closed += delegate([Nullable(2)] object _, EventArgs _)
			{
				base.Owner.SizeChanged -= new SizeChangedEventHandler(this.Owner_OnSizeChanged);
			};
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x000251F3 File Offset: 0x000233F3
		private void Owner_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			this.ConstrainMaxHeightToOwner();
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x000251FC File Offset: 0x000233FC
		private void ConstrainMaxHeightToOwner()
		{
			Window owner = base.Owner;
			if (owner == null)
			{
				return;
			}
			double num = owner.ActualHeight - 300.0;
			if (num > 0.0)
			{
				base.MaxHeight = num;
			}
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00025238 File Offset: 0x00023438
		private void UpdateButton_OnClick(object sender, RoutedEventArgs e)
		{
			base.DialogResult = new bool?(true);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00025248 File Offset: 0x00023448
		private void SkipVersionButton_OnClick(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			if (button != null)
			{
				UpdateModSelectionViewModel updateModSelectionViewModel = button.DataContext as UpdateModSelectionViewModel;
				if (updateModSelectionViewModel != null)
				{
					string targetUpdateVersion = updateModSelectionViewModel.TargetUpdateVersion;
					if (string.IsNullOrWhiteSpace(targetUpdateVersion))
					{
						return;
					}
					string messageBoxText = "This will permanently skip version " + targetUpdateVersion + " and prevent any further update prompts for it.";
					if (ModManagerMessageBox.Show(this, messageBoxText, "Simple VS Manager", 4, 48, null, null, null) != 6)
					{
						return;
					}
					this._configuration.SkipModVersion(updateModSelectionViewModel.Mod.ModId, targetUpdateVersion);
					this._configuration.SetModExcludedFromBulkUpdates(updateModSelectionViewModel.Mod.ModId, false);
					updateModSelectionViewModel.Mod.RefreshSkippedUpdateState();
					this.RemoveMod(updateModSelectionViewModel);
					return;
				}
			}
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x000252E8 File Offset: 0x000234E8
		private void CopyServerCommandButton_OnClick(object sender, RoutedEventArgs e)
		{
			Button button = sender as Button;
			if (button != null)
			{
				UpdateModSelectionViewModel updateModSelectionViewModel = button.DataContext as UpdateModSelectionViewModel;
				if (updateModSelectionViewModel != null)
				{
					string latestInstallCommand = updateModSelectionViewModel.LatestInstallCommand;
					if (string.IsNullOrWhiteSpace(latestInstallCommand))
					{
						return;
					}
					try
					{
						Clipboard.SetDataObject(latestInstallCommand, true, 10, 100);
						string text = latestInstallCommand.Trim();
						string message = "Copied " + text;
						MainWindow mainWindow = base.Owner as MainWindow;
						if (mainWindow != null)
						{
							mainWindow.ReportStatus(message, false);
						}
						else
						{
							StatusLogService.AppendStatus(message, false);
						}
					}
					catch (ExternalException)
					{
						ModManagerMessageBox.Show(this, "Failed to copy the server install command. Please try again.", "Simple VS Manager", 0, 16, null, null, null);
					}
					return;
				}
			}
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00025390 File Offset: 0x00023590
		private void CopyChangelogButton_OnClick(object sender, RoutedEventArgs e)
		{
			string text = this._viewModel.BuildChangelogClipboardText();
			if (string.IsNullOrWhiteSpace(text))
			{
				return;
			}
			try
			{
				Clipboard.SetDataObject(text, true, 10, 100);
				MainWindow mainWindow = base.Owner as MainWindow;
				if (mainWindow != null)
				{
					mainWindow.ReportStatus("Copied update changelog.", false);
				}
				else
				{
					StatusLogService.AppendStatus("Copied update changelog.", false);
				}
			}
			catch (ExternalException)
			{
				ModManagerMessageBox.Show(this, "Failed to copy the update changelog. Please try again.", "Simple VS Manager", 0, 16, null, null, null);
			}
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00025414 File Offset: 0x00023614
		protected override void OnClosed(EventArgs e)
		{
			foreach (UpdateModSelectionViewModel updateModSelectionViewModel in this._viewModel.Mods)
			{
				updateModSelectionViewModel.PropertyChanged -= new PropertyChangedEventHandler(this.OnSelectionPropertyChanged);
			}
			base.OnClosed(e);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00025478 File Offset: 0x00023678
		private void RemoveMod(UpdateModSelectionViewModel selection)
		{
			selection.PropertyChanged -= new PropertyChangedEventHandler(this.OnSelectionPropertyChanged);
			this._viewModel.RemoveMod(selection);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0002549C File Offset: 0x0002369C
		private void OnSelectionPropertyChanged([Nullable(2)] object sender, PropertyChangedEventArgs e)
		{
			if (!string.Equals(e.PropertyName, "IsSelected", 4))
			{
				return;
			}
			UpdateModSelectionViewModel updateModSelectionViewModel = sender as UpdateModSelectionViewModel;
			if (updateModSelectionViewModel == null)
			{
				return;
			}
			bool isExcluded = !updateModSelectionViewModel.IsSelected;
			this._configuration.SetModExcludedFromBulkUpdates(updateModSelectionViewModel.Mod.ModId, isExcluded);
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x000254EC File Offset: 0x000236EC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			Uri uri = new Uri("/Simple VS Manager;component/views/dialogs/updatemodsdialog.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0002551C File Offset: 0x0002371C
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((UpdateModsDialog)target).Loaded += new RoutedEventHandler(this.UpdateModsDialog_OnLoaded);
				return;
			case 4:
				((Button)target).Click += new RoutedEventHandler(this.CopyChangelogButton_OnClick);
				return;
			case 5:
				this.UpdateButton = (Button)target;
				this.UpdateButton.Click += new RoutedEventHandler(this.UpdateButton_OnClick);
				return;
			}
			this._contentLoaded = true;
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x000255A2 File Offset: 0x000237A2
		[NullableContext(0)]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		[EditorBrowsable(1)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 2)
			{
				((Button)target).Click += new RoutedEventHandler(this.SkipVersionButton_OnClick);
				return;
			}
			if (connectionId != 3)
			{
				return;
			}
			((Button)target).Click += new RoutedEventHandler(this.CopyServerCommandButton_OnClick);
		}

		// Token: 0x040002DA RID: 730
		private readonly UserConfigurationService _configuration;

		// Token: 0x040002DB RID: 731
		private readonly UpdateModsDialogViewModel _viewModel;

		// Token: 0x040002DC RID: 732
		[Nullable(0)]
		internal Button UpdateButton;

		// Token: 0x040002DD RID: 733
		private bool _contentLoaded;
	}
}
