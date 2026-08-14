using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OpenTK.Audio.OpenAL
{
	// Token: 0x02000048 RID: 72
	internal static class ALLoader
	{
		// Token: 0x060000F9 RID: 249 RVA: 0x00007233 File Offset: 0x00005433
		static ALLoader()
		{
			ALLoader.RegisterDllResolver();
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000724A File Offset: 0x0000544A
		internal static void RegisterDllResolver()
		{
			if (!ALLoader.RegisteredResolver)
			{
				NativeLibrary.SetDllImportResolver(typeof(ALLoader).Assembly, new DllImportResolver(ALLoader.ImportResolver));
				ALLoader.RegisteredResolver = true;
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0000727C File Offset: 0x0000547C
		private static IntPtr ImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
		{
			if (!(libraryName == "AL") && !(libraryName == "AL"))
			{
				return NativeLibrary.Load(libraryName, assembly, searchPath);
			}
			string libraryName2 = ALLoader.ALLibraryNameContainer.GetLibraryName();
			IntPtr result;
			if (!NativeLibrary.TryLoad(libraryName2, assembly, searchPath, ref result))
			{
				throw new DllNotFoundException(string.Concat(new string[]
				{
					"Could not load the dll '",
					libraryName2,
					"' (this load is intercepted, specified in DllImport as '",
					libraryName,
					"'), this could mean that OpenAL is not installed on this system or that we can't find the dll."
				}));
			}
			return result;
		}

		// Token: 0x0400022D RID: 557
		private static readonly OpenALLibraryNameContainer ALLibraryNameContainer = new OpenALLibraryNameContainer();

		// Token: 0x0400022E RID: 558
		private static bool RegisteredResolver = false;
	}
}
