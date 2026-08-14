using System;
using System.Numerics;
using System.Text;
using ImGuiNET;

namespace imnodesNET
{
	// Token: 0x02000004 RID: 4
	public static class imnodes
	{
		// Token: 0x0600000A RID: 10 RVA: 0x000020BC File Offset: 0x000002BC
		public static void BeginInputAttribute(int id)
		{
			ImNodesPinShape shape = ImNodesPinShape.CircleFilled;
			imnodesNative.imnodes_BeginInputAttribute(id, shape);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000020D2 File Offset: 0x000002D2
		public static void BeginInputAttribute(int id, ImNodesPinShape shape)
		{
			imnodesNative.imnodes_BeginInputAttribute(id, shape);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000020DB File Offset: 0x000002DB
		public static void BeginNode(int id)
		{
			imnodesNative.imnodes_BeginNode(id);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000020E3 File Offset: 0x000002E3
		public static void BeginNodeEditor()
		{
			imnodesNative.imnodes_BeginNodeEditor();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000020EA File Offset: 0x000002EA
		public static void BeginNodeTitleBar()
		{
			imnodesNative.imnodes_BeginNodeTitleBar();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000020F4 File Offset: 0x000002F4
		public static void BeginOutputAttribute(int id)
		{
			ImNodesPinShape shape = ImNodesPinShape.CircleFilled;
			imnodesNative.imnodes_BeginOutputAttribute(id, shape);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000210A File Offset: 0x0000030A
		public static void BeginOutputAttribute(int id, ImNodesPinShape shape)
		{
			imnodesNative.imnodes_BeginOutputAttribute(id, shape);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002113 File Offset: 0x00000313
		public static void BeginStaticAttribute(int id)
		{
			imnodesNative.imnodes_BeginStaticAttribute(id);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000211B File Offset: 0x0000031B
		public static void ClearLinkSelection()
		{
			imnodesNative.imnodes_ClearLinkSelection_Nil();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002122 File Offset: 0x00000322
		public static void ClearLinkSelection(int link_id)
		{
			imnodesNative.imnodes_ClearLinkSelection_Int(link_id);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000212A File Offset: 0x0000032A
		public static void ClearNodeSelection()
		{
			imnodesNative.imnodes_ClearNodeSelection_Nil();
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002131 File Offset: 0x00000331
		public static void ClearNodeSelection(int node_id)
		{
			imnodesNative.imnodes_ClearNodeSelection_Int(node_id);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002139 File Offset: 0x00000339
		public static IntPtr CreateContext()
		{
			return imnodesNative.imnodes_CreateContext();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002140 File Offset: 0x00000340
		public static void DestroyContext()
		{
			imnodesNative.imnodes_DestroyContext(IntPtr.Zero);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000214C File Offset: 0x0000034C
		public static void DestroyContext(IntPtr ctx)
		{
			imnodesNative.imnodes_DestroyContext(ctx);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002154 File Offset: 0x00000354
		public static IntPtr EditorContextCreate()
		{
			return imnodesNative.imnodes_EditorContextCreate();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000215B File Offset: 0x0000035B
		public static void EditorContextFree(IntPtr noname1)
		{
			imnodesNative.imnodes_EditorContextFree(noname1);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002164 File Offset: 0x00000364
		public unsafe static Vector2 EditorContextGetPanning()
		{
			Vector2 result;
			imnodesNative.imnodes_EditorContextGetPanning(&result);
			return result;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000217A File Offset: 0x0000037A
		public static void EditorContextMoveToNode(int node_id)
		{
			imnodesNative.imnodes_EditorContextMoveToNode(node_id);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002182 File Offset: 0x00000382
		public static void EditorContextResetPanning(Vector2 pos)
		{
			imnodesNative.imnodes_EditorContextResetPanning(pos);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000218A File Offset: 0x0000038A
		public static void EditorContextSet(IntPtr noname1)
		{
			imnodesNative.imnodes_EditorContextSet(noname1);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002192 File Offset: 0x00000392
		public static void EndInputAttribute()
		{
			imnodesNative.imnodes_EndInputAttribute();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002199 File Offset: 0x00000399
		public static void EndNode()
		{
			imnodesNative.imnodes_EndNode();
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000021A0 File Offset: 0x000003A0
		public static void EndNodeEditor()
		{
			imnodesNative.imnodes_EndNodeEditor();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000021A7 File Offset: 0x000003A7
		public static void EndNodeTitleBar()
		{
			imnodesNative.imnodes_EndNodeTitleBar();
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000021AE File Offset: 0x000003AE
		public static void EndOutputAttribute()
		{
			imnodesNative.imnodes_EndOutputAttribute();
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000021B5 File Offset: 0x000003B5
		public static void EndStaticAttribute()
		{
			imnodesNative.imnodes_EndStaticAttribute();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000021BC File Offset: 0x000003BC
		public static IntPtr GetCurrentContext()
		{
			return imnodesNative.imnodes_GetCurrentContext();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000021C3 File Offset: 0x000003C3
		public static ImNodesIOPtr GetIO()
		{
			return new ImNodesIOPtr(imnodesNative.imnodes_GetIO());
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000021D0 File Offset: 0x000003D0
		public unsafe static Vector2 GetNodeDimensions(int id)
		{
			Vector2 result;
			imnodesNative.imnodes_GetNodeDimensions(&result, id);
			return result;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000021E8 File Offset: 0x000003E8
		public unsafe static Vector2 GetNodeEditorSpacePos(int node_id)
		{
			Vector2 result;
			imnodesNative.imnodes_GetNodeEditorSpacePos(&result, node_id);
			return result;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002200 File Offset: 0x00000400
		public unsafe static Vector2 GetNodeGridSpacePos(int node_id)
		{
			Vector2 result;
			imnodesNative.imnodes_GetNodeGridSpacePos(&result, node_id);
			return result;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002218 File Offset: 0x00000418
		public unsafe static Vector2 GetNodeScreenSpacePos(int node_id)
		{
			Vector2 result;
			imnodesNative.imnodes_GetNodeScreenSpacePos(&result, node_id);
			return result;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002230 File Offset: 0x00000430
		public unsafe static void GetSelectedLinks(ref int link_ids)
		{
			fixed (int* ptr = &link_ids)
			{
				imnodesNative.imnodes_GetSelectedLinks(ptr);
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000224C File Offset: 0x0000044C
		public unsafe static void GetSelectedNodes(ref int node_ids)
		{
			fixed (int* ptr = &node_ids)
			{
				imnodesNative.imnodes_GetSelectedNodes(ptr);
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002265 File Offset: 0x00000465
		public static ImNodesStylePtr GetStyle()
		{
			return new ImNodesStylePtr(imnodesNative.imnodes_GetStyle());
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002274 File Offset: 0x00000474
		public unsafe static bool IsAnyAttributeActive()
		{
			int* attribute_id = null;
			return imnodesNative.imnodes_IsAnyAttributeActive(attribute_id) > 0;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002290 File Offset: 0x00000490
		public unsafe static bool IsAnyAttributeActive(ref int attribute_id)
		{
			fixed (int* ptr = &attribute_id)
			{
				return imnodesNative.imnodes_IsAnyAttributeActive(ptr) > 0;
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000022A9 File Offset: 0x000004A9
		public static bool IsAttributeActive()
		{
			return imnodesNative.imnodes_IsAttributeActive() > 0;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000022B3 File Offset: 0x000004B3
		public static bool IsEditorHovered()
		{
			return imnodesNative.imnodes_IsEditorHovered() > 0;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000022C0 File Offset: 0x000004C0
		public unsafe static bool IsLinkCreated(ref int started_at_attribute_id, ref int ended_at_attribute_id)
		{
			byte* created_from_snap = null;
			fixed (int* ptr = &started_at_attribute_id)
			{
				int* started_at_attribute_id2 = ptr;
				fixed (int* ptr2 = &ended_at_attribute_id)
				{
					int* ended_at_attribute_id2 = ptr2;
					return imnodesNative.imnodes_IsLinkCreated_BoolPtr(started_at_attribute_id2, ended_at_attribute_id2, created_from_snap) > 0;
				}
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000022E4 File Offset: 0x000004E4
		public unsafe static bool IsLinkCreated(ref int started_at_attribute_id, ref int ended_at_attribute_id, ref bool created_from_snap)
		{
			byte b = (created_from_snap > false) ? 1 : 0;
			byte* created_from_snap2 = &b;
			fixed (int* ptr = &started_at_attribute_id)
			{
				int* started_at_attribute_id2 = ptr;
				fixed (int* ptr2 = &ended_at_attribute_id)
				{
					int* ended_at_attribute_id2 = ptr2;
					int num = (int)imnodesNative.imnodes_IsLinkCreated_BoolPtr(started_at_attribute_id2, ended_at_attribute_id2, created_from_snap2);
					created_from_snap = (b > 0);
					return num != 0;
				}
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002318 File Offset: 0x00000518
		public unsafe static bool IsLinkCreated(ref int started_at_node_id, ref int started_at_attribute_id, ref int ended_at_node_id, ref int ended_at_attribute_id)
		{
			byte* created_from_snap = null;
			fixed (int* ptr = &started_at_node_id)
			{
				int* started_at_node_id2 = ptr;
				fixed (int* ptr2 = &started_at_attribute_id)
				{
					int* started_at_attribute_id2 = ptr2;
					fixed (int* ptr3 = &ended_at_node_id)
					{
						int* ended_at_node_id2 = ptr3;
						fixed (int* ptr4 = &ended_at_attribute_id)
						{
							int* ended_at_attribute_id2 = ptr4;
							return imnodesNative.imnodes_IsLinkCreated_IntPtr(started_at_node_id2, started_at_attribute_id2, ended_at_node_id2, ended_at_attribute_id2, created_from_snap) > 0;
						}
					}
				}
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002350 File Offset: 0x00000550
		public unsafe static bool IsLinkCreated(ref int started_at_node_id, ref int started_at_attribute_id, ref int ended_at_node_id, ref int ended_at_attribute_id, ref bool created_from_snap)
		{
			byte b = (created_from_snap > false) ? 1 : 0;
			byte* created_from_snap2 = &b;
			fixed (int* ptr = &started_at_node_id)
			{
				int* started_at_node_id2 = ptr;
				fixed (int* ptr2 = &started_at_attribute_id)
				{
					int* started_at_attribute_id2 = ptr2;
					fixed (int* ptr3 = &ended_at_node_id)
					{
						int* ended_at_node_id2 = ptr3;
						fixed (int* ptr4 = &ended_at_attribute_id)
						{
							int* ended_at_attribute_id2 = ptr4;
							int num = (int)imnodesNative.imnodes_IsLinkCreated_IntPtr(started_at_node_id2, started_at_attribute_id2, ended_at_node_id2, ended_at_attribute_id2, created_from_snap2);
							created_from_snap = (b > 0);
							return num != 0;
						}
					}
				}
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000239C File Offset: 0x0000059C
		public unsafe static bool IsLinkDestroyed(ref int link_id)
		{
			fixed (int* ptr = &link_id)
			{
				return imnodesNative.imnodes_IsLinkDestroyed(ptr) > 0;
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000023B8 File Offset: 0x000005B8
		public unsafe static bool IsLinkDropped()
		{
			int* started_at_attribute_id = null;
			byte including_detached_links = 1;
			return imnodesNative.imnodes_IsLinkDropped(started_at_attribute_id, including_detached_links) > 0;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000023D4 File Offset: 0x000005D4
		public unsafe static bool IsLinkDropped(ref int started_at_attribute_id)
		{
			byte including_detached_links = 1;
			fixed (int* ptr = &started_at_attribute_id)
			{
				return imnodesNative.imnodes_IsLinkDropped(ptr, including_detached_links) > 0;
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000023F0 File Offset: 0x000005F0
		public unsafe static bool IsLinkDropped(ref int started_at_attribute_id, bool including_detached_links)
		{
			byte including_detached_links2 = (including_detached_links > false) ? 1 : 0;
			fixed (int* ptr = &started_at_attribute_id)
			{
				return imnodesNative.imnodes_IsLinkDropped(ptr, including_detached_links2) > 0;
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002410 File Offset: 0x00000610
		public unsafe static bool IsLinkHovered(ref int link_id)
		{
			fixed (int* ptr = &link_id)
			{
				return imnodesNative.imnodes_IsLinkHovered(ptr) > 0;
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002429 File Offset: 0x00000629
		public static bool IsLinkSelected(int link_id)
		{
			return imnodesNative.imnodes_IsLinkSelected(link_id) > 0;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002434 File Offset: 0x00000634
		public unsafe static bool IsLinkStarted(ref int started_at_attribute_id)
		{
			fixed (int* ptr = &started_at_attribute_id)
			{
				return imnodesNative.imnodes_IsLinkStarted(ptr) > 0;
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002450 File Offset: 0x00000650
		public unsafe static bool IsNodeHovered(ref int node_id)
		{
			fixed (int* ptr = &node_id)
			{
				return imnodesNative.imnodes_IsNodeHovered(ptr) > 0;
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002469 File Offset: 0x00000669
		public static bool IsNodeSelected(int node_id)
		{
			return imnodesNative.imnodes_IsNodeSelected(node_id) > 0;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002474 File Offset: 0x00000674
		public unsafe static bool IsPinHovered(ref int attribute_id)
		{
			fixed (int* ptr = &attribute_id)
			{
				return imnodesNative.imnodes_IsPinHovered(ptr) > 0;
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000248D File Offset: 0x0000068D
		public static void Link(int id, int start_attribute_id, int end_attribute_id)
		{
			imnodesNative.imnodes_Link(id, start_attribute_id, end_attribute_id);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002498 File Offset: 0x00000698
		public unsafe static void LoadCurrentEditorStateFromIniFile(ReadOnlySpan<char> file_name)
		{
			int num = 0;
			byte* ptr;
			if (file_name != null)
			{
				num = Encoding.UTF8.GetByteCount(file_name);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(file_name, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			imnodesNative.imnodes_LoadCurrentEditorStateFromIniFile(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002504 File Offset: 0x00000704
		public unsafe static void LoadCurrentEditorStateFromIniString(ReadOnlySpan<char> data, uint data_size)
		{
			int num = 0;
			byte* ptr;
			if (data != null)
			{
				num = Encoding.UTF8.GetByteCount(data);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(data, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			imnodesNative.imnodes_LoadCurrentEditorStateFromIniString(ptr, data_size);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002570 File Offset: 0x00000770
		public unsafe static void LoadEditorStateFromIniFile(IntPtr editor, ReadOnlySpan<char> file_name)
		{
			int num = 0;
			byte* ptr;
			if (file_name != null)
			{
				num = Encoding.UTF8.GetByteCount(file_name);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(file_name, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			imnodesNative.imnodes_LoadEditorStateFromIniFile(editor, ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000025E0 File Offset: 0x000007E0
		public unsafe static void LoadEditorStateFromIniString(IntPtr editor, ReadOnlySpan<char> data, uint data_size)
		{
			int num = 0;
			byte* ptr;
			if (data != null)
			{
				num = Encoding.UTF8.GetByteCount(data);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(data, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			imnodesNative.imnodes_LoadEditorStateFromIniString(editor, ptr, data_size);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000264E File Offset: 0x0000084E
		public static int NumSelectedLinks()
		{
			return imnodesNative.imnodes_NumSelectedLinks();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002655 File Offset: 0x00000855
		public static int NumSelectedNodes()
		{
			return imnodesNative.imnodes_NumSelectedNodes();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0000265C File Offset: 0x0000085C
		public static void PopAttributeFlag()
		{
			imnodesNative.imnodes_PopAttributeFlag();
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002663 File Offset: 0x00000863
		public static void PopColorStyle()
		{
			imnodesNative.imnodes_PopColorStyle();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0000266A File Offset: 0x0000086A
		public static void PopStyleVar()
		{
			imnodesNative.imnodes_PopStyleVar(1);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002672 File Offset: 0x00000872
		public static void PopStyleVar(int count)
		{
			imnodesNative.imnodes_PopStyleVar(count);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000267A File Offset: 0x0000087A
		public static void PushAttributeFlag(ImNodesAttributeFlags flag)
		{
			imnodesNative.imnodes_PushAttributeFlag(flag);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002682 File Offset: 0x00000882
		public static void PushColorStyle(ImNodesCol item, uint color)
		{
			imnodesNative.imnodes_PushColorStyle(item, color);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0000268B File Offset: 0x0000088B
		public static void PushStyleVar(ImNodesStyleVar style_item, float value)
		{
			imnodesNative.imnodes_PushStyleVar_Float(style_item, value);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002694 File Offset: 0x00000894
		public static void PushStyleVar(ImNodesStyleVar style_item, Vector2 value)
		{
			imnodesNative.imnodes_PushStyleVar_Vec2(style_item, value);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000026A0 File Offset: 0x000008A0
		public unsafe static void SaveCurrentEditorStateToIniFile(ReadOnlySpan<char> file_name)
		{
			int num = 0;
			byte* ptr;
			if (file_name != null)
			{
				num = Encoding.UTF8.GetByteCount(file_name);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(file_name, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			imnodesNative.imnodes_SaveCurrentEditorStateToIniFile(ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000270C File Offset: 0x0000090C
		public unsafe static string SaveCurrentEditorStateToIniString()
		{
			uint* data_size = null;
			return Util.StringFromPtr(imnodesNative.imnodes_SaveCurrentEditorStateToIniString(data_size));
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002728 File Offset: 0x00000928
		public unsafe static string SaveCurrentEditorStateToIniString(ref uint data_size)
		{
			fixed (uint* ptr = &data_size)
			{
				return Util.StringFromPtr(imnodesNative.imnodes_SaveCurrentEditorStateToIniString(ptr));
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002744 File Offset: 0x00000944
		public unsafe static void SaveEditorStateToIniFile(IntPtr editor, ReadOnlySpan<char> file_name)
		{
			int num = 0;
			byte* ptr;
			if (file_name != null)
			{
				num = Encoding.UTF8.GetByteCount(file_name);
				if (num > 2048)
				{
					ptr = Util.Allocate(num + 1);
				}
				else
				{
					ptr = stackalloc byte[(UIntPtr)(num + 1)];
				}
				int utf = Util.GetUtf8(file_name, ptr, num);
				ptr[utf] = 0;
			}
			else
			{
				ptr = null;
			}
			imnodesNative.imnodes_SaveEditorStateToIniFile(editor, ptr);
			if (num > 2048)
			{
				Util.Free(ptr);
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000027B4 File Offset: 0x000009B4
		public unsafe static string SaveEditorStateToIniString(IntPtr editor)
		{
			uint* data_size = null;
			return Util.StringFromPtr(imnodesNative.imnodes_SaveEditorStateToIniString(editor, data_size));
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000027D0 File Offset: 0x000009D0
		public unsafe static string SaveEditorStateToIniString(IntPtr editor, ref uint data_size)
		{
			fixed (uint* ptr = &data_size)
			{
				uint* data_size2 = ptr;
				return Util.StringFromPtr(imnodesNative.imnodes_SaveEditorStateToIniString(editor, data_size2));
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000027EE File Offset: 0x000009EE
		public static void SelectLink(int link_id)
		{
			imnodesNative.imnodes_SelectLink(link_id);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000027F6 File Offset: 0x000009F6
		public static void SelectNode(int node_id)
		{
			imnodesNative.imnodes_SelectNode(node_id);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000027FE File Offset: 0x000009FE
		public static void SetCurrentContext(IntPtr ctx)
		{
			imnodesNative.imnodes_SetCurrentContext(ctx);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002806 File Offset: 0x00000A06
		public static void SetImGuiContext(IntPtr ctx)
		{
			imnodesNative.imnodes_SetImGuiContext(ctx);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002810 File Offset: 0x00000A10
		public static void SetNodeDraggable(int node_id, bool draggable)
		{
			byte draggable2 = (draggable > false) ? 1 : 0;
			imnodesNative.imnodes_SetNodeDraggable(node_id, draggable2);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000282A File Offset: 0x00000A2A
		public static void SetNodeEditorSpacePos(int node_id, Vector2 editor_space_pos)
		{
			imnodesNative.imnodes_SetNodeEditorSpacePos(node_id, editor_space_pos);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002833 File Offset: 0x00000A33
		public static void SetNodeGridSpacePos(int node_id, Vector2 grid_pos)
		{
			imnodesNative.imnodes_SetNodeGridSpacePos(node_id, grid_pos);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000283C File Offset: 0x00000A3C
		public static void SetNodeScreenSpacePos(int node_id, Vector2 screen_space_pos)
		{
			imnodesNative.imnodes_SetNodeScreenSpacePos(node_id, screen_space_pos);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002845 File Offset: 0x00000A45
		public static void SnapNodeToGrid(int node_id)
		{
			imnodesNative.imnodes_SnapNodeToGrid(node_id);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002850 File Offset: 0x00000A50
		public unsafe static void StyleColorsClassic()
		{
			ImNodesStyle* dest = null;
			imnodesNative.imnodes_StyleColorsClassic(dest);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002866 File Offset: 0x00000A66
		public static void StyleColorsClassic(ImNodesStylePtr dest)
		{
			imnodesNative.imnodes_StyleColorsClassic(dest.NativePtr);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002874 File Offset: 0x00000A74
		public unsafe static void StyleColorsDark()
		{
			ImNodesStyle* dest = null;
			imnodesNative.imnodes_StyleColorsDark(dest);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0000288A File Offset: 0x00000A8A
		public static void StyleColorsDark(ImNodesStylePtr dest)
		{
			imnodesNative.imnodes_StyleColorsDark(dest.NativePtr);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002898 File Offset: 0x00000A98
		public unsafe static void StyleColorsLight()
		{
			ImNodesStyle* dest = null;
			imnodesNative.imnodes_StyleColorsLight(dest);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000028AE File Offset: 0x00000AAE
		public static void StyleColorsLight(ImNodesStylePtr dest)
		{
			imnodesNative.imnodes_StyleColorsLight(dest.NativePtr);
		}
	}
}
