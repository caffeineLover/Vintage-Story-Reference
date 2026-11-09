using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000066 RID: 102
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModActivityLoggingService
	{
		// Token: 0x0600099F RID: 2463 RVA: 0x0003A3D6 File Offset: 0x000385D6
		public ModActivityLoggingService(UserConfigurationService userConfiguration)
		{
			this._userConfiguration = userConfiguration;
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0003A3E8 File Offset: 0x000385E8
		[NullableContext(2)]
		public void LogModUpdate([Nullable(1)] string modName, string oldVersion, string newVersion)
		{
			if (!this._userConfiguration.LogModUpdates)
			{
				return;
			}
			string text = string.IsNullOrWhiteSpace(oldVersion) ? "unknown" : oldVersion;
			string text2 = string.IsNullOrWhiteSpace(newVersion) ? "unknown" : newVersion;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(20, 3);
			defaultInterpolatedStringHandler.AppendFormatted(modName);
			defaultInterpolatedStringHandler.AppendLiteral(" updated from v");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral(" to v");
			defaultInterpolatedStringHandler.AppendFormatted(text2);
			string message = defaultInterpolatedStringHandler.ToStringAndClear();
			this.AppendLogEntry(message);
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0003A470 File Offset: 0x00038670
		public void LogModInstall(string modName, [Nullable(2)] string version)
		{
			if (!this._userConfiguration.LogModInstalls)
			{
				return;
			}
			string text = string.IsNullOrWhiteSpace(version) ? string.Empty : (" v" + version);
			string message = modName + " installed" + text;
			this.AppendLogEntry(message);
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0003A4BC File Offset: 0x000386BC
		public void LogModDeletion(string modName)
		{
			if (!this._userConfiguration.LogModDeletions)
			{
				return;
			}
			string message = modName + " deleted";
			this.AppendLogEntry(message);
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0003A4EC File Offset: 0x000386EC
		public void LogError(string message, [Nullable(2)] Exception exception = null)
		{
			if (!this._userConfiguration.LogErrorsAndExceptions)
			{
				return;
			}
			string text;
			if (exception == null)
			{
				text = "ERROR: " + message;
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(23, 3);
				defaultInterpolatedStringHandler.AppendLiteral("ERROR: ");
				defaultInterpolatedStringHandler.AppendFormatted(message);
				defaultInterpolatedStringHandler.AppendLiteral(" - Exception: ");
				defaultInterpolatedStringHandler.AppendFormatted(exception.GetType().Name);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(exception.Message);
				text = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			string message2 = text;
			this.AppendLogEntry(message2);
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0003A57C File Offset: 0x0003877C
		public void LogDiagnostic(string message)
		{
			if (!this._userConfiguration.LogErrorsAndExceptions)
			{
				return;
			}
			string message2 = "DIAGNOSTIC: " + message;
			this.AppendLogEntry(message2);
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0003A5AA File Offset: 0x000387AA
		public void LogAppLaunch()
		{
			this.LogLifecycleEvent("App launched");
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x0003A5B7 File Offset: 0x000387B7
		public void LogAppExit()
		{
			this.LogLifecycleEvent("App exited");
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0003A5C4 File Offset: 0x000387C4
		public void LogModLoadingTimingSummary(string summary)
		{
			if (!this._userConfiguration.LogErrorsAndExceptions)
			{
				return;
			}
			this.AppendLogEntry(summary);
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0003A5DC File Offset: 0x000387DC
		private void AppendLogEntry(string message)
		{
			try
			{
				string text = Path.Combine(this._userConfiguration.GetConfigurationDirectory(), "Simple VS Manager logs.txt");
				string text2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + message;
				File.AppendAllText(text, text2 + Environment.NewLine);
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
			{
			}
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0003A66C File Offset: 0x0003886C
		private void LogLifecycleEvent(string message)
		{
			if (!this._userConfiguration.LogAppLaunchAndExit)
			{
				return;
			}
			this.AppendLogEntry(message);
		}

		// Token: 0x04000497 RID: 1175
		private const string LogFileName = "Simple VS Manager logs.txt";

		// Token: 0x04000498 RID: 1176
		private readonly UserConfigurationService _userConfiguration;
	}
}
