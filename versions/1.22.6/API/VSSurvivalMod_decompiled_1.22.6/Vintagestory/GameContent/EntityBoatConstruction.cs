using System;
using System.IO;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class EntityBoatConstruction : Entity
{
	protected Vec3f launchStartPos = new Vec3f();

	protected RightClickConstruction rcc;

	private EntityAgent launchingEntity;

	public override double FrustumSphereRadius => base.FrustumSphereRadius * 2.0;

	private int CurrentStage
	{
		get
		{
			return WatchedAttributes.GetInt("currentStage");
		}
		set
		{
			WatchedAttributes.SetInt("currentStage", value);
		}
	}

	public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
	{
		requirePosesOnServer = true;
		WatchedAttributes.RegisterModifiedListener("currentStage", stagedChanged);
		WatchedAttributes.RegisterModifiedListener("wildcards", delegate
		{
			rcc.FromTreeAttributes(WatchedAttributes);
		});
		base.Initialize(properties, api, InChunkIndex3d);
		ConstructionStage[] stages = properties.Attributes["stages"].AsArray<ConstructionStage>();
		rcc.LateInit(stages, Api, () => base.Pos.XYZ, "entity " + Code);
		CurrentStage = rcc.CurrentCompletedStage;
	}

	private void stagedChanged()
	{
		if (World.Side != EnumAppSide.Server)
		{
			rcc.CurrentCompletedStage = CurrentStage;
			MarkShapeModified();
		}
	}

	public override void OnTesselation(ref Shape entityShape, string shapePathForLogging)
	{
		if (base.Properties.Client.Renderer is EntityShapeRenderer esr)
		{
			esr.OverrideSelectiveElements = rcc.getShapeElements();
		}
		if (Api is ICoreClientAPI && rcc.StoredWildCards.TryGetValue("wood", out var wood))
		{
			setTexture("debarked", new AssetLocation($"block/wood/debarked/{wood}"));
			setTexture("planks", new AssetLocation($"block/wood/planks/{wood}1"));
		}
		base.OnTesselation(ref entityShape, shapePathForLogging);
	}

	private void setTexture(string code, AssetLocation assetLocation)
	{
		ICoreClientAPI obj = Api as ICoreClientAPI;
		CompositeTexture compositeTexture = (base.Properties.Client.Textures[code] = new CompositeTexture(assetLocation));
		CompositeTexture ctex = compositeTexture;
		obj.EntityTextureAtlas.GetOrInsertTexture(ctex, out var tui, out var _);
		ctex.Baked.TextureSubId = tui;
	}

	public EntityBoatConstruction()
	{
		rcc = new RightClickConstruction();
	}

	public override void OnInteract(EntityAgent byEntity, ItemSlot handslot, Vec3d hitPosition, EnumInteractMode mode)
	{
		base.OnInteract(byEntity, handslot, hitPosition, mode);
		if (CurrentStage != rcc.CurrentCompletedStage)
		{
			CurrentStage = rcc.CurrentCompletedStage;
		}
		if (CurrentStage >= rcc.Stages.Length - 1)
		{
			return;
		}
		if (CurrentStage == 0 && handslot.Empty && byEntity.Controls.ShiftKey)
		{
			byEntity.TryGiveItemStack(new ItemStack(Api.World.GetItem(new AssetLocation("roller")), 5));
			Die();
			return;
		}
		if (rcc.OnInteract(byEntity, handslot))
		{
			CurrentStage = rcc.CurrentCompletedStage;
			rcc.ToTreeAttributes(WatchedAttributes);
			WatchedAttributes.MarkPathDirty("wildcards");
			WatchedAttributes.MarkPathDirty("currentStage");
		}
		if (CurrentStage >= rcc.Stages.Length - 2 && !AnimManager.IsAnimationActive("launch"))
		{
			launchingEntity = byEntity;
			launchStartPos = getCenterPos();
			StartAnimation("launch");
		}
	}

	private Vec3f getCenterPos()
	{
		AttachmentPointAndPose apap = AnimManager.Animator?.GetAttachmentPointPose("Center");
		if (apap != null)
		{
			Matrixf mat = new Matrixf();
			mat.RotateY(base.Pos.Yaw + (float)Math.PI / 2f);
			apap.Mul(mat);
			return mat.TransformVector(new Vec4f(0f, 0f, 0f, 1f)).XYZ;
		}
		return null;
	}

	public override void OnGameTick(float dt)
	{
		base.OnGameTick(dt);
		if ((double)(AnimManager.Animator?.GetAnimationState("launch").AnimProgress ?? 0f) >= 0.99)
		{
			AnimManager.StopAnimation("launch");
			CurrentStage = 0;
			rcc.CurrentCompletedStage = 0;
			MarkShapeModified();
			if (World.Side == EnumAppSide.Server)
			{
				Spawn();
			}
		}
	}

	private void Spawn()
	{
		if (rcc.StoredWildCards.TryGetValue("wood", out var wood))
		{
			Vec3f nowOff = getCenterPos();
			Vec3f offset = ((nowOff == null) ? new Vec3f() : (nowOff - launchStartPos));
			EntityProperties type = World.GetEntityType(new AssetLocation("boat-sailed-" + wood));
			Entity entity = World.ClassRegistry.CreateEntity(type);
			if ((int)Math.Abs(base.Pos.Yaw * (180f / (float)Math.PI)) == 90 || (int)Math.Abs(base.Pos.Yaw * (180f / (float)Math.PI)) == 270)
			{
				offset.X *= 1.1f;
			}
			offset.Y = 0.5f;
			entity.Pos.SetFrom(base.Pos).Add(offset);
			entity.Pos.Motion.Add((double)offset.X / 50.0, 0.0, (double)offset.Z / 50.0);
			IPlayer plr = (launchingEntity as EntityPlayer)?.Player;
			if (plr != null)
			{
				entity.WatchedAttributes.SetString("createdByPlayername", plr.PlayerName);
				entity.WatchedAttributes.SetString("createdByPlayerUID", plr.PlayerUID);
			}
			World.SpawnEntity(entity);
		}
	}

	public override WorldInteraction[] GetInteractionHelp(IClientWorldAccessor world, EntitySelection es, IClientPlayer player)
	{
		WorldInteraction[] wis = base.GetInteractionHelp(world, es, player);
		WorldInteraction[] ncwis = rcc.GetInteractionHelp(world, player);
		if (ncwis == null)
		{
			return wis;
		}
		wis = wis.Append(ncwis);
		if (CurrentStage == 0)
		{
			wis = wis.Append(new WorldInteraction
			{
				HotKeyCode = "shift",
				RequireFreeHand = true,
				MouseButton = EnumMouseButton.Right,
				ActionLangCode = "rollers-deconstruct"
			});
		}
		return wis;
	}

	public override void ToBytes(BinaryWriter writer, bool forClient)
	{
		rcc.ToTreeAttributes(WatchedAttributes);
		base.ToBytes(writer, forClient);
	}

	public override void FromBytes(BinaryReader reader, bool isSync)
	{
		base.FromBytes(reader, isSync);
		rcc.FromTreeAttributes(WatchedAttributes);
	}

	public override string GetInfoText()
	{
		string info = base.GetInfoText() + "\r\n";
		if (rcc.StoredWildCards.TryGetValue("wood", out var wood))
		{
			info = info + Lang.Get("Material: {0}", Lang.Get("material-" + wood)) + "\r\n";
		}
		return info + Lang.Get("build-stage-current-and-total", CurrentStage + 1, rcc.Stages.Length - 1);
	}
}
