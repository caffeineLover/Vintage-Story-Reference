using Newtonsoft.Json;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class AiTaskIdleConfig : AiTaskBaseTargetableConfig
{
	[JsonProperty]
	public bool StopWhenTargetDetected;

	[JsonProperty]
	public TagSet AllowedBlockBelowTags = new TagSet();

	[JsonProperty]
	public TagSet SkipBlockBelowTags = new TagSet();

	[JsonProperty]
	public AssetLocation? AllowedBlockBelowCode;

	[JsonProperty]
	public bool CheckForSolidUpSide = true;

	[JsonProperty]
	public int MinBlockInsideReplaceable = 6000;

	[JsonProperty]
	public float ChanceToCheckTarget = 0.3f;

	public bool IgnoreBlockCodeAndTags
	{
		get
		{
			if (AllowedBlockBelowTags.IsEmpty && SkipBlockBelowTags.IsEmpty)
			{
				return AllowedBlockBelowCode == null;
			}
			return false;
		}
	}
}
