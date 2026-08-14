using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;

namespace VintageStoryModManager.ViewModels
{
	// Token: 0x02000051 RID: 81
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class UpdateModsDialogViewModel : ObservableObject
	{
		// Token: 0x0600087E RID: 2174 RVA: 0x0003400C File Offset: 0x0003220C
		public UpdateModsDialogViewModel(IEnumerable<UpdateModSelectionViewModel> mods)
		{
			if (mods == null)
			{
				throw new ArgumentNullException("mods");
			}
			this.Mods = new ObservableCollection<UpdateModSelectionViewModel>(mods);
			foreach (UpdateModSelectionViewModel updateModSelectionViewModel in this.Mods)
			{
				updateModSelectionViewModel.PropertyChanged += new PropertyChangedEventHandler(this.OnModPropertyChanged);
			}
			this.UpdateSelectAllState();
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x00034088 File Offset: 0x00032288
		public ObservableCollection<UpdateModSelectionViewModel> Mods { get; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000880 RID: 2176 RVA: 0x00034090 File Offset: 0x00032290
		public bool HasSelection
		{
			get
			{
				return Enumerable.Any<UpdateModSelectionViewModel>(this.Mods, (UpdateModSelectionViewModel m) => m.IsSelected);
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000881 RID: 2177 RVA: 0x000340BC File Offset: 0x000322BC
		// (set) Token: 0x06000882 RID: 2178 RVA: 0x000340C4 File Offset: 0x000322C4
		public bool? SelectAllState
		{
			get
			{
				return this._selectAllState;
			}
			set
			{
				if (this._isUpdatingSelectAll)
				{
					return;
				}
				if (base.SetProperty<bool?>(ref this._selectAllState, value, "SelectAllState"))
				{
					if (value == null)
					{
						return;
					}
					try
					{
						this._isUpdatingSelectAll = true;
						bool value2 = value.Value;
						foreach (UpdateModSelectionViewModel updateModSelectionViewModel in this.Mods)
						{
							updateModSelectionViewModel.IsSelected = value2;
						}
					}
					finally
					{
						this._isUpdatingSelectAll = false;
					}
					this.UpdateSelectAllState();
				}
			}
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00034160 File Offset: 0x00032360
		public IReadOnlyList<ModListItemViewModel> GetSelectedMods()
		{
			return Enumerable.ToList<ModListItemViewModel>(Enumerable.Select<UpdateModSelectionViewModel, ModListItemViewModel>(Enumerable.Where<UpdateModSelectionViewModel>(this.Mods, (UpdateModSelectionViewModel m) => m.IsSelected), (UpdateModSelectionViewModel m) => m.Mod));
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x000341C0 File Offset: 0x000323C0
		public string BuildChangelogClipboardText()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (UpdateModSelectionViewModel updateModSelectionViewModel in this.Mods)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendLine(updateModSelectionViewModel.DisplayName);
				stringBuilder.AppendLine(updateModSelectionViewModel.VersionSummary);
				if (updateModSelectionViewModel.HasChangelogs)
				{
					stringBuilder.AppendLine(updateModSelectionViewModel.ChangelogHeader);
					foreach (ModListItemViewModel.ReleaseChangelog releaseChangelog in updateModSelectionViewModel.Changelogs)
					{
						stringBuilder.AppendLine(releaseChangelog.Version);
						if (!string.IsNullOrWhiteSpace(releaseChangelog.Changelog))
						{
							stringBuilder.AppendLine(UpdateModsDialogViewModel.NormalizeLineEndings(releaseChangelog.Changelog.TrimEnd()));
						}
					}
				}
			}
			return stringBuilder.ToString().TrimEnd();
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x000342C8 File Offset: 0x000324C8
		[NullableContext(2)]
		public bool RemoveMod(UpdateModSelectionViewModel mod)
		{
			if (mod == null)
			{
				return false;
			}
			if (!this.Mods.Remove(mod))
			{
				return false;
			}
			mod.PropertyChanged -= new PropertyChangedEventHandler(this.OnModPropertyChanged);
			this.UpdateSelectAllState();
			return true;
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x000342F8 File Offset: 0x000324F8
		private void OnModPropertyChanged([Nullable(2)] object sender, PropertyChangedEventArgs e)
		{
			if (!string.Equals(e.PropertyName, "IsSelected", 4))
			{
				return;
			}
			this.UpdateSelectAllState();
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00034314 File Offset: 0x00032514
		private void UpdateSelectAllState()
		{
			bool flag;
			if (this.Mods.Count > 0)
			{
				flag = Enumerable.All<UpdateModSelectionViewModel>(this.Mods, (UpdateModSelectionViewModel m) => m.IsSelected);
			}
			else
			{
				flag = false;
			}
			bool flag2 = Enumerable.Any<UpdateModSelectionViewModel>(this.Mods, (UpdateModSelectionViewModel m) => m.IsSelected);
			bool? flag3 = flag ? new bool?(true) : (flag2 ? default(bool?) : new bool?(false));
			bool? selectAllState = this._selectAllState;
			bool? flag4 = flag3;
			if (!(selectAllState.GetValueOrDefault() == flag4.GetValueOrDefault() & selectAllState != null == (flag4 != null)))
			{
				try
				{
					this._isUpdatingSelectAll = true;
					this._selectAllState = flag3;
					base.OnPropertyChanged("SelectAllState");
				}
				finally
				{
					this._isUpdatingSelectAll = false;
				}
			}
			base.OnPropertyChanged("HasSelection");
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x00034410 File Offset: 0x00032610
		private static string NormalizeLineEndings(string value)
		{
			return value.Replace("\r\n", "\n", 4).Replace('\r', '\n').Replace("\n", Environment.NewLine, 4);
		}

		// Token: 0x04000431 RID: 1073
		private bool _isUpdatingSelectAll;

		// Token: 0x04000432 RID: 1074
		private bool? _selectAllState;
	}
}
