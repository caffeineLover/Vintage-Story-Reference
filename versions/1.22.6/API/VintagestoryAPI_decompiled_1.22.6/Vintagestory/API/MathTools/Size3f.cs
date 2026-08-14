namespace Vintagestory.API.MathTools;

/// <summary>
/// Represents a vector of 3 floats. Go bug Tyron of you need more utility methods in this class.
/// </summary>
[DocumentAsJson]
public class Size3f : ISize3
{
	/// <summary>
	/// The X-dimension of this size.
	/// </summary>
	[DocumentAsJson("Recommended", "0", false)]
	public float Width;

	/// <summary>
	/// The Y-dimension for this size.
	/// </summary>
	[DocumentAsJson("Recommended", "0", false)]
	public float Height;

	/// <summary>
	/// The Z-dimension for this size.
	/// </summary>
	[DocumentAsJson("Recommended", "0", false)]
	public float Length;

	public float Volume => Width * Height * Length;

	public int WidthAsInt => (int)Width;

	public int HeightAsInt => (int)Height;

	public int LengthAsInt => (int)Length;

	public double WidthAsDouble => Width;

	public double HeightAsDouble => Height;

	public double LengthAsDouble => Length;

	public float WidthAsFloat => Width;

	public float HeightAsFloat => Height;

	public float LengthAsFloat => Length;

	public Size3f()
	{
	}

	public Size3f(float width, float height, float length)
	{
		Width = width;
		Height = height;
		Length = length;
	}

	public Size3f Clone()
	{
		return new Size3f(Width, Height, Length);
	}

	public bool CanContain(Size3f obj)
	{
		if (Width >= obj.Width && Height >= obj.Height)
		{
			return Length >= obj.Length;
		}
		return false;
	}
}
