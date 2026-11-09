using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace ModernWpf
{
	// Token: 0x0200020B RID: 523
	public class ColorPaletteResources : ResourceDictionary, ISupportInitialize
	{
		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000F2C RID: 3884 RVA: 0x0002B9F0 File Offset: 0x00029BF0
		// (set) Token: 0x06000F2D RID: 3885 RVA: 0x0002B9F8 File Offset: 0x00029BF8
		public ApplicationTheme? TargetTheme
		{
			get
			{
				return this._targetTheme;
			}
			set
			{
				if (this._targetTheme != null)
				{
					throw new InvalidOperationException("TargetTheme cannot be changed after it's set.");
				}
				ApplicationTheme? targetTheme = this._targetTheme;
				ApplicationTheme? applicationTheme = value;
				if (!(targetTheme.GetValueOrDefault() == applicationTheme.GetValueOrDefault() & targetTheme != null == (applicationTheme != null)))
				{
					this._targetTheme = value;
					this.UpdateBrushes();
				}
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000F2E RID: 3886 RVA: 0x0002BA56 File Offset: 0x00029C56
		// (set) Token: 0x06000F2F RID: 3887 RVA: 0x0002BA60 File Offset: 0x00029C60
		public Color? Accent
		{
			get
			{
				return this._accent;
			}
			set
			{
				if (this.Set(ref this._accent, value, false, "Accent"))
				{
					this.UpdateAccentShades();
					if (this.TargetTheme != null)
					{
						this.UpdateBrushes();
					}
				}
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000F30 RID: 3888 RVA: 0x0002BA9E File Offset: 0x00029C9E
		// (set) Token: 0x06000F31 RID: 3889 RVA: 0x0002BAA6 File Offset: 0x00029CA6
		public Color? AltHigh
		{
			get
			{
				return this._altHigh;
			}
			set
			{
				this.Set(ref this._altHigh, value, true, "AltHigh");
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000F32 RID: 3890 RVA: 0x0002BABC File Offset: 0x00029CBC
		// (set) Token: 0x06000F33 RID: 3891 RVA: 0x0002BAC4 File Offset: 0x00029CC4
		public Color? AltLow
		{
			get
			{
				return this._altLow;
			}
			set
			{
				this.Set(ref this._altLow, value, true, "AltLow");
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000F34 RID: 3892 RVA: 0x0002BADA File Offset: 0x00029CDA
		// (set) Token: 0x06000F35 RID: 3893 RVA: 0x0002BAE2 File Offset: 0x00029CE2
		public Color? AltMedium
		{
			get
			{
				return this._altMedium;
			}
			set
			{
				this.Set(ref this._altMedium, value, true, "AltMedium");
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000F36 RID: 3894 RVA: 0x0002BAF8 File Offset: 0x00029CF8
		// (set) Token: 0x06000F37 RID: 3895 RVA: 0x0002BB00 File Offset: 0x00029D00
		public Color? AltMediumHigh
		{
			get
			{
				return this._altMediumHigh;
			}
			set
			{
				this.Set(ref this._altMediumHigh, value, true, "AltMediumHigh");
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000F38 RID: 3896 RVA: 0x0002BB16 File Offset: 0x00029D16
		// (set) Token: 0x06000F39 RID: 3897 RVA: 0x0002BB1E File Offset: 0x00029D1E
		public Color? AltMediumLow
		{
			get
			{
				return this._altMediumLow;
			}
			set
			{
				this.Set(ref this._altMediumLow, value, true, "AltMediumLow");
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000F3A RID: 3898 RVA: 0x0002BB34 File Offset: 0x00029D34
		// (set) Token: 0x06000F3B RID: 3899 RVA: 0x0002BB3C File Offset: 0x00029D3C
		public Color? BaseHigh
		{
			get
			{
				return this._baseHigh;
			}
			set
			{
				this.Set(ref this._baseHigh, value, true, "BaseHigh");
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000F3C RID: 3900 RVA: 0x0002BB52 File Offset: 0x00029D52
		// (set) Token: 0x06000F3D RID: 3901 RVA: 0x0002BB5A File Offset: 0x00029D5A
		public Color? BaseLow
		{
			get
			{
				return this._baseLow;
			}
			set
			{
				this.Set(ref this._baseLow, value, true, "BaseLow");
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000F3E RID: 3902 RVA: 0x0002BB70 File Offset: 0x00029D70
		// (set) Token: 0x06000F3F RID: 3903 RVA: 0x0002BB78 File Offset: 0x00029D78
		public Color? BaseMedium
		{
			get
			{
				return this._baseMedium;
			}
			set
			{
				this.Set(ref this._baseMedium, value, true, "BaseMedium");
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000F40 RID: 3904 RVA: 0x0002BB8E File Offset: 0x00029D8E
		// (set) Token: 0x06000F41 RID: 3905 RVA: 0x0002BB96 File Offset: 0x00029D96
		public Color? BaseMediumHigh
		{
			get
			{
				return this._baseMediumHigh;
			}
			set
			{
				this.Set(ref this._baseMediumHigh, value, true, "BaseMediumHigh");
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000F42 RID: 3906 RVA: 0x0002BBAC File Offset: 0x00029DAC
		// (set) Token: 0x06000F43 RID: 3907 RVA: 0x0002BBB4 File Offset: 0x00029DB4
		public Color? BaseMediumLow
		{
			get
			{
				return this._baseMediumLow;
			}
			set
			{
				this.Set(ref this._baseMediumLow, value, true, "BaseMediumLow");
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000F44 RID: 3908 RVA: 0x0002BBCA File Offset: 0x00029DCA
		// (set) Token: 0x06000F45 RID: 3909 RVA: 0x0002BBD2 File Offset: 0x00029DD2
		public Color? ChromeAltLow
		{
			get
			{
				return this._chromeAltLow;
			}
			set
			{
				this.Set(ref this._chromeAltLow, value, true, "ChromeAltLow");
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000F46 RID: 3910 RVA: 0x0002BBE8 File Offset: 0x00029DE8
		// (set) Token: 0x06000F47 RID: 3911 RVA: 0x0002BBF0 File Offset: 0x00029DF0
		public Color? ChromeBlackHigh
		{
			get
			{
				return this._chromeBlackHigh;
			}
			set
			{
				this.Set(ref this._chromeBlackHigh, value, true, "ChromeBlackHigh");
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000F48 RID: 3912 RVA: 0x0002BC06 File Offset: 0x00029E06
		// (set) Token: 0x06000F49 RID: 3913 RVA: 0x0002BC0E File Offset: 0x00029E0E
		public Color? ChromeBlackLow
		{
			get
			{
				return this._chromeBlackLow;
			}
			set
			{
				this.Set(ref this._chromeBlackLow, value, true, "ChromeBlackLow");
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000F4A RID: 3914 RVA: 0x0002BC24 File Offset: 0x00029E24
		// (set) Token: 0x06000F4B RID: 3915 RVA: 0x0002BC2C File Offset: 0x00029E2C
		public Color? ChromeBlackMedium
		{
			get
			{
				return this._chromeBlackMedium;
			}
			set
			{
				this.Set(ref this._chromeBlackMedium, value, true, "ChromeBlackMedium");
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000F4C RID: 3916 RVA: 0x0002BC42 File Offset: 0x00029E42
		// (set) Token: 0x06000F4D RID: 3917 RVA: 0x0002BC4A File Offset: 0x00029E4A
		public Color? ChromeBlackMediumLow
		{
			get
			{
				return this._chromeBlackMediumLow;
			}
			set
			{
				this.Set(ref this._chromeBlackMediumLow, value, true, "ChromeBlackMediumLow");
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000F4E RID: 3918 RVA: 0x0002BC60 File Offset: 0x00029E60
		// (set) Token: 0x06000F4F RID: 3919 RVA: 0x0002BC68 File Offset: 0x00029E68
		public Color? ChromeDisabledHigh
		{
			get
			{
				return this._chromeDisabledHigh;
			}
			set
			{
				this.Set(ref this._chromeDisabledHigh, value, true, "ChromeDisabledHigh");
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000F50 RID: 3920 RVA: 0x0002BC7E File Offset: 0x00029E7E
		// (set) Token: 0x06000F51 RID: 3921 RVA: 0x0002BC86 File Offset: 0x00029E86
		public Color? ChromeDisabledLow
		{
			get
			{
				return this._chromeDisabledLow;
			}
			set
			{
				this.Set(ref this._chromeDisabledLow, value, true, "ChromeDisabledLow");
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000F52 RID: 3922 RVA: 0x0002BC9C File Offset: 0x00029E9C
		// (set) Token: 0x06000F53 RID: 3923 RVA: 0x0002BCA4 File Offset: 0x00029EA4
		public Color? ChromeGray
		{
			get
			{
				return this._chromeGray;
			}
			set
			{
				this.Set(ref this._chromeGray, value, true, "ChromeGray");
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000F54 RID: 3924 RVA: 0x0002BCBA File Offset: 0x00029EBA
		// (set) Token: 0x06000F55 RID: 3925 RVA: 0x0002BCC2 File Offset: 0x00029EC2
		public Color? ChromeHigh
		{
			get
			{
				return this._chromeHigh;
			}
			set
			{
				this.Set(ref this._chromeHigh, value, true, "ChromeHigh");
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000F56 RID: 3926 RVA: 0x0002BCD8 File Offset: 0x00029ED8
		// (set) Token: 0x06000F57 RID: 3927 RVA: 0x0002BCE0 File Offset: 0x00029EE0
		public Color? ChromeLow
		{
			get
			{
				return this._chromeLow;
			}
			set
			{
				this.Set(ref this._chromeLow, value, true, "ChromeLow");
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000F58 RID: 3928 RVA: 0x0002BCF6 File Offset: 0x00029EF6
		// (set) Token: 0x06000F59 RID: 3929 RVA: 0x0002BCFE File Offset: 0x00029EFE
		public Color? ChromeMedium
		{
			get
			{
				return this._chromeMedium;
			}
			set
			{
				this.Set(ref this._chromeMedium, value, true, "ChromeMedium");
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000F5A RID: 3930 RVA: 0x0002BD14 File Offset: 0x00029F14
		// (set) Token: 0x06000F5B RID: 3931 RVA: 0x0002BD1C File Offset: 0x00029F1C
		public Color? ChromeMediumLow
		{
			get
			{
				return this._chromeMediumLow;
			}
			set
			{
				this.Set(ref this._chromeMediumLow, value, true, "ChromeMediumLow");
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000F5C RID: 3932 RVA: 0x0002BD32 File Offset: 0x00029F32
		// (set) Token: 0x06000F5D RID: 3933 RVA: 0x0002BD3A File Offset: 0x00029F3A
		public Color? ChromeWhite
		{
			get
			{
				return this._chromeWhite;
			}
			set
			{
				this.Set(ref this._chromeWhite, value, true, "ChromeWhite");
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000F5E RID: 3934 RVA: 0x0002BD50 File Offset: 0x00029F50
		// (set) Token: 0x06000F5F RID: 3935 RVA: 0x0002BD58 File Offset: 0x00029F58
		public Color? ErrorText
		{
			get
			{
				return this._errorText;
			}
			set
			{
				this.Set(ref this._errorText, value, true, "ErrorText");
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000F60 RID: 3936 RVA: 0x0002BD6E File Offset: 0x00029F6E
		// (set) Token: 0x06000F61 RID: 3937 RVA: 0x0002BD76 File Offset: 0x00029F76
		public Color? ListLow
		{
			get
			{
				return this._listLow;
			}
			set
			{
				this.Set(ref this._listLow, value, true, "ListLow");
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000F62 RID: 3938 RVA: 0x0002BD8C File Offset: 0x00029F8C
		// (set) Token: 0x06000F63 RID: 3939 RVA: 0x0002BD94 File Offset: 0x00029F94
		public Color? ListMedium
		{
			get
			{
				return this._listMedium;
			}
			set
			{
				this.Set(ref this._listMedium, value, true, "ListMedium");
			}
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x0002BDAC File Offset: 0x00029FAC
		private bool Set(ref Color? storage, Color? value, bool updateBrushes = true, [CallerMemberName] string propertyName = null)
		{
			if (storage != value)
			{
				string text = "System" + propertyName + "Color";
				if (storage != null)
				{
					base.Remove(text);
				}
				storage = value;
				if (storage != null)
				{
					base.Add(text, storage.Value);
				}
				if (this.TargetTheme != null && updateBrushes)
				{
					this.UpdateBrushes();
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x0002BE54 File Offset: 0x0002A054
		private void UpdateAccentShades()
		{
			if (this.IsInitializePending)
			{
				return;
			}
			if (this.Accent != null)
			{
				ColorsHelper.UpdateShades(this, this.Accent.Value);
				return;
			}
			ColorsHelper.RemoveShades(this);
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x0002BE98 File Offset: 0x0002A098
		private void UpdateBrushes()
		{
			if (this.IsInitializePending)
			{
				return;
			}
			if (base.MergedDictionaries.Count > 0)
			{
				base.MergedDictionaries.Clear();
			}
			if (this.TargetTheme == null || base.Count == 0)
			{
				return;
			}
			ResourceDictionary defaultThemeDictionary = ThemeManager.GetDefaultThemeDictionary(this.TargetTheme.Value.ToString());
			ResourceDictionary resourceDictionary = new ResourceDictionary();
			Dictionary<SolidColorBrush, SolidColorBrush> dictionary = new Dictionary<SolidColorBrush, SolidColorBrush>();
			foreach (object obj in defaultThemeDictionary)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				SolidColorBrush solidColorBrush = dictionaryEntry.Value as SolidColorBrush;
				if (solidColorBrush != null)
				{
					object colorKey = ThemeResourceHelper.GetColorKey(solidColorBrush);
					if (colorKey != null && base.Contains(colorKey))
					{
						SolidColorBrush solidColorBrush2;
						if (!dictionary.TryGetValue(solidColorBrush, ref solidColorBrush2))
						{
							solidColorBrush2 = solidColorBrush.CloneCurrentValue();
							solidColorBrush2.Color = (Color)base[colorKey];
							dictionary[solidColorBrush] = solidColorBrush2;
						}
						resourceDictionary.Add(dictionaryEntry.Key, solidColorBrush2);
					}
				}
			}
			base.MergedDictionaries.Add(resourceDictionary);
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000F67 RID: 3943 RVA: 0x0002BFD0 File Offset: 0x0002A1D0
		// (set) Token: 0x06000F68 RID: 3944 RVA: 0x0002BFD8 File Offset: 0x0002A1D8
		private bool IsInitializePending { get; set; }

		// Token: 0x06000F69 RID: 3945 RVA: 0x0002BFE1 File Offset: 0x0002A1E1
		public void BeginInit()
		{
			base.BeginInit();
			this.IsInitializePending = true;
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x0002BFF0 File Offset: 0x0002A1F0
		public void EndInit()
		{
			base.EndInit();
			this.IsInitializePending = false;
			if (this.Accent != null)
			{
				this.UpdateAccentShades();
			}
			this.UpdateBrushes();
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x0002C026 File Offset: 0x0002A226
		void ISupportInitialize.BeginInit()
		{
			this.BeginInit();
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x0002C02E File Offset: 0x0002A22E
		void ISupportInitialize.EndInit()
		{
			this.EndInit();
		}

		// Token: 0x04000339 RID: 825
		private ApplicationTheme? _targetTheme;

		// Token: 0x0400033A RID: 826
		private Color? _accent;

		// Token: 0x0400033B RID: 827
		private Color? _altHigh;

		// Token: 0x0400033C RID: 828
		private Color? _altLow;

		// Token: 0x0400033D RID: 829
		private Color? _altMedium;

		// Token: 0x0400033E RID: 830
		private Color? _altMediumHigh;

		// Token: 0x0400033F RID: 831
		private Color? _altMediumLow;

		// Token: 0x04000340 RID: 832
		private Color? _baseHigh;

		// Token: 0x04000341 RID: 833
		private Color? _baseLow;

		// Token: 0x04000342 RID: 834
		private Color? _baseMedium;

		// Token: 0x04000343 RID: 835
		private Color? _baseMediumHigh;

		// Token: 0x04000344 RID: 836
		private Color? _baseMediumLow;

		// Token: 0x04000345 RID: 837
		private Color? _chromeAltLow;

		// Token: 0x04000346 RID: 838
		private Color? _chromeBlackHigh;

		// Token: 0x04000347 RID: 839
		private Color? _chromeBlackLow;

		// Token: 0x04000348 RID: 840
		private Color? _chromeBlackMedium;

		// Token: 0x04000349 RID: 841
		private Color? _chromeBlackMediumLow;

		// Token: 0x0400034A RID: 842
		private Color? _chromeDisabledHigh;

		// Token: 0x0400034B RID: 843
		private Color? _chromeDisabledLow;

		// Token: 0x0400034C RID: 844
		private Color? _chromeGray;

		// Token: 0x0400034D RID: 845
		private Color? _chromeHigh;

		// Token: 0x0400034E RID: 846
		private Color? _chromeLow;

		// Token: 0x0400034F RID: 847
		private Color? _chromeMedium;

		// Token: 0x04000350 RID: 848
		private Color? _chromeMediumLow;

		// Token: 0x04000351 RID: 849
		private Color? _chromeWhite;

		// Token: 0x04000352 RID: 850
		private Color? _errorText;

		// Token: 0x04000353 RID: 851
		private Color? _listLow;

		// Token: 0x04000354 RID: 852
		private Color? _listMedium;
	}
}
