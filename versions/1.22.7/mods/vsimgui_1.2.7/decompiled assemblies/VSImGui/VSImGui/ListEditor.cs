using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ImGuiNET;
using Vintagestory.API.Util;

namespace VSImGui
{
	// Token: 0x0200001D RID: 29
	[NullableContext(1)]
	[Nullable(0)]
	public static class ListEditor
	{
		// Token: 0x06000256 RID: 598 RVA: 0x0000CA88 File Offset: 0x0000AC88
		public static void Edit(string title, ref string[] list, ref int selected, [Nullable(new byte[]
		{
			2,
			1
		})] Func<int, string> onAdd = null, [Nullable(new byte[]
		{
			2,
			1
		})] Action<string, int> onRemove = null, bool appendToEnd = false)
		{
			if (selected >= list.Length)
			{
				selected = Math.Max(0, list.Length - 1);
			}
			if (ImGui.Button(MemoryExtensions.AsSpan("Add##" + title)))
			{
				if (appendToEnd)
				{
					list = Enumerable.ToArray<string>(ArrayExtensions.Append<string>(list, ((onAdd != null) ? onAdd.Invoke(selected) : null) ?? list[selected]));
				}
				else
				{
					int selected_not_ref = selected;
					IEnumerable<string> begin = Enumerable.Append<string>(Enumerable.Where<string>(list, (string _, int index) => index <= selected_not_ref), ((onAdd != null) ? onAdd.Invoke(selected) : null) ?? list[selected]);
					IEnumerable<string> end = Enumerable.Where<string>(list, (string _, int index) => index > selected_not_ref);
					list = Enumerable.ToArray<string>(Enumerable.Concat<string>(begin, end));
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
				if (onRemove != null)
				{
					onRemove.Invoke(list[selected], selected);
				}
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

		// Token: 0x06000257 RID: 599 RVA: 0x0000CBD0 File Offset: 0x0000ADD0
		public static void Edit(string title, string[] list, ref int selected, Action<string, int> onRemove, Func<int, string> onAdd)
		{
			if (selected >= list.Length)
			{
				selected = Math.Max(0, list.Length - 1);
			}
			if (ImGui.Button(MemoryExtensions.AsSpan("Add##" + title)))
			{
				list = Enumerable.ToArray<string>(ArrayExtensions.Append<string>(list, onAdd.Invoke(selected)));
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

		// Token: 0x06000258 RID: 600 RVA: 0x0000CC92 File Offset: 0x0000AE92
		[Obsolete("appendToEnd argument do nothing, use 'public static void Edit(string title, string[] list, ref int selected, Action<string, int> onRemove, System.Func<int, string> onAdd)' instead")]
		public static void Edit(string title, string[] list, ref int selected, Action<string, int> onRemove, Func<int, string> onAdd, bool appendToEnd = false)
		{
			ListEditor.Edit(title, list, ref selected, onRemove, onAdd);
		}
	}
}
