using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using QuestPDF.Helpers;

namespace QuestPDF.Skia.Text
{
	// Token: 0x02000026 RID: 38
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class SkFontManager
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00003810 File Offset: 0x00001A10
		public IntPtr Instance { get; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00003818 File Offset: 0x00001A18
		public static SkFontManager Local { get; } = new SkFontManager(SkFontManager.API.font_manager_create_local(Enumerable.FirstOrDefault<string>(Settings.FontDiscoveryPaths) ?? Helpers.ApplicationFilesPath));

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000CB RID: 203 RVA: 0x0000381F File Offset: 0x00001A1F
		public static SkFontManager Global { get; } = new SkFontManager(SkFontManager.API.font_manager_create_global());

		// Token: 0x060000CC RID: 204 RVA: 0x00003826 File Offset: 0x00001A26
		private SkFontManager(IntPtr instance)
		{
			this.Instance = instance;
			SkiaAPI.EnsureNotNull(this.Instance);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00003840 File Offset: 0x00001A40
		public SkTypeface CreateTypeface(SkData data)
		{
			IntPtr intPtr = SkFontManager.API.font_manager_create_typeface(this.Instance, data.Instance);
			if (intPtr == IntPtr.Zero)
			{
				throw new Exception("Cannot decode the provided font file.");
			}
			return new SkTypeface(intPtr);
		}

		// Token: 0x02000147 RID: 327
		[NullableContext(0)]
		private static class API
		{
			// Token: 0x06000B27 RID: 2855
			[NullableContext(1)]
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr font_manager_create_local(string path);

			// Token: 0x06000B28 RID: 2856
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr font_manager_create_global();

			// Token: 0x06000B29 RID: 2857
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr font_manager_create_typeface(IntPtr fontManager, IntPtr fontData);
		}
	}
}
