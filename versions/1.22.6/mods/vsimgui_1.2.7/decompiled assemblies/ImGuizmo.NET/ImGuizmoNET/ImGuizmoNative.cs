using System;
using System.Numerics;
using System.Runtime.InteropServices;
using ImGuiNET;

namespace ImGuizmoNET
{
	// Token: 0x02000003 RID: 3
	public static class ImGuizmoNative
	{
		// Token: 0x06000018 RID: 24
		[DllImport("cimguizmo", CallingConvention = 2)]
		public static extern void ImGuizmo_AllowAxisFlip(byte value);

		// Token: 0x06000019 RID: 25
		[DllImport("cimguizmo", CallingConvention = 2)]
		public static extern void ImGuizmo_BeginFrame();

		// Token: 0x0600001A RID: 26
		[DllImport("cimguizmo", CallingConvention = 2)]
		public unsafe static extern void ImGuizmo_DecomposeMatrixToComponents(float* matrix, float* translation, float* rotation, float* scale);

		// Token: 0x0600001B RID: 27
		[DllImport("cimguizmo", CallingConvention = 2)]
		public unsafe static extern void ImGuizmo_DrawCubes(float* view, float* projection, float* matrices, int matrixCount);

		// Token: 0x0600001C RID: 28
		[DllImport("cimguizmo", CallingConvention = 2)]
		public unsafe static extern void ImGuizmo_DrawGrid(float* view, float* projection, float* matrix, float gridSize);

		// Token: 0x0600001D RID: 29
		[DllImport("cimguizmo", CallingConvention = 2)]
		public static extern void ImGuizmo_Enable(byte enable);

		// Token: 0x0600001E RID: 30
		[DllImport("cimguizmo", CallingConvention = 2)]
		public static extern byte ImGuizmo_IsOverNil();

		// Token: 0x0600001F RID: 31
		[DllImport("cimguizmo", CallingConvention = 2)]
		public static extern byte ImGuizmo_IsOverOPERATION(OPERATION op);

		// Token: 0x06000020 RID: 32
		[DllImport("cimguizmo", CallingConvention = 2)]
		public static extern byte ImGuizmo_IsUsing();

		// Token: 0x06000021 RID: 33
		[DllImport("cimguizmo", CallingConvention = 2)]
		public unsafe static extern byte ImGuizmo_Manipulate(float* view, float* projection, OPERATION operation, MODE mode, float* matrix, float* deltaMatrix, float* snap, float* localBounds, float* boundsSnap);

		// Token: 0x06000022 RID: 34
		[DllImport("cimguizmo", CallingConvention = 2)]
		public unsafe static extern void ImGuizmo_RecomposeMatrixFromComponents(float* translation, float* rotation, float* scale, float* matrix);

		// Token: 0x06000023 RID: 35
		[DllImport("cimguizmo", CallingConvention = 2)]
		public unsafe static extern void ImGuizmo_SetDrawlist(ImDrawList* drawlist);

		// Token: 0x06000024 RID: 36
		[DllImport("cimguizmo", CallingConvention = 2)]
		public static extern void ImGuizmo_SetGizmoSizeClipSpace(float value);

		// Token: 0x06000025 RID: 37
		[DllImport("cimguizmo", CallingConvention = 2)]
		public static extern void ImGuizmo_SetID(int id);

		// Token: 0x06000026 RID: 38
		[DllImport("cimguizmo", CallingConvention = 2)]
		public static extern void ImGuizmo_SetImGuiContext(IntPtr ctx);

		// Token: 0x06000027 RID: 39
		[DllImport("cimguizmo", CallingConvention = 2)]
		public static extern void ImGuizmo_SetOrthographic(byte isOrthographic);

		// Token: 0x06000028 RID: 40
		[DllImport("cimguizmo", CallingConvention = 2)]
		public static extern void ImGuizmo_SetRect(float x, float y, float width, float height);

		// Token: 0x06000029 RID: 41
		[DllImport("cimguizmo", CallingConvention = 2)]
		public unsafe static extern void ImGuizmo_ViewManipulate_Float(float* view, float length, Vector2 position, Vector2 size, uint backgroundColor);
	}
}
