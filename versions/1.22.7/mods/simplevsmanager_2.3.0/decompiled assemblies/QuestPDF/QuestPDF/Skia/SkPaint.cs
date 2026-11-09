using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using QuestPDF.Infrastructure;

namespace QuestPDF.Skia
{
	// Token: 0x02000010 RID: 16
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class SkPaint : IDisposable
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00002C89 File Offset: 0x00000E89
		// (set) Token: 0x0600006B RID: 107 RVA: 0x00002C91 File Offset: 0x00000E91
		public IntPtr Instance { get; private set; }

		// Token: 0x0600006C RID: 108 RVA: 0x00002C9A File Offset: 0x00000E9A
		public SkPaint()
		{
			this.Instance = SkPaint.API.paint_create();
			SkiaAPI.EnsureNotNull(this.Instance);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002CB8 File Offset: 0x00000EB8
		public void SetSolidColor(uint color)
		{
			SkPaint.API.paint_set_solid_color(this.Instance, color);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002CC8 File Offset: 0x00000EC8
		public void SetLinearGradient(Position start, Position end, Color[] colors)
		{
			if (colors.Length == 0)
			{
				throw new ArgumentException("At least one color must be provided to create a gradient.", "colors");
			}
			SkPoint start2 = new SkPoint(start.X, start.Y);
			SkPoint end2 = new SkPoint(end.X, end.Y);
			uint[] array = Enumerable.ToArray<uint>(Enumerable.Select<Color, uint>(colors, (Color c) => c.Hex));
			SkPaint.API.paint_set_linear_gradient(this.Instance, start2, end2, array.Length, array);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002D4B File Offset: 0x00000F4B
		public void SetStroke(float thickness)
		{
			SkPaint.API.paint_set_stroke(this.Instance, thickness);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002D59 File Offset: 0x00000F59
		public void SetDashedPathEffect(float[] intervals)
		{
			if (intervals.Length == 0)
			{
				throw new ArgumentException("At least one interval must be provided to create a dashed path effect.", "intervals");
			}
			if (intervals.Length % 2 != 0)
			{
				throw new ArgumentException("The intervals array must contain an even number of elements.", "intervals");
			}
			SkPaint.API.paint_set_dashed_path_effect(this.Instance, intervals.Length, intervals);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00002D98 File Offset: 0x00000F98
		~SkPaint()
		{
			this.WarnThatFinalizerIsReached<SkPaint>();
			this.Dispose();
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00002DCC File Offset: 0x00000FCC
		public void Dispose()
		{
			if (this.Instance == IntPtr.Zero)
			{
				return;
			}
			SkPaint.API.paint_delete(this.Instance);
			this.Instance = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		// Token: 0x02000138 RID: 312
		[Nullable(0)]
		private static class API
		{
			// Token: 0x06000AFE RID: 2814
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr paint_create();

			// Token: 0x06000AFF RID: 2815
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void paint_delete(IntPtr paint);

			// Token: 0x06000B00 RID: 2816
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void paint_set_solid_color(IntPtr paint, uint color);

			// Token: 0x06000B01 RID: 2817
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void paint_set_linear_gradient(IntPtr paint, SkPoint start, SkPoint end, int colorsLength, uint[] colors);

			// Token: 0x06000B02 RID: 2818
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void paint_set_stroke(IntPtr paint, float thickness);

			// Token: 0x06000B03 RID: 2819
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void paint_set_dashed_path_effect(IntPtr paint, int arrayLength, float[] intervals);
		}
	}
}
