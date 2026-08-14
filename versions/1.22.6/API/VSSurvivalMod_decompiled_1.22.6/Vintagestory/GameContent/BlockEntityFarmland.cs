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

public class BlockEntityFarmland : BlockEntitySoilNutrition, IFarmlandBlockEntity, IAnimalFoodSource, IPointOfInterest
{
	protected bool unripeCropColdDamaged;

	protected bool unripeHeatDamaged;

	protected bool ripeCropColdDamaged;

	protected Block deadCropBlock;

	protected bool allowcropDeath;

	protected double totalHoursForNextStage;

	protected TreeAttribute cropAttrs = new TreeAttribute();

	protected override int RainHeightOffset => (GetCrop() != null) ? 1 : 0;

	protected override bool RecoverFertility
	{
		get
		{
			if (GetCrop() != null)
			{
				return !HasRipeCrop();
			}
			return true;
		}
	}

	public ITreeAttribute CropAttributes => cropAttrs;

	public double TotalHoursForNextStage => totalHoursForNextStage;

	public Vec3d Position => Pos.ToVec3d().Add(0.5, 1.0, 0.5);

	public string Type => "food";

	BlockPos IFarmlandBlockEntity.Pos => Pos;

	public double TotalHoursFertilityCheck
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override void Initialize(ICoreAPI api)
	{
		base.Initialize(api);
		if (api is ICoreServerAPI)
		{
			api.ModLoader.GetModSystem<POIRegistry>().AddPOI(this);
		}
		deadCropBlock = Api.World.GetBlock(new AssetLocation("deadcrop"));
		allowcropDeath = Api.World.Config.GetBool("allowCropDeath", defaultValue: true);
	}

	public override void OnCropBlockBroken()
	{
		ripeCropColdDamaged = false;
		unripeCropColdDamaged = false;
		unripeHeatDamaged = false;
		base.OnCropBlockBroken();
	}

	protected override void beginIntervalledUpdate(out FarmlandFastForwardUpdate onInterval, out FarmlandUpdateEnd onEnd)
	{
		Block cropBlock = GetCrop();
		bool hasCrop = cropBlock != null;
		bool hasRipeCrop = HasRipeCrop();
		double hoursNextStage = GetHoursForNextStage();
		base.beginIntervalledUpdate(out onInterval, out onEnd);
		FarmlandFastForwardUpdate prevOnInterval = onInterval;
		onInterval = delegate(double hourIntervall, ClimateCondition conds, double lightGrowthSpeedFactor, bool growthPaused)
		{
			prevOnInterval(hourIntervall, conds, lightGrowthSpeedFactor, growthPaused);
			totalHoursForNextStage += hourIntervall * (1.0 - lightGrowthSpeedFactor);
			hasCrop = updateCropDamage(hourIntervall, cropBlock, hasCrop, hasRipeCrop, conds);
			if (growthPaused)
			{
				totalHoursForNextStage += previousHourInterval;
			}
			else if (!((double)moistureLevel < 0.1) && totalHoursLastUpdate >= totalHoursForNextStage)
			{
				TryGrowCrop(totalHoursForNextStage);
				hasRipeCrop = HasRipeCrop();
				totalHoursForNextStage += hoursNextStage;
				hoursNextStage = GetHoursForNextStage();
			}
		};
	}

	protected override void onRollback(double hoursrolledback)
	{
		base.onRollback(hoursrolledback);
		totalHoursForNextStage -= hoursrolledback;
	}

	private bool updateCropDamage(double hourIntervall, Block cropBlock, bool hasCrop, bool hasRipeCrop, ClimateCondition conds)
	{
		if (!hasCrop)
		{
			ripeCropColdDamaged = false;
			unripeCropColdDamaged = false;
			unripeHeatDamaged = false;
			for (int i = 0; i < damageAccum.Length; i++)
			{
				damageAccum[i] = 0f;
			}
		}
		else
		{
			if (cropBlock?.CropProps != null && conds.Temperature < cropBlock.CropProps.ColdDamageBelow)
			{
				if (hasRipeCrop)
				{
					ripeCropColdDamaged = true;
				}
				else
				{
					unripeCropColdDamaged = true;
					damageAccum[2] += (float)hourIntervall;
				}
			}
			else
			{
				damageAccum[2] = Math.Max(0f, damageAccum[2] - (float)hourIntervall / 10f);
			}
			if (cropBlock?.CropProps != null && conds.Temperature > cropBlock.CropProps.HeatDamageAbove && hasCrop)
			{
				unripeHeatDamaged = true;
				damageAccum[1] += (float)hourIntervall;
			}
			else
			{
				damageAccum[1] = Math.Max(0f, damageAccum[1] - (float)hourIntervall / 10f);
			}
			for (int j = 0; j < damageAccum.Length; j++)
			{
				float dmg = damageAccum[j];
				if (!allowcropDeath)
				{
					dmg = (damageAccum[j] = 0f);
				}
				if (dmg > 48f)
				{
					Api.World.BlockAccessor.SetBlock(deadCropBlock.Id, upPos);
					BlockEntityDeadCrop obj = Api.World.BlockAccessor.GetBlockEntity(upPos) as BlockEntityDeadCrop;
					obj.Inventory[0].Itemstack = new ItemStack(cropBlock);
					obj.deathReason = (EnumCropStressType)j;
					hasCrop = false;
					break;
				}
			}
		}
		return hasCrop;
	}

	public double GetHoursForNextStage()
	{
		Block block = GetCrop();
		if (block == null)
		{
			return 99999999.0;
		}
		float totalDays = block.CropProps.TotalGrowthDays;
		totalDays = ((!(totalDays > 0f)) ? (block.CropProps.TotalGrowthMonths * (float)Api.World.Calendar.DaysPerMonth) : (totalDays / 12f * (float)Api.World.Calendar.DaysPerMonth));
		return Api.World.Calendar.HoursPerDay * totalDays / (float)Math.Max(1, block.CropProps.GrowthStages - 1) * (1f / GetGrowthRate(block.CropProps.RequiredNutrient)) * (float)(0.9 + 0.2 * BlockEntityFastForwardGrowth.rand.NextDouble()) / growthRateMul;
	}

	public bool TryPlant(Block block, ItemSlot itemslot, EntityAgent byEntity, BlockSelection blockSel)
	{
		if (CanPlant() && block.CropProps != null)
		{
			Api.World.BlockAccessor.SetBlock(block.BlockId, upPos);
			totalHoursForNextStage = Api.World.Calendar.TotalHours + GetHoursForNextStage();
			CropBehavior[] behaviors = block.CropProps.Behaviors;
			for (int i = 0; i < behaviors.Length; i++)
			{
				behaviors[i].OnPlanted(Api, itemslot, byEntity, blockSel);
			}
			return true;
		}
		return false;
	}

	public bool CanPlant()
	{
		Block block = Api.World.BlockAccessor.GetBlock(upPos);
		if (block != null)
		{
			return block.BlockMaterial == EnumBlockMaterial.Air;
		}
		return true;
	}

	public bool HasRipeCrop()
	{
		Block block = GetCrop();
		if (block != null)
		{
			return GetCropStage(block) >= block.CropProps.GrowthStages;
		}
		return false;
	}

	public bool TryGrowCrop(double currentTotalHours)
	{
		Block block = GetCrop();
		if (block == null)
		{
			return false;
		}
		int currentGrowthStage = GetCropStage(block);
		if (currentGrowthStage < block.CropProps.GrowthStages)
		{
			int newGrowthStage = currentGrowthStage + 1;
			Block nextBlock = Api.World.GetBlock(block.CodeWithParts(newGrowthStage.ToString() ?? ""));
			if (nextBlock == null)
			{
				return false;
			}
			if (block.CropProps.Behaviors != null)
			{
				EnumHandling handled = EnumHandling.PassThrough;
				bool result = false;
				CropBehavior[] behaviors = block.CropProps.Behaviors;
				for (int i = 0; i < behaviors.Length; i++)
				{
					result = behaviors[i].TryGrowCrop(Api, this, currentTotalHours, newGrowthStage, ref handled);
					if (handled == EnumHandling.PreventSubsequent)
					{
						return result;
					}
				}
				if (handled == EnumHandling.PreventDefault)
				{
					return result;
				}
			}
			if (Api.World.BlockAccessor.GetBlockEntity(upPos) == null)
			{
				Api.World.BlockAccessor.SetBlock(nextBlock.BlockId, upPos);
			}
			else
			{
				Api.World.BlockAccessor.ExchangeBlock(nextBlock.BlockId, upPos);
			}
			ConsumeNutrients(block);
			return true;
		}
		return false;
	}

	protected void ConsumeNutrients(Block cropBlock)
	{
		float nutrientLoss = cropBlock.CropProps.NutrientConsumption / (float)Math.Max(1, cropBlock.CropProps.GrowthStages - 1);
		ConsumeNutrients(cropBlock.CropProps.RequiredNutrient, nutrientLoss);
	}

	public Block GetCrop()
	{
		Block block = Api.World.BlockAccessor.GetBlock(upPos);
		if (block == null || block.CropProps == null)
		{
			return null;
		}
		return block;
	}

	public int GetCropStage(Block block)
	{
		int.TryParse(block.LastCodePart(), out var stage);
		return stage;
	}

	public ItemStack[] GetDrops(ItemStack[] drops)
	{
		BlockEntityDeadCrop beDeadCrop = Api.World.BlockAccessor.GetBlockEntity(upPos) as BlockEntityDeadCrop;
		bool isDead = beDeadCrop != null;
		if (!ripeCropColdDamaged && !unripeCropColdDamaged && !unripeHeatDamaged && !isDead)
		{
			return drops;
		}
		if (!Api.World.Config.GetString("harshWinters").ToBool(defaultValue: true))
		{
			return drops;
		}
		List<ItemStack> stacks = new List<ItemStack>();
		BlockCropProperties cropProps = GetCrop()?.CropProps;
		if (cropProps == null)
		{
			return drops;
		}
		float mul = 1f;
		if (ripeCropColdDamaged)
		{
			mul = cropProps.ColdDamageRipeMul;
		}
		if (unripeHeatDamaged || unripeCropColdDamaged)
		{
			mul = cropProps.DamageGrowthStuntMul;
		}
		if (isDead)
		{
			mul = ((beDeadCrop.deathReason == EnumCropStressType.Eaten) ? 0f : Math.Max(cropProps.ColdDamageRipeMul, cropProps.DamageGrowthStuntMul));
		}
		string[] debuffUnaffectedDrops = base.Block.Attributes?["debuffUnaffectedDrops"].AsArray<string>();
		foreach (ItemStack stack in drops)
		{
			if (WildcardUtil.Match(debuffUnaffectedDrops, stack.Collectible.Code.ToShortString()))
			{
				stacks.Add(stack);
				continue;
			}
			float q = (float)stack.StackSize * mul;
			float frac = q - (float)(int)q;
			stack.StackSize = (int)q + ((Api.World.Rand.NextDouble() > (double)frac) ? 1 : 0);
			if (stack.StackSize > 0)
			{
				stacks.Add(stack);
			}
		}
		MarkDirty(redrawOnClient: true);
		return stacks.ToArray();
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
	{
		base.FromTreeAttributes(tree, worldForResolving);
		if (tree.HasAttribute("totalHoursForNextStage"))
		{
			totalHoursForNextStage = tree.GetDouble("totalHoursForNextStage");
		}
		else
		{
			totalHoursForNextStage = tree.GetDouble("totalDaysForNextStage") * 24.0;
		}
		cropAttrs = tree["cropAttrs"] as TreeAttribute;
		if (cropAttrs == null)
		{
			cropAttrs = new TreeAttribute();
		}
		unripeCropColdDamaged = tree.GetBool("unripeCropExposedToFrost");
		ripeCropColdDamaged = tree.GetBool("ripeCropExposedToFrost");
		unripeHeatDamaged = tree.GetBool("unripeHeatDamaged");
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		tree.SetDouble("totalHoursForNextStage", totalHoursForNextStage);
		tree.SetBool("ripeCropExposedToFrost", ripeCropColdDamaged);
		tree.SetBool("unripeCropExposedToFrost", unripeCropColdDamaged);
		tree.SetBool("unripeHeatDamaged", unripeHeatDamaged);
		tree["cropAttrs"] = cropAttrs;
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
		BlockCropProperties cropProps = GetCrop()?.CropProps;
		if (cropProps != null)
		{
			dsc.AppendLine(Lang.Get("Required Nutrient: {0}", cropProps.RequiredNutrient));
			dsc.AppendLine(Lang.Get("Growth Stage: {0} / {1}", GetCropStage(GetCrop()), cropProps.GrowthStages));
			dsc.AppendLine();
		}
		if (cropProps == null)
		{
			float speedn = (float)Math.Round(100f * GetGrowthRate(EnumSoilNutrient.N), 0);
			float speedp = (float)Math.Round(100f * GetGrowthRate(EnumSoilNutrient.P), 0);
			float speedk = (float)Math.Round(100f * GetGrowthRate(EnumSoilNutrient.K), 0);
			string colorn = ColorUtil.Int2Hex(GuiStyle.DamageColorGradient[(int)Math.Min(99f, speedn)]);
			string colorp = ColorUtil.Int2Hex(GuiStyle.DamageColorGradient[(int)Math.Min(99f, speedp)]);
			string colork = ColorUtil.Int2Hex(GuiStyle.DamageColorGradient[(int)Math.Min(99f, speedk)]);
			dsc.AppendLine(Lang.Get("farmland-growthspeeds", colorn, speedn, colorp, speedp, colork, speedk));
		}
		else
		{
			float speed = (float)Math.Round(100f * GetGrowthRate(cropProps.RequiredNutrient), 0);
			string color = ColorUtil.Int2Hex(GuiStyle.DamageColorGradient[(int)Math.Min(99f, speed)]);
			dsc.AppendLine(Lang.Get("farmland-growthspeed", color, speed, cropProps.RequiredNutrient));
		}
		if ((ripeCropColdDamaged || unripeCropColdDamaged || unripeHeatDamaged) && cropProps != null)
		{
			if (ripeCropColdDamaged)
			{
				dsc.AppendLine(Lang.Get("farmland-ripecolddamaged", (int)(cropProps.ColdDamageRipeMul * 100f)));
			}
			else if (unripeCropColdDamaged)
			{
				dsc.AppendLine(Lang.Get("farmland-unripecolddamaged", (int)(cropProps.DamageGrowthStuntMul * 100f)));
			}
			else if (unripeHeatDamaged)
			{
				dsc.AppendLine(Lang.Get("farmland-unripeheatdamaged", (int)(cropProps.DamageGrowthStuntMul * 100f)));
			}
		}
		base.GetBlockInfo(forPlayer, dsc);
	}

	public bool IsSuitableFor(Entity entity, CreatureDiet diet)
	{
		if (diet == null)
		{
			return false;
		}
		Block cropBlock = GetCrop();
		if (cropBlock == null)
		{
			return false;
		}
		string[] creatureFoodTags = cropBlock.Attributes?["foodTags"].AsArray(Array.Empty<string>()) ?? Array.Empty<string>();
		return diet.Matches(EnumFoodCategory.NoNutrition, creatureFoodTags);
	}

	public float ConsumeOnePortion(Entity entity)
	{
		Block cropBlock = GetCrop();
		if (cropBlock == null)
		{
			return 0f;
		}
		Block deadCropBlock = Api.World.GetBlock(new AssetLocation("deadcrop"));
		Api.World.BlockAccessor.SetBlock(deadCropBlock.Id, upPos);
		BlockEntityDeadCrop obj = Api.World.BlockAccessor.GetBlockEntity(upPos) as BlockEntityDeadCrop;
		obj.Inventory[0].Itemstack = new ItemStack(cropBlock);
		obj.deathReason = EnumCropStressType.Eaten;
		return 1f;
	}

	protected override void UpdateFarmlandBlock()
	{
		int nowLevel = GetFertilityLevel((originalFertility[0] + originalFertility[1] + originalFertility[2]) / 3);
		Block hereblock = Api.World.BlockAccessor.GetBlock(Pos);
		AssetLocation newCode = hereblock?.CodeWithVariants(new string[2] { "state", "fertility" }, new string[2]
		{
			base.IsVisiblyMoist ? "moist" : "dry",
			BlockEntitySoilNutrition.Fertilities.GetKeyAtIndex(nowLevel)
		});
		Block nextBlock = Api.World.GetBlock(newCode);
		if (nextBlock != null && hereblock != null && hereblock.BlockId != nextBlock.BlockId)
		{
			Api.World.BlockAccessor.ExchangeBlock(nextBlock.BlockId, Pos);
			Api.World.BlockAccessor.MarkBlockEntityDirty(Pos);
			Api.World.BlockAccessor.MarkBlockDirty(Pos);
		}
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
}
