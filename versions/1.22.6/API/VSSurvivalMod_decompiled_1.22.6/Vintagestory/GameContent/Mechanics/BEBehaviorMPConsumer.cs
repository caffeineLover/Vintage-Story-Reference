using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent.Mechanics;

public class BEBehaviorMPConsumer : BEBehaviorMPBase
{
	public float Resistance = 0.1f;

	public Action OnConnected;

	public Action OnDisconnected;

	public override float AngleRad => base.AngleRad;

	public float TrueSpeed => Math.Abs((base.Network?.Speed * base.GearedRatio).GetValueOrDefault());

	public BEBehaviorMPConsumer(BlockEntity blockentity)
		: base(blockentity)
	{
	}

	public override void Initialize(ICoreAPI api, JsonObject properties)
	{
		base.Initialize(api, properties);
		Shape = properties["mechPartShape"].AsObject(base.Block.Shape);
		Shape?.Base.WithPathPrefixOnce("shapes/").WithPathAppendixOnce(".json");
		Resistance = properties["resistance"].AsFloat(0.1f);
		switch (base.Block.Variant["side"])
		{
		case "north":
			AxisSign = new int[3] { 0, 0, -1 };
			break;
		case "east":
			AxisSign = new int[3] { -1, 0, 0 };
			break;
		case "west":
			AxisSign = new int[3] { -1, 0, 0 };
			break;
		case "south":
			AxisSign = new int[3] { 0, 0, -1 };
			break;
		}
	}

	public override void JoinNetwork(MechanicalNetwork network)
	{
		base.JoinNetwork(network);
		OnConnected?.Invoke();
	}

	public override void LeaveNetwork()
	{
		base.LeaveNetwork();
		OnDisconnected?.Invoke();
	}

	public override float GetResistance()
	{
		return Resistance;
	}

	public override MechPowerPath[] GetMechPowerExits(MechPowerPath fromExitTurnDir)
	{
		return Array.Empty<MechPowerPath>();
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
	{
		if (base.Block == null)
		{
			return false;
		}
		return true;
	}
}
