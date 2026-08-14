using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Concentus.Native
{
	// Token: 0x0200006B RID: 107
	internal class KernelInteropLinux
	{
		// Token: 0x0600024D RID: 589 RVA: 0x0001FE38 File Offset: 0x0001E038
		private static KernelInteropLinux.ILibDl GetLibDL()
		{
			KernelInteropLinux.ILibDl result;
			try
			{
				KernelInteropLinux.LibDL libDL = new KernelInteropLinux.LibDL();
				libDL.DLError();
				result = libDL;
			}
			catch (Exception)
			{
				result = new KernelInteropLinux.LibDL2();
			}
			return result;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0001FE70 File Offset: 0x0001E070
		public static IntPtr dlopen(string fileName, int flags)
		{
			return KernelInteropLinux._libDlImpl.Value.DLOpen(fileName, flags);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0001FE83 File Offset: 0x0001E083
		public static int dlclose(IntPtr handle)
		{
			return KernelInteropLinux._libDlImpl.Value.DLClose(handle);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0001FE95 File Offset: 0x0001E095
		public static IntPtr dlerror()
		{
			return KernelInteropLinux._libDlImpl.Value.DLError();
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0001FEA8 File Offset: 0x0001E0A8
		internal static PlatformArchitecture? TryGetArchForUnix(TextWriter logger)
		{
			try
			{
				if (logger != null)
				{
					logger.WriteLine("Running uname to determine system info...");
				}
				using (Process process = Process.Start(new ProcessStartInfo
				{
					FileName = "uname",
					Arguments = "-m",
					UseShellExecute = false,
					RedirectStandardOutput = true,
					CreateNoWindow = true,
					WindowStyle = 1
				}))
				{
					using (StreamReader standardOutput = process.StandardOutput)
					{
						string text = standardOutput.ReadToEnd();
						if (string.IsNullOrEmpty(text))
						{
							return default(PlatformArchitecture?);
						}
						text = text.Trim();
						PlatformArchitecture platformArchitecture;
						if (KernelInteropLinux.POSSIBLE_UNIX_MACHINES.TryGetValue(text, ref platformArchitecture))
						{
							if (logger != null)
							{
								logger.WriteLine("Got architecture from uname: " + text);
							}
							return new PlatformArchitecture?(platformArchitecture);
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (logger != null)
				{
					logger.WriteLine(ex);
				}
			}
			return default(PlatformArchitecture?);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0001FFC0 File Offset: 0x0001E1C0
		// Note: this type is marked as 'beforefieldinit'.
		static KernelInteropLinux()
		{
			Dictionary<string, PlatformArchitecture> dictionary = new Dictionary<string, PlatformArchitecture>(StringComparer.OrdinalIgnoreCase);
			dictionary.Add("amd64", PlatformArchitecture.X64);
			dictionary.Add("x86_64", PlatformArchitecture.X64);
			dictionary.Add("i686-64", PlatformArchitecture.X64);
			dictionary.Add("x86", PlatformArchitecture.I386);
			dictionary.Add("i686", PlatformArchitecture.I386);
			dictionary.Add("i686-AT386", PlatformArchitecture.I386);
			dictionary.Add("i386", PlatformArchitecture.I386);
			dictionary.Add("x86pc", PlatformArchitecture.I386);
			dictionary.Add("i86pc", PlatformArchitecture.I386);
			dictionary.Add("armv6l", PlatformArchitecture.ArmV6);
			dictionary.Add("armv7l", PlatformArchitecture.ArmV7);
			dictionary.Add("arm64", PlatformArchitecture.Arm64);
			dictionary.Add("aarch64", PlatformArchitecture.Arm64);
			dictionary.Add("aarch64_be", PlatformArchitecture.Arm64);
			dictionary.Add("armv8l", PlatformArchitecture.Arm64);
			dictionary.Add("armv8b", PlatformArchitecture.Arm64);
			dictionary.Add("mips64", PlatformArchitecture.Mips64);
			dictionary.Add("ppc64", PlatformArchitecture.PowerPC64);
			dictionary.Add("ppc64le", PlatformArchitecture.PowerPC64);
			KernelInteropLinux.POSSIBLE_UNIX_MACHINES = dictionary;
		}

		// Token: 0x04000384 RID: 900
		internal const int RTLD_NOW = 2;

		// Token: 0x04000385 RID: 901
		private static Lazy<KernelInteropLinux.ILibDl> _libDlImpl = new Lazy<KernelInteropLinux.ILibDl>(new Func<KernelInteropLinux.ILibDl>(KernelInteropLinux.GetLibDL), 2);

		// Token: 0x04000386 RID: 902
		internal static readonly IReadOnlyDictionary<string, PlatformArchitecture> POSSIBLE_UNIX_MACHINES;

		// Token: 0x020000A4 RID: 164
		private interface ILibDl
		{
			// Token: 0x0600054C RID: 1356
			IntPtr DLOpen(string fileName, int flags);

			// Token: 0x0600054D RID: 1357
			int DLClose(IntPtr handle);

			// Token: 0x0600054E RID: 1358
			IntPtr DLError();
		}

		// Token: 0x020000A5 RID: 165
		private class LibDL : KernelInteropLinux.ILibDl
		{
			// Token: 0x0600054F RID: 1359 RVA: 0x0003644B File Offset: 0x0003464B
			public IntPtr DLOpen(string fileName, int flags)
			{
				return KernelInteropLinux.LibDL.dlopen(fileName, flags);
			}

			// Token: 0x06000550 RID: 1360 RVA: 0x00036454 File Offset: 0x00034654
			public int DLClose(IntPtr handle)
			{
				return KernelInteropLinux.LibDL.dlclose(handle);
			}

			// Token: 0x06000551 RID: 1361 RVA: 0x0003645C File Offset: 0x0003465C
			public IntPtr DLError()
			{
				return KernelInteropLinux.LibDL.dlerror();
			}

			// Token: 0x06000552 RID: 1362 RVA: 0x00036464 File Offset: 0x00034664
			[LibraryImport("libdl.so", StringMarshallingCustomType = typeof(AnsiStringMarshaller))]
			[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "8.0.10.6711")]
			[SkipLocalsInit]
			private unsafe static IntPtr dlopen(string fileName, int flags)
			{
				IntPtr result = (IntPtr)0;
				AnsiStringMarshaller.ManagedToUnmanagedIn managedToUnmanagedIn = default(AnsiStringMarshaller.ManagedToUnmanagedIn);
				try
				{
					int bufferSize = AnsiStringMarshaller.ManagedToUnmanagedIn.BufferSize;
					Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)bufferSize], bufferSize);
					managedToUnmanagedIn.FromManaged(fileName, span);
					result = KernelInteropLinux.LibDL.<dlopen>g____PInvoke|3_0(managedToUnmanagedIn.ToUnmanaged(), flags);
				}
				finally
				{
					managedToUnmanagedIn.Free();
				}
				return result;
			}

			// Token: 0x06000553 RID: 1363
			[LibraryImport("libdl.so")]
			[DllImport("libdl.so", ExactSpelling = true)]
			private static extern int dlclose(IntPtr handle);

			// Token: 0x06000554 RID: 1364
			[LibraryImport("libdl.so")]
			[DllImport("libdl.so", ExactSpelling = true)]
			private static extern IntPtr dlerror();

			// Token: 0x06000556 RID: 1366
			[CompilerGenerated]
			[DllImport("libdl.so", EntryPoint = "dlopen", ExactSpelling = true)]
			internal unsafe static extern IntPtr <dlopen>g____PInvoke|3_0(byte* __fileName_native, int __flags_native);
		}

		// Token: 0x020000A6 RID: 166
		private class LibDL2 : KernelInteropLinux.ILibDl
		{
			// Token: 0x06000557 RID: 1367 RVA: 0x000364CC File Offset: 0x000346CC
			public IntPtr DLOpen(string fileName, int flags)
			{
				return KernelInteropLinux.LibDL2.dlopen(fileName, flags);
			}

			// Token: 0x06000558 RID: 1368 RVA: 0x000364D5 File Offset: 0x000346D5
			public int DLClose(IntPtr handle)
			{
				return KernelInteropLinux.LibDL2.dlclose(handle);
			}

			// Token: 0x06000559 RID: 1369 RVA: 0x000364DD File Offset: 0x000346DD
			public IntPtr DLError()
			{
				return KernelInteropLinux.LibDL2.dlerror();
			}

			// Token: 0x0600055A RID: 1370 RVA: 0x000364E4 File Offset: 0x000346E4
			[LibraryImport("libdl.so.2", StringMarshallingCustomType = typeof(AnsiStringMarshaller))]
			[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "8.0.10.6711")]
			[SkipLocalsInit]
			private unsafe static IntPtr dlopen(string fileName, int flags)
			{
				IntPtr result = (IntPtr)0;
				AnsiStringMarshaller.ManagedToUnmanagedIn managedToUnmanagedIn = default(AnsiStringMarshaller.ManagedToUnmanagedIn);
				try
				{
					int bufferSize = AnsiStringMarshaller.ManagedToUnmanagedIn.BufferSize;
					Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)bufferSize], bufferSize);
					managedToUnmanagedIn.FromManaged(fileName, span);
					result = KernelInteropLinux.LibDL2.<dlopen>g____PInvoke|3_0(managedToUnmanagedIn.ToUnmanaged(), flags);
				}
				finally
				{
					managedToUnmanagedIn.Free();
				}
				return result;
			}

			// Token: 0x0600055B RID: 1371
			[LibraryImport("libdl.so.2")]
			[DllImport("libdl.so.2", ExactSpelling = true)]
			private static extern int dlclose(IntPtr handle);

			// Token: 0x0600055C RID: 1372
			[LibraryImport("libdl.so.2")]
			[DllImport("libdl.so.2", ExactSpelling = true)]
			private static extern IntPtr dlerror();

			// Token: 0x0600055E RID: 1374
			[CompilerGenerated]
			[DllImport("libdl.so.2", EntryPoint = "dlopen", ExactSpelling = true)]
			internal unsafe static extern IntPtr <dlopen>g____PInvoke|3_0(byte* __fileName_native, int __flags_native);
		}
	}
}
