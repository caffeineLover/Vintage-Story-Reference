using Newtonsoft.Json;
using Vintagestory.API.Common.Entities;

namespace Vintagestory.API.Common;

/// <summary>
/// Data about when an animation should be triggered.
/// </summary>
[DocumentAsJson]
public class AnimationTrigger
{
	/// <summary>
	/// An array of controls that should begin the animation.
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Recommended", "", false)]
	public EnumEntityActivity[] OnControls;

	/// <summary>
	/// If set to true, all OnControls elements need to be happening simultaneously to trigger the animation.
	/// If set to false, at least one OnControls element needs to be happening to trigger the animation.
	/// Defaults to false.
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Optional", "False", false)]
	public bool MatchExact;

	/// <summary>
	/// Is this animation the default animation for the entity?
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Optional", "False", false)]
	public bool DefaultAnim;

	public AnimationTrigger Clone()
	{
		return new AnimationTrigger
		{
			OnControls = (EnumEntityActivity[])OnControls?.Clone(),
			MatchExact = MatchExact,
			DefaultAnim = DefaultAnim
		};
	}
}
