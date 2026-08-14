using System;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;

namespace VintageStoryModManager.ViewModels
{
	// Token: 0x02000050 RID: 80
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TagFilterOptionViewModel : ObservableObject
	{
		// Token: 0x0600087A RID: 2170 RVA: 0x00033FB4 File Offset: 0x000321B4
		public TagFilterOptionViewModel(string name, bool isSelected = false)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException("Tag name cannot be null or whitespace.", "name");
			}
			this.Name = name.Trim();
			this._isSelected = isSelected;
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600087B RID: 2171 RVA: 0x00033FE7 File Offset: 0x000321E7
		public string Name { get; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x00033FEF File Offset: 0x000321EF
		// (set) Token: 0x0600087D RID: 2173 RVA: 0x00033FF7 File Offset: 0x000321F7
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				base.SetProperty<bool>(ref this._isSelected, value, "IsSelected");
			}
		}

		// Token: 0x0400042F RID: 1071
		private bool _isSelected;
	}
}
