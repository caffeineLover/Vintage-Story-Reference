using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

public interface ICrop
{
	bool Ripe { get; }

	EnumSoilNutrient RequiredNutrient { get; }
}
