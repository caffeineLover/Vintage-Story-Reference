using System;
using System.IO;
using SkiaSharp;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.API.Common;

public class BitmapExternal : BitmapRef
{
	public SKBitmap bmp;

	public override int Height => bmp.Height;

	public override int Width => bmp.Width;

	public override int[] Pixels => Array.ConvertAll(bmp.Pixels, (SKColor p) => (int)(uint)p);

	public nint PixelsPtrAndLock => bmp.GetPixels();

	[Obsolete("This requires to manually set the underlying SKBitmap, prefer other overloads.")]
	public BitmapExternal()
	{
	}

	public BitmapExternal(SKBitmap bmp)
	{
		this.bmp = bmp;
	}

	public BitmapExternal(int width, int height)
	{
		bmp = new SKBitmap(width, height);
	}

	public BitmapExternal(MemoryStream ms, ILogger logger, AssetLocation? loc = null)
	{
		try
		{
			bmp = Decode(ms.ToArray());
		}
		catch (Exception e)
		{
			if (loc != null)
			{
				logger.Error("Failed loading bitmap from png file {0}. Will default to an empty 1x1 bitmap.", loc);
				logger.Error(e);
			}
			else
			{
				logger.Error("Failed loading bitmap. Will default to an empty 1x1 bitmap.");
				logger.Error(e);
			}
			bmp = new SKBitmap(1, 1);
			bmp.SetPixel(0, 0, SKColors.Orange);
		}
	}

	public BitmapExternal(string filePath, ILogger? logger = null)
	{
		try
		{
			bmp = Decode(File.ReadAllBytes(filePath));
		}
		catch (Exception e)
		{
			logger?.Error("Failed loading bitmap from data. Will default to an empty 1x1 bitmap.");
			logger?.Error(e);
			bmp = new SKBitmap(1, 1);
			bmp.SetPixel(0, 0, SKColors.Orange);
		}
	}

	public BitmapExternal(Stream stream, ILogger? logger = null)
	{
		try
		{
			byte[] buffer = new byte[stream.Length];
			stream.ReadExactly(buffer);
			bmp = Decode(buffer);
		}
		catch (Exception e)
		{
			logger?.Error("Failed loading bitmap from data. Will default to an empty 1x1 bitmap.");
			logger?.Error(e);
			bmp = new SKBitmap(1, 1);
			bmp.SetPixel(0, 0, SKColors.Orange);
		}
	}

	/// <summary>
	/// Create a BitmapExternal from a byte array
	/// </summary>
	public BitmapExternal(byte[] data, int dataLength, ILogger logger)
	{
		try
		{
			bmp = Decode(data.AsSpan().Slice(0, dataLength));
		}
		catch (Exception e)
		{
			logger.Error("Failed loading bitmap from data. Will default to an empty 1x1 bitmap.");
			logger.Error(e);
			bmp = new SKBitmap(1, 1);
			bmp.SetPixel(0, 0, SKColors.Orange);
		}
	}

	public unsafe static SKBitmap Decode(ReadOnlySpan<byte> buffer)
	{
		fixed (byte* address = buffer)
		{
			using SKData data = SKData.Create((nint)address, buffer.Length);
			using SKCodec codec = SKCodec.Create(data);
			SKImageInfo bitmapInfo = codec.Info;
			bitmapInfo.AlphaType = SKAlphaType.Unpremul;
			bitmapInfo.ColorType = SKColorType.Bgra8888;
			SKBitmap bitmap = new SKBitmap(bitmapInfo);
			nint length;
			SKCodecResult result = codec.GetPixels(bitmapInfo, bitmap.GetPixels(out length));
			if (result != SKCodecResult.Success)
			{
				bitmap.Dispose();
				throw new InvalidDataException($"Failed to decode bitmap from data with error: {result}");
			}
			return bitmap;
		}
	}

	public override void Dispose()
	{
		bmp.Dispose();
	}

	public override void Save(string filename)
	{
		bmp.Save(filename);
	}

	/// <summary>
	/// Retrives the ARGB value from given coordinate
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public override SKColor GetPixel(int x, int y)
	{
		return bmp.GetPixel(x, y);
	}

	/// <summary>
	/// Retrives the ARGB value from given coordinate using normalized coordinates (0..1)
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public override SKColor GetPixelRel(float x, float y)
	{
		return bmp.GetPixel((int)Math.Min(bmp.Width - 1, x * (float)bmp.Width), (int)Math.Min(bmp.Height - 1, y * (float)bmp.Height));
	}

	public unsafe override void MulAlpha(int alpha = 255)
	{
		int len = Width * Height;
		float af = (float)alpha / 255f;
		byte* colp = (byte*)((IntPtr)bmp.GetPixels()).ToPointer();
		for (int i = 0; i < len; i++)
		{
			int a = colp[3];
			*colp = (byte)((float)(int)(*colp) * af);
			colp[1] = (byte)((float)(int)colp[1] * af);
			colp[2] = (byte)((float)(int)colp[2] * af);
			colp[3] = (byte)((float)a * af);
			colp += 4;
		}
	}

	public override int[] GetPixelsTransformed(int rot = 0, int mulAlpha = 255)
	{
		int[] bmpPixels = new int[Width * Height];
		int width = bmp.Width;
		int height = bmp.Height;
		FastBitmap fastBitmap = new FastBitmap();
		fastBitmap.bmp = bmp;
		int stride = fastBitmap.Stride;
		switch (rot)
		{
		case 0:
		{
			for (int m = 0; m < height; m++)
			{
				fastBitmap.GetPixelRow(width, m * stride, bmpPixels, m * width);
			}
			break;
		}
		case 90:
		{
			for (int i = 0; i < width; i++)
			{
				int baseY2 = i * width;
				for (int j = 0; j < height; j++)
				{
					bmpPixels[j + baseY2] = fastBitmap.GetPixel(width - i - 1, j * stride);
				}
			}
			break;
		}
		case 180:
		{
			for (int k = 0; k < height; k++)
			{
				int baseX = k * width;
				int yStride = (height - k - 1) * stride;
				for (int l = 0; l < width; l++)
				{
					bmpPixels[l + baseX] = fastBitmap.GetPixel(width - l - 1, yStride);
				}
			}
			break;
		}
		case 270:
		{
			for (int x = 0; x < width; x++)
			{
				int baseY = x * width;
				for (int y = 0; y < height; y++)
				{
					bmpPixels[y + baseY] = fastBitmap.GetPixel(x, (height - y - 1) * stride);
				}
			}
			break;
		}
		}
		if (mulAlpha != 255)
		{
			float alpaP = (float)mulAlpha / 255f;
			int clearAlpha = 16777215;
			for (int n = 0; n < bmpPixels.Length; n++)
			{
				int col = bmpPixels[n];
				uint curAlpha = (uint)col >> 24;
				col &= clearAlpha;
				bmpPixels[n] = col | ((int)((float)curAlpha * alpaP) << 24);
			}
		}
		return bmpPixels;
	}

	public override BitmapExternal CropTo(int newSize)
	{
		SKBitmap bmpPixels = new SKBitmap(newSize, newSize);
		int width = bmp.Width;
		_ = bmp.Height;
		int centerOffsetX = width - newSize;
		int centerOffsetY = 0;
		int brown = 1903361;
		for (int x = 0; x < newSize; x++)
		{
			for (int y = 0; y < newSize; y++)
			{
				SKColor color = bmp.GetPixel(x + centerOffsetX, y + centerOffsetY);
				int alpha = (int)((uint)color >> 24);
				if (alpha < 255)
				{
					int rgb = (int)((uint)color & 0xFFFFFF);
					color = (uint)(ColorUtil.ColorOverlay(brown, rgb, (float)alpha / 255f) | -16777216);
				}
				bmpPixels.SetPixel(x, y, color);
			}
		}
		return new BitmapExternal(bmpPixels);
	}
}
