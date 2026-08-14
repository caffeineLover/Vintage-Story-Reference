using System;
using System.Runtime.CompilerServices;
using ImGuiNET;

namespace VSImGui.Debug
{
	// Token: 0x0200000D RID: 13
	[NullableContext(1)]
	[Nullable(0)]
	internal static class Controls
	{
		// Token: 0x0600004D RID: 77 RVA: 0x000038A1 File Offset: 0x00001AA1
		public static void CopySupport(string value, string title)
		{
			if (ImGui.Button(MemoryExtensions.AsSpan(title)) || Controls.CopyCombination())
			{
				ImGui.SetClipboardText(MemoryExtensions.AsSpan(value));
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000038C2 File Offset: 0x00001AC2
		public static void PasteSupport(ref string value, string title)
		{
			if (ImGui.Button(MemoryExtensions.AsSpan(title)) || Controls.PasteCombination())
			{
				value = ImGui.GetClipboardText();
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000038DF File Offset: 0x00001ADF
		public static void CopyPasteSupport(ref string value, string copy, string paste)
		{
			Controls.CopySupport(value, copy);
			ImGui.SameLine();
			Controls.PasteSupport(ref value, paste);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000038F8 File Offset: 0x00001AF8
		private unsafe static bool CopyCombination()
		{
			ImGuiIOPtr io = ImGui.GetIO();
			return *io.KeyCtrl && *io.KeysDown[548];
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000392C File Offset: 0x00001B2C
		private unsafe static bool PasteCombination()
		{
			ImGuiIOPtr io = ImGui.GetIO();
			return *io.KeyCtrl && *io.KeysDown[567];
		}
	}
}
