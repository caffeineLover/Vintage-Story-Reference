using System;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent.Mechanics;

public class BEBehaviorWindmillRotor : BEBehaviorMPRotor
{
	protected WeatherSystemBase weatherSystem;

	protected double windSpeed;

	protected int sailLength;

	private AssetLocation sound;

	protected bool turbulenceExposed;

	protected float powerMul = 1f;

	public int SailLength => sailLength;

	protected override AssetLocation Sound => sound;

	protected override float Resistance => 0.003f;

	protected override double AccelerationFactor => 0.05;

	protected override float TargetSpeed => (float)Math.Min(0.6000000238418579, windSpeed);

	protected override float TorqueFactor => (float)sailLength / 4f * powerMul * (turbulenceExposed ? 0.5f : 1f);

	protected virtual float SelfTurbulenceRadius => (float)sailLength * 1.5f;

	private ItemStack SailStack
	{
		get
		{
			JsonItemStack? jsonItemStack = base.Block.Attributes["sailStack"].AsObject<JsonItemStack>();
			jsonItemStack.Resolve(Api.World, string.Concat(base.Block.Code, " sail stack"));
			return jsonItemStack.ResolvedItemstack;
		}
	}

	protected override float GetSoundVolume()
	{
		return (0.5f + 0.5f * (float)windSpeed * (turbulenceExposed ? 0.5f : 1f)) * (float)sailLength / 3f;
	}

	public BEBehaviorWindmillRotor(BlockEntity blockentity)
		: base(blockentity)
	{
	}

	public override void Initialize(ICoreAPI api, JsonObject properties)
	{
		base.Initialize(api, properties);
		powerMul = properties["powerMul"].AsFloat(1f);
		sound = new AssetLocation("sounds/effect/swoosh");
		weatherSystem = Api.ModLoader.GetModSystem<WeatherSystemBase>();
		Blockentity.RegisterGameTickListener(CheckWindSpeed, 1000);
		if (api.Side == EnumAppSide.Server)
		{
			api.ModLoader.GetModSystem<ModSystemWindTurbulence>().SetTurbulence(base.Pos, SelfTurbulenceRadius);
			if (SailLength > 0)
			{
				turbulenceExposed = api.ModLoader.GetModSystem<ModSystemWindTurbulence>().GetNearestTurbulenceDistance(base.Pos) < SelfTurbulenceRadius;
			}
		}
	}

	protected void CheckWindSpeed(float dt)
	{
		windSpeed = weatherSystem.WeatherDataSlowAccess.GetWindSpeed(Blockentity.Pos.ToVec3d());
		if (Api.World.BlockAccessor.GetLightLevel(Blockentity.Pos, EnumLightLevelType.OnlySunLight) < 5 && Api.World.Config.GetString("undergroundWindmills", "false") != "true")
		{
			windSpeed = 0.0;
		}
		if (Api.Side != EnumAppSide.Server || sailLength <= 0 || !(Api.World.Rand.NextDouble() < 0.1))
		{
			return;
		}
		bool num = turbulenceExposed;
		float dist = Api.ModLoader.GetModSystem<ModSystemWindTurbulence>().GetNearestTurbulenceDistance(base.Pos);
		turbulenceExposed = dist < SelfTurbulenceRadius;
		if (num != turbulenceExposed)
		{
			Blockentity.MarkDirty(redrawOnClient: true);
		}
		if (obstructed(sailLength + 1))
		{
			Api.World.PlaySoundAt(new AssetLocation("sounds/effect/toolbreak"), Position, 0.0, null, randomizePitch: false, 20f);
			while (sailLength-- > 0)
			{
				Api.World.SpawnItemEntity(SailStack, Blockentity.Pos);
			}
			sailLength = 0;
			Blockentity.MarkDirty(redrawOnClient: true);
			network.updateNetwork(manager.getTickNumber());
		}
	}

	public override void OnBlockBroken(IPlayer byPlayer = null)
	{
		while (sailLength-- > 0)
		{
			Api.World.SpawnItemEntity(SailStack, Blockentity.Pos);
		}
		base.OnBlockBroken(byPlayer);
	}

	public bool OnInteract(IPlayer byPlayer)
	{
		if (sailLength >= base.Block.Attributes["sailedShapes"]["maxLength"].AsInt())
		{
			return false;
		}
		ItemSlot slot = byPlayer.InventoryManager.ActiveHotbarSlot;
		if (slot.Empty || slot.StackSize < 4)
		{
			return false;
		}
		ItemStack sailStack = SailStack;
		if (!slot.Itemstack.Equals(Api.World, sailStack, GlobalConstants.IgnoredStackAttributes))
		{
			return false;
		}
		if (obstructed(sailLength + 2))
		{
			if (Api.Side == EnumAppSide.Client)
			{
				(Api as ICoreClientAPI).TriggerIngameError(this, "notenoughspace", Lang.Get("Cannot add more sails. Make sure there's space for the sails to rotate freely"));
			}
			return false;
		}
		if (byPlayer.WorldData.CurrentGameMode != EnumGameMode.Creative)
		{
			slot.TakeOut(sailStack.StackSize);
			slot.MarkDirty();
		}
		sailLength++;
		updateShape(Api.World);
		if (Api.Side == EnumAppSide.Server)
		{
			Api.ModLoader.GetModSystem<ModSystemWindTurbulence>().SetTurbulence(base.Pos, SelfTurbulenceRadius);
		}
		Blockentity.MarkDirty(redrawOnClient: true);
		return true;
	}

	protected bool obstructed(int len)
	{
		BlockPos tmpPos = new BlockPos(Position.dimension);
		for (int dxz = -len; dxz <= len; dxz++)
		{
			for (int dy = -len; dy <= len; dy++)
			{
				if ((dxz != 0 || dy != 0) && (len <= 1 || Math.Abs(dxz) != len || Math.Abs(dy) != len))
				{
					int dx = ((ownFacing.Axis == EnumAxis.Z) ? dxz : 0);
					int dz = ((ownFacing.Axis == EnumAxis.X) ? dxz : 0);
					tmpPos.Set(Position.X + dx, Position.Y + dy, Position.Z + dz);
					Block block = Api.World.BlockAccessor.GetBlock(tmpPos);
					Cuboidf[] collBoxes = block.GetCollisionBoxes(Api.World.BlockAccessor, tmpPos);
					if (collBoxes != null && collBoxes.Length != 0 && !(block is BlockSnowLayer) && !(block is BlockSnow))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		sailLength = tree.GetInt("sailLength");
		turbulenceExposed = tree.GetBool("turbulenceExposed");
		base.FromTreeAttributes(tree, worldAccessForResolve);
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		tree.SetInt("sailLength", sailLength);
		tree.SetBool("turbulenceExposed", turbulenceExposed);
		base.ToTreeAttributes(tree);
	}

	public override void OnBlockRemoved()
	{
		base.OnBlockRemoved();
		if (Api.Side == EnumAppSide.Server)
		{
			Api.ModLoader.GetModSystem<ModSystemWindTurbulence>().RemoveTurbulence(base.Pos);
		}
	}

	public override void OnBlockUnloaded()
	{
		base.OnBlockUnloaded();
		if (Api.Side == EnumAppSide.Server)
		{
			Api.ModLoader.GetModSystem<ModSystemWindTurbulence>().RemoveTurbulence(base.Pos);
		}
	}

	protected override void updateShape(IWorldAccessor worldForResolve)
	{
		if (worldForResolve.Side == EnumAppSide.Client && base.Block != null)
		{
			if (sailLength == 0)
			{
				Shape = new CompositeShape
				{
					Base = base.Block.Shape.Base,
					rotateY = base.Block.Shape.rotateY
				};
			}
			else
			{
				AssetLocation al = AssetLocation.Create(base.Block.Attributes["sailedShapes"]["wildcardPath"].AsString(), base.Block.Code.Domain);
				al.Path = al.Path.Replace("{length}", sailLength.ToString() ?? "");
				Shape = new CompositeShape
				{
					Base = al,
					rotateY = base.Block.Shape.rotateY
				};
			}
		}
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder sb)
	{
		base.GetBlockInfo(forPlayer, sb);
		sb.AppendLine(string.Format(Lang.Get("Wind speed: {0}%", (int)(100.0 * windSpeed)), default(ReadOnlySpan<object>)));
		sb.AppendLine(Lang.Get("Sails power output: {0} kN", (int)((float)sailLength / 5f * 100f * powerMul * (turbulenceExposed ? 0.5f : 1f))));
		if (turbulenceExposed)
		{
			sb.AppendLine("Exposed to turbulence from nearby windmill, power output reduced by 50%.");
		}
	}
}
