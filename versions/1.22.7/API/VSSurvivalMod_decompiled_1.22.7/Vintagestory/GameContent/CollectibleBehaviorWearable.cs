using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class CollectibleBehaviorWearable : CollectibleBehaviorWearableAttachment, IWearable, IWearableStatsSupplier
{
	private StatModifiers? StatModifers;

	private ProtectionModifiers? ProtectionModifiers;

	private AssetLocation[]? FootStepSounds;

	private EnumCharacterDressType DressType;

	private ICoreAPI api;

	public CollectibleBehaviorWearable(CollectibleObject collObj)
		: base(collObj)
	{
	}

	public override string GetMeshCacheKey(ItemStack stack, string slotType)
	{
		return slotType + "-wearableModelRef-" + stack.Collectible.Code.ToString();
	}

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		this.api = api;
		Enum.TryParse<EnumCharacterDressType>(collObj.Attributes["clothescategory"].AsString(), ignoreCase: true, out var dt);
		DressType = dt;
		JsonObject jsonObj = collObj.Attributes?["footStepSound"];
		if (jsonObj != null && jsonObj.Exists)
		{
			string soundloc = jsonObj.AsString();
			if (soundloc != null)
			{
				AssetLocation loc = AssetLocation.Create(soundloc, collObj.Code.Domain).WithPathPrefixOnce("sounds/");
				if (soundloc.EndsWith('*'))
				{
					loc.Path = loc.Path.TrimEnd('*');
					FootStepSounds = api.Assets.GetLocations(loc.Path, loc.Domain).ToArray();
				}
				else
				{
					FootStepSounds = new AssetLocation[1] { loc };
				}
			}
		}
		jsonObj = collObj.Attributes?["statModifiers"];
		if (jsonObj != null && jsonObj.Exists)
		{
			try
			{
				StatModifers = jsonObj.AsObject<StatModifiers>();
			}
			catch (Exception e)
			{
				api.World.Logger.Error("Failed loading statModifiers for item/block {0}. Will ignore.", collObj.Code);
				api.World.Logger.Error(e);
				StatModifers = null;
			}
		}
		ProtectionModifiers defMods = null;
		jsonObj = collObj.Attributes?["defaultProtLoss"];
		if (jsonObj != null && jsonObj.Exists)
		{
			try
			{
				defMods = jsonObj.AsObject<ProtectionModifiers>();
			}
			catch (Exception e2)
			{
				api.World.Logger.Error("Failed loading defaultProtLoss for item/block {0}. Will ignore.", collObj.Code);
				api.World.Logger.Error(e2);
			}
		}
		jsonObj = collObj.Attributes?["protectionModifiers"];
		if (jsonObj != null && jsonObj.Exists)
		{
			try
			{
				ProtectionModifiers = jsonObj.AsObject<ProtectionModifiers>();
			}
			catch (Exception e3)
			{
				api.World.Logger.Error("Failed loading protectionModifiers for item/block {0}. Will ignore.", collObj.Code);
				api.World.Logger.Error(e3);
				ProtectionModifiers = null;
			}
		}
		if (ProtectionModifiers != null && ProtectionModifiers.PerTierFlatDamageReductionLoss == null)
		{
			ProtectionModifiers.PerTierFlatDamageReductionLoss = defMods?.PerTierFlatDamageReductionLoss;
		}
		if (ProtectionModifiers != null && ProtectionModifiers.PerTierRelativeProtectionLoss == null)
		{
			ProtectionModifiers.PerTierRelativeProtectionLoss = defMods?.PerTierRelativeProtectionLoss;
		}
	}

	public override void OnUnloaded(ICoreAPI api)
	{
		base.OnUnloaded(api);
		Dictionary<string, MultiTextureMeshRef> armorMeshrefs = ObjectCacheUtil.TryGet<Dictionary<string, MultiTextureMeshRef>>(api, "armorMeshRefs");
		if (armorMeshrefs == null)
		{
			return;
		}
		foreach (MultiTextureMeshRef value in armorMeshrefs.Values)
		{
			value?.Dispose();
		}
		api.ObjectCache.Remove("armorMeshRefs");
	}

	public override void OnHandbookRecipeRender(ICoreClientAPI capi, IRecipeBase recipe, ItemSlot dummyslot, double x, double y, double z, double size, ref EnumHandling handling)
	{
		if (recipe.Name.Path.Contains("repair"))
		{
			int prevDura = dummyslot.Itemstack.Collectible.GetRemainingDurability(dummyslot.Itemstack);
			dummyslot.Itemstack.Collectible.SetDurability(dummyslot.Itemstack, 0);
			capi.Render.RenderItemstackToGui(dummyslot, x, y, z, (float)size * 0.58f, -1);
			dummyslot.Itemstack.Collectible.SetDurability(dummyslot.Itemstack, prevDura);
			handling = EnumHandling.PreventSubsequent;
		}
	}

	public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling, ref EnumHandling handling)
	{
		if (byEntity.Controls.ShiftKey)
		{
			return;
		}
		if (blockSel != null)
		{
			Block block = api.World.BlockAccessor.GetBlock(blockSel.Position);
			BlockPos pos = blockSel.Position;
			Vec3i relPos = block.GetBehavior<BlockBehaviorMultiblock>()?.ControllerPositionRel;
			if ((object)relPos != null)
			{
				pos = pos.AddCopy(relPos);
			}
			BlockEntity blockEntity = api.World.BlockAccessor.GetBlockEntity(pos);
			if (blockEntity != null && blockEntity.Behaviors.Any((BlockEntityBehavior bh) => (bh is BEBehaviorMannequin || bh is BEBehaviorDisplay) ? true : false))
			{
				return;
			}
		}
		IPlayer byPlayer = (byEntity as EntityPlayer)?.Player;
		if (byPlayer != null)
		{
			IInventory inv = byPlayer.InventoryManager.GetOwnInventory("character");
			if (inv != null && GetDressType(slot) != EnumCharacterDressType.Unknown && inv[(int)GetDressType(slot)].TryFlipWith(slot))
			{
				handHandling = EnumHandHandling.PreventDefault;
				handling = EnumHandling.PreventSubsequent;
			}
		}
	}

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
	{
		string descTextToAppend = "";
		string descText = collObj.GetItemDescText();
		if (descText.Length > 1)
		{
			int descIndex = dsc.ToString().IndexOfOrdinal(descText);
			if (descIndex > 0)
			{
				if (descIndex > 0)
				{
					descIndex--;
				}
				else
				{
					descTextToAppend = "\n";
				}
				descTextToAppend += dsc.ToString(descIndex, dsc.Length - descIndex);
				dsc.Remove(descIndex, dsc.Length - descIndex);
			}
		}
		EnumCharacterDressType dressType = GetDressType(inSlot);
		if (dressType == EnumCharacterDressType.Unknown)
		{
			dsc.AppendLine(Lang.Get("Clothing Category: Unknown"));
		}
		else
		{
			dsc.AppendLine(Lang.Get("Clothing Category: {0}", Lang.Get("clothcategory-" + dressType.ToString().ToLowerInvariant())));
		}
		ProtectionModifiers protectionModifiers = GetProtectionModifiers(inSlot);
		if (protectionModifiers != null)
		{
			if (protectionModifiers.FlatDamageReduction != 0f)
			{
				dsc.AppendLine(Lang.Get("Flat damage reduction: {0} hp", protectionModifiers.FlatDamageReduction));
			}
			if (protectionModifiers.RelativeProtection != 0f)
			{
				dsc.AppendLine(Lang.Get("Percent protection: {0}%", (int)(100f * protectionModifiers.RelativeProtection)));
			}
			dsc.AppendLine(Lang.Get("Protection tier: {0}", protectionModifiers.ProtectionTier));
		}
		StatModifiers statModifiers = GetStatModifiers(inSlot);
		if (statModifiers != null)
		{
			if (protectionModifiers != null)
			{
				dsc.AppendLine();
			}
			if (statModifiers.healingeffectivness != 0f)
			{
				dsc.AppendLine(Lang.Get("Healing effectivness: {0}%", (int)(100f * statModifiers.healingeffectivness)));
			}
			if (statModifiers.hungerrate != 0f)
			{
				dsc.AppendLine(Lang.Get("Hunger rate: {1}{0}%", (int)(100f * statModifiers.hungerrate), (statModifiers.hungerrate > 0f) ? "+" : ""));
			}
			if (statModifiers.rangedWeaponsAcc != 0f)
			{
				dsc.AppendLine(Lang.Get("Ranged Weapon Accuracy: {1}{0}%", (int)(100f * statModifiers.rangedWeaponsAcc), (statModifiers.rangedWeaponsAcc > 0f) ? "+" : ""));
			}
			if (statModifiers.rangedWeaponsSpeed != 0f)
			{
				dsc.AppendLine(Lang.Get("Ranged Weapon Charge Time: {1}{0}%", -(int)(100f * statModifiers.rangedWeaponsSpeed), (0f - statModifiers.rangedWeaponsSpeed > 0f) ? "+" : ""));
			}
			if (statModifiers.walkSpeed != 0f)
			{
				dsc.AppendLine(Lang.Get("Walk speed: {1}{0}%", (int)(100f * statModifiers.walkSpeed), (statModifiers.walkSpeed > 0f) ? "+" : ""));
			}
		}
		if (protectionModifiers != null && protectionModifiers.HighDamageTierResistant)
		{
			dsc.AppendLine("<font color=\"#86aad0\">" + Lang.Get("High damage tier resistant") + "</font> " + Lang.Get("When damaged by a higher tier attack, the loss of protection is only half as much."));
		}
		if (GetDressType(inSlot) == EnumCharacterDressType.Head)
		{
			float rainProt = collObj.Attributes["rainProtectionPerc"].AsFloat();
			if (rainProt > 0f)
			{
				dsc.AppendLine(Lang.Get("wearable-protection-from-rain", (int)(rainProt * 100f)));
			}
		}
		float maxWarmth = GetMaxWarmth(inSlot);
		if (maxWarmth != 0f)
		{
			if (!(inSlot is ItemSlotCreative))
			{
				ensureConditionExists(inSlot);
				float condition = inSlot.Itemstack.Attributes.GetFloat("condition", 1f);
				string condStr = (((double)condition > 0.5) ? Lang.Get("clothingcondition-good", (int)(condition * 100f)) : (((double)condition > 0.4) ? Lang.Get("clothingcondition-worn", (int)(condition * 100f)) : (((double)condition > 0.3) ? Lang.Get("clothingcondition-heavilyworn", (int)(condition * 100f)) : (((double)condition > 0.2) ? Lang.Get("clothingcondition-tattered", (int)(condition * 100f)) : ((!((double)condition > 0.1)) ? Lang.Get("clothingcondition-terrible", (int)(condition * 100f)) : Lang.Get("clothingcondition-heavilytattered", (int)(condition * 100f)))))));
				dsc.Append(Lang.Get("Condition:") + " ");
				float warmth = GetWarmth(inSlot);
				string color = ColorUtil.Int2Hex(GuiStyle.DamageColorGradient[(int)Math.Min(99f, condition * 200f)]);
				if ((double)warmth < 0.05)
				{
					dsc.AppendLine("<font color=\"" + color + "\">" + condStr + "</font>, <font color=\"#ff8484\">" + Lang.Get("+{0:0.#}°C", warmth) + "</font>");
				}
				else
				{
					dsc.AppendLine("<font color=\"" + color + "\">" + condStr + "</font>, <font color=\"#84ff84\">" + Lang.Get("+{0:0.#}°C", warmth) + "</font>");
				}
			}
			dsc.AppendLine();
			dsc.AppendLine(Lang.Get("clothing-maxwarmth", maxWarmth));
		}
		dsc.Append(descTextToAppend);
	}

	public virtual float GetWarmth(ItemSlot inslot)
	{
		ensureConditionExists(inslot);
		float maxWarmth = GetMaxWarmth(inslot);
		float condition = inslot.Itemstack.Attributes.GetFloat("condition", 1f);
		return Math.Min(maxWarmth, condition * 2f * maxWarmth);
	}

	public virtual void ChangeCondition(ItemSlot slot, float changeVal)
	{
		if (changeVal != 0f)
		{
			ensureConditionExists(slot);
			slot.Itemstack.Attributes.SetFloat("condition", GameMath.Clamp(slot.Itemstack.Attributes.GetFloat("condition", 1f) + changeVal, 0f, 1f));
			slot.MarkDirty();
		}
	}

	public override bool RequiresTransitionableTicking(IWorldAccessor world, ItemStack itemstack, ref EnumHandling handling)
	{
		handling = EnumHandling.PreventSubsequent;
		return !itemstack.Attributes.HasAttribute("condition");
	}

	protected virtual void ensureConditionExists(ItemSlot slot, bool markdirty = true)
	{
		if (!(slot is DummySlot) && !slot.Itemstack.Attributes.HasAttribute("condition") && api.Side == EnumAppSide.Server && GetMaxWarmth(slot) != 0f)
		{
			if (slot is ItemSlotTrade)
			{
				slot.Itemstack.Attributes.SetFloat("condition", (float)api.World.Rand.NextDouble() * 0.25f + 0.75f);
			}
			else
			{
				slot.Itemstack.Attributes.SetFloat("condition", (float)api.World.Rand.NextDouble() * 0.4f);
			}
			if (markdirty)
			{
				slot.MarkDirty();
			}
		}
	}

	public override void OnCreatedByCrafting(ItemSlot[] inSlots, ItemSlot outputSlot, IRecipeBase byRecipe, ref EnumHandling bhHandling)
	{
		base.OnCreatedByCrafting(inSlots, outputSlot, byRecipe, ref bhHandling);
		if (!(outputSlot is DummySlot))
		{
			ensureConditionExists(outputSlot);
			outputSlot.Itemstack.Attributes.SetFloat("condition", 1f);
			if (byRecipe.Name.Path.Contains("repair"))
			{
				CalculateRepairValue(inSlots, outputSlot, out var repairValue, out var _);
				int curDur = outputSlot.Itemstack.Collectible.GetRemainingDurability(outputSlot.Itemstack);
				int maxDur = collObj.GetMaxDurability(outputSlot.Itemstack);
				outputSlot.Itemstack.Collectible.SetDurability(outputSlot.Itemstack, Math.Min(maxDur, (int)((float)curDur + (float)maxDur * repairValue)));
				bhHandling = EnumHandling.Handled;
			}
		}
	}

	public override bool ConsumeCraftingIngredients(ItemSlot[] inSlots, ItemSlot outputSlot, IRecipeBase recipe, ref EnumHandling bhHandling)
	{
		if (recipe.Name.Path.Contains("repair"))
		{
			CalculateRepairValue(inSlots, outputSlot, out var _, out var matCostPerMatType);
			foreach (ItemSlot islot in inSlots)
			{
				if (!islot.Empty)
				{
					if (islot.Itemstack.Collectible == collObj)
					{
						islot.Itemstack = null;
					}
					else
					{
						islot.TakeOut(matCostPerMatType);
					}
				}
			}
			bhHandling = EnumHandling.PreventSubsequent;
			return true;
		}
		return false;
	}

	public virtual void CalculateRepairValue(ItemSlot[] inSlots, ItemSlot outputSlot, out float repairValue, out int matCostPerMatType)
	{
		int origMatCount = GetOrigMatCount(inSlots, outputSlot);
		ItemSlot armorSlot = inSlots.FirstOrDefault((ItemSlot slot) => slot.Itemstack?.Collectible.GetCollectibleInterface<IWearable>() != null);
		int curDur = outputSlot.Itemstack.Collectible.GetRemainingDurability(armorSlot.Itemstack);
		int maxDur = collObj.GetMaxDurability(outputSlot.Itemstack);
		float repairDurabilityPerItem = 2f / (float)origMatCount * (float)maxDur;
		int fullRepairMatCount = (int)Math.Max(1.0, Math.Round((float)(maxDur - curDur) / repairDurabilityPerItem));
		int minMatStackSize = GetInputRepairCount(inSlots);
		int matTypeCount = GetRepairMatTypeCount(inSlots);
		int availableRepairMatCount = Math.Min(fullRepairMatCount, minMatStackSize * matTypeCount);
		matCostPerMatType = Math.Min(fullRepairMatCount, minMatStackSize);
		repairValue = (float)availableRepairMatCount / (float)origMatCount * 2f;
	}

	protected virtual int GetRepairMatTypeCount(ItemSlot[] slots)
	{
		List<ItemStack> stackTypes = new List<ItemStack>();
		foreach (ItemSlot slot in slots)
		{
			if (slot.Empty)
			{
				continue;
			}
			bool found = false;
			if (slot.Itemstack.Collectible.GetCollectibleInterface<IWearable>() != null)
			{
				continue;
			}
			foreach (ItemStack stack in stackTypes)
			{
				if (slot.Itemstack.Satisfies(stack))
				{
					found = true;
					break;
				}
			}
			if (!found)
			{
				stackTypes.Add(slot.Itemstack);
			}
		}
		return stackTypes.Count;
	}

	public virtual int GetInputRepairCount(ItemSlot[] inputSlots)
	{
		Vintagestory.API.Datastructures.OrderedDictionary<int, int> matcounts = new Vintagestory.API.Datastructures.OrderedDictionary<int, int>();
		foreach (ItemSlot slot in inputSlots)
		{
			if (!slot.Empty && slot.Itemstack.Collectible.GetCollectibleInterface<IWearable>() == null)
			{
				int hash = slot.Itemstack.GetHashCode();
				matcounts.TryGetValue(hash, out var cnt);
				matcounts[hash] = cnt + slot.StackSize;
			}
		}
		return matcounts.Values.Min();
	}

	public virtual int GetOrigMatCount(ItemSlot[] inputSlots, ItemSlot outputSlot)
	{
		ItemStack stack = outputSlot.Itemstack;
		_ = inputSlots.FirstOrDefault((ItemSlot slot) => !slot.Empty && slot.Itemstack.Collectible != collObj).Itemstack;
		int origMatCount = 0;
		foreach (GridRecipe recipe in api.World.GridRecipes)
		{
			ItemStack? resolvedItemStack = recipe.RecipeOutput.ResolvedItemStack;
			if (resolvedItemStack == null || !resolvedItemStack.Satisfies(stack) || recipe.Name.Path.Contains("repair"))
			{
				continue;
			}
			CraftingRecipeIngredient[] resolvedIngredients = recipe.ResolvedIngredients;
			foreach (CraftingRecipeIngredient ingred in resolvedIngredients)
			{
				if (ingred == null)
				{
					continue;
				}
				JsonObject? recipeAttributes = ingred.RecipeAttributes;
				if (recipeAttributes != null && recipeAttributes["repairMat"].Exists)
				{
					JsonItemStack jstack = ingred.RecipeAttributes["repairMat"].AsObject<JsonItemStack>();
					jstack.Resolve(api.World, $"recipe '{recipe.Name}' repair mat");
					if (jstack.ResolvedItemstack != null)
					{
						origMatCount += jstack.ResolvedItemstack.StackSize;
					}
				}
				else
				{
					origMatCount += ingred.Quantity;
				}
			}
			break;
		}
		return origMatCount;
	}

	public override TransitionState[] UpdateAndGetTransitionStates(IWorldAccessor world, ItemSlot inslot, ref EnumHandling handling)
	{
		ensureConditionExists(inslot);
		handling = EnumHandling.PassThrough;
		return null;
	}

	public override TransitionState UpdateAndGetTransitionState(IWorldAccessor world, ItemSlot inslot, EnumTransitionType type, ref EnumHandling handling)
	{
		if (type != EnumTransitionType.Perish)
		{
			ensureConditionExists(inslot);
		}
		handling = EnumHandling.PassThrough;
		return null;
	}

	public override void OnDamageItem(IWorldAccessor world, Entity byEntity, ItemSlot itemslot, ref int amount, ref EnumHandling bhHandling)
	{
		if (!(collObj.Variant["construction"] == "improvised"))
		{
			float amountf = amount;
			EnumCharacterDressType dressType = GetDressType(itemslot);
			if (byEntity is EntityPlayer && (dressType == EnumCharacterDressType.ArmorHead || dressType == EnumCharacterDressType.ArmorBody || dressType == EnumCharacterDressType.ArmorLegs))
			{
				amountf *= byEntity.Stats.GetBlended("armorDurabilityLoss");
			}
			amount = GameMath.RoundRandom(world.Rand, amountf);
			int leftDurability = itemslot.Itemstack.Collectible.GetRemainingDurability(itemslot.Itemstack);
			if (leftDurability > 0 && leftDurability - amount < 0)
			{
				world.PlaySoundAt(new AssetLocation("sounds/effect/toolbreak"), byEntity.Pos.X, byEntity.Pos.InternalY, byEntity.Pos.Z, (byEntity as EntityPlayer)?.Player);
			}
			itemslot.Itemstack.Collectible.SetDurability(itemslot.Itemstack, Math.Max(0, leftDurability - amount));
			itemslot.MarkDirty();
			bhHandling = EnumHandling.PreventDefault;
		}
	}

	public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot, ref EnumHandling handling)
	{
		handling = EnumHandling.Handled;
		return new WorldInteraction[1]
		{
			new WorldInteraction
			{
				ActionLangCode = "heldhelp-dress",
				MouseButton = EnumMouseButton.Right
			}
		};
	}

	public override int GetMergableQuantity(ItemStack sinkStack, ItemStack sourceStack, EnumMergePriority priority, ref EnumHandling handling)
	{
		if (priority == EnumMergePriority.DirectMerge)
		{
			if (GetMaxWarmth(new DummySlot(sinkStack)) == 0f)
			{
				handling = EnumHandling.PassThrough;
				return 0;
			}
			if ((sourceStack?.ItemAttributes?["clothingRepairStrength"].AsFloat()).GetValueOrDefault() > 0f)
			{
				if (sinkStack.Attributes.GetFloat("condition") < 1f)
				{
					handling = EnumHandling.PreventDefault;
					return 1;
				}
				handling = EnumHandling.PreventDefault;
				return 0;
			}
		}
		handling = EnumHandling.PassThrough;
		return 0;
	}

	public override void TryMergeStacks(ItemStackMergeOperation op, ref EnumHandling handling)
	{
		if (op.CurrentPriority == EnumMergePriority.DirectMerge)
		{
			float repstr = op.SourceSlot.Itemstack.ItemAttributes?["clothingRepairStrength"].AsFloat() ?? 0f;
			if (repstr > 0f && op.SinkSlot.Itemstack.Attributes.GetFloat("condition") < 1f)
			{
				ChangeCondition(op.SinkSlot, repstr);
				op.MovedQuantity = 1;
				op.SourceSlot.TakeOut(1);
				handling = EnumHandling.PreventDefault;
				return;
			}
		}
		handling = EnumHandling.PassThrough;
	}

	public virtual bool IsArmorType(ItemSlot slot)
	{
		EnumCharacterDressType dressType = GetDressType(slot);
		if (dressType != EnumCharacterDressType.ArmorBody && dressType != EnumCharacterDressType.ArmorHead)
		{
			return dressType == EnumCharacterDressType.ArmorLegs;
		}
		return true;
	}

	public virtual EnumCharacterDressType GetDressType(ItemSlot slot)
	{
		return DressType;
	}

	public virtual StatModifiers GetStatModifiers(ItemSlot slot)
	{
		return StatModifers;
	}

	public virtual ProtectionModifiers GetProtectionModifiers(ItemSlot slot)
	{
		return ProtectionModifiers;
	}

	public virtual AssetLocation[] GetFootStepSounds(ItemSlot slot)
	{
		return FootStepSounds;
	}

	public virtual float GetMaxWarmth(ItemSlot inslot)
	{
		return inslot.Itemstack.ItemAttributes?["warmth"].AsFloat() ?? 0f;
	}
}
