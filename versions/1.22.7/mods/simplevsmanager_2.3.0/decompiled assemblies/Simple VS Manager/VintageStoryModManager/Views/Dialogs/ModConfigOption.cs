using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x0200002E RID: 46
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModConfigOption : INotifyPropertyChanged
	{
		// Token: 0x060004B3 RID: 1203 RVA: 0x000226C4 File Offset: 0x000208C4
		public ModConfigOption(string modId, string displayName, IEnumerable<string> configPaths, bool isSelected)
		{
			this.ModId = (modId ?? string.Empty);
			this.DisplayName = (string.IsNullOrWhiteSpace(displayName) ? this.ModId : displayName);
			this.ConfigPaths = Enumerable.ToList<string>(Enumerable.Select<string, string>(Enumerable.Where<string>(configPaths ?? ModConfigOption.EmptyPaths, (string path) => !string.IsNullOrWhiteSpace(path)), (string path) => path.Trim()));
			this._isSelected = isSelected;
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x00022763 File Offset: 0x00020963
		public string ModId { get; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x0002276B File Offset: 0x0002096B
		public string DisplayName { get; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x00022773 File Offset: 0x00020973
		public IReadOnlyList<string> ConfigPaths { get; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x0002277B File Offset: 0x0002097B
		public string ConfigPath
		{
			get
			{
				return Enumerable.FirstOrDefault<string>(this.ConfigPaths) ?? string.Empty;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x00022791 File Offset: 0x00020991
		// (set) Token: 0x060004B9 RID: 1209 RVA: 0x00022799 File Offset: 0x00020999
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (this._isSelected == value)
				{
					return;
				}
				this._isSelected = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged.Invoke(this, new PropertyChangedEventArgs("IsSelected"));
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060004BA RID: 1210 RVA: 0x000227C8 File Offset: 0x000209C8
		// (remove) Token: 0x060004BB RID: 1211 RVA: 0x00022800 File Offset: 0x00020A00
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04000282 RID: 642
		private static readonly IReadOnlyList<string> EmptyPaths = Array.Empty<string>();

		// Token: 0x04000283 RID: 643
		private bool _isSelected;
	}
}
