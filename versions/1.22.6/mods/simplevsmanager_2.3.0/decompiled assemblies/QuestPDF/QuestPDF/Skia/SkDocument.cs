using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace QuestPDF.Skia
{
	// Token: 0x0200000C RID: 12
	internal sealed class SkDocument : IDisposable
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000052 RID: 82 RVA: 0x000029D1 File Offset: 0x00000BD1
		// (set) Token: 0x06000053 RID: 83 RVA: 0x000029D9 File Offset: 0x00000BD9
		public IntPtr Instance { get; private set; }

		// Token: 0x06000054 RID: 84 RVA: 0x000029E2 File Offset: 0x00000BE2
		internal SkDocument(IntPtr instance)
		{
			this.Instance = instance;
			SkiaAPI.EnsureNotNull(this.Instance);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000029FC File Offset: 0x00000BFC
		[NullableContext(1)]
		public SkCanvas BeginPage(float width, float height)
		{
			return new SkCanvas(SkDocument.API.document_begin_page(this.Instance, width, height), false);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002A11 File Offset: 0x00000C11
		public void EndPage()
		{
			SkDocument.API.document_end_page(this.Instance);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002A1E File Offset: 0x00000C1E
		public void Close()
		{
			SkDocument.API.document_close(this.Instance);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002A2C File Offset: 0x00000C2C
		~SkDocument()
		{
			this.WarnThatFinalizerIsReached<SkDocument>();
			this.Dispose();
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002A60 File Offset: 0x00000C60
		public void Dispose()
		{
			if (this.Instance == IntPtr.Zero)
			{
				return;
			}
			SkDocument.API.document_unref(this.Instance);
			this.Instance = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		// Token: 0x02000135 RID: 309
		private static class API
		{
			// Token: 0x06000AF2 RID: 2802
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr document_begin_page(IntPtr document, float width, float height);

			// Token: 0x06000AF3 RID: 2803
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void document_end_page(IntPtr document);

			// Token: 0x06000AF4 RID: 2804
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void document_close(IntPtr document);

			// Token: 0x06000AF5 RID: 2805
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void document_unref(IntPtr document);
		}
	}
}
