namespace Vintagestory.API.MathTools;

/// <summary>
/// Represents a 2D size
/// </summary>
public class HorSize2i
{
	public int Width;

	public int Length;

	public int Area => Width * Length;

	public HorSize2i()
	{
	}

	public HorSize2i(int width, int length)
	{
		Width = width;
		Length = length;
	}

	public HorSize2i Clone()
	{
		return new HorSize2i(Width, Length);
	}
}
