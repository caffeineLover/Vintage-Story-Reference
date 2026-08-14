using System;
using SkiaSharp;

namespace Vintagestory.API.Common;

public abstract class BitmapRef : IDisposable, IBitmap
{
	public abstract int Width { get; }

	public abstract int Height { get; }

	public abstract int[] Pixels { get; }

	public abstract void Dispose();

	public abstract SKColor GetPixel(int x, int y);

	public abstract SKColor GetPixelRel(float x, float y);

	public abstract int[] GetPixelsTransformed(int rot = 0, int mulalpha = 255);

	public abstract void Save(string filename);

	public abstract void MulAlpha(int alpha = 255);

	/// <summary>
	/// Crops to a bitmap of newsize x newsize, taken from the center of the old bitmap
	/// </summary>
	public abstract BitmapRef CropTo(int newsize);
}
