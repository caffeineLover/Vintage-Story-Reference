using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class BlockLakeIce : BlockForFluidsLayer
{
	protected int waterBlock;

	protected bool stayIceWhenBroken;

	private BlockPos tmpPos = new BlockPos(0);

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		waterBlock = api.World.GetBlock(new AssetLocation("water-still-7")).BlockId;
		stayIceWhenBroken = Attributes?["stayIceWhenBroken"].AsBool() ?? false;
	}

	public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
	{
		bool preventDefault = false;
		BlockBehavior[] blockBehaviors = BlockBehaviors;
		foreach (BlockBehavior obj in blockBehaviors)
		{
			EnumHandling handled = EnumHandling.PassThrough;
			obj.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier, ref handled);
			if (handled == EnumHandling.PreventDefault)
			{
				preventDefault = true;
			}
			if (handled == EnumHandling.PreventSubsequent)
			{
				return;
			}
		}
		if (preventDefault)
		{
			return;
		}
		if (world.Side == EnumAppSide.Server && (byPlayer == null || byPlayer.WorldData.CurrentGameMode != EnumGameMode.Creative))
		{
			ItemStack[] drops = GetDrops(world, pos, byPlayer, dropQuantityMultiplier);
			if (drops != null)
			{
				for (int j = 0; j < drops.Length; j++)
				{
					if (SplitDropStacks)
					{
						for (int k = 0; k < drops[j].StackSize; k++)
						{
							ItemStack stack = drops[j].Clone();
							stack.StackSize = 1;
							world.SpawnItemEntity(stack, pos);
						}
					}
				}
			}
			if (Sounds != null)
			{
				world.PlaySoundAt(Sounds.GetBreakSound(byPlayer), pos, -0.5, byPlayer);
			}
		}
		SpawnBlockBrokenParticles(pos);
		if (stayIceWhenBroken)
		{
			world.BlockAccessor.SetBlock(0, pos, 2);
		}
		else
		{
			world.BlockAccessor.SetBlock(waterBlock, pos, 2);
		}
	}

	public override bool ShouldReceiveServerGameTicks(IWorldAccessor world, BlockPos pos, Random offThreadRandom, out object extra)
	{
		extra = null;
		float chance = GameMath.Clamp((world.BlockAccessor.GetClimateAt(pos, EnumGetClimateMode.ForSuppliedDate_TemperatureOnly, api.World.Calendar.TotalDays).Temperature - 2f) / 20f, 0f, 1f);
		return offThreadRandom.NextDouble() < (double)chance;
	}

	public override void OnServerGameTick(IWorldAccessor world, BlockPos pos, object extra = null)
	{
		world.BlockAccessor.SetBlock(waterBlock, pos, 2);
	}

	public override bool ShouldMergeFace(int facingIndex, Block neighbourIce, int intraChunkIndex3d)
	{
		return BlockMaterial == neighbourIce.BlockMaterial;
	}

	public override void CoolNow(ItemSlot slot, Vec3d pos, float dt, bool playSizzle = true)
	{
		IWorldAccessor world = api.World;
		if (!slot.Itemstack.TempAttributes.HasAttribute("thawStart"))
		{
			slot.Itemstack.TempAttributes.SetLong("thawStart", world.Calendar.ElapsedSeconds);
		}
		float stackTemp = slot.Itemstack.Collectible.GetTemperature(world, slot.Itemstack);
		if (stackTemp <= coolingMediumTemperature)
		{
			return;
		}
		float nextTemperature = Math.Max(20f, stackTemp - 2f) * dt * 50f;
		if (slot.Empty)
		{
			return;
		}
		slot.Itemstack.Collectible.SetTemperature(world, slot.Itemstack, nextTemperature);
		float tempDiff = stackTemp - coolingMediumTemperature;
		if (tempDiff > 90f)
		{
			double width = 0.0;
			Entity.SplashParticleProps.BasePos.Set(pos.X - width / 2.0, pos.Y - 0.75, pos.Z - width / 2.0);
			Entity.SplashParticleProps.AddVelocity.Set(0f, 0f, 0f);
			Entity.SplashParticleProps.QuantityMul = 0.1f;
			world.SpawnParticles(Entity.SplashParticleProps);
		}
		long lastPlayedSizzlesTotalMs = slot.Itemstack.TempAttributes.GetLong("lastPlayedSizzlesTotalMs", -99999L);
		if (playSizzle && tempDiff > 200f && world.Side == EnumAppSide.Client && world.ElapsedMilliseconds - lastPlayedSizzlesTotalMs > 10000)
		{
			world.PlaySoundAt(new AssetLocation("sounds/sizzle"), pos.X, pos.Y, pos.Z);
			slot.Itemstack.TempAttributes.SetLong("lastPlayedSizzlesTotalMs", world.ElapsedMilliseconds);
		}
		if (api.Side != EnumAppSide.Client && world.Calendar.ElapsedSeconds - slot.Itemstack.TempAttributes.GetLong("thawStart", 0L) >= 120)
		{
			slot.Itemstack.TempAttributes.RemoveAttribute("thawStart");
			tmpPos.Set(pos.XInt, pos.YInt, pos.ZInt);
			if (!(world.BlockAccessor.GetBlock(tmpPos) is BlockLakeIce))
			{
				tmpPos.Down();
			}
			if (world.BlockAccessor.GetBlock(tmpPos) is BlockLakeIce)
			{
				world.BlockAccessor.SetBlock(waterBlock, tmpPos, 2);
			}
		}
	}

	public override bool CanCool(ItemSlot slot, Vec3d pos)
	{
		return slot is EntityItemSlot;
	}
}
