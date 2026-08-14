using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using ImGuiNET;

namespace VSImGui.Debug
{
	// Token: 0x02000007 RID: 7
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ColorEditor
	{
		// Token: 0x06000030 RID: 48 RVA: 0x00002FB4 File Offset: 0x000011B4
		public static Vector4 HSVAEdit(string title, Vector4 value)
		{
			Vector4 result = value;
			ImGui.ColorEdit4(MemoryExtensions.AsSpan(title), ref result, 270532608);
			return result;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002FD8 File Offset: 0x000011D8
		public static uint HSVAEdit(string title, uint value)
		{
			Vector4 vector = ImGui.ColorConvertU32ToFloat4(value);
			ImGui.ColorEdit4(MemoryExtensions.AsSpan(title), ref vector, 270532608);
			return ImGui.ColorConvertFloat4ToU32(vector);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003008 File Offset: 0x00001208
		public static Vector4 RGBAEdit(string title, Vector4 value)
		{
			Vector4 result = value;
			ImGui.ColorEdit4(MemoryExtensions.AsSpan(title), ref result, 135266304);
			return result;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000302C File Offset: 0x0000122C
		public static uint RGBAEdit(string title, uint value)
		{
			Vector4 vector = ImGui.ColorConvertU32ToFloat4(value);
			ImGui.ColorEdit4(MemoryExtensions.AsSpan(title), ref vector, 135266304);
			return ImGui.ColorConvertFloat4ToU32(vector);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000305C File Offset: 0x0000125C
		public static Vector3 HSVEdit(string title, Vector3 value)
		{
			Vector3 result = value;
			ImGui.ColorEdit3(MemoryExtensions.AsSpan(title), ref result, 270532608);
			return result;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003080 File Offset: 0x00001280
		public static Vector3 RGBEdit(string title, Vector3 value)
		{
			Vector3 result = value;
			ImGui.ColorEdit3(MemoryExtensions.AsSpan(title), ref result, 135266304);
			return result;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000030A4 File Offset: 0x000012A4
		public static Vector4 HSVAPicker(string title, Vector4 value)
		{
			Vector4 result = value;
			ImGui.ColorPicker4(MemoryExtensions.AsSpan(title), ref result, 270532608);
			return result;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000030C8 File Offset: 0x000012C8
		public static uint HSVAPicker(string title, uint value)
		{
			Vector4 vector = ImGui.ColorConvertU32ToFloat4(value);
			ImGui.ColorPicker4(MemoryExtensions.AsSpan(title), ref vector, 270532608);
			return ImGui.ColorConvertFloat4ToU32(vector);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000030F8 File Offset: 0x000012F8
		public static Vector4 RGBAPicker(string title, Vector4 value)
		{
			Vector4 result = value;
			ImGui.ColorPicker4(MemoryExtensions.AsSpan(title), ref result, 135266304);
			return result;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000311C File Offset: 0x0000131C
		public static uint RGBAPicker(string title, uint value)
		{
			Vector4 vector = ImGui.ColorConvertU32ToFloat4(value);
			ImGui.ColorPicker4(MemoryExtensions.AsSpan(title), ref vector, 135266304);
			return ImGui.ColorConvertFloat4ToU32(vector);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000314C File Offset: 0x0000134C
		public static Vector3 HSVPicker(string title, Vector3 value)
		{
			Vector3 result = value;
			ImGui.ColorPicker3(MemoryExtensions.AsSpan(title), ref result, 270532608);
			return result;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003170 File Offset: 0x00001370
		public static Vector3 RGBPicker(string title, Vector3 value)
		{
			Vector3 result = value;
			ImGui.ColorPicker3(MemoryExtensions.AsSpan(title), ref result, 135266304);
			return result;
		}
	}
}
