namespace Vintagestory.GameContent;

public class BlurTool
{
	public static void Blur(byte[] data, int sizeX, int sizeZ, int range)
	{
		BoxBlurHorizontal(data, range, 0, 0, sizeX, sizeZ);
		BoxBlurVertical(data, range, 0, 0, sizeX, sizeZ);
	}

	public unsafe static void BoxBlurHorizontal(byte[] map, int range, int xStart, int yStart, int xEnd, int yEnd)
	{
		fixed (byte* pixels = map)
		{
			int w = xEnd - xStart;
			int halfRange = range / 2;
			int index = yStart * w;
			byte[] newColors = new byte[w];
			for (int y = yStart; y < yEnd; y++)
			{
				int hits = 0;
				int r = 0;
				for (int x = xStart - halfRange; x < xEnd; x++)
				{
					int oldPixel = x - halfRange - 1;
					if (oldPixel >= xStart)
					{
						byte col = pixels[index + oldPixel];
						if (col != 0)
						{
							r -= col;
						}
						hits--;
					}
					int newPixel = x + halfRange;
					if (newPixel < xEnd)
					{
						byte col2 = pixels[index + newPixel];
						if (col2 != 0)
						{
							r += col2;
						}
						hits++;
					}
					if (x >= xStart)
					{
						byte color = (byte)(r / hits);
						newColors[x] = color;
					}
				}
				for (int i = xStart; i < xEnd; i++)
				{
					pixels[index + i] = newColors[i];
				}
				index += w;
			}
		}
	}

	public unsafe static void BoxBlurVertical(byte[] map, int range, int xStart, int yStart, int xEnd, int yEnd)
	{
		fixed (byte* pixels = map)
		{
			int w = xEnd - xStart;
			int num = yEnd - yStart;
			int halfRange = range / 2;
			byte[] newColors = new byte[num];
			int oldPixelOffset = -(halfRange + 1) * w;
			int newPixelOffset = halfRange * w;
			for (int x = xStart; x < xEnd; x++)
			{
				int hits = 0;
				int r = 0;
				int index = yStart * w - halfRange * w + x;
				for (int y = yStart - halfRange; y < yEnd; y++)
				{
					if (y - halfRange - 1 >= yStart)
					{
						byte col = pixels[index + oldPixelOffset];
						if (col != 0)
						{
							r -= col;
						}
						hits--;
					}
					if (y + halfRange < yEnd)
					{
						byte col2 = pixels[index + newPixelOffset];
						if (col2 != 0)
						{
							r += col2;
						}
						hits++;
					}
					if (y >= yStart)
					{
						byte color = (byte)(r / hits);
						newColors[y] = color;
					}
					index += w;
				}
				for (int i = yStart; i < yEnd; i++)
				{
					pixels[i * w + x] = newColors[i];
				}
			}
		}
	}
}
