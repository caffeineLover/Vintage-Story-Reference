using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using QuestPDF.Skia.Text;

namespace QuestPDF.Skia
{
	// Token: 0x02000008 RID: 8
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class SkCanvas : IDisposable
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002484 File Offset: 0x00000684
		// (set) Token: 0x06000027 RID: 39 RVA: 0x0000248C File Offset: 0x0000068C
		public IntPtr Instance { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002495 File Offset: 0x00000695
		private bool DisposeNativeObject { get; }

		// Token: 0x06000029 RID: 41 RVA: 0x0000249D File Offset: 0x0000069D
		public SkCanvas(IntPtr instance, bool disposeNativeObject = true)
		{
			this.Instance = instance;
			SkiaAPI.EnsureNotNull(this.Instance);
			this.DisposeNativeObject = disposeNativeObject;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000024BE File Offset: 0x000006BE
		public static SkCanvas CreateFromBitmap(SkBitmap bitmap)
		{
			return new SkCanvas(SkCanvas.API.canvas_create_from_bitmap(bitmap.Instance), true);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000024D1 File Offset: 0x000006D1
		public void Save()
		{
			SkCanvas.API.canvas_save(this.Instance);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000024DE File Offset: 0x000006DE
		public void Restore()
		{
			SkCanvas.API.canvas_restore(this.Instance);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000024EB File Offset: 0x000006EB
		public void Translate(float x, float y)
		{
			SkCanvas.API.canvas_translate(this.Instance, x, y);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000024FA File Offset: 0x000006FA
		public void Scale(float factorX, float factorY)
		{
			SkCanvas.API.canvas_scale(this.Instance, factorX, factorY);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002509 File Offset: 0x00000709
		public void Rotate(float degrees)
		{
			SkCanvas.API.canvas_rotate(this.Instance, degrees);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002517 File Offset: 0x00000717
		public void DrawLine(SkPoint start, SkPoint end, SkPaint paint)
		{
			SkCanvas.API.canvas_draw_line(this.Instance, start, end, paint.Instance);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000252C File Offset: 0x0000072C
		public void DrawRectangle(SkRect position, SkPaint paint)
		{
			SkCanvas.API.canvas_draw_rectangle(this.Instance, position, paint.Instance);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002540 File Offset: 0x00000740
		public void DrawComplexBorder(SkRoundedRect innerRect, SkRoundedRect outerRect, SkPaint paint)
		{
			SkCanvas.API.canvas_draw_complex_border(this.Instance, innerRect, outerRect, paint.Instance);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002555 File Offset: 0x00000755
		public void DrawShadow(SkRoundedRect shadowRect, SkBoxShadow shadow)
		{
			SkCanvas.API.canvas_draw_shadow(this.Instance, shadowRect, shadow);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002564 File Offset: 0x00000764
		public void DrawImage(SkImage image, float width, float height)
		{
			SkCanvas.API.canvas_draw_image(this.Instance, image.Instance, width, height);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002579 File Offset: 0x00000779
		public void DrawPicture(SkPicture picture)
		{
			SkCanvas.API.canvas_draw_picture(this.Instance, picture.Instance);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000258C File Offset: 0x0000078C
		public void DrawParagraph(SkParagraph paragraph, int? lineFrom = null, int? lineTo = null)
		{
			SkCanvas.API.canvas_draw_paragraph(this.Instance, paragraph.Instance, lineFrom.GetValueOrDefault(), lineTo.GetValueOrDefault(int.MaxValue));
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000025B2 File Offset: 0x000007B2
		public void DrawSvgPath(string svg, uint color)
		{
			SkCanvas.API.canvas_draw_svg_path(this.Instance, svg, color);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000025C1 File Offset: 0x000007C1
		public void DrawSvg(SkSvgImage svgImage, float width, float height)
		{
			SkCanvas.API.canvas_draw_svg(this.Instance, svgImage.Instance, width, height);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000025D6 File Offset: 0x000007D6
		public void DrawOverflowArea(SkRect position)
		{
			SkCanvas.API.canvas_draw_overflow_area(this.Instance, position);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000025E4 File Offset: 0x000007E4
		public void ClipOverflowArea(SkRect availableSpace, SkRect requiredSpace)
		{
			SkCanvas.API.canvas_clip_overflow_area(this.Instance, availableSpace, requiredSpace);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000025F3 File Offset: 0x000007F3
		public void ClipRectangle(SkRect clipArea)
		{
			SkCanvas.API.canvas_clip_rectangle(this.Instance, clipArea);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002601 File Offset: 0x00000801
		public void ClipRoundedRectangle(SkRoundedRect rect)
		{
			SkCanvas.API.canvas_clip_rounded_rectangle(this.Instance, rect);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0000260F File Offset: 0x0000080F
		public void AnnotateUrl(float width, float height, string url, [Nullable(2)] string description)
		{
			SkCanvas.API.canvas_annotate_url(this.Instance, width, height, url, description);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002621 File Offset: 0x00000821
		public void AnnotateDestination(string destinationName)
		{
			SkCanvas.API.canvas_annotate_destination(this.Instance, destinationName);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x0000262F File Offset: 0x0000082F
		public void AnnotateDestinationLink(float width, float height, string destinationName, [Nullable(2)] string description)
		{
			SkCanvas.API.canvas_annotate_destination_link(this.Instance, width, height, destinationName, description);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002641 File Offset: 0x00000841
		public SkCanvasMatrix GetCurrentMatrix()
		{
			return SkCanvas.API.canvas_get_matrix9(this.Instance);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000264E File Offset: 0x0000084E
		public void SetCurrentMatrix(SkCanvasMatrix matrix)
		{
			SkCanvas.API.canvas_set_matrix9(this.Instance, matrix);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000265C File Offset: 0x0000085C
		public void SetSemanticNodeId(int nodeId)
		{
			SkCanvas.API.canvas_set_semantic_node_id(this.Instance, nodeId);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000266C File Offset: 0x0000086C
		~SkCanvas()
		{
			this.WarnThatFinalizerIsReached<SkCanvas>();
			this.Dispose();
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000026A0 File Offset: 0x000008A0
		public void Dispose()
		{
			if (this.Instance == IntPtr.Zero)
			{
				return;
			}
			if (this.DisposeNativeObject)
			{
				SkCanvas.API.canvas_delete(this.Instance);
			}
			this.Instance = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		// Token: 0x02000133 RID: 307
		[Nullable(0)]
		private static class API
		{
			// Token: 0x06000AD4 RID: 2772
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr canvas_create_from_bitmap(IntPtr bitmap);

			// Token: 0x06000AD5 RID: 2773
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_delete(IntPtr canvas);

			// Token: 0x06000AD6 RID: 2774
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_save(IntPtr canvas);

			// Token: 0x06000AD7 RID: 2775
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_restore(IntPtr canvas);

			// Token: 0x06000AD8 RID: 2776
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_translate(IntPtr canvas, float x, float y);

			// Token: 0x06000AD9 RID: 2777
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_rotate(IntPtr canvas, float angle);

			// Token: 0x06000ADA RID: 2778
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_scale(IntPtr canvas, float factorX, float factorY);

			// Token: 0x06000ADB RID: 2779
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_draw_image(IntPtr canvas, IntPtr image, float width, float height);

			// Token: 0x06000ADC RID: 2780
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_draw_picture(IntPtr canvas, IntPtr picture);

			// Token: 0x06000ADD RID: 2781
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_draw_line(IntPtr canvas, SkPoint start, SkPoint end, IntPtr paint);

			// Token: 0x06000ADE RID: 2782
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_draw_rectangle(IntPtr canvas, SkRect position, IntPtr paint);

			// Token: 0x06000ADF RID: 2783
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_draw_complex_border(IntPtr canvas, SkRoundedRect innerRect, SkRoundedRect outerRect, IntPtr paint);

			// Token: 0x06000AE0 RID: 2784
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_draw_shadow(IntPtr canvas, SkRoundedRect shadowRect, SkBoxShadow shadow);

			// Token: 0x06000AE1 RID: 2785
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_draw_paragraph(IntPtr canvas, IntPtr paragraph, int lineFrom, int lineTo);

			// Token: 0x06000AE2 RID: 2786
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_draw_svg_path(IntPtr canvas, [MarshalAs(44, MarshalTypeRef = QuestPDF.Skia.Utf8StringMarshaller)] string svg, uint color);

			// Token: 0x06000AE3 RID: 2787
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_draw_svg(IntPtr canvas, IntPtr svg, float width, float height);

			// Token: 0x06000AE4 RID: 2788
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_draw_overflow_area(IntPtr canvas, SkRect position);

			// Token: 0x06000AE5 RID: 2789
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_clip_overflow_area(IntPtr canvas, SkRect availableSpace, SkRect requiredSpace);

			// Token: 0x06000AE6 RID: 2790
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_clip_rectangle(IntPtr canvas, SkRect clipArea);

			// Token: 0x06000AE7 RID: 2791
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_clip_rounded_rectangle(IntPtr canvas, SkRoundedRect rect);

			// Token: 0x06000AE8 RID: 2792
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_annotate_url(IntPtr canvas, float width, float height, [MarshalAs(44, MarshalTypeRef = QuestPDF.Skia.Utf8StringMarshaller)] string url, [Nullable(2)] [MarshalAs(44, MarshalTypeRef = QuestPDF.Skia.Utf8StringMarshaller)] string description);

			// Token: 0x06000AE9 RID: 2793
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_annotate_destination(IntPtr canvas, [MarshalAs(44, MarshalTypeRef = QuestPDF.Skia.Utf8StringMarshaller)] string destinationName);

			// Token: 0x06000AEA RID: 2794
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_annotate_destination_link(IntPtr canvas, float width, float height, [MarshalAs(44, MarshalTypeRef = QuestPDF.Skia.Utf8StringMarshaller)] string destinationName, [Nullable(2)] [MarshalAs(44, MarshalTypeRef = QuestPDF.Skia.Utf8StringMarshaller)] string description);

			// Token: 0x06000AEB RID: 2795
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern SkCanvasMatrix canvas_get_matrix9(IntPtr canvas);

			// Token: 0x06000AEC RID: 2796
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_set_matrix9(IntPtr canvas, SkCanvasMatrix matrix);

			// Token: 0x06000AED RID: 2797
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void canvas_set_semantic_node_id(IntPtr canvas, int nodeId);
		}
	}
}
