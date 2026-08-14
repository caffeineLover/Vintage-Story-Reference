using System;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent.Mechanics;

public class BEBehaviorMPRotor : BEBehaviorMPBase
{
	protected double capableSpeed;

	protected double lastMsAngle;

	protected BlockFacing ownFacing;

	protected virtual AssetLocation Sound { get; }

	protected virtual float Resistance { get; }

	protected virtual double AccelerationFactor { get; }

	protected virtual float TargetSpeed { get; }

	protected virtual float TorqueFactor { get; }

	public override float AngleRad
	{
		get
		{
			if (Sound != null)
			{
				MechanicalNetwork mechanicalNetwork = network;
				if (mechanicalNetwork != null && mechanicalNetwork.Speed > 0f && (double)Api.World.ElapsedMilliseconds - lastMsAngle > (double)(500f / network.Speed) && Api.Side == EnumAppSide.Client)
				{
					Api.World.PlaySoundAt(Sound, Position, 0.0, null, randomizePitch: false, 18f, GetSoundVolume());
					lastMsAngle = Api.World.ElapsedMilliseconds;
				}
			}
			return base.AngleRad;
		}
	}

	protected virtual float GetSoundVolume()
	{
		return 0f;
	}

	public BEBehaviorMPRotor(BlockEntity blockentity)
		: base(blockentity)
	{
		Blockentity = blockentity;
		string orientation = blockentity.Block.Variant["side"];
		ownFacing = BlockFacing.FromCode(orientation);
		OutFacingForNetworkDiscovery = ownFacing.Opposite;
	}

	public override void Initialize(ICoreAPI api, JsonObject properties)
	{
		base.Initialize(api, properties);
		switch (ownFacing.Code)
		{
		case "north":
		case "south":
			AxisSign = new int[3] { 0, 0, -1 };
			break;
		case "east":
		case "west":
			AxisSign = new int[3] { -1, 0, 0 };
			break;
		}
		if (Api.Side == EnumAppSide.Client)
		{
			updateShape(api.World);
		}
	}

	public override float GetResistance()
	{
		return Resistance;
	}

	public override float GetTorque(long tick, float speed, out float resistance)
	{
		float targetSpeed = TargetSpeed;
		capableSpeed += ((double)targetSpeed - capableSpeed) * AccelerationFactor;
		float dir = ((propagationDir == OutFacingForNetworkDiscovery) ? 1f : (-1f));
		float absSpeed = Math.Abs(speed);
		double excessSpeed = (double)absSpeed - capableSpeed;
		bool wrongDirection = dir * speed < 0f;
		resistance = (float)(wrongDirection ? ((double)(Resistance * TorqueFactor * Math.Min(0.8f, absSpeed * 400f))) : ((excessSpeed > 0.0) ? ((double)Resistance * Math.Min(0.20000000298023224, excessSpeed * excessSpeed * 80.0)) : 0.0));
		double power = (wrongDirection ? capableSpeed : (capableSpeed - (double)absSpeed));
		return (float)Math.Max(0.0, power) * TorqueFactor * dir;
	}

	public override void WasPlaced(BlockFacing connectedOnFacing)
	{
	}

	public override MechPowerPath[] GetMechPowerExits(MechPowerPath fromExitTurnDir)
	{
		return Array.Empty<MechPowerPath>();
	}
}
