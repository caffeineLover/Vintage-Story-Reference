using System;
using System.Text;

namespace Vintagestory.ServerMods.NoObf;

internal static class StringBuilderCache
{
	[ThreadStatic]
	private static StringBuilder cachedInstance;

	public static StringBuilder Acquire()
	{
		StringBuilder result = cachedInstance;
		if (result == null)
		{
			return new StringBuilder(4096);
		}
		cachedInstance = null;
		result.Length = 0;
		return result;
	}

	public static string GetStringAndRelease(StringBuilder sb)
	{
		string result = sb.ToString();
		cachedInstance = sb;
		return result;
	}
}
