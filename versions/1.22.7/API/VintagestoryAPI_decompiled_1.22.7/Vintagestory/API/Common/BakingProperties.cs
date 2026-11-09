namespace Vintagestory.API.Common;

/// <summary>
/// Baking Properties are collectible attribute used for baking items in a clay oven.
/// You will need to add these attributes if using <see cref="F:Vintagestory.API.Common.EnumSmeltType.Bake" /> inside <see cref="F:Vintagestory.API.Common.CombustibleProperties.SmeltingType" />.
/// </summary>
/// <example>
/// Example taken from bread. Note that the levelTo value in the baking stage is the same as the levelFrom in the next baking stage.
/// <code language="json">
///             "attributesByType": {
///             	"*-partbaked": {
///             		"bakingProperties": {
///             			"temp": 160,
///             			"levelFrom": 0.25,
///             			"levelTo": 0.5,
///             			"startScaleY": 0.95,
///             			"endScaleY": 1.10,
///             			"resultCode": "bread-{type}-perfect",
///             			"initialCode": "dough-{type}"
///             		}
///             	},
///             	"*-perfect": {
///             		"bakingProperties": {
///             			"temp": 160,
///             			"levelFrom": 0.5,
///             			"levelTo": 0.75,
///             			"startScaleY": 1.10,
///             			"endScaleY": 1.13,
///             			"resultCode": "bread-{type}-charred",
///             			"initialCode": "bread-{type}-partbaked"
///             		}
///             	},
///             	"*-charred": {
///             		"bakingProperties": {
///             			"temp": 160,
///             			"levelFrom": 0.75,
///             			"levelTo": 1,
///             			"startScaleY": 1.13,
///             			"endScaleY": 1.10,
///             			"initialCode": "bread-{type}-perfect"
///             		}
///             	}
///             },
/// </code>
/// </example>
[DocumentAsJson]
public class BakingProperties
{
	/// <summary>
	/// The temperature required to bake the item.
	/// </summary>
	[DocumentAsJson("Recommended", "160", false)]
	public float? Temp;

	/// <summary>
	/// The initial value, from 0 to 1, that determines how cooked the item is.
	/// When cooking an object with numerous cooking stages, these stages can be stacked using these values. Simply set the second stage's <see cref="F:Vintagestory.API.Common.BakingProperties.LevelFrom" /> to the first stages <see cref="F:Vintagestory.API.Common.BakingProperties.LevelTo" />.
	/// </summary>
	[DocumentAsJson("Recommended", "0", false)]
	public float LevelFrom;

	/// <summary>
	/// The final value, from 0 to 1, that determines how cooked the item is.
	/// When the cooking value reaches this value, the collectible will change into the next item.
	/// When cooking an object with numerous cooking stages, these stages can be stacked using these values. Simply set the second stage's <see cref="F:Vintagestory.API.Common.BakingProperties.LevelFrom" /> to the first stages <see cref="F:Vintagestory.API.Common.BakingProperties.LevelTo" />.
	/// </summary>
	[DocumentAsJson("Recommended", "1", false)]
	public float LevelTo;

	/// <summary>
	/// The Y scale of this collectible when it begins cooking. Value will be linearly interpolated between this and <see cref="F:Vintagestory.API.Common.BakingProperties.EndScaleY" />.
	/// </summary>
	[DocumentAsJson("Optional", "1", false)]
	public float StartScaleY;

	/// <summary>
	/// The Y scale of this collectible when it has finished cooking. Value will be linearly interpolated between <see cref="F:Vintagestory.API.Common.BakingProperties.StartScaleY" /> and this.
	/// </summary>
	[DocumentAsJson("Optional", "1", false)]
	public float EndScaleY;

	/// <summary>
	/// The code of the resulting collectible when this item finishes its cooking stage.
	/// </summary>
	[DocumentAsJson("Required", "", false)]
	public string ResultCode;

	/// <summary>
	/// The code of the initial collectible that is being baked.
	/// </summary>
	[DocumentAsJson("Required", "", false)]
	public string InitialCode;

	/// <summary>
	/// If true, only one instance of this collectible can be baked at a time. If false, 4 of this collectible can be baked at a time.
	/// </summary>
	[DocumentAsJson("Recommended", "False", false)]
	public bool LargeItem;

	public static BakingProperties ReadFrom(ItemStack stack)
	{
		if (stack == null)
		{
			return null;
		}
		BakingProperties result = stack.Collectible?.Attributes?["bakingProperties"]?.AsObject<BakingProperties>();
		if (result == null)
		{
			return null;
		}
		if (!result.Temp.HasValue || result.Temp == 0f)
		{
			CombustibleProperties props = stack.Collectible.GetCombustibleProperties(null, stack, null);
			if (props != null)
			{
				result.Temp = props.MeltingPoint - 40;
			}
		}
		return result;
	}
}
