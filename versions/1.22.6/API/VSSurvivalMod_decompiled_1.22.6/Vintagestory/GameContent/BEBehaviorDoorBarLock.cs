using System;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

[DocumentAsJson]
[AddDocumentationProperty("easingSpeed", "Affects animation speed of door unlocking", "System.Single", "Optional", "10", true)]
public class BEBehaviorDoorBarLock : BlockEntityBehavior
{
	public bool IsLocked { get; set; } = true;

	public BEBehaviorDoorBarLock(BlockEntity blockentity)
		: base(blockentity)
	{
	}

	public override void Initialize(ICoreAPI api, JsonObject properties)
	{
		base.Initialize(api, properties);
		if (IsLocked)
		{
			BEBehaviorDoor behavior = Blockentity.GetBehavior<BEBehaviorDoor>();
			float easingSpeed = base.Block.Attributes?["easingSpeed"].AsFloat(10f) ?? 10f;
			AnimationMetaData animMeta = new AnimationMetaData
			{
				Animation = "lock",
				Code = "lock",
				EaseInSpeed = 10f,
				EaseOutSpeed = easingSpeed,
				AnimationSpeed = 0.6f
			};
			behavior.animUtil.StartAnimation(animMeta);
		}
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		base.FromTreeAttributes(tree, worldAccessForResolve);
		IsLocked = tree.GetBool("isLocked", defaultValue: true);
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		tree.SetBool("isLocked", IsLocked);
	}

	public bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		BEBehaviorDoor beBehaviorDoor = Blockentity.GetBehavior<BEBehaviorDoor>();
		float rot = beBehaviorDoor.RotateYRad;
		double y = (double)blockSel.Position.X + blockSel.HitPosition.X - byPlayer.Entity.Pos.X;
		double dz = (double)blockSel.Position.Z + blockSel.HitPosition.Z - byPlayer.Entity.Pos.Z;
		float relAngel = GameMath.Mod((float)Math.Atan2(y, dz) - rot, (float)Math.PI * 2f);
		if (IsLocked && relAngel > (float)Math.PI / 2f && relAngel < 4.712389f)
		{
			Api.Logger.Notification("open");
			beBehaviorDoor.animUtil.StopAnimation("lock");
			IsLocked = false;
			Blockentity.MarkDirty(redrawOnClient: true);
		}
		else if (IsLocked && Api is ICoreClientAPI capi)
		{
			capi.TriggerIngameError(this, "doorBarLocked", Lang.Get("ingameerror-doorbarlocked"));
		}
		if (IsLocked)
		{
			handling = EnumHandling.PreventSubsequent;
		}
		return !IsLocked;
	}
}
