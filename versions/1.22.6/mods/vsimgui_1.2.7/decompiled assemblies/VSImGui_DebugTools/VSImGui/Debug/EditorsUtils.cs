using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace VSImGui.Debug
{
	// Token: 0x0200000C RID: 12
	[NullableContext(1)]
	[Nullable(0)]
	internal static class EditorsUtils
	{
		// Token: 0x06000048 RID: 72 RVA: 0x000036CC File Offset: 0x000018CC
		public static void FilterElements(string filter, in IEnumerable<string> elements, out IEnumerable<string> filtered, out IEnumerable<int> indexes)
		{
			if (filter == "")
			{
				filtered = elements;
				indexes = Enumerable.Select<string, int>(elements, (string _, int index) => index);
				return;
			}
			string regexFilter = EditorsUtils.WildCardToRegular(filter);
			var enumerable = Enumerable.Where(Enumerable.Select(elements, (string value, int index) => new <>f__AnonymousType0<string, int>(value, index)), entry => EditorsUtils.Match(regexFilter, entry.value));
			filtered = Enumerable.ToArray<string>(Enumerable.Select(enumerable, entry => entry.value));
			indexes = Enumerable.ToArray<int>(Enumerable.Select(enumerable, entry => entry.index));
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000037B8 File Offset: 0x000019B8
		public static bool FilterElements(string filter, in IEnumerable<string> elements, out IEnumerable<string> filtered, out IEnumerable<int> indexes, ref int index, out int selection)
		{
			EditorsUtils.FilterElements(filter, elements, out filtered, out indexes);
			int selectedIndex = index;
			selectedIndex = Enumerable.FirstOrDefault<int>(Enumerable.Where<int>(indexes, (int value) => value == selectedIndex), -1);
			if (selectedIndex == -1)
			{
				index = 0;
				selection = 0;
				return true;
			}
			selection = selectedIndex;
			int num = Enumerable.ElementAt<int>(indexes, selectedIndex);
			if (index != num)
			{
				index = num;
				return true;
			}
			return false;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003833 File Offset: 0x00001A33
		public static string WildCardToRegular(string value)
		{
			return "^.*" + Regex.Escape(value).Replace("\\?", ".").Replace("\\*", ".*") + ".*$";
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003868 File Offset: 0x00001A68
		public static bool Match(string filter, string value)
		{
			return Regex.IsMatch(value, filter, 1);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003874 File Offset: 0x00001A74
		public static bool Match(string filter, params string[] values)
		{
			foreach (string value in values)
			{
				if (EditorsUtils.Match(filter, value))
				{
					return true;
				}
			}
			return false;
		}
	}
}
