using SkiaSharp;

namespace Vintagestory.API.Common;

public class BakedBitmap : IBitmap
{
	public int[] TexturePixels;

	public int Width;

	public int Height;

	public int[] Pixels => TexturePixels;

	int IBitmap.Width => Width;

	int IBitmap.Height => Height;

	public SKColor GetPixel(int x, int y)
	{
		return new SKColor((uint)TexturePixels[Width * y + x]);
	}

	public int GetPixelArgb(int x, int y)
	{
		return TexturePixels[Width * y + x];
	}

	public SKColor GetPixelRel(float x, float y)
	{
		return new SKColor((uint)TexturePixels[Width * (int)(y * (float)Height) + (int)(x * (float)Width)]);
	}

	public int[] GetPixelsTransformed(int rot = 0, int alpha = 100)
	{
		int[] bmpPixels = new int[Width * Height];
		switch (rot)
		{
		case 0:
		{
			for (int m = 0; m < Width; m++)
			{
				for (int n = 0; n < Height; n++)
				{
					bmpPixels[m + n * Width] = GetPixelArgb(m, n);
				}
			}
			break;
		}
		case 90:
		{
			for (int i = 0; i < Width; i++)
			{
				for (int j = 0; j < Height; j++)
				{
					bmpPixels[j + i * Width] = GetPixelArgb(Width - i - 1, j);
				}
			}
			break;
		}
		case 180:
		{
			for (int k = 0; k < Width; k++)
			{
				for (int l = 0; l < Height; l++)
				{
					bmpPixels[k + l * Width] = GetPixelArgb(Width - k - 1, Height - l - 1);
				}
			}
			break;
		}
		case 270:
		{
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					bmpPixels[y + x * Width] = GetPixelArgb(x, Height - y - 1);
				}
			}
			break;
		}
		}
		if (alpha != 100)
		{
			float af = (float)alpha / 100f;
			for (int num = 0; num < bmpPixels.Length; num++)
			{
				int current = bmpPixels[num];
				int currAlpha = (current >> 24) & 0xFF;
				bmpPixels[num] = current | ((int)((float)currAlpha * af) << 24);
			}
		}
		return bmpPixels;
	}
}
