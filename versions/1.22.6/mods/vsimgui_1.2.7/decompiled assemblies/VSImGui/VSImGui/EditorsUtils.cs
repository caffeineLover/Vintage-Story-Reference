using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace VSImGui
{
	// Token: 0x0200001F RID: 31
	[NullableContext(1)]
	[Nullable(0)]
	public static class EditorsUtils
	{
		// Token: 0x06000261 RID: 609 RVA: 0x0000CF10 File Offset: 0x0000B110
		public static void FilterElements(string filter, in IEnumerable<string> elements, out IEnumerable<string> filtered, out IEnumerable<int> indexes)
		{
			if (filter == "")
			{
				filtered = elements;
				indexes = Enumerable.Select<string, int>(elements, (string _, int index) => index);
				return;
			}
			string regexFilter = StyleEditor.WildCardToRegular(filter);
			var filteredMapping = Enumerable.Where(Enumerable.Select(elements, (string value, int index) => new <>f__AnonymousType0<string, int>(value, index)), entry => StyleEditor.Match(regexFilter, entry.value));
			filtered = Enumerable.ToArray<string>(Enumerable.Select(filteredMapping, entry => entry.value));
			indexes = Enumerable.ToArray<int>(Enumerable.Select(filteredMapping, entry => entry.index));
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000CFFC File Offset: 0x0000B1FC
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
			int newIndex = Enumerable.ElementAt<int>(indexes, selectedIndex);
			if (index != newIndex)
			{
				index = newIndex;
				return true;
			}
			return false;
		}
	}
}
