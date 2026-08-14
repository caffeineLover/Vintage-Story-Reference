using System;
using System.IO;
using Vintagestory.API.Config;

namespace Vintagestory.Common;

public class CleanInstallCheck
{
	public static bool IsCleanInstall()
	{
		if (RuntimeEnv.IsDevEnvironment)
		{
			return true;
		}
		string basePath = AppDomain.CurrentDomain.BaseDirectory;
		global::_003C_003Ey__InlineArray6<string> buffer = default(global::_003C_003Ey__InlineArray6<string>);
		buffer[0] = basePath;
		buffer[1] = "assets";
		buffer[2] = "survival";
		buffer[3] = "itemtypes";
		buffer[4] = "bag";
		buffer[5] = "backpack.json";
		if (!File.Exists(Path.Combine(buffer)))
		{
			bool num = File.Exists(Path.Combine(basePath, "assets", "version-1.22.6.txt"));
			string[] strings = Directory.GetFiles(Path.Combine(basePath, "assets"), "version-*.txt", SearchOption.TopDirectoryOnly);
			if (num)
			{
				return strings.Length == 1;
			}
			return false;
		}
		return false;
	}
}
