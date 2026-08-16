using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000134 RID: 308
	[NullableContext(1)]
	[Nullable(0)]
	public class WearableArmorBehavior : CollectibleBehavior, IWearableStatsSupplier
	{
		// Token: 0x06000EF9 RID: 3833 RVA: 0x0004A0AD File Offset: 0x000482AD
		public WearableArmorBehavior(CollectibleObject collObj) : base(collObj)
		{
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x0004A0B6 File Offset: 0x000482B6
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			this.api = api;
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x0004A0C8 File Offset: 0x000482C8
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling, ref EnumHandling handling)
		{
			if (byEntity.Controls.ShiftKey)
			{
				handling = 0;
				return;
			}
			ItemStack itemstack = slot.Itemstack;
			if (((itemstack != null) ? itemstack.Item : null) == null)
			{
				return;
			}
			EntityPlayer entityPlayer = byEntity as EntityPlayer;
			IPlayer player = (entityPlayer != null) ? entityPlayer.Player : null;
			if (player == null)
			{
				return;
			}
			ArmorInventory inventory = WearableArmorBehavior.GetGearInventory(byEntity) as ArmorInventory;
			if (inventory == null)
			{
				return;
			}
			ArmorBehavior behavior = this.collObj.GetCollectibleBehavior<ArmorBehavior>(true);
			if (behavior == null)
			{
				return;
			}
			string code = slot.Itemstack.Item.Code;
			ArmorType armorType = behavior.ArmorType;
			try
			{
				foreach (int index in inventory.GetSlotBlockingSlotsIndices(armorType))
				{
					ItemStack stack = inventory[index].TakeOutWhole();
					if (!player.InventoryManager.TryGiveItemstack(stack, false))
					{
						byEntity.Api.World.SpawnItemEntity(stack, byEntity.ServerPos.AsBlockPos, null);
					}
					inventory[index].MarkDirty();
				}
				int slotIndex = inventory.GetFittingSlotIndex(armorType);
				inventory[slotIndex].TryFlipWith(slot);
				inventory[slotIndex].MarkDirty();
				slot.MarkDirty();
				handHandling = 4;
				handling = 3;
			}
			catch (Exception exception)
			{
				ICoreAPI coreAPI = this.api;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(38, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Error on equipping '");
				defaultInterpolatedStringHandler.AppendFormatted(code);
				defaultInterpolatedStringHandler.AppendLiteral("' that occupies ");
				defaultInterpolatedStringHandler.AppendFormatted<ArmorType>(armorType);
				defaultInterpolatedStringHandler.AppendLiteral(":\n");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				LoggerUtil.Error(coreAPI, this, defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x0004A280 File Offset: 0x00048480
		public override void OnCreatedByCrafting(ItemSlot[] allInputSlots, ItemSlot outputSlot, IRecipeBase byRecipe, ref EnumHandling bhHandling)
		{
			bool isRepairRecipe = WearableArmorBehavior.IsRepairRecipe(byRecipe);
			int newDurability = 0;
			if (!(outputSlot is DummySlot))
			{
				this.EnsureConditionExists(outputSlot);
				outputSlot.Itemstack.Attributes.SetFloat("condition", 1f);
				if (isRepairRecipe)
				{
					float repairValue;
					int num;
					this.CalculateRepairValueProperly(allInputSlots, outputSlot, out repairValue, out num);
					int curDur = outputSlot.Itemstack.Collectible.GetRemainingDurability(outputSlot.Itemstack);
					int maxDur = outputSlot.Itemstack.Collectible.GetMaxDurability(outputSlot.Itemstack);
					newDurability = Math.Min(maxDur, (int)((float)curDur + (float)maxDur * repairValue));
				}
			}
			bhHandling = (isRepairRecipe ? 3 : 0);
			if (outputSlot is DummySlot)
			{
				return;
			}
			if (isRepairRecipe)
			{
				outputSlot.Itemstack.Attributes.SetInt("durability", newDurability);
			}
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x0004A33C File Offset: 0x0004853C
		public override bool ConsumeCraftingIngredients(ItemSlot[] slots, ItemSlot outputSlot, IRecipeBase matchingRecipe, ref EnumHandling handling)
		{
			if (WearableArmorBehavior.IsRepairRecipe(matchingRecipe))
			{
				float num;
				int matCostPerMatType;
				this.CalculateRepairValueProperly(slots, outputSlot, out num, out matCostPerMatType);
				foreach (ItemSlot islot in slots)
				{
					if (!islot.Empty)
					{
						if (WearableArmorBehavior.IsArmorSlot(islot))
						{
							islot.Itemstack = null;
						}
						else
						{
							islot.TakeOut(matCostPerMatType);
						}
					}
				}
				handling = 3;
				return true;
			}
			handling = 0;
			return false;
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x0004A3A2 File Offset: 0x000485A2
		public override int GetMergableQuantity(ItemStack sinkStack, ItemStack sourceStack, EnumMergePriority priority, ref EnumHandling handling)
		{
			if (priority != 1 || !WearableArmorBehavior.IsClothingRepairMaterial(sourceStack))
			{
				handling = 0;
				return 0;
			}
			if (!this.CanRepairCondition(sinkStack))
			{
				handling = 3;
				return 0;
			}
			handling = 3;
			return 1;
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x0004A3CC File Offset: 0x000485CC
		public override void TryMergeStacks(ItemStackMergeOperation op, ref EnumHandling handling)
		{
			if (op.CurrentPriority == 1)
			{
				ItemSlot sourceSlot = op.SourceSlot;
				if (WearableArmorBehavior.IsClothingRepairMaterial((sourceSlot != null) ? sourceSlot.Itemstack : null))
				{
					ItemSlot sinkSlot = op.SinkSlot;
					ItemStack sinkStack = (sinkSlot != null) ? sinkSlot.Itemstack : null;
					if (!this.CanRepairCondition(sinkStack))
					{
						handling = 3;
						return;
					}
					float repairStrength = WearableArmorBehavior.GetClothingRepairStrength(op.SourceSlot.Itemstack);
					this.ApplyConditionRepair(op.SinkSlot, repairStrength);
					op.MovedQuantity = 1;
					op.SourceSlot.TakeOut(1);
					handling = 3;
					return;
				}
			}
			handling = 0;
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x0004A454 File Offset: 0x00048654
		[NullableContext(2)]
		private static bool IsRepairRecipe(IRecipeBase recipe)
		{
			if (recipe == null)
			{
				return false;
			}
			AssetLocation name = recipe.Name;
			bool? flag;
			if (name == null)
			{
				flag = default(bool?);
			}
			else
			{
				string path = name.Path;
				flag = ((path != null) ? new bool?(path.Contains("repair", 5)) : default(bool?));
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x0004A4A8 File Offset: 0x000486A8
		[NullableContext(2)]
		private bool CanRepairCondition(ItemStack stack)
		{
			if (stack == null)
			{
				return false;
			}
			CollectibleBehaviorWearable vanillaWearableBehavior = this.GetVanillaWearableBehavior();
			return (vanillaWearableBehavior == null || vanillaWearableBehavior.GetMaxWarmth(new DummySlot(stack)) != 0f) && stack.Attributes.GetFloat("condition", 0f) < 1f;
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x0004A4F9 File Offset: 0x000486F9
		[NullableContext(2)]
		private static bool IsClothingRepairMaterial(ItemStack stack)
		{
			return WearableArmorBehavior.GetClothingRepairStrength(stack) > 0f;
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x0004A508 File Offset: 0x00048708
		[NullableContext(2)]
		private static float GetClothingRepairStrength(ItemStack stack)
		{
			float? num;
			if (stack == null)
			{
				num = default(float?);
			}
			else
			{
				JsonObject itemAttributes = stack.ItemAttributes;
				num = ((itemAttributes != null) ? new float?(itemAttributes["clothingRepairStrength"].AsFloat(0f)) : default(float?));
			}
			float? num2 = num;
			return num2.GetValueOrDefault();
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x0004A55C File Offset: 0x0004875C
		private void ApplyConditionRepair(ItemSlot slot, float repairStrength)
		{
			CollectibleBehaviorWearable vanillaWearable = this.GetVanillaWearableBehavior();
			if (vanillaWearable != null)
			{
				vanillaWearable.ChangeCondition(slot, repairStrength);
				return;
			}
			slot.Itemstack.Attributes.SetFloat("condition", GameMath.Clamp(slot.Itemstack.Attributes.GetFloat("condition", 1f) + repairStrength, 0f, 1f));
			slot.MarkDirty();
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x0004A5C2 File Offset: 0x000487C2
		public bool IsArmorType(ItemSlot slot)
		{
			CollectibleBehaviorWearable vanillaWearableBehavior = this.GetVanillaWearableBehavior();
			if (vanillaWearableBehavior == null)
			{
				return this.collObj.GetCollectibleBehavior<ArmorBehavior>(true) != null;
			}
			return vanillaWearableBehavior.IsArmorType(slot);
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x0004A5E4 File Offset: 0x000487E4
		public EnumCharacterDressType GetDressType(ItemSlot slot)
		{
			CollectibleBehaviorWearable vanillaWearableBehavior = this.GetVanillaWearableBehavior();
			if (vanillaWearableBehavior == null)
			{
				return -1;
			}
			return vanillaWearableBehavior.GetDressType(slot);
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x0004A5F8 File Offset: 0x000487F8
		[return: Nullable(2)]
		public StatModifiers GetStatModifiers(ItemSlot slot)
		{
			CollectibleBehaviorWearable vanillaWearableBehavior = this.GetVanillaWearableBehavior();
			if (vanillaWearableBehavior == null)
			{
				return null;
			}
			return vanillaWearableBehavior.GetStatModifiers(slot);
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x0004A60C File Offset: 0x0004880C
		[return: Nullable(2)]
		public ProtectionModifiers GetProtectionModifiers(ItemSlot slot)
		{
			CollectibleBehaviorWearable vanillaWearableBehavior = this.GetVanillaWearableBehavior();
			if (vanillaWearableBehavior == null)
			{
				return null;
			}
			return vanillaWearableBehavior.GetProtectionModifiers(slot);
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0004A620 File Offset: 0x00048820
		public AssetLocation[] GetFootStepSounds(ItemSlot slot)
		{
			return ArmorFootStepSoundUtil.GetFootStepSounds(this.api, slot, this.collObj, this.GetVanillaWearableBehavior());
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x0004A63A File Offset: 0x0004883A
		public float GetMaxWarmth(ItemSlot slot)
		{
			CollectibleBehaviorWearable vanillaWearableBehavior = this.GetVanillaWearableBehavior();
			if (vanillaWearableBehavior == null)
			{
				return 0f;
			}
			return vanillaWearableBehavior.GetMaxWarmth(slot);
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x0004A652 File Offset: 0x00048852
		[NullableContext(2)]
		private CollectibleBehaviorWearable GetVanillaWearableBehavior()
		{
			return this.collObj.GetBehavior<CollectibleBehaviorWearable>();
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x0004A65F File Offset: 0x0004885F
		[return: Nullable(2)]
		protected static InventoryBase GetGearInventory(Entity entity)
		{
			return entity.GetBehavior<EntityBehaviorPlayerInventory>().Inventory;
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x0004A66C File Offset: 0x0004886C
		protected virtual void EnsureConditionExists(ItemSlot slot)
		{
			if (slot is DummySlot)
			{
				return;
			}
			if (!slot.Itemstack.Attributes.HasAttribute("condition"))
			{
				ICoreAPI coreAPI = this.api;
				if (coreAPI != null && coreAPI.Side == 1)
				{
					JsonObject itemAttributes = slot.Itemstack.ItemAttributes;
					if (itemAttributes != null && itemAttributes["warmth"].Exists)
					{
						JsonObject itemAttributes2 = slot.Itemstack.ItemAttributes;
						if (itemAttributes2 == null || itemAttributes2["warmth"].AsFloat(0f) != 0f)
						{
							if (slot is ItemSlotTrade)
							{
								slot.Itemstack.Attributes.SetFloat("condition", (float)this.api.World.Rand.NextDouble() * 0.25f + 0.75f);
							}
							else
							{
								slot.Itemstack.Attributes.SetFloat("condition", (float)this.api.World.Rand.NextDouble() * 0.4f);
							}
							slot.MarkDirty();
						}
					}
				}
			}
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x0004A788 File Offset: 0x00048988
		protected virtual void CalculateRepairValueProperly(ItemSlot[] inSlots, ItemSlot outputSlot, out float repairValue, out int matCostPerMatType)
		{
			repairValue = 0f;
			matCostPerMatType = 0;
			Func<ItemSlot, bool> func;
			if ((func = WearableArmorBehavior.<>O.<0>__IsArmorSlot) == null)
			{
				func = (WearableArmorBehavior.<>O.<0>__IsArmorSlot = new Func<ItemSlot, bool>(WearableArmorBehavior.IsArmorSlot));
			}
			ItemSlot armorSlot = Enumerable.FirstOrDefault<ItemSlot>(inSlots, func);
			if (((armorSlot != null) ? armorSlot.Itemstack : null) == null || outputSlot.Itemstack == null)
			{
				return;
			}
			int origMatCount = this.GetOrigMatCount(inSlots, outputSlot);
			if (origMatCount == 0)
			{
				JsonObject attributes = this.collObj.Attributes;
				origMatCount = ((attributes != null) ? attributes["materialCount"].AsInt(1) : 1);
			}
			int curDur = outputSlot.Itemstack.Collectible.GetRemainingDurability(armorSlot.Itemstack);
			int maxDur = outputSlot.Itemstack.Collectible.GetMaxDurability(outputSlot.Itemstack);
			float repairDurabilityPerItem = 2f / (float)origMatCount * (float)maxDur;
			int fullRepairMatCount = (int)Math.Max(1.0, Math.Round((double)((float)(maxDur - curDur) / repairDurabilityPerItem)));
			int minMatStackSize = this.GetInputRepairCount(inSlots);
			int matTypeCount = this.GetRepairMatTypeCount(inSlots);
			int availableRepairMatCount = Math.Min(fullRepairMatCount, minMatStackSize * matTypeCount);
			matCostPerMatType = Math.Min(fullRepairMatCount, minMatStackSize);
			repairValue = (float)availableRepairMatCount / (float)origMatCount * 2f;
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x0004A89C File Offset: 0x00048A9C
		protected virtual int GetRepairMatTypeCount(ItemSlot[] slots)
		{
			List<ItemStack> stackTypes = new List<ItemStack>();
			foreach (ItemSlot slot in slots)
			{
				if (!slot.Empty)
				{
					bool found = false;
					if (!WearableArmorBehavior.IsArmorSlot(slot))
					{
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
				}
			}
			return stackTypes.Count;
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x0004A93C File Offset: 0x00048B3C
		protected virtual int GetOrigMatCount(ItemSlot[] inSlots, ItemSlot outputSlot)
		{
			int count = 0;
			foreach (ItemSlot slot in inSlots)
			{
				if (!slot.Empty && !WearableArmorBehavior.IsArmorSlot(slot))
				{
					count += Math.Max(1, slot.StackSize);
				}
			}
			return count;
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x0004A980 File Offset: 0x00048B80
		protected virtual int GetInputRepairCount(ItemSlot[] inSlots)
		{
			int min = int.MaxValue;
			foreach (ItemSlot slot in inSlots)
			{
				if (!slot.Empty && !WearableArmorBehavior.IsArmorSlot(slot))
				{
					min = Math.Min(min, slot.StackSize);
				}
			}
			if (min != 2147483647)
			{
				return min;
			}
			return 0;
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x0004A9D0 File Offset: 0x00048BD0
		protected static bool IsArmorSlot(ItemSlot slot)
		{
			ItemStack itemstack = slot.Itemstack;
			CollectibleObject collectible = (itemstack != null) ? itemstack.Collectible : null;
			return collectible != null && (collectible is ItemWearable || collectible.GetCollectibleBehavior<ArmorBehavior>(true) != null);
		}

		// Token: 0x0400072E RID: 1838
		[Nullable(2)]
		protected ICoreAPI api;

		// Token: 0x02000281 RID: 641
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000D38 RID: 3384
			[Nullable(0)]
			public static Func<ItemSlot, bool> <0>__IsArmorSlot;
		}
	}
}
