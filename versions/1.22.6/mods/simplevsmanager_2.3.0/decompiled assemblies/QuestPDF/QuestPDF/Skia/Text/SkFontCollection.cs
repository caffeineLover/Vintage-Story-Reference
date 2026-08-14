using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace QuestPDF.Skia.Text
{
	// Token: 0x02000025 RID: 37
	internal sealed class SkFontCollection : IDisposable
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x0000376C File Offset: 0x0000196C
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00003774 File Offset: 0x00001974
		public IntPtr Instance { get; private set; }

		// Token: 0x060000C5 RID: 197 RVA: 0x0000377D File Offset: 0x0000197D
		public SkFontCollection(IntPtr instance)
		{
			this.Instance = instance;
			SkiaAPI.EnsureNotNull(this.Instance);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00003797 File Offset: 0x00001997
		[NullableContext(1)]
		public static SkFontCollection Create(SkTypefaceProvider typefaceProvider, SkFontManager fontManager)
		{
			return new SkFontCollection(SkFontCollection.API.font_collection_create(fontManager.Instance, typefaceProvider.Instance));
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000037B0 File Offset: 0x000019B0
		~SkFontCollection()
		{
			this.WarnThatFinalizerIsReached<SkFontCollection>();
			this.Dispose();
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000037E4 File Offset: 0x000019E4
		public void Dispose()
		{
			if (this.Instance == IntPtr.Zero)
			{
				return;
			}
			SkFontCollection.API.font_collection_unref(this.Instance);
			this.Instance = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		// Token: 0x02000146 RID: 326
		private static class API
		{
			// Token: 0x06000B25 RID: 2853
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr font_collection_create(IntPtr fontManager, IntPtr typefaceProvider);

			// Token: 0x06000B26 RID: 2854
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void font_collection_unref(IntPtr fontCollection);
		}
	}
}
