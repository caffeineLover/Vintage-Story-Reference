using System;
using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Graphics.Colors;

namespace UglyToad.PdfPig.Images
{
	// Token: 0x020000E8 RID: 232
	public static class ColorSpaceDetailsByteConverter
	{
		// Token: 0x0600046E RID: 1134 RVA: 0x0001AEC0 File Offset: 0x000190C0
		public static byte[] Convert(ColorSpaceDetails details, IReadOnlyList<byte> decoded, int bitsPerComponent, int imageWidth, int imageHeight)
		{
			if (decoded == null)
			{
				return EmptyArray<byte>.Instance;
			}
			if (details == null)
			{
				return Enumerable.ToArray<byte>(decoded);
			}
			if (bitsPerComponent != 8)
			{
				decoded = ColorSpaceDetailsByteConverter.UnpackComponents(decoded, bitsPerComponent);
			}
			int numberOfColorComponents = details.NumberOfColorComponents;
			int num = decoded.Count / imageHeight / numberOfColorComponents;
			if (num != imageWidth)
			{
				decoded = ColorSpaceDetailsByteConverter.RemoveStridePadding(Enumerable.ToArray<byte>(decoded), num, imageWidth, imageHeight, numberOfColorComponents);
			}
			decoded = details.Transform(decoded);
			return Enumerable.ToArray<byte>(decoded);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0001AF28 File Offset: 0x00019128
		private static byte[] UnpackComponents(IReadOnlyList<byte> input, int bitsPerComponent)
		{
			ColorSpaceDetailsByteConverter.<>c__DisplayClass1_0 CS$<>8__locals1 = new ColorSpaceDetailsByteConverter.<>c__DisplayClass1_0();
			CS$<>8__locals1.bitsPerComponent = bitsPerComponent;
			return Enumerable.ToArray<byte>(Enumerable.SelectMany<byte, byte>(input, new Func<byte, IEnumerable<byte>>(CS$<>8__locals1.<UnpackComponents>g__Unpack|0)));
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0001AF5C File Offset: 0x0001915C
		private static byte[] RemoveStridePadding(byte[] input, int strideWidth, int imageWidth, int imageHeight, int multiplier)
		{
			byte[] array = new byte[imageWidth * imageHeight * multiplier];
			for (int i = 0; i < imageHeight; i++)
			{
				int num = i * strideWidth;
				int num2 = i * imageWidth;
				Array.Copy(input, num, array, num2, imageWidth);
			}
			return array;
		}
	}
}
