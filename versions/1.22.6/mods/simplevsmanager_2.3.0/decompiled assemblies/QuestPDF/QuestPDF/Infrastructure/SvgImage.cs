using System;
using System.IO;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Drawing.Exceptions;
using QuestPDF.Helpers;
using QuestPDF.Skia;

namespace QuestPDF.Infrastructure
{
	// Token: 0x02000065 RID: 101
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SvgImage : IDisposable
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600026A RID: 618 RVA: 0x00005C4A File Offset: 0x00003E4A
		internal SkSvgImage SkSvgImage { get; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600026B RID: 619 RVA: 0x00005C52 File Offset: 0x00003E52
		// (set) Token: 0x0600026C RID: 620 RVA: 0x00005C5A File Offset: 0x00003E5A
		internal bool IsShared { get; set; } = true;

		// Token: 0x0600026D RID: 621 RVA: 0x00005C63 File Offset: 0x00003E63
		private SvgImage(string content)
		{
			this.SkSvgImage = new SkSvgImage(content, SkResourceProvider.CurrentResourceProvider, FontManager.CurrentFontManager);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00005C88 File Offset: 0x00003E88
		~SvgImage()
		{
			this.WarnThatFinalizerIsReached<SvgImage>();
			this.Dispose();
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00005CBC File Offset: 0x00003EBC
		public void Dispose()
		{
			SkSvgImage skSvgImage = this.SkSvgImage;
			if (skSvgImage != null)
			{
				skSvgImage.Dispose();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00005CD5 File Offset: 0x00003ED5
		public static SvgImage FromFile(string filePath)
		{
			if (!File.Exists(filePath))
			{
				string text = Path.Combine(Helpers.ApplicationFilesPath, filePath);
				if (!File.Exists(text))
				{
					throw new DocumentComposeException("Cannot load an SVG image under the provided path, file not found: $" + filePath);
				}
				filePath = text;
			}
			return new SvgImage(File.ReadAllText(filePath));
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00005D10 File Offset: 0x00003F10
		public static SvgImage FromText(string svg)
		{
			return new SvgImage(svg);
		}
	}
}
