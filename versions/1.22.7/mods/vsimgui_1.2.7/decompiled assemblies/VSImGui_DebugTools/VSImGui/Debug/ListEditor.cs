using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ImGuiNET;
using Vintagestory.API.Util;

namespace VSImGui.Debug
{
	// Token: 0x0200000A RID: 10
	internal static class ListEditor
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00003324 File Offset: 0x00001524
		[NullableContext(1)]
		public static void Edit(string title, string[] list, ref int selected, Action<string, int> onRemove, Func<int, string> onAdd, bool appendToEnd = false)
		{
			if (selected >= list.Length)
			{
				selected = Math.Max(0, list.Length - 1);
			}
			if (ImGui.Button(MemoryExtensions.AsSpan("Add##" + title)))
			{
				if (appendToEnd)
				{
					list = Enumerable.ToArray<string>(ArrayExtensions.Append<string>(list, onAdd.Invoke(selected)));
				}
				else
				{
					int selected_not_ref = selected;
					IEnumerable<string> enumerable = Enumerable.Append<string>(Enumerable.Where<string>(list, (string _, int index) => index <= selected_not_ref), onAdd.Invoke(selected));
					IEnumerable<string> enumerable2 = Enumerable.Where<string>(list, (string _, int index) => index > selected_not_ref);
					list = Enumerable.ToArray<string>(Enumerable.Concat<string>(enumerable, enumerable2));
				}
			}
			ImGui.SameLine();
			bool flag = list.Length == 0;
			if (flag)
			{
				ImGui.BeginDisabled();
			}
			if (ImGui.Button(MemoryExtensions.AsSpan("Remove##" + title)))
			{
				onRemove.Invoke(list[selected], selected);
				list = ArrayExtensions.RemoveEntry<string>(list, selected);
			}
			if (flag)
			{
				ImGui.EndDisabled();
			}
			if (selected >= list.Length)
			{
				selected = Math.Max(0, list.Length - 1);
			}
			ImGui.ListBox(MemoryExtensions.AsSpan(title ?? ""), ref selected, list, list.Length);
		}
	}
}
