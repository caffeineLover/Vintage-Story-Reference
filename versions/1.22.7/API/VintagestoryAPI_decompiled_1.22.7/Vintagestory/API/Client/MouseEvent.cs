using Vintagestory.API.Common;

namespace Vintagestory.API.Client;

/// <summary>
/// This contains the data for what the mouse is currently doing.
/// </summary>
public class MouseEvent
{
	/// <summary>
	/// Current X position of the mouse.
	/// </summary>
	public int X { get; set; }

	/// <summary>
	/// Current Y position of the mouse.
	/// </summary>
	public int Y { get; set; }

	/// <summary>
	/// The X movement of the mouse.
	/// </summary>
	public int DeltaX { get; }

	/// <summary>
	/// The Y movement of the mouse.
	/// </summary>
	public int DeltaY { get; }

	/// <summary>
	/// Gets the current mouse button pressed.
	/// </summary>
	public EnumMouseButton Button { get; }

	public int Modifiers { get; }

	/// <summary>
	/// Am I handled?
	/// </summary>
	public bool Handled { get; set; }

	public MouseEvent(int x, int y, int deltaX, int deltaY, EnumMouseButton button, int modifiers)
	{
		X = x;
		Y = y;
		DeltaX = deltaX;
		DeltaY = deltaY;
		Button = button;
		Modifiers = modifiers;
	}

	public MouseEvent(int x, int y, int deltaX, int deltaY)
		: this(x, y, deltaX, deltaY, EnumMouseButton.None, 0)
	{
	}

	public MouseEvent(int x, int y, EnumMouseButton button, int modifiers)
		: this(x, y, 0, 0, button, modifiers)
	{
	}

	public MouseEvent(int x, int y)
		: this(x, y, 0, 0, EnumMouseButton.None, 0)
	{
	}
}
