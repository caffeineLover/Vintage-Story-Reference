using Vintagestory.API;
using Vintagestory.API.Common;

namespace Vintagestory.ServerMods.NoObf;

[DocumentAsJson]
public class BlockCropPropertiesType
{
	[DocumentAsJson("Recommended", "N", false)]
	public EnumSoilNutrient RequiredNutrient;

	[DocumentAsJson("Required", "", false)]
	public float NutrientConsumption;

	[DocumentAsJson("Required", "", false)]
	public int GrowthStages;

	[DocumentAsJson("Obsolete", "", false)]
	public float TotalGrowthDays;

	[DocumentAsJson("Required", "", false)]
	public float TotalGrowthMonths;

	[DocumentAsJson("Unused", "false", false)]
	public bool MultipleHarvests;

	[DocumentAsJson("Unused", "0", false)]
	public int HarvestGrowthStageLoss;

	[DocumentAsJson("Optional", "-5", false)]
	public float ColdDamageBelow = -5f;

	[DocumentAsJson("Optional", "0.5", false)]
	public float DamageGrowthStuntMul = 0.5f;

	[DocumentAsJson("Optional", "0.5", false)]
	public float ColdDamageRipeMul = 0.5f;

	[DocumentAsJson("Optional", "40", false)]
	public float HeatDamageAbove = 40f;

	[DocumentAsJson("Optional", "None", false)]
	public CropBehaviorType[] Behaviors;
}
