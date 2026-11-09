using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace QuestPDF.Skia
{
	// Token: 0x02000006 RID: 6
	internal sealed class SkBitmap : IDisposable
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000023B9 File Offset: 0x000005B9
		// (set) Token: 0x0600001F RID: 31 RVA: 0x000023C1 File Offset: 0x000005C1
		public IntPtr Instance { get; private set; }

		// Token: 0x06000020 RID: 32 RVA: 0x000023CA File Offset: 0x000005CA
		public SkBitmap(int width, int height)
		{
			this.Instance = SkBitmap.API.bitmap_create(width, height);
			SkiaAPI.EnsureNotNull(this.Instance);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000023EA File Offset: 0x000005EA
		[NullableContext(1)]
		public SkData EncodeAsJpeg(int quality)
		{
			return new SkData(SkBitmap.API.bitmap_encode_as_jpg(this.Instance, quality));
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000023FD File Offset: 0x000005FD
		[NullableContext(1)]
		public SkData EncodeAsPng()
		{
			return new SkData(SkBitmap.API.bitmap_encode_as_png(this.Instance));
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000240F File Offset: 0x0000060F
		[NullableContext(1)]
		public SkData EncodeAsWebp(int quality)
		{
			return new SkData(SkBitmap.API.bitmap_encode_as_webp(this.Instance, quality));
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002424 File Offset: 0x00000624
		~SkBitmap()
		{
			this.WarnThatFinalizerIsReached<SkBitmap>();
			this.Dispose();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002458 File Offset: 0x00000658
		public void Dispose()
		{
			if (this.Instance == IntPtr.Zero)
			{
				return;
			}
			SkBitmap.API.bitmap_delete(this.Instance);
			this.Instance = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		// Token: 0x02000132 RID: 306
		private static class API
		{
			// Token: 0x06000ACF RID: 2767
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr bitmap_create(int width, int height);

			// Token: 0x06000AD0 RID: 2768
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void bitmap_delete(IntPtr image);

			// Token: 0x06000AD1 RID: 2769
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr bitmap_encode_as_jpg(IntPtr image, int quality);

			// Token: 0x06000AD2 RID: 2770
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr bitmap_encode_as_png(IntPtr image);

			// Token: 0x06000AD3 RID: 2771
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr bitmap_encode_as_webp(IntPtr image, int quality);
		}
	}
}
