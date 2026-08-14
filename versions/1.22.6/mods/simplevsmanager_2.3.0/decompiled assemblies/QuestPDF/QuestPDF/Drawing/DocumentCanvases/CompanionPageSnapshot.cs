using System;
using System.Runtime.CompilerServices;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Drawing.DocumentCanvases
{
	// Token: 0x02000123 RID: 291
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class CompanionPageSnapshot
	{
		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000A3A RID: 2618 RVA: 0x0001A775 File Offset: 0x00018975
		// (set) Token: 0x06000A3B RID: 2619 RVA: 0x0001A77D File Offset: 0x0001897D
		public SkPicture Picture { get; set; }

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x0001A786 File Offset: 0x00018986
		// (set) Token: 0x06000A3D RID: 2621 RVA: 0x0001A78E File Offset: 0x0001898E
		public Size Size { get; set; }

		// Token: 0x06000A3E RID: 2622 RVA: 0x0001A797 File Offset: 0x00018997
		public CompanionPageSnapshot(SkPicture picture, Size size)
		{
			this.Picture = picture;
			this.Size = size;
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x0001A7B0 File Offset: 0x000189B0
		public byte[] RenderImage(int zoomLevel)
		{
			float num = (float)Math.Pow(2.0, (double)zoomLevel);
			byte[] result;
			using (SkBitmap skBitmap = new SkBitmap((int)(this.Size.Width * num), (int)(this.Size.Height * num)))
			{
				using (SkCanvas skCanvas = SkCanvas.CreateFromBitmap(skBitmap))
				{
					skCanvas.Scale(num, num);
					using (SkPaint skPaint = new SkPaint())
					{
						skPaint.SetSolidColor(Colors.White);
						SkRect position = new SkRect(0f, 0f, this.Size.Width, this.Size.Height);
						skCanvas.DrawRectangle(position, skPaint);
						skCanvas.DrawPicture(this.Picture);
						using (SkData skData = skBitmap.EncodeAsJpeg(90))
						{
							result = skData.ToBytes();
						}
					}
				}
			}
			return result;
		}
	}
}
