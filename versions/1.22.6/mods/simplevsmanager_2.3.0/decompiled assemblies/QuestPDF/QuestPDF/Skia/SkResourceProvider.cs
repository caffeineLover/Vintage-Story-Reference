using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using QuestPDF.Helpers;
using QuestPDF.Skia.Text;

namespace QuestPDF.Skia
{
	// Token: 0x0200001A RID: 26
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class SkResourceProvider
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600009B RID: 155 RVA: 0x0000332D File Offset: 0x0000152D
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00003335 File Offset: 0x00001535
		public IntPtr Instance { get; private set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600009D RID: 157 RVA: 0x0000333E File Offset: 0x0000153E
		public static SkResourceProvider Local { get; } = new SkResourceProvider(SkFontManager.Local);

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00003345 File Offset: 0x00001545
		public static SkResourceProvider Global { get; } = new SkResourceProvider(SkFontManager.Global);

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600009F RID: 159 RVA: 0x0000334C File Offset: 0x0000154C
		internal static SkResourceProvider CurrentResourceProvider
		{
			get
			{
				if (!Settings.UseEnvironmentFonts)
				{
					return SkResourceProvider.Local;
				}
				return SkResourceProvider.Global;
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003360 File Offset: 0x00001560
		private SkResourceProvider(SkFontManager fontManager)
		{
			this.Instance = SkResourceProvider.API.resource_provider_create(Helpers.ApplicationFilesPath, fontManager.Instance);
			SkiaAPI.EnsureNotNull(this.Instance);
		}

		// Token: 0x0200013F RID: 319
		[NullableContext(0)]
		private static class API
		{
			// Token: 0x06000B1A RID: 2842
			[NullableContext(1)]
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr resource_provider_create(string resourcesPath, IntPtr fontManager);
		}
	}
}
