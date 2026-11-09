using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32;
using ModernWpf.Media.ColorPalette;
using Windows.UI.ViewManagement;

namespace ModernWpf
{
	// Token: 0x02000211 RID: 529
	internal class ColorsHelper : DispatcherObject
	{
		// Token: 0x06000F85 RID: 3973 RVA: 0x0002C391 File Offset: 0x0002A591
		private ColorsHelper()
		{
			if (ColorsHelper.SystemColorsSupported)
			{
				this.ListenToSystemColorChanges();
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000F86 RID: 3974 RVA: 0x0002C3B1 File Offset: 0x0002A5B1
		public static bool SystemColorsSupported { get; } = OSVersionHelper.IsWindows10OrGreater;

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000F87 RID: 3975 RVA: 0x0002C3B8 File Offset: 0x0002A5B8
		public static ColorsHelper Current { get; } = new ColorsHelper();

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000F88 RID: 3976 RVA: 0x0002C3BF File Offset: 0x0002A5BF
		public ResourceDictionary Colors
		{
			get
			{
				return this._colors;
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000F89 RID: 3977 RVA: 0x0002C3C7 File Offset: 0x0002A5C7
		// (set) Token: 0x06000F8A RID: 3978 RVA: 0x0002C3CF File Offset: 0x0002A5CF
		public ApplicationTheme? SystemTheme { get; private set; }

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000F8B RID: 3979 RVA: 0x0002C3D8 File Offset: 0x0002A5D8
		public Color SystemAccentColor
		{
			get
			{
				return this._systemAccent;
			}
		}

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06000F8C RID: 3980 RVA: 0x0002C3E0 File Offset: 0x0002A5E0
		// (remove) Token: 0x06000F8D RID: 3981 RVA: 0x0002C418 File Offset: 0x0002A618
		public event EventHandler SystemThemeChanged;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06000F8E RID: 3982 RVA: 0x0002C450 File Offset: 0x0002A650
		// (remove) Token: 0x06000F8F RID: 3983 RVA: 0x0002C488 File Offset: 0x0002A688
		public event EventHandler SystemAccentColorChanged;

		// Token: 0x06000F90 RID: 3984 RVA: 0x0002C4C0 File Offset: 0x0002A6C0
		[MethodImpl(8)]
		public void FetchSystemAccentColors()
		{
			UISettings uisettings = new UISettings();
			this._colors["SystemAccentColor"] = uisettings.GetColorValue(UIColorType.Accent).ToColor();
			this._colors["SystemAccentColorDark1"] = uisettings.GetColorValue(UIColorType.AccentDark1).ToColor();
			this._colors["SystemAccentColorDark2"] = uisettings.GetColorValue(UIColorType.AccentDark2).ToColor();
			this._colors["SystemAccentColorDark3"] = uisettings.GetColorValue(UIColorType.AccentDark3).ToColor();
			this._colors["SystemAccentColorLight1"] = uisettings.GetColorValue(UIColorType.AccentLight1).ToColor();
			this._colors["SystemAccentColorLight2"] = uisettings.GetColorValue(UIColorType.AccentLight2).ToColor();
			this._colors["SystemAccentColorLight3"] = uisettings.GetColorValue(UIColorType.AccentLight3).ToColor();
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x0002C5BC File Offset: 0x0002A7BC
		public void SetAccent(Color accent)
		{
			this._colors["SystemAccentColor"] = accent;
			ColorsHelper.UpdateShades(this._colors, accent);
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x0002C5F0 File Offset: 0x0002A7F0
		public static void UpdateShades(ResourceDictionary colors, Color accent)
		{
			ColorPalette colorPalette = new ColorPalette(11, accent);
			colors["SystemAccentColorDark1"] = colorPalette.Palette[6].ActiveColor;
			colors["SystemAccentColorDark2"] = colorPalette.Palette[7].ActiveColor;
			colors["SystemAccentColorDark3"] = colorPalette.Palette[8].ActiveColor;
			colors["SystemAccentColorLight1"] = colorPalette.Palette[4].ActiveColor;
			colors["SystemAccentColorLight2"] = colorPalette.Palette[3].ActiveColor;
			colors["SystemAccentColorLight3"] = colorPalette.Palette[2].ActiveColor;
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x0002C6CC File Offset: 0x0002A8CC
		public static void RemoveShades(ResourceDictionary colors)
		{
			colors.Remove("SystemAccentColorDark3");
			colors.Remove("SystemAccentColorDark2");
			colors.Remove("SystemAccentColorDark1");
			colors.Remove("SystemAccentColorLight1");
			colors.Remove("SystemAccentColorLight2");
			colors.Remove("SystemAccentColorLight3");
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x0002C71B File Offset: 0x0002A91B
		public void UpdateBrushes(ResourceDictionary themeDictionary)
		{
			ColorsHelper.UpdateBrushes(themeDictionary, this._colors);
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x0002C72C File Offset: 0x0002A92C
		public static void UpdateBrushes(ResourceDictionary themeDictionary, ResourceDictionary colors)
		{
			foreach (object obj in themeDictionary)
			{
				SolidColorBrush solidColorBrush = ((DictionaryEntry)obj).Value as SolidColorBrush;
				if (solidColorBrush != null && !solidColorBrush.IsFrozen)
				{
					object colorKey = ThemeResourceHelper.GetColorKey(solidColorBrush);
					if (colorKey != null && colors.Contains(colorKey))
					{
						solidColorBrush.SetCurrentValue(SolidColorBrush.ColorProperty, (Color)colors[colorKey]);
					}
				}
			}
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x0002C7C4 File Offset: 0x0002A9C4
		[MethodImpl(8)]
		private void ListenToSystemColorChanges()
		{
			this._uiSettings = new UISettings();
			this._uiSettings.ColorValuesChanged += this.OnColorValuesChanged;
			if (PackagedAppHelper.IsPackagedApp)
			{
				SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(this.OnUserPreferenceChanged);
			}
			this._systemBackground = this._uiSettings.GetColorValue(UIColorType.Background).ToColor();
			this._systemAccent = this._uiSettings.GetColorValue(UIColorType.Accent).ToColor();
			this.UpdateSystemAppTheme();
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0002C83F File Offset: 0x0002AA3F
		[MethodImpl(8)]
		private void OnColorValuesChanged(UISettings sender, object args)
		{
			base.Dispatcher.BeginInvoke(new Action(this.UpdateColorValues), Array.Empty<object>());
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x0002C85E File Offset: 0x0002AA5E
		[MethodImpl(8)]
		private void OnUserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			if (e.Category == 4)
			{
				this.UpdateColorValues();
			}
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x0002C870 File Offset: 0x0002AA70
		[MethodImpl(8)]
		private void UpdateColorValues()
		{
			Color color = this._uiSettings.GetColorValue(UIColorType.Background).ToColor();
			if (this._systemBackground != color)
			{
				this._systemBackground = color;
				this.UpdateSystemAppTheme();
				EventHandler systemThemeChanged = this.SystemThemeChanged;
				if (systemThemeChanged != null)
				{
					systemThemeChanged.Invoke(null, EventArgs.Empty);
				}
			}
			Color color2 = this._uiSettings.GetColorValue(UIColorType.Accent).ToColor();
			if (this._systemAccent != color2)
			{
				this._systemAccent = color2;
				EventHandler systemAccentColorChanged = this.SystemAccentColorChanged;
				if (systemAccentColorChanged == null)
				{
					return;
				}
				systemAccentColorChanged.Invoke(null, EventArgs.Empty);
			}
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x0002C8FE File Offset: 0x0002AAFE
		private void UpdateSystemAppTheme()
		{
			this.SystemTheme = new ApplicationTheme?(ColorsHelper.IsDarkBackground(this._systemBackground) ? ApplicationTheme.Dark : ApplicationTheme.Light);
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0002C91C File Offset: 0x0002AB1C
		private static bool IsDarkBackground(Color color)
		{
			return (int)(color.R + color.G + color.B) < 765 - (int)color.R - (int)color.G - (int)color.B;
		}

		// Token: 0x040003F0 RID: 1008
		private const string AccentKey = "SystemAccentColor";

		// Token: 0x040003F1 RID: 1009
		private const string AccentDark1Key = "SystemAccentColorDark1";

		// Token: 0x040003F2 RID: 1010
		private const string AccentDark2Key = "SystemAccentColorDark2";

		// Token: 0x040003F3 RID: 1011
		private const string AccentDark3Key = "SystemAccentColorDark3";

		// Token: 0x040003F4 RID: 1012
		private const string AccentLight1Key = "SystemAccentColorLight1";

		// Token: 0x040003F5 RID: 1013
		private const string AccentLight2Key = "SystemAccentColorLight2";

		// Token: 0x040003F6 RID: 1014
		private const string AccentLight3Key = "SystemAccentColorLight3";

		// Token: 0x040003F7 RID: 1015
		internal static readonly Color DefaultAccentColor = Color.FromRgb(0, 120, 215);

		// Token: 0x040003F8 RID: 1016
		private readonly ResourceDictionary _colors = new ResourceDictionary();

		// Token: 0x040003F9 RID: 1017
		private UISettings _uiSettings;

		// Token: 0x040003FA RID: 1018
		private Color _systemBackground;

		// Token: 0x040003FB RID: 1019
		private Color _systemAccent;
	}
}
