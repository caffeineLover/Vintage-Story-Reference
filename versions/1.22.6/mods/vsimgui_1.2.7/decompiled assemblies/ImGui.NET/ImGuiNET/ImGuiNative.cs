using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace ImGuiNET
{
	// Token: 0x02000048 RID: 72
	public static class ImGuiNative
	{
		// Token: 0x0600052D RID: 1325
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiPayload* igAcceptDragDropPayload(byte* type, ImGuiDragDropFlags flags);

		// Token: 0x0600052E RID: 1326
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igAlignTextToFramePadding();

		// Token: 0x0600052F RID: 1327
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igArrowButton(byte* str_id, ImGuiDir dir);

		// Token: 0x06000530 RID: 1328
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igBegin(byte* name, byte* p_open, ImGuiWindowFlags flags);

		// Token: 0x06000531 RID: 1329
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igBeginChild_Str(byte* str_id, Vector2 size, byte border, ImGuiWindowFlags flags);

		// Token: 0x06000532 RID: 1330
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igBeginChild_ID(uint id, Vector2 size, byte border, ImGuiWindowFlags flags);

		// Token: 0x06000533 RID: 1331
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igBeginChildFrame(uint id, Vector2 size, ImGuiWindowFlags flags);

		// Token: 0x06000534 RID: 1332
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igBeginCombo(byte* label, byte* preview_value, ImGuiComboFlags flags);

		// Token: 0x06000535 RID: 1333
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igBeginDisabled(byte disabled);

		// Token: 0x06000536 RID: 1334
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igBeginDragDropSource(ImGuiDragDropFlags flags);

		// Token: 0x06000537 RID: 1335
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igBeginDragDropTarget();

		// Token: 0x06000538 RID: 1336
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igBeginGroup();

		// Token: 0x06000539 RID: 1337
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igBeginItemTooltip();

		// Token: 0x0600053A RID: 1338
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igBeginListBox(byte* label, Vector2 size);

		// Token: 0x0600053B RID: 1339
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igBeginMainMenuBar();

		// Token: 0x0600053C RID: 1340
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igBeginMenu(byte* label, byte enabled);

		// Token: 0x0600053D RID: 1341
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igBeginMenuBar();

		// Token: 0x0600053E RID: 1342
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igBeginPopup(byte* str_id, ImGuiWindowFlags flags);

		// Token: 0x0600053F RID: 1343
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igBeginPopupContextItem(byte* str_id, ImGuiPopupFlags popup_flags);

		// Token: 0x06000540 RID: 1344
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igBeginPopupContextVoid(byte* str_id, ImGuiPopupFlags popup_flags);

		// Token: 0x06000541 RID: 1345
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igBeginPopupContextWindow(byte* str_id, ImGuiPopupFlags popup_flags);

		// Token: 0x06000542 RID: 1346
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igBeginPopupModal(byte* name, byte* p_open, ImGuiWindowFlags flags);

		// Token: 0x06000543 RID: 1347
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igBeginTabBar(byte* str_id, ImGuiTabBarFlags flags);

		// Token: 0x06000544 RID: 1348
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igBeginTabItem(byte* label, byte* p_open, ImGuiTabItemFlags flags);

		// Token: 0x06000545 RID: 1349
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igBeginTable(byte* str_id, int column, ImGuiTableFlags flags, Vector2 outer_size, float inner_width);

		// Token: 0x06000546 RID: 1350
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igBeginTooltip();

		// Token: 0x06000547 RID: 1351
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igBullet();

		// Token: 0x06000548 RID: 1352
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igBulletText(byte* fmt);

		// Token: 0x06000549 RID: 1353
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igButton(byte* label, Vector2 size);

		// Token: 0x0600054A RID: 1354
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern float igCalcItemWidth();

		// Token: 0x0600054B RID: 1355
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igCalcTextSize(Vector2* pOut, byte* text, byte* text_end, byte hide_text_after_double_hash, float wrap_width);

		// Token: 0x0600054C RID: 1356
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igCheckbox(byte* label, byte* v);

		// Token: 0x0600054D RID: 1357
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igCheckboxFlags_IntPtr(byte* label, int* flags, int flags_value);

		// Token: 0x0600054E RID: 1358
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igCheckboxFlags_UintPtr(byte* label, uint* flags, uint flags_value);

		// Token: 0x0600054F RID: 1359
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igCloseCurrentPopup();

		// Token: 0x06000550 RID: 1360
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igCollapsingHeader_TreeNodeFlags(byte* label, ImGuiTreeNodeFlags flags);

		// Token: 0x06000551 RID: 1361
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igCollapsingHeader_BoolPtr(byte* label, byte* p_visible, ImGuiTreeNodeFlags flags);

		// Token: 0x06000552 RID: 1362
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igColorButton(byte* desc_id, Vector4 col, ImGuiColorEditFlags flags, Vector2 size);

		// Token: 0x06000553 RID: 1363
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern uint igColorConvertFloat4ToU32(Vector4 @in);

		// Token: 0x06000554 RID: 1364
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igColorConvertHSVtoRGB(float h, float s, float v, float* out_r, float* out_g, float* out_b);

		// Token: 0x06000555 RID: 1365
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igColorConvertRGBtoHSV(float r, float g, float b, float* out_h, float* out_s, float* out_v);

		// Token: 0x06000556 RID: 1366
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igColorConvertU32ToFloat4(Vector4* pOut, uint @in);

		// Token: 0x06000557 RID: 1367
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igColorEdit3(byte* label, Vector3* col, ImGuiColorEditFlags flags);

		// Token: 0x06000558 RID: 1368
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igColorEdit4(byte* label, Vector4* col, ImGuiColorEditFlags flags);

		// Token: 0x06000559 RID: 1369
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igColorPicker3(byte* label, Vector3* col, ImGuiColorEditFlags flags);

		// Token: 0x0600055A RID: 1370
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igColorPicker4(byte* label, Vector4* col, ImGuiColorEditFlags flags, float* ref_col);

		// Token: 0x0600055B RID: 1371
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igColumns(int count, byte* id, byte border);

		// Token: 0x0600055C RID: 1372
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igCombo_Str_arr(byte* label, int* current_item, byte** items, int items_count, int popup_max_height_in_items);

		// Token: 0x0600055D RID: 1373
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igCombo_Str(byte* label, int* current_item, byte* items_separated_by_zeros, int popup_max_height_in_items);

		// Token: 0x0600055E RID: 1374
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern IntPtr igCreateContext(ImFontAtlas* shared_font_atlas);

		// Token: 0x0600055F RID: 1375
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igDebugCheckVersionAndDataLayout(byte* version_str, uint sz_io, uint sz_style, uint sz_vec2, uint sz_vec4, uint sz_drawvert, uint sz_drawidx);

		// Token: 0x06000560 RID: 1376
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igDebugTextEncoding(byte* text);

		// Token: 0x06000561 RID: 1377
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igDestroyContext(IntPtr ctx);

		// Token: 0x06000562 RID: 1378
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igDestroyPlatformWindows();

		// Token: 0x06000563 RID: 1379
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern uint igDockSpace(uint id, Vector2 size, ImGuiDockNodeFlags flags, ImGuiWindowClass* window_class);

		// Token: 0x06000564 RID: 1380
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern uint igDockSpaceOverViewport(ImGuiViewport* viewport, ImGuiDockNodeFlags flags, ImGuiWindowClass* window_class);

		// Token: 0x06000565 RID: 1381
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igDragFloat(byte* label, float* v, float v_speed, float v_min, float v_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x06000566 RID: 1382
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igDragFloat2(byte* label, Vector2* v, float v_speed, float v_min, float v_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x06000567 RID: 1383
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igDragFloat3(byte* label, Vector3* v, float v_speed, float v_min, float v_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x06000568 RID: 1384
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igDragFloat4(byte* label, Vector4* v, float v_speed, float v_min, float v_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x06000569 RID: 1385
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igDragFloatRange2(byte* label, float* v_current_min, float* v_current_max, float v_speed, float v_min, float v_max, byte* format, byte* format_max, ImGuiSliderFlags flags);

		// Token: 0x0600056A RID: 1386
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igDragInt(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x0600056B RID: 1387
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igDragInt2(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x0600056C RID: 1388
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igDragInt3(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x0600056D RID: 1389
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igDragInt4(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x0600056E RID: 1390
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igDragIntRange2(byte* label, int* v_current_min, int* v_current_max, float v_speed, int v_min, int v_max, byte* format, byte* format_max, ImGuiSliderFlags flags);

		// Token: 0x0600056F RID: 1391
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igDragScalar(byte* label, ImGuiDataType data_type, void* p_data, float v_speed, void* p_min, void* p_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x06000570 RID: 1392
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igDragScalarN(byte* label, ImGuiDataType data_type, void* p_data, int components, float v_speed, void* p_min, void* p_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x06000571 RID: 1393
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igDummy(Vector2 size);

		// Token: 0x06000572 RID: 1394
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igEnd();

		// Token: 0x06000573 RID: 1395
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igEndChild();

		// Token: 0x06000574 RID: 1396
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igEndChildFrame();

		// Token: 0x06000575 RID: 1397
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igEndCombo();

		// Token: 0x06000576 RID: 1398
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igEndDisabled();

		// Token: 0x06000577 RID: 1399
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igEndDragDropSource();

		// Token: 0x06000578 RID: 1400
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igEndDragDropTarget();

		// Token: 0x06000579 RID: 1401
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igEndFrame();

		// Token: 0x0600057A RID: 1402
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igEndGroup();

		// Token: 0x0600057B RID: 1403
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igEndListBox();

		// Token: 0x0600057C RID: 1404
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igEndMainMenuBar();

		// Token: 0x0600057D RID: 1405
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igEndMenu();

		// Token: 0x0600057E RID: 1406
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igEndMenuBar();

		// Token: 0x0600057F RID: 1407
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igEndPopup();

		// Token: 0x06000580 RID: 1408
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igEndTabBar();

		// Token: 0x06000581 RID: 1409
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igEndTabItem();

		// Token: 0x06000582 RID: 1410
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igEndTable();

		// Token: 0x06000583 RID: 1411
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igEndTooltip();

		// Token: 0x06000584 RID: 1412
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiViewport* igFindViewportByID(uint id);

		// Token: 0x06000585 RID: 1413
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiViewport* igFindViewportByPlatformHandle(void* platform_handle);

		// Token: 0x06000586 RID: 1414
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igGetAllocatorFunctions(IntPtr* p_alloc_func, IntPtr* p_free_func, void** p_user_data);

		// Token: 0x06000587 RID: 1415
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImDrawList* igGetBackgroundDrawList_Nil();

		// Token: 0x06000588 RID: 1416
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImDrawList* igGetBackgroundDrawList_ViewportPtr(ImGuiViewport* viewport);

		// Token: 0x06000589 RID: 1417
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte* igGetClipboardText();

		// Token: 0x0600058A RID: 1418
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern uint igGetColorU32_Col(ImGuiCol idx, float alpha_mul);

		// Token: 0x0600058B RID: 1419
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern uint igGetColorU32_Vec4(Vector4 col);

		// Token: 0x0600058C RID: 1420
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern uint igGetColorU32_U32(uint col);

		// Token: 0x0600058D RID: 1421
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern int igGetColumnIndex();

		// Token: 0x0600058E RID: 1422
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern float igGetColumnOffset(int column_index);

		// Token: 0x0600058F RID: 1423
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern int igGetColumnsCount();

		// Token: 0x06000590 RID: 1424
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern float igGetColumnWidth(int column_index);

		// Token: 0x06000591 RID: 1425
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igGetContentRegionAvail(Vector2* pOut);

		// Token: 0x06000592 RID: 1426
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igGetContentRegionMax(Vector2* pOut);

		// Token: 0x06000593 RID: 1427
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern IntPtr igGetCurrentContext();

		// Token: 0x06000594 RID: 1428
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igGetCursorPos(Vector2* pOut);

		// Token: 0x06000595 RID: 1429
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern float igGetCursorPosX();

		// Token: 0x06000596 RID: 1430
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern float igGetCursorPosY();

		// Token: 0x06000597 RID: 1431
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igGetCursorScreenPos(Vector2* pOut);

		// Token: 0x06000598 RID: 1432
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igGetCursorStartPos(Vector2* pOut);

		// Token: 0x06000599 RID: 1433
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiPayload* igGetDragDropPayload();

		// Token: 0x0600059A RID: 1434
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImDrawData* igGetDrawData();

		// Token: 0x0600059B RID: 1435
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern IntPtr igGetDrawListSharedData();

		// Token: 0x0600059C RID: 1436
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImFont* igGetFont();

		// Token: 0x0600059D RID: 1437
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern float igGetFontSize();

		// Token: 0x0600059E RID: 1438
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igGetFontTexUvWhitePixel(Vector2* pOut);

		// Token: 0x0600059F RID: 1439
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImDrawList* igGetForegroundDrawList_Nil();

		// Token: 0x060005A0 RID: 1440
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImDrawList* igGetForegroundDrawList_ViewportPtr(ImGuiViewport* viewport);

		// Token: 0x060005A1 RID: 1441
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern int igGetFrameCount();

		// Token: 0x060005A2 RID: 1442
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern float igGetFrameHeight();

		// Token: 0x060005A3 RID: 1443
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern float igGetFrameHeightWithSpacing();

		// Token: 0x060005A4 RID: 1444
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern uint igGetID_Str(byte* str_id);

		// Token: 0x060005A5 RID: 1445
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern uint igGetID_StrStr(byte* str_id_begin, byte* str_id_end);

		// Token: 0x060005A6 RID: 1446
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern uint igGetID_Ptr(void* ptr_id);

		// Token: 0x060005A7 RID: 1447
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiIO* igGetIO();

		// Token: 0x060005A8 RID: 1448
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern uint igGetItemID();

		// Token: 0x060005A9 RID: 1449
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igGetItemRectMax(Vector2* pOut);

		// Token: 0x060005AA RID: 1450
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igGetItemRectMin(Vector2* pOut);

		// Token: 0x060005AB RID: 1451
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igGetItemRectSize(Vector2* pOut);

		// Token: 0x060005AC RID: 1452
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern ImGuiKey igGetKeyIndex(ImGuiKey key);

		// Token: 0x060005AD RID: 1453
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte* igGetKeyName(ImGuiKey key);

		// Token: 0x060005AE RID: 1454
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern int igGetKeyPressedAmount(ImGuiKey key, float repeat_delay, float rate);

		// Token: 0x060005AF RID: 1455
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiViewport* igGetMainViewport();

		// Token: 0x060005B0 RID: 1456
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern int igGetMouseClickedCount(ImGuiMouseButton button);

		// Token: 0x060005B1 RID: 1457
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern ImGuiMouseCursor igGetMouseCursor();

		// Token: 0x060005B2 RID: 1458
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igGetMouseDragDelta(Vector2* pOut, ImGuiMouseButton button, float lock_threshold);

		// Token: 0x060005B3 RID: 1459
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igGetMousePos(Vector2* pOut);

		// Token: 0x060005B4 RID: 1460
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igGetMousePosOnOpeningCurrentPopup(Vector2* pOut);

		// Token: 0x060005B5 RID: 1461
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiPlatformIO* igGetPlatformIO();

		// Token: 0x060005B6 RID: 1462
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern float igGetScrollMaxX();

		// Token: 0x060005B7 RID: 1463
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern float igGetScrollMaxY();

		// Token: 0x060005B8 RID: 1464
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern float igGetScrollX();

		// Token: 0x060005B9 RID: 1465
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern float igGetScrollY();

		// Token: 0x060005BA RID: 1466
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiStorage* igGetStateStorage();

		// Token: 0x060005BB RID: 1467
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiStyle* igGetStyle();

		// Token: 0x060005BC RID: 1468
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte* igGetStyleColorName(ImGuiCol idx);

		// Token: 0x060005BD RID: 1469
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern Vector4* igGetStyleColorVec4(ImGuiCol idx);

		// Token: 0x060005BE RID: 1470
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern float igGetTextLineHeight();

		// Token: 0x060005BF RID: 1471
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern float igGetTextLineHeightWithSpacing();

		// Token: 0x060005C0 RID: 1472
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern double igGetTime();

		// Token: 0x060005C1 RID: 1473
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern float igGetTreeNodeToLabelSpacing();

		// Token: 0x060005C2 RID: 1474
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte* igGetVersion();

		// Token: 0x060005C3 RID: 1475
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igGetWindowContentRegionMax(Vector2* pOut);

		// Token: 0x060005C4 RID: 1476
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igGetWindowContentRegionMin(Vector2* pOut);

		// Token: 0x060005C5 RID: 1477
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern uint igGetWindowDockID();

		// Token: 0x060005C6 RID: 1478
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern float igGetWindowDpiScale();

		// Token: 0x060005C7 RID: 1479
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImDrawList* igGetWindowDrawList();

		// Token: 0x060005C8 RID: 1480
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern float igGetWindowHeight();

		// Token: 0x060005C9 RID: 1481
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igGetWindowPos(Vector2* pOut);

		// Token: 0x060005CA RID: 1482
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igGetWindowSize(Vector2* pOut);

		// Token: 0x060005CB RID: 1483
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiViewport* igGetWindowViewport();

		// Token: 0x060005CC RID: 1484
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern float igGetWindowWidth();

		// Token: 0x060005CD RID: 1485
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igImage(IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1, Vector4 tint_col, Vector4 border_col);

		// Token: 0x060005CE RID: 1486
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igImageButton(byte* str_id, IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1, Vector4 bg_col, Vector4 tint_col);

		// Token: 0x060005CF RID: 1487
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igIndent(float indent_w);

		// Token: 0x060005D0 RID: 1488
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igInputDouble(byte* label, double* v, double step, double step_fast, byte* format, ImGuiInputTextFlags flags);

		// Token: 0x060005D1 RID: 1489
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igInputFloat(byte* label, float* v, float step, float step_fast, byte* format, ImGuiInputTextFlags flags);

		// Token: 0x060005D2 RID: 1490
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igInputFloat2(byte* label, Vector2* v, byte* format, ImGuiInputTextFlags flags);

		// Token: 0x060005D3 RID: 1491
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igInputFloat3(byte* label, Vector3* v, byte* format, ImGuiInputTextFlags flags);

		// Token: 0x060005D4 RID: 1492
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igInputFloat4(byte* label, Vector4* v, byte* format, ImGuiInputTextFlags flags);

		// Token: 0x060005D5 RID: 1493
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igInputInt(byte* label, int* v, int step, int step_fast, ImGuiInputTextFlags flags);

		// Token: 0x060005D6 RID: 1494
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igInputInt2(byte* label, int* v, ImGuiInputTextFlags flags);

		// Token: 0x060005D7 RID: 1495
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igInputInt3(byte* label, int* v, ImGuiInputTextFlags flags);

		// Token: 0x060005D8 RID: 1496
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igInputInt4(byte* label, int* v, ImGuiInputTextFlags flags);

		// Token: 0x060005D9 RID: 1497
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igInputScalar(byte* label, ImGuiDataType data_type, void* p_data, void* p_step, void* p_step_fast, byte* format, ImGuiInputTextFlags flags);

		// Token: 0x060005DA RID: 1498
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igInputScalarN(byte* label, ImGuiDataType data_type, void* p_data, int components, void* p_step, void* p_step_fast, byte* format, ImGuiInputTextFlags flags);

		// Token: 0x060005DB RID: 1499
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igInputText(byte* label, byte* buf, uint buf_size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data);

		// Token: 0x060005DC RID: 1500
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igInputTextMultiline(byte* label, byte* buf, uint buf_size, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data);

		// Token: 0x060005DD RID: 1501
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igInputTextWithHint(byte* label, byte* hint, byte* buf, uint buf_size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data);

		// Token: 0x060005DE RID: 1502
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igInvisibleButton(byte* str_id, Vector2 size, ImGuiButtonFlags flags);

		// Token: 0x060005DF RID: 1503
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsAnyItemActive();

		// Token: 0x060005E0 RID: 1504
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsAnyItemFocused();

		// Token: 0x060005E1 RID: 1505
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsAnyItemHovered();

		// Token: 0x060005E2 RID: 1506
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsAnyMouseDown();

		// Token: 0x060005E3 RID: 1507
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsItemActivated();

		// Token: 0x060005E4 RID: 1508
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsItemActive();

		// Token: 0x060005E5 RID: 1509
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsItemClicked(ImGuiMouseButton mouse_button);

		// Token: 0x060005E6 RID: 1510
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsItemDeactivated();

		// Token: 0x060005E7 RID: 1511
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsItemDeactivatedAfterEdit();

		// Token: 0x060005E8 RID: 1512
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsItemEdited();

		// Token: 0x060005E9 RID: 1513
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsItemFocused();

		// Token: 0x060005EA RID: 1514
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsItemHovered(ImGuiHoveredFlags flags);

		// Token: 0x060005EB RID: 1515
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsItemToggledOpen();

		// Token: 0x060005EC RID: 1516
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsItemVisible();

		// Token: 0x060005ED RID: 1517
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsKeyDown_Nil(ImGuiKey key);

		// Token: 0x060005EE RID: 1518
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsKeyPressed_Bool(ImGuiKey key, byte repeat);

		// Token: 0x060005EF RID: 1519
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsKeyReleased_Nil(ImGuiKey key);

		// Token: 0x060005F0 RID: 1520
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsMouseClicked_Bool(ImGuiMouseButton button, byte repeat);

		// Token: 0x060005F1 RID: 1521
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsMouseDoubleClicked(ImGuiMouseButton button);

		// Token: 0x060005F2 RID: 1522
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsMouseDown_Nil(ImGuiMouseButton button);

		// Token: 0x060005F3 RID: 1523
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsMouseDragging(ImGuiMouseButton button, float lock_threshold);

		// Token: 0x060005F4 RID: 1524
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsMouseHoveringRect(Vector2 r_min, Vector2 r_max, byte clip);

		// Token: 0x060005F5 RID: 1525
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igIsMousePosValid(Vector2* mouse_pos);

		// Token: 0x060005F6 RID: 1526
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsMouseReleased_Nil(ImGuiMouseButton button);

		// Token: 0x060005F7 RID: 1527
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igIsPopupOpen_Str(byte* str_id, ImGuiPopupFlags flags);

		// Token: 0x060005F8 RID: 1528
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsRectVisible_Nil(Vector2 size);

		// Token: 0x060005F9 RID: 1529
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsRectVisible_Vec2(Vector2 rect_min, Vector2 rect_max);

		// Token: 0x060005FA RID: 1530
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsWindowAppearing();

		// Token: 0x060005FB RID: 1531
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsWindowCollapsed();

		// Token: 0x060005FC RID: 1532
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsWindowDocked();

		// Token: 0x060005FD RID: 1533
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsWindowFocused(ImGuiFocusedFlags flags);

		// Token: 0x060005FE RID: 1534
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igIsWindowHovered(ImGuiHoveredFlags flags);

		// Token: 0x060005FF RID: 1535
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igLabelText(byte* label, byte* fmt);

		// Token: 0x06000600 RID: 1536
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igListBox_Str_arr(byte* label, int* current_item, byte** items, int items_count, int height_in_items);

		// Token: 0x06000601 RID: 1537
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igLoadIniSettingsFromDisk(byte* ini_filename);

		// Token: 0x06000602 RID: 1538
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igLoadIniSettingsFromMemory(byte* ini_data, uint ini_size);

		// Token: 0x06000603 RID: 1539
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igLogButtons();

		// Token: 0x06000604 RID: 1540
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igLogFinish();

		// Token: 0x06000605 RID: 1541
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igLogText(byte* fmt);

		// Token: 0x06000606 RID: 1542
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igLogToClipboard(int auto_open_depth);

		// Token: 0x06000607 RID: 1543
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igLogToFile(int auto_open_depth, byte* filename);

		// Token: 0x06000608 RID: 1544
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igLogToTTY(int auto_open_depth);

		// Token: 0x06000609 RID: 1545
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void* igMemAlloc(uint size);

		// Token: 0x0600060A RID: 1546
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igMemFree(void* ptr);

		// Token: 0x0600060B RID: 1547
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igMenuItem_Bool(byte* label, byte* shortcut, byte selected, byte enabled);

		// Token: 0x0600060C RID: 1548
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igMenuItem_BoolPtr(byte* label, byte* shortcut, byte* p_selected, byte enabled);

		// Token: 0x0600060D RID: 1549
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igNewFrame();

		// Token: 0x0600060E RID: 1550
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igNewLine();

		// Token: 0x0600060F RID: 1551
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igNextColumn();

		// Token: 0x06000610 RID: 1552
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igOpenPopup_Str(byte* str_id, ImGuiPopupFlags popup_flags);

		// Token: 0x06000611 RID: 1553
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igOpenPopup_ID(uint id, ImGuiPopupFlags popup_flags);

		// Token: 0x06000612 RID: 1554
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igOpenPopupOnItemClick(byte* str_id, ImGuiPopupFlags popup_flags);

		// Token: 0x06000613 RID: 1555
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igPlotHistogram_FloatPtr(byte* label, float* values, int values_count, int values_offset, byte* overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride);

		// Token: 0x06000614 RID: 1556
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igPlotLines_FloatPtr(byte* label, float* values, int values_count, int values_offset, byte* overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride);

		// Token: 0x06000615 RID: 1557
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igPopButtonRepeat();

		// Token: 0x06000616 RID: 1558
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igPopClipRect();

		// Token: 0x06000617 RID: 1559
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igPopFont();

		// Token: 0x06000618 RID: 1560
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igPopID();

		// Token: 0x06000619 RID: 1561
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igPopItemWidth();

		// Token: 0x0600061A RID: 1562
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igPopStyleColor(int count);

		// Token: 0x0600061B RID: 1563
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igPopStyleVar(int count);

		// Token: 0x0600061C RID: 1564
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igPopTabStop();

		// Token: 0x0600061D RID: 1565
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igPopTextWrapPos();

		// Token: 0x0600061E RID: 1566
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igProgressBar(float fraction, Vector2 size_arg, byte* overlay);

		// Token: 0x0600061F RID: 1567
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igPushButtonRepeat(byte repeat);

		// Token: 0x06000620 RID: 1568
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igPushClipRect(Vector2 clip_rect_min, Vector2 clip_rect_max, byte intersect_with_current_clip_rect);

		// Token: 0x06000621 RID: 1569
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igPushFont(ImFont* font);

		// Token: 0x06000622 RID: 1570
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igPushID_Str(byte* str_id);

		// Token: 0x06000623 RID: 1571
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igPushID_StrStr(byte* str_id_begin, byte* str_id_end);

		// Token: 0x06000624 RID: 1572
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igPushID_Ptr(void* ptr_id);

		// Token: 0x06000625 RID: 1573
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igPushID_Int(int int_id);

		// Token: 0x06000626 RID: 1574
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igPushItemWidth(float item_width);

		// Token: 0x06000627 RID: 1575
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igPushStyleColor_U32(ImGuiCol idx, uint col);

		// Token: 0x06000628 RID: 1576
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igPushStyleColor_Vec4(ImGuiCol idx, Vector4 col);

		// Token: 0x06000629 RID: 1577
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igPushStyleVar_Float(ImGuiStyleVar idx, float val);

		// Token: 0x0600062A RID: 1578
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igPushStyleVar_Vec2(ImGuiStyleVar idx, Vector2 val);

		// Token: 0x0600062B RID: 1579
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igPushTabStop(byte tab_stop);

		// Token: 0x0600062C RID: 1580
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igPushTextWrapPos(float wrap_local_pos_x);

		// Token: 0x0600062D RID: 1581
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igRadioButton_Bool(byte* label, byte active);

		// Token: 0x0600062E RID: 1582
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igRadioButton_IntPtr(byte* label, int* v, int v_button);

		// Token: 0x0600062F RID: 1583
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igRender();

		// Token: 0x06000630 RID: 1584
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igRenderPlatformWindowsDefault(void* platform_render_arg, void* renderer_render_arg);

		// Token: 0x06000631 RID: 1585
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igResetMouseDragDelta(ImGuiMouseButton button);

		// Token: 0x06000632 RID: 1586
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSameLine(float offset_from_start_x, float spacing);

		// Token: 0x06000633 RID: 1587
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igSaveIniSettingsToDisk(byte* ini_filename);

		// Token: 0x06000634 RID: 1588
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte* igSaveIniSettingsToMemory(uint* out_ini_size);

		// Token: 0x06000635 RID: 1589
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igSelectable_Bool(byte* label, byte selected, ImGuiSelectableFlags flags, Vector2 size);

		// Token: 0x06000636 RID: 1590
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igSelectable_BoolPtr(byte* label, byte* p_selected, ImGuiSelectableFlags flags, Vector2 size);

		// Token: 0x06000637 RID: 1591
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSeparator();

		// Token: 0x06000638 RID: 1592
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igSeparatorText(byte* label);

		// Token: 0x06000639 RID: 1593
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igSetAllocatorFunctions(IntPtr alloc_func, IntPtr free_func, void* user_data);

		// Token: 0x0600063A RID: 1594
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igSetClipboardText(byte* text);

		// Token: 0x0600063B RID: 1595
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetColorEditOptions(ImGuiColorEditFlags flags);

		// Token: 0x0600063C RID: 1596
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetColumnOffset(int column_index, float offset_x);

		// Token: 0x0600063D RID: 1597
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetColumnWidth(int column_index, float width);

		// Token: 0x0600063E RID: 1598
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetCurrentContext(IntPtr ctx);

		// Token: 0x0600063F RID: 1599
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetCursorPos(Vector2 local_pos);

		// Token: 0x06000640 RID: 1600
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetCursorPosX(float local_x);

		// Token: 0x06000641 RID: 1601
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetCursorPosY(float local_y);

		// Token: 0x06000642 RID: 1602
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetCursorScreenPos(Vector2 pos);

		// Token: 0x06000643 RID: 1603
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igSetDragDropPayload(byte* type, void* data, uint sz, ImGuiCond cond);

		// Token: 0x06000644 RID: 1604
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetItemDefaultFocus();

		// Token: 0x06000645 RID: 1605
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igSetItemTooltip(byte* fmt);

		// Token: 0x06000646 RID: 1606
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetKeyboardFocusHere(int offset);

		// Token: 0x06000647 RID: 1607
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetMouseCursor(ImGuiMouseCursor cursor_type);

		// Token: 0x06000648 RID: 1608
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetNextFrameWantCaptureKeyboard(byte want_capture_keyboard);

		// Token: 0x06000649 RID: 1609
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetNextFrameWantCaptureMouse(byte want_capture_mouse);

		// Token: 0x0600064A RID: 1610
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetNextItemAllowOverlap();

		// Token: 0x0600064B RID: 1611
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetNextItemOpen(byte is_open, ImGuiCond cond);

		// Token: 0x0600064C RID: 1612
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetNextItemWidth(float item_width);

		// Token: 0x0600064D RID: 1613
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetNextWindowBgAlpha(float alpha);

		// Token: 0x0600064E RID: 1614
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igSetNextWindowClass(ImGuiWindowClass* window_class);

		// Token: 0x0600064F RID: 1615
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetNextWindowCollapsed(byte collapsed, ImGuiCond cond);

		// Token: 0x06000650 RID: 1616
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetNextWindowContentSize(Vector2 size);

		// Token: 0x06000651 RID: 1617
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetNextWindowDockID(uint dock_id, ImGuiCond cond);

		// Token: 0x06000652 RID: 1618
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetNextWindowFocus();

		// Token: 0x06000653 RID: 1619
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetNextWindowPos(Vector2 pos, ImGuiCond cond, Vector2 pivot);

		// Token: 0x06000654 RID: 1620
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetNextWindowScroll(Vector2 scroll);

		// Token: 0x06000655 RID: 1621
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetNextWindowSize(Vector2 size, ImGuiCond cond);

		// Token: 0x06000656 RID: 1622
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igSetNextWindowSizeConstraints(Vector2 size_min, Vector2 size_max, ImGuiSizeCallback custom_callback, void* custom_callback_data);

		// Token: 0x06000657 RID: 1623
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetNextWindowViewport(uint viewport_id);

		// Token: 0x06000658 RID: 1624
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetScrollFromPosX_Float(float local_x, float center_x_ratio);

		// Token: 0x06000659 RID: 1625
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetScrollFromPosY_Float(float local_y, float center_y_ratio);

		// Token: 0x0600065A RID: 1626
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetScrollHereX(float center_x_ratio);

		// Token: 0x0600065B RID: 1627
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetScrollHereY(float center_y_ratio);

		// Token: 0x0600065C RID: 1628
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetScrollX_Float(float scroll_x);

		// Token: 0x0600065D RID: 1629
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetScrollY_Float(float scroll_y);

		// Token: 0x0600065E RID: 1630
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igSetStateStorage(ImGuiStorage* storage);

		// Token: 0x0600065F RID: 1631
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igSetTabItemClosed(byte* tab_or_docked_window_label);

		// Token: 0x06000660 RID: 1632
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igSetTooltip(byte* fmt);

		// Token: 0x06000661 RID: 1633
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetWindowCollapsed_Bool(byte collapsed, ImGuiCond cond);

		// Token: 0x06000662 RID: 1634
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igSetWindowCollapsed_Str(byte* name, byte collapsed, ImGuiCond cond);

		// Token: 0x06000663 RID: 1635
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetWindowFocus_Nil();

		// Token: 0x06000664 RID: 1636
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igSetWindowFocus_Str(byte* name);

		// Token: 0x06000665 RID: 1637
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetWindowFontScale(float scale);

		// Token: 0x06000666 RID: 1638
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetWindowPos_Vec2(Vector2 pos, ImGuiCond cond);

		// Token: 0x06000667 RID: 1639
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igSetWindowPos_Str(byte* name, Vector2 pos, ImGuiCond cond);

		// Token: 0x06000668 RID: 1640
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSetWindowSize_Vec2(Vector2 size, ImGuiCond cond);

		// Token: 0x06000669 RID: 1641
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igSetWindowSize_Str(byte* name, Vector2 size, ImGuiCond cond);

		// Token: 0x0600066A RID: 1642
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igShowAboutWindow(byte* p_open);

		// Token: 0x0600066B RID: 1643
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igShowDebugLogWindow(byte* p_open);

		// Token: 0x0600066C RID: 1644
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igShowDemoWindow(byte* p_open);

		// Token: 0x0600066D RID: 1645
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igShowFontSelector(byte* label);

		// Token: 0x0600066E RID: 1646
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igShowMetricsWindow(byte* p_open);

		// Token: 0x0600066F RID: 1647
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igShowStackToolWindow(byte* p_open);

		// Token: 0x06000670 RID: 1648
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igShowStyleEditor(ImGuiStyle* @ref);

		// Token: 0x06000671 RID: 1649
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igShowStyleSelector(byte* label);

		// Token: 0x06000672 RID: 1650
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igShowUserGuide();

		// Token: 0x06000673 RID: 1651
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igSliderAngle(byte* label, float* v_rad, float v_degrees_min, float v_degrees_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x06000674 RID: 1652
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igSliderFloat(byte* label, float* v, float v_min, float v_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x06000675 RID: 1653
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igSliderFloat2(byte* label, Vector2* v, float v_min, float v_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x06000676 RID: 1654
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igSliderFloat3(byte* label, Vector3* v, float v_min, float v_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x06000677 RID: 1655
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igSliderFloat4(byte* label, Vector4* v, float v_min, float v_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x06000678 RID: 1656
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igSliderInt(byte* label, int* v, int v_min, int v_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x06000679 RID: 1657
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igSliderInt2(byte* label, int* v, int v_min, int v_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x0600067A RID: 1658
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igSliderInt3(byte* label, int* v, int v_min, int v_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x0600067B RID: 1659
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igSliderInt4(byte* label, int* v, int v_min, int v_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x0600067C RID: 1660
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igSliderScalar(byte* label, ImGuiDataType data_type, void* p_data, void* p_min, void* p_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x0600067D RID: 1661
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igSliderScalarN(byte* label, ImGuiDataType data_type, void* p_data, int components, void* p_min, void* p_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x0600067E RID: 1662
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igSmallButton(byte* label);

		// Token: 0x0600067F RID: 1663
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igSpacing();

		// Token: 0x06000680 RID: 1664
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igStyleColorsClassic(ImGuiStyle* dst);

		// Token: 0x06000681 RID: 1665
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igStyleColorsDark(ImGuiStyle* dst);

		// Token: 0x06000682 RID: 1666
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igStyleColorsLight(ImGuiStyle* dst);

		// Token: 0x06000683 RID: 1667
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igTabItemButton(byte* label, ImGuiTabItemFlags flags);

		// Token: 0x06000684 RID: 1668
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern int igTableGetColumnCount();

		// Token: 0x06000685 RID: 1669
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern ImGuiTableColumnFlags igTableGetColumnFlags(int column_n);

		// Token: 0x06000686 RID: 1670
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern int igTableGetColumnIndex();

		// Token: 0x06000687 RID: 1671
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte* igTableGetColumnName_Int(int column_n);

		// Token: 0x06000688 RID: 1672
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern int igTableGetRowIndex();

		// Token: 0x06000689 RID: 1673
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiTableSortSpecs* igTableGetSortSpecs();

		// Token: 0x0600068A RID: 1674
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igTableHeader(byte* label);

		// Token: 0x0600068B RID: 1675
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igTableHeadersRow();

		// Token: 0x0600068C RID: 1676
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igTableNextColumn();

		// Token: 0x0600068D RID: 1677
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igTableNextRow(ImGuiTableRowFlags row_flags, float min_row_height);

		// Token: 0x0600068E RID: 1678
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igTableSetBgColor(ImGuiTableBgTarget target, uint color, int column_n);

		// Token: 0x0600068F RID: 1679
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igTableSetColumnEnabled(int column_n, byte v);

		// Token: 0x06000690 RID: 1680
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern byte igTableSetColumnIndex(int column_n);

		// Token: 0x06000691 RID: 1681
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igTableSetupColumn(byte* label, ImGuiTableColumnFlags flags, float init_width_or_weight, uint user_id);

		// Token: 0x06000692 RID: 1682
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igTableSetupScrollFreeze(int cols, int rows);

		// Token: 0x06000693 RID: 1683
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igText(byte* fmt);

		// Token: 0x06000694 RID: 1684
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igTextColored(Vector4 col, byte* fmt);

		// Token: 0x06000695 RID: 1685
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igTextDisabled(byte* fmt);

		// Token: 0x06000696 RID: 1686
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igTextUnformatted(byte* text, byte* text_end);

		// Token: 0x06000697 RID: 1687
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igTextWrapped(byte* fmt);

		// Token: 0x06000698 RID: 1688
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igTreeNode_Str(byte* label);

		// Token: 0x06000699 RID: 1689
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igTreeNode_StrStr(byte* str_id, byte* fmt);

		// Token: 0x0600069A RID: 1690
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igTreeNode_Ptr(void* ptr_id, byte* fmt);

		// Token: 0x0600069B RID: 1691
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igTreeNodeEx_Str(byte* label, ImGuiTreeNodeFlags flags);

		// Token: 0x0600069C RID: 1692
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igTreeNodeEx_StrStr(byte* str_id, ImGuiTreeNodeFlags flags, byte* fmt);

		// Token: 0x0600069D RID: 1693
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igTreeNodeEx_Ptr(void* ptr_id, ImGuiTreeNodeFlags flags, byte* fmt);

		// Token: 0x0600069E RID: 1694
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igTreePop();

		// Token: 0x0600069F RID: 1695
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igTreePush_Str(byte* str_id);

		// Token: 0x060006A0 RID: 1696
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igTreePush_Ptr(void* ptr_id);

		// Token: 0x060006A1 RID: 1697
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igUnindent(float indent_w);

		// Token: 0x060006A2 RID: 1698
		[DllImport("cimgui", CallingConvention = 2)]
		public static extern void igUpdatePlatformWindows();

		// Token: 0x060006A3 RID: 1699
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igValue_Bool(byte* prefix, byte b);

		// Token: 0x060006A4 RID: 1700
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igValue_Int(byte* prefix, int v);

		// Token: 0x060006A5 RID: 1701
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igValue_Uint(byte* prefix, uint v);

		// Token: 0x060006A6 RID: 1702
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void igValue_Float(byte* prefix, float v, byte* float_format);

		// Token: 0x060006A7 RID: 1703
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igVSliderFloat(byte* label, Vector2 size, float* v, float v_min, float v_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x060006A8 RID: 1704
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igVSliderInt(byte* label, Vector2 size, int* v, int v_min, int v_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x060006A9 RID: 1705
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte igVSliderScalar(byte* label, Vector2 size, ImGuiDataType data_type, void* p_data, void* p_min, void* p_max, byte* format, ImGuiSliderFlags flags);

		// Token: 0x060006AA RID: 1706
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImColor_destroy(ImColor* self);

		// Token: 0x060006AB RID: 1707
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImColor_HSV(ImColor* pOut, float h, float s, float v, float a);

		// Token: 0x060006AC RID: 1708
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImColor* ImColor_ImColor_Nil();

		// Token: 0x060006AD RID: 1709
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImColor* ImColor_ImColor_Float(float r, float g, float b, float a);

		// Token: 0x060006AE RID: 1710
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImColor* ImColor_ImColor_Vec4(Vector4 col);

		// Token: 0x060006AF RID: 1711
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImColor* ImColor_ImColor_Int(int r, int g, int b, int a);

		// Token: 0x060006B0 RID: 1712
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImColor* ImColor_ImColor_U32(uint rgba);

		// Token: 0x060006B1 RID: 1713
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImColor_SetHSV(ImColor* self, float h, float s, float v, float a);

		// Token: 0x060006B2 RID: 1714
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawCmd_destroy(ImDrawCmd* self);

		// Token: 0x060006B3 RID: 1715
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern IntPtr ImDrawCmd_GetTexID(ImDrawCmd* self);

		// Token: 0x060006B4 RID: 1716
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImDrawCmd* ImDrawCmd_ImDrawCmd();

		// Token: 0x060006B5 RID: 1717
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawData_AddDrawList(ImDrawData* self, ImDrawList* draw_list);

		// Token: 0x060006B6 RID: 1718
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawData_Clear(ImDrawData* self);

		// Token: 0x060006B7 RID: 1719
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawData_DeIndexAllBuffers(ImDrawData* self);

		// Token: 0x060006B8 RID: 1720
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawData_destroy(ImDrawData* self);

		// Token: 0x060006B9 RID: 1721
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImDrawData* ImDrawData_ImDrawData();

		// Token: 0x060006BA RID: 1722
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawData_ScaleClipRects(ImDrawData* self, Vector2 fb_scale);

		// Token: 0x060006BB RID: 1723
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern int ImDrawList__CalcCircleAutoSegmentCount(ImDrawList* self, float radius);

		// Token: 0x060006BC RID: 1724
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList__ClearFreeMemory(ImDrawList* self);

		// Token: 0x060006BD RID: 1725
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList__OnChangedClipRect(ImDrawList* self);

		// Token: 0x060006BE RID: 1726
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList__OnChangedTextureID(ImDrawList* self);

		// Token: 0x060006BF RID: 1727
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList__OnChangedVtxOffset(ImDrawList* self);

		// Token: 0x060006C0 RID: 1728
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList__PathArcToFastEx(ImDrawList* self, Vector2 center, float radius, int a_min_sample, int a_max_sample, int a_step);

		// Token: 0x060006C1 RID: 1729
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList__PathArcToN(ImDrawList* self, Vector2 center, float radius, float a_min, float a_max, int num_segments);

		// Token: 0x060006C2 RID: 1730
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList__PopUnusedDrawCmd(ImDrawList* self);

		// Token: 0x060006C3 RID: 1731
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList__ResetForNewFrame(ImDrawList* self);

		// Token: 0x060006C4 RID: 1732
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList__TryMergeDrawCmds(ImDrawList* self);

		// Token: 0x060006C5 RID: 1733
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddBezierCubic(ImDrawList* self, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col, float thickness, int num_segments);

		// Token: 0x060006C6 RID: 1734
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddBezierQuadratic(ImDrawList* self, Vector2 p1, Vector2 p2, Vector2 p3, uint col, float thickness, int num_segments);

		// Token: 0x060006C7 RID: 1735
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddCallback(ImDrawList* self, IntPtr callback, void* callback_data);

		// Token: 0x060006C8 RID: 1736
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddCircle(ImDrawList* self, Vector2 center, float radius, uint col, int num_segments, float thickness);

		// Token: 0x060006C9 RID: 1737
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddCircleFilled(ImDrawList* self, Vector2 center, float radius, uint col, int num_segments);

		// Token: 0x060006CA RID: 1738
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddConvexPolyFilled(ImDrawList* self, Vector2* points, int num_points, uint col);

		// Token: 0x060006CB RID: 1739
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddDrawCmd(ImDrawList* self);

		// Token: 0x060006CC RID: 1740
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddImage(ImDrawList* self, IntPtr user_texture_id, Vector2 p_min, Vector2 p_max, Vector2 uv_min, Vector2 uv_max, uint col);

		// Token: 0x060006CD RID: 1741
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddImageQuad(ImDrawList* self, IntPtr user_texture_id, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4, uint col);

		// Token: 0x060006CE RID: 1742
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddImageRounded(ImDrawList* self, IntPtr user_texture_id, Vector2 p_min, Vector2 p_max, Vector2 uv_min, Vector2 uv_max, uint col, float rounding, ImDrawFlags flags);

		// Token: 0x060006CF RID: 1743
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddLine(ImDrawList* self, Vector2 p1, Vector2 p2, uint col, float thickness);

		// Token: 0x060006D0 RID: 1744
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddNgon(ImDrawList* self, Vector2 center, float radius, uint col, int num_segments, float thickness);

		// Token: 0x060006D1 RID: 1745
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddNgonFilled(ImDrawList* self, Vector2 center, float radius, uint col, int num_segments);

		// Token: 0x060006D2 RID: 1746
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddPolyline(ImDrawList* self, Vector2* points, int num_points, uint col, ImDrawFlags flags, float thickness);

		// Token: 0x060006D3 RID: 1747
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddQuad(ImDrawList* self, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col, float thickness);

		// Token: 0x060006D4 RID: 1748
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddQuadFilled(ImDrawList* self, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, uint col);

		// Token: 0x060006D5 RID: 1749
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddRect(ImDrawList* self, Vector2 p_min, Vector2 p_max, uint col, float rounding, ImDrawFlags flags, float thickness);

		// Token: 0x060006D6 RID: 1750
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddRectFilled(ImDrawList* self, Vector2 p_min, Vector2 p_max, uint col, float rounding, ImDrawFlags flags);

		// Token: 0x060006D7 RID: 1751
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddRectFilledMultiColor(ImDrawList* self, Vector2 p_min, Vector2 p_max, uint col_upr_left, uint col_upr_right, uint col_bot_right, uint col_bot_left);

		// Token: 0x060006D8 RID: 1752
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddText_Vec2(ImDrawList* self, Vector2 pos, uint col, byte* text_begin, byte* text_end);

		// Token: 0x060006D9 RID: 1753
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddText_FontPtr(ImDrawList* self, ImFont* font, float font_size, Vector2 pos, uint col, byte* text_begin, byte* text_end, float wrap_width, Vector4* cpu_fine_clip_rect);

		// Token: 0x060006DA RID: 1754
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddTriangle(ImDrawList* self, Vector2 p1, Vector2 p2, Vector2 p3, uint col, float thickness);

		// Token: 0x060006DB RID: 1755
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_AddTriangleFilled(ImDrawList* self, Vector2 p1, Vector2 p2, Vector2 p3, uint col);

		// Token: 0x060006DC RID: 1756
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_ChannelsMerge(ImDrawList* self);

		// Token: 0x060006DD RID: 1757
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_ChannelsSetCurrent(ImDrawList* self, int n);

		// Token: 0x060006DE RID: 1758
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_ChannelsSplit(ImDrawList* self, int count);

		// Token: 0x060006DF RID: 1759
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImDrawList* ImDrawList_CloneOutput(ImDrawList* self);

		// Token: 0x060006E0 RID: 1760
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_destroy(ImDrawList* self);

		// Token: 0x060006E1 RID: 1761
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_GetClipRectMax(Vector2* pOut, ImDrawList* self);

		// Token: 0x060006E2 RID: 1762
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_GetClipRectMin(Vector2* pOut, ImDrawList* self);

		// Token: 0x060006E3 RID: 1763
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImDrawList* ImDrawList_ImDrawList(IntPtr shared_data);

		// Token: 0x060006E4 RID: 1764
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PathArcTo(ImDrawList* self, Vector2 center, float radius, float a_min, float a_max, int num_segments);

		// Token: 0x060006E5 RID: 1765
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PathArcToFast(ImDrawList* self, Vector2 center, float radius, int a_min_of_12, int a_max_of_12);

		// Token: 0x060006E6 RID: 1766
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PathBezierCubicCurveTo(ImDrawList* self, Vector2 p2, Vector2 p3, Vector2 p4, int num_segments);

		// Token: 0x060006E7 RID: 1767
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PathBezierQuadraticCurveTo(ImDrawList* self, Vector2 p2, Vector2 p3, int num_segments);

		// Token: 0x060006E8 RID: 1768
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PathClear(ImDrawList* self);

		// Token: 0x060006E9 RID: 1769
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PathFillConvex(ImDrawList* self, uint col);

		// Token: 0x060006EA RID: 1770
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PathLineTo(ImDrawList* self, Vector2 pos);

		// Token: 0x060006EB RID: 1771
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PathLineToMergeDuplicate(ImDrawList* self, Vector2 pos);

		// Token: 0x060006EC RID: 1772
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PathRect(ImDrawList* self, Vector2 rect_min, Vector2 rect_max, float rounding, ImDrawFlags flags);

		// Token: 0x060006ED RID: 1773
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PathStroke(ImDrawList* self, uint col, ImDrawFlags flags, float thickness);

		// Token: 0x060006EE RID: 1774
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PopClipRect(ImDrawList* self);

		// Token: 0x060006EF RID: 1775
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PopTextureID(ImDrawList* self);

		// Token: 0x060006F0 RID: 1776
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PrimQuadUV(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uv_a, Vector2 uv_b, Vector2 uv_c, Vector2 uv_d, uint col);

		// Token: 0x060006F1 RID: 1777
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PrimRect(ImDrawList* self, Vector2 a, Vector2 b, uint col);

		// Token: 0x060006F2 RID: 1778
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PrimRectUV(ImDrawList* self, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col);

		// Token: 0x060006F3 RID: 1779
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PrimReserve(ImDrawList* self, int idx_count, int vtx_count);

		// Token: 0x060006F4 RID: 1780
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PrimUnreserve(ImDrawList* self, int idx_count, int vtx_count);

		// Token: 0x060006F5 RID: 1781
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PrimVtx(ImDrawList* self, Vector2 pos, Vector2 uv, uint col);

		// Token: 0x060006F6 RID: 1782
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PrimWriteIdx(ImDrawList* self, ushort idx);

		// Token: 0x060006F7 RID: 1783
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PrimWriteVtx(ImDrawList* self, Vector2 pos, Vector2 uv, uint col);

		// Token: 0x060006F8 RID: 1784
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PushClipRect(ImDrawList* self, Vector2 clip_rect_min, Vector2 clip_rect_max, byte intersect_with_current_clip_rect);

		// Token: 0x060006F9 RID: 1785
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PushClipRectFullScreen(ImDrawList* self);

		// Token: 0x060006FA RID: 1786
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawList_PushTextureID(ImDrawList* self, IntPtr texture_id);

		// Token: 0x060006FB RID: 1787
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawListSplitter_Clear(ImDrawListSplitter* self);

		// Token: 0x060006FC RID: 1788
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawListSplitter_ClearFreeMemory(ImDrawListSplitter* self);

		// Token: 0x060006FD RID: 1789
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawListSplitter_destroy(ImDrawListSplitter* self);

		// Token: 0x060006FE RID: 1790
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImDrawListSplitter* ImDrawListSplitter_ImDrawListSplitter();

		// Token: 0x060006FF RID: 1791
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawListSplitter_Merge(ImDrawListSplitter* self, ImDrawList* draw_list);

		// Token: 0x06000700 RID: 1792
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawListSplitter_SetCurrentChannel(ImDrawListSplitter* self, ImDrawList* draw_list, int channel_idx);

		// Token: 0x06000701 RID: 1793
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImDrawListSplitter_Split(ImDrawListSplitter* self, ImDrawList* draw_list, int count);

		// Token: 0x06000702 RID: 1794
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFont_AddGlyph(ImFont* self, ImFontConfig* src_cfg, ushort c, float x0, float y0, float x1, float y1, float u0, float v0, float u1, float v1, float advance_x);

		// Token: 0x06000703 RID: 1795
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFont_AddRemapChar(ImFont* self, ushort dst, ushort src, byte overwrite_dst);

		// Token: 0x06000704 RID: 1796
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFont_BuildLookupTable(ImFont* self);

		// Token: 0x06000705 RID: 1797
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFont_CalcTextSizeA(Vector2* pOut, ImFont* self, float size, float max_width, float wrap_width, byte* text_begin, byte* text_end, byte** remaining);

		// Token: 0x06000706 RID: 1798
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte* ImFont_CalcWordWrapPositionA(ImFont* self, float scale, byte* text, byte* text_end, float wrap_width);

		// Token: 0x06000707 RID: 1799
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFont_ClearOutputData(ImFont* self);

		// Token: 0x06000708 RID: 1800
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFont_destroy(ImFont* self);

		// Token: 0x06000709 RID: 1801
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImFontGlyph* ImFont_FindGlyph(ImFont* self, ushort c);

		// Token: 0x0600070A RID: 1802
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImFontGlyph* ImFont_FindGlyphNoFallback(ImFont* self, ushort c);

		// Token: 0x0600070B RID: 1803
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern float ImFont_GetCharAdvance(ImFont* self, ushort c);

		// Token: 0x0600070C RID: 1804
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte* ImFont_GetDebugName(ImFont* self);

		// Token: 0x0600070D RID: 1805
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFont_GrowIndex(ImFont* self, int new_size);

		// Token: 0x0600070E RID: 1806
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImFont* ImFont_ImFont();

		// Token: 0x0600070F RID: 1807
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte ImFont_IsGlyphRangeUnused(ImFont* self, uint c_begin, uint c_last);

		// Token: 0x06000710 RID: 1808
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte ImFont_IsLoaded(ImFont* self);

		// Token: 0x06000711 RID: 1809
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFont_RenderChar(ImFont* self, ImDrawList* draw_list, float size, Vector2 pos, uint col, ushort c);

		// Token: 0x06000712 RID: 1810
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFont_RenderText(ImFont* self, ImDrawList* draw_list, float size, Vector2 pos, uint col, Vector4 clip_rect, byte* text_begin, byte* text_end, float wrap_width, byte cpu_fine_clip);

		// Token: 0x06000713 RID: 1811
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFont_SetGlyphVisible(ImFont* self, ushort c, byte visible);

		// Token: 0x06000714 RID: 1812
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern int ImFontAtlas_AddCustomRectFontGlyph(ImFontAtlas* self, ImFont* font, ushort id, int width, int height, float advance_x, Vector2 offset);

		// Token: 0x06000715 RID: 1813
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern int ImFontAtlas_AddCustomRectRegular(ImFontAtlas* self, int width, int height);

		// Token: 0x06000716 RID: 1814
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImFont* ImFontAtlas_AddFont(ImFontAtlas* self, ImFontConfig* font_cfg);

		// Token: 0x06000717 RID: 1815
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImFont* ImFontAtlas_AddFontDefault(ImFontAtlas* self, ImFontConfig* font_cfg);

		// Token: 0x06000718 RID: 1816
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImFont* ImFontAtlas_AddFontFromFileTTF(ImFontAtlas* self, byte* filename, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges);

		// Token: 0x06000719 RID: 1817
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImFont* ImFontAtlas_AddFontFromMemoryCompressedBase85TTF(ImFontAtlas* self, byte* compressed_font_data_base85, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges);

		// Token: 0x0600071A RID: 1818
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImFont* ImFontAtlas_AddFontFromMemoryCompressedTTF(ImFontAtlas* self, void* compressed_font_data, int compressed_font_size, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges);

		// Token: 0x0600071B RID: 1819
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImFont* ImFontAtlas_AddFontFromMemoryTTF(ImFontAtlas* self, void* font_data, int font_size, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges);

		// Token: 0x0600071C RID: 1820
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte ImFontAtlas_Build(ImFontAtlas* self);

		// Token: 0x0600071D RID: 1821
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFontAtlas_CalcCustomRectUV(ImFontAtlas* self, ImFontAtlasCustomRect* rect, Vector2* out_uv_min, Vector2* out_uv_max);

		// Token: 0x0600071E RID: 1822
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFontAtlas_Clear(ImFontAtlas* self);

		// Token: 0x0600071F RID: 1823
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFontAtlas_ClearFonts(ImFontAtlas* self);

		// Token: 0x06000720 RID: 1824
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFontAtlas_ClearInputData(ImFontAtlas* self);

		// Token: 0x06000721 RID: 1825
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFontAtlas_ClearTexData(ImFontAtlas* self);

		// Token: 0x06000722 RID: 1826
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFontAtlas_destroy(ImFontAtlas* self);

		// Token: 0x06000723 RID: 1827
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImFontAtlasCustomRect* ImFontAtlas_GetCustomRectByIndex(ImFontAtlas* self, int index);

		// Token: 0x06000724 RID: 1828
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ushort* ImFontAtlas_GetGlyphRangesChineseFull(ImFontAtlas* self);

		// Token: 0x06000725 RID: 1829
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ushort* ImFontAtlas_GetGlyphRangesChineseSimplifiedCommon(ImFontAtlas* self);

		// Token: 0x06000726 RID: 1830
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ushort* ImFontAtlas_GetGlyphRangesCyrillic(ImFontAtlas* self);

		// Token: 0x06000727 RID: 1831
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ushort* ImFontAtlas_GetGlyphRangesDefault(ImFontAtlas* self);

		// Token: 0x06000728 RID: 1832
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ushort* ImFontAtlas_GetGlyphRangesGreek(ImFontAtlas* self);

		// Token: 0x06000729 RID: 1833
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ushort* ImFontAtlas_GetGlyphRangesJapanese(ImFontAtlas* self);

		// Token: 0x0600072A RID: 1834
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ushort* ImFontAtlas_GetGlyphRangesKorean(ImFontAtlas* self);

		// Token: 0x0600072B RID: 1835
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ushort* ImFontAtlas_GetGlyphRangesThai(ImFontAtlas* self);

		// Token: 0x0600072C RID: 1836
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ushort* ImFontAtlas_GetGlyphRangesVietnamese(ImFontAtlas* self);

		// Token: 0x0600072D RID: 1837
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte ImFontAtlas_GetMouseCursorTexData(ImFontAtlas* self, ImGuiMouseCursor cursor, Vector2* out_offset, Vector2* out_size, Vector2* out_uv_border, Vector2* out_uv_fill);

		// Token: 0x0600072E RID: 1838
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFontAtlas_GetTexDataAsAlpha8(ImFontAtlas* self, byte** out_pixels, int* out_width, int* out_height, int* out_bytes_per_pixel);

		// Token: 0x0600072F RID: 1839
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFontAtlas_GetTexDataAsAlpha8(ImFontAtlas* self, IntPtr* out_pixels, int* out_width, int* out_height, int* out_bytes_per_pixel);

		// Token: 0x06000730 RID: 1840
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFontAtlas_GetTexDataAsRGBA32(ImFontAtlas* self, byte** out_pixels, int* out_width, int* out_height, int* out_bytes_per_pixel);

		// Token: 0x06000731 RID: 1841
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFontAtlas_GetTexDataAsRGBA32(ImFontAtlas* self, IntPtr* out_pixels, int* out_width, int* out_height, int* out_bytes_per_pixel);

		// Token: 0x06000732 RID: 1842
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImFontAtlas* ImFontAtlas_ImFontAtlas();

		// Token: 0x06000733 RID: 1843
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte ImFontAtlas_IsBuilt(ImFontAtlas* self);

		// Token: 0x06000734 RID: 1844
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFontAtlas_SetTexID(ImFontAtlas* self, IntPtr id);

		// Token: 0x06000735 RID: 1845
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFontAtlasCustomRect_destroy(ImFontAtlasCustomRect* self);

		// Token: 0x06000736 RID: 1846
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImFontAtlasCustomRect* ImFontAtlasCustomRect_ImFontAtlasCustomRect();

		// Token: 0x06000737 RID: 1847
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte ImFontAtlasCustomRect_IsPacked(ImFontAtlasCustomRect* self);

		// Token: 0x06000738 RID: 1848
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFontConfig_destroy(ImFontConfig* self);

		// Token: 0x06000739 RID: 1849
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImFontConfig* ImFontConfig_ImFontConfig();

		// Token: 0x0600073A RID: 1850
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFontGlyphRangesBuilder_AddChar(ImFontGlyphRangesBuilder* self, ushort c);

		// Token: 0x0600073B RID: 1851
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFontGlyphRangesBuilder_AddRanges(ImFontGlyphRangesBuilder* self, ushort* ranges);

		// Token: 0x0600073C RID: 1852
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFontGlyphRangesBuilder_AddText(ImFontGlyphRangesBuilder* self, byte* text, byte* text_end);

		// Token: 0x0600073D RID: 1853
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFontGlyphRangesBuilder_BuildRanges(ImFontGlyphRangesBuilder* self, ImVector* out_ranges);

		// Token: 0x0600073E RID: 1854
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFontGlyphRangesBuilder_Clear(ImFontGlyphRangesBuilder* self);

		// Token: 0x0600073F RID: 1855
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFontGlyphRangesBuilder_destroy(ImFontGlyphRangesBuilder* self);

		// Token: 0x06000740 RID: 1856
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte ImFontGlyphRangesBuilder_GetBit(ImFontGlyphRangesBuilder* self, uint n);

		// Token: 0x06000741 RID: 1857
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImFontGlyphRangesBuilder* ImFontGlyphRangesBuilder_ImFontGlyphRangesBuilder();

		// Token: 0x06000742 RID: 1858
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImFontGlyphRangesBuilder_SetBit(ImFontGlyphRangesBuilder* self, uint n);

		// Token: 0x06000743 RID: 1859
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiInputTextCallbackData_ClearSelection(ImGuiInputTextCallbackData* self);

		// Token: 0x06000744 RID: 1860
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiInputTextCallbackData_DeleteChars(ImGuiInputTextCallbackData* self, int pos, int bytes_count);

		// Token: 0x06000745 RID: 1861
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiInputTextCallbackData_destroy(ImGuiInputTextCallbackData* self);

		// Token: 0x06000746 RID: 1862
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte ImGuiInputTextCallbackData_HasSelection(ImGuiInputTextCallbackData* self);

		// Token: 0x06000747 RID: 1863
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiInputTextCallbackData* ImGuiInputTextCallbackData_ImGuiInputTextCallbackData();

		// Token: 0x06000748 RID: 1864
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiInputTextCallbackData_InsertChars(ImGuiInputTextCallbackData* self, int pos, byte* text, byte* text_end);

		// Token: 0x06000749 RID: 1865
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiInputTextCallbackData_SelectAll(ImGuiInputTextCallbackData* self);

		// Token: 0x0600074A RID: 1866
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiIO_AddFocusEvent(ImGuiIO* self, byte focused);

		// Token: 0x0600074B RID: 1867
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiIO_AddInputCharacter(ImGuiIO* self, uint c);

		// Token: 0x0600074C RID: 1868
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiIO_AddInputCharactersUTF8(ImGuiIO* self, byte* str);

		// Token: 0x0600074D RID: 1869
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiIO_AddInputCharacterUTF16(ImGuiIO* self, ushort c);

		// Token: 0x0600074E RID: 1870
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiIO_AddKeyAnalogEvent(ImGuiIO* self, ImGuiKey key, byte down, float v);

		// Token: 0x0600074F RID: 1871
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiIO_AddKeyEvent(ImGuiIO* self, ImGuiKey key, byte down);

		// Token: 0x06000750 RID: 1872
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiIO_AddMouseButtonEvent(ImGuiIO* self, int button, byte down);

		// Token: 0x06000751 RID: 1873
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiIO_AddMousePosEvent(ImGuiIO* self, float x, float y);

		// Token: 0x06000752 RID: 1874
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiIO_AddMouseSourceEvent(ImGuiIO* self, ImGuiMouseSource source);

		// Token: 0x06000753 RID: 1875
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiIO_AddMouseViewportEvent(ImGuiIO* self, uint id);

		// Token: 0x06000754 RID: 1876
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiIO_AddMouseWheelEvent(ImGuiIO* self, float wheel_x, float wheel_y);

		// Token: 0x06000755 RID: 1877
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiIO_ClearEventsQueue(ImGuiIO* self);

		// Token: 0x06000756 RID: 1878
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiIO_ClearInputKeys(ImGuiIO* self);

		// Token: 0x06000757 RID: 1879
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiIO_destroy(ImGuiIO* self);

		// Token: 0x06000758 RID: 1880
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiIO* ImGuiIO_ImGuiIO();

		// Token: 0x06000759 RID: 1881
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiIO_SetAppAcceptingEvents(ImGuiIO* self, byte accepting_events);

		// Token: 0x0600075A RID: 1882
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiIO_SetKeyEventNativeData(ImGuiIO* self, ImGuiKey key, int native_keycode, int native_scancode, int native_legacy_index);

		// Token: 0x0600075B RID: 1883
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiListClipper_Begin(ImGuiListClipper* self, int items_count, float items_height);

		// Token: 0x0600075C RID: 1884
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiListClipper_destroy(ImGuiListClipper* self);

		// Token: 0x0600075D RID: 1885
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiListClipper_End(ImGuiListClipper* self);

		// Token: 0x0600075E RID: 1886
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiListClipper* ImGuiListClipper_ImGuiListClipper();

		// Token: 0x0600075F RID: 1887
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiListClipper_IncludeItemByIndex(ImGuiListClipper* self, int item_index);

		// Token: 0x06000760 RID: 1888
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiListClipper_IncludeItemsByIndex(ImGuiListClipper* self, int item_begin, int item_end);

		// Token: 0x06000761 RID: 1889
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte ImGuiListClipper_Step(ImGuiListClipper* self);

		// Token: 0x06000762 RID: 1890
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiOnceUponAFrame_destroy(ImGuiOnceUponAFrame* self);

		// Token: 0x06000763 RID: 1891
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiOnceUponAFrame* ImGuiOnceUponAFrame_ImGuiOnceUponAFrame();

		// Token: 0x06000764 RID: 1892
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiPayload_Clear(ImGuiPayload* self);

		// Token: 0x06000765 RID: 1893
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiPayload_destroy(ImGuiPayload* self);

		// Token: 0x06000766 RID: 1894
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiPayload* ImGuiPayload_ImGuiPayload();

		// Token: 0x06000767 RID: 1895
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte ImGuiPayload_IsDataType(ImGuiPayload* self, byte* type);

		// Token: 0x06000768 RID: 1896
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte ImGuiPayload_IsDelivery(ImGuiPayload* self);

		// Token: 0x06000769 RID: 1897
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte ImGuiPayload_IsPreview(ImGuiPayload* self);

		// Token: 0x0600076A RID: 1898
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiPlatformImeData_destroy(ImGuiPlatformImeData* self);

		// Token: 0x0600076B RID: 1899
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiPlatformImeData* ImGuiPlatformImeData_ImGuiPlatformImeData();

		// Token: 0x0600076C RID: 1900
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiPlatformIO_destroy(ImGuiPlatformIO* self);

		// Token: 0x0600076D RID: 1901
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiPlatformIO* ImGuiPlatformIO_ImGuiPlatformIO();

		// Token: 0x0600076E RID: 1902
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiPlatformMonitor_destroy(ImGuiPlatformMonitor* self);

		// Token: 0x0600076F RID: 1903
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiPlatformMonitor* ImGuiPlatformMonitor_ImGuiPlatformMonitor();

		// Token: 0x06000770 RID: 1904
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiStorage_BuildSortByKey(ImGuiStorage* self);

		// Token: 0x06000771 RID: 1905
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiStorage_Clear(ImGuiStorage* self);

		// Token: 0x06000772 RID: 1906
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte ImGuiStorage_GetBool(ImGuiStorage* self, uint key, byte default_val);

		// Token: 0x06000773 RID: 1907
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte* ImGuiStorage_GetBoolRef(ImGuiStorage* self, uint key, byte default_val);

		// Token: 0x06000774 RID: 1908
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern float ImGuiStorage_GetFloat(ImGuiStorage* self, uint key, float default_val);

		// Token: 0x06000775 RID: 1909
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern float* ImGuiStorage_GetFloatRef(ImGuiStorage* self, uint key, float default_val);

		// Token: 0x06000776 RID: 1910
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern int ImGuiStorage_GetInt(ImGuiStorage* self, uint key, int default_val);

		// Token: 0x06000777 RID: 1911
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern int* ImGuiStorage_GetIntRef(ImGuiStorage* self, uint key, int default_val);

		// Token: 0x06000778 RID: 1912
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void* ImGuiStorage_GetVoidPtr(ImGuiStorage* self, uint key);

		// Token: 0x06000779 RID: 1913
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void** ImGuiStorage_GetVoidPtrRef(ImGuiStorage* self, uint key, void* default_val);

		// Token: 0x0600077A RID: 1914
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiStorage_SetAllInt(ImGuiStorage* self, int val);

		// Token: 0x0600077B RID: 1915
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiStorage_SetBool(ImGuiStorage* self, uint key, byte val);

		// Token: 0x0600077C RID: 1916
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiStorage_SetFloat(ImGuiStorage* self, uint key, float val);

		// Token: 0x0600077D RID: 1917
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiStorage_SetInt(ImGuiStorage* self, uint key, int val);

		// Token: 0x0600077E RID: 1918
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiStorage_SetVoidPtr(ImGuiStorage* self, uint key, void* val);

		// Token: 0x0600077F RID: 1919
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiStoragePair_destroy(ImGuiStoragePair* self);

		// Token: 0x06000780 RID: 1920
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiStoragePair* ImGuiStoragePair_ImGuiStoragePair_Int(uint _key, int _val_i);

		// Token: 0x06000781 RID: 1921
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiStoragePair* ImGuiStoragePair_ImGuiStoragePair_Float(uint _key, float _val_f);

		// Token: 0x06000782 RID: 1922
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiStoragePair* ImGuiStoragePair_ImGuiStoragePair_Ptr(uint _key, void* _val_p);

		// Token: 0x06000783 RID: 1923
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiStyle_destroy(ImGuiStyle* self);

		// Token: 0x06000784 RID: 1924
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiStyle* ImGuiStyle_ImGuiStyle();

		// Token: 0x06000785 RID: 1925
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiStyle_ScaleAllSizes(ImGuiStyle* self, float scale_factor);

		// Token: 0x06000786 RID: 1926
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiTableColumnSortSpecs_destroy(ImGuiTableColumnSortSpecs* self);

		// Token: 0x06000787 RID: 1927
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiTableColumnSortSpecs* ImGuiTableColumnSortSpecs_ImGuiTableColumnSortSpecs();

		// Token: 0x06000788 RID: 1928
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiTableSortSpecs_destroy(ImGuiTableSortSpecs* self);

		// Token: 0x06000789 RID: 1929
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiTableSortSpecs* ImGuiTableSortSpecs_ImGuiTableSortSpecs();

		// Token: 0x0600078A RID: 1930
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiTextBuffer_append(ImGuiTextBuffer* self, byte* str, byte* str_end);

		// Token: 0x0600078B RID: 1931
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiTextBuffer_appendf(ImGuiTextBuffer* self, byte* fmt);

		// Token: 0x0600078C RID: 1932
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte* ImGuiTextBuffer_begin(ImGuiTextBuffer* self);

		// Token: 0x0600078D RID: 1933
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte* ImGuiTextBuffer_c_str(ImGuiTextBuffer* self);

		// Token: 0x0600078E RID: 1934
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiTextBuffer_clear(ImGuiTextBuffer* self);

		// Token: 0x0600078F RID: 1935
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiTextBuffer_destroy(ImGuiTextBuffer* self);

		// Token: 0x06000790 RID: 1936
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte ImGuiTextBuffer_empty(ImGuiTextBuffer* self);

		// Token: 0x06000791 RID: 1937
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte* ImGuiTextBuffer_end(ImGuiTextBuffer* self);

		// Token: 0x06000792 RID: 1938
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiTextBuffer* ImGuiTextBuffer_ImGuiTextBuffer();

		// Token: 0x06000793 RID: 1939
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiTextBuffer_reserve(ImGuiTextBuffer* self, int capacity);

		// Token: 0x06000794 RID: 1940
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern int ImGuiTextBuffer_size(ImGuiTextBuffer* self);

		// Token: 0x06000795 RID: 1941
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiTextFilter_Build(ImGuiTextFilter* self);

		// Token: 0x06000796 RID: 1942
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiTextFilter_Clear(ImGuiTextFilter* self);

		// Token: 0x06000797 RID: 1943
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiTextFilter_destroy(ImGuiTextFilter* self);

		// Token: 0x06000798 RID: 1944
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte ImGuiTextFilter_Draw(ImGuiTextFilter* self, byte* label, float width);

		// Token: 0x06000799 RID: 1945
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiTextFilter* ImGuiTextFilter_ImGuiTextFilter(byte* default_filter);

		// Token: 0x0600079A RID: 1946
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte ImGuiTextFilter_IsActive(ImGuiTextFilter* self);

		// Token: 0x0600079B RID: 1947
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte ImGuiTextFilter_PassFilter(ImGuiTextFilter* self, byte* text, byte* text_end);

		// Token: 0x0600079C RID: 1948
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiTextRange_destroy(ImGuiTextRange* self);

		// Token: 0x0600079D RID: 1949
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern byte ImGuiTextRange_empty(ImGuiTextRange* self);

		// Token: 0x0600079E RID: 1950
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiTextRange* ImGuiTextRange_ImGuiTextRange_Nil();

		// Token: 0x0600079F RID: 1951
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiTextRange* ImGuiTextRange_ImGuiTextRange_Str(byte* _b, byte* _e);

		// Token: 0x060007A0 RID: 1952
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiTextRange_split(ImGuiTextRange* self, byte separator, ImVector* @out);

		// Token: 0x060007A1 RID: 1953
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiViewport_destroy(ImGuiViewport* self);

		// Token: 0x060007A2 RID: 1954
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiViewport_GetCenter(Vector2* pOut, ImGuiViewport* self);

		// Token: 0x060007A3 RID: 1955
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiViewport_GetWorkCenter(Vector2* pOut, ImGuiViewport* self);

		// Token: 0x060007A4 RID: 1956
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiViewport* ImGuiViewport_ImGuiViewport();

		// Token: 0x060007A5 RID: 1957
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiWindowClass_destroy(ImGuiWindowClass* self);

		// Token: 0x060007A6 RID: 1958
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern ImGuiWindowClass* ImGuiWindowClass_ImGuiWindowClass();

		// Token: 0x060007A7 RID: 1959
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImVec2_destroy(Vector2* self);

		// Token: 0x060007A8 RID: 1960
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern Vector2* ImVec2_ImVec2_Nil();

		// Token: 0x060007A9 RID: 1961
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern Vector2* ImVec2_ImVec2_Float(float _x, float _y);

		// Token: 0x060007AA RID: 1962
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImVec4_destroy(Vector4* self);

		// Token: 0x060007AB RID: 1963
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern Vector4* ImVec4_ImVec4_Nil();

		// Token: 0x060007AC RID: 1964
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern Vector4* ImVec4_ImVec4_Float(float _x, float _y, float _z, float _w);

		// Token: 0x060007AD RID: 1965
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiPlatformIO_Set_Platform_GetWindowPos(ImGuiPlatformIO* platform_io, IntPtr funcPtr);

		// Token: 0x060007AE RID: 1966
		[DllImport("cimgui", CallingConvention = 2)]
		public unsafe static extern void ImGuiPlatformIO_Set_Platform_GetWindowSize(ImGuiPlatformIO* platform_io, IntPtr funcPtr);
	}
}
