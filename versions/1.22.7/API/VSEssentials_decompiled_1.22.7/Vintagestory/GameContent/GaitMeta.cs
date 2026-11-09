using Vintagestory.API;
using Vintagestory.API.Common;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class GaitMeta : AnimationMetaData
{
	[DocumentAsJson("Optional", "1", false)]
	public float YawMultiplier = 1f;

	[DocumentAsJson("Optional", "0", false)]
	public float MoveSpeed;

	[DocumentAsJson("Optional", "False", false)]
	public bool Backwards;

	[DocumentAsJson("Optional", "True", false)]
	public bool Natural = true;

	[DocumentAsJson("Unused", "", false)]
	public float StaminaCost;

	[DocumentAsJson("Unused", "None", false)]
	public string? FallbackGaitCode;

	[DocumentAsJson("Optional", "None", false)]
	public AssetLocation? Sound;

	[DocumentAsJson("Optional", "Land", false)]
	public EnumHabitat Environment = EnumHabitat.Land;

	public int Direction
	{
		get
		{
			if (MoveSpeed != 0f)
			{
				if (!Backwards)
				{
					return 1;
				}
				return -1;
			}
			return 0;
		}
	}

	public bool HasMotion => MoveSpeed > 0f;

	public bool HasForwardMotion
	{
		get
		{
			if (MoveSpeed > 0f)
			{
				return !Backwards;
			}
			return false;
		}
	}

	public bool HasBackwardMotion
	{
		get
		{
			if (MoveSpeed > 0f)
			{
				return Backwards;
			}
			return false;
		}
	}
}
