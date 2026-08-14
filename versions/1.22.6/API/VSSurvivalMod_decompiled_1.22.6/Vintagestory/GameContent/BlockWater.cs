using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockWater : BlockForFluidsLayer, IBlockFlowing
{
	protected bool freezable;

	protected Block iceBlock;

	protected float freezingPoint = -4f;

	protected bool isBoiling;

	public string Flow { get; set; }

	public Vec3i FlowNormali { get; set; }

	public bool IsLava => false;

	public virtual bool HasNormalWaves => true;

	public int Height { get; set; }

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		string f = Variant["flow"];
		Flow = ((f != null) ? string.Intern(f) : null);
		FlowNormali = ((Flow == null) ? null : Cardinal.FromInitial(Flow)?.Normali);
		Height = Variant["height"]?.ToInt() ?? 7;
		int fspeed = Attributes["flowSpeed"].AsInt(1);
		if (fspeed > 1)
		{
			FlowNormali = FlowNormali?.Clone().Mul(fspeed);
		}
		freezable = Flow == "still" && Height == 7;
		if (Attributes != null)
		{
			freezable &= Attributes["freezable"].AsBool(defaultValue: true);
			iceBlock = api.World.GetBlock(AssetLocation.Create(Attributes["iceBlockCode"].AsString("lakeice"), Code.Domain));
			freezingPoint = Attributes["freezingPoint"].AsFloat(-4f);
		}
		else
		{
			iceBlock = api.World.GetBlock(AssetLocation.Create("lakeice", Code.Domain));
		}
		isBoiling = HasBehavior<BlockBehaviorSteaming>();
	}

	public override float GetAmbientSoundStrength(IWorldAccessor world, BlockPos pos)
	{
		return (world.BlockAccessor.GetBlockAbove(pos).Id == 0 && world.BlockAccessor.IsSideSolid(pos.X, pos.Y - 1, pos.Z, BlockFacing.UP)) ? 1 : 0;
	}

	public override void OnAsyncClientParticleTick(IAsyncParticleManager manager, BlockPos pos, float windAffectednessAtPos, float secondsTicking)
	{
		BlockBehavior[] blockBehaviors = BlockBehaviors;
		for (int i = 0; i < blockBehaviors.Length; i++)
		{
			blockBehaviors[i].OnAsyncClientParticleTick(manager, pos, windAffectednessAtPos, secondsTicking);
		}
	}

	public override bool ShouldReceiveServerGameTicks(IWorldAccessor world, BlockPos pos, Random offThreadRandom, out object extra)
	{
		extra = null;
		if (!GlobalConstants.MeltingFreezingEnabled)
		{
			return false;
		}
		if (freezable && offThreadRandom.NextDouble() < 0.6 && world.BlockAccessor.GetRainMapHeightAt(pos) <= pos.Y)
		{
			BlockPos nPos = pos.Copy();
			for (int i = 0; i < BlockFacing.HORIZONTALS.Length; i++)
			{
				BlockFacing.HORIZONTALS[i].IterateThruFacingOffsets(nPos);
				if ((world.BlockAccessor.GetBlock(nPos, 2) is BlockLakeIce || world.BlockAccessor.GetBlock(nPos).Replaceable < 6000) && world.BlockAccessor.GetClimateAt(pos, EnumGetClimateMode.ForSuppliedDate_TemperatureOnly, api.World.Calendar.TotalDays).Temperature <= freezingPoint)
				{
					return true;
				}
			}
		}
		return false;
	}

	public override void OnServerGameTick(IWorldAccessor world, BlockPos pos, object extra = null)
	{
		if (iceBlock != null)
		{
			world.BlockAccessor.SetBlock(iceBlock.Id, pos, 2);
		}
	}

	public override void OnGroundIdle(EntityItem entityItem)
	{
		entityItem.Die(EnumDespawnReason.Removed);
		if (entityItem.World.Side == EnumAppSide.Server)
		{
			splashLiquid(entityItem.Itemstack, entityItem.Pos.XYZ);
		}
		base.OnGroundIdle(entityItem);
	}

	private void splashLiquid(ItemStack stack, Vec3d pos)
	{
		WaterTightContainableProps props = BlockLiquidContainerBase.GetContainableProps(stack);
		float litres = (float)stack.StackSize / (props?.ItemsPerLitre ?? 1f);
		api.World.SpawnCubeParticles(pos, stack, 0.75f, Math.Min(100, (int)(2f * litres)), 0.45f);
		api.World.PlaySoundAt(new AssetLocation("sounds/environment/smallsplash"), (float)pos.X, (float)pos.Y, (float)pos.Z);
		if (api.World.BlockAccessor.GetBlockEntity(pos.AsBlockPos) is BlockEntityFarmland bef)
		{
			bef.WaterFarmland((float)Height / 6f, waterNeightbours: false);
			bef.MarkDirty(redrawOnClient: true);
		}
	}

	public override void OnCollected(ItemStack stack, Entity entity)
	{
		base.OnCollected(stack, entity);
	}

	public override int GetRandomColor(ICoreClientAPI capi, ItemStack stack)
	{
		WaterTightContainableProps props = BlockLiquidContainerBase.GetContainableProps(stack);
		if (props != null)
		{
			props.WhenFilled?.Stack?.Resolve(api.World, "get random color resolve");
			if (props.WhenFilled.Stack.ResolvedItemStack != null)
			{
				return props.WhenFilled.Stack.ResolvedItemStack.Collectible.GetRandomColor(capi, props.WhenFilled.Stack.ResolvedItemStack);
			}
		}
		EntityPos pos = capi.World.Player.Entity.Pos;
		return capi.World.ApplyColorMapOnRgba(ClimateColorMap, SeasonColorMap, base.GetRandomColor(capi, stack), (int)pos.X, (int)pos.Y, (int)pos.Z);
	}

	public override bool CanPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref string failureCode)
	{
		Block oldBlock = world.BlockAccessor.GetBlock(blockSel.Position);
		if (oldBlock.DisplacesLiquids(world.BlockAccessor, blockSel.Position) && !oldBlock.IsReplacableBy(this))
		{
			failureCode = "notreplaceable";
			return false;
		}
		bool result = true;
		if (byPlayer != null && !world.Claims.TryAccess(byPlayer, blockSel.Position, EnumBlockAccessFlags.BuildOrBreak))
		{
			byPlayer.InventoryManager.ActiveHotbarSlot.MarkDirty();
			failureCode = "claimed";
			return false;
		}
		bool preventDefault = false;
		BlockBehavior[] blockBehaviors = BlockBehaviors;
		foreach (BlockBehavior obj in blockBehaviors)
		{
			EnumHandling handled = EnumHandling.PassThrough;
			bool behaviorResult = obj.CanPlaceBlock(world, byPlayer, blockSel, ref handled, ref failureCode);
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
		return true;
	}

	public override float GetTraversalCost(BlockPos pos, EnumAICreatureType creatureType)
	{
		if (creatureType == EnumAICreatureType.SeaCreature && !isBoiling)
		{
			return 0f;
		}
		if (!isBoiling || creatureType == EnumAICreatureType.HeatProofCreature)
		{
			return 5f;
		}
		return 99999f;
	}

	public override void DetermineTopMiddlePos()
	{
		TopMiddlePos.Y = (float)(Height + 1) / 8f;
	}
}
