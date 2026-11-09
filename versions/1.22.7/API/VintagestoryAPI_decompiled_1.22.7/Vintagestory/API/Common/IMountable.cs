using Vintagestory.API.Common.Entities;

namespace Vintagestory.API.Common;

/// <summary>
/// Represents something the player can mount. Usually a block or an entity.
/// </summary>
public interface IMountable
{
	/// <summary>
	/// The seats of this mountable
	/// </summary>
	IMountableSeat[] Seats { get; }

	/// <summary>
	/// Position of this mountable
	/// </summary>
	EntityPos Position { get; }

	/// <summary>
	/// StepPitch (pitching when stepping up or down a block) of this mountable - valid client-side only, taken from EntityShapeRenderer
	/// </summary>
	double StepPitch { get; }

	/// <summary>
	/// The entity that controls this mountable - there can only be one
	/// </summary>
	Entity Controller { get; }

	/// <summary>
	/// The entity which this mountable really is (for example raft, boat or elk) - may be null if the IMountable is a bed or other block
	/// </summary>
	Entity OnEntity { get; }

	/// <summary>
	/// The controls of the controlling seat (if any)
	/// </summary>
	EntityControls ControllingControls { get; }

	bool AnyMounted();
}
