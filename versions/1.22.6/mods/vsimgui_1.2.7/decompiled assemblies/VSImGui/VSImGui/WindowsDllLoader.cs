using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Vintagestory.API.Common;

namespace VSImGui
{
	// Token: 0x02000023 RID: 35
	[NullableContext(1)]
	[Nullable(0)]
	internal class WindowsDllLoader : DllLoader
	{
		// Token: 0x06000272 RID: 626
		[DllImport("kernel32", CharSet = 3, SetLastError = true)]
		private static extern IntPtr LoadLibrary(string fileName);

		// Token: 0x06000273 RID: 627
		[DllImport("kernel32")]
		private static extern uint GetLastError();

		// Token: 0x06000274 RID: 628
		[DllImport("kernel32")]
		private static extern uint FormatMessage(uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, [Out] StringBuilder lpBuffer, uint nSize, IntPtr[] Arguments);

		// Token: 0x06000275 RID: 629 RVA: 0x0000D2F4 File Offset: 0x0000B4F4
		protected override bool Load(string dllPath, ILogger logger)
		{
			IntPtr? intPtr = new IntPtr?(WindowsDllLoader.LoadLibrary(dllPath));
			IntPtr zero = IntPtr.Zero;
			if (intPtr.GetValueOrDefault() == zero & intPtr != null)
			{
				uint errorCode = WindowsDllLoader.GetLastError();
				StringBuilder errorMessageBuilder = new StringBuilder(255);
				WindowsDllLoader.FormatMessage(4096U, IntPtr.Zero, errorCode, 0U, errorMessageBuilder, 255U, null);
				string errorMessage = errorMessageBuilder.ToString();
				Exception innerException = new Win32Exception();
				Exception exception = new DllNotFoundException("Unable to load library: " + dllPath, innerException);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(52, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to load embedded DLL:\n\nDlError:\n");
				defaultInterpolatedStringHandler.AppendFormatted(errorMessage);
				defaultInterpolatedStringHandler.AppendLiteral("\n\nException:\n");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				logger.Fatal(defaultInterpolatedStringHandler.ToStringAndClear());
				return false;
			}
			return true;
		}
	}
}
