using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Concentus.Native
{
	// Token: 0x0200006C RID: 108
	internal class KernelInteropMacOS
	{
		// Token: 0x06000254 RID: 596 RVA: 0x000200DC File Offset: 0x0001E2DC
		[LibraryImport("libSystem.dylib", StringMarshallingCustomType = typeof(AnsiStringMarshaller))]
		[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "8.0.10.6711")]
		[SkipLocalsInit]
		internal unsafe static IntPtr dlopen(string fileName, int flags)
		{
			IntPtr result = (IntPtr)0;
			AnsiStringMarshaller.ManagedToUnmanagedIn managedToUnmanagedIn = default(AnsiStringMarshaller.ManagedToUnmanagedIn);
			try
			{
				int bufferSize = AnsiStringMarshaller.ManagedToUnmanagedIn.BufferSize;
				Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)bufferSize], bufferSize);
				managedToUnmanagedIn.FromManaged(fileName, span);
				result = KernelInteropMacOS.<dlopen>g____PInvoke|1_0(managedToUnmanagedIn.ToUnmanaged(), flags);
			}
			finally
			{
				managedToUnmanagedIn.Free();
			}
			return result;
		}

		// Token: 0x06000255 RID: 597
		[LibraryImport("libSystem.dylib")]
		[DllImport("libSystem.dylib", ExactSpelling = true)]
		internal static extern int dlclose(IntPtr handle);

		// Token: 0x06000256 RID: 598
		[LibraryImport("libSystem.dylib")]
		[DllImport("libSystem.dylib", ExactSpelling = true)]
		internal static extern IntPtr dlerror();

		// Token: 0x06000258 RID: 600
		[CompilerGenerated]
		[DllImport("libSystem.dylib", EntryPoint = "dlopen", ExactSpelling = true)]
		internal unsafe static extern IntPtr <dlopen>g____PInvoke|1_0(byte* __fileName_native, int __flags_native);

		// Token: 0x04000387 RID: 903
		internal const int RTLD_NOW = 2;
	}
}
