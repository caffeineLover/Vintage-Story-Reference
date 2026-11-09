using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace imnodesNET
{
	// Token: 0x0200000A RID: 10
	public static class imnodesNative
	{
		// Token: 0x06000070 RID: 112
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern void EmulateThreeButtonMouse_destroy(EmulateThreeButtonMouse* self);

		// Token: 0x06000071 RID: 113
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern EmulateThreeButtonMouse* EmulateThreeButtonMouse_EmulateThreeButtonMouse();

		// Token: 0x06000072 RID: 114
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_BeginInputAttribute(int id, ImNodesPinShape shape);

		// Token: 0x06000073 RID: 115
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_BeginNode(int id);

		// Token: 0x06000074 RID: 116
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_BeginNodeEditor();

		// Token: 0x06000075 RID: 117
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_BeginNodeTitleBar();

		// Token: 0x06000076 RID: 118
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_BeginOutputAttribute(int id, ImNodesPinShape shape);

		// Token: 0x06000077 RID: 119
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_BeginStaticAttribute(int id);

		// Token: 0x06000078 RID: 120
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_ClearLinkSelection_Nil();

		// Token: 0x06000079 RID: 121
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_ClearLinkSelection_Int(int link_id);

		// Token: 0x0600007A RID: 122
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_ClearNodeSelection_Nil();

		// Token: 0x0600007B RID: 123
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_ClearNodeSelection_Int(int node_id);

		// Token: 0x0600007C RID: 124
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern IntPtr imnodes_CreateContext();

		// Token: 0x0600007D RID: 125
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_DestroyContext(IntPtr ctx);

		// Token: 0x0600007E RID: 126
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern IntPtr imnodes_EditorContextCreate();

		// Token: 0x0600007F RID: 127
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_EditorContextFree(IntPtr noname1);

		// Token: 0x06000080 RID: 128
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern void imnodes_EditorContextGetPanning(Vector2* pOut);

		// Token: 0x06000081 RID: 129
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_EditorContextMoveToNode(int node_id);

		// Token: 0x06000082 RID: 130
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_EditorContextResetPanning(Vector2 pos);

		// Token: 0x06000083 RID: 131
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_EditorContextSet(IntPtr noname1);

		// Token: 0x06000084 RID: 132
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_EndInputAttribute();

		// Token: 0x06000085 RID: 133
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_EndNode();

		// Token: 0x06000086 RID: 134
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_EndNodeEditor();

		// Token: 0x06000087 RID: 135
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_EndNodeTitleBar();

		// Token: 0x06000088 RID: 136
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_EndOutputAttribute();

		// Token: 0x06000089 RID: 137
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_EndStaticAttribute();

		// Token: 0x0600008A RID: 138
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern IntPtr imnodes_GetCurrentContext();

		// Token: 0x0600008B RID: 139
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern ImNodesIO* imnodes_GetIO();

		// Token: 0x0600008C RID: 140
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern void imnodes_GetNodeDimensions(Vector2* pOut, int id);

		// Token: 0x0600008D RID: 141
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern void imnodes_GetNodeEditorSpacePos(Vector2* pOut, int node_id);

		// Token: 0x0600008E RID: 142
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern void imnodes_GetNodeGridSpacePos(Vector2* pOut, int node_id);

		// Token: 0x0600008F RID: 143
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern void imnodes_GetNodeScreenSpacePos(Vector2* pOut, int node_id);

		// Token: 0x06000090 RID: 144
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern void imnodes_GetSelectedLinks(int* link_ids);

		// Token: 0x06000091 RID: 145
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern void imnodes_GetSelectedNodes(int* node_ids);

		// Token: 0x06000092 RID: 146
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern ImNodesStyle* imnodes_GetStyle();

		// Token: 0x06000093 RID: 147
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern byte imnodes_IsAnyAttributeActive(int* attribute_id);

		// Token: 0x06000094 RID: 148
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern byte imnodes_IsAttributeActive();

		// Token: 0x06000095 RID: 149
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern byte imnodes_IsEditorHovered();

		// Token: 0x06000096 RID: 150
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern byte imnodes_IsLinkCreated_BoolPtr(int* started_at_attribute_id, int* ended_at_attribute_id, byte* created_from_snap);

		// Token: 0x06000097 RID: 151
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern byte imnodes_IsLinkCreated_IntPtr(int* started_at_node_id, int* started_at_attribute_id, int* ended_at_node_id, int* ended_at_attribute_id, byte* created_from_snap);

		// Token: 0x06000098 RID: 152
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern byte imnodes_IsLinkDestroyed(int* link_id);

		// Token: 0x06000099 RID: 153
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern byte imnodes_IsLinkDropped(int* started_at_attribute_id, byte including_detached_links);

		// Token: 0x0600009A RID: 154
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern byte imnodes_IsLinkHovered(int* link_id);

		// Token: 0x0600009B RID: 155
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern byte imnodes_IsLinkSelected(int link_id);

		// Token: 0x0600009C RID: 156
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern byte imnodes_IsLinkStarted(int* started_at_attribute_id);

		// Token: 0x0600009D RID: 157
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern byte imnodes_IsNodeHovered(int* node_id);

		// Token: 0x0600009E RID: 158
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern byte imnodes_IsNodeSelected(int node_id);

		// Token: 0x0600009F RID: 159
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern byte imnodes_IsPinHovered(int* attribute_id);

		// Token: 0x060000A0 RID: 160
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_Link(int id, int start_attribute_id, int end_attribute_id);

		// Token: 0x060000A1 RID: 161
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern void imnodes_LoadCurrentEditorStateFromIniFile(byte* file_name);

		// Token: 0x060000A2 RID: 162
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern void imnodes_LoadCurrentEditorStateFromIniString(byte* data, uint data_size);

		// Token: 0x060000A3 RID: 163
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern void imnodes_LoadEditorStateFromIniFile(IntPtr editor, byte* file_name);

		// Token: 0x060000A4 RID: 164
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern void imnodes_LoadEditorStateFromIniString(IntPtr editor, byte* data, uint data_size);

		// Token: 0x060000A5 RID: 165
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern int imnodes_NumSelectedLinks();

		// Token: 0x060000A6 RID: 166
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern int imnodes_NumSelectedNodes();

		// Token: 0x060000A7 RID: 167
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_PopAttributeFlag();

		// Token: 0x060000A8 RID: 168
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_PopColorStyle();

		// Token: 0x060000A9 RID: 169
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_PopStyleVar(int count);

		// Token: 0x060000AA RID: 170
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_PushAttributeFlag(ImNodesAttributeFlags flag);

		// Token: 0x060000AB RID: 171
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_PushColorStyle(ImNodesCol item, uint color);

		// Token: 0x060000AC RID: 172
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_PushStyleVar_Float(ImNodesStyleVar style_item, float value);

		// Token: 0x060000AD RID: 173
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_PushStyleVar_Vec2(ImNodesStyleVar style_item, Vector2 value);

		// Token: 0x060000AE RID: 174
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern void imnodes_SaveCurrentEditorStateToIniFile(byte* file_name);

		// Token: 0x060000AF RID: 175
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern byte* imnodes_SaveCurrentEditorStateToIniString(uint* data_size);

		// Token: 0x060000B0 RID: 176
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern void imnodes_SaveEditorStateToIniFile(IntPtr editor, byte* file_name);

		// Token: 0x060000B1 RID: 177
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern byte* imnodes_SaveEditorStateToIniString(IntPtr editor, uint* data_size);

		// Token: 0x060000B2 RID: 178
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_SelectLink(int link_id);

		// Token: 0x060000B3 RID: 179
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_SelectNode(int node_id);

		// Token: 0x060000B4 RID: 180
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_SetCurrentContext(IntPtr ctx);

		// Token: 0x060000B5 RID: 181
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_SetImGuiContext(IntPtr ctx);

		// Token: 0x060000B6 RID: 182
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_SetNodeDraggable(int node_id, byte draggable);

		// Token: 0x060000B7 RID: 183
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_SetNodeEditorSpacePos(int node_id, Vector2 editor_space_pos);

		// Token: 0x060000B8 RID: 184
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_SetNodeGridSpacePos(int node_id, Vector2 grid_pos);

		// Token: 0x060000B9 RID: 185
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_SetNodeScreenSpacePos(int node_id, Vector2 screen_space_pos);

		// Token: 0x060000BA RID: 186
		[DllImport("cimnodes", CallingConvention = 2)]
		public static extern void imnodes_SnapNodeToGrid(int node_id);

		// Token: 0x060000BB RID: 187
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern void imnodes_StyleColorsClassic(ImNodesStyle* dest);

		// Token: 0x060000BC RID: 188
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern void imnodes_StyleColorsDark(ImNodesStyle* dest);

		// Token: 0x060000BD RID: 189
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern void imnodes_StyleColorsLight(ImNodesStyle* dest);

		// Token: 0x060000BE RID: 190
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern void ImNodesIO_destroy(ImNodesIO* self);

		// Token: 0x060000BF RID: 191
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern ImNodesIO* ImNodesIO_ImNodesIO();

		// Token: 0x060000C0 RID: 192
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern void ImNodesStyle_destroy(ImNodesStyle* self);

		// Token: 0x060000C1 RID: 193
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern ImNodesStyle* ImNodesStyle_ImNodesStyle();

		// Token: 0x060000C2 RID: 194
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern void LinkDetachWithModifierClick_destroy(LinkDetachWithModifierClick* self);

		// Token: 0x060000C3 RID: 195
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern LinkDetachWithModifierClick* LinkDetachWithModifierClick_LinkDetachWithModifierClick();

		// Token: 0x060000C4 RID: 196
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern void MultipleSelectModifier_destroy(MultipleSelectModifier* self);

		// Token: 0x060000C5 RID: 197
		[DllImport("cimnodes", CallingConvention = 2)]
		public unsafe static extern MultipleSelectModifier* MultipleSelectModifier_MultipleSelectModifier();
	}
}
