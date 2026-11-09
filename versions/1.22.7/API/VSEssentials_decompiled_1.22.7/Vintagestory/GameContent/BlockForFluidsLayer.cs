using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class BlockForFluidsLayer : Block, ICoolingMedium
{
	public float InsideDamage;

	public EnumDamageType DamageType;

	protected float coolingMediumTemperature;

	public override bool ForFluidsLayer => true;

	public bool IsStill => base.PushVector == null;

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		InsideDamage = Attributes?["insideDamage"].AsFloat() ?? 0f;
		DamageType = (EnumDamageType)Enum.Parse(typeof(EnumDamageType), Attributes?["damageType"].AsString("Fire") ?? "Fire");
		coolingMediumTemperature = Attributes["coolingMediumTemperature"].AsInt(GlobalConstants.CollectibleDefaultTemperature);
	}

	public override void OnEntityInside(IWorldAccessor world, Entity entity, BlockPos pos)
	{
		base.OnEntityInside(world, entity, pos);
		if (InsideDamage > 0f && world.Side == EnumAppSide.Server)
		{
			entity.ReceiveDamage(new DamageSource
			{
				Type = DamageType,
				Source = EnumDamageSource.Block,
				SourceBlock = this,
				SourcePos = pos.ToVec3d()
			}, InsideDamage);
		}
	}

	public override bool DoPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ItemStack byItemStack)
	{
		bool result = true;
		bool preventDefault = false;
		BlockBehavior[] blockBehaviors = BlockBehaviors;
		foreach (BlockBehavior obj in blockBehaviors)
		{
			EnumHandling handled = EnumHandling.PassThrough;
			bool behaviorResult = obj.DoPlaceBlock(world, byPlayer, blockSel, byItemStack, ref handled);
			if (handled != EnumHandling.PassThrough)
			{
				result = result && behaviorResult;
				preventDefault = true;
			}
			if (handled == EnumHandling.PreventSubsequent)
			{
				return result;
			}
		}
		if (preventDefault)
		{
			return result;
		}
		world.BlockAccessor.SetBlock(BlockId, blockSel.Position, 2);
		return true;
	}

	public virtual void CoolNow(ItemSlot slot, Vec3d pos, float dt, bool playSizzle = true)
	{
		CollectibleBehaviorQuenchable.CoolToTemperature(api.World, slot, pos, dt, coolingMediumTemperature, playSizzle);
	}

	public virtual bool CanCool(ItemSlot slot, Vec3d pos)
	{
		return true;
	}

	public virtual FastVec3f GetPushVector(BlockPos pos)
	{
		return new FastVec3f(base.PushVector);
	}

	public virtual float FlowRate(BlockPos pos)
	{
		Vec3i flowVec = (this as IBlockFlowing)?.FlowNormali;
		if ((object)flowVec != null)
		{
			float length = flowVec.Length();
			if (length < 2f && length >= 1f)
			{
				return 1f;
			}
			if (length < 3f && length >= 2f)
			{
				return 2f;
			}
			return length;
		}
		return 1f;
	}
}
