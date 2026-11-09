using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ImGuiNET;

namespace VSImGui
{
	// Token: 0x02000019 RID: 25
	public static class TextEditor
	{
		// Token: 0x06000243 RID: 579 RVA: 0x0000C5C4 File Offset: 0x0000A7C4
		[NullableContext(1)]
		public static void ListWithFilter(string title, string filterHolder, in IEnumerable<string> elements, ref int index, out string value, uint maxFilterLength = 256U, string hint = "supports wildcards")
		{
			ImGui.InputTextWithHint(MemoryExtensions.AsSpan("Filter##" + title), MemoryExtensions.AsSpan(hint), ref filterHolder, maxFilterLength);
			IEnumerable<string> filtered;
			IEnumerable<int> indexes;
			int selection;
			EditorsUtils.FilterElements(filterHolder, elements, out filtered, out indexes, ref index, out selection);
			ImGui.ListBox(MemoryExtensions.AsSpan(title), ref selection, Enumerable.ToArray<string>(filtered), Enumerable.Count<string>(filtered));
			index = Enumerable.ElementAt<int>(indexes, selection);
			value = Enumerable.ElementAt<string>(elements, index);
		}
	}
}
