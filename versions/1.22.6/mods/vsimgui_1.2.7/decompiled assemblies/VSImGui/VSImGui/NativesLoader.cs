using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace VSImGui
{
	// Token: 0x02000021 RID: 33
	[NullableContext(1)]
	[Nullable(0)]
	internal static class NativesLoader
	{
		// Token: 0x0600026C RID: 620 RVA: 0x0000D12C File Offset: 0x0000B32C
		public static bool Load(ILogger logger, ModSystem mod)
		{
			DllLoader loader = DllLoader.Loader();
			foreach (string library in NativesLoader._nativeLibraries)
			{
				if (!loader.Load(library, logger, mod.Mod))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000D194 File Offset: 0x0000B394
		// Note: this type is marked as 'beforefieldinit'.
		static NativesLoader()
		{
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add("cimgui");
			hashSet.Add("cimguizmo");
			hashSet.Add("cimnodes");
			hashSet.Add("cimplot");
			NativesLoader._nativeLibraries = hashSet;
		}

		// Token: 0x04000049 RID: 73
		private static readonly HashSet<string> _nativeLibraries;
	}
}
