using System;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class BlockEntityForge : BlockEntityContainer, IHeatSource, ITemperatureSensitive, IBellowsAirReceiver
{
	protected static SimpleParticleProperties smallMetalSparks;

	protected static SimpleParticleProperties smokeQuads;

	protected InventoryGeneric inv;

	protected ForgeContentsRenderer renderer;

	protected float partialFuelConsumed;

	protected bool burning;

	protected double lastTickTotalHours;

	protected ILoadedSound ambientSound;

	protected Vec3f blockRotRad = new Vec3f();

	protected bool clientSidePrevBurning;

	public float MaxExtraHeatRate = 0.6428572f;

	public float extraOxygenRate;

	public float extraOxygenRateRender;

	public float extraOxygenRateParticles;

	public BlockFacing blowDirection = BlockFacing.NORTH;

	public ItemSlot FuelSlot => inv[1];

	public ItemSlot WorkItemSlot => inv[0];

	public ItemStack WorkItemStack => inv[0].Itemstack;

	public float FuelLevel => (float)FuelSlot.StackSize - partialFuelConsumed;

	public bool IsBurning => burning;

	public bool CanIgnite
	{
		get
		{
			if (!burning)
			{
				return FuelLevel > 0f;
			}
			return false;
		}
	}

	public bool IsHot => (inv[1].Itemstack?.Collectible.GetTemperature(Api.World, inv[1].Itemstack) ?? 0f) > 20f;

	public virtual float BurnRate => 0.5f / (FuelSlot.Itemstack?.ItemAttributes?["inForge"]["durationMul"].AsFloat(1f) ?? 1f);

	public int MaxTemperature => 700 + (FuelSlot.Itemstack?.ItemAttributes?["inForge"]["tempGainDeg"].AsInt(1) ?? 1);

	public virtual float MeshAngleRad
	{
		get
		{
			return blockRotRad.Y;
		}
		set
		{
			blockRotRad.Y = value;
		}
	}

	public override InventoryBase Inventory => inv;

	public override string InventoryClassName => "forge";

	public BlockEntityForge()
	{
		inv = new InventoryGeneric(2, null, null);
	}

	public override void Initialize(ICoreAPI api)
	{
		base.Initialize(api);
		inv.LateInitialize("forge-" + Pos, api);
		if (api is ICoreClientAPI)
		{
			ICoreClientAPI capi = (ICoreClientAPI)api;
			capi.Event.RegisterRenderer(renderer = new ForgeContentsRenderer(base.Block, Pos, capi, blockRotRad), EnumRenderStage.Opaque, "forge");
			renderer.SetContents(WorkItemStack, FuelLevel, burning, regen: true, extraOxygenRateRender);
			RegisterGameTickListener(OnClientTick, 50);
			api.Event.RegisterEventBusListener(onEventBusEvent, 0.5, "genjsontransform");
		}
		RegisterGameTickListener(OnCommonTick200ms, 200);
		if (smallMetalSparks == null)
		{
			smallMetalSparks = BlockEntityCoalPile.smallMetalSparks.Clone(api.World);
			smokeQuads = BlockEntityCoalPile.smokeParticles.Clone(api.World);
			smokeQuads.LifeLength = 0.5f;
			smokeQuads.OpacityEvolve = EvolvingNatFloat.create(EnumTransformFunction.QUADRATIC, -16f);
		}
	}

	private void onEventBusEvent(string eventName, ref EnumHandling handling, IAttribute data)
	{
		renderer.RegenMesh();
	}

	public void ToggleAmbientSounds(bool on)
	{
		if (Api.Side != EnumAppSide.Client)
		{
			return;
		}
		if (on)
		{
			if (ambientSound == null || !ambientSound.IsPlaying)
			{
				ambientSound = ((IClientWorldAccessor)Api.World).LoadSound(new SoundParams
				{
					Location = new AssetLocation("sounds/effect/embers.ogg"),
					ShouldLoop = true,
					Position = Pos.ToVec3f().Add(0.5f, 0.25f, 0.5f),
					DisposeOnFinish = false,
					Volume = 1f
				});
				ambientSound.Start();
			}
		}
		else
		{
			ambientSound.Stop();
			ambientSound.Dispose();
			ambientSound = null;
		}
	}

	public void BlowAirInto(IWorldAccessor world, BlockPos pos, float amount, BlockFacing direction)
	{
		BlowAirInto(amount, direction);
	}

	public void BlowAirInto(float amount, BlockFacing direction)
	{
		if (burning)
		{
			extraOxygenRate = Math.Min(MaxExtraHeatRate, extraOxygenRate + amount);
			extraOxygenRateRender = Math.Min(1f, extraOxygenRateRender + amount);
			extraOxygenRateParticles = Math.Max(extraOxygenRateParticles, amount);
			blowDirection = direction;
		}
	}

	protected void OnClientTick(float dt)
	{
		extraOxygenRateParticles = Math.Max(0f, extraOxygenRateParticles - dt * 0.5f);
		if ((double)extraOxygenRateRender > 0.15)
		{
			Vec3f dir = blowDirection.Normalf;
			smallMetalSparks.MinPos.Set((float)Pos.X + 0.25f, (float)Pos.Y + 0.875f, (float)Pos.Z + 0.25f);
			smallMetalSparks.AddPos.Set(0.5, 0.10000000149011612, 0.5);
			smallMetalSparks.MinVelocity.Set(dir.X * 0.5f, 1f, dir.Z * 0.5f);
			smallMetalSparks.AddVelocity.Set(dir.X * 0.5f, 3f, dir.Z * 0.5f);
			smokeQuads.MinPos.Set((float)Pos.X + 0.25f, (float)Pos.Y + 0.875f, (float)Pos.Z + 0.25f);
			smokeQuads.AddPos.Set(0.5, 0.10000000149011612, 0.5);
			smokeQuads.MinVelocity.Set(-0.125f, 0.25f, -0.125f);
			smokeQuads.AddVelocity.Set(0.25f, 0.5f, 0.25f);
			smallMetalSparks.MinQuantity = (float)Math.Pow(extraOxygenRateRender, 3.0) * 250f;
			smallMetalSparks.AddQuantity = 0f;
			Api.World.SpawnParticles(smallMetalSparks);
			smokeQuads.MinQuantity = extraOxygenRateRender;
			Api.World.SpawnParticles(smokeQuads);
		}
		ICoreAPI api = Api;
		if (api != null && api.Side == EnumAppSide.Client && clientSidePrevBurning != burning)
		{
			ToggleAmbientSounds(IsBurning);
			clientSidePrevBurning = IsBurning;
		}
		if (burning && Api.World.Rand.NextDouble() < 0.13)
		{
			BlockEntityCoalPile.SpawnBurningCoalParticles(Api, Pos.ToVec3d().Add(0.25, 0.875, 0.25), 0.5f, 0.5f);
		}
		renderer?.SetContents(WorkItemSlot.Itemstack, FuelLevel, burning, regen: false, extraOxygenRateRender);
		extraOxygenRateRender *= 0.97f;
	}

	protected void OnCommonTick200ms(float dt)
	{
		if (burning)
		{
			double hoursPassed = Api.World.Calendar.TotalHours - lastTickTotalHours;
			if (hoursPassed < 0.0)
			{
				lastTickTotalHours = Api.World.Calendar.TotalHours;
				hoursPassed = 0.0;
			}
			float oxygenBurnMul = 1f + extraOxygenRate;
			partialFuelConsumed += (float)((double)BurnRate * hoursPassed * (double)oxygenBurnMul);
			updateFuelLevel();
			if (FuelLevel <= 0f)
			{
				burning = false;
				partialFuelConsumed = 0f;
			}
			if (WorkItemStack != null && Api.Side == EnumAppSide.Server)
			{
				float temp = WorkItemStack.Collectible.GetTemperature(Api.World, WorkItemStack);
				if (temp < (float)MaxTemperature * oxygenBurnMul)
				{
					float tempGain = (float)(hoursPassed * 1500.0 * (double)oxygenBurnMul);
					WorkItemStack.Collectible.SetTemperature(Api.World, WorkItemStack, Math.Min((float)MaxTemperature * oxygenBurnMul, temp + tempGain));
					MarkDirty();
				}
			}
			extraOxygenRate *= Math.Max(0f, 1f - (float)hoursPassed * 30f);
		}
		else
		{
			partialFuelConsumed = 0f;
			if (Api.Side == EnumAppSide.Server && Api.World.Rand.NextDouble() < 0.05 && WorkItemStack != null && WorkItemStack.Collectible.GetTemperature(Api.World, WorkItemStack) > 900f)
			{
				TryIgnite();
			}
			extraOxygenRate = 0f;
		}
		lastTickTotalHours = Api.World.Calendar.TotalHours;
	}

	public void TryIgnite()
	{
		if (!burning)
		{
			burning = true;
			renderer?.SetContents(WorkItemStack, FuelLevel, burning, regen: false, extraOxygenRate);
			lastTickTotalHours = Api.World.Calendar.TotalHours;
			MarkDirty();
		}
	}

	public bool OnPlayerInteract(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		ItemSlot slot = byPlayer.InventoryManager.ActiveHotbarSlot;
		if (!byPlayer.Entity.Controls.ShiftKey)
		{
			if (WorkItemStack == null)
			{
				return false;
			}
			ItemStack splitStack = WorkItemStack.Clone();
			splitStack.StackSize = 1;
			WorkItemStack.StackSize--;
			if (WorkItemStack.StackSize == 0)
			{
				WorkItemSlot.Itemstack = null;
			}
			if (byPlayer.InventoryManager.TryGiveItemstack(splitStack))
			{
				Api.ModLoader.GetModSystem<ModSystemSubTongsDurability>()?.OnItemPickedUp(byPlayer.Entity, splitStack);
			}
			else
			{
				world.SpawnItemEntity(splitStack, Pos);
			}
			Api.World.Logger.Audit("{0} Took 1x{1} from Forge at {2}.", byPlayer.PlayerName, splitStack.Collectible.Code, blockSel.Position);
			renderer?.SetContents(WorkItemStack, FuelLevel, burning, regen: true, extraOxygenRateRender);
			MarkDirty();
			Api.World.PlaySoundAt(new AssetLocation("sounds/block/ingot"), Pos, 0.4375, byPlayer);
			return true;
		}
		if (slot.Itemstack == null)
		{
			return false;
		}
		CombustibleProperties combustibleProperties = slot.Itemstack.Collectible.GetCombustibleProperties(world, slot.Itemstack, null);
		if (combustibleProperties != null && combustibleProperties.BurnTemperature > 1000)
		{
			if (FuelLevel > 4.5f)
			{
				return false;
			}
			CollectibleObject collectible = slot.Itemstack.Collectible;
			bool flag = ((collectible is ItemCoal || collectible is ItemOre) ? true : false);
			bool isFuel = flag;
			if (slot.TryPutInto(Api.World, FuelSlot) == 0)
			{
				return false;
			}
			if (isFuel)
			{
				Api.World.PlaySoundAt(new AssetLocation("sounds/block/charcoal"), byPlayer, byPlayer, randomizePitch: true, 16f);
			}
			(Api as ICoreClientAPI)?.World.Player.TriggerFpAnimation(EnumHandInteract.HeldItemInteract);
			renderer?.SetContents(WorkItemStack, FuelLevel, burning, regen: false, extraOxygenRateRender);
			MarkDirty();
			return true;
		}
		string firstCodePart = slot.Itemstack.Collectible.FirstCodePart();
		bool forgableGeneric = slot.Itemstack.Collectible.Attributes?.IsTrue("forgable") ?? false;
		if (WorkItemStack == null && (firstCodePart == "ingot" || firstCodePart == "metalplate" || firstCodePart == "workitem" || forgableGeneric))
		{
			WorkItemSlot.Itemstack = slot.TakeOut(1);
			slot.MarkDirty();
			Api.World.Logger.Audit("{0} Put 1x{1} into Forge at {2}.", byPlayer.PlayerName, WorkItemStack.Collectible.Code, blockSel.Position);
			renderer?.SetContents(WorkItemStack, FuelLevel, burning, regen: true, extraOxygenRateRender);
			MarkDirty();
			Api.World.PlaySoundAt(new AssetLocation("sounds/block/ingot"), Pos, 0.4375, byPlayer);
			return true;
		}
		if (!forgableGeneric && WorkItemStack != null && WorkItemStack.Equals(Api.World, slot.Itemstack, GlobalConstants.IgnoredStackAttributes) && WorkItemStack.StackSize < 4 && WorkItemStack.StackSize < WorkItemStack.Collectible.MaxStackSize)
		{
			float myTemp = WorkItemStack.Collectible.GetTemperature(Api.World, WorkItemStack);
			float histemp = slot.Itemstack.Collectible.GetTemperature(Api.World, slot.Itemstack);
			WorkItemStack.Collectible.SetTemperature(world, WorkItemStack, (myTemp * (float)WorkItemStack.StackSize + histemp * 1f) / (float)(WorkItemStack.StackSize + 1));
			WorkItemStack.StackSize++;
			slot.TakeOut(1);
			slot.MarkDirty();
			Api.World.Logger.Audit("{0} Put 1x{1} into Forge at {2}.", byPlayer.PlayerName, WorkItemStack.Collectible.Code, blockSel.Position);
			renderer?.SetContents(WorkItemStack, FuelLevel, burning, regen: true, extraOxygenRateRender);
			Api.World.PlaySoundAt(new AssetLocation("sounds/block/ingot"), Pos, 0.4375, byPlayer);
			MarkDirty();
			return true;
		}
		return false;
	}

	public override void OnBlockRemoved()
	{
		base.OnBlockRemoved();
		renderer?.Dispose();
		renderer = null;
		ambientSound?.Dispose();
		(Api as ICoreClientAPI)?.Event.UnregisterEventBusListener(onEventBusEvent);
	}

	public override void OnBlockUnloaded()
	{
		base.OnBlockUnloaded();
		renderer?.Dispose();
		(Api as ICoreClientAPI)?.Event.UnregisterEventBusListener(onEventBusEvent);
	}

	public override void OnBlockBroken(IPlayer byPlayer = null)
	{
		if (!burning && !FuelSlot.Empty)
		{
			Api.World.SpawnItemEntity(FuelSlot.Itemstack, Pos);
		}
		if (WorkItemStack != null)
		{
			Api.World.SpawnItemEntity(WorkItemStack, Pos);
		}
		Inventory.Clear();
		base.OnBlockBroken(byPlayer);
		ambientSound?.Dispose();
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
	{
		ItemStack prevStack = WorkItemStack?.Clone();
		base.FromTreeAttributes(tree, worldForResolving);
		ItemStack contents = tree.GetItemstack("contents");
		if (contents != null && WorkItemStack == null)
		{
			WorkItemSlot.Itemstack = contents;
			if (Api != null)
			{
				contents?.ResolveBlockOrItem(Api.World);
			}
		}
		partialFuelConsumed = tree.GetFloat("partialFuelConsumed");
		burning = tree.GetInt("burning") > 0;
		lastTickTotalHours = tree.GetDouble("lastTickTotalHours");
		MeshAngleRad = tree.GetFloat("meshAngle", MeshAngleRad);
		bool remesh = ((prevStack == null) ^ (WorkItemStack == null)) || (prevStack != null && WorkItemStack != null && (prevStack.StackSize != WorkItemStack.StackSize || !prevStack.Equals(Api.World, WorkItemStack, GlobalConstants.IgnoredStackAttributes)));
		renderer?.SetContents(WorkItemStack, FuelLevel, burning, remesh, extraOxygenRateRender);
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		tree.SetFloat("partialFuelConsumed", partialFuelConsumed);
		tree.SetInt("burning", burning ? 1 : 0);
		tree.SetDouble("lastTickTotalHours", lastTickTotalHours);
		tree.SetFloat("meshAngle", MeshAngleRad);
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
		if (!WorkItemSlot.Empty)
		{
			int temp = (int)WorkItemStack.Collectible.GetTemperature(Api.World, WorkItemStack);
			if (temp <= 25)
			{
				dsc.AppendLine(Lang.Get("forge-contentsandtemp-cold", WorkItemStack.StackSize, WorkItemStack.GetName()));
			}
			else
			{
				dsc.AppendLine(Lang.Get("forge-contentsandtemp", WorkItemStack.StackSize, WorkItemStack.GetName(), temp));
			}
		}
		if (!FuelSlot.Empty)
		{
			float oxygenBurnMul = 1f + extraOxygenRate;
			dsc.AppendLine(Lang.Get("forge-fuel", FuelSlot.Itemstack.GetName()));
			dsc.AppendLine(Lang.Get("forge-fuel-for-hour-amount", FuelLevel / oxygenBurnMul / BurnRate));
		}
	}

	public float GetHeatStrength(IWorldAccessor world, BlockPos heatSourcePos, BlockPos heatReceiverPos)
	{
		return IsBurning ? 7 : 0;
	}

	public void CoolNow(float amountRel, OnStackToCool onStackToCoolCallback)
	{
		bool playsound = false;
		if (burning)
		{
			playsound = true;
			partialFuelConsumed += amountRel / 250f;
			updateFuelLevel();
			if (Api.World.Rand.NextDouble() < (double)(amountRel / 30f) || FuelLevel <= 0f)
			{
				burning = false;
			}
			MarkDirty(redrawOnClient: true);
		}
		float temp = ((WorkItemStack == null) ? 0f : WorkItemStack.Collectible.GetTemperature(Api.World, WorkItemStack));
		if (temp > 20f)
		{
			playsound = temp > 100f;
			onStackToCoolCallback(WorkItemSlot, Pos.ToVec3d(), GlobalConstants.CollectibleDefaultTemperature, playsound);
			MarkDirty(redrawOnClient: true);
		}
		if (playsound)
		{
			Api.World.PlaySoundAt(new AssetLocation("sounds/effect/extinguish"), Pos, 0.25, null, randomizePitch: false, 16f);
		}
	}

	private void updateFuelLevel()
	{
		if (partialFuelConsumed > 1f)
		{
			FuelSlot.Itemstack.StackSize = Math.Max(0, FuelSlot.StackSize - (int)partialFuelConsumed);
			partialFuelConsumed %= 1f;
			if (FuelSlot.Itemstack.StackSize <= 0)
			{
				FuelSlot.Itemstack = null;
			}
		}
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
	{
		MeshData mesh = (Api as ICoreClientAPI).TesselatorManager.GetDefaultBlockMesh(base.Block);
		float[] rotMatrix = new Matrixf().Translate(0.5f, 0f, 0.5f).RotateY(MeshAngleRad).Translate(-0.5f, 0f, -0.5f)
			.Values;
		mesher.AddMeshData(mesh, rotMatrix);
		return true;
	}
}
