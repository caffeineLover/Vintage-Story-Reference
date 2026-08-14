using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Vintagestory.API.Common;

namespace VSImGui
{
	// Token: 0x02000024 RID: 36
	[NullableContext(1)]
	[Nullable(0)]
	internal class LinuxDllLoader : DllLoader
	{
		// Token: 0x06000277 RID: 631
		[DllImport("libdl.so.2")]
		private static extern IntPtr dlopen(string fileName, int flags);

		// Token: 0x06000278 RID: 632
		[DllImport("libdl.so.2")]
		private static extern IntPtr dlerror();

		// Token: 0x06000279 RID: 633 RVA: 0x0000D3C4 File Offset: 0x0000B5C4
		protected override bool Load(string dllPath, ILogger logger)
		{
			IntPtr? intPtr = new IntPtr?(LinuxDllLoader.dlopen(dllPath, 1));
			IntPtr zero = IntPtr.Zero;
			if (intPtr.GetValueOrDefault() == zero & intPtr != null)
			{
				Exception innerException = new Win32Exception();
				Exception exception = new DllNotFoundException("Unable to load library: " + dllPath, innerException);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(52, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to load embedded DLL:\n\nDlError:\n");
				defaultInterpolatedStringHandler.AppendFormatted(Marshal.PtrToStringAnsi(LinuxDllLoader.dlerror()));
				defaultInterpolatedStringHandler.AppendLiteral("\n\nException:\n");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				logger.Fatal(defaultInterpolatedStringHandler.ToStringAndClear());
				return false;
			}
			return true;
		}
	}
}
