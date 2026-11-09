using System;
using System.Numerics;
using ImGuiNET;

namespace ImGuizmoNET
{
	// Token: 0x02000002 RID: 2
	public static class ImGuizmo
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static void AllowAxisFlip(bool value)
		{
			ImGuizmoNative.ImGuizmo_AllowAxisFlip((value > false) ? 1 : 0);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000205C File Offset: 0x0000025C
		public static void BeginFrame()
		{
			ImGuizmoNative.ImGuizmo_BeginFrame();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002064 File Offset: 0x00000264
		public unsafe static void DecomposeMatrixToComponents(ref float matrix, ref float translation, ref float rotation, ref float scale)
		{
			fixed (float* ptr = &matrix)
			{
				float* matrix2 = ptr;
				fixed (float* ptr2 = &translation)
				{
					float* translation2 = ptr2;
					fixed (float* ptr3 = &rotation)
					{
						float* rotation2 = ptr3;
						fixed (float* ptr4 = &scale)
						{
							float* scale2 = ptr4;
							ImGuizmoNative.ImGuizmo_DecomposeMatrixToComponents(matrix2, translation2, rotation2, scale2);
						}
					}
				}
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020A0 File Offset: 0x000002A0
		public unsafe static void DrawCubes(ref float view, ref float projection, ref float matrices, int matrixCount)
		{
			fixed (float* ptr = &view)
			{
				float* view2 = ptr;
				fixed (float* ptr2 = &projection)
				{
					float* projection2 = ptr2;
					fixed (float* ptr3 = &matrices)
					{
						float* matrices2 = ptr3;
						ImGuizmoNative.ImGuizmo_DrawCubes(view2, projection2, matrices2, matrixCount);
					}
				}
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020D0 File Offset: 0x000002D0
		public unsafe static void DrawGrid(ref float view, ref float projection, ref float matrix, float gridSize)
		{
			fixed (float* ptr = &view)
			{
				float* view2 = ptr;
				fixed (float* ptr2 = &projection)
				{
					float* projection2 = ptr2;
					fixed (float* ptr3 = &matrix)
					{
						float* matrix2 = ptr3;
						ImGuizmoNative.ImGuizmo_DrawGrid(view2, projection2, matrix2, gridSize);
					}
				}
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020FF File Offset: 0x000002FF
		public static void Enable(bool enable)
		{
			ImGuizmoNative.ImGuizmo_Enable((enable > false) ? 1 : 0);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000210B File Offset: 0x0000030B
		public static bool IsOver()
		{
			return ImGuizmoNative.ImGuizmo_IsOverNil() > 0;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002115 File Offset: 0x00000315
		public static bool IsOver(OPERATION op)
		{
			return ImGuizmoNative.ImGuizmo_IsOverOPERATION(op) > 0;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002120 File Offset: 0x00000320
		public static bool IsUsing()
		{
			return ImGuizmoNative.ImGuizmo_IsUsing() > 0;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000212C File Offset: 0x0000032C
		public unsafe static bool Manipulate(ref float view, ref float projection, OPERATION operation, MODE mode, ref float matrix)
		{
			float* deltaMatrix = null;
			float* snap = null;
			float* localBounds = null;
			float* boundsSnap = null;
			fixed (float* ptr = &view)
			{
				float* view2 = ptr;
				fixed (float* ptr2 = &projection)
				{
					float* projection2 = ptr2;
					fixed (float* ptr3 = &matrix)
					{
						float* matrix2 = ptr3;
						return ImGuizmoNative.ImGuizmo_Manipulate(view2, projection2, operation, mode, matrix2, deltaMatrix, snap, localBounds, boundsSnap) > 0;
					}
				}
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002170 File Offset: 0x00000370
		public unsafe static bool Manipulate(ref float view, ref float projection, OPERATION operation, MODE mode, ref float matrix, ref float deltaMatrix)
		{
			float* snap = null;
			float* localBounds = null;
			float* boundsSnap = null;
			fixed (float* ptr = &view)
			{
				float* view2 = ptr;
				fixed (float* ptr2 = &projection)
				{
					float* projection2 = ptr2;
					fixed (float* ptr3 = &matrix)
					{
						float* matrix2 = ptr3;
						fixed (float* ptr4 = &deltaMatrix)
						{
							float* deltaMatrix2 = ptr4;
							return ImGuizmoNative.ImGuizmo_Manipulate(view2, projection2, operation, mode, matrix2, deltaMatrix2, snap, localBounds, boundsSnap) > 0;
						}
					}
				}
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021B8 File Offset: 0x000003B8
		public unsafe static bool Manipulate(ref float view, ref float projection, OPERATION operation, MODE mode, ref float matrix, ref float deltaMatrix, ref float snap)
		{
			float* localBounds = null;
			float* boundsSnap = null;
			fixed (float* ptr = &view)
			{
				float* view2 = ptr;
				fixed (float* ptr2 = &projection)
				{
					float* projection2 = ptr2;
					fixed (float* ptr3 = &matrix)
					{
						float* matrix2 = ptr3;
						fixed (float* ptr4 = &deltaMatrix)
						{
							float* deltaMatrix2 = ptr4;
							fixed (float* ptr5 = &snap)
							{
								float* snap2 = ptr5;
								return ImGuizmoNative.ImGuizmo_Manipulate(view2, projection2, operation, mode, matrix2, deltaMatrix2, snap2, localBounds, boundsSnap) > 0;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002204 File Offset: 0x00000404
		public unsafe static bool Manipulate(ref float view, ref float projection, OPERATION operation, MODE mode, ref float matrix, ref float deltaMatrix, ref float snap, ref float localBounds)
		{
			float* boundsSnap = null;
			fixed (float* ptr = &view)
			{
				float* view2 = ptr;
				fixed (float* ptr2 = &projection)
				{
					float* projection2 = ptr2;
					fixed (float* ptr3 = &matrix)
					{
						float* matrix2 = ptr3;
						fixed (float* ptr4 = &deltaMatrix)
						{
							float* deltaMatrix2 = ptr4;
							fixed (float* ptr5 = &snap)
							{
								float* snap2 = ptr5;
								fixed (float* ptr6 = &localBounds)
								{
									float* localBounds2 = ptr6;
									return ImGuizmoNative.ImGuizmo_Manipulate(view2, projection2, operation, mode, matrix2, deltaMatrix2, snap2, localBounds2, boundsSnap) > 0;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002258 File Offset: 0x00000458
		public unsafe static bool Manipulate(ref float view, ref float projection, OPERATION operation, MODE mode, ref float matrix, ref float deltaMatrix, ref float snap, ref float localBounds, ref float boundsSnap)
		{
			fixed (float* ptr = &view)
			{
				float* view2 = ptr;
				fixed (float* ptr2 = &projection)
				{
					float* projection2 = ptr2;
					fixed (float* ptr3 = &matrix)
					{
						float* matrix2 = ptr3;
						fixed (float* ptr4 = &deltaMatrix)
						{
							float* deltaMatrix2 = ptr4;
							fixed (float* ptr5 = &snap)
							{
								float* snap2 = ptr5;
								fixed (float* ptr6 = &localBounds)
								{
									float* localBounds2 = ptr6;
									fixed (float* ptr7 = &boundsSnap)
									{
										float* boundsSnap2 = ptr7;
										return ImGuizmoNative.ImGuizmo_Manipulate(view2, projection2, operation, mode, matrix2, deltaMatrix2, snap2, localBounds2, boundsSnap2) > 0;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000022B0 File Offset: 0x000004B0
		public unsafe static void RecomposeMatrixFromComponents(ref float translation, ref float rotation, ref float scale, ref float matrix)
		{
			fixed (float* ptr = &translation)
			{
				float* translation2 = ptr;
				fixed (float* ptr2 = &rotation)
				{
					float* rotation2 = ptr2;
					fixed (float* ptr3 = &scale)
					{
						float* scale2 = ptr3;
						fixed (float* ptr4 = &matrix)
						{
							float* matrix2 = ptr4;
							ImGuizmoNative.ImGuizmo_RecomposeMatrixFromComponents(translation2, rotation2, scale2, matrix2);
						}
					}
				}
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000022EC File Offset: 0x000004EC
		public unsafe static void SetDrawlist()
		{
			ImDrawList* drawlist = null;
			ImGuizmoNative.ImGuizmo_SetDrawlist(drawlist);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002302 File Offset: 0x00000502
		public static void SetDrawlist(ImDrawListPtr drawlist)
		{
			ImGuizmoNative.ImGuizmo_SetDrawlist(drawlist.NativePtr);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002310 File Offset: 0x00000510
		public static void SetGizmoSizeClipSpace(float value)
		{
			ImGuizmoNative.ImGuizmo_SetGizmoSizeClipSpace(value);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002318 File Offset: 0x00000518
		public static void SetID(int id)
		{
			ImGuizmoNative.ImGuizmo_SetID(id);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002320 File Offset: 0x00000520
		public static void SetImGuiContext(IntPtr ctx)
		{
			ImGuizmoNative.ImGuizmo_SetImGuiContext(ctx);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002328 File Offset: 0x00000528
		public static void SetOrthographic(bool isOrthographic)
		{
			ImGuizmoNative.ImGuizmo_SetOrthographic((isOrthographic > false) ? 1 : 0);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002334 File Offset: 0x00000534
		public static void SetRect(float x, float y, float width, float height)
		{
			ImGuizmoNative.ImGuizmo_SetRect(x, y, width, height);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002340 File Offset: 0x00000540
		public unsafe static void ViewManipulate(ref float view, float length, Vector2 position, Vector2 size, uint backgroundColor)
		{
			fixed (float* ptr = &view)
			{
				ImGuizmoNative.ImGuizmo_ViewManipulate_Float(ptr, length, position, size, backgroundColor);
			}
		}
	}
}
