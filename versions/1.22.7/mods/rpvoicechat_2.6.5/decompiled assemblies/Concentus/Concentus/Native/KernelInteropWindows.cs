using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Concentus.Native
{
	// Token: 0x0200006D RID: 109
	internal class KernelInteropWindows
	{
		// Token: 0x06000259 RID: 601 RVA: 0x00020144 File Offset: 0x0001E344
		[LibraryImport("kernel32.dll", SetLastError = true, StringMarshalling = 2)]
		[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "8.0.10.6711")]
		[SkipLocalsInit]
		internal unsafe static IntPtr LoadLibraryExW(string lpFileName, IntPtr hFile, uint dwFlags)
		{
			IntPtr result;
			int lastSystemError;
			fixed (char* pinnableReference = Utf16StringMarshaller.GetPinnableReference(lpFileName))
			{
				void* _lpFileName_native = (void*)pinnableReference;
				Marshal.SetLastSystemError(0);
				result = KernelInteropWindows.<LoadLibraryExW>g____PInvoke|8_0((ushort*)_lpFileName_native, hFile, dwFlags);
				lastSystemError = Marshal.GetLastSystemError();
			}
			Marshal.SetLastPInvokeError(lastSystemError);
			return result;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00020178 File Offset: 0x0001E378
		[LibraryImport("kernel32.dll", SetLastError = true)]
		[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "8.0.10.6711")]
		[SkipLocalsInit]
		[return: MarshalAs(2)]
		internal static bool FreeLibrary(IntPtr hModule)
		{
			Marshal.SetLastSystemError(0);
			int num = KernelInteropWindows.<FreeLibrary>g____PInvoke|9_0(hModule);
			int lastSystemError = Marshal.GetLastSystemError();
			bool result = num != 0;
			Marshal.SetLastPInvokeError(lastSystemError);
			return result;
		}

		// Token: 0x0600025B RID: 603
		[LibraryImport("kernel32.dll", SetLastError = false)]
		[DllImport("kernel32.dll", ExactSpelling = true)]
		internal static extern uint GetLastError();

		// Token: 0x0600025C RID: 604
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern void GetSystemInfo(ref KernelInteropWindows.SYSTEM_INFO Info);

		// Token: 0x0600025E RID: 606
		[CompilerGenerated]
		[DllImport("kernel32.dll", EntryPoint = "LoadLibraryExW", ExactSpelling = true)]
		internal unsafe static extern IntPtr <LoadLibraryExW>g____PInvoke|8_0(ushort* __lpFileName_native, IntPtr __hFile_native, uint __dwFlags_native);

		// Token: 0x0600025F RID: 607
		[CompilerGenerated]
		[DllImport("kernel32.dll", EntryPoint = "FreeLibrary", ExactSpelling = true)]
		internal static extern int <FreeLibrary>g____PInvoke|9_0(IntPtr __hModule_native);

		// Token: 0x04000388 RID: 904
		internal const uint LOAD_LIBRARY_AS_DATAFILE = 2U;

		// Token: 0x04000389 RID: 905
		internal const uint LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 4096U;

		// Token: 0x0400038A RID: 906
		internal const ushort PROCESSOR_ARCHITECTURE_INTEL = 0;

		// Token: 0x0400038B RID: 907
		internal const ushort PROCESSOR_ARCHITECTURE_AMD64 = 9;

		// Token: 0x0400038C RID: 908
		internal const ushort PROCESSOR_ARCHITECTURE_ARM = 5;

		// Token: 0x0400038D RID: 909
		internal const ushort PROCESSOR_ARCHITECTURE_ARM64 = 18;

		// Token: 0x0400038E RID: 910
		internal const ushort PROCESSOR_ARCHITECTURE_IA64 = 6;

		// Token: 0x0400038F RID: 911
		internal const ushort PROCESSOR_ARCHITECTURE_UNKNOWN = 65535;

		// Token: 0x020000A7 RID: 167
		internal struct SYSTEM_INFO
		{
			// Token: 0x040005F3 RID: 1523
			internal ushort wProcessorArchitecture;

			// Token: 0x040005F4 RID: 1524
			internal ushort wReserved;

			// Token: 0x040005F5 RID: 1525
			internal uint dwPageSize;

			// Token: 0x040005F6 RID: 1526
			internal IntPtr lpMinimumApplicationAddress;

			// Token: 0x040005F7 RID: 1527
			internal IntPtr lpMaximumApplicationAddress;

			// Token: 0x040005F8 RID: 1528
			internal IntPtr dwActiveProcessorMask;

			// Token: 0x040005F9 RID: 1529
			internal uint dwNumberOfProcessors;

			// Token: 0x040005FA RID: 1530
			internal uint dwProcessorType;

			// Token: 0x040005FB RID: 1531
			internal uint dwAllocationGranularity;

			// Token: 0x040005FC RID: 1532
			internal ushort wProcessorLevel;

			// Token: 0x040005FD RID: 1533
			internal ushort wProcessorRevision;
		}
	}
}
