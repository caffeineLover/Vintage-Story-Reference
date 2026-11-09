using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Graphics.Colors;

namespace UglyToad.PdfPig.Images.Png
{
	// Token: 0x020000FD RID: 253
	internal static class PngFromPdfImageFactory
	{
		// Token: 0x060004CC RID: 1228 RVA: 0x0001C804 File Offset: 0x0001AA04
		public static bool TryGenerate(IPdfImage image, out byte[] bytes)
		{
			bytes = null;
			IReadOnlyList<byte> readOnlyList;
			if (image.ColorSpaceDetails == null || image.ColorSpaceDetails is UnsupportedColorSpaceDetails || image.ColorSpaceDetails.BaseType == ColorSpace.Pattern || !image.TryGetBytes(out readOnlyList))
			{
				return false;
			}
			try
			{
				readOnlyList = ColorSpaceDetailsByteConverter.Convert(image.ColorSpaceDetails, readOnlyList, image.BitsPerComponent, image.WidthInSamples, image.HeightInSamples);
				int baseNumberOfColorComponents = image.ColorSpaceDetails.BaseNumberOfColorComponents;
				bool flag = baseNumberOfColorComponents == 3;
				PngBuilder pngBuilder = PngBuilder.Create(image.WidthInSamples, image.HeightInSamples, false);
				int num = image.WidthInSamples * image.HeightInSamples * baseNumberOfColorComponents;
				int count = readOnlyList.Count;
				if (readOnlyList.Count != num && (count != num + 1 || readOnlyList[count - 1] != 10) && (count != num + 1 || readOnlyList[count - 1] != 13) && (count != num + 2 || readOnlyList[count - 2] != 13 || readOnlyList[count - 1] != 10))
				{
					return false;
				}
				if (image.ColorSpaceDetails.BaseType == ColorSpace.DeviceCMYK || baseNumberOfColorComponents == 4)
				{
					int num2 = 0;
					for (int i = 0; i < image.HeightInSamples; i++)
					{
						for (int j = 0; j < image.WidthInSamples; j++)
						{
							double num3 = (double)readOnlyList[num2++] / 255.0;
							double num4 = (double)readOnlyList[num2++] / 255.0;
							double num5 = (double)readOnlyList[num2++] / 255.0;
							double num6 = (double)readOnlyList[num2++] / 255.0;
							byte r = (byte)(255.0 * (1.0 - num3) * (1.0 - num6));
							byte g = (byte)(255.0 * (1.0 - num4) * (1.0 - num6));
							byte b = (byte)(255.0 * (1.0 - num5) * (1.0 - num6));
							pngBuilder.SetPixel(r, g, b, j, i);
						}
					}
				}
				else if (flag)
				{
					int num7 = 0;
					for (int k = 0; k < image.HeightInSamples; k++)
					{
						for (int l = 0; l < image.WidthInSamples; l++)
						{
							pngBuilder.SetPixel(readOnlyList[num7++], readOnlyList[num7++], readOnlyList[num7++], l, k);
						}
					}
				}
				else
				{
					int num8 = 0;
					for (int m = 0; m < image.HeightInSamples; m++)
					{
						for (int n = 0; n < image.WidthInSamples; n++)
						{
							byte b2 = readOnlyList[num8++];
							pngBuilder.SetPixel(b2, b2, b2, n, m);
						}
					}
				}
				bytes = pngBuilder.Save();
				return true;
			}
			catch
			{
			}
			return false;
		}
	}
}
