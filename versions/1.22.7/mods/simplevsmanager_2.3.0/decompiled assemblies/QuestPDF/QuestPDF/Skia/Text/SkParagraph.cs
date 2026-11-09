using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace QuestPDF.Skia.Text
{
	// Token: 0x02000027 RID: 39
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class SkParagraph : IDisposable
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000CF RID: 207 RVA: 0x0000389E File Offset: 0x00001A9E
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x000038A6 File Offset: 0x00001AA6
		public IntPtr Instance { get; private set; }

		// Token: 0x060000D1 RID: 209 RVA: 0x000038AF File Offset: 0x00001AAF
		public SkParagraph(IntPtr instance)
		{
			this.Instance = instance;
			SkiaAPI.EnsureNotNull(this.Instance);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000038C9 File Offset: 0x00001AC9
		public void PlanLayout(float availableWidth)
		{
			SkParagraph.API.paragraph_plan_layout(this.Instance, availableWidth);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000038D8 File Offset: 0x00001AD8
		public SkSize[] GetLineMetrics()
		{
			IntPtr intPtr;
			int num;
			SkParagraph.API.paragraph_get_line_metrics(this.Instance, out intPtr, out num);
			SkSize[] array = new SkSize[num];
			int num2 = Marshal.SizeOf<SkSize>();
			for (int i = 0; i < num; i++)
			{
				IntPtr intPtr2 = IntPtr.Add(intPtr, i * num2);
				array[i] = Marshal.PtrToStructure<SkSize>(intPtr2);
			}
			SkParagraph.API.paragraph_delete_line_metrics(intPtr);
			return array;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00003934 File Offset: 0x00001B34
		public int[] GetUnresolvedCodepoints()
		{
			IntPtr intPtr;
			int num;
			SkParagraph.API.paragraph_get_unresolved_codepoints(this.Instance, out intPtr, out num);
			int[] array = new int[num];
			Marshal.Copy(intPtr, array, 0, num);
			SkParagraph.API.paragraph_delete_unresolved_codepoints(intPtr);
			return array;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00003968 File Offset: 0x00001B68
		public SkRect[] GetPlaceholderPositions()
		{
			IntPtr intPtr;
			int num;
			SkParagraph.API.paragraph_get_placeholder_positions(this.Instance, out intPtr, out num);
			SkRect[] array = new SkRect[num];
			int num2 = Marshal.SizeOf<SkRect>();
			for (int i = 0; i < num; i++)
			{
				IntPtr intPtr2 = IntPtr.Add(intPtr, i * num2);
				array[i] = Marshal.PtrToStructure<SkRect>(intPtr2);
			}
			SkParagraph.API.paragraph_delete_positions(intPtr);
			return array;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000039C4 File Offset: 0x00001BC4
		public SkRect[] GetTextRangePositions(int rangeStart, int rangeEnd)
		{
			IntPtr intPtr;
			int num;
			SkParagraph.API.paragraph_get_text_range_positions(this.Instance, rangeStart, rangeEnd, out intPtr, out num);
			SkRect[] array = new SkRect[num];
			int num2 = Marshal.SizeOf<SkRect>();
			for (int i = 0; i < num; i++)
			{
				IntPtr intPtr2 = IntPtr.Add(intPtr, i * num2);
				array[i] = Marshal.PtrToStructure<SkRect>(intPtr2);
			}
			SkParagraph.API.paragraph_delete_positions(intPtr);
			return array;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00003A24 File Offset: 0x00001C24
		~SkParagraph()
		{
			this.WarnThatFinalizerIsReached<SkParagraph>();
			this.Dispose();
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00003A58 File Offset: 0x00001C58
		public void Dispose()
		{
			if (this.Instance == IntPtr.Zero)
			{
				return;
			}
			SkParagraph.API.paragraph_delete(this.Instance);
			this.Instance = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		// Token: 0x02000148 RID: 328
		[NullableContext(0)]
		private static class API
		{
			// Token: 0x06000B2A RID: 2858
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void paragraph_plan_layout(IntPtr paragraph, float availableWidth);

			// Token: 0x06000B2B RID: 2859
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void paragraph_get_line_metrics(IntPtr paragraph, out IntPtr array, out int arrayLength);

			// Token: 0x06000B2C RID: 2860
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void paragraph_delete_line_metrics(IntPtr array);

			// Token: 0x06000B2D RID: 2861
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void paragraph_get_unresolved_codepoints(IntPtr paragraph, out IntPtr array, out int arrayLength);

			// Token: 0x06000B2E RID: 2862
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void paragraph_delete_unresolved_codepoints(IntPtr array);

			// Token: 0x06000B2F RID: 2863
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void paragraph_get_placeholder_positions(IntPtr paragraph, out IntPtr array, out int arrayLength);

			// Token: 0x06000B30 RID: 2864
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void paragraph_get_text_range_positions(IntPtr paragraph, int rangeStart, int rangeEnd, out IntPtr array, out int arrayLength);

			// Token: 0x06000B31 RID: 2865
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void paragraph_delete_positions(IntPtr array);

			// Token: 0x06000B32 RID: 2866
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void paragraph_delete(IntPtr paragraph);
		}
	}
}
