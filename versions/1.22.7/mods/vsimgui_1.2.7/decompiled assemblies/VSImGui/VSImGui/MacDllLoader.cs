using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Vintagestory.API.Common;

namespace VSImGui
{
	// Token: 0x02000025 RID: 37
	[NullableContext(1)]
	[Nullable(0)]
	internal class MacDllLoader : DllLoader
	{
		// Token: 0x0600027B RID: 635
		[DllImport("libdl.dylib")]
		private static extern IntPtr dlopen(string filename, int flags);

		// Token: 0x0600027C RID: 636
		[DllImport("libdl.dylib")]
		private static extern IntPtr dlerror();

		// Token: 0x0600027D RID: 637 RVA: 0x0000D464 File Offset: 0x0000B664
		protected override bool Load(string dllPath, ILogger logger)
		{
			IntPtr? handle = new IntPtr?(MacDllLoader.dlopen(dllPath, 1));
			IntPtr? intPtr = handle;
			IntPtr zero = IntPtr.Zero;
			if (intPtr.GetValueOrDefault() == zero & intPtr != null)
			{
				dllPath = dllPath.Replace("native/mac/", "native/macarm/");
				handle = new IntPtr?(MacDllLoader.dlopen(dllPath, 1));
			}
			intPtr = handle;
			zero = IntPtr.Zero;
			if (intPtr.GetValueOrDefault() == zero & intPtr != null)
			{
				Exception innerException = new Win32Exception();
				Exception exception = new DllNotFoundException("Unable to load library: " + dllPath, innerException);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(52, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to load embedded DLL:\n\nDlError:\n");
				defaultInterpolatedStringHandler.AppendFormatted(Marshal.PtrToStringAnsi(MacDllLoader.dlerror()));
				defaultInterpolatedStringHandler.AppendLiteral("\n\nException:\n");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				logger.Fatal(defaultInterpolatedStringHandler.ToStringAndClear());
				return false;
			}
			return true;
		}
	}
}
