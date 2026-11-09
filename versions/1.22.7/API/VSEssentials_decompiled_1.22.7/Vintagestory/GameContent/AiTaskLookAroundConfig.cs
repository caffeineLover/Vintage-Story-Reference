using Newtonsoft.Json;
using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
public class AiTaskLookAroundConfig : AiTaskBaseConfig
{
	[JsonProperty]
	public float TurnAngleFactor = 0.75f;
}
