namespace Vintagestory.API.Common;

/// <summary>
/// Marks an item as combustible, either by cooking, smelting or firing. This can either imply it is used as a fuel, or can be cooked into another object.
/// </summary>
/// <example>
/// Cooking:
/// <code language="json">
///             "combustiblePropsByType": {
///             	"bushmeat-raw": {
///             		"meltingPoint": 150,
///             		"meltingDuration": 30,
///             		"smeltedRatio": 1,
///             		"smeltingType": "cook",
///             		"smeltedStack": {
///             			"type": "item",
///             			"code": "bushmeat-cooked"
///             		},
///             		"requiresContainer": false
///             	}
///             },
/// </code>
/// Clay Firing:
/// <code language="json">
///             "combustiblePropsByType": {
///             	"bowl-raw": {
///             		"meltingPoint": 650,
///             		"meltingDuration": 45,
///             		"smeltedRatio": 1,
///             		"smeltingType": "fire",
///             		"smeltedStack": {
///             			"type": "block",
///             			"code": "bowl-fired"
///             		},
///             		"requiresContainer": false
///             	}
///             },
/// </code>
/// Fuel Source:
/// <code language="json">
///             "combustibleProps": {
///             	"burnTemperature": 1300,
///             	"burnDuration": 40
///             },
/// </code>
/// </example>
[DocumentAsJson]
public class CombustibleProperties
{
	/// <summary>
	/// The temperature at which this collectible burns when used as a fuel.
	/// </summary>
	[DocumentAsJson("Optional", "0", false)]
	public int BurnTemperature;

	/// <summary>
	/// The duration, in real life seconds, that this collectible burns for when used as a fuel. 
	/// </summary>
	[DocumentAsJson("Optional", "0", false)]
	public float BurnDuration;

	/// <summary>
	/// How many degrees celsius it can resists before it ignites
	/// </summary>
	[DocumentAsJson("Optional", "500", false)]
	public int HeatResistance = 500;

	/// <summary>
	/// How many degrees celsius it takes to smelt/transform this collectible into another. Required if <see cref="F:Vintagestory.API.Common.CombustibleProperties.SmeltedStack" /> is set.
	/// </summary>
	[DocumentAsJson("Recommended", "0", false)]
	public int MeltingPoint;

	/// <summary>
	/// If there is a melting point, the max temperature it can reach. A value of 0 implies no limit.
	/// </summary>
	[DocumentAsJson("Optional", "0", false)]
	public int MaxTemperature;

	/// <summary>
	/// For how many seconds the temperature has to be above the melting point until the item is smelted. Recommended if <see cref="F:Vintagestory.API.Common.CombustibleProperties.SmeltedStack" /> is set.
	/// </summary>
	[DocumentAsJson("Recommended", "0", false)]
	public float MeltingDuration;

	/// <summary>
	/// How much smoke this item produces when being used as fuel
	/// </summary>
	[DocumentAsJson("Optional", "1", false)]
	public float SmokeLevel = 1f;

	/// <summary>
	/// How many of this collectible are needed to smelt into <see cref="F:Vintagestory.API.Common.CombustibleProperties.SmeltedStack" />.
	/// </summary>
	[DocumentAsJson("Optional", "1", false)]
	public int SmeltedRatio = 1;

	/// <summary>
	/// Some smelt types have specific functionality, and are also used for correct naming in the tool tip.
	/// If using <see cref="F:Vintagestory.API.Common.EnumSmeltType.Bake" />, you will need to include <see cref="T:Vintagestory.API.Common.BakingProperties" /> in your item attributes.
	/// </summary>
	[DocumentAsJson("Recommended", "Smelt", false)]
	public EnumSmeltType SmeltingType;

	/// <summary>
	/// If set, this is the resulting itemstack once the MeltingPoint has been reached for the supplied duration.
	/// </summary>
	[DocumentAsJson("Recommended", "0", false)]
	public JsonItemStack SmeltedStack;

	/// <summary>
	/// If true, a container is required to smelt this item. 
	/// </summary>
	[DocumentAsJson("Optional", "True", false)]
	public bool RequiresContainer = true;

	/// <summary>
	/// Creates a deep copy
	/// </summary>
	/// <returns></returns>
	public CombustibleProperties Clone()
	{
		CombustibleProperties cloned = new CombustibleProperties();
		cloned.BurnDuration = BurnDuration;
		cloned.BurnTemperature = BurnTemperature;
		cloned.HeatResistance = HeatResistance;
		cloned.MeltingDuration = MeltingDuration;
		cloned.MeltingPoint = MeltingPoint;
		cloned.SmokeLevel = SmokeLevel;
		cloned.SmeltedRatio = SmeltedRatio;
		cloned.RequiresContainer = RequiresContainer;
		cloned.SmeltingType = SmeltingType;
		cloned.MaxTemperature = MaxTemperature;
		if (SmeltedStack != null)
		{
			cloned.SmeltedStack = SmeltedStack.Clone();
		}
		return cloned;
	}
}
