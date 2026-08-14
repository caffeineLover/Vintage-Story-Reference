using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace Vintagestory.GameContent;

public class ErelAnimManager : AnimationManager
{
	private EntityErel entityErel;

	public override void Init(ICoreAPI api, Entity entity)
	{
		base.Init(api, entity);
		entityErel = entity as EntityErel;
	}

	public override bool StartAnimation(AnimationMetaData animdata)
	{
		if (entityErel.StandingOnGround && animdata.Code == "hurt")
		{
			return false;
		}
		return base.StartAnimation(animdata);
	}

	protected override void onReceivedServerAnimation(AnimationMetaData animmetadata)
	{
		if (!entityErel.StandingOnGround || !(animmetadata.Code == "hurt"))
		{
			base.onReceivedServerAnimation(animmetadata);
		}
	}
}
