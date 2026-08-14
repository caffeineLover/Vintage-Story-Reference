using System;

namespace UglyToad.PdfPig.Images.Png
{
	// Token: 0x020000F9 RID: 249
	internal class Palette
	{
		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x0001C06D File Offset: 0x0001A26D
		// (set) Token: 0x060004AB RID: 1195 RVA: 0x0001C075 File Offset: 0x0001A275
		public bool HasAlphaValues { get; private set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x0001C07E File Offset: 0x0001A27E
		public byte[] Data { get; }

		// Token: 0x060004AD RID: 1197 RVA: 0x0001C088 File Offset: 0x0001A288
		public Palette(byte[] data)
		{
			this.Data = new byte[data.Length * 4 / 3];
			int num = 0;
			for (int i = 0; i < data.Length; i += 3)
			{
				this.Data[num++] = data[i];
				this.Data[num++] = data[i + 1];
				this.Data[num++] = data[i + 2];
				this.Data[num++] = byte.MaxValue;
			}
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0001C100 File Offset: 0x0001A300
		public void SetAlphaValues(byte[] bytes)
		{
			this.HasAlphaValues = true;
			for (int i = 0; i < bytes.Length; i++)
			{
				this.Data[i * 4 + 3] = bytes[i];
			}
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0001C134 File Offset: 0x0001A334
		public Pixel GetPixel(int index)
		{
			int num = index * 4;
			return new Pixel(this.Data[num], this.Data[num + 1], this.Data[num + 2], this.Data[num + 3], false);
		}
	}
}
