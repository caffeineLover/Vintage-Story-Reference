namespace Vintagestory.API.MathTools;

/// <summary>
/// Represents a vector of 3 doubles. Go bug Tyron of you need more utility methods in this class.
/// </summary>
public class Size3d : ISize3
{
	public double Width;

	public double Height;

	public double Length;

	public int WidthAsInt => (int)Width;

	public int HeightAsInt => (int)Height;

	public int LengthAsInt => (int)Length;

	public double WidthAsDouble => Width;

	public double HeightAsDouble => Height;

	public double LengthAsDouble => Length;

	public float WidthAsFloat => (float)Width;

	public float HeightAsFloat => (float)Height;

	public float LengthAsFloat => (float)Length;

	public double Volume => Width * Height * Length;

	public Size3d()
	{
	}

	public Size3d(double width, double height, double length)
	{
		Width = width;
		Height = height;
		Length = length;
	}

	public Size3d Clone()
	{
		return new Size3d(Width, Height, Length);
	}

	public bool CanContain(Size3d obj)
	{
		if (Width >= obj.Width && Height >= obj.Height)
		{
			return Length >= obj.Length;
		}
		return false;
	}
}
