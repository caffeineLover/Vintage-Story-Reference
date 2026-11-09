using System;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent.Mechanics;

public class BEBehaviorMPCreativeRotor : BEBehaviorMPRotor
{
	private int powerSetting;

	private int speedSetting;

	protected override AssetLocation Sound => null;

	protected override float Resistance => 0.3f;

	protected override double AccelerationFactor => 1.0;

	protected override float TargetSpeed => 0.1f * (float)speedSetting;

	protected override float TorqueFactor => 0.5f * (float)powerSetting;

	public BEBehaviorMPCreativeRotor(BlockEntity blockentity)
		: base(blockentity)
	{
		powerSetting = 3;
		speedSetting = 3;
	}

	public override void Initialize(ICoreAPI api, JsonObject properties)
	{
		base.Initialize(api, properties);
	}

	protected override CompositeShape GetShape()
	{
		CompositeShape compositeShape = base.Block.Shape.Clone();
		compositeShape.Base = new AssetLocation("shapes/block/metal/mechanics/creativerotor-spinbar.json");
		return compositeShape;
	}

	internal bool OnInteract(IPlayer byPlayer)
	{
		int dir = ((!byPlayer.Entity.Controls.CtrlKey) ? 1 : (-1));
		if (byPlayer.Entity.Controls.ShiftKey)
		{
			speedSetting = GameMath.Mod(speedSetting + dir, 11);
		}
		else
		{
			powerSetting = GameMath.Mod(powerSetting + dir, 10);
		}
		Blockentity.MarkDirty(redrawOnClient: true);
		Api.World.PlaySoundAt(new AssetLocation("sounds/toggleswitch"), Blockentity.Pos, -0.2, byPlayer, randomizePitch: false, 16f);
		return true;
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
	{
		base.OnTesselation(mesher, tesselator);
		ICoreClientAPI capi = Api as ICoreClientAPI;
		Shape shape = Vintagestory.API.Common.Shape.TryGet(capi, "shapes/block/metal/mechanics/creativerotor-frame.json");
		float rotateY = 0f;
		switch (BlockFacing.FromCode(base.Block.Variant["side"]).Index)
		{
		case 0:
			AxisSign = new int[3] { 0, 0, 1 };
			rotateY = 180f;
			break;
		case 1:
			AxisSign = new int[3] { -1, 0, 0 };
			rotateY = 90f;
			break;
		case 3:
			AxisSign = new int[3] { 1, 0, 0 };
			rotateY = 270f;
			break;
		}
		capi.Tesselator.TesselateShape(base.Block, shape, out var mesh, new Vec3f(0f, rotateY, 0f));
		mesher.AddMeshData(mesh);
		return true;
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor world)
	{
		powerSetting = tree.GetInt("p");
		speedSetting = tree.GetInt("s");
		if (powerSetting > 10 || powerSetting < 1)
		{
			powerSetting = 3;
		}
		base.FromTreeAttributes(tree, world);
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		tree.SetInt("p", powerSetting);
		tree.SetInt("s", speedSetting);
		base.ToTreeAttributes(tree);
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder sb)
	{
		base.GetBlockInfo(forPlayer, sb);
		sb.AppendLine(string.Format(Lang.Get("mechanical-torque-kn", (int)(TorqueFactor * 100f)), default(ReadOnlySpan<object>)));
		sb.AppendLine(string.Format(Lang.Get("mechanical-speed-rps", Math.Round((float)speedSetting * 0.1f, 2)), default(ReadOnlySpan<object>)));
	}
}
