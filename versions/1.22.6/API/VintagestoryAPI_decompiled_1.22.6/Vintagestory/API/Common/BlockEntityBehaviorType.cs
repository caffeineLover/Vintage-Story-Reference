using Newtonsoft.Json;
using Vintagestory.API.Datastructures;

namespace Vintagestory.API.Common;

/// <summary>
/// Allows adding behaviors for block entities. Remember, block entities are specific instances of blocks placed within the world.
/// See <see cref="T:Vintagestory.API.Common.BlockEntityBehavior" /> and its derived types for all block entities.
/// </summary>
/// <example>
/// <code language="json">
///             "entityClass": "Brake",
///             "entityBehaviors": [
///             	{ "name": "MPBrake" },
///             	{ "name": "Animatable" }
///             ],
/// </code>
/// </example>
[DocumentAsJson]
public class BlockEntityBehaviorType
{
	/// <summary>
	/// The ID for this block entity behavior.
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Required", "", false)]
	public string Name;

	/// <summary>
	/// A set of properties specific to the block entity behavior class.
	/// </summary>
	[JsonProperty]
	[JsonConverter(typeof(JsonAttributesConverter))]
	[DocumentAsJson("Optional", "None", false)]
	public JsonObject? properties;
}
