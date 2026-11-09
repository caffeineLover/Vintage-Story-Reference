using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;

namespace VintageStoryModManager.ViewModels
{
	// Token: 0x02000046 RID: 70
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class ModConfigNodeViewModel : ObservableObject
	{
		// Token: 0x0600074A RID: 1866 RVA: 0x0002E8AC File Offset: 0x0002CAAC
		protected ModConfigNodeViewModel(string name, string typeDisplay, [Nullable(2)] string displayName = null)
		{
			this.Name = name;
			this.TypeDisplay = typeDisplay;
			this.DisplayName = (displayName ?? name);
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600074B RID: 1867 RVA: 0x0002E8CE File Offset: 0x0002CACE
		public string Name { get; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x0002E8D6 File Offset: 0x0002CAD6
		public string DisplayName { get; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x0002E8DE File Offset: 0x0002CADE
		public bool HasDisplayName
		{
			get
			{
				return !string.IsNullOrWhiteSpace(this.DisplayName);
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600074E RID: 1870 RVA: 0x0002E8EE File Offset: 0x0002CAEE
		public string DisplayLabel
		{
			get
			{
				if (!this.HasDisplayName)
				{
					return this.Name;
				}
				return this.DisplayName;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x0002E905 File Offset: 0x0002CB05
		public string TypeDisplay { get; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000750 RID: 1872 RVA: 0x0002E90D File Offset: 0x0002CB0D
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public virtual ObservableCollection<ModConfigNodeViewModel> Children
		{
			[return: Nullable(new byte[]
			{
				2,
				1
			})]
			get
			{
				return null;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000751 RID: 1873 RVA: 0x0002E910 File Offset: 0x0002CB10
		public virtual bool IsEditable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000752 RID: 1874
		public abstract void ApplyChanges();
	}
}
