using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using ModernWpf.DesignTime;

namespace ModernWpf
{
	// Token: 0x02000228 RID: 552
	public class ThemeManager : DependencyObject
	{
		// Token: 0x06000FEB RID: 4075 RVA: 0x0002D610 File Offset: 0x0002B810
		static ThemeManager()
		{
			ThemeManager.ThemeProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(new PropertyChangedCallback(ThemeManager.OnThemeChanged)));
			ThemeManager._actualThemeChangedEventArgs = new RoutedEventArgs(ThemeManager.ActualThemeChangedEvent);
			MenuDropAlignmentHelper.EnsureStandardPopupAlignment();
			if (DesignMode.DesignModeEnabled)
			{
				ThemeManager.GetDefaultThemeDictionary("Light");
				ThemeManager.GetDefaultThemeDictionary("Dark");
				ThemeManager.GetDefaultThemeDictionary("HighContrast");
			}
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0002D9BA File Offset: 0x0002BBBA
		private ThemeManager()
		{
			this._data = new ThemeManager.Data(this);
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000FED RID: 4077 RVA: 0x0002D9D5 File Offset: 0x0002BBD5
		// (set) Token: 0x06000FEE RID: 4078 RVA: 0x0002D9E7 File Offset: 0x0002BBE7
		public ApplicationTheme? ApplicationTheme
		{
			get
			{
				return (ApplicationTheme?)base.GetValue(ThemeManager.ApplicationThemeProperty);
			}
			set
			{
				base.SetValue(ThemeManager.ApplicationThemeProperty, value);
			}
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x0002D9FA File Offset: 0x0002BBFA
		private static void OnApplicationThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ThemeManager)d).UpdateActualApplicationTheme();
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000FF0 RID: 4080 RVA: 0x0002DA07 File Offset: 0x0002BC07
		// (set) Token: 0x06000FF1 RID: 4081 RVA: 0x0002DA19 File Offset: 0x0002BC19
		public ApplicationTheme ActualApplicationTheme
		{
			get
			{
				return (ApplicationTheme)base.GetValue(ThemeManager.ActualApplicationThemeProperty);
			}
			private set
			{
				base.SetValue(ThemeManager.ActualApplicationThemePropertyKey, value);
			}
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0002DA2C File Offset: 0x0002BC2C
		private static void OnActualApplicationThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ThemeManager themeManager = (ThemeManager)d;
			ApplicationTheme applicationTheme = (ApplicationTheme)e.NewValue;
			if (applicationTheme != ModernWpf.ApplicationTheme.Light)
			{
				if (applicationTheme == ModernWpf.ApplicationTheme.Dark)
				{
					themeManager._defaultActualTheme = ElementTheme.Dark;
				}
			}
			else
			{
				themeManager._defaultActualTheme = ElementTheme.Light;
			}
			themeManager._data.ActualApplicationTheme = applicationTheme;
			themeManager.ApplyApplicationTheme();
			TypedEventHandler<ThemeManager, object> actualApplicationThemeChanged = themeManager.ActualApplicationThemeChanged;
			if (actualApplicationThemeChanged == null)
			{
				return;
			}
			actualApplicationThemeChanged.Invoke(themeManager, null);
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x0002DA8C File Offset: 0x0002BC8C
		private void UpdateActualApplicationTheme()
		{
			if (this.UsingSystemTheme)
			{
				this.ActualApplicationTheme = ThemeManager.GetDefaultAppTheme();
				return;
			}
			this.ActualApplicationTheme = this.ApplicationTheme.GetValueOrDefault();
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0002DAC1 File Offset: 0x0002BCC1
		private void ApplyApplicationTheme()
		{
			if (this._applicationInitialized)
			{
				ThemeResources.Current.ApplyApplicationTheme(this.ActualApplicationTheme);
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000FF5 RID: 4085 RVA: 0x0002DADB File Offset: 0x0002BCDB
		// (set) Token: 0x06000FF6 RID: 4086 RVA: 0x0002DAED File Offset: 0x0002BCED
		public Color? AccentColor
		{
			get
			{
				return (Color?)base.GetValue(ThemeManager.AccentColorProperty);
			}
			set
			{
				base.SetValue(ThemeManager.AccentColorProperty, value);
			}
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0002DB00 File Offset: 0x0002BD00
		private static void OnAccentColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ThemeManager)d).UpdateActualAccentColor();
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000FF8 RID: 4088 RVA: 0x0002DB0D File Offset: 0x0002BD0D
		// (set) Token: 0x06000FF9 RID: 4089 RVA: 0x0002DB1F File Offset: 0x0002BD1F
		public Color ActualAccentColor
		{
			get
			{
				return (Color)base.GetValue(ThemeManager.ActualAccentColorProperty);
			}
			private set
			{
				base.SetValue(ThemeManager.ActualAccentColorPropertyKey, value);
			}
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x0002DB34 File Offset: 0x0002BD34
		private static void OnActualAccentColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ThemeManager themeManager = (ThemeManager)d;
			themeManager.ApplyAccentColor();
			TypedEventHandler<ThemeManager, object> actualAccentColorChanged = themeManager.ActualAccentColorChanged;
			if (actualAccentColorChanged == null)
			{
				return;
			}
			actualAccentColorChanged.Invoke(themeManager, null);
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x0002DB60 File Offset: 0x0002BD60
		private void ApplyAccentColor()
		{
			if (this._applicationInitialized)
			{
				this.UpdateAccentColors();
				foreach (ResourceDictionary themeDictionary in ThemeManager._defaultThemeDictionaries.Values)
				{
					ColorsHelper.Current.UpdateBrushes(themeDictionary);
				}
			}
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x0002DBCC File Offset: 0x0002BDCC
		private void UpdateActualAccentColor()
		{
			if (this.UsingSystemAccentColor)
			{
				this.ActualAccentColor = ColorsHelper.Current.SystemAccentColor;
				return;
			}
			this.ActualAccentColor = (this.AccentColor ?? ColorsHelper.DefaultAccentColor);
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x0002DC16 File Offset: 0x0002BE16
		private void UpdateAccentColors()
		{
			if (this.UsingSystemAccentColor)
			{
				ColorsHelper.Current.FetchSystemAccentColors();
				return;
			}
			ColorsHelper.Current.SetAccent(this.ActualAccentColor);
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x0002DC3B File Offset: 0x0002BE3B
		public static ElementTheme GetRequestedTheme(FrameworkElement element)
		{
			return (ElementTheme)element.GetValue(ThemeManager.RequestedThemeProperty);
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x0002DC4D File Offset: 0x0002BE4D
		public static void SetRequestedTheme(FrameworkElement element, ElementTheme value)
		{
			element.SetValue(ThemeManager.RequestedThemeProperty, value);
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x0002DC60 File Offset: 0x0002BE60
		private static void OnRequestedThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			ThemeManager.StartOrStopListeningForHighContrastChanges(frameworkElement);
			if (frameworkElement.IsInitialized)
			{
				ThemeManager.ApplyRequestedTheme(frameworkElement);
				return;
			}
			ThemeManager.SetSubscribedToInitialized(frameworkElement, true);
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x0002DC90 File Offset: 0x0002BE90
		private static void ApplyRequestedTheme(FrameworkElement element)
		{
			ResourceDictionary resources = element.Resources;
			ElementTheme requestedTheme = ThemeManager.GetRequestedTheme(element);
			ThemeResources.Current.ApplyElementTheme(resources, requestedTheme);
			Window window = element as Window;
			if (window != null)
			{
				ThemeManager.UpdateWindowTheme(window);
				return;
			}
			if (requestedTheme != ElementTheme.Default)
			{
				ThemeManager.SetTheme(element, requestedTheme);
				return;
			}
			element.ClearValue(ThemeManager.ThemeProperty);
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x0002DCDE File Offset: 0x0002BEDE
		private static void SetTheme(FrameworkElement element, ElementTheme value)
		{
			element.SetValue(ThemeManager.ThemeProperty, value);
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x0002DCF4 File Offset: 0x0002BEF4
		private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = d as FrameworkElement;
			if (frameworkElement != null)
			{
				ThemeManager.UpdateActualTheme(frameworkElement, (ElementTheme)e.NewValue);
			}
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0002DD20 File Offset: 0x0002BF20
		private static void UpdateWindowTheme(Window window)
		{
			ElementTheme requestedTheme = ThemeManager.GetRequestedTheme(window);
			if (requestedTheme != ElementTheme.Default)
			{
				ThemeManager.SetTheme(window, requestedTheme);
				return;
			}
			ThemeManager.SetTheme(window, ThemeManager.Current._defaultActualTheme);
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0002DD4F File Offset: 0x0002BF4F
		public static ElementTheme GetActualTheme(FrameworkElement element)
		{
			return (ElementTheme)element.GetValue(ThemeManager.ActualThemeProperty);
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0002DD64 File Offset: 0x0002BF64
		private static void OnActualThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = d as FrameworkElement;
			if (frameworkElement != null)
			{
				if (ThemeManager.GetHasThemeResources(frameworkElement))
				{
					ThemeManager.UpdateThemeResourcesForElement(frameworkElement);
				}
				ThemeManager.RaiseActualThemeChanged(frameworkElement);
			}
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0002DD90 File Offset: 0x0002BF90
		private static void UpdateActualTheme(FrameworkElement element, ElementTheme theme)
		{
			ElementTheme elementTheme;
			if (theme != ElementTheme.Default)
			{
				elementTheme = theme;
			}
			else
			{
				elementTheme = ThemeManager.Current._defaultActualTheme;
			}
			element.SetValue(ThemeManager.ActualThemePropertyKey, elementTheme);
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0002DDC0 File Offset: 0x0002BFC0
		public static void AddActualThemeChangedHandler(FrameworkElement element, RoutedEventHandler handler)
		{
			element.AddHandler(ThemeManager.ActualThemeChangedEvent, handler);
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0002DDCE File Offset: 0x0002BFCE
		public static void RemoveActualThemeChangedHandler(FrameworkElement element, RoutedEventHandler handler)
		{
			element.RemoveHandler(ThemeManager.ActualThemeChangedEvent, handler);
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x0002DDDC File Offset: 0x0002BFDC
		private static void RaiseActualThemeChanged(FrameworkElement element)
		{
			element.RaiseEvent(ThemeManager._actualThemeChangedEventArgs);
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0002DDE9 File Offset: 0x0002BFE9
		public static bool GetIsThemeAware(Window window)
		{
			return (bool)window.GetValue(ThemeManager.IsThemeAwareProperty);
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0002DDFB File Offset: 0x0002BFFB
		public static void SetIsThemeAware(Window window, bool value)
		{
			window.SetValue(ThemeManager.IsThemeAwareProperty, value);
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0002DE10 File Offset: 0x0002C010
		private static void OnIsThemeAwareChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Window window = d as Window;
			if (window != null)
			{
				if ((bool)e.NewValue)
				{
					window.SetBinding(ThemeManager.InheritedApplicationThemeProperty, new Binding("ActualApplicationTheme")
					{
						Source = ThemeManager.Current._data
					});
					ThemeManager.UpdateWindowTheme((Window)d);
					return;
				}
				window.ClearValue(ThemeManager.InheritedApplicationThemeProperty);
			}
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0002DE72 File Offset: 0x0002C072
		private static void OnInheritedApplicationThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ThemeManager.UpdateWindowTheme((Window)d);
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x0002DE7F File Offset: 0x0002C07F
		public static bool GetHasThemeResources(FrameworkElement element)
		{
			return (bool)element.GetValue(ThemeManager.HasThemeResourcesProperty);
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0002DE91 File Offset: 0x0002C091
		public static void SetHasThemeResources(FrameworkElement element, bool value)
		{
			element.SetValue(ThemeManager.HasThemeResourcesProperty, value);
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0002DEA4 File Offset: 0x0002C0A4
		private static void OnHasThemeResourcesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			if (!(bool)e.NewValue)
			{
				ThemeManager.UpdateSubscribedToInitialized(frameworkElement);
				return;
			}
			if (frameworkElement.IsInitialized)
			{
				ThemeManager.UpdateThemeResourcesForElement(frameworkElement);
				return;
			}
			ThemeManager.SetSubscribedToInitialized(frameworkElement, true);
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0002DEE3 File Offset: 0x0002C0E3
		private static void UpdateThemeResourcesForElement(FrameworkElement element)
		{
			ThemeManager.UpdateElementThemeResources(element.Resources, ThemeManager.GetEffectiveThemeKey(element));
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x0002DEF8 File Offset: 0x0002C0F8
		private static void UpdateElementThemeResources(ResourceDictionary resources, string themeKey)
		{
			ResourceDictionaryEx resourceDictionaryEx = resources as ResourceDictionaryEx;
			if (resourceDictionaryEx != null)
			{
				resourceDictionaryEx.Update(themeKey);
			}
			foreach (ResourceDictionary resources2 in resources.MergedDictionaries)
			{
				ThemeManager.UpdateElementThemeResources(resources2, themeKey);
			}
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x0002DF54 File Offset: 0x0002C154
		private static string GetEffectiveThemeKey(FrameworkElement element)
		{
			if (SystemParameters.HighContrast)
			{
				return "HighContrast";
			}
			ElementTheme actualTheme = ThemeManager.GetActualTheme(element);
			if (actualTheme == ElementTheme.Light)
			{
				return "Light";
			}
			if (actualTheme != ElementTheme.Dark)
			{
				throw new InvalidOperationException();
			}
			return "Dark";
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0002DF90 File Offset: 0x0002C190
		private static void SetSubscribedToInitialized(FrameworkElement element, bool value)
		{
			element.SetValue(ThemeManager.SubscribedToInitializedProperty, value);
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0002DFA4 File Offset: 0x0002C1A4
		private static void OnSubscribedToInitializedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			if ((bool)e.NewValue)
			{
				frameworkElement.Initialized += new EventHandler(ThemeManager.OnElementInitialized);
				return;
			}
			frameworkElement.Initialized -= new EventHandler(ThemeManager.OnElementInitialized);
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0002DFEB File Offset: 0x0002C1EB
		private static void UpdateSubscribedToInitialized(FrameworkElement element)
		{
			if (ThemeManager.ShouldSubscribeToInitialized(element))
			{
				ThemeManager.SetSubscribedToInitialized(element, true);
				return;
			}
			element.ClearValue(ThemeManager.SubscribedToInitializedProperty);
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x0002E008 File Offset: 0x0002C208
		private static bool ShouldSubscribeToInitialized(FrameworkElement element)
		{
			return !element.IsInitialized && (ThemeManager.GetRequestedTheme(element) != ElementTheme.Default || ThemeManager.GetHasThemeResources(element));
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0002E024 File Offset: 0x0002C224
		private static void OnElementInitialized(object sender, EventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)sender;
			frameworkElement.ClearValue(ThemeManager.SubscribedToInitializedProperty);
			if (ThemeManager.GetRequestedTheme(frameworkElement) != ElementTheme.Default)
			{
				ThemeManager.ApplyRequestedTheme(frameworkElement);
			}
			if (ThemeManager.GetHasThemeResources(frameworkElement))
			{
				ThemeManager.UpdateThemeResourcesForElement(frameworkElement);
			}
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0002E060 File Offset: 0x0002C260
		private static void OnElementHighContrastChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			if (frameworkElement.IsInitialized)
			{
				ThemeManager.ApplyRequestedTheme(frameworkElement);
				ThemeManager.UpdateThemeResourcesForElement(frameworkElement);
				return;
			}
			ThemeManager.SetSubscribedToInitialized(frameworkElement, true);
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x0002E090 File Offset: 0x0002C290
		private static void OnIsListeningForHighContrastChangesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			if ((bool)e.OldValue)
			{
				frameworkElement.ClearValue(ThemeManager.ElementHighContrastProperty);
			}
			if ((bool)e.NewValue)
			{
				frameworkElement.SetBinding(ThemeManager.ElementHighContrastProperty, ThemeManager._highContrastBinding);
			}
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x0002E0DC File Offset: 0x0002C2DC
		private static bool ShouldListenForHighContrastChanges(FrameworkElement element)
		{
			return ThemeManager.GetRequestedTheme(element) != ElementTheme.Default || ThemeManager.GetHasThemeResources(element);
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0002E0EE File Offset: 0x0002C2EE
		private static void StartOrStopListeningForHighContrastChanges(FrameworkElement element)
		{
			if (ThemeManager.ShouldListenForHighContrastChanges(element))
			{
				element.SetValue(ThemeManager.IsListeningForHighContrastChangesProperty, true);
				return;
			}
			element.ClearValue(ThemeManager.IsListeningForHighContrastChangesProperty);
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x0600101E RID: 4126 RVA: 0x0002E115 File Offset: 0x0002C315
		public static ThemeManager Current { get; } = new ThemeManager();

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x0600101F RID: 4127 RVA: 0x0002E11C File Offset: 0x0002C31C
		internal bool UsingSystemTheme
		{
			get
			{
				return ColorsHelper.SystemColorsSupported && this.ApplicationTheme == null;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06001020 RID: 4128 RVA: 0x0002E144 File Offset: 0x0002C344
		internal bool UsingSystemAccentColor
		{
			get
			{
				return ColorsHelper.SystemColorsSupported && this.AccentColor == null;
			}
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0002E16C File Offset: 0x0002C36C
		internal static void UpdateThemeBrushes(ResourceDictionary colors)
		{
			foreach (ResourceDictionary themeDictionary in ThemeManager._defaultThemeDictionaries.Values)
			{
				ColorsHelper.UpdateBrushes(themeDictionary, colors);
			}
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0002E1C4 File Offset: 0x0002C3C4
		internal static ResourceDictionary GetDefaultThemeDictionary(string key)
		{
			ResourceDictionary resourceDictionary;
			if (!ThemeManager._defaultThemeDictionaries.TryGetValue(key, ref resourceDictionary))
			{
				resourceDictionary = new ResourceDictionary
				{
					Source = ThemeManager.GetDefaultSource(key)
				};
				ThemeManager._defaultThemeDictionaries[key] = resourceDictionary;
			}
			return resourceDictionary;
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x0002E1FF File Offset: 0x0002C3FF
		internal static void SetDefaultThemeDictionary(string key, ResourceDictionary dictionary)
		{
			ThemeManager._defaultThemeDictionaries[key] = dictionary;
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x0002E210 File Offset: 0x0002C410
		private static ApplicationTheme GetDefaultAppTheme()
		{
			return ColorsHelper.Current.SystemTheme.GetValueOrDefault(ModernWpf.ApplicationTheme.Light);
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0002E230 File Offset: 0x0002C430
		private static Uri GetDefaultSource(string theme)
		{
			return PackUriHelper.GetAbsoluteUri("ThemeResources/" + theme + ".xaml");
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x0002E248 File Offset: 0x0002C448
		private static ResourceDictionary FindDictionary(ResourceDictionary parent, Uri source)
		{
			if (parent.Source == source)
			{
				return parent;
			}
			foreach (ResourceDictionary resourceDictionary in parent.MergedDictionaries)
			{
				if (resourceDictionary != null)
				{
					if (resourceDictionary.Source == source)
					{
						return resourceDictionary;
					}
					ResourceDictionary resourceDictionary2 = ThemeManager.FindDictionary(resourceDictionary, source);
					if (resourceDictionary2 != null)
					{
						return resourceDictionary2;
					}
				}
			}
			return null;
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x0002E2C8 File Offset: 0x0002C4C8
		internal void Initialize()
		{
			if (this._isInitialized)
			{
				return;
			}
			SystemParameters.StaticPropertyChanged += new PropertyChangedEventHandler(this.OnSystemParametersChanged);
			if (Application.Current != null)
			{
				ResourceDictionary resources = Application.Current.Resources;
				resources.MergedDictionaries.RemoveAll<IntellisenseResourcesBase>();
				ColorsHelper.Current.SystemThemeChanged += new EventHandler(this.OnSystemThemeChanged);
				ColorsHelper.Current.SystemAccentColorChanged += new EventHandler(this.OnSystemAccentColorChanged);
				resources.MergedDictionaries.Insert(0, ColorsHelper.Current.Colors);
				this.UpdateActualAccentColor();
				this.UpdateActualApplicationTheme();
				this._applicationInitialized = true;
				this.ApplyAccentColor();
				this.ApplyApplicationTheme();
			}
			this._isInitialized = true;
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x0002E372 File Offset: 0x0002C572
		private void OnSystemThemeChanged(object sender, EventArgs e)
		{
			if (this.UsingSystemTheme)
			{
				this.UpdateActualApplicationTheme();
			}
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0002E382 File Offset: 0x0002C582
		private void OnSystemAccentColorChanged(object sender, EventArgs e)
		{
			if (this.UsingSystemAccentColor)
			{
				this.UpdateActualAccentColor();
			}
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x0002E392 File Offset: 0x0002C592
		private void OnSystemParametersChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "HighContrast")
			{
				this.RunOnMainThread(new Action(this.ApplyApplicationTheme));
			}
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x0002E3B8 File Offset: 0x0002C5B8
		private void RunOnMainThread(Action action)
		{
			if (base.Dispatcher.CheckAccess())
			{
				action.Invoke();
				return;
			}
			base.Dispatcher.BeginInvoke(action, Array.Empty<object>());
		}

		// Token: 0x04000417 RID: 1047
		internal const string LightKey = "Light";

		// Token: 0x04000418 RID: 1048
		internal const string DarkKey = "Dark";

		// Token: 0x04000419 RID: 1049
		internal const string HighContrastKey = "HighContrast";

		// Token: 0x0400041A RID: 1050
		private static readonly Binding _highContrastBinding = new Binding("(SystemParameters.HighContrast)");

		// Token: 0x0400041B RID: 1051
		private static readonly RoutedEventArgs _actualThemeChangedEventArgs;

		// Token: 0x0400041C RID: 1052
		private static readonly Dictionary<string, ResourceDictionary> _defaultThemeDictionaries = new Dictionary<string, ResourceDictionary>();

		// Token: 0x0400041D RID: 1053
		private readonly ThemeManager.Data _data;

		// Token: 0x0400041E RID: 1054
		private bool _isInitialized;

		// Token: 0x0400041F RID: 1055
		private bool _applicationInitialized;

		// Token: 0x04000420 RID: 1056
		public static readonly DependencyProperty ApplicationThemeProperty = DependencyProperty.Register("ApplicationTheme", typeof(ApplicationTheme?), typeof(ThemeManager), new PropertyMetadata(new PropertyChangedCallback(ThemeManager.OnApplicationThemeChanged)));

		// Token: 0x04000421 RID: 1057
		private static readonly DependencyPropertyKey ActualApplicationThemePropertyKey = DependencyProperty.RegisterReadOnly("ActualApplicationTheme", typeof(ApplicationTheme), typeof(ThemeManager), new PropertyMetadata(ModernWpf.ApplicationTheme.Light, new PropertyChangedCallback(ThemeManager.OnActualApplicationThemeChanged)));

		// Token: 0x04000422 RID: 1058
		public static readonly DependencyProperty ActualApplicationThemeProperty = ThemeManager.ActualApplicationThemePropertyKey.DependencyProperty;

		// Token: 0x04000423 RID: 1059
		public static readonly DependencyProperty AccentColorProperty = DependencyProperty.Register("AccentColor", typeof(Color?), typeof(ThemeManager), new PropertyMetadata(new PropertyChangedCallback(ThemeManager.OnAccentColorChanged)));

		// Token: 0x04000424 RID: 1060
		private static readonly DependencyPropertyKey ActualAccentColorPropertyKey = DependencyProperty.RegisterReadOnly("ActualAccentColor", typeof(Color), typeof(ThemeManager), new PropertyMetadata(ColorsHelper.DefaultAccentColor, new PropertyChangedCallback(ThemeManager.OnActualAccentColorChanged)));

		// Token: 0x04000425 RID: 1061
		public static readonly DependencyProperty ActualAccentColorProperty = ThemeManager.ActualAccentColorPropertyKey.DependencyProperty;

		// Token: 0x04000426 RID: 1062
		public static readonly DependencyProperty RequestedThemeProperty = DependencyProperty.RegisterAttached("RequestedTheme", typeof(ElementTheme), typeof(ThemeManager), new PropertyMetadata(ElementTheme.Default, new PropertyChangedCallback(ThemeManager.OnRequestedThemeChanged)));

		// Token: 0x04000427 RID: 1063
		private static readonly DependencyProperty ThemeProperty = DependencyProperty.RegisterAttached("Theme", typeof(ElementTheme), typeof(ThemeManager), new FrameworkPropertyMetadata(ElementTheme.Default, 32));

		// Token: 0x04000428 RID: 1064
		private static readonly DependencyPropertyKey ActualThemePropertyKey = DependencyProperty.RegisterAttachedReadOnly("ActualTheme", typeof(ElementTheme), typeof(ThemeManager), new FrameworkPropertyMetadata(ElementTheme.Light, new PropertyChangedCallback(ThemeManager.OnActualThemeChanged)));

		// Token: 0x04000429 RID: 1065
		public static readonly DependencyProperty ActualThemeProperty = ThemeManager.ActualThemePropertyKey.DependencyProperty;

		// Token: 0x0400042A RID: 1066
		private ElementTheme _defaultActualTheme = ElementTheme.Light;

		// Token: 0x0400042B RID: 1067
		public static readonly RoutedEvent ActualThemeChangedEvent = EventManager.RegisterRoutedEvent("ActualThemeChanged", 2, typeof(RoutedEventHandler), typeof(ThemeManager));

		// Token: 0x0400042C RID: 1068
		public static readonly DependencyProperty IsThemeAwareProperty = DependencyProperty.RegisterAttached("IsThemeAware", typeof(bool), typeof(ThemeManager), new PropertyMetadata(new PropertyChangedCallback(ThemeManager.OnIsThemeAwareChanged)));

		// Token: 0x0400042D RID: 1069
		private static readonly DependencyProperty InheritedApplicationThemeProperty = DependencyProperty.RegisterAttached("InheritedApplicationTheme", typeof(ApplicationTheme), typeof(ThemeManager), new PropertyMetadata(ModernWpf.ApplicationTheme.Light, new PropertyChangedCallback(ThemeManager.OnInheritedApplicationThemeChanged)));

		// Token: 0x0400042E RID: 1070
		public static readonly DependencyProperty HasThemeResourcesProperty = DependencyProperty.RegisterAttached("HasThemeResources", typeof(bool), typeof(ThemeManager), new PropertyMetadata(new PropertyChangedCallback(ThemeManager.OnHasThemeResourcesChanged)));

		// Token: 0x0400042F RID: 1071
		private static readonly DependencyProperty SubscribedToInitializedProperty = DependencyProperty.RegisterAttached("SubscribedToInitialized", typeof(bool), typeof(ThemeManager), new PropertyMetadata(false, new PropertyChangedCallback(ThemeManager.OnSubscribedToInitializedChanged)));

		// Token: 0x04000430 RID: 1072
		private static readonly DependencyProperty ElementHighContrastProperty = DependencyProperty.RegisterAttached("ElementHighContrast", typeof(bool), typeof(ThemeManager), new PropertyMetadata(new PropertyChangedCallback(ThemeManager.OnElementHighContrastChanged)));

		// Token: 0x04000431 RID: 1073
		private static readonly DependencyProperty IsListeningForHighContrastChangesProperty = DependencyProperty.RegisterAttached("IsListeningForHighContrastChanges", typeof(bool), typeof(ThemeManager), new PropertyMetadata(new PropertyChangedCallback(ThemeManager.OnIsListeningForHighContrastChangesChanged)));

		// Token: 0x04000433 RID: 1075
		public TypedEventHandler<ThemeManager, object> ActualApplicationThemeChanged;

		// Token: 0x04000434 RID: 1076
		public TypedEventHandler<ThemeManager, object> ActualAccentColorChanged;

		// Token: 0x0200040D RID: 1037
		private class Data : INotifyPropertyChanged
		{
			// Token: 0x06001BBC RID: 7100 RVA: 0x0004A5AB File Offset: 0x000487AB
			public Data(ThemeManager owner)
			{
				this._actualApplicationTheme = owner.ActualApplicationTheme;
			}

			// Token: 0x17000506 RID: 1286
			// (get) Token: 0x06001BBD RID: 7101 RVA: 0x0004A5BF File Offset: 0x000487BF
			// (set) Token: 0x06001BBE RID: 7102 RVA: 0x0004A5C7 File Offset: 0x000487C7
			public ApplicationTheme ActualApplicationTheme
			{
				get
				{
					return this._actualApplicationTheme;
				}
				set
				{
					this.Set<ApplicationTheme>(ref this._actualApplicationTheme, value, "ActualApplicationTheme");
				}
			}

			// Token: 0x14000022 RID: 34
			// (add) Token: 0x06001BBF RID: 7103 RVA: 0x0004A5DC File Offset: 0x000487DC
			// (remove) Token: 0x06001BC0 RID: 7104 RVA: 0x0004A614 File Offset: 0x00048814
			public event PropertyChangedEventHandler PropertyChanged;

			// Token: 0x06001BC1 RID: 7105 RVA: 0x0004A649 File Offset: 0x00048849
			private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
			{
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}

			// Token: 0x06001BC2 RID: 7106 RVA: 0x0004A662 File Offset: 0x00048862
			private void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
			{
				if (!object.Equals(storage, value))
				{
					storage = value;
					this.RaisePropertyChanged(propertyName);
				}
			}

			// Token: 0x040009FF RID: 2559
			private ApplicationTheme _actualApplicationTheme;
		}
	}
}
