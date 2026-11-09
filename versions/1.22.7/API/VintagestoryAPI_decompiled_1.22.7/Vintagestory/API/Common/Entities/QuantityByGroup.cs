namespace Vintagestory.API.Common.Entities;

/// <summary>
/// Allows you to control spawn limits based on a set of entity codes using a wildcard.
/// </summary>
[DocumentAsJson]
public class QuantityByGroup
{
	/// <summary>
	/// The maximum quantity for all entities that match the <see cref="F:Vintagestory.API.Common.Entities.QuantityByGroup.Code" /> wildcard.
	/// </summary>
	[DocumentAsJson("Required", "", false)]
	public int MaxQuantity;

	/// <summary>
	/// A wildcard asset location which can group many entities together.
	/// </summary>
	[DocumentAsJson("Required", "", false)]
	public AssetLocation Code;
}
