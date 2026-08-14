using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace ModernWpf
{
	// Token: 0x0200022B RID: 555
	public class ThemeResources : ResourceDictionaryEx, ISupportInitialize
	{
		// Token: 0x0600102C RID: 4140 RVA: 0x0002E3E0 File Offset: 0x0002C5E0
		public ThemeResources()
		{
			if (ThemeResources.Current == null)
			{
				ThemeResources.Current = this;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x0600102D RID: 4141 RVA: 0x0002E3F5 File Offset: 0x0002C5F5
		// (set) Token: 0x0600102E RID: 4142 RVA: 0x0002E3FC File Offset: 0x0002C5FC
		internal static ThemeResources Current
		{
			get
			{
				return ThemeResources._current;
			}
			set
			{
				if (ThemeResources._current != null)
				{
					throw new InvalidOperationException("Current cannot be changed after it's set.");
				}
				ThemeResources._current = value;
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x0600102F RID: 4143 RVA: 0x0002E416 File Offset: 0x0002C616
		// (set) Token: 0x06001030 RID: 4144 RVA: 0x0002E424 File Offset: 0x0002C624
		public ApplicationTheme? RequestedTheme
		{
			get
			{
				return ThemeManager.Current.ApplicationTheme;
			}
			set
			{
				ApplicationTheme? applicationTheme = ThemeManager.Current.ApplicationTheme;
				ApplicationTheme? applicationTheme2 = value;
				if (!(applicationTheme.GetValueOrDefault() == applicationTheme2.GetValueOrDefault() & applicationTheme != null == (applicationTheme2 != null)))
				{
					ThemeManager.Current.SetCurrentValue(ThemeManager.ApplicationThemeProperty, value);
					if (DesignMode.DesignModeEnabled)
					{
						this.UpdateDesignTimeThemeDictionary();
					}
				}
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06001031 RID: 4145 RVA: 0x0002E483 File Offset: 0x0002C683
		// (set) Token: 0x06001032 RID: 4146 RVA: 0x0002E490 File Offset: 0x0002C690
		public Color? AccentColor
		{
			get
			{
				return ThemeManager.Current.AccentColor;
			}
			set
			{
				if (ThemeManager.Current.AccentColor != value)
				{
					ThemeManager.Current.SetCurrentValue(ThemeManager.AccentColorProperty, value);
					if (DesignMode.DesignModeEnabled)
					{
						this.UpdateDesignTimeSystemColors();
					}
				}
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06001033 RID: 4147 RVA: 0x0002E500 File Offset: 0x0002C700
		// (set) Token: 0x06001034 RID: 4148 RVA: 0x0002E508 File Offset: 0x0002C708
		public bool CanBeAccessedAcrossThreads
		{
			get
			{
				return this._canBeAccessedAcrossThreads;
			}
			set
			{
				if (DesignMode.DesignModeEnabled)
				{
					return;
				}
				if (this.IsInitialized)
				{
					throw new InvalidOperationException();
				}
				this._canBeAccessedAcrossThreads = value;
			}
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x0002E527 File Offset: 0x0002C727
		private void DesignTimeInit()
		{
			this.UpdateDesignTimeSystemColors();
			this.UpdateDesignTimeThemeDictionary();
			SystemParameters.StaticPropertyChanged += delegate([Nullable(2)] object sender, PropertyChangedEventArgs e)
			{
				if (e.PropertyName == "HighContrast")
				{
					this.UpdateDesignTimeThemeDictionary();
				}
			};
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x0002E548 File Offset: 0x0002C748
		private void UpdateDesignTimeSystemColors()
		{
			if (this.IsInitializePending)
			{
				return;
			}
			ResourceDictionary designTimeSystemColors = this.GetDesignTimeSystemColors();
			base.MergedDictionaries.InsertOrReplace(0, designTimeSystemColors);
			ThemeManager.UpdateThemeBrushes(designTimeSystemColors);
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0002E578 File Offset: 0x0002C778
		private void UpdateDesignTimeThemeDictionary()
		{
			if (this.IsInitializePending)
			{
				return;
			}
			if (SystemParameters.HighContrast)
			{
				this.EnsureHighContrastResources();
				this.<UpdateDesignTimeThemeDictionary>g__updateTo|20_0(this._highContrastResources);
				return;
			}
			ApplicationTheme valueOrDefault = this.RequestedTheme.GetValueOrDefault();
			if (valueOrDefault == ApplicationTheme.Light)
			{
				this.EnsureLightResources();
				this.<UpdateDesignTimeThemeDictionary>g__updateTo|20_0(this._lightResources);
				return;
			}
			if (valueOrDefault != ApplicationTheme.Dark)
			{
				return;
			}
			this.EnsureDarkResources();
			this.<UpdateDesignTimeThemeDictionary>g__updateTo|20_0(this._darkResources);
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x0002E5E4 File Offset: 0x0002C7E4
		private ResourceDictionary GetDesignTimeSystemColors()
		{
			if (this.AccentColor != null)
			{
				return new ColorPaletteResources
				{
					Accent = this.AccentColor
				};
			}
			return new ResourceDictionary
			{
				Source = PackUriHelper.GetAbsoluteUri("DesignTime/SystemColors.xaml")
			};
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06001039 RID: 4153 RVA: 0x0002E628 File Offset: 0x0002C828
		// (set) Token: 0x0600103A RID: 4154 RVA: 0x0002E630 File Offset: 0x0002C830
		private bool IsInitialized { get; set; }

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x0600103B RID: 4155 RVA: 0x0002E639 File Offset: 0x0002C839
		// (set) Token: 0x0600103C RID: 4156 RVA: 0x0002E641 File Offset: 0x0002C841
		private bool IsInitializePending { get; set; }

		// Token: 0x0600103D RID: 4157 RVA: 0x0002E64A File Offset: 0x0002C84A
		public void BeginInit()
		{
			base.BeginInit();
			this.IsInitializePending = true;
			this.IsInitialized = false;
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x0002E660 File Offset: 0x0002C860
		public void EndInit()
		{
			this.IsInitializePending = false;
			this.IsInitialized = true;
			if (DesignMode.DesignModeEnabled)
			{
				this.DesignTimeInit();
			}
			else
			{
				ThemeManager.Current.Initialize();
				if (this.CanBeAccessedAcrossThreads)
				{
					this.EnsureLightResources();
					this.EnsureDarkResources();
					this.EnsureHighContrastResources();
					this._lightResources.SealValues();
					this._darkResources.SealValues();
					this._highContrastResources.SealValues();
				}
			}
			base.EndInit();
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x0002E6D5 File Offset: 0x0002C8D5
		void ISupportInitialize.BeginInit()
		{
			this.BeginInit();
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x0002E6DD File Offset: 0x0002C8DD
		void ISupportInitialize.EndInit()
		{
			this.EndInit();
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06001041 RID: 4161 RVA: 0x0002E6E8 File Offset: 0x0002C8E8
		private int MergedThemeDictionaryCount
		{
			get
			{
				int num = 0;
				if (this.IsMerged(this._lightResources))
				{
					num++;
				}
				if (this.IsMerged(this._darkResources))
				{
					num++;
				}
				if (this.IsMerged(this._highContrastResources))
				{
					num++;
				}
				return num;
			}
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x0002E730 File Offset: 0x0002C930
		internal void ApplyApplicationTheme(ApplicationTheme theme)
		{
			int index = DesignMode.DesignModeEnabled ? 1 : 0;
			if (SystemParameters.HighContrast)
			{
				this.EnsureHighContrastResources();
				if (!this.IsMerged(this._highContrastResources))
				{
					base.MergedDictionaries.InsertOrReplace(index, this._highContrastResources);
					base.MergedDictionaries.RemoveIfNotNull(this._lightResources);
					base.MergedDictionaries.RemoveIfNotNull(this._darkResources);
					return;
				}
				if (this.CanBeAccessedAcrossThreads)
				{
					this.RefreshHighContrastResources();
					return;
				}
			}
			else
			{
				if (theme == ApplicationTheme.Light)
				{
					this.EnsureLightResources();
					base.MergedDictionaries.InsertOrReplace(index, this._lightResources);
					base.MergedDictionaries.RemoveIfNotNull(this._darkResources);
				}
				else
				{
					if (theme != ApplicationTheme.Dark)
					{
						throw new ArgumentOutOfRangeException("theme");
					}
					this.EnsureDarkResources();
					base.MergedDictionaries.InsertOrReplace(index, this._darkResources);
					base.MergedDictionaries.RemoveIfNotNull(this._lightResources);
				}
				base.MergedDictionaries.RemoveIfNotNull(this._highContrastResources);
			}
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x0002E824 File Offset: 0x0002CA24
		internal void ApplyElementTheme(ResourceDictionary target, ElementTheme theme)
		{
			ResourceDictionary mergedAppThemeDictionary = null;
			if (SystemParameters.HighContrast)
			{
				target.MergedDictionaries.RemoveIfNotNull(this._lightResources);
				target.MergedDictionaries.RemoveIfNotNull(this._darkResources);
			}
			else if (theme == ElementTheme.Light)
			{
				this.EnsureLightResources();
				target.MergedDictionaries.RemoveIfNotNull(this._darkResources);
				target.MergedDictionaries.InsertIfNotExists(0, this._lightResources);
				mergedAppThemeDictionary = this._lightResources;
			}
			else if (theme == ElementTheme.Dark)
			{
				this.EnsureDarkResources();
				target.MergedDictionaries.RemoveIfNotNull(this._lightResources);
				target.MergedDictionaries.InsertIfNotExists(0, this._darkResources);
				mergedAppThemeDictionary = this._darkResources;
			}
			else
			{
				target.MergedDictionaries.RemoveIfNotNull(this._lightResources);
				target.MergedDictionaries.RemoveIfNotNull(this._darkResources);
			}
			ResourceDictionaryEx resourceDictionaryEx = target as ResourceDictionaryEx;
			if (resourceDictionaryEx != null)
			{
				resourceDictionaryEx.MergedAppThemeDictionary = mergedAppThemeDictionary;
			}
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x0002E900 File Offset: 0x0002CB00
		internal ResourceDictionary GetThemeDictionary(string key)
		{
			if (key == "Light")
			{
				this.EnsureLightResources();
				return this._lightResources;
			}
			if (key == "Dark")
			{
				this.EnsureDarkResources();
				return this._darkResources;
			}
			if (!(key == "HighContrast"))
			{
				throw new ArgumentException();
			}
			this.EnsureHighContrastResources();
			return this._highContrastResources;
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x0002E964 File Offset: 0x0002CB64
		internal ResourceDictionary TryGetThemeDictionary(string key)
		{
			ResourceDictionary result;
			if (!(key == "Light"))
			{
				if (!(key == "Dark"))
				{
					if (!(key == "HighContrast"))
					{
						result = null;
					}
					else
					{
						result = this._highContrastResources;
					}
				}
				else
				{
					result = this._darkResources;
				}
			}
			else
			{
				result = this._lightResources;
			}
			return result;
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x0002E9B8 File Offset: 0x0002CBB8
		private void EnsureLightResources()
		{
			if (this._lightResources == null)
			{
				this._lightResources = this.InitializeThemeDictionary("Light");
			}
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x0002E9D3 File Offset: 0x0002CBD3
		private void EnsureDarkResources()
		{
			if (this._darkResources == null)
			{
				this._darkResources = this.InitializeThemeDictionary("Dark");
			}
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x0002E9EE File Offset: 0x0002CBEE
		private void EnsureHighContrastResources()
		{
			if (this._highContrastResources == null)
			{
				this._highContrastResources = this.InitializeThemeDictionary("HighContrast");
			}
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x0002EA0C File Offset: 0x0002CC0C
		private void RefreshHighContrastResources()
		{
			Collection<ResourceDictionary> mergedDictionaries = this._highContrastResources.MergedDictionaries;
			ResourceDictionary defaultThemeDictionary = ThemeManager.GetDefaultThemeDictionary("HighContrast");
			for (int i = 0; i < mergedDictionaries.Count; i++)
			{
				ResourceDictionary resourceDictionary = mergedDictionaries[i];
				if (resourceDictionary.Source != null)
				{
					ResourceDictionary resourceDictionary2 = new ResourceDictionary
					{
						Source = resourceDictionary.Source
					};
					resourceDictionary2.SealValues();
					if (resourceDictionary == defaultThemeDictionary)
					{
						ThemeManager.SetDefaultThemeDictionary("HighContrast", resourceDictionary2);
					}
					mergedDictionaries[i] = resourceDictionary2;
				}
			}
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x0002EA8A File Offset: 0x0002CC8A
		private bool IsMerged(ResourceDictionary dictionary)
		{
			return dictionary != null && base.MergedDictionaries.Contains(dictionary);
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x0002EAA0 File Offset: 0x0002CCA0
		private ResourceDictionary InitializeThemeDictionary(string key)
		{
			ResourceDictionary defaultThemeDictionary = ThemeManager.GetDefaultThemeDictionary(key);
			ResourceDictionary resourceDictionary;
			if (base.ThemeDictionaries.TryGetValue(key, ref resourceDictionary))
			{
				if (!ThemeResources.ContainsDefaultThemeResources(resourceDictionary, defaultThemeDictionary))
				{
					resourceDictionary.MergedDictionaries.Insert(0, defaultThemeDictionary);
				}
			}
			else if (key == "HighContrast")
			{
				resourceDictionary = new ResourceDictionary();
				resourceDictionary.MergedDictionaries.Add(defaultThemeDictionary);
			}
			else
			{
				resourceDictionary = defaultThemeDictionary;
			}
			return resourceDictionary;
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x0002EB00 File Offset: 0x0002CD00
		private static bool ContainsDefaultThemeResources(ResourceDictionary dictionary, ResourceDictionary defaultResources)
		{
			if (dictionary == defaultResources || ThemeResources.<ContainsDefaultThemeResources>g__SourceEquals|46_0(dictionary.Source, defaultResources.Source))
			{
				return true;
			}
			foreach (ResourceDictionary resourceDictionary in dictionary.MergedDictionaries)
			{
				if (resourceDictionary != null && ThemeResources.ContainsDefaultThemeResources(resourceDictionary, defaultResources))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x0002EB90 File Offset: 0x0002CD90
		[CompilerGenerated]
		private void <UpdateDesignTimeThemeDictionary>g__updateTo|20_0(ResourceDictionary themeDictionary)
		{
			base.MergedDictionaries.RemoveIfNotNull(this._lightResources);
			base.MergedDictionaries.RemoveIfNotNull(this._darkResources);
			base.MergedDictionaries.RemoveIfNotNull(this._highContrastResources);
			base.MergedDictionaries.Insert(1, themeDictionary);
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x0002EBE0 File Offset: 0x0002CDE0
		[CompilerGenerated]
		internal static bool <ContainsDefaultThemeResources>g__SourceEquals|46_0(Uri x, Uri y)
		{
			if (x == null || y == null)
			{
				return false;
			}
			string text = x.IsAbsoluteUri ? x.LocalPath : x.ToString();
			string text2 = y.IsAbsoluteUri ? y.LocalPath : y.ToString();
			return string.Equals(text, text2, 5);
		}

		// Token: 0x0400043C RID: 1084
		private static ThemeResources _current;

		// Token: 0x0400043D RID: 1085
		private ResourceDictionary _lightResources;

		// Token: 0x0400043E RID: 1086
		private ResourceDictionary _darkResources;

		// Token: 0x0400043F RID: 1087
		private ResourceDictionary _highContrastResources;

		// Token: 0x04000440 RID: 1088
		private bool _canBeAccessedAcrossThreads;
	}
}
