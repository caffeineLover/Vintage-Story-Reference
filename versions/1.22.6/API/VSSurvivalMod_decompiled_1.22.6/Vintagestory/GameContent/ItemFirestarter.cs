using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class ItemFirestarter : Item
{
	private string igniteAnimation;

	public override void OnLoaded(ICoreAPI api)
	{
		igniteAnimation = Attributes["igniteAnimation"].AsString();
	}

	public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
	{
		base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handling);
		BlockPos pos = blockSel?.Position;
		if ((object)pos == null)
		{
			return;
		}
		IWorldAccessor world = byEntity.World;
		IPlayer byPlayer = (byEntity as EntityPlayer)?.Player;
		if (!world.Claims.TryAccess(byPlayer, pos, EnumBlockAccessFlags.Use))
		{
			return;
		}
		switch (world.BlockAccessor.GetBlock(pos).GetInterface<IIgnitable>(world, pos)?.OnTryIgniteBlock(byEntity, pos, 0f) ?? EnumIgniteState.NotIgnitable)
		{
		case EnumIgniteState.NotIgnitablePreventDefault:
			handling = EnumHandHandling.PreventDefault;
			break;
		case EnumIgniteState.Ignitable:
			handling = EnumHandHandling.PreventDefault;
			byEntity.AnimManager.StartAnimation(igniteAnimation);
			if (api.Side == EnumAppSide.Client)
			{
				api.Event.UnregisterCallback(ObjectCacheUtil.TryGet<long>(api, "firestartersound"));
				api.ObjectCache["firestartersound"] = api.Event.RegisterCallback(delegate
				{
					byEntity.World.PlaySoundAt("sounds/player/handdrill", byEntity, byPlayer, randomizePitch: false, 16f);
				}, 500);
			}
			break;
		}
	}

	public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		BlockPos pos = blockSel?.Position;
		if ((object)pos == null)
		{
			api.Event.UnregisterCallback(ObjectCacheUtil.TryGet<long>(api, "firestartersound"));
			return false;
		}
		IWorldAccessor world = byEntity.World;
		IPlayer byPlayer = (byEntity as EntityPlayer)?.Player;
		if (!world.Claims.TryAccess(byPlayer, pos, EnumBlockAccessFlags.Use))
		{
			api.Event.UnregisterCallback(ObjectCacheUtil.TryGet<long>(api, "firestartersound"));
			return false;
		}
		EnumIgniteState igniteState = world.BlockAccessor.GetBlock(pos).GetInterface<IIgnitable>(world, pos)?.OnTryIgniteBlock(byEntity, pos, secondsUsed) ?? EnumIgniteState.NotIgnitable;
		if ((uint)igniteState <= 1u)
		{
			api.Event.UnregisterCallback(ObjectCacheUtil.TryGet<long>(api, "firestartersound"));
			return false;
		}
		if (!(world is IClientWorldAccessor cWorld))
		{
			return igniteState == EnumIgniteState.Ignitable;
		}
		ModelTransform tf = new ModelTransform();
		tf.EnsureDefaultValues();
		float f = GameMath.Clamp(1f - 2f * secondsUsed, 0f, 1f);
		Random rand = cWorld.Rand;
		tf.Translation.Set(f * f * f * 1.6f - 1.6f, 0f, 0f);
		tf.Rotation.Y = 0f - Math.Min(secondsUsed * 120f, 30f);
		if (secondsUsed > 0.5f)
		{
			tf.Translation.Add((float)rand.NextDouble() * 0.1f, (float)rand.NextDouble() * 0.1f, (float)rand.NextDouble() * 0.1f);
			cWorld.SetCameraShake(0.04f);
		}
		if (!(secondsUsed > 0.25f) || (int)(30f * secondsUsed) % 2 != 1)
		{
			return igniteState == EnumIgniteState.Ignitable;
		}
		AdvancedParticleProperties props = cWorld.GetBlock(new AssetLocation("fire")).ParticleProperties[^1].Clone();
		props.basePos = pos.ToVec3d().Add(blockSel.HitPosition);
		props.Quantity.avg = 0.3f;
		props.Size.avg = 0.03f;
		cWorld.SpawnParticles(props, byPlayer);
		props.Quantity.avg = 0f;
		return igniteState == EnumIgniteState.Ignitable;
	}

	public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		byEntity.AnimManager.StopAnimation(igniteAnimation);
		BlockPos pos = blockSel?.Position;
		if ((object)pos != null && byEntity.World is IServerWorldAccessor world && !(world.Rand.NextDouble() > 0.25))
		{
			IIgnitable ign = world.BlockAccessor.GetBlock(pos).GetInterface<IIgnitable>(world, pos);
			if ((ign?.OnTryIgniteBlock(byEntity, pos, secondsUsed) ?? EnumIgniteState.NotIgnitable) != EnumIgniteState.IgniteNow)
			{
				api.Event.UnregisterCallback(ObjectCacheUtil.TryGet<long>(api, "firestartersound"));
			}
			else if (world.Claims.TryAccess((byEntity as EntityPlayer)?.Player, pos, EnumBlockAccessFlags.Use))
			{
				DamageItem(world, byEntity, slot);
				EnumHandling handled = EnumHandling.PassThrough;
				ign?.OnTryIgniteBlockOver(byEntity, pos, secondsUsed, ref handled);
			}
		}
	}

	public override bool OnHeldInteractCancel(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, EnumItemUseCancelReason cancelReason)
	{
		byEntity.AnimManager.StopAnimation(igniteAnimation);
		api.Event.UnregisterCallback(ObjectCacheUtil.TryGet<long>(api, "firestartersound"));
		return true;
	}

	public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
	{
		WorldInteraction[] interactions = new WorldInteraction[1]
		{
			new WorldInteraction
			{
				HotKeyCode = "shift",
				ActionLangCode = "heldhelp-igniteblock",
				MouseButton = EnumMouseButton.Right
			}
		};
		return base.GetHeldInteractionHelp(inSlot).Append(interactions);
	}
}
