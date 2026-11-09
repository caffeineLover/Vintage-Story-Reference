using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;

namespace VintageStoryModManager.ViewModels
{
	// Token: 0x02000045 RID: 69
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModConfigEditorViewModel : ObservableObject
	{
		// Token: 0x0600073D RID: 1853 RVA: 0x0002E634 File Offset: 0x0002C834
		public ModConfigEditorViewModel(string modDisplayName, IEnumerable<string> filePaths)
		{
			if (string.IsNullOrWhiteSpace(modDisplayName))
			{
				throw new ArgumentException("Mod name is required.", "modDisplayName");
			}
			this.ModDisplayName = modDisplayName;
			this.WindowTitle = "Edit Config - " + this.ModDisplayName;
			foreach (string filePath in (filePaths ?? Array.Empty<string>()))
			{
				this.AddConfiguration(filePath, false);
			}
			this.SelectedConfiguration = Enumerable.FirstOrDefault<ModConfigurationViewModel>(this.Configurations);
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x0002E6E0 File Offset: 0x0002C8E0
		public string ModDisplayName { get; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x0002E6E8 File Offset: 0x0002C8E8
		public string WindowTitle { get; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x0002E6F0 File Offset: 0x0002C8F0
		public ObservableCollection<ModConfigurationViewModel> Configurations { get; } = new ObservableCollection<ModConfigurationViewModel>();

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x0002E6F8 File Offset: 0x0002C8F8
		// (set) Token: 0x06000742 RID: 1858 RVA: 0x0002E700 File Offset: 0x0002C900
		[Nullable(2)]
		public ModConfigurationViewModel SelectedConfiguration
		{
			[NullableContext(2)]
			get
			{
				return this._selectedConfiguration;
			}
			[NullableContext(2)]
			set
			{
				if (base.SetProperty<ModConfigurationViewModel>(ref this._selectedConfiguration, value, "SelectedConfiguration"))
				{
					base.OnPropertyChanged("FilePath");
					base.OnPropertyChanged("HasConfigurations");
				}
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x0002E72C File Offset: 0x0002C92C
		public bool HasConfigurations
		{
			get
			{
				return this.SelectedConfiguration != null;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x0002E73A File Offset: 0x0002C93A
		public string FilePath
		{
			get
			{
				ModConfigurationViewModel selectedConfiguration = this.SelectedConfiguration;
				return ((selectedConfiguration != null) ? selectedConfiguration.FilePath : null) ?? string.Empty;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x0002E757 File Offset: 0x0002C957
		public IReadOnlyList<string> ConfigPaths
		{
			get
			{
				return Enumerable.ToList<string>(Enumerable.Select<ModConfigurationViewModel, string>(this.Configurations, (ModConfigurationViewModel config) => config.FilePath));
			}
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0002E788 File Offset: 0x0002C988
		public ModConfigurationViewModel AddConfiguration(string filePath, bool select = true)
		{
			ModConfigurationViewModel modConfigurationViewModel = new ModConfigurationViewModel(filePath);
			this.Configurations.Add(modConfigurationViewModel);
			if (select || this.SelectedConfiguration == null)
			{
				this.SelectedConfiguration = modConfigurationViewModel;
			}
			return modConfigurationViewModel;
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0002E7BC File Offset: 0x0002C9BC
		public bool RemoveSelectedConfiguration()
		{
			if (this.SelectedConfiguration == null)
			{
				return false;
			}
			int num = this.Configurations.IndexOf(this.SelectedConfiguration);
			if (num < 0)
			{
				return false;
			}
			this.Configurations.RemoveAt(num);
			if (this.Configurations.Count == 0)
			{
				this.SelectedConfiguration = null;
			}
			else
			{
				int num2 = Math.Min(num, this.Configurations.Count - 1);
				this.SelectedConfiguration = this.Configurations[num2];
			}
			base.OnPropertyChanged("FilePath");
			return true;
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0002E83F File Offset: 0x0002CA3F
		public void ReplaceConfigurationFile(string filePath)
		{
			ModConfigurationViewModel selectedConfiguration = this.SelectedConfiguration;
			if (selectedConfiguration != null)
			{
				selectedConfiguration.ReplaceConfigurationFile(filePath);
			}
			base.OnPropertyChanged("FilePath");
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0002E860 File Offset: 0x0002CA60
		public void Save()
		{
			foreach (ModConfigurationViewModel modConfigurationViewModel in this.Configurations)
			{
				modConfigurationViewModel.Save();
			}
		}

		// Token: 0x040003BA RID: 954
		[Nullable(2)]
		private ModConfigurationViewModel _selectedConfiguration;
	}
}
