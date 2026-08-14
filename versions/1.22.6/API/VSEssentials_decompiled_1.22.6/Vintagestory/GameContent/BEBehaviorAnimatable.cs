using System.Collections.Generic;
using System.Text;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class BEBehaviorAnimatable : BlockEntityBehavior
{
	public BlockEntityAnimationUtil animUtil;

	public BEBehaviorAnimatable(BlockEntity blockentity)
		: base(blockentity)
	{
	}

	public override void Initialize(ICoreAPI api, JsonObject properties)
	{
		base.Initialize(api, properties);
		animUtil = new BlockEntityAnimationUtil(api, Blockentity);
	}

	public override void OnBlockUnloaded()
	{
		base.OnBlockUnloaded();
		animUtil?.Dispose();
	}

	public override void OnBlockBroken(IPlayer byPlayer = null)
	{
		base.OnBlockBroken(byPlayer);
		animUtil?.Dispose();
	}

	public override void OnBlockRemoved()
	{
		base.OnBlockRemoved();
		animUtil?.Dispose();
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
	{
		if (animUtil.activeAnimationsByAnimCode.Count <= 0)
		{
			if (animUtil.animator != null)
			{
				return animUtil.animator.ActiveAnimationCount > 0;
			}
			return false;
		}
		return true;
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
		if (!(Api is ICoreClientAPI capi) || !capi.Settings.Bool["extendedDebugInfo"])
		{
			return;
		}
		dsc.Append("<font color=\"#bbb\">Active animations:");
		foreach (KeyValuePair<string, AnimationMetaData> val in animUtil.activeAnimationsByAnimCode)
		{
			dsc.Append(val.Key + " (frame " + animUtil.animator.GetAnimationState(val.Key)?.CurrentFrame + ")");
		}
		dsc.AppendLine("</font>");
	}
}
