using System;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace VintageStoryModManager.Views.Dialogs
{
	// Token: 0x0200003D RID: 61
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PaletteColorEntry : ObservableObject
	{
		// Token: 0x06000544 RID: 1348 RVA: 0x00024FB8 File Offset: 0x000231B8
		public PaletteColorEntry(string key, string displayName, string hexValue, Action<PaletteColorEntry> selectColorAction)
		{
			this.Key = key;
			this.DisplayName = displayName;
			this._hexValue = hexValue;
			if (selectColorAction == null)
			{
				throw new ArgumentNullException("selectColorAction");
			}
			this._selectColorAction = selectColorAction;
			this._previewBrush = PaletteColorEntry.CreateBrush(hexValue);
			this.SelectColorCommand = new RelayCommand(delegate()
			{
				this._selectColorAction.Invoke(this);
			});
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x0002501A File Offset: 0x0002321A
		public string Key { get; }

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x00025022 File Offset: 0x00023222
		public string DisplayName { get; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x0002502A File Offset: 0x0002322A
		// (set) Token: 0x06000548 RID: 1352 RVA: 0x00025032 File Offset: 0x00023232
		public string HexValue
		{
			get
			{
				return this._hexValue;
			}
			private set
			{
				base.SetProperty<string>(ref this._hexValue, value, "HexValue");
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x00025047 File Offset: 0x00023247
		// (set) Token: 0x0600054A RID: 1354 RVA: 0x0002504F File Offset: 0x0002324F
		public Brush PreviewBrush
		{
			get
			{
				return this._previewBrush;
			}
			private set
			{
				base.SetProperty<Brush>(ref this._previewBrush, value, "PreviewBrush");
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600054B RID: 1355 RVA: 0x00025064 File Offset: 0x00023264
		public IRelayCommand SelectColorCommand { get; }

		// Token: 0x0600054C RID: 1356 RVA: 0x0002506C File Offset: 0x0002326C
		public void UpdateFromHex(string hexValue)
		{
			this.HexValue = hexValue;
			this.PreviewBrush = PaletteColorEntry.CreateBrush(hexValue);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00025084 File Offset: 0x00023284
		private static Brush CreateBrush(string hexValue)
		{
			try
			{
				object obj = ColorConverter.ConvertFromString(hexValue);
				if (obj is Color)
				{
					Color color = (Color)obj;
					SolidColorBrush solidColorBrush = new SolidColorBrush(color);
					solidColorBrush.Freeze();
					return solidColorBrush;
				}
			}
			catch
			{
			}
			return Brushes.Transparent;
		}

		// Token: 0x040002D4 RID: 724
		private readonly Action<PaletteColorEntry> _selectColorAction;

		// Token: 0x040002D5 RID: 725
		private string _hexValue;

		// Token: 0x040002D6 RID: 726
		private Brush _previewBrush;
	}
}
