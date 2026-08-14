using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Views
{
	// Token: 0x0200001A RID: 26
	[NullableContext(1)]
	[Nullable(0)]
	public class SelectableItem : INotifyPropertyChanged
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x0001EBF0 File Offset: 0x0001CDF0
		// (set) Token: 0x060003E5 RID: 997 RVA: 0x0001EBF8 File Offset: 0x0001CDF8
		public object Item { get; set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x0001EC01 File Offset: 0x0001CE01
		// (set) Token: 0x060003E7 RID: 999 RVA: 0x0001EC09 File Offset: 0x0001CE09
		public string DisplayText { get; set; } = string.Empty;

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x0001EC12 File Offset: 0x0001CE12
		// (set) Token: 0x060003E9 RID: 1001 RVA: 0x0001EC1A File Offset: 0x0001CE1A
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (this._isSelected != value)
				{
					this._isSelected = value;
					PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
					if (propertyChanged == null)
					{
						return;
					}
					propertyChanged.Invoke(this, new PropertyChangedEventArgs("IsSelected"));
				}
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060003EA RID: 1002 RVA: 0x0001EC48 File Offset: 0x0001CE48
		// (remove) Token: 0x060003EB RID: 1003 RVA: 0x0001EC80 File Offset: 0x0001CE80
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x040001F9 RID: 505
		private bool _isSelected;
	}
}
