namespace Vintagestory.API.Common;

/// <summary>
/// Implement this on a Block, BlockBehavior, BlockEntity or BlockEntityBehavior
/// to allow it to be traversed for players with <see cref="F:Vintagestory.API.Common.EnumBlockAccessFlags.Traverse" /> permissions
/// to the land claim or if AllowUseEveryone or AllowTraverseEveryone is set.
/// </summary>
public interface IClaimTraverseable
{
	/// <summary>
	/// Return true if you want to allow this block to be interacted with when the player has traverse permissions on this land claim.
	/// Implement this if you need custom logic else it will just return true by default
	/// </summary>
	/// <returns></returns>
	bool AllowTraverse()
	{
		return true;
	}
}
