using System;
using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public class InPieProperties
{
	public bool AllowMixing = true;

	public EnumPiePartType PartType;

	public required AssetLocation Texture;

	public EnumFoodCategory? FoodCategory;

	public string[] MixingCodes = Array.Empty<string>();
}
