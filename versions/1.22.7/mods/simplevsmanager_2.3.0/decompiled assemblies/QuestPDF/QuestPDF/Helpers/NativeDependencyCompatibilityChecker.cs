using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace QuestPDF.Helpers
{
	// Token: 0x02000075 RID: 117
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class NativeDependencyCompatibilityChecker
	{
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002DD RID: 733 RVA: 0x000077CA File Offset: 0x000059CA
		// (set) Token: 0x060002DE RID: 734 RVA: 0x000077D2 File Offset: 0x000059D2
		private bool IsCompatibilityChecked { get; set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002DF RID: 735 RVA: 0x000077DB File Offset: 0x000059DB
		// (set) Token: 0x060002E0 RID: 736 RVA: 0x000077E3 File Offset: 0x000059E3
		public Action ExecuteNativeCode { get; set; } = delegate()
		{
		};

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x000077EC File Offset: 0x000059EC
		// (set) Token: 0x060002E2 RID: 738 RVA: 0x000077F4 File Offset: 0x000059F4
		public Func<bool> CheckNativeLibraryVersion { get; set; } = () => true;

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x000077FD File Offset: 0x000059FD
		// (set) Token: 0x060002E4 RID: 740 RVA: 0x00007805 File Offset: 0x00005A05
		public Func<string> ExceptionHint { get; set; } = () => string.Empty;

		// Token: 0x060002E5 RID: 741 RVA: 0x0000780E File Offset: 0x00005A0E
		public void Test()
		{
			if (this.IsCompatibilityChecked)
			{
				return;
			}
			this.TestOnce();
			this.IsCompatibilityChecked = true;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00007828 File Offset: 0x00005A28
		private void TestOnce()
		{
			if (this.IsCompatibilityChecked)
			{
				return;
			}
			Exception ex = this.CheckIfExceptionIsThrownWhenLoadingNativeDependencies();
			if (ex == null)
			{
				if (!this.CheckNativeLibraryVersion.Invoke())
				{
					throw new Exception("The QuestPDF library has encountered an issue while loading one of its dependencies.\n\nThe loaded native library version is incompatible with the current QuestPDF version. To resolve this issue, please: 1) Clean and rebuild your solution, 2) Remove the bin and obj folders, and 3) Ensure all projects in your solution use the same QuestPDF NuGet package version.");
				}
				return;
			}
			else
			{
				if (!NativeDependencyProvider.IsCurrentPlatformSupported())
				{
					this.<TestOnce>g__ThrowCompatibilityException|18_0(ex);
				}
				NativeDependencyProvider.EnsureNativeFileAvailability();
				ex = this.CheckIfExceptionIsThrownWhenLoadingNativeDependencies();
				if (ex == null)
				{
					return;
				}
				this.<TestOnce>g__ThrowCompatibilityException|18_0(ex);
				return;
			}
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x00007888 File Offset: 0x00005A88
		[NullableContext(2)]
		private Exception CheckIfExceptionIsThrownWhenLoadingNativeDependencies()
		{
			Exception result;
			try
			{
				this.ExecuteNativeCode.Invoke();
				result = null;
			}
			catch (Exception result)
			{
			}
			return result;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000794C File Offset: 0x00005B4C
		[CompilerGenerated]
		private void <TestOnce>g__ThrowCompatibilityException|18_0(Exception innerException)
		{
			string text = string.Join(", ", NativeDependencyProvider.SupportedPlatforms);
			string runtimePlatform = NativeDependencyProvider.GetRuntimePlatform();
			bool flag = Enumerable.Contains<string>(NativeDependencyProvider.SupportedPlatforms, runtimePlatform);
			string text2 = "The QuestPDF library has encountered an issue while loading one of its dependencies.";
			if (!flag)
			{
				string text3 = text2;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(123, 3);
				defaultInterpolatedStringHandler.AppendFormatted("\n\n");
				defaultInterpolatedStringHandler.AppendLiteral("Your runtime is not supported by QuestPDF. ");
				defaultInterpolatedStringHandler.AppendLiteral("The following runtimes are supported: ");
				defaultInterpolatedStringHandler.AppendFormatted(text);
				defaultInterpolatedStringHandler.AppendLiteral(". ");
				defaultInterpolatedStringHandler.AppendLiteral("Your current runtime is detected as '");
				defaultInterpolatedStringHandler.AppendFormatted(runtimePlatform);
				defaultInterpolatedStringHandler.AppendLiteral("'. ");
				text2 = text3 + defaultInterpolatedStringHandler.ToStringAndClear();
			}
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				text2 += "\n\nPlease always set the 'Platform target' to either 'X86' or 'X64' in your startup project settings. Please do not use the 'Any CPU' option.";
			}
			if (RuntimeInformation.ProcessArchitecture == 2)
			{
				text2 += "\n\nPlease consider setting the 'Platform target' property to 'Arm64' in your project settings.";
			}
			string text4 = this.ExceptionHint.Invoke();
			if (!string.IsNullOrEmpty(text4))
			{
				text2 = text2 + "\n\n" + text4;
			}
			if (flag)
			{
				Version glibcVersion = NativeDependencyProvider.GetGlibcVersion();
				if (glibcVersion != null && glibcVersion < NativeDependencyCompatibilityChecker.RequiredGlibcVersionOnLinux)
				{
					string text5 = text2;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
					defaultInterpolatedStringHandler2..ctor(120, 3);
					defaultInterpolatedStringHandler2.AppendFormatted("\n\n");
					defaultInterpolatedStringHandler2.AppendLiteral("Please consider updating your operating system distribution. ");
					defaultInterpolatedStringHandler2.AppendLiteral("Current GLIBC version: ");
					defaultInterpolatedStringHandler2.AppendFormatted<Version>(glibcVersion);
					defaultInterpolatedStringHandler2.AppendLiteral(". ");
					defaultInterpolatedStringHandler2.AppendLiteral("The minimum required version is ");
					defaultInterpolatedStringHandler2.AppendFormatted<Version>(NativeDependencyCompatibilityChecker.RequiredGlibcVersionOnLinux);
					defaultInterpolatedStringHandler2.AppendLiteral(". ");
					text2 = text5 + defaultInterpolatedStringHandler2.ToStringAndClear();
				}
				else
				{
					text2 += "\n\nIf the problem persists, it may mean that your current operating system distribution is outdated. For optimal compatibility, please consider updating it to a more recent version.";
				}
			}
			throw new Exception(text2, innerException);
		}

		// Token: 0x0400022E RID: 558
		private static readonly Version RequiredGlibcVersionOnLinux = Version.Parse("2.29");
	}
}
