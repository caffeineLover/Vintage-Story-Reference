using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent.Mechanics;

public abstract class BEBehaviorMPBase : BlockEntityBehavior, IMechanicalPowerDevice, IMechanicalPowerRenderable, IMechanicalPowerNode
{
	protected static readonly bool DEBUG;

	protected MechanicalPowerMod manager;

	protected MechanicalNetwork network;

	public Vec4f lightRbs = new Vec4f();

	private CompositeShape shape;

	protected BlockFacing propagationDir = BlockFacing.NORTH;

	private float gearedRatio = 1f;

	protected float lastKnownAngleRad;

	public bool disconnected;

	public virtual BlockPos Position => Blockentity.Pos;

	public virtual Vec4f LightRgba => lightRbs;

	public virtual CompositeShape Shape
	{
		get
		{
			return shape;
		}
		set
		{
			CompositeShape prev = Shape;
			if (prev != null && manager != null && prev != value)
			{
				manager.RemoveDeviceForRender(this);
				shape = value;
				manager.AddDeviceForRender(this);
			}
			else
			{
				shape = value;
			}
		}
	}

	public virtual int[] AxisSign { get; protected set; }

	public long NetworkId { get; set; }

	public MechanicalNetwork Network => network;

	public virtual BlockFacing OutFacingForNetworkDiscovery { get; protected set; }

	public float GearedRatio
	{
		get
		{
			return gearedRatio;
		}
		set
		{
			gearedRatio = value;
		}
	}

	public float OverheatValue { get; set; }

	public virtual float AngleRad
	{
		get
		{
			if (network == null)
			{
				return lastKnownAngleRad;
			}
			if (IsRotationReversed())
			{
				return lastKnownAngleRad = (float)Math.PI * 2f - network.AngleRad * gearedRatio % ((float)Math.PI * 2f);
			}
			return lastKnownAngleRad = network.AngleRad * gearedRatio % ((float)Math.PI * 2f);
		}
	}

	public BlockPos GetPosition()
	{
		return Position;
	}

	public virtual float GetGearedRatio(BlockFacing face)
	{
		return gearedRatio;
	}

	public virtual bool IsRotationReversed()
	{
		if (propagationDir == null)
		{
			return false;
		}
		if (propagationDir != BlockFacing.DOWN && propagationDir != BlockFacing.EAST)
		{
			return propagationDir == BlockFacing.SOUTH;
		}
		return true;
	}

	public virtual bool IsInvertedNetworkFor(BlockPos pos)
	{
		if (propagationDir == null || pos == null)
		{
			return false;
		}
		return !Position.AddCopy(propagationDir).Equals(pos);
	}

	public BEBehaviorMPBase(BlockEntity blockentity)
		: base(blockentity)
	{
	}

	public override void Initialize(ICoreAPI api, JsonObject properties)
	{
		base.Initialize(api, properties);
		Shape = GetShape();
		manager = Api.ModLoader.GetModSystem<MechanicalPowerMod>();
		if (Api.World.Side == EnumAppSide.Client)
		{
			lightRbs = Api.World.BlockAccessor.GetLightRGBs(Blockentity.Pos);
			if (NetworkId > 0)
			{
				network = manager.GetOrCreateNetwork(NetworkId);
				JoinNetwork(network);
			}
		}
		manager.AddDeviceForRender(this);
		AxisSign = new int[3] { 0, 0, 1 };
		SetOrientations();
		if (api.Side == EnumAppSide.Server && OutFacingForNetworkDiscovery != null)
		{
			CreateJoinAndDiscoverNetwork(OutFacingForNetworkDiscovery);
		}
	}

	protected virtual CompositeShape GetShape()
	{
		return base.Block.Shape;
	}

	public virtual void SetOrientations()
	{
	}

	public virtual void WasPlaced(BlockFacing connectedOnFacing)
	{
		if ((Api.Side != EnumAppSide.Client && OutFacingForNetworkDiscovery != null) || connectedOnFacing == null)
		{
			return;
		}
		if (!tryConnect(connectedOnFacing))
		{
			if (DEBUG)
			{
				Api.Logger.Notification("Was placed fail connect 2nd: " + connectedOnFacing?.ToString() + " at " + Position);
			}
		}
		else if (DEBUG)
		{
			Api.Logger.Notification("Was placed connected 1st: " + connectedOnFacing?.ToString() + " at " + Position);
		}
	}

	public bool tryConnect(BlockFacing toFacing)
	{
		Vec3i unloadedChunkPos;
		return tryConnect(toFacing, out unloadedChunkPos);
	}

	public bool tryConnect(BlockFacing toFacing, out Vec3i unloadedChunkPos)
	{
		unloadedChunkPos = null;
		if (Api == null)
		{
			return false;
		}
		BlockPos pos = Position.AddCopy(toFacing);
		IMechanicalPowerBlock connectedToBlock = Api.World.BlockAccessor.GetBlock(pos) as IMechanicalPowerBlock;
		if (DEBUG)
		{
			Api.Logger.Notification("tryConnect at " + Position?.ToString() + " towards " + toFacing?.ToString() + " " + pos);
		}
		if (connectedToBlock == null || !connectedToBlock.HasMechPowerConnectorAt(Api.World, pos, toFacing.Opposite, base.Block as BlockMPBase))
		{
			return false;
		}
		MechanicalNetwork newNetwork = connectedToBlock.GetNetwork(Api.World, pos);
		if (newNetwork != null)
		{
			IMechanicalPowerDevice node = Api.World.BlockAccessor.GetBlockEntity(pos).GetBehavior<BEBehaviorMPBase>();
			connectedToBlock.DidConnectAt(Api.World, pos, toFacing.Opposite);
			MechPowerPath curPath = new MechPowerPath(toFacing, node.GetGearedRatio(toFacing), pos, !node.IsPropagationDirection(Position, toFacing));
			curPath.turnDir = node.GetPropagatingTurnDir(toFacing.Opposite);
			SetPropagationDirection(curPath);
			MechPowerPath[] paths = GetMechPowerExits(curPath);
			JoinNetwork(newNetwork);
			for (int i = 0; i < paths.Length; i++)
			{
				if (DEBUG)
				{
					Api.Logger.Notification("== spreading path " + (paths[i].invert ? "-" : "") + paths[i].OutFacing?.ToString() + "  " + paths[i].gearingRatio);
				}
				BlockPos exitPos = Position.AddCopy(paths[i].OutFacing);
				if (!spreadTo(Api, newNetwork, exitPos, paths[i], out unloadedChunkPos))
				{
					return false;
				}
			}
			return true;
		}
		if (network != null)
		{
			BEBehaviorMPBase node2 = Api.World.BlockAccessor.GetBlockEntity(pos)?.GetBehavior<BEBehaviorMPBase>();
			if (node2 != null)
			{
				return node2.tryConnect(toFacing.Opposite, out unloadedChunkPos);
			}
		}
		return false;
	}

	public virtual void JoinNetwork(MechanicalNetwork network)
	{
		if (this.network != null && this.network != network)
		{
			LeaveNetwork();
		}
		if (this.network == null)
		{
			this.network = network;
			network?.Join(this);
		}
		if (network == null)
		{
			NetworkId = 0L;
		}
		else
		{
			NetworkId = network.networkId;
		}
		Blockentity.MarkDirty();
	}

	public virtual void LeaveNetwork()
	{
		if (DEBUG)
		{
			Api.Logger.Notification("Leaving network " + NetworkId + " at " + Position);
		}
		network?.Leave(this);
		network = null;
		NetworkId = 0L;
		Blockentity.MarkDirty();
	}

	public override void OnBlockBroken(IPlayer byPlayer = null)
	{
		disconnected = true;
	}

	public override void OnBlockRemoved()
	{
		base.OnBlockRemoved();
		if (network != null)
		{
			manager.OnNodeRemoved(this);
		}
		LeaveNetwork();
		manager.RemoveDeviceForRender(this);
	}

	public override void OnBlockUnloaded()
	{
		base.OnBlockUnloaded();
		network?.DidUnload(this);
		manager?.RemoveDeviceForRender(this);
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
	{
		lightRbs = Api.World.BlockAccessor.GetLightRGBs(Blockentity.Pos);
		return true;
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		base.FromTreeAttributes(tree, worldAccessForResolve);
		long nowNetworkId = tree.GetLong("networkid", 0L);
		if (worldAccessForResolve.Side == EnumAppSide.Client)
		{
			propagationDir = BlockFacing.ALLFACES[tree.GetInt("turnDirFromFacing")];
			gearedRatio = tree.GetFloat("g");
			if (NetworkId != nowNetworkId)
			{
				NetworkId = 0L;
				if (worldAccessForResolve.Side == EnumAppSide.Client)
				{
					NetworkId = nowNetworkId;
					if (NetworkId == 0L)
					{
						LeaveNetwork();
						network = null;
					}
					else if (manager != null)
					{
						network = manager.GetOrCreateNetwork(NetworkId);
						JoinNetwork(network);
						Blockentity.MarkDirty();
					}
				}
			}
		}
		SetOrientations();
		updateShape(worldAccessForResolve);
	}

	protected virtual void updateShape(IWorldAccessor worldForResolve)
	{
		Shape = GetShape();
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		tree.SetLong("networkid", NetworkId);
		tree.SetInt("turnDirFromFacing", propagationDir.Index);
		tree.SetFloat("g", gearedRatio);
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder sb)
	{
		if (Api.World.EntityDebugMode && forPlayer != null && forPlayer.WorldData?.CurrentGameMode == EnumGameMode.Creative)
		{
			global::_003C_003Ey__InlineArray4<object> buffer = default(global::_003C_003Ey__InlineArray4<object>);
			buffer[0] = NetworkId;
			buffer[1] = propagationDir;
			buffer[2] = network?.TurnDir.ToString();
			buffer[3] = gearedRatio;
			sb.AppendLine(string.Format("<font color='#ccc'>networkid: {0}  turnDir: {1}  {2}  {3:G3}</font>", (ReadOnlySpan<object?>)buffer));
			global::_003C_003Ey__InlineArray4<object> buffer2 = default(global::_003C_003Ey__InlineArray4<object>);
			buffer2[0] = network?.Speed * GearedRatio;
			buffer2[1] = network?.TotalAvailableTorque / GearedRatio;
			buffer2[2] = network?.NetworkTorque / GearedRatio;
			buffer2[3] = network?.NetworkResistance / GearedRatio;
			sb.AppendLine(string.Format("<font color='#ccc'>speed: {0:G4}  avail torque: {1:G4}  torque sum: {2:G4}  resist sum: {3:G4}</font>", (ReadOnlySpan<object?>)buffer2));
		}
	}

	public virtual BlockFacing GetPropagationDirection()
	{
		return propagationDir;
	}

	public virtual BlockFacing GetPropagationDirectionInput()
	{
		return propagationDir;
	}

	public virtual bool IsPropagationDirection(BlockPos fromPos, BlockFacing test)
	{
		return propagationDir == test;
	}

	public virtual void SetPropagationDirection(MechPowerPath path)
	{
		BlockFacing turnDir = path.NetworkDir();
		if (propagationDir == turnDir.Opposite && network != null)
		{
			if (!network.DirectionHasReversed)
			{
				network.TurnDir = ((network.TurnDir == EnumRotDirection.Clockwise) ? EnumRotDirection.Counterclockwise : EnumRotDirection.Clockwise);
			}
			network.DirectionHasReversed = true;
		}
		propagationDir = turnDir;
		GearedRatio = path.gearingRatio;
		if (DEBUG)
		{
			Api.Logger.Notification("setting dir " + propagationDir?.ToString() + " " + Position);
		}
	}

	public virtual float GetTorque(long tick, float speed, out float resistance)
	{
		resistance = GetResistance();
		return 0f;
	}

	public abstract float GetResistance();

	public virtual void DestroyJoin(BlockPos pos)
	{
	}

	public virtual MechanicalNetwork CreateJoinAndDiscoverNetwork(BlockFacing powerOutFacing)
	{
		BlockPos neibPos = Position.AddCopy(powerOutFacing);
		IMechanicalPowerBlock neibMechBlock = null;
		neibMechBlock = Api.World.BlockAccessor.GetBlock(neibPos) as IMechanicalPowerBlock;
		if (neibMechBlock != null && !neibMechBlock.HasMechPowerConnectorAt(Api.World, neibPos, powerOutFacing.Opposite, base.Block as BlockMPBase))
		{
			neibMechBlock = null;
		}
		MechanicalNetwork neibNetwork = neibMechBlock?.GetNetwork(Api.World, neibPos);
		if (neibNetwork == null || !neibNetwork.Valid)
		{
			MechanicalNetwork newNetwork = network;
			if (newNetwork == null)
			{
				newNetwork = manager.CreateNetwork(this);
				JoinNetwork(newNetwork);
				if (DEBUG)
				{
					Api.Logger.Notification("===setting inturn at " + Position?.ToString() + " " + powerOutFacing);
				}
				SetPropagationDirection(new MechPowerPath(powerOutFacing, 1f));
			}
			if (!spreadTo(Api, newNetwork, neibPos, new MechPowerPath(GetPropagationDirection(), gearedRatio), out var missingChunkPos))
			{
				network.AwaitChunkThenDiscover(missingChunkPos);
				manager.testFullyLoaded(network);
				return network;
			}
			IMechanicalPowerDevice node = Api.World.BlockAccessor.GetBlockEntity(neibPos)?.GetBehavior<BEBehaviorMPBase>();
			if (node != null)
			{
				BlockFacing facing = (node.IsPropagationDirection(Position, powerOutFacing) ? powerOutFacing : powerOutFacing.Opposite);
				SetPropagationDirection(new MechPowerPath(facing, node.GetGearedRatio(facing.Opposite), neibPos));
			}
		}
		else
		{
			BEBehaviorMPBase neib = Api.World.BlockAccessor.GetBlockEntity(neibPos).GetBehavior<BEBehaviorMPBase>();
			if (OutFacingForNetworkDiscovery != null)
			{
				if (tryConnect(OutFacingForNetworkDiscovery, out var missingChunkPos2))
				{
					gearedRatio = neib.GetGearedRatio(OutFacingForNetworkDiscovery);
				}
				else if (missingChunkPos2 != null)
				{
					network.AwaitChunkThenDiscover(missingChunkPos2);
					manager.testFullyLoaded(network);
					return network;
				}
			}
			else
			{
				JoinNetwork(neibNetwork);
				SetPropagationDirection(new MechPowerPath(neib.propagationDir, neib.GetGearedRatio(neib.propagationDir), neibPos));
			}
		}
		return network;
	}

	public virtual bool JoinAndSpreadNetworkToNeighbours(ICoreAPI api, MechanicalNetwork network, MechPowerPath exitTurnDir, out Vec3i missingChunkPos)
	{
		missingChunkPos = null;
		if (this.network?.networkId == network?.networkId)
		{
			return true;
		}
		if (DEBUG)
		{
			api.Logger.Notification("Spread to " + Position?.ToString() + " with direction " + exitTurnDir.OutFacing?.ToString() + (exitTurnDir.invert ? "-" : "") + " Network:" + network.networkId);
		}
		SetPropagationDirection(exitTurnDir);
		JoinNetwork(network);
		(base.Block as IMechanicalPowerBlock).DidConnectAt(api.World, Position, exitTurnDir.OutFacing.Opposite);
		MechPowerPath[] paths = GetMechPowerExits(exitTurnDir);
		for (int i = 0; i < paths.Length; i++)
		{
			paths = GetMechPowerExits(exitTurnDir);
			if (DEBUG)
			{
				api.Logger.Notification("-- spreading path " + (paths[i].invert ? "-" : "") + paths[i].OutFacing?.ToString() + "  " + paths[i].gearingRatio);
			}
			BlockPos exitPos = Position.AddCopy(paths[i].OutFacing);
			if (!spreadTo(api, network, exitPos, paths[i], out missingChunkPos))
			{
				return false;
			}
		}
		return true;
	}

	protected virtual bool spreadTo(ICoreAPI api, MechanicalNetwork network, BlockPos exitPos, MechPowerPath propagatePath, out Vec3i missingChunkPos)
	{
		missingChunkPos = null;
		BEBehaviorMPBase beMechBase = api.World.BlockAccessor.GetBlockEntity(exitPos)?.GetBehavior<BEBehaviorMPBase>();
		IMechanicalPowerBlock mechBlock = beMechBase?.Block as IMechanicalPowerBlock;
		if (DEBUG)
		{
			api.Logger.Notification("attempting spread to " + exitPos?.ToString() + ((beMechBase == null) ? " -" : ""));
		}
		if (beMechBase == null && api.World.BlockAccessor.GetChunkAtBlockPos(exitPos) == null)
		{
			if (OutsideMap(api.World.BlockAccessor, exitPos))
			{
				return true;
			}
			missingChunkPos = new Vec3i(exitPos.X / 32, exitPos.Y / 32, exitPos.Z / 32);
			return false;
		}
		if (beMechBase != null && mechBlock.HasMechPowerConnectorAt(api.World, exitPos, propagatePath.OutFacing.Opposite, base.Block as BlockMPBase))
		{
			beMechBase.Api = api;
			if (!beMechBase.JoinAndSpreadNetworkToNeighbours(api, network, propagatePath, out missingChunkPos))
			{
				return false;
			}
		}
		else if (DEBUG)
		{
			api.Logger.Notification("no connector at " + exitPos?.ToString() + " " + propagatePath.OutFacing.Opposite);
		}
		return true;
	}

	private bool OutsideMap(IBlockAccessor blockAccessor, BlockPos exitPos)
	{
		if (exitPos.X < 0 || exitPos.X >= blockAccessor.MapSizeX)
		{
			return true;
		}
		if (exitPos.Y < 0 || exitPos.Y >= blockAccessor.MapSizeY)
		{
			return true;
		}
		if (exitPos.Z < 0 || exitPos.Z >= blockAccessor.MapSizeZ)
		{
			return true;
		}
		return false;
	}

	public virtual MechPowerPath[] GetMechPowerExits(MechPowerPath entryDir)
	{
		return new MechPowerPath[2]
		{
			entryDir,
			new MechPowerPath(entryDir.OutFacing.Opposite, entryDir.gearingRatio, Position, !entryDir.invert)
		};
	}

	public override void OnPlacementBySchematic(ICoreServerAPI api, IBlockAccessor blockAccessor, BlockPos pos, Dictionary<int, Dictionary<int, int>> replaceBlocks, int centerrockblockid, Block layerBlock, bool resolveImports)
	{
		base.OnPlacementBySchematic(api, blockAccessor, pos, replaceBlocks, centerrockblockid, layerBlock, resolveImports);
		BlockFacing[] aLLFACES = BlockFacing.ALLFACES;
		foreach (BlockFacing blockFacing in aLLFACES)
		{
			tryConnect(blockFacing);
		}
	}

	public virtual BlockFacing GetPropagatingTurnDir(BlockFacing toFacing)
	{
		return null;
	}
}
