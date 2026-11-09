using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class ItemWormGrunter : Item
{
	private ILoadedSound gruntingSound;

	public override string GetHeldTpUseAnimation(ItemSlot activeHotbarSlot, Entity forEntity)
	{
		if ((forEntity as EntityPlayer).Controls.HandUse != EnumHandInteract.HeldItemInteract)
		{
			return null;
		}
		return base.GetHeldTpUseAnimation(activeHotbarSlot, forEntity);
	}

	public override void OnUnloaded(ICoreAPI api)
	{
		base.OnUnloaded(api);
		gruntingSound?.Dispose();
	}

	protected void startSound(EntityAgent byEntity)
	{
		if (api is ICoreClientAPI capi)
		{
			if (gruntingSound == null)
			{
				gruntingSound = capi.World.LoadSound(new SoundParams
				{
					Location = new AssetLocation("sounds/effect/wormgrunting"),
					Range = 12f,
					ShouldLoop = true,
					DisposeOnFinish = false
				});
			}
			gruntingSound.SetPosition(byEntity.Pos.XYZFloat);
			gruntingSound.Start();
		}
	}

	protected void stopSound()
	{
		gruntingSound?.Stop();
	}

	public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
	{
		if (!firstEvent || blockSel == null || blockSel.Face != BlockFacing.UP || !byEntity.Controls.ShiftKey)
		{
			return;
		}
		if (byEntity.Controls.CtrlKey)
		{
			base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handling);
			return;
		}
		Block block = api.World.BlockAccessor.GetBlock(blockSel.Position);
		if (block.Fertility <= 100 && !(block is BlockFarmland))
		{
			return;
		}
		if (byEntity.LeftHandItemSlot.Empty || byEntity.LeftHandItemSlot.Itemstack.Collectible.Code.Path != "stick")
		{
			(api as ICoreClientAPI)?.TriggerIngameError(this, "missingstick", Lang.Get("Requires a stick in offhand"));
			return;
		}
		if (byEntity.World.BlockAccessor.GetClimateAt(blockSel.Position).Temperature <= 0f)
		{
			(api as ICoreClientAPI)?.TriggerIngameError(this, "toocold", Lang.Get("The ground is frozen, worms won't come out"));
			return;
		}
		handling = EnumHandHandling.PreventDefault;
		startSound(byEntity);
		if (api.Side == EnumAppSide.Server)
		{
			float maxQuantity = api.ModLoader.GetModSystem<ModSystemWormGrunting>().GetEarthWormAmount(blockSel.Position);
			int cnt = GameMath.RoundRandom(api.World.Rand, (float)Math.Min(0.5, api.World.Rand.NextDouble()) * maxQuantity);
			slot.Itemstack.TempAttributes.SetInt("spawnAmount", cnt);
		}
	}

	public override bool OnHeldInteractStep(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		if (blockSel == null || blockSel.Face != BlockFacing.UP)
		{
			return false;
		}
		float th = ((api.Side == EnumAppSide.Server) ? 4f : 5f);
		if (secondsUsed > th || blockSel == null)
		{
			return false;
		}
		if (api.Side == EnumAppSide.Server)
		{
			Block block = api.World.BlockAccessor.GetBlock(blockSel.Position);
			int cnt = slot.Itemstack.TempAttributes.GetInt("spawnAmount");
			if (block.Fertility > 0 && secondsUsed >= 1f && cnt > 0 && api.World.Rand.NextDouble() < 0.06)
			{
				int nowamount = 1 + ((cnt > 1 && api.World.Rand.NextDouble() < 0.1) ? 1 : 0);
				spawnWorm(slot, byEntity, blockSel, nowamount);
				slot.Itemstack.TempAttributes.SetInt("spawnAmount", cnt - nowamount);
			}
		}
		return true;
	}

	public override bool OnHeldInteractCancel(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, EnumItemUseCancelReason cancelReason)
	{
		stopSound();
		byEntity.AnimManager.StopAnimation("wormgrunting");
		return true;
	}

	public override void OnHeldInteractStop(float secondsUsed, ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel)
	{
		stopSound();
		byEntity.AnimManager.StopAnimation("wormgrunting");
		if (blockSel == null || blockSel.Face != BlockFacing.UP)
		{
			return;
		}
		Block block = api.World.BlockAccessor.GetBlock(blockSel.Position);
		if ((block.Fertility > 100 || block is BlockFarmland) && secondsUsed > 4f && api.Side == EnumAppSide.Server)
		{
			int cnt = slot.Itemstack.TempAttributes.GetInt("spawnAmount");
			if (cnt > 0)
			{
				spawnWorm(slot, byEntity, blockSel, cnt);
			}
		}
		if (secondsUsed >= 1f && (byEntity as EntityPlayer).Player.WorldData.CurrentGameMode != EnumGameMode.Creative && byEntity.World.Side == EnumAppSide.Server)
		{
			slot.Itemstack.Collectible.DamageItem(byEntity.World, byEntity, slot);
			slot.MarkDirty();
		}
		base.OnHeldInteractStop(secondsUsed, slot, byEntity, blockSel, entitySel);
	}

	private void spawnWorm(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, int amount)
	{
		ModSystemWormGrunting mswg = api.ModLoader.GetModSystem<ModSystemWormGrunting>();
		if (amount > 0)
		{
			mswg.AddHarvest(blockSel.Position, amount);
		}
		EntityProperties type = byEntity.World.GetEntityType(new AssetLocation("earthworm"));
		for (int i = 0; i < amount; i++)
		{
			Entity eWorm = byEntity.World.ClassRegistry.CreateEntity(type);
			eWorm.Pos.X = (float)blockSel.Position.X + (float)api.World.Rand.NextDouble();
			eWorm.Pos.Y = blockSel.Position.Y + 1;
			eWorm.Pos.Z = (float)blockSel.Position.Z + (float)api.World.Rand.NextDouble();
			eWorm.Pos.Yaw = (float)api.World.Rand.NextDouble() * 2f * (float)Math.PI;
			byEntity.World.SpawnEntity(eWorm);
		}
	}
}
