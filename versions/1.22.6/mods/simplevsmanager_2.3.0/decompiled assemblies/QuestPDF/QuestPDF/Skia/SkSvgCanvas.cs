using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace QuestPDF.Skia
{
	// Token: 0x0200001E RID: 30
	internal sealed class SkSvgCanvas
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x000033C1 File Offset: 0x000015C1
		[NullableContext(1)]
		public static SkCanvas CreateSvg(float width, float height, SkWriteStream writeStream)
		{
			return new SkCanvas(SkSvgCanvas.API.svg_create_canvas(new SkRect(0f, 0f, width, height), writeStream.Instance), true);
		}

		// Token: 0x02000140 RID: 320
		private static class API
		{
			// Token: 0x06000B1B RID: 2843
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr svg_create_canvas(SkRect bounds, IntPtr writeStream);
		}
	}
}
