using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockWaterflowing : BlockForFluidsLayer, IBlockFlowing
{
	private float particleQuantity = 0.2f;

	private bool isBoiling;

	public string Flow { get; set; }

	public Vec3i FlowNormali { get; set; }

	public bool IsLava => false;

	public virtual bool HasNormalWaves => true;

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		string f = Variant["flow"];
		Flow = ((f != null) ? string.Intern(f) : null);
		FlowNormali = ((Flow == null) ? null : Cardinal.FromInitial(Flow)?.Normali);
		int fspeed = Attributes["flowSpeed"].AsInt(1);
		if (fspeed > 1)
		{
			FlowNormali = FlowNormali?.Clone().Mul(fspeed);
		}
		if (api.Side == EnumAppSide.Client)
		{
			(api as ICoreClientAPI).Settings.Int.AddWatcher("particleLevel", OnClientParticleLevelChanged);
			OnClientParticleLevelChanged(0);
		}
		ParticleProperties[0].SwimOnLiquid = true;
		isBoiling = HasBehavior<BlockBehaviorSteaming>();
	}

	public virtual void OnClientParticleLevelChanged(int newValue)
	{
		particleQuantity = 0.4f * (float)(api as ICoreClientAPI).Settings.Int["particleLevel"] / 100f;
	}

	public override float GetAmbientSoundStrength(IWorldAccessor world, BlockPos pos)
	{
		if (world.BlockAccessor.GetBlockAbove(pos).Replaceable >= 6000 && !world.BlockAccessor.GetBlockAbove(pos, 1, 2).IsLiquid())
		{
			return 1f;
		}
		return 0f;
	}

	public override void OnAsyncClientParticleTick(IAsyncParticleManager manager, BlockPos pos, float windAffectednessAtPos, float secondsTicking)
	{
		BlockBehavior[] blockBehaviors = BlockBehaviors;
		for (int i = 0; i < blockBehaviors.Length; i++)
		{
			blockBehaviors[i].OnAsyncClientParticleTick(manager, pos, windAffectednessAtPos, secondsTicking);
		}
		if (!(api.World.Rand.NextDouble() > (double)particleQuantity))
		{
			AdvancedParticleProperties bps = ParticleProperties[0];
			bps.basePos.X = pos.X;
			bps.basePos.Y = pos.InternalY;
			bps.basePos.Z = pos.Z;
			FastVec3f pushVector = GetPushVector(pos);
			bps.Velocity[0].avg = pushVector.X * 500f;
			bps.Velocity[1].avg = pushVector.Y * 1000f;
			bps.Velocity[2].avg = pushVector.Z * 500f;
			bps.GravityEffect.avg = 0.5f;
			bps.HsvaColor[3].avg = 180f * Math.Min(1f, secondsTicking / 7f);
			bps.PosOffset[1].avg = 0.125f;
			bps.PosOffset[1].var = (float)LiquidLevel / 8f * 0.75f;
			manager.Spawn(bps);
		}
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
}
