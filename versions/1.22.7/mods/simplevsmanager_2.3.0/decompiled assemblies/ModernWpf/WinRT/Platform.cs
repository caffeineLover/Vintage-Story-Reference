using System;
using System.Runtime.InteropServices;

namespace WinRT
{
	// Token: 0x020001AB RID: 427
	internal sealed class Platform
	{
		// Token: 0x06000DCD RID: 3533
		[DllImport("api-ms-win-core-com-l1-1-0.dll")]
		internal unsafe static extern int CoCreateInstance(ref Guid clsid, IntPtr outer, uint clsContext, ref Guid iid, IntPtr* instance);

		// Token: 0x06000DCE RID: 3534
		[DllImport("api-ms-win-core-com-l1-1-0.dll")]
		internal static extern int CoDecrementMTAUsage(IntPtr cookie);

		// Token: 0x06000DCF RID: 3535
		[DllImport("api-ms-win-core-com-l1-1-0.dll")]
		internal unsafe static extern int CoIncrementMTAUsage(IntPtr* cookie);

		// Token: 0x06000DD0 RID: 3536
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(2)]
		internal static extern bool FreeLibrary(IntPtr moduleHandle);

		// Token: 0x06000DD1 RID: 3537
		[DllImport("kernel32.dll", BestFitMapping = false, EntryPoint = "GetProcAddress", SetLastError = true)]
		internal unsafe static extern void* TryGetProcAddress(IntPtr moduleHandle, [MarshalAs(20)] string functionName);

		// Token: 0x06000DD2 RID: 3538 RVA: 0x0002A0AB File Offset: 0x000282AB
		internal unsafe static void* GetProcAddress(IntPtr moduleHandle, string functionName)
		{
			void* ptr = Platform.TryGetProcAddress(moduleHandle, functionName);
			if (ptr == null)
			{
				Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error(), new IntPtr(-1));
			}
			return ptr;
		}

		// Token: 0x06000DD3 RID: 3539
		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern IntPtr LoadLibraryExW([MarshalAs(21)] string fileName, IntPtr fileHandle, uint flags);

		// Token: 0x06000DD4 RID: 3540
		[DllImport("api-ms-win-core-winrt-l1-1-0.dll")]
		internal unsafe static extern int RoGetActivationFactory(IntPtr runtimeClassId, ref Guid iid, IntPtr* factory);

		// Token: 0x06000DD5 RID: 3541
		[DllImport("api-ms-win-core-winrt-string-l1-1-0.dll", CallingConvention = 3)]
		internal unsafe static extern int WindowsCreateString([MarshalAs(21)] string sourceString, int length, IntPtr* hstring);

		// Token: 0x06000DD6 RID: 3542
		[DllImport("api-ms-win-core-winrt-string-l1-1-0.dll", CallingConvention = 3)]
		internal unsafe static extern int WindowsCreateStringReference(char* sourceString, int length, IntPtr* hstring_header, IntPtr* hstring);

		// Token: 0x06000DD7 RID: 3543
		[DllImport("api-ms-win-core-winrt-string-l1-1-0.dll", CallingConvention = 3)]
		internal static extern int WindowsDeleteString(IntPtr hstring);

		// Token: 0x06000DD8 RID: 3544
		[DllImport("api-ms-win-core-winrt-string-l1-1-0.dll", CallingConvention = 3)]
		internal unsafe static extern int WindowsDuplicateString(IntPtr sourceString, IntPtr* hstring);

		// Token: 0x06000DD9 RID: 3545
		[DllImport("api-ms-win-core-winrt-string-l1-1-0.dll", CallingConvention = 3)]
		internal unsafe static extern char* WindowsGetStringRawBuffer(IntPtr hstring, uint* length);

		// Token: 0x06000DDA RID: 3546
		[DllImport("api-ms-win-core-com-l1-1-1.dll", CallingConvention = 3)]
		internal unsafe static extern int RoGetAgileReference(uint options, ref Guid iid, IntPtr unknown, IntPtr* agileReference);
	}
}
