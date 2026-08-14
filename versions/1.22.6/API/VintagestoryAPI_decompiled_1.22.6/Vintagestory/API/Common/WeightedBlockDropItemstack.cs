namespace Vintagestory.API.Common;

[DocumentAsJson]
public class WeightedBlockDropItemstack : BlockDropItemStack
{
	/// <summary>
	/// How strongly to weight this drop
	/// </summary>
	[DocumentAsJson("Optional", "1", false)]
	public float Weight = 1f;
}
