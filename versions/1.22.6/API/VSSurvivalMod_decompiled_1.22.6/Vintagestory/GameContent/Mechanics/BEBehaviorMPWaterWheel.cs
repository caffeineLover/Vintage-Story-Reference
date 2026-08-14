using System;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent.Mechanics;

public class BEBehaviorMPWaterWheel : BEBehaviorMPRotor
{
	protected float flowRate;

	protected BlockFacing facing;

	protected float dir;

	protected BEBehaviorRightClickConstructable bebconstructable;

	protected Vec3f flowVec = new Vec3f();

	protected bool blocked;

	protected int diameter = 3;

	protected int suitablePowerSourceBlockCount;

	protected float requiresMinFlowSpeed = 1.5f;

	private AssetLocation sound;

	protected override AssetLocation Sound => sound;

	protected override float Resistance
	{
		get
		{
			if (!blocked)
			{
				return 0.003f;
			}
			return 1f;
		}
	}

	protected override double AccelerationFactor => 0.05;

	protected override float TargetSpeed => Math.Min(0.3f, flowRate);

	protected override float TorqueFactor => flowRate;

	public override float AngleRad => base.AngleRad * dir;

	protected override float GetSoundVolume()
	{
		return 0.5f + 0.5f * flowRate;
	}

	public BEBehaviorMPWaterWheel(BlockEntity blockentity)
		: base(blockentity)
	{
	}

	public override void Initialize(ICoreAPI api, JsonObject properties)
	{
		bebconstructable = Blockentity.GetBehavior<BEBehaviorRightClickConstructable>();
		BEBehaviorRightClickConstructable bEBehaviorRightClickConstructable = bebconstructable;
		if (bEBehaviorRightClickConstructable != null)
		{
			bEBehaviorRightClickConstructable.OnAttemptConstruct = OnAttemptConstruction;
		}
		facing = BlockFacing.FromCode(base.Block.Variant["side"]).Opposite;
		base.Initialize(api, properties);
		sound = new AssetLocation("sounds/block/waterwheel-wood");
		Blockentity.RegisterGameTickListener(CheckWater, 1000);
		diameter = properties["diameter"].AsInt(3);
		requiresMinFlowSpeed = properties["requiresMinFlowSpeed"].AsFloat(1.5f);
		bebconstructable.OnShapeChanged += delegate(CompositeShape shape)
		{
			Shape = shape;
		};
	}

	protected override CompositeShape GetShape()
	{
		return bebconstructable?.shape;
	}

	public override MechPowerPath[] GetMechPowerExits(MechPowerPath entryDir)
	{
		return new MechPowerPath[2]
		{
			entryDir,
			new MechPowerPath(entryDir.OutFacing.Opposite, entryDir.gearingRatio, Position, !entryDir.invert)
		};
	}

	public bool OnAttemptConstruction(IPlayer byPlayer, BlockSelection blockSel)
	{
		CheckWater(0f);
		if (blocked)
		{
			(Api as ICoreClientAPI)?.TriggerIngameError(this, "waterwheelblocked", Lang.Get("error-wheel-blocked"));
			return false;
		}
		if (suitablePowerSourceBlockCount == 0)
		{
			(Api as ICoreClientAPI)?.TriggerIngameError(this, "waterwheelnowater", Lang.Get("error-wheel-nowater"));
		}
		return true;
	}

	protected void CheckWater(float dt)
	{
		bool incomplete = !bebconstructable.IsComplete;
		suitablePowerSourceBlockCount = 0;
		flowVec.Set(0f, 0f, 0f);
		BlockFacing cW = facing.GetCW();
		int wheelX = cW.Normali.X;
		int wheelZ = cW.Normali.Z;
		FastVec3f axialVec = new FastVec3f(facing.Normalf);
		blocked = false;
		int radius = diameter / 2;
		if (radius < 1)
		{
			flowRate = 0f;
			return;
		}
		float moment = 0f;
		BlockPos pos = new BlockPos(base.Pos.dimension);
		FastVec3i firstWaterPos = new FastVec3i(-1, -1, -1);
		FastVec3i lastWaterPos = new FastVec3i(-1, -1, -1);
		Block firstsWaterBlock = null;
		Block lastWaterBlock = null;
		int numberOfPoints = 4 + radius * 4;
		for (int i = 0; i < numberOfPoints; i++)
		{
			int dX = (int)(GameMath.Sin((float)Math.PI * 2f * (float)i / (float)numberOfPoints) * ((float)radius + 0.5f));
			int dY = (int)(GameMath.Cos((float)Math.PI * 2f * (float)i / (float)numberOfPoints) * ((float)radius + 0.5f));
			int dZ = dX * wheelZ;
			dX *= wheelX;
			pos.Set(base.Pos.X + dX, base.Pos.Y + dY, base.Pos.Z + dZ);
			Block block = Api.World.BlockAccessor.GetBlock(pos, 4);
			BlockFacing blockSide = BlockFacing.UP;
			if (Math.Abs(dY) < Math.Abs(dX + dZ))
			{
				blockSide = ((dX != 0) ? ((dX > 0) ? BlockFacing.WEST : BlockFacing.EAST) : ((dZ > 0) ? BlockFacing.NORTH : BlockFacing.SOUTH));
			}
			else if (dY > 0)
			{
				blockSide = BlockFacing.DOWN;
			}
			if (blockedBy(block, pos, blockSide))
			{
				flowRate = 0f;
				return;
			}
			if (!block.ForFluidsLayer)
			{
				block = Api.World.BlockAccessor.GetBlock(pos, 2);
			}
			if (block is IBlockFlowing { IsStill: false } waterBlock && getFlowSpeed(block) > requiresMinFlowSpeed)
			{
				FastVec3f pushVector = waterBlock.GetPushVector(pos);
				moment += new FastVec3f(dX, dY, dZ).Normalize().Cross(axialVec).Dot(pushVector);
				suitablePowerSourceBlockCount++;
				lastWaterPos.Set(pos);
				lastWaterBlock = block;
				if (firstsWaterBlock == null)
				{
					firstWaterPos.Set(pos);
					firstsWaterBlock = block;
				}
			}
		}
		moment *= (float)radius;
		if (moment > 0f)
		{
			lastWaterBlock = firstsWaterBlock;
			lastWaterPos = firstWaterPos;
		}
		if (lastWaterBlock != null)
		{
			pos.Set(lastWaterPos);
			pos.Down();
			if (!ReplaceRapidWater(pos) && lastWaterBlock.LiquidLevel > 1)
			{
				pos.Up();
				FastVec3f pushVector2 = (lastWaterBlock as IBlockFlowing).GetPushVector(pos).Normalize();
				pos.Add(pushVector2.X * 1.5f, 0f, pushVector2.Z * 1.5f);
				if (!ReplaceRapidWater(pos) && Math.Abs(pushVector2.X) + Math.Abs(pushVector2.Z) > 1f)
				{
					pos.Add(pushVector2.X * -1.5f, 0f, 0f);
					if (!ReplaceRapidWater(pos))
					{
						pos.Add(pushVector2.X * 1.5f, 0f, (0f - pushVector2.Z) * 1.5f);
						ReplaceRapidWater(pos);
					}
				}
			}
		}
		flowRate = Math.Abs(moment * 750f);
		if (flowRate > 0f)
		{
			float prevDir = dir;
			dir = Math.Sign(moment) * -1;
			if (dir != prevDir)
			{
				bool oppositeDir = dir < 0f;
				SetPropagationDirection(new MechPowerPath(oppositeDir ? facing.Opposite : facing, 1f, base.Pos));
			}
		}
		if (incomplete)
		{
			flowRate = 0f;
			flowVec.Set(0f, 0f, 0f);
		}
	}

	protected bool ReplaceRapidWater(BlockPos pos)
	{
		Block nextBlock = Api.World.BlockAccessor.GetBlock(pos, 2);
		BlockBehaviorFiniteSpreadingLiquid bh = nextBlock.GetBehavior<BlockBehaviorFiniteSpreadingLiquid>();
		if (bh != null && nextBlock.LiquidCode == "water")
		{
			if (!bh.multiplySpread)
			{
				string code = nextBlock.Code.FirstCodePart();
				code = "water" + nextBlock.Code.Path.Substring(code.Length);
				Block? block = Api.World.GetBlock(new AssetLocation(nextBlock.Code.Domain, code));
				int newBlockId = block?.Id ?? 0;
				Api.World.BlockAccessor.SetBlock(newBlockId, pos, 2);
				block.OnNeighbourBlockChange(Api.World, pos, pos);
				Api.World.BlockAccessor.TriggerNeighbourBlockUpdate(pos);
			}
			return true;
		}
		return false;
	}

	public override MechanicalNetwork CreateJoinAndDiscoverNetwork(BlockFacing powerOutFacing)
	{
		MechanicalNetwork newNetwork = base.CreateJoinAndDiscoverNetwork(powerOutFacing);
		if (powerOutFacing == facing && newNetwork != null)
		{
			BlockPos neibPos = Position.AddCopy(powerOutFacing.Opposite);
			BEBehaviorMPBase beMechBase = Api.World.BlockAccessor.GetBlockEntity(neibPos)?.GetBehavior<BEBehaviorMPBase>();
			if (beMechBase != null)
			{
				Api.Event.EnqueueMainThreadTask(delegate
				{
					if (Api.World.BlockAccessor.GetBlock(neibPos) is IMechanicalPowerBlock mechanicalPowerBlock && mechanicalPowerBlock.HasMechPowerConnectorAt(Api.World, neibPos, facing, base.Block as BlockMPBase))
					{
						beMechBase.tryConnect(facing);
					}
				}, "waterwheel 2nd axis");
			}
		}
		return newNetwork;
	}

	private float getFlowSpeed(Block block)
	{
		return block.Attributes?["flowspeed"].AsFloat() ?? 0f;
	}

	protected bool blockedBy(Block block, BlockPos pos, BlockFacing face)
	{
		if (block.SideIsSolid(pos, face.Index))
		{
			blocked = true;
			flowRate = 0f;
			return true;
		}
		return false;
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder sb)
	{
		base.GetBlockInfo(forPlayer, sb);
		if (!bebconstructable.IsComplete)
		{
			sb.AppendLine(Lang.Get("waterwheel-construction-step-with-stage-current-and-total", bebconstructable.CurrentCompletedStage, bebconstructable.Stages));
			if (blocked)
			{
				sb.AppendLine(Lang.Get("Wheel is blocked, make sure the entire wheel is free from solid blocks."));
			}
			return;
		}
		if (blocked)
		{
			sb.AppendLine(Lang.Get("Wheel is blocked, make sure the entire wheel is free from solid blocks."));
		}
		else
		{
			sb.AppendLine(Lang.Get("waterwheel-suitable-power-source-blocks-nearby-blockamount", suitablePowerSourceBlockCount));
		}
		if (Api.World.EntityDebugMode)
		{
			sb.AppendLine("<font color='#ccc'>flow vector= " + flowVec?.ToString() + "</font>");
			sb.AppendLine("<font color='#ccc'>torque= " + TorqueFactor + "</font>");
			sb.AppendLine("<font color='#ccc'>targetspeed= " + TargetSpeed + "</font>");
			sb.AppendLine("<font color='#ccc'>flowrate = " + flowRate + "</font>");
		}
	}
}
