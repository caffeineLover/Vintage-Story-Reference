using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using VintageStoryModManager.Services;
using VintageStoryModManager.Views.Dialogs;

namespace VintageStoryModManager
{
	// Token: 0x02000011 RID: 17
	[NullableContext(1)]
	[Nullable(0)]
	public class App : Application
	{
		// Token: 0x060000BB RID: 187 RVA: 0x00005198 File Offset: 0x00003398
		public App()
		{
			DispatcherUnhandledExceptionEventHandler dispatcherUnhandledExceptionEventHandler;
			if ((dispatcherUnhandledExceptionEventHandler = App.<>O.<0>__OnDispatcherUnhandledException) == null)
			{
				dispatcherUnhandledExceptionEventHandler = (App.<>O.<0>__OnDispatcherUnhandledException = new DispatcherUnhandledExceptionEventHandler(App.OnDispatcherUnhandledException));
			}
			base.DispatcherUnhandledException += dispatcherUnhandledExceptionEventHandler;
			AppDomain currentDomain = AppDomain.CurrentDomain;
			EventHandler<FirstChanceExceptionEventArgs> eventHandler;
			if ((eventHandler = App.<>O.<1>__OnFirstChanceException) == null)
			{
				eventHandler = (App.<>O.<1>__OnFirstChanceException = new EventHandler<FirstChanceExceptionEventArgs>(App.OnFirstChanceException));
			}
			currentDomain.FirstChanceException += eventHandler;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000051F4 File Offset: 0x000033F4
		protected override void OnStartup(StartupEventArgs e)
		{
			bool flag;
			this._instanceMutex = new Mutex(true, App.SingleInstanceMutexName, ref flag);
			this._ownsMutex = flag;
			if (flag)
			{
				this.ApplyPreferredTheme();
				base.OnStartup(e);
				return;
			}
			App.ShowSingleInstanceWarning();
			App.ActivateExistingInstance();
			Application application = Application.Current;
			if (application == null)
			{
				return;
			}
			application.Shutdown();
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00005248 File Offset: 0x00003448
		protected override void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);
			AppDomain currentDomain = AppDomain.CurrentDomain;
			EventHandler<FirstChanceExceptionEventArgs> eventHandler;
			if ((eventHandler = App.<>O.<1>__OnFirstChanceException) == null)
			{
				eventHandler = (App.<>O.<1>__OnFirstChanceException = new EventHandler<FirstChanceExceptionEventArgs>(App.OnFirstChanceException));
			}
			currentDomain.FirstChanceException -= eventHandler;
			if (this._instanceMutex != null)
			{
				if (this._ownsMutex)
				{
					this._instanceMutex.ReleaseMutex();
				}
				this._instanceMutex.Dispose();
				this._instanceMutex = null;
				this._ownsMutex = false;
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000052B8 File Offset: 0x000034B8
		private static void OnFirstChanceException([Nullable(2)] object sender, FirstChanceExceptionEventArgs e)
		{
			if (!(e.Exception is InvalidOperationException))
			{
				return;
			}
			try
			{
			}
			catch
			{
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000052EC File Offset: 0x000034EC
		private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			ModManagerMessageBox.Show("An unexpected error occurred:\n" + e.Exception.Message, "Simple VS Manager", 0, 16, null, null, null);
			try
			{
				Application application = Application.Current;
				if (application != null)
				{
					application.Shutdown(-1);
				}
			}
			catch
			{
				e.Handled = false;
				return;
			}
			e.Handled = true;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00005354 File Offset: 0x00003554
		private static void ActivateExistingInstance()
		{
			try
			{
				Process currentProcess = Process.GetCurrentProcess();
				foreach (Process process in Process.GetProcessesByName(currentProcess.ProcessName))
				{
					if (process.Id != currentProcess.Id)
					{
						IntPtr mainWindowHandle = process.MainWindowHandle;
						if (mainWindowHandle != IntPtr.Zero)
						{
							App.WindowActivator.ShowAndActivate(mainWindowHandle);
							break;
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000053C4 File Offset: 0x000035C4
		private static void ShowSingleInstanceWarning()
		{
			try
			{
				MessageDialogButtonContentOverrides buttonContentOverrides = new MessageDialogButtonContentOverrides
				{
					Ok = "Abort"
				};
				ModManagerMessageBox.Show("Simple VS Manager is already running. This launch will be aborted.", "Simple VS Manager", 0, 48, null, buttonContentOverrides, null);
			}
			catch
			{
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00005410 File Offset: 0x00003610
		private void ApplyPreferredTheme()
		{
			ColorTheme theme = ColorTheme.VintageStory;
			IReadOnlyDictionary<string, string> paletteOverrides = null;
			try
			{
				UserConfigurationService userConfigurationService = new UserConfigurationService();
				theme = userConfigurationService.ColorTheme;
				paletteOverrides = userConfigurationService.GetThemePaletteColors();
			}
			catch (Exception)
			{
			}
			this.EnsureActiveThemeDictionary();
			App.ApplyTheme(theme, paletteOverrides);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005458 File Offset: 0x00003658
		public static void ApplyTheme(ColorTheme theme, [Nullable(new byte[]
		{
			2,
			1,
			1
		})] IReadOnlyDictionary<string, string> paletteOverrides)
		{
			App app = Application.Current as App;
			if (app == null)
			{
				return;
			}
			app.EnsureActiveThemeDictionary();
			IReadOnlyDictionary<string, string> paletteOverrides2 = (paletteOverrides != null && paletteOverrides.Count > 0) ? paletteOverrides : null;
			app.ApplyThemeDictionary(App.ResolveThemeUri(theme), paletteOverrides2);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00005498 File Offset: 0x00003698
		private void EnsureActiveThemeDictionary()
		{
			if (this._activeTheme == null)
			{
				ResourceDictionary resources = base.Resources;
				if (resources != null)
				{
					Collection<ResourceDictionary> mergedDictionaries = resources.MergedDictionaries;
					if (mergedDictionaries != null && mergedDictionaries.Count > 0)
					{
						foreach (ResourceDictionary resourceDictionary in base.Resources.MergedDictionaries)
						{
							Uri source = resourceDictionary.Source;
							string text = (source != null) ? source.OriginalString : null;
							if (!string.IsNullOrEmpty(text) && text.Contains("Resources/Themes/", 5))
							{
								this._activeTheme = resourceDictionary;
								break;
							}
						}
						return;
					}
				}
			}
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00005540 File Offset: 0x00003740
		private void ApplyThemeDictionary(Uri source, [Nullable(new byte[]
		{
			2,
			1,
			1
		})] IReadOnlyDictionary<string, string> paletteOverrides)
		{
			if (base.Resources == null)
			{
				return;
			}
			if (this._activeTheme != null)
			{
				base.Resources.MergedDictionaries.Remove(this._activeTheme);
				this._activeTheme = null;
			}
			try
			{
				ResourceDictionary resourceDictionary = new ResourceDictionary
				{
					Source = source
				};
				if (paletteOverrides != null)
				{
					App.ApplyPaletteOverrides(resourceDictionary, paletteOverrides);
				}
				base.Resources.MergedDictionaries.Add(resourceDictionary);
				this._activeTheme = resourceDictionary;
			}
			catch (Exception)
			{
				if (source != App.DarkVsThemeUri)
				{
					this.ApplyThemeDictionary(App.DarkVsThemeUri, paletteOverrides);
				}
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000055D8 File Offset: 0x000037D8
		private static Uri ResolveThemeUri(ColorTheme theme)
		{
			return App.DarkVsThemeUri;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000055E0 File Offset: 0x000037E0
		private static void ApplyPaletteOverrides(ResourceDictionary dictionary, IReadOnlyDictionary<string, string> paletteOverrides)
		{
			foreach (KeyValuePair<string, string> keyValuePair in paletteOverrides)
			{
				Color color;
				if (!string.IsNullOrWhiteSpace(keyValuePair.Key) && !string.IsNullOrWhiteSpace(keyValuePair.Value) && dictionary.Contains(keyValuePair.Key) && App.TryParseColor(keyValuePair.Value, out color))
				{
					dictionary[keyValuePair.Key] = color;
				}
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00005670 File Offset: 0x00003870
		private unsafe static bool TryParseColor(string value, out Color color)
		{
			color = default(Color);
			if (string.IsNullOrWhiteSpace(value))
			{
				return false;
			}
			ReadOnlySpan<char> readOnlySpan = MemoryExtensions.Trim(MemoryExtensions.AsSpan(value));
			if (readOnlySpan.IsEmpty || *readOnlySpan[0] != 35 || readOnlySpan.Length <= 1)
			{
				return false;
			}
			ReadOnlySpan<char> readOnlySpan2 = readOnlySpan.Slice(1);
			if (readOnlySpan2.Length == 6)
			{
				byte b;
				byte b2;
				byte b3;
				if (App.TryParseHexByte(readOnlySpan2.Slice(0, 2), out b) && App.TryParseHexByte(readOnlySpan2.Slice(2, 2), out b2) && App.TryParseHexByte(readOnlySpan2.Slice(4, 2), out b3))
				{
					color = Color.FromRgb(b, b2, b3);
					return true;
				}
				return false;
			}
			else
			{
				if (readOnlySpan2.Length != 8)
				{
					return false;
				}
				byte b4;
				byte b5;
				byte b6;
				byte b7;
				if (App.TryParseHexByte(readOnlySpan2.Slice(0, 2), out b4) && App.TryParseHexByte(readOnlySpan2.Slice(2, 2), out b5) && App.TryParseHexByte(readOnlySpan2.Slice(4, 2), out b6) && App.TryParseHexByte(readOnlySpan2.Slice(6, 2), out b7))
				{
					color = Color.FromArgb(b4, b5, b6, b7);
					return true;
				}
				return false;
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00005782 File Offset: 0x00003982
		[NullableContext(0)]
		private static bool TryParseHexByte(ReadOnlySpan<char> hex, out byte value)
		{
			return byte.TryParse(hex, 515, CultureInfo.InvariantCulture, ref value);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005798 File Offset: 0x00003998
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public void InitializeComponent()
		{
			if (this._contentLoaded)
			{
				return;
			}
			this._contentLoaded = true;
			base.StartupUri = new Uri("Views/MainWindow.xaml", 2);
			Uri uri = new Uri("/Simple VS Manager;component/app.xaml", 2);
			Application.LoadComponent(this, uri);
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000057D9 File Offset: 0x000039D9
		[STAThread]
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "8.0.22.0")]
		public static void Main()
		{
			App app = new App();
			app.InitializeComponent();
			app.Run();
		}

		// Token: 0x04000075 RID: 117
		private static readonly string SingleInstanceMutexName = DevConfig.SingleInstanceMutexName;

		// Token: 0x04000076 RID: 118
		private static readonly Uri DarkVsThemeUri = new Uri("Resources/Themes/DarkVsTheme.xaml", 2);

		// Token: 0x04000077 RID: 119
		[Nullable(2)]
		private ResourceDictionary _activeTheme;

		// Token: 0x04000078 RID: 120
		[Nullable(2)]
		private Mutex _instanceMutex;

		// Token: 0x04000079 RID: 121
		private bool _ownsMutex;

		// Token: 0x0400007A RID: 122
		private bool _contentLoaded;

		// Token: 0x02000117 RID: 279
		[NullableContext(0)]
		private static class WindowActivator
		{
			// Token: 0x06001152 RID: 4434
			[DllImport("user32.dll")]
			private static extern bool SetForegroundWindow(IntPtr hWnd);

			// Token: 0x06001153 RID: 4435
			[DllImport("user32.dll")]
			private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

			// Token: 0x06001154 RID: 4436 RVA: 0x0005E516 File Offset: 0x0005C716
			public static void ShowAndActivate(IntPtr windowHandle)
			{
				if (windowHandle == IntPtr.Zero)
				{
					return;
				}
				App.WindowActivator.ShowWindow(windowHandle, 9);
				App.WindowActivator.SetForegroundWindow(windowHandle);
			}

			// Token: 0x0400087F RID: 2175
			private const int SwRestore = 9;
		}

		// Token: 0x02000118 RID: 280
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000880 RID: 2176
			[Nullable(0)]
			public static DispatcherUnhandledExceptionEventHandler <0>__OnDispatcherUnhandledException;

			// Token: 0x04000881 RID: 2177
			[Nullable(new byte[]
			{
				0,
				1
			})]
			public static EventHandler<FirstChanceExceptionEventArgs> <1>__OnFirstChanceException;
		}
	}
}
