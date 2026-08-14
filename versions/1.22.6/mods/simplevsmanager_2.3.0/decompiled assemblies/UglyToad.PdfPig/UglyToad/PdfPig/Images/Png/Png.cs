using System;
using System.IO;

namespace UglyToad.PdfPig.Images.Png
{
	// Token: 0x020000FB RID: 251
	internal class Png
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x0001C3D9 File Offset: 0x0001A5D9
		public ImageHeader Header { get; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x0001C3E4 File Offset: 0x0001A5E4
		public int Width
		{
			get
			{
				return this.Header.Width;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x0001C400 File Offset: 0x0001A600
		public int Height
		{
			get
			{
				return this.Header.Height;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x0001C41C File Offset: 0x0001A61C
		public bool HasAlphaChannel
		{
			get
			{
				return (this.Header.ColorType & ColorType.AlphaChannelUsed) != ColorType.None || this.hasTransparencyChunk;
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0001C443 File Offset: 0x0001A643
		internal Png(ImageHeader header, RawPngData data, bool hasTransparencyChunk)
		{
			this.Header = header;
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			this.data = data;
			this.hasTransparencyChunk = hasTransparencyChunk;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0001C46F File Offset: 0x0001A66F
		public Pixel GetPixel(int x, int y)
		{
			return this.data.GetPixel(x, y);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0001C47E File Offset: 0x0001A67E
		public static Png Open(Stream stream, IChunkVisitor chunkVisitor = null)
		{
			return PngOpener.Open(stream, chunkVisitor);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0001C488 File Offset: 0x0001A688
		public static Png Open(byte[] bytes, IChunkVisitor chunkVisitor = null)
		{
			Png result;
			using (MemoryStream memoryStream = new MemoryStream(bytes))
			{
				result = PngOpener.Open(memoryStream, chunkVisitor);
			}
			return result;
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0001C4C4 File Offset: 0x0001A6C4
		public static Png Open(string filePath, IChunkVisitor chunkVisitor = null)
		{
			Png result;
			using (FileStream fileStream = File.OpenRead(filePath))
			{
				result = Png.Open(fileStream, chunkVisitor);
			}
			return result;
		}

		// Token: 0x04000290 RID: 656
		private readonly RawPngData data;

		// Token: 0x04000291 RID: 657
		private readonly bool hasTransparencyChunk;
	}
}
