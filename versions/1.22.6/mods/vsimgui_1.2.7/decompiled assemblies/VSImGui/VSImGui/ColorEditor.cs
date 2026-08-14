using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using ImGuiNET;

namespace VSImGui
{
	// Token: 0x0200001A RID: 26
	[NullableContext(1)]
	[Nullable(0)]
	public static class ColorEditor
	{
		// Token: 0x06000244 RID: 580 RVA: 0x0000C630 File Offset: 0x0000A830
		public static Value4 ColorEdit(string title, Value4 value, ImGuiColorEditFlags flags)
		{
			Vector4 vector = value;
			ImGui.ColorEdit4(MemoryExtensions.AsSpan(title), ref vector, flags);
			return vector;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000C65C File Offset: 0x0000A85C
		public static Value4 HSVAEdit(string title, Value4 value)
		{
			Vector4 vector = value;
			ImGui.ColorEdit4(MemoryExtensions.AsSpan(title), ref vector, 270532608);
			return vector;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000C68C File Offset: 0x0000A88C
		public static uint HSVAEdit(string title, uint value)
		{
			Vector4 vector = ImGui.ColorConvertU32ToFloat4(value);
			ImGui.ColorEdit4(MemoryExtensions.AsSpan(title), ref vector, 136314880);
			return ImGui.ColorConvertFloat4ToU32(vector);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000C6BC File Offset: 0x0000A8BC
		public static Value4 RGBAEdit(string title, Value4 value)
		{
			Vector4 vector = value;
			ImGui.ColorEdit4(MemoryExtensions.AsSpan(title), ref vector, 135266304);
			return vector;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000C6EC File Offset: 0x0000A8EC
		public static uint RGBAEdit(string title, uint value)
		{
			Vector4 vector = ImGui.ColorConvertU32ToFloat4(value);
			ImGui.ColorEdit4(MemoryExtensions.AsSpan(title), ref vector, 135266304);
			return ImGui.ColorConvertFloat4ToU32(vector);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000C71C File Offset: 0x0000A91C
		public static Value3 HSVEdit(string title, Value3 value)
		{
			Vector3 vector = value;
			ImGui.ColorEdit3(MemoryExtensions.AsSpan(title), ref vector, 270532608);
			return vector;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000C74C File Offset: 0x0000A94C
		public static Value3 RGBEdit(string title, Value3 value)
		{
			Vector3 vector = value;
			ImGui.ColorEdit3(MemoryExtensions.AsSpan(title), ref vector, 135266304);
			return vector;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000C77C File Offset: 0x0000A97C
		public static Value4 ColorPicker(string title, Value4 value, ImGuiColorEditFlags flags)
		{
			Vector4 vector = value;
			ImGui.ColorPicker4(MemoryExtensions.AsSpan(title), ref vector, flags);
			return vector;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000C7A8 File Offset: 0x0000A9A8
		public static Value4 HSVAPicker(string title, Value4 value)
		{
			Vector4 vector = value;
			ImGui.ColorPicker4(MemoryExtensions.AsSpan(title), ref vector, 270532608);
			return vector;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000C7D8 File Offset: 0x0000A9D8
		public static uint HSVAPicker(string title, uint value)
		{
			Vector4 vector = ImGui.ColorConvertU32ToFloat4(value);
			ImGui.ColorPicker4(MemoryExtensions.AsSpan(title), ref vector, 136314880);
			return ImGui.ColorConvertFloat4ToU32(vector);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000C808 File Offset: 0x0000AA08
		public static Value4 RGBAPicker(string title, Value4 value)
		{
			Vector4 vector = value;
			ImGui.ColorPicker4(MemoryExtensions.AsSpan(title), ref vector, 135266304);
			return vector;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000C838 File Offset: 0x0000AA38
		public static uint RGBAPicker(string title, uint value)
		{
			Vector4 vector = ImGui.ColorConvertU32ToFloat4(value);
			ImGui.ColorPicker4(MemoryExtensions.AsSpan(title), ref vector, 135266304);
			return ImGui.ColorConvertFloat4ToU32(vector);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000C868 File Offset: 0x0000AA68
		public static Value3 HSVPicker(string title, Value3 value)
		{
			Vector3 vector = value;
			ImGui.ColorPicker3(MemoryExtensions.AsSpan(title), ref vector, 270532608);
			return vector;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000C898 File Offset: 0x0000AA98
		public static Value3 RGBPicker(string title, Value3 value)
		{
			Vector3 vector = value;
			ImGui.ColorPicker3(MemoryExtensions.AsSpan(title), ref vector, 135266304);
			return vector;
		}
	}
}
