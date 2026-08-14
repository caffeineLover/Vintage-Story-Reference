using System;

namespace Vintagestory.ServerMods;

internal class MapLayerBlur : MapLayerBase
{
	private int range;

	private MapLayerBase parent;

	public MapLayerBlur(long seed, MapLayerBase parent, int range)
		: base(seed)
	{
		if ((range & 1) == 0)
		{
			throw new InvalidOperationException("Range must be odd!");
		}
		this.parent = parent;
		this.range = range;
	}

	public override int[] GenLayer(int xCoord, int zCoord, int sizeX, int sizeZ)
	{
		int[] map = parent.GenLayer(xCoord, zCoord, sizeX, sizeZ);
		BoxBlurHorizontal(map, range, 0, 0, sizeX, sizeZ);
		BoxBlurVertical(map, range, 0, 0, sizeX, sizeZ);
		return map;
	}

	internal unsafe void BoxBlurHorizontal(int[] map, int range, int xStart, int yStart, int xEnd, int yEnd)
	{
		fixed (int* pixelsPre = map)
		{
			uint* pixels = (uint*)pixelsPre;
			int w = xEnd - xStart;
			int halfRange = range / 2;
			int index = yStart * w;
			uint[] newColors = new uint[w];
			for (int y = yStart; y < yEnd; y++)
			{
				int hits = 0;
				int a = 0;
				int r = 0;
				int g = 0;
				int b = 0;
				for (int x = xStart - halfRange; x < xEnd; x++)
				{
					int oldPixel = x - halfRange - 1;
					if (oldPixel >= xStart)
					{
						int col = (int)pixels[index + oldPixel];
						if (col != 0)
						{
							a -= (col >> 24) & 0xFF;
							r -= (col >> 16) & 0xFF;
							g -= (col >> 8) & 0xFF;
							b -= col & 0xFF;
						}
						hits--;
					}
					int newPixel = x + halfRange;
					if (newPixel < xEnd)
					{
						int col2 = (int)pixels[index + newPixel];
						if (col2 != 0)
						{
							a += (col2 >> 24) & 0xFF;
							r += (col2 >> 16) & 0xFF;
							g += (col2 >> 8) & 0xFF;
							b += col2 & 0xFF;
						}
						hits++;
					}
					if (x >= xStart)
					{
						uint color = (uint)(((byte)(a / hits) << 24) | ((byte)(r / hits) << 16) | ((byte)(g / hits) << 8) | (byte)(b / hits));
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

	internal unsafe void BoxBlurVertical(int[] map, int range, int xStart, int yStart, int xEnd, int yEnd)
	{
		fixed (int* pixelsPre = map)
		{
			uint* pixels = (uint*)pixelsPre;
			int w = xEnd - xStart;
			int num = yEnd - yStart;
			int halfRange = range / 2;
			uint[] newColors = new uint[num];
			int oldPixelOffset = -(halfRange + 1) * w;
			int newPixelOffset = halfRange * w;
			for (int x = xStart; x < xEnd; x++)
			{
				int hits = 0;
				int a = 0;
				int r = 0;
				int g = 0;
				int b = 0;
				int index = yStart * w - halfRange * w + x;
				for (int y = yStart - halfRange; y < yEnd; y++)
				{
					if (y - halfRange - 1 >= yStart)
					{
						int col = (int)pixels[index + oldPixelOffset];
						if (col != 0)
						{
							a -= (col >> 24) & 0xFF;
							r -= (col >> 16) & 0xFF;
							g -= (col >> 8) & 0xFF;
							b -= col & 0xFF;
						}
						hits--;
					}
					if (y + halfRange < yEnd)
					{
						int col2 = (int)pixels[index + newPixelOffset];
						if (col2 != 0)
						{
							a += (col2 >> 24) & 0xFF;
							r += (col2 >> 16) & 0xFF;
							g += (col2 >> 8) & 0xFF;
							b += col2 & 0xFF;
						}
						hits++;
					}
					if (y >= yStart)
					{
						uint color = (uint)(((byte)(a / hits) << 24) | ((byte)(r / hits) << 16) | ((byte)(g / hits) << 8) | (byte)(b / hits));
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
