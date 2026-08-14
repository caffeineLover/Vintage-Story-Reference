using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent.Mechanics;

namespace Vintagestory.GameContent;

public class BlockEntityGrindingWheel : BlockEntity, IMechanicalPowerRenderable
{
	protected CompositeShape shape;

	protected MechanicalPowerMod manager;

	protected ICoreClientAPI capi;

	protected ILoadedSound grindSound;

	protected ILoadedSound turnSound;

	public Dictionary<string, float> PlayersGrinding = new Dictionary<string, float>();

	public bool Grinding;

	private long sparkTickListener;

	public BlockFacing Facing => BlockFacing.FromCode(base.Block.Variant["side"]);

	public virtual BlockPos Position => Pos;

	public virtual Vec4f LightRgba { get; set; } = new Vec4f();

	public virtual int[] AxisSign { get; set; }

	public virtual float AngleRad => GetBehavior<BEBehaviorMPConsumer>().AngleRad;

	public virtual CompositeShape Shape
	{
		get
		{
			return shape;
		}
		set
		{
			if (Shape != null && manager != null)
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

	private float turnSpeed => GetBehavior<BEBehaviorMPConsumer>().TrueSpeed;

	protected MeshData StandMesh => ObjectCacheUtil.GetOrCreate(capi, "grindingwheel-standmesh" + Facing.Code, () => getMesh(new string[2] { "stand1/*", "stand2/*" }));

	public override void Initialize(ICoreAPI api)
	{
		base.Initialize(api);
		Shape = base.Block.Shape;
		manager = Api.ModLoader.GetModSystem<MechanicalPowerMod>();
		manager.AddDeviceForRender(this);
		AxisSign = new int[3];
		switch (Facing.Index)
		{
		case 0:
			AxisSign[0] = -1;
			break;
		case 1:
			AxisSign[2] = -1;
			break;
		case 2:
			AxisSign[0] = -1;
			break;
		default:
			AxisSign[2] = -1;
			break;
		}
		capi = api as ICoreClientAPI;
		RegisterGameTickListener(checkIsGrinding, 100);
	}

	private void checkIsGrinding(float dt)
	{
		if (Api.Side == EnumAppSide.Server)
		{
			if (Grinding && PlayersGrinding.Count == 0)
			{
				Grinding = false;
				MarkDirty(redrawOnClient: true);
			}
			if (!Grinding && PlayersGrinding.Count > 0)
			{
				Grinding = true;
				MarkDirty(redrawOnClient: true);
			}
			if (Api.World.Rand.NextDouble() > 0.05 * (double)turnSpeed)
			{
				return;
			}
			{
				foreach (string uid in new List<string>(PlayersGrinding.Keys))
				{
					IPlayer plr = Api.World.PlayerByUid(uid);
					if (plr == null)
					{
						PlayersGrinding.Remove(uid);
						continue;
					}
					if (plr.CurrentBlockSelection == null || plr.CurrentBlockSelection.Position != Pos)
					{
						PlayersGrinding.Remove(uid);
						continue;
					}
					ItemSlot slot = plr.InventoryManager.ActiveHotbarSlot;
					if (slot.Empty)
					{
						PlayersGrinding.Remove(uid);
						continue;
					}
					float maxdura = slot.Itemstack.Collectible.GetMaxDurability(slot.Itemstack);
					if (slot != null && slot.Itemstack.Collectible.HasBehavior<CollectibleBehaviorSharpenable>())
					{
						slot.Itemstack.Collectible.DamageItem(Api.World, plr.Entity, slot, (int)Math.Max(1f, 0.003f * maxdura));
					}
					PlayersGrinding[uid] += dt;
					CollectibleBehaviorBuffable bhb = slot.Itemstack.Collectible.GetBehavior<CollectibleBehaviorBuffable>();
					AppliedCollectibleBuff curBuff = bhb.GetItemBuffs(slot.Itemstack).FirstOrDefault((AppliedCollectibleBuff buff) => buff.Code == "sharpened");
					if (curBuff == null || (double)curBuff.Multiplier < 1.099)
					{
						bhb.AddBuff(slot.Itemstack, new AppliedCollectibleBuff
						{
							Code = "sharpened",
							RemainingDurability = ((curBuff == null) ? ((int)(maxdura / 4f)) : 0),
							Multiplier = 1.01f,
							StatCode = "critchance"
						}, EnumBuffAddType.AddOnDuplicate);
					}
				}
				return;
			}
		}
		if (turnSound == null && (double)turnSpeed > 0.05)
		{
			turnSound = capi.World.LoadSound(new SoundParams
			{
				Location = new AssetLocation("sounds/block/grindingwheel.ogg"),
				ShouldLoop = true,
				DisposeOnFinish = false,
				Position = Pos.ToVec3f(),
				RelativePosition = false,
				Range = 16f,
				SoundType = EnumSoundType.Sound,
				Volume = 1f
			});
			turnSound.Start();
		}
		if ((double)Math.Abs(turnSpeed) > 0.05)
		{
			if (!turnSound.IsPlaying)
			{
				turnSound.Start();
			}
			turnSound.SetPitch(Math.Abs(turnSpeed));
		}
		else if (turnSound != null && turnSound.IsPlaying)
		{
			turnSound.Stop();
		}
		if (grindSound == null)
		{
			return;
		}
		if (Grinding)
		{
			grindSound.SetPitch(Math.Abs(turnSpeed));
			if (sparkTickListener == 0L)
			{
				grindSound.Start();
				grindSound.FadeIn(0.32f, null);
				sparkTickListener = RegisterGameTickListener(onParticleTick, 20);
			}
		}
		else if (sparkTickListener != 0L)
		{
			grindSound.FadeOut(0.32f, delegate
			{
				grindSound.Stop();
			});
			UnregisterGameTickListener(sparkTickListener);
			sparkTickListener = 0L;
		}
	}

	private void onParticleTick(float dt)
	{
		Vec3f facing = BlockFacing.FromCode(base.Block.Variant["side"]).Normalf.Clone();
		_ = AxisSign[0];
		_ = 0;
		double swipe = Math.Sin((double)Api.World.ElapsedMilliseconds / 300.0) / 12.0;
		float ws = (0f - turnSpeed) / 2f;
		SimpleParticleProperties props = new SimpleParticleProperties
		{
			MinQuantity = 1f,
			AddQuantity = 3f,
			VertexFlags = 255,
			Color = ColorUtil.ColorFromRgba(0, 255, 255, 255),
			LifeLength = 0.05f,
			MinSize = 0.02f,
			MaxSize = 0.05f,
			ParticleModel = EnumParticleModel.Quad,
			MinVelocity = new Vec3f(-0.25f - facing.X * 2f * ws, -0.25f, -0.25f - facing.Z * 2f * ws),
			AddVelocity = new Vec3f(0.5f - facing.X * 4f * ws, -0.5f, 0.5f - facing.Z * 4f * ws)
		};
		props.MinPos = Pos.ToVec3d().Add(0.5, 0.9, 0.5);
		props.AddPos = new Vec3d((double)facing.Z * swipe, 0.0, (double)facing.X * swipe);
		Api.World.SpawnParticles(props);
	}

	protected MeshData getMesh(string[] selEles)
	{
		Shape shape = Vintagestory.API.Common.Shape.TryGet(capi, "shapes/block/wood/mechanics/grindingwheel.json");
		float rotateY = 0f;
		switch (Facing.Index)
		{
		case 0:
			rotateY = 180f;
			break;
		case 1:
			rotateY = 90f;
			break;
		case 3:
			rotateY = 270f;
			break;
		}
		capi.Tesselator.TesselateShape(base.Block, shape, out var mesh, new Vec3f(0f, rotateY, 0f), null, selEles);
		return mesh;
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
	{
		LightRgba = Api.World.BlockAccessor.GetLightRGBs(Pos);
		mesher.AddMeshData(StandMesh);
		return true;
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		base.FromTreeAttributes(tree, worldAccessForResolve);
		if (worldAccessForResolve.Side == EnumAppSide.Client)
		{
			Grinding = tree.GetBool("grinding");
		}
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		tree.SetBool("grinding", Grinding);
	}

	public bool OnInteractStart(IPlayer byPlayer, BlockSelection blockSel)
	{
		if (!canBuff(byPlayer.InventoryManager.ActiveHotbarSlot))
		{
			return false;
		}
		if ((double)Math.Abs(turnSpeed) < 0.1)
		{
			return false;
		}
		PlayersGrinding[byPlayer.PlayerUID] = 0f;
		if (grindSound == null)
		{
			loadGrindSound();
		}
		return true;
	}

	private bool canBuff(ItemSlot slot)
	{
		if (slot.Empty)
		{
			return false;
		}
		CollectibleBehaviorBuffable bhb = slot.Itemstack.Collectible.GetBehavior<CollectibleBehaviorBuffable>();
		if (bhb == null)
		{
			return false;
		}
		AppliedCollectibleBuff curBuff = bhb.GetItemBuffs(slot.Itemstack).FirstOrDefault((AppliedCollectibleBuff buff) => buff.Code == "sharpened");
		if (curBuff != null)
		{
			return (double)curBuff.Multiplier < 1.099;
		}
		return true;
	}

	public bool OnInteractStep(float secondsUsed, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (!canBuff(byPlayer.InventoryManager.ActiveHotbarSlot))
		{
			return false;
		}
		return true;
	}

	public override void OnBlockUnloaded()
	{
		base.OnBlockUnloaded();
		grindSound?.Dispose();
		turnSound?.Dispose();
	}

	public override void OnBlockRemoved()
	{
		base.OnBlockRemoved();
		grindSound?.Dispose();
		turnSound?.Dispose();
	}

	private void loadGrindSound()
	{
		if (capi != null)
		{
			grindSound = capi.World.LoadSound(new SoundParams
			{
				Location = new AssetLocation("sounds/effect/metalgrinding.ogg"),
				ShouldLoop = true,
				DisposeOnFinish = false,
				Position = Pos.ToVec3f(),
				RelativePosition = false,
				Range = 16f,
				SoundType = EnumSoundType.Sound,
				Volume = 1f
			});
		}
	}

	public void OnInteractStop(float secondsUsed, IPlayer byPlayer, BlockSelection blockSel)
	{
		PlayersGrinding.Remove(byPlayer.PlayerUID);
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
		if (turnSpeed > 0f && (double)turnSpeed < 0.1)
		{
			dsc.AppendLine(Lang.Get("Turns too slowly to grind"));
		}
		base.GetBlockInfo(forPlayer, dsc);
	}
}
