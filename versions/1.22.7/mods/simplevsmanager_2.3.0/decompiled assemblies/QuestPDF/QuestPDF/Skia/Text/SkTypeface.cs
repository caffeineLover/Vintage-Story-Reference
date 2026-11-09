using System;
using System.Runtime.InteropServices;

namespace QuestPDF.Skia.Text
{
	// Token: 0x0200002E RID: 46
	internal sealed class SkTypeface : IDisposable
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00004194 File Offset: 0x00002394
		// (set) Token: 0x06000108 RID: 264 RVA: 0x0000419C File Offset: 0x0000239C
		public IntPtr Instance { get; private set; }

		// Token: 0x06000109 RID: 265 RVA: 0x000041A5 File Offset: 0x000023A5
		public SkTypeface(IntPtr instance)
		{
			this.Instance = instance;
			SkiaAPI.EnsureNotNull(this.Instance);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000041C0 File Offset: 0x000023C0
		~SkTypeface()
		{
			this.WarnThatFinalizerIsReached<SkTypeface>();
			this.Dispose();
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000041F4 File Offset: 0x000023F4
		public void Dispose()
		{
			if (this.Instance == IntPtr.Zero)
			{
				return;
			}
			SkTypeface.API.typeface_unref(this.Instance);
			this.Instance = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		// Token: 0x02000154 RID: 340
		private static class API
		{
			// Token: 0x06000B3B RID: 2875
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void typeface_unref(IntPtr typeface);
		}
	}
}
