using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BEBehaviorFruitingBush : BlockEntityBehavior, IAnimalFoodSource, IPointOfInterest, ILongInteractable, IInteractable, IHarvestable
{
	protected static readonly float[] NoNutrients = new float[3];

	protected ICoreClientAPI capi;

	protected BlockPos soilPos;

	protected BlockBehaviorFruitingBush bhBush;

	public FruitingBushState BState;

	protected double yearsBetweenCuttings = 1.0;

	private float[] dropRates = new float[4] { 0f, 0.5f, 1f, 1.5f };

	protected BlockEntitySoilNutrition BESoil => Api.World.BlockAccessor.GetBlockEntity<BlockEntitySoilNutrition>(soilPos);

	protected float[] npkNutrients => BESoil?.Nutrients ?? NoNutrients;

	public Vec3d Position => base.Pos.ToVec3d().Add(0.5, 0.5, 0.5);

	public string Type => "food";

	public AssetLocation HarvestableSound => bhBush.HarvestingSound;

	public BEBehaviorFruitingBush(BlockEntity blockentity)
		: base(blockentity)
	{
		BState = new FruitingBushState();
	}

	public override void Initialize(ICoreAPI api, JsonObject properties)
	{
		base.Initialize(api, properties);
		soilPos = Blockentity.Pos.DownCopy();
		capi = api as ICoreClientAPI;
		bhBush = base.Block.GetBehavior<BlockBehaviorFruitingBush>();
		if (!(api is ICoreServerAPI))
		{
			return;
		}
		api.ModLoader.GetModSystem<POIRegistry>().AddPOI(this);
		if (base.Block.Variant["state"] == "grown")
		{
			Block belowBlock = api.World.BlockAccessor.GetBlock(base.Pos.DownCopy());
			BlockEntity be = Api.World.BlockAccessor.GetBlockEntity(base.Pos.DownCopy());
			if (be == null && belowBlock.Fertility > 0)
			{
				Blockentity.RegisterDelayedCallback(spawnBe, 500 + api.World.Rand.Next(500));
			}
			else if (!(be is BlockEntitySoilNutrition))
			{
				Api.World.BlockAccessor.SetBlock(0, base.Pos);
			}
		}
	}

	private void spawnBe(float dt)
	{
		BlockPos below = base.Pos.DownCopy();
		Block belowBlock = Api.World.BlockAccessor.GetBlock(below);
		if (Api.World.BlockAccessor.GetBlockEntity(below) == null && belowBlock.Fertility > 0)
		{
			Api.World.BlockAccessor.SpawnBlockEntity("BerryBushFarmland", below);
			if (Api.World.BlockAccessor.GetBlockEntity(below) is BlockEntitySoilNutrition besn)
			{
				besn.OnCreatedFromSoil(belowBlock);
			}
		}
	}

	public void OnGrownFromCutting(string traits)
	{
		BState.WildBushState = null;
		if (traits != null)
		{
			BState.Traits = traits.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		}
		BState.MatureTotalDays = BState.PlantedTotalDays + (double)(bhBush.growthStageMonths[0].nextFloat() * (float)Api.World.Calendar.DaysPerMonth);
		BState.Growthstate = EnumFruitingBushGrowthState.Young;
	}

	public FarmlandFastForwardUpdate onUpdate()
	{
		if (GetHealthState() != BState.PrevHealthState)
		{
			BState.MeshDirty = true;
			Blockentity.MarkDirty(redrawOnClient: true);
		}
		bool isYoung = BState.Growthstate == EnumFruitingBushGrowthState.Young;
		if (BESoil == null && Api.World.Calendar.TotalDays >= BState.MatureTotalDays && isYoung)
		{
			isYoung = false;
			BState.Growthstate = EnumFruitingBushGrowthState.Mature;
			Blockentity.MarkDirty(redrawOnClient: true);
		}
		BState.PrevHealthState = GetHealthState();
		return delegate(double hourIntervall, ClimateCondition conds, double lightGrowthSpeedFactor, bool _)
		{
			BState.TransitionHoursLeft -= hourIntervall * lightGrowthSpeedFactor;
			if (BState.Growthstate == EnumFruitingBushGrowthState.Dormant)
			{
				if (conds.Temperature > bhBush.LeaveDormantAboveTemperature)
				{
					setGrowthState(EnumFruitingBushGrowthState.Mature);
				}
			}
			else if (conds.Temperature < bhBush.GoDormantBelowTemperature && !isYoung)
			{
				setGrowthState(EnumFruitingBushGrowthState.Dormant);
			}
			else
			{
				if ((conds.Temperature < bhBush.ResetGrowthBelowTemperature || conds.Temperature > bhBush.ResetGrowthAboveTemperature) && !isYoung)
				{
					EnumFruitingBushGrowthState growthstate = BState.Growthstate;
					if ((uint)(growthstate - 2) <= 2u)
					{
						setGrowthState(EnumFruitingBushGrowthState.Mature);
						return;
					}
				}
				if (conds.Temperature < bhBush.PauseGrowthBelowTemperature || conds.Temperature > bhBush.PauseGrowthAboveTemperature)
				{
					if (BState.Growthstate != EnumFruitingBushGrowthState.Ripe)
					{
						BState.TransitionHoursLeft += hourIntervall * lightGrowthSpeedFactor;
					}
					if (isYoung)
					{
						BState.MatureTotalDays += hourIntervall * lightGrowthSpeedFactor / (double)Api.World.Calendar.HoursPerDay;
					}
					Blockentity.MarkDirty(redrawOnClient: true);
				}
				else if (isYoung && BESoil != null)
				{
					if (BESoil.TotalHoursLastUpdate >= BState.MatureTotalDays * (double)Api.World.Calendar.HoursPerDay)
					{
						isYoung = false;
						setGrowthState(EnumFruitingBushGrowthState.Mature);
					}
					else
					{
						BState.MatureTotalDays += hourIntervall * (1.0 - lightGrowthSpeedFactor) / (double)Api.World.Calendar.HoursPerDay;
					}
				}
				else if (BState.TransitionHoursLeft <= 0.0)
				{
					setGrowthState((EnumFruitingBushGrowthState)(1 + GameMath.Mod((int)BState.Growthstate, 4)));
					if (BState.Growthstate == EnumFruitingBushGrowthState.Ripe)
					{
						consumeNutrients();
					}
				}
				else
				{
					Blockentity.MarkDirty(redrawOnClient: true);
				}
			}
		};
	}

	protected void consumeNutrients()
	{
		if (!BState.WildBushState.HasValue)
		{
			EnumFruitingBushHealthState hs = GetHealthState();
			NpkNutrients amount = bhBush.nutrientUseByHealthState[hs.ToString().ToLowerInvariant()].Clone();
			amount *= getNutrientUseMul();
			float yearsAlive = (float)(Api.World.Calendar.TotalDays - BState.MatureTotalDays) / (float)Api.World.Calendar.DaysPerYear;
			amount *= (float)Math.Pow(0.8500000238418579, yearsAlive);
			if (BESoil == null)
			{
				BState.WildBushState = ((!(Api.World.Rand.NextDouble() < 0.5)) ? EnumFruitingBushHealthState.Struggling : EnumFruitingBushHealthState.Healthy);
				return;
			}
			BESoil.ConsumeNutrients(EnumSoilNutrient.N, amount.N);
			BESoil.ConsumeNutrients(EnumSoilNutrient.P, amount.P);
			BESoil.ConsumeNutrients(EnumSoilNutrient.K, amount.K);
		}
	}

	protected void setGrowthState(EnumFruitingBushGrowthState state)
	{
		BState.Growthstate = state;
		BState.TransitionHoursLeft = GetHoursForNextStage();
		Blockentity.MarkDirty(redrawOnClient: true);
	}

	public virtual double GetHoursForNextStage()
	{
		if (BState.Growthstate == EnumFruitingBushGrowthState.Dormant)
		{
			return 0.0;
		}
		NatFloat nf = bhBush.growthStageMonths[(int)BState.Growthstate];
		return ((BState.Growthstate == EnumFruitingBushGrowthState.Ripe) ? getRipeTimeMul() : (1f / bhBush.GrowthRateMul)) * nf.nextFloat() * (float)Api.World.Calendar.DaysPerMonth * Api.World.Calendar.HoursPerDay;
	}

	public override void OnBlockPlaced(ItemStack byItemStack = null)
	{
		BState.PlantedTotalDays = Api.World.Calendar.TotalDays;
		BState.MatureTotalDays = BState.PlantedTotalDays;
		if (byItemStack?.Block.Variant["state"] == "wild" || base.Block.Variant["state"] == "wild")
		{
			BState.WildBushState = ((!(Api.World.Rand.NextDouble() < 0.5)) ? EnumFruitingBushHealthState.Struggling : EnumFruitingBushHealthState.Healthy);
			BState.Growthstate = (EnumFruitingBushGrowthState)(1 + GameMath.MurmurHash3Mod(base.Pos.X, base.Pos.Y + 1, base.Pos.Z, 4));
			genTraits(Api.World);
		}
		else if (byItemStack != null)
		{
			BState.Growthstate = EnumFruitingBushGrowthState.Mature;
			BState.Traits = Array.Empty<string>();
		}
		BState.TransitionHoursLeft = GetHoursForNextStage();
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
		EnumFruitingBushHealthState healthState = GetHealthState();
		double days = BState.TransitionHoursLeft / (double)Api.World.Calendar.HoursPerDay;
		switch (BState.Growthstate)
		{
		case EnumFruitingBushGrowthState.Ripe:
			dsc.AppendLine(Lang.Get("Fresh fruit for about {0:0.#} days.", days));
			break;
		case EnumFruitingBushGrowthState.Ripening:
			dsc.AppendLine(Lang.Get("Ripe in about {0:0.#} days, weather permitting.", days));
			break;
		case EnumFruitingBushGrowthState.Flowering:
			dsc.AppendLine(Lang.Get("Flowering for about {0:0.#} days, weather permitting.", days));
			break;
		case EnumFruitingBushGrowthState.Young:
			dsc.AppendLine(Lang.Get("fruitingplant-maturesindays", (int)Math.Ceiling(BState.MatureTotalDays - Api.World.Calendar.TotalDays)));
			break;
		}
		dsc.AppendLine(Lang.Get("healthstate-label", Lang.Get("healthstate-" + healthState.ToString().ToLowerInvariant())));
		dsc.AppendLine(Lang.Get("growthstate-label", Lang.Get("growthstate-" + BState.Growthstate.ToString().ToLowerInvariant())));
		Api.World.BlockAccessor.GetBlockEntity<BlockEntitySoilNutrition>(base.Pos.DownCopy())?.GetBlockInfo(forPlayer, dsc);
		addTraits(dsc, BState.Traits);
		double yearsPassed = (Api.World.Calendar.TotalDays - BState.LastCuttingTakenTotalDays) / (double)Api.World.Calendar.DaysPerYear;
		if (yearsPassed < yearsBetweenCuttings)
		{
			dsc.AppendLine(Lang.Get("cutting-canbetaken-in-months", Math.Ceiling((yearsBetweenCuttings - yearsPassed) * 12.0)));
		}
	}

	public static void addTraits(StringBuilder dsc, string[] traits)
	{
		if (traits.Length != 0)
		{
			dsc.AppendLine();
		}
		int i = 0;
		foreach (string val in traits)
		{
			if (i++ > 0)
			{
				dsc.Append(", ");
			}
			dsc.Append(Lang.Get("{0}", Lang.Get("trait-" + val.ToLowerInvariant())));
		}
		if (traits.Length != 0)
		{
			dsc.AppendLine();
		}
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		base.FromTreeAttributes(tree, worldAccessForResolve);
		BState.FromTreeAttributes(tree, worldAccessForResolve);
		if (BState.Traits == null && worldAccessForResolve.Side == EnumAppSide.Server)
		{
			genTraits(worldAccessForResolve);
		}
	}

	private void genTraits(IWorldAccessor world)
	{
		BState.Traits = Array.Empty<string>();
		for (int i = 0; i < FruitingBushState.AllTraits.Length; i++)
		{
			string[] t = FruitingBushState.AllTraits[i];
			if (world.Rand.NextDouble() < 0.15)
			{
				BState.Traits = BState.Traits.Append<string>(t[(!(world.Rand.NextDouble() < 0.6)) ? 1u : 0u]);
			}
		}
	}

	protected virtual float getHarvestTimeMul()
	{
		if (BState.Traits.Contains<string>("weakclusteredberries"))
		{
			return 1.35f;
		}
		if (BState.Traits.Contains<string>("strongclusteredberries"))
		{
			return 0.65f;
		}
		return 1f;
	}

	protected virtual float getYieldMul()
	{
		if (BState.Traits.Contains<string>("heavybearer"))
		{
			return 1.15f;
		}
		if (BState.Traits.Contains<string>("shybearer"))
		{
			return 0.85f;
		}
		return 1f;
	}

	protected virtual float getNutrientUseMul()
	{
		if (BState.Traits.Contains<string>("weakrooted"))
		{
			return 1.3f;
		}
		if (BState.Traits.Contains<string>("strongrooted"))
		{
			return 0.7f;
		}
		return 1f;
	}

	protected virtual float getRipeTimeMul()
	{
		if (BState.Traits.Contains<string>("thinskinnedfruit"))
		{
			return 0.65f;
		}
		if (BState.Traits.Contains<string>("thickskinnedfruit"))
		{
			return 1.35f;
		}
		return 1f;
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		BState.ToTreeAttributes(tree);
	}

	public EnumFruitingBushHealthState GetHealthState()
	{
		if (BState.WildBushState.HasValue)
		{
			return BState.WildBushState.Value;
		}
		float num = (npkNutrients[0] + npkNutrients[1] + npkNutrients[2]) / 3f / 100f;
		if (!(num < 0.1f))
		{
			if (!(num < 0.3f))
			{
				if (num < 0.8f)
				{
					return EnumFruitingBushHealthState.Healthy;
				}
				return EnumFruitingBushHealthState.Bountiful;
			}
			return EnumFruitingBushHealthState.Struggling;
		}
		return EnumFruitingBushHealthState.Barren;
	}

	public bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		BlockEntitySoilNutrition besn = world.BlockAccessor.GetBlockEntity<BlockEntitySoilNutrition>(base.Pos.DownCopy());
		if (!BState.WildBushState.HasValue && besn != null && besn.OnBlockInteract(byPlayer))
		{
			handling = EnumHandling.PreventDefault;
			Blockentity.MarkDirty(redrawOnClient: true);
			return true;
		}
		if (BState.Growthstate == EnumFruitingBushGrowthState.Ripe)
		{
			handling = EnumHandling.PreventDefault;
			world.PlaySoundAt(bhBush.HarvestingSound, blockSel.Position, 0.0, byPlayer);
			return true;
		}
		return false;
	}

	public bool OnBlockInteractStep(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		if (BState.Growthstate != EnumFruitingBushGrowthState.Ripe)
		{
			return false;
		}
		handling = EnumHandling.PreventDefault;
		playHarvestEffects(byPlayer, blockSel, bhBush.harvestedStacks[0].ResolvedItemstack);
		if (world.Side == EnumAppSide.Client)
		{
			return secondsUsed < bhBush.harvestTime * getHarvestTimeMul();
		}
		return true;
	}

	protected void playHarvestEffects(IPlayer byPlayer, BlockSelection blockSel, ItemStack particlestack)
	{
		IWorldAccessor world = Api.World;
		if (bhBush != null && world.Rand.NextDouble() < 0.05)
		{
			world.PlaySoundAt(bhBush.HarvestingSound, blockSel.Position, 0.0, byPlayer);
		}
		if (world.Side != EnumAppSide.Client)
		{
			return;
		}
		(byPlayer as IClientPlayer)?.TriggerFpAnimation(EnumHandInteract.HeldItemAttack);
		if (world.Rand.NextDouble() < 0.25)
		{
			if (particlestack != null)
			{
				world.SpawnCubeParticles(blockSel.Position.ToVec3d().Add(blockSel.HitPosition), particlestack, 0.25f, 1, 0.5f, byPlayer, new Vec3f(0f, 1f, 0f));
			}
			else
			{
				world.SpawnCubeParticles(base.Pos, base.Pos.ToVec3d(), 0.25f, 1, 0.5f, byPlayer, new Vec3f(0f, 1f, 0f));
			}
		}
	}

	public void OnBlockInteractStop(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		if (BState.Growthstate != EnumFruitingBushGrowthState.Ripe)
		{
			return;
		}
		handling = EnumHandling.PreventDefault;
		if (!(secondsUsed > bhBush.harvestTime * getHarvestTimeMul() - 0.05f) || bhBush.harvestedStacks == null || world.Side != EnumAppSide.Server)
		{
			return;
		}
		float dropRate = getYieldMul();
		JsonObject attributes = base.Block.Attributes;
		if (attributes != null && attributes.IsTrue("forageStatAffected"))
		{
			dropRate *= byPlayer.Entity.Stats.GetBlended("forageDropRate");
		}
		bhBush.harvestedStacks.Foreach(delegate(BlockDropItemStack harvestedStack)
		{
			ItemStack nextItemStack = harvestedStack.GetNextItemStack(dropRate);
			if (nextItemStack != null)
			{
				ItemStack itemStack = nextItemStack.Clone();
				nextItemStack.StackSize = GameMath.RoundRandom(Api.World.Rand, (float)nextItemStack.StackSize * dropRates[(int)GetHealthState()]);
				int stackSize = nextItemStack.StackSize;
				if (!byPlayer.InventoryManager.TryGiveItemstack(nextItemStack))
				{
					world.SpawnItemEntity(nextItemStack, blockSel.Position);
				}
				world.Logger.Audit("{0} Took {1}x{2} from {3} at {4}.", byPlayer.PlayerName, stackSize, nextItemStack.Collectible.Code, base.Block.Code, blockSel.Position);
				TreeAttribute data = new TreeAttribute
				{
					["itemstack"] = new ItemstackAttribute(itemStack.Clone()),
					["byentityid"] = new LongAttribute(byPlayer.Entity.EntityId)
				};
				world.Api.Event.PushEvent("onitemcollected", data);
			}
		});
		world.PlaySoundAt(bhBush.HarvestingSound, blockSel.Position, 0.0, byPlayer);
		setGrowthState(EnumFruitingBushGrowthState.Mature);
	}

	public ItemStack[] GetRipeDrops(IPlayer byPlayer)
	{
		if (bhBush.harvestedStacks == null)
		{
			return null;
		}
		List<ItemStack> todrop = new List<ItemStack>();
		float dropQuantityMultiplier = getYieldMul();
		if (byPlayer != null)
		{
			JsonObject attributes = base.Block.Attributes;
			if (attributes != null && attributes.IsTrue("forageStatAffected"))
			{
				dropQuantityMultiplier *= byPlayer.Entity.Stats.GetBlended("forageDropRate");
			}
		}
		BlockDropItemStack[] harvestedStacks = bhBush.harvestedStacks;
		for (int i = 0; i < harvestedStacks.Length; i++)
		{
			ItemStack stack = harvestedStacks[i].GetNextItemStack(dropQuantityMultiplier);
			if (stack != null)
			{
				stack.StackSize = GameMath.RoundRandom(Api.World.Rand, (float)stack.StackSize * dropRates[(int)GetHealthState()]);
				todrop.Add(stack);
			}
		}
		return todrop.ToArray();
	}

	public bool OnBlockInteractCancel(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
	{
		return false;
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
	{
		if (BESoil != null)
		{
			float[] tfMatrix = new Matrixf().Translate(0f, -0.9375f, 0f).Values;
			mesher.AddMeshData(BESoil.FertilizerQuad, tfMatrix);
		}
		return base.OnTesselation(mesher, tessThreadTesselator);
	}

	public bool IsSuitableFor(Entity entity, CreatureDiet diet)
	{
		if (diet == null || BState.Growthstate != EnumFruitingBushGrowthState.Ripe)
		{
			return false;
		}
		return diet.Matches(EnumFoodCategory.NoNutrition, bhBush.CreatureDietFoodTags);
	}

	public float ConsumeOnePortion(Entity entity)
	{
		float dropRate = getYieldMul();
		bhBush.harvestedStacks.Foreach(delegate(BlockDropItemStack harvestedStack)
		{
			ItemStack nextItemStack = harvestedStack.GetNextItemStack(dropRate);
			if (nextItemStack != null)
			{
				nextItemStack.StackSize = GameMath.RoundRandom(Api.World.Rand, (float)nextItemStack.StackSize * dropRates[(int)GetHealthState()]);
				int stackSize = nextItemStack.StackSize;
				Api.World.SpawnItemEntity(nextItemStack, base.Pos);
				Api.World.Logger.Audit("{0} Took {1}x{2} from {3} at {4}.", entity.GetName(), stackSize, nextItemStack.Collectible.Code, base.Block.Code, base.Pos);
			}
		});
		Api.World.PlaySoundAt(bhBush.HarvestingSound, base.Pos, 0.0);
		setGrowthState(EnumFruitingBushGrowthState.Mature);
		return 0.1f;
	}

	public override void OnBlockRemoved()
	{
		base.OnBlockRemoved();
		if (Api.Side == EnumAppSide.Server)
		{
			Api.ModLoader.GetModSystem<POIRegistry>().RemovePOI(this);
		}
	}

	public override void OnBlockUnloaded()
	{
		base.OnBlockUnloaded();
		ICoreAPI api = Api;
		if (api != null && api.Side == EnumAppSide.Server)
		{
			Api.ModLoader.GetModSystem<POIRegistry>().RemovePOI(this);
		}
	}

	public bool IsHarvestable(ItemSlot slot, Entity forEntity)
	{
		ItemStack? itemstack = slot.Itemstack;
		if (itemstack != null && itemstack.Collectible.GetTool(slot) == EnumTool.Knife)
		{
			return (Api.World.Calendar.TotalDays - BState.LastCuttingTakenTotalDays) / (double)Api.World.Calendar.DaysPerYear >= yearsBetweenCuttings;
		}
		return false;
	}

	public float GetHarvestDuration(ItemSlot slot, Entity forEntity)
	{
		EntityPlayer eplr = forEntity as EntityPlayer;
		playHarvestEffects(eplr?.Player, eplr.BlockSelection, null);
		return bhBush.cuttingTime;
	}

	public void SetHarvested(IPlayer byPlayer, float dropQuantityMultiplier = 1f)
	{
		ItemStack cuttingStack = new ItemStack(Api.World.GetBlock(AssetLocation.Create(base.Block.Attributes["cuttingBlockCode"].AsString(), base.Block.Code.Domain)));
		cuttingStack.Attributes.SetString("traits", string.Join(",", BState.Traits));
		if (!byPlayer.InventoryManager.TryGiveItemstack(cuttingStack))
		{
			Api.World.SpawnItemEntity(cuttingStack, base.Pos.ToVec3d().Add(0.5, 0.5, 0.5));
		}
		BState.LastCuttingTakenTotalDays = Api.World.Calendar.TotalDays;
		Blockentity.MarkDirty(redrawOnClient: true);
	}
}
