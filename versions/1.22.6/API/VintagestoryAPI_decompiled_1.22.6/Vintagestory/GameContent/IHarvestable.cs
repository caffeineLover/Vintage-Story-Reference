using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace Vintagestory.GameContent;

/// <summary>
/// Currently only the knife looks for this interface
/// </summary>
public interface IHarvestable
{
	string HarvestAnimation => "knifecut";

	AssetLocation HarvestableSound => new AssetLocation("sounds/player/scrape");

	bool IsHarvestable(ItemSlot slot, Entity forEntity);

	float GetHarvestDuration(ItemSlot slot, Entity forEntity);

	void SetHarvested(IPlayer byPlayer, float dropQuantityMultiplier = 1f);
}
