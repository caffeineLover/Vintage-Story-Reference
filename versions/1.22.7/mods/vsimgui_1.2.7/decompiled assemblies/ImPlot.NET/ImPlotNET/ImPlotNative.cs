using System;
using System.Numerics;
using System.Runtime.InteropServices;
using ImGuiNET;

namespace ImPlotNET
{
	// Token: 0x0200001D RID: 29
	public static class ImPlotNative
	{
		// Token: 0x06000542 RID: 1346
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern ImPlotColormap ImPlot_AddColormap_Vec4Ptr(byte* name, Vector4* cols, int size, byte qual);

		// Token: 0x06000543 RID: 1347
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern ImPlotColormap ImPlot_AddColormap_U32Ptr(byte* name, uint* cols, int size, byte qual);

		// Token: 0x06000544 RID: 1348
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_Annotation_Bool(double x, double y, Vector4 col, Vector2 pix_offset, byte clamp, byte round);

		// Token: 0x06000545 RID: 1349
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_Annotation_Str(double x, double y, Vector4 col, Vector2 pix_offset, byte clamp, byte* fmt);

		// Token: 0x06000546 RID: 1350
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern byte ImPlot_BeginAlignedPlots(byte* group_id, byte vertical);

		// Token: 0x06000547 RID: 1351
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern byte ImPlot_BeginDragDropSourceAxis(ImAxis axis, ImGuiDragDropFlags flags);

		// Token: 0x06000548 RID: 1352
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern byte ImPlot_BeginDragDropSourceItem(byte* label_id, ImGuiDragDropFlags flags);

		// Token: 0x06000549 RID: 1353
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern byte ImPlot_BeginDragDropSourcePlot(ImGuiDragDropFlags flags);

		// Token: 0x0600054A RID: 1354
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern byte ImPlot_BeginDragDropTargetAxis(ImAxis axis);

		// Token: 0x0600054B RID: 1355
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern byte ImPlot_BeginDragDropTargetLegend();

		// Token: 0x0600054C RID: 1356
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern byte ImPlot_BeginDragDropTargetPlot();

		// Token: 0x0600054D RID: 1357
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern byte ImPlot_BeginLegendPopup(byte* label_id, ImGuiMouseButton mouse_button);

		// Token: 0x0600054E RID: 1358
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern byte ImPlot_BeginPlot(byte* title_id, Vector2 size, ImPlotFlags flags);

		// Token: 0x0600054F RID: 1359
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern byte ImPlot_BeginSubplots(byte* title_id, int rows, int cols, Vector2 size, ImPlotSubplotFlags flags, float* row_ratios, float* col_ratios);

		// Token: 0x06000550 RID: 1360
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_BustColorCache(byte* plot_title_id);

		// Token: 0x06000551 RID: 1361
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_CancelPlotSelection();

		// Token: 0x06000552 RID: 1362
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern byte ImPlot_ColormapButton(byte* label, Vector2 size, ImPlotColormap cmap);

		// Token: 0x06000553 RID: 1363
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_ColormapIcon(ImPlotColormap cmap);

		// Token: 0x06000554 RID: 1364
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_ColormapScale(byte* label, double scale_min, double scale_max, Vector2 size, byte* format, ImPlotColormapScaleFlags flags, ImPlotColormap cmap);

		// Token: 0x06000555 RID: 1365
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern byte ImPlot_ColormapSlider(byte* label, float* t, Vector4* @out, byte* format, ImPlotColormap cmap);

		// Token: 0x06000556 RID: 1366
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern IntPtr ImPlot_CreateContext();

		// Token: 0x06000557 RID: 1367
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_DestroyContext(IntPtr ctx);

		// Token: 0x06000558 RID: 1368
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern byte ImPlot_DragLineX(int id, double* x, Vector4 col, float thickness, ImPlotDragToolFlags flags);

		// Token: 0x06000559 RID: 1369
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern byte ImPlot_DragLineY(int id, double* y, Vector4 col, float thickness, ImPlotDragToolFlags flags);

		// Token: 0x0600055A RID: 1370
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern byte ImPlot_DragPoint(int id, double* x, double* y, Vector4 col, float size, ImPlotDragToolFlags flags);

		// Token: 0x0600055B RID: 1371
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern byte ImPlot_DragRect(int id, double* x1, double* y1, double* x2, double* y2, Vector4 col, ImPlotDragToolFlags flags);

		// Token: 0x0600055C RID: 1372
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_EndAlignedPlots();

		// Token: 0x0600055D RID: 1373
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_EndDragDropSource();

		// Token: 0x0600055E RID: 1374
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_EndDragDropTarget();

		// Token: 0x0600055F RID: 1375
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_EndLegendPopup();

		// Token: 0x06000560 RID: 1376
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_EndPlot();

		// Token: 0x06000561 RID: 1377
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_EndSubplots();

		// Token: 0x06000562 RID: 1378
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_GetColormapColor(Vector4* pOut, int idx, ImPlotColormap cmap);

		// Token: 0x06000563 RID: 1379
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern int ImPlot_GetColormapCount();

		// Token: 0x06000564 RID: 1380
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern ImPlotColormap ImPlot_GetColormapIndex(byte* name);

		// Token: 0x06000565 RID: 1381
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern byte* ImPlot_GetColormapName(ImPlotColormap cmap);

		// Token: 0x06000566 RID: 1382
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern int ImPlot_GetColormapSize(ImPlotColormap cmap);

		// Token: 0x06000567 RID: 1383
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern IntPtr ImPlot_GetCurrentContext();

		// Token: 0x06000568 RID: 1384
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern ImPlotInputMap* ImPlot_GetInputMap();

		// Token: 0x06000569 RID: 1385
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_GetLastItemColor(Vector4* pOut);

		// Token: 0x0600056A RID: 1386
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern byte* ImPlot_GetMarkerName(ImPlotMarker idx);

		// Token: 0x0600056B RID: 1387
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern ImDrawList* ImPlot_GetPlotDrawList();

		// Token: 0x0600056C RID: 1388
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern ImPlotRect ImPlot_GetPlotLimits(ImAxis x_axis, ImAxis y_axis);

		// Token: 0x0600056D RID: 1389
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_GetPlotMousePos(ImPlotPoint* pOut, ImAxis x_axis, ImAxis y_axis);

		// Token: 0x0600056E RID: 1390
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_GetPlotPos(Vector2* pOut);

		// Token: 0x0600056F RID: 1391
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern ImPlotRect ImPlot_GetPlotSelection(ImAxis x_axis, ImAxis y_axis);

		// Token: 0x06000570 RID: 1392
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_GetPlotSize(Vector2* pOut);

		// Token: 0x06000571 RID: 1393
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern ImPlotStyle* ImPlot_GetStyle();

		// Token: 0x06000572 RID: 1394
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern byte* ImPlot_GetStyleColorName(ImPlotCol idx);

		// Token: 0x06000573 RID: 1395
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_HideNextItem(byte hidden, ImPlotCond cond);

		// Token: 0x06000574 RID: 1396
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern byte ImPlot_IsAxisHovered(ImAxis axis);

		// Token: 0x06000575 RID: 1397
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern byte ImPlot_IsLegendEntryHovered(byte* label_id);

		// Token: 0x06000576 RID: 1398
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern byte ImPlot_IsPlotHovered();

		// Token: 0x06000577 RID: 1399
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern byte ImPlot_IsPlotSelected();

		// Token: 0x06000578 RID: 1400
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern byte ImPlot_IsSubplotsHovered();

		// Token: 0x06000579 RID: 1401
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_ItemIcon_Vec4(Vector4 col);

		// Token: 0x0600057A RID: 1402
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_ItemIcon_U32(uint col);

		// Token: 0x0600057B RID: 1403
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_MapInputDefault(ImPlotInputMap* dst);

		// Token: 0x0600057C RID: 1404
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_MapInputReverse(ImPlotInputMap* dst);

		// Token: 0x0600057D RID: 1405
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_NextColormapColor(Vector4* pOut);

		// Token: 0x0600057E RID: 1406
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PixelsToPlot_Vec2(ImPlotPoint* pOut, Vector2 pix, ImAxis x_axis, ImAxis y_axis);

		// Token: 0x0600057F RID: 1407
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PixelsToPlot_Float(ImPlotPoint* pOut, float x, float y, ImAxis x_axis, ImAxis y_axis);

		// Token: 0x06000580 RID: 1408
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBarGroups_FloatPtr(byte** label_ids, float* values, int item_count, int group_count, double group_size, double shift, ImPlotBarGroupsFlags flags);

		// Token: 0x06000581 RID: 1409
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBarGroups_doublePtr(byte** label_ids, double* values, int item_count, int group_count, double group_size, double shift, ImPlotBarGroupsFlags flags);

		// Token: 0x06000582 RID: 1410
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBarGroups_S8Ptr(byte** label_ids, sbyte* values, int item_count, int group_count, double group_size, double shift, ImPlotBarGroupsFlags flags);

		// Token: 0x06000583 RID: 1411
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBarGroups_U8Ptr(byte** label_ids, byte* values, int item_count, int group_count, double group_size, double shift, ImPlotBarGroupsFlags flags);

		// Token: 0x06000584 RID: 1412
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBarGroups_S16Ptr(byte** label_ids, short* values, int item_count, int group_count, double group_size, double shift, ImPlotBarGroupsFlags flags);

		// Token: 0x06000585 RID: 1413
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBarGroups_U16Ptr(byte** label_ids, ushort* values, int item_count, int group_count, double group_size, double shift, ImPlotBarGroupsFlags flags);

		// Token: 0x06000586 RID: 1414
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBarGroups_S32Ptr(byte** label_ids, int* values, int item_count, int group_count, double group_size, double shift, ImPlotBarGroupsFlags flags);

		// Token: 0x06000587 RID: 1415
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBarGroups_U32Ptr(byte** label_ids, uint* values, int item_count, int group_count, double group_size, double shift, ImPlotBarGroupsFlags flags);

		// Token: 0x06000588 RID: 1416
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBarGroups_S64Ptr(byte** label_ids, long* values, int item_count, int group_count, double group_size, double shift, ImPlotBarGroupsFlags flags);

		// Token: 0x06000589 RID: 1417
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBarGroups_U64Ptr(byte** label_ids, ulong* values, int item_count, int group_count, double group_size, double shift, ImPlotBarGroupsFlags flags);

		// Token: 0x0600058A RID: 1418
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBars_FloatPtrInt(byte* label_id, float* values, int count, double bar_size, double shift, ImPlotBarsFlags flags, int offset, int stride);

		// Token: 0x0600058B RID: 1419
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBars_doublePtrInt(byte* label_id, double* values, int count, double bar_size, double shift, ImPlotBarsFlags flags, int offset, int stride);

		// Token: 0x0600058C RID: 1420
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBars_S8PtrInt(byte* label_id, sbyte* values, int count, double bar_size, double shift, ImPlotBarsFlags flags, int offset, int stride);

		// Token: 0x0600058D RID: 1421
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBars_U8PtrInt(byte* label_id, byte* values, int count, double bar_size, double shift, ImPlotBarsFlags flags, int offset, int stride);

		// Token: 0x0600058E RID: 1422
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBars_S16PtrInt(byte* label_id, short* values, int count, double bar_size, double shift, ImPlotBarsFlags flags, int offset, int stride);

		// Token: 0x0600058F RID: 1423
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBars_U16PtrInt(byte* label_id, ushort* values, int count, double bar_size, double shift, ImPlotBarsFlags flags, int offset, int stride);

		// Token: 0x06000590 RID: 1424
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBars_S32PtrInt(byte* label_id, int* values, int count, double bar_size, double shift, ImPlotBarsFlags flags, int offset, int stride);

		// Token: 0x06000591 RID: 1425
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBars_U32PtrInt(byte* label_id, uint* values, int count, double bar_size, double shift, ImPlotBarsFlags flags, int offset, int stride);

		// Token: 0x06000592 RID: 1426
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBars_S64PtrInt(byte* label_id, long* values, int count, double bar_size, double shift, ImPlotBarsFlags flags, int offset, int stride);

		// Token: 0x06000593 RID: 1427
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBars_U64PtrInt(byte* label_id, ulong* values, int count, double bar_size, double shift, ImPlotBarsFlags flags, int offset, int stride);

		// Token: 0x06000594 RID: 1428
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBars_FloatPtrFloatPtr(byte* label_id, float* xs, float* ys, int count, double bar_size, ImPlotBarsFlags flags, int offset, int stride);

		// Token: 0x06000595 RID: 1429
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBars_doublePtrdoublePtr(byte* label_id, double* xs, double* ys, int count, double bar_size, ImPlotBarsFlags flags, int offset, int stride);

		// Token: 0x06000596 RID: 1430
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBars_S8PtrS8Ptr(byte* label_id, sbyte* xs, sbyte* ys, int count, double bar_size, ImPlotBarsFlags flags, int offset, int stride);

		// Token: 0x06000597 RID: 1431
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBars_U8PtrU8Ptr(byte* label_id, byte* xs, byte* ys, int count, double bar_size, ImPlotBarsFlags flags, int offset, int stride);

		// Token: 0x06000598 RID: 1432
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBars_S16PtrS16Ptr(byte* label_id, short* xs, short* ys, int count, double bar_size, ImPlotBarsFlags flags, int offset, int stride);

		// Token: 0x06000599 RID: 1433
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBars_U16PtrU16Ptr(byte* label_id, ushort* xs, ushort* ys, int count, double bar_size, ImPlotBarsFlags flags, int offset, int stride);

		// Token: 0x0600059A RID: 1434
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBars_S32PtrS32Ptr(byte* label_id, int* xs, int* ys, int count, double bar_size, ImPlotBarsFlags flags, int offset, int stride);

		// Token: 0x0600059B RID: 1435
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBars_U32PtrU32Ptr(byte* label_id, uint* xs, uint* ys, int count, double bar_size, ImPlotBarsFlags flags, int offset, int stride);

		// Token: 0x0600059C RID: 1436
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBars_S64PtrS64Ptr(byte* label_id, long* xs, long* ys, int count, double bar_size, ImPlotBarsFlags flags, int offset, int stride);

		// Token: 0x0600059D RID: 1437
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBars_U64PtrU64Ptr(byte* label_id, ulong* xs, ulong* ys, int count, double bar_size, ImPlotBarsFlags flags, int offset, int stride);

		// Token: 0x0600059E RID: 1438
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotBarsG(byte* label_id, IntPtr getter, void* data, int count, double bar_size, ImPlotBarsFlags flags);

		// Token: 0x0600059F RID: 1439
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotDigital_FloatPtr(byte* label_id, float* xs, float* ys, int count, ImPlotDigitalFlags flags, int offset, int stride);

		// Token: 0x060005A0 RID: 1440
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotDigital_doublePtr(byte* label_id, double* xs, double* ys, int count, ImPlotDigitalFlags flags, int offset, int stride);

		// Token: 0x060005A1 RID: 1441
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotDigital_S8Ptr(byte* label_id, sbyte* xs, sbyte* ys, int count, ImPlotDigitalFlags flags, int offset, int stride);

		// Token: 0x060005A2 RID: 1442
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotDigital_U8Ptr(byte* label_id, byte* xs, byte* ys, int count, ImPlotDigitalFlags flags, int offset, int stride);

		// Token: 0x060005A3 RID: 1443
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotDigital_S16Ptr(byte* label_id, short* xs, short* ys, int count, ImPlotDigitalFlags flags, int offset, int stride);

		// Token: 0x060005A4 RID: 1444
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotDigital_U16Ptr(byte* label_id, ushort* xs, ushort* ys, int count, ImPlotDigitalFlags flags, int offset, int stride);

		// Token: 0x060005A5 RID: 1445
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotDigital_S32Ptr(byte* label_id, int* xs, int* ys, int count, ImPlotDigitalFlags flags, int offset, int stride);

		// Token: 0x060005A6 RID: 1446
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotDigital_U32Ptr(byte* label_id, uint* xs, uint* ys, int count, ImPlotDigitalFlags flags, int offset, int stride);

		// Token: 0x060005A7 RID: 1447
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotDigital_S64Ptr(byte* label_id, long* xs, long* ys, int count, ImPlotDigitalFlags flags, int offset, int stride);

		// Token: 0x060005A8 RID: 1448
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotDigital_U64Ptr(byte* label_id, ulong* xs, ulong* ys, int count, ImPlotDigitalFlags flags, int offset, int stride);

		// Token: 0x060005A9 RID: 1449
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotDigitalG(byte* label_id, IntPtr getter, void* data, int count, ImPlotDigitalFlags flags);

		// Token: 0x060005AA RID: 1450
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotDummy(byte* label_id, ImPlotDummyFlags flags);

		// Token: 0x060005AB RID: 1451
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotErrorBars_FloatPtrFloatPtrFloatPtrInt(byte* label_id, float* xs, float* ys, float* err, int count, ImPlotErrorBarsFlags flags, int offset, int stride);

		// Token: 0x060005AC RID: 1452
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotErrorBars_doublePtrdoublePtrdoublePtrInt(byte* label_id, double* xs, double* ys, double* err, int count, ImPlotErrorBarsFlags flags, int offset, int stride);

		// Token: 0x060005AD RID: 1453
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotErrorBars_S8PtrS8PtrS8PtrInt(byte* label_id, sbyte* xs, sbyte* ys, sbyte* err, int count, ImPlotErrorBarsFlags flags, int offset, int stride);

		// Token: 0x060005AE RID: 1454
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotErrorBars_U8PtrU8PtrU8PtrInt(byte* label_id, byte* xs, byte* ys, byte* err, int count, ImPlotErrorBarsFlags flags, int offset, int stride);

		// Token: 0x060005AF RID: 1455
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotErrorBars_S16PtrS16PtrS16PtrInt(byte* label_id, short* xs, short* ys, short* err, int count, ImPlotErrorBarsFlags flags, int offset, int stride);

		// Token: 0x060005B0 RID: 1456
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotErrorBars_U16PtrU16PtrU16PtrInt(byte* label_id, ushort* xs, ushort* ys, ushort* err, int count, ImPlotErrorBarsFlags flags, int offset, int stride);

		// Token: 0x060005B1 RID: 1457
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotErrorBars_S32PtrS32PtrS32PtrInt(byte* label_id, int* xs, int* ys, int* err, int count, ImPlotErrorBarsFlags flags, int offset, int stride);

		// Token: 0x060005B2 RID: 1458
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotErrorBars_U32PtrU32PtrU32PtrInt(byte* label_id, uint* xs, uint* ys, uint* err, int count, ImPlotErrorBarsFlags flags, int offset, int stride);

		// Token: 0x060005B3 RID: 1459
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotErrorBars_S64PtrS64PtrS64PtrInt(byte* label_id, long* xs, long* ys, long* err, int count, ImPlotErrorBarsFlags flags, int offset, int stride);

		// Token: 0x060005B4 RID: 1460
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotErrorBars_U64PtrU64PtrU64PtrInt(byte* label_id, ulong* xs, ulong* ys, ulong* err, int count, ImPlotErrorBarsFlags flags, int offset, int stride);

		// Token: 0x060005B5 RID: 1461
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotErrorBars_FloatPtrFloatPtrFloatPtrFloatPtr(byte* label_id, float* xs, float* ys, float* neg, float* pos, int count, ImPlotErrorBarsFlags flags, int offset, int stride);

		// Token: 0x060005B6 RID: 1462
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotErrorBars_doublePtrdoublePtrdoublePtrdoublePtr(byte* label_id, double* xs, double* ys, double* neg, double* pos, int count, ImPlotErrorBarsFlags flags, int offset, int stride);

		// Token: 0x060005B7 RID: 1463
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotErrorBars_S8PtrS8PtrS8PtrS8Ptr(byte* label_id, sbyte* xs, sbyte* ys, sbyte* neg, sbyte* pos, int count, ImPlotErrorBarsFlags flags, int offset, int stride);

		// Token: 0x060005B8 RID: 1464
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotErrorBars_U8PtrU8PtrU8PtrU8Ptr(byte* label_id, byte* xs, byte* ys, byte* neg, byte* pos, int count, ImPlotErrorBarsFlags flags, int offset, int stride);

		// Token: 0x060005B9 RID: 1465
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotErrorBars_S16PtrS16PtrS16PtrS16Ptr(byte* label_id, short* xs, short* ys, short* neg, short* pos, int count, ImPlotErrorBarsFlags flags, int offset, int stride);

		// Token: 0x060005BA RID: 1466
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotErrorBars_U16PtrU16PtrU16PtrU16Ptr(byte* label_id, ushort* xs, ushort* ys, ushort* neg, ushort* pos, int count, ImPlotErrorBarsFlags flags, int offset, int stride);

		// Token: 0x060005BB RID: 1467
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotErrorBars_S32PtrS32PtrS32PtrS32Ptr(byte* label_id, int* xs, int* ys, int* neg, int* pos, int count, ImPlotErrorBarsFlags flags, int offset, int stride);

		// Token: 0x060005BC RID: 1468
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotErrorBars_U32PtrU32PtrU32PtrU32Ptr(byte* label_id, uint* xs, uint* ys, uint* neg, uint* pos, int count, ImPlotErrorBarsFlags flags, int offset, int stride);

		// Token: 0x060005BD RID: 1469
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotErrorBars_S64PtrS64PtrS64PtrS64Ptr(byte* label_id, long* xs, long* ys, long* neg, long* pos, int count, ImPlotErrorBarsFlags flags, int offset, int stride);

		// Token: 0x060005BE RID: 1470
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotErrorBars_U64PtrU64PtrU64PtrU64Ptr(byte* label_id, ulong* xs, ulong* ys, ulong* neg, ulong* pos, int count, ImPlotErrorBarsFlags flags, int offset, int stride);

		// Token: 0x060005BF RID: 1471
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotHeatmap_FloatPtr(byte* label_id, float* values, int rows, int cols, double scale_min, double scale_max, byte* label_fmt, ImPlotPoint bounds_min, ImPlotPoint bounds_max, ImPlotHeatmapFlags flags);

		// Token: 0x060005C0 RID: 1472
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotHeatmap_doublePtr(byte* label_id, double* values, int rows, int cols, double scale_min, double scale_max, byte* label_fmt, ImPlotPoint bounds_min, ImPlotPoint bounds_max, ImPlotHeatmapFlags flags);

		// Token: 0x060005C1 RID: 1473
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotHeatmap_S8Ptr(byte* label_id, sbyte* values, int rows, int cols, double scale_min, double scale_max, byte* label_fmt, ImPlotPoint bounds_min, ImPlotPoint bounds_max, ImPlotHeatmapFlags flags);

		// Token: 0x060005C2 RID: 1474
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotHeatmap_U8Ptr(byte* label_id, byte* values, int rows, int cols, double scale_min, double scale_max, byte* label_fmt, ImPlotPoint bounds_min, ImPlotPoint bounds_max, ImPlotHeatmapFlags flags);

		// Token: 0x060005C3 RID: 1475
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotHeatmap_S16Ptr(byte* label_id, short* values, int rows, int cols, double scale_min, double scale_max, byte* label_fmt, ImPlotPoint bounds_min, ImPlotPoint bounds_max, ImPlotHeatmapFlags flags);

		// Token: 0x060005C4 RID: 1476
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotHeatmap_U16Ptr(byte* label_id, ushort* values, int rows, int cols, double scale_min, double scale_max, byte* label_fmt, ImPlotPoint bounds_min, ImPlotPoint bounds_max, ImPlotHeatmapFlags flags);

		// Token: 0x060005C5 RID: 1477
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotHeatmap_S32Ptr(byte* label_id, int* values, int rows, int cols, double scale_min, double scale_max, byte* label_fmt, ImPlotPoint bounds_min, ImPlotPoint bounds_max, ImPlotHeatmapFlags flags);

		// Token: 0x060005C6 RID: 1478
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotHeatmap_U32Ptr(byte* label_id, uint* values, int rows, int cols, double scale_min, double scale_max, byte* label_fmt, ImPlotPoint bounds_min, ImPlotPoint bounds_max, ImPlotHeatmapFlags flags);

		// Token: 0x060005C7 RID: 1479
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotHeatmap_S64Ptr(byte* label_id, long* values, int rows, int cols, double scale_min, double scale_max, byte* label_fmt, ImPlotPoint bounds_min, ImPlotPoint bounds_max, ImPlotHeatmapFlags flags);

		// Token: 0x060005C8 RID: 1480
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotHeatmap_U64Ptr(byte* label_id, ulong* values, int rows, int cols, double scale_min, double scale_max, byte* label_fmt, ImPlotPoint bounds_min, ImPlotPoint bounds_max, ImPlotHeatmapFlags flags);

		// Token: 0x060005C9 RID: 1481
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern double ImPlot_PlotHistogram_FloatPtr(byte* label_id, float* values, int count, int bins, double bar_scale, ImPlotRange range, ImPlotHistogramFlags flags);

		// Token: 0x060005CA RID: 1482
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern double ImPlot_PlotHistogram_doublePtr(byte* label_id, double* values, int count, int bins, double bar_scale, ImPlotRange range, ImPlotHistogramFlags flags);

		// Token: 0x060005CB RID: 1483
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern double ImPlot_PlotHistogram_S8Ptr(byte* label_id, sbyte* values, int count, int bins, double bar_scale, ImPlotRange range, ImPlotHistogramFlags flags);

		// Token: 0x060005CC RID: 1484
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern double ImPlot_PlotHistogram_U8Ptr(byte* label_id, byte* values, int count, int bins, double bar_scale, ImPlotRange range, ImPlotHistogramFlags flags);

		// Token: 0x060005CD RID: 1485
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern double ImPlot_PlotHistogram_S16Ptr(byte* label_id, short* values, int count, int bins, double bar_scale, ImPlotRange range, ImPlotHistogramFlags flags);

		// Token: 0x060005CE RID: 1486
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern double ImPlot_PlotHistogram_U16Ptr(byte* label_id, ushort* values, int count, int bins, double bar_scale, ImPlotRange range, ImPlotHistogramFlags flags);

		// Token: 0x060005CF RID: 1487
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern double ImPlot_PlotHistogram_S32Ptr(byte* label_id, int* values, int count, int bins, double bar_scale, ImPlotRange range, ImPlotHistogramFlags flags);

		// Token: 0x060005D0 RID: 1488
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern double ImPlot_PlotHistogram_U32Ptr(byte* label_id, uint* values, int count, int bins, double bar_scale, ImPlotRange range, ImPlotHistogramFlags flags);

		// Token: 0x060005D1 RID: 1489
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern double ImPlot_PlotHistogram_S64Ptr(byte* label_id, long* values, int count, int bins, double bar_scale, ImPlotRange range, ImPlotHistogramFlags flags);

		// Token: 0x060005D2 RID: 1490
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern double ImPlot_PlotHistogram_U64Ptr(byte* label_id, ulong* values, int count, int bins, double bar_scale, ImPlotRange range, ImPlotHistogramFlags flags);

		// Token: 0x060005D3 RID: 1491
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern double ImPlot_PlotHistogram2D_FloatPtr(byte* label_id, float* xs, float* ys, int count, int x_bins, int y_bins, ImPlotRect range, ImPlotHistogramFlags flags);

		// Token: 0x060005D4 RID: 1492
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern double ImPlot_PlotHistogram2D_doublePtr(byte* label_id, double* xs, double* ys, int count, int x_bins, int y_bins, ImPlotRect range, ImPlotHistogramFlags flags);

		// Token: 0x060005D5 RID: 1493
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern double ImPlot_PlotHistogram2D_S8Ptr(byte* label_id, sbyte* xs, sbyte* ys, int count, int x_bins, int y_bins, ImPlotRect range, ImPlotHistogramFlags flags);

		// Token: 0x060005D6 RID: 1494
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern double ImPlot_PlotHistogram2D_U8Ptr(byte* label_id, byte* xs, byte* ys, int count, int x_bins, int y_bins, ImPlotRect range, ImPlotHistogramFlags flags);

		// Token: 0x060005D7 RID: 1495
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern double ImPlot_PlotHistogram2D_S16Ptr(byte* label_id, short* xs, short* ys, int count, int x_bins, int y_bins, ImPlotRect range, ImPlotHistogramFlags flags);

		// Token: 0x060005D8 RID: 1496
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern double ImPlot_PlotHistogram2D_U16Ptr(byte* label_id, ushort* xs, ushort* ys, int count, int x_bins, int y_bins, ImPlotRect range, ImPlotHistogramFlags flags);

		// Token: 0x060005D9 RID: 1497
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern double ImPlot_PlotHistogram2D_S32Ptr(byte* label_id, int* xs, int* ys, int count, int x_bins, int y_bins, ImPlotRect range, ImPlotHistogramFlags flags);

		// Token: 0x060005DA RID: 1498
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern double ImPlot_PlotHistogram2D_U32Ptr(byte* label_id, uint* xs, uint* ys, int count, int x_bins, int y_bins, ImPlotRect range, ImPlotHistogramFlags flags);

		// Token: 0x060005DB RID: 1499
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern double ImPlot_PlotHistogram2D_S64Ptr(byte* label_id, long* xs, long* ys, int count, int x_bins, int y_bins, ImPlotRect range, ImPlotHistogramFlags flags);

		// Token: 0x060005DC RID: 1500
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern double ImPlot_PlotHistogram2D_U64Ptr(byte* label_id, ulong* xs, ulong* ys, int count, int x_bins, int y_bins, ImPlotRect range, ImPlotHistogramFlags flags);

		// Token: 0x060005DD RID: 1501
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotImage(byte* label_id, IntPtr user_texture_id, ImPlotPoint bounds_min, ImPlotPoint bounds_max, Vector2 uv0, Vector2 uv1, Vector4 tint_col, ImPlotImageFlags flags);

		// Token: 0x060005DE RID: 1502
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotInfLines_FloatPtr(byte* label_id, float* values, int count, ImPlotInfLinesFlags flags, int offset, int stride);

		// Token: 0x060005DF RID: 1503
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotInfLines_doublePtr(byte* label_id, double* values, int count, ImPlotInfLinesFlags flags, int offset, int stride);

		// Token: 0x060005E0 RID: 1504
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotInfLines_S8Ptr(byte* label_id, sbyte* values, int count, ImPlotInfLinesFlags flags, int offset, int stride);

		// Token: 0x060005E1 RID: 1505
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotInfLines_U8Ptr(byte* label_id, byte* values, int count, ImPlotInfLinesFlags flags, int offset, int stride);

		// Token: 0x060005E2 RID: 1506
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotInfLines_S16Ptr(byte* label_id, short* values, int count, ImPlotInfLinesFlags flags, int offset, int stride);

		// Token: 0x060005E3 RID: 1507
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotInfLines_U16Ptr(byte* label_id, ushort* values, int count, ImPlotInfLinesFlags flags, int offset, int stride);

		// Token: 0x060005E4 RID: 1508
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotInfLines_S32Ptr(byte* label_id, int* values, int count, ImPlotInfLinesFlags flags, int offset, int stride);

		// Token: 0x060005E5 RID: 1509
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotInfLines_U32Ptr(byte* label_id, uint* values, int count, ImPlotInfLinesFlags flags, int offset, int stride);

		// Token: 0x060005E6 RID: 1510
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotInfLines_S64Ptr(byte* label_id, long* values, int count, ImPlotInfLinesFlags flags, int offset, int stride);

		// Token: 0x060005E7 RID: 1511
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotInfLines_U64Ptr(byte* label_id, ulong* values, int count, ImPlotInfLinesFlags flags, int offset, int stride);

		// Token: 0x060005E8 RID: 1512
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotLine_FloatPtrInt(byte* label_id, float* values, int count, double xscale, double xstart, ImPlotLineFlags flags, int offset, int stride);

		// Token: 0x060005E9 RID: 1513
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotLine_doublePtrInt(byte* label_id, double* values, int count, double xscale, double xstart, ImPlotLineFlags flags, int offset, int stride);

		// Token: 0x060005EA RID: 1514
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotLine_S8PtrInt(byte* label_id, sbyte* values, int count, double xscale, double xstart, ImPlotLineFlags flags, int offset, int stride);

		// Token: 0x060005EB RID: 1515
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotLine_U8PtrInt(byte* label_id, byte* values, int count, double xscale, double xstart, ImPlotLineFlags flags, int offset, int stride);

		// Token: 0x060005EC RID: 1516
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotLine_S16PtrInt(byte* label_id, short* values, int count, double xscale, double xstart, ImPlotLineFlags flags, int offset, int stride);

		// Token: 0x060005ED RID: 1517
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotLine_U16PtrInt(byte* label_id, ushort* values, int count, double xscale, double xstart, ImPlotLineFlags flags, int offset, int stride);

		// Token: 0x060005EE RID: 1518
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotLine_S32PtrInt(byte* label_id, int* values, int count, double xscale, double xstart, ImPlotLineFlags flags, int offset, int stride);

		// Token: 0x060005EF RID: 1519
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotLine_U32PtrInt(byte* label_id, uint* values, int count, double xscale, double xstart, ImPlotLineFlags flags, int offset, int stride);

		// Token: 0x060005F0 RID: 1520
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotLine_S64PtrInt(byte* label_id, long* values, int count, double xscale, double xstart, ImPlotLineFlags flags, int offset, int stride);

		// Token: 0x060005F1 RID: 1521
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotLine_U64PtrInt(byte* label_id, ulong* values, int count, double xscale, double xstart, ImPlotLineFlags flags, int offset, int stride);

		// Token: 0x060005F2 RID: 1522
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotLine_FloatPtrFloatPtr(byte* label_id, float* xs, float* ys, int count, ImPlotLineFlags flags, int offset, int stride);

		// Token: 0x060005F3 RID: 1523
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotLine_doublePtrdoublePtr(byte* label_id, double* xs, double* ys, int count, ImPlotLineFlags flags, int offset, int stride);

		// Token: 0x060005F4 RID: 1524
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotLine_S8PtrS8Ptr(byte* label_id, sbyte* xs, sbyte* ys, int count, ImPlotLineFlags flags, int offset, int stride);

		// Token: 0x060005F5 RID: 1525
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotLine_U8PtrU8Ptr(byte* label_id, byte* xs, byte* ys, int count, ImPlotLineFlags flags, int offset, int stride);

		// Token: 0x060005F6 RID: 1526
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotLine_S16PtrS16Ptr(byte* label_id, short* xs, short* ys, int count, ImPlotLineFlags flags, int offset, int stride);

		// Token: 0x060005F7 RID: 1527
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotLine_U16PtrU16Ptr(byte* label_id, ushort* xs, ushort* ys, int count, ImPlotLineFlags flags, int offset, int stride);

		// Token: 0x060005F8 RID: 1528
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotLine_S32PtrS32Ptr(byte* label_id, int* xs, int* ys, int count, ImPlotLineFlags flags, int offset, int stride);

		// Token: 0x060005F9 RID: 1529
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotLine_U32PtrU32Ptr(byte* label_id, uint* xs, uint* ys, int count, ImPlotLineFlags flags, int offset, int stride);

		// Token: 0x060005FA RID: 1530
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotLine_S64PtrS64Ptr(byte* label_id, long* xs, long* ys, int count, ImPlotLineFlags flags, int offset, int stride);

		// Token: 0x060005FB RID: 1531
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotLine_U64PtrU64Ptr(byte* label_id, ulong* xs, ulong* ys, int count, ImPlotLineFlags flags, int offset, int stride);

		// Token: 0x060005FC RID: 1532
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotLineG(byte* label_id, IntPtr getter, void* data, int count, ImPlotLineFlags flags);

		// Token: 0x060005FD RID: 1533
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotPieChart_FloatPtr(byte** label_ids, float* values, int count, double x, double y, double radius, byte* label_fmt, double angle0, ImPlotPieChartFlags flags);

		// Token: 0x060005FE RID: 1534
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotPieChart_doublePtr(byte** label_ids, double* values, int count, double x, double y, double radius, byte* label_fmt, double angle0, ImPlotPieChartFlags flags);

		// Token: 0x060005FF RID: 1535
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotPieChart_S8Ptr(byte** label_ids, sbyte* values, int count, double x, double y, double radius, byte* label_fmt, double angle0, ImPlotPieChartFlags flags);

		// Token: 0x06000600 RID: 1536
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotPieChart_U8Ptr(byte** label_ids, byte* values, int count, double x, double y, double radius, byte* label_fmt, double angle0, ImPlotPieChartFlags flags);

		// Token: 0x06000601 RID: 1537
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotPieChart_S16Ptr(byte** label_ids, short* values, int count, double x, double y, double radius, byte* label_fmt, double angle0, ImPlotPieChartFlags flags);

		// Token: 0x06000602 RID: 1538
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotPieChart_U16Ptr(byte** label_ids, ushort* values, int count, double x, double y, double radius, byte* label_fmt, double angle0, ImPlotPieChartFlags flags);

		// Token: 0x06000603 RID: 1539
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotPieChart_S32Ptr(byte** label_ids, int* values, int count, double x, double y, double radius, byte* label_fmt, double angle0, ImPlotPieChartFlags flags);

		// Token: 0x06000604 RID: 1540
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotPieChart_U32Ptr(byte** label_ids, uint* values, int count, double x, double y, double radius, byte* label_fmt, double angle0, ImPlotPieChartFlags flags);

		// Token: 0x06000605 RID: 1541
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotPieChart_S64Ptr(byte** label_ids, long* values, int count, double x, double y, double radius, byte* label_fmt, double angle0, ImPlotPieChartFlags flags);

		// Token: 0x06000606 RID: 1542
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotPieChart_U64Ptr(byte** label_ids, ulong* values, int count, double x, double y, double radius, byte* label_fmt, double angle0, ImPlotPieChartFlags flags);

		// Token: 0x06000607 RID: 1543
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotScatter_FloatPtrInt(byte* label_id, float* values, int count, double xscale, double xstart, ImPlotScatterFlags flags, int offset, int stride);

		// Token: 0x06000608 RID: 1544
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotScatter_doublePtrInt(byte* label_id, double* values, int count, double xscale, double xstart, ImPlotScatterFlags flags, int offset, int stride);

		// Token: 0x06000609 RID: 1545
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotScatter_S8PtrInt(byte* label_id, sbyte* values, int count, double xscale, double xstart, ImPlotScatterFlags flags, int offset, int stride);

		// Token: 0x0600060A RID: 1546
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotScatter_U8PtrInt(byte* label_id, byte* values, int count, double xscale, double xstart, ImPlotScatterFlags flags, int offset, int stride);

		// Token: 0x0600060B RID: 1547
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotScatter_S16PtrInt(byte* label_id, short* values, int count, double xscale, double xstart, ImPlotScatterFlags flags, int offset, int stride);

		// Token: 0x0600060C RID: 1548
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotScatter_U16PtrInt(byte* label_id, ushort* values, int count, double xscale, double xstart, ImPlotScatterFlags flags, int offset, int stride);

		// Token: 0x0600060D RID: 1549
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotScatter_S32PtrInt(byte* label_id, int* values, int count, double xscale, double xstart, ImPlotScatterFlags flags, int offset, int stride);

		// Token: 0x0600060E RID: 1550
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotScatter_U32PtrInt(byte* label_id, uint* values, int count, double xscale, double xstart, ImPlotScatterFlags flags, int offset, int stride);

		// Token: 0x0600060F RID: 1551
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotScatter_S64PtrInt(byte* label_id, long* values, int count, double xscale, double xstart, ImPlotScatterFlags flags, int offset, int stride);

		// Token: 0x06000610 RID: 1552
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotScatter_U64PtrInt(byte* label_id, ulong* values, int count, double xscale, double xstart, ImPlotScatterFlags flags, int offset, int stride);

		// Token: 0x06000611 RID: 1553
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotScatter_FloatPtrFloatPtr(byte* label_id, float* xs, float* ys, int count, ImPlotScatterFlags flags, int offset, int stride);

		// Token: 0x06000612 RID: 1554
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotScatter_doublePtrdoublePtr(byte* label_id, double* xs, double* ys, int count, ImPlotScatterFlags flags, int offset, int stride);

		// Token: 0x06000613 RID: 1555
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotScatter_S8PtrS8Ptr(byte* label_id, sbyte* xs, sbyte* ys, int count, ImPlotScatterFlags flags, int offset, int stride);

		// Token: 0x06000614 RID: 1556
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotScatter_U8PtrU8Ptr(byte* label_id, byte* xs, byte* ys, int count, ImPlotScatterFlags flags, int offset, int stride);

		// Token: 0x06000615 RID: 1557
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotScatter_S16PtrS16Ptr(byte* label_id, short* xs, short* ys, int count, ImPlotScatterFlags flags, int offset, int stride);

		// Token: 0x06000616 RID: 1558
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotScatter_U16PtrU16Ptr(byte* label_id, ushort* xs, ushort* ys, int count, ImPlotScatterFlags flags, int offset, int stride);

		// Token: 0x06000617 RID: 1559
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotScatter_S32PtrS32Ptr(byte* label_id, int* xs, int* ys, int count, ImPlotScatterFlags flags, int offset, int stride);

		// Token: 0x06000618 RID: 1560
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotScatter_U32PtrU32Ptr(byte* label_id, uint* xs, uint* ys, int count, ImPlotScatterFlags flags, int offset, int stride);

		// Token: 0x06000619 RID: 1561
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotScatter_S64PtrS64Ptr(byte* label_id, long* xs, long* ys, int count, ImPlotScatterFlags flags, int offset, int stride);

		// Token: 0x0600061A RID: 1562
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotScatter_U64PtrU64Ptr(byte* label_id, ulong* xs, ulong* ys, int count, ImPlotScatterFlags flags, int offset, int stride);

		// Token: 0x0600061B RID: 1563
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotScatterG(byte* label_id, IntPtr getter, void* data, int count, ImPlotScatterFlags flags);

		// Token: 0x0600061C RID: 1564
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_FloatPtrInt(byte* label_id, float* values, int count, double yref, double xscale, double xstart, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x0600061D RID: 1565
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_doublePtrInt(byte* label_id, double* values, int count, double yref, double xscale, double xstart, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x0600061E RID: 1566
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_S8PtrInt(byte* label_id, sbyte* values, int count, double yref, double xscale, double xstart, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x0600061F RID: 1567
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_U8PtrInt(byte* label_id, byte* values, int count, double yref, double xscale, double xstart, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x06000620 RID: 1568
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_S16PtrInt(byte* label_id, short* values, int count, double yref, double xscale, double xstart, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x06000621 RID: 1569
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_U16PtrInt(byte* label_id, ushort* values, int count, double yref, double xscale, double xstart, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x06000622 RID: 1570
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_S32PtrInt(byte* label_id, int* values, int count, double yref, double xscale, double xstart, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x06000623 RID: 1571
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_U32PtrInt(byte* label_id, uint* values, int count, double yref, double xscale, double xstart, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x06000624 RID: 1572
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_S64PtrInt(byte* label_id, long* values, int count, double yref, double xscale, double xstart, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x06000625 RID: 1573
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_U64PtrInt(byte* label_id, ulong* values, int count, double yref, double xscale, double xstart, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x06000626 RID: 1574
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_FloatPtrFloatPtrInt(byte* label_id, float* xs, float* ys, int count, double yref, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x06000627 RID: 1575
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_doublePtrdoublePtrInt(byte* label_id, double* xs, double* ys, int count, double yref, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x06000628 RID: 1576
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_S8PtrS8PtrInt(byte* label_id, sbyte* xs, sbyte* ys, int count, double yref, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x06000629 RID: 1577
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_U8PtrU8PtrInt(byte* label_id, byte* xs, byte* ys, int count, double yref, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x0600062A RID: 1578
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_S16PtrS16PtrInt(byte* label_id, short* xs, short* ys, int count, double yref, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x0600062B RID: 1579
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_U16PtrU16PtrInt(byte* label_id, ushort* xs, ushort* ys, int count, double yref, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x0600062C RID: 1580
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_S32PtrS32PtrInt(byte* label_id, int* xs, int* ys, int count, double yref, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x0600062D RID: 1581
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_U32PtrU32PtrInt(byte* label_id, uint* xs, uint* ys, int count, double yref, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x0600062E RID: 1582
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_S64PtrS64PtrInt(byte* label_id, long* xs, long* ys, int count, double yref, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x0600062F RID: 1583
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_U64PtrU64PtrInt(byte* label_id, ulong* xs, ulong* ys, int count, double yref, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x06000630 RID: 1584
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_FloatPtrFloatPtrFloatPtr(byte* label_id, float* xs, float* ys1, float* ys2, int count, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x06000631 RID: 1585
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_doublePtrdoublePtrdoublePtr(byte* label_id, double* xs, double* ys1, double* ys2, int count, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x06000632 RID: 1586
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_S8PtrS8PtrS8Ptr(byte* label_id, sbyte* xs, sbyte* ys1, sbyte* ys2, int count, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x06000633 RID: 1587
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_U8PtrU8PtrU8Ptr(byte* label_id, byte* xs, byte* ys1, byte* ys2, int count, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x06000634 RID: 1588
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_S16PtrS16PtrS16Ptr(byte* label_id, short* xs, short* ys1, short* ys2, int count, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x06000635 RID: 1589
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_U16PtrU16PtrU16Ptr(byte* label_id, ushort* xs, ushort* ys1, ushort* ys2, int count, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x06000636 RID: 1590
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_S32PtrS32PtrS32Ptr(byte* label_id, int* xs, int* ys1, int* ys2, int count, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x06000637 RID: 1591
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_U32PtrU32PtrU32Ptr(byte* label_id, uint* xs, uint* ys1, uint* ys2, int count, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x06000638 RID: 1592
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_S64PtrS64PtrS64Ptr(byte* label_id, long* xs, long* ys1, long* ys2, int count, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x06000639 RID: 1593
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShaded_U64PtrU64PtrU64Ptr(byte* label_id, ulong* xs, ulong* ys1, ulong* ys2, int count, ImPlotShadedFlags flags, int offset, int stride);

		// Token: 0x0600063A RID: 1594
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotShadedG(byte* label_id, IntPtr getter1, void* data1, IntPtr getter2, void* data2, int count, ImPlotShadedFlags flags);

		// Token: 0x0600063B RID: 1595
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStairs_FloatPtrInt(byte* label_id, float* values, int count, double xscale, double xstart, ImPlotStairsFlags flags, int offset, int stride);

		// Token: 0x0600063C RID: 1596
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStairs_doublePtrInt(byte* label_id, double* values, int count, double xscale, double xstart, ImPlotStairsFlags flags, int offset, int stride);

		// Token: 0x0600063D RID: 1597
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStairs_S8PtrInt(byte* label_id, sbyte* values, int count, double xscale, double xstart, ImPlotStairsFlags flags, int offset, int stride);

		// Token: 0x0600063E RID: 1598
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStairs_U8PtrInt(byte* label_id, byte* values, int count, double xscale, double xstart, ImPlotStairsFlags flags, int offset, int stride);

		// Token: 0x0600063F RID: 1599
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStairs_S16PtrInt(byte* label_id, short* values, int count, double xscale, double xstart, ImPlotStairsFlags flags, int offset, int stride);

		// Token: 0x06000640 RID: 1600
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStairs_U16PtrInt(byte* label_id, ushort* values, int count, double xscale, double xstart, ImPlotStairsFlags flags, int offset, int stride);

		// Token: 0x06000641 RID: 1601
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStairs_S32PtrInt(byte* label_id, int* values, int count, double xscale, double xstart, ImPlotStairsFlags flags, int offset, int stride);

		// Token: 0x06000642 RID: 1602
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStairs_U32PtrInt(byte* label_id, uint* values, int count, double xscale, double xstart, ImPlotStairsFlags flags, int offset, int stride);

		// Token: 0x06000643 RID: 1603
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStairs_S64PtrInt(byte* label_id, long* values, int count, double xscale, double xstart, ImPlotStairsFlags flags, int offset, int stride);

		// Token: 0x06000644 RID: 1604
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStairs_U64PtrInt(byte* label_id, ulong* values, int count, double xscale, double xstart, ImPlotStairsFlags flags, int offset, int stride);

		// Token: 0x06000645 RID: 1605
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStairs_FloatPtrFloatPtr(byte* label_id, float* xs, float* ys, int count, ImPlotStairsFlags flags, int offset, int stride);

		// Token: 0x06000646 RID: 1606
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStairs_doublePtrdoublePtr(byte* label_id, double* xs, double* ys, int count, ImPlotStairsFlags flags, int offset, int stride);

		// Token: 0x06000647 RID: 1607
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStairs_S8PtrS8Ptr(byte* label_id, sbyte* xs, sbyte* ys, int count, ImPlotStairsFlags flags, int offset, int stride);

		// Token: 0x06000648 RID: 1608
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStairs_U8PtrU8Ptr(byte* label_id, byte* xs, byte* ys, int count, ImPlotStairsFlags flags, int offset, int stride);

		// Token: 0x06000649 RID: 1609
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStairs_S16PtrS16Ptr(byte* label_id, short* xs, short* ys, int count, ImPlotStairsFlags flags, int offset, int stride);

		// Token: 0x0600064A RID: 1610
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStairs_U16PtrU16Ptr(byte* label_id, ushort* xs, ushort* ys, int count, ImPlotStairsFlags flags, int offset, int stride);

		// Token: 0x0600064B RID: 1611
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStairs_S32PtrS32Ptr(byte* label_id, int* xs, int* ys, int count, ImPlotStairsFlags flags, int offset, int stride);

		// Token: 0x0600064C RID: 1612
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStairs_U32PtrU32Ptr(byte* label_id, uint* xs, uint* ys, int count, ImPlotStairsFlags flags, int offset, int stride);

		// Token: 0x0600064D RID: 1613
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStairs_S64PtrS64Ptr(byte* label_id, long* xs, long* ys, int count, ImPlotStairsFlags flags, int offset, int stride);

		// Token: 0x0600064E RID: 1614
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStairs_U64PtrU64Ptr(byte* label_id, ulong* xs, ulong* ys, int count, ImPlotStairsFlags flags, int offset, int stride);

		// Token: 0x0600064F RID: 1615
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStairsG(byte* label_id, IntPtr getter, void* data, int count, ImPlotStairsFlags flags);

		// Token: 0x06000650 RID: 1616
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStems_FloatPtrInt(byte* label_id, float* values, int count, double @ref, double scale, double start, ImPlotStemsFlags flags, int offset, int stride);

		// Token: 0x06000651 RID: 1617
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStems_doublePtrInt(byte* label_id, double* values, int count, double @ref, double scale, double start, ImPlotStemsFlags flags, int offset, int stride);

		// Token: 0x06000652 RID: 1618
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStems_S8PtrInt(byte* label_id, sbyte* values, int count, double @ref, double scale, double start, ImPlotStemsFlags flags, int offset, int stride);

		// Token: 0x06000653 RID: 1619
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStems_U8PtrInt(byte* label_id, byte* values, int count, double @ref, double scale, double start, ImPlotStemsFlags flags, int offset, int stride);

		// Token: 0x06000654 RID: 1620
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStems_S16PtrInt(byte* label_id, short* values, int count, double @ref, double scale, double start, ImPlotStemsFlags flags, int offset, int stride);

		// Token: 0x06000655 RID: 1621
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStems_U16PtrInt(byte* label_id, ushort* values, int count, double @ref, double scale, double start, ImPlotStemsFlags flags, int offset, int stride);

		// Token: 0x06000656 RID: 1622
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStems_S32PtrInt(byte* label_id, int* values, int count, double @ref, double scale, double start, ImPlotStemsFlags flags, int offset, int stride);

		// Token: 0x06000657 RID: 1623
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStems_U32PtrInt(byte* label_id, uint* values, int count, double @ref, double scale, double start, ImPlotStemsFlags flags, int offset, int stride);

		// Token: 0x06000658 RID: 1624
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStems_S64PtrInt(byte* label_id, long* values, int count, double @ref, double scale, double start, ImPlotStemsFlags flags, int offset, int stride);

		// Token: 0x06000659 RID: 1625
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStems_U64PtrInt(byte* label_id, ulong* values, int count, double @ref, double scale, double start, ImPlotStemsFlags flags, int offset, int stride);

		// Token: 0x0600065A RID: 1626
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStems_FloatPtrFloatPtr(byte* label_id, float* xs, float* ys, int count, double @ref, ImPlotStemsFlags flags, int offset, int stride);

		// Token: 0x0600065B RID: 1627
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStems_doublePtrdoublePtr(byte* label_id, double* xs, double* ys, int count, double @ref, ImPlotStemsFlags flags, int offset, int stride);

		// Token: 0x0600065C RID: 1628
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStems_S8PtrS8Ptr(byte* label_id, sbyte* xs, sbyte* ys, int count, double @ref, ImPlotStemsFlags flags, int offset, int stride);

		// Token: 0x0600065D RID: 1629
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStems_U8PtrU8Ptr(byte* label_id, byte* xs, byte* ys, int count, double @ref, ImPlotStemsFlags flags, int offset, int stride);

		// Token: 0x0600065E RID: 1630
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStems_S16PtrS16Ptr(byte* label_id, short* xs, short* ys, int count, double @ref, ImPlotStemsFlags flags, int offset, int stride);

		// Token: 0x0600065F RID: 1631
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStems_U16PtrU16Ptr(byte* label_id, ushort* xs, ushort* ys, int count, double @ref, ImPlotStemsFlags flags, int offset, int stride);

		// Token: 0x06000660 RID: 1632
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStems_S32PtrS32Ptr(byte* label_id, int* xs, int* ys, int count, double @ref, ImPlotStemsFlags flags, int offset, int stride);

		// Token: 0x06000661 RID: 1633
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStems_U32PtrU32Ptr(byte* label_id, uint* xs, uint* ys, int count, double @ref, ImPlotStemsFlags flags, int offset, int stride);

		// Token: 0x06000662 RID: 1634
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStems_S64PtrS64Ptr(byte* label_id, long* xs, long* ys, int count, double @ref, ImPlotStemsFlags flags, int offset, int stride);

		// Token: 0x06000663 RID: 1635
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotStems_U64PtrU64Ptr(byte* label_id, ulong* xs, ulong* ys, int count, double @ref, ImPlotStemsFlags flags, int offset, int stride);

		// Token: 0x06000664 RID: 1636
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotText(byte* text, double x, double y, Vector2 pix_offset, ImPlotTextFlags flags);

		// Token: 0x06000665 RID: 1637
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotToPixels_PlotPoInt(Vector2* pOut, ImPlotPoint plt, ImAxis x_axis, ImAxis y_axis);

		// Token: 0x06000666 RID: 1638
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PlotToPixels_double(Vector2* pOut, double x, double y, ImAxis x_axis, ImAxis y_axis);

		// Token: 0x06000667 RID: 1639
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_PopColormap(int count);

		// Token: 0x06000668 RID: 1640
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_PopPlotClipRect();

		// Token: 0x06000669 RID: 1641
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_PopStyleColor(int count);

		// Token: 0x0600066A RID: 1642
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_PopStyleVar(int count);

		// Token: 0x0600066B RID: 1643
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_PushColormap_PlotColormap(ImPlotColormap cmap);

		// Token: 0x0600066C RID: 1644
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_PushColormap_Str(byte* name);

		// Token: 0x0600066D RID: 1645
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_PushPlotClipRect(float expand);

		// Token: 0x0600066E RID: 1646
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_PushStyleColor_U32(ImPlotCol idx, uint col);

		// Token: 0x0600066F RID: 1647
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_PushStyleColor_Vec4(ImPlotCol idx, Vector4 col);

		// Token: 0x06000670 RID: 1648
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_PushStyleVar_Float(ImPlotStyleVar idx, float val);

		// Token: 0x06000671 RID: 1649
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_PushStyleVar_Int(ImPlotStyleVar idx, int val);

		// Token: 0x06000672 RID: 1650
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_PushStyleVar_Vec2(ImPlotStyleVar idx, Vector2 val);

		// Token: 0x06000673 RID: 1651
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_SampleColormap(Vector4* pOut, float t, ImPlotColormap cmap);

		// Token: 0x06000674 RID: 1652
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_SetAxes(ImAxis x_axis, ImAxis y_axis);

		// Token: 0x06000675 RID: 1653
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_SetAxis(ImAxis axis);

		// Token: 0x06000676 RID: 1654
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_SetCurrentContext(IntPtr ctx);

		// Token: 0x06000677 RID: 1655
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_SetImGuiContext(IntPtr ctx);

		// Token: 0x06000678 RID: 1656
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_SetNextAxesLimits(double x_min, double x_max, double y_min, double y_max, ImPlotCond cond);

		// Token: 0x06000679 RID: 1657
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_SetNextAxesToFit();

		// Token: 0x0600067A RID: 1658
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_SetNextAxisLimits(ImAxis axis, double v_min, double v_max, ImPlotCond cond);

		// Token: 0x0600067B RID: 1659
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_SetNextAxisLinks(ImAxis axis, double* link_min, double* link_max);

		// Token: 0x0600067C RID: 1660
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_SetNextAxisToFit(ImAxis axis);

		// Token: 0x0600067D RID: 1661
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_SetNextErrorBarStyle(Vector4 col, float size, float weight);

		// Token: 0x0600067E RID: 1662
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_SetNextFillStyle(Vector4 col, float alpha_mod);

		// Token: 0x0600067F RID: 1663
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_SetNextLineStyle(Vector4 col, float weight);

		// Token: 0x06000680 RID: 1664
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_SetNextMarkerStyle(ImPlotMarker marker, float size, Vector4 fill, float weight, Vector4 outline);

		// Token: 0x06000681 RID: 1665
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_SetupAxes(byte* x_label, byte* y_label, ImPlotAxisFlags x_flags, ImPlotAxisFlags y_flags);

		// Token: 0x06000682 RID: 1666
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_SetupAxesLimits(double x_min, double x_max, double y_min, double y_max, ImPlotCond cond);

		// Token: 0x06000683 RID: 1667
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_SetupAxis(ImAxis axis, byte* label, ImPlotAxisFlags flags);

		// Token: 0x06000684 RID: 1668
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_SetupAxisFormat_Str(ImAxis axis, byte* fmt);

		// Token: 0x06000685 RID: 1669
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_SetupAxisFormat_PlotFormatter(ImAxis axis, IntPtr formatter, void* data);

		// Token: 0x06000686 RID: 1670
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_SetupAxisLimits(ImAxis axis, double v_min, double v_max, ImPlotCond cond);

		// Token: 0x06000687 RID: 1671
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_SetupAxisLimitsConstraints(ImAxis axis, double v_min, double v_max);

		// Token: 0x06000688 RID: 1672
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_SetupAxisLinks(ImAxis axis, double* link_min, double* link_max);

		// Token: 0x06000689 RID: 1673
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_SetupAxisScale_PlotScale(ImAxis axis, ImPlotScale scale);

		// Token: 0x0600068A RID: 1674
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_SetupAxisScale_PlotTransform(ImAxis axis, IntPtr forward, IntPtr inverse, void* data);

		// Token: 0x0600068B RID: 1675
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_SetupAxisTicks_doublePtr(ImAxis axis, double* values, int n_ticks, byte** labels, byte keep_default);

		// Token: 0x0600068C RID: 1676
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_SetupAxisTicks_double(ImAxis axis, double v_min, double v_max, int n_ticks, byte** labels, byte keep_default);

		// Token: 0x0600068D RID: 1677
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_SetupAxisZoomConstraints(ImAxis axis, double z_min, double z_max);

		// Token: 0x0600068E RID: 1678
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_SetupFinish();

		// Token: 0x0600068F RID: 1679
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_SetupLegend(ImPlotLocation location, ImPlotLegendFlags flags);

		// Token: 0x06000690 RID: 1680
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_SetupMouseText(ImPlotLocation location, ImPlotMouseTextFlags flags);

		// Token: 0x06000691 RID: 1681
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern byte ImPlot_ShowColormapSelector(byte* label);

		// Token: 0x06000692 RID: 1682
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_ShowDemoWindow(byte* p_open);

		// Token: 0x06000693 RID: 1683
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern byte ImPlot_ShowInputMapSelector(byte* label);

		// Token: 0x06000694 RID: 1684
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_ShowMetricsWindow(byte* p_popen);

		// Token: 0x06000695 RID: 1685
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_ShowStyleEditor(ImPlotStyle* @ref);

		// Token: 0x06000696 RID: 1686
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern byte ImPlot_ShowStyleSelector(byte* label);

		// Token: 0x06000697 RID: 1687
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_ShowUserGuide();

		// Token: 0x06000698 RID: 1688
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_StyleColorsAuto(ImPlotStyle* dst);

		// Token: 0x06000699 RID: 1689
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_StyleColorsClassic(ImPlotStyle* dst);

		// Token: 0x0600069A RID: 1690
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_StyleColorsDark(ImPlotStyle* dst);

		// Token: 0x0600069B RID: 1691
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_StyleColorsLight(ImPlotStyle* dst);

		// Token: 0x0600069C RID: 1692
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_TagX_Bool(double x, Vector4 col, byte round);

		// Token: 0x0600069D RID: 1693
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_TagX_Str(double x, Vector4 col, byte* fmt);

		// Token: 0x0600069E RID: 1694
		[DllImport("cimplot", CallingConvention = 2)]
		public static extern void ImPlot_TagY_Bool(double y, Vector4 col, byte round);

		// Token: 0x0600069F RID: 1695
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlot_TagY_Str(double y, Vector4 col, byte* fmt);

		// Token: 0x060006A0 RID: 1696
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlotInputMap_destroy(ImPlotInputMap* self);

		// Token: 0x060006A1 RID: 1697
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern ImPlotInputMap* ImPlotInputMap_ImPlotInputMap();

		// Token: 0x060006A2 RID: 1698
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlotPoint_destroy(ImPlotPoint* self);

		// Token: 0x060006A3 RID: 1699
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern ImPlotPoint* ImPlotPoint_ImPlotPoint_Nil();

		// Token: 0x060006A4 RID: 1700
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern ImPlotPoint* ImPlotPoint_ImPlotPoint_double(double _x, double _y);

		// Token: 0x060006A5 RID: 1701
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern ImPlotPoint* ImPlotPoint_ImPlotPoint_Vec2(Vector2 p);

		// Token: 0x060006A6 RID: 1702
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern double ImPlotRange_Clamp(ImPlotRange* self, double value);

		// Token: 0x060006A7 RID: 1703
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern byte ImPlotRange_Contains(ImPlotRange* self, double value);

		// Token: 0x060006A8 RID: 1704
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlotRange_destroy(ImPlotRange* self);

		// Token: 0x060006A9 RID: 1705
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern ImPlotRange* ImPlotRange_ImPlotRange_Nil();

		// Token: 0x060006AA RID: 1706
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern ImPlotRange* ImPlotRange_ImPlotRange_double(double _min, double _max);

		// Token: 0x060006AB RID: 1707
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern double ImPlotRange_Size(ImPlotRange* self);

		// Token: 0x060006AC RID: 1708
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlotRect_Clamp_PlotPoInt(ImPlotPoint* pOut, ImPlotRect* self, ImPlotPoint p);

		// Token: 0x060006AD RID: 1709
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlotRect_Clamp_double(ImPlotPoint* pOut, ImPlotRect* self, double x, double y);

		// Token: 0x060006AE RID: 1710
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern byte ImPlotRect_Contains_PlotPoInt(ImPlotRect* self, ImPlotPoint p);

		// Token: 0x060006AF RID: 1711
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern byte ImPlotRect_Contains_double(ImPlotRect* self, double x, double y);

		// Token: 0x060006B0 RID: 1712
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlotRect_destroy(ImPlotRect* self);

		// Token: 0x060006B1 RID: 1713
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern ImPlotRect* ImPlotRect_ImPlotRect_Nil();

		// Token: 0x060006B2 RID: 1714
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern ImPlotRect* ImPlotRect_ImPlotRect_double(double x_min, double x_max, double y_min, double y_max);

		// Token: 0x060006B3 RID: 1715
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlotRect_Max(ImPlotPoint* pOut, ImPlotRect* self);

		// Token: 0x060006B4 RID: 1716
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlotRect_Min(ImPlotPoint* pOut, ImPlotRect* self);

		// Token: 0x060006B5 RID: 1717
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlotRect_Size(ImPlotPoint* pOut, ImPlotRect* self);

		// Token: 0x060006B6 RID: 1718
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern void ImPlotStyle_destroy(ImPlotStyle* self);

		// Token: 0x060006B7 RID: 1719
		[DllImport("cimplot", CallingConvention = 2)]
		public unsafe static extern ImPlotStyle* ImPlotStyle_ImPlotStyle();
	}
}
