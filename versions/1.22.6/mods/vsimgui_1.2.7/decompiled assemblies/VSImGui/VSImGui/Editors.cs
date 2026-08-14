using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using ImGuiNET;

namespace VSImGui
{
	// Token: 0x0200000C RID: 12
	[NullableContext(1)]
	[Nullable(0)]
	public static class Editors
	{
		// Token: 0x0600017F RID: 383 RVA: 0x0000A50C File Offset: 0x0000870C
		public unsafe static Value4 Color(string name, Vector4 value, string hint = null)
		{
			if (hint == null)
			{
				ImGui.ColorEdit4(MemoryExtensions.AsSpan(name), ref value, 327680);
			}
			else
			{
				Vector2 spacing = *ImGui.GetStyle().ItemSpacing;
				spacing.X = 1f;
				ImGui.ColorEdit4(MemoryExtensions.AsSpan("##" + name), ref value, 327680);
				ImGui.PushStyleVar(14, spacing);
				ImGui.SameLine();
				Editors.DrawHint(hint);
				ImGui.SameLine();
				ImGui.Text(MemoryExtensions.AsSpan(name.Split("##", 2, 0)[0]));
				ImGui.PopStyleVar();
			}
			return value;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000A5AC File Offset: 0x000087AC
		public unsafe static Value2 Pair(string name, Value2 value, string hint = null)
		{
			Vector2 pair = value;
			if (hint == null)
			{
				ImGui.DragFloat2(MemoryExtensions.AsSpan(name), ref pair);
			}
			else
			{
				Vector2 spacing = *ImGui.GetStyle().ItemSpacing;
				spacing.X = 1f;
				ImGui.DragFloat2(MemoryExtensions.AsSpan("##" + name), ref pair);
				ImGui.PushStyleVar(14, spacing);
				ImGui.SameLine();
				Editors.DrawHint(hint);
				ImGui.SameLine();
				ImGui.Text(MemoryExtensions.AsSpan(name.Split("##", 2, 0)[0]));
				ImGui.PopStyleVar();
			}
			return pair;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000A648 File Offset: 0x00008848
		public unsafe static float Float(string name, float value, string hint = null)
		{
			if (hint == null)
			{
				ImGui.DragFloat(MemoryExtensions.AsSpan(name), ref value);
			}
			else
			{
				Vector2 spacing = *ImGui.GetStyle().ItemSpacing;
				spacing.X = 1f;
				ImGui.DragFloat(MemoryExtensions.AsSpan("##" + name), ref value);
				ImGui.PushStyleVar(14, spacing);
				ImGui.SameLine();
				Editors.DrawHint(hint);
				ImGui.SameLine();
				ImGui.Text(MemoryExtensions.AsSpan(name.Split("##", 2, 0)[0]));
				ImGui.PopStyleVar();
			}
			return value;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000A6D8 File Offset: 0x000088D8
		public unsafe static int Integer(string name, int value, string hint = null)
		{
			if (hint == null)
			{
				ImGui.DragInt(MemoryExtensions.AsSpan(name), ref value);
			}
			else
			{
				Vector2 spacing = *ImGui.GetStyle().ItemSpacing;
				spacing.X = 1f;
				ImGui.DragInt(MemoryExtensions.AsSpan("##" + name), ref value);
				ImGui.PushStyleVar(14, spacing);
				ImGui.SameLine();
				Editors.DrawHint(hint);
				ImGui.SameLine();
				ImGui.Text(MemoryExtensions.AsSpan(name.Split("##", 2, 0)[0]));
				ImGui.PopStyleVar();
			}
			return value;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000A768 File Offset: 0x00008968
		public unsafe static bool Boolean(string name, bool value, string hint = null)
		{
			if (hint == null)
			{
				ImGui.Checkbox(MemoryExtensions.AsSpan(name), ref value);
			}
			else
			{
				Vector2 spacing = *ImGui.GetStyle().ItemSpacing;
				spacing.X = 1f;
				ImGui.Checkbox(MemoryExtensions.AsSpan("##" + name), ref value);
				ImGui.PushStyleVar(14, spacing);
				ImGui.SameLine();
				Editors.DrawHint(hint);
				ImGui.SameLine();
				ImGui.Text(MemoryExtensions.AsSpan(name.Split("##", 2, 0)[0]));
				ImGui.PopStyleVar();
			}
			return value;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000A7F8 File Offset: 0x000089F8
		public unsafe static ImGuiDir Direction(string name, ImGuiDir value, string hint = null)
		{
			int dir = value;
			if (hint == null)
			{
				ImGui.Combo(MemoryExtensions.AsSpan(name), ref dir, Editors.Directions, Editors.Directions.Length);
			}
			else
			{
				Vector2 spacing = *ImGui.GetStyle().ItemSpacing;
				spacing.X = 1f;
				ImGui.Combo(MemoryExtensions.AsSpan("##" + name), ref dir, Editors.Directions, Editors.Directions.Length);
				ImGui.PushStyleVar(14, spacing);
				ImGui.SameLine();
				Editors.DrawHint(hint);
				ImGui.SameLine();
				ImGui.Text(MemoryExtensions.AsSpan(name.Split("##", 2, 0)[0]));
				ImGui.PopStyleVar();
			}
			return dir;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000A8A0 File Offset: 0x00008AA0
		public static ImGuiHoveredFlags HoveredFlag(string name, ImGuiHoveredFlags value)
		{
			int dir = value;
			if (ImGui.TreeNode(MemoryExtensions.AsSpan(name)))
			{
				ImGui.Indent();
				for (int index = 0; index < Editors.HoveredFlagsValues.Length; index++)
				{
					ImGui.CheckboxFlags(MemoryExtensions.AsSpan(Editors.HoveredFlags[index] + "##" + name), ref dir, Editors.HoveredFlagsValues[index]);
				}
				ImGui.Unindent();
				ImGui.TreePop();
			}
			return dir;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000A904 File Offset: 0x00008B04
		public static void DrawHint(string hint)
		{
			ImGui.SameLine();
			ImGui.TextDisabled(MemoryExtensions.AsSpan("(?)"));
			if (ImGui.BeginItemTooltip())
			{
				ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35f);
				ImGui.TextUnformatted(MemoryExtensions.AsSpan(hint));
				ImGui.PopTextWrapPos();
				ImGui.EndTooltip();
			}
		}

		// Token: 0x0400002F RID: 47
		public static readonly string[] Directions = new string[]
		{
			"NONE",
			"Left",
			"Right",
			"Up",
			"Down"
		};

		// Token: 0x04000030 RID: 48
		public static readonly string[] HoveredFlags = new string[]
		{
			"ChildWindows",
			"RootWindow",
			"AnyWindow",
			"NoPopupHierarchy",
			"DockHierarchy",
			"AllowWhenBlockedByPopup",
			"AllowWhenBlockedByActiveItem",
			"AllowWhenOverlappedByItem",
			"AllowWhenOverlappedByWindow",
			"AllowWhenDisabled",
			"NoNavOverride",
			"AllowWhenOverlapped",
			"RectOnly",
			"RootAndChildWindows",
			"ForTooltip",
			"Stationary",
			"DelayNone",
			"DelayShort",
			"DelayNormal",
			"NoSharedDelay"
		};

		// Token: 0x04000031 RID: 49
		public static readonly int[] HoveredFlagsValues = new int[]
		{
			1,
			2,
			4,
			8,
			16,
			32,
			128,
			256,
			512,
			1024,
			2048,
			768,
			928,
			3,
			2048,
			4096,
			8192,
			16384,
			32768,
			65536
		};
	}
}
