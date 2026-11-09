using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ImGuiNET;

namespace VSImGui.Debug
{
	// Token: 0x02000006 RID: 6
	internal static class TextEditor
	{
		// Token: 0x0600002F RID: 47 RVA: 0x00002F48 File Offset: 0x00001148
		[NullableContext(1)]
		public static void ListWithFilter(string title, string filterHolder, in IEnumerable<string> elements, ref int index, out string value, uint maxFilterLength = 256U, string hint = "supports wildcards")
		{
			ImGui.InputTextWithHint(MemoryExtensions.AsSpan("Filter##" + title), MemoryExtensions.AsSpan(hint), ref filterHolder, maxFilterLength);
			IEnumerable<string> enumerable;
			IEnumerable<int> enumerable2;
			int num;
			EditorsUtils.FilterElements(filterHolder, elements, out enumerable, out enumerable2, ref index, out num);
			ImGui.ListBox(MemoryExtensions.AsSpan(title), ref num, Enumerable.ToArray<string>(enumerable), Enumerable.Count<string>(enumerable));
			index = Enumerable.ElementAt<int>(enumerable2, num);
			value = Enumerable.ElementAt<string>(elements, index);
		}
	}
}
