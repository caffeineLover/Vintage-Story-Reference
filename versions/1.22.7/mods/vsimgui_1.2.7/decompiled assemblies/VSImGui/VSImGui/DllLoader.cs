using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.Common;

namespace VSImGui
{
	// Token: 0x02000022 RID: 34
	[NullableContext(1)]
	[Nullable(0)]
	internal abstract class DllLoader
	{
		// Token: 0x0600026E RID: 622 RVA: 0x0000D1D0 File Offset: 0x0000B3D0
		public static DllLoader Loader()
		{
			DllLoader result;
			switch (RuntimeEnv.OS)
			{
			case 0:
				result = new WindowsDllLoader();
				break;
			case 1:
				result = new MacDllLoader();
				break;
			case 2:
				result = new LinuxDllLoader();
				break;
			default:
				result = new WindowsDllLoader();
				break;
			}
			return result;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000D220 File Offset: 0x0000B420
		public bool Load(string dllName, ILogger logger, Mod mod)
		{
			string text;
			switch (RuntimeEnv.OS)
			{
			case 0:
				text = ".dll";
				break;
			case 1:
				text = ".dylib";
				break;
			case 2:
				text = ".so";
				break;
			default:
				text = ".so";
				break;
			}
			string suffix = text;
			switch (RuntimeEnv.OS)
			{
			case 0:
				text = "win/";
				break;
			case 1:
				text = "mac/";
				break;
			case 2:
				text = "linux/";
				break;
			default:
				text = "linux";
				break;
			}
			string prefix = text;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(8, 4);
			defaultInterpolatedStringHandler.AppendFormatted(((ModContainer)mod).FolderPath);
			defaultInterpolatedStringHandler.AppendLiteral("/native/");
			defaultInterpolatedStringHandler.AppendFormatted(prefix);
			defaultInterpolatedStringHandler.AppendFormatted(dllName);
			defaultInterpolatedStringHandler.AppendFormatted(suffix);
			string dllPath = defaultInterpolatedStringHandler.ToStringAndClear();
			return this.Load(dllPath, logger);
		}

		// Token: 0x06000271 RID: 625
		protected abstract bool Load(string dllPath, ILogger logger);
	}
}
