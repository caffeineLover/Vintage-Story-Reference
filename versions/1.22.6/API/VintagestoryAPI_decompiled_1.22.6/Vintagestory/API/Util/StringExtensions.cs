using System;
using System.Diagnostics.CodeAnalysis;

namespace Vintagestory.API.Util;

public static class StringExtensions
{
	public static bool CaseInsensitiveContains(this string text, string value, StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
	{
		return text.IndexOf(value, stringComparison) >= 0;
	}

	[return: NotNullIfNotNull("str")]
	public static string? DeDuplicate(this string? str)
	{
		if (str != null)
		{
			return string.Intern(str);
		}
		return null;
	}
}
